using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class Deploy1esImagesResult
    {
        public Deploy1esImagesResult()
        {
        }

        [JsonProperty("Images")]
        public IImmutableList<Models.Deployed1esImage> Images { get; set; }

        [JsonProperty("ResourceGroupName")]
        public string ResourceGroupName { get; set; }
    }
}
