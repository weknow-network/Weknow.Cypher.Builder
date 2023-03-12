using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.CypherExtensions;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;


namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]

    public class FunctionsExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public FunctionsExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n) SET n:Person:Animal / Label_Test

        [Fact]
        public void Label_Test()
        {
            var n = Variables.Create();
            CypherCommand cypher = _(() => Match(N(n))
                                    .Set(n, Person & Animal));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                "SET n:Person:Animal", cypher.Query);
        }

        #endregion // MATCH (n) SET n:Person:Animal / Label_Test

        #region MATCH (n) RETURN labels(n) / Labels_Test

        [Fact]
        public void Labels_Test()
        {
            var n = Variables.Create();
            CypherCommand cypher = _(() => Match(N(n))
                                    .Return(n.Labels()));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                "RETURN labels(n)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN labels(n) / Labels_Test

        #region timestamp()

        [Fact]
        public void Timestamp_Test()
        {
            var (n, m) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Match(N(n, new { Id = 1 }))
                                    .Set(n, new { Date = Fn.Cal.Timestamp(), Today = Fn.Cal.Date() })
                                    .Merge(N(m))
                                    .OnCreateSet(n, new { CreationDate = Fn.Cal.Timestamp() })
                                    .OnMatchSet(n, new { ModifiedDate = Fn.Cal.Timestamp() })
                                    .Return(n, Fn.Cal.Timestamp().As("date")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n {{ Id: $p_0 }}){NewLine}" +
                $"SET n = {{ Date: timestamp(), Today: date() }}{NewLine}" +
                $"MERGE (m){NewLine}" +
                $"\tON CREATE SET n = {{ CreationDate: timestamp() }}{NewLine}" +
                $"\tON MATCH SET n = {{ ModifiedDate: timestamp() }}{NewLine}" +
                "RETURN n, timestamp() AS date", cypher.Query);
        }

        #endregion // timestamp()

        #region MATCH (n1)-[r]->(n2) ... / Functions_Test

        [Fact]
        public void Functions_Test()
        {
            var (n1, r, n2) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Match(N(n1) - R[r] > N(n2))
                                    .Return(Fn.Cal.Timestamp().As("time"),
                                            n1.Labels().As("label"),
                                            n1.Id(),
                                            r.Type(),
                                            r.StartNode(),
                                            r.EndNode(),
                                            n2.Count(),
                                            n1.CountDistinct().As("distCount")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n1)-[r]->(n2){NewLine}" +
                         "RETURN " +
                                "timestamp() AS time, " +
                                "labels(n1) AS label, " +
                                "id(n1), " +
                                "type(r), " +
                                "startNode(r), " +
                                "endNode(r), " +
                                "count(n2), " +
                                "count(DISTINCT n1) AS distCount", cypher.Query);
        }

        #endregion // MATCH (n1)-[r]->(n2) ... / Functions_Test

        #region MATCH (n) RETURN count(n.PropA) AS count

        [Fact]
        public void Aggregation_Count_Test()
        {
            var n = Variables.Create<Foo>();
            var count = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Ag.Count(n._.PropA).As(count)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "count(n.PropA) AS count", cypher.Query);
        }

        #endregion // MATCH (n) RETURN count(n.PropA) AS count

        #region MATCH (n) RETURN count(DISTINCT n.PropA) AS count

        [Fact]
        public void Aggregation_CountDistinct_Test()
        {
            var n = Variables.Create<Foo>();
            var count = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Ag.CountDistinct(n._.PropA).As(count)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "count(DISTINCT n.PropA) AS count", cypher.Query);
        }

        #endregion // MATCH (n) RETURN count(DISTINCT n.PropA) AS count

        #region MATCH (n) RETURN collect(n.PropA) AS collect

        [Fact]
        public void Aggregation_Collect_Test()
        {
            var n = Variables.Create<Foo>();
            var collect = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Ag.Collect(n._.PropA).As(collect)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "collect(n.PropA) AS collect", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(n.PropA) AS collect

        #region MATCH (n) RETURN collect(DISTINCT n.PropA) AS collect

        [Fact]
        public void Aggregation_CollectDistinct_Test()
        {
            var n = Variables.Create<Foo>();
            var collect = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Ag.CollectDistinct(n._.PropA).As(collect)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "collect(DISTINCT n.PropA) AS collect", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(DISTINCT n.PropA) AS count

        #region MATCH (n) RETURN avg(DISTINCT n.PropA) AS avg

        [Fact]
        public void Aggregation_Short_DISTINCT_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Ag.AvgDistinct(n._.PropA).As("avg")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "avg(DISTINCT n.PropA) AS avg", cypher.Query);
        }

        #endregion // MATCH (n) RETURN avg(DISTINCT n.PropA) AS avg

        #region MATCH (n) RETURN avg(n.PropA) AS avg

        [Fact]
        public void Aggregation_Short_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Ag.Avg(n._.PropA).As("avg")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "avg(n.PropA) AS avg", cypher.Query);
        }

        #endregion // MATCH (n) RETURN avg(n.PropA) AS avg

        #region MATCH (n) RETURN sum(n.PropA) AS sum, min(n.PropA) AS min 

        [Fact]
        public void Aggregation_2_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Ag.Avg(n._.Version).As(n._.Version),
                                        Fn.Ag.Sum(n._.PropA).As("sum"),
                                        Fn.Ag.Min(n._.PropA).As("min")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "avg(n.Version) AS Version, " +
                                "sum(n.PropA) AS sum, " +
                                "min(n.PropA) AS min", cypher.Query);
        }

        #endregion // MATCH (n) RETURN sum(n.PropA) AS sum, min(n.PropA) AS min 

        #region MATCH (n) RETURN sum(n.PropA) AS sum, min(n.PropA) AS min ...

        [Fact]
        public void Aggregation_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Aggregation.Sum(n._.PropA).As("sum"),
                                        Fn.Ag.Min(n._.PropA).As("min"),
                                        Fn.Ag.Max(n._.PropA).As("max"),
                                        Fn.Ag.Avg(n._.PropA).As("avg")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "sum(n.PropA) AS sum, " +
                                "min(n.PropA) AS min, " +
                                "max(n.PropA) AS max, " +
                                "avg(n.PropA) AS avg", cypher.Query);
        }

        #endregion // MATCH (n) RETURN sum(n.PropA) AS sum, min(n.PropA) AS min ...

        #region MATCH (n) RETURN sum(DISTINCT n.PropA) AS sum, min(DISTINCT n.PropA) AS min ...

        [Fact]
        public void Aggregation_DISTINCT_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Fn.Aggregation.SumDistinct(n._.PropA).As("sum"),
                                        Fn.Ag.MinDistinct(n._.PropA).As("min"),
                                        Fn.Ag.MaxDistinct(n._.PropA).As("max"),
                                        Fn.Ag.AvgDistinct(n._.PropA).As("avg")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "sum(DISTINCT n.PropA) AS sum, " +
                                "min(DISTINCT n.PropA) AS min, " +
                                "max(DISTINCT n.PropA) AS max, " +
                                "avg(DISTINCT n.PropA) AS avg", cypher.Query);
        }

        #endregion // MATCH (n) RETURN sum(n.PropA) AS sum, min(n.PropA) AS min ...

        #region MATCH (n) RETURN coalesce(n.PropA, n.PropB)

        [Fact]
        public void Coalesce_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(n.Coalesce(n)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN coalesce(n, n)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN coalesce(n.PropA, n.PropB)

        #region MATCH (n) RETURN coalesce(n.PropA, n.PropB)

        [Fact]
        public void Coalesce_Of_T_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(n._.PropA.Coalesce(n._.PropB)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN coalesce(n.PropA, n.PropB)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN coalesce(n.PropA, n.PropB)

        #region MATCH (n) RETURN collect(n) / Collect_Test

        [Fact]
        public void Collect_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(n.Collect()));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN collect(n)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(n) / Collect_Test

        #region MATCH (n) RETURN collect(n.PropA) / Collect_Prop_Test

        [Fact]
        public void Collect_Prop_Test()
        {
            var n = Variables.Create<Foo>();
            var id = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .With(n._.Id.As(id))
                                    .Return(id.Collect()));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         $"WITH n.Id AS id{NewLine}" +
                         "RETURN collect(id)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(n.PropA) / Collect_Prop_Test

        #region MATCH (n) RETURN collect(n.PropA)

        [Fact]
        public void Collect_Var_Prop_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(Collect(n._.PropA)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN collect(n.PropA)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(n.PropA)

        #region MATCH (n) RETURN collect(n.PropA) / Collect_PropT_Test

        [Fact]
        public void Collect_PropT_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(Collect(n._.Id)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN collect(n.Id)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(n.PropA) / Collect_PropT_Test

        #region MATCH (n) RETURN collect(DISTINCT n) / CollectDistinct_Test

        [Fact]
        public void CollectDistinct_Test()
        {
            var n = Variables.Create();

            CypherCommand cypher = _(n =>
                                    Match(N(n))
                                    .Return(n.CollectDistinct()));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN collect(DISTINCT n)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(DISTINCT n) / CollectDistinct_Test

        #region MATCH (n) RETURN collect(DISTINCT n.PropA) / CollectDistinct_Prop_Test

        [Fact]
        public void CollectDistinct_Prop_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(CollectDistinct(n._.PropA)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN collect(DISTINCT n.PropA)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(DISTINCT n.PropA) / CollectDistinct_Prop_Test

        #region MATCH (n) RETURN collect(DISTINCT n.PropA) / CollectDistinct_PropT_Test

        [Fact]
        public void CollectDistinct_PropT_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(CollectDistinct(n._.PropA)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN collect(DISTINCT n.PropA)", cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(DISTINCT n.PropA) / CollectDistinct_PropT_Test
    }
}

