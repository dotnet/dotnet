// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class PackageSourceMappingSourceItemTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_WithInvalidKey_Throws(string key)
        {
            Assert.Throws<ArgumentException>(() => new PackageSourceMappingSourceItem(key, new List<PackagePatternItem>() { new PackagePatternItem("stuff") }));
        }

        [Fact]
        public void Constructor_WithEmptyPackagePatterns_Throws()
        {
            Assert.Throws<ArgumentException>(() => new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { }));
        }

        [Fact]
        public void Constructor_WithNullPackagePatterns_Throws()
        {
            Assert.Throws<ArgumentException>(() => new PackageSourceMappingSourceItem("name", null));
        }

        [Fact]
        public void Equals_WithEquivalentName_ReturnsTrue()
        {
            var left = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            left.Equals(right).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentlyCasedName_ReturnsTrue()
        {
            var left = new PackageSourceMappingSourceItem("NAME", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            left.Equals(right).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentNames_ReturnsFalse()
        {
            var left = new PackageSourceMappingSourceItem("name2", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            left.Equals(right).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithDifferentPackagePatterns_ReturnsTrue()
        {
            var left = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff3") });
            left.Equals(right).Should().BeTrue();
        }

        [Fact]
        public void HashCode_WithEquivalentPackagePatterns_ReturnsTrue()
        {
            var left = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            left.GetHashCode().Equals(right.GetHashCode()).Should().BeTrue();
        }

        [Fact]
        public void HashCode_WithDifferentlyCasedName_ReturnsTrue()
        {
            var left = new PackageSourceMappingSourceItem("NAME", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff") });
            left.GetHashCode().Equals(right.GetHashCode()).Should().BeTrue();
        }

        [Fact]
        public void HashCode_WithDifferentNames_ReturnsFalse()
        {
            var left = new PackageSourceMappingSourceItem("name2", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            left.GetHashCode().Equals(right.GetHashCode()).Should().BeFalse();
        }

        [Fact]
        public void HashCode_WithDifferentPackagePatterns_ReturnsTrue()
        {
            var left = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            var right = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff3") });
            left.GetHashCode().Equals(right.GetHashCode()).Should().BeTrue();
        }

        [Fact]
        public void ElementNameGetter_ReturnsPackageSource()
        {
            var original = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            original.ElementName.Should().Be("packageSource");
        }

        [Fact]
        public void Clone_CreatesEquivalentPackagePatterns()
        {
            var original = new PackageSourceMappingSourceItem("name", new List<PackagePatternItem>() { new PackagePatternItem("stuff"), new PackagePatternItem("stuff2") });
            var clone = original.Clone() as PackageSourceMappingSourceItem;
            original.Equals(clone).Should().BeTrue();
            original.GetHashCode().Equals(clone.GetHashCode()).Should().BeTrue();
            SettingsTestUtils.DeepEquals(original, clone).Should().BeTrue();
            ReferenceEquals(original, clone).Should().BeFalse();
            original.Key.Equals(clone.Key);
        }

        [Fact]
        public void PackageSourcePatternItemParse_WithoutKey_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource id=""nuget.org"">
            <package pattern=""sadas""  />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<NuGetConfigurationException>();
            ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'key' in element 'packageSource'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
        }

        [Fact]
        public void PackageSourceMappingSourceItemParse_WithoutPattern_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<NuGetConfigurationException>();
            ex.Message.Should().Be(string.Format("Package source '{0}' must have at least one package pattern. Path: '{1}'", "nuget.org", Path.Combine(mockBaseDirectory, nugetConfigPath)));
        }

        [Fact]
        public void PackageSourceMappingSourceItemParse_WithValidData_ParsesCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""sadas"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var settingsFile = new SettingsFile(mockBaseDirectory);
            var section = settingsFile.GetSection("packageSourceMapping");
            section.Should().NotBeNull();

            section.Items.Count.Should().Be(1);
            var packageSourceMappingSourceItem = section.Items.First() as PackageSourceMappingSourceItem;
            var item = packageSourceMappingSourceItem.Patterns.First();
            var expectedItem = new PackagePatternItem("sadas");
            SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
        }

        [Fact]
        public void PackageSourceMappingSourceItemParse_WithUnrecognizedItems_UnknownItemsAreIgnored()
        {
            // Arrange
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""sadas"" />
            <notANamespace id=""sadas"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var settingsFile = new SettingsFile(mockBaseDirectory);
            var section = settingsFile.GetSection("packageSourceMapping");
            section.Should().NotBeNull();

            section.Items.Count.Should().Be(1);
            var packageSourceMappingSourceItem = section.Items.First() as PackageSourceMappingSourceItem;
            var item = packageSourceMappingSourceItem.Patterns.First();
            var expectedItem = new PackagePatternItem("sadas");
            SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
        }

        [Fact]
        public void Update_WithAdditionalPatterns_AddsAdditionalPattern()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var settingsFile = new SettingsFile(mockBaseDirectory);
            settingsFile.TryGetSection("packageSourceMapping", out var section).Should().BeTrue();
            section.Should().NotBeNull();

            section.Items.Count.Should().Be(1);
            var packageSourceMappingSourceItem = section.Items.First() as PackageSourceMappingSourceItem;
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(1);

            var clone = packageSourceMappingSourceItem.Clone() as PackageSourceMappingSourceItem;
            clone.Patterns.Add(new PackagePatternItem("second"));

            packageSourceMappingSourceItem.Update(clone);
            settingsFile.SaveToDisk();

            // Assert
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
            <package pattern=""second"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
        }

        [Fact]
        public void Update_WithDuplicatePatterns_WritesSinglePattern()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
            <package pattern=""first"" />
            <package pattern=""second"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var settingsFile = new SettingsFile(mockBaseDirectory);
            settingsFile.TryGetSection("packageSourceMapping", out var section).Should().BeTrue();
            section.Should().NotBeNull();

            section.Items.Count.Should().Be(1);
            var packageSourceMappingSourceItem = section.Items.First() as PackageSourceMappingSourceItem;
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(3);

            var clone = packageSourceMappingSourceItem.Clone() as PackageSourceMappingSourceItem;
            clone.Patterns.Add(new PackagePatternItem("third"));
            clone.Patterns.Add(new PackagePatternItem("third")); // Add a duplicate pattern to ensure it's handled without exception.

            packageSourceMappingSourceItem.Update(clone);
            settingsFile.SaveToDisk();

            // Assert
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
            <package pattern=""second"" />
            <package pattern=""third"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
        }

        [Fact]
        public void Update_WithRemovedPattern_RemovesPattern()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
            <package pattern=""second"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var settingsFile = new SettingsFile(mockBaseDirectory);
            settingsFile.TryGetSection("packageSourceMapping", out var section).Should().BeTrue();
            section.Should().NotBeNull();

            section.Items.Count.Should().Be(1);
            var packageSourceMappingSourceItem = section.Items.First() as PackageSourceMappingSourceItem;
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(2);

            var clone = packageSourceMappingSourceItem.Clone() as PackageSourceMappingSourceItem;
            clone.Patterns.RemoveAt(1);

            packageSourceMappingSourceItem.Update(clone);
            settingsFile.SaveToDisk();

            // Assert
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
        }

        [Fact]
        public void Update_WithoutAnyPackagePattern_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var settingsFile = new SettingsFile(mockBaseDirectory);
            settingsFile.TryGetSection("packageSourceMapping", out var section).Should().BeTrue();
            section.Should().NotBeNull();

            section.Items.Count.Should().Be(1);
            var packageSourceMappingSourceItem = section.Items.First() as PackageSourceMappingSourceItem;
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(1);

            var clone = packageSourceMappingSourceItem.Clone() as PackageSourceMappingSourceItem;
            clone.Patterns.Clear();

            var ex = Record.Exception(() => packageSourceMappingSourceItem.Update(clone));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<InvalidOperationException>();
            ex.Message.Should().Be(string.Format("Package source 'nuget.org' must have at least one package pattern."));
        }

        [Fact]
        public void Update_WithAddedAndRemovedPatterns_CorrectlyAddsAndRemovesPatterns()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
            <package pattern=""second"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

            // Act and Assert
            var settingsFile = new SettingsFile(mockBaseDirectory);
            settingsFile.TryGetSection("packageSourceMapping", out var section).Should().BeTrue();
            section.Should().NotBeNull();

            section.Items.Count.Should().Be(1);
            var packageSourceMappingSourceItem = section.Items.First() as PackageSourceMappingSourceItem;
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(2);

            var clone = packageSourceMappingSourceItem.Clone() as PackageSourceMappingSourceItem;
            clone.Patterns.RemoveAt(1);
            clone.Patterns.Add(new PackagePatternItem("third"));

            packageSourceMappingSourceItem.Update(clone);
            settingsFile.SaveToDisk();

            // Assert
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""first"" />
            <package pattern=""third"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
        }
    }
}
