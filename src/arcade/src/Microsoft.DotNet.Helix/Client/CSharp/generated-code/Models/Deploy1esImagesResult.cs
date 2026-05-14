// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
