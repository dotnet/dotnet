// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class SettingsTests
    {
        [Fact]
        public void GetValues_SingleConfigFileWithClear_IgnoresClearedValues()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
        <add key=""key2"" value=""value2"" />
        <clear />
        <add key=""key3"" value=""value3"" />
        <add key=""key4"" value=""value4"" />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                // Act
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                // Assert
                var children = section.Items.ToList();
                children.Count.Should().Be(3);
                SettingsTestUtils.DeepEquals(children[1], new AddItem("key3", "value3")).Should().BeTrue();
                SettingsTestUtils.DeepEquals(children[2], new AddItem("key4", "value4")).Should().BeTrue();
            }
        }

        [Fact]
        public void GetValues_MultipleConfigFilesWithClear_IgnoresClearedValues()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <clear /> <!-- i.e. ignore values from prior conf files -->
        <add key=""key3"" value=""value3"" />
        <add key=""key4"" value=""value4"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, @"dir1\dir2"), config);
                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
        <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config);

                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, @"dir1\dir2"),
                    configFileName: null,
                    machineWideSettings: null);

                // Act
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                // Assert
                var children = section.Items.ToList();
                children.Count.Should().Be(3);

                children[0].Should().BeOfType<ClearItem>();
                SettingsTestUtils.DeepEquals(children[1], new AddItem("key3", "value3")).Should().BeTrue();
                SettingsTestUtils.DeepEquals(children[2], new AddItem("key4", "value4")).Should().BeTrue();
            }
        }

        [Fact]
        public void GetValues_ItemsProvideOriginData()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value3"" />
        <add key=""key3"" value=""value4"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1", "dir2"), config);

                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
        <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config);

                // Act
                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, "dir1", "dir2"),
                    configFileName: null,
                    machineWideSettings: null);

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var parentConfig = Path.Combine(mockBaseDirectory, "dir1", "NuGet.Config");
                var childConfig = Path.Combine(mockBaseDirectory, "dir1", "dir2", "NuGet.Config");

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                item.Origin.ConfigFilePath.Should().Be(childConfig); // key1 was overidden, so it has a new origin!

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key2");
                item.Origin.ConfigFilePath.Should().Be(parentConfig);

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key3");
                item.Origin.ConfigFilePath.Should().Be(childConfig);
            }
        }

        [Fact]
        public void GetValues_ItemsAreCopiesAndSectionsAreAbstract()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value3"" />
        <add key=""key3"" value=""value4"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1", "dir2"), config);

                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
        <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config);

                // Act
                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, "dir1", "dir2"),
                    configFileName: null,
                    machineWideSettings: null);

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();
                section.IsAbstract().Should().BeTrue();

                foreach (var item in section.Items)
                {
                    item.IsCopy().Should().BeTrue();
                }
            }
        }

        [Fact]
        public void GetValues_MultipleConfigFiles_AllDifferentItems_MergeCorrectly()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""value3"" />
        <add key=""key4"" value=""value4"" />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1", "dir2"), config);

                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
        <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config);

                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, "dir1", "dir2"),
                    configFileName: null,
                    machineWideSettings: null);

                // Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                section.Items.Count.Should().Be(4);

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "key1") as AddItem;
                item.Value.Should().Be("value1");

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key2") as AddItem;
                item.Value.Should().Be("value2");

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key3") as AddItem;
                item.Value.Should().Be("value3");

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key4") as AddItem;
                item.Value.Should().Be("value4");
            }
        }

        [Fact]
        public void GetValues_MultipleConfigFiles_WithDupes_MergeCorrectly()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""LastOneWins1"" />
        <add key=""key2"" value=""LastOneWins2"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, @"dir1\dir2"), config);
                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
        <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config);

                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, @"dir1\dir2"),
                    configFileName: null,
                    machineWideSettings: null);

                // Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "key2");
                item.Value.Should().Be("LastOneWins2");

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                item.Value.Should().Be("LastOneWins1");
            }
        }

        [Fact]
        public void GetValues_MultipleConfigFiles_WithClear_MergeCorrectly()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <clear /> <!-- i.e. ignore values from prior conf files -->
        <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, @"dir1\dir2"), config);
                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config);

                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, @"dir1\dir2"),
                    configFileName: null,
                    machineWideSettings: null);

                // Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "key2") as AddItem;
                item.Should().NotBeNull();
                item.Value.Should().Be("value2");

                section.GetFirstItemWithAttribute<AddItem>("key", "key1").Should().BeNull();
            }
        }

        [Theory]
        [InlineData(@"foo\bar")]
        [InlineData(@"..\Blah")]
        public void GetValues_ValueAsPath_ReturnsPathRelativeToConfigWhenPathIsNotRooted(string value)
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""path-key"" value=""{0}"" />
    </SectionName>
