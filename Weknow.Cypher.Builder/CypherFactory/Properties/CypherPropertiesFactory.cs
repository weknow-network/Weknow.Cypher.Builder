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
        ICypherPropertiesConfig<T>
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
            string variable): base(config, variable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherReturn" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="config">The configuration.</param>
        internal CypherPropertiesFactory(
            CypherPropertiesFactory copyFrom,
            string cypher,
            CypherConfig? config = null)
            : base(copyFrom, cypher, config)
        {
        }

        #endregion // Ctor

        #region Config

        /// <summary>
        /// Configurations for the property build.
        /// </summary>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSign">The parameter sign.</param>
        /// <returns></returns>
        ICypherPropertiesFactory<T> ICypherPropertiesConfig<T>.Config(
                    string? parameterPrefix,
                    string? parameterSign)
        {
            var cfg = this._config.Clone();
            if(parameterPrefix != null)
                cfg.Naming.PropertyParameterConvention.Prefix = parameterPrefix;
            if(parameterSign != null)
                cfg.Naming.PropertyParameterConvention.Sign = parameterSign;
            return new CypherPropertiesFactory<T>(
                                this,
                                string.Empty,
                                cfg);
        }

        #endregion // Config

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
        ICypherPropertiesFactory<T> ICypherPropertiesFactory<T>.AddByConvention(Func<string, bool> filter)
        {
            IEnumerable<string> names = GetProperties<T>();
            IEnumerable<string> propNames =
                            names.Where(name => filter(name));
            return AddNames(propNames);
        }

        #endregion // AddByConvention

        #region AddAll

        /// <summary>
        /// Compose all properties of a type with optional excludes.
        /// </summary>
        /// <param name="excludes">The excludes .</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        ICypherPropertiesFactory<T> ICypherPropertiesFactory<T>.AddAll(
            params Expression<Func<T, dynamic>>[] excludes)
        {
            IEnumerable<string> avoid = from exclude in excludes
                                        let lambda = ExtractLambdaExpression(exclude)
                                        select lambda.Name;
            var excludeMap = avoid.ToDictionary(m => m);

            ICypherPropertiesFactory<T> self = this;
            ICypherPropertiesFactory<T> properties =
                self.AddByConvention(name => !excludeMap.ContainsKey(name));
            return properties;
        }

        #endregion // AddAll
    }

    /// <summary>
    /// Properties factory
    /// </summary>
    /// <seealso cref="Weknow.CypherPropertiesFactory" />
    /// <seealso cref="Weknow.ICypherPropertiesConfig{T}" />
    /// <seealso cref="Weknow.ICypherPropertiesFactory{T}" />
    internal class CypherPropertiesFactory :
        FluentCypher,
        ICypherPropertiesConfig

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
            CypherConfig config,
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
            CypherConfig config,
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
            var cfg = this._config.Clone();
            if(parameterPrefix != null)
                cfg.Naming.PropertyParameterConvention.Prefix = parameterPrefix;
            if(parameterSign != null)
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
        /// <param name="config">The configuration.</param>
        internal CypherPropertiesFactory(
            CypherPropertiesFactory copyFrom,
            string cypher,
            CypherConfig? config = null)
            : base(copyFrom, cypher, CypherPhrase.Property, childrenSeparator: ",", config: config ?? copyFrom._config)
        {
            _variable = copyFrom._variable;
        }

        #endregion // Ctor

        #region Prefix

        /// <summary>
        /// Gets the property's parameter prefix.
        /// </summary>
        /// <example>
        /// { n.Id: prefixId }
        /// </example>
        public string Prefix => _config.Naming.PropertyParameterConvention.Prefix;

        #endregion // Prefix

        #region Sign

        /// <summary>
        /// Gets the property's parameter sign.
        /// </summary>
        /// <example>
        /// { n.Id: $Id }
        /// </example>
        public string Sign => _config.Naming.PropertyParameterConvention.Sign;

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
            return new CypherPropertiesFactory(this, prop);
        }

        /// <summary>
        /// Compose properties phrase.
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