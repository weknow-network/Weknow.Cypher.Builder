using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    [Trait("Segment", "Deprecate")]
    public class CypherFactoryMathematicTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherFactoryMathematicTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Abs_Test

        [Fact]
        public void Abs_Test()
        {
            string result = M.Abs("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("abs(f.Name)", result);
        } 

        #endregion // Abs_Test

        #region Acos_Test

        [Fact]
        public void Acos_Test()
        {
            string result = M.Acos("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("acos(f.Name)", result);
        } 

        #endregion // Acos_Test

        #region Asin_Test

        [Fact]
        public void Asin_Test()
        {
            string result = M.Asin("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("asin(f.Name)", result);
        } 

        #endregion // Asin_Test

        #region Atan_Test

        [Fact]
        public void Atan_Test()
        {
            string result = M.Atan("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("atan(f.Name)", result);
        }

        #endregion // Atan_Test

        #region Atan2_Test

        [Fact]
        public void Atan2_Test()
        {
            string result = M.Atan2("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("atan2(f.Name)", result);
        } 

        #endregion // Atan2_Test

        #region Cos_Test

        [Fact]
        public void Cos_Test()
        {
            string result = M.Cos("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("cos(f.Name)", result);
        } 

        #endregion // Cos_Test

        #region Cot_Test

        [Fact]
        public void Cot_Test()
        {
            string result = M.Cot("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("cot(f.Name)", result);
        } 

        #endregion // Cot_Test

        #region E_Test

        [Fact]
        public void E_Test()
        {
            string result = M.E();
            _outputHelper.WriteLine(result);
            Assert.Equal("e()", result);
        } 

        #endregion // E_Test

        #region Exp_Test

        [Fact]
        public void Exp_Test()
        {
            string result = M.Exp("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("exp(f.Name)", result);
        } 

        #endregion // Exp_Test

        #region Haversin_Test

        [Fact]
        public void Haversin_Test()
        {
            string result = M.Haversin("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("haversin(f.Name)", result);
        } 

        #endregion // Haversin_Test

        #region Log_Test

        [Fact]
        public void Log_Test()
        {
            string result = M.Log("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("log(f.Name)", result);
        } 

        #endregion // Log_Test

        #region Log10_Test

        [Fact]
        public void Log10_Test()
        {
            string result = M.Log10("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("log10(f.Name)", result);
        } 

        #endregion // Log10_Test

        #region Pi_Test

        [Fact]
        public void Pi_Test()
        {
            string result = M.Pi();
            _outputHelper.WriteLine(result);
            Assert.Equal("pi()", result);
        } 

        #endregion // Pi_Test

        #region Radians_Test

        [Fact]
        public void Radians_Test()
        {
            string result = M.Radians("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("radians(f.Name)", result);
        } 

        #endregion // Radians_Test

        #region Rand_Test

        [Fact]
        public void Rand_Test()
        {
            string result = M.Rand();
            _outputHelper.WriteLine(result);
            Assert.Equal("rand()", result);
        } 

        #endregion // Rand_Test

        #region Round_Test

        [Fact]
        public void Round_Test()
        {
            string result = M.Round("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("round(f.Name)", result);
        } 

        #endregion // Round_Test

        #region Sign_Test

        [Fact]
        public void Sign_Test()
        {
            string result = M.Sign("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("sign(f.Name)", result);
        } 

        #endregion // Sign_Test

        #region Sin_Test

        [Fact]
        public void Sin_Test()
        {
            string result = M.Sin("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("sin(f.Name)", result);
        }

        #endregion // Sin_Test  

        #region Sqrt_Test

        [Fact]
        public void Sqrt_Test()
        {
            string result = M.Sqrt("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("sqrt(f.Name)", result);
        }

        #endregion // Sqrt_Test

        #region Tan_Test

        [Fact]
        public void Tan_Test()
        {
            string result = M.Tan("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("tan(f.Name)", result);
        }

        #endregion // Tan_Test

    }
}