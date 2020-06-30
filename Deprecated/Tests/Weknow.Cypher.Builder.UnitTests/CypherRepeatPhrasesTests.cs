using Xunit;
using Xunit.Abstractions;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    [Trait("Segment", "Deprecate")]
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
            var cypherCommand = CypherBuilder.Create(cfg => cfg.Naming.NodeLabelConvention = CypherNamingConvention.SCREAMING_CASE)
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
            var cypherCommand = CypherBuilder.Create(cfg => cfg.Naming.NodeLabelConvention = CypherNamingConvention.SCREAMING_CASE)
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
            var cypherCommand = CypherBuilder.Create(cfg => cfg.Naming.NodeLabelConvention = CypherNamingConvention.SCREAMING_CASE)
                            .Match("(f:Foo)")
                            .Set("f", "Name")
                            .Set("f", "Date");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (f:Foo) SET f.Name = $Name , f.Date = $Date", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Multi_Set_Test

        #region Multi_Where_Test

        [Fact]
        public void Multi_Where_Test()
        {
            var cypherCommand = CypherBuilder.Create(cfg => cfg.Naming.NodeLabelConvention = CypherNamingConvention.SCREAMING_CASE)
                            .Match("(f:Foo)")
                            .Where("f", "Name")
                            .And
                            .Where("f", "Date");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (f:Foo) WHERE f.Name = $Name AND f.Date = $Date ", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Multi_Where_Test

        #region Multi_Where_WithPrefix_Test

        [Fact]
        public void Multi_Where_WithPrefix_Test()
        {
            var cypherCommand = CypherBuilder.Create(cfg => cfg.Naming.NodeLabelConvention = CypherNamingConvention.SCREAMING_CASE)
                            .Match("(f:Foo)")
                            .Where<Foo>(f => f.Name, "f_", string.Empty, "<")
                            .And
                            .Where("f", "Date");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (f:Foo) WHERE f.Name < f_Name AND f.Date = $Date ", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Multi_Where_WithPrefix_Test
    }
}