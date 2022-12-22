using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Sdk
{
    public static class Constants
    {
        public static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            Formatting = Formatting.Indented,
        };
    }
}
