using System.Data;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbCommands;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public abstract partial class BaseCypherCardsTests : BaseIntegrationTests
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

    [Dictionaryable]
    private partial record PersonEntity(string name, int age);

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

    #region OPTIONAL MATCH (n)-[r]->(m)

    [Fact]
    public virtual async Task OPTIONAL_MATCH_Test()
    {
        string ALICE = "Alice";
        string MIKE = "Mike";
        string BOB = "Bob";


        var (m, n, k) = Variables.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        var r = Variables.Create();

        #region Prepare

        var (n_name, m_name, k_name) = Parameters.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(n, Person, new { name = n_name }))
                                .Create(N(Person, new { name = "Borka" }))
                                .Create(N(k, Person, new { name = k_name }) - R[Knows] > N(n))
                                .Create(N(m, Person, new { name = m_name }) - R[Knows] > N(n)), CONFIGURATION);
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare.AddString(nameof(n_name), MIKE);
        prmsPrepare.AddString(nameof(m_name), ALICE);
        prmsPrepare.AddString(nameof(k_name), BOB);
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var name = Parameters.Create<string>();
        CypherCommand cypher = _(() =>
                                OptionalMatch(N(m) - R[r] > N(n))
                                .Return(m._.name, n._.name, r.type()));


        CypherParameters prms = cypher.Parameters;
        prms.AddString(nameof(name), ALICE);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

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

    #region WHERE n.property <> $value

    [Fact]
    public virtual async Task Where_Prop_NotEq_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        #region Prepare

        var (n_name, m_name, k_name) = Parameters.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(Person, new { name = "Dana", age = 10 }))
                                .Create(N(Person, new { name = "Groum", age = 20 }))
                                .Create(N(Person, new { name = "Borka", age = 25 })));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var value = Parameters.Create<int>();
        var n = Variables.Create<PersonEntity>();
        CypherCommand cypher = _(() =>
                                Match(N(n, Person))
                                .Where(n._.age != value)
                                .Return(n._.name));


        CypherParameters prms = cypher.Parameters;
        prms.AddValue(nameof(value), 25);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var people = await response.GetRangeAsync<string>(nameof(n), nameof(n._.name)).ToArrayAsync();

        Assert.Equal(2, people.Length);
        Assert.Contains("Dana", people);
        Assert.Contains("Groum", people);

        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // WHERE n.property <> $value

    #region WHERE EXISTS { MATCH(n)-->(m) WHERE n.age = m.age }

    [Fact]
    public virtual async Task Where_Exists_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        string ALICE = "Alice";
        string MIKE = "Mike";
        string BOB = "Bob";

        var (n, m, k) = Variables.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        var r = Variables.Create();

        #region Prepare

        var (n_name, m_name, k_name) = Parameters.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(n, Person, new { name = n_name, age = 25 }))
                                .Create(N(Person, new { name = "Borka" }))
                                .Create(N(k, Person, new { name = k_name, age = 30 }) - R[Knows] > N(n))
                                .Create(N(m, Person, new { name = m_name, age = 25 }) - R[Knows] > N(n)), CONFIGURATION);
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare.AddString(nameof(n_name), MIKE);
        prmsPrepare.AddString(nameof(m_name), ALICE);
        prmsPrepare.AddString(nameof(k_name), BOB);
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        CypherCommand cypher = _(() =>
                                Match(N(n, Person))
                                .Where(Exists(() => Match(N(n) > N(m))
                                                    .Where(n._.age == m._.age)))
                                .Return(n));

        CypherParameters prms = cypher.Parameters;
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var people = await response.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        Assert.Single(people);
        Assert.Equal(ALICE, people[0].name);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // WHERE EXISTS { MATCH(n)-->(m) WHERE n.age = m.age }

    #region UNWIND .. RETURN *

    [Fact]
    public virtual async Task Return_Star_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(n, Person))
                                       .Set(n, map))
                                .Return("*"));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms.AddRange(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();
        Assert.True(r3.Length == 10);
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(Factory(i) , r3[i]);

        }

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i % 10 + 5);
    }

    #endregion // UNWIND .. RETURN *

    #region UNWIND .. RETURN n AS x

    [Fact]
    public virtual async Task Return_Alias_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, map) = Variables.CreateMulti<PersonEntity, PersonEntity>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map,
                                     Create(N(n, Person))
                                       .Set(n, map))
                                .Return(n.As("x")));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms.AddRange(nameof(items), Enumerable.Range(0, 10)
                                .Select(Factory));
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<PersonEntity>("x").ToArrayAsync();
        Assert.True(r3.Length == 10);
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(Factory(i) , r3[i]);

        }

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}", i % 10 + 5);
    }

    #endregion // UNWIND .. RETURN n AS x
}
