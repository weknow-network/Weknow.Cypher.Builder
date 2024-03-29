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
It bring as match intellisense & cypher correction as it can while keeping the Cypher expression readable and well documented.  

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


## GraphDb Client

### Make your first insert

1. Define a schema

```cs
public ILabel Person => throw new NotImplementedException();
public IType Follow => throw new NotImplementedException();
```
> Yes, we believe in schema 😃

2. Create entity class/record.

```cs
[Dictionaryable(Flavor = Flavor.Neo4j)]
private partial record Person(string name, int age);
```

> Node: [`[Dictionaryable]`](https://github.com/weknow-network/Weknow.Mapping.Generation) is using [Weknow.Mapping.Generation.SrcGen](https://www.nuget.org/packages/Weknow.Mapping.Generation.SrcGen) in order to generate serialization code out of `record Person`.

3. Write a Cypher query.

```cs
var map = Parameters.Create<PersonEntity>();

CypherCommand cypher = _(user =>
                        Create(N(user, Person, map)));
````
> Produce: `CREATE (user:Person $map)`

4. To execute the query, you can use [GraphDb Client](GraphDB-Client).

```cs
CypherParameters prms = cypher.Parameters
                              .AddOrUpdate(nameof(map), new Person("Nina", 76));
await _graphDB.RunAsync(cypher, prms);
```

---

See more on our [wiki](https://github.com/weknow-network/Weknow.Cypher.Builder/wiki)


