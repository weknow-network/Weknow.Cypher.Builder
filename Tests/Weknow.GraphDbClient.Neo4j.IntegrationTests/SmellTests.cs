using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Weknow.GraphDbClient.IntegrationTests.Abstract;

using Xunit;
using Xunit.Abstractions;

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
    private static IServiceProvider RegisterGraphDB()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging(configure => configure.AddConsole());
        services.RegisterScopedNeo4j(envVarPrefix: ENV_VAR_PREFIX);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }

    #endregion // RegisterGraphDB

    #region CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    [Fact]
    public override Task Create_Match_Test()
    {
        return base.Create_Match_Test();
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    [Fact]
    public override Task Create_Match_Var_Test()
    {
        return base.Create_Match_Var_Test();
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region  CREATE (p:PERSON:_TEST_ { Name: $pName }) RETURN p.Name

    [Fact]
    public override Task Create_Match_Property_Test()
    {
        return base.Create_Match_Property_Test();
    }

    #endregion //  CREATE (p:PERSON:_TEST_ { Name: $pName }) RETURN p.Name

    #region CREATE (p1:PERSON:_TEST_ { Name: $pName }) CREATE(p2:PERSON:_TEST_ { Name: $pName }) RETURN p1, p2

    [Fact]
    public override Task Create_Match_Multi_StepByStep_Test()
    {
        return base.Create_Match_Multi_StepByStep_Test();
    }

    #endregion // CREATE (p1:PERSON:_TEST_ { Name: $pName }) CREATE(p2:PERSON:_TEST_ { Name: $pName }) RETURN p1, p2
    #region Create_Map_Match_Test

    [Fact]
    public override Task Create_Map_Match_Test()
    {
        return base.Create_Map_Match_Test();
    }

    #endregion // Create_Map_Match_Test

    #region UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    [Fact]
    public override Task Create_Match_Multi_Unwind_Param_Add_Test()
    {
        return base.Create_Match_Multi_Unwind_Param_Add_Test();
    }

    #endregion // UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    #region UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    [Fact]
    public override Task Create_Match_Multi_Unwind_Test()
    {
        return base.Create_Match_Multi_Unwind_Test();
    }

    #endregion // UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x



}
