using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// <param name="authToken">The authentication token.</param>
    /// <param name="envVarPrefix">The environment variable prefix.</param>
    /// <returns></returns>
    public static IDriver CreateDriver(IAuthToken? authToken = null, string envVarPrefix = DEFAULT_ENV_VAR_PREFIX)
    {
        string connectionString = Environment.GetEnvironmentVariable($"{envVarPrefix}URL") ?? throw new ArgumentNullException("NEO4J_URL");
        if (authToken == null)
        {
            string userName = Environment.GetEnvironmentVariable($"{envVarPrefix}USER") ?? "neo4j";
            string password = Environment.GetEnvironmentVariable($"{envVarPrefix}PASS") ?? throw new ArgumentNullException("NEO4J_PASS");

            authToken = AuthTokens.Basic(userName, password);
        }

        IDriver driver = GraphDatabase.Driver(connectionString, authToken);
        return driver;
    }
}
