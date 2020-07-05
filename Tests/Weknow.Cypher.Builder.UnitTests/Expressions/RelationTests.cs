using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

// TODO: Duplicate class Pattern to FullNamePattern for naming standard

// TODO: parameter factory injection for enabling to work with Neo4jParameters (Neo4jMapper)
//       Mimic Neo4jMappaer WithEntity, WithEntities + integration test
//       validate flat entity (in deep complex type throw exception with recommendation for best practice)

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Deprecated")]
    public class RelationTests
    {
        protected readonly ITestOutputHelper _outputHelper;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigTests"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper.</param>
        public RelationTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Relation_Test

        [Fact]
        public void Relation_Test()
        {
            CypherCommand cypher = _(a => b =>
             Match(N(a, Person) - R[KNOWS] > N(b, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
            @"MATCH (a:Person)-[:KNOWS]->(b:Person)", cypher.Query);
        }

        #endregion // Relation_Test

        #region Relation_WithVar_Test

        [Fact]
        public void Relation_WithVar_Test()
        {
            CypherCommand cypher = _(a => r => b =>
             Match(N(a, Person) - R[r, KNOWS] > N(b, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
            @"MATCH (a:Person)-[r:KNOWS]->(b:Person)", cypher.Query);
        }

        #endregion // Relation_WithVar_Test

        #region Relation_WithProp_Test

        [Fact]
        public void Relation_WithProp_Test()
        {
            CypherCommand cypher = _(a => r => b =>
             Match(N(a, Person) - R[r, KNOWS, P(PropA)] > N(b, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (a:Person)-[r:KNOWS { PropA: $PropA }]->(b:Person)", cypher.Query);
        }

        #endregion // Relation_WithProp_Test

        #region Relation_2_Test

        [Fact]
        public void Relation_2_Test()
        {
            CypherCommand cypher = _(a => b => c =>
             Match(N(a, Person) - R[KNOWS] > N(b, Person) < R[KNOWS] - N(c, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
            @"MATCH (a:Person)-[:KNOWS]->(b:Person)<-[:KNOWS]-(c:Person)", cypher.Query);
        }

        #endregion // Relation_2_Test

        #region Relation_WithVar_2_Test

        [Fact]
        public void Relation_WithVar_2_Test()
        {
            CypherCommand cypher = _(a => r1 => b => r2 => c =>
             Match(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
            @"MATCH (a:Person)-[r1:KNOWS]->(b:Person)<-[r2:KNOWS]-(c:Person)", cypher.Query);
        }

        #endregion // Relation_WithVar_2_Test

        #region Relation_WithProp_2_Test

        [Fact]
        public void Relation_WithProp_2_Test()
        {
            CypherCommand cypher = _(a => r1 => b => r2 => c =>
             Match(N(a, Person) - R[r1, KNOWS, P(PropA)] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
             .Where(a.As<Foo>().Name == "Avi")
             .Return(a.As<Foo>().Name, r1, b.All<Bar>(), r2, c));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (a:Person)-[r1:KNOWS { PropA: $PropA }]->(b:Person)<-[r2:KNOWS]-(c:Person)
WHERE a.Name = $p_1
RETURN a.Name, r1, b.Id, b.Name, b.Date, r2, c", cypher.Query);

        }

        #endregion // Relation_WithProp_2_Test
    }
}

