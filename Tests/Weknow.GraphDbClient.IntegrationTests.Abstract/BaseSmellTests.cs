using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable(Flavor = Flavor.Neo4j)]
internal partial record NameDictionaryable(string Name);

internal record Name1(string Name);
internal record Name2
{
    public string Name { get; init; } = string.Empty;
}

[Dictionaryable(Flavor = Flavor.Neo4j)]
internal partial record Someone(int Id, string Name, int Age);

//    //[Trait("Group", "Predicates")]
//[Trait("Integration", "abstract")]
//[Trait("TestType", "Integration")]
public abstract class BaseSmellTests : BaseIntegrationTests
{
    #region Ctor

    public BaseSmellTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    private ILabel Person => throw new NotImplementedException();

    private ILabel Manager => throw new NotImplementedException();

    private IType WorkFor => throw new NotImplementedException();


    #region CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    [Fact]
    public virtual async Task Create_Match_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        NameDictionaryable entity = await response.GetAsync<NameDictionaryable>();
        NameDictionaryable[] entities = await response.GetRangeAsync<NameDictionaryable>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    [Fact]
    public virtual async Task Create_Match_Var_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var v = Variables.Create();
        CypherCommand cypher = _(() =>
                                Create(N(v, Person, new { Name = pName }))
                                .Return(v));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        NameDictionaryable entity = await response.GetAsync<NameDictionaryable>();
        NameDictionaryable[] entities = await response.GetRangeAsync<NameDictionaryable>(nameof(v)).ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region CREATE (p:PERSON:_TEST_ { Name: $pName }) RETURN p.Name

    [Fact]
    public virtual async Task Create_Match_Property_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var v = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Create(N(v, Person, new { Name = pName }))
                                .Return(v._.Name));
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        string name = await response.GetAsync<string>(nameof(v), nameof(NameDictionaryable.Name));
        string[] names = await response.GetRangeAsync<string>(nameof(v), nameof(NameDictionaryable.Name)).ToArrayAsync();

        Assert.Equal(EXPECTED, name);
        Assert.Single(names);
        Assert.Equal(EXPECTED, names.Single());

    }

    #endregion // CREATE (p:PERSON:_TEST_ { Name: $pName }) RETURN p.Name

    #region Create_Map_Match_Test

    [Fact]
    public virtual async Task Create_Map_Match_Test()
    {
        var i = new Someone((int)(DateTime.Now.Ticks % 10L), "Name", (int)51L);
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var (p1, p2, p3, p4, p5) = Parameters.CreateMulti<Someone, Someone, Someone, Someone, Someone>();
        var someone1 = new Someone(1, "Deby", 51);
        var someone2 = new Someone(1, "Ruth", 28);
        var someone3 = new Someone(1, "Maygen", 30);
        var someone4 = new Someone(1, "Derk", 30);
        var someone5 = new Someone(1, "Maya", 30);

        var (c1, c2, c3, c4, c5, m1, m2) = Variables.CreateMulti();
        CypherCommand cypher = _(() =>
                                Create(N(c1, Manager & _Test_, p1))
                                .Create(N(c5, Manager & _Test_, p5))
                                .Create(N(c2, Person & _Test_, p2) - R[WorkFor] > N(c1))
                                .Create(N(c3, Person & _Test_, p3) - R[WorkFor] > N(c1))
                                .Create(N(c4, Person & _Test_, p4) - R[WorkFor] > N(c5))
                                .With()
                                .Match(N(m1, Person) - R[WorkFor] > N(m2, Manager))
                                .Return(m1, m2));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region CypherParameters prms = ...

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(p1), someone1.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p2), someone2.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p3), someone3.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p4), someone4.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p5), someone5.ToDictionary());

        #endregion // CypherParameters prms = ...

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var managers = await response.GetRangeAsync<Someone>(nameof(m1)).ToArrayAsync();
        var persons = await response.GetRangeAsync<Someone>(nameof(m2)).ToArrayAsync();
        Assert.True(managers.Length == 3);
        Assert.True(persons.Length == 3);
        //response.GetMapRangeAsync<Someone, Someone, string>(nameof(m1), nameof(m2), (m1, m2) => string.Empty);
    }

    #endregion // Create_Map_Match_Test
    #region CREATE (p1:PERSON:_TEST_ { Name: $pName }) CREATE(p2:PERSON:_TEST_ { Name: $pName }) RETURN p1, p2

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
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var r1 = await response.GetAsync<NameDictionaryable>(nameof(p1));
        Assert.Equal(EXPECTED, r1.Name);

        var r2 = await response.GetAsync<NameDictionaryable>(nameof(p2));
        Assert.Equal(EXPECTED, r2.Name);
    }

    #endregion // CREATE (p1:PERSON:_TEST_ { Name: $pName }) CREATE(p2:PERSON:_TEST_ { Name: $pName }) RETURN p1, p2
    #region UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    [Fact]
    public virtual async Task Create_Match_Multi_Unwind_Param_Add_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, map, x) = Variables.CreateMulti<Someone, Someone, Someone>();
        var p = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(x, Person))
                                       .Set(x, map))
                                .Return(x));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(m => new Someone(m, $"Number {n}", m % 10 + 5)));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<Someone>(nameof(x)).ToArrayAsync();
        Assert.NotEmpty(r3);
    }

    #endregion // UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    #region UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    [Fact]
    public virtual async Task Create_Match_Multi_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, map, x) = Variables.CreateMulti<Someone>();
        var p = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(x, Person))
                                       .Set(x, map))
                                .Return(x));
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(m => new Someone(m, $"Number {n}", m % 10 + 5).ToDictionary())
                                .ToArray());
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<Someone>(nameof(x)).ToArrayAsync();
        Assert.NotEmpty(r3);
    }

    #endregion // UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    [Fact(Skip = "Unwind BAD SYNTAX, use Create_Match_Multi_Unwind_Test")]
    public virtual async Task BAD_SYNTAX_Create_Match_Multi_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, map, x) = Variables.CreateMulti<Someone, Someone, Someone>();
        var p = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(x, Person, map.AsParameter)))
                                .Return(x));
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(m => new Someone(m, $"Number {m}", m % 10 + 5).ToDictionary())
                                .ToArray());
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<Someone>(nameof(x)).ToArrayAsync();
        Assert.Empty(r3);
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
    //_outputHelper.WriteLine($"CYPHER: {cypher}");
    //    CypherParameters prms = cypher.Parameters;
    //    prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

    //    IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
    //    var(r1, r2) = response.GetRangeAsync<NameDictionaryable, NameDictionaryable>(nameof(p1), nameof(p2));
    //    var e1 = await r1.ToArrayAsync();
    //    Assert.Equal(EXPECTED, e1.Single().Name);
    //    var e2 = await r1.ToArrayAsync();
    //    Assert.Equal(EXPECTED, e2.Single().Name);

    //}

    [Fact(Skip = "Not Supported Yet")]
    public virtual async Task Create_Match1_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        Name1 entity = await response.GetAsync<Name1>();
        Name1[] entities = await response.GetRangeAsync<Name1>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    [Fact(Skip = "Not Supported Yet")]
    public virtual async Task Create_Match2_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        Name2 entity = await response.GetAsync<Name2>();
        Name2[] entities = await response.GetRangeAsync<Name2>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }
}
