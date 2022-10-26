using System.Data;
using System.Xml.Linq;

using Microsoft.Extensions.DependencyInjection;

using Neo4j.Driver;
using Neo4j.Driver.Extensions;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbClient.IntegrationTests.Abstract;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.Neo4jProvider.IntegrationTests;

[Trait("TestType", "Integration")]
[Trait("Integration", "neo4j")]
public class SmellTests : BaseSmellTests
{
    private const string ENV_VAR_PREFIX = "TEST_N4J_";

    #region Ctor

    public SmellTests(
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
    private static IGraphDB RegisterGraphDB()
    {
        var services = new ServiceCollection();
        services.RegisterNeo4j(envVarPrefix: ENV_VAR_PREFIX);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IGraphDB client = serviceProvider.GetRequiredService<IGraphDB>();
        return client;
    }

    #endregion // RegisterGraphDB

    [Fact]
    public override Task Create_Match_Test()
    {
        return base.Create_Match_Test();
    }

    [Fact]
    public override Task Create_Match_MapResult_Test()
    {
        return base.Create_Match_MapResult_Test();
    }

    [Fact]
    public override Task Create_Match1_Test()
    {
        return base.Create_Match1_Test();
    }

    [Fact]
    public override Task Create_Match2_Test()
    {
        return base.Create_Match2_Test();
    }

    [Fact]
    public override Task Create_Match_Property_Test()
    {
        return base.Create_Match_Property_Test();
    }
}
