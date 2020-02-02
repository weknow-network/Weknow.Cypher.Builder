using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static test.Pattern;
using static test.Schema;

namespace test
{
    class DisposeableAction : IDisposable
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

    class ContextValue<T>
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
    class CypherVisitor : ExpressionVisitor
    {
        public StringBuilder Query { get; } = new StringBuilder();
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
        private ContextValue<bool> isProperties = new ContextValue<bool>(false);
        private Dictionary<int, ContextValue<Expression>> expression = new Dictionary<int, ContextValue<Expression>>()
        {
            [0] = new ContextValue<Expression>(null),
            [1] = new ContextValue<Expression>(null),
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
                            expression[int.Parse(format[++i].ToString())].Set(node.Arguments[int.Parse(format[++i].ToString())]);
                            break;
                        case '.':
                            expression[int.Parse(format[++i].ToString())].Set(node);
                            break;
                        default:
                            Query.Append(format[i]);
                            break;
                    }
                }
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
                var properties = (expression[1].Value as MethodCallExpression).Method.GetGenericArguments()[0].GetProperties().Where(p => filter(p.Name)).ToArray();
                foreach (var item in properties)
                {
                    Query.Append(item.Name);
                    Query.Append(": $");
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
            if (node.Expression != null && !isProperties.Value)
            {
                Visit(node.Expression);
                Query.Append(".");
            }
            Query.Append(node.Member.Name);
            if (node.Member is PropertyInfo pi && pi.PropertyType == typeof(IProperty) || isProperties.Value)
            {
                Query.Append(": $");
                if (expression[0].Value != null)
                    Visit(expression[0].Value);
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

    class CypherCommand
    {
        public string Query { get; }
        public Dictionary<string, object> Parameters { get; }

        public CypherCommand(string query, Dictionary<string, object> parameters)
        {
            Query = query;
            Parameters = parameters;
        }

        public void Print()
        {
            Console.WriteLine(Query);
            Console.WriteLine("---Parameters---");
            Console.WriteLine(string.Join(Environment.NewLine, Parameters));
        }
    }

    class Relation
    {
        public Relation this[IVar var, IType type] { [Cypher("[$0:$1]")]get => throw new NotImplementedException(); }
        public static Relation operator -(PD l, Relation r) => throw new NotImplementedException();
        public static Relation operator -(Relation l, PD r) => throw new NotImplementedException();
        public static Relation operator >(Relation l, Relation r) => throw new NotImplementedException();
        public static Relation operator <(Relation l, Relation r) => throw new NotImplementedException();
        public static PD operator >(Relation l, PD r) => throw new NotImplementedException();
        public static PD operator <(Relation l, PD r) => throw new NotImplementedException();
        public static PD operator >(PD l, Relation r) => throw new NotImplementedException();
        public static PD operator <(PD l, Relation r) => throw new NotImplementedException();
    }

    interface INode { }
    interface IVar { }
    interface ILabel { }
    interface IType { }
    interface IProperty { }
    interface IProperties { }

    class CypherAttribute : Attribute
    {
        public string Format { get; }

        public CypherAttribute(string format)
        {
            Format = format;
        }
    }

    static class Pattern
    {
        public delegate PD PD(IVar var);

        public static CypherCommand P(Expression<PD> expr)
        {
            var visitor = new CypherVisitor();
            visitor.Visit(expr);
            return new CypherCommand(visitor.Query.ToString(), visitor.Parameters);
        }

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
        [Cypher("+00$1")]
        public static IProperties Pre(IVar var, IProperties properties) => throw new NotImplementedException();
        public static IProperties Convention(Func<string, bool> filter) => throw new NotImplementedException();
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
    }

    static class PatternExtensions
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
    }

    static class Schema
    {
        public static ILabel Person => throw new NotImplementedException();
        public static IType KNOWS => throw new NotImplementedException();
        public static IProperty PropA => throw new NotImplementedException();
        public static IProperty PropB => throw new NotImplementedException();
    }

    class Foo
    {
        public string Name { get; set; }
        public string PropA { get; set; }
        public string PropB { get; set; }
    }

    class Bar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            P(a => r1 => b => r2 => c =>
                      Match(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
                      .Where(a.As<Foo>().Name == "Avi")
                      .Return(a.As<Foo>().Name, r1, b.All<Bar>(), r2, c)
                      .OrderBy(a.As<Foo>().Name)
                      .Skip(1)
                      .Limit(10)).Print();

            P(n => N(n, Person, P(PropA, PropB))).Print();
            P(n => N<Foo>(n, n => P(n.PropA, n.PropB))).Print();
            P(n => N<Foo>(n, Person, n => P(n.PropA, n.PropB))).Print();
            P(n => N(n, Person, P<Foo>(n => P(n.PropA, n.PropB)))).Print();
            P(n1 => n2 => n2_ => N(n1, Person, P(PropA, PropB)) - R[n1, KNOWS] > N(n2, Person, Pre(n2_, P(PropA, PropB)))).Print();
            P(n => N<Foo>(n, Convention(name => name.EndsWith("B")))).Print();
        }
    }
}