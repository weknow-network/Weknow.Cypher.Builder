#pragma warning disable S2699 // Tests should include assertions

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Weknow.GraphDbClient.IntegrationTests.Abstract;

using Xunit;
using Xunit.Abstractions;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.Neo4jProvider.IntegrationTests;

[Trait("TestType", "Integration")]
[Trait("Integration", "neo4j")]
[Trait("Segment", "FullText")]
public class FullTextTests : BaseFullTextTests
{
    private const string ENV_VAR_PREFIX = "TEST_N4J_";

    #region Ctor

    public FullTextTests(
        ITestOutputHelper outputHelper)
        : base(RegisterGraphDB(), outputHelper)
    {
    }

    #endregion // Ctor

    #region RegisterGraphDB

    /// <summary>
    /// Registers the graph database.
    /// </summary>
    /// <returns></returns>
    private static IServiceProvider RegisterGraphDB()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging(configure => configure.AddConsole());
        services.RegisterSingletonNeo4j(envVarPrefix: ENV_VAR_PREFIX);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }

    #endregion // RegisterGraphDB

    [Fact]
    public override Task FullText1_Test()
    {
        return base.FullText1_Test();
    }

}
