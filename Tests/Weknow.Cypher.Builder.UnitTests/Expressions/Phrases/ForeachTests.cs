using Weknow.CypherBuilder.Declarations;

using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]

    public class ForeachTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ForeachTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region FOREACH (item IN $items | SET item.Version = 1)

        [Fact]
        public void Foreach_Parameters_Test()
        {
            ParameterDeclaration items = Parameters.Create();
            VariableDeclaration item = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Foreach(items, item => Set(item, new { Version = 1 }))
                                    .Foreach(items, item => Set(item, new { Version = 2 })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"FOREACH (item IN $items |{NewLine}\t" +
                $"SET item = {{ Version: $p_1 }}){NewLine}" +
                $"FOREACH (item IN $items |{NewLine}\t" +
                "SET item = { Version: $p_2 })",
                cypher.Query);
            Assert.Null(cypher.Parameters["items"]);
            Assert.Equal(1, cypher.Parameters["p_1"]);
            Assert.Equal(2, cypher.Parameters["p_2"]);
            Assert.Equal(3, cypher.Parameters.Count);
        }

        #endregion // FOREACH (item IN $items | SET item.Version = 1)

        #region FOREACH (item IN $items | SET item.Version = 1)

        [Fact]
        public void Foreach_Parameters_Obsolete_Test()
        {
            var items = Parameters.Create();
            var item = Variables.Create<Foo>();

#pragma warning disable CS0618 // Type or member is obsolete
            CypherCommand cypher = _(() =>
                                    Foreach(item, items, Set(item, new { Version = 1 }))
                                    .Foreach(item, items, Set(item, new { Version = 2 })));
#pragma warning restore CS0618 // Type or member is obsolete

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"FOREACH (item IN $items |{NewLine}\t" +
                $"SET item = {{ Version: $p_1 }}){NewLine}" +
                $"FOREACH (item IN $items |{NewLine}\t" +
                "SET item = { Version: $p_2 })",
                cypher.Query);
            Assert.Null(cypher.Parameters["items"]);
            Assert.Equal(1, cypher.Parameters["p_1"]);
            Assert.Equal(2, cypher.Parameters["p_2"]);
            Assert.Equal(3, cypher.Parameters.Count);
        }

        #endregion // FOREACH (item IN $items | SET item.Version = 1)

        #region FOREACH (item IN items | SET item.Version = 1)

        [Fact]
        public void Foreach_Variables_Test()
        {
            var items = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Foreach(items, item => Set(item.__.Version, 1)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"FOREACH (item IN items |{NewLine}\t" +
                "SET item.Version = $p_0)",
                cypher.Query);
            Assert.Equal(1, cypher.Parameters["p_0"]);
            Assert.Equal(1, cypher.Parameters.Count);
        }

        #endregion // FOREACH (item IN items | SET item.Version = 1)

        #region FOREACH (item IN [1,2,3] | SET n.Value = item)

        [Fact] // (Skip = "Not implemented")]
        public void Foreach_Array_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Foreach(new[] { 1, 2, 3 }, item => Set(n, new { Value = item })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"FOREACH (item IN [1, 2, 3] |{NewLine}\t" +
                "SET n = { Value: item })",
                cypher.Query);
            Assert.Equal(0, cypher.Parameters.Count);
        }

        #endregion // FOREACH (item IN [1,2,3] | SET n.Value = item)

        #region MATCH (n:Person) FOREACH (item IN n | SET item.Enabled = true)

        [Fact]
        public void Foreach_Match_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person))
                                    .Foreach(n, item => Set(item, new { Enabled = true })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH (n:Person){NewLine}" +
                $"FOREACH (item IN n |{NewLine}\t" +
                "SET item = { Enabled: $p_0 })",
                cypher.Query);
            Assert.Equal(true, cypher.Parameters["p_0"]);
            Assert.Equal(1, cypher.Parameters.Count);
        }

        #endregion // MATCH (n:Person) FOREACH (item IN n | SET item.Enabled = true)

        #region MATCH v = ()-->() WHERE .. FOREACH (item IN nodes(v) | SET item.Enabled = $p_2)

        [Fact]
        public void Foreach_Match_Assignment_Test()
        {
            var v = Variables.CreatePath();
            var (p, a) = Variables.CreateMulti<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(v, N(p, Person) > N(a, Animal))
                                    .Where(p._.Name == "Eric" && a._.Name == "Doggy")
                                    .Foreach(Nodes(v), item => Set(item, new { Enabled = true })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH v = (p:Person)-->(a:Animal){NewLine}" +
                $"WHERE p.Name = $p_0 AND a.Name = $p_1{NewLine}" +
                $"FOREACH (item IN nodes(v) |{NewLine}\t" +
                "SET item = { Enabled: $p_2 })",
                cypher.Query);
            Assert.Equal("Eric", cypher.Parameters["p_0"]);
            Assert.Equal("Doggy", cypher.Parameters["p_1"]);
            Assert.Equal(true, cypher.Parameters["p_2"]);
            Assert.Equal(3, cypher.Parameters.Count);
        }

        #endregion // MATCH v = ()-->() WHERE .. FOREACH (item IN nodes(v) | SET item.Enabled = $p_2)
    }
}

