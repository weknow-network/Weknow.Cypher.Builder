using Neo4j.Driver;
using System;
using System.Threading.Tasks;
using Weknow.Cypher.Builder;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;
using static Weknow.Cypher.Builder.Cypher;

namespace Weknow.Tests
{
    public static class Neo4jExtensions
    {
        public static IDictionary<string, object> WithEntity<T>(this IDictionary<string, object> parameters, string key, T value)
        {
            parameters.Add(key, value.From());
            return parameters;
        }

        public static IDictionary<string, object> WithEntities<T>(this IDictionary<string, object> parameters, string key, T[] value)
        {
            parameters.Add(key, value.Select(v => v.From()));
            return parameters;
        }

        public static Task<TReturn> MapSingleAsync<TReturn>(this IResultCursor resultCursor)
        {
            return resultCursor.SingleAsync(r => r[0].As<IEntity>().To<TReturn>());
        }

        public static async Task<TReturn[]> MapAsync<TReturn>(this IResultCursor resultCursor)
        {
            var res = await resultCursor.ToListAsync(r => r[0].As<IEntity>().To<TReturn>());
            return res.ToArray();
        }

        public static T To<T>(this IEntity value)
        {
            var instance = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (value.Properties.ContainsKey(property.Name))
                    property.SetValue(instance, value[property.Name].ConvertValue(property.PropertyType));
            }
            return instance;
        }

        public static IDictionary<string, object> From<T>(this T value)
        {
            return typeof(T).GetProperties().ToDictionary(p => p.Name, p => p.GetValue(value));
        }

