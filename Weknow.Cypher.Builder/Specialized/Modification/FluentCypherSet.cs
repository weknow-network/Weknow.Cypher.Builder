// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using static Weknow.Helpers.Helper;

namespace Weknow
{
    /// <summary>
    /// Additional Sets phrases
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Weknow.CypherBuilder" />
    [DebuggerDisplay("{_cypherCommand}")]
    public class FluentCypherSet<T> : FluentCypher
    {
        // internal static readonly FluentCypherSet<T> Empty = new FluentCypherSet<T>();

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherSet{T}"/> class.
        /// </summary>
        public FluentCypherSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherSet{T}"/> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        public FluentCypherSet(FluentCypher copyFrom, string cypher, CypherPhrase phrase)
            : base(copyFrom, cypher, phrase)
        {
        }

        #endregion // Ctor

        #region SetMore

        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <param name="propExpression">The property expression.</param>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSign">The parameter sign.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example><![CDATA[
        /// Set((User user) =&gt; user.Name).Also(user =&gt; user.Id)
        /// SET user.Name = $user.Name, user.Id = $user.Id // Update or create a property.
        /// ]]></example>
        public FluentCypherSet<T> SetMore(
            Expression<Func<T, object>> propExpression,
            string? parameterPrefix = null,
            string parameterSign = "$")
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            string statement = $"{variable}.{name} = {parameterSign}{parameterPrefix}{name}";
            var result = new FluentCypherSet<T>(this, statement, CypherPhrase.Set);
            return result;
        }

        #endregion // SetMore
    }
}
