using Neo4j.Driver;

namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal class N4jSession : IDisposable
{
    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="N4jSession"/> class.
    /// </summary>
    /// <param name="driver">The driver.</param>
    public N4jSession(IDriver driver)
    {
        string database = Environment.GetEnvironmentVariable("NEO4J_DB") ?? "neo4j";
        Session = driver.AsyncSession(o =>
        {
            o.WithDatabase(database);
            //o.WithDefaultAccessMode(AccessMode.Read);
        });
    }

    #endregion // Ctor

    #region Session

    /// <summary>
    /// Gets the session.
    /// </summary>
    public IAsyncSession Session { get; }

    #endregion // Session

    #region Dispose pattern

    /// <summary>
    /// Finalizes an instance of the <see cref="N4jSession"/> class.
    /// </summary>
    ~N4jSession()
    {
        GC.SuppressFinalize(this);
        Dispose(false);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() => Dispose(true);

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    public void Dispose(bool disposing)
    {
        Session.Dispose();
    }

    #endregion // Dispose pattern
}
