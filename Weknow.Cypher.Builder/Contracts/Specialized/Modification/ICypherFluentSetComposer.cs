//// https://neo4j.com/docs/cypher-refcard/current/
//// https://github.com/Readify/Neo4jClient
//// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
//// https://neo4jmapper.tk/guide.html
//// https://github.com/barnardos-au/Neo4jMapper

//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

//namespace Weknow
//{

//    public interface ICypherFluentSetComposer: ICypherable
//    {

//        /// <summary>
//        /// Compose SET phrase
//        /// </summary>
//        /// <param name="variable">The variable.</param>
//        /// <param name="propNames">The property names.</param>
//        /// <returns></returns>
//        /// <example>
//        /// Set("n", new [] { nameof(Foo.Name), nameof(Bar.Id)})
//        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
//        /// </example>
//        string Set(string variable, IEnumerable<string> propNames);

//        /// <summary>
//        /// Compose SET phrase
//        /// </summary>
//        /// <param name="variable">The variable.</param>
//        /// <param name="propNames">The property names.</param>
//        /// <returns></returns>
//        /// <example>
//        /// Set("n", nameof(Foo.Name), nameof(Bar.Id))
//        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
//        /// </example>
//        string Set(string variable, params string[] propNames);

//        /// <summary>
//        /// Compose SET phrase by convention.
//        /// </summary>
//        /// <param name="filter">The filter.</param>
//        /// <returns></returns>
//        /// <example>
//        /// Set((User user) => user.Name.StartWith("Name"))
//        /// SET user.FirstName = $FirstName, usr.LastName = $LastName // Update or create a property.
//        /// </example>
//        string SetByConvention(Func<string, bool> filter);
//    }
//}
