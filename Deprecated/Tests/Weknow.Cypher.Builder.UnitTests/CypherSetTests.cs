using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherSetTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherSetTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region SetCombination_Test

        [Fact]
        public void SetCombination_Test()
        {
            var cypherCommand = CypherBuilder.Create(cfg => cfg.Naming.NodeLabelConvention = CypherNamingConvention.SCREAMING_CASE)
                            .Merge($"(f:Foo)")
                            .Merge($"(b:Bar)")
                                .Set("f.SomeProperty = $sp")
                                .Set("f", "SomeOtherProp", "More")
                                .Set<Foo>(f => f.Id)
                                .Set<Bar>(b => b.Value)
                                .SetMore(b => b.Name);

            string expected =
                "MERGE (f:Foo) " +
                "MERGE (b:Bar) " +
                "SET f.SomeProperty = $sp , " +
                "f.SomeOtherProp = $SomeOtherProp , " +
                "f.More = $More , " +
                "f.Id = $Id , " +
                "b.Value = $Value , " +
                "b.Name = $Name";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetCombination_Test

        #region SetByConvention_Test

        [Fact]
        public void SetByConvention_Test()
        {
            string props = P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Create()
                            .Merge($"(f:Foo {props})")
                               .SetByConvention<Foo>("f", n => n != "Id");

            string expected = "MERGE (f:Foo { Id: $f_Id }) " +
                "SET f.Name = $Name , f.DateOfBirth = $DateOfBirth";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetByConvention_Test

        #region SetAll_Test

        [Fact]
        public void SetAll_Test()
        {
            string props = P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Create()
                            .Merge($"(f:Foo {props})")
                               .SetAll<Foo>("f", f => f.Id);

            string expected = "MERGE (f:Foo { Id: $f_Id }) " +
                "SET f.Name = $Name , f.DateOfBirth = $DateOfBirth";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetAll_Test

        #region SetLabel_Test

        [Fact]
        public void SetLabel_Test()
        {
            string props = P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Create()
                            .Merge($"(f:Foo {props})")
                               .SetLabel("f", "NewLabel");

            string expected = "MERGE (f:Foo { Id: $f_Id }) " +
                "SET f:NewLabel";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetLabel_Test

        #region SetInstance_Test

        [Fact]
        public void SetInstance_Test()
        {
            var cypherCommand = CypherBuilder.Create()
                            .Merge($"(f:Foo)")
                               .SetEntity<Foo>("f");

            string expected = "MERGE (f:Foo) " +
                "SET f += $f_Foo";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetInstance_Test

        #region SetInstanceReplace_Test

        [Fact]
        public void SetInstanceReplace_Test()
        {
            var cypherCommand = CypherBuilder.Create()
                            .Merge($"(f:Foo)")
                               .SetEntity<Foo>("f", SetInstanceBehavior.Replace);

            string expected = "MERGE (f:Foo) " +
                "SET f = $f_Foo";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetInstanceReplace_Test

        #region MultiSets_Test

        [Fact]
        public void MultiSets_Test()
        {
            var cypherCommand = CypherBuilder.Create()
                    .Match("(person:Person {name: 'Cuba Gooding Jr.'})-[:ACTED_IN]->(movie:Movie)")
                    .With($"person, {A.Collect($"movie.Rank")} as ranks")
                    .Set("person.MinRank = apoc.coll.min(ranks)")
                    .Set("person.MaxRank = apoc.coll.max(ranks)")
                    .Set("person.AvgRank = apoc.coll.avg(ranks)");


            string expected =
                "MATCH (person:Person {name: 'Cuba Gooding Jr.'})-[:ACTED_IN]->(movie:Movie) " +
                "WITH person, collect(movie.Rank) as ranks " +
                "SET person.MinRank = apoc.coll.min(ranks) , " +
                "person.MaxRank = apoc.coll.max(ranks) , " +
                "person.AvgRank = apoc.coll.avg(ranks)";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // MultiSets_Test
    }
}