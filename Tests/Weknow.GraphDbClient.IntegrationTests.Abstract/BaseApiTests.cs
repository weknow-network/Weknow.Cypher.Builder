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
using Weknow.GraphDbCommands.Declarations;
using System;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;


//[Trait("Group", "Predicates")]
//[Trait("Integration", "abstract")]
[Trait("TestType", "Integration")]
public abstract class BaseApiTests : BaseIntegrationTests
{
    private ILabel Person => throw new NotImplementedException();
    private ILabel Friend => throw new NotImplementedException();
    private IType Knows => throw new NotImplementedException();

    #region Ctor

    public BaseApiTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    #region GetAsync

    [Fact]
    public virtual async Task GetAsync_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        CypherCommand cypher = _(n => m =>
                                Create(N(n, Person) - R[Knows] > N(m, Friend)));


        CypherParameters prms = cypher.Parameters;
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherCommand query = _(n => m => r =>
                                Match(N(n, Person) - R[r, Knows] > N(m, Friend))
                                .Return(n.Labels(), r.type(), m.Labels()));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        IList<string> n = await response.GetAsync<IList<string>>("labels(n)");
        IEnumerable<string> m = await response.GetAsync<IEnumerable<string>>("labels(m)");
        var r = await response.GetAsync<string>("type(r)");

        #region Validation

        Assert.Equal(nameof(Knows), r);
        Assert.Equal(2, n.Count);
        Assert.Contains(nameof(_Test_).ToUpper(), n);
        Assert.Contains(nameof(Person).ToUpper(), n);
        Assert.Equal(2, m.Count());
        Assert.Contains(nameof(_Test_).ToUpper(), m);
        Assert.Contains(nameof(Friend).ToUpper(), m);

        #endregion // Validation
    }

    #endregion // GetAsync

    #region GetAsync(factory)

    [Fact]
    public virtual async Task GetAsync_Factory_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        CypherCommand cypher = _(n => m =>
                                Create(N(n, Person) - R[Knows] > N(m, Friend)));


        CypherParameters prms = cypher.Parameters;
        await _graphDB.RunAsync(cypher, prms);
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherCommand query = _(n => m => r =>
                                Match(N(n, Person) - R[r, Knows] > N(m, Friend))
                                .Return(n.Labels(), r.type(), m.Labels()));
        IGraphDBResponse response = await _graphDB.RunAsync(query, query.Parameters);
        var (n, m, r) = await response.GetAsync<(IList<string>, IList<string>, string)>(m => 
                    (
                        m.Get<IList<string>>("labels(n)"), 
                        m.Get<IList<string>>("labels(m)"), 
                        m.Get<string>("type(r)") 
                        ));

        #region Validation

        Assert.Equal(nameof(Knows), r);
        Assert.Equal(2, n.Count);
        Assert.Contains(nameof(_Test_).ToUpper(), n);
        Assert.Contains(nameof(Person).ToUpper(), n);
        Assert.Equal(2, m.Count());
        Assert.Contains(nameof(_Test_).ToUpper(), m);
        Assert.Contains(nameof(Friend).ToUpper(), m);

        #endregion // Validation
    }

    #endregion // GetAsync(factory)

}
