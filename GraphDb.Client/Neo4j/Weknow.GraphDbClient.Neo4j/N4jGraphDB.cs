using Neo4j.Driver;

using Weknow.GraphDbCommands;
using Weknow.GraphDbClient.Abstraction;
using Neo4j.Driver.Extensions;
using System.Reflection;
using Weknow.Mapping;
using EnsureThat;

namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal class N4jGraphDB : IGraphDB
{
    private readonly IAsyncSession _session;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="N4jGraphDB" /> class.
    /// </summary>
    /// <param name="session">The session.</param>
    public N4jGraphDB(IAsyncSession session)
    {
        _session = session;
    }

    #endregion // Ctor

    #region RunAsync

    /// <summary>
    /// Executes Cypher
    /// </summary>
    /// <param name="cypherCommand">The cypher command.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// Response factory
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    async ValueTask<IGraphDBResponse> IGraphDB.RunAsync(CypherCommand cypherCommand, CypherParameters? parameters)
    {
        IResultCursor cursor = await _session.RunAsync(cypherCommand, parameters);
        return new GraphDBResponse(cursor);
    }

    #endregion // RunAsync

    #region class GraphDBResponse : IGraphDBResponse

    /// <summary>
    /// GraphDB response abstraction
    /// </summary>
    /// <seealso cref="Weknow.GraphDbClient.Abstraction.IGraphDBResponse" />
    private class GraphDBResponse : IGraphDBResponse
    {
        private static readonly Type IDictionaryableType = typeof(IDictionaryable);
        private readonly IResultCursor _cursor;
        //List<IRecord> results = await result.ToListAsync();
        //result.GetContent()
        //result.GetRecords();

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDBResponse"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public GraphDBResponse(IResultCursor result)
        {
            _cursor = result;
        }

        #endregion // Ctor

        async ValueTask<T> IGraphDBResponse.GetAsync<T>()
        {
            if (IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                await _cursor.FetchAsync();
                IRecord record = _cursor.Current;
                var node = (Neo4j.Driver.INode)record.Values.First().Value;
                if (node.TryAs(out T r)) return r;
                var props = (Dictionary<string, object?>)node.Properties;
                T result = (T)(props as dynamic);
                return result;
            }
            return await _cursor.SingleAsync(Mapper<T>);
        }

        async ValueTask<T> IGraphDBResponse.GetAsync<T>(string key, string? mappingKey)
        {
            var results = _cursor.GetContent<T>(key);
            await foreach (var item in results)
            {

            }
            //record.GetValueStrict
            throw new NotImplementedException();
        }

        async ValueTask<T[]> IGraphDBResponse.GetRangeAsync<T>()
        {
            var results = await _cursor.ToListAsync(Mapper<T>);
            return results.ToArray();
        }

        async ValueTask<T[]> IGraphDBResponse.GetRangeAsync<T>(string key, string? mappingKey)
        {
            throw new NotImplementedException();
        }

        async ValueTask<(T1[], T2[])> IGraphDBResponse.GetRangeAsync<T1, T2>(string key1, string key2)
        {
            throw new NotImplementedException();
        }

        async ValueTask<(T1[], T2[], T3[])> IGraphDBResponse.GetRangeAsync<T1, T2, T3>(string key1, string key2, string key3)
        {
            throw new NotImplementedException();
        }


        private static T Mapper<T>(IRecord record)
        {
            if (record.TryAs<T>(out T result))
            {
                return result;
            }
            if (IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                T dictionaryable = (T)record.Values;
                return dictionaryable;
            }

            throw new NotImplementedException();
            //return (T)record.Values.First().Value;
        }

        private static T Mapper<T>(IRecord record, string key)
        {
            if (IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                T result = (T)record.Values;
                return result;
            }

            //record.GetValueStrict
            return record.GetValue<T>(key);
        }
    }

    #endregion // class GraphDBResponse : IGraphDBResponse
}
