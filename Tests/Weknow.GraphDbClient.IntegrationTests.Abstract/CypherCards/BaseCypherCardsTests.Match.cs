using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
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
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(n_name), ALICE);
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(m_name), MIKE);
        await _tx.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var name = Parameters.Create<string>();
        CypherCommand cypher = _(() =>
                                Match(N(n, Person) - R[Knows] > N(m, Person))
                                .Where(m._.name == name)
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(name), ALICE);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);

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


        var (n, m) = Variables.CreateMulti<PersonEntity>();

        #region Prepare

        var (n_name, m_name) = Parameters.CreateMulti<PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(m, Person, new { name = n_name }))
                                .Create(N(n, Person, new { name = m_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(n_name), ALICE);
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(m_name), MIKE);
        await _tx.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        CypherCommand cypher = _(() =>
                                Match(N(n, Person) - R[Knows] > N(m, Person))
                                .Where(m._.name == "Alice")
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters;
        //prms.AddString(nameof(name), "p_0");

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);

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
                                Create(N(m, Person, new { name = m_name }))
                                .Create(N(n, Person, new { name = n_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(m_name), MIKE);
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(n_name), ALICE);
        await _tx.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        CypherCommand cypher = _(() =>
                                Match(N(n) > N(m))
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters;

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);

        var alice = await response.GetAsync<string>(nameof(n), nameof(n._.name));
        var mike = await response.GetAsync<string>(nameof(m), nameof(m._.name));

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

        var (n_name, m_name) = Parameters.CreateMulti<PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(m, Person, new { name = n_name }))
                                .Create(N(n, Person, new { name = m_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(n_name), MIKE);
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(m_name), ALICE);
        await _tx.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var name = Parameters.Create<string>();
        CypherCommand cypher = _(() =>
                                Match(N(n, new { name = name }) > N(m))
                                .Return(n._.name, m._.name));


        CypherParameters prms = cypher.Parameters
                                        .AddOrUpdate(nameof(name), ALICE);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);

        var alice = await response.GetAsync<string>(nameof(n), nameof(n._.name));
        var mike = await response.GetAsync<string>(nameof(m), nameof(m._.name));

        Assert.Equal(MIKE, mike);
        Assert.Equal(ALICE, alice);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // MATCH (n {name: 'Alice'})-->(m)

    #region MATCH MATCH (n {name: 'Alice'})-->(m)

    [Fact]
    public virtual async Task MATCH_N_PROP_M_OBJ_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        string ALICE = "Alice";
        string MIKE = "Mike";


        var (n, m) = Variables.CreateMulti<PersonEntity>();

        #region Prepare

        var (n_name, m_name) = Parameters.CreateMulti<PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(m, Person, new { name = n_name, age = 30 }))
                                .Create(N(n, Person, new { name = m_name }) - R[Knows] > N(m)));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(n_name), MIKE);
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(m_name), ALICE);
        await _tx.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var name = Parameters.Create<string>();
        CypherCommand cypher = _(() =>
                                Match(N(n, new { name = name }) > N(m))
                                .Return(n._.name, m));


        CypherParameters prms = cypher.Parameters
                                        .AddOrUpdate(nameof(name), ALICE);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);

        var alice = await response.GetAsync<string>(nameof(n), nameof(n._.name));
        var mike = await response.GetAsync<PersonEntity>(nameof(m));

        Assert.Equal(MIKE, mike.name);
        Assert.Equal(ALICE, alice);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // MATCH (n {name: 'Alice'})-->(m)

    #region OPTIONAL MATCH (n)-[r]->(m)

    [Fact]
    public virtual async Task OPTIONAL_MATCH_Test()
    {
        string ALICE = "Alice";
        string MIKE = "Mike";
        string BOB = "Bob";


        var (m, n, k) = Variables.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        var r = Variables.Create();
        var tx = _tx;
        //await using var tx = await _graphDB.StartTransaction();

        #region Prepare

        var (n_name, m_name, k_name) = Parameters.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(n, Person, new { name = n_name }))
                                .Create(N(Person, new { name = "Borka" }))
                                .Create(N(k, Person, new { name = k_name }) - R[Knows] > N(n))
                                .Create(N(m, Person, new { name = m_name }) - R[Knows] > N(n)), CONFIGURATION);
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(n_name), MIKE);
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(m_name), ALICE);
        prmsPrepare = prmsPrepare.AddOrUpdate(nameof(k_name), BOB);
        await tx.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        CypherCommand cypher = _(() =>
                                OptionalMatch(N(m) - R[r] > N(n))
                                .Return(m._.name, n._.name, r.type()), CONFIGURATION);
        _outputHelper.WriteLine(cypher.Dump());


        CypherParameters prms = cypher.Parameters
                                      .AddOrUpdate(nameof(n._.name), ALICE);

        IGraphDBResponse response = await tx.RunAsync(cypher, prms);

        var left = await response.GetRangeAsync<string>(nameof(m), nameof(m._.name)).ToArrayAsync();
        var right = await response.GetRangeAsync<string>(nameof(n), nameof(n._.name)).ToArrayAsync();
        var rel = await response.GetRangeAsync<string>("type(r)").ToArrayAsync();

        Assert.Equal(2, left.Length);
        Assert.Equal(BOB, left[0]);
        Assert.Equal(MIKE, right[0]);
        Assert.Equal(nameof(Knows), rel[0]);
        Assert.Equal(ALICE, left[1]);
        Assert.Equal(MIKE, right[1]);
        Assert.Equal(nameof(Knows), rel[1]);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // OPTIONAL MATCH (n)-[r]->(m)
}
