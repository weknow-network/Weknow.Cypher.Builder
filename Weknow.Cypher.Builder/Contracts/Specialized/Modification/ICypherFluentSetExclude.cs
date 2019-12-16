﻿// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Linq.Expressions;

namespace Weknow.N4J
{

    public interface ICypherFluentSetExclude<T> : ICypherFluent
    {
        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example>
        /// Set<User>().Exclude(user => user.Other)
        /// SET user.Id = $Id, user.Name = $Name // Update or create a property.
        /// </example>
        ICypherFluentSetExclude<T> Exclude(Expression<Func<T, dynamic>> propExpression);
    }
}
