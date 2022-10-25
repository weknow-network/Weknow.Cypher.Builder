using Neo4j.Driver;

using Weknow.GraphDbCommands;
using Weknow.GraphDbClient.Abstraction;
using Neo4j.Driver.Extensions;
using System.Reflection;
using Weknow.Mapping;
using EnsureThat;

namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal static class N4jGraphDBExtensions
{
    #region TryGetValue

    /// <summary>
    /// Tries to get value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record">The record.</param>
    /// <param name="identifier">The identifier.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool TryGetValue<T>(this IRecord record, string identifier, out T value)
    {
        Param<IRecord> param = Ensure.That(record);
        EnsureThatAnyExtensions.IsNotNull(in param);
        StringParam param2 = Ensure.That(identifier);
        param2.IsNotEmptyOrWhiteSpace();
        if (!record.Keys.Contains(identifier))
        {
            value = default;
            return false;
        }

        value = record.Values[identifier].As<T>();
        return true;
    }

    /// <summary>
    /// Tries to get the first record set's value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record">The record.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool TryGetValue<T>(this IRecord record, out T value)
    {
        Param<IRecord> param = Ensure.That(record);
        EnsureThatAnyExtensions.IsNotNull(in param);
        if (record.Values.Count == 0)
        {
            value = default;
            return false;
        }

        value = record.Values.First().Value.As<T>();
        return true;
    }

    #endregion // TryGetValue
}
