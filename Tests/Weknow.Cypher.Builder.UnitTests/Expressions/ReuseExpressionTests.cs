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
            IPattern reusedPerson = Reuse(person => N(person, Person));
            IPattern reusedAnimal = Reuse(animal => N(animal, Animal));

            CypherCommand cypher = _( r =>
                          Match(reusedPerson - R[r, LIKE] > reusedAnimal));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // LazyReuse_Node_Test

        #region LazyReuse_Overloads_Test

        [Fact]
        public void LazyReuse_Overloads_Test()
        {
            IPattern? pattern1 = Reuse(n => N(n, Person, P_(n, PropA)));
            IPattern? pattern2 = Reuse(n => n_ => N(n, Person, P_(n_, PropA)));
            IPattern? pattern3 = Reuse(n => n1_ => n2_ => N(n, Person, P_(n1_, PropA), P_(n2_, PropB)));
            IPattern? pattern4 = Reuse(n => n1_ => n2_ => n3_ => N(n, Person, P_(n1_, Id), P_(n2_, PropA), P_(n3_, PropB)));
            IPattern? pattern5 = Reuse(n => n1_ => n2_ => n3_ => n4_ => N(n, Person, P_(n1_, Id), P_(n2_, PropA), P_(n3_, PropB), P_(n4_, PropC)));

            string? cypher1 = pattern1?.ToString();
            string? cypher2 = pattern2?.ToString();
            string? cypher3 = pattern3?.ToString();
            string? cypher4 = pattern4?.ToString();
            string? cypher5 = pattern5?.ToString();

            _outputHelper.WriteLine(cypher1);
            _outputHelper.WriteLine(cypher2);
            _outputHelper.WriteLine(cypher3);
            _outputHelper.WriteLine(cypher4);
            _outputHelper.WriteLine(cypher5);

            Assert.Equal("(n:Person { PropA: $nPropA })", cypher1);
            Assert.Equal("(n:Person { PropA: $n_PropA })", cypher2);
            Assert.Equal("(n:Person { PropA: $n1_PropA, PropB: $n2_PropB })", cypher3);
            Assert.Equal("(n:Person { Id: $n1_Id, PropA: $n2_PropA, PropB: $n3_PropB })", cypher4);
            Assert.Equal("(n:Person { Id: $n1_Id, PropA: $n2_PropA, PropB: $n3_PropB, PropC: $n4_PropC })", cypher5);
        }

        #endregion // LazyReuse_Overloads_Test

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

