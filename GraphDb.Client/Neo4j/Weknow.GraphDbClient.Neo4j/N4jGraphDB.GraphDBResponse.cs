using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Logging;

using Neo4j.Driver;

using Weknow.GraphDbClient.Abstraction;
using Weknow.Mapping;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// GraphDB response abstraction
/// </summary>
/// <seealso cref="Weknow.GraphDbClient.Abstraction.IGraphDBResponse" />
internal class GraphDBResponse : IGraphDBResponse
{
    private const int FLATTEN_LIMIT = 20;
    private static readonly Type IDictionaryableType = typeof(IDictionaryable);
    private readonly IResultCursor _cursor;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    private bool _completed;
    private ImmutableList<IRecord> _records = ImmutableList<IRecord>.Empty;
    private readonly AsyncLock _lock = new AsyncLock(Debugger.IsAttached ? TimeSpan.FromMinutes(5) : TimeSpan.FromSeconds(10));

    /// <summary>
    /// Creates a response abstraction.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    public static async ValueTask<IGraphDBResponse> Create(
        IResultCursor result,
        Microsoft.Extensions.Logging.ILogger logger)
    {
        var res = new GraphDBResponse(result, logger);
        res._completed = !await result.FetchAsync();
        return res;
    }

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDBResponse" /> class.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="logger">The logger.</param>
    private GraphDBResponse(
        IResultCursor result,
        Microsoft.Extensions.Logging.ILogger logger)
    {
        _cursor = result;
        _logger = logger;
    }

    #endregion // Ctor

    #region GetInfoAsync

    /// <summary>
    /// Gets the information asynchronous.
    /// </summary>
    /// <returns></returns>
    async ValueTask<IGraphExecutionSummary> IGraphDBResponse.GetInfoAsync()
    {
        IResultSummary summary = await _cursor.ConsumeAsync();
        IGraphExecutionSummary res = new GraphExecutionSummary(summary);
        return res;
    }

    #endregion // GetInfoAsync

    #region GetOrFetchAsync

    /// <summary>
    /// Get from cache or Fetch.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    private async ValueTask<IRecord?> GetOrFetchAsync(int index)
    {
        ImmutableList<IRecord> records = _records;
        if (records.Count > index)
            return records[index];
        if (!_completed)
        {
            using (await _lock.AcquireAsync())
            {
                if (!_completed)
                {
                    _records = _records.Add(_cursor.Current);
                    _completed = !await _cursor.FetchAsync();
                }
            }
        }
        if (_records.Count > index)
            return _records[index];
        return null;
    }

    #endregion // GetOrFetchAsync  

    #region GetAsync<T>()

    /// <summary>
    /// Gets the first result set as T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    /// the first result
    /// </returns>
    /// <exception cref="System.IndexOutOfRangeException"></exception>
    /// <example><![CDATA[
    /// MATCH (p:Person) RETURN p
    /// var person = results.Get<Person>();
    /// ]]></example>
    async ValueTask<T> IGraphDBResponse.GetAsync<T>()
    {
        Type t = typeof(T);
        if (IDictionaryableType.IsAssignableFrom(typeof(T)))
        {
            IRecord? record = await GetOrFetchAsync(0);
            if (record == null)
                throw new IndexOutOfRangeException("no results");

            var result = ConvertTo<T>(record.Values, _logger);

            return result;
        }

        return await _cursor.SingleAsync(Mapper<T>);
    }

    #endregion // GetAsync<T>()

    #region GetAsync<T>(string key, string? property)

    /// <summary>
    /// Cast result set as T by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <param name="property">The property.</param>
    /// <returns>
    /// Results by keys
    /// </returns>
    /// <exception cref="System.IndexOutOfRangeException"></exception>
    /// <example><![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p
    /// var person = results.Get<Person>("p");
    /// var company = results.Get<Company>("c");
    /// ]]></example>
    async ValueTask<T> IGraphDBResponse.GetAsync<T>(string key, string? property)
    {
        string fullKey = GetFullName(key, property);
        IRecord? record = await GetOrFetchAsync(0);
        if (record == null)
            throw new IndexOutOfRangeException();
        T result = ConvertTo<T>(record, fullKey, _logger);
        return result;
    }

    #endregion // GetAsync<T>(string key, string? property)

    #region GetRangeAsync<T>()

    /// <summary>
    /// Gets the first result set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    /// the first result
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person) RETURN p
    /// var person = results.Get<Person>();
    /// ]]></example>
    async IAsyncEnumerable<T> IGraphDBResponse.GetRangeAsync<T>()
    {
        int index = 0;
        while (true)
        {
            IRecord? record = await GetOrFetchAsync(index++);
            if (record == null)
                yield break;
            if (IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                var result = ConvertTo<T>(record.Values, _logger);
                yield return result;
            }
            else
            {
                var node = record.Values.First().Value;
                T result = ConvertTo<T>(node, _logger);
                yield return result;
            }
        }
    }

    #endregion // GetRangeAsync<T>()

    #region GetAsync<T>(Func<IGraphDBRecord, T> factory)

    /// <summary>
    /// Gets the first result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory">The factory.</param>
    /// <returns>
    /// the first result
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person) RETURN p
    /// var person = results.Get<Person>();
    /// ]]></example>
    async ValueTask<T> IGraphDBResponse.GetAsync<T>(Func<IGraphDBRecord, T> factory)
    {
        IGraphDBResponse self = this;
        var range = self.GetRangeAsync(factory);
        await foreach (var item in range)
        {
            return item;
        }
        throw new IndexOutOfRangeException();
    }

    #endregion // GetAsync<T>(Func<IGraphDBRecord, T> factory)

    #region GetRangeAsync<T>(Func<IGraphDBRecord, T> factory)

    /// <summary>
    /// Gets the first result set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory">The factory.</param>
    /// <returns>
    /// the first result
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person) RETURN p
    /// var person = results.Get<Person>();
    /// ]]></example>
    async IAsyncEnumerable<T> IGraphDBResponse.GetRangeAsync<T>(Func<IGraphDBRecord, T> factory)
    {
        int index = 0;
        while (true)
        {
            IRecord? record = await GetOrFetchAsync(index++);
            if (record == null)
                yield break;
            var mapper = new GraphDbRecord(record, _logger);
            T result = factory(mapper);
            yield return result;
        }
    }

    #endregion // GetRangeAsync<T>(Func<IGraphDBRecord, T> factory)

    #region GetRangeAsync<T>(string key, string? property)

    /// <summary>
    /// Gets result set by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <param name="property">The property.</param>
    /// <returns>
    /// Results by keys
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p, c
    /// var person = results.Get<Person>("p");
    /// var company = results.Get<Company>("c");
    /// ]]></example>
    async IAsyncEnumerable<T> IGraphDBResponse.GetRangeAsync<T>(string key, string? property)
    {

        int index = 0;
        while (true)
        {
            IRecord? record = await GetOrFetchAsync(index++);
            if (record == null)
                yield break;

            T result = ConvertTo<T>(record, key, property, _logger);
            yield return result;
        }
    }

    #endregion // GetRangeAsync<T>(string key, string? property)

    #region T Mapper<T>(IRecord record)

    /// <summary>
    /// Mappers a record.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record">The record.</param>
    /// <returns></returns>
    private static T Mapper<T>(IRecord record)
    {
        if (IDictionaryableType.IsAssignableFrom(typeof(T)))
        {
            T dictionaryable = (T)record.Values;
            return dictionaryable;
        }
        return record.As<T>();
    }

    #endregion // T Mapper<T>(IRecord record)

    #region T Mapper<T>(IRecord record, string key)

    /// <summary>
    /// Mappers a record segment (key).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record">The record.</param>
    /// <param name="key">The key.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    private static T Mapper<T>(
                        IRecord record,
                        string key,
                        Microsoft.Extensions.Logging.ILogger logger)
    {
        if (IDictionaryableType.IsAssignableFrom(typeof(T)))
        {
            T result = (T)record.Values;
            return result;
        }

        T res = ConvertTo<T>(record, key, logger);
        return res;
    }

    #endregion // T Mapper<T>(IRecord record, string key)

    #region GetFullName

    /// <summary>
    /// Gets the full name.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="property">The property.</param>
    /// <returns></returns>
    private static string GetFullName(string key, string? property)
    {
        string fullKey = key;
        if (property != null)
            fullKey = $"{key}.{property}";
        return fullKey;
    }

    #endregion // GetFullName

    #region T ConvertTo<T> (IRecord record, string key, string? property)

    /// <summary>
    /// Mappers by key and property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record">The record.</param>
    /// <param name="key">The key.</param>
    /// <param name="property">The property.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    private static T ConvertTo<T>(
                        IRecord record,
                        string key, 
                        string? property,
                        Microsoft.Extensions.Logging.ILogger logger)
    {
        string fullKey = GetFullName(key, property);
        T result = ConvertTo<T>(record, fullKey, logger);
        return result;
    }

    #endregion // T ConvertTo<T> (IRecord record, string key, string? property)

    #region T ConvertTo<T> (IRecord record, string key)

    /// <summary>
    /// Mappers by key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record">The record.</param>
    /// <param name="key">The key.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    private static T ConvertTo<T>(
                        IRecord record,
                        string key,
                        Microsoft.Extensions.Logging.ILogger logger)
    {
        object entity = record[key];
        T r = ConvertTo<T>(entity, logger);
        return r;
    }

    #endregion // T ConvertTo<T> (IRecord record, string key)

    #region T ConvertTo<T>(object entity)

    /// <summary>
    /// Converts to.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity">The entity.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    private static T ConvertTo<T>(
        object entity, 
        Microsoft.Extensions.Logging.ILogger logger)
    {
        T result;
        if (IDictionaryableType.IsAssignableFrom(typeof(T)))
        {
            if (entity is Neo4j.Driver.INode node)
            {
                var props = (Dictionary<string, object?>)node.Properties;
                result = (T)(props as dynamic);
                return result;
            }
            if (entity is Dictionary<string, object?> dic)
            {
                Dictionary<string, object?> Flatten()
                {
                    return new Dictionary<string, object?>(dic.Select(m => KeyValuePair.Create(m.Key.Substring(m.Key.IndexOf('.') + 1), m.Value)));
                }

                if (dic.Count < FLATTEN_LIMIT && dic.Keys.Any(m => m.IndexOf('.') != -1))
                {
                    dic = Flatten();
                }
                try
                {
                    result = (T)(dic as dynamic);
                }
                #region Exception Handling

                catch
                {
                    if (dic.Count >= FLATTEN_LIMIT)
                    {
                        logger.LogWarning("Converting result of [{type}] was flatten after a failure, it's recommended to flatten it at the query level by using `As` phrase", typeof(T).Name);
                        dic = Flatten();
                        result = (T)(dic as dynamic);
                    }
                    else throw;
                }

                #endregion // Exception Handling

                return result;
            }
            result = (T)entity;
            return result;
        }
        result = entity.As<T>();
        return result;

    }

    #endregion // T ConvertTo<T>(object entity)

    private bool TryCast<T>(Dictionary<string, object?> dic, out T result)
    {
        result = default;
        if (IDictionaryableType.IsAssignableFrom(typeof(T)))
        {
            result = (T)(dic as dynamic);
            return true;
        }
        return false;
    }

    #region class GraphDbRecord: IGraphDBRecord

    /// <summary>
    /// Handle a single record mapping
    /// </summary>
    /// <seealso cref="Weknow.GraphDbClient.Abstraction.IGraphDBRecord" />
    private class GraphDbRecord : IGraphDBRecord
    {
        private readonly IRecord _record;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public GraphDbRecord(
                        IRecord record,
                        Microsoft.Extensions.Logging.ILogger logger)
        {
            _record = record;
            _logger = logger;
        }

        T IGraphDBRecord.Get<T>(string key, string? property)
        {
            T result = ConvertTo<T>(_record, key, property, _logger);
            return result;
        }
    }

    #endregion // class GraphDbRecord: IGraphDBRecord

    #region class GraphExecutionSummary : IGraphExecutionSummary

    /// <summary>
    /// The result summary of running a query. The result summary interface can be used 
    /// to investigate details about the result, like the type of query run, how many
    /// and which kinds of updates have been executed, and query plan and profiling information
    ///  if available. The result summary is only available after all result records have
    ///  been consumed. Keeping the result summary around does not influence the lifecycle
    ///  of any associated session and/or transaction.
    /// </summary>
    private class GraphExecutionSummary : IGraphExecutionSummary
    {
        private readonly TimeSpan _resultAvailableAfter;
        private readonly string _query;
        private readonly IResultSummary _summary;

        public GraphExecutionSummary(IResultSummary summary)
        {
            _query = summary.Query.ToString();
            _resultAvailableAfter = summary.ResultAvailableAfter;
            _summary = summary;
        }

        string IGraphExecutionSummary.Query => _query;

        TimeSpan IGraphExecutionSummary.ResultAvailableAfter => _resultAvailableAfter;

        public override string ToString()
        {
            return _summary?.ToString() ?? _query;
        }
    }

    #endregion // class GraphExecutionSummary : IGraphExecutionSummary

}