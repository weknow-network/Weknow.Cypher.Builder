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

    public interface ICypherFluentSet<T> : ICypherFluent
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
        ICypherFluentSet<T> SetMore(Expression<Func<T, object>> propExpression);
    }

    public interface ICypherFluentSet
    {
        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// SET n.property1 = $value1, n.property2 = $value2 // Update or create a property.
        /// SET n = $map // Update or create a property.
        /// SET n += $map // Add and update properties, while keeping existing ones.
        /// SET n:Person // Adds a label Person to a node.
        /// </example>
        ICypherFluent Set(string statement);

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
        ICypherFluent Set(string variable, IEnumerable<string> propNames);

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
        ICypherFluent Set(string variable, params string[] propNames);

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
        ICypherFluentSet<T> Set<T>(Expression<Func<T, dynamic>> propExpression);

        /// <summary>
        /// Compose SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example>
        /// Set((User user) =&gt; user.Name.StartWith("Name"))
        /// SET user.FirstName = $FirstName, usr.LastName = $LastName // Update or create a property.
        /// </example>
        ICypherFluent SetByConvention<T>(string variable, Func<string, bool> filter);

        /// <summary>
        /// Set all properties. This will remove any existing properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// Set<UserEntity>("u")
        /// SET u = $userEntity
        /// </example>
        ICypherFluent Set<T>(string variable);

        /// <summary>
        /// Add and update properties, while keeping existing ones.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// Set<UserEntity>("u")
        /// SET u += $userEntity
        /// </example>
        ICypherFluent SetUpdate<T>(string variable);

        /// <summary>
        /// Sets the label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        /// <example>
        /// SET n:Person
        /// </example>
        ICypherFluent SetLabel<T>(string variable, string label);
    }
}
