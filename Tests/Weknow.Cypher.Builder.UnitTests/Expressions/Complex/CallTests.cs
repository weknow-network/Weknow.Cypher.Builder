using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-cheat-sheet/current/
// https://neo4j.com/docs/cypher-manual/5/indexs/

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Call")]
    public class CallTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private ILabel Person = ILabel.Fake;

        #region Ctor

        public CallTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region CALL {...}

        [Fact]
        public void Call_Test()
        {
            CypherCommand cypher = _((n,m, k) => 
                                        Call(Match(N(n, Person)))
                                        .Match(N(n)-N(m))
                                        .Call(Match(N(m)-N(k))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $$"""
                CALL {
                MATCH (n:Person)
                }
                MATCH (n)--(m)
                CALL {
                MATCH (m)--(k)
                }
                """
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // CALL {...}

        #region 

        //[Fact]
        //public void Call_FullText_Test()
        //{
        //    /*
        //    CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50}) YIELD node as locale, score
        //    WITH locale, score
        //    MATCH (locale) <- [:LOCALE_TITLE] - (industry:Industry)
        //    CALL {
        //        WITH industry, score
        //        MATCH p = (industry)-[:CHILD*..15]->(:Industry)<-[:INDUSTRY]-(a:Analyst)
        //        RETURN a.Email as email, length(p) as len, p as path
        //        UNION ALL
        //        WITH industry
        //        MATCH p = (industry)<-[:INDUSTRY]-(a:Analyst)
        //        RETURN  a.Email as email, length(p) as len, p as path
        //    }
        //    // RETURN industry.Title, score, collect(email) // present direct analyst's count per industry 
        //    RETURN industry.Title, score, count(distinct email) // present direct analyst's count per industry 
        //    ORDER BY score DESC 
        //    // RETURN industry.Title, score, email, len // present distance per industry
        //    // RETURN industry, score, email, len, path
        //    // ORDER BY score DESC, len 

        //     */
        //    //var n = Variables.Create<Foo>();
        //    CypherCommand cypher = _(() => Call();

        //    _outputHelper.WriteLine(cypher);
        //    Assert.Equal(
        //        $""
        //        , cypher.Query);
        //    Assert.Empty(cypher.Parameters);
        //}

        #endregion // 
    }
}

