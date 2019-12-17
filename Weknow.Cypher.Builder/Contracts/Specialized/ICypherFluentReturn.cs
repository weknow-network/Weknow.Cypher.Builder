// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper


// https://neo4j.com/docs/cypher-manual/3.5/syntax/operators/

namespace Weknow
{

    public interface ICypherFluentReturn: IFluentCypher
    {
        #region OrderBy 

        /// <summary>
        /// Create ORDER BY phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property
        /// </example>
        ICypherFluentReturn OrderBy(string statement);

        #endregion // OrderBy

        #region OrderByDesc 

        /// <summary>
        /// Create ORDER BY DESC phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property DESC
        /// </example>
        ICypherFluentReturn OrderByDesc(string statement);

        #endregion // OrderByDesc

        #region Skip 

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// SKIP $skipNumber
        /// </example>
        ICypherFluentReturn Skip(string statement);

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example>
        /// SKIP 10
        /// </example>
        ICypherFluentReturn Skip(int number);

        #endregion // Skip

        #region Limit 

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// LIMIT $skipNumber
        /// </example>
        ICypherFluentReturn Limit(string statement);

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example>
        /// LIMIT 10
        /// </example>
        ICypherFluentReturn Limit(int number);

        #endregion // Limit

        #region Count 

        /// <summary>
        /// Create count function.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// RETURN count(*)
        /// </example>
        ICypherFluentReturn Count();

        #endregion // Limit
    }
}
