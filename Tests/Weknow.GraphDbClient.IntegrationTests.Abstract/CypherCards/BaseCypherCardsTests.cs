using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public abstract partial class BaseCypherCardsTests : BaseIntegrationTests
{
    #region Ctor

    public BaseCypherCardsTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    private ILabel Product => throw new NotImplementedException();
    private ILabel Person => throw new NotImplementedException();
    private ILabel Friend => throw new NotImplementedException();

    private IType Knows => throw new NotImplementedException();
    private IType Follow => throw new NotImplementedException();

    #region partial record PersonEntity

    [Dictionaryable(Flavor = Flavor.Neo4j)]
    private partial record PersonEntity(string name, int age)
    {
        public int? key { get; init; }
        public string? desc { get; init; } = null;
        public int? version { get; init; } = 0;
        public DateTime? updatedOn { get; init; }
    }

    #endregion // partial record PersonEntity

    #region UNWIND $items AS map MERGE(n:PERSON { key: map.key }) SET n = map

    [Fact]
    public virtual async Task Unwind_Merge_Set_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Merge(N(n, Person, new { key = (~map)._.key /* result in map.key*/ }))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");
        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n));
        _outputHelper.WriteLine($"CYPHER: {query}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        #endregion // Prepare

        IGraphDBResponse response1 = await _graphDB.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 5);
        for (int i = 5; i < 10; i++)
        {
            var item = Factory(i);
            var res = r3[i - 5];
            Assert.Equal(item, res);
        }

        #endregion // Validation

        PersonEntity Factory(int i) => i < 5
            ? new PersonEntity($"Person {i}", i % 10 + 5) { key = i % 5 }
            : new PersonEntity($"Person {i}", i % 10 + 5) { key = i % 5, desc = $"desc {i}" }
            ;
    }

    #endregion // UNWIND $items AS map MERGE(n:PERSON { key: map.key }) SET n = map

    // TODO:
    //  (n)-[*1..5]->(m) Variable length path of between 1 and 5 relationships from n to m
    //  (n)-[*]->(m)     Variable length path of any number of relationships from n to m. (See Performance section.)
    // shortestPath((n1:Person)-[*..6]-(n2:Person))         Find a single shortest path.
    // allShortestPaths((n1:Person)-[*..6]->(n2:Person))    Find all shortest paths.
    // size((n)-->()-->())    Count the paths matching the pattern.
    // NOT (n)-[:KNOWS]->(m)  Exclude matches to (n)-[:KNOWS]->(m) from the result.
    // n.property IN [$value1, $value2] Check if an element exists in a list.

    // String matching.
    // n.property STARTS WITH 'Tim' OR
    // n.property ENDS WITH 'n' OR
    // n.property CONTAINS 'goodie'

    // {name: 'Alice', age: 38, address: {city: 'London', residential: true}}
    // WITH {person: {name: 'Anne', age: 25}} AS p  RETURN p.person.name    Access the property of a nested map.

    // CASE https://neo4j.com/docs/cypher-manual/4.4/syntax/expressions/

}
