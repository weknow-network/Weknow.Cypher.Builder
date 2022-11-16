using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
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
            var (Id, p) = Parameters.CreateMulti();
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { Id }))
                                    .SetPlus(n, p));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
"SET n += $p", cypher.Query);
        }

        #endregion // Match_SetAsMap_Update_Test

        #region CreateAsMap_Test

        [Fact]
        public void CreateAsMap_Test()
        {
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Create(N(n, Person, n.AsParameter)));

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

            CypherCommand cypher = _(() =>
                                    Create(N(n, Person, map)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person $map)", cypher.Query);
        }

        #endregion // CreateAsMap_WithParamName_Test

        #region Node_Variable_Label_Map_Test

        [Fact]
        public void Node_Variable_Label_Test()
        {
            var n = Variables.Create();

            var pattern = Reuse(() => N(n, Person, n.AsParameter));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person $n)", pattern.ToString());
        }

        #endregion // Node_Variable_Label_Map_Test

        #region Node_Variable_Label_MapAsVar_Test

        [Fact]
        public void Node_Variable_Label_MapAsVar_Test()
        {
            var n = Variables.Create();
            var map = Parameters.Create();

            var pattern = _(() => Create(N(n, Person, map)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"CREATE (n:Person $map)", pattern.ToString());
        }

        #endregion // Node_Variable_Label_MapAsVar_Test
    }
}

