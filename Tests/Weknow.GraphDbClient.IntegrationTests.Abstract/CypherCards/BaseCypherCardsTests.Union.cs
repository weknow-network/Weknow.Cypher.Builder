using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region MATCH .. RETURN m.name UNION MATCH .. RETURN m.name

    [Fact]
    public virtual async Task Union_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        #region Prepare

        var users = Parameters.Create();
        var friends = Variables.Create();
        var userName = Parameters.Create<string>();
        var (user, friend, map) = Variables.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();

        CypherCommand cypherOfUsers = _(() =>
                                Unwind(users, map,
                                     Create(N(user, Person))
                                       .Set(user, map)));


        CypherParameters prms = cypherOfUsers.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms = prms.AddRangeOrUpdate(nameof(users), usersPrm);
        await _graphDB.RunAsync(cypherOfUsers, prms);
        foreach (var u in usersPrm.Skip(5))
        {
            var id = Parameters.Create();
            CypherCommand cypherOfFriends = _(() =>
                                    Match(N(user, Person))
                                    .Where(user._.key == id)
                                    .Create(N(friend, Friend))
                                       .Set(friend, friend.AsParameter)
                                    .With(user, friend)
                                    .Merge(N(user) < R[Knows] - N(friend)));

            prms = cypherOfFriends.Parameters;
            prms = prms.AddOrUpdate(nameof(friend), FriendFactory(u, (u.key ?? 0) + 40));
            prms = prms.AddOrUpdate(nameof(id), u.key ?? 0);
            await _graphDB.RunAsync(cypherOfFriends, prms);
        }
        foreach (var u in usersPrm.Skip(2).Take(6))
        {
            var id = Parameters.Create();
            CypherCommand cypherOfFriends = _(() =>
                                    Match(N(user, Person))
                                    .Where(user._.key == id)
                                    .Create(N(friend, Friend))
                                       .Set(friend, friend.Prm)
                                    .With(user, friend)
                                    .Merge(N(user) < R[Follow] - N(friend)));

            prms = cypherOfFriends.Parameters;
            prms = prms.AddOrUpdate(nameof(friend), FriendFactory(u, (u.key ?? 0) + 60));
            prms = prms.AddOrUpdate(nameof(id), u.key ?? 0);
            await _graphDB.RunAsync(cypherOfFriends, prms);
        }

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(friend, Friend) - R[Knows] > N(user, Person))
                                .Return(user._.name)
                                .Union()
                                .Match(N(friend, Friend) - R[Follow] > N(user, Person))
                                .Return(user._.name));
        _outputHelper.WriteLine($"CYPHER: {query}");
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var results = await response.GetRangeAsync<string>(nameof(user), nameof(user._.name)).ToArrayAsync();
        var ordered = results.OrderBy(m => m).ToArray();

        #region Validation

        Assert.True(results.Length == 8);
        for (int i = 0; i < 8; i++)
        {
            var item = UserFactory(i + 2).name;
            var res = ordered[i];
            Assert.Equal(item, res);
        }

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
        PersonEntity FriendFactory(PersonEntity user, int i) => new PersonEntity($"Friend {i} of {user.name}", i + 30) { key = i + 10 + (user.key * 100) };
    }

    #endregion // MATCH .. RETURN m.name UNION MATCH .. RETURN m.name 

    #region MATCH .. RETURN m.name UNION ALL MATCH .. RETURN m.name

    [Fact]
    public virtual async Task UnionAll_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        #region Prepare

        var users = Parameters.Create();
        var friends = Variables.Create();
        var userName = Parameters.Create<string>();
        var (user, friend, map) = Variables.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();

        CypherCommand cypherOfUsers = _(() =>
                                Unwind(users, map,
                                     Create(N(user, Person))
                                       .Set(user, map)));


        CypherParameters prms = cypherOfUsers.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms = prms.AddRangeOrUpdate(nameof(users), usersPrm);
        await _graphDB.RunAsync(cypherOfUsers, prms);
        foreach (var u in usersPrm.Skip(5))
        {
            var id = Parameters.Create();
            CypherCommand cypherOfFriends = _(() =>
                                    Match(N(user, Person))
                                    .Where(user._.key == id)
                                    .Create(N(friend, Friend))
                                       .Set(friend, friend.AsParameter)
                                    .With(user, friend)
                                    .Merge(N(user) < R[Knows] - N(friend)));

            prms = cypherOfFriends.Parameters;
            prms = prms.AddOrUpdate(nameof(friend), FriendFactory(u, (u.key ?? 0) + 40));
            prms = prms.AddOrUpdate(nameof(id), u.key ?? 0);
            await _graphDB.RunAsync(cypherOfFriends, prms);
        }
        foreach (var u in usersPrm.Skip(2).Take(6))
        {
            var id = Parameters.Create();
            CypherCommand cypherOfFriends = _(() =>
                                    Match(N(user, Person))
                                    .Where(user._.key == id)
                                    .Create(N(friend, Friend))
                                       .Set(friend, friend.AsParameter)
                                    .With(user, friend)
                                    .Merge(N(user) < R[Follow] - N(friend)));

            prms = cypherOfFriends.Parameters;
            prms = prms.AddOrUpdate(nameof(friend), FriendFactory(u, (u.key ?? 0) + 60));
            prms = prms.AddOrUpdate(nameof(id), u.key ?? 0);
            await _graphDB.RunAsync(cypherOfFriends, prms);
        }

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(friend, Friend) - R[Knows] > N(user, Person))
                                .Return(user._.name)
                                .UnionAll()
                                .Match(N(friend, Friend) - R[Follow] > N(user, Person))
                                .Return(user._.name));
        _outputHelper.WriteLine($"CYPHER: {query}");
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var results = await response.GetRangeAsync<string>(nameof(user), nameof(user._.name)).ToArrayAsync();

        #region Validation

        Assert.True(results.Length == 11);
        Assert.Single(results.Where(m => m == "User 2"));
        Assert.Equal(2, results.Where(m => m == "User 5").Count());

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
        PersonEntity FriendFactory(PersonEntity user, int i) => new PersonEntity($"Friend {i} of {user.name}", i + 30) { key = i + 10 + (user.key * 100) };
    }

    #endregion // MATCH .. RETURN m.name UNION ALL MATCH .. RETURN m.name 
}
