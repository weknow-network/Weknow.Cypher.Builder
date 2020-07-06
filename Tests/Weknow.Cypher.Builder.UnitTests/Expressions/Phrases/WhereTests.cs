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

        #region Where_Test

        [Fact]
        public void Where_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(n.As<Foo>().Name == "my-name"));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.Name = $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value,"my-name"));
        }

        #endregion // Where_Test

        #region Where_Parameter_Test

        [Fact]
        public void Where_Parameter_Test()
        {
            CypherCommand cypher = _(n => n_ =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(n.P(PropA, P_(n_, PropB))));

            _outputHelper.WriteLine(cypher.Dump());

            Assert.Contains(cypher.Parameters, p => p.Key == "n_PropB");
            Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.PropA = $PropA, n.PropB = $n_PropB", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
        }

        #endregion // Where_Parameter_Test

        #region WhereExists_Test

        [Fact]
        public void WhereExists_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Exists(m => r =>
                                        Match(N(n) - R[r, KNOWS] > N(m))
                                        .Where(n.As<Foo>().Name == m.As<Foo>().Name))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m)
WHERE n.Name = m.Name }", cypher.Query);
        }

        #endregion // WhereExists_Test

        #region WhereExists_Const_Test

        [Fact]
        public void WhereExists_Const_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Exists(m => r =>
                                        Match(N(n) - R[r, KNOWS] > N(m))
                                        .Where(true))));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m)
WHERE $p_1 }", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value, true));
        }

        #endregion // WhereExists_Const_Test
    }
}

