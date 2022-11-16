using FakeItEasy;

using Microsoft.Extensions.DependencyInjection;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbCommands;

using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public class BaseIntegrationTests : IDisposable
{
    protected readonly IGraphDB _graphDB;
    protected readonly ITestOutputHelper _outputHelper;
    protected const string TEST_LABEL = nameof(_Test_);
    protected ILabel _Test_ => throw new NotImplementedException();
    private readonly IServiceProvider _serviceProvider;

    private readonly Microsoft.Extensions.Logging.ILogger _logger = A.Fake<Microsoft.Extensions.Logging.ILogger>();

    #region Action<CypherConfig> CONFIGURATION = ...

    protected Action<CypherConfig> CONFIGURATION = cfg =>
    {
        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
        cfg.AmbientLabels.Add(TEST_LABEL);
    };

    #endregion // Action<CypherConfig> CONFIGURATION = ...

    #region Action<CypherConfig> CONFIGURATION_NO_AMBIENT

    protected Action<CypherConfig> CONFIGURATION_NO_AMBIENT = cfg =>
    {
        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
    };

    #endregion // Action<CypherConfig> CONFIGURATION_NO_AMBIENT

    #region Ctor

    public BaseIntegrationTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
    {
        _serviceProvider = serviceProvider;
        IGraphDB graphDB = serviceProvider.GetRequiredService<IGraphDB>();

        _graphDB = graphDB;
        _outputHelper = outputHelper;
    }

    #endregion // Ctor

    #region CleanAsync

    private async Task CleanAsync()
    {
        CypherCommand cypher = _(n =>
                                Match(N(n))
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
        Dispose(true);
        CleanAsync().Wait();
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }
    ~BaseIntegrationTests()
    {
        Dispose(false);
    }

    #endregion // Dispose
}
