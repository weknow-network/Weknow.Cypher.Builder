namespace Weknow.GraphDbClient.Abstraction;


public interface IGraphDBTransaction : IGraphDBRunner, IAsyncDisposable
{
    /// <summary>
    /// Asynchronously commit this transaction.
    /// </summary>
    /// <returns></returns>
    Task CommitAsync();

    /// <summary>
    /// Asynchronously roll back this transaction.
    /// </summary>
    /// <returns></returns>
    Task RollbackAsync();
}