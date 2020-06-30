using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;


namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Reuse")]
    [Trait("Segment", "Expression")]
    public class ReuseExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ReuseExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region LazyReuse_Node_Test

        [Fact]
        public void LazyReuse_Node_Test()
        {
            var reusedPerson = Reuse(person => N(person, Person));
            var reusedAnimal = Reuse(animal => N(animal, Animal));

            CypherCommand cypher = _( r =>
                          Match(reusedPerson - R[r, LIKE] > reusedAnimal));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // LazyReuse_Node_Test

        #region CaptureProperties_Test

        [Fact]
        public void CaptureProperties_Test()
        {
            CypherCommand cypher = _(_ => P(PropA, PropB).Reuse()
                                         .By(p => n => Match(N(n, Person, p))));

            _outputHelper.WriteLine(cypher);
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

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // CaptureNodeAndProperties_Test

        #region Reuse_Node_Test

        [Fact]
        public void Reuse_Node_Test()
        {
            CypherCommand cypher = _(person => animal => 
                                     N(person, Person).Reuse(
                                     N(animal, Animal).Reuse())
                         .By(reusedPerson => reusedAnimal => r =>
                          Match(reusedPerson - R[r, LIKE] > reusedAnimal)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // Reuse_Node_Test

        #region Reuse_Plural_UNWIND_Test

        [Fact]
        public void Reuse_Plural_UNWIND_Test()
        {
            CypherCommand cypher = _(n =>
                          P(PropA, PropB).Reuse()
                         .By(p => items =>
                            Unwind(items, Match(N(n, Person, p)))
                         ));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Reuse_Plural_UNWIND_Test
    }
}

