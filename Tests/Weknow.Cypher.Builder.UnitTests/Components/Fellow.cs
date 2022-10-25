#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands
{

    public class Fellow
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int eTag { get; set; }
    }

}
