using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Weknow
{
    /// <summary>
    /// Raw execution results facilitator.
    /// </summary>
    public interface ICypherRunnerResultCursor
    {
        /// <summary>
        /// Gets single result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ToSingleAsync<T>();
        /// <summary>
        /// Get multiple results (all result materialized not deferred ).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> ToManyAsync<T>();
        /// <summary>
        /// Exposed as async stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IAsyncEnumerable<T> ToAsyncStream<T>();
        /// <summary>
        /// Exposed as reactive stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObservable<CypherResult<T, object>> ToReactive<T>();
        /// <summary>
        /// Exposed as reactive stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObservable<CypherResult<T, TContext>> ToReactive<T, TContext>(TContext context);
        /// <summary>
        /// Exposed as reactive stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        ISourceBlock<CypherResult<T, object>> ToDataflow<T>(DataflowBlockOptions? options = null);
        /// <summary>
        /// Exposed as reactive stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        ISourceBlock<CypherResult<T, TContext>> ToDataflow<T, TContext>(TContext context, DataflowBlockOptions? options = null);
    }
}
