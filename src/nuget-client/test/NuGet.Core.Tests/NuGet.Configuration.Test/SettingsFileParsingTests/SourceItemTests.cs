// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class SourceItemTests
    {
        [Fact]
        public void SourceItem_CaseInsensitive_ParsedSuccessfully()
        {
            // Arrange
            var config = @"
<configuration>
    <PACkagEsourCEs>
        <AdD key='nugetorg' value='http://serviceIndexorg.test/api/index.json' />
    </PACkagEsourCEs>
</configuration>";

            var expectedValue = new SourceItem("nugetorg", "http://serviceIndexorg.test/api/index.json");

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.Should().NotBeNull();

                var section = settingsFile.GetSection("PACkagEsourCEs");
                section.Should().NotBeNull();

                var children = section.Items.ToList();

                children.Should().NotBeEmpty();
                children.Count.Should().Be(1);

                SettingsTestUtils.DeepEquals(children[0], expectedValue).Should().BeTrue();
            }
        }

        [Fact]
        public void SourceItem_ParsedSuccessfully()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSources>
        <add key='nugetorg' value='http://serviceIndexorg.test/api/index.json' />
        <add key='nuget2' value='http://serviceIndex.test/api/index.json' protocolVersion='3' />
    </packageSources>
</configuration>";

            var expectedValues = new List<SourceItem>()
            {
                new SourceItem("nugetorg","http://serviceIndexorg.test/api/index.json"),
                new SourceItem("nuget2","http://serviceIndex.test/api/index.json", "3" ),
            };

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.Should().NotBeNull();

                var section = settingsFile.GetSection("packageSources");
                section.Should().NotBeNull();

                var children = section.Items.ToList();

                children.Should().NotBeEmpty();
                children.Count.Should().Be(2);

                for (var i = 0; i < children.Count; i++)
                {
                    SettingsTestUtils.DeepEquals(children[i], expectedValues[i]).Should().BeTrue();
                }
            }
        }

        [Fact]
        public void SourceItem_Parsing_ElementWithChildren_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <packageSources>
        <add key='nugetorg' value='http://serviceIndexorg.test/api/index.json'>
            <add key='key2' value='value2' />
        </add>
    </packageSources>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Error parsing NuGet.Config. Element '{0}' cannot have descendant elements. Path: '{1}'.", "add", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void SourceItem_Equals_WithSameKeyAndProtocol_ReturnsTrue()
        {
            var source1 = new SourceItem("key1", "value1", "3");
            var source2 = new SourceItem("key1", "valueN", "3");

            source1.Equals(source2).Should().BeTrue();
        }

        [Fact]
        public void SourceItem_Equals_WithSameKeyDifferentProtocol_ReturnsTrue()
        {
            var source1 = new SourceItem("key1", "value1", "2");
            var source2 = new SourceItem("key1", "value1", "3");

            source1.Equals(source2).Should().BeTrue();
        }

        [Fact]
        public void SourceItem_Equals_WithSameProtocolDifferentKey_ReturnsFalse()
        {
            var source1 = new SourceItem("key1", "value1", "3");
            var source2 = new SourceItem("keyN", "value1", "3");

            source1.Equals(source2).Should().BeFalse();
        }

        [Fact]
        public void SourceItem_ElementName_IsCorrect()
        {
            var sourceItem = new SourceItem("key1", "value1");

            sourceItem.ElementName.Should().Be("add");
        }

        [Fact]
        public void SourceItem_Clone_ReturnsItemClone()
        {
            // Arrange
            var config = @"
<configuration>
    <packageSources>
        <add key=""key1"" value=""val"" protocolVersion=""5"" />
    </packageSources>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("packageSources", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section.Items.Count.Should().Be(1);
                var item = section.Items.First();
                item.IsCopy().Should().BeFalse();
                item.Origin.Should().NotBeNull();

                var clone = item.Clone() as SourceItem;
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();
            }
        }
    }
}
