using System.Data;
using System.Xml.Linq;

using Neo4j.Driver;
using Neo4j.Driver.Extensions;

using Weknow.GraphDbClient.Abstraction;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

//    //[Trait("Group", "Predicates")]
//[Trait("Segment", "Expression")]
public abstract class BaseSmellTests : BaseIntegrationTests
{
    #region Ctor

    public BaseSmellTests(
        IGraphDB graphDB,
        ITestOutputHelper outputHelper)
        : base(graphDB, outputHelper)
    {
    }

    #endregion // Ctor

    #region Create_Match_Test

    [Fact]
    public async Task Create_Match_Test()
    {
        throw new NotImplementedException();
    }

    #endregion // Create_Match_Test
}
