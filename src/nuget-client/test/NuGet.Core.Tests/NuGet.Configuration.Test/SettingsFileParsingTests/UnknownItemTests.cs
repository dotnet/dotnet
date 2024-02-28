// Copyright(c) .NET Foundation. All rights reserved.
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
    public class UnknownItemTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void UnknownItem_Constructor_WithEmptyOrNullName_Throws(string name)
        {
            var ex = Record.Exception(() => new UnknownItem(name: name, attributes: null, children: null));
            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
        }

        [Fact]
        public void UnknownItem_Empty_ParsedCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown />
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown", attributes: null, children: null);

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                SettingsTestUtils.DeepEquals(element!, expectedSetting).Should().BeTrue();
            }
        }

        [Fact]
        public void UnknownItem_WithChildren_OnlyItems_ParsedCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown>
            <add key=""key"" value=""Val"" />
        </Unknown>
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown", attributes: null, children: new List<SettingBase>() { new AddItem("key", "Val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                SettingsTestUtils.DeepEquals(element!, expectedSetting).Should().BeTrue();
            }
        }

        [Fact]
        public void UnknownItem_WithChildren_OnlyText_ParsedCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown>Text for test</Unknown>
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown", attributes: null, children: new List<SettingBase>() { new SettingText("Text for test") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                SettingsTestUtils.DeepEquals(element!, expectedSetting).Should().BeTrue();
            }
        }

        [Fact]
        public void UnknownItem_WithChildren_ItemsAndText_ParsedCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown>
            Text for test
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown", attributes: null, children: new List<SettingBase>() { new SettingText("Text for test"), new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                SettingsTestUtils.DeepEquals(element!, expectedSetting).Should().BeTrue();
            }
        }


        [Fact]
        public void UnknownItem_WithAttributes_ParsedCorrectly()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown meta=""data"">
            Text for test
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "meta", "data" } },
                children: new List<SettingBase>() { new SettingText("Text for test"), new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                SettingsTestUtils.DeepEquals(element!, expectedSetting).Should().BeTrue();
            }
        }

        [Fact]
        public void UnknownItem_Add_ToMachineWide_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown />
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: true, isReadOnly: false);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                var ex = Record.Exception(() => element!.Add(new SettingText("test")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
            }
        }

        [Fact]
        public void UnknownItem_Add_ToReadOnly_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown />
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: false, isReadOnly: true);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                var ex = Record.Exception(() => element!.Add(new SettingText("test")));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
            }
        }

        [Fact]
        public void UnknownItem_Add_Item_WorksSuccessfully()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown />
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown", attributes: null,
                children: new List<SettingBase>() { new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Add(new AddItem("key", "val")).Should().BeTrue();

                // Assert
                SettingsTestUtils.DeepEquals(element, expectedSetting).Should().BeTrue();
            }
        }

        [Fact]
        public void UnknownItem_Add_Text_WorksSuccessfully()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown />
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown", attributes: null,
                children: new List<SettingBase>() { new SettingText("Text for test") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Add(new SettingText("Text for test")).Should().BeTrue();

                // Assert
                SettingsTestUtils.DeepEquals(element, expectedSetting).Should().BeTrue();
            }
        }


        [Fact]
        public void UnknownItem_Remove_ToMachineWide_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown>
            <Unknown2 />
        </Unknown>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: true, isReadOnly: false);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                var ex = Record.Exception(() => element!.Remove(element.Children.First()));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
            }
        }

        [Fact]
        public void UnknownItem_Remove_ToReadOnly_Throws()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown>
            <Unknown2 />
        </Unknown>
    </Section>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory, nugetConfigPath, isMachineWide: false, isReadOnly: true);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                // Assert
                var ex = Record.Exception(() => element!.Remove(element.Children.First()));
                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
            }
        }

        [Fact]
        public void UnknownItem_Remove_UnexistingChild_DoesNotRemoveAnything()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown meta=""data"">
            Text for test
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "meta", "data" } },
                children: new List<SettingBase>() { new SettingText("Text for test"), new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Remove(new AddItem("key3", "val3"));

                // Assert
                SettingsTestUtils.DeepEquals(element, expectedSetting).Should().BeTrue();
            }
        }


        [Fact]
        public void UnknownItem_Remove_ExistingChild_Succeeds()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown meta=""data"">
            Text for test
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "meta", "data" } },
                children: new List<SettingBase>() { new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Remove(element.Children.First());

                // Assert
                SettingsTestUtils.DeepEquals(element, expectedSetting).Should().BeTrue();
            }
        }

        [Fact]
        public void UnknownItem_Remove_ExistingChild_PreservesComments()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <!-- This is a section -->
    <Section>
        <!-- Unknown Item -->
        <Unknown meta=""data"">
            <!-- Text child -->
            Text for test
            <!-- Item child -->
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var expectedSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "meta", "data" } },
                children: new List<SettingBase>() { new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();
                element!.Remove(element.Children.First());

                settingsFile.AddOrUpdate("Section", element);
                settingsFile.SaveToDisk();

                var expectedConfig = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <!-- This is a section -->
    <Section>
        <!-- Unknown Item -->
        <Unknown meta=""data"">
            <!-- Text child -->
            <!-- Item child -->
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>");

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath))).Should().Be(expectedConfig);

                // Assert
                SettingsTestUtils.DeepEquals(element, expectedSetting).Should().BeTrue();
            }
        }

        [Fact]
        public void UnknownItem_Update_AddsNewAttributes()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"" />
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" }, { "new", "attr" } },
                children: null);

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Attributes["new"].Should().Be("attr");
            }
        }

        [Fact]
        public void UnknownItem_Update_RemovesMissingAttributes()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"" />
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { },
                children: null);

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Attributes.TryGetValue("old", out var _).Should().BeFalse();
            }
        }

        [Fact]
        public void UnknownItem_Update_UpdatesExistingAttributes()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"" />
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "newAttr" } },
                children: null);

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Attributes["old"].Should().Be("newAttr");
            }
        }

        [Fact]
        public void UnknownItem_Update_AddsNewChildItems()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"" />
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: new List<SettingBase>() { new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Children.Count.Should().Be(1);
                element.Children.First().Should().BeOfType<AddItem>();
            }
        }

        [Fact]
        public void UnknownItem_Update_AddsNewChildTexts()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"" />
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: new List<SettingBase>() { new SettingText("test") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Children.Count.Should().Be(1);
                element.Children.First().Should().BeOfType<SettingText>();
            }
        }

        [Fact]
        public void UnknownItem_Update_RemovesMissingChildItems()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"">
            test
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: new List<SettingBase>() { new SettingText("test") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Children.Count.Should().Be(1);
                element.Children.First().Should().BeOfType<SettingText>();
            }
        }


        [Fact]
        public void UnknownItem_Update_RemovesMissingChildText()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"">
            test
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: new List<SettingBase>() { new AddItem("key", "val") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Children.Count.Should().Be(1);
                element.Children.First().Should().BeOfType<AddItem>();
            }
        }


        [Fact]
        public void UnknownItem_Update_UpdatesExistingChildItems()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"">
            <add key=""key"" value=""val"" />
        </Unknown>
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: new List<SettingBase>() { new AddItem("key", "val", new Dictionary<string, string>() { { "meta", "data" } }) });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Children.Count.Should().Be(1);
                element.Children.First().Should().BeOfType<AddItem>();
                ((AddItem)element.Children.First()).AdditionalAttributes["meta"].Should().Be("data");
            }
        }

        [Fact]
        public void UnknownItem_Update_UpdatesExistingChildTexts()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"
