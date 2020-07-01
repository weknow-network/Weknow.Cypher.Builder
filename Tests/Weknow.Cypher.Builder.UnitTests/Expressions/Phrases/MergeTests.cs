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
@"MERGE (n:Person { Id: $Id })
SET n += $n", cypher.Query);
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
@"MERGE (n:Person { Id: $Id })
SET n = $n", cypher.Query);
        }

        #endregion // Merge_SetAsMap_Replace_Test

        #region Merge_On_Create_SetProperties_Test

        [Fact]
        public void Merge_On_Create_SetProperties_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(P(PropA, PropB)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MERGE (n:Person { Id: $Id })
    ON CREATE SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
            throw new NotImplementedException();
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

        #region Merge_On_SetAsMap_Update_Test

        [Fact]
        public void Merge_On_SetAsMap_Update_Test()
        {
            //            CypherCommand cypher = _(n =>
            //                                    Merge(N(n, Person, P(Id)))
            //                                    .OnCreate(Convention<Foo>(m => n != nameof(Foo.Id)))
            //                                    .OnMatch(+n.AsMap));

            //			 _outputHelper.WriteLine(cypher);
            //			 Assert.Equal(
            //@"MERGE (n:Person { Id: $Id })
            //ON CREATE SET n.Name = $Name, n.DateOfBirth = DateOfBirth
            //ON MATCH SET n += $n", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Merge_On_SetAsMap_Update_Test

        #region Merge_On_SetAsMap_Replace_Test

        [Fact]
        public void Merge_On_SetAsMap_Replace_Test()
        {
            //            CypherCommand cypher = _(n =>
            //                                    Merge(N(n, Person, P(Id)))
            //                                    .OnCreate(Convention<Foo>(m => n != nameof(Foo.Id)))
            //                                    .OnMatch(n.AsMap));

            //			 _outputHelper.WriteLine(cypher);
            //			 Assert.Equal(
            //@"MERGE (n:Person { Id: $Id })
            //ON CREATE SET n.Name = $Name, n.DateOfBirth = DateOfBirth
            //ON MATCH SET n = $n", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Merge_On_SetAsMap_Replace_Test

        #region Merge_On_SetNamedAsMap_Update_Test

        [Fact]
        public void Merge_On_SetNamedAsMap_Update_Test()
        {
            //            CypherCommand cypher = _(n => map =>
            //                                    Merge(N(n, Person, P(Id)))
            //                                    .OnCreate(Convention<Foo>(m => n != nameof(Foo.Id)))
            //                                    .OnMatch(+map.AsMap));

            //			 _outputHelper.WriteLine(cypher);
            //			 Assert.Equal(
            //@"MERGE (n:Person { Id: $Id })
            //ON CREATE SET n.Name = $Name, n.DateOfBirth = DateOfBirth
            //ON MATCH SET n += $map", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Merge_On_SetNamedAsMap_Update_Test

        #region Merge_On_SetNamedAsMap_Replace_Test

        [Fact]
        public void Merge_On_SetNamedAsMap_Replace_Test()
        {
            //            CypherCommand cypher = _(n => map =>
            //                                    Merge(N(n, Person, P(Id)))
            //                                    .OnCreate(Convention<Foo>(m => n != nameof(Foo.Id)))
            //                                    .OnMatch(map.AsMap));

            //            _outputHelper.WriteLine(cypher);
            //            Assert.EqualConvention
            //@"MERGE (n:Person { Id: $Id })
            //            ON CREATE SET n.Name = $Name, n.DateOfBirth = DateOfBirth
            //            ON MATCH SET n = $map", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Merge_On_SetNamedAsMap_Replace_Test
    }
}