</configuration>", value);

                SettingsTestUtils.CreateConfigurationFile("nuget.config", mockBaseDirectory, config);

                var settings = new Settings(mockBaseDirectory, "nuget.config");

                // Act
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "path-key");
                item.Should().NotBeNull();

                var result = item.GetValueAsPath();

                // Assert
                result.Should().Be(new DirectoryInfo(Path.Combine(mockBaseDirectory, value)).FullName);
            }
        }

        [Theory]
        [InlineData(@"z:\foo", "windows")]
        [InlineData(@"x:\foo\bar\qux", "windows")]
        [InlineData(@"\\share\folder\subfolder", "windows")]
        [InlineData(@"/a/b/c", "linux")]
        public void GetValues_ValueAsPath_ReturnsPathWhenPathIsRooted(string value, string os)
        {
            if ((os == "linux" && RuntimeEnvironmentHelper.IsLinux) || (os == "windows" && RuntimeEnvironmentHelper.IsWindows))
            {
                // Arrange
                using (var mockBaseDirectory = TestDirectory.Create())
                {
                    var config = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""path-key"" value=""{0}"" />
    </SectionName>
</configuration>", value);
                    SettingsTestUtils.CreateConfigurationFile("nuget.config", mockBaseDirectory, config);
                    var settings = new Settings(mockBaseDirectory, "nuget.config");

                    // Act
                    var section = settings.GetSection("SectionName");
                    section.Should().NotBeNull();

                    var addItem = section.GetFirstItemWithAttribute<AddItem>("key", "path-key");
                    addItem.Should().NotBeNull();

                    var result = addItem.GetValueAsPath();
                    // Assert
                    result.Should().Be(value);
                }
            }
        }

        [Fact]
        public void GetValues_WithUserSpecifiedDefaultConfigFile_ParsedCorrectly()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""value3"" />
        <add key=""key4"" value=""value4"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "dir1"), config);
                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
        <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("UserDefinedConfigFile.config", Path.Combine(mockBaseDirectory, "dir1", "dir2"), config);

                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, "dir1", "dir2"),
                    configFileName: "UserDefinedConfigFile.config",
                    machineWideSettings: null);

                // Act
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var children = section.Items.ToList();

                // Assert
                children.Count.Should().Be(2);
                SettingsTestUtils.DeepEquals(children[0], new AddItem("key1", "value1")).Should().BeTrue();
                SettingsTestUtils.DeepEquals(children[1], new AddItem("key2", "value2")).Should().BeTrue();
            }
        }

        [Fact]
        public void GetValues_WithMachineWideConfigFile_ParsedCorrectly()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var fileContent1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
    <add key=""key1"" value=""value1"" />
    <add key=""key2"" value=""value2"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent1);
                var fileContent2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
    <add key=""key2"" value=""value3"" />
    <add key=""key3"" value=""value4"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent2);
                var fileContent3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
    <add key=""key3"" value=""user"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "TestingGlobalPath"), fileContent3);

                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(
                    Settings.LoadMachineWideSettings(Path.Combine(mockBaseDirectory, "nuget", "Config"), "IDE", "Version", "SKU"));

                // Act
                var settings = Settings.LoadSettings(
                    root: mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: m.Object,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var addItem = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                addItem.Should().NotBeNull();
                addItem.Value.Should().Be("value1");

                // the value in NuGet\Config\IDE\a1.config overrides the value in
                // NuGet\Config\a1.config
                addItem = section.GetFirstItemWithAttribute<AddItem>("key", "key2");
                addItem.Should().NotBeNull();
                addItem.Value.Should().Be("value3");

                // the value in user.config overrides the value in NuGet\Config\IDE\a1.config
                addItem = section.GetFirstItemWithAttribute<AddItem>("key", "key3");
                addItem.Should().NotBeNull();
                addItem.Value.Should().Be("user");
            }
        }

        [Fact]
        public void GetValues_FromTwoUserSettings_WithMachineWideSettings_ParsedCorrectly()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            using (var mockBaseDirectory1 = TestDirectory.Create())
            using (var mockBaseDirectory2 = TestDirectory.Create())
            {
                var FileContent1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
    </SectionName>
</configuration>";

                var FileContent2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""user1"" />
    </SectionName>
</configuration>";

                var FileContent3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""user2"" />
    </SectionName>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), FileContent1);
                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory1, "TestingGlobalPath"), FileContent2);
                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory2, "TestingGlobalPath"), FileContent3);

                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(
                    Settings.LoadMachineWideSettings(Path.Combine(mockBaseDirectory, "nuget", "Config"), "IDE", "Version", "SKU"));

                // Act
                var settings1 = Settings.LoadSettings(
                    root: mockBaseDirectory1,
                    configFileName: null,
                    machineWideSettings: m.Object,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                var settings2 = Settings.LoadSettings(
                    root: mockBaseDirectory2,
                    configFileName: null,
                    machineWideSettings: m.Object,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Assert
                var section = settings1.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "key3");
                item.Should().NotBeNull();
                item.Value.Should().Be("user1");

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                item.Should().NotBeNull();
                item.Value.Should().Be("value1");

                section = settings2.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key3");
                item.Should().NotBeNull();
                item.Value.Should().Be("user2");

                item = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                item.Should().NotBeNull();
                item.Value.Should().Be("value1");
            }
        }

        [Fact]
        public void AddOrUpdate_WithEmptySectionName_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settings = new Settings(mockBaseDirectory);

                // Act & Assert
                var ex = Record.Exception(() => settings.AddOrUpdate("", new AddItem("SomeKey", "SomeValue")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<ArgumentException>();
            }
        }

        [Fact]
        public void AddOrUpdate_WithNullItem_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settings = new Settings(mockBaseDirectory);

                // Act & Assert
                var ex = Record.Exception(() => settings.AddOrUpdate("SomeKey", item: null));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<ArgumentNullException>();
            }
        }

        [Fact]
        public void AddOrUpdate_SectionThatDoesntExist_WillAddSection()
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
                var settings = new Settings(mockBaseDirectory);

                // Act
                settings.AddOrUpdate("NewSectionName", new AddItem("key", "value"));
                settings.SaveToDisk();

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
                var section = settings.GetSection("NewSectionName");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(1);
            }
        }

        [Fact]
        public void AddOrUpdate_SectionThatExist_WillAddToSection()
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
                var settings = new Settings(mockBaseDirectory);

                // Act
                settings.AddOrUpdate("SectionName", new AddItem("keyTwo", "valueTwo"));
                settings.SaveToDisk();

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
        public void AddOrUpdate_WhenItemExistsInSection_OverrideItem()
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
                var settings = new Settings(mockBaseDirectory);

                // Act
                settings.AddOrUpdate("SectionName", new AddItem("key", "NewValue"));
                settings.SaveToDisk();

                // Assert
                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""NewValue"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, configFile))).Should().Be(result);
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "key");
                item.Should().NotBeNull();
                item.Value.Should().Be("NewValue");
            }
        }

        [Fact]
        public void AddOrUpdate_WithMachineWideSettings_OnlyUpdatesUserSpecific()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var a1Config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""value1"" />
    </SectionName>
</configuration>";

                var a2Config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key2"" value=""value2"" />
        <add key=""key3"" value=""value3"" />
    </SectionName>
</configuration>";

                var userConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""user"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), a1Config);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), a2Config);
                SettingsTestUtils.CreateConfigurationFile("user.config", mockBaseDirectory, userConfig);

                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(
                    Settings.LoadMachineWideSettings(Path.Combine(mockBaseDirectory, "nuget", "Config"), "IDE", "Version", "SKU"));

                var settings = Settings.LoadDefaultSettings(
                    root: mockBaseDirectory,
                    configFileName: "user.config",
                    machineWideSettings: m.Object);

                // Act
                settings.AddOrUpdate("SectionName", new AddItem("key1", "newValue"));
                settings.SaveToDisk();

                // Assert
                var text = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "user.config")));
                var expectedResult = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""user"" />
        <add key=""key1"" value=""newValue"" />
    </SectionName>
