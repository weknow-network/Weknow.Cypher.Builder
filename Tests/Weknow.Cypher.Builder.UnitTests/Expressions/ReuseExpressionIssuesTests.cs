using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;


namespace Weknow.Cypher.Builder
{
    //    //[Trait("Segment", "Expression")]
    [Trait("Issues", "open")]
    public class ReuseExpressionIssuesTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ReuseExpressionIssuesTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Lazy_Reuse_Properties_Test

        [Fact]
        public void Lazy_Reuse_Properties_Test()
        {
            throw new NotImplementedException();

            //var p = Reuse(P(PropA, PropB));
            //var reusedPerson = Reuse(person => N(person, p));

            //CypherCommand cypher = _(() =>
            //             Match(reusedPerson));

            //_outputHelper.WriteLine(cypher);
			// Assert.Equal("MATCH (person:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Lazy_Reuse_Properties_Test

        #region Reuse_Unordered_Test

        [Fact]
        public void Reuse_Unordered_Test()
        {
            CypherCommand cypher = _(n => P(PropA, PropB).AsReuse(
                                          N(n, Person).AsReuse())
                                     .By(p => n => n1 =>
                                      Match(N(n1, Person, p) - n)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })--(n:Person)", cypher.Query);
            throw new InvalidOperationException("disable the option of chaining Reuse in a row because of the backward ordering (confusion)");
        }

        #endregion // Reuse_Unordered_Test
    }
}

