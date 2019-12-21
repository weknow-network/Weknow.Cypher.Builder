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


        #region WhereCollection_WithVariable_Test

        [Fact]
        public void WhereCollection_WithVariable_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Match($"(n:Foo)")
                            .Where("n", "Name", "Id");

            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal("MATCH (n:Foo) WHERE n.Name = $n_Name AND n.Id = $n_Id", cypherCommand.CypherLine);
        }

        #endregion // WhereCollection_WithVariable_Test
    }
}