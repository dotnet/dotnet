// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.DotNet.GitSync
{
    public class PullRequestInfo
    {
        public int Number { get; set; }
    }

    public class RepositoryInfo
    {
        [JsonIgnore]
        public string CloneUrl => Url.ToString().EndsWith(".git") ? Url.ToString() : Url + ".git";
        public Configuration Configuration { get; set; }
        public Dictionary<string, string> LastSynchronizedCommits { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        [JsonIgnore]
        public string Path => System.IO.Path.Combine(Configuration.RepositoryBasePath, Name);
        public Dictionary<string, PullRequestInfo> PendingPRs { get; set; }
        public string SharedPath { get; set; }
        public string UpstreamOwner { get; set; }
        [JsonIgnore]
        public Uri Url => new Uri($"https://github.com/{Owner}/{Name}");
        public override string ToString()
        {
            return $"{Owner}/{Name}";
        }
    }
}