</configuration>");

                text.Should().Be(expectedResult);

                text = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "nuget", "Config", "a1.config")));
                SettingsTestUtils.RemoveWhitespace(a1Config).Should().Be(text);

                text = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "a2.config")));
                SettingsTestUtils.RemoveWhitespace(a2Config).Should().Be(text);

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var key1 = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                key1.Should().NotBeNull();

                key1.Value.Should().Be("newValue");
            }
        }

        [Fact]
        public void AddOrUpdate_WithMultipleSections_ClearedInDifferentConfigs_AddsItemInFurthestCompatibleConfig()
        {
            var configFile = "NuGet.Config";

            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userConfigFurthest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <Section1>
        <clear />
        <add key=""key11"" value=""value11"" />
    </Section1>
    <Section2>
        <add key=""key12"" value=""value12"" />
    </Section2>
    <Section3>
        <add key=""key13"" value=""value13"" />
    </Section3>
</configuration>";

                var userConfigMiddle = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <Section1>
        <add key=""key21"" value=""value21"" />
    </Section1>
    <Section2>
        <clear />
        <add key=""key22"" value=""value22"" />
    </Section2>
    <Section3>
        <add key=""key23"" value=""value23"" />
    </Section3>
</configuration>";

                var userConfigClosest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <Section1>
        <add key=""key31"" value=""value31"" />
    </Section1>
    <Section2>
        <add key=""key32"" value=""value32"" />
    </Section2>
    <Section3>
        <clear />
        <add key=""key33"" value=""value33"" />
    </Section3>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile(configFile, Path.Combine(mockBaseDirectory, "d1", "d2"), userConfigClosest);
                SettingsTestUtils.CreateConfigurationFile(configFile, Path.Combine(mockBaseDirectory, "d1"), userConfigMiddle);
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, userConfigFurthest);
                var settings = Settings.LoadSettings(
                    root: Path.Combine(mockBaseDirectory, "d1", "d2"),
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Act
                settings.AddOrUpdate("Section1", new AddItem("newKey1", "newValue"));
                settings.AddOrUpdate("Section2", new AddItem("newKey2", "newValue"));
                settings.AddOrUpdate("Section3", new AddItem("newKey3", "newValue"));
                settings.AddOrUpdate("SectionN", new AddItem("newKeyN", "newValue"));
                settings.SaveToDisk();

                // Assert
                var actualFurthestConfig = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, configFile)));
                var actualMiddleConfig = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "d1", configFile)));
                var actualClosestConfig = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "d1", "d2", configFile)));
                var actualTestingGlobalConfig = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "d1", "d2", "TestingGlobalPath", configFile)));

                var expectedFurthestConfig = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <Section1>
        <clear />
        <add key=""key11"" value=""value11"" />
        <add key=""newKey1"" value=""newValue"" />
    </Section1>
    <Section2>
        <add key=""key12"" value=""value12"" />
    </Section2>
    <Section3>
        <add key=""key13"" value=""value13"" />
    </Section3>
</configuration>");

                var expectedMiddleConfig = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <Section1>
        <add key=""key21"" value=""value21"" />
    </Section1>
    <Section2>
        <clear />
        <add key=""key22"" value=""value22"" />
        <add key=""newKey2"" value=""newValue"" />
    </Section2>
    <Section3>
        <add key=""key23"" value=""value23"" />
    </Section3>
</configuration>");

                var expectedClosestConfig = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <Section1>
        <add key=""key31"" value=""value31"" />
    </Section1>
    <Section2>
        <add key=""key32"" value=""value32"" />
    </Section2>
    <Section3>
        <clear />
        <add key=""key33"" value=""value33"" />
        <add key=""newKey3"" value=""newValue"" />
    </Section3>
</configuration>");

                var expectedTestingGlobalConfig = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""nuget.org"" value=""https://api.nuget.org/v3/index.json"" protocolVersion=""3"" />
    </packageSources>
    <SectionN>
        <add key=""newKeyN"" value=""newValue"" />
    </SectionN>
