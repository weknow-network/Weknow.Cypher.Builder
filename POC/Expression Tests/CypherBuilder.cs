using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Cypher;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Cypher Builder
    /// </summary>
    public class CypherBuilder
    {
        private readonly CypherConfig _config;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherBuilder"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private CypherBuilder(CypherConfig config)
        {
            _config = config;
        }

        #endregion // Ctor

        #region Config

        /// <summary>
        /// Set the cypher builder configuration.
        /// </summary>
        /// <param name="configuration">The configuration setter.</param>
        /// <returns></returns>
        public static CypherBuilder Config(
            Action<CypherConfig> configuration)
        { 
            var cfg = new CypherConfig();
            configuration(cfg);
            return new CypherBuilder(cfg);
        }

        /// <summary>
        /// Set the cypher builder configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherBuilder Config(
            CypherConfig configuration)
        { 
            return new CypherBuilder(configuration);
        }

        #endregion // Config

        #region Build

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public CypherCommand Build(Expression<PD> expression)
        {
            CypherCommand result = Init(_config, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public CypherCommand Build<T>(Expression<PDT<T, PD>> expression)
        {
            CypherCommand result = Init(_config, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public CypherCommand Build<T1, T2>(Expression<PDT<T1, PDT<T2, PD>>> expression)
        {
            CypherCommand result = Init(_config, expression);
            return result;
        }


        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public CypherCommand Build<T1, T2, T3>(Expression<PDT<T1, PDT<T2, PDT<T3, PD>>>> expression)
        {
            CypherCommand result = Init(_config, expression);
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
        /// <returns></returns>
        public CypherCommand Build<T1, T2, T3, T4>(Expression<PDT<T1, PDT<T2, PDT<T3, PDT<T4, PD>>>>> expression)
        {
            CypherCommand result = Init(_config, expression);
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
        /// <returns></returns>
        public CypherCommand Build<T1, T2, T3, T4, T5>(
                            Expression<PDT<T1, PDT<T2, PDT<T3, PDT<T4, PDT<T5, PD>>>>>> expression)
        {
            CypherCommand result = Init(_config, expression);
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
        /// <returns></returns>
        public CypherCommand Build<T1, T2, T3, T4, T5, T6>(
                            Expression<PDT<T1, PDT<T2, PDT<T3, PDT<T4, PDT<T5, PDT<T6, PD>>>>>>> expression)
        {
            CypherCommand result = Init(_config, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public CypherCommand Build(Expression<PDE> expression)
        {
            CypherCommand result = Init(_config, expression);
            return result;
        }

        #endregion // Build
    }
}
