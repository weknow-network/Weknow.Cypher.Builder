using System;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

// TODO: Duplicate class Pattern to FullNamePattern for naming standard

// TODO: parameter factory injection for enabling to work with Neo4jParameters (Neo4jMapper)
//       Mimic Neo4jMappaer WithEntity, WithEntities + integration test
//       validate flat entity (in deep complex type throw exception with recommendation for best practice)

namespace Weknow.Cypher.Builder
{
    public class ReuseExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ReuseExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region CaptureProperties_Test

        [Fact]
        public void CaptureProperties_Test()
        {
            CypherCommand cypher = _(_ => P(PropA, PropB).Reuse()
                                         .By(p => n => Match(N(n, Person, p))));

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

        #region Reuse_Node_Test

        [Fact]
        public void Reuse_Node_Test()
        {
            CypherCommand cypher = _(person => animal => 
                                     N(person, Person).Reuse(
                                     N(animal, Animal).Reuse())
                         .By(reusedPerson => reusedAnimal => r =>
                          Match(reusedPerson - R[r, LIKE] > reusedAnimal)));

            Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // Reuse_Node_Test

        #region Lazy_Reuse_Properties_Test

        [Fact]
        public void Lazy_Reuse_Properties_Test()
        {
            throw new NotFiniteNumberException();

            //var p = Reuse(P(PropA, PropB));
            //var reusedPerson = Reuse(person => N(person, p));

            //CypherCommand cypher = _(() =>
            //             Match(reusedPerson));

            //Assert.Equal("MATCH (person:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Lazy_Reuse_Properties_Test

        #region LazyReuse_Node_Test

        [Fact]
        public void LazyReuse_Node_Test()
        {
            var reusedPerson = Reuse(person => N(person, Person));
            var reusedAnimal = Reuse(animal => N(animal, Animal));

            CypherCommand cypher = _( r =>
                          Match(reusedPerson - R[r, LIKE] > reusedAnimal));

            Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // LazyReuse_Node_Test

        #region Reuse_Unordered_Test

        [Fact]
        public void Reuse_Unordered_Test()
        {
            CypherCommand cypher = _(n => P(PropA, PropB).Reuse(
                                          N(n, Person).Reuse())
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
    }
}

