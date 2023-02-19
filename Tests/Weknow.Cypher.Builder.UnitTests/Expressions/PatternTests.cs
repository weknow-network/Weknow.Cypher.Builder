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

        #region MATCH p = (n1)-[r1]-(n2)

        [Fact]
        public void UndirectedPath_Test()
        {
            //var (n, r) = Variables.CreateMulti();

            //Assert.True(N(n) - R[r] is INodeRelation);
            //Assert.True(N(n) - R[r] - N(n) is INode);

            var pattern = _((p, n1,n2, n3, n4, r1, r2, r3) => Match(p,
                       N(n1) - R[r1] - N(n2) ));


            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"MATCH p = (n1)-[r1]-(n2)", pattern.ToString());
        }

        #endregion // MATCH p = (n1)-[r1]-(n2)

        #region MATCH p = (n1)-[r1]->(n2)<-[r2]-(n3)-[r3]->(n4)

        [Fact]
        public void Path7_Test()
        {
            //var (n, r) = Variables.CreateMulti();
            //Assert.True(N(n) - R[r] is INodeRelation);
            //Assert.True(N(n) - R[r] > N(n) is INode);
            //Assert.True(N(n) - R[r] > N(n) < R[r] is IRelation);
            //Assert.True(N(n) - R[r] > N(n) < R[r] - N(n) is INode);
            //Assert.True(N(n) - R[r] > N(n) < R[r] - N(n) - R[r] is INode); // TODO: [bnaya 2023-02-19] review it
            //Assert.True(N(n) - R[r] > N(n) < R[r] - N(n) - R[r] > N(n) is INode);

            var pattern = _((p, n1,n2, n3, n4, r1, r2, r3) => Match(p,
                       N(n1) - R[r1] > N(n2) < R[r2] - N(n3) - R[r3] > N(n4) ));


            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"MATCH p = (n1)-[r1]->(n2)<-[r2]-(n3)-[r3]->(n4)", pattern.ToString());
        }

        #endregion // MATCH p = (n1)-[r1]->(n2)<-[r2]-(n3)-[r3]->(n4)
    }
}

