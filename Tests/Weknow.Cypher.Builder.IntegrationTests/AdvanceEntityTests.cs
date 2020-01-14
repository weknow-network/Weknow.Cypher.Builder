using Neo4j.Driver.V1;
using Neo4jMapper;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using static Weknow.CypherFactory;

// TODO: check how to bypass empty unwind with Union (for mutate entities)

// https://neo4j.com/docs/cypher-refcard/current/

#pragma warning disable RCS1090 // Call 'ConfigureAwait(false)'.
#pragma warning disable ConfigureAwaitEnforcer // ConfigureAwaitEnforcer
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA2213 // Disposable fields should be disposed


namespace Weknow.CoreIntegrationTests
{
    /// <summary>
    /// Advance Entity
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class AdvanceEntityTests : IDisposable
    {
        private readonly ISession _session;
        private const string TEST_ENV_LABEL = "TEST_ENV";
        private const string LABEL = "N4J_TEST";
        private const string ID = "Id";
        private readonly FluentCypher _builder = CypherBuilder.Default
                                    .Context.Conventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE)
                                    .Context.Label.AddFromHere(TEST_ENV_LABEL);


        #region Ctor

        public AdvanceEntityTests()
        {
            string connectionString = Environment.GetEnvironmentVariable("TEST_N4J_URL") ?? "bolt://localhost";
            string userName = Environment.GetEnvironmentVariable("TEST_N4J_USER") ?? "neo4j";
            string password = Environment.GetEnvironmentVariable("TEST_N4J_PASS") ?? "123456";

            var driver = GraphDatabase.Driver(connectionString, AuthTokens.Basic(userName, password));
            _session = driver.Session(AccessMode.Write);

            try
            {
                _session.Run($"MATCH (n:{TEST_ENV_LABEL}) DETACH DELETE n");
                string cypher = I.CreateUniqueConstraint(LABEL, ID, convention: CypherNamingConvention.SCREAMING_CASE);
                _session.Run(cypher);
                cypher = I.CreateUniqueConstraint<Payload>(P => P.Id, CypherNamingConvention.SCREAMING_CASE);
                _session.Run(cypher);
            }
            catch (Exception)
            {
                Trace.TraceError($"Cannot connect to Neo4j [{connectionString}], make sure that it up and running");
                throw;
            }
        }

        #endregion // Ctor

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="TimeoutException">Neo4J close session timeout.</exception>
        public void Dispose()
        {
            if (!_session.CloseAsync().Wait(TimeSpan.FromMinutes(1)))
                throw new TimeoutException("Neo4J close session timeout.");
        }

        #endregion // Dispose

        #region CreateNew_Test

        [Fact]
        public async Task CreateNew_Test()
        {
            var cypher = _builder
                                    .Entity
                                    .CreateNew("n", "Payload", "map")
                                    .Return("n");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateNew_Test

        #region CreateNew_OfT_Test

        [Fact]
        public async Task CreateNew_OfT_Test()
        {
            var cypher = _builder
                                    .Entity
                                    .CreateNew<Payload>("n", "map")
                                    .Return("n");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateNew_OfT_Test

        #region CreateNew_OfT_NoParam_Test

        [Fact]
        public async Task CreateNew_OfT_NoParam_Test()
        {
            var cypher = _builder
                                    .Entity
                                    .CreateNew<Payload>("map")
                                    .Return("map");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateNew_OfT_NoParam_Test

        #region CreateNew_Fail_OnDuplicate_Test

        [Fact]
        public async Task CreateNew_Fail_OnDuplicate_Test()
        {
            await CreateNew_Test();

            await Assert.ThrowsAsync<ClientException>(() => CreateNew_Test());
            await Assert.ThrowsAsync<ClientException>(() => CreateNew_OfT_Test());
        }

        #endregion // CreateNew_Fail_OnDuplicate_Test

        #region CreateIfNotExists_Test

        [Fact]
        public async Task CreateIfNotExists_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateIfNotExists("n", nameof(Payload), "map", nameof(Payload.Id))
                                    .Return("n");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateIfNotExists_Test

        #region CreateIfNotExists_NoParam_Test

        [Fact]
        public async Task CreateIfNotExists_NoParam_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateIfNotExists("map", nameof(Payload).AsYield(), nameof(Payload.Id).AsYield())
                                    .Return("map");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateIfNotExists_NoParam_Test

        #region CreateIfNotExists_OfT_Test

        [Fact]
        public async Task CreateIfNotExists_OfT_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateIfNotExists<Payload>("n", "map", nameof(Payload.Id))
                                    .Return("n");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateIfNotExists_OfT_Test

        #region CreateIfNotExists_OfT_NoParam_Test

        [Fact]
        public async Task CreateIfNotExists_OfT_NoParam_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateIfNotExists<Payload>(map => map.Id)
                                    .Return("map");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateIfNotExists_OfT_NoParam_Test

        #region CreateOrUpdate_Test

        [Fact]
        public async Task CreateOrUpdate_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateOrUpdate("n", nameof(Payload), "map", nameof(Payload.Id))
                                    .Return("n");

            await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        }

        #endregion // CreateOrUpdate_Test

        #region CreateOrUpdate_OfT_Test

        [Fact]
        public async Task CreateOrUpdate_OfT_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateOrUpdate<Payload>("n", "map", nameof(Payload.Id))
                                    .Return("n");

            await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        }

        #endregion // CreateOrUpdate_OfT_Test

        #region CreateOrUpdate_OfT_Expression_Test

        [Fact]
        public async Task CreateOrUpdate_OfT_Expression_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateOrUpdate<Payload>(n => n.Id, "map")
                                    .Return("n");

            await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        }

