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
        public delegate R PDT<T, R>(IVar<T> var);
        public delegate PD PDE();

        public static IPattern Reuse(
                            Expression<Func<IVar, IPattern>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            return new ExpressionPattern(expression.Body, cfg);
        }


        #region Init

        /// <summary>
        /// Initializes the builder.
        /// </summary>
        /// <param name="cfg">The CFG.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        internal static CypherCommand Init(CypherConfig cfg, Expression expression)
        {
            var visitor = new CypherVisitor(cfg);
            visitor.Visit(expression);
            return new CypherCommand(
                            visitor.Query.ToString(), // TODO: format according to the configuration
                            visitor.Parameters);
        }

        #endregion // Init

        #region _

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _(
                            Expression<PD> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T>(
                            Expression<PDT<T, PD>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2>(
                            Expression<PDT<T1, PDT<T2, PD>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }


        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3>(
                            Expression<PDT<T1, PDT<T2, PDT<T3, PD>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }


        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3, T4>(
                            Expression<PDT<T1, PDT<T2, PDT<T3, PDT<T4, PD>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3, T4, T5>(
                            Expression<PDT<T1, PDT<T2, PDT<T3, PDT<T4, PDT<T5, PD>>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3, T4, T5, T6>(
                            Expression<PDT<T1, PDT<T2, PDT<T3, PDT<T4, PDT<T5, PDT<T6, PD>>>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _(
                            Expression<PDE> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        #endregion // _

        [Cypher("(:$0)")]
        public static IPattern N(ILabel label) => throw new NotImplementedException();
        [Cypher("($0)")]
        public static IPattern N(IVar var) => throw new NotImplementedException();
        [Cypher("($0$1)")]
        public static IPattern N(IVar var, ILabel label) => throw new NotImplementedException();
        [Cypher("($0$1 { $2 })")]
        public static IPattern N(IVar var, ILabel label, IProperties properties) => throw new NotImplementedException();
        [Cypher("($0$1 { $2 })")]
        public static IPattern N(IVar var, ILabel label, IMap properties) => throw new NotImplementedException();
        [Cypher("($0:!l0)")]
        public static IPattern N<T>(IVar var) => throw new NotImplementedException();
        [Cypher("($0:!l0 $1)")]
        public static IPattern N<T>(IVar var, IMap properties) => throw new NotImplementedException();
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar var, IProperties properties) => throw new NotImplementedException();
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar<T> var, IProperties properties) => throw new NotImplementedException();
        [Cypher("($0:!0$1)")]
        public static IPattern N<T>(IVar var, ILabel label) => throw new NotImplementedException();
        [Cypher("($0:!0$1 { $2 })")]
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
        /// <summary>
        /// All properties of the type.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IProperties All() => throw new NotImplementedException();
        /// <summary>
        /// All properties of the type except the ones specify in the parameters.
        /// </summary>
        /// <param name="except">The except.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IProperties AllExcept(params object[] except) => throw new NotImplementedException();
        public static IProperties All<T>(this IVar var) => throw new NotImplementedException();
        public static IProperties Convention<T>(this IVar var, Func<string, bool> filter) => throw new NotImplementedException();

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
        [Cypher("UNWIND \\$$0 AS $s0\r\n+s20$1")]
        public static PD Unwind(IVar items, PD p) => throw new NotImplementedException();
        [Cypher("EXISTS { $0 }")]
        public static bool Exists(PD p) => throw new NotImplementedException();

        public static IReuse<T, PD> Reuse<T>(this T v) => new Reuse<T, PD>(f => f(v));
        public static IReuse<T, Func<U, R>> Reuse<T, U, R>(this T r, IReuse<U, R> v) => new Reuse<T, Func<U, R>>(f => v.By(f(r)));
    }

    public interface IReuse<T, U>
    {
        PD By(Func<T, U> a);
    }

    class Reuse<T, U> : IReuse<T, U>
    {
        private Func<Func<T, U>, PD> _by;

        public Reuse(Func<Func<T, U>, PD> by)
        {
            _by = by;
        }

        PD IReuse<T, U>.By(Func<T, U> a) => _by(a);
    }
}
