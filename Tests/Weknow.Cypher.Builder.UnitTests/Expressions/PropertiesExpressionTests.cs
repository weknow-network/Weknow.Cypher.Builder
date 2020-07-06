using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;
using static System.Environment;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Property")]
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
            IPattern pattern = Reuse(n => N(n, Person, PropA, PropB));

            var cypher = pattern.ToString();
            _outputHelper.WriteLine(cypher);
            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Test

        #region (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Test

        [Fact]
        public void Properties_P_Test()
        {
            IPattern pattern = Reuse(n => N(n, Person, P(PropA, PropB)));

            var cypher = pattern.ToString();
            _outputHelper.WriteLine(cypher);
            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Test

        #region (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Match_Test

        [Fact]
        public void Properties_Match_Test()
        {
            IPattern pattern = Reuse(n => N(n, Person, P(PropA, PropB)));
            var cypher = pattern.ToString();

            _outputHelper.WriteLine(cypher);
            Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Match_Test

        #region MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_OfT_DefaultLabel_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_OfT_DefaultLabel_Test

        #region MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_OfT_DefaultLabel_AvoidDuplication_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_AvoidDuplication_Test()
        {
            CypherCommand cypher = _<Foo>(n => Match(N(n, P(n.P.PropA, n.P.PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) /Properties_OfT_DefaultLabel_AvoidDuplication_Test

        #region (n:Foo:Person { PropA: $PropA, PropB: $PropB }) / Properties_OfT_DefaultAndAdditionLabel_Test

        [Fact]
        public void Properties_OfT_DefaultAndAdditionLabel_Test()
        {
            IPattern pattern = Reuse(n => N<Foo>(n, Person, P(n.As<Foo>().PropA, n.As<Foo>().PropB)));
            var cypher = pattern.ToString();

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Foo:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Foo:Person { PropA: $PropA, PropB: $PropB }) / Properties_OfT_DefaultAndAdditionLabel_Test

        #region (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_OfT_Test

        [Fact]
        public void Properties_OfT_Test()
        {
            IPattern pattern = Reuse(n => N(n, Person, P(n.As<Foo>().PropA, n.As<Foo>().PropB)));
            var cypher = pattern.ToString();

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_OfT_Test

        #region (n:Person { PropA: $nPropA }) / Properties_WithPrefix_Light_Test

        [Fact]
        public void Properties_WithPrefix_Light_Test()
        {
            IPattern pattern = Reuse(n => N(n, Person, _P(n, PropA)));

            string? cypher = pattern?.ToString();
            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Person { PropA: $nPropA })", cypher);
        }

        #endregion // (n:Person { PropA: $nPropA }) / Properties_WithPrefix_Light_Test

        #region (n:Person { PropA: $n_PropA }) / Properties_WithPrefixMulti_Light_Test

        [Fact]
        public void Properties_WithPrefixMulti_Light_Test()
        {
            IPattern? pattern = Reuse(n => n_ => N(n, Person, _P(n_, PropA)));

            string? cypher = pattern?.ToString();
            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Person { PropA: $n_PropA })", cypher);
        }

        #endregion // (n:Person { PropA: $n_PropA }) / Properties_WithPrefixMulti_Light_Test

        #region (n:Person { PropA: $n_PropA }) / Properties_WithPrefix_Test

        [Fact]
        public void Properties_WithPrefix_Test()
        {
            IPattern pattern = Reuse(n => n_ => N(n, Person, _P(n_, P(PropA))));

            string? cypher = pattern?.ToString();
            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Person { PropA: $n_PropA })", cypher);
        }

        #endregion // (n:Person { PropA: $n_PropA }) / Properties_WithPrefix_Test

        #region MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-[n1:KNOWS]->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB }) / Properties_WithPrefix_Complex_Test

        [Fact]
        public void Properties_WithPrefix_Complex_Test()
        {
            CypherCommand cypher = _(n1 => n2 => n2_ =>
                                    Match(N(n1, Person, P(PropA, PropB)) -
                                          R[n1, KNOWS] >
                                          N(n2, Person, _P(n2_, P(PropA, PropB)))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-" +
                         "[n1:KNOWS]->" +
                         "(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-[n1:KNOWS]->(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB }) / Properties_WithPrefix_Complex_Test

        #region (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_Convention_WithDefaultLabel_Test

        [Fact]
        public void Properties_Convention_WithDefaultLabel_Test()
        {
            IPattern pattern = Reuse(n => N<Foo>(n, Convention(name => name.StartsWith("Prop"))));
            string? cypher = pattern?.ToString();

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Foo { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_Convention_WithDefaultLabel_Test

        #region (n:Foo { Id: $Id, Name: $Name, PropA: $PropA, PropB: $PropB } / Properties_All_WithDefaultLabel_Test

        [Fact]
        public void Properties_All_WithDefaultLabel_Test()
        {
            IPattern pattern = Reuse(n => N<Foo>(n, All()));
            string? cypher = pattern?.ToString();

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Foo { Id: $Id, Name: $Name, PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Foo { Id: $Id, Name: $Name, PropA: $PropA, PropB: $PropB } / Properties_All_WithDefaultLabel_Test

        #region (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_All_Except_WithDefaultLabel_Test

        [Fact]
        public void Properties_All_Except_WithDefaultLabel_Test()
        {
            IPattern pattern = Reuse(n => N<Foo>(n, AllExcept(n.As<Foo>().Id, n.As<Foo>().Name)));
            string? cypher = pattern?.ToString();

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Foo { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Foo { PropA: $PropA, PropB: $PropB }) / Properties_All_Except_WithDefaultLabel_Test

        #region (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Convention_Test

        [Fact]
        public void Properties_Convention_Test()
        {
            IPattern pattern = Reuse(n => N(n, Person, Convention<Foo>(name => name.StartsWith("Prop"))));
            string? cypher = pattern?.ToString();

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("(n:Person { PropA: $PropA, PropB: $PropB })", cypher);
        }

        #endregion // (n:Person { PropA: $PropA, PropB: $PropB }) / Properties_Convention_Test

        #region UNWIND $items AS item MERGE (n:Person { Id: item }) RETURN n / Properties_Const_Test

        [Fact]
        public void Properties_Const_Test()
        {
            CypherCommand cypher =  _(items => item => n =>
                                    Unwind(items,
                                    Merge(N(n, Person, P(Id, item)))
                                    .Return(n)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal($"UNWIND $items AS item{NewLine}" +
                            $"MERGE (n:Person {{ Id: item }}){NewLine}" +
                             "RETURN n", cypher.Query);
        }

        #endregion // UNWIND $items AS item MERGE (n:Person { Id: item }) RETURN n / Properties_Const_Test
    }
}

