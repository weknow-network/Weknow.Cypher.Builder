using Neo4j.Driver;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbClient.Neo4jProvider;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Neo4j scoped DI 
/// </summary>
public static class RegistrationOfN4jProvider
{
    private static readonly TimeSpan DEFAULT_HEALTH_TIMEOUNT = Debugger.IsAttached ?
                                                            TimeSpan.FromMinutes(5) :
                                                            TimeSpan.FromSeconds(30);

    /// <summary>
    /// Scoped registration for module's dependencies.
    /// Fit for Web-API
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="healthBuilder">The health builder.</param>
    /// <param name="authToken">The authentication token.</param>
    /// <param name="envVarPrefix">The environment variable prefix.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterScopedNeo4j(
        this IServiceCollection services,
        IHealthChecksBuilder? healthBuilder = null,
        IAuthToken? authToken = null,
        string envVarPrefix = N4jProvider.DEFAULT_ENV_VAR_PREFIX,
        Microsoft.Extensions.Logging.ILogger? logger = null)
    {
        services.AddSingleton(m => m.CreateDriver(authToken, envVarPrefix, logger));
        services.AddScoped<N4jSession>();
        services.AddScoped<IGraphDB, N4jGraphDB>();
        healthBuilder.RegisterHealthCheck();

        return services;
    }

    /// <summary>
    /// Singleton registration for module's dependencies.
    /// Fit for Jobs
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="healthBuilder">The health builder.</param>
    /// <param name="authToken">The authentication token.</param>
    /// <param name="envVarPrefix">The environment variable prefix.</param>
    /// <param name="">The .</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterSingletonNeo4j(
        this IServiceCollection services,
        IHealthChecksBuilder? healthBuilder = null,
        IAuthToken? authToken = null,
        string envVarPrefix = N4jProvider.DEFAULT_ENV_VAR_PREFIX,
        Microsoft.Extensions.Logging.ILogger? logger = null)
    {
        services.AddSingleton(m => m.CreateDriver(authToken, envVarPrefix, logger));
        services.AddSingleton<N4jSession>();
        services.AddSingleton<IGraphDB, N4jGraphDB>();

        healthBuilder.RegisterHealthCheck();

        return services;
    }

    #region CreateDriver

    /// <summary>
    /// Creates the driver.
    /// </summary>
    /// <param name="sp">The sp.</param>
    /// <param name="authToken">The authentication token.</param>
    /// <param name="envVarPrefix">The environment variable prefix.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    private static IDriver CreateDriver(this IServiceProvider sp,
        IAuthToken? authToken,
        string envVarPrefix,
        Microsoft.Extensions.Logging.ILogger? logger) => N4jProvider.CreateDriver(sp, authToken, envVarPrefix,
                        logger);


    #endregion // CreateDriver

    #region RegisterHealthCheck

    /// <summary>
    /// Register Health the check.
    /// </summary>
    /// <param name="healthBuilder">The health builder.</param>
    private static void RegisterHealthCheck(this IHealthChecksBuilder? healthBuilder)
    {
        healthBuilder?.AddTypeActivatedCheck<N4jHealth>(
                nameof(N4jHealth),
                failureStatus: null,
                tags: new[] { "health" },
                timeout: DEFAULT_HEALTH_TIMEOUNT);
    }

    #endregion // RegisterHealthCheck
}
