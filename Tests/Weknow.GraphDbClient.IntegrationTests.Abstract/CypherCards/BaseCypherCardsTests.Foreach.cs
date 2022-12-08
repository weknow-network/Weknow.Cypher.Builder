using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region CREATE(user:PERSON:_TEST_ $map)

    [Fact]
    public virtual async Task Foreach_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        var items = Parameters.Create();

        CypherCommand cypher = _(n => item =>
                                Foreach(item, items, Create(N(Person, new { Version = item }))));


        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters
                                      .AddOrUpdate(nameof(items), new[] {1,2,3});
        var response = await _graphDB.RunAsync(cypher, prms);

        CypherCommand cypherGet = _(n => item =>
                                Match(N(n, Person, new { Version = 1 }))
                                .Return("n.Version"));


        _outputHelper.WriteLine($"CYPHER GET: {cypherGet}");

        var responseGet = await _graphDB.RunAsync(cypherGet);
        var result = await responseGet.GetAsync<int>("n.Version");
        Assert.Equal(1, result);
    }

    #endregion // CREATE(user:PERSON:_TEST_ $map)
}
