// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Weknow
{

    public interface IFluentCypherSet<T> : IFluentCypher
    {
        /// <summary>
        /// Compose SET continuation phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example>
        /// Set((User user) => user.Name).Also(user => user.Id)
        /// SET user.Name = $Name, user.Id = $Id // Update or create a property.
        /// </example>
        IFluentCypherSet<T> SetMore(Expression<Func<T, object>> propExpression);
    }
}
