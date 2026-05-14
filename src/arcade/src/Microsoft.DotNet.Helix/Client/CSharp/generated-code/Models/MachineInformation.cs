// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class MachineInformation
    {
        public MachineInformation(bool? isOnline)
        {
            IsOnline = isOnline;
        }

        [JsonProperty("Created")]
        public DateTimeOffset? Created { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("IsOnline")]
        public bool? IsOnline { get; }

        [JsonProperty("OperatingSystemGroup")]
        public string OperatingSystemGroup { get; set; }

        [JsonProperty("OSVersion")]
        public string OSVersion { get; set; }

        [JsonProperty("OfflineReason")]
        public string OfflineReason { get; set; }
    }
}
