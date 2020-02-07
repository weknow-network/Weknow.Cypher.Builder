using System;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    public class HigherOrderConcurrencyTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public HigherOrderConcurrencyTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        // TODO: Move Higher order component tests
        #region Concurrency_Pattern_Test

        [Fact]
        public void Concurrency_Pattern_Test()
        {
            //CypherCommand cypher = P(n =>
            //                        Merge(N(n, Person, P(PropA, PropB, Concurrency))),
            //                        SET(eTag(n, Concurrency));

            //Assert.Equal(@"
            //Merge (n:Person { PropA: $PropA, PropB: $PropB, Concurrency: $Concurrency })
            //ON CREATE SET n.Concurrency = 1
            //ON MATCH SET n.Concurrency = n.Concurrency + 1", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Concurrency_Pattern_Test

        // TODO: Move to Higher component tests
        #region Unwind_Concurrency_Pattern_Test

        [Fact]
        public void Unwind_Concurrency_Pattern_Test()
        {
            //CypherCommand cypher = P(items => item => n =>
            //                        Unwind(items, item,
            //                        Merge(N(n, Person, P(PropA, PropB, Concurrency))),
            //                        SET(eTag(n, Concurrency)));

            //Assert.Equal(@"UNWIND $items as item
            //Merge (n:Person { PropA: item.PropA, PropB: item.PropB, Concurrency: $Concurrency })
            //ON CREATE SET n.Concurrency = 1
            //ON MATCH SET n.Concurrency = n.Concurrency + 1", cypher.Query);
            throw new NotImplementedException();
        }

        #endregion // Unwind_Concurrency_Pattern_Test
    }
}

