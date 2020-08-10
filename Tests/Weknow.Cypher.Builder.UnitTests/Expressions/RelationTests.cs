using System.Data;
using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
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
            var PropA = CreateParameter();
            CypherCommand cypher = _(a => r => b =>
             Match(N(a, Person) - R[r, KNOWS, new { PropA }] > N(b, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (a:Person)-[r:KNOWS { PropA: $PropA }]->(b:Person)", cypher.Query);
        }

        #endregion // Relation_WithProp_Test

        #region Relation_WithPropConst_Test

        [Fact]
        public void Relation_WithPropConst_Test()
        {
            ParameterDeclaration? prop = null;
            CypherCommand cypher = _(a => r => b =>
             Match(N(a, Person) - R[r, KNOWS, new { PropA = prop }] > N(b, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (a:Person)-[r:KNOWS { PropA: $prop }]->(b:Person)", cypher.Query);
        }

        #endregion // Relation_WithPropConst_Test

        #region (n:Person: Animal) / Relation_MultiType_Test

        [Fact]
        public void Relation_MultiType_Test()
        {
            CypherCommand cypher = _(a => r => b =>
             Match(N(a, Person) - R[r, KNOWS | LIKE] > N(b, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
            @"MATCH (a:Person)-[r:KNOWS|LIKE]->(b:Person)", cypher.Query);
        }

        #endregion // (n:Person $n) / Relation_MultiType_Test

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
            var PropA = CreateParameter();
            CypherCommand cypher = _(a => r1 => b => r2 => c =>
             Match(N(a, Person) - R[r1, KNOWS, new { PropA }] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
             .Where(a.OfType<Foo>().Name == "Avi")
             .Return(a.OfType<Foo>().Name, r1, b.All<Bar>(), r2, c));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (a:Person)-[r1:KNOWS { PropA: $PropA }]->(b:Person)<-[r2:KNOWS]-(c:Person)
WHERE a.Name = $p_1
RETURN a.Name, r1, b.Id, b.Name, b.Date, r2, c", cypher.Query);

        }

        #endregion // Relation_WithProp_2_Test

        #region Relation_WithReuse_Test

        [Fact]
        public void Relation_WithReuse_Test()
        {
            ParameterDeclaration? maintainer_Id = null;
            var maintainer = Reuse(maintainer_ => R[By] > N(maintainer_, Maintainer, new { Id = maintainer_Id }));

            CypherCommand cypher = _(n => Merge(N(n, Person, n.P(Id)) - maintainer));

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal("MERGE (n:Person { Id: $Id })-[:By]->(maintainer_:Maintainer { Id: $maintainer_Id })", cypher.Query);
        }

        #endregion // Relation_WithReuse_Test
    }
}

