// https://neo4j.com/docs/cypher-refcard/current/

using Generator.Equals;

using Weknow.Mapping;

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable]
[Equatable]
internal partial record Name1(string Name);
