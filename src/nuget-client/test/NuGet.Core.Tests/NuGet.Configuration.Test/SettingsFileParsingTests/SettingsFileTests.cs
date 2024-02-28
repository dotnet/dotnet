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
    public class SettingsFileTests
    {
        [Fact]
        public void SettingsFile_Constructor_WithNullRoot_Throws()
        {
            // Act & Assert
            var ex = Record.Exception(() => new SettingsFile(null!));
            Assert.NotNull(ex);
            Assert.IsAssignableFrom<ArgumentException>(ex);
        }

        [Fact]
        public void SettingsFile_Constructor_WithMalformedConfig_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration><sectionName></configuration>");

                // Act & Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("NuGet.Config is not valid XML. Path: '{0}'.", Path.Combine(mockBaseDirectory, configFile)));
            }
        }


        [Fact]
        public void SettingsFile_Constructor_InvalidXml_Throws()
        {
            // Arrange
            var config = @"boo>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                // Assert
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("NuGet.Config is not valid XML. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void SettingsFile_Constructor_WithInvalidRootElement_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = @"
<notvalid>
    <SectionName>
        <add key='key1' value='value1' />
        <add key='key2' value='value2' />
    </SectionName>
</notvalid>";

                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, config);

                // Act & Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("NuGet.Config does not contain the expected root element: 'configuration'. Path: '{0}'.", Path.Combine(mockBaseDirectory, configFile)));
            }
        }

        [Fact]
        public void SettingsFile_Constructor_ConfigurationPath_Succeds()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = @"
<configuration>
    <SectionName>
        <add key='key1' value='value1' />
        <add key='key2' value='value2' />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, config);

                // Act
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Assert
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var key1Element = section!.GetFirstItemWithAttribute<AddItem>("key", "key1");
                var key2Element = section.GetFirstItemWithAttribute<AddItem>("key", "key2");
                key1Element.Should().NotBeNull();
                key2Element.Should().NotBeNull();

                key1Element!.Value.Should().Be("value1");
                key2Element!.Value.Should().Be("value2");
            }
        }

        [Fact]
        public void SettingsFile_Constructor_ConfigurationPath_AndFilename_Succeds()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = @"
<configuration>
    <SectionName>
        <add key='key1' value='value1' />
        <add key='key2' value='value2' />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, config);

                // Act
                var settingsFile = new SettingsFile(mockBaseDirectory, configFile);

                // Assert
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var key1Element = section!.GetFirstItemWithAttribute<AddItem>("key", "key1");
                var key2Element = section.GetFirstItemWithAttribute<AddItem>("key", "key2");
                key1Element.Should().NotBeNull();
                key2Element.Should().NotBeNull();

                key1Element!.Value.Should().Be("value1");
                key2Element!.Value.Should().Be("value2");
            }
        }

        [Fact]
        public void SettingsFile_Constructor_CreateDefaultConfigFileIfNoConfig()
        {
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Act
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Assert
                var text = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "NuGet.Config")));

                var expectedResult = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <add key=""nuget.org"" value=""https://api.nuget.org/v3/index.json"" protocolVersion=""3"" />
    </packageSources>
