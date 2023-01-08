using Castle.Core.Configuration;

using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Config")]
    public class ApiTests
    {
        protected readonly ITestOutputHelper _outputHelper;
        private IType Like => IType.Fake;
        private ILabel Person => ILabel.Fake;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTests"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper.</param>
        public ApiTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Label_Convention_Match_Test

        [Fact] 
        public void Label_Convention_Match_Neo4j_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _((n, m) => 
                         Match(N(n, Person, new { id = m})));

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MATCH (n:Person { id: m })" 
                           , cypher.Query);
        }

        #endregion // Label_Convention_Match_Test
    }
}

