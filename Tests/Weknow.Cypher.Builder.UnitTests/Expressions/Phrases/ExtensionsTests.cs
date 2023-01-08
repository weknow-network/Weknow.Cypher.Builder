using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.ICypherHigerAbstraction;
using static Weknow.CypherBuilder.Schema;
using Weknow.CypherBuilder.Declarations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Extensions")]

    public class ExtensionsTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ExtensionsTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MERGE ... SetDateConvention

        [Fact]
        public void MergeSetDateConvention_Test()
        {
            CypherCommand cypher = _(n =>
                                        Merge(N(n, Person, new { id = 1}))
                                        .SetDateConvention(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $$"""
                MERGE (n:Person { id: $p_0 })
                {{"\t"}}ON CREATE SET n.`creation-date` = datetime()
                {{"\t"}}ON MATCH SET n.`modification-date` = datetime()
                """,
                cypher.Query);
            Assert.Equal(1, cypher.Parameters["p_0"]);
            Assert.Equal(1, cypher.Parameters.Count);
        }

        #endregion // MERGE ... SetDateConvention

        #region CREATE ... SetDateConvention

        [Fact]
        public void CreateSetDateConvention_Test()
        {
            CypherCommand cypher = _(n =>
                                        Create(N(n, Person, new { id = 1}))
                                        .SetDateConvention(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $$"""
                CREATE (n:Person { id: $p_0 })
                {{"\t"}}SET n.`creation-date` = datetime()
                """,
                cypher.Query);
            Assert.Equal(1, cypher.Parameters["p_0"]);
            Assert.Equal(1, cypher.Parameters.Count);
        }

        #endregion // CREATE ... SetDateConvention
    }
}

