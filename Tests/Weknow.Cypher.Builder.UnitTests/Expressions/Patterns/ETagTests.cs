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
            CypherCommand cypher = _<Fellow>(n => map =>
                                        Merge(N(n, Person, P(n._.Id, n._.eTag)))
                                        .Set(+n, map.AsMap)
                                        .Set(n.P(n.Inc.eTag))
                                        .Return(n._.eTag));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                        "MERGE (n:Person { Id: $Id, eTag: $eTag })\r\n" +
                        "SET n += map\r\n" +
                        "SET n.eTag = n.eTag + 1\r\n" +
                        "RETURN n.eTag", cypher.Query);
        }

        #endregion // ETag_Test

        #region ETag_Test

        [Fact]
        public void ETag_Unwind_Test()
        {
            CypherCommand cypher = _<Fellow>(n => items => item =>
                                    Unwind(items, item,
                                        Merge(N(n, Person, item._(n._.Id, n._.eTag)))
                                        .Set(+n, item.AsMap)
                                        .Set(n.P(n.Inc.eTag))
                                        .Return(n._.eTag)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("UNWIND $items AS item\r\n" +
                        "MERGE (n:Person { Id: item.Id, eTag: item.eTag })\r\n" +
                        "SET n += item\r\n" +
                        "SET n.eTag = n.eTag + 1\r\n" +
                        "RETURN n.eTag", cypher.Query);
        }

        #endregion // ETag_Test
    }
}

