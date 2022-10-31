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

    #region SKIP $skipNumber

    [Fact]
    public virtual async Task Sip_Test()
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

        CypherParameters prmsPrepare = cypher.Parameters;
        prmsPrepare.AddRange(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prmsPrepare);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n)
                                .OrderBy(n._.age)
                                .Skip(2));
        _outputHelper.WriteLine($"CYPHER: {query}");
        CypherParameters prms = query.Parameters;
        IGraphDBResponse response1 = await _graphDB.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 8);
        for (int i = 0; i < 8; i++)
        {
            var res = r3[i];
            Assert.Equal(i + 2, res.age);
        }

        #endregion // Validation

        PersonEntity Factory(int i) =>  new PersonEntity($"Person {i}", i) ;
    }

    #endregion // SKIP $skipNumber
}
