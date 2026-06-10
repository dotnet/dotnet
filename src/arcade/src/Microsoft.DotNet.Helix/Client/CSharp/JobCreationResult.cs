// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobCreationResult
    {
        [JsonProperty("queueStats")]
        public QueueStats QueueStats { get; set; }
    }
}
