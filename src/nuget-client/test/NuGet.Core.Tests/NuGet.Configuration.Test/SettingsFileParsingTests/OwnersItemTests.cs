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
    public class OwnersItemTests
    {
        [Fact]
        public void OwnersItem_WithoutContent_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <owners />
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(
                    string.Format("Unable to parse config file because: Owners item must only have text content and cannot be empty. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void OwnersItem_WithItemsAsContent_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <owners>
            <add key=""key"" value=""val"" />
        </owners>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(
                    string.Format("Unable to parse config file because: Owners item must only have text content and cannot be empty. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void OwnersItem_WithItemsAndContentAsChildren_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <owners>
            test;text
            <add key=""key"" value=""val"" />
        </owners>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(
                    string.Format("Unable to parse config file because: Owners item must only have text content and cannot be empty. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void OwnersItem_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <owners>test;text;owner</owners>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (OwnersItem)section.Items.First();

                var expectedItem = new OwnersItem("test;text;owner");
                SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
            }
        }

        [Fact]
        public void CertificateItem_Update_UpdatesContentCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <owners>test;text;owner</owners>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (OwnersItem)section.Items.First();

                var updatedItem = (OwnersItem)item.Clone();
                updatedItem.Content.Clear();
                updatedItem.Content.Add("abc");

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <owners>abc</owners>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void CertificateItem_Update_RemovingContent_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <owners>test;text;owner</owners>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (OwnersItem)section.Items.First();

                var updatedItem = (OwnersItem)item.Clone();
                updatedItem.Content.Clear();

                var ex = Record.Exception(() => item.Update(updatedItem));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
            }
        }

        [Fact]
        public void OwnersItem_Equals_WithSameFingerprint_ReturnsTrue()
        {
            var owners1 = new OwnersItem("one;two;three");
            var owners2 = new OwnersItem("one;two;three");

            owners1.Equals(owners2).Should().BeTrue();
        }

        [Fact]
        public void OwnersItem_Equals_WithDifferentOrderedOwners_ReturnsTrue()
        {
            var owners1 = new OwnersItem("one;two;three");
            var owners2 = new OwnersItem("one;three;two");

            owners1.Equals(owners2).Should().BeTrue();
        }

        [Fact]
        public void OwnersItem_Equals_WithDifferentContent_ReturnsFalse()
        {
            var owners1 = new OwnersItem("one;two;three");
            var owners2 = new OwnersItem("one;two");

            owners1.Equals(owners2).Should().BeFalse();
        }

        [Fact]
        public void OwnersItem_ElementName_IsCorrect()
        {
            var ownersItem = new OwnersItem("one;two;three");

            ownersItem.ElementName.Should().Be("owners");
        }

        [Fact]
        public void OwnersItem_Clone_CopiesTheSameItem()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <owners>test;text;owner</owners>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = section.Items.First();
                item.IsCopy().Should().BeFalse();
                item.Origin.Should().NotBeNull();

                var clone = item.Clone();
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();
            }
        }
    }
}
