using System.Data;
using System.Text.Json.Serialization;

using Generator.Equals;

using Weknow.CypherBuilder;
using Weknow.CypherBuilder.Declarations;
using Weknow.GraphDbClient.Abstraction;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;



public abstract class BasePathTests : TxBaseIntegrationTests
{
    #region Ctor

    public BasePathTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    protected override void Init(IGraphDB graphDB)
    {
        var n = Variables.Create<UnitEntity>();
        var indexCypher = _(() => TryCreateConstraint("test_path", ConstraintType.IsUnique, N(n, Unit), n._.Id));
        graphDB.RunAsync(indexCypher).GetAwaiter().GetResult();
        base.Init(graphDB);
    }

    private ILabel Unit => throw new NotImplementedException();

    private IType RelatedTo => throw new NotImplementedException();


    #region Create_Tree_Test

    [Fact]
    public virtual async Task Create_Tree_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var spine = Parameters.Create<UnitEntity>();
        var (n, p) = Variables.CreateMulti<UnitEntity>();
        
        IEnumerable<UnitEntity> GenerateEntities (int from = 1, int count = 1, int parentId = -1)
        {
            if(from >= 1_000_000)
                yield break;
            foreach (var i in Enumerable.Range(from, count))
            {
                var entity = new UnitEntity(i, $"Unit {i}",
                                            i == 0 ? UnitType.Target : UnitType.Ancestor,
                                            parentId);
                yield return entity;

                int local = i;
                int start = i * 10;
                foreach (var item in GenerateEntities(start, from switch { 
                            >= 100_000 => 3,     
                            _ => 2
                            }, local))
                {
                    yield return item;
                }
            }                                 
        }
        var EXPECTED = GenerateEntities().ToArray();

        CypherCommand cypher = _((m) =>
                                Unwind(spine, u => 
                                    Merge(N(m, Unit, new { u.__.Id }))
                                    .SetPlus(m, u)
                                    .Proc().If(u.__.ParentId != -1,
                                        Merge(N(p, Unit, new { Id = u.__.ParentId }))
                                        .Merge(N(m) < R[RelatedTo] - N(p))
                                    )
                                    .With(u)
                                    .Match(N(n, Unit))
                                    .Where(n.__.Id == u.__.Id)
                                    .Return(n)));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters
                                .AddRangeOrUpdate(nameof(spine), EXPECTED.Select(x => x));

        IGraphDBResponse response = await _runner.RunAsync(cypher, prms);
        UnitEntity[] entities = await response.GetRangeAsync<UnitEntity>().ToArrayAsync();

        Assert.Equal(EXPECTED.Length, entities.Length);
        Assert.True(EXPECTED.OrderBy(m => m.Id)
                   .SequenceEqual(
                        entities.OrderBy(m => m.Id)));
    }

    #endregion // Create_Tree_Test

    #region Range_Test

    [Fact]
    public virtual async Task Range_Test()
    {
        int[] EXPECTED = { 10_000, 1_000, 100, 10 };
        CypherConfig.Scope.Value = CONFIGURATION;
        var id = Parameters.Create<int>();
        var n = Variables.Create<UnitEntity>();
        var p = Variables.CreatePath<UnitEntity>();
        var longestPath = Variables.CreatePath();

        await Create_Tree_Test();


        var cypher = _(() => Match(p, N(n, Unit) < R[RelatedTo * 3] - N(Unit))
                             .Where(n.__.Id == 10_000)
                             .Unwind(Nodes(p), item => ReturnDistinct(item.__.Id)));
        
        _outputHelper.WriteLine("---------------------");
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        
        var prms = cypher.Parameters.AddIfEmpty(nameof(id), 10_000);
        IGraphDBResponse response = await _runner.RunAsync(cypher, prms);
        int[] entities = await response.GetRangeAsync<int>().ToArrayAsync();

        Assert.Equal(4, entities.Length);
        Assert.True(EXPECTED.SequenceEqual(entities));
    }

    #endregion // Range_Test

    #region Range_With_Params_Test

    [Fact]
    public virtual async Task Range_With_Params_Test()
    {
        int[] EXPECTED = { 10_000, 1_000, 100, 10 };
        CypherConfig.Scope.Value = CONFIGURATION;
        var id = Parameters.Create<int>();
        var n = Variables.Create<UnitEntity>();
        var p = Variables.CreatePath<UnitEntity>();
        var rng = Parameters.Create<int>();
        var longestPath = Variables.CreatePath();

        await Create_Tree_Test();


        var cypher = _(() => Match(p, N(n, Unit) < R[RelatedTo * rng] - N(Unit))
                             .Where(n.__.Id == 10_000)
                             .Unwind(Nodes(p), item => ReturnDistinct(item.__.Id)));
        
        _outputHelper.WriteLine("---------------------");
        _outputHelper.WriteLine($"CYPHER: {cypher.Embed()}");
        
        var prms = cypher.Parameters.AddIfEmpty(nameof(id), 10_000)
                         .AddIfEmpty(nameof(rng), 3, CypherParameterKind.Embed);

        IGraphDBResponse response = await _runner.RunAsync(cypher, prms);
        int[] entities = await response.GetRangeAsync<int>().ToArrayAsync();

        Assert.Equal(4, entities.Length);
        Assert.True(EXPECTED.SequenceEqual(entities));
    }

    #endregion // Range_With_Params_Test
}
