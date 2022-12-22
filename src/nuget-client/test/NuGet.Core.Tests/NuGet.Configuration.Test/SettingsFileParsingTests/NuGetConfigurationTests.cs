// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class NuGetConfigurationTests
    {
        [Fact]
        public void NuGetConfiguration_AsXNode_ReturnsExpectedXNode()
        {
            var configuration = new NuGetConfiguration(
                new VirtualSettingSection("Section",
                    new AddItem("key0", "value0")));

            var expectedXNode = new XElement("configuration",
                new XElement("Section",
                    new XElement("add",
                        new XAttribute("key", "key0"),
                        new XAttribute("value", "value0"))));

            var xNode = configuration.AsXNode();

            XNode.DeepEquals(xNode, expectedXNode).Should().BeTrue();
        }

        [Fact]
        public void NuGetConfiguration__CaseInsensitive_ParsedSuccessfully()
        {
            // Arrange
            var config = @"
<cOnfiGUraTiOn>
</cOnfiGUraTiOn>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.Should().NotBeNull();
            }
        }

        [Fact]
        public void NuGetConfiguration_Equals_WithSameSections_ReturnsTrue()
        {
            var configuration1 = new NuGetConfiguration(new VirtualSettingSection("section1"), new VirtualSettingSection("section2"));
            var configuration2 = new NuGetConfiguration(new VirtualSettingSection("section1"), new VirtualSettingSection("section2"));

            configuration1.Equals(configuration2).Should().BeTrue();
        }

        [Fact]
        public void NuGetConfiguration_Equals_WithDifferentSections_ReturnsFalse()
        {
            var configuration1 = new NuGetConfiguration(new VirtualSettingSection("section1"), new VirtualSettingSection("section2"));
            var configuration2 = new NuGetConfiguration(new VirtualSettingSection("section1"));

            configuration1.Equals(configuration2).Should().BeFalse();
        }

        [Fact]
        public void NuGetConfiguration_ElementName_IsCorrect()
        {
            var nugetConfiguration = new NuGetConfiguration();

            nugetConfiguration.ElementName.Should().Be("configuration");
        }
    }
}
