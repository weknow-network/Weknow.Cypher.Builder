using System;
using System.Linq.Expressions;
using Xunit;
using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

// TODO: Copy class Pattern to FullBamePattern for naming standard

// TODO: parameter factory injection for enabling to work with Neo4jParameters (Neo4jMapper)
//       Mimic Neo4jMappaer WithEntity, WithEntities + integration test
//       validate flat entity (in deep complex type throw exception with recommendation for best practice)

namespace Weknow.Cypher.Builder
{
    public class ExpressionTests
    {
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

        // TODO: P(n => N(n, Person, p)) instead of P(n => N(n, Person, P(p))) 
        #region CaptureProperties_Test

        [Fact]
        public void CaptureProperties_Test()
        {
            Expression<Func<IProperties>> p = () => P(PropA, PropB);
            CypherCommand cypher = _(n => N(n, Person, P(p)));

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // CaptureProperties_Test

        #region Properties_Test

        [Fact]
        public void Properties_Test()
        {
            CypherCommand cypher = _(n => N(n, Person, P(PropA, PropB)));

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Test

        #region Properties_OfT_DefaultLabel_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_Test()
        {
            CypherCommand cypher = _(n => N<Foo>(n, n => P(n.PropA, n.PropB)));

            Assert.Equal("(n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultLabel_Test

        #region Properties_OfT_DefaultAndAdditionLabel_Test

        [Fact]
        public void Properties_OfT_DefaultAndAdditionLabel_Test()
        {
            CypherCommand cypher = _(n => N<Foo>(n, Person, n => P(n.PropA, n.PropB)));

            Assert.Equal("(n:Foo:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultAndAdditionLabel_Test

        // TODO: TBD: how can we use syntax with single P()
        #region Properties_OfT_Test

        [Fact]
        public void Properties_OfT_Test()
        {
            CypherCommand cypher = _(n => N(n, Person, P<Foo>(n => P(n.PropA, n.PropB))));

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_Test

        #region Properties_WithPrefix_Test

        [Fact]
        public void Properties_WithPrefix_Test()
        {
            CypherCommand cypher = _(n1 => n2 => n2_ =>
                                    N(n1, Person, P(PropA, PropB)) -
                                    R[n1, KNOWS] >
                                    N(n2, Person, Pre(n2_, P(PropA, PropB))));

            Assert.Equal("(n1:Person { PropA: $PropA, PropB: $PropB })-" +
                         "[n1:KNOWS]->" +
                         "(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // Properties_WithPrefix_Test

        #region Properties_Convention_WithDefaultLabel_Test

        [Fact]
        public void Properties_Convention_WithDefaultLabel_Test()
        {
            CypherCommand cypher = _(n => N<Foo>(n, Convention(name => name.StartsWith("Prop"))));

            Assert.Equal("(n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_WithDefaultLabel_Test

        #region Properties_All_WithDefaultLabel_Test

        [Fact]
        public void Properties_All_WithDefaultLabel_Test()
        {
            //CypherCommand cypher = P(n => N<Foo>(n, All(f => f.Id, f => f.Name)));

            //Assert.Equal("(n:Foo {  PropA: $PropA, PropB: $PropB })", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Properties_All_WithDefaultLabel_Test

        #region Properties_Convention_Test

        [Fact]
        public void Properties_Convention_Test()
        {
            CypherCommand cypher = _(n => N(n, Person, Convention<Foo>(name => name.StartsWith("Prop"))));

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_Test

        #region Match_SetAsMap_Update_Test

        [Fact]
        public void Match_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(+n, n.AsMap)); // + should be unary operator of IVar

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
                                    .Set(+n, n.AsMap));

            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n += $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Replace_Test

        #region Match_Set_WithPrefix_Test

        [Fact]
        public void Match_Set_WithPrefix_Test()
        {
            //            CypherCommand cypher = P(n => n_ =>
            //                                    Match(N(n, Person, P(Id)))
            //                                    .Set(n, P(PropA, Pre(n_, PropB)))); 

            //            Assert.Equal(
            //@"MATCH (n:Person {Id: $Id})
            //SET n.PropA  = $PropA, n.PropB  = $n_PropB", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Match_Set_WithPrefix_Test

        #region Match_Set_Test

        [Fact]
        public void Match_Set_Test()
        {
            //            CypherCommand cypher = _(n =>
            //                                    Match(N(n, Person, P(Id)))
            //                                    .Set(n, P(PropA, PropB)));

            //            Assert.Equal(
            //@"MATCH (n:Person { Id: $Id })
            //SET n.PropA  = $PropA, n.PropB  = $PropB", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Match_Set_Test

        #region Match_Set_OfT_Test

        [Fact]
        public void Match_Set_OfT_Test()
        {
            //            CypherCommand cypher = P(n =>
            //                                    Match(N(n, Person, P(Id)))
            //                                    .Set(n, P<Foo>(f => P(f.PropA, f.PropB)))); 

            //            Assert.Equal(
            //@"MATCH (n:Person {Id: $Id})
            //            SET n.PropA  = $PropA, n.PropB  = $PropB", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Match_Set_OfT_Test

        #region Match_Set_OfT_Convention_Test

        [Fact]
        public void Match_Set_OfT_Convention_Test()
        {
            //            CypherCommand cypher = P(n =>
            //                                    Match(N(n, Person, P(Id)))
            //                                    .Set(n, Convention<T>(name => name.StartsWith("Prop"))); 

            //            Assert.Equal(
            //@"MATCH (n:Person {Id: $Id})
            //            SET n.PropA  = $PropA, n.PropB  = $PropB", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Match_Set_OfT_Convention_Test

        #region Match_Set_OfT_All_Test

        [Fact]
        public void Match_Set_OfT_All_Test()
        {
            //            CypherCommand cypher = P(n =>
            //                                    Match(N(n, Person, P(Id)))
            //                                    .Set(n, All<Foo>(f => f.Id, f => f.Name))); 

            //            Assert.Equal(
            //@"MATCH (n:Person {Id: $Id})
            //            SET n.PropA  = $PropA, n.PropB  = $PropB", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Match_Set_OfT_All_Test

        #region Match_Set_OfT_Except_Test

        [Fact]
        public void Match_Set_OfT_Except_Test()
        {
            //            CypherCommand cypher = P(n =>
            //                                    Match(N(n, Person, P(Id)))
            //                                    .Set(n, Except<Foo>(n => P(n.Id, n.Name)))); 

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

        #region Unwind_Entities_Update_Test

        [Fact]
        public void Unwind_Entities_Update_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(Id)))
                                    .Set(+n, item))); // + should be unary operator of IVar

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

        // TODO: Move Higher order component tests
        #region Concurrency_Pattern_Test

        [Fact]
        public void Concurrency_Pattern_Test()
        {
            //CypherCommand cypher = P(n =>
            //                        Merge(N(n, Person, P(PropA, PropB, Concurrency))),
            //                        SET(eTag(n, Concurrency));

            //Assert.Equal(@"
            //Merge (n:Person { PropA: $PropA, PropB: $PropB, Concurrency: $Concurrency })
            //ON CREATE SET n.Concurrency = 1
            //ON MATCH SET n.Concurrency = n.Concurrency + 1", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Concurrency_Pattern_Test

        // TODO: Move to Higher component tests
        #region Unwind_Concurrency_Pattern_Test

        [Fact]
        public void Unwind_Concurrency_Pattern_Test()
        {
            //CypherCommand cypher = P(items => item => n =>
            //                        Unwind(items, item,
            //                        Merge(N(n, Person, P(PropA, PropB, Concurrency))),
            //                        SET(eTag(n, Concurrency)));

            //Assert.Equal(@"UNWIND $items as item
            //Merge (n:Person { PropA: item.PropA, PropB: item.PropB, Concurrency: $Concurrency })
            //ON CREATE SET n.Concurrency = 1
            //ON MATCH SET n.Concurrency = n.Concurrency + 1", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Unwind_Concurrency_Pattern_Test

        #region NodeToNode_Test

        [Fact]
        public void NodeToNode_Test()
        {
            //CypherCommand cypher = P(n1 => n2 =>
            //                        Match(N(n1, Person))-N(n2 Person));

            //Assert.Equal("MATCH (n1:Person)--(n2:Person)", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // NodeToNode_Test

        #region NodeToNode_Forward_Test

        [Fact]
        public void NodeToNode_Forward_Test()
        {
            //CypherCommand cypher = P(n1 => n2 =>
            //                        Match(N(n1, Person))>N(n2 Person));

            //Assert.Equal("MATCH (n1:Person)-->(n2:Person)", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // NodeToNode_Forward_Test

        #region NodeToNode_Backward_Test

        [Fact]
        public void NodeToNode_Backward_Test()
        {
            //CypherCommand cypher = _(n1 => n2 =>
            //                        Match(N(n1, Person)) < N(n2, Person));

            //Assert.Equal("MATCH (n1:Person)<--(n2:Person)", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // NodeToNodNodeToNode_Backward_Teste_Forward_Test

        #region Nested_NodeToNode_WithProp_Test

        [Fact]
        public void Nested_NodeToNode_WithProp_Test()
        {
            //CypherCommand cypher = P(n1 => n2 => n2_ =>
            //                        N(n1, Person, P(PropA, PropB))->
            //                        N(n2, Person, Pre(n2_, P(PropA, PropB))));

            //Assert.Equal("MATCH (n1:Person)-->(n2:Person)", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Nested_NodeToNode_WithProp_Test

        #region Nested_NodeToNode_ReusedProp_Test

        [Fact]
        public void Nested_NodeToNode_ReusedProp_Test()
        {
            //CypherCommand cypher = P(p => P(PropA, PropB),
            //                        n1 => n2 => n2_ =>
            //                        N(n1, Person, p) ->
            //                        N(n2, Person, Pre(n2_, p)));

            //Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Nested_NodeToNode_ReusedProp_Test

        #region WhereExists_Test

        [Fact]
        public void WhereExists_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Exists(m => r => Match(N(n))-R[r, KNOWS]>N(m)
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

        // todo: {name: 'Alice', age: 38,
        //       address: {city: 'London', residential: true}}

        // TODO: FOREACH, DELETE, DETACH
        // TODO: UNION, UNION ALL
        // TODO: Auto WITH
        // TODO: RETURN & RETURN<T> with projection

        // TODO: AND, OR, AS, LIMIT, SKIP, ORDER BY, 
        // TODO: variable IS NULL
        // TODO: NOT exists(n.property
        // TODO: MATCH OnCreate OnMatch
        // TODO: [x IN list | x.prop]
        // TODO: [x IN list WHERE x.prop <> $value]
        // TODO: [x IN list WHERE x.prop <> $value | x.prop]
        // TODO: reduce(s = "", x IN list | s + x.prop)
        // TODO: all(x IN coll WHERE exists(x.property))
        // TODO: any(x IN coll WHERE exists(x.property))
        // TODO: none(x IN coll WHERE exists(x.property))
        // TODO: single(x IN coll WHERE exists(x.property))
        // TODO: CASE  WHEN ELSE

        // TODO:list[$idx] AS value,
        // TODO:list[$startIdx..$endIdx] AS slice

        // TODO: n.property STARTS WITH 'Tim' OR
        // TODO: n.property ENDS WITH 'n' OR
        // TODO: n.property CONTAINS 'goodie'
        // TODO: n.property =~ 'Tim.*'
        // TODO: n.property IN [$value1, $value2]'

        // TODO: exists(n.property),
        // TODO: coalesce,timestamp, id, toInteger, toFloat, toBoolean,
        // TODO: head, last, tail,
        // TODO: keys, properties, 
        // TODO: count, collect, sum, percentileDisc, stDev
        // TODO: length, nodes, relationships, 

        // TODO: abs, rand, sqrt, sign, 
        // TODO: sin, cos, tan, cot, asin, acos, atan, atan2, haversin
        // TODO: degrees, radians, pi, 
        // TODO: log10, log, exp, pi
        // TODO: toString, replace, substring, left, trim, toUpper, split, reverse, size

        // TODO: Spatial, date time / duration related
        // TODO: Constraint and Index

        // TODO: USER / ROLE MANAGEMENT and Privileges

        /* TODO:          
         CALL {
  MATCH (p:Person)-[:FRIEND_OF]->(other:Person) RETURN p, other
  UNION
  MATCH (p:Child)-[:CHILD_OF]->(other:Parent) RETURN p, other
}*/

    }
}

