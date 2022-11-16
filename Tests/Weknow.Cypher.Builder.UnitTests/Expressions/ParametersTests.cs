using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.Cypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
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

        #region Parameters_CreateMulti_T_Deconstruct_Test

        [Fact]
        public void Parameters_CreateMulti_T_Deconstruct_Test()
        {
            var a1 = Parameters.CreateMulti<Foo>();
            var (a2, b2) = Parameters.CreateMulti<Foo>();
            var (a3, b3, c3) = Parameters.CreateMulti<Foo>();
            var (a4, b4, c4, d4) = Parameters.CreateMulti<Foo>();
            var (a5, b5, c5, d5, e5) = Parameters.CreateMulti<Foo>();
            var (a6, b6, c6, d6, e6, f6) = Parameters.CreateMulti<Foo>();
            var (a7, b7, c7, d7, e7, f7, g7) = Parameters.CreateMulti<Foo>();
            var (a8, b8, c8, d8, e8, f8, g8, h8) = Parameters.CreateMulti<Foo>();
            var (a9, b9, c9, d9, e9, f9, g9, h9, i9) = Parameters.CreateMulti<Foo>();
            var (a10, b10, c10, d10, e10, f10, g10, h10, i10, j10) = Parameters.CreateMulti<Foo>();
        }

        #endregion // Parameters_CreateMulti_T_Deconstruct_Test 

        #region UNWIND $items AS item MATCH (n:Person { Id: $Id })

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

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: $Id }) 
    }
}