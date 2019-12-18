using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherFactoryMathematicTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherFactoryMathematicTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region TODO_Test

        [Fact]
        public void TODO_Test()
        {
            // please test me
        }

        #endregion // TODO_Test

    }
}