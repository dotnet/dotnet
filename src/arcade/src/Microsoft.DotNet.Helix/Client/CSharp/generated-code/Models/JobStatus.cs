// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobStatus
    {
        [JsonProperty("JobName")]
        public string JobName { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }
}
