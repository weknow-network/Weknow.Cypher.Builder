using System.Collections.Generic;
using System;
using System.Data;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbCommands;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using System.Xml.Linq;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

partial class BaseCypherCardsTests
{
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
        prms.Add(nameof(p), expected);
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

    #region MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p

    [Fact]
    public virtual async Task Merge_ON_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        PersonEntity expected = UserFactory(10);

        var p = Parameters.Create<PersonEntity>();
        CypherCommand cypher = _(n =>
                                Merge(N(n, Person, new { key = 10 }))
                                .OnCreateSetPlus(n, p)
                                .OnMatchSet(AsCypher("n.version = coalesce(n.version, 0) + 1")));


        CypherParameters prms = cypher.Parameters;
        prms.Add(nameof(p), expected);
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

        Assert.Equal(expected with { version = expected.version + 1}, result);

        #endregion // Validation


        PersonEntity UserFactory(int i) => new PersonEntity($"User {i}", i + 30) { key = i };
    }

    #endregion // MERGE (n:_TEST_:PERSON { key: 10 }) SET n = $p
}
