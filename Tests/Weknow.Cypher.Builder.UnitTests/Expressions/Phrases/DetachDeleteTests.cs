using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;
using static System.Environment;

namespace Weknow.Cypher.Builder
{
        [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class DetachDeleteTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public DetachDeleteTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n) DELETE n / Delete_Test

        [Fact]
        public void Delete_Test()
        {
            var n = Variables.Create();
            CypherCommand cypher = _(() => Match(N(n))
                                    .Delete(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "DELETE n", cypher.Query);
        }

        #endregion // MATCH (n) DELETE n / Delete_Test

        #region MATCH (n) DETACH DELETE n / Detach_Delete_Test

        [Fact]
        public void Detach_Delete_Test()
        {
            var n = Variables.Create();
            CypherCommand cypher = _(() => Match(N(n))
                                    .DetachDelete(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "DETACH DELETE n", cypher.Query);
        }

        #endregion // MATCH (n) DETACH DELETE n / Detach_Delete_Test
    }
}

