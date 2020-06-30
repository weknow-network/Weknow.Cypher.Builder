#pragma warning disable CA1063 // Implement IDisposable Correctly

using System;
using System.Diagnostics;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Represent Formatting state.
    /// Relevant to parsing of CypherAttribute's Format
    /// </summary>
    [DebuggerDisplay("{Format} » '{Current}'/[{Index}]")]
    public class FormatingState
    {
        /// <summary>
        /// Singleton
        /// </summary>
        public static readonly FormatingState Default = new FormatingState(string.Empty);

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatingState"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public FormatingState(string format)
        {
            Format = format;
        }

        #endregion // Ctor

        #region Index

        /// <summary>
        /// Gets the current index.
        /// </summary>
        public int Index { get; private set; }

        #endregion // Index

        #region Format

        /// <summary>
        /// Gets the format.
        /// </summary>
        public string Format { get; }

        #endregion // Format

        #region Current

        /// <summary>
        /// Gets the current char.
        /// </summary>
        public char Current => Ended ? '\0' : Format[Index];

        #endregion // Current

        #region Ended

        /// <summary>
        /// Gets a value indicating whether this <see cref="FormatingState"/> is ended.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ended; otherwise, <c>false</c>.
        /// </value>
        public bool Ended => Index >= Format.Length;

        #endregion // Ended

        #region Operator Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="FormatingState"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(FormatingState item) => item.Index;


        /// <summary>
        /// Implements the operator ++.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static FormatingState operator ++(FormatingState instance) 
        {
            instance.Index += 1;
            return instance;
        }

        /// <summary>
        /// Implements the operator --.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static FormatingState operator --(FormatingState instance) 
        {
            instance.Index -= 1;
            return instance;
        }

        #endregion // Operator Overloads
    }
}
