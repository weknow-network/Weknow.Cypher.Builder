using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        } 

        #endregion // Ctor

        #region Match_Statement_Test

        [Fact]
        public void Match_Statement_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = Cypher.Builder()
                            .Match("(n:Foo)")
                            .Build();

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo)", cypherCommand.CypherLine);
        }

        #endregion // Match_Statement_Test

        #region Match_Props_Exp_Test

        [Fact]
        public void Match_Props_Exp_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherProps.Create<Foo>(f => f.Id, f => f.Name);
            var cypherCommand = Cypher.Builder()
                            .Match($"(n:Foo {props})")
                            .Build();

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_Exp_Test

        #region Match_Props_All_Test

        [Fact]
        public void Match_Props_All_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherProps.CreateAll<Foo>();
            var cypherCommand = Cypher.Builder()
                            .Match($"(n:Foo {props})")
                            .Build();

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id ,Name: $Name ,DateOfBirth: $DateOfBirth })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_All_Test

        #region Match_Props_All_Exclude_Test

        [Fact]
        public void Match_Props_All_Exclude_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherProps.CreateAll<Foo>(f => f.DateOfBirth);
            var cypherCommand = Cypher.Builder()
                            .Match($"(n:Foo {props})")
                            .Build();

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_All_Exclude_Test

        #region Match_Props_Convention_Test

        [Fact]
        public void Match_Props_Convention_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherProps.CreateByConvention<Foo>(n => !n.StartsWith("Date"));
            var cypherCommand = Cypher.Builder()
                            .Match($"(n:Foo {props})")
                            .Build();

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_Convention_Test

        #region Match_Props_Test

        [Fact]
        public void Match_Props_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherProps.Create("Id");
            var cypherCommand = Cypher.Builder()
                            .Match($"(n:Foo {props})")
                            .Build();

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id })", cypherCommand.CypherLine);
        } 

        #endregion // Match_Props_Test

        #region Merge_OnCreate_OnMatch_Test

        [Fact]
        public void Merge_OnCreate_OnMatch_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherProps.Create<Foo>(f => f.Id);
            var cypherCommand = Cypher.Builder()
                            .Merge($"(n:Foo {props})")
                            .OnCreateSet("f", nameof(Foo.Id), nameof(Foo.Name))
                            .OnMatch()
                            .SetByConvention<Foo>("f", name => name != nameof(Foo.Id))
                            .Build();

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "ON CREATE " +
                "SET f.Id = $f.Id, f.Name = $f.Name " +
                "ON MATCH " +
                "SET f.Name = $f.Name, f.DateOfBirth = $f.DateOfBirth";
            _outputHelper.WriteLine(cypherCommand.CypherLine);
            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(expected, cypherCommand.CypherLine);
        }

        #endregion // Merge_OnCreate_OnMatch_Test

        #region Merge_OnCreate_OnMatch_Exp_Test

        [Fact]
        public void Merge_OnCreate_OnMatch_Exp_Test()
        {
            Cypher.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherProps.Create<Foo>(f => f.Id);
            var cypherCommand = Cypher.Builder()
                            .Merge($"(n:Foo {props})")
                            .OnCreate()
                                .Set<Foo>(f => f.Id).SetMore(f => f.Name)
                            .OnMatchSet<Foo>(f => f.Name).SetMore(f => f.DateOfBirth)
                            .Build();

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "ON CREATE " +
                "SET f.Id = $f.Id ,f.Name = $f.Name " +
                "ON MATCH " +
                "SET f.Name = $f.Name ,f.DateOfBirth = $f.DateOfBirth";
            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(expected, cypherCommand.CypherLine);
        }

        #endregion // Merge_OnCreate_OnMatch_Exp_Test
    }
}