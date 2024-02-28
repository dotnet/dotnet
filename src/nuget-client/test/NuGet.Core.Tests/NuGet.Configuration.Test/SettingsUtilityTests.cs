// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using FluentAssertions;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class SettingsUtilityTests
    {
        [Fact]
        public void GetGlobalPackagesFolder_Default()
        {
            // Arrange
#if !IS_CORECLR
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
#else
            string userProfile;
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                userProfile = Environment.GetEnvironmentVariable("UserProfile")!;
            }
            else
            {
                userProfile = Environment.GetEnvironmentVariable("HOME")!;
            }
#endif
            var expectedPath = Path.Combine(userProfile, ".nuget", SettingsUtility.DefaultGlobalPackagesFolderPath);

            // Act
            var globalPackagesFolderPath = SettingsUtility.GetGlobalPackagesFolder(NullSettings.Instance);

            // Assert
            globalPackagesFolderPath.Should().Be(expectedPath);
        }

        [Fact]
        public void GetGlobalPackagesFolder_FromNuGetConfig()
        {
            // Arrange
            var config = @"<?xml version='1.0' encoding='utf-8'?>
<configuration>
    <config>
        <add key='globalPackagesFolder' value='a' />
    </config>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                // Act
                var globalPackagesFolderPath = SettingsUtility.GetGlobalPackagesFolder(settings);

                // Assert
                globalPackagesFolderPath.Should().Be(Path.Combine(mockBaseDirectory, "a"));
            }
        }

        [Fact]
        public void GetUpdatePackageLastAccessTimeEnabledStatus_FromNuGetConfig()
        {
            // Arrange
            var config = @"<?xml version='1.0' encoding='utf-8'?>
<configuration>
    <config>
        <add key='updatePackageLastAccessTime' value='true' />
    </config>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                // Act
                var updatePackageLastAccessTimeEnabled = SettingsUtility.GetUpdatePackageLastAccessTimeEnabledStatus(settings);

                // Assert
                updatePackageLastAccessTimeEnabled.Should().Be(true);
            }
        }

        [Fact]
        public void GetGlobalPackagesFolder_FromNuGetConfig_RelativePath()
        {
            // Arrange
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <config>
        <add key=""globalPackagesFolder"" value=""..\..\NuGetPackages"" />
    </config>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                var expected = Path.GetFullPath(Path.Combine(mockBaseDirectory, @"..\..\NuGetPackages"));

                // Act
                var globalPackagesFolderPath = SettingsUtility.GetGlobalPackagesFolder(settings);

                // Assert
                globalPackagesFolderPath.Should().Be(expected);
            }
        }

#if !IS_CORECLR
        [Fact]
        public void SetEncryptedValueForAddItem_UpdatesExistingItemValueWithEncryptedOne()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""value"" />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                // Act
                SettingsUtility.SetEncryptedValueForAddItem(settings, "SectionName", "key", "NewValue");

                // Assert
                var content = File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath));
                content.Should().NotContain("NewValue");
            }
        }

        [Fact]
        public void GetDecryptedValueForAddItem_ReturnsDecryptedValue()
        {
            // Arrange
            var configFile = "NuGet.Config";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(configFile, mockBaseDirectory, @"<configuration></configuration>");
                var settings = new Settings(mockBaseDirectory);

                SettingsUtility.SetEncryptedValueForAddItem(settings, "SectionName", "key", "value");

                // Act
                var result = SettingsUtility.GetDecryptedValueForAddItem(settings, "SectionName", "key");

                // Assert
                result.Should().Be("value");
            }
        }
#endif

        [Fact]
        public void GetDecryptedValueForAddItem_WithNoKey_ReturnsNull()
        {
            // Arrange
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <add key=""key"" value=""value"" />
    </SectionName>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                // Act
                var result = SettingsUtility.GetDecryptedValueForAddItem(settings, "SectionName", "NoKeyByThatName");

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void GetValueForAddItem_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetValueForAddItem(settings: null!, section: "randomSection", key: "randomKey"));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void DeleteValue_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.DeleteValue(settings: null!, section: "randomSection", attributeKey: "randomKey", attributeValue: "randomValue"));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetRepositoryPath_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetRepositoryPath(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetMaxHttpRequest_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetMaxHttpRequest(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetSignatureValidationMode_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetSignatureValidationMode(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetDecryptedValueForAddItem_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetDecryptedValueForAddItem(settings: null!, section: "randomSection", key: "randomKey"));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void SetEncryptedValueForAddItem_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.SetEncryptedValueForAddItem(settings: null!, section: "randomSection", key: "randomKey", value: "value"));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetConfigValue_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetConfigValue(settings: null!, key: "randomKey"));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void SetConfigValue_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.SetConfigValue(settings: null!, key: "randomKey", value: "value"));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void DeleteConfigValue_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.DeleteConfigValue(settings: null!, key: "randomKey"));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void DeleteConfigValue_WithValidSettings_DeletesKey()
        {
            // Arrange
            var keyName = "dependencyVersion";
            var nugetConfigPath = "NuGet.Config";
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <config>
    <add key=""" + keyName + @""" value=""Highest"" />
  </config>
</configuration>";

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                // Act
                SettingsUtility.DeleteConfigValue(settings, keyName);

                // Assert
                var content = File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath));
                content.Should().NotContain(keyName);
            }
        }

        [Fact]
        public void GetGlobalPackagesFolder_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetGlobalPackagesFolder(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetFallbackPackageFolders_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetFallbackPackageFolders(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetEnabledSources_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetEnabledSources(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetDefaultPushSource_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetDefaultPushSource(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetUpdatePackageLastAccessTimeEnabledStatus_WithNullSettings_Throws()
        {
            var ex = Record.Exception(() => SettingsUtility.GetUpdatePackageLastAccessTimeEnabledStatus(settings: null!));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }
    }
}
