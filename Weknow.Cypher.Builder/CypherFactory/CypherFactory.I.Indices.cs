// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: main phrases + prop setup + where

#pragma warning disable RCS1102 // Make class static.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using static Weknow.Helpers.Helper;
using System.Collections;

namespace Weknow
{
    partial class CypherFactory
    {
        /// <summary>
        /// Cypher Indices & Constraints
        /// </summary>
        public class I : Indices { }
        /// <summary>
        /// Cypher Aggregate Function
        /// </summary>
        public class Indices
        {
            #region CreateIndex

            /// <summary>
            /// Create an index on label and property.
            /// if the compositePropertyNames isn't empty it
            /// Create a composite index on the label and the all properties names (propertyName + compositePropertyNames).
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName">Name of the property.</param>
            /// <returns></returns>
            /// <example>
            /// CREATE INDEX ON :Person(name)
            /// </example>
            public static FluentCypher CreateIndex(
                string label,
                string propertyName,
                params string[] compositePropertyNames)
            {
                if (compositePropertyNames != null && compositePropertyNames.Length != 0)
                {
                    var properties = propertyName.ToYield(compositePropertyNames);
                    var indices = string.Join(", ", properties);
                    return CypherBuilder.Default.Add($"CREATE INDEX ON :{label}({indices})");
                }
                return CypherBuilder.Default.Add($"CREATE INDEX ON :{label}({propertyName})");
            }

            #endregion // CreateIndex

            #region DropIndex

            /// <summary>
            /// Drop the index on the label and properties..
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName">Name of the property.</param>
            /// <returns></returns>
            public static FluentCypher DropIndex(
                string label,
                string propertyName,
                params string[] compositePropertyNames)
            {
                if (compositePropertyNames != null && compositePropertyNames.Length != 0)
                {
                    var properties = propertyName.ToYield(compositePropertyNames);
                    var indices = string.Join(",", properties);
                    return CypherBuilder.Default.Add($"DROP INDEX ON :{label}({indices}");
                }
                return CypherBuilder.Default.Add($"DROP INDEX ON :{label}({propertyName})");
            }

            #endregion // DropIndex

            #region CreateUniqueConstraint

            /// <summary>
            /// Create a unique property constraint on the label and property.
            /// If any other node with that label is updated or 
            /// created with a name that already exists, 
            /// the write operation will fail. 
            /// This constraint will create an accompanying index.
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName">Name of the property.</param>
            /// <returns></returns>
            public static FluentCypher CreateUniqueConstraint(
                string label,
                string propertyName)
            {
                return CypherBuilder.Default.Add($"CREATE CONSTRAINT ON (n:{label}) ASSERT n.{propertyName} IS UNIQUE");
            }

            #endregion // CreateUniqueConstraint

            #region DropUniqueConstraint

            /// <summary>
            /// Drop the unique constraint and index on the label and property.
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName">Name of the property.</param>
            /// <returns></returns>
            public static FluentCypher DropUniqueConstraint(
                string label,
                string propertyName)
            {
                return CypherBuilder.Default.Add($"DROP CONSTRAINT ON (n:{label}) ASSERT n.{propertyName} IS UNIQUE");
            }

            #endregion // DropUniqueConstraint

            #region CreateExistsConstraint

            /// <summary>
            /// Create a node property existence constraint on the label and property. 
            /// If a node with that label is created without a name, 
            /// or if the name property is removed 
            /// from an existing node with the Person label, 
            /// the write operation will fail.
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName">Name of the property.</param>
            /// <returns></returns>
            public static FluentCypher CreateExistsConstraint(
                string label,
                string propertyName)
            {
                return CypherBuilder.Default.Add($"CREATE CONSTRAINT ON (n:{label}) ASSERT exists(n.{propertyName})");
            }

            #endregion // CreateExistsConstraint

            #region DropExistsConstraint

            /// <summary>
            /// Drop the node property existence constraint on the label and property.
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName">Name of the property.</param>
            /// <returns></returns>
            public static FluentCypher DropExistsConstraint(
                string label,
                string propertyName)
            {
                return CypherBuilder.Default.Add($"DROP CONSTRAINT ON (n:{label}) ASSERT exists(n.{propertyName})");
            }

            #endregion // DropExistsConstraint

            #region CreateNodeKeyConstraint

            /// <summary>
            /// Create a Node Key constraint on the Person and properties.
            /// If a node with that label is created without both one of the properties or 
            /// if the combination of the properties is not unique, the write operation will fail.
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName1">The property name1.</param>
            /// <param name="propertyName2">The property name2.</param>
            /// <param name="morePropertyNames">The more property names.</param>
            /// <returns></returns>
            public static FluentCypher CreateNodeKeyConstraint(
                string label,
                string propertyName1,
                string propertyName2,
                params string[] morePropertyNames)
            {
                    var properties = propertyName1.ToYield(propertyName2, morePropertyNames)
                                                .Select(m => CypherBuilder.Default.Add($"n.{m}"));
                    var indices = string.Join(",", properties);
                    return CypherBuilder.Default.Add($"CREATE CONSTRAINT ON (n:{label}) ASSERT ({indices}) IS NODE KEY");
            }

            #endregion // CreateNodeKeyConstraint

            #region DropNodeKeyConstraint

            /// <summary>
            /// Drop the Node Key constraint on the label and properties.
            /// </summary>
            /// <param name="label">The label.</param>
            /// <param name="propertyName1">The property name1.</param>
            /// <param name="propertyName2">The property name2.</param>
            /// <param name="morePropertyNames">The more property names.</param>
            /// <returns></returns>
            public static FluentCypher DropNodeKeyConstraint(
                string label,
                string propertyName1,
                string propertyName2,
                params string[] morePropertyNames)
            {
                    var properties = propertyName1.ToYield(propertyName2, morePropertyNames)
                                                .Select(m => CypherBuilder.Default.Add($"n.{m}"));
                    var indices = string.Join(",", properties);
                    return CypherBuilder.Default.Add($"DROP CONSTRAINT ON (n:{label}) ASSERT ({indices}) IS NODE KEY");
            }

            #endregion // DropNodeKeyConstraint
        }
    }
}