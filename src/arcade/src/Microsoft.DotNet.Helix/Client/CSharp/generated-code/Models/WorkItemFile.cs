// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class WorkItemFile
    {
        public WorkItemFile(string fileName, string uri)
        {
            FileName = fileName;
            Uri = uri;
        }

        [JsonProperty("FileName")]
        public string FileName { get; set; }

        [JsonProperty("Uri")]
        public string Uri { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Uri))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
