using FakeItEasy;

using Microsoft.Extensions.DependencyInjection;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public class NonTxBaseIntegrationTests : IDisposable
{
    protected readonly IGraphDB _graphDB;
    protected readonly IGraphDBRunner _runner;
    protected readonly ITestOutputHelper _outputHelper;
    protected const string TEST_LABEL = nameof(_Test_);
    protected ILabel _Test_ => throw new NotImplementedException();
    private readonly IServiceProvider _serviceProvider;

    private readonly Microsoft.Extensions.Logging.ILogger _logger = A.Fake<Microsoft.Extensions.Logging.ILogger>();

    #region Action<CypherConfig> CONFIGURATION = ...

    protected Action<CypherConfig> CONFIGURATION = cfg =>
    {
        cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
        cfg.AmbientLabels.Add(TEST_LABEL);
    };

    #endregion // Action<CypherConfig> CONFIGURATION = ...

    #region Action<CypherConfig> CONFIGURATION_NO_AMBIENT

    protected Action<CypherConfig> CONFIGURATION_NO_AMBIENT = cfg =>
    {
        cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
    };

    #endregion // Action<CypherConfig> CONFIGURATION_NO_AMBIENT

    #region Ctor

    public NonTxBaseIntegrationTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
    {
        _serviceProvider = serviceProvider;
        _graphDB = serviceProvider.GetRequiredService<IGraphDB>();
        _runner = _graphDB;
        _outputHelper = outputHelper;
        Init(_graphDB);
    }

    #endregion // Ctor

    protected virtual void Init(IGraphDB graphDB)
    {
        // DO Nothing
    }


    #region CleanAsync

    private async Task CleanAsync()
    {
        CypherCommand cypher = _(n =>
                                Match(N(n, _Test_))
                                .DetachDelete(n),
                                CONFIGURATION);
        var res = await _graphDB.RunAsync(cypher);
        await res.GetInfoAsync();
    }

    #endregion // CleanAsync

    #region Dispose

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
        CleanAsync().Wait();
    }

    protected virtual void Dispose(bool disposing) { }
    ~NonTxBaseIntegrationTests()
    {
        Dispose(false);
    }

    #endregion // Dispose
}
