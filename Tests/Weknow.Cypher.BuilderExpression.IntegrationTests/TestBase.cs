using Neo4j.Driver;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weknow.Cypher.Builder;
using Xunit;
using Xunit.Abstractions;
using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Tests
{
    public abstract class TestBase
    {
        protected readonly ITestOutputHelper _outputHelper;
        protected readonly IAsyncSession _session;
        protected const string TEST_ENV_LABEL = "TEST_ENV";

        #region Ctor

        public TestBase(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;

            string connectionString = Environment.GetEnvironmentVariable("TEST_N4J_URL") ?? "bolt://localhost";
            string userName = Environment.GetEnvironmentVariable("TEST_N4J_USER") ?? "neo4j";
            string password = Environment.GetEnvironmentVariable("TEST_N4J_PASS") ?? "123456";

            var driver = GraphDatabase.Driver(connectionString, AuthTokens.Basic(userName, password));
            _session = driver.AsyncSession(cfg => cfg.WithDefaultAccessMode(AccessMode.Write));
            var session = driver.AsyncSession(cfg => cfg.WithDefaultAccessMode(AccessMode.Write));

            try
            {
                session.RunAsync($"MATCH (n:{TEST_ENV_LABEL}) DETACH DELETE n").Wait();
                session.RunAsync(@"CREATE CONSTRAINT ON (p:Payload) ASSERT p.Id IS UNIQUE").Wait();
            }
            catch (Exception)
            {
                _outputHelper.WriteLine($"Cannot connect to Neo4j [{connectionString}], make sure that it up and running");
                throw;
            }
        }

        #endregion // Ctor
    }
}

