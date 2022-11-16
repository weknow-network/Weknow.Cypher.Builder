using Xunit;
using Xunit.Abstractions;

namespace Weknow.CypherBuilder.UnitTests
{
    [Trait("TestType", "Unit")]
    public class CypherQueryBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherQueryBuilderTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region CypherQueryBuilder_Range_Test

        [Fact]
        //[Theory]
        public void CypherQueryBuilder_Range_Test()
        {
            var builder = new CypherQueryBuilder();
            string data = "ABCDEFG";
            builder.Append(data);

            Assert.Equal(data[..3], builder.ToString(..3));
            Assert.Equal(data[3..], builder.ToString(3..));
            Assert.Equal(data[2..6], builder.ToString(2..6));
            Assert.Equal(data[2..2], builder.ToString(2..2));
        }

        #endregion // CypherQueryBuilder_Range_Test

        #region CypherQueryBuilder_Range_End_Test

        [Fact]
        //[Theory]
        public void CypherQueryBuilder_Range_End_Test()
        {
            var builder = new CypherQueryBuilder();
            string data = "ABCDEFGHIJ";
            builder.Append(data);

            Assert.Equal(data[..^3], builder.ToString(..^3));
            Assert.Equal(data[^3..], builder.ToString(^3..));
            Assert.Equal(data[^7..6], builder.ToString(^7..6));
            Assert.Equal(data[^4..^2], builder.ToString(^4..^2));
        }

        #endregion // CypherQueryBuilder_Range_End_Test
    }
}
