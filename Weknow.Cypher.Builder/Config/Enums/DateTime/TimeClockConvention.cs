namespace Weknow.CypherBuilder;

/// <summary>
/// Controlling when the time is sampled (for cypher date/time function)
/// </summary>
public enum TimeClockConvention
{
    /// <summary>
    /// The default clock (Neo4j is using 'statement' as the default)
    /// </summary>
    /// <example>
    /// DateTimeOffset.Today become date() 
    /// if the TimeConvention is AsFunction
    /// 
    /// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
    /// </example>
    Default,
    /// <summary>
    ///  The instant produced will be the live clock of the system.
    /// 
    /// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
    /// </summary>
    /// <example>
    /// DateTimeOffset.Today become date.realtime() 
    /// if the TimeConvention is AsFunction
    /// 
    /// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
    /// </example>
    Realtime,
    /// <summary>
    /// The same instant is produced 
    /// for each invocation within the same statement. 
    /// A different time may be produced 
    /// for different statements within the same transaction.
    /// 
    /// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
    /// </summary>
    /// <example>
    /// DateTimeOffset.Today become date.statement() 
    /// if the TimeConvention is AsFunction
    /// 
    /// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
    /// </example>
    Statement,
    /// <summary>
    /// The same instant is produced for each invocation 
    /// within the same transaction. A different time may be produced for 
    /// different transactions.
    /// </summary>
    /// <example>
    /// DateTimeOffset.Today become date.transaction() 
    /// if the TimeConvention is AsFunction
    /// 
    /// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
    /// </example>
    Transaction,
}
