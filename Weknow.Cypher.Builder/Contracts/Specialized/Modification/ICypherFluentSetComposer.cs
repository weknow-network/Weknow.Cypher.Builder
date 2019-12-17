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
    public interface ICypherFluentSetComposer<T>: ICypherable
    {
        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example>
        /// Set((User user) => user.Name).Also(user => user.Id)
        /// SET user.Name = $Name, user.Id = $Id // Update or create a property.
        /// </example>
        ICypherFluentSetComposer<T> Also(Expression<Func<T, dynamic>> propExpression);
    }

    public interface ICypherFluentForSetComposerExclude<T> : ICypherable
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
        ICypherFluentForSetComposerExclude<T> Exclude(Expression<Func<T, dynamic>> propExpression);
    }

    public interface ICypherFluentForSetComposer: ICypherable
    {

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// Set("n", new [] { nameof(Foo.Name), nameof(Bar.Id)})
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// </example>
        string Set(string variable, IEnumerable<string> propNames);

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// Set("n", nameof(Foo.Name), nameof(Bar.Id))
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// </example>
        string Set(string variable, params string[] propNames);

        /// <summary>
        /// Compose SET phrase by convention.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example>
        /// Set((User user) => user.Name.StartWith("Name"))
        /// SET user.FirstName = $FirstName, usr.LastName = $LastName // Update or create a property.
        /// </example>
        string SetByConvention(Func<string, bool> filter);

        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example>
        /// Set((User user) => user.Name)
        /// SET user.Name = $Name // Update or create a property.
        /// </example>
        ICypherFluentSetComposer<T> Set<T>(Expression<Func<T, dynamic>> propExpression);

        /// <summary>
        /// Compose SET phrase by reflection with exclude option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludes">The excludes.</param>
        /// <returns></returns>
        /// <example>
        /// Set<User>()
        /// SET user.Id = $Id, user.Name = $Name, user.Other = $Other // Update or create a property.
        /// </example>
        ICypherFluentForSetComposerExclude<T> SET<T>();
    }
}
