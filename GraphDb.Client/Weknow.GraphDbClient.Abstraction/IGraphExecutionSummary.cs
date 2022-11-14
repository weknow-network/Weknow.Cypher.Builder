// TODO: [bnaya 2022-10-23] get statistic back (check if it possible on Neo4J and abstract it for both REDIS & neo4j)
// TODO: [bnaya 2022-10-23] use lambda to attach relationships 

namespace Weknow.GraphDbClient.Abstraction;

/// <summary>
/// The result summary of running a query. The result summary interface can be used 
/// to investigate details about the result, like the type of query run, how many
/// and which kinds of updates have been executed, and query plan and profiling information
//  if available. The result summary is only available after all result records have
//  been consumed. Keeping the result summary around does not influence the lifecycle
//  of any associated session and/or transaction.
/// </summary>
public interface IGraphExecutionSummary
{
    //
    // Summary:
    //     Gets query that has been executed.
    string Query { get; }

    /// <summary>
    /// The time it took the server to make the result available for consumption.
    /// Default to -00:00:00.001 if the server version does not support this field in summary.
    /// </summary>
    TimeSpan ResultAvailableAfter { get; }
}
