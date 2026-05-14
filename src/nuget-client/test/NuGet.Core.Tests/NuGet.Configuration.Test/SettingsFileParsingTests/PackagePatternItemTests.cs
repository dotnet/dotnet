// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class PackagePatternItemTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_WithInvalidPackagePattern_Throws(string name)
        {
            Assert.Throws<ArgumentException>(() => new PackagePatternItem(name));
        }

        [Theory]
        [InlineData("NuGet.Common", "NuGet.Common")]
        [InlineData("nuget.common", "NuGet.Common")]
        [InlineData("NuGet.*", "NuGet.*")]
        [InlineData("NuGet.*", "nuget.*")]
        [InlineData("*", "*")]
        public void Equals_WithEquivalentPatterns_ReturnsTrue(string first, string second)
        {
            var firstPatternItem = new PackagePatternItem(first);
            var secondPatternItem = new PackagePatternItem(second);

            firstPatternItem.Equals(secondPatternItem).Should().BeTrue();
        }

        [Theory]
        [InlineData("NuGet.", "NuGet.Common")]
        [InlineData("NuGet.*", "stuff")]
        [InlineData("NuGet.*", "NuGet.")]
        public void Equals_WithUnequivalentPatterns_ReturnsFalse(string first, string second)
        {
            var firstPatternItem = new PackagePatternItem(first);
            var secondPatternItem = new PackagePatternItem(second);

            firstPatternItem.Equals(secondPatternItem).Should().BeFalse();
        }

        [Theory]
        [InlineData("NuGet.Common", "NuGet.Common")]
        [InlineData("nuget.common", "NuGet.Common")]
        [InlineData("NuGet.*", "NuGet.*")]
        [InlineData("NuGet.*", "nuget.*")]
        public void HashCode_WithEquivalentPatterns_ReturnsTrue(string first, string second)
        {
            var firstPatternItem = new PackagePatternItem(first);
            var secondPatternItem = new PackagePatternItem(second);

            firstPatternItem.GetHashCode().Should().Be(secondPatternItem.GetHashCode());
        }

        [Theory]
        [InlineData("NuGet.", "NuGet.Common")]
        [InlineData("NuGet.*", "stuff")]
        [InlineData("NuGet.*", "NuGet.")]
        public void HashCode_WithUnequivalentPatterns_ReturnsFalse(string first, string second)
        {
            var firstPatternItem = new PackagePatternItem(first);
            var secondPatternItem = new PackagePatternItem(second);

            firstPatternItem.GetHashCode().Equals(secondPatternItem.GetHashCode()).Should().BeFalse();
        }

        [Theory]
        [InlineData("NuGet.Common")]
        [InlineData("nuget.common")]
        [InlineData("NuGet.*")]
        [InlineData("nuget.*")]
        public void Clone_CreatesEquivalentObjects(string patternName)
        {
            var original = new PackagePatternItem(patternName);
            var clone = (PackagePatternItem)original.Clone();

            original.Equals(clone).Should().BeTrue();
            original.GetHashCode().Equals(clone.GetHashCode()).Should().BeTrue();
            SettingsTestUtils.DeepEquals(original, clone).Should().BeTrue();
            ReferenceEquals(original, clone).Should().BeFalse();
            original.Pattern.Equals(clone.Pattern);
        }

        [Fact]
        public void ElementNameGetter_ReturnsPattern()
        {
            var original = new PackagePatternItem("item");
            original.ElementName.Should().Be("package");
        }

        [Fact]
        public void PackagePatternItemParse_WithoutId_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package stuff=""sadas""  />
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
            ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'pattern' in element 'package'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
        }

        [Fact]
        public void PackagePatternItemParse_WithChildren_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""sadas"">
                <add key=""key"" value=""val"" />
            </package>
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
            ex.Message.Should().Be(string.Format("Error parsing NuGet.Config. Element 'package' cannot have descendant elements. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
        }

        [Fact]
        public void PackagePatternItemParse_WithValidData_ParsesCorrectly()
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

            section!.Items.Count.Should().Be(1);
            var item = ((PackageSourceMappingSourceItem)section.Items.First()).Patterns.First();

            var expectedItem = new PackagePatternItem("sadas");
            SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
        }

        [Fact]
        public void Update_UpdatesKeyCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""original"" />
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

            section!.Items.Count.Should().Be(1);
            var packageSourcePatternsItem = (PackageSourceMappingSourceItem)section.Items.First();
            var updatedItem = new PackagePatternItem("updated");
            packageSourcePatternsItem.Patterns.First().Update(updatedItem);
            SettingsTestUtils.DeepEquals(packageSourcePatternsItem.Patterns.First(), updatedItem).Should().BeTrue();

            settingsFile.SaveToDisk();

            // Assert
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""updated"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
        }
    }
}
