// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper


// https://neo4j.com/docs/cypher-manual/3.5/syntax/operators/

namespace Weknow
{

    public interface ICypherFluentFunctions
    {
        /// <summary>
        /// Labels of the node..
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        ICypherFluent Labels(string nodeVariable);

        /// <summary>
        /// Coalesces The first non-null expression.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>coalesce(n.property, $defaultValue)</example>
        ICypherFluent Coalesce(params string[] expressions);

        /// <summary>
        /// Milliseconds since midnight, January 1, 1970 UTC.
        /// </summary>
        /// <returns></returns>
        /// <example>timestamp()</example>
        ICypherFluent Timestamp();

        /// <summary>
        /// The internal id of the relationship or node.
        /// </summary>
        /// <param name="nodeOrRelationship">The node or relationship.</param>
        /// <returns></returns>
        /// <example>id(nodeOrRelationship)</example>
        ICypherFluent Id(string nodeOrRelationship);

        /// <summary>
        /// Converts the given input into an integer if possible; otherwise it returns null.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example>toInteger($expr)</example>
        ICypherFluent ToInteger(string expression);

        /// <summary>
        /// Converts the given input into a floating point number if possible; otherwise it returns null.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example>toFloat($expr)</example>
        ICypherFluent ToFloat(string expression);

        /// <summary>
        /// Converts the given input into a boolean if possible; otherwise it returns null.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example>toFloat($expr)</example>
        ICypherFluent ToBoolean(string expression);

        /// <summary>
        /// Returns a list of string representations for the property names of a node, relationship, or map.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example>keys($expr)</example>
        ICypherFluent Keys(string expression);

        /// <summary>
        /// Returns a map containing all the properties of a node or relationship.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example>properties($expr)</example>
        ICypherFluent Properties(string expression);
    }
}
