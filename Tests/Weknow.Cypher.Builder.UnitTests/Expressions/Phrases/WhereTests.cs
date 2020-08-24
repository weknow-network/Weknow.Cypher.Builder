using System;
using System.Reflection.Metadata;

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
            var n = Variables.Create<Foo>();
            var PropA = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(n._.Name == "my-name"));

            _outputHelper.WriteLine(cypher.Dump());
            Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.Name = $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value, "my-name"));
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name = $p_1 / Where_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 / Where_Regex_Test

        [Fact]
        public void Where_Regex_Test()
        {
            var PropA = Parameters.Create();
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(Rgx(n._.Name == "my-name.*")));

            _outputHelper.WriteLine(cypher.Dump());
            Assert.Equal(
@"MATCH (n:Person { PropA: $PropA })
WHERE n.Name =~ $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value, "my-name.*"));
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 / Where_Regex_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 / Where_Regex_Prop_Test

        [Fact]
        public void Where_Regex_Prop_Test()
        {
            var PropA = Parameters.Create();
            var p = Parameters.Create<Foo>();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(Rgx(n._.Name == p._.Name)));

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
            var x_name = Parameters.Create();
            var p = Parameters.Create<Bar>();
            var n = Variables.Create<Bar>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { p._.Id }))
                                    .Where(n._.Date < p._.Date && Equals(n._.Name, x_name)));

            _outputHelper.WriteLine(cypher.Dump());

            string isStr = nameof(p._.Id);
            string dateStr = nameof(p._.Date);
            Assert.Contains(cypher.Parameters, p => p.Key == isStr);
            Assert.Contains(cypher.Parameters, p => p.Key == nameof(x_name));
            Assert.Contains(cypher.Parameters, p => p.Key == nameof(dateStr));
            Assert.Equal(
@"MATCH (n:Person { Id: $Id }), (m:Person)
            WHERE n.Date < $Date AND n.Name = $x_name", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB / Where_Parameter_Test

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB / Where_Parameter_Gen_Test

        [Fact]
        public void Where_Parameter_Gen_Test()
        {
            var Name = Parameters.Create<string>();
            var PropA = Parameters.Create();
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(n._.Name == Name));

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
            var PropA = Parameters.Create<Foo>();
            var n = Variables.Create<Foo>();
            var m = Variables.Create<Foo>();
            var r = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(Exists(() => Match(N(n) - R[r, KNOWS] > N(m))
                                            // using var.(exp) should format into var.exp 
                                            .Where(n._.Name == m._.Name))));

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
            var (n, m, r) = Variables.CreateMulti();
            var PropA = Parameters.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(Exists(() =>
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
            var PropA = Parameters.Create();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    // using var.(exp) should format into var.exp 
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
            var n = Variables.Create<Foo>();
            var PropA = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
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

        #region Where_GreatThan_Test

        [Fact]
        public void Where_GreatThan_Test()
        {
            var n = Variables.Create<Bar>();
            var PropA = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(n._.Date > DateTime.Now));

            _outputHelper.WriteLine(cypher.Dump());
            Assert.Equal(
                   "MATCH (n:Person { PropA: $PropA })\r\n" +
                   "WHERE n.Date > $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1");
        }

        #endregion // Where_GreatThan_Test

        #region Where_GreatOrEquals_Test

        [Fact]
        public void Where_GreatOrEquals_Test()
        {
            var n = Variables.Create<Bar>();
            var PropA = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { PropA }))
                                    .Where(n._.Date >= DateTime.Now));

            _outputHelper.WriteLine(cypher.Dump());
            Assert.Equal(
                   "MATCH (n:Person { PropA: $PropA })\r\n" +
                   "WHERE n.Date >= $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1");
        }

        #endregion // Where_GreatOrEquals_Test

        #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Where_Unwind_Test

        [Fact]
        public void Where_Unwind_Test()
        {
            var items = Parameters.Create();
            var (item, n) = Variables.CreateMulti<Foo, Foo>();

            CypherCommand cypher = _(() =>
                                    Unwind(items, item,
                                    Match(N(n, Person, new { (~item)._.Id }))
                                    .Where(n._.Name != item._.Name)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("UNWIND $items AS item\r\n" +
                            "MATCH (n:Person { Id: item.Id })\r\n" +
                            "WHERE n.Name <> item.Name", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Where_Unwind_Test

    }
}

