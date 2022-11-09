// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class SettingSectionTests
    {
        [Fact]
        public void SettingSection_GetValues_UnexistantChild_ReturnsNull()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <SectionName>
        <add key='key1' value='value1' />
        <add key='key2' value='value2' />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var key3Element = section.GetFirstItemWithAttribute<AddItem>("key", "key3");

                // Assert
                key3Element.Should().BeNull();
            }
        }

        [Fact]
        public void SettingSection_GetValues_ReturnsAllChildElements()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <SectionName>
        <add key='key1' value='value1' />
        <add key='key2' value='value2' />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var children = section.Items;

                // Assert
                children.Should().NotBeEmpty();
                children.Count.Should().Be(2);
                children.Should().AllBeOfType<AddItem>();
            }
        }

        [Fact]
        public void SettingSection_WithAClear_ParseClearCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <config>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
        <clear />
        <add key='key2' value='value2' meta3='data3'/>
    </config>
</configuration>";

            var expectedItem = new AddItem("key2", "value2", new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string> {
                    { "meta3", "data3" }
                }));

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("config");
                section.Should().NotBeNull();

                var children = section.Items.ToList();

                // Assert
                children.Should().NotBeEmpty();
                children.Count.Should().Be(2);
                children.FirstOrDefault().Should().BeOfType<ClearItem>();
                SettingsTestUtils.DeepEquals(children[1], expectedItem).Should().BeTrue();
            }
        }

        [Fact]
        public void SettingSection_AddOrUpdate_AddsAnElementCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(2);

                settingsFile.AddOrUpdate("Section", new AddItem("key2", "value2"));

                section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(3);

                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().NotBeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingSection_AddOrUpdate_UpdatesAnElementCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(2);

                settingsFile.AddOrUpdate("Section", new AddItem("key0", "value0", new Dictionary<string, string>() { { "meta1", "data1" } }));

                section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(2);

                var item = section.Items.First() as AddItem;
                item.Should().NotBeNull();
                item.AdditionalAttributes.Count.Should().Be(1);
                item.AdditionalAttributes["meta1"].Should().Be("data1");

                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().NotBeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingSection_AddOrUpdate_ToMachineWide_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: true, isReadOnly: false);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(2);

                var ex = Record.Exception(() => settingsFile.AddOrUpdate("Section", new AddItem("key2", "value2")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("Unable to update setting since it is in a machine-wide NuGet.Config.");

                section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(2);

                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingSection_AddOrUpdate_ToReadOnly_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: false, isReadOnly: true);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(2);

                var ex = Record.Exception(() => settingsFile.AddOrUpdate("Section", new AddItem("key2", "value2")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be(Resources.CannotUpdateReadOnlyConfig);

                section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(2);

                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingSection_Remove_Succeeds()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var child = section.GetFirstItemWithAttribute<AddItem>("key", "key0");
                child.Should().NotBeNull();

                settingsFile.Remove("Section", child);
                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().NotBeEquivalentTo(configFileHash);

                section = settingsFile.GetSection("Section");
                section.Items.Count.Should().Be(1);
                var deletedChild = section.GetFirstItemWithAttribute<AddItem>("key", "key0");
                deletedChild.Should().BeNull();
            }
        }

        [Fact]
        public void SettingSection_Remove_ToMachineWide_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: true, isReadOnly: false);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var child = section.GetFirstItemWithAttribute<AddItem>("key", "key0");
                child.Should().NotBeNull();

                var ex = Record.Exception(() => settingsFile.Remove("Section", child));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("Unable to update setting since it is in a machine-wide NuGet.Config.");

                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingSection_Remove_ToReadOnly_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: false, isReadOnly: true);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var child = section.GetFirstItemWithAttribute<AddItem>("key", "key0");
                child.Should().NotBeNull();

                var ex = Record.Exception(() => settingsFile.Remove("Section", child));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be(Resources.CannotUpdateReadOnlyConfig);

                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingSection_Remove_OnlyOneChild_SucceedsAndRemovesSection()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var child = section.GetFirstItemWithAttribute<AddItem>("key", "key0");
                child.Should().NotBeNull();

                settingsFile.Remove("Section", child);
                settingsFile.SaveToDisk();

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().NotBeEquivalentTo(configFileHash);

                section = settingsFile.GetSection("Section");
                section.Should().BeNull();
            }
        }

        [Fact]
        public void SettingSection_Remove_UnexistantChild_DoesNotRemoveAnything()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <add key='key0' value='value0' />
        <add key='key1' value='value1' meta1='data1' meta2='data2'/>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var configFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));

                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                section.Remove(new AddItem("key7", "value7"));

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);

                section.Items.Count.Should().Be(2);
            }
        }

        [Fact]
        public void SettingSection_Merge_JoinsTwoSectionsCorrectly()
        {
            var firstSection = new VirtualSettingSection("Section", new AddItem("key1", "value1"), new AddItem("key2", "value2"));
            var secondSection = new VirtualSettingSection("Section", new AddItem("key2", "valueX"), new AddItem("key3", "value3"));

            var expectedSection = new VirtualSettingSection("Section", new AddItem("key1", "value1"), new AddItem("key2", "valueX"), new AddItem("key3", "value3"));

            firstSection.Merge(secondSection);

            SettingsTestUtils.DeepEquals(firstSection, expectedSection).Should().BeTrue();
        }

        [Fact]
        public void SettingSection_Merge_WithTwoDifferentSections_Throws()
        {
            var firstSection = new VirtualSettingSection("Section", new AddItem("key1", "value1"), new AddItem("key2", "value2"));
            var secondSection = new VirtualSettingSection("SectionName", new AddItem("key2", "valueX"), new AddItem("key3", "value3"));

            var ex = Record.Exception(() => firstSection.Merge(secondSection));
            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
            ex.Message.Should().Be("Cannot merge two different sections.");
        }

        [Fact]
        public void SettingSection_Equals_WithSameElementName_ReturnsTrue()
        {
            var section1 = new VirtualSettingSection("section", new AddItem("key1", "val"));
            var section2 = new VirtualSettingSection("section");

            section1.Equals(section2).Should().BeTrue();
        }

        [Fact]
        public void SettingSection_Equals_WithDifferentElementName_ReturnsFalse()
        {
            var section1 = new VirtualSettingSection("section", new AddItem("key1", "val"));
            var section2 = new VirtualSettingSection("section1", new AddItem("key1", "val"));

            section1.Equals(section2).Should().BeFalse();
        }

        [Fact]
        public void VirtualSection_AsXNode_WithSpaceOnName_IsCorrect()
        {
            var section = new VirtualSettingSection("section name", new AddItem("key1", "val"));

            var xNode = section.AsXNode();
            xNode.Should().BeOfType<XElement>();
            var xelement = xNode as XElement;
            xelement.Name.LocalName.Should().Be("section_x0020_name");
        }

        [Fact]
        public void VirtualSection_ElementName_IsCorrect()
        {
            var settingSection = new VirtualSettingSection("section", new AddItem("key1", "val"));

            settingSection.ElementName.Should().Be("section");
        }

        [Fact]
        public void VirtualSection_ElementName_WithSpaceOnName_IsCorrect()
        {
            var section = new VirtualSettingSection("section name", new AddItem("key1", "val"));

            section.ElementName.Should().Be("section name");
        }

        [Fact]
        public void ParsedSection_ElementName_IsCorrect()
        {
            var settingSection = new ParsedSettingSection("section", new AddItem("key1", "val"));

            settingSection.ElementName.Should().Be("section");
        }

        [Fact]
        public void SettingSection_Clone_ReturnsSectionClone()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <add key=""key1"" value=""val"" meta=""data"" />
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
                section.Should().BeOfType<ParsedSettingSection>();

                section.IsCopy().Should().BeFalse();
                section.Origin.Should().NotBeNull();

                var clone = section.Clone() as SettingSection;
                clone.IsAbstract().Should().BeTrue();
                clone.Origin.Should().BeNull();
                clone.Should().BeOfType<VirtualSettingSection>();

                SettingsTestUtils.DeepEquals(clone, section).Should().BeTrue();

                var virtualSection = new VirtualSettingSection("SectionName",
                    new AddItem("key1", "val",
                        new Dictionary<string, string>() { { "meta", "data" } }));
                virtualSection.Origin.Should().BeNull();
                virtualSection.IsAbstract().Should().BeTrue();

                var virtualClone = virtualSection.Clone() as SettingSection;
                virtualClone.IsAbstract().Should().BeTrue();
                virtualClone.Origin.Should().BeNull();
                virtualClone.Should().BeOfType<VirtualSettingSection>();

                SettingsTestUtils.DeepEquals(virtualClone, virtualSection).Should().BeTrue();
            }
        }
    }
}
