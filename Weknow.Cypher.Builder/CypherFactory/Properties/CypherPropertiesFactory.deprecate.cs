// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

#pragma warning disable RCS1102 // Make class static.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using static Weknow.Helpers.Helper;
using System.Collections;
using static Weknow.CypherFactory;

namespace Weknow
{
    /// <summary>
    /// Properties factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Weknow.CypherPropertiesFactory" />
    /// <seealso cref="Weknow.ICypherPropertiesConfig{T}" />
    /// <seealso cref="Weknow.ICypherPropertiesFactory{T}" />
    internal class CypherPropertiesFactory<T> :
        CypherPropertiesFactory,
        ICypherPropertiesFactory<T>
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        internal protected CypherPropertiesFactory()
        {

        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression" /> class from being created.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="variable">The variable.</param>
        internal protected CypherPropertiesFactory(
            CypherConfig? config,
            string variable) : base(config, variable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherReturn" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        internal CypherPropertiesFactory(
            CypherPropertiesFactory copyFrom,
            string cypher)
            : base(copyFrom, cypher)
        {
        }

        #endregion // Ctor

        #region AddName

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private CypherPropertiesFactory<T> AddName(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return this;
            var prop = FormatProperty(name);
            return new CypherPropertiesFactory<T>(this, prop);
        }

        #endregion // AddName

        #region AddNames

        /// <summary>
        /// Adds the specified names.
        /// </summary>
        /// <param name="propNames">The names.</param>
        /// <returns></returns>
        private CypherPropertiesFactory<T> AddNames(IEnumerable<string> propNames)
        {
            var cur = AddName(propNames.FirstOrDefault());
            foreach (string more in propNames.Skip(1))
            {
                cur = cur.AddName(more);
            }
            return cur;
        }

        #endregion // AddNames

        #region Add

        /// <summary>
        /// Compose properties phrase from a type expression.
        /// (Add and _ are same, only matter of code styling).
        /// </summary>
        /// <param name="propExpressions">The property expressions.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Add(f => f.Name, f => f.Id)
        /// { Name: $Name, Id: $Id}
        /// ]]></example>
        ICypherPropertiesFactory<T> ICypherPropertiesFactory<T>._(params Expression<Func<T, dynamic>>[] propExpressions)
        {
            ICypherPropertiesFactory<T> self = this;
            return self.Add(propExpressions);
        }

        /// <summary>
        /// Compose properties phrase from a type expression.
        /// </summary>
        /// <param name="propExpressions">The property expressions.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Add(f => f.Name, f => f.Id)
        /// { Name: $Name, Id: $Id}
        /// ]]></example>
        ICypherPropertiesFactory<T> ICypherPropertiesFactory<T>.Add(params Expression<Func<T, dynamic>>[] propExpressions)
        {
            IEnumerable<string> names =
                                    from exp in propExpressions
                                    select ExtractLambdaExpression(exp).Name;

            return AddNames(names);
        }

        #endregion // Add

        #region AddByConvention

        /// <summary>
        /// Compose properties phrase by convention.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Add(name => name == nameof(Foo.Id) || name == nameof(Foo.Name))
        /// { Name: $Name, Id: $Id}
        /// ]]></example>
        ICypherPropertiesFactory<T> ICypherPropertiesFactory<T>.ByConvention(Func<string, bool> filter)
        {
            IEnumerable<string> names = GetProperties<T>();
            IEnumerable<string> propNames =
                            names.Where(name => filter(name));
            return AddNames(propNames);
        }

        #endregion // AddByConvention

        #region All

        /// <summary>
        /// Compose all properties of a type with optional excludes.
        /// </summary>
        /// <param name="excludes">The excludes .</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        ICypherPropertiesFactory<T> ICypherPropertiesFactory<T>.All(
            params Expression<Func<T, dynamic>>[] excludes)
        {
            IEnumerable<string> avoid = from exclude in excludes
                                        let lambda = ExtractLambdaExpression(exclude)
                                        select lambda.Name;
            var excludeMap = avoid.ToDictionary(m => m);

            ICypherPropertiesFactory<T> self = this;
            ICypherPropertiesFactory<T> properties =
                self.ByConvention(name => !excludeMap.ContainsKey(name));
            return properties;
        }

