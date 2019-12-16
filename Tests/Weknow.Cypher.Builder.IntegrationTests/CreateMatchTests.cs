using Neo4j.Driver.V1;
using System;
using System.Diagnostics;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.CoreIntegrationTests
{
    public class CreateMatchTests : IDisposable
    {
        private readonly ISession _session;
        private const string LABEL = "N4J_TEST";

        #region Ctor

        public CreateMatchTests()
        {
            string connectionString = Environment.GetEnvironmentVariable("TEST_N4J_URL") ?? "bolt://localhost";
            string userName = Environment.GetEnvironmentVariable("TEST_N4J_USER") ?? "neo4j";
            string password = Environment.GetEnvironmentVariable("TEST_N4J_PASS") ?? "123456";

            var driver = GraphDatabase.Driver(connectionString, AuthTokens.Basic(userName, password));
            _session = driver.Session(AccessMode.Write);

            try
            {
                _session.Run($"MATCH (n:{LABEL}) DETACH DELETE n");
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

        //#region MatchNodeTest

        //[Fact]
        //public async Task CreateMatch_ByDictionary_Test()
        //{
        //    var createQuery = N4jBuilder.Default.Create
        //        .AddNode<JustForTest1>("n", m =>
        //                m.WithAutoProps(m => m != nameof(JustForTest1.Id))
        //                .WithProperty("id")
        //                .WithLabels(LABEL));

        //    var parameters = new Dictionary<string, object>
        //    {
        //        ["id"] = 10,
        //        ["Name"] = "Bnaya",
        //        ["Date"] = DateTime.Now
        //    };
        //    IStatementResultCursor createCursor =
        //        await createQuery.Build(_session)
        //                    .RunAsync(parameters)
        //                    .ConfigureAwait(false);

        //    var matchQUery = N4jBuilder.Default.Match
        //        .AddNode("i", m => m.WithLabels(LABEL))
        //        .AsReturn();

        //    // TODO: IStatementResultCursor extensions for materialization [variable].As<>()

        //    IStatementResultCursor matchCursor =
        //        await matchQUery.Build(_session)
        //                    .RunAsync()
        //                    .ConfigureAwait(false);

        //    IList<JustForTest2> data = await matchCursor.ToListAsync(r =>
        //                            {
        //                                var item = r["i"].As<INode>();
        //                                return new JustForTest2
        //                                {
        //                                    Labels = item.Labels.ToArray(),
        //                                    ID = item.Properties["id"].As<int>(),
        //                                    Name = item.Properties["Name"].As<string>(),
        //                                    Date = item.Properties["Date"].As<DateTime>()
        //                                };
        //                           }).ConfigureAwait(false);

        //    JustForTest2 result = data.Single();
        //    Assert.Equal(LABEL, result.Labels[0]);
        //    Assert.Equal(10, result.ID);
        //    Assert.Equal("Bnaya", result.Name);
        //    Assert.True(result.Date <= DateTime.Now);
        //}

        //#endregion // MatchNodeTest

    }
}