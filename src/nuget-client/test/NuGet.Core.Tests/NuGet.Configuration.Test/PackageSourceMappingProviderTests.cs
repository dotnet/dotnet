// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class PackageSourceMappingProviderTests
    {
        [Fact]
        public void Constructor_WithNullSettingsThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageSourceMappingProvider(null!));
        }

        [Fact]
        public void GetPackageSourceMappingItems_WithOneConfig_ReturnsCorrectPatterns()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(1);
            var packageSourceMappingSourceItem = packageSourceMappingItems.First();
            packageSourceMappingSourceItem.Key.Should().Be("nuget.org");
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(1);
            packageSourceMappingSourceItem.Patterns.First().Pattern.Should().Be("stuff");
        }

        [Fact]
        public void GetPackageSourceMappingItems_WithOneConfig_WithDuplicateKeys_Throws()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear/>
        <packageSource key=""dotnet"">
            <package pattern=""stuff"" />
        </packageSource>
        <packageSource key=""dotnet"">
            <package pattern=""stuff1"" />
        </packageSource>
        <packageSource key=""nuget.org"">
            <package pattern=""stuf2"" />
        </packageSource>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff3"" />
        </packageSource>
        <packageSource key=""source"">
            <package pattern=""stuff4"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            // Act & Assert
            var exception = Assert.Throws<NuGetConfigurationException>(
                () => Settings.LoadSettingsGivenConfigPaths(new string[] { configPath }));
            Assert.Equal(string.Format(CultureInfo.CurrentCulture, "PackageSourceMapping is enabled and there are multiple package sources associated with the same key(s): dotnet, nuget.org. Path: {0}", configPath), exception.Message);
        }

        [Fact]
        public void GetPackageSourceMappingItems_WithMultipleConfigs_ReturnsClosestPatterns()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var configPath2 = Path.Combine(mockBaseDirectory, "NuGet.Config.2");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1, configPath2 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(1);
            var packageSourceMappingSourceItem = packageSourceMappingItems.First();
            packageSourceMappingSourceItem.Key.Should().Be("nuget.org");
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(1);
            packageSourceMappingSourceItem.Patterns.First().Pattern.Should().Be("stuff");
        }

        [Fact]
        public void GetPackageSourceMappingItems_WithMultipleConfigs_CombinesDifferentKeys()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var configPath2 = Path.Combine(mockBaseDirectory, "NuGet.Config.2");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1, configPath2 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(2);

            var contosoSourceItem = packageSourceMappingItems.First();
            contosoSourceItem.Key.Should().Be("contoso");
            contosoSourceItem.Patterns.Should().HaveCount(1);
            contosoSourceItem.Patterns.First().Pattern.Should().Be("stuff2");

            var nugetOrgSourceItem = packageSourceMappingItems.Last();
            nugetOrgSourceItem.Key.Should().Be("nuget.org");
            nugetOrgSourceItem.Patterns.Should().HaveCount(1);
            nugetOrgSourceItem.Patterns.First().Pattern.Should().Be("stuff");
        }

        [Fact]
        public void GetPackageSourceMappingItems_WithMultipleConfigs_RespectsClearTag()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var configPath2 = Path.Combine(mockBaseDirectory, "NuGet.Config.2");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1, configPath2 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(1);

            var contosoSourceItem = packageSourceMappingItems.First();
            contosoSourceItem.Key.Should().Be("nuget.org");
            contosoSourceItem.Patterns.Should().HaveCount(1);
            contosoSourceItem.Patterns.First().Pattern.Should().Be("stuff");
        }

        [Fact]
        public void Remove_WithOneConfig_RemovesElementOfInterest()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(2);
            var contosoSourceItem = packageSourceMappingItems.First(e => e.Key.Equals("contoso"));

            // Act & Assert
            sourceMappingProvider.Remove(new PackageSourceMappingSourceItem[] { contosoSourceItem });
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void Remove_WithElementNotInConfig_DoesntChangeConfig()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            var configContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
            SettingsTestUtils.CreateConfigurationFile(configPath1, configContent);
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(2);

            // Act & Assert
            sourceMappingProvider.Remove(new PackageSourceMappingSourceItem[] { new PackageSourceMappingSourceItem("localConfig", new PackagePatternItem[] { new PackagePatternItem("item") }) });


            File.ReadAllText(configPath1).Replace("\r\n", "\n")
                .Should().BeEquivalentTo(configContent.Replace("\r\n", "\n"));
        }

        [Fact]
        public void Remove_WithMultipleConfigs_ChangesOriginConfig()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var configPath2 = Path.Combine(mockBaseDirectory, "NuGet.Config.2");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1, configPath2 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            sourceMappingProvider.Remove(new PackageSourceMappingSourceItem[] { packageSourceMappingItems.Last() });
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void AddOrUpdatePackageSourceMapping_WithUpdatedPatterns()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var configPath2 = Path.Combine(mockBaseDirectory, "NuGet.Config.2");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1, configPath2 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            var packageSourceMappingItem = packageSourceMappingItems.Last();
            packageSourceMappingItem.Patterns.Add(new PackagePatternItem("added"));
            sourceMappingProvider.AddOrUpdatePackageSourceMappingSourceItem(packageSourceMappingItem);
            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
            <package pattern=""added"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void AddOrUpdatePackageSourceMapping_WithANewPattern_AddInFurthestConfig()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var configPath2 = Path.Combine(mockBaseDirectory, "NuGet.Config.2");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1, configPath2 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            var patternToAdd = new PackageSourceMappingSourceItem("localSource", new PackagePatternItem[] { new PackagePatternItem("added") });
            sourceMappingProvider.AddOrUpdatePackageSourceMappingSourceItem(patternToAdd);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
        <packageSource key=""localSource"">
            <package pattern=""added"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath2).Replace("\r\n", "\n"));
        }

        [Fact]
        public void AddOrUpdatePackageSourceMapping_WithAClearItem_WithANewPattern_AddInFurthestMatchingConfig()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var configPath2 = Path.Combine(mockBaseDirectory, "NuGet.Config.2");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""contoso"">
            <package pattern=""stuff2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1, configPath2 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            var patternToAdd = new PackageSourceMappingSourceItem("localSource", new PackagePatternItem[] { new PackagePatternItem("added") });
            sourceMappingProvider.AddOrUpdatePackageSourceMappingSourceItem(patternToAdd);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
        <packageSource key=""localSource"">
            <package pattern=""added"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_WithNewMappings_Add()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            PackagePatternItem testPackagePatternItem = new PackagePatternItem("stuff");
            List<PackagePatternItem> testPackagePatternItems = new List<PackagePatternItem>();
            testPackagePatternItems.Add(testPackagePatternItem);
            PackageSourceMappingSourceItem testMappingItem = new PackageSourceMappingSourceItem("nuget.org", testPackagePatternItems);

            List<PackagePatternItem> packagePatternItems = new List<PackagePatternItem>();
            for (int i = 0; i < 3; i++)
            {
                PackagePatternItem tempPackagePatternItem = new PackagePatternItem(i.ToString());
                packagePatternItems.Add(tempPackagePatternItem);
            }
            PackageSourceMappingSourceItem tempMapping = new PackageSourceMappingSourceItem("tempSource", packagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(testMappingItem);
            tempMappings.Add(tempMapping);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
        <packageSource key=""tempSource"">
            <package pattern=""0"" />
            <package pattern=""1"" />
            <package pattern=""2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_WithUpdatedMappings_Update()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            List<PackagePatternItem> packagePatternItems = new List<PackagePatternItem>();
            for (int i = 0; i < 3; i++)
            {
                PackagePatternItem tempPackagePatternItem = new PackagePatternItem(i.ToString());
                packagePatternItems.Add(tempPackagePatternItem);
            }
            PackageSourceMappingSourceItem tempMapping = new PackageSourceMappingSourceItem("nuget.org", packagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(tempMapping);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""0"" />
            <package pattern=""1"" />
            <package pattern=""2"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_WithMappingsDeleted_Remove()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
        <packageSource key=""nuget2.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            PackagePatternItem testPackagePatternItem = new PackagePatternItem("stuff");
            List<PackagePatternItem> testPackagePatternItems = new List<PackagePatternItem>();
            testPackagePatternItems.Add(testPackagePatternItem);
            PackageSourceMappingSourceItem testMappingItem = new PackageSourceMappingSourceItem("nuget.org", testPackagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(testMappingItem);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_WithMappingsAddedRemovedUpdated_AddRemoveUpdate()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            PackagePatternItem testPackagePatternItem = new PackagePatternItem("stuff");
            List<PackagePatternItem> testPackagePatternItems = new List<PackagePatternItem>();
            testPackagePatternItems.Add(testPackagePatternItem);
            PackageSourceMappingSourceItem testMappingItem = new PackageSourceMappingSourceItem("nuget.org", testPackagePatternItems);
            PackageSourceMappingSourceItem testMappingItem2 = new PackageSourceMappingSourceItem("nuget2.org", testPackagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(testMappingItem);
            tempMappings.Add(testMappingItem2);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            //Update
            List<PackagePatternItem> packagePatternItems = new List<PackagePatternItem>();
            for (int i = 0; i < 3; i++)
            {
                PackagePatternItem tempPackagePatternItem = new PackagePatternItem(i.ToString());
                packagePatternItems.Add(tempPackagePatternItem);
            }
            PackageSourceMappingSourceItem tempMapping = new PackageSourceMappingSourceItem("nuget.org", packagePatternItems);
            tempMappings.Add(tempMapping);

            //Add
            PackagePatternItem packagePatternItemAdd = new PackagePatternItem("stuff");
            List<PackagePatternItem> packagePatternItemsAdd = new List<PackagePatternItem>();
            packagePatternItemsAdd.Add(packagePatternItemAdd);
            PackageSourceMappingSourceItem testMappingItemAdd = new PackageSourceMappingSourceItem("newSource", packagePatternItemsAdd);

            tempMappings.Add(testMappingItemAdd);
            tempMappings.Remove(testMappingItem2);

            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""0"" />
            <package pattern=""1"" />
            <package pattern=""2"" />
        </packageSource>
        <packageSource key=""newSource"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_PackageSourceMappingDisabled_AddMapping()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>");

            List<PackagePatternItem> testPackagePatternItems = new List<PackagePatternItem>();

            List<PackagePatternItem> packagePatternItems = new List<PackagePatternItem>();
            for (int i = 0; i < 3; i++)
            {
                PackagePatternItem tempPackagePatternItem = new PackagePatternItem(i.ToString());
                packagePatternItems.Add(tempPackagePatternItem);
            }
            PackageSourceMappingSourceItem tempMapping = new PackageSourceMappingSourceItem("tempSource", packagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(tempMapping);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSourceMapping>
    <packageSource key=""tempSource"">
        <package pattern=""0"" />
        <package pattern=""1"" />
        <package pattern=""2"" />
      </packageSource>
  </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_NewPatternExistingSource_Add()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            PackagePatternItem testPackagePatternItem = new PackagePatternItem("stuff");
            PackagePatternItem testPackagePatternItem2 = new PackagePatternItem("newPattern");
            List<PackagePatternItem> testPackagePatternItems = new List<PackagePatternItem>();
            testPackagePatternItems.Add(testPackagePatternItem);
            testPackagePatternItems.Add(testPackagePatternItem2);
            PackageSourceMappingSourceItem testMappingItem = new PackageSourceMappingSourceItem("nuget.org", testPackagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(testMappingItem);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
            <package pattern=""newPattern"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_RemovePatternExistingSource_Remove()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
            <package pattern=""newPattern"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            PackagePatternItem testPackagePatternItem = new PackagePatternItem("stuff");
            List<PackagePatternItem> testPackagePatternItems = new List<PackagePatternItem>();
            testPackagePatternItems.Add(testPackagePatternItem);
            PackageSourceMappingSourceItem testMappingItem = new PackageSourceMappingSourceItem("nuget.org", testPackagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(testMappingItem);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);

            var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />
        <packageSource key=""nuget.org"">
            <package pattern=""stuff"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";

            result.Replace("\r\n", "\n")
                .Should().BeEquivalentTo(
                File.ReadAllText(configPath1).Replace("\r\n", "\n"));
        }

        [Fact]
        public void SavePackageSourceMappings_WithTwoConfigs_UseCorrectMapping()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <packageSource key=""nuget.org"">
            <package pattern=""pattern1"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            using var mockBaseDirectory2 = TestDirectory.Create();
            var configPath2 = Path.Combine(mockBaseDirectory2, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath2, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSourceMapping>
        <clear />        
    </packageSourceMapping>
</configuration>");

            PackagePatternItem testPackagePatternItem = new PackagePatternItem("pattern1");
            List<PackagePatternItem> testPackagePatternItems = new List<PackagePatternItem>();
            testPackagePatternItems.Add(testPackagePatternItem);
            PackageSourceMappingSourceItem testMappingItem = new PackageSourceMappingSourceItem("testSource", testPackagePatternItems);
            List<PackageSourceMappingSourceItem> tempMappings = new List<PackageSourceMappingSourceItem>();
            tempMappings.Add(testMappingItem);

            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath2, configPath1 });

            // Act & Assert
            var sourceMappingProvider = new PackageSourceMappingProvider(settings);
            sourceMappingProvider.SavePackageSourceMappings(tempMappings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = sourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(1);
            var packageSourceMappingSourceItem = packageSourceMappingItems.First();
            packageSourceMappingSourceItem.Key.Should().Be("testSource");
            packageSourceMappingSourceItem.Patterns.Should().HaveCount(1);
            packageSourceMappingSourceItem.Patterns.First().Pattern.Should().Be("pattern1");
        }
    }
}
