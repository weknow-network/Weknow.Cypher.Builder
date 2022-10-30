using System.Data;
using System.Xml.Linq;

using FakeItEasy;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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
    private IServiceProvider _serviceProvider; // avoid GC collection

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
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    private static IServiceProvider RegisterGraphDB()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging(configure => configure.AddConsole());
        services.RegisterNeo4j(envVarPrefix: ENV_VAR_PREFIX);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
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

    [Fact(Skip = "not support yet")]
    public override Task Create_Match1_Test()
    {
        return base.Create_Match1_Test();
    }

    [Fact(Skip = "not support yet")]
    public override Task Create_Match2_Test()
    {
        return base.Create_Match2_Test();
    }

    [Fact]
    public override Task Create_Match_Property_Test()
    {
        return base.Create_Match_Property_Test();
    }

    [Fact]
    public override Task Create_Match_Multi_StepByStep_Test()
    {
        return base.Create_Match_Multi_StepByStep_Test();
    }

    [Fact]
    public override Task Create_Map_Match_Test()
    {
        return base.Create_Map_Match_Test();
    }

    [Fact]
    public override Task Create_Match_Multi_Unwind_Test()
    {
        return base.Create_Match_Multi_Unwind_Test();
    }


    //[Fact]
    //public override Task Create_Match_Multi_Test()
    //{
    //    return base.Create_Match_Multi_Test();
    //}

    // TODO: test with multi result singular + collection (UNWIND, WITH)
}
