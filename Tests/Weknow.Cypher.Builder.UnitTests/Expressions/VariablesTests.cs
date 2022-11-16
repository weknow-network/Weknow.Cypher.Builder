using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Execution")]
    public class VariablesTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public VariablesTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Variables_CreateMulti_T_Deconstruct_Test

        [Fact]
        public void Variables_CreateMulti_T_Deconstruct_Test()
        {
            var a1 = Variables.CreateMulti<Foo>();
            var (a2, b2) = Variables.CreateMulti<Foo>();
            var (a3, b3, c3) = Variables.CreateMulti<Foo>();
            var (a4, b4, c4, d4) = Variables.CreateMulti<Foo>();
            var (a5, b5, c5, d5, e5) = Variables.CreateMulti<Foo>();
            var (a6, b6, c6, d6, e6, f6) = Variables.CreateMulti<Foo>();
            var (a7, b7, c7, d7, e7, f7, g7) = Variables.CreateMulti<Foo>();
            var (a8, b8, c8, d8, e8, f8, g8, h8) = Variables.CreateMulti<Foo>();
            var (a9, b9, c9, d9, e9, f9, g9, h9, i9) = Variables.CreateMulti<Foo>();
            var (a10, b10, c10, d10, e10, f10, g10, h10, i10, j10) = Variables.CreateMulti<Foo>();
        }

        #endregion // Variables_CreateMulti_T_Deconstruct_Test 
    }
}

