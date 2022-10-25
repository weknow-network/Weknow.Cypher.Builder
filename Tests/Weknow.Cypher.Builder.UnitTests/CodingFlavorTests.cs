using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Flavor")]
    public class CodingFlavorTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CodingFlavorTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Empty_vs_variable_Test

        [Fact]
        public void Empty_vs_variable_Test()
        {
            string cypher1 = _(() => Match(N(Person)));
            string cypher2 = _(n => Match(N(Person)));

            _outputHelper.WriteLine(cypher1);
            _outputHelper.WriteLine(cypher2);
			 Assert.Equal(cypher1, cypher2);
        }

        #endregion // Empty_vs_variable_Test
    }
}

