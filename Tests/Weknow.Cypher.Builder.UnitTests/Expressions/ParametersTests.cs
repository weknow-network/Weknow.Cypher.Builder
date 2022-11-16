using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Execution")]
    public class ParametersTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ParametersTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region UNWIND $items AS item MATCH (n:Person { Id: $Id }) / Parameters_Unwind_NonWindProp_T_Test

        [Fact]
        public void Parameters_Unwind_NonWindProp_T_Test()
        {
            var (items, Id) = Parameters.CreateMulti();
            CypherCommand cypher = _(item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, new { Id }))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: $Id })", cypher.Query);
            Assert.True(cypher.Parameters.ContainsKey("items"), "ContainsKey items");
            Assert.True(cypher.Parameters.ContainsKey(nameof(Id)), "ContainsKey Id");
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: $Id }) / Parameters_Unwind_NonWindProp_T_Test
    }
}

