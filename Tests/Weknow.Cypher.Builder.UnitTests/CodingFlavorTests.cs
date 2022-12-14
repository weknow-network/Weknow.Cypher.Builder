using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;
using static System.Environment;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Flavor")]
    public class CodingFlavorTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CodingFlavorTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Flavor_Match_Test

        [Fact]
        public void Flavor_Match_Test()
        {
            string cypher = _(() => Match(N(Person & Maintainer))
                                    .Match(N(Animal & Friend)), 
                                    c => c.Flavor = CypherFlavor.Neo4j5);

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (:Person&Maintainer){NewLine}" +
                          "MATCH (:Animal&Friend)", cypher);
        }

        #endregion // Flavor_Match_Test

        // TODO: [bnaya 2022-12-14] WITH n:Person&Maintainer, WHERE n:Person&Maintainer, RETURN n:Person&Maintainer (suggested syntax n.WithLabels(Person & Maintainer) )

        //#region Flavor_With_Test

        //[Fact]
        //public void Flavor_With_Test()
        //{
        //    string cypher = _(n => Match(N(n, Person&Maintainer))
        //                            .With(n:Animal&Friend)), 
        //                            c => c.Flavor = CypherFlavor.Neo4j5);

        //    _outputHelper.WriteLine(cypher);
        //    Assert.Equal($"MATCH (n){NewLine}" +
        //                  "WITH n:Person&Maintainer", cypher);
        //}

        //#endregion // Flavor_With_Test

        #region Flavor_Merge_Test

        [Fact]
        public void Flavor_Merge_Test()
        {
            string cypher = _(() => Merge(N(Person&Maintainer))
                                    .Merge(N(Animal & Friend)),
                c => c.Flavor = CypherFlavor.Neo4j5);

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (:Person:Maintainer){NewLine}" +
                 "MERGE (:Animal:Friend)",
                cypher);
        }

        #endregion // Flavor_Merge_Test

        #region Empty_vs_variable_Test

        [Fact]
        public void Empty_vs_variable_Test()
        {
            string cypher1 = _(() => Match(N(Person)));
            string cypher2 = _(n => Match(N(Person)));

            _outputHelper.WriteLine(cypher1);
            _outputHelper.WriteLine(cypher2);
            Assert.Equal(cypher1, cypher2);
        }

        #endregion // Empty_vs_variable_Test
    }
}

