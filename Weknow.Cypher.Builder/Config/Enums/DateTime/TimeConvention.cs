namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher formatting options
/// </summary>
public enum TimeConvention
{
    /// <summary>
    /// Convert date-time expression like DateTime.Now into Cypher func like datetime()
    /// </summary>
    /// <example>
    /// DateTime.Today or DateTimeOffset.Today become date()
    /// DateTime.Now/UtcNow or DateTimeOffset.Now/UtcNow become datetime()
    /// DateTime.Now.TimeOfDay or DateTimeOffset.TimeOfDay become time()    
    /// 
    /// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
    /// </example>
    AsFunction,
    /// <summary>
    /// Date/Time expression will mapped into parameters (with default of the time when it construct)
    /// </summary>
    AsParameter,
}
