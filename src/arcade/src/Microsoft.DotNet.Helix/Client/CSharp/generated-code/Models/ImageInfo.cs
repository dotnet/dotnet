// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class ImageInfo
    {
        public ImageInfo()
        {
        }

        [JsonProperty("Publisher")]
        public string Publisher { get; set; }

        [JsonProperty("Offer")]
        public string Offer { get; set; }

        [JsonProperty("Sku")]
        public string Sku { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }
    }
}
