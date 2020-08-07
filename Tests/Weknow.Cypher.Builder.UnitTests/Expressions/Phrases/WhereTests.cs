using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
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

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name = $p_1 / Where_Test

        [Fact]
        public void Where_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(n.OfType<Foo>().Name == "my-name"));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.Name = $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value,"my-name"));
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name = $p_1 / Where_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 / Where_Regex_Test

        [Fact]
        public void Where_Regex_Test()
        {
            CypherCommand cypher = _<Foo>(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Rgx(n._.Name == "my-name.*")));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.Name =~ $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value,"my-name.*"));
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 / Where_Regex_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 / Where_Regex_Prop_Test

        [Fact]
        public void Where_Regex_Prop_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Rgx(n.P(Name))));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.Name =~ $Name", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "Name");
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 / Where_Regex_Prop_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB / Where_Parameter_Test

        [Fact]
        public void Where_Parameter_Test()
        {
            IParameter? PropA = null, n_PropB = null;
            CypherCommand cypher = _(n => n_ =>
                                    Match(N(n, Person, new { PropA = PropA }))
                                    .Where(n.P(new { PropA, PropB = n_PropB })));

            _outputHelper.WriteLine(cypher.Dump());

            Assert.Contains(cypher.Parameters, p => p.Key == "n_PropB");
            Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.PropA = $PropA, n.PropB = $n_PropB", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB / Where_Parameter_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB / Where_Parameter_Gen_Test

        [Fact]
        public void Where_Parameter_Gen_Test()
        {
            IParameter? Name = null, PropA = null;
            CypherCommand cypher = _<Foo>(n =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(n.P(new { Name = Name })));

            _outputHelper.WriteLine(cypher.Dump());

            Assert.Contains(cypher.Parameters, p => p.Key == "Name");
            Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.Name = $Name", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB / Where_Parameter_Gen_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m) WHERE n.Name = m.Name } / WhereExists_Test

        [Fact]
        public void WhereExists_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(Exists(m => r =>
                                        Match(N(n) - R[r, KNOWS] > N(m))
                                        .Where(n.OfType<Foo>().Name == m.OfType<Foo>().Name))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m)
WHERE n.Name = m.Name }", cypher.Query);
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m) WHERE n.Name = m.Name } / WhereExists_Test

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

        #region Where_Multi_Test

        [Fact]
        public void Where_Multi_Test()
        {
            CypherCommand cypher = _<Foo>(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(n._.Name == "my-name" &&
                                           n._.PropA == "done" ||
                                           n._.PropB == "skip"));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                    "MATCH (n:Person { PropA: $PropA })\r\n" +
                    "WHERE n.Name = $p_1 AND " +
                            "n.PropA = $p_2 OR " +
                            "n.PropB = $p_3", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value, "my-name"));
            Assert.Contains(cypher.Parameters, p => p.Key == "p_2" && Equals(p.Value, "done"));
            Assert.Contains(cypher.Parameters, p => p.Key == "p_3" && Equals(p.Value, "skip"));
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name = $p_1 AND  / Where_Multi_Test

        #region Where_NOT_Test

        [Fact]
        public void Where_NOT_Test()
        {
            CypherCommand cypher = _<Foo>(n =>
                                    Match(N(n, Person, P(PropA)))
                                    .Where(n._.Name != "my-name"));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                    "MATCH (n:Person { PropA: $PropA })\r\n" +
                    "WHERE n.Name <> $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value, "my-name"));
        }

        #endregion // Where_NOT_Test

        #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Where_Unwind_Test

        [Fact]
        public void Where_Unwind_Test()
        {
            CypherCommand cypher = _<Foo, Foo>(item => n => items =>
                                    Unwind(items, item,
                                    Match(N(n, Person, item._(Id)))
                                    .Where(n._.Name != item._.Name)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("UNWIND $items AS item\r\n" +
                            "MATCH (n:Person { Id: item.Id })\r\n" +
                            "WHERE n.Name <> item.Name", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Where_Unwind_Test

    }
}