</configuration>");

                actualFurthestConfig.Should().Be(expectedFurthestConfig);
                actualMiddleConfig.Should().Be(expectedMiddleConfig);
                actualClosestConfig.Should().Be(expectedClosestConfig);
                actualTestingGlobalConfig.Should().Be(expectedTestingGlobalConfig);

                var section = settings.GetSection("Section1");
                section.Should().NotBeNull();

                var key1 = section.GetFirstItemWithAttribute<AddItem>("key", "newKey1");
                key1.Should().NotBeNull();
                key1.Value.Should().Be("newValue");

                section = settings.GetSection("Section2");
                section.Should().NotBeNull();

                key1 = section.GetFirstItemWithAttribute<AddItem>("key", "newKey2");
                key1.Should().NotBeNull();
                key1.Value.Should().Be("newValue");

                section = settings.GetSection("Section3");
                section.Should().NotBeNull();

                key1 = section.GetFirstItemWithAttribute<AddItem>("key", "newKey3");
                key1.Should().NotBeNull();
                key1.Value.Should().Be("newValue");

                section = settings.GetSection("SectionN");
                section.Should().NotBeNull();

                key1 = section.GetFirstItemWithAttribute<AddItem>("key", "newKeyN");
                key1.Should().NotBeNull();
                key1.Value.Should().Be("newValue");
            }
        }

        [Fact]
        public void AddOrUpdate_PreserveComments()
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
                var settings = new Settings(mockBaseDirectory);

                // Act & Assert
                settings.AddOrUpdate("SectionName", new AddItem("newKey", "value"));
                settings.SaveToDisk();

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

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "newKey");
                item.Should().NotBeNull();
                item.Value.Should().Be("value");
            }
        }

        [Fact]
        public void AddOrUpdate_PreserveUnknownItems()
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
                var settings = new Settings(mockBaseDirectory);

                // Act & Assert
                settings.AddOrUpdate("SectionName", new AddItem("newKey", "value"));
                settings.SaveToDisk();

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

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "newKey");
                item.Should().NotBeNull();
                item.Value.Should().Be("value");
            }
        }

        [Fact]
        public void AddOrUpdate_WithSpecificConfig_WithEmptySectionName_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act & Assert
                var ex = Record.Exception(() => settings.AddOrUpdate(settingsFile, "", new AddItem("SomeKey", "SomeValue")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<ArgumentException>();
            }
        }

        [Fact]
        public void AddOrUpdate_WithSpecificConfig_WithNullItem_Throws()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act & Assert
                var ex = Record.Exception(() => settings.AddOrUpdate(settingsFile, "SomeKey", item: null));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<ArgumentNullException>();
            }
        }

        [Fact]
        public void AddOrUpdate_WithSpecificConfig_WithMachineWideSettings_Throws()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), @"<configuration></configuration>");
                var settingsFile = new SettingsFile(Path.Combine(mockBaseDirectory, "nuget", "Config"), "a1.config", isMachineWide: true, isReadOnly: false);
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act
                var ex = Record.Exception(() => settings.AddOrUpdate(settingsFile, "section", new AddItem("SomeKey", "SomeValue")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();

            }
        }

        [Fact]
        public void AddOrUpdate_WithSpecificConfig_SectionThatDoesntExist_WillAddSection()
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
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act
                settings.AddOrUpdate(settingsFile, "NewSectionName", new AddItem("key", "value"));
                settings.SaveToDisk();

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
                var section = settings.GetSection("NewSectionName");
                section.Should().NotBeNull();
                section.Items.Count.Should().Be(1);
            }
        }

        [Fact]
        public void AddOrUpdate_WithSpecificConfig_SectionThatExist_WillAddToSection()
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
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act
                settings.AddOrUpdate(settingsFile, "SectionName", new AddItem("keyTwo", "valueTwo"));
                settings.SaveToDisk();

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
        public void AddOrUpdate_WithSpecificConfig_WhenItemExistsInSection_OverrideItem()
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
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act
                settings.AddOrUpdate(settingsFile, "SectionName", new AddItem("key", "NewValue"));
                settings.SaveToDisk();

                // Assert
                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""NewValue"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, configFile))).Should().Be(result);
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "key");
                item.Should().NotBeNull();
                item.Value.Should().Be("NewValue");
            }
        }

        [Fact]
        public void AddOrUpdate_WithSpecificConfig_PreserveComments()
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
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act & Assert
                settings.AddOrUpdate(settingsFile, "SectionName", new AddItem("newKey", "value"));
                settings.SaveToDisk();

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

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "newKey");
                item.Should().NotBeNull();
                item.Value.Should().Be("value");
            }
        }

        [Fact]
        public void AddOrUpdate_WithSpecificConfig_PreserveUnknownItems()
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
                var settings = new Settings(new SettingsFile[] { settingsFile });

                // Act & Assert
                settings.AddOrUpdate(settingsFile, "SectionName", new AddItem("newKey", "value"));
                settings.SaveToDisk();

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

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "newKey");
                item.Should().NotBeNull();
                item.Value.Should().Be("value");
            }
        }

        [Fact]
        public void Remove_MachineWide_Throws()
        {
            // Arrange
            var config1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value1"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), config1);

                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(
                    Settings.LoadMachineWideSettings(Path.Combine(mockBaseDirectory, "nuget", "Config")));

                var settings = Settings.LoadDefaultSettings(
                    root: mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: m.Object);

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value1");

                var ex = Record.Exception(() => settings.Remove("SectionName", item));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("Unable to update setting since it is in a machine-wide NuGet.Config.");

                settings.SaveToDisk();

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "nuget", "Config", "a1.config"))).Should().Be(SettingsTestUtils.RemoveWhitespace(config1));

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value1");
            }
        }

        [Fact]
        public void Remove_LastValueInSectionOfComputedValues_RemovesSection()
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

                var settings = new Settings(root: Path.Combine(mockBaseDirectory));

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value2");

                settings.Remove("SectionName", item);
                settings.SaveToDisk();

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?><configuration></configuration>"));

                section = settings.GetSection("SectionName");
                section.Should().BeNull();
            }
        }

        [Fact]
        public void Remove_LastValueInOneSpecificConfig_RemovesSectionInThatConfig_DoesNotRemoveSectionInComputedValues()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value1"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            var config2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value2"" />
    </SectionName>
</configuration>";

            var config3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value3"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";


            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config1);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config2);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1", "dir2"), config3);

                var settings = Settings.LoadDefaultSettings(root: Path.Combine(mockBaseDirectory, "dir1", "dir2"));

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value3");

                settings.Remove("SectionName", item);
                settings.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", "dir2", nugetConfigPath))).Should().Be(result);
                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", nugetConfigPath))).Should().Be(SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?><configuration></configuration>"));

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void Remove_RemovesAllMergedValuesInAllConfigs_ExceptMachineWide()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value1"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            var config2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value2"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            var config3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value3"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";


            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), config1);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config2);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1", "dir2"), config3);

                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(
                    Settings.LoadMachineWideSettings(Path.Combine(mockBaseDirectory, "nuget", "Config")));

                var settings = Settings.LoadDefaultSettings(
                    root: Path.Combine(mockBaseDirectory, "dir1", "dir2"),
                    configFileName: null,
                    machineWideSettings: m.Object);

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value3");

                settings.Remove("SectionName", item);
                settings.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", "dir2", nugetConfigPath))).Should().Be(result);
                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", nugetConfigPath))).Should().Be(result);
                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "nuget", "Config", "a1.config"))).Should().Be(SettingsTestUtils.RemoveWhitespace(config1));

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value1");
            }
        }

        [Fact]
        public void Remove_RemovesAllMergedValuesInAllConfigs()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value1"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            var config2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value2"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            var config3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value3"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config1);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config2);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1", "dir2"), config3);

                var settings = Settings.LoadDefaultSettings(Path.Combine(mockBaseDirectory, "dir1", "dir2"));

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value3");

                settings.Remove("SectionName", item);
                settings.SaveToDisk();

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", "dir2", nugetConfigPath))).Should().Be(result);
                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", nugetConfigPath))).Should().Be(result);
                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void Remove_RemovesAllMergedValuesInAllConfigsAfterClear()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value1"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            var config2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <clear />
        <add key=""DeleteMe"" value=""value2"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";

            var config3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""DeleteMe"" value=""value3"" />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>";


            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config1);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1"), config2);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "dir1", "dir2"), config3);

                var settings = Settings.LoadDefaultSettings(Path.Combine(mockBaseDirectory, "dir1", "dir2"));

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().NotBeNull();
                item.Value.Should().Be("value3");

                settings.Remove("SectionName", item);
                settings.SaveToDisk();

                var result1 = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>");

                var result2 = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <clear />
        <add key=""keyNotToDelete"" value=""value"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", "dir2", nugetConfigPath))).Should().Be(result1);
                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "dir1", nugetConfigPath))).Should().Be(result2);
                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(SettingsTestUtils.RemoveWhitespace(config1));

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void Remove_WithValidSectionAndKey_DeletesTheEntry()
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
                var settings = new Settings(mockBaseDirectory);

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<SettingItem>("key", "DeleteMe");
                settings.Remove("SectionName", item);
                settings.SaveToDisk();

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

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void Remove_PreserveComments()
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
                var settings = new Settings(mockBaseDirectory);

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<SettingItem>("key", "DeleteMe");
                settings.Remove("SectionName", item);
                settings.SaveToDisk();

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

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        [Fact]
        public void Remove_PreserveUnknownItems()
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
                var settings = new Settings(mockBaseDirectory);

                // Act & Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item = section.GetFirstItemWithAttribute<SettingItem>("key", "DeleteMe");
                settings.Remove("SectionName", item);
                settings.SaveToDisk();

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

                section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                item = section.GetFirstItemWithAttribute<AddItem>("key", "DeleteMe");
                item.Should().BeNull();
            }
        }

        // Checks that the correct files are read, in the right order,
        // when laoding machine wide settings.
        [Fact]
        public void LoadMachineWideSettings_ReadsFilesCorrectly()
        {
            // Arrange
            var fileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <configuration>
          <SectionName>
            <add key=""key"" value=""value"" />
          </SectionName>
        </configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a3.xconfig", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a3.xconfig", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a3.xconfig", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a3.xconfig", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "Dir"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1_uppercase.Config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);

                // Act
                var settings = Settings.LoadMachineWideSettings(
                    Path.Combine(mockBaseDirectory, "nuget", "Config"), "IDE", "Version", "SKU", "TestDir");

                // Assert
                var files = settings.GetConfigFilePaths();

                files.Count().Should().Be(9);
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "a2.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "a1.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "a2.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "a1.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "a2.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "a1.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "a2.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "a1.config"));
                files.Should().Contain(Path.Combine(mockBaseDirectory, "nuget", "Config", "a1_uppercase.Config"));
            }
        }

        // Tests that when configFileName is not null, the specified
        // file must exist.
        [Fact]
        public void LoadDefaultSettings_WithUnexistantUserSpecifiedConfigFile_Throws()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Act and assert
                var ex = Record.Exception(() => Settings.LoadDefaultSettings(
                    mockBaseDirectory,
                    configFileName: "user.config",
                    machineWideSettings: null));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be(string.Format(@"File '{0}' does not exist.", Path.Combine(mockBaseDirectory, "user.config")));
            }
        }

        // Tests that when configFileName is not null, machineWideSettings and
        // NuGet.Config files in base directory ancestors are ignored.
        [Fact]
        public void LoadDefaultSettings_WithUserSpecifiedConfigFile_IgnoresOtherSettings()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var environmentFileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""environment"" value=""true"" />
    </SectionName>
