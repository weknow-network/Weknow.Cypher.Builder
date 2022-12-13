using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]

    public class NodeTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public NodeTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region (n) / Node_Variable_Test

        [Fact]
        public void Node_Variable_Test()
        {
            var n = Variables.Create();
            var pattern = Reuse(() => N(n));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n)", pattern.ToString());
        }

        #endregion // (n) / Node_Variable_Test

        #region (:Person) / Node_Label_Test

        [Fact]
        public void Node_Label_Test()
        {
            var pattern = Reuse(() => N(Person));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(:Person)", pattern.ToString());
        }

        #endregion // (:Person) / Node_Label_Test

        #region (n:Person) / Node_Variable_Label_Test

        [Fact]
        public void Node_Variable_Label_Test()
        {
            var n = Variables.Create();
            var pattern = Reuse(() => N(n, Person));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person)", pattern.ToString());
        }

        #endregion // (n:Person) / Node_Variable_Label_Test

        #region (n:Person { Id: $Id }) / Node_Variable_Label_Property_Test

        [Fact]
        public void Node_Variable_Label_Property_Test()
        {
            var n = Variables.Create();
            var Id = Parameters.Create();
            var pattern = Reuse(() => N(n, Person, new { Id }));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person { Id: $Id })", pattern.ToString());
        }

        #endregion // (n:Person { Id: $Id }) / Node_Variable_Label_Property_Test

        #region (n:Person $n) / Node_Variable_Label_Map_Test

        [Fact]
        public void Node_Variable_Label_Map_Test()
        {
            var n = Variables.Create();

            var pattern = Reuse(() => N(n, Person, n.AsParameter));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person $n)", pattern.ToString());
        }

        #endregion // (n:Person $n) / Node_Variable_Label_Map_Test

        #region (n:Person: Animal) / Node_MultiLabel_Test

        [Fact]
        public void Node_MultiLabel_Test()
        {
            var n = Variables.Create();
            var pattern = Reuse(() => N(n, Person & Animal));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person:Animal)", pattern.ToString());
        }

        #endregion // (n:Person $n) / Node_MultiLabel_Test

        #region CREATE (n:Person $map) / Node_Variable_Label_MapAsVar_Test

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

        #endregion // CREATE (n:Person $map) / Node_Variable_Label_MapAsVar_Test

        #region MATCH (n1:Person)--(n2:Person) / NodeToNode_Test

        [Fact]
        public void NodeToNode_Test()
        {
            CypherCommand cypher = _(n1 => n2 =>
                                    Match(N(n1, Person) - N(n2, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n1:Person)--(n2:Person)", cypher.Query);
        }

        #endregion // MATCH (n1:Person)--(n2:Person) / NodeToNode_Test

        #region MATCH (n1:Person)-->(n2:Person) / NodeToNode_Forward_Test

        [Fact]
        public void NodeToNode_Forward_Test()
        {
            CypherCommand cypher = _(n1 => n2 =>
                                    Match(N(n1, Person) > N(n2, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n1:Person)-->(n2:Person)", cypher.Query);
        }

        #endregion // MATCH (n1:Person)-->(n2:Person) / NodeToNode_Forward_Test

        #region MATCH (n1:Person)<--(n2:Person) / NodeToNode_Backward_Test

        [Fact]
        public void NodeToNode_Backward_Test()
        {
            CypherCommand cypher = _(n1 => n2 =>
                                    Match(N(n1, Person) < N(n2, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n1:Person)<--(n2:Person)", cypher.Query);
        }

        #endregion // MATCH (n1:Person)<--(n2:Person) / NodeToNodNodeToNode_Backward_Teste_Forward_Test

        #region MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })

        [Fact]
        public void Nested_NodeToNode_WithProp_Test()
        {
            var (n2_PropA, n2_PropB, PropA, PropB) = Parameters.CreateMulti();
            var (n1, n2) = Variables.CreateMulti();

            CypherCommand cypher = _(() =>
                                    Match(N(n1, Person, new { PropA, PropB }) >
                                          N(n2, Person, new { PropA = n2_PropA, PropB = n2_PropB })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB }) 
    }
}