</configuration>");

                text.Should().Be(expectedResult);
            }
        }

        [Fact]
        public void SettingsFile_GetSections_WithNonExistantSection_ReturnsNull()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                var section = settingsFile.GetSection("DoesNotExist");
                section.Should().BeNull();
            }
        }

        [Fact]
        public void SettingsFile_GetSections_DuplicatedSections_TakesFirstAndIgnoresRest()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <add key='key1' value='value1' />
    </SectionName>
    <SectionName>
        <add key='key2' value='value2' />
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

                var addItem = section!.Items.FirstOrDefault() as AddItem;
                addItem.Should().NotBeNull();

                addItem!.Key.Should().Be("key1");
                addItem.Value.Should().Be("value1");
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_WithEmptySectionName_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                var ex = Record.Exception(() => settingsFile.AddOrUpdate("", new AddItem("SomeKey", "SomeValue")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<ArgumentException>();
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_WithNullItem_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                var ex = Record.Exception(() => settingsFile.AddOrUpdate("SomeKey", item: null!));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<ArgumentNullException>();
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_WithMachineWideSettings_Throws()
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

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: true, isReadOnly: false);

                // Act
                var ex = Record.Exception(() => settingsFile.AddOrUpdate("section", new AddItem("SomeKey", "SomeValue")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be(Resources.CannotUpdateMachineWide);

                settingsFile.SaveToDisk();

                var section = settingsFile.GetSection("Section");
                section!.Items.Count.Should().Be(1);

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_WithReadOnlySettings_Throws()
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

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: false, isReadOnly: true);

                // Act
                var ex = Record.Exception(() => settingsFile.AddOrUpdate("section", new AddItem("SomeKey", "SomeValue")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be(Resources.CannotUpdateReadOnlyConfig);

                settingsFile.SaveToDisk();

                var section = settingsFile.GetSection("Section");
                section!.Items.Count.Should().Be(1);

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_SectionThatDoesntExist_WillAddSection()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""value"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                settingsFile.AddOrUpdate("NewSectionName", new AddItem("key", "value"));
                settingsFile.SaveToDisk();

                // Assert
                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""value"" />
    </SectionName>
    <NewSectionName>
        <add key=""key"" value=""value"" />
    </NewSectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, configFile))).Should().Be(result);
                var section = settingsFile.GetSection("NewSectionName");
                section.Should().NotBeNull();
                section!.Items.Count.Should().Be(1);
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_SectionThatExist_WillAddToSection()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""value"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                settingsFile.AddOrUpdate("SectionName", new AddItem("keyTwo", "valueTwo"));
                settingsFile.SaveToDisk();

                // Assert
                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""value"" />
        <add key=""keyTwo"" value=""valueTwo"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, configFile))).Should().Be(result);
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_WhenItemExistsInSection_OverrideItem()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""value"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                settingsFile.AddOrUpdate("SectionName", new AddItem("key", "NewValue"));
                settingsFile.SaveToDisk();

                // Assert
                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""NewValue"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, configFile))).Should().Be(result);
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<AddItem>("key", "key");
                item.Should().NotBeNull();
                item!.Value.Should().Be("NewValue");
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_PreserveComments()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- Comment in nuget configuration -->
<configuration>
    <!-- Will add an item to this section -->
    <SectionName>
        <add key=""key1"" value=""value"" />
    </SectionName>
    <!-- This section wont have a new item -->
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                settingsFile.AddOrUpdate("SectionName", new AddItem("newKey", "value"));
                settingsFile.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- Comment in nuget configuration -->
<configuration>
    <!-- Will add an item to this section -->
    <SectionName>
        <add key=""key1"" value=""value"" />
        <add key=""newKey"" value=""value"" />
    </SectionName>
    <!-- This section wont have a new item -->
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);

                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<AddItem>("key", "newKey");
                item.Should().NotBeNull();
                item!.Value.Should().Be("value");
            }
        }

        [Fact]
        public void SettingsFile_AddOrUpdate_PreserveUnknownItems()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value"" />
    </SectionName>
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
    <UnknownSection>
        <UnknownItem meta1=""data1"" />
        <OtherUnknownItem>
        </OtherUnknownItem>
    </UnknownSection>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                settingsFile.AddOrUpdate("SectionName", new AddItem("newKey", "value"));
                settingsFile.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value"" />
        <add key=""newKey"" value=""value"" />
    </SectionName>
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
    <UnknownSection>
        <UnknownItem meta1=""data1"" />
        <OtherUnknownItem>
        </OtherUnknownItem>
    </UnknownSection>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);

                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<AddItem>("key", "newKey");
                item.Should().NotBeNull();
                item!.Value.Should().Be("value");
            }
        }

        [Fact]
        public void SettingsFile_Remove_WithMachineWideSettings_Throws()
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

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: true, isReadOnly: false);

                // Act & Assert
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<AddItem>("key", "key0");
                item.Should().NotBeNull();
                item!.Value.Should().Be("value0");

                var ex = Record.Exception(() => settingsFile.Remove("Section", item));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("Unable to update setting since it is in a machine-wide NuGet.Config.");

                settingsFile.SaveToDisk();

                var section1 = settingsFile.GetSection("Section");
                section1!.Items.Count.Should().Be(1);

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingsFile_Remove_WithReadOnlySettings_Throws()
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

                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: false, isReadOnly: true);

                // Act & Assert
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<AddItem>("key", "key0");
                item.Should().NotBeNull();
                item!.Value.Should().Be("value0");

                var ex = Record.Exception(() => settingsFile.Remove("Section", item));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be(Resources.CannotUpdateReadOnlyConfig);

                settingsFile.SaveToDisk();

                var section1 = settingsFile.GetSection("Section");
                section1!.Items.Count.Should().Be(1);

                var updatedFileHash = SettingsTestUtils.GetFileHash(Path.Combine(mockBaseDirectory, nugetConfigPath));
                updatedFileHash.Should().BeEquivalentTo(configFileHash);
            }
        }

        [Fact]
        public void SettingsFile_Remove_LastValueInSection_RemovesSection()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value2"" />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                var settingsFile = new SettingsFile(Path.Combine(mockBaseDirectory));

                // Act & Assert
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item!.Value.Should().Be("value2");

                settingsFile.Remove("SectionName", item);
                settingsFile.SaveToDisk();

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?><configuration></configuration>"));

                section = settingsFile.GetSection("SectionName");
                section.Should().BeNull();
            }
        }


        [Fact]
        public void SettingsFile_Remove_WithValidSectionAndKey_DeletesTheEntry()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<SettingItem>("key", "DeleteMe");
                settingsFile.Remove("SectionName", item!);
                settingsFile.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);

                section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section!.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void SettingsFile_Remove_PreserveComments()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- Comment in nuget configuration -->
