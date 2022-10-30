using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Neo4j.Driver;

namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j provider abstraction
/// </summary>
internal static class N4jProvider
{
    public const string DEFAULT_ENV_VAR_PREFIX = "NEO4J_";
    /// <summary>
    /// Creates the driver.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="authToken">The authentication token.</param>
    /// <param name="envVarPrefix">The environment variable prefix.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">NEO4J_URL
    /// or
    /// NEO4J_PASS</exception>
    public static IDriver CreateDriver(
        IServiceProvider serviceProvider,
        IAuthToken? authToken = null,
        string envVarPrefix = DEFAULT_ENV_VAR_PREFIX,
        Microsoft.Extensions.Logging.ILogger? logger = null)
    {
        string connectionString = Environment.GetEnvironmentVariable($"{envVarPrefix}URL") ?? throw new ArgumentNullException("NEO4J_URL");
        if (authToken == null)
        {
            string userName = Environment.GetEnvironmentVariable($"{envVarPrefix}USER") ?? "neo4j";
            string password = Environment.GetEnvironmentVariable($"{envVarPrefix}PASS") ?? throw new ArgumentNullException("NEO4J_PASS");

            authToken = AuthTokens.Basic(userName, password);
        }

        logger = logger ?? serviceProvider.GetService<ILogger<N4jGraphDB>>() ?? throw new ArgumentNullException("ILogger<N4jGraphDB>");
        IDriver driver = GraphDatabase.Driver(connectionString, authToken, c=> c.WithLogger(new Logger(logger)));
        return driver;
    }

    #region class Logger

    private class Logger : Neo4j.Driver.ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public Logger(Microsoft.Extensions.Logging.ILogger logger)
        {
            _logger = logger;
        }

        void Neo4j.Driver.ILogger.Debug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        void Neo4j.Driver.ILogger.Error(Exception cause, string message, params object[] args)
        {
            _logger.LogError(cause, message, args);
        }

        void Neo4j.Driver.ILogger.Info(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        void Neo4j.Driver.ILogger.Trace(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        void Neo4j.Driver.ILogger.Warn(Exception cause, string message, params object[] args)
        {
            _logger.LogWarning(cause, message, args);
        }

        bool Neo4j.Driver.ILogger.IsDebugEnabled() => Debugger.IsAttached;

        bool Neo4j.Driver.ILogger.IsTraceEnabled() => false;
    }

    #endregion // class Logger
}
