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

        #region Properties_Test

        [Fact]
        public void Properties_Test()
        {
            CypherCommand cypher = _(n => Match(N(n, Person, P(PropA, PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Test

        #region Properties_OfT_DefaultLabel_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultLabel_Test

        #region Properties_OfT_DefaultLabel_AvoidDuplication_Test

        [Fact]
        public void Properties_OfT_DefaultLabel_AvoidDuplication_Test()
        {
            CypherCommand cypher = _<Foo>(n => Match(N(n, P(n.P.PropA, n.P.PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultLabel_AvoidDuplication_Test

        #region Properties_OfT_DefaultAndAdditionLabel_Test

        [Fact]
        public void Properties_OfT_DefaultAndAdditionLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, Person, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_DefaultAndAdditionLabel_Test

        #region Properties_OfT_Test

        [Fact]
        public void Properties_OfT_Test()
        {
            CypherCommand cypher = _(n => Match(N(n, Person, P(n.As<Foo>().PropA, n.As<Foo>().PropB))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_OfT_Test

        #region Properties_WithPrefix_Test

        [Fact]
        public void Properties_WithPrefix_Test()
        {
            CypherCommand cypher = _(n1 => n2 => n2_ =>
                                    Match(N(n1, Person, P(PropA, PropB)) -
                                          R[n1, KNOWS] >
                                          N(n2, Person, Pre(n2_, P(PropA, PropB)))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n1:Person { PropA: $PropA, PropB: $PropB })-" +
                         "[n1:KNOWS]->" +
                         "(n2:Person { PropA: $n2_PropA, PropB: $n2_PropB })", cypher.Query);
        }

        #endregion // Properties_WithPrefix_Test

        #region Properties_Convention_WithDefaultLabel_Test

        [Fact]
        public void Properties_Convention_WithDefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, Convention(name => name.StartsWith("Prop")))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_WithDefaultLabel_Test

        #region Properties_All_WithDefaultLabel_Test

        [Fact]
        public void Properties_All_WithDefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, All())));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo { Id: $Id, Name: $Name, PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_All_WithDefaultLabel_Test

        #region Properties_All_Except_WithDefaultLabel_Test

        [Fact]
        public void Properties_All_Except_WithDefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, AllExcept(n.As<Foo>().Id, n.As<Foo>().Name))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_All_Except_WithDefaultLabel_Test

        #region Properties_Convention_Test

        [Fact]
        public void Properties_Convention_Test()
        {
            CypherCommand cypher = _(n => Match(N(n, Person, Convention<Foo>(name => name.StartsWith("Prop")))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (n:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // Properties_Convention_Test

        #region Properties_Const_Test

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

        #endregion // Properties_Const_Test
    }
}

