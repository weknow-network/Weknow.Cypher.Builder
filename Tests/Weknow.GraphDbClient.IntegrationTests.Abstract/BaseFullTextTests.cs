using System.Data;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public abstract partial class BaseFullTextTests : TxBaseIntegrationTests
{
    private const string INDEX_NAME = "fulltext_test_index";
    private IGraphDB _graphDB;

    #region string[] TITLES = ...

    private readonly string[] TITLES =
        {
            "She sells seashells by the seashore",
            "The quick brown fox jumps over the lazy dog",
            "The rain in Spain stays mainly in the plain",
            "The rain in Paris use to be wet",
            "Four score and seven years ago our fathers brought forth on this continent, a new nation, conceived in Liberty, and dedicated to the proposition that all men are created equal",
            "All that glitters is not gold",
            "Most that glitters is gold",
            "Glitters are wonderful"
        };

    #endregion // string[] TITLES = ...

    #region string[] DESC = ...

    private readonly string[] DESC =
        {
            "Education is the cornerstone of a successful society. It provides individuals with the skills and knowledge they need to succeed in the workforce and lead fulfilling lives. Investing in education, from early childhood through higher education, is essential for a strong and thriving communit",
            "Climate change is one of the biggest challenges facing the world today. It is causing temperatures to rise, sea levels to rise, and weather patterns to become more extreme. Governments, organizations, and individuals must work together to reduce greenhouse gas emissions and find sustainable solutions to address this global crisis.",
            "The healthcare industry is an essential component of modern society. It plays a critical role in ensuring that individuals receive the medical care they need to maintain their health and well-being. The industry is constantly evolving, driven by advances in technology and changes in demographics and healthcare needs",
            "The use of technology has revolutionized the way we live and work. From communication and entertainment to education and commerce, technology has transformed virtually every aspect of our lives. While it has brought many benefits, it has also created new challenges, such as privacy concerns and the need to balance the benefits with the potential harm.",
            "The importance of physical activity cannot be overstated. Regular exercise helps to maintain a healthy weight, reduce the risk of chronic diseases, and improve mental health and well-being. Whether through organized sports, individual pursuits, or simply walking or cycling, incorporating physical activity into our daily routines is vital for our health and happiness.",
            "The natural environment is a precious resource that we must protect for future generations. Deforestation, pollution, and climate change are just some of the issues that threaten the health of our planet. It is up to us to take action to reduce our impact on the environment, through conservation efforts, sustainable living, and responsible use of natural resources",
            "The arts play a vital role in our cultural heritage and personal expression. From painting and music to dance and literature, the arts enrich our lives and provide us with a means to connect with one another and the world around us. Supporting the arts, through funding and attendance, is crucial for preserving and promoting our artistic heritage.",
            "Good nutrition is essential for a healthy body and mind. Eating a balanced diet that includes a variety of fruits, vegetables, whole grains, and lean proteins provides us with the nutrients we need to thrive. It is important to be mindful of portion sizes and to limit our intake of unhealthy fats, sugars, and salts. By making healthy food choices, we can improve our overall health and well-being",

        };

    #endregion // string[] DESC = ...

    #region Ctor

    protected BaseFullTextTests(
            IServiceProvider serviceProvider,
            ITestOutputHelper outputHelper)
        : base(serviceProvider, outputHelper)
    {
    }

    #endregion // Ctor

    protected override void Init(IGraphDB graphDB)
    {
        _graphDB = graphDB;
        CreateIndex().Wait();
        base.Init(graphDB);
    }

    private ILabel Person => throw new NotImplementedException();

    #region partial record PersonEntity

    [Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
    private partial record PersonEntity(string name)
    {
        public required int key { get; init; }
        public required string title { get; init; }
        public string? desc { get; init; } = null;
        public DateTime updatedOn { get; init; }
    }

    #endregion // partial record PersonEntity

    #region FullText1_Test

    [Fact]
    public async Task FullText1_Test()
    {
        var n = Variables.Create<PersonEntity>();
        await CreateData();
        CypherConfig.Scope.Value = CONFIGURATION;
        CypherCommand cypher = _((rate) =>
           FullText(INDEX_NAME, """
                                            "quick brown fox"
                                            """,
                     n, rate,
                     2)
           .With(n, rate)
           .Return(n.__.key, rate));

        _outputHelper.WriteLine($"CYPHER: {cypher}");

        IGraphDBResponse response = await _tx.RunAsync(cypher);
        IAsyncEnumerable<int> keys = response.GetRangeAsync<int>(nameof(n), nameof(n.__.key));
        int[] results = await keys.ToArrayAsync();
        Assert.Equal(1, results[0]);

        var prms = cypher.Parameters.AddOrUpdate("p_0", """
                                            "quick fox"~3
                                            """);

        response = await _tx.RunAsync(cypher, prms);
        keys = response.GetRangeAsync<int>(nameof(n), nameof(n.__.key));
        results = await keys.ToArrayAsync();
        Assert.Equal(1, results[0]);

        prms = cypher.Parameters.AddOrUpdate("p_0", """
                                            "quick fox"
                                            """);

        response = await _tx.RunAsync(cypher, prms);
        keys = response.GetRangeAsync<int>(nameof(n), nameof(n.__.key));
        results = await keys.ToArrayAsync();
        Assert.Empty(results);
    }

    #endregion // FullText1_Test

    #region FullText2_Test

    [Fact]
    public async Task FullText2_Test()
    {
        var n = Variables.Create<PersonEntity>();
        var search = Parameters.Create<string>();

        await CreateData();
        CypherConfig.Scope.Value = CONFIGURATION;
        CypherCommand cypher = _((rate) =>
           FullText(INDEX_NAME, search,
                     n, rate,
                     2)
           .With(n, rate)
           .Return(n.__.key, rate));

        _outputHelper.WriteLine($"CYPHER: {cypher}");
        var prms = cypher.Parameters.AddOrUpdate(nameof(search), """
                                            "quick brown fox"
                                            """);
        IGraphDBResponse response = await _tx.RunAsync(cypher, prms);
        IAsyncEnumerable<int> keys = response.GetRangeAsync<int>(nameof(n), nameof(n.__.key));
        int[] results = await keys.ToArrayAsync();
        Assert.Equal(1, results[0]);
    }

    #endregion // FullText2_Test

    #region CreateIndex

    private async Task CreateIndex()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var n = Variables.Create<PersonEntity>();

        CypherCommand cypher = _(() => TryCreateFullTextIndex(INDEX_NAME, N(n, Person), FullTextAnalyzer.english, n._.title, n._.desc));

        await _graphDB.RunAsync(cypher);
    }

    #endregion // CreateIndex

    #region CreateData

    private async Task CreateData()
    {
        CypherConfig.Scope.Value = CONFIGURATION;
        var items = Parameters.Create<PersonEntity>();
        var n = Variables.Create<PersonEntity>();

        CypherCommand cypher = _(() =>
                                Unwind(items, map =>
                                     Merge(N(n, Person, new { map.__.key /* result in key = map.key*/ }))
                                       .Set(n, map)));

        _outputHelper.WriteLine($"Create CYPHER (prepare): {cypher}");

        CypherParameters prms = cypher.Parameters;
        prms = prms.AddRangeOrUpdate(nameof(items), Enumerable.Range(0, TITLES.Length)
                                .Select(Factory));
        await _tx.RunAsync(cypher, prms);

        PersonEntity Factory(int i) => new PersonEntity($"Person {i}")
        {
            key = i,
            title = TITLES[i],
            desc = DESC[i % DESC.Length],
            updatedOn = DateTime.Now
        };
    }

    #endregion // CreateData

    #region Dispose

    protected override void DisposeAfterTxRollback()
    {
        CypherCommand cypher = _(() => TryDropIndex(INDEX_NAME));

        _outputHelper.WriteLine($"Index CYPHER (prepare): {cypher}");

        _graphDB.RunAsync(cypher).AsTask().Wait();
    }

    #endregion // Dispose
}
