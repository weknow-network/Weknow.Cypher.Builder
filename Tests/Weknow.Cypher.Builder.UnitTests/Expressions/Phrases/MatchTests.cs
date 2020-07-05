using System;

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
    [Trait("Category", "Match")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class MatchTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public MatchTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Match_SetAsMap_Update_Test

        [Fact]
        public void Match_SetAsMap_Update_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(+n.AsMap));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n += $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Update_Test

        #region Match_SetAsMap_Replace_Test

        [Fact]
        public void Match_SetAsMap_Replace_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.AsMap));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n = $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Replace_Test

        #region Match_Set_WithPrefix_Test

        [Fact]
        public void Match_Set_WithPrefix_Test()
        {
            CypherCommand cypher = _(n => n_ =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.P(PropA, Pre(n_, PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $n_PropB", cypher.Query);
        }

        #endregion // Match_Set_WithPrefix_Test

        #region Match_Set_Test

        [Fact]
        public void Match_Set_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.P(PropA, PropB)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_Test

        #region Match_Set_OfT_Test

        [Fact]
        public void Match_Set_OfT_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(P(n.As<Foo>().PropA, n.As<Foo>().PropB)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_Test

        #region Match_Set_OfT_Convention_Test

        [Fact]
        public void Match_Set_OfT_Convention_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.Convention<Foo>(name => name.StartsWith("Prop"))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_Convention_Test

        #region Match_Set_OfT_All_Test

        [Fact]
        public void Match_Set_OfT_All_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(n.All<Foo>()));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.Id = $Id, n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_All_Test

        #region Match_Set_AddLabel_Test

        [Fact]
        public void Match_Set_AddLabel_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N<Foo>(n, P(Id)))
                                    .Set(n, Person));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Foo { Id: $Id })
SET n:Person", cypher.Query);
        }

        #endregion // Match_Set_AddLabel_Test
    }
}