<configuration>
    <!-- This section has the item to delete -->
    <SectionName>
        <add key=""DeleteMe"" value=""value"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
    <!-- This section doesn't have the item to delete -->
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<SettingItem>("key", "DeleteMe");
                settingsFile.Remove("SectionName", item!);
                settingsFile.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- Comment in nuget configuration -->
<configuration>
    <!-- This section has the item to delete -->
    <SectionName>
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
    <!-- This section doesn't have the item to delete -->
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);

                section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section!.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void SettingsFile_Remove_PreserveUnknownItems()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
    <UnknownSection>
        <UnknownItem meta1=""data1"" />
        <OtherUnknownItem>
        </OtherUnknownItem>
    </UnknownSection>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section!.GetFirstItemWithAttribute<SettingItem>("key", "DeleteMe");
                settingsFile.Remove("SectionName", item!);
                settingsFile.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
    <SectionName2>
        <add key=""key"" value=""value"" />
    </SectionName2>
    <UnknownSection>
        <UnknownItem meta1=""data1"" />
        <OtherUnknownItem>
        </OtherUnknownItem>
    </UnknownSection>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);

                section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section!.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void SettingsFile_IsEmpty_WithEmptyValidConfig_ReturnsTrue()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                settingsFile.IsEmpty().Should().BeTrue();
            }
        }

        [Fact]
        public void SettingsFile_IsEmpty_WithNonemptyNuGetConfig_ReturnsFalse()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <add key='key1' value='value1' />
    </SectionName>
