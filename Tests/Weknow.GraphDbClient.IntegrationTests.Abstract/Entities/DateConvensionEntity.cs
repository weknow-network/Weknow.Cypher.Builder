using System.Text.Json.Serialization;

using Weknow.Mapping;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
internal partial record DateConvensionEntity
{
    public required string Id { get; init; }
    [JsonPropertyName("creation-date")]
    public required DateTimeOffset CreatedAt { get; init; }
    [JsonPropertyName("modification-date")]
    public DateTimeOffset? ModifiedAt { get; init; }
}
