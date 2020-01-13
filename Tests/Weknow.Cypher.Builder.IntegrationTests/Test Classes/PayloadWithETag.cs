using System;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.CoreIntegrationTests
{
    public class PayloadWithETag: Payload
    {
        public int eTag { get; set; }
    }
}