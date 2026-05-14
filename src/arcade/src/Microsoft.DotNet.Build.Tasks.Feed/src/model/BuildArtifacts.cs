// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class Properties
    {
        public string artifactsize { get; set; }
        public string RootId { get; set; }
        public string localpath { get; set; }
    }

    public class Resource
    {
        public string type { get; set; }
        public string data { get; set; }
        public Properties properties { get; set; }
        public string url { get; set; }
        public string downloadUrl { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public Resource resource { get; set; }
    }

    public class BuildArtifacts
    {
        public int count { get; set; }
        public IList<Value> value { get; set; }
    }
}
