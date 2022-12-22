// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    internal sealed class OptProfTrainingTest
    {
        [JsonProperty(PropertyName = "container", Order = 3)]
        public string Container { get; set; }

        [JsonProperty(PropertyName = "testCases", Order = 3)]
        public string[] TestCases { get; set; }

        [JsonProperty(PropertyName = "filteredTestCases", Order = 3)]
        public OptProfFileFilteredTest[] FilteredTestCases { get; set; }
    }
}
