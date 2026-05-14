// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class QueueInfo
    {
        public QueueInfo()
        {
        }

        [JsonProperty("Artifacts")]
        public IImmutableList<Models.Artifact> Artifacts { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("GalleryImage")]
        public Models.ImageInfo GalleryImage { get; set; }

        [JsonProperty("Purpose")]
        public string Purpose { get; set; }

        [JsonProperty("Architecture")]
        public string Architecture { get; set; }

        [JsonProperty("IsAvailable")]
        public bool? IsAvailable { get; set; }

        [JsonProperty("IsInternalOnly")]
        public bool? IsInternalOnly { get; set; }

        [JsonProperty("IsOnPremises")]
        public bool? IsOnPremises { get; set; }

        [JsonProperty("OperatingSystemGroup")]
        public string OperatingSystemGroup { get; set; }

        [JsonProperty("PreInstalledImage")]
        public Models.CustomImagePreInstalled PreInstalledImage { get; set; }

        [JsonProperty("PreparedImage")]
        public Models.CustomImagePrepared PreparedImage { get; set; }

        [JsonProperty("QueueId")]
        public string QueueId { get; set; }

        [JsonProperty("QueueDepth")]
        public long? QueueDepth { get; set; }

        [JsonProperty("ScaleMin")]
        public int? ScaleMin { get; set; }

        [JsonProperty("ScaleMax")]
        public int? ScaleMax { get; set; }

        [JsonProperty("UserList")]
        public string UserList { get; set; }

        [JsonProperty("WorkspacePath")]
        public string WorkspacePath { get; set; }

        [JsonProperty("EstimatedRemovalDate")]
        public string EstimatedRemovalDate { get; set; }
    }
}
