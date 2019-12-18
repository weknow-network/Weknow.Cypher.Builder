using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherFactoryListTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherFactoryListTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Size_Test

        [Fact]
        public void Size_Test()
        {
            var cypher = L.Size("$list");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("size($list)", cypher);
        }

        #endregion // Size_Test

        #region Reverse_Test

        [Fact]
        public void Reverse_Test()
        {
            var cypher = L.Reverse("$list");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("reverse($list)", cypher);
        }

        #endregion // Reverse_Test

        #region Head_Test

        [Fact]
        public void Head_Test()
        {
            var cypher = L.Head("$list");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("head($list)", cypher);
        }

        #endregion // Head_Test

        #region Last_Test

        [Fact]
        public void Last_Test()
        {
            var cypher = L.Last("$list");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("last($list)", cypher);
        }

        #endregion // Last_Test

        #region Tail_Test

        [Fact]
        public void Tail_Test()
        {
            var cypher = L.Tail("$list");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("tail($list)", cypher);
        }

        #endregion // Tail_Test
    }
}