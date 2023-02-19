using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Pattern")]

    public class ETagTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ETagTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region ETag_Test

        [Fact]
        public void ETag_Test()
        {
            var map = Parameters.Create();
            var p = Parameters.Create<Fellow>();
            var n = Variables.Create<Fellow>();

            CypherCommand cypher = _(() =>
                                        Merge(N(n, Person, new { p._.Id, p._.eTag }))
                                        .SetPlus(n, map)
                                        .Set(n, new { n.Inc.eTag })
                                        .Return(n._.eTag));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                       $"MERGE (n:Person {{ Id: $Id, eTag: $eTag }}){NewLine}" +
                       $"SET n += $map{NewLine}" +
                       $"SET n = {{ eTag: n.eTag + 1 }}{NewLine}" +
                       "RETURN n.eTag", cypher.Query);
        }

        #endregion // ETag_Test
    }
}

