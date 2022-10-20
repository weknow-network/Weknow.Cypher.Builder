using System;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;
using static System.Environment;

namespace Weknow.Cypher.Builder
{
        [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class MergeTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public MergeTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Create_SetProperties_Test

        [Fact]
        public void Merge_On_Create_SetProperties_Test()
        {
            var Id = Parameters.Create();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnCreateSet(n, new { n.AsParameter()._.PropA, n.AsParameter()._.PropB }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON CREATE SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Create_SetProperties_Test

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n = $map / Merge_On_Create_SetProperties_Update_Test

        [Fact]
        public void Merge_On_Create_SetAsMap_Update_Test()
        {
            var (Id, map) = Parameters.CreateMulti();
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnCreateSet(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON CREATE SET n = $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n = $map / Merge_On_Create_SetAsMap_Update_Test

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_Test

        [Fact]
        public void Merge_On_Match_SetProperties_Test()
        {
            var (Id, PropA, PropB) = Parameters.CreateMulti();
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnMatchSet(n, new { PropA, PropB }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON MATCH SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_Test

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map / Merge_On_Match_SetProperties_Update_Test

        [Fact]
        public void Merge_On_Match_SetProperties_Update_Test()
        {
            var (Id, map) = Parameters.CreateMulti();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnMatchSet(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON MATCH SET n = $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map / Merge_On_Match_SetProperties_Update_Test

        #region MATCH (n:Person { Id: $Id }) MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id }) / Merge_AfterMatch_Test

        [Fact]
        public void Merge_AfterMatch_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n => a =>
                                    Match(N(n,Person, new { Id }))
                                    .Merge(N(n)-R[KNOWS]>N(a, Animal, new { Id })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
                "MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id })", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id }) / Merge_AfterMatch_Test
    }
}

