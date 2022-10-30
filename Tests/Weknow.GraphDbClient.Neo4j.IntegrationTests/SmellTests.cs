using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
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


    //[Fact]
    //public override Task Create_Match_Multi_Test()
    //{
    //    return base.Create_Match_Multi_Test();
    //}

    // TODO: test with multi result singular + collection (UNWIND, WITH)


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

}
