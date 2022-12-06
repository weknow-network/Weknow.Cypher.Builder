using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]
    
    public class CaseTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CaseTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n) RETURN * / Return_Star_Test

        [Fact]
        public void Return_Star_Test()
        {
            var n =  Variables.Create<Foo>();
            CypherCommand cypher = _(r => m => v =>
                                    Match(N(n) - R[r] > N(m))
                                    .Return()
                                    .Case()
                                        .When(n, Person & Friend).Then(1)
                                        .When(r,!LIKE & !KNOWS).Then(2)
                                        .Else(-1)
                                    .End(v),
                                    cfg => cfg.Flavor = CypherFlavor.Neo4j5);
            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n)-[r]->(m){NewLine}" +
                         $"RETURN{NewLine}" +
                         $"CASE{NewLine}" +
                         $"\tWHEN n:Person&Friend THEN $p_0{NewLine}" +
                         $"\tWHEN r:!LIKE&!KNOWS THEN $p_1{NewLine}" +
                         $"\tELSE $p_2{NewLine}" +
                         $"END AS v"
                           , cypher.Query);
            //CypherCommand cypher = _(r => m => v =>
            //                        Match(N(n) - R[r] > N(m))
            //                        .Return()
            //                        .Case(n.__.Version)
            //                        .When(1, 1)
            //                        .End(v));

            //_outputHelper.WriteLine(cypher);
            //Assert.Equal($"MATCH (n)-[r]->(m){NewLine}" +
            //             $"RETURN{NewLine}" +
            //             $"CASE n.Version{NewLine}" +
            //             $"\tWHEN {NewLine}" +
            //             $"END as v"
            //               , cypher.Query);
        }

        #endregion // MATCH (n) RETURN * / Return_Star_Test
    }
}

