#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable S3881 // "IDisposable" should be implemented correctly

using FakeItEasy;

using Microsoft.Extensions.DependencyInjection;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using Xunit.Abstractions;

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public class TxBaseIntegrationTests : IDisposable
{
    protected IGraphDBTransaction _tx;
    protected IGraphDBRunner _runner;
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
        Init(graphDB);

        _outputHelper = outputHelper;
    }

    #endregion // Ctor

    #region Init

    protected virtual void Init(IGraphDB graphDB)
    {
        _tx = graphDB.StartTransaction().Result;
        _runner = _tx;
    }

    #endregion // Init

    #region Dispose

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        _tx.RollbackAsync().Wait();
        DisposeAfterTxRollback();
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }

    protected virtual void DisposeAfterTxRollback() { }

    ~TxBaseIntegrationTests()
    {
        Dispose(false);
    }

    #endregion // Dispose
}
