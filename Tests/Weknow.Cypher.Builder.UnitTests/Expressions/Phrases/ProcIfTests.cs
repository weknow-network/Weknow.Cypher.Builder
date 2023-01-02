using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.ICypherHigerAbstraction;
using static Weknow.CypherBuilder.Schema;
using Weknow.CypherBuilder.Declarations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Procedures")]

    public class ProcIfTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ProcIfTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region FOREACH (_rnd_ IN CASE WHEN $p IS NULL THEN [1] ELSE [] END |

        [Fact]
        public void Foreach_Parameters_Test()
        {
            ParameterDeclaration p = Parameters.Create();

            CypherCommand cypher = _(item =>
                                        Proc().If(p.IsNull(), Set(item, new { Version = 1 }))
                                        .Proc().If(p.IsNotNull(), Set(item, new { Version = 2 })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"FOREACH (var_0 IN CASE WHEN $p IS NULL THEN [1] ELSE [] END |{NewLine}\t" +
                $"SET item.Version = $p_1){NewLine}" +
                $"FOREACH (var_1 IN CASE WHEN $p IS NOT NULL THEN [1] ELSE [] END |{NewLine}\t" +
                "SET item.Version = $p_2)",
                cypher.Query);
            Assert.Null(cypher.Parameters["p"]);
            Assert.Equal(1, cypher.Parameters["p_1"]);
            Assert.Equal(2, cypher.Parameters["p_2"]);
            Assert.Equal(3, cypher.Parameters.Count);
        }

        #endregion // FOREACH (_rnd_ IN CASE WHEN $p IS NULL THEN [1] ELSE [] END |
    }
}

