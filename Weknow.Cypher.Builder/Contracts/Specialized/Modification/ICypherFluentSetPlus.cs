// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Weknow
{

    public interface ICypherFluentSetPlus : IFluentCypher, ICypherFluentSet
    {
    }
}
