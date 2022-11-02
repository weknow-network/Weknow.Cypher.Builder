using Neo4j.Driver;
using Weknow.GraphDbClient.Abstraction;
using Neo4j.Driver.Extensions;
using Weknow.Mapping;
using System;

namespace Weknow.GraphDbClient.Neo4jProvider;

partial class N4jGraphDB
{
    /// <summary>
    /// GraphDB response abstraction
    /// </summary>
    /// <seealso cref="Weknow.GraphDbClient.Abstraction.IGraphDBResponse" />
    private class GraphDBResponse : IGraphDBResponse
    {
        private static readonly Type IDictionaryableType = typeof(IDictionaryable);
        private readonly IResultCursor _cursor;
        private bool _completed;
        private ImmutableList<IRecord> _records = ImmutableList<IRecord>.Empty;
        private readonly AsyncLock _lock = new AsyncLock(Debugger.IsAttached ? TimeSpan.FromMinutes(5) : TimeSpan.FromSeconds(10));

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
                        bool completed = !await _cursor.FetchAsync();
                        if (completed)
                            _completed = completed;
                        else
                            _records = _records.Add(_cursor.Current);
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
            if (IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                IRecord? record = await GetOrFetchAsync(0);
                if (record == null)
                    throw new IndexOutOfRangeException();
                var node = record.Values.First().Value;
                T result = ConvertTo<T>(node);
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
            T result;
            if (property == null)
            {
                result = ConvertTo<T>(record, key);
            }
            else
            {
                result = record.GetValue<T>(fullKey);
            }
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
                var node = record.Values.First().Value;
                T result = ConvertTo<T>(node);
                yield return result;
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
                var mapper = new GraphDbRecord(record);
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

                T result = result = ConvertTo<T>( record, key, property);
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

        #endregion // T Mapper<T>(IRecord record)

        #region T Mapper<T>(IRecord record, string key)

        /// <summary>
        /// Mappers a record segment (key).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        private static T ConvertTo<T>(IRecord record, string key, string? property)
        {
            T result;
            if (property == null)
            {
                result = ConvertTo<T>(record, key);
            }
            else
            {
                string fullKey = GetFullName(key, property);
                result = record.GetValue<T>(fullKey);
            }
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
        /// <returns></returns>
        private static T ConvertTo<T>(IRecord record, string key)
        {
            object entity = record[key];
            T r = ConvertTo<T>(entity);
            return r;
        }

        #endregion // T ConvertTo<T> (IRecord record, string key)

        #region T ConvertTo<T>(object entity)

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidCastException"></exception>
        private static T ConvertTo<T>(object entity)
        {
            T result;
            if (entity is Neo4j.Driver.INode node && IDictionaryableType.IsAssignableFrom(typeof(T)))
            {
                var props = (Dictionary<string, object?>)node.Properties;
                result = (T)(props as dynamic); // TODO: [bnaya 2022-11-01] static interface factory
                return result;
            }
            if (entity.TryAs(out result))
                return result;

            throw new InvalidCastException(typeof(T).Name);
        }

        #endregion // T ConvertTo<T>(object entity)

        #region class GraphDbRecord: IGraphDBRecord

        /// <summary>
        /// Handle a single record mapping
        /// </summary>
        /// <seealso cref="Weknow.GraphDbClient.Abstraction.IGraphDBRecord" />
        private class GraphDbRecord : IGraphDBRecord
        {
            private readonly IRecord _record;

            public GraphDbRecord(IRecord record)
            {
                _record = record;
            }

            T IGraphDBRecord.Get<T>(string key, string? property)
            {
                T result = result = ConvertTo<T>(_record, key, property);
                return result;
            }
        }

        #endregion // class GraphDbRecord: IGraphDBRecord
    }
}
