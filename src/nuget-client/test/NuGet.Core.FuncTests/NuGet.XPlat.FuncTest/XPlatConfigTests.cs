// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.CommandLine.XPlat;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatConfigTests
    {
        private static readonly string XplatDll = DotnetCliUtil.GetXplatDll();
        private static readonly string DotnetCli = TestFileSystemUtility.GetDotnetCli();

        [Fact]
        public void ConfigPathsCommand_WithDirectoryArg_ListsConfigPaths()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config paths --working-directory {testInfo.WorkingPath}");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, Path.Combine(testInfo.WorkingPath.Path, "NuGet.Config"));
        }

        [Fact]
        public void ConfigPathsCommand_WithoutDirectoryArg_ListsConfigPaths()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                testInfo.WorkingPath,
                $"{XplatDll} config paths");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, Path.Combine(testInfo.WorkingPath.Path, "NuGet.Config"));
        }

        [Fact]
        public void ConfigPathsCommand_UsingHelpOption_DisplaysHelpMessage()
        {
            // Arrange
            var helpMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigPathsWorkingDirectoryDescription);

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config paths --help");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, helpMessage);
        }

        [Fact]
        public void ConfigGetCommand_WithConfigKeyAndDirectoryArgs_GetsConfigKeyValue()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get http_proxy --working-directory {testInfo.WorkingPath}");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, @"http://company-squid:3128@contoso.test");
        }

        [Fact]
        public void ConfigGetCommand_WithConfigKeyAndDirectoryArgsAndShowPathOption_GetsConfigKeyValueAndShowsPath()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get http_proxy --working-directory {testInfo.WorkingPath} --show-path");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, Path.Combine(testInfo.WorkingPath.Path, "NuGet.Config"));
            DotnetCliUtil.VerifyResultSuccess(result, @"http://company-squid:3128@contoso.test");
        }

        [Fact]
        public void ConfigGetCommand_WithConfigKeyArg_GetsConfigKeyValue()
        {
            // Arrange & Act
            var testInfo = new TestInfo("NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                testInfo.WorkingPath,
                $"{XplatDll} config get http_proxy");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, @"http://company-squid:3128@contoso.test");
        }

        [Fact]
        public void ConfigGetCommand_WithAllAndDirectoryArgs_ShowsAllSettings()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config", "subfolder");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get all --working-directory {Path.Combine(testInfo.WorkingPath, "subfolder")}");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, "value=\"https://fontoso.test/v3/index.json\"");
            DotnetCliUtil.VerifyResultSuccess(result, "value=\"https://bontoso.test/v3/index.json\"");
        }

        [Fact]
        public void ConfigGetCommand_WithAllAndDirectoryArgsAndShowPathOption_ShowsAllSettingsAndPaths()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config", "subfolder");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get all --working-directory {Path.Combine(testInfo.WorkingPath, "subfolder")} --show-path");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, Path.Combine(testInfo.WorkingPath.Path, "NuGet.Config"));
            DotnetCliUtil.VerifyResultSuccess(result, Path.Combine(testInfo.WorkingPath.Path, "subfolder", "NuGet.Config"));
            DotnetCliUtil.VerifyResultSuccess(result, "value=\"https://fontoso.test/v3/index.json\"");
            DotnetCliUtil.VerifyResultSuccess(result, "value=\"https://bontoso.test/v3/index.json\"");
        }

        [Fact]
        public void ConfigGetCommand_WithAllArg_ShowsAllSettings()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config", "subfolder");

            var result = CommandRunner.Run(
                DotnetCli,
                Path.Combine(testInfo.WorkingPath, "subfolder"),
                $"{XplatDll} config get all");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, "value=\"https://fontoso.test/v3/index.json\"");
            DotnetCliUtil.VerifyResultSuccess(result, "value=\"https://bontoso.test/v3/index.json\"");
        }

        [Fact]
        public void ConfigGetCommand_WithAllArg_ShowsSettingsInPriorityOrder()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config", "subfolder");

            var result = CommandRunner.Run(
                DotnetCli,
                Path.Combine(testInfo.WorkingPath, "subfolder"),
                $"{XplatDll} config get all");
            var firstString = "add key=\"Bar\" value=\"https://bontoso.test/v3/index.json\"";
            var secondString = "add key=\"Foo\" value=\"https://fontoso.test/v3/index.json\"";
            var firstStringIndex = result.Output.IndexOf(firstString);
            var secondStringIndex = result.Output.IndexOf(secondString);

            // Assert
            Assert.True(firstStringIndex < secondStringIndex);
        }

        [Fact]
        public void ConfigGetCommand_WithAllArgAndShowPathOption_ShowsPathsInPriorityOrder()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config", "subfolder");
            var workingDirectory = Path.Combine(testInfo.WorkingPath, "subfolder");

            var result = CommandRunner.Run(
                DotnetCli,
                workingDirectory,
                $"{XplatDll} config get all --show-path");
            var firstPath = Path.Combine(workingDirectory, "NuGet.Config");
            var secondPath = Path.Combine(testInfo.WorkingPath, "NuGet.Config");
            var firstPathIndex = result.Output.IndexOf(firstPath);
            var secondPathIndex = result.Output.IndexOf(secondPath);

            // Assert
            Assert.True(firstPathIndex < secondPathIndex);
        }

        [Fact]
        public void ConfigGetCommand_UsingHelpOption_DisplaysHelpMessage()
        {
            // Arrange
            var helpMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigGetAllOrConfigKeyDescription);

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get --help");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, helpMessage);
        }

        [Fact]
        public void ConfigGetCommand_UsingAllArgAndHelpOption_DisplaysHelpMessage()
        {
            // Arrange
            var helpMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigGetAllOrConfigKeyDescription); ;

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get all --help");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, helpMessage);
        }

        [Fact]
        public void ConfigGetCommand_UsingConfigKeyArgAndHelpOption_DisplaysHelpMessage()
        {
            // Arrange
            var helpMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigGetAllOrConfigKeyDescription); ;

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get http_proxy --help");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, helpMessage);
        }

        [Theory]
        [InlineData("signatureValidationMode", "accept")]
        [InlineData("maxHttpRequestsPerSource", "64")]
        public void ConfigSetCommand_WithConfigFileArg_AddsSetting(string key, string value)
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");
            var filePath = Path.Combine(testInfo.WorkingPath, "NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config set {key} {value} --configfile {filePath}");

            ISettings settings = Configuration.Settings.LoadDefaultSettings(
                testInfo.WorkingPath,
                configFileName: null,
                machineWideSettings: new XPlatMachineWideSetting());

            var configSection = settings.GetSection("config");
            var values = configSection.Items.Select(c => c as AddItem).Where(c => c != null).ToList();
            var configItems = values.Where(i => i.Key == key);
            var configFilePath = configItems.Single().ConfigPath;

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal(value, configItems.Single().Value);
            Assert.Equal(filePath, configFilePath);
        }

        [Fact]
        public void ConfigSetCommand_WithConfigFileArg_UpdatesSetting()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");
            var key = "http_proxy";
            var value = "http://company-octopus:8765@contoso.test";
            var filePath = Path.Combine(testInfo.WorkingPath, "NuGet.Config");

            var initialSettings = Configuration.Settings.LoadDefaultSettings(
                testInfo.WorkingPath,
                configFileName: null,
                machineWideSettings: new XPlatMachineWideSetting());
            var initialConfigSection = initialSettings.GetSection(ConfigurationConstants.Config);
            var initialConfigItem = initialConfigSection.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, key);

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config set {key} {value} --configfile {filePath}");

            var updatedSettings = Configuration.Settings.LoadDefaultSettings(
                testInfo.WorkingPath,
                configFileName: null,
                machineWideSettings: new XPlatMachineWideSetting());
            var updatedConfigSection = updatedSettings.GetSection("config");
            var values = updatedConfigSection.Items.Select(c => c as AddItem).Where(c => c != null).ToList();
            var updatedConfigItem = values.Where(i => i.Key == key);
            var configFilePath = updatedConfigItem.Single().ConfigPath;

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.NotEqual(initialConfigItem.Value, updatedConfigItem.Single().Value);
            Assert.Equal(value, updatedConfigItem.Single().Value);
            Assert.Equal(filePath, configFilePath);
        }

        [Fact]
        public void ConfigSetCommand_WithNonExistingConfigSection_AddsConfigSetting()
        {
            // Arrange & Act
            using var testInfo = new TestInfo();
            var key = "signatureValidationMode";
            var value = "accept";
            var filePath = Path.Combine(testInfo.WorkingPath, "NuGet.Config");
            var initialSettings = Configuration.Settings.LoadSpecificSettings(testInfo.WorkingPath, filePath);
            var initialConfigSection = initialSettings.GetSection("config");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config set {key} {value} --configfile {filePath}");

            var updatedSettings = Configuration.Settings.LoadSpecificSettings(testInfo.WorkingPath, filePath);
            var updatedConfigSection = updatedSettings.GetSection("config");
            var values = updatedConfigSection.Items.Select(c => c as AddItem).Where(c => c != null).ToList();
            var configItems = values.Where(i => i.Key == key);
            var configFilePath = configItems.Single().ConfigPath;

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Null(initialConfigSection);
            Assert.NotNull(updatedConfigSection);
            Assert.Equal(1, configItems.Count());
            Assert.Equal(value, configItems.First().Value);
            Assert.Equal(filePath, configFilePath);
        }

        [Fact]
        public void ConfigSetCommand_UsingHelpOption_DisplaysHelpMessage()
        {
            // Arrange
            var helpMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigSetConfigKeyDescription);

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config set --help");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, helpMessage);
        }

        [Fact]
        public void ConfigUnsetCommand_WithConfigFileOption_DeletesConfigSettingInSpecifiedFile()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");
            var filePath = Path.Combine(testInfo.WorkingPath, "NuGet.Config");
            var key = "http_proxy";

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config unset {key} --configfile {filePath}");
            var settings = Settings.LoadSpecificSettings(testInfo.WorkingPath, filePath);
            var configSection = settings.GetSection("config");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Null(configSection);
        }

        [Fact]
        public void ConfigUnsetCommand_WithNonExistingConfigKey_DisplaysKeyNotFoundMessage()
        {
            // Arrange & Act
            using var testInfo = new TestInfo();
            var filePath = Path.Combine(testInfo.WorkingPath, "NuGet.Config");
            var key = "http_proxy";

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config unset {key} --configfile {filePath}");
            var expectedMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigUnsetNonExistingKey, key);

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, expectedMessage);
        }

        [Fact]
        public void ConfigUnsetCommand_UsingHelpOption_DisplaysHelpMessage()
        {
            // Arrange
            var helpMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigUnsetConfigKeyDescription);

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config unset --help");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, helpMessage);
        }

        [Fact]
        public void ConfigCommand_UsingHelpOption_DisplaysHelpMessage()
        {
            // Arrange
            var helpMessage = string.Format(CultureInfo.CurrentCulture, Strings.ConfigPathsCommandDescription);

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config --help");

            // Assert
            DotnetCliUtil.VerifyResultSuccess(result, helpMessage);
        }

        [Fact]
        public void ConfigPathsCommand_WithNonExistingDirectoryArg_ThrowsCommandException()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var nonExistingDirectory = Path.Combine(testInfo.WorkingPath.Path, @"\NonExistingRepos");
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config paths --working-directory {nonExistingDirectory}");
            var expectedError = string.Format(CultureInfo.CurrentCulture, Strings.Error_PathNotFound, nonExistingDirectory);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedError);
        }

        [Fact]
        public void ConfigGetCommand_WithNonExistingDirectoryArg_ThrowsCommandException()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var nonExistingDirectory = Path.Combine(testInfo.WorkingPath.Path, @"\NonExistingRepos");
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get all --working-directory {nonExistingDirectory}");
            var expectedError = string.Format(CultureInfo.CurrentCulture, Strings.Error_PathNotFound, nonExistingDirectory);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedError);
        }

        [Fact]
        public void ConfigGetCommand_WithInvalidConfigKeyArg_ThrowsCommandException()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var invalidKey = "invalidKey";
            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get {invalidKey} --working-directory {testInfo.WorkingPath}");
            var expectedError = string.Format(CultureInfo.CurrentCulture, Strings.ConfigCommandKeyNotFound, invalidKey);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedError);
        }

        [Fact]
        public void ConfigGetCommand_WithNullAllOrConfigKeyArg_ThrowsCommandException()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config get");
            var expectedError = string.Format(CultureInfo.CurrentCulture, Strings.ConfigCommandKeyNotFound, "");

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedError);
        }

        [Fact]
        public void ConfigSetCommand_WithInvalidConfigKey_ThrowsCommandException()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");
            var key = "InvalidConfigKey123";
            var value = "https://TestRepo2/ES/api/v2/package";
            var filePath = Path.Combine(testInfo.WorkingPath, "NuGet.Config");

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config set {key} {value} --configfile {filePath}");
            var expectedError = string.Format(CultureInfo.CurrentCulture, Strings.Error_ConfigSetInvalidKey, key);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedError);
        }

        [Fact]
        public void ConfigUnsetCommand_WithInvalidConfigKeyArg_ThrowsCommandException()
        {
            // Arrange & Act
            using var testInfo = new TestInfo("NuGet.Config");
            var key = "InvalidConfigKey123";

            var result = CommandRunner.Run(
                DotnetCli,
                Directory.GetCurrentDirectory(),
                $"{XplatDll} config unset {key}");
            var expectedError = string.Format(CultureInfo.CurrentCulture, Strings.Error_ConfigSetInvalidKey, key);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedError);
        }

        internal class TestInfo : IDisposable
        {
            public static void CreateFile(string directory, string fileName, string fileContent)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fileFullName = Path.Combine(directory, fileName);
                CreateFile(fileFullName, fileContent);
            }

            public static void CreateFile(string fileFullName, string fileContent)
            {
                using var writer = new StreamWriter(fileFullName);
                writer.Write(fileContent);
            }

            public TestInfo(string configPath)
            {
                WorkingPath = TestDirectory.Create();
                ConfigFile = configPath;
                CreateFile(
                    WorkingPath.Path,
                    Path.GetFileName(ConfigFile),
                    $@"
<configuration>
    <packageSources>
        <add key=""Foo"" value=""https://contoso.test/v3/index.json"" />
    </packageSources>
    <config>
        <add key=""http_proxy"" value=""http://company-squid:3128@contoso.test"" />
    </config>
</configuration>
");
            }

            public TestInfo(string configPath, string subfolder)
            {
                WorkingPath = TestDirectory.Create();
                ConfigFile = configPath;
                CreateFile(
                    WorkingPath.Path,
                    Path.GetFileName(ConfigFile),
                    $@"
<configuration>
    <packageSources>
        <add key=""Foo"" value=""https://fontoso.test/v3/index.json"" />
    </packageSources>
</configuration>
");
                var subfolderPath = Path.Combine(WorkingPath.Path, subfolder);
                CreateFile(
                    subfolderPath,
                    Path.GetFileName(ConfigFile),
                    $@"
<configuration>
    <packageSources>
        <add key=""Bar"" value=""https://bontoso.test/v3/index.json"" />
    </packageSources>
    <config>
        <add key=""http_proxy"" value=""http://company-squid:3128@bontoso.test"" />
    </config>
</configuration>
");
            }

            public TestInfo()
            {
                WorkingPath = TestDirectory.Create();
                ConfigFile = "NuGet.Config";
                CreateFile(
                    WorkingPath.Path,
                    Path.GetFileName(ConfigFile),
                    $@"
<configuration>
</configuration>
");
            }

            public TestDirectory WorkingPath { get; }
            public string ConfigFile { get; }
            public void Dispose()
            {
                WorkingPath.Dispose();
            }
        }
    }
}
