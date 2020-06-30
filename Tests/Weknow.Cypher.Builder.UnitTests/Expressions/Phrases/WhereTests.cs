using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Where")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class WhereTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public WhereTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region WhereExists_Test

        [Fact]
        public void WhereExists_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Exists(m => r =>
                                        Match(N(n) - R[r, KNOWS] > N(m))
                                        .Where(n.As<Foo>().Name == m.As<Foo>().Name))));

            Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m)
WHERE n.Name = m.Name }", cypher.Query);
        }

        #endregion // WhereExists_Test
    }
}

