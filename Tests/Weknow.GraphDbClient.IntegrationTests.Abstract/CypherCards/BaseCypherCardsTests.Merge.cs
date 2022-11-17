using Weknow.GraphDbClient.Abstraction;
using Weknow.CypherBuilder;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    private record Fellow(string name);

    #region MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p

    [Fact]
    public virtual async Task Merge_Set_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        PersonEntity expected = UserFactory(10);

        var p = Parameters.Create();
        CypherCommand cypher = _(n =>
                                Create(N(Person, new { key = 10 }))
                                .Merge(N(n, Person, new { key = 10 }))
                                .Set(n, p));


        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(p), expected);
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region Validation

        CypherCommand query = _((n) =>
                                Match(N(n, Person, new { key = 10 }))
                                .Return(n));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var result = await response.GetAsync<PersonEntity>("n");

        Assert.Equal(expected, result);

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
    }

    #endregion // MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p

    //#region MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p

    //[Fact]
    //public virtual async Task Merge_Set_Plus_Test()
    //{
    //    CypherConfig.Scope.Value = CONFIGURATION;

    //    PersonEntity expected = UserFactory(10);

    //    var p = Parameters.Create();
    //    CypherCommand cypher = _(n =>
    //                            Create(N(Person, new { key = 10, age = 11 }))
    //                            .Merge(N(n, Person, new { key = 10 }))
    //                            .Set(n, +p));


    //    CypherParameters prms = cypher.Parameters;
    //    prms.Add(nameof(p), new { name = "Bnaya"});
    //    await _graphDB.RunAsync(cypher, prms);
    //    _outputHelper.WriteLine($"CYPHER: {cypher}");

    //    #region Validation

    //    CypherCommand query = _((n) =>
    //                            Match(N(n, Person, new { key = 10 }))
    //                            .Return(n));
    //    IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
    //    var result = await response.GetAsync<PersonEntity>("n");

    //    Assert.Equal(expected, result);

    //    #endregion // Validation

    //    PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
    //}

    //#endregion // MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p

    #region MERGE (n:..) ON CREATE SET n = $p ON MATCH SET n.version = coalesce(n.version, 0) + 1
#pragma warning disable CS0618 // Type or member is obsolete

    [Fact]
    public virtual async Task Merge_ON_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        PersonEntity expected = UserFactory(10);

        var p = Parameters.Create<PersonEntity>();
        CypherCommand cypher = _(n =>
                                Merge(N(n, Person, new { key = 10 }))
                                .OnCreateSetPlus(n, p)
                                .OnMatchSet(FromRawCypher("n.version = coalesce(n.version, 0) + 1")));


        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(p), expected);
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region Validation

        CypherCommand query = _((n) =>
                                Match(N(n, Person, new { key = 10 }))
                                .Return(n));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var result = await response.GetAsync<PersonEntity>("n");

        Assert.Equal(expected, result);

        #endregion // Validation

        await _graphDB.RunAsync(cypher, prms);

        #region Validation

        response = await _graphDB.RunAsync(query, query.Parameters);
        result = await response.GetAsync<PersonEntity>("n");

        Assert.Equal(expected with { version = expected.version + 1 }, result);

        #endregion // Validation


        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
    }
#pragma warning restore CS0618 

    #endregion // MERGE (n:..) ON CREATE SET n = $p ON MATCH SET n.version = coalesce(n.version, 0) + 1

    #region MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p

    [Fact]
    public virtual async Task Merge_Relations_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        var (n, m) = Variables.CreateMulti<Fellow, Fellow>();

        CypherCommand cypher = _(() =>
                                Create(N(n, Person, new { name = "Lucy" }))
                                .Create(N(m, Person, new { name = "Pola" }))
                                .Merge(N(n) - R[Knows] > N(m)));


        CypherParameters prms = cypher.Parameters;
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region Validation

        CypherCommand query = _(() =>
                                Match(N(n, Person) - R[Knows] > N(m))
                                .Return(n._.name, m._.name));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var (a, b) = await response.GetAsync<(string a, string b)>(r =>
                        (
                            r.Get<string>(nameof(n), nameof(n._.name)),
                            r.Get<string>(nameof(m), nameof(m._.name))
                        ));

        Assert.Equal("Lucy", a);
        Assert.Equal("Pola", b);

        #endregion // Validation
    }

    #endregion // MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p
}
