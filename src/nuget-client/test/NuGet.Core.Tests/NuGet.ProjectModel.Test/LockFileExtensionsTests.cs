// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileExtensionsTests
    {
        [Fact]
        public void GivenALogMessageVerifyTargetGraphIsReturned()
        {
            var assetsFile = new LockFile();
            assetsFile.Targets.Add(new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net45"),
            });

            assetsFile.Targets.Single().Libraries.Add(new LockFileTargetLibrary()
            {
                Name = "x",
                Version = NuGetVersion.Parse("1.0.0")
            });

            var expected = NuGetFramework.Parse("net45").DotNetFrameworkName;
            var message = new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1103, "test", expected);

            var graphs = message.GetTargetGraphs(assetsFile);

            graphs.Select(e => e.Name).Should().BeEquivalentTo(new[] { expected });
        }

        [Fact]
        public void GivenALogMessageWithMultipleGraphsVerifyTargetGraphsAreReturned()
        {
            var assetsFile = new LockFile();
            assetsFile.Targets.Add(new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net45"),
            });

            assetsFile.Targets.Add(new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net46"),
                RuntimeIdentifier = "win8"
            });

            var expected1 = NuGetFramework.Parse("net45").DotNetFrameworkName;
            var expected2 = NuGetFramework.Parse("net46").DotNetFrameworkName + "/win8";
            var message = new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1103, "test")
            {
                TargetGraphs = new[] { expected1, expected2 }
            };

            var graphs = message.GetTargetGraphs(assetsFile);

            graphs.Select(e => e.Name).Should().BeEquivalentTo(new[] { expected1, expected2 });
        }

        [Fact]
        public void GivenALogMessageWithNoTargetGraphsVerifyAllGraphsAreReturned()
        {
            var assetsFile = new LockFile();
            assetsFile.Targets.Add(new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net45"),
            });

            assetsFile.Targets.Add(new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net46"),
                RuntimeIdentifier = "win8"
            });

            assetsFile.Targets.Add(new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("netstandard2.0")
            });

            var expected1 = NuGetFramework.Parse("net45").DotNetFrameworkName;
            var expected2 = NuGetFramework.Parse("net46").DotNetFrameworkName + "/win8";
            var expected3 = NuGetFramework.Parse("netstandard2.0").DotNetFrameworkName;

            // Create a message with no target graphs
            var message = new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1103, "test");

            var graphs = message.GetTargetGraphs(assetsFile);

            graphs.Select(e => e.Name).Should().BeEquivalentTo(new[] { expected1, expected2, expected3 });
        }

        [Fact]
        public void GivenATargetGraphVerifyLibraryReturned()
        {
            var graph = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net45"),
            };

            graph.Libraries.Add(new LockFileTargetLibrary()
            {
                Name = "x",
                Version = NuGetVersion.Parse("1.0.0")
            });

            graph.GetTargetLibrary("x").Version.ToNormalizedString().Should().Be("1.0.0");
        }

        [Fact]
        public void GivenALogMessageVerifyTargetGraphLibraryIsReturned()
        {
            var assetsFile = new LockFile();
            assetsFile.Targets.Add(new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net45"),
            });

            assetsFile.Targets.Single().Libraries.Add(new LockFileTargetLibrary()
            {
                Name = "x",
                Version = NuGetVersion.Parse("1.0.0")
            });

            var expected = NuGetFramework.Parse("net45").DotNetFrameworkName;
            var message = new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1103, "test", expected);

            var graphs = message.GetTargetGraphs(assetsFile);

            graphs.SelectMany(e => e.Libraries).Select(e => e.Name).Should().BeEquivalentTo(new[] { "x" });
        }
    }
}
