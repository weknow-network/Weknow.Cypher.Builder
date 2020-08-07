using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
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
            CypherCommand cypher = _(items => item => n => 
                                    Unwind(items, item,
                                    Match(N(n, Person, P("Id")))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: $Id })", cypher.Query);
            Assert.True(cypher.Parameters.ContainsKey("items"), "ContainsKey items");
            Assert.True(cypher.Parameters.ContainsKey(nameof(Id)), "ContainsKey Id");
            Assert.True(cypher.Parameters.ContainsKey(nameof(PropB)), "ContainsKey PropB");
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: $Id }) / Parameters_Unwind_NonWindProp_T_Test
    }
}

