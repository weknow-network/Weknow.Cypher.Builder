using Neo4j.Driver;

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
