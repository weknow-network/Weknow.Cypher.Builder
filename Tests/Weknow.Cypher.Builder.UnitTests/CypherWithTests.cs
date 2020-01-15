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
            var cypher = CypherBuilder.Create()
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
                "SET a.ConcurrencyVersion = $ConcurrencyVersion " +
                "WITH * " +
                "UNWIND $Movies AS mv " +
                "MATCH (movie:MOVIE { Id = mv }) " +
                "MERGE (movie)<-[p:Played]-(a) " +
                "SET p.Strength = $Strength " +
                "WITH * " +
                "UNWIND $Years AS year " +
                "MATCH (a) " +
                "WHERE a.BirthDay = year", cypher.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // With_Test

        #region With_Create_Match_Test

        [Fact]
        public void With_Create_Match_Test()
        {
            var cypher = CypherBuilder.Create()
                            .Create("(a:ACTOR)")
                            .Set("a", "ConcurrencyVersion")
                            .Match("(movie:MOVIE { Id = $Id })");

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "CREATE (a:ACTOR) " +
                "SET a.ConcurrencyVersion = $ConcurrencyVersion " +
                "WITH * " +
                "MATCH (movie:MOVIE { Id = $Id })",
                cypher.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // With_Create_Match_Test

        #region With_Merge_Match_Test

        [Fact]
        public void With_Merge_Match_Test()
        {

            var cypher = CypherBuilder.Create()
                            .Merge("(a:ACTOR)")
                            .Set("a", "ConcurrencyVersion")
                            .Match("(movie:MOVIE { Id = $Id })");

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (a:ACTOR) " +
                "SET a.ConcurrencyVersion = $ConcurrencyVersion " +
                "WITH * " +
                "MATCH (movie:MOVIE { Id = $Id })",
                cypher.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // With_Merge_Match_Test

        #region With_Create_Unwind_Test

        [Fact]
        public void With_Create_Unwind_Test()
        {
            var cypher = CypherBuilder.Create()
                            .Create("(a:ACTOR)")
                            .Set("a", "ConcurrencyVersion")
                            .Unwind("$Movies", "mv")
                            .Match("(movie:MOVIE { Id = mv })");

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "CREATE (a:ACTOR) " +
                "SET a.ConcurrencyVersion = $ConcurrencyVersion " +
                "WITH * " +
                "UNWIND $Movies AS mv " +
                "MATCH (movie:MOVIE { Id = mv })",
                cypher.ToCypher(CypherFormat.SingleLine)); 
        }

        #endregion // With_Create_Unwind_Test

        #region With_Merge_Match_Test

        [Fact]
        public void With_Merge_Unwind_Test()
        {
            var cypher = CypherBuilder.Create()
                            .Merge("(a:ACTOR)")
                            .Set("a", "ConcurrencyVersion")
                            .Unwind("$Movies", "mv")
                            .Match("(movie:MOVIE { Id = mv })");

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (a:ACTOR) " +
                "SET a.ConcurrencyVersion = $ConcurrencyVersion " +
                "WITH * " +
                "UNWIND $Movies AS mv " +
                "MATCH (movie:MOVIE { Id = mv })",
                cypher.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // With_Merge_Unwind_Test
    }
}