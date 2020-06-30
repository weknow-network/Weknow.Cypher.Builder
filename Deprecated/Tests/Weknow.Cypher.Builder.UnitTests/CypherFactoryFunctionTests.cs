using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    [Trait("Segment", "Deprecate")]
    public class CypherFactoryFunctionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherFactoryFunctionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Coalesce_Test

        [Fact]
        public void Coalesce_Test()
        {
            var cypher = F.Coalesce("a", "$b", "c");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("coalesce(a, $b, c)", cypher);
        }

        [Fact]
        public void Coalesce_Composite_Test()
        {
            var cypher = F.Coalesce(m =>
                                m.Composite(
                                    L.Reduce("s", "''", "x", "listA", "s + x.prop"),
                                    ",",
                                    L.Reduce("s", "''", "x", "listB", "s + x.prop")));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("coalesce( reduce(s = '', x IN listA | s + x.prop), reduce(s = '', x IN listB | s + x.prop))", cypher.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Coalesce_Test

        #region Labels_Test

        [Fact]
        public void Labels_Test()
        {
            var cypher = F.Labels("a");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("labels(a)", cypher);
        }

        #endregion // Labels_Test

        #region Timestamp_Test

        [Fact]
        public void Timestamp_Test()
        {
            var cypher = F.Timestamp();

            _outputHelper.WriteLine(cypher);
            Assert.Equal("timestamp()", cypher);
        }

        #endregion // Timestamp_Test

        #region Id_Test

        [Fact]
        public void Id_Test()
        {
            var cypher = F.Id("a");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("id(a)", cypher);
        }

        #endregion // Id_Test

        #region ToInteger_Test

        [Fact]
        public void ToInteger_Test()
        {
            var cypher = F.ToInteger("a");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("toInteger(a)", cypher);
        }

        #endregion // ToInteger_Test

        #region ToFloat_Test

        [Fact]
        public void ToFloat_Test()
        {
            var cypher = F.ToFloat("a");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("toFloat(a)", cypher);
        }

        #endregion // ToFloat_Test

        #region ToBoolean_Test

        [Fact]
        public void ToBoolean_Test()
        {
            var cypher = F.ToBoolean("a");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("toBoolean(a)", cypher);
        }

        #endregion // ToBoolean_Test

        #region Keys_Test

        [Fact]
        public void Keys_Test()
        {
            var cypher = F.Keys("a");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("keys(a)", cypher);
        }

        #endregion // Keys_Test

        #region Properties_Test

        [Fact]
        public void Properties_Test()
        {
            var cypher = F.Properties("a");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("properties(a)", cypher);
        }

        #endregion // Properties_Test
    }
}