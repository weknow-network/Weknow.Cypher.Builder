using System.Collections.Generic;
using System;
using System.Data;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbCommands;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

partial class BaseCypherCardsTests 
{

    #region UNWIND .. ORDER BY n.age

    [Fact]
    public virtual async Task Unwind_Orderby_Age_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(n, Person))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms.AddRange(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n)
                                .OrderBy(n._.age));
        _outputHelper.WriteLine($"CYPHER: {query}");
        IGraphDBResponse response1 = await _graphDB.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 10);
        for (int i = 0; i < 10; i++)
        {
            var res = r3[i];
            Assert.Equal(i, res.age);
        }

        #endregion // Validation

        PersonEntity Factory(int i) => i < 5
            ? new PersonEntity($"Person {i}", i % 5 + 5) 
            : new PersonEntity($"Person {i}", i % 5) 
            ;
    }

    #endregion // UNWIND .. ORDER BY n.age

    #region UNWIND .. ORDER BY n.age

    [Fact]
    public virtual async Task Unwind_Orderby_Desc_Age_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(n, Person))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms.AddRange(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n)
                                .OrderByDesc(n._.age));
        _outputHelper.WriteLine($"CYPHER: {query}");
        IGraphDBResponse response1 = await _graphDB.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 10);
        for (int i = 0; i < 10; i++)
        {
            var res = r3[i];
            Assert.Equal(9 - i, res.age);
        }

        #endregion // Validation

        PersonEntity Factory(int i) => i < 5
            ? new PersonEntity($"Person {i}", i % 5 + 5)
            : new PersonEntity($"Person {i}", i % 5)
            ;
    }

    #endregion // UNWIND .. ORDER BY n.age

}
