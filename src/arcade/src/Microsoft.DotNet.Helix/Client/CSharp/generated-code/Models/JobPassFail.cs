// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobPassFail
    {
        public JobPassFail(int total, int working, IImmutableList<string> failed)
        {
            Total = total;
            Working = working;
            Failed = failed;
        }

        [JsonProperty("Total")]
        public int Total { get; set; }

        [JsonProperty("Working")]
        public int Working { get; set; }

        [JsonProperty("Failed")]
        public IImmutableList<string> Failed { get; set; }

        [JsonProperty("Passed")]
        public IImmutableList<string> Passed { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (Failed == default(IImmutableList<string>))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
