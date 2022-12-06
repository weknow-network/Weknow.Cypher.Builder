using Weknow.CypherBuilder.Declarations;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]
    
    public class CreateTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CreateTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region CREATE (n:Person { PropA: $PropA, PropB: $PropB })

        [Fact]
        public void Create_Test()
        {
            var n = Variables.Create();
            var (PropA, PropB) = Parameters.CreateMulti();

            CypherCommand cypher = _(() => Create(N(n, Person, new { PropA, PropB })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);

            Assert.True(cypher.Parameters.ContainsKey(nameof(PropA)));
            Assert.True(cypher.Parameters.ContainsKey(nameof(PropB)));

            var prms = cypher.Parameters
                             .AddOrUpdate(nameof(PropA), "Hello")
                             .AddOrUpdate(nameof(PropB), "World");

        }

        #endregion // CREATE (n:Person { PropA: $PropA, PropB: $PropB })

        #region CREATE (n:Person { PropA: $PropA, PropB: $PropB })

        [Fact]
        public void Create_VarPrm_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() => Create(N(n, Person, new { n.Prm._.Id, n.Prm._.Name })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person { Id: $Id, Name: $Name })", cypher.Query);

            Assert.True(cypher.Parameters.ContainsKey(nameof(Foo.Id)));
            Assert.True(cypher.Parameters.ContainsKey(nameof(Foo.Name)));

            var prms = cypher.Parameters
                             .AddOrUpdate(nameof(Foo.Id), "Hello")
                             .AddOrUpdate(nameof(Foo.Name), "World");

        }

        #endregion // CREATE (n:Person { PropA: $PropA, PropB: $PropB })

        #region CREATE (n:Person { PropA: $PropA, PropB: $PropB })

        [Fact]
        public void Create_InlineVarPrm_Test()
        {
            CypherCommand cypher = _(n => Create(N(n, Person, new { n.Prm._<Foo>().Id, n.Prm._<Foo>().Name })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person { Id: $Id, Name: $Name })", cypher.Query);

            Assert.True(cypher.Parameters.ContainsKey(nameof(Foo.Id)));
            Assert.True(cypher.Parameters.ContainsKey(nameof(Foo.Name)));

            var prms = cypher.Parameters
                             .AddOrUpdate(nameof(Foo.Id), "Hello")
                             .AddOrUpdate(nameof(Foo.Name), "World");

        }

        #endregion // CREATE (n:Person { PropA: $PropA, PropB: $PropB })

        #region CREATE (n:Person $n)

        [Fact]
        public void CreateAsMap_Test()
        {
            var n = Variables.Create();
            CypherCommand cypher = _(() => Create(N(n, Person, n.AsParameter)));

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
            CypherCommand cypher = _(() => Create(N(n, Person, map)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n:Person $map)", cypher.Query);
        }

        #endregion // CREATE (n:Person $map)

        #region CREATE (n)-[r:KNOWS]->(m)

        [Fact]
        public void CreateRelation_Test()
        {
            var (n, r, m) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Create(N(n) - R[r, KNOWS] > N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n)-[r:KNOWS]->(m)", cypher.Query);
        }

        #endregion // CREATE (n)-[r:KNOWS]->(m)

        #region CREATE (n)-[r:KNOWS { PropA: $PropA, PropB: $PropB }]->(m)

        [Fact]
        public void CreateRelation_WithParams_Test()
        {
            var (n, r, m) = Variables.CreateMulti();
            ParameterDeclaration PropA = Parameters.Create(), PropB = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Create(N(n) - R[r, KNOWS, new { PropA, PropB }] > N(m)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE (n)-[r:KNOWS { PropA: $PropA, PropB: $PropB }]->(m)", cypher.Query);
        }

        #endregion // CREATE (n)-[r:KNOWS { PropA: $PropA, PropB: $PropB }]->(m)
    }
}

