using FakeItEasy;

using Microsoft.Extensions.DependencyInjection;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit.Abstractions;

using static Weknow.CypherBuilder.ICypher;

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public class TxBaseIntegrationTests : IDisposable
{
    protected readonly IGraphDBTransaction _tx;
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

    public TxBaseIntegrationTests(
        IServiceProvider serviceProvider,
        ITestOutputHelper outputHelper)
    {
        _serviceProvider = serviceProvider;
        var graphDB = serviceProvider.GetRequiredService<IGraphDB>();

        _tx = graphDB.StartTransaction().Result;

        _outputHelper = outputHelper;
    }

    #endregion // Ctor


    #region Dispose

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        _tx.RollbackAsync().Wait();
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }
    ~TxBaseIntegrationTests()
    {
        Dispose(false);
    }

    #endregion // Dispose
}