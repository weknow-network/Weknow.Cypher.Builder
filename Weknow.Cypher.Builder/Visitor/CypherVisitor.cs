using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using static Weknow.Cypher.Builder.CypherDelegates;

#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// The cypher visitor is the heart of the ORM implementation
    /// </summary>
    /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
    internal sealed class CypherVisitor : ExpressionVisitor, IDisposable
    {
        private readonly CypherConfig _configuration;

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
        private readonly ContextValue<bool> _isPluralize = new ContextValue<bool>(false);
        private readonly ContextValue<bool> _isSingularize = new ContextValue<bool>(false);

        private readonly ContextValue<string?> _isCustomProp = new ContextValue<string?>(null);
        private readonly ContextValue<string?> _varExtension = new ContextValue<string?>(null);
        private readonly ContextValue<bool> _noSelfFormatting = new ContextValue<bool>(false);

        private readonly ContextValue<MethodCallExpression?> _methodExpr = new ContextValue<MethodCallExpression?>(null);
        private readonly ContextValue<string> _reusedParameterName = new ContextValue<string>(string.Empty);
        private readonly ContextValue<string?> _propPrefix = new ContextValue<string?>(null);

        private readonly Dictionary<int, ContextValue<ContextExpression?>> _expression = new Dictionary<int, ContextValue<ContextExpression?>>()
        {
            [0] = new ContextValue<ContextExpression?>(null),
            [1] = new ContextValue<ContextExpression?>(null),
            [2] = new ContextValue<ContextExpression?>(null),
            [3] = new ContextValue<ContextExpression?>(null),
            [4] = new ContextValue<ContextExpression?>(null),
        };

        private readonly HashSet<Expression> _duplication = new HashSet<Expression>();

        private readonly List<Expression> _reuseParameters = new List<Expression>();
        private readonly List<string> _reuseParameterNames = new List<string>();

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
            if (_reuseParameterNames.Count < _reuseParameters.Count)
                _reuseParameterNames.Add(node.Parameters[0].Name);

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
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    if (node.Left.Type == typeof(INode) && node.Right.Type == typeof(INode))
                        Query.Append("-");
                    Query.Append("->");
                    break;
                case ExpressionType.LessThan:
                    Query.Append("<-");
                    if (node.Left.Type == typeof(INode) && node.Right.Type == typeof(INode))
                        Query.Append("-");
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
                case ExpressionType.Add:
                    Query.Append(" += ");
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
            Visit(node.Right);
            return node;
        }

        #endregion // VisitBinaryVisitUnary

        #region VisitUnary

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.UnaryExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            Visit(node.Operand);
            if (node.NodeType == ExpressionType.UnaryPlus)
            {
                if (!_duplication.Contains(node))
                {
                    Query.Append(" +");
                    _duplication.Add(node);
                }
            }

            return node;
        }

        #endregion // VisitUnary


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

            using var _ = IsProperty(node.Method.ReturnType);
            using IDisposable inScp = mtdName switch
            {
                nameof(CypherPredicateExtensions.In) => _methodExpr.Set(node),
                _ => DisposeableAction.Empty
            };

            using IDisposable selfFormatting = node.Type.Name switch
            {
                nameof(ISelfFormat) => _noSelfFormatting.Set(true),
                _ => DisposeableAction.Empty
            };

            var attributes = node.Method.GetCustomAttributes(typeof(CypherAttribute), false);
            var format = attributes.Length > 0 ? (attributes[0] as CypherAttribute)?.Format : null;
            if (format != null)
            {
                using IDisposable scp = IsCustomProp(node);

                // Looking for property options
                var argMtd = node.Arguments.LastOrDefault() as MethodCallExpression;
                var opt = argMtd?.Arguments?.FirstOrDefault() as ConstantExpression;
                using IDisposable mapProps = node.Method.Name switch
                {
                    nameof(Cypher.P) when firstArg.Type == typeof(IMap) &&
                                          args.Count > 1 &&
                                          firstArg is MemberExpression me &&
                                          me.Expression is ParameterExpression mme =>
                         _propPrefix.Set($"{mme.Name}."),
                    _ => DisposeableAction.Empty
                };
                ApplyFormat(node, format);
            }
            else if (mtdName == nameof(IReuse<Fluent, Fluent>.By))
            {
                Visit(node.Object);
                Visit(node.Arguments[0]);
            }
            else if (mtdName == nameof(Cypher.Reuse) || mtdName == nameof(Cypher.AsReuse))
            {
                if (node.Arguments.Count == 2)
                {
                    _reuseParameters.Add(node.Arguments[0]);
                    Visit(node.Arguments[1]);
                }
                else
                    _reuseParameters.Add(node.Arguments[0]);
            }
            else if (mtdName == nameof(Range.EndAt))
            {
                Query.Append("*..");
                var index = node.Arguments[0] as UnaryExpression;
                Query.Append(index?.Operand);
            }
            else if (type == nameof(Rng))
            {
                if (mtdName == nameof(Rng.Scope))
                {
                    Query.Append("*");
                    var index0 = node.Arguments[0] as ConstantExpression;
                    Query.Append(index0?.Value);
                    Query.Append("..");
                    var index1 = node.Arguments[1] as ConstantExpression;
                    Query.Append(index1?.Value);
                }
                else if (mtdName == nameof(Rng.AtMost))
                {
                    Query.Append("*..");
                    var index = node.Arguments[0] as ConstantExpression;
                    Query.Append(index?.Value);
                }
                else if (mtdName == nameof(Rng.AtLeast))
                {
                    Query.Append("*");
                    var index = node.Arguments[0] as ConstantExpression;
                    Query.Append(index?.Value);
                    Query.Append("..");
                }
                else if (mtdName == nameof(Rng.Any))
                {
                    Query.Append("*");
                }
            }
            else if (mtdName == "get_item")
            {
                Visit(node.Arguments[0]);
            }
            else if (mtdName == "get_" + nameof(Range.All))
            {
                Query.Append("*");
            }
            else if (mtdName == nameof(Cypher.All) ||
                    mtdName == nameof(Cypher.AllExcept))
            {
                bool isExcept = mtdName == nameof(Cypher.AllExcept);
                if (node.Method.IsGenericMethod)
                {
                    var properties = node.Method.GetGenericArguments()[0].GetProperties();
                    foreach (var item in properties)
                    {
                        Visit(node.Arguments[0]);
                        Query.Append(".");
                        Query.Append(item.Name);
                        if (_methodExpr.Value?.Method.Name == "Set")
                        {
                            Query.Append(" = $");
                            Query.Append(item.Name);
                        }
                        if (item != properties.Last())
                            Query.Append(", ");
                    }
                }
                else
                {
                    MethodCallExpression? methodExp = _expression[1].Value?.Expression as MethodCallExpression;
                    var properties = methodExp == null ?
                                    Array.Empty<PropertyInfo>() :
                                    methodExp.Method.GetGenericArguments()[0].GetProperties();

                    NewArrayExpression? arrayExp = node.Arguments.Count > 0 ? node.Arguments[0] as NewArrayExpression : null;
                    string[] exclude = !isExcept || arrayExp == null ?
                        Array.Empty<string>() :
                        arrayExp.Expressions.OfType<MemberExpression>().Select(x => x.Member.Name).ToArray();
                    foreach (var item in properties)
                    {
                        if (exclude.Contains(item.Name))
                            continue;

                        Query.Append(item.Name);
                        Query.Append(": $");
                        Query.Append(item.Name);
                        if (item != properties.Last())
                            Query.Append(", ");
                    }
                }
            }
            else if (mtdName == nameof(Cypher.Convention))
            {
                var filter = (node.Arguments[node.Arguments.Count == 1 ? 0 : 1] as Expression<Func<string, bool>>)?.Compile();
                var arguments = node.Method.IsGenericMethod
                    ? node.Method.GetGenericArguments()
                    : (_expression[1].Value?.Expression as MethodCallExpression)?.Method?.GetGenericArguments();
                Type? firstArgType = arguments?[0];
                var properties = firstArgType?.GetProperties()?.Where(p => filter?.Invoke(p.Name) ?? true).ToArray();
                foreach (var item in properties ?? Array.Empty<PropertyInfo>())
                {
                    if (node.Arguments.Count == 2)
                    {
                        Visit(node.Arguments[0]);
                        Query.Append(".");
                    }
                    Query.Append(item.Name);
                    if (node.Arguments.Count == 1)
                        Query.Append(": ");
                    else
                        Query.Append(" = ");
                    if (_expression[2].Value != null)
                    {
                        Visit(_expression[2].Value);
                        Query.Append(".");
                    }
                    else
                        Query.Append("$");
                    Query.Append(item.Name);
                    Parameters[item.Name] = null;
                    if (item != properties.Last())
                        Query.Append(", ");
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

            if (node.Expression is MemberExpression mme && mme.Member.Name == nameof(IVar<int>.Inc))
            {
                Query.Append(name);
                Query.Append(" = ");
                Visit(mme.Expression);
                Query.Append(".");
                Query.Append(name);
                Query.Append(" + 1");
                return node;
            }

            AddGenPrefix();

            if (name == nameof(IVar.AsMap))
            {
                if (_expression[2].Value == null && _methodExpr.Value?.Method.Name != "Set" && _methodExpr.Value?.Method.Name != "OnMatchSet")
                    Query.Append("$");

                Visit(node.Expression);
            }
            else if ((node.Type == typeof(INode) || node.Type == typeof(IRelation) || node.Type == typeof(INodeRelation) || node.Type == typeof(IRelationNode)) &&
                    node.Expression is ConstantExpression c &&
                    node.Member is FieldInfo fi &&
                    fi.GetValue(c.Value) is ExpressionPattern p)
            {
                Visit(p.expression);
                return node;
            }
            else if (node.Expression != null &&
                     node.Type != typeof(IMap) &&
                     (!_isProperties.Value || _methodExpr.Value?.Method.Name == "Set"))
            {
                Visit(node.Expression);
                Query.Append(".");
            }
            if (node.Type == typeof(ILabel))
            {
                char lastChar = Query[^1];
                if (lastChar != ':')
                    Query.Append(":");
                Query.Append(_configuration.AmbientLabels.Combine(name));
                return node;
            }

            if (name == nameof(IVar.AsMap))
                return node;

            Query.Append(name);

            bool ignore = _methodExpr.Value?.Method.Name switch
            {
                nameof(CypherPhraseExtensions.Return) => true,
                nameof(CypherPredicateExtensions.In) => true,
                _ => false
            };
            if ((_isProperties.Value) && !ignore)
            {
                HandleProperties(name);
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
                if (_expression[3].Value != null) // generics properties, example: n.P(ID)
                {
                    string? equalPattern = EqualPattern();
                    bool isReturn = _methodExpr.Value?.Method.Name == nameof(CypherPhraseExtensions.Return);
                    if (!_isProperties.Value || equalPattern != null || isReturn)
                    {
                        // use for tracing duplication
                        int idx = Query.Length;
                        Visit(_expression[3].Value);
                        Query.Append(".");
                        var range = Range.StartAt(idx);
                        var addition = Query[range];
                        int len = addition.Length;
                        var prev = Query[idx - len, len];
                        if (addition.Compare(prev))
                        {
                            Query.Remove(idx, len);
                        }
                    }
                }
                if (_methodExpr.Value?.Method.Name == nameof(CypherPredicateExtensions.In))
                    Query.Append("$");
                using IDisposable _ = IsProperty(expr.Type);
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
            if (node.Type == typeof(Range))
            {
                Query.Append("*");
                var index = node.Arguments[0] as UnaryExpression;
                Query.Append(index?.Operand);
                Query.Append("..");
                index = node.Arguments[1] as UnaryExpression;
                Query.Append(index?.Operand);

                return node;
            }

            using var _ = _isProperties.Set(true);
            foreach (var expr in node.Arguments)
            {
                Visit(expr);
                if (expr != node.Arguments.Last())
                    Query.Append(", ");
            }
            return node;
        }

        #endregion // VisitNew

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
            else if (_isProperties.Value)
            {
                Query.Append(node.Value);
                HandleProperties(node.Value);
            }
            else if (_noSelfFormatting.Value)
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
            if (_reusedParameterName != node.Name && _reuseParameterNames.Contains(node.Name) && node.Type != typeof(IVar))
            {
                using var _ = _reusedParameterName.Set(node.Name);
                Visit(_reuseParameters[_reuseParameterNames.IndexOf(node.Name)]);
            }
            else if (_isSingularize)
                Query.Append(_configuration.Naming.Pluralization.Singularize(node.Name));
            else if (_isPluralize)
                Query.Append(_configuration.Naming.Pluralization.Pluralize(node.Name));
            else
                Query.Append(node.Name);
            return node;
        }

        #endregion // VisitParameter

        #region Visit

        /// <summary>
        /// Visits the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        private void Visit(ContextExpression? expression)
        {
            if (expression == null) return;

            if (expression.IsPluralize)
            {
                using var _ = _isPluralize.Set(true);
                Visit(expression.Expression);
            }
            else if (expression.IsSingularize)
            {
                using var _ = _isSingularize.Set(true);
                Visit(expression.Expression);
            }
            else
            {
                Visit(expression.Expression);
            }
        }

        #endregion // Visit

        #region ApplyFormat

        /// <summary>
        /// Applies the format.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="format">The format.</param>
        private void ApplyFormat(MethodCallExpression node, string format)
        {
            IDisposable? disp = null;
            for (var i = 0; i < format.Length; i++)
            {
                switch (format[i])
                {
                    case '$':
                        {
                            var ch = format[++i];
                            IDisposable scope = DisposeableAction.Empty;
                            int index = -1;
                            if (ch == 'p')
                            {
                                scope = _isPluralize.Set(true);
                                index = int.Parse(format[++i].ToString());
                            }
                            else if (ch == 's')
                            {
                                scope = _isSingularize.Set(true);
                                index = int.Parse(format[++i].ToString());
                            }
                            else
                            {
                                index = int.Parse(ch.ToString());
                            }
                            using (scope)
                            {
                                Expression expr = node.Arguments[index];
                                IDisposable d = SetGenPropertiesContext(node, expr);
                                using (d)
                                {
                                    Visit(expr);
                                }
                            }
                        }
                        break;
                    case '!':
                        {
                            var ch = format[++i];
                            if (node.Arguments.All(UseGenericsAsLabel))
                            {
                                if (ch == 'l')
                                {
                                    int index = int.Parse(format[++i].ToString());
                                    Type[] tps = node.Method.GetGenericArguments();
                                    string name = tps[index].Name;
                                    Query.Append(_configuration.AmbientLabels.Combine(name));

                                }
                                else
                                {
                                    int index = int.Parse(ch.ToString());
                                    Query.Append(node.Method.GetGenericArguments()[index].Name);
                                }
                            }
                        }
                        break;
                    case '+':
                        {
                            var ch = format[++i];
                            string fmt1;
                            if (ch == 'p' || ch == 's')
                                fmt1 = format[++i].ToString();
                            else
                                fmt1 = ch.ToString();

                            string fmt2 = format[++i].ToString();
                            int index1 = int.Parse(fmt1);
                            int index2 = int.Parse(fmt2);
                            Expression? expr = node.Arguments[index2];
                            ContextValue<ContextExpression?>? ctx = _expression[index1];
                            if (ch == 'p')
                            {
                                disp = ctx.Set(new ContextExpression(true, false, expr));
                            }
                            else if (ch == 's')
                            {
                                disp = ctx.Set(new ContextExpression(false, true, expr));
                            }
                            else
                            {
                                disp = ctx.Set(new ContextExpression(false, false, expr));
                            }
                        }
                        break;
                    case '.':
                        {
                            string fmt = format[++i].ToString();
                            int index = int.Parse(fmt);
                            ContextValue<ContextExpression?>? expr = _expression[index];
                            disp = expr.Set(new ContextExpression(false, false, node));
                            break;
                        }
                    case '&':
                        if (disp == null)
                            disp = _methodExpr.Set(node);
                        else
                            disp.Dispose();
                        break;
                    case '\\':
                        {
                            char f = format[++i];
                            Query.Append(f);
                            break;
                        }
                    case '=':
                        {
                            char last2 = Query[^2];
                            char last1 = Query[^1];
                            if (last2 == '+' && last1 == ' ')
                                Query.Remove(Query.Length - 1, 1);

                            char f = format[i];
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
            disp?.Dispose();
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

        #region HandleProperties

        /// <summary>
        /// Handles the properties.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">name</exception>
        private void HandleProperties(object? name)
        {
            string parameterName = name?.ToString() ?? throw new ArgumentNullException(nameof(name));
            AppendPropSeparator();
            if (_expression[0].Value != null)
            {
                Query.Append("$");
                var length = Query.Length;
                Visit(_expression[0].Value);
                parameterName = Query.ToString().Substring(length) + parameterName;
            }
            else if (_expression[2].Value != null && _expression[4].Value != null &&
                _expression[2].Value?.Expression == _expression[4].Value?.Expression)
            {
                Visit(_expression[2].Value);
                Query.Append(".");
            }
            else if (_expression[2]?.Value != null && _expression[2].Value?.Expression is ParameterExpression pe && pe.Name == _isCustomProp.Value)
            {

            }
            else
            {
                string? prefix = _propPrefix.Value;
                Query.Append(prefix ?? "$");
            }
            Query.Append(_isCustomProp.Value ?? name);
            Parameters[parameterName] = null;
        }

        #endregion // HandleProperties

        #region UseGenericsAsLabel

        /// <summary>
        /// Uses the generics as label.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        private bool UseGenericsAsLabel(Expression arg)
        {
            if (!(arg is ConstantExpression c))
                return true;
            var result = !(c.Type == typeof(LabelFromGenerics) &&
                                        Equals(c.Value, LabelFromGenerics.Ignore));
            return result;
        }

        #endregion // UseGenericsAsLabel

        #region SetGenPropertiesContext

        /// <summary>
        /// Sets the gen properties context.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="expr">The expr.</param>
        /// <returns></returns>
        private IDisposable SetGenPropertiesContext(MethodCallExpression node, Expression expr)
        {
            IDisposable result = DisposeableAction.Empty;
            string? eq = EqualPattern();
            if (eq == null)
                return result;

            var args = node.Arguments;
            Expression? arg0 = args.FirstOrDefault();
            if (arg0?.Type != typeof(Fluent))
                return result;

            MethodCallExpression? propExpr = args.OfType<MethodCallExpression>()
                                   .LastOrDefault(
                                        m => m.Type == typeof(IProperties) ||
                                        m.Type == typeof(IPropertyOfType) ||
                                        m.Type == typeof(IPropertiesOfType));
            if (propExpr == null)
                return result;

            var propArgs = propExpr.Arguments;
            var varExpr = propArgs.FirstOrDefault() as ParameterExpression;
            var lambdaEpr = propArgs.Skip(1).LastOrDefault();

            if (varExpr == null || lambdaEpr == null)
                return result;

            var lambdaType = lambdaEpr.GetType();
            if (!lambdaType.IsGenericType)
                return result;

            var lambdaArgs = lambdaType.GenericTypeArguments;
            if (lambdaArgs.Length != 1)
                return result;

            var lambda = lambdaArgs[0];
            var genArgs = lambda.GenericTypeArguments;
            if (genArgs.Length == 2 &&
                genArgs[1] == typeof(object))
            {
                result = _varExtension.Set(varExpr?.Name);
            }

            return result;
        }

        #endregion // SetGenPropertiesContext

        #region AddGenPrefix

        /// <summary>
        /// Adds the gen prefix.
        /// </summary>
        private void AddGenPrefix()
        {
            var prefix = _varExtension.Value;
            if (!string.IsNullOrEmpty(prefix))
            {
                bool shouldAppend = false;
                string append = $"{prefix}.";
                int qlen = Query.Length;
                int start = qlen - append.Length;
                if (start > 0)
                {
                    int j = 0;
                    for (int i = start; i < Query.Length && !shouldAppend; i++)
                    {
                        if (Query[i] != append[j++])
                            shouldAppend = true;
                    }
                }
                if (shouldAppend)
                    Query.Append(append);
            }
        }

        #endregion // AddGenPrefix

        #region IsCustomProp

        /// <summary>
        /// Determines whether [is custom property] [the specified node].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private IDisposable IsCustomProp(MethodCallExpression node)
        {
            if (node.Method?.Name != nameof(Cypher.P_))
                return DisposeableAction.Empty;

            var arg1 = node.Arguments[1];
            switch (arg1)
            {
                case ParameterExpression e:
                    return _isCustomProp.Set(e.Name);
                case MemberExpression e:
                    return _isCustomProp.Set(e.Member.Name);
                case LambdaExpression e:
                    if (e?.Body is MemberExpression e1)
                        return _isCustomProp.Set(e1.Member.Name);
                    break;

            }
            return DisposeableAction.Empty;
        }

        #endregion // IsCustomProp

        #region IsProperty

        /// <summary>
        /// Determines whether the specified type is property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private IDisposable IsProperty(Type type)
        {
            var result = type.Name switch
            {
                nameof(IProperty) => _isProperties.Set(true),
                nameof(IProperties) => _isProperties.Set(true),
                nameof(IPropertyOfType) => _isProperties.Set(true),
                nameof(IPropertiesOfType) => _isProperties.Set(true),
                _ => DisposeableAction.Empty
            };
            return result;
        }

        #endregion // IsProperty

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