<configuration>
    <Section>
        <Unknown old=""attr"">
            test
        </Unknown>
    </Section>
</configuration>";

            var updateSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: new List<SettingBase>() { new SettingText("New test") });

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settingsFile = new SettingsFile(mockBaseDirectory);

                // Act
                var section = settingsFile.GetSection("Section");
                section.Should().NotBeNull();

                var element = (UnknownItem?)section!.Items.FirstOrDefault();
                element.Should().NotBeNull();

                element!.Update(updateSetting);

                // Assert
                element.Children.Count.Should().Be(1);
                element.Children.First().Should().BeOfType<SettingText>();
                ((SettingText)element.Children.First()).Value.Should().Be("New test");
            }
        }

        [Fact]
        public void UnknownItem_Merge_OverridesSimilarAttributes()
        {
            var originalSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: null);

            var newSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "newAttr" } },
                children: null);

            originalSetting.Merge(newSetting);

            var expectedSetting = new UnknownItem("Unknown",
            attributes: new Dictionary<string, string>() { { "old", "newAttr" } },
            children: null);

            SettingsTestUtils.DeepEquals(originalSetting, expectedSetting).Should().BeTrue();
        }

        [Fact]
        public void UnknownItem_Merge_AddsNewAttributes()
        {
            var originalSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "old", "attr" } },
                children: null);

            var newSetting = new UnknownItem("Unknown",
                attributes: new Dictionary<string, string>() { { "new", "newAttr" } },
                children: null);

            originalSetting.Merge(newSetting);

            var expectedSetting = new UnknownItem("Unknown",
            attributes: new Dictionary<string, string>() { { "old", "attr" }, { "new", "newAttr" } },
            children: null);

            SettingsTestUtils.DeepEquals(originalSetting, expectedSetting).Should().BeTrue();
        }

        [Fact]
        public void UnknownItem_Merge_UpdatesSimilarChildren()
        {
            var originalSetting = new UnknownItem("Unknown",
                attributes: null,
                children: new List<SettingBase>() { new AddItem("key", "val") });

            var newSetting = new UnknownItem("Unknown",
                attributes: null,
                children: new List<SettingBase>() { new AddItem("key", "val1") });

            originalSetting.Merge(newSetting);

            var expectedSetting = new UnknownItem("Unknown",
            attributes: null,
            children: new List<SettingBase>() { new AddItem("key", "val1") });

            SettingsTestUtils.DeepEquals(originalSetting, expectedSetting).Should().BeTrue();
        }

        [Fact]
        public void UnknownItem_Merge_AddshNewChildren()
        {
            var originalSetting = new UnknownItem("Unknown",
                attributes: null,
                children: new List<SettingBase>() { new AddItem("key", "val") });

            var newSetting = new UnknownItem("Unknown",
                attributes: null,
                children: new List<SettingBase>() { new SettingText("New test") });

            originalSetting.Merge(newSetting);

            var expectedSetting = new UnknownItem("Unknown",
            attributes: null,
            children: new List<SettingBase>() { new AddItem("key", "val"), new SettingText("New test") });

            SettingsTestUtils.DeepEquals(originalSetting, expectedSetting).Should().BeTrue();
        }

        [Fact]
        public void UnknownItem_Equals_WithSameElementName_ReturnsTrue()
        {
            var unkown1 = new UnknownItem("item1", attributes: new Dictionary<string, string>() { { "meta1", "data1" } }, children: new List<SettingBase>() { new AddItem("key", "val") });
            var unkown2 = new UnknownItem("item1", attributes: new Dictionary<string, string>() { { "meta2", "data2" }, { "meta3", "data4" } }, children: new List<SettingBase>() { new ClearItem() });

            unkown1.Equals(unkown2).Should().BeTrue();
        }

        [Fact]
        public void UnknownItem_Equals_WithDifferentElementName_ReturnsFalse()
        {
            var unkown1 = new UnknownItem("item1", attributes: new Dictionary<string, string>() { { "meta1", "data1" } }, children: new List<SettingBase>() { new AddItem("key", "val") });
            var unkown2 = new UnknownItem("item2", attributes: new Dictionary<string, string>() { { "meta1", "data1" } }, children: new List<SettingBase>() { new AddItem("key", "val") });

            unkown1.Equals(unkown2).Should().BeFalse();
        }

        [Fact]
        public void UnknownItem_ElementName_IsCorrect()
        {
            var unkownItem = new UnknownItem("item", attributes: null, children: null);

            unkownItem.ElementName.Should().Be("item");
        }

        [Fact]
        public void UnknownItem_Clone_ReturnsItemClone()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <item meta=""data"">
            <add key=""key1"" value=""val"" meta=""data"" />
        </item>
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

                var clone = (UnknownItem)item.Clone();
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();
            }
        }
    }
}