</configuration>";

                var machineConfigFileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""machine"" value=""true"" />
    </SectionName>
</configuration>";

                var userFileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""user"" value=""true"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "a", "b"), environmentFileContent);
                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "a"), environmentFileContent);
                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory), environmentFileContent);

                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "machine"), machineConfigFileContent);

                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "nuget"), userFileContent);

                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(
                    Settings.LoadMachineWideSettings(Path.Combine(mockBaseDirectory, "machine")));

                // Act and assert
                var settings = Settings.LoadDefaultSettings(
                    Path.Combine(mockBaseDirectory, "a", "b"),
                    Path.Combine(mockBaseDirectory, "nuget", "NuGet.Config"),
                    m.Object);

                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var machineValue = section.GetFirstItemWithAttribute<AddItem>("key", "machine");
                machineValue.Should().BeNull();

                var environmentValue = section.GetFirstItemWithAttribute<AddItem>("key", "environment");
                environmentValue.Should().BeNull();

                var userFileValue = section.GetFirstItemWithAttribute<AddItem>("key", "user");
                userFileValue.Should().NotBeNull();
                userFileValue.Value.Should().Be("true");
            }
        }

        [Fact]
        public void LoadSettings_EmptyUserWideConfigFile_DoNotAddNuGetOrg()
        {
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Arrange
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";

                var nugetConfigPath = "NuGet.Config";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(mockBaseDirectory, "TestingGlobalPath"), config);

                // Act
                var settings = Settings.LoadSettings(
                    root: mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Assert
                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, "TestingGlobalPath", "NuGet.Config")));
                var expected = SettingsTestUtils.RemoveWhitespace(config);

                actual.Should().Be(expected);
            }
        }

        [Fact]
        public void LoadSettings_NonExistingUserWideConfigFile_CreateUserWideConfigFileWithNuGetOrg()
        {
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Arrange
                var nugetConfigPath = Path.Combine(mockBaseDirectory, "TestingGlobalPath", "NuGet.Config");
                File.Exists(nugetConfigPath).Should().BeFalse();

                // Act
                var settings = Settings.LoadSettings(
                    root: mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Assert
                File.Exists(nugetConfigPath).Should().BeTrue();
                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(nugetConfigPath));
                var expected = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""nuget.org"" value=""https://api.nuget.org/v3/index.json"" protocolVersion=""3"" />
    </packageSources>
</configuration>");

                actual.Should().Be(expected);
            }
        }

        [Theory]
        [InlineData(@"D:\", @"C:\Users\SomeUsers\AppData\Roaming\nuget\nuget.config", @"C:\Users\SomeUsers\AppData\Roaming\nuget", @"nuget.config", "windows")]
        [InlineData(@"D:\", (string)null, @"D:\", (string)null, "windows")]
        [InlineData(@"D:\", "nuget.config", @"D:\", "nuget.config", "windows")]
        [InlineData(@"/Root", @"/Home/Users/nuget/nuget.config", @"/Home/Users/nuget", @"nuget.config", "linux")]
        [InlineData(@"/", (string)null, @"/", (string)null, "linux")]
        [InlineData(@"/", "nuget.config", @"/", "nuget.config", "linux")]
        public void GetFileNameAndItsRoot_ParsesPathsCorrectly(string root, string settingsPath, string expectedRoot, string expectedFileName, string os)
        {
            if ((os == "linux" && RuntimeEnvironmentHelper.IsLinux) || (os == "windows" && RuntimeEnvironmentHelper.IsWindows))
            {
                // Act
                var tuple = Settings.GetFileNameAndItsRoot(root, settingsPath);

                // Assert
                tuple.Item1.Should().Be(expectedFileName);
                tuple.Item2.Should().Be(expectedRoot);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void NuGetEnviromentPath_OnWindows_ReturnsCorrectPath()
        {
            // Arrange
#if IS_CORECLR
            var programFilesX86Data = Environment.GetEnvironmentVariable("PROGRAMFILES(X86)");

            if (string.IsNullOrEmpty(programFilesX86Data))
            {
                programFilesX86Data = Environment.GetEnvironmentVariable("PROGRAMFILES");
            }
            var userSetting = Environment.GetEnvironmentVariable("APPDATA");
#else
            var programFilesX86Data = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            if (string.IsNullOrEmpty(programFilesX86Data))
            {
                programFilesX86Data = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            }
            var userSetting = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif

            // Act
            var machineWidePath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.MachineWideSettingsBaseDirectory), "NuGet.Config");
            var globalConfigPath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory), "NuGet.Config");
            var machineWidePathTuple = Settings.GetFileNameAndItsRoot("test root", machineWidePath);
            var globalConfigTuple = Settings.GetFileNameAndItsRoot("test root", globalConfigPath);

            // Assert
            machineWidePathTuple.Item2.Should().Be(Path.Combine(programFilesX86Data, "NuGet"));
            machineWidePathTuple.Item1.Should().Be("NuGet.Config");
            globalConfigTuple.Item2.Should().Be(Path.Combine(userSetting, "NuGet"));
            globalConfigTuple.Item1.Should().Be("NuGet.Config");
        }

        [PlatformFact(Platform.Darwin)]
        public void NuGetEnviromentPath_OnMac_ReturnsCorrectPath()
        {
            // Arrange
#if IS_CORECLR
            var commonApplicationData = @"/Library/Application Support";
            var userSetting = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".nuget");
#else
            var commonApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var userSetting = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif

            // Act
            var machineWidePath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.MachineWideSettingsBaseDirectory), "NuGet.Config");
            var globalConfigPath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory), "NuGet.Config");
            var machineWidePathTuple = Settings.GetFileNameAndItsRoot("test root", machineWidePath);
            var globalConfigTuple = Settings.GetFileNameAndItsRoot("test root", globalConfigPath);

            // Assert
            machineWidePathTuple.Item2.Should().Be(Path.Combine(commonApplicationData, "NuGet"));
            machineWidePathTuple.Item1.Should().Be("NuGet.Config");
            globalConfigTuple.Item2.Should().Be(Path.Combine(userSetting, "NuGet"));
            globalConfigTuple.Item1.Should().Be("NuGet.Config");
        }

        [PlatformFact(Platform.Linux)]
        public void NuGetEnviromentPath_OnLinux_ReturnsCorrectPath()
        {
            // Arrange
#if IS_CORECLR
            var commonApplicationData = @"/etc/opt";
            var userSetting = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".nuget");
