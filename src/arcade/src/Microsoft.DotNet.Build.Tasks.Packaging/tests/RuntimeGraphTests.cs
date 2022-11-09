// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Frameworks;
using NuGet.RuntimeModel;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class RuntimeGraphTests
    {
        [Fact]
        public void RuntimeGraphRoundTrips()
        {
            string file = $"{nameof(RuntimeGraphRoundTrips)}.json";

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            RuntimeGraph runtimeGraph = new RuntimeGraph(new[] { new RuntimeDescription("RID") });

            // Issue: https://github.com/NuGet/Home/issues/9532
            // When this is fixed, this test should fail. Fix it by deleting the NuGetUtility.WriteRuntimeGraph
            // method and replacing with JsonRuntimeFormat.WriteRuntimeGraph.
            NuGetUtility.WriteRuntimeGraph(file, runtimeGraph);

            File.Exists(file).Should().BeTrue();

            RuntimeGraph readRuntimeGraph = JsonRuntimeFormat.ReadRuntimeGraph(file);

            readRuntimeGraph.Should().NotBeNull();
            readRuntimeGraph.Should().Be(runtimeGraph);
        }
    }
}
