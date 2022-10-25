#pragma warning disable CA1063 // Implement IDisposable Correctly

using Neo4j.Driver.Extensions;
using System.Xml.Linq;

namespace Weknow.GraphDbCommands
{

    public class Foo
    {
        [Neo4jProperty(Name = "Id")]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PropA { get; set; } = string.Empty;
        public string PropB { get; set; } = string.Empty;
    }

}
