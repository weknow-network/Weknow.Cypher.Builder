using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Pattern;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    public class DisposeableAction : IDisposable
    {
        Action _action;

        public DisposeableAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }

    public class ContextValue<T>
    {
        private Stack<T> values = new Stack<T>();

        public T Value
        {
            get => values.Peek();
        }

        public ContextValue(T defaultValue)
        {
            values.Push(defaultValue);
        }

        public IDisposable Set(T value)
        {
            values.Push(value);
            return new DisposeableAction(() => values.Pop());
        }
    }
    public class CypherVisitor : ExpressionVisitor
    {
        public StringBuilder Query { get; } = new StringBuilder();
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
        private ContextValue<bool> isProperties = new ContextValue<bool>(false);
        private Dictionary<int, ContextValue<Expression>> expression = new Dictionary<int, ContextValue<Expression>>()
        {
            [0] = new ContextValue<Expression>(null),
            [1] = new ContextValue<Expression>(null),
            [2] = new ContextValue<Expression>(null)
        };

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Visit(node.Body);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    Query.Append("->");
                    break;
                case ExpressionType.LessThan:
                    Query.Append("<-");
                    break;
                case ExpressionType.Subtract:
                    Query.Append("-");
                    break;
                case ExpressionType.Equal:
                    Query.Append(" = ");
                    break;
            }
            Visit(node.Right);
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            using var _ = isProperties.Set(isProperties.Value || node.Method.ReturnType == typeof(IProperties));

            var attributes = node.Method.GetCustomAttributes(typeof(CypherAttribute), false);
            var format = attributes.Length > 0 ? (attributes[0] as CypherAttribute)?.Format : null;
            if (format != null)
            {
                IDisposable disp = null;
                for (int i = 0; i < format.Length; i++)
                {
                    switch (format[i])
                    {
                        case '$':
                            Visit(node.Arguments[int.Parse(format[++i].ToString())]);
                            break;
                        case '!':
                            Query.Append(node.Method.GetGenericArguments()[int.Parse(format[++i].ToString())].Name);
                            break;
                        case '+':
                            disp = expression[int.Parse(format[++i].ToString())].Set(node.Arguments[int.Parse(format[++i].ToString())]);
                            break;
                        case '.':
                            disp = expression[int.Parse(format[++i].ToString())].Set(node);
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
            else if (node.Method.Name == nameof(Range.EndAt))
            {
                Query.Append("*..");
                var index = node.Arguments[0] as UnaryExpression;
                Query.Append(index.Operand);
            }
            else if (node.Method.Name == "get_" + nameof(Range.All))
            {
                Query.Append("*");
            }
            else if (node.Method.Name == nameof(Pattern.All))
            {
                var properties = node.Method.GetGenericArguments()[0].GetProperties();
                foreach (var item in properties)
                {
                    Visit(node.Arguments[0]);
                    Query.Append(".");
                    Query.Append(item.Name);
                    if (item != properties.Last())
                        Query.Append(", ");
                }
            }
            else if (node.Method.Name == nameof(Pattern.Convention))
            {
                var filter = (node.Arguments[0] as Expression<Func<string, bool>>).Compile();
                var arguments = node.Method.IsGenericMethod
                    ? node.Method.GetGenericArguments()
                    : (expression[1].Value as MethodCallExpression).Method.GetGenericArguments();
                var properties = arguments[0].GetProperties().Where(p => filter(p.Name)).ToArray();
                foreach (var item in properties)
                {
                    Query.Append(item.Name);
                    Query.Append(": ");
                    if (expression[2].Value != null)
                    {
                        Visit(expression[2].Value);
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
            if (node.Type == typeof(Expression<Func<IProperties>>))
            {
                var expr = (node.Member as FieldInfo).GetValue((node.Expression as ConstantExpression).Value) as Expression;
                Visit(expr);
                return node;
            }
            if (node.Expression != null && !isProperties.Value)
            {
                Visit(node.Expression);
                Query.Append(".");
            }
            Query.Append(node.Member.Name);
            if (node.Member is PropertyInfo pi && pi.PropertyType == typeof(IProperty) || isProperties.Value)
            {
                Query.Append(": ");
                if (expression[0].Value != null)
                {
                    Query.Append("$");
                    Visit(expression[0].Value);
                }
                else if (expression[2].Value != null)
                {
                    Visit(expression[2].Value);
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
                Query.Append(index.Operand);
                Query.Append("..");
                index = node.Arguments[1] as UnaryExpression;
                Query.Append(index.Operand);

                return node;
            }

            using var _ = isProperties.Set(true);
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
            Query.Append(node.Name);
            return node;
        }
    }

    public class CypherCommand
    {
        public string Query { get; }
        public Dictionary<string, object> Parameters { get; }

        public CypherCommand(string query, Dictionary<string, object> parameters)
        {
            Query = query;
            Parameters = parameters;
        }

        public void Print() => Console.WriteLine(this);

        public override string ToString()
        {
            return $@"{Query}
---Parameters---
{string.Join(Environment.NewLine, Parameters)}";
        }
    }

    public class Relation
    {
        public Relation this[IVar var, IType type] { [Cypher("[$0:$1]")]get => throw new NotImplementedException(); }
        public Relation this[IVar var, IType type, IProperties properties] { [Cypher("[$0:$1 { $2 }]")]get => throw new NotImplementedException(); }
        public Relation this[Range r] { [Cypher("[$0]")]get => throw new NotImplementedException(); }
        public Relation this[IVar var, Range r] { [Cypher("[$0$1]")]get => throw new NotImplementedException(); }
        public Relation this[IVar var, IType type, IProperties properties, Range r] { [Cypher("[$0:$1 { $2 } $3]")]get => throw new NotImplementedException(); }
        public static Relation operator -(PD l, Relation r) => throw new NotImplementedException();
        public static Relation operator -(Relation l, PD r) => throw new NotImplementedException();
        public static Relation operator >(Relation l, Relation r) => throw new NotImplementedException();
        public static Relation operator <(Relation l, Relation r) => throw new NotImplementedException();
        public static PD operator >(Relation l, PD r) => throw new NotImplementedException();
        public static PD operator <(Relation l, PD r) => throw new NotImplementedException();
        public static PD operator >(PD l, Relation r) => throw new NotImplementedException();
        public static PD operator <(PD l, Relation r) => throw new NotImplementedException();
    }

    public interface INode { }
    public interface IVar { }
    public interface ILabel { }
    public interface IType { }
    public interface IProperty { }
    public interface IETagProperty { }
    public interface IProperties { }
    public interface IParameter { }
    public interface IInfinit { }

    [AttributeUsage(AttributeTargets.Method)]
    public class CypherAttribute : Attribute
    {
        public string Format { get; }

        public CypherAttribute(string format)
        {
            Format = format;
        }
    }

    public static class Pattern
    {
        public delegate PD PD(IVar var);

        public static CypherCommand P(Expression<PD> expr)
        {
            var visitor = new CypherVisitor();
            visitor.Visit(expr);
            return new CypherCommand(visitor.Query.ToString(), visitor.Parameters);
        }

        [Cypher("($0)")]
        public static PD N(IVar var) => throw new NotImplementedException();
        [Cypher("($0:$1)")]
        public static PD N(IVar var, ILabel label) => throw new NotImplementedException();
        [Cypher("($0:$1 { $2 })")]
        public static PD N(IVar var, ILabel label, IProperties properties) => throw new NotImplementedException();
        [Cypher("($0:!0)")]
        public static PD N<T>(IVar var) => throw new NotImplementedException();
        [Cypher("($0:!0 { $1 })")]
        public static PD N<T>(IVar var, Func<T, IProperties> properties) => throw new NotImplementedException();
        [Cypher("(.1$0:!0 { $1 })")]
        public static PD N<T>(IVar var, IProperties properties) => throw new NotImplementedException();
        [Cypher("($0:!0:$1)")]
        public static PD N<T>(IVar var, ILabel label) => throw new NotImplementedException();
        [Cypher("($0:!0:$1 { $2 })")]
        public static PD N<T>(IVar var, ILabel label, Func<T, IProperties> properties) => throw new NotImplementedException();

        public static Relation R => throw new NotImplementedException();

        [Cypher("$0")]
        public static IProperties P(params IProperty[] properties) => throw new NotImplementedException();
        [Cypher("$0")]
        public static IProperties P(params object[] properties) => throw new NotImplementedException();
        [Cypher("$0")]
        public static IProperties P<T>(Func<T, IProperties> properties) => throw new NotImplementedException();
        [Cypher("$0")]
        public static IProperties P(Expression<Func<IProperties>> properties) => throw new NotImplementedException();
        [Cypher("+00$1")]
        public static IProperties Pre(IVar var, IProperties properties) => throw new NotImplementedException();
        public static IProperties Convention(Func<string, bool> filter) => throw new NotImplementedException();
        public static IProperties Convention<T>(Func<string, bool> filter) => throw new NotImplementedException();
        [Cypher("$0")]
        public static T As<T>(this IVar var) => throw new NotImplementedException();
        public static object All<T>(this IVar var) => throw new NotImplementedException();

        [Cypher("PROFILE")]
        public static PD Profile() => throw new NotImplementedException();
        [Cypher("MATCH $0")]
        public static PD Match(PD p) => throw new NotImplementedException();
        [Cypher("CREATE $0")]
        public static PD Create(PD p) => throw new NotImplementedException();
        [Cypher("MERGE $0")]
        public static PD Merge(PD p) => throw new NotImplementedException();
        [Cypher("UNWIND \\$$0 as $1\r\n+21$2")]
        public static PD Unwind(IVar items, IVar item, PD p) => throw new NotImplementedException();
    }

    public static class PatternExtensions
    {
        [Cypher("$0\r\nMATCH $1")]
        public static PD Match(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nCREATE $1")]
        public static PD Create(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nMERGE $1")]
        public static PD Merge(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nOPTIONAL MATCH $1")]
        public static PD OptionalMatch(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nWHERE $1")]
        public static PD Where(this PD p, bool condition) => throw new NotImplementedException();
        [Cypher("$0\r\nRETURN $1")]
        public static PD Return(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nWITH $1")]
        public static PD With(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nORDER BY $1")]
        public static PD OrderBy(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nORDER BY $1 DESC")]
        public static PD OrderByDesc(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nSKIP $1")]
        public static PD Skip(this PD p, int count) => throw new NotImplementedException();
        [Cypher("$0\r\nLIMIT $1")]
        public static PD Limit(this PD p, int count) => throw new NotImplementedException();
        [Cypher("$0\r\nSET $1:$2")]
        public static PD Set(this PD p, IVar node, ILabel label) => throw new NotImplementedException();
    }

    static class Schema
    {
        public static ILabel Person => throw new NotImplementedException();
        public static IType KNOWS => throw new NotImplementedException();
        public static IProperty Id => throw new NotImplementedException();
        public static IProperty PropA => throw new NotImplementedException();
        public static IProperty PropB => throw new NotImplementedException();
        public static IETagProperty Concurrency => throw new NotImplementedException();
        public static IParameter map => throw new NotImplementedException();
        public static IInfinit Infinit => throw new NotImplementedException();
    }

    public class Foo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PropA { get; set; }
        public string PropB { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

}
