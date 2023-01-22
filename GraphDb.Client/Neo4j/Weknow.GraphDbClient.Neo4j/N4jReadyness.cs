using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

using static Weknow.CypherBuilder.ICypher;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// REDIS health check
/// </summary>
internal class N4jReadiness : IHealthCheck
{
    private static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(10);
    private readonly ILogger<N4jReadiness> _logger;
    private readonly IGraphDB _graphDB;
    private readonly CypherCommand _cypher;
    private readonly ILabel __READINESS__ = ILabel.Fake;

    #region Ctor

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="driver">The driver.</param>
    /// <param name="logger">The logger.</param>
    public N4jReadiness(
            ILogger<N4jReadiness> logger,
            IGraphDB graphDB)
    {
        _logger = logger;
        _graphDB = graphDB;

        _cypher = _((n) => Merge(N(n, __READINESS__))
                            .SetDateConvention(n));
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
            await using var tx = await _graphDB.StartTransaction();
            try
            {
                await tx.RunAsync(_cypher);
                return HealthCheckResult.Healthy(nameof(N4jReadiness));
            }
            catch (Exception ex)
            {
                await tx.CommitAsync();

                _logger.LogWarning(ex.FormatLazy(), "Readiness issue on [{component}]", nameof(N4jReadiness));
                return HealthCheckResult.Unhealthy(nameof(N4jReadiness));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.FormatLazy(), "Readiness Transaction issue on [{component}]", nameof(N4jReadiness));
            return HealthCheckResult.Unhealthy(nameof(N4jReadiness));
        }
    }

    #endregion IHealthCheck.CheckHealthAsync

}