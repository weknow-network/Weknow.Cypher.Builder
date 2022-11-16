using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

using Neo4j.Driver;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// REDIS health check
/// </summary>
internal class N4jHealth : IHealthCheck
{
    private static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(10);
    private readonly IDriver _driver;
    private readonly ILogger<N4jHealth> _logger;

    #region Ctor

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="driver">The driver.</param>
    /// <param name="logger">The logger.</param>
    public N4jHealth(
        IDriver driver,
        ILogger<N4jHealth> logger)
    {
        _driver = driver;
        _logger = logger;
    }

    #endregion // Ctor

    #region IHealthCheck.CheckHealth

    /// <summary>
    /// Runs the health check, returning the status of the component being checked.
    /// </summary>
    /// <param name="context">A context object associated with the current execution.</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> that can be used to cancel the health check.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task`1" /> that completes when the health check has finished, yielding the status of the component being checked.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    async Task<HealthCheckResult> IHealthCheck.CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken)
    {
        try
        {
            await _driver.VerifyConnectivityAsync();
            return HealthCheckResult.Healthy(nameof(N4jHealth));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.FormatLazy());
            _logger.LogWarning("Health issue on [{component}]", nameof(N4jHealth));
            return HealthCheckResult.Unhealthy(nameof(N4jHealth));
        }
    }

    #endregion IHealthCheck.CheckHealthAsync

}