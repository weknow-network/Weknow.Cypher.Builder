using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]

    public class RangeTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public RangeTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n)-[*1..5]->(m)

        [Fact]
        public void Range_Scope_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[KNOWS * new System.Range(1, 5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[:KNOWS*1..5]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[*1..5]->(m)

        #region MATCH (n)-[*1..]->(m)

        [Fact]
        public void Range_StartAt_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[KNOWS * System.Range.StartAt(1)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[:KNOWS*1..]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[*1..]->(m)

        #region MATCH (n)-[*..5]->(m)

        [Fact]
        public void Range_Multiply_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[KNOWS * 5] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[:KNOWS*..5]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[*..5]->(m)

        #region MATCH (n)-[*1..5]->(m)

        [Fact]
        public void Range_Enum_Scope_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[new Range(1, 5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[*1..5]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[*1..5]->(m)

        #region MATCH (n)-[*..5]->(m)

        [Fact]
        public void Range_Enum_AtMost_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[System.Range.EndAt(5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[*..5]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[*..5]->(m)

        #region MATCH (n)-[*3..]->(m)

        [Fact]
        public void Range_Enum_AtLeast_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[System.Range.StartAt(3)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[*3..]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[*3..]->(m)

        #region MATCH (n)-[r*1..5]->(m)

        [Fact]
        public void Range_Enum_WithVar_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[r, new System.Range(1, 5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[r*1..5]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[r*1..5]->(m)

        #region MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)

        [Fact]
        public void Range_Enum_WithVarAndProp_Test()
        {
            var PropA = Parameters.Create();
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[r, KNOWS, new { PropA }, new System.Range(1, 5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)

        #region MATCH (n)-[:KNOWS*1..5]->(m)

        [Fact]
        public void Range_Enum_WithType_Test()
        {
            var PropA = Parameters.Create();
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[KNOWS, new System.Range(1, 5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[:KNOWS*1..5]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[:KNOWS*1..5]->(m)

        #region MATCH (n)-[*]->(m)

        [Fact]
        public void Range_Enum_Infinit_Test()
        {
            CypherCommand cypher = _((n, r, m) =>
                                    Match(N(n) -
                                    R[System.Range.All] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)-[*]->(m)", cypher.Query);
        }

        #endregion // MATCH (n)-[*]->(m)
    }
}