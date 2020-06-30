using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Unwind")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class UnwindTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public UnwindTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Unwind_WithPropConvention_Test

        [Fact]
        public void Unwind_WithPropConvention_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, Convention<Foo>(name => name.StartsWith("Prop"))))));

            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_WithPropConvention_Test

        #region Unwind_Test

        [Fact]
        public void Unwind_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(PropA, PropB)))));

            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_Test

        #region Unwind_ShouldUseDefaultPlurality_Test

        [Fact]
        public void Unwind_ShouldUseDefaultPlurality_Test()
        {
            CypherCommand cypher = _(items => n =>
                                    Unwind(items, // TODO: should use plurality
                                    Match(N(n, Person, P(PropA, PropB)))));

            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_ShouldUseDefaultPlurality_Test

        #region Unwind_ShouldUseCustomPlurality_Test

        [Fact]
        public void Unwind_ShouldUseCustomPlurality_Test()
        {
            CypherCommand cypher = _(items => n =>
                                    Unwind(items, // TODO: should use plurality
                                    Match(N(n, Person, P(PropA, PropB)))),
                                    cfg => cfg.Naming.SetPluralization(
                                                n => n switch
                                                {
                                                    "unit" => "items",
                                                    _ => $"{n}s"
                                                },
                                                n => n switch
                                                {
                                                    "items" => "unit",
                                                    _ => $"unitOf{n}"
                                                }));

            Assert.Equal(@"UNWIND $items AS unit
MATCH (n:Person { PropA: unit.PropA, PropB: unit.PropB })", cypher.Query);
        }

        #endregion // Unwind_ShouldUseCustomPlurality_Test

        #region Unwind_Entities_Update_Test

        [Fact]
        public void Unwind_Entities_Update_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(Id)))
                                    .Set(+n, item)));

            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n += item", cypher.Query);
        }

        #endregion // Unwind_Entities_Update_Test

        #region Unwind_Entities_Replace_Test

        [Fact]
        public void Unwind_Entities_Replace_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(Id)))
                                    .Set(n, item))); // + should be unary operator of IVar

            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n = item", cypher.Query);
        }

        #endregion // Unwind_Entities_Replace_Test

        #region NodeToNode_Test

        [Fact]
        public void NodeToNode_Test()
        {
            CypherCommand cypher = _(n1 => n2 =>
                                    Match(N(n1, Person) - N(n2, Person)));

            Assert.Equal("MATCH (n1:Person)--(n2:Person)", cypher.Query);
        }

        #endregion // NodeToNode_Test

        #region NodeToNode_Forward_Test

        [Fact]
        public void NodeToNode_Forward_Test()
        {
            CypherCommand cypher = _(n1 => n2 =>
                                    Match(N(n1, Person) > N(n2, Person)));

            Assert.Equal("MATCH (n1:Person)-->(n2:Person)", cypher.Query);
        }

        #endregion // NodeToNode_Forward_Test

        #region NodeToNode_Backward_Test

        [Fact]
        public void NodeToNode_Backward_Test()
        {
            CypherCommand cypher = _(n1 => n2 =>
                                    Match(N(n1, Person) < N(n2, Person)));

            Assert.Equal("MATCH (n1:Person)<--(n2:Person)", cypher.Query);
        }

        #endregion // NodeToNodNodeToNode_Backward_Teste_Forward_Test

        #region Nested_NodeToNode_WithProp_Test

        [Fact]
        public void Nested_NodeToNode_WithProp_Test()
        {
            CypherCommand cypher = _(n1 => n2 => n2_ =>
                                    Match(N(n1, Person, P(PropA, PropB)) >
                                          N(n2, Person, Pre(n2_, P(PropA, PropB)))));

            Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // Nested_NodeToNode_WithProp_Test

        #region Nested_NodeToNode_ReusedProp_Test

        [Fact]
        public void Nested_NodeToNode_ReusedProp_Test()
        {
            CypherCommand cypher = _(_ => P(PropA, PropB).Reuse()
                                    .By(p => n1 => n2 => n2_ =>
                                    Match(N(n1, Person, p) > N(n2, Person, Pre(n2_, p)))));

            Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // Nested_NodeToNode_ReusedProp_Test

        #region WhereExists_Test

        [Fact]
        public void WhereExists_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Exists(m => r =>
                                        Match(N(n) - R[r, KNOWS] > N(m))
                                        .Where(n.As<Foo>().Name == m.As<Foo>().Name))));

            Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m)