</configuration>";
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act & Assert
                settingsFile.IsEmpty().Should().BeFalse();
            }
        }

        [Fact]
        public void SettingsFile_PriorityIsPreservedInSettings()
        {
            // Arrange
            var configFile = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            using (var mockSubDirectory = TestDirectory.Create(mockBaseDirectory))
            using (var mockSubSubDirectory = TestDirectory.Create(mockSubDirectory))
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                SettingsTestUtils.CreateConfigurationFile(configFile, mockSubDirectory, @"<configuration></configuration>");
                SettingsTestUtils.CreateConfigurationFile(configFile, mockSubSubDirectory, @"<configuration></configuration>");

                var baseSettingsFile = new SettingsFile(mockBaseDirectory);
                var subSettingsFile = new SettingsFile(mockSubDirectory);
                var subSubSettingsFile = new SettingsFile(mockSubSubDirectory);

                // Act & Assert
                baseSettingsFile.Should().NotBeNull();
                subSettingsFile.Should().NotBeNull();
                subSubSettingsFile.Should().NotBeNull();
                var settings = new Settings(new List<SettingsFile>() { subSubSettingsFile, subSettingsFile, baseSettingsFile });

                var priority = settings.Priority.GetEnumerator();

                Assert.True(priority.MoveNext());
                priority.Current.Should().BeSameAs(subSubSettingsFile);
                Assert.True(priority.MoveNext());
                priority.Current.Should().BeSameAs(subSettingsFile);
                Assert.True(priority.MoveNext());
                priority.Current.Should().BeSameAs(baseSettingsFile);
                Assert.False(priority.MoveNext());
            }
        }

        [Fact]
        public void SettingsFile_MergeSectionsInto_WhenSectionDoNotMatch_AllSectionsAreReturned()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section1>
        <add key='key1' value='value1' />
    </Section1>
    <Section2>
        <add key='key2' value='value2' />
    </Section2>
    <Section3>
        <add key='key3' value='value3' />
    </Section3>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var settingsDict = new Dictionary<string, VirtualSettingSection>() {
                    { "Section4", new VirtualSettingSection("Section4", new AddItem("key4", "value4")) }
                };

                settingsFile.MergeSectionsInto(settingsDict);

                var expectedSettingsDict = new Dictionary<string, VirtualSettingSection>() {
                    { "Section4", new VirtualSettingSection("Section4", new AddItem("key4", "value4")) },
                    { "Section1", new VirtualSettingSection("Section1", new AddItem("key1", "value1")) },
                    { "Section2", new VirtualSettingSection("Section2", new AddItem("key2", "value2")) },
                    { "Section3", new VirtualSettingSection("Section3", new AddItem("key3", "value3")) }
                };

                foreach (var pair in settingsDict)
                {
                    expectedSettingsDict.TryGetValue(pair.Key, out var expectedSection).Should().BeTrue();
                    SettingsTestUtils.DeepEquals(pair.Value, expectedSection!).Should().BeTrue();
                    expectedSettingsDict.Remove(pair.Key);
                }
                expectedSettingsDict.Should().BeEmpty();
            }
        }

        [Fact]
        public void SettingsFile_MergeSectionsInto_WithSectionsInCommon_ReturnsConfigWithAllSectionsMerged()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section1>
        <add key='key1' value='value1' />
    </Section1>
    <Section2>
        <add key='key2' value='value2' />
    </Section2>
    <Section3>
        <add key='key3' value='value3' />
    </Section3>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var settingsDict = new Dictionary<string, VirtualSettingSection>() {
                    { "Section2", new VirtualSettingSection("Section2", new AddItem("keyX", "valueX")) },
                    { "Section3", new VirtualSettingSection("Section3", new AddItem("key3", "valueY")) },
                    { "Section4", new VirtualSettingSection("Section4", new AddItem("key4", "value4")) }
                };

                settingsFile.MergeSectionsInto(settingsDict);

                var expectedSettingsDict = new Dictionary<string, VirtualSettingSection>() {
                    { "Section2", new VirtualSettingSection("Section2", new AddItem("keyX", "valueX"), new AddItem("key2", "value2")) },
                    { "Section3", new VirtualSettingSection("Section3", new AddItem("key3", "value3")) },
                    { "Section4", new VirtualSettingSection("Section4", new AddItem("key4", "value4")) },
                    { "Section1", new VirtualSettingSection("Section1", new AddItem("key1", "value1")) },
                };

                foreach (var pair in settingsDict)
                {
                    expectedSettingsDict.TryGetValue(pair.Key, out var expectedSection).Should().BeTrue();
                    SettingsTestUtils.DeepEquals(pair.Value, expectedSection!).Should().BeTrue();
                    expectedSettingsDict.Remove(pair.Key);
                }
                expectedSettingsDict.Should().BeEmpty();
            }
        }

        [Fact]
        public void SettingsFile_MergeSectionsInto_WithSectionsInCommon_AndClear_ClearsSection()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section1>
        <add key='key1' value='value1' />
    </Section1>
    <Section2>
        <clear />
    </Section2>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var settingsDict = new Dictionary<string, VirtualSettingSection>() {
                    { "Section2", new VirtualSettingSection("Section2", new AddItem("keyX", "valueX")) },
                    { "Section3", new VirtualSettingSection("Section3", new AddItem("key3", "valueY")) },
                };

                settingsFile.MergeSectionsInto(settingsDict);

                var expectedSettingsDict = new Dictionary<string, VirtualSettingSection>() {
                    { "Section2", new VirtualSettingSection("Section2", new ClearItem()) },
                    { "Section3", new VirtualSettingSection("Section3", new AddItem("key3", "valueY")) },
                    { "Section1", new VirtualSettingSection("Section1", new AddItem("key1", "value1")) },
                };

                foreach (var pair in settingsDict)
                {
                    expectedSettingsDict.TryGetValue(pair.Key, out var expectedSection).Should().BeTrue();
                    SettingsTestUtils.DeepEquals(pair.Value, expectedSection!).Should().BeTrue();
                    expectedSettingsDict.Remove(pair.Key);
                }
                expectedSettingsDict.Should().BeEmpty();
            }
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, true)]
        public void SettingsFile_Constructor_MachineWideConfigsAreReadOnly(bool isMachineWide, bool isReadOnlyInput, bool expected)
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
                // Set-up and Act
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: isMachineWide, isReadOnly: isReadOnlyInput);

                // Assert
                settingsFile.IsReadOnly.Should().Be(expected);
                settingsFile.IsMachineWide.Should().Be(isMachineWide);
            }
        }
    }
}
