using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{

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
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();
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
                                    Unwind(items, map,
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
}
