// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class ClearItemTests
    {
        [Fact]
        public void ClearItem_CaseInsensitive_ParsedSuccessfully()
        {
            // Arrange
            var config = @"
<configuration>
    <section>
        <clEaR />
    </section>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.Should().NotBeNull();

                var section = settingsFile.GetSection("section");
                section.Should().NotBeNull();

                var children = section!.Items.ToList();

                children.Should().NotBeEmpty();
                children.Count.Should().Be(1);

                children[0].Should().BeOfType<ClearItem>();
            }
        }

        [Fact]
        public void ClearItem_SingleTag_ParsedSuccessfully()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <clear />
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.Should().NotBeNull();

                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                section.Items.FirstOrDefault().Should().BeOfType<ClearItem>();
            }
        }

        [Fact]
        public void ClearItem_MultiTag_ParsedSuccessfully()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <clear></clear>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.Should().NotBeNull();

                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                section.Items.FirstOrDefault().Should().BeOfType<ClearItem>();
            }
        }

        [Fact]
        public void ClearItem_Parsing_ElementWithChildren_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <clear>
            <add key='key2' value='value2' />
        </clear>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Error parsing NuGet.Config. Element '{0}' cannot have descendant elements. Path: '{1}'.", "clear", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void ClearItem_Equals_WithAnotherClearItem_ReturnsTrue()
        {
            var clear1 = new ClearItem();
            var clear2 = new ClearItem();

            clear1.Equals(clear2).Should().BeTrue();
        }

        [Fact]
        public void ClearItem_Equals_WithDifferentItem_ReturnsFalse()
        {
            var clear1 = new ClearItem();
            var differentItem = new AddItem("keyN", "value1");

            clear1.Equals(differentItem).Should().BeFalse();
        }

        [Fact]
        public void ClearItem_ElementName_IsCorrect()
        {
            var clearItem = new ClearItem();

            clearItem.ElementName.Should().Be("clear");
        }

        [Fact]
        public void ClearItem_Clone_ReturnsItemClone()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <clear />
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

                var clone = (ClearItem)item.Clone();
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();
            }
        }
    }
}
