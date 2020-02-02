using System;
using System.Linq.Expressions;
using Xunit;
using static Weknow.Cypher.Builder.Pattern;
using static Weknow.Cypher.Builder.Schema;

// TODO: replace the Pattern P with Cypher C or with _ in order to differentiate it form Properties P
namespace Weknow.Cypher.Builder
{
    public class ExpressionTests
    {
        #region ComplexExpression_Test

        [Fact]
        public void ComplexExpression_Test()
        {
            CypherCommand cypher = P(a => r1 => b => r2 => c =>
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
            CypherCommand cypher = P(n => N(n, Person, P(p))); 

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // CaptureProperties_Test

        #region Properties_Test

        [Fact]
        public void Properties_Test()
        {
            CypherCommand cypher = P(n => N(n, Person, P(PropA, PropB))); 

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Test

        #region Properties_OfT_DefaultLabel_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_Test()
        {
            CypherCommand cypher = P(n => N<Foo>(n, n => P(n.PropA, n.PropB))); 

            Assert.Equal("(n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultLabel_Test

        #region Properties_OfT_DefaultAndAdditionLabel_Test

        [Fact]
        public void Properties_OfT_DefaultAndAdditionLabel_Test()
        {
            CypherCommand cypher = P(n => N<Foo>(n, Person, n => P(n.PropA, n.PropB))); 

            Assert.Equal("(n:Foo:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_Test

        // TODO: make it less repetitive P<Foo>(n => (n.PropA, n.PropB)) with value tuple instead of P<Foo>(n => P(n.PropA, n.PropB))
        #region Properties_OfT_DefaultAndAdditionLabel_Test

        [Fact]
        public void Properties_OfT_Test()
        {
            CypherCommand cypher = P(n => N(n, Person, P<Foo>(n => P(n.PropA, n.PropB)))); 

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_Test

        #region Properties_WithPrefix_Test

        [Fact]
        public void Properties_WithPrefix_Test()
        {
            CypherCommand cypher = P(n1 => n2 => n2_ => 
                                    N(n1, Person, P(PropA, PropB)) -
                                    R[n1, KNOWS] > 
                                    N(n2, Person, Pre(n2_, P(PropA, PropB)))); 

            Assert.Equal("(n1:Person { PropA: $PropA, PropB: $PropB })-" +
                         "[n1:KNOWS]->" +
                         "(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // Properties_WithPrefix_Test

        #region Properties_Convention_Test

        [Fact]
        public void Properties_Convention_WithDefaultLabel_Test()
        {
            CypherCommand cypher = P(n => N<Foo>(n, Convention(name => name.StartsWith("Prop"))));

            Assert.Equal("(n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_Test

        #region Properties_Convention_Test

        [Fact]
        public void Properties_Convention_Test()
        {
            CypherCommand cypher = P(n => N(n, Person , Convention<Foo>(name => name.StartsWith("Prop")))); 

            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_Test

        #region Unwind_WithPropConvention_Test

        [Fact]
        public void Unwind_WithPropConvention_Test()
        {
            CypherCommand cypher = P(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, Convention<Foo>(name => name.StartsWith("Prop"))))));

            Assert.Equal(@"UNWIND items as item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_WithPropConvention_Test

        #region Unwind_Test

        [Fact]
        public void Unwind_Test()
        {
            CypherCommand cypher = P(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(PropA, PropB)))));

            Assert.Equal(@"UNWIND $items as item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_Test

        #region Unwind_Entities_Test

        [Fact]
        public void Unwind_Entities_Test()
        {
//            CypherCommand cypher = P(items => item => n =>
//                                    Unwind(items, item,
//                                    Match(N(n, Person, P(Id))))
//                                    .SetEntity(item, EntitySetBehavior.Update));

//            Assert.Equal(@"UNWIND $items AS item
//MATCH (n:Person {Id: item.Id}) // NO $ SIGN
//SET n += item", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Unwind_Entities_Test

        #region Concurrency_Pattern_Test

        [Fact]
        public void Concurrency_Pattern_Test()
        {
            //CypherCommand cypher = P(n =>
            //                        Match(N(n, Person, P(PropA, PropB, Concurrency))),
            //                        SET(eTag(n, Concurrency));

            //Assert.Equal(@"
            //MATCH (n:Person { PropA: $PropA, PropB: $PropB, Concurrency: $Concurrency })
            //ON CREATE SET n.Concurrency = 1
            //ON MATCH SET n.Concurrency = n.Concurrency + 1", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Concurrency_Pattern_Test

        #region Unwind_Concurrency_Pattern_Test

        [Fact]
        public void Unwind_Concurrency_Pattern_Test()
        {
            //CypherCommand cypher = P(items => item => n =>
            //                        Unwind(items, item,
            //                        Match(N(n, Person, P(PropA, PropB, Concurrency))),
            //                        SET(eTag(n, Concurrency)));

            //Assert.Equal(@"UNWIND $items as item
            //MATCH (n:Person { PropA: item.PropA, PropB: item.PropB, Concurrency: $Concurrency })
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
            //                        Match(N(n1, Person))->N(n2 Person));

            //Assert.Equal("MATCH (n1:Person)-->(n2:Person)", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // NodeToNode_Test

        #region Nested_NodeToNode_WithProp_Test

        [Fact]
        public void Nested_NodeToNode_WithProp_Test()
        {
            CypherCommand cypher = P(n1 => n2 => n2_ =>
                                    N(n1, Person, P(PropA, PropB)) ->
                                    N(n2, Person, Pre(n2_, P(PropA, PropB))));

            Assert.Equal("MATCH (n1:Person)-->(n2:Person)", cypher.Query);
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
//            CypherCommand cypher = P(n => m =>
//                                    Match(N(n, Person, P(PropA)))
//                                    .WhereExists<Foo>(Match(N(n))->N(n)
//                                    .Where(n.Name = m.Name)));

//            Assert.Equal(
//@"WHERE EXISTS {
//  MATCH (n)-->(m) WHERE n.Name = m.Name
//}", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // WhereExists_Test

        // todo: {name: 'Alice', age: 38,
        //       address: {city: 'London', residential: true}
}
    }
}
