// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Test.Utility;
using Xunit;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetSourcesTests
    {
        private readonly DotnetIntegrationTestFixture _fixture;

        public DotnetSourcesTests(DotnetIntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_WhenAddingSource_GotAdded()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    "https://source.test",
                    "--name",
                    "test_source",
                    "--configfile",
                    settings.ConfigPath
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));

                // Assert
                var loadedSettings = Settings.LoadDefaultSettings(root: workingPath, configFileName: null, machineWideSettings: null);
                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("https://source.test", sourceItem.GetValueAsPath());
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_WhenAddingSourceWithCredentials_CredentialsWereAddedAndEncrypted()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    "https://source.test",
                    "--name",
                    "test_source",
                    "--username",
                    "test_user_name",
                    "--password",
                    "test_password",
                    "--configfile",
                    settings.ConfigPath
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));


                // Assert
                var loadedSettings = Settings.LoadDefaultSettings(root: workingPath, configFileName: null, machineWideSettings: null);

                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("https://source.test", sourceItem.GetValueAsPath());

                var sourceCredentialsSection = loadedSettings.GetSection("packageSourceCredentials");
                var credentialItem = sourceCredentialsSection?.Items.First(c => string.Equals(c.ElementName, "test_source", StringComparison.OrdinalIgnoreCase)) as CredentialsItem;
                Assert.NotNull(credentialItem);

                Assert.Equal("test_user_name", credentialItem.Username);

                var password = EncryptionUtility.DecryptString(credentialItem.Password);
                Assert.Equal("test_password", password);
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("http://source.test", true)]
        [InlineData("https://source.test", false)]
        public void Sources_WarnWhenAdding(string source, bool shouldWarn)
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                TestDirectory workingPath = pathContext.WorkingDirectory;
                SimpleTestSettingsContext settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    source,
                    "--name",
                    "test_source",
                    "--configfile",
                    settings.ConfigPath
                };

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));

                // Assert
                ISettings loadedSettings = Settings.LoadDefaultSettings(root: workingPath, configFileName: null, machineWideSettings: null);

                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal(source, sourceItem.GetValueAsPath());
                Assert.Equal(shouldWarn, result.Output.Contains("warn : You are running the 'add source' operation with an 'HTTP' source"));
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("http://source.test", true)]
        [InlineData("https://source.test", false)]
        public void Sources_WarnWhenUpdatingHttpSource(string updateSource, bool shouldWarn)
        {
            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://source.test.initial"" />
  </packageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                PackageSourceProvider packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                PackageSource source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://source.test.initial", source.Source);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "update",
                    "source",
                    "test_source",
                    "--source",
                    updateSource,
                    "--configfile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                ISettings loadedSettings = Settings.LoadDefaultSettings(root: configFileDirectory, configFileName: null, machineWideSettings: null);

                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal(updateSource, sourceItem.GetValueAsPath());
                Assert.Equal(shouldWarn, result.Output.Contains("warn : You are running the 'update source' operation with an 'HTTP' source"));
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("http://source.test", "http://source.test.2", true, "warn : You are running the 'list source' operation with 'HTTP' source")]
        [InlineData("https://source.test", "http://source.test.2", true, "warn : You are running the 'list source' operation with an 'HTTP' source")]
        [InlineData("https://source.test", "https://source.test.2", false, "warn")]
        public void Sources_WarnWhenListHttpSource(string initialSource, string secondSource, bool shouldWarn, string warningMessage)
        {
            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig = string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""{0}"" />
    <add key=""test_source_2"" value=""{1}"" />
  </packageSources>
</configuration>", initialSource, secondSource);
                CreateXmlFile(configFilePath, nugetConfig);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "list",
                    "source",
                };

                // Act
                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                Assert.Equal(shouldWarn, result.Output.Contains(warningMessage));
            }
        }

        [Fact]
        public void SourcesList_WithAllowInsecureConnections_WarnsCorrectly()
        {
            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);
                string nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""source1"" value=""http://source.test1"" />
    <add key=""source2"" value=""http://source.test2"" allowInsecureConnections=""""/>
    <add key=""source3"" value=""http://source.test3"" allowInsecureConnections=""false""/>
    <add key=""source4"" value=""http://source.test4"" allowInsecureConnections=""FASLE""/>
    <add key=""source5"" value=""http://source.test5"" allowInsecureConnections=""invalidString""/>
    <add key=""source6"" value=""http://source.test6"" allowInsecureConnections=""true""/>
    <add key=""source7"" value=""http://source.test7"" allowInsecureConnections=""TRUE""/>
    <add key=""source8"" value=""https://source.test8"" allowInsecureConnections=""true""/>
    <add key=""source9"" value=""https://source.test9"" allowInsecureConnections=""false""/>
  </packageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "list",
                    "source",
                };

                // Act
                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                // http source with false allowInsecureConnections have warnings.
                string expectedWarning = SettingsTestUtils.RemoveWhitespace(@"
warn : You are running the 'list source' operation with 'HTTP' sources: 
warn : source1
warn : source2
warn : source3
warn : source4
warn : source5
warn : Non-HTTPS access will be removed in a future version. Consider migrating to 'HTTPS' sources.");
                Assert.Contains(expectedWarning, SettingsTestUtils.RemoveWhitespace(result.Output));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_WarnWhenEnableHttpSource()
        {
            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://source.test"" />
  </packageSources>
  <disabledPackageSources>
    <add key=""test_source"" value=""true"" />
  </disabledPackageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "enable",
                    "source",
                    "test_source",
                };

                // Act
                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                PackageSourceProvider packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                PackageSource source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://source.test", source.Source);
                Assert.False(source.IsEnabled);

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                packageSourceProvider = new PackageSourceProvider(settings);
                sources = packageSourceProvider.LoadPackageSources().ToList();

                var testSources = sources.Where(s => s.Name == "test_source");
                Assert.Single(testSources);
                source = testSources.Single();

                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://source.test", source.Source);
                Assert.True(result.Output.Contains("warn : You are running the 'enable source' operation with an 'HTTP' source, 'http://source.test'. Non-HTTPS access will be removed in a future version. Consider migrating to an 'HTTPS' source."));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_NoWarnWhenDisableHttpSource()
        {
            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://source.test"" />
  </packageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "disable",
                    "source",
                    "test_source",
                };

                // Act
                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                PackageSourceProvider packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                PackageSource source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://source.test", source.Source);
                Assert.True(source.IsEnabled);

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                packageSourceProvider = new PackageSourceProvider(settings);
                sources = packageSourceProvider.LoadPackageSources().ToList();

                var testSources = sources.Where(s => s.Name == "test_source");
                Assert.Single(testSources);
                source = testSources.Single();

                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://source.test", source.Source);
                Assert.False(result.Output.Contains("warn :"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_WhenAddingSourceWithCredentialsInClearText_CredentialsWereAddedAndNotEncrypted()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    "https://source.test",
                    "--name",
                    "test_source",
                    "--username",
                    "test_user_name",
                    "--password",
                    "test_password",
                    "--store-password-in-clear-text",
                    "--configfile",
                    settings.ConfigPath
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));

                // Assert
                var loadedSettings = Settings.LoadDefaultSettings(root: workingPath, configFileName: null, machineWideSettings: null);

                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("https://source.test", sourceItem.GetValueAsPath());

                var sourceCredentialsSection = loadedSettings.GetSection("packageSourceCredentials");
                var credentialItem = sourceCredentialsSection?.Items.First(c => string.Equals(c.ElementName, "test_source", StringComparison.OrdinalIgnoreCase)) as CredentialsItem;
                Assert.NotNull(credentialItem);

                Assert.Equal("test_user_name", credentialItem.Username);
                Assert.Equal("test_password", credentialItem.Password);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_WhenAddingSourceWithCredentialsToUserConfigFile_CredentialsWereAddedAndEncryptedInUserConfigFile()
        {
            // Arrange
            using (var configFileDirectory = _fixture.CreateTestDirectory())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                string nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    "https://source.test",
                    "--name",
                    "test_source",
                    "--username",
                    "test_user_name",
                    "--password",
                    "test_password",
                    "--configfile",
                    configFilePath
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                var settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                var packageSourcesSection = settings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("https://source.test", sourceItem.GetValueAsPath());

                var sourceCredentialsSection = settings.GetSection("packageSourceCredentials");
                var credentialItem = sourceCredentialsSection?.Items.First(c => string.Equals(c.ElementName, "test_source", StringComparison.OrdinalIgnoreCase)) as CredentialsItem;
                Assert.NotNull(credentialItem);

                Assert.Equal("test_user_name", credentialItem.Username);

                var password = EncryptionUtility.DecryptString(credentialItem.Password);
                Assert.Equal("test_password", password);
            }
        }

        private static void CreateXmlFile(string configFilePath, string nugetConfigString)
        {
            using (var stream = File.OpenWrite(configFilePath))
            {
                var nugetConfigXDoc = XDocument.Parse(nugetConfigString);
                ProjectFileUtils.WriteXmlToFile(nugetConfigXDoc, stream);
            }
        }

        [Fact]
        public void Sources_WhenAddingSourceWithProtocolVersion_WasAddedWithProtocolVersion()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    @"https://source.test",
                    "--name",
                    "test_source",
                    "--configfile",
                    settings.ConfigPath,
                    "--protocol-version",
                    "3"
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                var loadedSettings = Settings.LoadDefaultSettings(root: workingPath, configFileName: null, machineWideSettings: null);
                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("3", sourceItem.ProtocolVersion);
            }
        }

        [Fact]
        public void Sources_WhenAddingLocalSourceWithProtocolVersion_ProtocolVersionNotWritten()
        {
            var source = RuntimeEnvironmentHelper.IsWindows
                ? @"c:\path\to\packages"
                : "/path/to/packages";

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    source,
                    "--name",
                    "test_source",
                    "--configfile",
                    settings.ConfigPath,
                    "--protocol-version",
                    "3"
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                var loadedSettings = Settings.LoadDefaultSettings(root: workingPath, configFileName: null, machineWideSettings: null);
                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Null(sourceItem.ProtocolVersion);
            }
        }

        [Theory]
        [InlineData("1", false)]
        [InlineData("2", true)]
        [InlineData("3", true)]
        [InlineData("4", false)]
        [InlineData("5", false)]
        public void Sources_WhenAddingSourceWithProtocolVersion_ProtocolVersionIsValidated(string protocolVersion, bool shouldSucceed)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    "https://source.test",
                    "--name",
                    "test_source",
                    "--configfile",
                    settings.ConfigPath,
                    "--protocol-version",
                    protocolVersion
                };

                // Act
                var command = string.Join(" ", args);
                var result = shouldSucceed ? _fixture.RunDotnetExpectSuccess(workingPath, command) : _fixture.RunDotnetExpectFailure(workingPath, command);

                // Assert
                if (!shouldSucceed)
                {
                    // Assert error message
                    string expectedErrorMessage = "The protocol version specified is invalid.";
                    Assert.True(result.Output.Contains(expectedErrorMessage), "Expected error is " + expectedErrorMessage + ". Actual error is " + result.Output);
                }
            }
        }

        [Fact]
        public void Sources_WhenUpdatingSourceWithProtocolVersion_WasUpdatedWithProtocolVersion()
        {
            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test.initial"" />
  </packageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                PackageSourceProvider packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                PackageSource packageSource = sources.Single();
                Assert.Equal("test_source", packageSource.Name);
                Assert.Equal("https://source.test.initial", packageSource.Source);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "update",
                    "source",
                    "test_source",
                    "--source",
                    @"https://source.test",
                    "--configfile",
                    configFilePath,
                    "--protocol-version",
                    "3"
                };

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                Assert.True(result.Success, result.Output + " " + result.Errors);

                ISettings loadedSettings = Settings.LoadDefaultSettings(root: configFileDirectory, configFileName: null, machineWideSettings: null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("3", sourceItem.ProtocolVersion);
            }
        }

        [Fact]
        public void Sources_WhenUpdatingLocalSourceWithProtocolVersion_ProtocolVersionNotWritten()
        {
            var source = RuntimeEnvironmentHelper.IsWindows
                ? @"c:\path\to\packages"
                : "/path/to/packages";

            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test.initial"" />
  </packageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                PackageSourceProvider packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                PackageSource packageSource = sources.Single();
                Assert.Equal("test_source", packageSource.Name);
                Assert.Equal("https://source.test.initial", packageSource.Source);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "update",
                    "source",
                    "test_source",
                    "--source",
                    source,
                    "--configfile",
                    configFilePath,
                    "--protocol-version",
                    "3"
                };

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                Assert.True(result.Success, result.Output + " " + result.Errors);

                ISettings loadedSettings = Settings.LoadDefaultSettings(root: configFileDirectory, configFileName: null, machineWideSettings: null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Null(sourceItem.ProtocolVersion);
            }
        }

        [Theory]
        [InlineData("1", false)]
        [InlineData("2", true)]
        [InlineData("3", true)]
        [InlineData("4", false)]
        [InlineData("5", false)]
        public void Sources_WhenUpdatingSourceWithProtocolVersion_ProtocolVersionIsValidated(string protocolVersion, bool shouldSucceed)
        {
            using (TestDirectory configFileDirectory = _fixture.CreateTestDirectory())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test.initial"" />
  </packageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                ISettings settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                PackageSourceProvider packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                PackageSource source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("https://source.test.initial", source.Source);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "update",
                    "source",
                    "test_source",
                    "--source",
                    "https://source.test",
                    "--configfile",
                    configFilePath,
                    "--protocol-version",
                    protocolVersion
                };

                // Act
                var command = string.Join(" ", args);
                CommandRunnerResult result = shouldSucceed ? _fixture.RunDotnetExpectSuccess(configFileDirectory, command) : _fixture.RunDotnetExpectFailure(configFileDirectory, command);

                // Assert error message
                if (!shouldSucceed)
                {
                    string expectedErrorMessage = "The protocol version specified is invalid.";
                    Assert.Contains(expectedErrorMessage, result.Output);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_WhenEnablingADisabledSource_SourceBecameEnabled()
        {
            // Arrange
            using (var configFileDirectory = _fixture.CreateTestDirectory())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test"" />
  </packageSources>
  <disabledPackageSources>
    <add key=""test_source"" value=""true"" />
    <add key=""Microsoft and .NET"" value=""true"" />
  </disabledPackageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                var args = new string[]
                {
                    "nuget",
                    "enable",
                    "source",
                    "TEST_source", // this should work in a case sensitive manner
                    "--configfile",
                    configFilePath
                };

                // Act
                var settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    machineWideSettings: null);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                var source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("https://source.test", source.Source);
                Assert.False(source.IsEnabled);

                // Main Act
                var result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                var disabledSourcesSection = settings.GetSection("disabledPackageSources");
                var disabledSources = disabledSourcesSection?.Items.Select(c => c as AddItem).Where(c => c != null).ToList();
                Assert.Single(disabledSources);
                var disabledSource = disabledSources.Single();
                Assert.Equal("Microsoft and .NET", disabledSource.Key);

                packageSourceProvider = new PackageSourceProvider(settings);
                sources = packageSourceProvider.LoadPackageSources().ToList();

                var testSources = sources.Where(s => s.Name == "test_source");
                Assert.Single(testSources);
                source = testSources.Single();

                Assert.Equal("test_source", source.Name);
                Assert.Equal("https://source.test", source.Source);
                Assert.True(source.IsEnabled, "Source is not enabled");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Sources_WhenDisablingAnEnabledSource_SourceBecameDisabled()
        {
            // Arrange
            using (var configFileDirectory = _fixture.CreateTestDirectory())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test"" />
  </packageSources>
</configuration>";
                CreateXmlFile(configFilePath, nugetConfig);

                var args = new string[]
                {
                    "nuget",
                    "disable",
                    "source",
                    "TEST_source",
                    "--configfile",
                    configFilePath
                };

                // Act
                var settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                var packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                var source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("https://source.test", source.Source);
                Assert.True(source.IsEnabled);

                // Main Act
                var result = _fixture.RunDotnetExpectSuccess(configFileDirectory, string.Join(" ", args));

                // Assert
                settings = Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                packageSourceProvider = new PackageSourceProvider(settings);
                sources = packageSourceProvider.LoadPackageSources().ToList();

                var testSources = sources.Where(s => s.Name == "test_source");
                Assert.Single(testSources);
                source = testSources.Single();

                Assert.Equal("test_source", source.Name);
                Assert.Equal("https://source.test", source.Source);
                Assert.False(source.IsEnabled, "Source is not disabled");
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("list source --foo", 2)]
        [InlineData("add source foo bar", 3)]
        [InlineData("remove source a b", 3)]
        [InlineData("remove a b c", 1)]
        [InlineData("add source B a --configfile file.txt --name x --source y", 3)]
        [InlineData("list source --configfile file.txt B a", 4)]
        public void Sources_WhenPassingInvalidArguments_ProperErrorsAreRaised(string cmd, int badParam)
        {
            // all of these commands need to start with "nuget ", and need to adjust bad param to account for those 2 new params
            TestCommandInvalidArguments("nuget " + cmd, badParam + 1);
        }

        [Fact(Skip = "cutting verbosity Quiet for now. #6374 covers fixing it for `dotnet add package` too.")]
        public void TestVerbosityQuiet_DoesNotShowInfoMessages()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "add",
                    "source",
                    "https://source.test",
                    "--name",
                    "test_source",
                    "--verbosity",
                    "Quiet",
                    "--configfile",
                    settings.ConfigPath
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));

                // Assert
                // Ensure that no messages are shown with Verbosity as Quiet
                Assert.Equal(string.Empty, result.Output);
                var loadedSettings = Settings.LoadDefaultSettings(root: workingPath, configFileName: null, machineWideSettings: null);
                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");

                Assert.Equal("https://source.test", sourceItem.GetValueAsPath());
            }
        }

        [PlatformFact(Platform.Windows, Skip = "https://github.com/NuGet/Home/issues/12503")]
        public void List_Sources_LocalizatedPackagesourceKeys_ConsideredDiffererent()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;
                var nugetConfig =
    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""encyclopaedia"" value=""https://source.test1"" />
    <add key=""Encyclopaedia"" value=""https://source.test2"" />
    <add key=""encyclopædia"" value=""https://source.test3"" />
    <add key=""Encyclopædia"" value=""https://source.test4"" />
  </packageSources>
