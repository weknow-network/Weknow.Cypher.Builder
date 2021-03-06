using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Group", "Pattern")]
    [Trait("Segment", "Expression")]
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
                        "MERGE (n:Person { Id: $Id, eTag: $eTag })\r\n" +
                        "SET n += $map\r\n" +
                        "SET n.eTag = n.eTag + 1\r\n" +
                        "RETURN n.eTag", cypher.Query);
        }

        #endregion // ETag_Test
    }
}

