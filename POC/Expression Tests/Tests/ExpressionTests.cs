using System;
using System.Linq.Expressions;
using Xunit;
using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

// TODO: Duplicate class Pattern to FullNamePattern for naming standard

// TODO: parameter factory injection for enabling to work with Neo4jParameters (Neo4jMapper)
//       Mimic Neo4jMappaer WithEntity, WithEntities + integration test
//       validate flat entity (in deep complex type throw exception with recommendation for best practice)

namespace Weknow.Cypher.Builder
{
    public class ExpressionTests
    {
        #region Config_Test

        [Fact]
        public void Config_Test()
        {
            CypherCommand cypher = _(a => r1 => b => r2 => c =>
             Match(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
             .Where(a.As<Foo>().Name == "Avi")
             .Return(a.As<Foo>().Name, r1, b.All<Bar>(), r2, c)
             .OrderBy(a.As<Foo>().Name)
             .Skip(1)
             .Limit(10),
             cfg =>
             {
                 cfg.AmbientLabels.Add("Prod", "MyOrg");
                 cfg.AmbientLabels.Formatter = "`@{0}`";
                 cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
             });

            Assert.Equal(
@"MATCH (a:PERSON:`@PROD`:`@MY_ORG`)-[r1:KNOWS]->(b:PERSON:`@PROD`:`@MY_ORG`)<-[r2:KNOWS]-(c:PERSON:`@PROD`:`@MY_ORG`)
WHERE a.Name = $p_0
RETURN a.Name, r1, b.Id, b.Name, b.Date, r2, c
ORDER BY a.Name
SKIP $p_1
LIMIT $p_2", cypher.Query);

            Assert.Equal("Avi", cypher.Parameters["p_0"]);
            Assert.Equal(1, cypher.Parameters["p_1"]);
            Assert.Equal(10, cypher.Parameters["p_2"]);
        }

        #endregion // Config_Test

        #region ComplexExpression_Test

        [Fact]
        public void ComplexExpression_Test()
        {
            CypherCommand cypher = _(a => r1 => b => r2 => c =>
             Match(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
             .Where(a.As<Foo>().Name == "Avi")
             .Return(a.As<Foo>().Name, r1, b.All<Bar>(), r2, c)
             .OrderBy(a.As<Foo>().Name)
             .Skip(1)
             .Limit(10));

            Assert.Equal(
@"MATCH (a:Person)-[r1:KNOWS]->(b:Person)<-[r2:KNOWS]-(c:Person)
WHERE a.Name = $p_0
RETURN a.Name, r1, b.Id, b.Name, b.Date, r2, c
ORDER BY a.Name
SKIP $p_1
LIMIT $p_2", cypher.Query);

            Assert.Equal("Avi", cypher.Parameters["p_0"]);
            Assert.Equal(1, cypher.Parameters["p_1"]);
            Assert.Equal(10, cypher.Parameters["p_2"]);
        }

        #endregion // ComplexExpression_Test

        #region CaptureProperties_Test

        [Fact]
        public void CaptureProperties_Test()
        {
            CypherCommand cypher = _(_ => P(PropA, PropB).Reuse().By(p => n => Match(N(n, Person, p))));

            Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // CaptureProperties_Test


        #region CaptureNodeAndProperties_Test

        [Fact]
        public void CaptureNodeAndProperties_Test()
        {
            CypherCommand cypher = _(n => P(PropA, PropB).Reuse()
                                          .By(p => N(n, Person, p).Reuse()
                                          .By(n => Match(n))));

            Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // CaptureNodeAndProperties_Test


        // TODO: disable the option of chaining Reuse in a row because of the backward ordering (confusion)
        // TODO: Thinking how to maintain the order, for example having base class which will get enumerable and return enumerable. the enumerable can be reorder, it should be hidden from our user (maybe via base class).
        #region ReuseSuggestion_Test

        [Fact]
        public void ReuseSuggestion_Test()
        {
            // What if the reuse will return CypherPart class, which is not a full cypher query. this class can implement enumerable (backward implementation)

            //CypherPhrases phrases = _(person => animal => Reuse(N(person, Person))
            //                 .Reuse(N(animal, Animal)));
            //CypherCommand cypher = _(.Use(phrases, person => animal => r => // backward ordering bug will be very tricky to observe
            //              Match(person - R[r, LIKE] > animal)));


            // Assert.Equal("MATCH (n1:Person)-[r:LIKE]->(n:Animal)", cypher.Query);

            throw new NotImplementedException();
        }

        #endregion // ReuseSuggestion_Test

        #region Node_LabelOnly_Test

        [Fact]
        public void Node_LabelOnly_Test()
        {
            string cypher1 = _(() => Match(N(Person)));
            string cypher2 = _(n => Match(N(Person)));

            Assert.Equal(cypher1, cypher2);
        }

        #endregion // Node_LabelOnly_Test

        #region Reuse_Unordered_Test

        [Fact]
        public void Reuse_UnorderedNode_Test()
        {
            CypherCommand cypher = _(person => animal => N(person, Person)
                                    .Reuse(N(animal, Animal).Reuse())
                         .By(person => animal => r =>
                          Match(person - R[r, LIKE] > animal)));

            Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // Reuse_Unordered_Test

        #region Reuse_Unordered_Test

        [Fact]
        public void Reuse_Unordered_Test()
        {
            CypherCommand cypher = _(n => P(PropA, PropB).Reuse(N(n, Person).Reuse())
                                     .By(p => n => n1 =>
                                      Match(N(n1, Person, p) - n)));

            Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })--(n:Person)", cypher.Query);
        }

        #endregion // Reuse_Unordered_Test

        #region Reuse_Plural_UNWIND_Test

        [Fact]
        public void Reuse_Plural_UNWIND_Test()
        {
            CypherCommand cypher = _(n =>
                          P(PropA, PropB).Reuse()
                         .By(p => items =>
                            Unwind(items, Match(N(n, Person, p)))
                         ));

            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Reuse_Plural_UNWIND_Test

        #region Properties_Test

        [Fact]
        public void Properties_Test()
        {
            CypherCommand cypher = _(n => Match(N(n, Person, P(PropA, PropB))));

            Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Test

        #region Properties_OfT_DefaultLabel_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));

            Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultLabel_Test

        // TODO: thinking on a way to keep the generics within the method scope in order to reduce suplication
        #region Properties_OfT_DefaultLabel_AvoidDuplication_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_AvoidDuplication_Test()
        {
            //// Current style: CypherCommand cypher = _(n => Match(N<Foo>(n, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));
            //CypherCommand cypher = _(n => Match(N<Foo>(n, P(n.PropA /* N<Foo> */, n.As<Bar>().Date))));

            //Assert.Equal("MATCH (n:Foo { PropA: $PropA, Date: $Date })", cypher.Query);
            throw new NotFiniteNumberException();
        }

        #endregion // Properties_OfT_DefaultLabel_AvoidDuplication_Test

        #region Properties_OfT_DefaultAndAdditionLabel_Test

        [Fact]
        public void Properties_OfT_DefaultAndAdditionLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, Person, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));

            Assert.Equal("MATCH (n:Foo:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultAndAdditionLabel_Test

        #region Properties_OfT_Test

        [Fact]
        public void Properties_OfT_Test()
        {
            CypherCommand cypher = _(n => Match(N(n, Person, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));

            Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_Test

        #region Properties_WithPrefix_Test

        [Fact]
        public void Properties_WithPrefix_Test()
        {
            CypherCommand cypher = _(n1 => n2 => n2_ =>
                                    Match(N(n1, Person, P(PropA, PropB)) -
                                          R[n1, KNOWS] >
                                          N(n2, Person, Pre(n2_, P(PropA, PropB)))));

            Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-" +
                         "[n1:KNOWS]->" +
                         "(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // Properties_WithPrefix_Test

        #region Properties_Convention_WithDefaultLabel_Test

        [Fact]
        public void Properties_Convention_WithDefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, Convention(name => name.StartsWith("Prop")))));

            Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_WithDefaultLabel_Test

        #region Properties_All_WithDefaultLabel_Test

        [Fact]
        public void Properties_All_WithDefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, All(n.As<Foo>().Id, n.As<Foo>().Name))));

            Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_All_WithDefaultLabel_Test

        #region Properties_Convention_Test

        [Fact]
        public void Properties_Convention_Test()
        {
            CypherCommand cypher = _(n => Match(N(n, Person, Convention<Foo>(name => name.StartsWith("Prop")))));

            Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_Test

        #region Match_SetAsMap_Update_Test

        [Fact]
        public void Match_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(+n.AsMap));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n += $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Update_Test

        #region Match_SetAsMap_Replace_Test

        [Fact]
        public void Match_SetAsMap_Replace_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.AsMap));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n = $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Replace_Test

        #region Match_Set_WithPrefix_Test

        [Fact]
        public void Match_Set_WithPrefix_Test()
        {
            CypherCommand cypher = _(n => n_ =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.P(PropA, Pre(n_, PropB))));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $n_PropB", cypher.Query);
        }

