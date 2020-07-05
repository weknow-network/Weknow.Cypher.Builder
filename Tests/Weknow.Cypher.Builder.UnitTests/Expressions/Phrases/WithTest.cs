using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "With")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class WithTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public WithTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Explicit_With_Test

        [Fact]
        public void Explicit_With_Test()
        {
            CypherCommand cypher = _(items => map => n => i =>
                        Unwind(items, map,
                        Match(N(i, Person, n.P(Id)))
                        .With(
                        Merge(N(n, Person, n.P(Id)))
                        .OnCreateSet(n, map.AsMap)
                        .OnMatchSet(+n, map.AsMap)
                        .Return(n))),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                "UNWIND $items AS map\r\n" +
                "MATCH (i:PERSON { Id: map.Id })\r\n" +
                "WITH MERGE (n:PERSON { Id: map.Id })\r\n\t" +
                    "ON CREATE SET n = map\r\n\t" +
                    "ON MATCH SET n += map\r\n" +
                "RETURN n", cypher.Query);
        }

        #endregion // Explicit_With_Test
    }
}

