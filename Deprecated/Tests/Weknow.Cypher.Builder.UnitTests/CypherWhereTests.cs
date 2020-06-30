using System.Collections;

using Xunit;
using Xunit.Abstractions;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    [Trait("Segment", "Deprecate")]
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
            var cypherCommand = CypherBuilder.Create()
                            .Match($"(n:Foo)")
                            .Where("n", "Name");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $Name ", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Match_WhereWithVariable_Test

        #region WhereCollection_WithVariable_Test

        [Fact]
        public void WhereCollection_WithVariable_Test()
        {
            var cypherCommand = CypherBuilder.Create()
                            .Match($"(n:Foo)")
                            .Where("n", "Name", "Id");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $Name AND n.Id = $Id ", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // WhereCollection_WithVariable_Test

        #region WhereCollection_WithVariable_Test

        [Fact]
        public void WhereCollection__WithVariable_Test()
        {
            var cypherCommand = CypherBuilder.Create()
                            .Match($"(n:Foo)")
                            .Where<Foo>(n => n.Id, compareSign: ">")
                            .And.Where("n", "Date")
                            .Or.Where("n", "PropA".ToYield("PropB"));

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("MATCH (n:Foo) WHERE n.Id > $Id AND n.Date = $Date OR n.PropA = $PropA AND n.PropB = $PropB ", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // WhereCollection_WithVariable_Test
    }
}