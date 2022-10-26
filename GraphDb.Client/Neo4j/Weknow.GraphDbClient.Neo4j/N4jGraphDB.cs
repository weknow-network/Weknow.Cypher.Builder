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

        private static string GetFullName(string key, string? property)
        {
            string fullKey = key;
            if (property != null)
                fullKey = $"{key}.{property}";
            return fullKey;
        }

        #endregion // Ctor
        private T ConvertTo<T>(object entity)
        {
            T result;
            if (entity is Neo4j.Driver.INode node && IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                var props = (Dictionary<string, object?>)node.Properties;
                result = (T)(props as dynamic); // TODO: static interface factory
                return result;
            }
            if (entity.TryAs(out result))
                return result;
            //result = node.ToObject<T>();
            throw new InvalidCastException();
        }

        async ValueTask<T> IGraphDBResponse.GetAsync<T>()
        {
            if (IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                if (!await _cursor.FetchAsync())
                    throw new IndexOutOfRangeException();
                IRecord record = _cursor.Current;
                var node = (Neo4j.Driver.INode)record.Values.First().Value;
                T result = ConvertTo<T>(node);
                return result;
            }

            return await _cursor.SingleAsync(Mapper<T>);
        }

        async ValueTask<T> IGraphDBResponse.GetAsync<T>(string key, string? property)
        {
            string fullKey = GetFullName(key, property);
            if (!await _cursor.FetchAsync())
                throw new IndexOutOfRangeException();
            T result = _cursor.GetValue<T>(fullKey);
            return result;
        }

        async IAsyncEnumerable<T> IGraphDBResponse.GetRangeAsync<T>()
        {
            while (await _cursor.FetchAsync())
            {
                IRecord record = _cursor.Current;
                var node = (Neo4j.Driver.INode)record.Values.First().Value;
                T result = ConvertTo<T>(node);
                yield return result;
            }
            //var results = await _cursor.ToListAsync(Mapper<T>);
            //return results.ToArray();
        }

        async IAsyncEnumerable<T> IGraphDBResponse.GetRangeAsync<T>(string key, string? property)
        {
            string fullKey = GetFullName(key, property);
            while (await _cursor.FetchAsync())
            {
                IRecord record = _cursor.Current;
                object entity = record[fullKey];
                T result = ConvertTo<T>(entity);
                yield return result;
            }
        }

        (IAsyncEnumerable<T1>, IAsyncEnumerable<T2>) IGraphDBResponse.GetRangeAsync<T1, T2>(string key1, string key2)
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
