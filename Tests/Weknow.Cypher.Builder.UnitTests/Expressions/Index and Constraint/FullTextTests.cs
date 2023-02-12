using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

// https://neo4j.com/docs/cypher-cheat-sheet/current/
// https://neo4j.com/docs/cypher-manual/5/indexs/

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Index")]
    public class FullTextTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public FullTextTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region FullText1_Test

        [Fact]
        public void FullText1_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _((n,rate) =>
               FullText("test-index-1", "health~",
                         n, rate, 
                         20 )
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

        #endregion // FullText1_Test
    }
}

