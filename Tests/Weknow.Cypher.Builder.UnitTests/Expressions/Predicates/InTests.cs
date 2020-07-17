using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;


namespace Weknow.Cypher.Builder
{
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

        #region MATCH (n:Person { Id: $Id }) WHERE n IN $items RETURN n / In_Test

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
                    "WHERE n IN $items\r\n" +
                    "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n IN $items RETURN n / In_Test

        #region MATCH (n:Person { Id: $Id }) WHERE n IN $items RETURN n / In_Prop_Test

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
                    "WHERE n.Id IN $items\r\n" +
                    "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n.Id IN $items RETURN n / In_Prop_Test

        #region MATCH (n:Person { Id: $Id }) WHERE n.PropA IN $items AND n.PropB IN $items RETURN n / In_Complex_Test

        [Fact]
        public void In_Complex_Test()
        {
            CypherCommand cypher = _(n => items =>
                                    Match(N(n, Person, P(Id)))
                                    .Where(n.In(PropA, items) && n.In(PropB, items))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                    "MATCH (n:Person { Id: $Id })\r\n" +
                    "WHERE n.PropA IN $items AND n.PropB IN $items\r\n" +
                    "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n.PropA IN $items AND n.PropB IN $items RETURN n / In_Complex_Test
    }
}

