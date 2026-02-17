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

        [Fact]
        public void GetNearestTargetFramework_WithMultipleFrameworks_ReturnsNearestCompatible()
        {
            // Arrange
            TargetFrameworkInformation net50 = new()
            {
                FrameworkName = NuGetFramework.Parse("net5.0"),
                TargetAlias = "net5.0"
            };

            TargetFrameworkInformation net70 = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "net7.0"
            };

            TargetFrameworkInformation net90 = new()
            {
                FrameworkName = NuGetFramework.Parse("net9.0"),
                TargetAlias = "net9.0"
            };

            var projectSpec = new PackageSpec([net50, net70, net90]);

            // Act & Assert - Exact matches
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net5.0"), string.Empty).Should().Be(net50);
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net7.0"), string.Empty).Should().Be(net70);
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net9.0"), string.Empty).Should().Be(net90);

            // Act & Assert - Nearest compatible (net6.0 should match net5.0 as nearest)
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net6.0"), string.Empty).Should().Be(net50);

            // Act & Assert - Nearest compatible (net8.0 should match net7.0 as nearest)
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net8.0"), string.Empty).Should().Be(net70);

            // Act & Assert - Incompatible framework (netcoreapp3.1) should return empty/dummy
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("netcoreapp3.1"), string.Empty).FrameworkName.Should().BeNull();

            // Act & Assert - Incompatible framework (net472) should return empty/dummy
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net472"), string.Empty).FrameworkName.Should().BeNull();
        }

        [Fact]
        public void GetNearestTargetFramework_WithIncompatibleFrameworkButMatchingAlias_ReturnsEmpty()
        {
            // Arrange - Project only has .NET Core frameworks
            TargetFrameworkInformation net50 = new()
            {
                FrameworkName = NuGetFramework.Parse("net5.0"),
                TargetAlias = "net5.0"
            };

            TargetFrameworkInformation net70 = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "net7.0"
            };

            var projectSpec = new PackageSpec([net50, net70]);

            // Act & Assert - Try to find nearest for .NET Framework which is incompatible, but with an alias that matches
            // Should return empty since net472 is incompatible with net5.0 and net7.0
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net472"), "net7.0").FrameworkName.Should().BeNull();
        }

        [Fact]
        public void GetNearestTargetFramework_WithDuplicateFrameworks_WhenSomethingMatches_ReturnsMatch()
        {
            // Arrange - Project has duplicate net7.0 frameworks with different aliases
            TargetFrameworkInformation net70Apple = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "apple"
            };

            TargetFrameworkInformation net70Banana = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "banana"
            };

            var projectSpec = new PackageSpec([net70Apple, net70Banana]);

            // Act & Assert - Request with matching alias
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net7.0"), "apple").Should().Be(net70Apple);
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net7.0"), "banana").Should().Be(net70Banana);
        }

        [Fact]
        public void GetNearestTargetFramework_WithDuplicateFrameworks_WhenNothingMatches_ReturnsEmpty()
        {
            // Arrange - Project has duplicate net7.0 frameworks with different aliases
            TargetFrameworkInformation net70Apple = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "apple"
            };

            TargetFrameworkInformation net70Banana = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "banana"
            };

            var projectSpec = new PackageSpec([net70Apple, net70Banana]);

            // Act & Assert - Request with non-matching alias, should return empty since alias doesn't disambiguate
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net7.0"), "orange").FrameworkName.Should().BeNull();
        }

        [Fact]
        public void GetNearestTargetFramework_WithDuplicateFrameworks_DisambiguationByAlias()
        {
            // Arrange - Complex scenario with multiple frameworks including duplicates
            TargetFrameworkInformation net50 = new()
            {
                FrameworkName = NuGetFramework.Parse("net5.0"),
                TargetAlias = "net5.0"
            };

            TargetFrameworkInformation net70Apple = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "apple"
            };

            TargetFrameworkInformation net70Banana = new()
            {
                FrameworkName = NuGetFramework.Parse("net7.0"),
                TargetAlias = "banana"
            };

            TargetFrameworkInformation net90 = new()
            {
                FrameworkName = NuGetFramework.Parse("net9.0"),
                TargetAlias = "net9.0"
            };

            var projectSpec = new PackageSpec([net50, net70Apple, net70Banana, net90]);

            // Act & Assert - Exact match with single framework (no ambiguity)
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net5.0"), "net5.0").Should().Be(net50);

            // Act & Assert - Exact match with duplicate frameworks, disambiguated by alias
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net7.0"), "apple").Should().Be(net70Apple);
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net7.0"), "banana").Should().Be(net70Banana);

            // Act & Assert - Nearest match (net6.0) should find net5.0
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net6.0"), "net6.0").Should().Be(net50);

            // Act & Assert - Nearest match (net8.0) with matching alias should disambiguate
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net8.0"), "apple").Should().Be(net70Apple);
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net8.0"), "banana").Should().Be(net70Banana);

            // Act & Assert - Exact match with single framework (no ambiguity)
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net9.0"), "net9.0").Should().Be(net90);

            // Act & Assert - Nearest match (net8.0) should find net9.0 (single, no ambiguity)
            projectSpec.GetNearestTargetFramework(NuGetFramework.Parse("net8.0"), "net8.0").FrameworkName.Should().BeNull();
        }
    }
}
