using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    public class Cmlx
    {
        public int Id { get; set; }
        public string[] Names { get; set; } = Array.Empty<string>();
    }

}
