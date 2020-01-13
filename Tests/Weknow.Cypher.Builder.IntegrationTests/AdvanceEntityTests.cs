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
                                    .CreateNew<Payload>("n", "map")
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

        #endregion // CreateIfNotExists_Test
    }
}