using Weknow.CypherBuilder.Declarations;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.Cypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class CreateTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CreateTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Create_Test

        [Fact]
        public void Create_Test()
        {
            var n = Variables.Create();
            var (PropA, PropB) = Parameters.CreateMulti();

            CypherCommand cypher = _(() => Create(N(n, Person, new { PropA, PropB })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Create_Test

        #region CreateAsMap_Test

        [Fact]
        public void CreateAsMap_Test()
        {
            var n = Variables.Create();
            CypherCommand cypher = _(() => Create(N(n, Person, n.AsParameter)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person $n)", cypher.Query);
        }

        #endregion // CreateAsMap_Test

        #region CreateAsMap_WithParamName_Test

        [Fact]
        public void CreateAsMap_WithParamName_Test()
        {
            var n = Variables.Create();
            var map = Parameters.Create();
            CypherCommand cypher = _(() => Create(N(n, Person, map)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person $map)", cypher.Query);
        }

        #endregion // CreateAsMap_WithParamName_Test

        #region CreateRelation_Test

        [Fact]
        public void CreateRelation_Test()
        {
            var (n, r, m) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Create(N(n) - R[r, KNOWS] > N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n)-[r:KNOWS]->(m)", cypher.Query);
        }

        #endregion // CreateRelation_Test

        #region CreateRelation_WithParams_Test

        [Fact]
        public void CreateRelation_WithParams_Test()
        {
            var (n, r, m) = Variables.CreateMulti();
            ParameterDeclaration PropA = Parameters.Create(), PropB = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Create(N(n) - R[r, KNOWS, new { PropA, PropB }] > N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n)-[r:KNOWS { PropA: $PropA, PropB: $PropB }]->(m)", cypher.Query);
        }

        #endregion // CreateRelation_WithParams_Test
    }
}

