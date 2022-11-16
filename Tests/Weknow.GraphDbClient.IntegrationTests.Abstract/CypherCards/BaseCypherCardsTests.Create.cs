using System.Data;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbCommands;

using Xunit;

using static Weknow.GraphDbCommands.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region CREATE(user:PERSON:_TEST_ $map)

    [Fact]
    public virtual async Task CreateMap_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        var expected = UserFactory(0);

        var userName = Parameters.Create<string>();
        var user = Variables.Create<PersonEntity>();
        var map = Parameters.Create<PersonEntity>();

        CypherCommand cypher = _(() =>
                                Create(N(user, Person, map)));


        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(map), expected);
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region Validation

        CypherCommand query = _(() =>
                                Match(N(user, Person))
                                .Return(user));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var result = await response.GetAsync<PersonEntity>(nameof(user));

        Assert.Equal(expected, result);

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
    }

    #endregion // CREATE(user:PERSON:_TEST_ $map)

    #region CREATE(user:PERSON:_TEST_) SET user = map

    [Fact]
    public virtual async Task CreateSet_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        var expected = UserFactory(0);

        var userName = Parameters.Create<string>();
        var user = Variables.Create<PersonEntity>();
        var map = Parameters.Create<PersonEntity>();

        CypherCommand cypher = _(() =>
                                Create(N(user, Person))
                                       .Set(user, map));


        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(map), expected);
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region Validation

        CypherCommand query = _(() =>
                                Match(N(user, Person))
                                .Return(user));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var result = await response.GetAsync<PersonEntity>(nameof(user));

        Assert.Equal(expected, result);

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
    }

    #endregion // UNWIND $users AS map CREATE(user:PERSON:_TEST_) SET user = map

    #region CREATE (n)-[r:KNOWS]->(m)

    [Fact]
    public virtual async Task CreateRelation_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        CypherCommand cypher = _(n => m =>
                                Create(N(n, Person) - R[Knows] > N(m, Friend)));


        CypherParameters prms = cypher.Parameters;
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region Validation

        CypherCommand query = _(n => m => r =>
                                Match(N(n, Person) - R[r, Knows] > N(m, Friend))
                                .Return(n.Labels(), r.type(), m.Labels()));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var n = await response.GetAsync<IList<string>>("labels(n)");
        var m = await response.GetAsync<IEnumerable<string>>("labels(m)");
        var r = await response.GetAsync<string>("type(r)");

        Assert.Equal(nameof(Knows), r);
        Assert.Equal(2, n.Count);
        Assert.Contains(nameof(_Test_).ToUpper(), n);
        Assert.Contains(nameof(Person).ToUpper(), n);
        Assert.Equal(2, m.Count());
        Assert.Contains(nameof(_Test_).ToUpper(), m);
        Assert.Contains(nameof(Friend).ToUpper(), m);

        #endregion // Validation
    }

    #endregion // CREATE (n)-[r:KNOWS]->(m)

    #region UNWIND $users AS map CREATE(user:PERSON:_TEST_) SET user = map

    [Fact]
    public virtual async Task UnwindCreateSet_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;


        var users = Parameters.Create();
        var friends = Variables.Create();
        var userName = Parameters.Create<string>();
        var (user, friend, map) = Variables.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();

        CypherCommand cypher = _(() =>
                                Unwind(users, map,
                                     Create(N(user, Person))
                                       .Set(user, map)));


        CypherParameters prms = cypher.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms.AddRangeOrUpdate(nameof(users), usersPrm);
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region Validation

        CypherCommand query = _(() =>
                                Match(N(user, Person))
                                .Return(user)
                                .OrderBy(user._.key));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var results = await response.GetRangeAsync<PersonEntity>(nameof(user)).ToArrayAsync();

        Assert.True(results.Length == 10);
        for (int i = 0; i < 10; i++)
        {
            var item = UserFactory(i);
            var res = results[i];
            Assert.Equal(item, res);
        }

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
    }

    #endregion // UNWIND $users AS map CREATE(user:PERSON:_TEST_) SET user = map
}
