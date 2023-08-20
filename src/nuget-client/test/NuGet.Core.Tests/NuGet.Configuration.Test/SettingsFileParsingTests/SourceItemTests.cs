// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
        <add key='nuget2' value='http://serviceIndex.test2/api/index.json' protocolVersion='3' />
        <add key='nuget3' value='http://serviceIndex.test3/api/index.json' protocolVersion='3' ALLOWInsecureConnections='true' />
        <add key='nuget4' value='http://serviceIndex.test4/api/index.json' allowInsecureConnections='true' />
        <add key='nuget5' value='http://serviceIndex.test5/api/index.json' allowInsecureConnections='false' protocolVersion='2' />
    </packageSources>
</configuration>";

            var expectedValues = new List<SourceItem>()
            {
                new SourceItem("nugetorg","http://serviceIndexorg.test/api/index.json"),
                new SourceItem("nuget2","http://serviceIndex.test2/api/index.json", "3" ),
                new SourceItem("nuget3","http://serviceIndex.test3/api/index.json", protocolVersion: "3", allowInsecureConnections: "true" ),
                new SourceItem("nuget4","http://serviceIndex.test4/api/index.json", protocolVersion: null, allowInsecureConnections: "true"  ),
                new SourceItem("nuget5","http://serviceIndex.test5/api/index.json", protocolVersion: "2", allowInsecureConnections: "false"),
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

                var children = section.Items.Cast<SourceItem>().ToList();

                children.Should().NotBeEmpty();
                children.Count.Should().Be(5);

                for (var i = 0; i < children.Count; i++)
                {
                    SettingsTestUtils.DeepEquals(children[i], expectedValues[i]).Should().BeTrue();
                    children[i].Key.Should().Be(expectedValues[i].Key, because: $"SourceItem[{i}].Key is {children[i].Key}, but it's expected to be {expectedValues[i].Key}");
                    children[i].Value.Should().Be(expectedValues[i].Value, because: $"SourceItem[{i}].Value is {children[i].Value}, but it's expected to be {expectedValues[i].Value}");
                    children[i].ProtocolVersion.Should().Be(expectedValues[i].ProtocolVersion, because: $"SourceItem[{i}].ProtocolVersion is {children[i].ProtocolVersion}, but it's expected to be {expectedValues[i].ProtocolVersion}");
                    children[i].AllowInsecureConnections.Should().Be(expectedValues[i].AllowInsecureConnections, because: $"SourceItem[{i}].AllowInsecureConnections is {children[i].AllowInsecureConnections}, but it's expected to be {expectedValues[i].AllowInsecureConnections}");
                }
            }
        }

        [Fact]
        public void SourceItem_AsXNode_ReturnsExpectedXNode()
        {
            var configuration = new NuGetConfiguration(
                new VirtualSettingSection("packageSources",
                    new SourceItem("nuget1", "http://serviceIndex.test1/api/index.json", protocolVersion: "3", allowInsecureConnections: "true"),
                    new SourceItem("nuget2", "http://serviceIndex.test2/api/index.json", protocolVersion: "2", allowInsecureConnections: "false"),
                    new SourceItem("nuget3", "http://serviceIndex.test3/api/index.json", protocolVersion: null, allowInsecureConnections: "true"),
                    new SourceItem("nuget4", "http://serviceIndex.test4/api/index.json", protocolVersion: "3")));
            var resultXml = SettingsTestUtils.RemoveWhitespace(configuration.AsXNode().ToString());

            var expectedXNode = new XElement("configuration",
                new XElement("packageSources",
                    new XElement("add",
                        new XAttribute("key", "nuget1"),
                        new XAttribute("value", "http://serviceIndex.test1/api/index.json"),
                        new XAttribute("protocolVersion", "3"),
                        new XAttribute("allowInsecureConnections", "true")),
                    new XElement("add",
                        new XAttribute("key", "nuget2"),
                        new XAttribute("value", "http://serviceIndex.test2/api/index.json"),
                        new XAttribute("protocolVersion", "2"),
                        new XAttribute("allowInsecureConnections", "false")),
                    new XElement("add",
                        new XAttribute("key", "nuget3"),
                        new XAttribute("value", "http://serviceIndex.test3/api/index.json"),
                        new XAttribute("allowInsecureConnections", "true")),
                    new XElement("add",
                        new XAttribute("key", "nuget4"),
                        new XAttribute("value", "http://serviceIndex.test4/api/index.json"),
                        new XAttribute("protocolVersion", "3"))));
            var expectedXml = SettingsTestUtils.RemoveWhitespace(expectedXNode.ToString());

            resultXml.Should().Be(expectedXml, because: resultXml);
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
