using System;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

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

        #region MERGE (n:Person { Id: $Id }) SET n += $n / Merge_SetAsMap_Update_Test

        [Fact]
        public void Merge_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .Set(+n.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n" +
                "SET n += $n", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) SET n += $n / Merge_SetAsMap_Update_Test

        #region MERGE (n:Person { Id: $Id }) SET n = $n / Merge_SetAsMap_Replace_Test

        [Fact]
        public void Merge_SetAsMap_Replace_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .Set(n.AsMap));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n" +
                "SET n = $n", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) SET n = $n / Merge_SetAsMap_Replace_Test

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Create_SetProperties_Test

        [Fact]
        public void Merge_On_Create_SetProperties_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.P(PropA, PropB)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON CREATE SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Create_SetProperties_Test

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n = $map / Merge_On_Create_SetProperties_Update_Test

        [Fact]
        public void Merge_On_Create_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n, map.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON CREATE SET n = $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n = $map / Merge_On_Create_SetAsMap_Update_Test

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_Test

        [Fact]
        public void Merge_On_Match_SetProperties_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnMatchSet(n.P(PropA, PropB)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON MATCH SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_Test

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_OfT_Test

        [Fact]
        public void Merge_On_Match_SetProperties_OfT_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnMatchSet(n.P<Foo>(x => new { x.PropA, x.PropB })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON MATCH SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_OfT_Test

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_OfT_Test

        [Fact]
        public void Merge_On_Match_SetProperties_OfT__Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, n.P<Foo>(x => x.Id)))
                                    .OnMatchSet(n.P<Foo>(x => new { x.PropA, x.PropB })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON MATCH SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_OfT_Test

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_OfT_Array_Test

        [Fact]
        public void Merge_On_Match_SetProperties_OfT_Array_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnMatchSet(n.P<Foo>(x => new []{ x.PropA, x.PropB })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON MATCH SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB / Merge_On_Match_SetProperties_OfT_Array_Test

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map / Merge_On_Match_SetProperties_Update_Test

        [Fact]
        public void Merge_On_Match_SetProperties_Update_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnMatchSet(n, map.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON MATCH SET n = $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map / Merge_On_Match_SetProperties_Update_Test

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.Date = $Date ON MATCH SET n += $n / Merge_On_SetAsMap_Update_Test

        [Fact]
        public void Merge_On_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.Convention<Bar>(m => m != nameof(Foo.Id)))
                                    .OnMatchSet(+n.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                    "ON CREATE SET n.Name = $Name, n.Date = $Date\r\n\t" +
                    "ON MATCH SET n += $n", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.Date = $Date ON MATCH SET n += $n / Merge_On_SetAsMap_Update_Test

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.Date = $Date ON MATCH SET n = $n / Merge_On_SetAsMap_Replace_Test

        [Fact]
        public void Merge_On_SetAsMap_Replace_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.Convention<Foo>(m => m != nameof(Foo.Id)))
                                    .OnMatchSet(n, n.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                    "ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB\r\n\t" +
                    "ON MATCH SET n = $n", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.Date = $Date ON MATCH SET n = $n / Merge_On_SetAsMap_Replace_Test

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB ON MATCH SET n += $n / Merge_On_SetNamedAsMapDefault_Update_Test

        [Fact]
        public void Merge_On_SetNamedAsMapDefault_Update_Test()
        {
            CypherCommand cypher = _(n => 
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.Convention<Foo>(m => m != nameof(Foo.Id)))
                                    .OnMatchSet(+n.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                    "ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB\r\n\t" +
                    "ON MATCH SET n += $n", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB ON MATCH SET n += $n / Merge_On_SetNamedAsMapDefault_Update_Test

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB ON MATCH SET n = $map / Merge_On_SetNamedAsMap_Replace_Test

        [Fact]
        public void Merge_On_SetNamedAsMap_Replace_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.Convention<Foo>(m => m != nameof(Foo.Id)))
                                    .OnMatchSet(n, map.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
            "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB\r\n\t" +
                "ON MATCH SET n = $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB ON MATCH SET n = $map / Merge_On_SetNamedAsMap_Replace_Test

        #region MATCH (n:Person { Id: $Id }) MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id }) / Merge_AfterMatch_Test

        [Fact]
        public void Merge_AfterMatch_Test()
        {
            CypherCommand cypher = _(n => a =>
                                    Match(N(n,Person, Id))
                                    .Merge(N(n)-R[KNOWS]>N(a, Animal, P(Id))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MATCH (n:Person { Id: $Id })\r\n" +
                "MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id })", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id }) / Merge_AfterMatch_Test
    }
}

