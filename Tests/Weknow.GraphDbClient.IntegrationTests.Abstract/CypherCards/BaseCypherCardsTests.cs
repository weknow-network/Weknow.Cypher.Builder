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

public abstract partial class BaseCypherCardsTests : BaseIntegrationTests
{
    #region Ctor

    public BaseCypherCardsTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    private ILabel Person => throw new NotImplementedException();
    private ILabel Friend => throw new NotImplementedException();

    private IType Knows => throw new NotImplementedException();
    private IType Follow => throw new NotImplementedException();

    #region partial record PersonEntity

    [Dictionaryable]
    private partial record PersonEntity(string name, int age)
    {
        public int? key { get; init; }
        public string? desc { get; init; } = null;
    }

    #endregion // partial record PersonEntity

    #region UNWIND $items AS map MERGE(n:PERSON { key: map.key }) SET n = map

    [Fact]
    public virtual async Task Unwind_Merge_Set_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Merge(N(n, Person, new { key = (~map)._.key /* result in map.key*/ }))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms.AddRange(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n));
        _outputHelper.WriteLine($"CYPHER: {query}");
        IGraphDBResponse response1 = await _graphDB.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 5);
        for (int i = 5; i < 10; i++)
        {
            var item = Factory(i);
            var res = r3[i - 5];
            Assert.Equal(item, res);
        }

        #endregion // Validation

        PersonEntity Factory(int i) => i < 5
            ? new PersonEntity($"Person {i}", i % 10 + 5) { key = i % 5 }
            : new PersonEntity($"Person {i}", i % 10 + 5) { key = i % 5, desc = $"desc {i}" }
            ;
    }

    #endregion // UNWIND $items AS map MERGE(n:PERSON { key: map.key }) SET n = map
}
