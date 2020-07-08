using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;
using System.Text;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Responsible for abstracting the logic of the actual 
    /// Cypher text building.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal sealed class CypherQueryBuilder: IDisposable
    {
        private static readonly ObjectPoolProvider _objectPoolProvider = new DefaultObjectPoolProvider();
        private static readonly ObjectPool<StringBuilder> _stringBuilderPool = _objectPoolProvider.CreateStringBuilderPool();
        private readonly StringBuilder _builder;

        #region Ctor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public CypherQueryBuilder()
        {
            _builder = _stringBuilderPool.Get();
        }

        #endregion // Ctor

        #region char this[int index]

        /// <summary>
        /// Gets the <see cref="System.Char"/> at the specified index.
        /// </summary>
        /// <returns></returns>
        public char this[int index] => _builder[index];

        #endregion // char this[int index]

        //public Span<char> this[Range range]
        //{
        //    get 
        //    {
        //        int end = range.End.Value;
        //        if (range.End.IsFromEnd)
        //            end = range.End.Value;
        //        int start = 0h;
        //        if (range.Start.IsFromEnd)
        //            start = range.End.Value;

        //        char[] buffer = new char[ - range.Start];
        //           var s = new Span<char>()
        //    }
        //}

        #region Append

        /// <summary>
        /// Appends text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Append<T>(T text)
        {
            _builder.Append(text);
        }

        #endregion // Append

        #region Remove

        /// <summary>
        /// Removes the specified start index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        public void Remove(int startIndex, int length) =>
                            _builder.Remove(startIndex, length);

        #endregion // Remove

        #region Length

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length => _builder.Length;

        #endregion // Length

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _stringBuilderPool.Return(_builder);
        }

        #endregion // Dispose

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => 
                    _builder
                        .Replace("]--(", "]-(") // TODO: Avi review (ugly fix for the issu in tests [Relation_WithReuse_Test])
                        .Replace(")--[", ")-[")
                        .ToString();

        #endregion // ToString
    }
}
