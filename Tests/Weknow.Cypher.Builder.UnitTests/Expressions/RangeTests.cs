using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
        [Trait("Segment", "Expression")]
    public class WhereXTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public WhereXTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Range_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[1..5] >
            //                        N(m)));

            //_outputHelper.WriteLine(cypher);
			// Assert.Equal("MATCH (n)-[*1..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_Test

        #region Range_FromAny_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_FromAny_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[..5] >
            //                        N(m)));

            //_outputHelper.WriteLine(cypher);
			// Assert.Equal("MATCH (n)-[*..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_FromAny_Test

        #region Range_WithVar_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_WithVar_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[r, 1..5] >
            //                        N(m)));

            //_outputHelper.WriteLine(cypher);
			// Assert.Equal("MATCH (n)-[r*1..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_WithVar_Test

        #region Range_WithVarAndProp_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_WithVarAndProp_Test()
        {
            //    CypherCommand cypher = _(n => r => m =>
            //                            Match(N(n) -
            //                            R[r, KNOWS, P(PropA), 1..5] >
            //                            N(m)));

            //    _outputHelper.WriteLine(cypher);
			// Assert.Equal("MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_WithVarAndProp_Test

        #region Range_Infinit_Test

        [Fact(Skip = "Expression don't support Range")]
        public void Range_Infinit_Test()
        {
            //CypherCommand cypher = _(n => r => m =>
            //                        Match(N(n) -
            //                        R[..] >
            //                        N(m)));

            //_outputHelper.WriteLine(cypher);
			// Assert.Equal("MATCH (n)-[*]->(m)", cypher.Query);
            throw new NotSupportedException();
        }

        #endregion // Range_Infinit_Test

        #region Range_Enum_AtMost_Test

        [Fact]
        public void Range_Enum_AtMost_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[Rng.AtMost(5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)-[*..5]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_AtMost_Test

        #region Range_Enum_AtLeast_Test

        [Fact]
        public void Range_Enum_AtLeast_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[Rng.AtLeast(3)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)-[*3..]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_AtLeast_Test

        #region Range_Enum_WithVar_Test

        [Fact]
        public void Range_Enum_WithVar_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[r, Rng.Scope(1,5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)-[r*1..5]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_WithVar_Test

        #region Range_Enum_WithVarAndProp_Test

        [Fact]
        public void Range_Enum_WithVarAndProp_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[r, KNOWS, P(PropA), Rng.Scope(1, 5)] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)-[r:KNOWS { PropA: $PropA } *1..5]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_WithVarAndProp_Test

        #region Range_Enum_Infinit_Test

        [Fact]
        public void Range_Enum_Infinit_Test()
        {
            CypherCommand cypher = _(n => r => m =>
                                    Match(N(n) -
                                    R[Rng.Any()] >
                                    N(m)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n)-[*]->(m)", cypher.Query);
        }

        #endregion // Range_Enum_Infinit_Test
    }
}