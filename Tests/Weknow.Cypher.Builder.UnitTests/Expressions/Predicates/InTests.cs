using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;

namespace Weknow.GraphDbCommands
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
            var (items, Id) = Parameters.CreateMulti();
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Where(In(n, items))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                   $"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
                   $"WHERE n IN $items{NewLine}" +
                   "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n IN $items RETURN n / In_Test

        #region MATCH (n:Person { Id: $Id }) WHERE n IN $items RETURN n / In_Prop_Test

        [Fact]
        public void In_Prop_Test()
        {
            var (items, Id) = Parameters.CreateMulti();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Where(In(n._.Id, items))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                   $"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
                   $"WHERE n.Id IN $items{NewLine}" +
                   "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n.Id IN $items RETURN n / In_Prop_Test

        #region MATCH (n:Person { Id: $Id }) WHERE n.PropA IN $items AND n.PropB IN $items RETURN n / In_Complex_Test

        [Fact]
        public void In_Complex_Test()
        {
            var (items, Id) = Parameters.CreateMulti();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Where(In(n._.PropA, items) && In(n._.PropB, items))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                   $"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
                   $"WHERE n.PropA IN $items AND n.PropB IN $items{NewLine}" +
                   "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n.PropA IN $items AND n.PropB IN $items RETURN n / In_Complex_Test

        [Fact]
        public void In_Var_Test()
        {
            var Id = Parameters.Create();
            var n = Variables.Create<Foo>();
            var item = Variables.Create<Cmlx>();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Where(In(n._.Id, item._.Names))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                   $"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
                   $"WHERE n.Id IN item.Names{NewLine}" +
                   "RETURN n", cypher.Query);
        }

    }
}

