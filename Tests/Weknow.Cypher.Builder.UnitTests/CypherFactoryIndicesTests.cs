using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherFactoryIndicesTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherFactoryIndicesTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region CreateIndex_Test

        [Fact]
        public void CreateIndex_Test()
        {
            var cypher = I.CreateIndex("Person", "Name");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE INDEX ON :Person(Name)", cypher);
        }

        #endregion // CreateIndex_Test

        #region CreateCompositeIndex_Test

        [Fact]
        public void CreateCompositeIndex_Test()
        {
            var cypher = I.CreateIndex("Person", "Name", "Age");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("CREATE INDEX ON :Person(Name, Age)", cypher);
        }

        #endregion // CreateCompositeIndex_Test
    }
}