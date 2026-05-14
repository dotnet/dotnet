// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class Deployed1esImage
    {
        public Deployed1esImage()
        {
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("IsPublic")]
        public bool? IsPublic { get; set; }

        [JsonProperty("Artifacts")]
        public IImmutableList<Models.Artifact> Artifacts { get; set; }

        [JsonProperty("Purpose")]
        public string Purpose { get; set; }

        [JsonProperty("OsGroup")]
        public string OsGroup { get; set; }

        [JsonProperty("Image")]
        public Models.ImageInfo Image { get; set; }

        [JsonProperty("PreInstalledImage")]
        public Models.CustomImagePreInstalled PreInstalledImage { get; set; }

        [JsonProperty("CustomImageName")]
        public Models.CustomImagePrepared CustomImageName { get; set; }
    }
}