</configuration>";
                CreateXmlFile(settings.ConfigPath, nugetConfig);

                // Arrange
                var args = new string[]
                {
                    "nuget",
                    "list",
                    "source",
                    "--configfile",
                    settings.ConfigPath
                };

                // Act
                var result = _fixture.RunDotnetExpectSuccess(workingPath, string.Join(" ", args));

                // Assert
                Assert.True(result.Output.StartsWith("Registered Sources:"));
                Assert.Contains("encyclopaedia [Enabled]", result.Output);
                Assert.Contains("encyclopædia [Enabled]", result.Output);
                Assert.DoesNotContain("Encyclopaedia", result.Output);
                Assert.DoesNotContain("Encyclopædia", result.Output);
            }
        }

        /// <summary>
        /// Verify non-zero status code and proper messages
        /// </summary>
        /// <remarks>Checks invalid arguments message in stderr, check help message in stdout</remarks>
        /// <param name="commandName">The nuget.exe command name to verify, without "nuget.exe" at the beginning</param>
        public void TestCommandInvalidArguments(string command, int badCommandIndex)
        {
            using (var testDirectory = _fixture.CreateTestDirectory())
            {
                // Act
                var result = _fixture.RunDotnetExpectFailure(testDirectory, command);

                var commandSplit = command.Split(' ');

                // Break the test if no proper command is found
                if (commandSplit.Length < 1 || string.IsNullOrEmpty(commandSplit[0]))
                    Assert.True(false, "command not found");

                // 0th - "nuget"
                // 1st - "source"
                // 2nd - action
                // 3rd - nextParam
                string badCommand = commandSplit[badCommandIndex];

                // Assert command
                Assert.Contains("'" + badCommand + "'", result.Output, StringComparison.InvariantCultureIgnoreCase);


                // Assert invalid argument message
                string invalidMessage;
                if (badCommand.StartsWith("-"))
                {
                    invalidMessage = ": Unrecognized option";
                }
                else
                {
                    invalidMessage = ": Unrecognized command";
                }

                Assert.True(result.Output.Contains(invalidMessage), "Expected error is " + invalidMessage + ". Actual error is " + result.Output);
                // Verify traits of help message in stdout
                Assert.Contains("Specify --help for a list of available options and commands.", result.Output);
            }
        }
    }
}
