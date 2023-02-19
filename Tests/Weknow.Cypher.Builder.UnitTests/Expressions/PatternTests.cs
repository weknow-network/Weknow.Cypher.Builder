#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]

    public class PatternTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public PatternTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region (a)-[r1]->(b)<-[r2]-(c) 

        [Fact]
        public void Reuse_Complex6_Broken_Test()
        {
            var pattern = _((p, n1,n2, n3, n4, r1, r2, r3) => Match(p,
                       N(n1) - R[r1] > N(n2) < R[r2] - N(n3) - R[r3] > N(n4) ));


            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"MATCH p = (n1)-[r1]->(n2)<-[r2]-(n3)-[r3]->(n4)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b)<-[r2]-(c) 
    }
}

