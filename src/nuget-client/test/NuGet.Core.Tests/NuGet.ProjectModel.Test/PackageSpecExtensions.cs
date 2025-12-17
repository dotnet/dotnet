// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackageSpecExtensionsTests
    {
        [Fact]
        public void GetTargetFramework_WithAlias_ReturnsTargetFramework()
        {
            // Arrrange
            TargetFrameworkInformation net50 = new()
            {
                FrameworkName = NuGetFramework.Parse("net5.0"),
                TargetAlias = "net5.0"
            };

            TargetFrameworkInformation net60 = new()
            {
                FrameworkName = NuGetFramework.Parse("net6.0"),
                TargetAlias = "net6.0"
            };

            TargetFrameworkInformation emptyAlias = new()
            {
                FrameworkName = NuGetFramework.Parse("net6.0"),
            };

            var projectSpec = new PackageSpec([net50, net60, emptyAlias]);

            // Act & Assert
            projectSpec.GetTargetFramework("nonexistingAlias").Should().BeNull();
            projectSpec.GetTargetFramework("net6.0").Should().Be(net60);
            projectSpec.GetTargetFramework("net5.0").Should().Be(net50);
            projectSpec.GetTargetFramework("").Should().Be(emptyAlias);
            projectSpec.GetTargetFramework(targetAlias: null).Should().BeNull();
        }

        [Fact]
        public void GetRestoreMetadataFramework_WithAlias_ReturnsTargetFramework()
        {
            // Arrange
            TargetFrameworkInformation net50tfi = new()
            {
                FrameworkName = NuGetFramework.Parse("net5.0"),
                TargetAlias = "net5.0"
            };

            TargetFrameworkInformation net60tfi = new()
            {
                FrameworkName = NuGetFramework.Parse("net6.0"),
                TargetAlias = "net6.0"
            };

            TargetFrameworkInformation emptyAliastfi = new()
            {
                FrameworkName = NuGetFramework.Parse("net6.0"),
            };

            ProjectRestoreMetadataFrameworkInfo net50 = new()
            {
                FrameworkName = NuGetFramework.Parse("net5.0"),
                TargetAlias = "net5.0"
            };
            ProjectRestoreMetadataFrameworkInfo net60 = new()
            {
                FrameworkName = NuGetFramework.Parse("net6.0"),
                TargetAlias = "net6.0"
            };
            ProjectRestoreMetadataFrameworkInfo emptyAlias = new()
            {
                FrameworkName = NuGetFramework.Parse("net6.0"),
            };
            var projectSpec = new PackageSpec([net50tfi, net60tfi, emptyAliastfi]);
            projectSpec.RestoreMetadata = new ProjectRestoreMetadata();
            projectSpec.RestoreMetadata.TargetFrameworks.Add(net50);
            projectSpec.RestoreMetadata.TargetFrameworks.Add(net60);
            projectSpec.RestoreMetadata.TargetFrameworks.Add(emptyAlias);

            // Act & Assert
            projectSpec.GetRestoreMetadataFramework("nonexistingAlias").Should().BeNull();
            projectSpec.GetRestoreMetadataFramework("net6.0").Should().Be(net60);
            projectSpec.GetRestoreMetadataFramework("net5.0").Should().Be(net50);
            projectSpec.GetRestoreMetadataFramework("").Should().Be(emptyAlias);
            projectSpec.GetRestoreMetadataFramework(targetAlias: null).Should().BeNull();
        }
    }
}
