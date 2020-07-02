using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Merge")]
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

        #region Merge_SetAsMap_Update_Test

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

        #endregion // Merge_SetAsMap_Update_Test

        #region Merge_SetAsMap_Replace_Test

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

        #endregion // Merge_SetAsMap_Replace_Test

        #region Merge_On_Create_SetProperties_Test

        [Fact]
        public void Merge_On_Create_SetProperties_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.P(PropA, PropB)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
"MERGE (n:Person { Id: $Id })\r\n\tON CREATE SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Merge_On_Create_SetProperties_Test

        #region Merge_On_Create_SetProperties_Update_Test

        [Fact]
        public void Merge_On_Create_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n, map.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
"MERGE (n:Person { Id: $Id })\r\n\tON CREATE SET n = $map", cypher.Query);
        }

        #endregion // Merge_On_Create_SetAsMap_Update_Test

        #region Merge_On_Match_SetProperties_Test

        [Fact]
        public void Merge_On_Match_SetProperties_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnMatchSet(n.P(PropA, PropB)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
"MERGE (n:Person { Id: $Id })\r\n\tON MATCH SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Merge_On_Match_SetProperties_Test

        #region Merge_On_Match_SetProperties_Update_Test

        [Fact]
        public void Merge_On_Match_SetProperties_Update_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnMatchSet(n, map.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
"MERGE (n:Person { Id: $Id })\r\n\tON MATCH SET n = $map", cypher.Query);
        }

        #endregion // Merge_On_Match_SetProperties_Update_Test

        #region Merge_On_SetAsMap_Update_Test

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

        #endregion // Merge_On_SetAsMap_Update_Test

        #region Merge_On_SetAsMap_Replace_Test

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
                    "ON CREATE SET n.Name = $Name, n.DateOfBirth = DateOfBirth\r\n\t" +
                    "ON MATCH SET n = $n", cypher.Query);
        }

        #endregion // Merge_On_SetAsMap_Replace_Test

        #region Merge_On_SetNamedAsMap_Update_Test

        [Fact]
        public void Merge_On_SetNamedAsMap_Update_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.Convention<Foo>(m => m != nameof(Foo.Id)))
                                    .OnMatchSet(n, +map.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                    "ON CREATE SET n.Name = $Name, n.DateOfBirth = DateOfBirth\r\n\t" +
                    "ON MATCH SET n += $map", cypher.Query);
        }

        #endregion // Merge_On_SetNamedAsMap_Update_Test

        #region Merge_On_SetNamedAsMap_Replace_Test

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
                "ON CREATE SET n.Name = $Name, n.DateOfBirth = DateOfBirth\r\n\t" +
                "ON MATCH SET n = $map", cypher.Query);
        }

        #endregion // Merge_On_SetNamedAsMap_Replace_Test
    }
}

