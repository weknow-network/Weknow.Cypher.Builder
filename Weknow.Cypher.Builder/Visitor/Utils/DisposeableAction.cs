using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Dispose-able action
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DisposeableAction : IDisposable
    {
        /// <summary>
        /// Empty
        /// </summary>
        public static readonly IDisposable Empty = new DisposeableAction(() => { });

        private readonly Action _action;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposeableAction"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public DisposeableAction(Action action)
        {
            _action = action;
        }

        #endregion // Ctor

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _action();
        }

        #endregion // Dispose
    }
}
