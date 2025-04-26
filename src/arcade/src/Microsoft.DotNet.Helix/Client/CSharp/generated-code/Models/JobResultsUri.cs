// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobResultsUri
    {
        public JobResultsUri()
        {
        }

        [JsonProperty("ResultsUri")]
        public string ResultsUri { get; set; }

        [JsonProperty("ResultsUriRSAS")]
        public string ResultsUriRSAS { get; set; }
    }
}
