using System.Threading;

namespace Weknow
{
    /// <summary>
    /// Cypher query result factory observe some context (useful for reactive queries)
    /// </summary>
    public static class CypherResult
    {
        /// <summary>
        /// Create.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static CypherResult<T, TContext> Create<T, TContext>(
                            T item,
                            TContext context,
                            CancellationToken? cancellationToken = null)
        {
            return new CypherResult<T, TContext>(item, context);
        }
    }
    /// <summary>
    /// Cypher query result with some context (useful for reactive queries)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public class CypherResult<T, TContext>
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public CypherResult(T item, TContext context,
                            CancellationToken? cancellationToken = null)
        {
            Item = item;
            Context = context;
            CancellationToken = cancellationToken ?? 
                                System.Threading.CancellationToken.None;
        }

        #endregion // Ctor

        #region Item

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public T Item { get; }

        #endregion // Item

        #region Context

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public TContext Context { get; }

        #endregion // Context

        #region CancellationToken

        /// <summary>
        /// Gets the cancellation token (indication for cancellation).
        /// </summary>
        public CancellationToken CancellationToken { get; }

        #endregion // CancellationToken

        #region Deconstruct

        /// <summary>
        /// Enable the de-construct functionality.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public void Deconstruct(
            out T item,
            out TContext context,
            out CancellationToken cancellationToken)
        {
            item = Item;
            context = Context;
            cancellationToken = CancellationToken;
        }

        #endregion // Deconstruct

        #region Cast Overloads

        /// <summary>
        /// Casting overload.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator T(CypherResult<T, TContext> instance)
        {
            return instance.Item;
        }

        #endregion // Cast Overloads
    }
}
