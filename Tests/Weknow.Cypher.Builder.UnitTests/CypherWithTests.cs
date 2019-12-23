using System.Collections;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherWithTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherWithTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region With_Test

        [Fact]
        public void With_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypher = CypherBuilder.Default
                            .Merge("(a:ACTOR)")
                            .Set("a", "ConcurrencyVersion")
                            .Unwind("$Movies", "mv")
                            .Match("(movie:MOVIE { Id = mv })")
                            .Merge("(movie)<-[p:Played]-(a)")
                            .Set("p", "Strength")
                            .Unwind("$Years", "year")
                            .Match("(a)")
                            .Where("a.BirthDay = year");

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (a:ACTOR) " +
                "SET a.ConcurrencyVersion = $a_ConcurrencyVersion " +
                "WITH * " +
                "UNWIND $Movies AS mv " +
                "MATCH (movie:MOVIE { Id = mv }) " +
                "MERGE (movie)<-[p:Played]-(a) " +
                "SET p.Strength = $p_Strength " +
                "WITH * " +
                "UNWIND $Years AS year " +
                "MATCH (a) " +
                "WHERE a.BirthDay = year", cypher.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // With_Test
    }
}