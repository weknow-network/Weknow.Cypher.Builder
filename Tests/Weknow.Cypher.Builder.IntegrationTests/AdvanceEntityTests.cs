using Neo4j.Driver.V1;
using Neo4jMapper;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using static Weknow.CypherFactory;

// TODO: check how to bypass empty unwind with Union (for mutate entities)

// https://neo4j.com/docs/cypher-refcard/current/

#pragma warning disable RCS1090 // Call 'ConfigureAwait(false)'.
#pragma warning disable ConfigureAwaitEnforcer // ConfigureAwaitEnforcer


namespace Weknow.CoreIntegrationTests
{
    public class AdvanceEntityTests : IDisposable
    {
        private readonly ISession _session;
        private const string LABEL = "N4J_TEST";
        private const string ID = "Id";

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
                _session.Run($"MATCH (n:{LABEL}) DETACH DELETE n");
                string cypher = I.CreateUniqueConstraint(LABEL, ID);
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
            var cypher = CypherBuilder.Default.Entity
                                    .CreateNew<Payload>("n", "map", LABEL)
                                    .Return("n");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"n_map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateNew_Test

        #region CreateNew_Fail_OnDuplicate_Test

        [Fact]
        public async Task CreateNew_Fail_OnDuplicate_Test()
        {
            await CreateNew_Test();

            await Assert.ThrowsAsync<ClientException>(() => CreateNew_Test());
        }

        #endregion // CreateNew_Fail_OnDuplicate_Test

        #region CreateIfNotExists_Test

        [Fact]
        public async Task CreateIfNotExists_Test()
        {
            var cypher = CypherBuilder.Default.Entity
                                    .CreateIfNotExists<Payload>("n", "map", nameof(Payload.Id), LABEL)
                                    .Return("n");

            var payload = new Payload { Id = 1, Date = DateTime.Now, Name = "Test 1" };

            var parms = new Neo4jParameters()
                         .WithEntity<Payload>($"n_map", payload);

            IStatementResultCursor cursor = await _session.RunAsync(cypher, parms).ConfigureAwait(false);
            Payload result = await cursor.MapSingleAsync<Payload>().ConfigureAwait(false);

            Assert.Equal(payload, result);
        }

        #endregion // CreateIfNotExists_Test
    }
}