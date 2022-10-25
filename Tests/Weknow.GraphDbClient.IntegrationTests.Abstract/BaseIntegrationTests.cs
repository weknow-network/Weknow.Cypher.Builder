using Neo4j.Driver;

using Weknow.GraphDbCommands;
using Weknow.GraphDbClient.Abstraction;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public class BaseIntegrationTests : IDisposable
{
    protected readonly IGraphDB _graphDB;
    protected readonly ITestOutputHelper _outputHelper;
    protected readonly IAsyncSession _session;
    private const string TEST_LABEL = nameof(_Test_);
    private ILabel _Test_ => throw new NotImplementedException();

    #region Action<CypherConfig> CONFIGURATION = ...

    protected Action<CypherConfig> CONFIGURATION = cfg =>
    {
        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
        cfg.AmbientLabels.Add(TEST_LABEL);
    };

    #endregion // Action<CypherConfig> CONFIGURATION = ...

    #region Ctor

    public BaseIntegrationTests(
        IGraphDB graphDB,
        ITestOutputHelper outputHelper)
    {
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
        await _graphDB.RunAsync(cypher);
    }

    #endregion // CleanAsync

    #region Dispose

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public void Dispose()
    {
        Dispose(true);
        CleanAsync().Wait();
        _session.CloseAsync().Wait();
        GC.SuppressFinalize(this);
    }
#pragma warning restore CA1063 // Implement IDisposable Correctly

    protected virtual void Dispose(bool disposing) { }
    ~BaseIntegrationTests()
    {
        Dispose(false);
    }

    #endregion // Dispose
}
