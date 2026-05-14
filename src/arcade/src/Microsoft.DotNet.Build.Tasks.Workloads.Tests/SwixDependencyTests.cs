// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public class SwixDependencyTests
    {
        [WindowsOnlyTheory]
        [InlineData("1.0.0", null, "[1.0.0,)")]
        [InlineData("1.0.0", "2.0.0", "[1.0.0,2.0.0)")]
        [InlineData("1.0.0", "1.0.0", "[1.0.0]")]
        [InlineData(null, "1.2.3", "[,1.2.3)")]
        public void ItGeneratesVersionRanges(string minVersion, string maxVersion, string expectedVersionRange)
        {
            Version v1 = string.IsNullOrWhiteSpace(minVersion) ? null : new Version(minVersion);
            Version v2 = string.IsNullOrWhiteSpace(maxVersion) ? null : new Version(maxVersion);

            SwixDependency dep = new("foo", v1, v2);

            Assert.Equal(expectedVersionRange, dep.GetVersionRange());
        }
    }
}
