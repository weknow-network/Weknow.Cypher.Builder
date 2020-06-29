using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    public class NodeExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public NodeExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Node_SingleSchema_Property_Test

        [Fact]
        public void Node_SingleSchema_Property_Test()
        {
            //string cypher = _(n => Match(N(n, Person, "Id")));
            var pattern = Reuse(n => N(n, Person, P(Id)));

            _outputHelper.WriteLine(pattern.ToString());

            Assert.Equal(@"(n:Person { Id: $Id })", pattern.ToString());
        }

        #endregion // Node_SingleSchema_Property_Test

        #region Node_LabelOnly_Test

        [Fact]
        public void Node_LabelOnly_Test()
        {
            string cypher1 = _(() => Match(N(Person)));
            string cypher2 = _(n => Match(N(Person)));

            Assert.Equal(cypher1, cypher2);
        }

        #endregion // Node_LabelOnly_Test
    }
}

