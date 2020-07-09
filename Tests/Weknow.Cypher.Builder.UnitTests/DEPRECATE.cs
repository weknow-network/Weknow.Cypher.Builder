using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

// TODO: Duplicate class Pattern to FullNamePattern for naming standard

// TODO: parameter factory injection for enabling to work with Neo4jParameters (Neo4jMapper)
//       Mimic Neo4jMappaer WithEntity, WithEntities + integration test
//       validate flat entity (in deep complex type throw exception with recommendation for best practice)

namespace Weknow.Cypher.Builder
{
    public class DEPRECATE
    {
        // TODO: SUB QUERY, UNION, UNION ALL
        /*
         CALL {
  MATCH (p:Person)-[:FRIEND_OF]->(other:Person) RETURN p, other
  UNION
  MATCH (p:Child)-[:CHILD_OF]->(other:Parent) RETURN p, other
}
         */
        // TODO: Constraint and Index: CREATE CONSTRAINT ON (p:Payload) ASSERT p.Id IS UNIQUE

        // TODO: FOREACH, DELETE, DETACH

        // TODO: Ambient Context Label
        // TODO: Label convention

        // TODO: Auto WITH

        // TODO: AND, OR, AS,
        // TODO: variable IS NULL
        // TODO: NOT exists(n.property
        // TODO: MERGE OnCreate OnMatch
        // TODO: [x IN list | x.prop]
        // TODO: [x IN list WHERE x.prop <> $value]
        // TODO: [x IN list WHERE x.prop <> $value | x.prop]
        // TODO: reduce(s = "", x IN list | s + x.prop) // Aggregate + Reduce overloads
        // TODO: all(x IN coll WHERE exists(x.property)) // LINQ
        // TODO: any(x IN coll WHERE exists(x.property)) // LINQ
        // TODO: none(x IN coll WHERE exists(x.property))  // LINQ + overload for none
        // TODO: single(x IN coll WHERE exists(x.property)) // LINQ
        // TODO: CASE  WHEN ELSE 

        // TODO:list[$idx] AS value, // Indexer
        // TODO:list[$startIdx..$endIdx] AS slice // Indexer[range]

        // TODO: n.property STARTS WITH 'Tim' OR // string + analyzer in future
        // TODO: n.property ENDS WITH 'n' OR// string + analyzer in future
        // TODO: n.property CONTAINS 'goodie'// string + analyzer in future
        // TODO: n.property =~ 'Tim.*' 
        // TODO: n.property IN [$value1, $value2]' // LINQ Contains + overload In

        // TODO: exists(n.property),
        // TODO: coalesce, // ??
        // TODO: timestamp, id, 
        // TODO: toInteger, toFloat, toBoolean, // cast
        // TODO: head, last, tail, // LINQ first, last, skip(1), apoc -> skip (n)
        // TODO: keys, properties, 
        // TODO: count // LINQ
        // TODO: length, Size // TODO: what is the right usage
        // TODO: collect, 
        // TODO: nodes, relationships, 

        // TODO: sum, percentileDisc, stDev, abs, rand, sqrt, sign, 
        // TODO: sin, cos, tan, cot, asin, acos, atan, atan2, haversin
        // TODO: degrees, radians, pi, 
        // TODO: log10, log, exp, pi

        // TODO: toString, replace, substring, left, trim, toUpper, split, reverse

        // TODO: Spatial, 
        // TODO: date time / duration related // DateTime / TimeSpan


        // TODO: USER / ROLE MANAGEMENT and Privileges

        /* TODO:          
         CALL {
  MATCH (p:Person)-[:FRIEND_OF]->(other:Person) RETURN p, other
  UNION
  MATCH (p:Child)-[:CHILD_OF]->(other:Parent) RETURN p, other
}*/


        // todo: {name: 'Alice', age: 38,
        //       address: {city: 'London', residential: true}}



        /*
MATCH (n:INDUSTRY) RETURN n {.Label, .Id , 
                                strength:[(n)--(r:TAG_MEDIUM) | 
                                    { level: labels(r), 
                                        tags:[(r)--(t:TAG) | t.Id]}]}         
         */
    }
}

