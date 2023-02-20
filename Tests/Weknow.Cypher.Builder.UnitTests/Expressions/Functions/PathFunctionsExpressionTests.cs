using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.CypherExtensions;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;


namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]

    public class PathFunctionsExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public PathFunctionsExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n) RETURN length(n) / Collect_Test

        [Fact]
        public void Length_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _((p, m) =>
                                    Match(p.AsPath, N(n) - R[KNOWS * 8] > N(m))
                                    .Where(n.__.FirstName == "Wong")
                                    .Return(n.Length()));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("""
                        MATCH p = (n)-[:KNOWS*..8]->(m)
                        WHERE n.FirstName = $p_0
                        RETURN length(n)
                        """, cypher.Query);
        }

        #endregion // MATCH (n) RETURN collect(n) / Collect_Test

        #region [x IN nodes(p) | x.Name]

        [Fact]
        public void ToList_Gen_Test()
        {
            var p = Variables.CreatePath<Foo>();
            CypherCommand cypher = _((n,m) => Match(p, N(n, Person)-N(m))
                                    .Return(p.Nodes().ToList( x => x.__.Name)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                """
                MATCH p = (n:Person)--(m)
                RETURN [x IN nodes(p) | x.Name]
                """, cypher.Query);
        }

        #endregion // [x IN nodes(p) WHERE x.Version < $p_0 | x.Name]

        #region [x IN nodes(p) | { Name: x.Name, Version: x.Version }]

        [Fact]
        public void ToList_Gen_Objects_Test()
        {
            var p = Variables.CreatePath<Foo>();
            CypherCommand cypher = _((n,m) => Match(p, N(n, Person)-N(m))
                                    .Return(p.Nodes()
                                             .ToList( 
                                                x => new { x.__.Name, x.__.Version })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                """
                MATCH p = (n:Person)--(m)
                RETURN [x IN nodes(p) | { Name: x.Name, Version: x.Version }]
                """, cypher.Query);
        }

        #endregion // [x IN nodes(p) WHERE x.Version < $p_0 | { Name: x.Name, Version: x.Version }]

        #region [x IN nodes(p) | x.Name]

        [Fact]
        public void ToList_Where_Test()
        {
            var p = Variables.CreatePath<Foo>();
            CypherCommand cypher = _((n,m) => Match(p, N(n, Person)-N(m))
                                    .Return(p.Nodes().ToList(x => x.__.Version < 3, x => x.__.Name)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                """
                MATCH p = (n:Person)--(m)
                RETURN [x IN nodes(p) WHERE x.Version < $p_0 | x.Name]
                """, cypher.Query);
        }

        #endregion // [x IN nodes(p) WHERE x.Version < $p_0 | x.Name]
    }
}

