using System.Data;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbCommands;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public abstract class BaseCypherCardsTests : BaseIntegrationTests
{
    #region Ctor

    public BaseCypherCardsTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    private ILabel Person => throw new NotImplementedException();

    private IType Knows => throw new NotImplementedException();

    private record PersonEntity(string name);

    #region MATCH (n:PERSON)-[:KNOWS]->(m:PERSON) WHERE n.name = 'Alice'

    [Fact]
    public virtual async Task MATCH_PERSON_KNOWS_PERSON_1_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        string ALICE = "Alice";
        string MIKE = "Mike";


        var (n, m) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        var (n_name, m_name) = Parameters.CreateMulti<PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(m, Person, new { name = n_name }))
                                .Create(N(n, Person, new { name = m_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare.AddString(nameof(n_name), ALICE);
        prmsPrepare[nameof(m_name)] = MIKE;
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var name = Parameters.Create<string>();
        CypherCommand cypher = _(() =>
                                Match(N(n, Person) - R[Knows] > N(m, Person))
                                .Where(m._.name == name)
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters;
        prms.AddString(nameof(name), ALICE);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var mike = await response.GetAsync<string>(nameof(n), nameof(n._.name));
        var alice = await response.GetAsync<string>(nameof(m), nameof(m._.name));

        Assert.Equal(MIKE, mike);
        Assert.Equal(ALICE, alice);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // MATCH (n:Person)-[:KNOWS]->(m:Person) WHERE n.name = 'Alice'

    #region MATCH (n:PERSON)-[:KNOWS]->(m:PERSON) WHERE n.name = 'Alice'

    [Fact]
    public virtual async Task MATCH_PERSON_KNOWS_PERSON_2_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        string ALICE = "Alice";
        string MIKE = "Mike";


        var (n, m) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        var (n_name, m_name) = Parameters.CreateMulti<PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(m, Person, new { name = n_name }))
                                .Create(N(n, Person, new { name = m_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare.AddString(nameof(n_name), ALICE);
        prmsPrepare[nameof(m_name)] = MIKE;
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        CypherCommand cypher = _(() =>
                                Match(N(n, Person) - R[Knows] > N(m, Person))
                                .Where(m._.name == "Alice")
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters;
        //prms.AddString(nameof(name), "p_0");

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var mike = await response.GetAsync<string>(nameof(n), nameof(n._.name));
        var alice = await response.GetAsync<string>(nameof(m), nameof(m._.name));

        Assert.Equal(MIKE, mike);
        Assert.Equal(ALICE, alice);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // MATCH (n:Person)-[:KNOWS]->(m:Person) WHERE n.name = 'Alice'

    #region MATCH (n)-->(m)

    [Fact]
    public virtual async Task MATCH_N_M_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        string ALICE = "Alice";
        string MIKE = "Mike";


        var (n, m) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        var (n_name, m_name) = Parameters.CreateMulti<PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(m, Person, new { name = n_name }))
                                .Create(N(n, Person, new { name = m_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare.AddString(nameof(n_name), ALICE);
        prmsPrepare[nameof(m_name)] = MIKE;
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        CypherCommand cypher = _(() =>
                                Match(N(n) > N(m))
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters;

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var mike = await response.GetAsync<string>(nameof(n), nameof(n._.name));
        var alice = await response.GetAsync<string>(nameof(m), nameof(m._.name));

        Assert.Equal(MIKE, mike);
        Assert.Equal(ALICE, alice);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // MATCH (n)-->(m)

    #region MATCH MATCH (n {name: 'Alice'})-->(m)

    [Fact]
    public virtual async Task MATCH_N_PROP_M_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        string ALICE = "Alice";
        string MIKE = "Mike";


        var (n, m) = Variables.CreateMulti<PersonEntity, PersonEntity>();

        #region Prepare

        var (n_name, m_name) = Parameters.CreateMulti<PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(m, Person, new { name = n_name }))
                                .Create(N(n, Person, new { name = m_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare.AddString(nameof(n_name), MIKE);
        prmsPrepare.AddString(nameof(m_name), ALICE);
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var name = Parameters.Create<string>();
        CypherCommand cypher = _(() =>
                                Match(N(n, new { name = name }) > N(m))
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters;
        prms.AddString(nameof(name), ALICE);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var alice = await response.GetAsync<string>(nameof(n), nameof(n._.name));
        var mike = await response.GetAsync<string>(nameof(m), nameof(m._.name));

        Assert.Equal(MIKE, mike);
        Assert.Equal(ALICE, alice);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // MATCH (n {name: 'Alice'})-->(m)
}