        #endregion // All
    }

    /// <summary>
    /// Properties factory
    /// </summary>
    /// <seealso cref="Weknow.CypherPropertiesFactory" />
    /// <seealso cref="Weknow.ICypherPropertiesConfig{T}" />
    /// <seealso cref="Weknow.ICypherPropertiesFactory{T}" />
    internal class CypherPropertiesFactory :
        FluentCypher,
        ICypherPropertiesFactory

    {
        private readonly string _variable;

        #region Create

        /// <summary>
        /// The create instance
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        internal static ICypherPropertiesConfig CreateProperties(
            CypherConfig? config,
            string variable)
        {
            return new CypherPropertiesFactory(config, variable);
        }

        /// <summary>
        /// The create instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static ICypherPropertiesConfig<T> CreateProperties<T>(
            CypherConfig? config,
            string variable)
        {
            return new CypherPropertiesFactory<T>(config, variable);
        }

        #endregion // Create

        #region Config

        /// <summary>
        /// Configurations for the property build.
        /// </summary>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSign">The parameter sign.</param>
        /// <returns></returns>
        ICypherPropertiesFactory ICypherPropertiesConfig.Config(
                    string? parameterPrefix,
                    string? parameterSign)
        {
            var cfg = Configuration.Clone();
            if (parameterPrefix != null)
                cfg.Naming.PropertyParameterConvention.Prefix = parameterPrefix;
            if (parameterSign != null)
                cfg.Naming.PropertyParameterConvention.Sign = parameterSign;
            return new CypherPropertiesFactory(
                                this,
                                string.Empty,
                                cfg);
        }

        #endregion // Config

        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression" /> class from being created.
        /// </summary>
        private protected CypherPropertiesFactory()
            : base(string.Empty, CypherPhrase.Property)
        {
            _variable = string.Empty;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression" /> class from being created.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="variable">The variable.</param>
        internal protected CypherPropertiesFactory(
            CypherConfig? config,
            string variable) :
            base("{ ", CypherPhrase.PropertyScope, " }", null, "", config)
        {
            _variable = variable;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherReturn" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        internal CypherPropertiesFactory(
            CypherPropertiesFactory copyFrom,
            string cypher)
            : base(
                  copyFrom?._previous, 
                  "{ ", 
                  CypherPhrase.Property,
                  " }",
                  (copyFrom?._children ?? Array.Empty<FluentCypher>())
                        .Concat(new FluentCypher(cypher, CypherPhrase.Property)),
                  childrenSeparator: ",")
        {
            _variable = copyFrom?._variable ?? string.Empty;
        }

        #endregion // Ctor

        #region Prefix

        /// <summary>
        /// Gets the property's parameter prefix.
        /// </summary>
        /// <example>
        /// { n.Id: prefixId }
        /// </example>
        public string Prefix => Configuration.Naming.PropertyParameterConvention.Prefix;

        #endregion // Prefix

        #region Sign

        /// <summary>
        /// Gets the property's parameter sign.
        /// </summary>
        /// <example>
        /// { n.Id: $Id }
        /// </example>
        public string Sign => Configuration.Naming.PropertyParameterConvention.Sign;

        #endregion // Sign

        #region FormatProperty

        /// <summary>
        /// Formats the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected string FormatProperty(string name) =>
            $"{_variable}.{name}: {Sign}{Prefix}{name}";

        #endregion // FormatProperty

        #region Add

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private CypherPropertiesFactory AddName(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return this;
            var prop = FormatProperty(name);
            if (_phrase == CypherPhrase.PropertyScope)
            { 
                return this._previous.AppendChild(prop, ",");
            }
            return new CypherPropertiesFactory(this, prop);
        }

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
        ICypherPropertiesFactory ICypherPropertiesFactory._(
            IEnumerable<string> propNames)
        {
            ICypherPropertiesFactory self = this;
            return self.Add(propNames);
        }

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
        ICypherPropertiesFactory ICypherPropertiesFactory.Add(IEnumerable<string> propNames)
        {
            var cur = AddName(propNames.FirstOrDefault());
            foreach (string more in propNames.Skip(1))
            {
                cur = cur.AddName(more);
            }
            return cur;
        }

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
        ICypherPropertiesFactory ICypherPropertiesFactory._(string name, params string[] moreNames)
        {
            ICypherPropertiesFactory self = this;
            return self.Add(name, moreNames);
        }

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
        ICypherPropertiesFactory ICypherPropertiesFactory.Add(string name, params string[] moreNames)
        {
            var cur = AddName(name);
            foreach (string more in moreNames)
            {
                cur = cur.AddName(more);
            }
            return cur;
        }

        #endregion // Add
    }
}