using System.Data;
using System.Xml.Linq;

using Neo4j.Driver;
using Neo4j.Driver.Extensions;

using Weknow.GraphDbCommands;
using Weknow.GraphDbClient.Abstraction;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using Xunit.Sdk;
using Weknow.Mapping;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable]
internal partial record NameEntity(string Name);

//    //[Trait("Group", "Predicates")]
//[Trait("Integration", "abstract")]
//[Trait("TestType", "Integration")]
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

    ILabel Person => throw new NotImplementedException();


    #region Create_Match_Test

    [Fact]
    public virtual async Task Create_Match_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));

        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        NameEntity entity = await response.GetAsync<NameEntity>();
        NameEntity[] entities = await response.GetRangeAsync<NameEntity>();

        Assert.Equal(EXPECTED, entity.Name);

    }

    [Fact]
    public virtual async Task Create_Match_Property_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p.As<NameEntity>().Name));
        CypherParameters prms = cypher.Parameters;
        prms[nameof(pName)] = EXPECTED;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        string name = await response.GetAsync<string>();
        string[] names = await response.GetRangeAsync<string>("name");

        Assert.Equal(EXPECTED, name);

    }

    #endregion // Create_Match_Test
}
