using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;


namespace Weknow.Cypher.Builder
{
    //    //[Trait("Segment", "Expression")]
    [Trait("Issues", "open")]
    public class ReuseIssuesTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ReuseIssuesTests(ITestOutputHelper outputHelper)
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
    }
}

