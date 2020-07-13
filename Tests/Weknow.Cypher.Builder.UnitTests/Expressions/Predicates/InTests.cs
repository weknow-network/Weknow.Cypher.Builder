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
    [Trait("Category", "In")]
    [Trait("Group", "Predicates")]
    [Trait("Segment", "Expression")]
    public class InTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public InTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n:Person { Id: $Id }) WHERE n IN [$items] RETURN n / In_Test

        [Fact]
        public void In_Test()
        {
            CypherCommand cypher = _(n => items =>
                                    Match(N(n, Person, P(Id)))
                                    .Where(n.In(items))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                    "MATCH (n:Person { Id: $Id })\r\n" +
                    "WHERE n IN [$items]\r\n" +
                    "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n IN [$items] RETURN n / In_Test

        #region MATCH (n:Person { Id: $Id }) WHERE n IN [$x, $y] RETURN n / In_Multi_Test

        [Fact]
        public void In_Multi_Test()
        {
            CypherCommand cypher = _(n => x => y =>
                                    Match(N(n, Person, P(Id)))
                                    .Where(n.In(x, y))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                    "MATCH (n:Person { Id: $Id })\r\n" +
                    "WHERE n IN [$x, $y]\r\n" +
                    "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n.Id IN [$x, $y] RETURN n / In_Multi_Test

        #region MATCH (n:Person { Id: $Id }) WHERE n IN [$items] RETURN n / In_Prop_Test

        [Fact]
        public void In_Prop_Test()
        {
            CypherCommand cypher = _(n => items =>
                                    Match(N(n, Person, P(Id)))
                                    .Where(n.In(Id, items))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                    "MATCH (n:Person { Id: $Id })\r\n" +
                    "WHERE n.Id IN [$items]\r\n" +
                    "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n.Id IN [$items] RETURN n / In_Prop_Test
    }
}

