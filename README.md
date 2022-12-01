# Weknow Cypher Builder **Beta**.  
[![Build & Deploy NuGet](https://github.com/weknow-network/Weknow.Cypher.Builder/actions/workflows/Deploy.yml/badge.svg)](https://github.com/weknow-network/Weknow.Cypher.Builder/actions/workflows/Deploy.yml)


## Usage
- [**Weknow.Cypher.Builder:**](https://www.nuget.org/packages/Weknow.Cypher.Builder) Produce Cypher Query from structural C# builder.  
  `dotnet add package Weknow.Cypher.Builder`
- [**Weknow.GraphDbClient.Abstraction:**](https://www.nuget.org/packages/Weknow.GraphDbClient.Abstraction) Abstract Graph Database client.  
  `dotnet add package Weknow.GraphDbClient.Abstraction`
- [**Weknow.GraphDbClient.Redis:**](https://www.nuget.org/packages/Weknow.GraphDbClient.Redis) Client implementation for Redis Graph (Not implementes yet).  
  `dotnet add package Weknow.GraphDbClient.Redis`
- [**Weknow.GraphDbClient.Neo4j:**](https://www.nuget.org/packages/Weknow.GraphDbClient.Neo4j) Client implementation for Neo4j.  
  `dotnet add package Weknow.GraphDbClient.Neo4j`

## Goals
Cypher Builder aim to be developer friendly library for cypher query.
It bring as match intellisense & cypher correction as it can while keeping the Cypher expression readable.  

When this library evolve over time, we consider:
- Source code generation which will provide a type-safe parameters 
- Analyzer which will recommend best practice

## Samples

```cs
var n = Variables.Create();
var Id = Parameters.Create();
CypherCommand cypher = _(() => Match(N(n, Person & Manager, new { Id }))
                        .Return(n));
```
Produce: 
```cypher
MATCH (n:Person:Manager {{ Id: $Id }}) RETURN n
```

---

```cs
var items = Parameters.Create();
var n = Variables.Create();
var map = Variables.Create<Foo>();

CypherCommand cypher = _(() =>
                        Unwind(items, map,
                        Merge(N(n, Person, new { map.__.Id, map.__.Name }))
                        .OnCreateSet(n, map)
                        .Return(n)),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

```
Produce: 
```cypher
UNWIND $items AS map
MERGE (n:me1.Member.Name {{ Id: map.Id, Name: map.Name }})
  ON CREATE SET n = map
RETURN n
```

> Note: *The label `Person` become `me1.Member.Name` because of the `SCREAMING_CASE` convention*

See more on our [Documentation](wiki)


