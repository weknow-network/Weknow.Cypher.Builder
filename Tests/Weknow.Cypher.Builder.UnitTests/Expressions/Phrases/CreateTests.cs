using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
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
            ParameterDeclaration PropA = CreateParameter(), PropB = CreateParameter();

            CypherCommand cypher = _(n =>
                                    Create(N(n, Person, new { PropA, PropB })));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("CREATE (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Create_Test

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

        #region CreateRelation_Test

        [Fact]
        public void CreateRelation_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Create(N(n) - R[r, KNOWS] > N(m)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("CREATE (n)-[r:KNOWS]->(m)", cypher.Query);
        }

        #endregion // CreateRelation_Test

        #region CreateRelation_WithParams_Test

        [Fact]
        public void CreateRelation_WithParams_Test()
        {
            ParameterDeclaration PropA = CreateParameter(), PropB = CreateParameter();
            CypherCommand cypher = _(n => r => m =>
                                    Create(N(n) - R[r, KNOWS, new { PropA, PropB }] > N(m)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("CREATE (n)-[r:KNOWS { PropA: $PropA, PropB: $PropB }]->(m)", cypher.Query);
        }

        #endregion // CreateRelation_WithParams_Test
    }
}

