using System.Data;
using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
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
            var PropA = Parameters.Create();
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
            var prop = Parameters.Create();
            var (a, r, b) = Variables.CreateMulti();

            CypherCommand cypher = _(() =>
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

        #region Relation_WithReuse_Test

        [Fact]
        public void Relation_WithReuse_Test()
        {
            var (maintainer_Id, Id) = Parameters.CreateMulti();
            var (maintainer_, n) = Variables.CreateMulti();

            var maintainer = Reuse(() => R[By] > N(maintainer_, Maintainer, new { Id = maintainer_Id }));

            CypherCommand cypher = _(() => Merge(N(n, Person, new { Id }) - maintainer));

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal("MERGE (n:Person { Id: $Id })-[:By]->(maintainer_:Maintainer { Id: $maintainer_Id })", cypher.Query);
            // MERGE (n:Person { Id: $Id })-[:By]->(maintainer_:Maintainer { Id: $maintainer_Id })maintainer
        }

        #endregion // Relation_WithReuse_Test
    }
}