#else
            var commonApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var userSetting = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif

            // Act
            var machineWidePath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.MachineWideSettingsBaseDirectory), "NuGet.Config");
            var globalConfigPath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory), "NuGet.Config");
            var machineWidePathTuple = Settings.GetFileNameAndItsRoot("test root", machineWidePath);
            var globalConfigTuple = Settings.GetFileNameAndItsRoot("test root", globalConfigPath);

            // Assert
            machineWidePathTuple.Item2.Should().Be(Path.Combine(commonApplicationData, "NuGet"));
            machineWidePathTuple.Item1.Should().Be("NuGet.Config");
            globalConfigTuple.Item2.Should().Be(Path.Combine(userSetting, "NuGet"));
            globalConfigTuple.Item1.Should().Be("NuGet.Config");
        }

        [Fact]
        public void GetConfigFilePaths_ReadsFilesCorrectly()
        {
            // Arrange
            var fileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <configuration>
          <SectionName>
            <add key=""key"" value=""value"" />
          </SectionName>
        </configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "Dir"), fileContent);

                var configPath1 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "Dir", "a1.config");
                var configPath2 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "a1.config");
                var configPath3 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "a2.config");
                var configPath4 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "a1.config");
                var configPath5 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "a2.config");
                var configPath6 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "a1.config");
                var configPath7 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "a2.config");
                var configPath8 = Path.Combine(mockBaseDirectory, "nuget", "Config", "a1.config");
                var configPath9 = Path.Combine(mockBaseDirectory, "nuget", "Config", "a2.config");
                var configPath10 = Path.Combine(mockBaseDirectory, "nuget", "a1.config");

                // Act
                var settings = Settings.LoadSettingsGivenConfigPaths(new List<string>() { configPath1, configPath2, configPath3,
                                                                                          configPath4, configPath5, configPath6,
                                                                                          configPath7, configPath8, configPath9,
                                                                                          configPath10 });

                // Assert
                var files = settings.GetConfigFilePaths();

                files.Count().Should().Be(10);
                files.Should().Contain(configPath1);
                files.Should().Contain(configPath2);
                files.Should().Contain(configPath3);
                files.Should().Contain(configPath4);
                files.Should().Contain(configPath5);
                files.Should().Contain(configPath6);
                files.Should().Contain(configPath7);
                files.Should().Contain(configPath8);
                files.Should().Contain(configPath9);
                files.Should().Contain(configPath10);
            }
        }

        [Fact]
        public void GetConfigFilePaths_SettingsWithoutFiles_ReturnEmptyList()
        {
            var settings = new Settings(new List<SettingsFile>());

            var configFilePaths = settings.GetConfigFilePaths();

            configFilePaths.Should().NotBeNull();
            configFilePaths.Should().BeEmpty();
        }

        [Fact]
        public void GetConfigRoots_ReadsFilesCorrectly()
        {
            // Arrange
            var fileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <configuration>
          <SectionName>
            <add key=""key"" value=""value"" />
          </SectionName>
        </configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a2.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU"), fileContent);
                SettingsTestUtils.CreateConfigurationFile("a1.config", Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "Dir"), fileContent);

                var configPath1 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "Dir", "a1.config");
                var configPath2 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "a1.config");
                var configPath3 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "a2.config");
                var configPath4 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "a1.config");
                var configPath5 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "a2.config");
                var configPath6 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "a1.config");
                var configPath7 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "a2.config");
                var configPath8 = Path.Combine(mockBaseDirectory, "nuget", "Config", "a1.config");
                var configPath9 = Path.Combine(mockBaseDirectory, "nuget", "Config", "a2.config");
                var configPath10 = Path.Combine(mockBaseDirectory, "nuget", "a1.config");

                // Act
                var settings = Settings.LoadSettingsGivenConfigPaths(new List<string>() { configPath1, configPath2, configPath3,
                                                                                          configPath4, configPath5, configPath6,
                                                                                          configPath7, configPath8, configPath9,
                                                                                          configPath10 });


                var configRoot1 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU", "Dir");
                var configRoot2 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version", "SKU");
                var configRoot3 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE", "Version");
                var configRoot4 = Path.Combine(mockBaseDirectory, "nuget", "Config", "IDE");
                var configRoot5 = Path.Combine(mockBaseDirectory, "nuget", "Config");
                var configRoot6 = Path.Combine(mockBaseDirectory, "nuget");

                // Assert
                var files = settings.GetConfigRoots();

                files.Count().Should().Be(6);
                files.Should().Contain(configRoot1);
                files.Should().Contain(configRoot2);
                files.Should().Contain(configRoot3);
                files.Should().Contain(configRoot4);
                files.Should().Contain(configRoot5);
                files.Should().Contain(configRoot6);
            }
        }

        [Fact]
        public void GetConfigRoots_SettingsWithoutFiles_ReturnEmptyList()
        {
            var settings = new Settings(new List<SettingsFile>());

            var configRoots = settings.GetConfigRoots();

            configRoots.Should().NotBeNull();
            configRoots.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", null)]
        [InlineData("a", "b", null)]
        [InlineData(null, null, "")]
        [InlineData("", "", "")]
        [InlineData("a", "b", "")]
        public void ResolvePathFromOrigin_WhenPathIsNullOrEmpty_ReturnsPath(string originDirectoryPath, string originFilePath, string path)
        {
            var resolvedPath = Settings.ResolvePathFromOrigin(originDirectoryPath, originFilePath, path);

            Assert.Equal(path, resolvedPath);
        }

#if IS_DESKTOP
        // The .NET Core implementation of System.IO.Path.GetPathRoot(...) never throws.
        [Fact]
        public void ResolvePathFromOrigin_WhenPathIsInvalidRelativeFileSystemPath_Throws()
        {
            string originDirectoryPath = GetOriginDirectoryPath();
            string originFilePath = Path.Combine(originDirectoryPath, "b.c");

            const string path = "|";

            ResourceManager resourceManager = new ResourceManager("NuGet.Configuration.Resources", typeof(Resources).Assembly);
            var errorString = resourceManager.GetString("ShowError_ConfigHasInvalidPackageSource", CultureInfo.CurrentCulture);
            var expectedErrorMessage = string.Format(errorString, NuGetLogCode.NU1006, path, "");

            var exception = Assert.Throws<NuGetConfigurationException>(
                () => Settings.ResolvePathFromOrigin(originDirectoryPath, originFilePath, path));
            Assert.Contains(expectedErrorMessage, exception.Message);
        }
#endif

        [Fact]
        public void ResolvePathFromOrigin_WhenPathIsValidRelativeFileSystemPath_ReturnsResolvedPath()
        {
            string originDirectoryPath = GetOriginDirectoryPath();
            string originFilePath = Path.Combine(originDirectoryPath, "b.c");

            const string path = "d";

            string resolvedPath = Settings.ResolvePathFromOrigin(originDirectoryPath, originFilePath, path);

            Assert.Equal(Path.Combine(originDirectoryPath, path), resolvedPath);
        }

        [Fact]
        public void LoadImmutableSettingsGivenConfigPaths_MergesConfigsInCorrectOrder()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""a"" />
    </SectionName>
</configuration>";
                var subDir = Path.Combine(mockBaseDirectory, "a");
                var configAPath = Path.Combine(subDir, nugetConfigPath);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);

                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""b"" />
    </SectionName>
