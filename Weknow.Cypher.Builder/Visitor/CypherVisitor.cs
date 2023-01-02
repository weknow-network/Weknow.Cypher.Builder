using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using Weknow.CypherBuilder.Declarations;
using Weknow.Disposables;
using Weknow.Mapping;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder
{
    /// <summary>
    /// The cypher visitor is the heart of the ORM implementation
    /// </summary>
    /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
    internal sealed class CypherVisitor : ExpressionVisitor, IDisposable
    {
        private const string EXT_ASSEMBLY_NAME = "Weknow.Cypher.Builder.Extensions";
        private const string AUTO_VAR = "$auto-var$";
        private static readonly int AUTO_VAR_LEN = AUTO_VAR.Length;
        private int _autoVarCounter = 0;
        private readonly CypherConfig _configuration;
        private readonly CypherFlavor _flavor;

        private readonly HashSet<string> _ambientOnce = new();
        private readonly AmbientContextStack _shouldHandleAmbient = new AmbientContextStack();
        private bool _isRawChypher = false;
        private readonly IStackCancelable<bool> _shouldCreateParameter = Disposable.CreateStack(true);
        private readonly IStackCancelable<bool> _isCypherInput = Disposable.CreateStack(false);
        private readonly IStackCancelable<IgnoreBehavior> _ignoreScope = Disposable.CreateStack(IgnoreBehavior.None);

        private readonly ContextValue<bool> _isProperties = new ContextValue<bool>(false);


        // track the recent cypher operator (in contrast with _methodExpr which pick specific operators)
        private readonly IStackCancelable<string> _directOperation = Disposable.CreateStack(string.Empty);
        private readonly ContextValue<MethodCallExpression?> _methodExpr = new ContextValue<MethodCallExpression?>(null);

        private readonly Dictionary<int, ContextValue<Expression?>> _expression = new Dictionary<int, ContextValue<Expression?>>()
        {
            [0] = new ContextValue<Expression?>(null),
        };

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CypherVisitor(CypherConfig configuration)
        {
            _configuration = configuration;
            _flavor = configuration.Flavor;
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

        public CypherParameters _parameters = new CypherParameters();

        /// <summary>
        /// Mutable state of the parameters.
        /// Parameters build during the visitor traverse.
        /// </summary>
        public CypherParameters Parameters => _parameters;

        #endregion // Parameters

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
            if (node.Type.Name.StartsWith(nameof(FluentUnwindAction)))
            {
                Query.Append(node.Parameters[0].Name);
                Query.Append(Environment.NewLine);
            }
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

            bool isRightNull = node.Right is ConstantExpression rexp && rexp.Value == null;

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
                    if (isRightNull)
                    {
                        Query.Append(" IS NULL ");
                        return node;
                    }
                    string? eq = EqualPattern();
                    Query.Append(eq);
                    break;
                case ExpressionType.NotEqual:
                    if (isRightNull)
                    {
                        Query.Append(" IS NOT NULL ");
                        return node;
                    }
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
                case ExpressionType.And:
                    string sign = AndRepresentation();
                    Query.Append(sign);
                    break;
                case ExpressionType.AndAlso:
                    Query.Append(" AND ");
                    break;
                case ExpressionType.OrElse:
                    Query.Append(" OR ");
                    break;
                case ExpressionType.Modulo:
                    Query.Append(" % ");
                    break;
                case ExpressionType.Divide:
                    Query.Append(" / ");
                    break;
                case ExpressionType.Multiply:
                    Query.Append(" * ");
                    break;
                    //case ExpressionType.UnaryPlus:
                    //    Query.Append(" * ");
                    //    break;
            }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Visit(node.Right);
            return node;
        }

        #endregion // VisitBinaryVisitUnary

        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            var res = base.Visit(node);
            return res;
        }

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
            var mtd = node.Method;
            string name = mtd.Name;
            ReadOnlyCollection<Expression> args = node.Arguments;
            string type = node.Type.Name;
            string? assembly = node?.Type?.Assembly?.GetName()?.Name;

            #region string? format = ...

            var atts = mtd.GetCustomAttributes<CypherAttribute>(false).ToArray();
            string? format = atts.Where(m => m.Flavor == _flavor)
                            .Select(att => att.Format)
                            .FirstOrDefault();
            if(format == null && atts.Length != 0 && _flavor != CypherFlavor.OpenCypher) 
            {
                format = atts
                            .Select(att => att.Format)
                            .FirstOrDefault();
            }

            #endregion // string? format = ...

            #region Proc

            if (node.Object is MethodCallExpression om && 
                om.Method.Name == "Proc" &&
                om.Type.Assembly.GetName().Name == EXT_ASSEMBLY_NAME)
            {
                
                Visit(node.Object);
            }

            #endregion // Proc

            if (name == "Proc" && assembly == EXT_ASSEMBLY_NAME)
            {
                #region Proc

                if(format != null)
                {
                    ApplyFormat(node, format);
                }
                else if (args.Count != 0)
                {
                    Visit(args[0]);
                }
                return node;

                #endregion // Proc
            }
            else if (name == nameof(CypherExtensions.NoAmbient))
            {
                int idx = 0;
                if (args.Count == 2)
                {
                    Visit(args[0]);
                    idx = 1;
                    Query.Append(Environment.NewLine);
                }
                using (_ignoreScope.Push(IgnoreBehavior.Ambient))
                {
                    Visit(args[idx]);
                    return node;
                }
            }


            if (name == nameof(Array.Empty) &&
                mtd.DeclaringType?.Name == nameof(Array))
            {
                Query.Append("[]");
                return node;
            }


            var opScope = Disposable.Empty;
            if (mtd.GetCustomAttribute<CypherClauseAttribute>() != null)
            {
                opScope = _directOperation.Push(name);
            }
            using var opScp = opScope;

            bool shouldCreatePrms = name switch
            {
                nameof(CypherExtensions.Case) => false,
                nameof(CypherExtensions.When) => false,
                nameof(CypherExtensions.Then) => false,
                nameof(CypherExtensions.Else) => false,
                nameof(CypherGeneralExtensions.As) => false,
                _ => true
            };
            using var _ = _shouldCreateParameter.Push(shouldCreatePrms);


            if (name == nameof(CypherExtensions.Foreach) && mtd.GetCustomAttribute<ObsoleteAttribute>() == null)
            {
                LambdaExpression iteration = args.Count switch
                {
                    2 => args[1] as LambdaExpression ?? throw new ArgumentNullException(nameof(FluentForEachAction)),
                    3 => args[2] as LambdaExpression ?? throw new ArgumentNullException(nameof(FluentForEachAction)),
                    _ => throw new NotSupportedException(nameof(CypherExtensions.Foreach))
                };
                Expression items = args.Count switch
                {
                    2 => args[0],
                    3 => args[1],
                    _ => throw new NotSupportedException(nameof(CypherExtensions.Foreach))
                };
                if (args.Count == 3)
                {
                    Visit(args[0]);
                    Query.Append(Environment.NewLine);
                }
                Query.Append("FOREACH (");
                var variable = iteration.Parameters[0];
                Query.Append(variable.Name);
                Query.Append(" IN ");
                using (_shouldCreateParameter.Push(items.NodeType != ExpressionType.NewArrayInit))
                {
                    Visit(items);
                }
                Query.Append(" |");
                Query.Append(Environment.NewLine);
                Query.Append("\t");
                Visit(iteration.Body);
                Query.Append(")");
            }
            if (name == nameof(CypherExtensions.SetAmbientLabels))
            {
                Visit(args[0]);
                if (_configuration.AmbientLabels.Values.Count != 0)
                {
                    Query.Append(Environment.NewLine);
                    Query.Append("SET ");
                    Visit(args[1]);
                    Query.Append(":");
                    using (_ignoreScope.Push(IgnoreBehavior.None))
                    using (_shouldHandleAmbient.Activate())
                    {
                        HandleAmbientLabels(node);
                    }
                }
            }
            else if (format != null)
            {
                bool ambScope = node.Type == typeof(INode);
                using (ambScope ? _shouldHandleAmbient.Activate() : Disposable.Empty)
                {
                    ApplyFormat(node, format);
                }
            }
            else if (type == nameof(Rng))
            {
                if (name == nameof(Rng.Scope))
                {
                    Query.Append("*");
                    var index0 = (ConstantExpression)node.Arguments[0];
                    Query.Append(index0.Value);
                    Query.Append("..");
                    var index1 = (ConstantExpression)node.Arguments[1];
                    Query.Append(index1.Value);
                }
                else if (name == nameof(Rng.AtMost))
                {
                    Query.Append("*..");
                    var index = (ConstantExpression)node.Arguments[0];
                    Query.Append(index.Value);
                }
                else if (name == nameof(Rng.AtLeast))
                {
                    Query.Append("*");
                    var index = (ConstantExpression)node.Arguments[0];
                    Query.Append(index.Value);
                    Query.Append("..");
                }
                else if (name == nameof(Rng.Any))
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

            bool shouldTryHandleAmbient = true;
            bool shouldCreatePrm = _shouldCreateParameter.State;
            if (shouldCreatePrm && node.Member.Name == nameof(DateTime.Now) && node.Member.DeclaringType == typeof(DateTime))
            {
                var parameterName = $"p_{Parameters.Count}";
                Query.Append($"${parameterName}");
                _parameters = _parameters.AddOrUpdate(parameterName, DateTime.Now);
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
                HandleAmbientLabels(node, name);
                return node;
            }
            else if (node.Type == typeof(IType))
            {
                if (node.Expression is MemberExpression tme && node.Member.Name == nameof(ILabel.R))
                {
                    name = tme.Member.Name;
                }
            }

            if (typeof(ParameterDeclaration).IsAssignableFrom(node.Type))
            {
                Query.Append("$");
                if (node.Member.Name is (nameof(VariableDeclaration.AsParameter)) or (nameof(VariableDeclaration.Prm)))
                {
                    if (node.Expression is MemberExpression nme)
                    {
                        name = nme?.Member?.Name ?? throw new ArgumentNullException("((MemberExpression?)node.Expression)?.Member?.Name");
                    }
                    else
                    {
                        name = node.Expression?.ToString() ?? throw new ArgumentNullException("Null expression");
                    }
                }
                if (!Parameters.ContainsKey(name))
                    Parameters.SetToNull(name);
            }
            else if (node.Expression is MemberExpression me &&
                    (me.Member.Name is nameof(ParameterDeclaration<int>._) or nameof(ParameterDeclaration<int>.__))
                    && typeof(ParameterDeclaration).IsAssignableFrom(me.Member.DeclaringType))
            {
                Query.Append("$");
                bool addNullPrm = true;
                if (me.Expression is UnaryExpression ue && ue.NodeType == ExpressionType.Not &&
                    ue.Operand is MemberExpression ime)
                {
                    string candidateVariable = string.Empty;
                    var candidateLen = candidateVariable.Length;
                    if (ime.Expression is MemberExpression me2 &&
                        ime.Member.Name is nameof(VariableDeclaration.AsParameter) or nameof(VariableDeclaration.Prm))
                    {
                        candidateVariable = $"{me2.Member.Name}.";
                    }
                    else
                        candidateVariable = $"{ime.Member.Name}.";

                    if (Query[^candidateLen..].ToString() != candidateVariable)
                        Query.Append(candidateVariable);
                    addNullPrm = false;
                }
                else if (me.Expression is MemberExpression me1 && me.Member.Name == nameof(ParameterDeclaration<int>.__))
                {
                    if (me1.Expression is MemberExpression me2 &&
                        me1.Member.Name is nameof(VariableDeclaration.AsParameter) or nameof(VariableDeclaration.Prm))
                    {
                        Query.Append(me2.Member.Name);
                    }
                    else
                        Query.Append(me1.Member.Name);
                    Query.Append(".");
                    addNullPrm = false;
                }
                if (addNullPrm)
                    Parameters.SetToNull(name);
            }
            else if (node.Expression is MemberExpression me__ && me__.Member.Name == nameof(ParameterDeclaration<int>.__)
                && typeof(ParameterDeclaration).IsAssignableFrom(me__.Member.DeclaringType))
            {
                Query.Append("$");
                if (me__.Expression is UnaryExpression ue && ue.NodeType == ExpressionType.Not &&
                    ue.Operand is MemberExpression ime)
                {
                    string candidateVariable = $"{ime.Member.Name}.";
                    var candidateLen = candidateVariable.Length;
                    if (Query[^candidateLen..].ToString() != candidateVariable)
                        Query.Append(ime.Member.Name);
                }
                if (!Parameters.ContainsKey(name))
                    Parameters.SetToNull(name);
            }
            else if (node.Expression is MemberExpression vme &&
                (vme.Member.Name == nameof(VariableDeclaration<int>._) ||
                vme.Member.Name == nameof(VariableDeclaration<int>.__))
                && typeof(VariableDeclaration).IsAssignableFrom(vme.Member.DeclaringType))
            {
                string candidateVariable = string.Empty;
                if (vme.Expression is UnaryExpression ue && ue.NodeType == ExpressionType.Not &&
                    ue.Operand is MemberExpression ime)
                {
                    candidateVariable = $"{ime.Member.Name}.";
                }
                if (vme.Member.Name == nameof(VariableDeclaration<int>.__) && vme.Expression is MemberExpression vme1)
                {
                    candidateVariable = $"{vme1.Member.Name}.";
                }
                if (vme.Member.Name == nameof(VariableDeclaration<int>.__) && vme.Expression is ParameterExpression vmpe)
                {
                    candidateVariable = $"{vmpe.Name}.";
                }
                var candidateLen = candidateVariable.Length;
                if (Query[^candidateLen..].ToString() != candidateVariable)
                {
                    Query.Append(candidateVariable);
                }
            }
            else if (node.Expression is MethodCallExpression pme && pme.Method.Name == "_"
                && typeof(ParameterDeclaration).IsAssignableFrom(pme.Method.DeclaringType))
            {
                Query.Append("$");
                if (!Parameters.ContainsKey(name))
                    Parameters.SetToNull(name);
            }
            else if (node.Expression is MethodCallExpression mce &&
                (mce.Method.Name == "__" || mce.Method.Name == "_") &&
                typeof(VariableDeclaration).IsAssignableFrom(mce.Method.DeclaringType))
            {
                if (mce.Method.Name == "__" && mce?.Object?.Type == typeof(VariableDeclaration))
                {
                    string candidateVariable = $"{mce.Object}.";
                    var candidateLen = candidateVariable.Length;
                    if (Query[^candidateLen..].ToString() != candidateVariable)
                        Query?.Append(candidateVariable);
                }
            }
            else if (name == nameof(VariableDeclaration.NoAmbient) && node.Type == typeof(VariableDeclaration))
            {
                name = node?.Expression?.ToString()!;
                shouldTryHandleAmbient = false;
            }

            if (node.Type == typeof(IType))
            {
                name = _configuration.Naming.ConvertToTypeConvention(name);
            }
            Query?.Append(name);
            if (node.Type == typeof(VariableDeclaration) && shouldTryHandleAmbient)
            {
                HandleAmbientLabels(node);
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
            string separator = AndRepresentation();

            if (!_isCypherInput.State)
                Query.Append("[");
            foreach (var expr in node.Expressions)
            {
                Visit(expr);
                bool isLabelsOrType = node.Type == typeof(ILabel[]) || node.Type == typeof(IType[]);
                if (expr != node.Expressions.Last())
                {
                    if (isLabelsOrType)
                        Query.Append(separator);
                    else
                        Query.Append(", ");
                }
            }
            if (!_isCypherInput.State)
                Query.Append("]");
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
            bool isEnumerable = node.Type.IsAssignableTo(typeof(IEnumerable));
            bool isObject = _expression[0].Value == null;
            bool ignoreEnumerable = Query[Query.Length - 1] == '[';
            if (isObject)
            {
                if (isEnumerable)
                {
                    if (!ignoreEnumerable && !_isCypherInput.State)
                        Query.Append("[");
                }
                else
                    Query.Append("{ ");
            }
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                if (_expression[0].Value != null)
                {
                    Visit(_expression[0].Value);
                    Query.Append('.');
                }
                if (node.Members == null) throw new ArgumentNullException("node.Members");
                Query.Append(node.Members[i].Name);
                AppendPropSeparator();
                Expression? expr = node.Arguments[i];
                Visit(expr);
                if (expr != node.Arguments.Last())
                    Query.Append(", ");
            }
            if (isObject)
            {
                if (isEnumerable)
                {
                    if (!ignoreEnumerable && !_isCypherInput.State)
                        Query.Append("]");
                }
                else
                    Query.Append(" }");
            }
            return node;
        }

        #endregion // VisitNew

        #region VisitMemberInit

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

        #endregion // VisitMemberInit

        #region VisitMemberBinding

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            Query.Append(node.Member.Name);
            AppendPropSeparator();
            return base.VisitMemberBinding(node);
        }

        #endregion // VisitMemberBinding

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
            bool isReturn = _methodExpr.Value?.Method.Name == nameof(CypherExtensions.Return);
            bool isAnalyzer = node.Type.Name == "FullTextAnalyzer";
            bool shouldCreatePrm = _shouldCreateParameter.State;

            if (node.Type.FullName == typeof(ConstraintType).FullName)
            {
                Query.Append(node.Value?.ToString()?.ToSCREAMING(' '));
            }
            else if (!shouldCreatePrm || isReturn || isAnalyzer || _isRawChypher)
            {
                Query.Append(node.Value);
            }
            else
            {
                var parameterName = $"p_{Parameters.Count}";
                Query.Append($"${parameterName}");
                _parameters = _parameters.AddOrUpdate(parameterName, node.Value);
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
            string? name = node.Name;
            if (name == null) throw new ArgumentNullException("VisitParameter");
            Query.Append(name);
            if (!_ambientOnce.Contains(name))
            {
                HandleAmbientLabels(node);
                _ambientOnce.Add(name);
            }
            return node;
        }

        #endregion // VisitParameter

        #region VisitUnary

        protected override Expression VisitUnary(UnaryExpression node)
        {
            // TODO: [bnaya 2022-12-06] support n += $map

            if (node.NodeType == ExpressionType.Not)
                Query.Append("!");
            return base.VisitUnary(node);
        }

        #endregion // VisitUnary

        #region VisitListInit

        protected override Expression VisitListInit(ListInitExpression node)
        {
            if (!_isCypherInput.State)
                Query.Append("[");

            var result = base.VisitListInit(node);

            if (Query[^2..].ToString() == ", ")
                Query.Remove(Query.Length - 2, 2);

            if (!_isCypherInput.State)
                Query.Append("]");

            return result;
        }

        #endregion // VisitListInit

        #region VisitElementInit

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            var result = base.VisitElementInit(node);
            Query.Append(", ");
            return result;
        }

        #endregion // VisitElementInit

        #region ApplyFormat

        /// <summary>
        /// Applies the format.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="format">The format.</param>
        private void ApplyFormat(MethodCallExpression node, string format)
        {
            var disp = new List<IDisposable>();
            MethodInfo mtd = node.Method;
            var mtdPrms = mtd.GetParameters();

            for (var i = 0; i < format.Length; i++)
            {
                switch (format[i])
                {
                    case '$':
                        {
                            if (i + AUTO_VAR_LEN < format.Length)
                            {
                                bool isAutoVar = format[(i)..(i + AUTO_VAR_LEN)] == AUTO_VAR;
                                if (isAutoVar)
                                {
                                    i += AUTO_VAR_LEN - 1;
                                    Query.Append($"var_{_autoVarCounter++}");
                                    continue;
                                }
                            }
                            i++;
                            bool isArray = format.Length > i + 1 &&
                                          format[(i)..(i + 2)] == "[]";
                            if (isArray)
                                i += 2;

                            var ch = format[i];
                            int index = int.Parse(ch.ToString());
                            var args = node.Arguments;
                            Expression expr = args[index];
                            bool isCypherInput = mtdPrms[index].GetCustomAttribute<CypherInputCollectionAttribute>() != null;
                            IDisposable inputScope = Disposable.Empty;
                            if (isCypherInput != _isCypherInput.State)
                                inputScope = _isCypherInput.Push(isCypherInput);
                            using (inputScope)
                            {
                                if (index == args.Count - 1)
                                {
                                }

                                int count = args.Count;
                                // handling case of safe params array (when having ParamsFirst parameter to avoid empty array)
                                if (expr is NewArrayExpression naExp &&
                                    expr.NodeType == ExpressionType.NewArrayInit)
                                {
                                    if (isArray)
                                    {
                                        var parameterName = $"p_{Parameters.Count}";
                                        Query.Append(parameterName);
                                        _parameters = _parameters.SetToNull(parameterName); // naExp.Value;
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

                                bool isIndexConstraint = node is MethodCallExpression mc && mc.Method.Name switch
                                {
                                    nameof(ICypher.CreateConstraint) => true,
                                    nameof(ICypher.TryCreateConstraint) => true,
                                    nameof(ICypher.CreateIndex) => true,
                                    nameof(ICypher.TryCreateIndex) => true,
                                    nameof(ICypher.TryDropConstraint) => true,
                                    nameof(ICypher.DropConstraint) => true,
                                    nameof(ICypher.TryDropIndex) => true,
                                    nameof(ICypher.DropIndex) => true,
                                    nameof(ICypher.CreateTextIndex) => true,
                                    nameof(ICypher.TryCreateFullTextIndex) => true,
                                    nameof(ICypher.CreateFullTextIndex) => true,
                                    nameof(ICypher.TryCreateTextIndex) => true,
                                    _ => false
                                };

#pragma warning disable CS0618
                                _isRawChypher = expr.Type.Name == nameof(RawCypher) || isIndexConstraint;
#pragma warning restore CS0618
                                using (isIndexConstraint ? _shouldHandleAmbient.Deny() : Disposable.Empty)
                                {
                                    bool isVar = expr.Type.IsAssignableTo(typeof(VariableDeclaration));
                                    var qlen = Query.Length;
                                    Visit(expr);
                                    if (isVar)
                                    {
                                        if (count > index + 1)
                                        {
                                            Expression nextEXpr = args[index + 1];
                                            if (nextEXpr.Type == typeof(ILabel) ||
                                                nextEXpr.Type == typeof(IType))
                                            {
                                                string addition = Query.ToString(qlen..);
                                                if (addition.IndexOf(':') == -1) // check if addition is having the first ':'
                                                {
                                                    Query.Append(":");
                                                }
                                                else
                                                {
                                                    string sep = AndRepresentation();
                                                    Query.Append(sep);
                                                }
                                            }
                                            else if (nextEXpr is NewArrayExpression nae &&
                                                        nae.Expressions.Count != 0)
                                            {
                                                Expression first = nae.Expressions.First();
                                                if (first.Type == typeof(ILabel) ||
                                                first.Type == typeof(IType))
                                                {
                                                    Query.Append(":");
                                                }
                                            }
                                        }
                                    }
                                }
                                _isRawChypher = false;
                            }
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
                nameof(CypherExtensions.Set) => " = ",
                nameof(CypherExtensions.Where) => " = ",
                nameof(CypherExtensions.OnCreateSet) => " = ",
                nameof(CypherExtensions.OnMatchSet) => " = ",
                nameof(CypherExtensions.When) => " = ",
                nameof(ICypher.Rgx) => " =~ ",
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

        #region AmbientContextStack

        private enum AmbientContextState
        {
            None,
            Active,
            Deny,
        }

        /// <summary>
        /// Should handle ambient stack
        /// </summary>
        /// <seealso cref="System.IDisposable" />
        private class AmbientContextStack
        {
            private AmbientContextState _value;
            public bool Value => _value == AmbientContextState.Active;

            public IDisposable Activate()
            {
                if (_value != AmbientContextState.Deny)
                    _value = AmbientContextState.Active;
                return Disposable.Create(Deactivate);
            }

            public void Deactivate()
            {
                if (_value == AmbientContextState.Active)
                    _value = AmbientContextState.None;
            }

            public IDisposable Deny()
            {
                _value = AmbientContextState.Deny;
                return Disposable.Create(Undeny);
            }

            public void Undeny()
            {
                if (_value == AmbientContextState.Deny)
                    _value = AmbientContextState.None;
            }
        }

        #endregion // AmbientContextStack

        #region HandleAmbientLabels

        private void HandleAmbientLabels(Expression node, params string[] labels)
        {
            if (_configuration.AmbientLabels.Values.Count == 0 && (labels == null || labels.Length == 0))
                return;

            string separator = AndRepresentation();
            if ((_ignoreScope.State & IgnoreBehavior.Ambient) == IgnoreBehavior.Ambient || !_shouldHandleAmbient.Value)
            {
                if (labels == null || labels.Length == 0)
                    return;

                HandleStartChar();


                IEnumerable<string> formatted = labels.Select(m => _configuration.AmbientLabels.FormatByConvention(m));
                var addition = string.Join(separator, formatted);
                Query.Append(addition);
                return;
            }

            _shouldHandleAmbient.Deactivate();

            HandleStartChar();

            Query.Append(_configuration.AmbientLabels.Combine(separator, labels));

            void HandleStartChar()
            {
                if (!node.Type.Name.StartsWith(nameof(VariableDeclaration)))
                    return;
                char lastChar = Query[^1];
                if (lastChar != ':')
                    Query.Append(text: ':');
            }
        }

        #endregion // HandleAmbientLabels

        #region IsNeo4jAndExpression()
        private bool IsNeo4jAndExpression() => _directOperation.State switch
        {
            nameof(ICypher.Match) => true,
            nameof(ICypher.OptionalMatch) => true,
            nameof(CypherExtensions.Where) => true,
            nameof(CypherExtensions.When) => true,
            nameof(CypherExtensions.With) => true,
            nameof(CypherExtensions.Return) => true,
            nameof(CypherExtensions.ReturnDistinct) => true,
            _ => false
        };

        #endregion // IsNeo4jAndExpression()

        #region AndRepresentation

        /// <summary>
        /// <![CDATA[representation of &.]]>
        /// </summary>
        /// <returns></returns>
        private string AndRepresentation()
        {
            if (_flavor == CypherFlavor.Neo4j5 && IsNeo4jAndExpression())
                return "&";
            return ":";
        }

        #endregion // AndRepresentation
    }
}
