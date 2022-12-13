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

    // TODO: no RawCypher & switch u with user

    #region FOREACH - When pattern

    [Fact]
    public virtual async Task Foreach_Condition_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        var users = Parameters.Create();
        var (user, u) = Variables.CreateMulti<PersonEntity>();
        CypherCommand cypher = _(map => n =>
                Unwind(users, map,
                        Create(N(user, Person))
                        .Set(user, map)
                        .Foreach(u, Case()
                            .When(user.__.desc != null)
                            .Then(new [] { user})
                            .Else(Array.Empty<PersonEntity>())
                        .End(),
                        Create(N(n, Desc, new { Text = u.__.desc }) < R[Desc.R] - N(u))
                        )
                    ));

        CypherParameters prms = cypher.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms = prms.AddRangeOrUpdate(nameof(users), usersPrm);
        var r = await _graphDB.RunAsync(cypher, prms);


        CypherCommand cypherGet = _(() =>
                        Match(N(user, Person))
                        .Where(user.__.key == 0)
                        .Return(user.__.desc));


        _outputHelper.WriteLine($"CYPHER GET: {cypherGet}");

        var responseGet = await _graphDB.RunAsync(cypherGet);
        // TODO: [bnaya 2022-12-13] GetAsync should work with expression
        //var result = await responseGet.GetAsync<string>(user.__.desc);
        var result = await responseGet.GetAsync<string>("user.desc");
        Assert.Equal("Describe 0", result);


        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) 
        {
            key = i ,
            desc = i % 3 == 0 ? $"Describe {i}" : null
        };
    }

    #endregion // 

    #region FOREACH .. RawCypher

    [Fact]
    public virtual async Task Foreach_Condition_RawCypher_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        var users = Parameters.Create();
        var (user, u) = Variables.CreateMulti<PersonEntity>();
        CypherCommand cypher = _(map => n =>
                Unwind(users, map,
                        Create(N(user, Person))
                        .Set(user, map)
                        .Foreach(u, Case()
                            .When(RawCypher("user.desc IS NOT NULL"))
                            .Then(RawCypher("[user]"))
                            .Else(RawCypher("[]"))
                        .End(),
                        Create(N(n, Desc, new { Text = u.__.desc }) < R[Desc.R] - N(u))
                        )
                    ));

        CypherParameters prms = cypher.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms = prms.AddRangeOrUpdate(nameof(users), usersPrm);
        var r = await _graphDB.RunAsync(cypher, prms);


        CypherCommand cypherGet = _(() =>
                        Match(N(user, Person))
                        .Where(user.__.key == 0)
                        .Return(user.__.desc));


        _outputHelper.WriteLine($"CYPHER GET: {cypherGet}");

        var responseGet = await _graphDB.RunAsync(cypherGet);
        var result = await responseGet.GetAsync<string>("user.desc");
        Assert.Equal("Describe 0", result);


        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) 
        {
            key = i ,
            desc = i % 3 == 0 ? $"Describe {i}" : null
        };
    }

    #endregion // FOREACH .. RawCypher
}