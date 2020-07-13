using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Responsible for abstracting the logic of the actual 
    /// Cypher text building.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal sealed class CypherQueryBuilder : IDisposable, IEnumerable<char>
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

        #region string this[Range range]

        /// <summary>
        /// <![CDATA[Gets the <see cref="ReadOnlySpan{System.Char}" /> with the specified start index.]]>
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public ReadOnlySpan<char> this[int startIndex, int length] => GetRange(startIndex, length);

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public ReadOnlySpan<char> this[Range range]
        {
            get
            {
                int from = range.Start.Value;
                if (range.Start.IsFromEnd)
                    from = _builder.Length - from;
                int to =  range.End.Value;
                if (range.End.IsFromEnd)
                    to = _builder.Length - to;
                ReadOnlySpan<char> result = GetRange(from, to - from);
                return result;
            }
        }

        #endregion // string this[Range range]

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
                        .ToString();

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(Range range) =>
            new string(this[range]);

        #endregion // ToString

        #region IEnumerable<char>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<char> GetEnumerator()
        {
            foreach (ReadOnlyMemory<char> chunk in _builder.GetChunks())
            {
                for (int i = 0; i < chunk.Length; i++)
                {
                    yield return chunk.Span[i];
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion // IEnumerable<char>

        #region GetRange

        /// <summary>
        /// Gets the range.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        private ReadOnlySpan<char> GetRange(int startIndex, int length)
        {
            Span<char> result = new char[length];
            
            int index = 0;
            foreach (ReadOnlyMemory<char> chunk in _builder.GetChunks())
            {
                ReadOnlySpan<char> span = chunk.Span;
                int len = span.Length;
                if (len + index < startIndex)
                {
                    index += len;
                    continue;
                }
                int startPoint = startIndex - index;
                var enumerator = chunk.Span.GetEnumerator();
                for (int i = 0; i < startPoint; i++)
                {
                    enumerator.MoveNext();
                    index++;
                }
                int end = startIndex + length;
                while (enumerator.MoveNext() && index < end)
                {
                    result[index - startIndex] = enumerator.Current;
                    index++;
                }
                if (index >= end)
                    break;
            }
            return result;
        }

        #endregion // GetRange
    }
}
