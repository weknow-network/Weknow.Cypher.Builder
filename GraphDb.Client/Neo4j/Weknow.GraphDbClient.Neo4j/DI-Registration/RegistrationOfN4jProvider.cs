using System;
using System.Threading;
using System.Xml.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    /// Registers module's dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="healthBuilder">The health builder.</param>
    /// <param name="authToken">The authentication token.</param>
    /// <param name="envVarPrefix">The environment variable prefix.</param>
    /// <param name="">The .</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterNeo4j(
        this IServiceCollection services,
        IHealthChecksBuilder? healthBuilder = null,
        IAuthToken? authToken = null,
        string envVarPrefix = N4jProvider.DEFAULT_ENV_VAR_PREFIX,
        Microsoft.Extensions.Logging.ILogger? logger = null)
    {
        services.AddSingleton(sp => N4jProvider.CreateDriver(sp, authToken, envVarPrefix,
                logger));
        services.AddScoped<IGraphDB, N4jGraphDB>();
        services.AddScoped<N4jSession>();
        //services.AddScoped<IAsyncSession>((sp) =>
        //{
        //    var n4jSession = sp.GetService<N4jSession>() ?? throw new ArgumentNullException("N4jSession injection");
        //    return n4jSession.Session;
        //});

        //healthBuilder = healthBuilder ?? services.AddHealthChecks();
        healthBuilder?.AddTypeActivatedCheck<N4jHealth>(
                nameof(N4jHealth),
                failureStatus: null,
                tags: new[] { "health" },
                timeout: DEFAULT_HEALTH_TIMEOUNT);

        return services;
    }
}
