using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Return")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class ReturnTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ReturnTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n) RETURN n / Return_Test

        [Fact]
        public void Return_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)\r\n" +
                            "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n / Return_Test

        #region MATCH (n) RETURN n.Id / Return_Prop_Test

        [Fact]
        public void Return_Prop_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n))
                                    .Return(n.P(Id)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)\r\n" +
                            "RETURN n.Id", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id / Return_Prop_Test

        #region MATCH (n) RETURN n.Id, n.PropA / Return_Props_Test

        [Fact]
        public void Return_Props_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n))
                                    .Return(n.P(Id, PropA)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)\r\n" +
                            "RETURN n.Id, n.PropA", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id, n.PropA / Return_Prop_Test

        #region MATCH (n) RETURN n.Id, n.PropA, n.PropB / Return_Props_Lambda_Test

        [Fact]
        public void Return_Props_Lambda_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n))
                                    .Return(n.P(Id, P<Foo>(x => x.PropA, x => x.PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)\r\n" +
                            "RETURN n.Id, n.PropA, n.PropB", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id, n.PropA, n.PropB / Return_Props_Lambda_Test

        #region MATCH (n) RETURN n.Id, n.PropA, n.PropB / Return_Objects_Test

        [Fact]
        public void Return_Objects_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n))
                                    .Return("n.Id", "n.PropA", "n.PropB"));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)\r\n" +
                            "RETURN n.Id, n.PropA, n.PropB", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id, n.PropA, n.PropB / Return_Objects_Test
    }
}
