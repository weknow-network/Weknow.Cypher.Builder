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

        #region ToString_Test

        [Fact]
        public void ToString_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            FluentCypherWhereExpression cypherCommand = CypherBuilder.Default
                            .Match("(n:Foo)")
                            .Where<Foo>(f => f.Name);

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(cypherCommand.Cypher, cypherCommand.ToString());
        }

        #endregion // ToString_Test

        #region CastToString_Test

        [Fact]
        public void CastToString_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string cypher = CypherBuilder.Default
                            .Match("(n:Foo)")
                            .Where<Foo>(f => f.Name);
            ICypherable cypherCommand = CypherBuilder.Default
                            .Match("(n:Foo)")
                            .Where<Foo>(f => f.Name);

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(cypherCommand.Cypher, cypherCommand.ToString());
            Assert.Equal(cypherCommand.Cypher, cypher);
        }

        #endregion // CastToString_Test

        #region Match_Statement_Test

        [Fact]
        public void Match_Statement_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match("(n:Foo)");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo)", cypherCommand.CypherLine);
        }

        #endregion // Match_Statement_Test

        #region Match_Props_Exp_Test

        [Fact]
        public void Match_Props_Exp_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id, f => f.Name);
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo {props})");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_Exp_Test

        #region Match_Props_All_Test

        [Fact]
        public void Match_Props_All_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.CreateAll<Foo>();
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo {props})");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id ,Name: $Name ,DateOfBirth: $DateOfBirth })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_All_Test

        #region Match_Props_All_Exclude_Test

        [Fact]
        public void Match_Props_All_Exclude_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.CreateAll<Foo>(f => f.DateOfBirth);
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo {props})");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_All_Exclude_Test

        #region Match_Props_Convention_Test

        [Fact]
        public void Match_Props_Convention_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.CreateByConvention<Foo>(n => !n.StartsWith("Date"));
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo {props})");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.CypherLine);
        }

        #endregion // Match_Props_Convention_Test

        #region Match_Props_Test

        [Fact]
        public void Match_Props_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create("Id");
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo {props})");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo { Id: $Id })", cypherCommand.CypherLine);
        } 

        #endregion // Match_Props_Test

        #region OptionalMatch_Props_Test

        [Fact]
        public void OptionalMatch_Props_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create("Id");
            var cypherCommand = CypherBuilder.Default
                            .OptionalMatch($"(n:Foo {props})");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("OPTIONAL MATCH (n:Foo { Id: $Id })", cypherCommand.CypherLine);
        } 

        #endregion // OptionalMatch_Props_Test

        #region Match_Where_Test

        [Fact]
        public void Match_Where_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo)")
                            .Where("n.Name = $Name");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $Name", cypherCommand.CypherLine);
        }

        #endregion // Match_Where_Test

        #region Match_WhereWithVariable_Test

        [Fact]
        public void Match_WhereWithVariable_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo)")
                            .Where("n","Name", "Id");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $n_Name, n.Id = $n_Id", cypherCommand.CypherLine);
        }

        #endregion // Match_WhereWithVariable_Test

        #region Merge_OnCreate_OnMatch_Test

        [Fact]
        public void Merge_OnCreate_OnMatch_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(n:Foo {props})")
                            .OnCreateSet("f", nameof(Foo.Id), nameof(Foo.Name))
                            .OnMatch()
                            .SetByConvention<Foo>("f", name => name != nameof(Foo.Id));

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "ON CREATE " +
                "SET f.Id = $f_Id, f.Name = $f_Name " +
                "ON MATCH " +
                "SET f.Name = $f_Name, f.DateOfBirth = $f_DateOfBirth";
            _outputHelper.WriteLine(cypherCommand.CypherLine);
            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(expected, cypherCommand.CypherLine);
        }

        #endregion // Merge_OnCreate_OnMatch_Test

        #region Merge_OnCreate_OnMatch_Exp_Test

        [Fact]
        public void Merge_OnCreate_OnMatch_Exp_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(n:Foo {props})")
                            .OnCreate()
                                .Set<Foo>(f => f.Id).SetMore(f => f.Name)
                            .OnMatchSet("f","Name","DateOfBirth");

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "ON CREATE " +
                "SET f.Id = $f_Id ,f.Name = $f_Name " +
                "ON MATCH " +
                "SET f.Name = $f_Name, f.DateOfBirth = $f_DateOfBirth";
            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(expected, cypherCommand.CypherLine);
        }

        #endregion // Merge_OnCreate_OnMatch_Exp_Test
    }
}