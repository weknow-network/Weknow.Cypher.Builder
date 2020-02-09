using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Cypher;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    public class CypherVisitor : ExpressionVisitor
    {
        private readonly CypherConfig _configuration; // TODO: Use to format cypher

        public CypherVisitor(CypherConfig configuration)
        {
            _configuration = configuration;
        }

        public StringBuilder Query { get; } = new StringBuilder();
        public Dictionary<string, object?> Parameters { get; } = new Dictionary<string, object?>();

        private ContextValue<bool> _isProperties = new ContextValue<bool>(false);
        private ContextValue<MethodCallExpression?> _methodExpr = new ContextValue<MethodCallExpression?>(null);
        private ContextValue<FormatingState> _formatter = new ContextValue<FormatingState>(FormatingState.Default);
        private ContextValue<string> _reusedParameterName = new ContextValue<string>(null);
        private Dictionary<int, ContextValue<Expression?>> _expression = new Dictionary<int, ContextValue<Expression?>>()
        {
            [0] = new ContextValue<Expression?>(null),
            [1] = new ContextValue<Expression?>(null),
            [2] = new ContextValue<Expression?>(null),
            [3] = new ContextValue<Expression?>(null),
        };
        private List<Expression> _reuseParameters = new List<Expression>();
        private List<string> _reuseParameterNames = new List<string>();

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (_reuseParameterNames.Count < _reuseParameters.Count)
                _reuseParameterNames.Add(node.Parameters[0].Name);

            Visit(node.Body);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    if (node.Left.Type != typeof(IRelation) && node.Right.Type != typeof(IRelation))
                        Query.Append("-");
                    Query.Append("->");
                    break;
                case ExpressionType.LessThan:
                    Query.Append("<-");
                    if (node.Left.Type != typeof(IRelation) && node.Right.Type != typeof(IRelation))
                        Query.Append("-");
                    break;
                case ExpressionType.Subtract:
                    Query.Append("-");
                    if (node.Left.Type != typeof(IRelation) && node.Right.Type != typeof(IRelation))
                        Query.Append("-");
                    break;
                case ExpressionType.Equal:
                    Query.Append(" = ");
                    break;
                case ExpressionType.Add:
                    Query.Append(" += ");
                    break;
            }
            Visit(node.Right);
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            Visit(node.Operand);
            var formatter = _formatter.Value;
            if (_methodExpr.Value?.Method.Name == "Set" && formatter.Index + 2 < formatter.Format.Length && formatter.Format[formatter.Index + 2] == '=' && node.NodeType == ExpressionType.UnaryPlus)
            {
                formatter++;
                Query.Append(" +");
            }

            return node;
        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            using var _ = _isProperties.Set(_isProperties.Value || node.Method.ReturnType == typeof(IProperties));

            var attributes = node.Method.GetCustomAttributes(typeof(CypherAttribute), false);
            var format = attributes.Length > 0 ? (attributes[0] as CypherAttribute)?.Format : null;
            if (format != null)
            {
                IDisposable? disp = null;
                using var formatter = _formatter.Set(new FormatingState(format));
                for (var i = _formatter.Value; !i.Ended; i++)
                {
                    switch (format[i])
                    {
                        case '$':
                            var expr = node.Arguments[int.Parse(format[++i].ToString())];
                            Visit(expr);
                            break;
                        case '!':
                            Query.Append(node.Method.GetGenericArguments()[int.Parse(format[++i].ToString())].Name);
                            break;
                        case '+':
                            disp = _expression[int.Parse(format[++i].ToString())].Set(node.Arguments[int.Parse(format[++i].ToString())]);
                            break;
                        case '.':
                            disp = _expression[int.Parse(format[++i].ToString())].Set(node);
                            break;
                        case '&':
                            disp = _methodExpr.Set(node);
                            break;
                        case '\\':
                            Query.Append(format[++i]);
                            break;
                        default:
                            Query.Append(format[i]);
                            break;
                    }
                }
                disp?.Dispose();
            }
            else if (node.Method.Name == nameof(IReuse<PD, PD>.By))
            {
                Visit(node.Object);
                Visit(node.Arguments[0]);
            }
            else if (node.Method.Name == nameof(Cypher.Reuse))
            {
                if (node.Arguments.Count == 2)
                {
                    _reuseParameters.Add(node.Arguments[1]);
                    Visit(node.Arguments[0]);
                }
                else
                    _reuseParameters.Add(node.Arguments[0]);
            }
            else if (node.Method.Name == nameof(Range.EndAt))
            {   // TODO: consider to move implementation into VisitUnary
                Query.Append("*..");
                var index = node.Arguments[0] as UnaryExpression;
                Query.Append(index?.Operand);
            }
            else if (node.Method.Name == "get_" + nameof(Range.All))
            {
                Query.Append("*");
            }
            else if (node.Method.Name == nameof(Cypher.All))
            {
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
                    MethodCallExpression? methodExp = _expression[1].Value as MethodCallExpression;
                    var properties = methodExp == null ?
                                    Array.Empty<PropertyInfo>() :
                                    methodExp.Method.GetGenericArguments()[0].GetProperties();

                    NewArrayExpression? arrayExp = node.Arguments[0] as NewArrayExpression;
                    string[] exclude = arrayExp == null ?
                        Array.Empty<string>() :
                        arrayExp.Expressions.OfType<MemberExpression>().Select(x => x.Member.Name).ToArray();
                    foreach (var item in properties)
                    {
                        if (exclude.Contains(item.Name)) continue;

                        Query.Append(item.Name);
                        Query.Append(": $");
                        Query.Append(item.Name);
                        if (item != properties.Last())
                            Query.Append(", ");
                    }
                }
            }
            else if (node.Method.Name == nameof(Cypher.Convention))
            {
                var filter = (node.Arguments[node.Arguments.Count == 1 ? 0 : 1] as Expression<Func<string, bool>>)?.Compile();
                var arguments = node.Method.IsGenericMethod
                    ? node.Method.GetGenericArguments()
                    : (_expression[1].Value as MethodCallExpression)?.Method?.GetGenericArguments();
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

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && (!_isProperties.Value || _methodExpr.Value?.Method.Name == "Set"))
            {
                Visit(node.Expression);
                if (node.Member.Name == nameof(IVar.AsMap))
                    return node;
                Query.Append(".");
            }
            if (node.Type == typeof(ILabel))
            {
                Query.Append(":");
                Query.Append(_configuration.AmbientLabels.Combine(node.Member.Name));
                return node;
            }
            Query.Append(node.Member.Name);
            if (_isProperties.Value)
            {
                Query.Append(_methodExpr.Value?.Method.Name == "Set" ? " = " : ": ");
                if (_expression[0].Value != null)
                {
                    Query.Append("$");
                    Visit(_expression[0].Value);
                }
                else if (_expression[2].Value != null)
                {
                    Visit(_expression[2].Value);
                    Query.Append(".");
                }
                else
                    Query.Append("$");
                Query.Append(node.Member.Name);
                Parameters[node.Member.Name] = null;
            }
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            foreach (var expr in node.Expressions)
            {
                if (_expression[3].Value != null)
                {
                    Visit(_expression[3].Value);
                    Query.Append(".");
                }
                Visit(expr);
                if (expr != node.Expressions.Last())
                    Query.Append(", ");
            }
            return node;
        }

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

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var parameterName = $"p_{Parameters.Count}";
            Query.Append($"${parameterName}");
            Parameters[parameterName] = node.Value;
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_reusedParameterName != node.Name && _reuseParameterNames.Contains(node.Name))
            {
                using var _ = _reusedParameterName.Set(node.Name);
                Visit(_reuseParameters[_reuseParameterNames.IndexOf(node.Name)]);
            }
            else
                Query.Append(node.Name);
            return node;
        }
    }

}
