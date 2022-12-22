using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class UploadedFile
    {
        public UploadedFile(string name, string link)
        {
            Name = name;
            Link = link;
        }

        [JsonProperty("Name")]
        public string Name { get; }

        [JsonProperty("Link")]
        public string Link { get; }
    }
}
