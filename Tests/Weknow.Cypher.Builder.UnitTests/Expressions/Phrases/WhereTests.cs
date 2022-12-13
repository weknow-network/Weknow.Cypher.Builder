using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]

    public class WhereTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public WhereTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name = $p_1 

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

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name = $p_1 

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 

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

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 

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

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.Name =~ $p_1 

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB 

        [Fact]
        public void Where_Parameter_Test()
        {
            var x_name = Parameters.Create<string>();
            var p = Parameters.Create<Bar>();
            var n = Variables.Create<Bar>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { p._.Id }))
                                    .Where(n._.Date < p._.Date && n._.Name == x_name));

            _outputHelper.WriteLine(cypher.Dump());

            string isStr = nameof(p._.Id);
            string dateStr = nameof(p._.Date);
            Assert.Contains(cypher.Parameters, p => p.Key == isStr);
            Assert.Contains(cypher.Parameters, p => p.Key == nameof(x_name));
            Assert.Contains(cypher.Parameters, p => p.Key == dateStr);
            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
WHERE n.Date < $Date AND n.Name = $x_name", cypher.Query);
        }

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB 

        #region MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB 

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

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE n.PropA = $PropA, n.PropB = $n_PropB

        #region MATCH (n:Person { PropA: $PropA }) WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m) WHERE n.Name = m.Name } 

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

        #endregion // MATCH (n:Person { PropA: $PropA }) WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m) WHERE n.Name = m.Name }

        #region WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m)

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

        #endregion // WHERE EXISTS { MATCH (n)-[r:KNOWS]->(m)

        #region WHERE n.Name = 'my-name' AND n.PropA = 'done' OR n.PropB = 'skip'

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
                   $"MATCH (n:Person {{ PropA: $PropA }}){NewLine}" +
                   "WHERE n.Name = $p_1 AND " +
                           "n.PropA = $p_2 OR " +
                           "n.PropB = $p_3", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value, "my-name"));
            Assert.Contains(cypher.Parameters, p => p.Key == "p_2" && Equals(p.Value, "done"));
            Assert.Contains(cypher.Parameters, p => p.Key == "p_3" && Equals(p.Value, "skip"));
        }

        #endregion // WHERE n.Name = 'my-name' AND n.PropA = 'done' OR n.PropB = 'skip'

        #region WHERE n.Name <> 'my-name'

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
                   $"MATCH (n:Person {{ PropA: $PropA }}){NewLine}" +
                   "WHERE n.Name <> $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1" && Equals(p.Value, "my-name"));
        }

        #endregion // WHERE n.Name <> 'my-name'

        #region HERE n.Date > DateTime.Now

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
                   $"MATCH (n:Person {{ PropA: $PropA }}){NewLine}" +
                   "WHERE n.Date > $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1");
        }

        #endregion // HERE n.Date > DateTime.Now

        #region WHERE n.Date >= DateTime.Now

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
                   $"MATCH (n:Person {{ PropA: $PropA }}){NewLine}" +
                   "WHERE n.Date >= $p_1", cypher.Query);
            Assert.NotEmpty(cypher.Parameters);
            Assert.Contains(cypher.Parameters, p => p.Key == "PropA");
            Assert.Contains(cypher.Parameters, p => p.Key == "p_1");
        }

        #endregion // WHERE n.Date >= DateTime.Now

        #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })

        [Fact]
        public void Where_Unwind_Test()
        {
            var items = Parameters.Create();
            var (item, n) = Variables.CreateMulti<Foo, Foo>();

            CypherCommand cypher = _(() =>
                                    Unwind(items, item,
                                    Match(N(n, Person, new { item.__.Id }))
                                    .Where(n._.Name != item._.Name)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"UNWIND $items AS item{NewLine}" +
                            $"MATCH (n:Person {{ Id: item.Id }}){NewLine}" +
                            "WHERE n.Name <> item.Name", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })

        #region WHERE n.Name IS NOT NULL 

        [Fact]
        public void WhereNotIsNull_Test()
        {
            var n = Variables.Create<Foo>();
            var PropA = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person))
                                    .Where(n._.Name != null));

            _outputHelper.WriteLine(cypher.Dump());
            Assert.Equal(
@"MATCH (n:Person)
WHERE n.Name IS NOT NULL ", cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // WHERE n.Name IS NOT NULL  
    }
}