        #endregion // Match_Set_WithPrefix_Test

        #region Match_Set_Test

        [Fact]
        public void Match_Set_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.P(PropA, PropB)));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_Test

        #region Match_Set_OfT_Test

        [Fact]
        public void Match_Set_OfT_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(P(n.As<Foo>().PropA, n.As<Foo>().PropB)));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_Test

        #region Match_Set_OfT_Convention_Test

        [Fact]
        public void Match_Set_OfT_Convention_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.Convention<Foo>(name => name.StartsWith("Prop"))));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_Convention_Test

        #region Match_Set_OfT_All_Test

        [Fact]
        public void Match_Set_OfT_All_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.All<Foo>()));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.Id = $Id, n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_All_Test

        #region Match_Set_OfT_Except_Test

        [Fact]
        public void Match_Set_OfT_Except_Test()
        {
            //            CypherCommand cypher = P(n =>
            //                                    Match(N(n, Person, P(Id)))
            //                                    .Set(n.Except<Foo>(n => P(n.Id, n.Name)))); 

            //            Assert.Equal(
            //@"MATCH (n:Person {Id: $Id})
            //            SET n.PropA  = $PropA, n.PropB  = $PropB", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Match_Set_OfT_Except_Test

        #region Match_Set_AddLabel_Test

        [Fact]
        public void Match_Set_AddLabel_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N<Foo>(n, P(Id)))
                                    .Set(n, Person));

            Assert.Equal(
@"MATCH (n:Foo { Id: $Id })
SET n:Person", cypher.Query);
        }

        #endregion // Match_Set_AddLabel_Test

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
                                                    "items" => "item",
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

        [Fact]
        public void Range_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[1..5] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[*1..5]->(m)", cypher.Query);
        }

        #endregion // Range_Test

        #region Range_FromAny_Test

        [Fact]
        public void Range_FromAny_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[..5] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[*..5]->(m)", cypher.Query);
        }

        #endregion // Range_FromAny_Test

        #region Range_WithVar_Test

        [Fact]
        public void Range_WithVar_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[r, 1..5] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[r*1..5]->(m)", cypher.Query);
        }

        #endregion // Range_WithVar_Test

        #region Range_WithVarAndProp_Test

        [Fact]
        public void Range_WithVarAndProp_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[r, KNOWS, P(PropA), 1..5] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)", cypher.Query);
        }

        #endregion // Range_WithVarAndProp_Test

        #region Range_Infinit_Test

        [Fact]
        public void Range_Infinit_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[..] >
                                    N(m)));

            Assert.Equal("MATCH (n)-[*]->(m)", cypher.Query);
        }

        #endregion // Range_Infinit_Test

        // TODO: FOREACH, DELETE, DETACH
        // TODO: UNION, UNION ALL
        /*
         CALL {
  MATCH (p:Person)-[:FRIEND_OF]->(other:Person) RETURN p, other
  UNION
  MATCH (p:Child)-[:CHILD_OF]->(other:Parent) RETURN p, other
}
         */

        // TODO: Ambient Context Label
        // TODO: Label convention

        // TODO: Auto WITH

        // TODO: AND, OR, AS,
        // TODO: variable IS NULL
        // TODO: NOT exists(n.property
        // TODO: MERGE OnCreate OnMatch
        // TODO: [x IN list | x.prop]
        // TODO: [x IN list WHERE x.prop <> $value]
        // TODO: [x IN list WHERE x.prop <> $value | x.prop]
        // TODO: reduce(s = "", x IN list | s + x.prop) // Aggregate + Reduce overloads
        // TODO: all(x IN coll WHERE exists(x.property)) // LINQ
        // TODO: any(x IN coll WHERE exists(x.property)) // LINQ
        // TODO: none(x IN coll WHERE exists(x.property))  // LINQ + overload for none
        // TODO: single(x IN coll WHERE exists(x.property)) // LINQ
        // TODO: CASE  WHEN ELSE 

        // TODO:list[$idx] AS value, // Indexer
        // TODO:list[$startIdx..$endIdx] AS slice // Indexer[range]

        // TODO: n.property STARTS WITH 'Tim' OR // string + analyzer in future
        // TODO: n.property ENDS WITH 'n' OR// string + analyzer in future
        // TODO: n.property CONTAINS 'goodie'// string + analyzer in future
        // TODO: n.property =~ 'Tim.*' 
        // TODO: n.property IN [$value1, $value2]' // LINQ Contains + overload In

        // TODO: exists(n.property),
        // TODO: coalesce, // ??
        // TODO: timestamp, id, 
        // TODO: toInteger, toFloat, toBoolean, // cast
        // TODO: head, last, tail, // LINQ first, last, skip(1), apoc -> skip (n)
        // TODO: keys, properties, 
        // TODO: count // LINQ
        // TODO: length, Size // TODO: what is the right usage
        // TODO: collect, 
        // TODO: nodes, relationships, 

        // TODO: sum, percentileDisc, stDev, abs, rand, sqrt, sign, 
        // TODO: sin, cos, tan, cot, asin, acos, atan, atan2, haversin
        // TODO: degrees, radians, pi, 
        // TODO: log10, log, exp, pi

        // TODO: toString, replace, substring, left, trim, toUpper, split, reverse

        // TODO: Spatial, 
        // TODO: date time / duration related // DateTime / TimeSpan

        // TODO: Constraint and Index

        // TODO: USER / ROLE MANAGEMENT and Privileges

        /* TODO:          
         CALL {
  MATCH (p:Person)-[:FRIEND_OF]->(other:Person) RETURN p, other
  UNION
  MATCH (p:Child)-[:CHILD_OF]->(other:Parent) RETURN p, other
}*/


        // todo: {name: 'Alice', age: 38,
        //       address: {city: 'London', residential: true}}
    }
}

