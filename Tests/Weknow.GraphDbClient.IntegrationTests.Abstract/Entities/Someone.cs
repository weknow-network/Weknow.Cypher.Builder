using Generator.Equals;

using Weknow.Mapping;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable(Flavor = Mapping.Flavor.Neo4j)]
[Equatable]
internal partial record Someone(int Id, string Name, int Age);
