using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;
using static System.Environment;

namespace Weknow.Cypher.Builder
{
        [Trait("Segment", "Expression")]
    public class PropertiesExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public PropertiesExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Test

        [Fact]
        public void Properties_Test()
        {
            var n = Variables.Create();
            var p1 = Parameters.Create<Foo>();
            IPattern pattern = Reuse(() => 
                        N(n, Person, 
                                new
                                {
                                    p1._.PropA,
                                    p1._.PropB
                                }));

            var cypher = pattern.ToString();
            _outputHelper.WriteLine(cypher);
            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Test

        #region (n:Person { Id: $Id }) / Properties_Lambda_Test

        [Fact]
        public void Properties_Lambda_Test()
        {
            var n = Variables.Create();
            var Length = Parameters.Create();

            IPattern pattern = Reuse(() => N(n, Person, new { Length = Length }));

            var cypher = pattern.ToString();
            _outputHelper.WriteLine(cypher);
            Assert.Equal("(n:Person { Length: $Length })", cypher);
        }

        #endregion // (n:Person { Id: $Id }) / Properties_Lambda_Test

        #region (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Test

        [Fact]
        public void Properties_P_Test()
        {
            var (PropA, Prop_B) = Parameters.CreateMulti();
            var n = Variables.Create();

            IPattern pattern = Reuse(() => N(n, Person, new { PropA, PropB = Prop_B }));

            var cypher = pattern.ToString();
            _outputHelper.WriteLine(cypher);
            Assert.Equal("(n:Person { PropA: $PropA, PropB: $Prop_B })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Test

        #region MATCH MATCH (n:Person { Id: $Id } SET n.PropA = $PropA / Properties_Match_Set_nameof_Test_Test

        [Fact]
        public void Properties_Match_Set_nameof_Test_Test()
        {
            var p = Parameters.Create<Foo>();
            var n = Variables.Create();

            CypherCommand cypher = _(() => Match(N(n, Person, new { p._.Id }))
                                            .Set(n, new { p._.PropA }));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Person { Id: $Id })\r\n" +
                            "SET n.PropA = $PropA", cypher.Query);
        }

        #endregion // MATCH MATCH (n:Person { Id: $Id } SET n.PropA = $PropA / Properties_Match_Set_nameof_Test_Test

        #region MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_OfT_DefaultLabel_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_Test()
        {
            var n = Variables.Create();
            var p = Parameters.Create<Foo>();

            CypherCommand cypher = _(() => Match(N(n, Person,
                                                new { p._.PropA, p._.PropB })));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_OfT_DefaultLabel_Test

        #region MATCH (n:Foo { PropA: $PropA, Date: $Date }) / Properties_OfTT_DefaultLabel_AvoidDuplication_Test

        [Fact]
        public void Properties_OfTT_DefaultLabel_AvoidDuplication_Test()
        {
            var (f, b) = Parameters.CreateMulti<Foo, Bar>();
            var n = Variables.Create();

            CypherCommand cypher = _(() => Match(N(n, Person, new { f._.PropA, b._.Date })));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Person { PropA: $PropA, Date: $Date })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, Date: $Date }) / Properties_OfTT_DefaultLabel_AvoidDuplication_Test

        #region (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_OfT_Test

        [Fact]
        public void Properties_OfT_Test()
        {
            var n = Variables.Create<Foo>(); 
            var p = Parameters.Create<Foo>(); 

            var pattern = Reuse(() => N(n, Person, 
                                            new { p._.PropA, p._.PropB }));
            var cypher = pattern.ToString();

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_OfT_Test

        #region UNWIND $items AS item MERGE (n:Person { Id: item }) RETURN n / Properties_Const_Test

        [Fact]
        public void Properties_Const_Test()
        {
            CypherCommand cypher =  _(items => item => n =>
                                    Unwind(items, item,
                                    Merge(N(n, Person, new { Id = item }))
                                    .Return(n)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"UNWIND $items AS item{NewLine}" +
                            $"MERGE (n:Person {{ Id: item }}){NewLine}" +
                             "RETURN n", cypher.Query);
        }

        #endregion // UNWIND $items AS item MERGE (n:Person { Id: item }) RETURN n / Properties_Const_Test
    }
}