        public static object ConvertValue(this object value, Type type)
        {
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return (value as LocalDateTime).ToDateTime();
            }
            return Convert.ChangeType(value, type);
        }
    }

    public class HigherOrderEntityTests : TestBase
    {
        #region Ctor

        public HigherOrderEntityTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        #endregion // Ctor

        #region CreateNewEntity_Test

        [Fact]
        [Trait("Type", "Integration")]
        [Trait("Category", "Entity")]
        public async Task CreateNewEntity_Test()
        {
            CypherCommand cypher = Builder.Build(a =>
                             Create(N<Payload>(a, a.AsMap))
                             .Return(a));


            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };
            // TODO: encapsulate Parameter to enable WithEntity (it might not have to be dictionary)
            var parms = cypher.Parameters
                         .WithEntity<Payload>("a", payload);

            IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateNewEntity_Test

        #region CreateNewEntities_Test

        [Fact]
        [Trait("Type", "Integration")]
        [Trait("Category", "Entity")]
        public async Task CreateNewEntities_Test()
        {
            CypherCommand cypher = Builder.Build(a => items => item =>
                Unwind(items, item,
                    Create(N<Payload>(a))
                    .Set(a, item))
                .Return(a));


            var payloads = new[]
            {
                new Payload { Id = 2, Date = DateTime.Now, Name = "Test 1" },
                new Payload { Id = 3, Date = DateTime.Now.AddDays(1), Name = "Test 2" },
            };
            // TODO: encapsulate Parameter to enable WithEntity (it might not have to be dictionary)
            var parms = cypher.Parameters
                         .WithEntities<Payload>("items", payloads);

            IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload[] result = await cursor.MapAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payloads, result);
        }

        #endregion // CreateNewEntities_Test

        //#region CreateNew_OfT_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateNew")]
        //public async Task CreateNew_OfT_Test()
        //{
        //    var cypher = _builder
        //                            .Entity
        //                            .CreateNew<Payload>("n", "map");

        //    var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", payload);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(payload, result);
        //}

        //#endregion // CreateNew_OfT_Test

        //#region CreateNew_OfT_NoParam_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateNew")]
        //public async Task CreateNew_OfT_NoParam_Test()
        //{
        //    var cypher = _builder
        //                            .Entity
        //                            .CreateNew<Payload>("map");

        //    var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", payload);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(payload, result);
        //}

        //#endregion // CreateNew_OfT_NoParam_Test

        //#region CreateNew_Fail_OnDuplicate_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateNew")]
        //public async Task CreateNew_Fail_OnDuplicate_Test()
        //{
        //    await CreateNew_Test();

        //    await Assert.ThrowsAsync<ClientException>(() => CreateNew_Test());
        //    await Assert.ThrowsAsync<ClientException>(() => CreateNew_OfT_Test());
        //}

        //#endregion // CreateNew_Fail_OnDuplicate_Test

        //#region CreateIfNotExists_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateIfNotExists")]
        //public async Task CreateIfNotExists_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateIfNotExists("n", nameof(Payload), "map", nameof(Payload.Id));

        //    var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", payload);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(payload, result);
        //}

        //#endregion // CreateIfNotExists_Test

        //#region CreateIfNotExists_NoParam_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateIfNotExists")]
        //public async Task CreateIfNotExists_NoParam_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateIfNotExists("map",
        //                                                nameof(Payload).AsYield(),
        //                                                nameof(Payload.Id).AsYield());

        //    var payload = new Payload { Id = 1, Date = new DateTime(2019, 01, 01), Name = "Test 1" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", payload);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(payload, result);
        //}

        //#endregion // CreateIfNotExists_NoParam_Test

        //#region CreateIfNotExists_OfT_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateIfNotExists")]
        //public async Task CreateIfNotExists_OfT_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateIfNotExists<Payload>("n",
        //                                                        "map",
        //                                                        nameof(Payload.Id));

        //    var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", payload);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(payload, result);
        //}

        //#endregion // CreateIfNotExists_OfT_Test

        //#region CreateIfNotExists_OfT_NoParam_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateIfNotExists")]
        //public async Task CreateIfNotExists_OfT_NoParam_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateIfNotExists<Payload>(map => map.Id);

        //    var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", payload);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(payload, result);
        //}

        //#endregion // CreateIfNotExists_OfT_NoParam_Test

        //#region CreateOrUpdate_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateOrUpdate")]
        //public async Task CreateOrUpdate_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateOrUpdate("n",
        //                                            nameof(Payload),
        //                                            "map",
        //                                            nameof(Payload.Id));

        //    await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        //}

        //#endregion // CreateOrUpdate_Test

        //#region CreateOrUpdate_OfT_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateOrUpdate")]
        //public async Task CreateOrUpdate_OfT_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateOrUpdate<Payload>("n",
        //                                                    "map",
        //                                                    nameof(Payload.Id));

        //    await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        //}

        //#endregion // CreateOrUpdate_OfT_Test

        //#region CreateOrUpdate_OfT_Expression_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateOrUpdate")]
        //public async Task CreateOrUpdate_OfT_Expression_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateOrUpdate<Payload>(n => n.Id, "map");

        //    await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        //}

        //#endregion // CreateOrUpdate_OfT_Expression_Test

        //#region CreateOrUpdate_OfT_Expression_NoParam_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateOrUpdate")]
        //public async Task CreateOrUpdate_OfT_Expression_NoParam_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateOrUpdate<Payload>(map => map.Id);

        //    await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        //}

        //#endregion // CreateOrUpdate_OfT_Expression_NoParam_Test

        //#region ExecuteAndAssertCreateOrUpdateAsync

        //private async Task ExecuteAndAssertCreateOrUpdateAsync(FluentCypher cypher)
        //{
        //    // CREATE

        //    var item1 = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1", Description = "bla bla" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", item1);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result1 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(item1, result1);

        //    // UPDATE

        //    var item2 = new { Id = 1, Name = "Test 2" };

        //    parms = new Neo4jParameters()
        //                 .WithEntity("map", item2);

        //    cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result2 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    var expected2 = new Payload { Id = 1, Name = "Test 2", Date = item1.Date, Description = item1.Description };
        //    Assert.Equal(expected2, result2);
        //}

        //#endregion // ExecuteAndAssertCreateOrUpdateAsync

        //#region CreateOrReplace_OfT_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateOrReplace")]
        //public async Task CreateOrReplace_OfT_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateOrReplace<Payload>("n", "map", nameof(Payload.Id));

        //    await ExecuteAndAssertCreateOrReplaceAsync(cypher);
        //}

        //#endregion // CreateOrReplace_OfT_Test

        //#region CreateOrReplace_OfT_Expression_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateOrReplace")]
        //public async Task CreateOrReplace_OfT_Expression_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateOrReplace<Payload>(n => n.Id, "map");

        //    await ExecuteAndAssertCreateOrReplaceAsync(cypher);
        //}

        //#endregion // CreateOrReplace_OfT_Expression_Test

        //#region CreateOrReplace_OfT_Expression_NoParam_Test

        //[Fact]
        //[Trait("Category", "Entity")]
        //[Trait("Case", "CreateOrReplace")]
        //public async Task CreateOrReplace_OfT_Expression_NoParam_Test()
        //{
        //    var cypher = _builder
        //                        .Entity
        //                            .CreateOrReplace<Payload>(map => map.Id);

        //    await ExecuteAndAssertCreateOrReplaceAsync(cypher);
        //}

        //#endregion // CreateOrReplace_OfT_Expression_NoParam_Test

        //#region ExecuteAndAssertCreateOrReplaceAsync

        //private async Task ExecuteAndAssertCreateOrReplaceAsync(FluentCypher cypher)
        //{
        //    // CREATE

        //    var item1 = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1", Description = "bla bla" };

        //    var parms = new Neo4jParameters()
        //                 .WithEntity<Payload>("map", item1);

        //    IResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result1 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    Assert.Equal(item1, result1);

        //    // UPDATE

        //    var item2 = new { Id = 1, Name = "Test 2" };

        //    parms = new Neo4jParameters()
        //                 .WithEntity("map", item2);

        //    cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
        //    Payload result2 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

        //    var expected2 = new Payload { Id = 1, Name = "Test 2" };
        //    Assert.Equal(expected2, result2);
        //}

        //#endregion // ExecuteAndAssertCreateOrReplaceAsync
    }
}

