using System.Collections;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherWhereTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherWhereTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Match_WhereWithVariable_Test

        [Fact]
        public void Match_WhereWithVariable_Test()
        {
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo)")
                            .Where("n", "Name");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $n_Name", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Match_WhereWithVariable_Test

        #region WhereCollection_WithVariable_Test

        [Fact]
        public void WhereCollection_WithVariable_Test()
        {
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo)")
                            .Where("n", "Name", "Id");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $n_Name AND n.Id = $n_Id", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // WhereCollection_WithVariable_Test

        #region WhereCollection_WithVariable_Test

        [Fact]
        public void WhereCollection__WithVariable_Test()
        {
            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo)")
                            .Where<Foo>(n => n.Id, ">")
                            .And.Where("n", "Date")
                            .Or.Where("n", "PropA".ToYield("PropB"));

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo) WHERE n.Id > $n_Id AND n.Date = $n_Date OR n.PropA = $n_PropA AND n.PropB = $n_PropB", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // WhereCollection_WithVariable_Test
    }
}