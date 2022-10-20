using System;
using System.Security.Cryptography;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;
using static System.Environment;


namespace Weknow.Cypher.Builder
{
        [Trait("Segment", "Expression")]
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

        #region MATCH (n) RETURN labels(n) / Timestamp_Test

        [Fact]
        public void Timestamp_Test()
        {
            var (n, m) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Match(N(n))
                                    .Set(n, new { Date = Timestamp() })
                                    .Merge(N(m))
                                    .OnCreateSet(n, new { CreationDate = Timestamp() })
                                    .OnMatchSet(n, new { ModifiedDate = Timestamp() })
                                    .Return(n, Timestamp().As("date")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                $"SET n.Date = timestamp(){NewLine}" +
                $"MERGE (m){NewLine}" +
                $"\tON CREATE SET n.CreationDate = timestamp(){NewLine}" +
                $"\tON MATCH SET n.ModifiedDate = timestamp(){NewLine}" +
                "RETURN n, timestamp() AS date", cypher.Query);
        }

        #endregion // MATCH (n) RETURN labels(n) / Timestamp_Test

        #region MATCH (n1)-[r]->(n2) ... / Functions_Test

        [Fact]
        public void Functions_Test()
        {
            var (n1, r, n2) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Match(N(n1)-R[r]>N(n2))
                                    .Return(Timestamp().As("time"),
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

        #region MATCH (n1)-[r]->(n2) ... / Aggregation_Test

        [Fact]
        public void Aggregation_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(
                                        Sum(n._.PropA).As("sum"), 
                                        Min(n._.PropA).As("min"),
                                        Max(n._.PropA).As("max"),
                                        Avg(n._.PropA).As("avg")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n){NewLine}" +
                         "RETURN " +
                                "sum(n.PropA) AS sum, " +
                                "min(n.PropA) AS min, " +
                                "max(n.PropA) AS max, " +
                                "avg(n.PropA) AS avg", cypher.Query);
        }

        #endregion // MATCH (n1)-[r]->(n2) ..., startNode(r), endNode(r) / Aggregation_Test

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

