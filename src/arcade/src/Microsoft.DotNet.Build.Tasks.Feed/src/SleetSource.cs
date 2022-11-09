// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class SleetSource
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public string BaseUri { get; set; }
        public string Container { get; set; }
        public string ConnectionString { get; set; }
        public string FeedSubPath { get; set; }
        public string AccountName { get; internal set; }
    }
}
