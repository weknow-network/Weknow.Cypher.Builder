using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Weknow.GraphDbCommands.Declarations;

using static Weknow.GraphDbCommands.CypherDelegates;

#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// The cypher visitor is the heart of the ORM implementation
    /// </summary>
    /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
    internal sealed class CypherVisitor : ExpressionVisitor, IDisposable
    {
        private readonly CypherConfig _configuration;
        private bool _shouldHandleAmbient = false;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CypherVisitor(CypherConfig configuration)
        {
            _configuration = configuration;
        }

        #endregion // Ctor

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Query.Dispose();
        }

        #endregion // Dispose

        #region Query

        /// <summary>
        /// Mutable state of the cypher query.
        /// Query build during the visitor traverse.
        /// </summary>
        public CypherQueryBuilder Query { get; } = new CypherQueryBuilder();

        #endregion // Query

        #region Parameters

        /// <summary>
        /// Mutable state of the parameters.
        /// Parameters build during the visitor traverse.
        /// </summary>
        public CypherParameters Parameters { get; } = new CypherParameters();

        #endregion // Parameters

        private readonly ContextValue<bool> _isProperties = new ContextValue<bool>(false);

        private readonly ContextValue<MethodCallExpression?> _methodExpr = new ContextValue<MethodCallExpression?>(null);

        private readonly Dictionary<int, ContextValue<Expression?>> _expression = new Dictionary<int, ContextValue<Expression?>>()
        {
            [0] = new ContextValue<Expression?>(null),
        };

        #region HandleAmbientLabels

        private void HandleAmbientLabels(params string[] labels)
        {
            if (_configuration.AmbientLabels.Values.Count == 0 && (labels == null || labels.Length == 0))
                return;

            if (!_shouldHandleAmbient)
            {
                if (labels == null || labels.Length == 0)
                    return;

                HandleStartChar();

                IEnumerable<string> formatted = labels.Select(m => _configuration.AmbientLabels.FormatByConvention(m));
                var addition = string.Join(":", formatted);
                Query.Append(addition);
                return;
            }

            _shouldHandleAmbient = false;

            HandleStartChar();

            Query.Append(_configuration.AmbientLabels.Combine(labels));

            void HandleStartChar()
            {
                char lastChar = Query[^1];
                if (lastChar != ':')
                    Query.Append(":");
            }
        }

        #endregion // HandleAmbientLabels

        #region VisitLambda

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.Expression`1" />.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Visit(node.Body);
            return node;
        }

        #endregion // VisitLambda

        #region VisitBinary

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.BinaryExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    if (node.Method.ReturnType == typeof(bool))
                        Query.Append(" > ");
                    else
                    {
                        if (node.Left.Type == typeof(INode) && node.Right.Type == typeof(INode))
                            Query.Append("-");
                        Query.Append("->");
                    }
                    break;
                case ExpressionType.LessThan:
                    if (node.Method.ReturnType == typeof(bool))
                        Query.Append(" < ");
                    else
                    {
                        Query.Append("<-");
                        if (node.Left.Type == typeof(INode) && node.Right.Type == typeof(INode))
                            Query.Append("-");
                    }
                    break;
                case ExpressionType.Subtract:
                    Query.Append("-");
                    if (node.Left.Type == typeof(INode) && node.Right.Type == typeof(INode))
                        Query.Append("-");
                    break;
                case ExpressionType.Equal:
                    string? eq = EqualPattern();
                    Query.Append(eq);
                    break;
                case ExpressionType.NotEqual:
                    Query.Append(" <> ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    Query.Append(" >= ");
                    break;
                case ExpressionType.Add:
                    Query.Append(" + ");
                    break;
                case ExpressionType.Or:
                    Query.Append("|");
                    break;
                case ExpressionType.AndAlso:
                    Query.Append(" AND ");
                    break;
                case ExpressionType.OrElse:
                    Query.Append(" OR ");
                    break;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Visit(node.Right);
            return node;
        }

        #endregion // VisitBinaryVisitUnary

        #region VisitMethodCall

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MethodCallExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            string mtdName = node.Method.Name;
            string type = node.Type.Name;
            ReadOnlyCollection<Expression> args = node.Arguments;
            Expression? firstArg = args.FirstOrDefault();

            var format = node.Method.GetCustomAttributes<CypherAttribute>(false).Select(att => att.Format).FirstOrDefault();
            if (format != null)
            {
                bool ambScope = (node.Type == typeof(INode) || node.Type == typeof(IRelation) || node.Type == typeof(INodeRelation) || node.Type == typeof(IRelationNode));
                if (ambScope)
                    _shouldHandleAmbient = true;

                ApplyFormat(node, format);

                if (ambScope)
                    _shouldHandleAmbient = false;
            }
            else if (type == nameof(Rng))
            {
                if (mtdName == nameof(Rng.Scope))
                {
                    Query.Append("*");
                    var index0 = (ConstantExpression)node.Arguments[0];
                    Query.Append(index0.Value);
                    Query.Append("..");
                    var index1 = (ConstantExpression)node.Arguments[1];
                    Query.Append(index1.Value);
                }
                else if (mtdName == nameof(Rng.AtMost))
                {
                    Query.Append("*..");
                    var index = (ConstantExpression)node.Arguments[0];
                    Query.Append(index.Value);
                }
                else if (mtdName == nameof(Rng.AtLeast))
                {
                    Query.Append("*");
                    var index = (ConstantExpression)node.Arguments[0];
                    Query.Append(index.Value);
                    Query.Append("..");
                }
                else if (mtdName == nameof(Rng.Any))
                {
                    Query.Append("*");
                }
            }

            return node;
        }

        #endregion // VisitMethodCall

        #region VisitMember

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MemberExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            string name = node.Member.Name;

            var pi = node.Member as PropertyInfo;

            if (node.Member.Name == nameof(DateTime.Now) && node.Member.DeclaringType == typeof(DateTime))
            {
                var parameterName = $"p_{Parameters.Count}";
                Query.Append($"${parameterName}");
                Parameters[parameterName] = DateTime.Now;
                return node;
            }

            if (node.Expression is MemberExpression mme && mme.Member.Name == nameof(VariableDeclaration<int>.Inc))
            {
                Visit(mme.Expression);
                Query.Append(".");
                Query.Append(name);
                Query.Append(" + 1");
                return node;
            }

            if ((node.Type == typeof(INode) || node.Type == typeof(IRelation) || node.Type == typeof(INodeRelation) || node.Type == typeof(IRelationNode)) &&
                    node.Expression is ConstantExpression c &&
                    node.Member is FieldInfo fi &&
                    fi.GetValue(c.Value) is ExpressionPattern p)
            {
                Visit(p.expression);
                return node;
            }
            else if (node.Expression is MemberExpression me &&
                     typeof(VariableDeclaration).IsAssignableFrom(me.Member.DeclaringType) &&
                     !_isProperties.Value)
            {
                Visit(me.Expression);
                Query.Append(".");
            }
            if (node.Type == typeof(ILabel))
            {
                HandleAmbientLabels(name);
                return node;
            }

            if (typeof(ParameterDeclaration).IsAssignableFrom(node.Type))
            {
                Query.Append("$");
                if (node.Member.Name == nameof(VariableDeclaration.AsParameter))
                    name = ((MemberExpression)node.Expression).Member.Name;
                if (!Parameters.ContainsKey(name))
                    Parameters.AddNull(name);
            }
            else if (node.Expression is MemberExpression me && me.Member.Name == nameof(ParameterDeclaration<int>._)
                && typeof(ParameterDeclaration).IsAssignableFrom(me.Member.DeclaringType))
            {
                Query.Append("$");
                if (me.Expression is UnaryExpression ue && ue.NodeType == ExpressionType.Not &&
                    ue.Operand is MemberExpression ime)
                {
                    Query.Append(ime.Member.Name);
                    Query.Append(".");
                }
                if (!Parameters.ContainsKey(name))
                    Parameters.AddNull(name);
            }
            else if (node.Expression is MemberExpression vme && vme.Member.Name == nameof(VariableDeclaration<int>._)
                && typeof(VariableDeclaration).IsAssignableFrom(vme.Member.DeclaringType))
            {
                if (vme.Expression is UnaryExpression ue && ue.NodeType == ExpressionType.Not &&
                    ue.Operand is MemberExpression ime)
                {
                    Query.Append(ime.Member.Name);
                    Query.Append(".");
                }
            }
            Query.Append(name);
            if (node.Type == typeof(VariableDeclaration))
            {
                HandleAmbientLabels();
            }

            return node;
        }

        #endregion // VisitMember

        #region VisitNewArray

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.NewArrayExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            foreach (var expr in node.Expressions)
            {
                Visit(expr);
                bool isLabels = node.Type == typeof(ILabel[]);
                if (expr != node.Expressions.Last() && !isLabels)
                    Query.Append(", ");
            }
            return node;
        }

        #endregion // VisitNewArray

        #region VisitNew

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.NewExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitNew(NewExpression node)
        {
            using var _ = _isProperties.Set(true);
            if (_expression[0].Value == null)
                Query.Append("{ ");
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                if (_expression[0].Value != null)
                {
                    Visit(_expression[0].Value);
                    Query.Append('.');
                }
                Query.Append(node.Members[i].Name);
                AppendPropSeparator();
                Expression? expr = node.Arguments[i];
                Visit(expr);
                if (expr != node.Arguments.Last())
                    Query.Append(", ");
            }
            if (_expression[0].Value == null)
                Query.Append(" }");
            return node;
        }

        #endregion // VisitNew

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            using var _ = _isProperties.Set(true);
            if (_expression[0].Value == null)
                Query.Append("{ ");
            foreach (var item in node.Bindings)
            {
                if (_expression[0].Value != null)
                {
                    Visit(_expression[0].Value);
                    Query.Append('.');
                }
                VisitMemberBinding(item);
                if (item != node.Bindings.Last())
                    Query.Append(", ");
            }
            if (_expression[0].Value == null)
                Query.Append(" }");
            return node;
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            Query.Append(node.Member.Name);
            AppendPropSeparator();
            return base.VisitMemberBinding(node);
        }
        #region VisitConstant

        /// <summary>
        /// Visits the <see cref="T:System.Linq.Expressions.ConstantExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            bool isReturn = _methodExpr.Value?.Method.Name == nameof(CypherPhraseExtensions.Return);
            if (isReturn)
            {
                Query.Append(node.Value);
            }
            else
            {
                var parameterName = $"p_{Parameters.Count}";
                Query.Append($"${parameterName}");
                Parameters[parameterName] = node.Value;
            }
            return node;
        }

        #endregion // VisitConstant

        #region VisitParameter

        /// <summary>
        /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            Query.Append(node.Name);
            HandleAmbientLabels();
            return node;
        }

        #endregion // VisitParameter

        #region ApplyFormat

        /// <summary>
        /// Applies the format.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="format">The format.</param>
        private void ApplyFormat(MethodCallExpression node, string format)
        {
            var disp = new List<IDisposable>();
            for (var i = 0; i < format.Length; i++)
            {
                switch (format[i])
                {
                    case '$':
                        {
                            i++;
                            var isArray = format.Length > i + 1 &&
                                          format[(i)..(i + 2)] == "[]";
                            if (isArray)
                                i += 2;
                            var ch = format[i];
                            int index = int.Parse(ch.ToString());
                            var args = node.Arguments;
                            Expression expr = args[index];
                            int count = args.Count;
                            // handling case of safe params array (when having ParamsFirst parameter to avoid empty array)
                            if (expr is NewArrayExpression naExp && expr.NodeType == ExpressionType.NewArrayInit)
                            {
                                if (isArray)
                                {
                                    var parameterName = $"p_{Parameters.Count}";
                                    Query.Append(parameterName);
                                    // TODO: Assing the value of the array
                                    Parameters[parameterName] = null; // naExp.Value;
                                    continue;
                                }
                                else if (count > 1)
                                {
                                    var prv = args[count - 2];
                                    if (prv.NodeType == ExpressionType.Convert &&
                                        prv.Type.Name == "ParamsFirst`1" &&
                                        naExp.Expressions.Count != 0)
                                    {
                                        Query.Append(", ");
                                    }
                                }

                            }
                            Visit(expr);
                        }
                        break;
                    case '+':
                        {
                            var ch = format[++i];
                            string fmt1 = ch.ToString();

                            string fmt2 = format[++i].ToString();
                            int index1 = int.Parse(fmt1);
                            int index2 = int.Parse(fmt2);
                            Expression? expr = node.Arguments[index2];
                            ContextValue<Expression?>? ctx = _expression[index1];
                            disp.Add(ctx.Set(expr));
                        }
                        break;
                    case '&':
                        disp.Add(_methodExpr.Set(node));
                        break;
                    case '\\':
                        {
                            char f = format[++i];
                            Query.Append(f);
                            break;
                        }
                    default:
                        {
                            char f = format[i];
                            Query.Append(f);
                            break;
                        }
                }
            }
            foreach (var d in disp)
            {
                d.Dispose();
            }
        }

        #endregion // ApplyFormat

        #region EqualPattern

        /// <summary>
        /// Determines whether [is equal pattern].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is equal pattern]; otherwise, <c>false</c>.
        /// </returns>
        private string? EqualPattern()
        {
            return _methodExpr.Value?.Method.Name switch
            {
                nameof(CypherPhraseExtensions.Set) => " = ",
                nameof(CypherPhraseExtensions.Where) => " = ",
                nameof(CypherPhraseExtensions.OnCreateSet) => " = ",
                nameof(CypherPhraseExtensions.OnMatchSet) => " = ",
                nameof(Cypher.Rgx) => " =~ ",
                _ => null,
            };
        }

        #endregion // EqualPattern

        #region AppendPropSeparator

        /// <summary>
        /// Appends the property separator.
        /// </summary>
        private void AppendPropSeparator()
        {
            string? equalPattern = EqualPattern();
            if (equalPattern != null)
                Query.Append(equalPattern);
            else
                Query.Append(": ");
        }

        #endregion // AppendPropSeparator
    }
}
