using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-cheat-sheet/current/
// https://neo4j.com/docs/cypher-manual/5/indexs/

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "FullTextSearch")]
    public class FullTextTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public FullTextTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Node_Score_Limit

        [Fact]
        public void FullText_Node_Score_Limit_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _((n, rate) =>
               FullText("test-index-1", "health~",
                         n, rate,
                         20)
               .FullText("test-index-2",
                         """
                         title: "nice fence"~3^1 description: good year
                         """,
                         n, rate,
                         10));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($$"""
                                CALL db.index.fulltext.queryNodes('test-index-1', $p_0, { limit:$p_1 })
                                {{"\t"}}YIELD node AS n, score AS rate
                                CALL db.index.fulltext.queryNodes('test-index-2', $p_2, { limit:$p_3 })
                                {{"\t"}}YIELD node AS n, score AS rate
                                """, cypher.Query);
            CypherParameters parameters = cypher.Parameters;
            Assert.Equal("health~", parameters["p_0"]);
            Assert.Equal(20, parameters["p_1"]);
            Assert.Equal("title: \"nice fence\"~3^1 description: good year", parameters["p_2"]);
            Assert.Equal(10, parameters["p_3"]);
        }

        #endregion // Node_Score_Limit

        #region Node_Limit

        [Fact]
        public void FullText_Node_Limit_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _((n, rate) =>
               FullText("test-index-1", "health~",
                         n,
                         20)
               .FullText("test-index-2",
                         """
                         title: "nice fence"~3^1 description: good year
                         """,
                         n,
                         10));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($$"""
                                CALL db.index.fulltext.queryNodes('test-index-1', $p_0, { limit:$p_1 })
                                {{"\t"}}YIELD node AS n, score
                                CALL db.index.fulltext.queryNodes('test-index-2', $p_2, { limit:$p_3 })
                                {{"\t"}}YIELD node AS n, score
                                """, cypher.Query);
            CypherParameters parameters = cypher.Parameters;
            Assert.Equal("health~", parameters["p_0"]);
            Assert.Equal(20, parameters["p_1"]);
            Assert.Equal("title: \"nice fence\"~3^1 description: good year", parameters["p_2"]);
            Assert.Equal(10, parameters["p_3"]);
        }

        #endregion // Node_Limit

        #region Node_Score

        [Fact]
        public void FullText_Node_Score_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _((n, rate) =>
               FullText("test-index-1", "health~",
                         n, rate)
               .FullText("test-index-2",
                         """
                         title: "nice fence"~3^1 description: good year
                         """,
                         n, rate));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($$"""
                                CALL db.index.fulltext.queryNodes('test-index-1', $p_0)
                                {{"\t"}}YIELD node AS n, score AS rate
                                CALL db.index.fulltext.queryNodes('test-index-2', $p_1)
                                {{"\t"}}YIELD node AS n, score AS rate
                                """, cypher.Query);
            CypherParameters parameters = cypher.Parameters;
            Assert.Equal("health~", parameters["p_0"]);
            Assert.Equal("title: \"nice fence\"~3^1 description: good year", parameters["p_1"]);
        }

        #endregion // Node_Score

        #region Limit

        [Fact]
        public void FullTextLimit_Test()
        {
            CypherCommand cypher = _(() =>
               FullText("test-index-1", "health~",
                         20)
               .FullText("test-index-2",
                         """
                         title: "nice fence"~3^1 description: good year
                         """,
                         10));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($$"""
                                CALL db.index.fulltext.queryNodes('test-index-1', $p_0, { limit:$p_1 })
                                {{"\t"}}YIELD node, score
                                CALL db.index.fulltext.queryNodes('test-index-2', $p_2, { limit:$p_3 })
                                {{"\t"}}YIELD node, score
                                """, cypher.Query);
            CypherParameters parameters = cypher.Parameters;
            Assert.Equal("health~", parameters["p_0"]);
            Assert.Equal(20, parameters["p_1"]);
            Assert.Equal("title: \"nice fence\"~3^1 description: good year", parameters["p_2"]);
            Assert.Equal(10, parameters["p_3"]);
        }

        #endregion // Limit

        #region Minimal

        [Fact]
        public void FullText_Minimal_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
               FullText("test-index-1", "health~")
               .FullText("test-index-2",
                         """
                         title: "nice fence"~3^1 description: good year
                         """));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($$"""
                                CALL db.index.fulltext.queryNodes('test-index-1', $p_0)
                                {{"\t"}}YIELD node, score
                                CALL db.index.fulltext.queryNodes('test-index-2', $p_1)
                                {{"\t"}}YIELD node, score
                                """, cypher.Query);
            CypherParameters parameters = cypher.Parameters;
            Assert.Equal("health~", parameters["p_0"]);
            Assert.Equal("title: \"nice fence\"~3^1 description: good year", parameters["p_1"]);
        }

        #endregion // Minimal
    }
}

