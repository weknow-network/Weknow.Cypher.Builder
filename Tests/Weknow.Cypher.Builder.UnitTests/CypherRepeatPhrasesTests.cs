using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherRepeatPhrasesTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherRepeatPhrasesTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Multi_Return_Test

        [Fact]
        public void Multi_Return_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match("(f:Foo)")
                            .Match("(b:Bar)")
                            .Return("f")
                            .Return("b");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (f:Foo) MATCH (b:Bar) RETURN f , b", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Multi_Return_Test

        #region Multi_With_Test

        [Fact]
        public void Multi_With_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match("(f:Foo)")
                            .Match("(b:Bar)")
                            .With("f")
                            .With("b");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (f:Foo) MATCH (b:Bar) WITH f , b", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Multi_With_Test

        #region Multi_Set_Test

        [Fact]
        public void Multi_Set_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match("(f:Foo)")
                            .Set("f", "Name")
                            .Set("f", "Date");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (f:Foo) SET f.Name = $f_Name , f.Date = $f_Date", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Multi_Set_Test

        #region Multi_Where_Test

        [Fact]
        public void Multi_Where_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match("(f:Foo)")
                            .Where("f", "Name")
                            .And
                            .Where("f", "Date");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (f:Foo) WHERE f.Name = $f_Name AND f.Date = $f_Date", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Multi_Where_Test
    }
}