        #endregion // CreateOrUpdate_OfT_Expression_Test

        #region CreateOrUpdate_OfT_Expression_NoParam_Test

        [Fact]
        public async Task CreateOrUpdate_OfT_Expression_NoParam_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateOrUpdate<Payload>(map => map.Id)
                                    .Return("map");

            await ExecuteAndAssertCreateOrUpdateAsync(cypher);
        }

        #endregion // CreateOrUpdate_OfT_Expression_NoParam_Test

        #region ExecuteAndAssertCreateOrUpdateAsync

        private async Task ExecuteAndAssertCreateOrUpdateAsync(FluentCypher cypher)
        {
            // CREATE

            var item1 = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1", Description = "bla bla" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>("map", item1);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result1 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(item1, result1);

            // UPDATE

            var item2 = new { Id = 1, Name = "Test 2" };

            parms = new Neo4jParameters()
                         .WithEntity($"map", item2);

            cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result2 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            var expected2 = new Payload { Id = 1, Name = "Test 2", Date = item1.Date, Description = item1.Description };
            Assert.Equal(expected2, result2);
        }

        #endregion // ExecuteAndAssertCreateOrUpdateAsync

        #region CreateOrReplace_OfT_Test

        [Fact]
        public async Task CreateOrReplace_OfT_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateOrReplace<Payload>("n", "map", nameof(Payload.Id))
                                    .Return("n");

            await ExecuteAndAssertCreateOrReplaceAsync(cypher);
        }

        #endregion // CreateOrReplace_OfT_Test

        #region CreateOrReplace_OfT_Expression_Test

        [Fact]
        public async Task CreateOrReplace_OfT_Expression_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateOrReplace<Payload>(n => n.Id, "map")
                                    .Return("n");

            await ExecuteAndAssertCreateOrReplaceAsync(cypher);
        }

        #endregion // CreateOrReplace_OfT_Expression_Test

        #region CreateOrReplace_OfT_Expression_NoParam_Test

        [Fact]
        public async Task CreateOrReplace_OfT_Expression_NoParam_Test()
        {
            var cypher = _builder
                                .Context.Label.AddFromHere(LABEL)
                                .Entity
                                    .CreateOrReplace<Payload>(map => map.Id)
                                    .Return("map");

            await ExecuteAndAssertCreateOrReplaceAsync(cypher);
        }

        #endregion // CreateOrReplace_OfT_Expression_NoParam_Test

        #region ExecuteAndAssertCreateOrReplaceAsync

        private async Task ExecuteAndAssertCreateOrReplaceAsync(FluentCypher cypher)
        {
            // CREATE

            var item1 = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1", Description = "bla bla" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>("map", item1);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result1 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(item1, result1);

            // UPDATE

            var item2 = new { Id = 1, Name = "Test 2" };

            parms = new Neo4jParameters()
                         .WithEntity($"map", item2);

            cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result2 = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            var expected2 = new Payload { Id = 1, Name = "Test 2" };
            Assert.Equal(expected2, result2);
        }

        #endregion // ExecuteAndAssertCreateOrReplaceAsync
    }
}