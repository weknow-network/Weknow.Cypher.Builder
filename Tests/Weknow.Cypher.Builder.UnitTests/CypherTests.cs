using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private static readonly Regex TrimX = new Regex(@"\s+"); // TODO: remove it when changing the API https://github.com/weknow-network/Weknow.Cypher.Builder/issues/3

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

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(cypherCommand.ToCypher(CypherFormat.SingleLine), cypherCommand.ToString());
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
            string cypherCommand = CypherBuilder.Default
                            .Match("(n:Foo)")
                            .Where<Foo>(f => f.Name)
                            .ToCypher(CypherFormat.MultiLineDense);

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(cypherCommand, cypherCommand.ToString());
            Assert.Equal(cypherCommand, cypher);
        }

        #endregion // CastToString_Test

        #region Match_Statement_Test

        [Fact]
        public void Match_Statement_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match("(n:Foo)");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo)", cypherCommand);
        }

        #endregion // Match_Statement_Test

        #region Match_Props_Exp_Test

        [Fact]
        public void Match_Props_Exp_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            // using static Weknow.CypherFactory; enable to avoid CypherFactory
            string props = P.Create<Foo>(f => f.Id, f => f.Name); // same as CypherFactory.P or CypherFactory.Properties
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo {props})");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Match_Props_Exp_Test

        #region Match_Props_All_Test

        [Fact]
        public void Match_Props_All_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            // using static Weknow.CypherFactory; enable to avoid CypherFactory
            string props = Properties.CreateAll<Foo>(); // same as CypherFactory.P or CypherFactory.Properties
            props = TrimX.Replace(props, " "); // TODO: remove it when changing the API https://github.com/weknow-network/Weknow.Cypher.Builder/issues/3
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo {props})");
                            // TODO: API CHANGE: .Match(ps => $"(n:Foo {props})", () => Properties.CreateAll<Foo>());

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo { Id: $Id ,Name: $Name ,DateOfBirth: $DateOfBirth })",
                                                cypherCommand.ToCypher(CypherFormat.SingleLine));
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

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
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

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
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

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo { Id: $Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
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

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("OPTIONAL MATCH (n:Foo { Id: $Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
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

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $Name", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Match_Where_Test

        #region Merge_OnCreate_OnMatch_Test

        [Fact]
        public void Merge_OnCreate_OnMatch_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            props = TrimX.Replace(props, " "); // TODO: remove it when changing the API https://github.com/weknow-network/Weknow.Cypher.Builder/issues/3
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(n:Foo {props})")
                            .OnCreateSet("f", nameof(Foo.Id), nameof(Foo.Name))
                            .OnMatch()
                            .SetByConvention<Foo>("f", name => name != nameof(Foo.Id));

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "ON CREATE " +
                "SET f.Id = $f_Id , f.Name = $f_Name " +
                "ON MATCH " +
                "SET f.Name = $f_Name , f.DateOfBirth = $f_DateOfBirth";
            _outputHelper.WriteLine(cypherCommand.ToCypher(CypherFormat.SingleLine));
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
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
                                .Set<Foo>(f => f.Id)
                                .Set<Foo>(f => f.Name)
                            .OnMatchSet("f","Name","DateOfBirth");

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "ON CREATE " +
                "SET f.Id = $f_Id , f.Name = $f_Name " +
                "ON MATCH " +
                "SET f.Name = $f_Name , f.DateOfBirth = $f_DateOfBirth";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Merge_OnCreate_OnMatch_Exp_Test

        #region Merge_Match_AutoWith_Test

        [Fact]
        public void Merge_Match_AutoWith_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(n:Foo {props})")
                            .Match($"(a)")
                            .Return("n");

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "WITH * " +
                "MATCH (a) " +
                "RETURN n";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Merge_Match_AutoWith_Test

        #region Create_Test

        [Fact]
        public void Create_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Create($"(n:Foo {props})");

            string expected = "CREATE (n:Foo { Id: $Id })";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Create_Test

        #region CreateInstance_Test

        [Fact]
        public void CreateInstance_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .CreateInstance("Foo", "foo","x");

            string expected = "CREATE (x:Foo $foo)";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // CreateInstance_Test

        #region CreateInstance_NoVar_Test

        [Fact]
        public void CreateInstance_NoVar_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .CreateInstance("Foo", "foo");

            string expected = "CREATE (n:Foo $foo)";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // CreateInstance_NoVar_Test

        #region CreateInstance_OnlyLabel_Test

        [Fact]
        public void CreateInstance_OnlyLabel_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .CreateInstance("Foo");

            string expected = "CREATE (n:Foo $n_Foo)";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // CreateInstance_OnlyLabel_Test

        #region CreateInstance_OfT_Test

        [Fact]
        public void CreateInstance_OfT_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = CypherFactory.P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .CreateInstance<Foo>("foo","x");

            string expected = "CREATE (x:Foo $foo)";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // CreateInstance_OfT_Test

    }
}