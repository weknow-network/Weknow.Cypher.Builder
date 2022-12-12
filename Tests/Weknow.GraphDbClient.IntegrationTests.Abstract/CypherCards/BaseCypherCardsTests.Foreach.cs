using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region FOREACH (item IN $items | CREATE (:PERSON:_TEST_ { Version: item }))

    [Fact]
    public virtual async Task Foreach_From_PRM_Test()
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

    #endregion // FOREACH (item IN $items | CREATE (:PERSON:_TEST_ { Version: item }))

    #region CREATE(user:PERSON:_TEST_ $map)

    [Fact]
    public virtual async Task Foreach_Condition_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        var users = Parameters.Create();
        var user = Variables.Create<PersonEntity>();
        CypherCommand cypher = _(map => d => n =>
                Unwind(users, map,
                        Create(N(user, Person))
                        .Set(user, map)
                        //.Foreach(d, Case().When(d.__<PersonEntity>().desc != null)
                        .Foreach(d, Case()
                            .When("d.desc IS NOT NULL")
                            .Then("[1]")
                            .Else("[]")
                        .End(),
                        Create(N(n, Desc, new { Text = user.__.desc }) < R[Desc.R] - N(user))
                        )
                    ));


        CypherParameters prms = cypher.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms = prms.AddRangeOrUpdate(nameof(users), usersPrm);
        var r = await _graphDB.RunAsync(cypher, prms);


        CypherCommand cypherGet = _(n => d =>
                        Match(N(Person) < R[Desc.R] - N(d, Desc))
                        .Return("d.Text"));


        _outputHelper.WriteLine($"CYPHER GET: {cypherGet}");

        var responseGet = await _graphDB.RunAsync(cypherGet);
        var result = await responseGet.GetAsync<string>("n.Version");
        Assert.Equal("Describe 0", result);


        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) 
        {
            key = i ,
            desc = i % 3 == 0 ? $"Describe {i}" : null
        };
    }

    #endregion // CREATE(user:PERSON:_TEST_ $map)
}
