// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class CustomImagePrepared
    {
        public CustomImagePrepared()
        {
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("System")]
        public string System { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }
    }
}
