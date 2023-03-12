using Weknow.Mapping;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
internal partial record Sometime
{
    public required string Name { get; init; }
    public DateTimeOffset Birthday { get; init; }
    public DateTimeOffset Local { get; init; }
    public DateTime IssueDate { get; init; }
    public TimeSpan At { get; init; }
}
