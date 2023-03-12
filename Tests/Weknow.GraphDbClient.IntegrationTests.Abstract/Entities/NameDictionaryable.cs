using Weknow.Mapping;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
internal partial record NameDictionaryable(string Name);