</configuration>";
                subDir = Path.Combine(mockBaseDirectory, "b");
                var configBPath = Path.Combine(subDir, nugetConfigPath);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);

                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""c"" />
    </SectionName>
</configuration>";
                subDir = Path.Combine(mockBaseDirectory, "c");
                var configCPath = Path.Combine(subDir, nugetConfigPath);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);

                var settingsLoadContext = new SettingsLoadingContext();

                // Act
                var settings = Settings.LoadImmutableSettingsGivenConfigPaths(new string[] { configAPath, configBPath }, settingsLoadContext);
                // Assert
                var section = settings.GetSection("SectionName");
                Assert.Equal(1, section.Items.Count);
                Assert.Equal("a", ((AddItem)section.Items.First()).Value);

                // Act
                settings = Settings.LoadImmutableSettingsGivenConfigPaths(new string[] { configCPath, configBPath }, settingsLoadContext);
                // Assert
                section = settings.GetSection("SectionName");
                Assert.Equal(1, section.Items.Count);
                Assert.Equal("c", ((AddItem)section.Items.First()).Value);

                // Act
                settings = Settings.LoadImmutableSettingsGivenConfigPaths(new string[] { configBPath, configCPath }, settingsLoadContext);
                // Assert
                section = settings.GetSection("SectionName");
                Assert.Equal(1, section.Items.Count);
                Assert.Equal("b", ((AddItem)section.Items.First()).Value);
            }
        }

        [Fact]
        public void LoadImmutableSettingsGivenConfigPaths_CachesConfigs()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""a"" />
    </SectionName>
</configuration>";
                var subDir = Path.Combine(mockBaseDirectory, "a");
                var configAPath = Path.Combine(subDir, nugetConfigPath);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);


                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""b"" />
    </SectionName>
</configuration>";
                subDir = Path.Combine(mockBaseDirectory, "b");
                var configBPath = Path.Combine(subDir, nugetConfigPath);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);

                // Act
                var settings = Settings.LoadImmutableSettingsGivenConfigPaths(new string[] { configAPath, configBPath }, new SettingsLoadingContext());
                // Assert
                var section = settings.GetSection("SectionName");
                Assert.Equal(1, section.Items.Count);
                Assert.Equal("a", ((AddItem)section.Items.First()).Value);

                // Change the value of config A...basically this ensures we get the cached version, since there's no way to ensure that the same SettingsFile was returned.

                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""new"" />
    </SectionName>
</configuration>";
                subDir = Path.Combine(mockBaseDirectory, "a");
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);

                settings = Settings.LoadImmutableSettingsGivenConfigPaths(new string[] { configAPath, configBPath }, new SettingsLoadingContext());
                section = settings.GetSection("SectionName");
                Assert.Equal(1, section.Items.Count);
                Assert.Equal("new", ((AddItem)section.Items.First()).Value);
            }
        }

        [Fact]
        public void LoadImmutableSettingsGivenConfigPaths_ImmutableSettigns_ThrowForNotSupportedOperations()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""a"" />
    </SectionName>
</configuration>";
                var subDir = Path.Combine(mockBaseDirectory, "a");
                var configAPath = Path.Combine(subDir, nugetConfigPath);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);


                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""b"" />
    </SectionName>
