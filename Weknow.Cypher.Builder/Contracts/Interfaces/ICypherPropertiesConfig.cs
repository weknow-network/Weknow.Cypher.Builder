using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// TODO: with concurrency increment

namespace Weknow
{
    /// <summary>
    /// Config setup
    /// </summary>
    /// <seealso cref="Weknow.ICypherPropertiesFactory" />
    public interface ICypherPropertiesConfig: ICypherPropertiesFactory
    {
        /// <summary>
        /// Configurations for the property build.
        /// </summary>
        /// <param name="parameterPrefix">The property's parameter prefix.
        /// if not sets, will use the configuration (default = "")</param>
        /// <param name="parameterSign">The property's parameter sign.
        /// if not sets, will use the configuration (default = "$")</param>
        /// <returns></returns>
        ICypherPropertiesFactory Config(
                    string? parameterPrefix = null,
                    string? parameterSign = null);
    }

    /// <summary>
    /// Config setup
    /// </summary>
    /// <seealso cref="Weknow.ICypherPropertiesFactory" />
    public interface ICypherPropertiesConfig<T>: ICypherPropertiesFactory<T>
    {
        /// <summary>
        /// Configurations for the property build.
        /// </summary>
        /// <param name="parameterPrefix">The property's parameter prefix.
        /// if not sets, will use the configuration (default = "")</param>
        /// <param name="parameterSign">The property's parameter sign.
        /// if not sets, will use the configuration (default = "$")</param>
        /// <returns></returns>
        ICypherPropertiesFactory<T> Config(
                    string? parameterPrefix = null,
                    string? parameterSign = null);
    }
}
