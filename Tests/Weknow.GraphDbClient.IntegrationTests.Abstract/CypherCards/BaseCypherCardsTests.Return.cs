using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region UNWIND .. RETURN *

    [Fact]
    public virtual async Task Return_Star_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create<PersonEntity>();
        var n = Variables.Create<PersonEntity>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(n, Person))
                                       .Set(n, map))
                                .Return("*"));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();
        Assert.True(r3.Length == 10);
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(Factory(i), r3[i]);

        }

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i % 10 + 5);
    }

    #endregion // UNWIND .. RETURN *

    #region UNWIND .. RETURN n AS x

    [Fact]
    public virtual async Task Return_Alias_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create<PersonEntity>();
        var n = Variables.Create<PersonEntity>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(n, Person))
                                       .Set(n, map))
                                .Return(n.As("x")));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<PersonEntity>("x").ToArrayAsync();
        Assert.True(r3.Length == 10);
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(Factory(i), r3[i]);

        }

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i % 10 + 5);
    }

    #endregion // UNWIND .. RETURN n AS x

    #region RETURN DISTINCT n

    // see: https://neo4j.com/docs/cypher-manual/current/clauses/return/#return-unique-results
    [Fact]
    public virtual async Task Return_Distinct_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var n = Variables.Create<PersonEntity>();

        #region Prepare

        CypherCommand cypher = _(
            () => Create(N(Person, new { key = 1, name = "adam", age = 20, version = 1 }) - R[Knows] > N(n, Person, new { key = 2, name = "Ariana", age = 28 }))
            .Create(N(Person, new { key = 1, name = "Mor", age = 23, version = 1 }) - R[Knows] > N(n)));

        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        await _tx.RunAsync(cypher, prms);

        #endregion // Prepare

        var pattern = Reuse(() => N(Person) > N(n, Person));

        CypherCommand query1 = _(() =>
                        Match(pattern)
                        .ReturnDistinct(n));
        IGraphDBResponse response1 = await _tx.RunAsync(query1, prms);

        _outputHelper.WriteLine($"CYPHER: {query1}");

        var r1 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();
        Assert.True(r1.Length == 1);
        Assert.Equal("Ariana", r1[0].name);

        CypherCommand query2 = _(() =>
                        Match(pattern)
                        .Return(n));

        _outputHelper.WriteLine($"CYPHER: {query2}");
        IGraphDBResponse response2 = await _tx.RunAsync(query2, prms);
        var r2 = await response2.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();
        Assert.True(r2.Length == 2);
    }

    #endregion // RETURN DISTINCT n
}
