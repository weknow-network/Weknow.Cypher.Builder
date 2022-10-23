using System.Data;
using System.Xml.Linq;

using Neo4j.Driver;
using Neo4j.Driver.Extensions;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbClient.IntegrationTests.Abstract;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.Neo4j.IntegrationTests;

//    //[Trait("Group", "Predicates")]
//[Trait("Segment", "Expression")]
public class SmellTests : BaseSmellTests
{
    #region Ctor

    public SmellTests(
        //IGraphDB graphDB, 
        ITestOutputHelper outputHelper)
        : base(graphDB, outputHelper)
    {
        // TODO: DI injection of async session
    }

    #endregion // Ctor
}
