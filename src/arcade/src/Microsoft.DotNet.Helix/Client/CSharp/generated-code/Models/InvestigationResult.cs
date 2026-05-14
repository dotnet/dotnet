// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class InvestigationResult
    {
        public InvestigationResult()
        {
        }

        [JsonProperty("Result")]
        public IImmutableList<Models.InvestigationGroup> Result { get; set; }

        [JsonProperty("ContinuationToken")]
        public string ContinuationToken { get; set; }
    }
}