</configuration>";
                subDir = Path.Combine(mockBaseDirectory, "b");
                var configBPath = Path.Combine(subDir, nugetConfigPath);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subDir, config);

                // Act
                var settings = Settings.LoadImmutableSettingsGivenConfigPaths(new string[] { configAPath, configBPath }, new SettingsLoadingContext());

                // Assert
                Assert.Throws<NotSupportedException>(() => settings.AddOrUpdate("name", new AddItem("key", "value")));
                Assert.Throws<NotSupportedException>(() => settings.Remove("name", new AddItem("key", "value")));
                Assert.Throws<NotSupportedException>(() => settings.SaveToDisk());
            }
        }

        /// <summary>
        /// We have 3 configs, one in the working directory, 2 in the user directory.
        /// One of those is the default one and 1 is the additional config dropped in the directory.
        /// The default config takes priority over the additional ones, so it's expected that values from the additional config are overwritten by the default ones.
        /// </summary>
        [Fact]
        public void LoadSettings_WithAdditionalUserSpecificConfigs_ParsesInCorrectOrder()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var fileContentLocal = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
    <add key=""key1"" value=""local"" />
    <add key=""key2"" value=""local"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.config", mockBaseDirectory, fileContentLocal);

                var fileContentUser = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key2"" value=""user"" />
        <add key=""key3"" value=""user"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "TestingGlobalPath"), fileContentUser);

                var additionalUserConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""additional"" />
        <add key=""key4"" value=""additional"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Contoso.Config", Path.Combine(mockBaseDirectory, "TestingGlobalPath", "config"), additionalUserConfig);

                // Act
                var settings = Settings.LoadSettings(
                    root: mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: new XPlatMachineWideSetting(),
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Assert
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item1 = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                item1.Should().NotBeNull();
                item1.Value.Should().Be("local");

                var item2 = section.GetFirstItemWithAttribute<AddItem>("key", "key2");
                item2.Should().NotBeNull();
                item2.Value.Should().Be("local");

                var item3 = section.GetFirstItemWithAttribute<AddItem>("key", "key3");
                item3.Should().NotBeNull();
                item3.Value.Should().Be("user");

                var item4 = section.GetFirstItemWithAttribute<AddItem>("key", "key4");
                item4.Should().NotBeNull();
                item4.Value.Should().Be("additional");
            }
        }

        /// <summary>
        /// We have 3 configs, one in the working directory, 2 in the user directory.
        /// One of those is the default one and 1 is the additional config dropped in the directory.
        /// The default config takes priority over the additional ones, so it's expected that values from the additional config are overwritten by the default ones.
        /// </summary>
        [Fact]
        public void LoadSettings_WithAdditonalConfig_And_WithoutDefaultUserConfig_CreatesDefaultNuGetConfig()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var fileContentLocal = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
    <add key=""key1"" value=""local"" />
    <add key=""key2"" value=""local"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.config", mockBaseDirectory, fileContentLocal);

                var additionalUserConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""additional"" />
        <add key=""key4"" value=""additional"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Contoso.Config", Path.Combine(mockBaseDirectory, "TestingGlobalPath", "config"), additionalUserConfig);

                // Act
                var settings = Settings.LoadSettings(
                    root: mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: new XPlatMachineWideSetting(),
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Assert
                // The default config should still be created.
                File.Exists(Path.Combine(mockBaseDirectory, "TestingGlobalPath", "NuGet.Config")).Should().BeTrue();

                // Ensure the configs are merged correctly.
                var section = settings.GetSection("SectionName");
                section.Should().NotBeNull();

                var item1 = section.GetFirstItemWithAttribute<AddItem>("key", "key1");
                item1.Should().NotBeNull();
                item1.Value.Should().Be("local");

                var item2 = section.GetFirstItemWithAttribute<AddItem>("key", "key2");
                item2.Should().NotBeNull();
                item2.Value.Should().Be("local");

                var item3 = section.GetFirstItemWithAttribute<AddItem>("key", "key3");
                item3.Should().NotBeNull();
                item3.Value.Should().Be("additional");

                var item4 = section.GetFirstItemWithAttribute<AddItem>("key", "key4");
                item4.Should().NotBeNull();
                item4.Value.Should().Be("additional");
            }
        }

        /// <summary>
        /// We have 3 configs, one in the working directory, 2 in the user directory.
        /// We always write to the furthest compatible write-able config. While the additional config is further, it's also not write-able.
        /// </summary>
        [Fact]
        public void AddOrUpdate_WithAdditionalUserSpecificConfigs_AddsToFurthestUserWideConfig()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var fileContentLocal = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key1"" value=""local"" />
        <add key=""key2"" value=""local"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.config", mockBaseDirectory, fileContentLocal);

                var fileContentUser = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key2"" value=""user"" />
        <add key=""key3"" value=""user"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Config", Path.Combine(mockBaseDirectory, "TestingGlobalPath"), fileContentUser);

                var additionalUserConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key3"" value=""additional"" />
        <add key=""key4"" value=""additional"" />
    </SectionName>
</configuration>";

                SettingsTestUtils.CreateConfigurationFile("NuGet.Contoso.Config", Path.Combine(mockBaseDirectory, "TestingGlobalPath"), additionalUserConfig);

                // Act
                var settings = Settings.LoadSettings(
                    root: mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: new XPlatMachineWideSetting(),
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Act
                settings.AddOrUpdate("SectionName", new AddItem("newKey", "newValue"));
                settings.SaveToDisk();

                // Assert
                var expectedPath = Path.Combine(mockBaseDirectory, "TestingGlobalPath", "NuGet.Config");

                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key2"" value=""user"" />
        <add key=""key3"" value=""user"" />
        <add key=""newKey"" value=""newValue"" />
    </SectionName>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, expectedPath))).Should().Be(result);
            }
        }

        [Fact]
        public void SettingsFileParse_WithUnknownElements_IgnoredWhenPackagePatternsAreUpdated()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <yay/>
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            using var mockBaseDirectory = TestDirectory.Create();
            SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
            var settingsFile = new SettingsFile(mockBaseDirectory);
            var settings = new Settings(new SettingsFile[] { settingsFile });

            // Act &
            settings.AddOrUpdate(settingsFile, "packageSourceMapping", new PackageSourceMappingSourceItem("nuget.org", new List<PackagePatternItem>() { new PackagePatternItem("moreStuff") }));
            settings.SaveToDisk();

            var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <yay/>
            <package pattern=""moreStuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(result);
        }

        private static string GetOriginDirectoryPath()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return @"C:\a";
            }

            return "/a";
        }
    }
}
