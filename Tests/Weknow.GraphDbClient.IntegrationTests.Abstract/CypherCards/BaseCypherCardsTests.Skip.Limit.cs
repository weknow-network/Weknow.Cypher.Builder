using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region SKIP $skipNumber LIMIT $limitNumber

    [Fact]
    public virtual async Task SkipLimit_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create<PersonEntity>();
        var n = Variables.Create<PersonEntity>();
        var (skipNumber, limitNumber) = Parameters.CreateMulti();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(n, Person))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");

        CypherParameters prmsPrepare = cypher.Parameters;
        prmsPrepare = prmsPrepare.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _tx.RunAsync(cypher, prmsPrepare);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n)
                                .OrderBy(n._.age)
                                .Skip(skipNumber)
                                .Limit(limitNumber));
        _outputHelper.WriteLine($"CYPHER: {query}");
        CypherParameters prms = query.Parameters;
        prms = prms.AddOrUpdate(nameof(skipNumber), 2);
        prms = prms.AddOrUpdate(nameof(limitNumber), 6);
        IGraphDBResponse response1 = await _tx.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 6);
        for (int i = 0; i < 6; i++)
        {
            var res = r3[i];
            Assert.Equal(i + 2, res.age);
        }

        #endregion // Validation

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i);
    }

    #endregion // SKIP $skipNumber LIMIT $limitNumber

    #region SKIP $skipNumber

    [Fact]
    public virtual async Task Skip_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create<PersonEntity>();
        var n = Variables.Create<PersonEntity>();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(n, Person))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");

        CypherParameters prmsPrepare = cypher.Parameters;
        prmsPrepare = prmsPrepare.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _tx.RunAsync(cypher, prmsPrepare);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n)
                                .OrderBy(n._.age)
                                .Skip(2));
        _outputHelper.WriteLine($"CYPHER: {query}");
        CypherParameters prms = query.Parameters;
        IGraphDBResponse response1 = await _tx.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 8);
        for (int i = 0; i < 8; i++)
        {
            var res = r3[i];
            Assert.Equal(i + 2, res.age);
        }

        #endregion // Validation

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i);
    }

    #endregion // SKIP $skipNumber

    #region LIMIT 2

    [Fact]
    public virtual async Task Limit_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create<PersonEntity>();
        var n = Variables.Create<PersonEntity>();

        #region Prepare

        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(n, Person))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"CYPHER (prepare): {cypher}");

        CypherParameters prmsPrepare = cypher.Parameters;
        prmsPrepare = prmsPrepare.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _tx.RunAsync(cypher, prmsPrepare);

        #endregion // Prepare

        CypherCommand query = _(() =>
                                Match(N(n, Person))
                                .Return(n)
                                .Limit(2));
        _outputHelper.WriteLine($"CYPHER: {query}");
        CypherParameters prms = query.Parameters;
        IGraphDBResponse response1 = await _tx.RunAsync(query, prms);
        var r3 = await response1.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        #region Validation

        Assert.True(r3.Length == 2);

        #endregion // Validation

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i);
    }

    #endregion // LIMIT 2
}
