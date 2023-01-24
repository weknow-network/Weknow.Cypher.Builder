using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public abstract partial class CypherCardsConstraintsBaseTests : NonTxBaseIntegrationTests
{
    private ILabel Product => ILabel.Fake;
    private ILabel Academy => ILabel.Fake;
    private ILabel User => ILabel.Fake;
    private IType Graduate => IType.Fake;

    #region partial record PersonEntity

    [Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
    private partial record PersonEntity(string name, int age)
    {
        public int? key { get; init; }
        public string? desc { get; init; } = null;
        public int? version { get; init; } = 0;
        public DateTime? updatedOn { get; init; }
    }

    [Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
    private partial record AcademyEntity(string name, string id);

    [Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
    private partial record UserEntity(string firstName, string email);

    [Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
    private partial record GraduateEntity(int year, string degree);


    #endregion // partial record PersonEntity


    public CypherCardsConstraintsBaseTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper) :
                base(serviceProvider, outputHelper)
    {
    }

    /*
CREATE CONSTRAINT constraint_name
FOR (p:Person)
REQUIRE (p.name, p.surname) IS NODE KEY


CREATE CONSTRAINT constraint_name IF NOT EXISTS
FOR (p:Person)
REQUIRE p.name IS UNIQUE


CREATE FULLTEXT INDEX index_name
FOR (n:Friend) ON EACH [n.name]
OPTIONS {
  indexConfig: {
    `fulltext.analyzer`: 'english'
  }
}
     */

    #region CREATE CONSTRAINT / INDEX

    [Fact]
    public virtual async Task Constraint_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create<PersonEntity>();
        var n = Variables.Create<PersonEntity>();
        var (skipNumber, limitNumber) = Parameters.CreateMulti();

        try
        {
            CypherCommand cypher = _(() =>
                                    TryCreateConstraint(
                                         "TEST_CONSTRAINT1",
                                         ConstraintType.IsNotNull,
                                         N(n, Product), n._.age));

            IGraphDBResponse constraintResponse = await _graphDB.RunAsync(cypher);
            var info = await constraintResponse.GetInfoAsync();

            cypher = _(() => TryCreateTextIndex("TEST_INDEX", N(n, Product), n._.desc));
            constraintResponse = await _graphDB.RunAsync(cypher);
            info = await constraintResponse.GetInfoAsync();

            cypher = _(() => TryCreateConstraint(
                                         "TEST_CONSTRAINT",
                                         ConstraintType.IsNodeKey,
                                         N(n, Product), n._.key, n._.name));
            constraintResponse = await _graphDB.RunAsync(cypher);
            info = await constraintResponse.GetInfoAsync();

            CypherCommand testCypher = _(() =>
                                    Unwind(items, map =>
                                         Create(N(n, Product))
                                           .Set(n, map)));
            _outputHelper.WriteLine($"First Create: {testCypher}");

            CypherParameters prmsPrepare = testCypher.Parameters;
            prmsPrepare = prmsPrepare.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                    .Select(Factory));
            IGraphDBResponse response = await _graphDB.RunAsync(testCypher);
            info = await response.GetInfoAsync();
            await Assert.ThrowsAsync<Neo4j.Driver.ClientException>(async () =>
            {
                response = await _graphDB.RunAsync(testCypher);
                var i = await response.GetInfoAsync();
            });

        }
        finally
        {
            CypherCommand cypherDrop = _(() => TryDropConstraint("TEST_CONSTRAINT"));
            IGraphDBResponse dropResponse = await _graphDB.RunAsync(cypherDrop);
            var dropInfo = await dropResponse.GetInfoAsync();

            cypherDrop = _(() => TryDropConstraint("TEST_CONSTRAINT1"));
            dropResponse = await _graphDB.RunAsync(cypherDrop);
            dropInfo = await dropResponse.GetInfoAsync();

            cypherDrop = _(() => TryDropIndex("TEST_INDEX"));
            dropResponse = await _graphDB.RunAsync(cypherDrop);
            dropInfo = await dropResponse.GetInfoAsync();


            await Task.Delay(300); // no way to wait until delete completed
        }

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i) { key = i, desc = $"Something to say on {i}" };
    }

    #endregion // CREATE CONSTRAINT / INDEX

    #region CREATE CONSTRAINT / INDEX

    [Fact]
    public virtual async Task Constraint_Pattern_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;


        var a = Variables.Create<AcademyEntity>();
        var u = Variables.Create<UserEntity>();
        var g = Variables.Create<GraduateEntity>();

        var (skipNumber, limitNumber) = Parameters.CreateMulti();

        try
        {
            CypherCommand constraintCypher = _(() =>
                                    TryCreateConstraint(
                                         "TEST_CONSTRAINT",
                                         ConstraintType.IsNotNull,
                                         N() - R[g, Graduate] > N(), 
                                            g.__.year));

            IGraphDBResponse constraintResponse = await _graphDB.RunAsync(constraintCypher);
            var info = await constraintResponse.GetInfoAsync();

            CypherCommand cypher = _(() =>
                                         Merge(N(a, Academy, new { a.Prm.__.id }))
                                           .Set(a, a.Prm)
                                         .Merge(N(u, User, new { u.Prm.__.email }))
                                           .Set(u, u.Prm)
                                         .Create(N(a) < R[g, Graduate, new { g.Prm.__.degree, g.Prm.__.year }] - N(u))                                           
                                           );
            _outputHelper.WriteLine($"First Create: {cypher}");

            var tx = await _graphDB.StartTransaction();
            CypherParameters prms = cypher.Parameters 
                                                .AddOrUpdate(nameof(a), new AcademyEntity("University of California, Los Angeles", "UCLA").ToDictionary())
                                                .AddOrUpdate(nameof(u), new UserEntity("Mike", "mike@gmail.com").ToDictionary())
                                                .AddOrUpdate(nameof(g), new GraduateEntity(2000, "B.A"))
                                                ;
            IGraphDBResponse response = await tx.RunAsync(cypher, prms);
            info = await response.GetInfoAsync();

            var d = prms[nameof(g)] as Dictionary<string, object?>;
            d[nameof(GraduateEntity.year)] = null;
            bool hasError = false;
            try
            {
                response = await tx.RunAsync(cypher, prms);
                await tx.CommitAsync(); // will trigger the constraint
            }
            catch 
            {
                hasError = true;
            }
            if (!hasError)
            {
                await tx.RollbackAsync();
                throw new Exception("expecting constraint");
            }
        }
        finally
        {
            CypherCommand cypherDrop = _(() => TryDropConstraint("TEST_CONSTRAINT"));
            IGraphDBResponse dropResponse = await _graphDB.RunAsync(cypherDrop);
            var dropInfo = await dropResponse.GetInfoAsync();

            await Task.Delay(300); // no way to wait until delete completed
        }

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i) { key = i, desc = $"Something to say on {i}" };
    }

    #endregion // CREATE CONSTRAINT / INDEX
}
