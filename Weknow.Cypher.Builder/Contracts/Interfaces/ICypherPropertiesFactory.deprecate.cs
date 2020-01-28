using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Properties contract
    /// </summary>
    public interface ICypherPropertiesFactory
    {
        /// <summary>
        /// Compose properties phrase.
        /// (Add and _ are same, only matter of code styling).
        /// </summary>
        /// <param name="propNames">The property's names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// -----------------------------------------------
        /// Add(new ["Name", "Id"])
        /// Results in (depend on the initialized prefix & sign):
        /// { Name: $Name, Id: $Id }
        /// { Name: $prefix_Name, Id: $prefix_Id }
        /// ]]></example>
        ICypherPropertiesFactory _(IEnumerable<string> propNames);
        /// <summary>
        /// Compose properties phrase.
        /// (Add and _ are same, only matter of code styling).
        /// </summary>
        /// <param name="propNames">The property's names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// -----------------------------------------------
        /// Add(new ["Name", "Id"])
        /// Results in (depend on the initialized prefix & sign):
        /// { Name: $Name, Id: $Id }
        /// { Name: $prefix_Name, Id: $prefix_Id }
        /// ]]></example>
        ICypherPropertiesFactory Add(IEnumerable<string> propNames);
        /// <summary>
        /// Compose properties phrase.
        /// (Add and _ are same, only matter of code styling).
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <param name="moreNames">The more property's names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// -----------------------------------------------
        /// Add("Name", "Id")
        /// Results in (depend on the initialized prefix & sign):
        /// { Name: $Name, Id: $Id }
        /// { Name: $prefix_Name, Id: $prefix_Id }
        /// ]]></example>
        ICypherPropertiesFactory _(string name, params string[] moreNames);
        /// <summary>
        /// Compose properties phrase.
        /// (Add and _ are same, only matter of code styling).
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <param name="moreNames">The more property's names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// -----------------------------------------------
        /// Add("Name", "Id")
        /// Results in (depend on the initialized prefix & sign):
        /// { Name: $Name, Id: $Id }
        /// { Name: $prefix_Name, Id: $prefix_Id }
        /// ]]></example>
        ICypherPropertiesFactory Add(string name, params string[] moreNames);
    }
    /// <summary>
    /// Properties contract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICypherPropertiesFactory<T>: ICypherPropertiesFactory
    {
        /// <summary>
        /// Compose properties phrase from a type expression.
        /// (Add and _ are same, only matter of code styling).
        /// </summary>
        /// <param name="propExpressions">The property expressions.</param>
        /// <example><![CDATA[
        /// Add(f => f.Name, f => f.Id)
        /// { Name: $Name, Id: $Id}
        /// ]]></example>
        /// <returns></returns>
        ICypherPropertiesFactory<T> _(
            params Expression<Func<T, dynamic>>[] propExpressions);
        /// <summary>
        /// Compose properties phrase from a type expression.
        /// </summary>
        /// <param name="propExpressions">The property expressions.</param>
        /// <example><![CDATA[
        /// Add(f => f.Name, f => f.Id)
        /// { Name: $Name, Id: $Id}
        /// ]]></example>
        /// <returns></returns>
        ICypherPropertiesFactory<T> Add(
            params Expression<Func<T, dynamic>>[] propExpressions);

        /// <summary>
        /// Compose properties phrase by reflection with exclude option.
        /// </summary>
        /// <param name="excludes">The excludes.</param>
        /// <returns></returns>
        ICypherPropertiesFactory<T> All(params Expression<Func<T, dynamic>>[] excludes);

        /// <summary>
        /// Compose properties phrase by convention.
        /// (ByConvention and Conv are same, only matter of code styling).
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Add(name => name == nameof(Foo.Id) || name == nameof(Foo.Name))
        /// { Name: $Name, Id: $Id}
        /// ]]></example>
        ICypherPropertiesFactory<T> ByConvention(
                                        Func<string, bool> filter);
    }
}
