// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class RestoreTargetGraphTests
    {
        [Fact]
        public void GetTargetGraphName_WithNullTargetAlias_ThrowsArgumentNullException()
        {
            // Arrange
            string? targetAlias = null;
            string runtimeIdentifier = "win-x64";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RestoreTargetGraph.GetTargetGraphName(targetAlias, runtimeIdentifier));
        }

        [Theory]
        [InlineData("net472", null, "net472")]
        [InlineData("net472", "", "net472")]
        [InlineData("net472", "win-x64", "net472/win-x64")]
        [InlineData("net5.0-windows", null, "net5.0-windows")]
        [InlineData("net5.0-windows", "win-x64", "net5.0-windows/win-x64")]
        [InlineData("netstandard2.0", null, "netstandard2.0")]
        [InlineData("netstandard2.0", "linux-x64", "netstandard2.0/linux-x64")]
        [InlineData("my-custom-alias", null, "my-custom-alias")]
        [InlineData("my-custom-alias", "osx-x64", "my-custom-alias/osx-x64")]
        public void GetTargetGraphName_VariousInputs_ReturnsExpectedFormat(string targetAlias, string runtimeIdentifier, string expectedResult)
        {
            // Act
            string result = RestoreTargetGraph.GetTargetGraphName(targetAlias, runtimeIdentifier);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TargetGraphName_WithEmptyTargetAlias_UsesFrameworkRuntimePair()
        {
            // Arrange
            var framework = NuGetFramework.Parse("netcoreapp3.0");
            string? targetAlias = null;
            var context = CreateRemoteWalkContext();
            var logger = new TestLogger();

            // Create a minimal RestoreTargetGraph with empty target alias
            var graph = RestoreTargetGraph.Create(
                graphs: [],
                context: context,
                logger: logger,
                targetAlias: targetAlias,
                framework: framework);

            // Act
            string targetGraphName = graph.TargetGraphName;

            // Assert
            // When targetAlias is null/empty, it uses FrameworkRuntimePair.GetTargetGraphName
            Assert.Equal(".NETCoreApp,Version=v3.0", targetGraphName);
        }

        [Fact]
        public void TargetGraphName_WithEmptyTargetAliasAndRuntime_UsesFrameworkRuntimePair()
        {
            // Arrange
            var framework = NuGetFramework.Parse("netcoreapp3.0");
            string runtimeIdentifier = "win-x64";
            string? targetAlias = null;
            var context = CreateRemoteWalkContext();
            var logger = new TestLogger();

            // Create a minimal RestoreTargetGraph with empty target alias and runtime
            var graph = RestoreTargetGraph.Create(
                runtimeGraph: RuntimeModel.RuntimeGraph.Empty,
                graphs: [],
                context: context,
                log: logger,
                targetAlias: targetAlias,
                framework: framework,
                runtimeIdentifier: runtimeIdentifier);

            // Act
            string targetGraphName = graph.TargetGraphName;

            // Assert
            // When targetAlias is null/empty, it uses FrameworkRuntimePair.GetTargetGraphName with runtime
            Assert.Equal(".NETCoreApp,Version=v3.0/win-x64", targetGraphName);
        }

        [Fact]
        public void TargetGraphName_WithTargetAlias_UsesGetTargetGraphName()
        {
            // Arrange
            var framework = NuGetFramework.Parse("net5.0");
            string runtimeIdentifier = "win-x64";
            string targetAlias = "net5.0";
            var context = CreateRemoteWalkContext();
            var logger = new TestLogger();

            // Create a minimal RestoreTargetGraph with target alias
            var graph = RestoreTargetGraph.Create(
                runtimeGraph: RuntimeModel.RuntimeGraph.Empty,
                graphs: [],
                context: context,
                log: logger,
                targetAlias: targetAlias,
                framework: framework,
                runtimeIdentifier: runtimeIdentifier);

            // Act
            string targetGraphName = graph.TargetGraphName;

            // Assert
            Assert.Equal("net5.0/win-x64", targetGraphName);
        }

        [Fact]
        public void TargetGraphName_WithTargetAliasNoRuntime_ReturnsTargetAlias()
        {
            // Arrange
            var framework = NuGetFramework.Parse("net5.0");
            string? runtimeIdentifier = null;
            string targetAlias = "net5.0";
            var context = CreateRemoteWalkContext();
            var logger = new TestLogger();

            // Create a minimal RestoreTargetGraph
            var graph = RestoreTargetGraph.Create(
                runtimeGraph: RuntimeModel.RuntimeGraph.Empty,
                graphs: [],
                context: context,
                log: logger,
                targetAlias: targetAlias,
                framework: framework,
                runtimeIdentifier: runtimeIdentifier);

            // Act
            string targetGraphName = graph.TargetGraphName;

            // Assert
            Assert.Equal("net5.0", targetGraphName);
        }

        private static RemoteWalkContext CreateRemoteWalkContext()
        {
            return new RemoteWalkContext(
                new SourceCacheContext(),
                new PackageSourceMapping(new Dictionary<string, IReadOnlyList<string>>()),
                new TestLogger());
        }
    }
}
