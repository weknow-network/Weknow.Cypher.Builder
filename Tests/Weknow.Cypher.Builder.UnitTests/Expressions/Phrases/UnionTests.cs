using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]
    
    public class UnionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public UnionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH .. RETURN b.Name UNION MATCH .. RETURN b.Name

        [Fact]
        public void Union_Test()
        {
            var b = Variables.Create<Foo>();

            CypherCommand cypher = _(a =>
                                    Match(N(a) - R[KNOWS] > N(b))
                                    .Return(b._.Name)
                                    .Union()
                                    .Match(N(a) - R[LIKE] > N(b))
                                    .Return(b._.Name));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH (a)-[:KNOWS]->(b){NewLine}" +
                $"RETURN b.Name{NewLine}" +
                $"UNION{NewLine}" +
                $"MATCH (a)-[:LIKE]->(b){NewLine}" +
                $"RETURN b.Name",
                cypher.Query);
        }

        #endregion // MATCH .. RETURN b.Name UNION MATCH .. RETURN b.Name

        #region MATCH .. RETURN b.Name UNION ALL MATCH .. RETURN b.Name

        [Fact]
        public void UnionAll_Test()
        {
            var b = Variables.Create<Foo>();

            CypherCommand cypher = _(a =>
                                    Match(N(a) - R[KNOWS] > N(b))
                                    .Return(b._.Name)
                                    .UnionAll()
                                    .Match(N(a) - R[LIKE] > N(b))
                                    .Return(b._.Name));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH (a)-[:KNOWS]->(b){NewLine}" +
                $"RETURN b.Name{NewLine}" +
                $"UNION ALL{NewLine}" +
                $"MATCH (a)-[:LIKE]->(b){NewLine}" +
                $"RETURN b.Name",
                cypher.Query);
        }

        #endregion // MATCH .. RETURN b.Name UNION ALL MATCH .. RETURN b.Name
    }
}

