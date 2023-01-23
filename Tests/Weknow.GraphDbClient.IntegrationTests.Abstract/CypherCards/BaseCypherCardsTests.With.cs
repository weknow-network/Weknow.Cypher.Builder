using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region MATCH .. WITH user, count(friend) AS friends WHERE friends > 5   

    [Fact]
    public virtual async Task With_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var users = Parameters.Create<PersonEntity>();
        var friends = Variables.Create<PersonEntity>();
        var userName = Parameters.Create<string>();
        var (user, friend) = Variables.CreateMulti<PersonEntity>();

        #region Prepare

        CypherCommand cypherOfUsers = _(() =>
                                Unwind(users, map =>
                                     Merge(N(user, Person, new { key = map.__.key /* result in map.key*/ }))
                                       .Set(user, map)));


        CypherParameters prms = cypherOfUsers.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms = prms.AddRangeOrUpdate(nameof(users), usersPrm);
        await _tx.RunAsync(cypherOfUsers, prms);
        foreach (var u in usersPrm)
        {
            var id = Parameters.Create();
            CypherCommand cypherOfFriends = _(() =>
                                    Match(N(user, Person))
                                    .Where(user._.key == id)
                                    .With(user)
                                    .Unwind(friends.AsParameter, map =>
                                         Merge(N(friend, Friend, new { key = map.__.key }))
                                            .Set(friend, map)
                                         .Merge(N(user) - R[Knows] > N(friend))));

            prms = cypherOfFriends.Parameters;
            var friendsPrm = Enumerable.Range(0, (u.key ?? 0))
                                    .Select(m => FriendFactory(u, m))
                                    .ToArray();
            prms = prms.AddRangeOrUpdate(nameof(friends), friendsPrm);
            prms = prms.AddOrUpdate(nameof(id), u.key ?? 0);
            await _tx.RunAsync(cypherOfFriends, prms);
        }

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(user, Person) - R[Knows] > N(friend, Friend))
                                .With(user, friend.Count().As(friends))
                                .Where(friends > 5)
                                .Return(user)
                                .OrderBy(user.__.name));
        _outputHelper.WriteLine($"CYPHER: {query}");
        IGraphDBResponse response = await _tx.RunAsync(query, query.Parameters);
        var results = await response.GetRangeAsync<PersonEntity>(nameof(user)).ToArrayAsync();

        #region Validation

        Assert.True(results.Length == 4);
        for (int i = 0; i < 4; i++)
        {
            var item = UserFactory(i + 6);
            var res = results[i];
            Assert.Equal(item, res);
        }

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
        PersonEntity FriendFactory(PersonEntity user, int i) => new PersonEntity($"Friend {i} of {user.name}", i + 30) { key = i + 10 + (user.key * 100) };
    }

    #endregion // MATCH .. WITH user, count(friend) AS friends WHERE friends > 5  

    #region MATCH .. WITH user, count(friend) AS friends WHERE friends > 5 ORDER BY friends DESC   

    [Fact]
    public virtual async Task With_OrderBy_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var users = Parameters.Create<PersonEntity>();
        var friends = Variables.Create<PersonEntity>();
        var userName = Parameters.Create<string>();
        var (user, friend) = Variables.CreateMulti<PersonEntity>();

        #region Prepare

        CypherCommand cypherOfUsers = _(() =>
                                Unwind(users, map =>
                                     Merge(N(user, Person, new { map.__.key /* result in map.key*/ }))
                                       .Set(user, map)));


        CypherParameters prms = cypherOfUsers.Parameters;
        var usersPrm = Enumerable.Range(0, 10)
                                .Select(UserFactory)
                                .ToArray();
        prms = prms.AddRangeOrUpdate(nameof(users), usersPrm);
        await _tx.RunAsync(cypherOfUsers, prms);
        foreach (var u in usersPrm)
        {
            var id = Parameters.Create();
            CypherCommand cypherOfFriends = _(() =>
                                    Match(N(user, Person))
                                    .Where(user._.key == id)
                                    .With(user)
                                    .Unwind(friends.Prm, map =>
                                         Merge(N(friend, Friend, new { key = map.__.key }))
                                            .Set(friend, map)
                                         .Merge(N(user) - R[Knows] > N(friend))));

            prms = cypherOfFriends.Parameters;
            var friendsPrm = Enumerable.Range(0, (u.key ?? 0))
                                    .Select(m => FriendFactory(u, m))
                                    .ToArray();
            prms = prms.AddRangeOrUpdate(nameof(friends), friendsPrm);
            prms = prms.AddOrUpdate(nameof(id), u.key ?? 0);
            await _tx.RunAsync(cypherOfFriends, prms);
        }

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(user, Person) - R[Knows] > N(friend, Friend))
                                .With(user, friend.Count().As(friends))
                                .Where(friends > 5)
                                .Return(user)
                                .OrderByDesc(friends));
        _outputHelper.WriteLine($"CYPHER: {query}");
        IGraphDBResponse response = await _tx.RunAsync(query, query.Parameters);
        var results = await response.GetRangeAsync<PersonEntity>(nameof(user)).ToArrayAsync();

        #region Validation

        Assert.True(results.Length == 4);
        for (int i = 0; i < 4; i++)
        {
            var item = UserFactory(9 - i);
            var res = results[i];
            Assert.Equal(item, res);
        }

        #endregion // Validation

        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
        PersonEntity FriendFactory(PersonEntity user, int i) => new PersonEntity($"Friend {i} of {user.name}", i + 30) { key = i + 10 + (user.key * 100) };
    }

    #endregion // MATCH .. WITH user, count(friend) AS friends WHERE friends > 5 ORDER BY friends DESC
}
