using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]

    public class MapTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public MapTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n:Person {{ Id: $Id }}) SET n += $p

        [Fact]
        public void Match_SetAsMap_Update_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { n.__.Id }))
                                    .SetPlus(n, n.Prm));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person {{ Id: n.Id }}){NewLine}" +
"SET n += $n", cypher.Query);

            Assert.Equal(1, cypher.Parameters.Count);
            Assert.True(cypher.Parameters.ContainsKey("n"));
        }

        #endregion // MATCH (n:Person {{ Id: $Id }}) SET n += $p

        #region MATCH (n:Person {{ Id: $Id }}) SET n += $p

        [Fact]
        public void Match_SetAsMap_Update_Inline_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { n.__<Foo>().Id }))
                                    .SetPlus(n, n.Prm));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person {{ Id: n.Id }}){NewLine}" +
"SET n += $n", cypher.Query);

            Assert.Equal(1, cypher.Parameters.Count);
            Assert.True(cypher.Parameters.ContainsKey("n"));
        }

        #endregion // MATCH (n:Person {{ Id: $Id }}) SET n += $p

        #region MATCH (n:Person {{ Id: $Id }}) SET n += $p

        [Fact]
        public void Match_SetAsMap_Explicit_Update_Test()
        {
            var p = Parameters.Create<Foo>();
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { p._.Id }))
                                    .SetPlus(n, p));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
"SET n += $p", cypher.Query);

            Assert.Equal(2, cypher.Parameters.Count);
            Assert.True(cypher.Parameters.ContainsKey(nameof(Foo.Id)));
            Assert.True(cypher.Parameters.ContainsKey(nameof(p)));
        }

        #endregion // MATCH (n:Person {{ Id: $Id }}) SET n += $p

        #region CREATE (n:Person $n)

        [Fact]
        public void CreateAsMap_Test()
        {
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Create(N(n, Person, n.Prm)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person $n)", cypher.Query);
        }

        #endregion // CREATE (n:Person $n)

        #region CREATE (n:Person $map)

        [Fact]
        public void CreateAsMap_WithParamName_Test()
        {
            var n = Variables.Create();
            var map = Parameters.Create();

            CypherCommand cypher = _(() =>
                                    Create(N(n, Person, map)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person $map)", cypher.Query);
        }

        #endregion // "CREATE (n:Person $map)

        #region Reuse: (n:Person $n)

        [Fact]
        public void Node_Variable_Label_Test()
        {
            var n = Variables.Create();

            var pattern = Reuse(() => N(n, Person, n.AsParameter));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person $n)", pattern.ToString());
        }

        #endregion // Reuse: (n:Person $n)

        #region CREATE (n:Person $map)

        [Fact]
        public void Node_Variable_Label_MapAsVar_Test()
        {
            var n = Variables.Create();
            var map = Parameters.Create();

            var pattern = _(() => Create(N(n, Person, map)));

            _outputHelper.WriteLine(pattern.ToString());

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"CREATE (n:Person $map)", pattern.ToString());
        }

        #endregion // CREATE (n:Person $map)
    }
}

