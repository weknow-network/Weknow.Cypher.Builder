
namespace Weknow.GraphDbCommands
{
    [Obsolete("It's better to use the Cypher methods instead of clear text as log as it supported", false)]
    public class RawCypher
    {
        private readonly string _cypher = string.Empty;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RawCypher"/> class.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        public RawCypher(string cypher)
        {
            _cypher = cypher;
        }

        #endregion // Ctor

        #region Casting Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="RawCypher"/>.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator RawCypher(string cypher) => new RawCypher(cypher);

        #endregion // Casting Overloads

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => _cypher;

        #endregion // ToString

    }
}
