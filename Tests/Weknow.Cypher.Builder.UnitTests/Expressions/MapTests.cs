using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Match")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class MapTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public MapTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Match_SetAsMap_Update_Test

        [Fact]
        public void Match_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(+n.AsMap));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n += $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Update_Test

        #region Match_SetAsMap_Replace_Test

        [Fact]
        public void Match_SetAsMap_Replace_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.AsMap));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n = $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Replace_Test

        #region CreateAsMap_Test

        [Fact]
        public void CreateAsMap_Test()
        {
            CypherCommand cypher = _(n =>
                                    Create(N(n, Person, n.AsMap)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("CREATE (n:Person $n)", cypher.Query);
        }

        #endregion // CreateAsMap_Test

        #region CreateAsMap_WithParamName_Test

        [Fact]
        public void CreateAsMap_WithParamName_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Create(N(n, Person, map.AsMap)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("CREATE (n:Person $map)", cypher.Query);
        }

        #endregion // CreateAsMap_WithParamName_Test

        #region Node_Variable_Label_Map_Test

        [Fact]
        public void Node_Variable_Label_Test()
        {
            var pattern = Reuse(n => N(n, Person, n.AsMap));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Person $n)", pattern.ToString());
        }

        #endregion // Node_Variable_Label_Map_Test

        #region Node_Variable_Label_MapAsVar_Test

        [Fact]
        public void Node_Variable_Label_MapAsVar_Test()
        {
            var pattern = _(n => map => Create(N(n, Person, map.AsMap)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"CREATE (n:Person $map)", pattern.ToString());
        }

        #endregion // Node_Variable_Label_MapAsVar_Test

        #region Node_T_Variable_Label_Map_Test

        [Fact]
        public void Node_T_Variable_Label_Map_Test()
        {
            var pattern = Reuse(n => N<Foo>(n, Person, n.AsMap));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Foo:Person $n)", pattern.ToString());
        }

        #endregion // Node_T_Variable_Label_Map_Test

        #region Node_T_Variable_Label_MapAsVar_Test

        [Fact]
        public void Node_T_Variable_Label_MapAsVar_Test()
        {
            var pattern = _(n => map => Create(N(n, Person, map.AsMap)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"CREATE (n:Person $map)", pattern.ToString());
        }

        #endregion // Node_T_Variable_Label_MapAsVar_Test

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


        // MERGE(p:Person { name: $map.name})
        //            ON CREATE SET p = $map
    }
}

