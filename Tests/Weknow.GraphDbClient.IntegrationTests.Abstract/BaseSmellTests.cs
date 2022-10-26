using System.Data;
using System.Xml.Linq;

using Neo4j.Driver;
using Neo4j.Driver.Extensions;

using Weknow.GraphDbCommands;
using Weknow.GraphDbClient.Abstraction;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using Xunit.Sdk;
using Weknow.Mapping;
using Weknow.GraphDbCommands.Declarations;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable]
internal partial record NameDictionaryable(string Name);

internal record Name1(string Name);
internal record Name2
{
    public string Name { get; init; }
}

//    //[Trait("Group", "Predicates")]
//[Trait("Integration", "abstract")]
//[Trait("TestType", "Integration")]
public abstract class BaseSmellTests : BaseIntegrationTests
{
    #region Ctor

    public BaseSmellTests(
        IGraphDB graphDB,
        ITestOutputHelper outputHelper)
        : base(graphDB, outputHelper)
    {
    }

    #endregion // Ctor

    ILabel Person => throw new NotImplementedException();


    [Fact]
    public virtual async Task Create_Match_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));

        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        NameDictionaryable entity = await response.GetAsync<NameDictionaryable>();
        NameDictionaryable[] entities = await response.GetRangeAsync<NameDictionaryable>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    [Fact]
    public virtual async Task Create_Match_MapResult_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var p = Variables.Create();
        CypherCommand cypher = _(() =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));

        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        NameDictionaryable entity = await response.GetAsync<NameDictionaryable>();
        NameDictionaryable[] entities = await response.GetRangeAsync<NameDictionaryable>(nameof(p)).ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    [Fact]
    public virtual async Task Create_Match1_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));

        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        Name1 entity = await response.GetAsync<Name1>();
        Name1[] entities = await response.GetRangeAsync<Name1>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    [Fact]
    public virtual async Task Create_Match2_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));

        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        Name2 entity = await response.GetAsync<Name2>();
        Name2[] entities = await response.GetRangeAsync<Name2>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    [Fact]
    public virtual async Task Create_Match_Property_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var p = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p._.Name));
        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        string name = await response.GetAsync<string>(nameof(p), nameof(NameDictionaryable.Name));
        string[] names = await response.GetRangeAsync<string>(nameof(p), nameof(NameDictionaryable.Name)).ToArrayAsync();

        Assert.Equal(EXPECTED, name);
        Assert.Single(names);
        Assert.Equal(EXPECTED, names.Single());

    }

    [Fact]
    public virtual async Task Create_Match_Multi_StepByStep_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var (p1, p2) = Variables.CreateMulti<NameDictionaryable, NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Create(N(p1, Person, new { Name = pName }))
                                .Create(N(p2, Person, new { Name = pName }))
                                .Return(p1, p2));
        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var r1 = await response.GetAsync<NameDictionaryable>(nameof(p1));
        Assert.Equal(EXPECTED, r1.Name);

        var r2 = await response.GetAsync<NameDictionaryable>(nameof(p2));
        Assert.Equal(EXPECTED, r2.Name);
    }

    //[Fact]
    //public virtual async Task Create_Match_Multi_Test()
    //{
    //    const string EXPECTED = "Ben";
    //    CypherConfig.Scope.Value = CONFIGURATION;
    //    var pName = Parameters.Create();

    //    var (p1, p2) = Variables.CreateMulti<NameDictionaryable, NameDictionaryable>();
    //    CypherCommand cypher = _(() =>
    //                            Create(N(p1, Person, new { Name = pName }))
    //                            .Create(N(p2, Person, new { Name = pName }))
    //                            .Return(p1, p2));
    //    CypherParameters prms = cypher.Parameters;
    //    prms[nameof(pName)] = EXPECTED;

    //    IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
    //    var(r1, r2) = response.GetRangeAsync<NameDictionaryable, NameDictionaryable>(nameof(p1), nameof(p2));
    //    var e1 = await r1.ToArrayAsync();
    //    Assert.Equal(EXPECTED, e1.Single().Name);
    //    var e2 = await r1.ToArrayAsync();
    //    Assert.Equal(EXPECTED, e2.Single().Name);

    //}
}
