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

    public static class Cypher
    {
        public delegate PD PD(IVar var);

        public static CypherCommand _(
                            Expression<PD> expr, 
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            var visitor = new CypherVisitor(cfg);
            visitor.Visit(expr);
            return new CypherCommand(
                            visitor.Query.ToString(), // TODO: format according to the configuration
                            visitor.Parameters);
        }

        [Cypher("($0)")]
        public static IPattern N(IVar var) => throw new NotImplementedException();
        [Cypher("($0:$1)")]
        public static IPattern N(IVar var, ILabel label) => throw new NotImplementedException();
        [Cypher("($0:$1 { $2 })")]
        public static IPattern N(IVar var, ILabel label, IProperties properties) => throw new NotImplementedException();
        [Cypher("($0:$1 { \\$$2 })")]
        public static IPattern N(IVar var, ILabel label, IMap properties) => throw new NotImplementedException();
        [Cypher("($0:!0)")]
        public static IPattern N<T>(IVar var) => throw new NotImplementedException();
        [Cypher(".1($0:!0 { $1 })")]
        public static IPattern N<T>(IVar var, IProperties properties) => throw new NotImplementedException();
        [Cypher("($0:!0:$1)")]
        public static IPattern N<T>(IVar var, ILabel label) => throw new NotImplementedException();
        [Cypher("($0:!0:$1 { $2 })")]
        public static IPattern N<T>(IVar var, ILabel label, IProperties properties) => throw new NotImplementedException();

        public static IRelation R => throw new NotImplementedException();

        [Cypher("$0")]
        public static IProperties P(params IProperty[] properties) => throw new NotImplementedException();
        [Cypher("$0")]
        public static IProperties P(params object[] properties) => throw new NotImplementedException();
        [Cypher("$0")]
        public static IProperties P(Expression<Func<IProperties>> properties) => throw new NotImplementedException();
        [Cypher("+30$1")]
        public static IProperties P(this IVar var, params IProperty[] properties) => throw new NotImplementedException();
        [Cypher("+00$1")]
        public static IProperties Pre(IVar var, IProperties properties) => throw new NotImplementedException();
        [Cypher("+00$1")]
        public static IProperty Pre(IVar var, IProperty properties) => throw new NotImplementedException();
        public static IProperties Convention(Func<string, bool> filter) => throw new NotImplementedException();
        public static IProperties Convention<T>(Func<string, bool> filter) => throw new NotImplementedException();
        [Cypher("$0")]
        public static T As<T>(this IVar var) => throw new NotImplementedException();
        public static IProperties All<T>(this IVar var) => throw new NotImplementedException();
        public static IProperties Convention<T>(this IVar var, Func<string, bool> filter) => throw new NotImplementedException();
        public static IProperties All(params object[] p) => throw new NotImplementedException();

        [Cypher("PROFILE")]
        public static PD Profile() => throw new NotImplementedException();
        [Cypher("MATCH $0")]
        public static PD Match(IPattern p) => throw new NotImplementedException();
        [Cypher("CREATE $0")]
        public static PD Create(IPattern p) => throw new NotImplementedException();
        [Cypher("MERGE $0")]
        public static PD Merge(IPattern p) => throw new NotImplementedException();
        [Cypher("UNWIND \\$$0 AS $1\r\n+21$2")]
        public static PD Unwind(IVar items, IVar item, PD p) => throw new NotImplementedException();
        [Cypher("UNWIND \\$$0 AS $1\r\n+21$2")]
        public static PD Unwind(IVar items, PD p) => throw new NotImplementedException();
        [Cypher("EXISTS { $0 }")]
        public static bool Exists(PD p) => throw new NotImplementedException();

        public static Func<Func<T, PD>, PD> Reuse<T>(this IVar var, T v) => f => f(v);
    }

}
