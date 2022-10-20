using System;

using Weknow.Cypher.Builder.Declarations;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;
using static System.Environment;

namespace Weknow.Cypher.Builder
{
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
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n / Return_Test

        #region MATCH (n) RETURN n.Id / Return_Prop_Test

        [Fact]
        public void Return_Prop_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(n._.Id));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "RETURN n.Id", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id / Return_Prop_Test

        #region MATCH (n) RETURN n.Id, n.PropA / Return_Props_Test

        [Fact]
        public void Return_Props_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(n._.Id, n._.PropA));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "RETURN n.Id, n.PropA", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id, n.PropA / Return_Prop_Test

        #region MATCH (n) RETURN n.Id, n.PropA, n.PropB / Return_Props_Lambda_Test

        [Fact]
        public void Return_Props_Lambda_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .Return(n._.Id, n._.PropA, n._.PropB));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"MATCH (n){NewLine}" +
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
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "RETURN n.Id, n.PropA, n.PropB", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id, n.PropA, n.PropB / Return_Objects_Test

        #region MATCH (n) RETURN DISTINCT n / ReturnDistinct_Test

        [Fact]
        public void ReturnDistinct_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n))
                                    .ReturnDistinct(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "RETURN DISTINCT n", cypher.Query);
        }

        #endregion // MATCH (n) RETURN DISTINCT n / ReturnDistinct_Test

        #region MATCH (n) RETURN DISTINCT n.Name / ReturnDistinct_Obj_Test

        [Fact]
        public void ReturnDistinct_Obj_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
                                    Match(N(n))
                                    .ReturnDistinct(n._.Name));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"MATCH (n){NewLine}" +
                            "RETURN DISTINCT n.Name", cypher.Query);
        }

        #endregion // MATCH (n) RETURN DISTINCT n.Name / ReturnDistinct_Obj_Test
    }
}

