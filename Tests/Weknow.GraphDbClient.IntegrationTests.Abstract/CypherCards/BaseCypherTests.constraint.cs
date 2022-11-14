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

    #region 

    [Fact]
    public virtual async Task Constraint_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();
        var (skipNumber, limitNumber) = Parameters.CreateMulti();


        CypherCommand cypherDrop = _(() =>
                                TryDropIndex(
                                     "TEST_INDEX")
                                .TryDropConstraint(
                                     "TEST_CONSTRAINT")
                                .TryDropConstraint(
                                     "TEST_CONSTRAINT1"));

        try
        {

            IGraphDBResponse constraintDrop = await _graphDB.RunAsync(cypherDrop);

            CypherCommand cypher = _(() =>
                                    TryCreateConstraint(
                                         "TEST_CONSTRAINT",
                                         ConstraintType.IsNodeKey,
                                         N(n, Person), n._.key, n._.name ));

            IGraphDBResponse constraintResponse = await _graphDB.RunAsync(cypher);

            CypherCommand cypherAddition = _(() =>
                                    TryCreateConstraint(
                                         "TEST_CONSTRAINT1",
                                         ConstraintType.IsNotNull,
                                         N(n, Person), n._.age)
                                    .TryCreateTextIndex("TEST_INDEX", N(n, Person), n._.desc));

            constraintResponse = await _graphDB.RunAsync(cypherAddition);

            CypherCommand testCypher = _(() =>
                                    Unwind(items, map,
                                         Create(N(n, Person))
                                           .Set(n, map)));

            _outputHelper.WriteLine($"First Create: {testCypher}");

            CypherParameters prmsPrepare = testCypher.Parameters;
            prmsPrepare.AddRange(nameof(items), Enumerable.Range(0, 10)
                                    .Select(Factory));
            IGraphDBResponse response = await _graphDB.RunAsync(testCypher);
            Assert.ThrowsAsync<Exception>(async () =>
                await _graphDB.RunAsync(testCypher));


        }
        finally
        {
            await _graphDB.RunAsync(cypherDrop);
        }

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i);
    }

    #endregion // 
}
