// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: main phrases + prop setup + where

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using static Weknow.Helpers.Helper;
using System.Collections;

#pragma warning disable RCS1102 // Make class static.

namespace Weknow
{
    partial class CypherFactory
    {
        /// <summary>
        /// Cypher Functions
        /// </summary>
        public class F : Function { }
        /// <summary>
        /// Cypher Functions
        /// </summary>
        public class Function
        {
            #region Labels

            /// <summary>
            /// Labels of the node..
            /// </summary>
            /// <param name="nodeVariable">The node variable.</param>
            /// <returns></returns>
            public static FluentCypher Labels(string nodeVariable) => CypherBuilder.Default.Add($"labels({nodeVariable})");

            #endregion // Labels

            #region Coalesce

            /// <summary>
            /// Coalesces the specified first.
            /// </summary>
            /// <param name="first">The first.</param>
            /// <param name="second">The second.</param>
            /// <param name="more">The more.</param>
            /// <returns></returns>
            /// <example><![CDATA[coalesce(n.property, $defaultValue)]]></example>
            public static FluentCypher Coalesce(string first, string second, params string[] more) => CypherBuilder.Default.Add($"coalesce({string.Join(", ", first.ToYield(second, more))})");

            /// <summary>
            /// Coalesces The first non-null expression.
            /// </summary>
            /// <param name="contentExpression"></param>
            /// <returns></returns>
            /// <example><![CDATA[coalesce(n.property, $defaultValue)]]></example>
            public static FluentCypher Coalesce(Func<FluentCypher, FluentCypher> contentExpression)
            {
                return CypherBuilder.Default.Composite(contentExpression, CypherPhrase.Dynamic, "coalesce(", ")");
            }

            /// <summary>
            /// Coalesces The first non-null expression.
            /// </summary>
            /// <param name="content">The delegated.</param>
            /// <returns></returns>
            /// <example><![CDATA[coalesce(n.property, $defaultValue)]]></example>
            public static FluentCypher Coalesce(FluentCypher content)
            {
                return CypherBuilder.Default.Composite(content, CypherPhrase.Dynamic, "coalesce(", ")");
            }

            #endregion // Coalesce

            #region Timestamp

            /// <summary>
            /// Milliseconds since midnight, January 1, 1970 UTC.
            /// </summary>
            /// <returns></returns>
            /// <example><![CDATA[timestamp()]]></example>
            public static FluentCypher Timestamp() => CypherBuilder.Default.Add("timestamp()");

            #endregion // Timestamp

            #region Id

            /// <summary>
            /// The internal id of the relationship or node.
            /// </summary>
            /// <param name="nodeOrRelationship">The node or relationship.</param>
            /// <returns></returns>
            /// <example><![CDATA[id(nodeOrRelationship)]]></example>
            public static FluentCypher Id(string nodeOrRelationship) => CypherBuilder.Default.Add($"id({nodeOrRelationship})");

            #endregion // Id

            #region ToInteger

            /// <summary>
            /// Converts the given input into an integer if possible; otherwise it returns null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example><![CDATA[toInteger($expr)]]></example>
            public static FluentCypher ToInteger(string expression) => CypherBuilder.Default.Add($"toInteger({expression})");

            #endregion // ToInteger

            #region ToFloat

            /// <summary>
            /// Converts the given input into a floating point number if possible; otherwise it returns null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example><![CDATA[toFloat($expr)]]></example>
            public static FluentCypher ToFloat(string expression) => CypherBuilder.Default.Add($"toFloat({expression})");

            #endregion // ToFloat

            #region ToBoolean

            /// <summary>
            /// Converts the given input into a boolean if possible; otherwise it returns null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example><![CDATA[toFloat($expr)]]></example>
            public static FluentCypher ToBoolean(string expression) => CypherBuilder.Default.Add($"toBoolean({expression})");

            #endregion // ToBoolean

            #region Keys

            /// <summary>
            /// Returns a list of string representations for the property names of a node, relationship, or map.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example><![CDATA[keys($expr)]]></example>
            public static FluentCypher Keys(string expression) => CypherBuilder.Default.Add($"keys({expression})");

            #endregion // Keys

            #region Properties

            /// <summary>
            /// Returns a map containing all the properties of a node or relationship.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example><![CDATA[properties($expr)]]></example>
            public static FluentCypher Properties(string expression) => CypherBuilder.Default.Add($"properties({expression})");

            #endregion // Properties      
        }
    }
}
