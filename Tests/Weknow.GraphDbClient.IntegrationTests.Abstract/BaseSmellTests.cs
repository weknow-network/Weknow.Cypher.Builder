using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

//    //[Trait("Group", "Predicates")]
//[Trait("Integration", "abstract")]
//[Trait("TestType", "Integration")]
public abstract class BaseSmellTests : TxBaseIntegrationTests
{
    #region Ctor

    public BaseSmellTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    private ILabel Person => throw new NotImplementedException();

    private ILabel Manager => throw new NotImplementedException();

    private IType WorkFor => throw new NotImplementedException();


    #region CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    [Fact]
    public virtual async Task Create_Match_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        NameDictionaryable entity = await response.GetAsync<NameDictionaryable>();
        NameDictionaryable[] entities = await response.GetRangeAsync<NameDictionaryable>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region CREATE (p:Person $pName) RETURN p.Id, p.Name, p.Age

    [Fact]
    public virtual async Task Create_Match_Dictionaryable_Test()
    {
        Someone EXPECTED = new Someone(20, "Mosh", 42);
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create<Someone>();
        var p = Variables.Create<Someone>();

        CypherCommand cypher = _(() =>
                                Create(N(p, Person,  pName))
                                .Return(p.__.Id, p.__.Name, p.__.Age));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
            Someone entity = await response.GetAsync<Someone>();
        Someone[] entities = await response.GetRangeAsync<Someone>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single());
    }

    #endregion // CREATE (p:Person $pName) RETURN p.Id, p.Name, p.Age

    #region CREATE (p:Person $pName) RETURN p.Id AS id, p.Name AS name, p.Age AS age

    [Fact]
    public virtual async Task Create_Match_Flatten_Dictionaryable_Test()
    {
        Someone EXPECTED = new Someone(20, "Mosh", 42);
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create<Someone>();
        var p = Variables.Create<Someone>();

        CypherCommand cypher = _(() =>
                                Create(N(p, Person,  pName))
                                .Return(p.__.Id.As("id"), p.__.Name.As("name"), p.__.Age.As("age")));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
            Someone entity = await response.GetAsync<Someone>();
        Someone[] entities = await response.GetRangeAsync<Someone>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single());
    }

    #endregion // CREATE (p:Person $pName) RETURN p.Id AS id, p.Name AS name, p.Age AS age

    #region CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    [Fact]
    public virtual async Task Create_Match_Var_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var v = Variables.Create();
        CypherCommand cypher = _(() =>
                                Create(N(v, Person, new { Name = pName }))
                                .Return(v));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        NameDictionaryable entity = await response.GetAsync<NameDictionaryable>();
        NameDictionaryable[] entities = await response.GetRangeAsync<NameDictionaryable>(nameof(v)).ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region CREATE (p:PERSON:_TEST_ { Name: $pName }) RETURN p.Name

    [Fact]
    public virtual async Task Create_Match_Property_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var v = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Create(N(v, Person, new { Name = pName }))
                                .Return(v._.Name));
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        string name = await response.GetAsync<string>(nameof(v), nameof(NameDictionaryable.Name));
        string[] names = await response.GetRangeAsync<string>(nameof(v), nameof(NameDictionaryable.Name)).ToArrayAsync();

        Assert.Equal(EXPECTED, name);
        Assert.Single(names);
        Assert.Equal(EXPECTED, names.Single());

    }

    #endregion // CREATE (p:PERSON:_TEST_ { Name: $pName }) RETURN p.Name

    #region Create_Map_Match_Test

    [Fact]
    public virtual async Task Create_Map_Match_Test()
    {
        var i = new Someone((int)(DateTime.Now.Ticks % 10L), "Name", (int)51L);
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var (p1, p2, p3, p4, p5) = Parameters.CreateMulti<Someone, Someone, Someone, Someone, Someone>();
        var someone1 = new Someone(1, "Deby", 51);
        var someone2 = new Someone(1, "Ruth", 28);
        var someone3 = new Someone(1, "Maygen", 30);
        var someone4 = new Someone(1, "Derk", 30);
        var someone5 = new Someone(1, "Maya", 30);

        var (c1, c2, c3, c4, c5, m1, m2) = Variables.CreateMulti();
        CypherCommand cypher = _(() =>
                                Create(N(c1, Manager & _Test_, p1))
                                .Create(N(c5, Manager & _Test_, p5))
                                .Create(N(c2, Person & _Test_, p2) - R[WorkFor] > N(c1))
                                .Create(N(c3, Person & _Test_, p3) - R[WorkFor] > N(c1))
                                .Create(N(c4, Person & _Test_, p4) - R[WorkFor] > N(c5))
                                .With()
                                .Match(N(m1, Person) - R[WorkFor] > N(m2, Manager))
                                .Return(m1, m2));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        #region CypherParameters prms = ...

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(p1), someone1.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p2), someone2.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p3), someone3.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p4), someone4.ToDictionary());
        prms = prms.AddOrUpdate(nameof(p5), someone5.ToDictionary());

        #endregion // CypherParameters prms = ...

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var managers = await response.GetRangeAsync<Someone>(nameof(m1)).ToArrayAsync();
        var persons = await response.GetRangeAsync<Someone>(nameof(m2)).ToArrayAsync();
        Assert.True(managers.Length == 3);
        Assert.True(persons.Length == 3);
        //response.GetMapRangeAsync<Someone, Someone, string>(nameof(m1), nameof(m2), (m1, m2) => string.Empty);
    }

    #endregion // Create_Map_Match_Test

    #region CREATE (p1:PERSON:_TEST_ { Name: $pName }) CREATE(p2:PERSON:_TEST_ { Name: $pName }) RETURN p1, p2

    [Fact]
    public virtual async Task Create_Match_Multi_StepByStep_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        var (p1, p2) = Variables.CreateMulti<NameDictionaryable, NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Create(N(p1, Person, new { Name = pName }))
                                .Create(N(p2, Person, new { Name = pName }))
                                .Return(p1, p2));
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var r1 = await response.GetAsync<NameDictionaryable>(nameof(p1));
        Assert.Equal(EXPECTED, r1.Name);

        var r2 = await response.GetAsync<NameDictionaryable>(nameof(p2));
        Assert.Equal(EXPECTED, r2.Name);
    }

    #endregion // CREATE (p1:PERSON:_TEST_ { Name: $pName }) CREATE(p2:PERSON:_TEST_ { Name: $pName }) RETURN p1, p2

    #region UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    [Fact]
    public virtual async Task Create_Match_Multi_Unwind_Param_Add_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, x) = Variables.CreateMulti<Someone>();
        var p = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(x, Person))
                                       .Set(x, map))
                                .Return(x));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(m => new Someone(m, $"Number {n}", m % 10 + 5)));
        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<Someone>(nameof(x)).ToArrayAsync();
        Assert.NotEmpty(r3);
    }

    #endregion // UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    #region UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    [Fact]
    public virtual async Task Create_Match_Multi_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, x) = Variables.CreateMulti<Someone>();
        var p = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(x, Person))
                                       .Set(x, map))
                                .Return(x));
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(m => new Someone(m, $"Number {n}", m % 10 + 5).ToDictionary())
                                .ToArray());
        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<Someone>(nameof(x)).ToArrayAsync();
        Assert.NotEmpty(r3);
    }

    #endregion // UNWIND $items AS map CREATE(x:PERSON) SET x = map RETURN x

    #region CREATE (p:_TEST_:PERSON { Name: $n, Birthday: $b, IssueDate: $i, At: $t }) RETURN p

    [Fact]
    public virtual async Task Create_Date_Explicit_Prm_Match_Test()
    {
        const string EXPECTED = "someone";
        CypherConfig.Scope.Value = CONFIGURATION;

        var (prm1, prm2, prm3, prm4) = Parameters.CreateMulti();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person,
                                new
                                {
                                    Name = "someone",
                                    Birthday = prm1,
                                    IssueDate = prm2,
                                    At = prm3,
                                    Local = prm4
                                }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters
                                    .AddOrUpdate(nameof(prm1), DateTimeOffset.UtcNow)
                                    .AddOrUpdate(nameof(prm2), DateTime.Today)
                                    .AddOrUpdate(nameof(prm3), DateTime.Now.TimeOfDay)
                                    .AddOrUpdate(nameof(prm4), DateTimeOffset.Now);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        Sometime entity = await response.GetAsync<Sometime>();
        Sometime[] entities = await response.GetRangeAsync<Sometime>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Equal(prms[nameof(prm1)], entity.Birthday);
        Assert.Equal(prms[nameof(prm2)], entity.IssueDate);
        Assert.Equal(prms[nameof(prm3)], entity.At);
        Assert.Equal(prms[nameof(prm4)], entity.Local);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region CREATE (p:_TEST_:PERSON { Name: $n, Birthday: $b, IssueDate: $i, At: $t }) RETURN p

    [Fact]
    public virtual async Task Create_Date_Prm_Match_Test()
    {
        const string EXPECTED = "someone";
        CypherConfig.Scope.Value = CONFIGURATION;

        CypherCommand cypher = _(p =>
                                Create(N(p, Person,
                                new Sometime
                                {
                                    Name = "someone",
                                    Birthday = DateTimeOffset.UtcNow,
                                    IssueDate = DateTime.Today,
                                    At = DateTime.Now.TimeOfDay,
                                    Local = DateTimeOffset.Now
                                }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        Sometime entity = await response.GetAsync<Sometime>();
        Sometime[] entities = await response.GetRangeAsync<Sometime>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Equal(DateTime.Today, entity.IssueDate);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    #endregion // CREATE (p:_TEST_:PERSON { Name: $pName }) RETURN p

    #region MERGE ... SetDateConvention()

    [Fact]
    public virtual async Task Merge_Dat_Convention_Test()
    {
        const string EXPECTED = "merge-date-convention";
        CypherConfig.Scope.Value = CONFIGURATION;
        var date = DateTimeOffset.UtcNow;

        CypherCommand cypher = _(p =>
                                Merge(N(p, Person,
                                new
                                {
                                    Id = "merge-date-convention"
                                }))
                                .SetDateConvention(p)
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var entity = await response.GetAsync<DateConvensionEntity>();

        Assert.Equal(EXPECTED, entity.Id);
        Assert.True(date <= entity.CreatedAt);
        Assert.Null(entity.ModifiedAt);

        response = await _tx.RunAsync(cypher, prms);
        entity = await response.GetAsync<DateConvensionEntity>();

        Assert.True(date <= entity.CreatedAt);
        Assert.NotNull(entity.ModifiedAt);
    }

    #endregion // MERGE ... SetDateConvention()

    #region CREATE ... SetDateConvention()

    [Fact]
    public virtual async Task Create_Dat_Convention_Test()
    {
        const string EXPECTED = "merge-date-convention";
        CypherConfig.Scope.Value = CONFIGURATION;
        var date = DateTimeOffset.UtcNow;
        var p = Variables.Create<DateConvensionEntity>();
        CypherCommand cypher = _(() =>
                                Create(N(p, Person,
                                new
                                {
                                    Id = "merge-date-convention"
                                }))
                                .SetDateConvention(p)
                                //.With(p)
                                //.Match(N(p, Person))
                                //.Where(p.__.Id == "merge-date-convention")
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var entity = await response.GetAsync<DateConvensionEntity>();

        Assert.Equal(EXPECTED, entity.Id);
        Assert.True(date <= entity.CreatedAt);
    }

    #endregion // CREATE ... SetDateConvention()

    #region Implicit_Explicit_GetRangeAsync_Test

    [Fact]
    public virtual async Task Implicit_Explicit_GetRangeAsync_Test()
    {
        var EXPECTED = Enumerable.Range(0, 5)
                                 .Select(i => new Name1($"Unit {i}"))
                                 .ToArray();
        CypherConfig.Scope.Value = CONFIGURATION;
        var spine = Parameters.Create<Name1>();
        var n = Variables.Create<Name1>();

        CypherCommand cypher = _(() =>
                                Unwind(spine, u =>
                                    Create(N(Person, new { u.__.Name }))
                                    .With(u)
                                    .Match(N(n, Person))
                                    .Where(n.__.Name == u.__.Name)
                                    .Return(u)));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters
                                .AddRangeOrUpdate(nameof(spine), EXPECTED.Select(x => x.ToDictionary()));

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        Name1[] entitiesImplicit = await response.GetRangeAsync<Name1>().ToArrayAsync();
        Name1[] entitiesExplicit = await response.GetRangeAsync<Name1>("u").ToArrayAsync();

        Assert.True(EXPECTED.SequenceEqual(entitiesImplicit));
        Assert.True(EXPECTED.SequenceEqual(entitiesExplicit));
    }

    #endregion // Implicit_Explicit_GetRangeAsync_Test


    [Fact(Skip = "Unwind BAD SYNTAX, use Create_Match_Multi_Unwind_Test")]
    public virtual async Task BAD_SYNTAX_Create_Match_Multi_Unwind_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION_NO_AMBIENT;
        var items = Parameters.Create();
        var (n, x) = Variables.CreateMulti<Someone>();
        var p = Variables.Create<NameDictionaryable>();
        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Create(N(x, Person, map.AsParameter)))
                                .Return(x));
        _outputHelper.WriteLine($"CYPHER: {cypher}");
        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(items), Enumerable.Range(0, 10)
                                .Select(m => new Someone(m, $"Number {m}", m % 10 + 5).ToDictionary())
                                .ToArray());
        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        var r3 = await response.GetRangeAsync<Someone>(nameof(x)).ToArrayAsync();
        Assert.Empty(r3);
    }

    [Fact(Skip = "Not Supported Yet")]
    public virtual async Task Create_Match1_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        Name1 entity = await response.GetAsync<Name1>();
        Name1[] entities = await response.GetRangeAsync<Name1>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }

    [Fact(Skip = "Not Supported Yet")]
    public virtual async Task Create_Match2_Test()
    {
        const string EXPECTED = "Ben";
        CypherConfig.Scope.Value = CONFIGURATION;
        var pName = Parameters.Create();

        CypherCommand cypher = _(p =>
                                Create(N(p, Person, new { Name = pName }))
                                .Return(p));
        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddOrUpdate(nameof(pName), EXPECTED);

        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        Name2 entity = await response.GetAsync<Name2>();
        Name2[] entities = await response.GetRangeAsync<Name2>().ToArrayAsync();

        Assert.Equal(EXPECTED, entity.Name);
        Assert.Single(entities);
        Assert.Equal(EXPECTED, entities.Single().Name);
    }
}
