using System;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class Bar
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Foo[] Friends { get; set; }
    }
}