using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;


namespace Weknow.Cypher.Builder
{
        [Trait("Segment", "Expression")]
    public class ComplexExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ComplexExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region ComplexExpression_Test

        [Fact]
        public void ComplexExpression_Test()
        {
            CypherCommand cypher = _(a => r1 => b => r2 => c =>
             Match(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
             .Where(a.OfType<Foo>().Name == "Avi")
             .Return(a.OfType<Foo>().Name, r1, b.All<Bar>(), r2, c)
             .OrderBy(a.OfType<Foo>().Name)
             .Skip(1)
             .Limit(10));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (a:Person)-[r1:KNOWS]->(b:Person)<-[r2:KNOWS]-(c:Person)
WHERE a.Name = $p_0
RETURN a.Name, r1, b.Id, b.Name, b.Date, r2, c
ORDER BY a.Name
SKIP $p_1
LIMIT $p_2", cypher.Query);

			 Assert.Equal("Avi", cypher.Parameters["p_0"]);
            _outputHelper.WriteLine(cypher);
			 Assert.Equal(1, cypher.Parameters["p_1"]);
            _outputHelper.WriteLine(cypher);
			 Assert.Equal(10, cypher.Parameters["p_2"]);
        }

        #endregion // ComplexExpression_Test
    }
}