WHERE n.Name = m.Name }", cypher.Query);
        }

        #endregion // WhereExists_Test

        #region Create_Test

        [Fact]
        public void Create_Test()
        {
            CypherCommand cypher = _(n =>
                                    Create(N(n, Person, P(PropA, PropB))));

            Assert.Equal("CREATE (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Create_Test

        #region CreateAsMap_Test

        [Fact]
        public void CreateAsMap_Test()
        {
            CypherCommand cypher = _(n =>
                                    Create(N(n, Person, n.AsMap)));

            Assert.Equal("CREATE (n:Person { $n })", cypher.Query);
        }

        #endregion // CreateAsMap_Test

        #region CreateAsMap_WithParamName_Test

        [Fact]
        public void CreateAsMap_WithParamName_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Create(N(n, Person, map.AsMap)));

            Assert.Equal("CREATE (n:Person { $map })", cypher.Query);
        }

        #endregion // CreateAsMap_WithParamName_Test

        #region CreateRelation_Test

        [Fact]
        public void CreateRelation_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Create(N(n) - R[r, KNOWS] > N(m)));

            Assert.Equal("CREATE (n)-[r:KNOWS]->(m)", cypher.Query);
        }

        #endregion // CreateRelation_Test

        #region CreateRelation_WithParams_Test

        [Fact]
        public void CreateRelation_WithParams_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Create(N(n) - R[r, KNOWS, P(PropA, PropB)] > N(m)));

            Assert.Equal("CREATE (n)-[r:KNOWS { PropA: $PropA, PropB: $PropB }]->(m)", cypher.Query);
        }

        #endregion // CreateRelation_WithParams_Test

        #region Range_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[1..5] >
            //                        N(m)));

            //Assert.Equal("MATCH (n)-[*1..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_Test

        #region Range_FromAny_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_FromAny_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[..5] >
            //                        N(m)));

            //Assert.Equal("MATCH (n)-[*..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_FromAny_Test

        #region Range_WithVar_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_WithVar_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[r, 1..5] >
            //                        N(m)));

            //Assert.Equal("MATCH (n)-[r*1..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_WithVar_Test

        #region Range_WithVarAndProp_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_WithVarAndProp_Test()
        {
            //    CypherCommand cypher = _(n => r => m =>
            //                            Match(N(n) -
            //                            R[r, KNOWS, P(PropA), 1..5] >
            //                            N(m)));

            //    Assert.Equal("MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_WithVarAndProp_Test

        #region Range_Infinit_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_Infinit_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[..] >
            //                        N(m)));

            //Assert.Equal("MATCH (n)-[*]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_Infinit_Test

        #region Range_Enum_AtMost_Test

        [Fact]
        public void Range_Enum_AtMost_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[Rng.AtMost(5)] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[*..5]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_AtMost_Test

        #region Range_Enum_AtLeast_Test

        [Fact]
        public void Range_Enum_AtLeast_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[Rng.AtLeast(3)] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[*3..]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_AtLeast_Test

        #region Range_Enum_WithVar_Test

        [Fact]
        public void Range_Enum_WithVar_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[r, Rng.Scope(1,5)] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[r*1..5]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_WithVar_Test

        #region Range_Enum_WithVarAndProp_Test

        [Fact]
        public void Range_Enum_WithVarAndProp_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[r, KNOWS, P(PropA), Rng.Scope(1, 5)] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_WithVarAndProp_Test

        #region Range_Enum_Infinit_Test

        [Fact]
        public void Range_Enum_Infinit_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[Rng.Any()] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[*]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_Infinit_Test
    }
}

