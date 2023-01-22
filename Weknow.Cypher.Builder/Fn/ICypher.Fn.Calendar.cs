using Weknow.CypherBuilder.Declarations;

// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
// https://neo4j.com/docs/cypher-manual/5/functions/temporal/

namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher Function Extensions
/// </summary>
public partial interface ICypher
{
    partial interface IFn
    {
        /// <summary>
        /// Calendar functions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICypherCalendar Cal => throw new NotImplementedException();
        /// <summary>
        /// Calendar functions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICypherCalendar Calendar => throw new NotImplementedException();
    }

    public interface ICypherCalendar
    {
        #region timestamp()

        /// <summary>
        /// Milliseconds since midnight, January 1, 1970 UTC.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN timestamp()
        /// </example>
        [Cypher("timestamp()")]
        public TimeVariableDeclaration Timestamp() => throw new NotImplementedException();

        #endregion // timestamp()

        [Cypher("date()")]
        public TimeVariableDeclaration Date() => throw new NotImplementedException();

        [Cypher("localdate()")]
        public TimeVariableDeclaration LocalDate() => throw new NotImplementedException();

        [Cypher("time()")]
        public TimeVariableDeclaration Time() => throw new NotImplementedException();

        [Cypher("localtime()")]
        public TimeVariableDeclaration LocalTime() => throw new NotImplementedException();

        [Cypher("datetime()")]
        public TimeVariableDeclaration DateTime() => throw new NotImplementedException();

        [Cypher("localdatetime()")]
        public TimeVariableDeclaration LocalDateTime() => throw new NotImplementedException();
    }
}