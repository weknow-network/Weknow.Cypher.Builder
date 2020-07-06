using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Node")]
    [Trait("Segment", "Expression")]
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
            var pattern = Reuse(n => N(n));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n)", pattern.ToString());
        }

        #endregion // (n) / Node_Variable_Test

        #region (:Person) / Node_Label_Test

        [Fact]
        public void Node_Label_Test()
        {
            var pattern = Reuse(n => N(Person));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(:Person)", pattern.ToString());
        }

        #endregion // (:Person) / Node_Label_Test

        #region (n:Person) / Node_Variable_Label_Test

        [Fact]
        public void Node_Variable_Label_Test()
        {
            var pattern = Reuse(n => N(n, Person));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Person)", pattern.ToString());
        }

        #endregion // (n:Person) / Node_Variable_Label_Test

        #region (n:Person { Id: $Id }) / Node_Variable_Label_Property_Test

        [Fact]
        public void Node_Variable_Label_Property_Test()
        {
            var pattern = Reuse(n => N(n, Person, P(Id)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Person { Id: $Id })", pattern.ToString());
        }

        #endregion // (n:Person { Id: $Id }) / Node_Variable_Label_Property_Test

        #region (n:Person $n) / Node_Variable_Label_Map_Test

        [Fact]
        public void Node_Variable_Label_Map_Test()
        {
            var pattern = Reuse(n => N(n, Person, n.AsMap));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Person $n)", pattern.ToString());
        }

        #endregion // (n:Person $n) / Node_Variable_Label_Map_Test

        #region CREATE (n:Person $map) / Node_Variable_Label_MapAsVar_Test

        [Fact]
        public void Node_Variable_Label_MapAsVar_Test()
        {
            var pattern = _(n => map => Create(N(n, Person, map.AsMap)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"CREATE (n:Person $map)", pattern.ToString());
        }

        #endregion // CREATE (n:Person $map) / Node_Variable_Label_MapAsVar_Test

        #region (n:Foo) / Node_T_Variable_Test

        [Fact]
        public void Node_T_Variable_Test()
        {
            var pattern = Reuse(n => N<Foo>(n));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Foo)", pattern.ToString());
        }

        #endregion // (n:Foo) / Node_T_Variable_Test

        #region (n:Foo:Person) / Node_T_Variable_Label_Test

        [Fact]
        public void Node_T_Variable_Label_Test()
        {
            var pattern = Reuse(n => N<Foo>(n, Person));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Foo:Person)", pattern.ToString());
        }

        #endregion // (n:Foo:Person) / Node_T_Variable_Label_Test

        #region (n:Foo:Person { Id: $Id }) / Node_T_Variable_Label_Property_Test

        [Fact]
        public void Node_T_Variable_Label_Property_Test()
        {
            var pattern = Reuse(n => N<Foo>(n, Person, P(Id)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Foo:Person { Id: $Id })", pattern.ToString());
        }

        #endregion // (n:Foo:Person { Id: $Id }) / Node_T_Variable_Label_Property_Test

        #region (n:Foo:Person $n) / Node_T_Variable_Label_Map_Test

        [Fact]
        public void Node_T_Variable_Label_Map_Test()
        {
            var pattern = Reuse(n => N<Foo>(n, Person, n.AsMap));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"(n:Foo:Person $n)", pattern.ToString());
        }

        #endregion //(n:Foo:Person $n) /  Node_T_Variable_Label_Map_Test

        #region CREATE (n:Person $map / Node_T_Variable_Label_MapAsVar_Test

        [Fact]
        public void Node_T_Variable_Label_MapAsVar_Test()
        {
            var pattern = _(n => map => Create(N(n, Person, map.AsMap)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
			 Assert.Equal(@"CREATE (n:Person $map)", pattern.ToString());
        }

        #endregion // CREATE (n:Person $map / Node_T_Variable_Label_MapAsVar_Test

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

        #region MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB }) / Nested_NodeToNode_WithProp_Test

        [Fact]
        public void Nested_NodeToNode_WithProp_Test()
        {
            CypherCommand cypher = _(n1 => n2 => n2_ =>
                                    Match(N(n1, Person, P(PropA, PropB)) >
                                          N(n2, Person, _P(n2_, P(PropA, PropB)))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB }) / Nested_NodeToNode_WithProp_Test

        #region MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB } / Nested_NodeToNode_ReusedProp_Test

        [Fact]
        public void Nested_NodeToNode_ReusedProp_Test()
        {
            CypherCommand cypher = _(_ => P(PropA, PropB).Reuse()
                                    .By(p => n1 => n2 => n2_ =>
                                    Match(N(n1, Person, p) > N(n2, Person, _P(n2_, p)))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB } / Nested_NodeToNode_ReusedProp_Test
    }
}

