using System.Data;

using Weknow.GraphDbClient.Abstraction;
using Weknow.CypherBuilder;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region RETURN count(*)

    [Fact]
    public virtual async Task Count_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();
        var (skipNumber, limitNumber) = Parameters.CreateMulti();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(n, Person))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");

        CypherParameters prmsPrepare = cypher.Parameters;
        prmsPrepare.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prmsPrepare);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n.Count()));
        _outputHelper.WriteLine($"CYPHER: {query}");
        CypherParameters prms = query.Parameters;
        prms = prms.AddOrUpdate(nameof(skipNumber), 2);
        prms = prms.AddOrUpdate(nameof(limitNumber), 6);
        IGraphDBResponse response1 = await _graphDB.RunAsync(query, prms);
        var r = await response1.GetAsync<int>("count(n)");

        Assert.Equal(10, r);

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i);
    }

    #endregion // RETURN count(*)
}
