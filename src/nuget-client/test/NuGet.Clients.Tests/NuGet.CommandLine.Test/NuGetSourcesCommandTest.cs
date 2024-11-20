// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetSourcesCommandTest
    {

        [Fact]
        public void SourcesCommandTest_AddSourceWithHTTPSource_ShouldFail()
        {
            string source = "http://test_source";
            using (var pathContext = new SimpleTestPathContext())
            {
                TestDirectory workingPath = pathContext.WorkingDirectory;
                SimpleTestSettingsContext settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    settings.ConfigPath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Assert.Equal(1, result.ExitCode);
                Assert.Contains(source, result.Errors);
            }
        }

        [Fact]
        public void SourcesCommandTest_AddSourceWithHTTPSSource_ShouldSucceed()
        {
            string source = "https://test_source";
            using (var pathContext = new SimpleTestPathContext())
            {
                TestDirectory workingPath = pathContext.WorkingDirectory;
                SimpleTestSettingsContext settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    settings.ConfigPath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(workingPath, null, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal(source, sourceItem.GetValueAsPath());
            }
        }

        [Theory]
        [InlineData("http://test_source")]
        [InlineData("https://test_source")]
        public void SourcesCommandTest_AddSource_AllowInsecureConnections(string source)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                TestDirectory workingPath = pathContext.WorkingDirectory;
                SimpleTestSettingsContext settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    settings.ConfigPath,
                    "-AllowInsecureConnections"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(workingPath, null, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal(source, sourceItem.GetValueAsPath());
                Assert.Equal("True", sourceItem.AllowInsecureConnections);
                Assert.False(result.Errors.Contains(source));
            }
        }

        [Fact]
        public void SourcesCommandTest_UpdateSourceWithHTTPSource_ShouldFail()
        {
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                string source = "http://source.test";
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(pathContext.WorkingDirectory, configFileName);

                pathContext.Settings.AddSource("test_source", "http://source.test.initial");

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(1, result.ExitCode);
                Assert.Contains(source, result.Errors);
            }
        }

        [Fact]
        public void SourcesCommandTest_UpdateSourceWithHTTPSSource_ShouldSucceed()
        {
            string source = "https://source.test";
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(pathContext.WorkingDirectory, configFileName);

                pathContext.Settings.AddSource("test_source", "http://source.test.initial");

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(pathContext.WorkingDirectory, configFileName, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal(source, sourceItem.GetValueAsPath());
            }
        }

        [Fact]
        public void SourcesCommandTest_RemoveAllowInsecureConnectionsOfHttpSource_Fails()
        {
            string source = "http://source.test";
            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);
                var nugetConfig = string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://source.test.initial"" AllowInsecureConnections=""True""/>
  </packageSources>
</configuration>", source);
                Util.CreateFile(configFileDirectory, configFileName, nugetConfig);

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    configFileDirectory,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(1, result.ExitCode);
                Assert.Contains(source, result.Errors);
            }
        }
        [Fact]
        public void SourcesCommandTest_RemoveAllowInsecureConnectionsOfHttpsSource_Succeeds()
        {
            string source = "https://source.test";
            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);
                var nugetConfig = string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://source.test.initial"" AllowInsecureConnections=""True""/>
  </packageSources>
</configuration>", source);
                Util.CreateFile(configFileDirectory, configFileName, nugetConfig);

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    configFileDirectory,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(configFileDirectory, configFileName, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal(source, sourceItem.GetValueAsPath());
                Assert.Null(sourceItem.AllowInsecureConnections);
            }
        }

        [Theory]
        [InlineData("http://source.test")]
        [InlineData("https://source.test")]
        public void SourcesCommandTest_UpdateSource_AddAllowInsecureConnections(string source)
        {
            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig = string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://source.test.initial"" />
  </packageSources>
</configuration>", source);
                Util.CreateFile(configFileDirectory, configFileName, nugetConfig);

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    configFilePath,
                    "-AllowInsecureConnections"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    configFileDirectory,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(configFileDirectory, configFileName, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal(source, sourceItem.GetValueAsPath());
                Assert.Equal("True", sourceItem.AllowInsecureConnections);
                Assert.False(result.Errors.Contains(source));
            }
        }

        [Fact]
        public void SourcesCommandTest_EnableSource_ThrowsAnExceptionWhenUsingHTTP()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://test_source"" />
  </packageSources>
  <disabledPackageSources>
    <add key=""test_source"" value=""true"" />
    <add key=""Microsoft and .NET"" value=""true"" />
  </disabledPackageSources>
</configuration>");

                var args = new string[] {
                    "sources",
                    "Enable",
                    "-Name",
                    "test_source",
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Assert.Equal(1, result.ExitCode);
                Assert.Contains("http://test_source", result.Errors);
            }
        }

        [Fact]
        public void SourcesCommandTest_DisableSource_NoErrorWhenUsingHttp()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var configFileDirectory = TestDirectory.Create())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://test_source"" />
  </packageSources>
</configuration>");

                var args = new string[] {
                    "sources",
                    "Disable",
                    "-Name",
                    "test_source",
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                ISettings settings = Configuration.Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                PackageSourceProvider packageSourceProvider = new Configuration.PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();

                var testSources = sources.Where(s => s.Name == "test_source");
                Assert.Single(testSources);
                PackageSource source = testSources.Single();

                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://test_source", source.Source);
                Assert.False(source.IsEnabled, "Source is not disabled");
                Assert.False(result.Errors.Contains("http://test_source"));
            }
        }

        [Theory]
        [InlineData("http://source.test", "http://source.test.2")]
        [InlineData("https://source.test", "http://source.test.2")]
        [InlineData("https://source.test", "https://source.test.2")]
        public void SourcesList_WithDefaultFormat_UsesDetailedFormat(string source, string secondSource)
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName, string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""{0}"" />
    <add key=""test_source_2"" value=""{1}"" />
  </packageSources>
</configuration>", source, secondSource));

                var args = new string[] {
                    "sources",
                    "list",
                    "-ConfigFile",
                    configFilePath
                };

                // Main Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                // test to ensure detailed format is the default
                Assert.True(result.Output.StartsWith("Registered Sources:"));
            }
        }

        [Fact]
        public void SourcesCommandTest_AddWithUserNamePassword()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    "https://test_source",
                    "-UserName",
                    "test_user_name",
                    "-Password",
                    "test_password",
                    "-ConfigFile",
                    settings.ConfigPath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Assert.True(0 == result.ExitCode, result.Output + " " + result.Errors);

                var loadedSettings = Configuration.Settings.LoadDefaultSettings(workingPath, null, null);

                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("https://test_source", sourceItem.GetValueAsPath());

                var sourceCredentialsSection = loadedSettings.GetSection("packageSourceCredentials");
                var credentialItem = sourceCredentialsSection?.Items.First(c => string.Equals(c.ElementName, "test_source", StringComparison.OrdinalIgnoreCase)) as CredentialsItem;
                Assert.NotNull(credentialItem);

                Assert.Equal("test_user_name", credentialItem.Username);

                var password = Configuration.EncryptionUtility.DecryptString(credentialItem.Password);
                Assert.Equal("test_password", password);
            }
        }

        [Fact]
        public void SourcesList_WithAllowInsecureConnections_WarnsCorrectly()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                string configFileName = "nuget.config";
                string configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName,
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
</configuration>");
                List<PackageSource> httpPackageSources =
                [
                    new PackageSource("http://source.test1", "source1"),
                    new PackageSource("http://source.test2", "source2"),
                    new PackageSource("http://source.test3", "source3"),
                    new PackageSource("http://source.test4", "source4"),
                    new PackageSource("http://source.test5", "source5")
                ];

                var args = new string[] {
                    "sources",
                    "list",
                    "-ConfigFile",
                    configFilePath
                };

                // Main Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                // http source with false allowInsecureConnections have warnings.

                Assert.Contains("http://source.test1", result.AllOutput);
                Assert.Contains("http://source.test2", result.AllOutput);
                Assert.Contains("http://source.test3", result.AllOutput);
                Assert.Contains("http://source.test4", result.AllOutput);
                Assert.Contains("http://source.test5", result.AllOutput);
            }
        }

        [Fact]
        public void SourcesCommandTest_AddWithUserNamePasswordInClearText()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    "https://test_source",
                    "-UserName",
                    "test_user_name",
                    "-Password",
                    "test_password",
                    "-StorePasswordInClearText",
                    "-ConfigFile",
                    settings.ConfigPath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Assert.True(0 == result.ExitCode, result.Output + " " + result.Errors);

                var loadedSettings = Configuration.Settings.LoadDefaultSettings(workingPath, null, null);

                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("https://test_source", sourceItem.GetValueAsPath());

                var sourceCredentialsSection = loadedSettings.GetSection("packageSourceCredentials");
                var credentialItem = sourceCredentialsSection?.Items.First(c => string.Equals(c.ElementName, "test_source", StringComparison.OrdinalIgnoreCase)) as CredentialsItem;
                Assert.NotNull(credentialItem);

                Assert.Equal("test_user_name", credentialItem.Username);
                Assert.Equal("test_password", credentialItem.Password);
            }
        }

        [Fact]
        public void SourcesCommandTest_AddWithUserNamePassword_UserDefinedConfigFile()
        {
            // Arrange
            using (var configFileDirectory = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>");

                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    "https://test_source",
                    "-UserName",
                    "test_user_name",
                    "-Password",
                    "test_password",
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    configFileDirectory,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);

                var settings = Configuration.Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                var packageSourcesSection = settings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("https://test_source", sourceItem.GetValueAsPath());

                var sourceCredentialsSection = settings.GetSection("packageSourceCredentials");
                var credentialItem = sourceCredentialsSection?.Items.First(c => string.Equals(c.ElementName, "test_source", StringComparison.OrdinalIgnoreCase)) as CredentialsItem;
                Assert.NotNull(credentialItem);

                Assert.Equal("test_user_name", credentialItem.Username);

                var password = Configuration.EncryptionUtility.DecryptString(credentialItem.Password);
                Assert.Equal("test_password", password);
            }
        }

        [Fact]
        public void SourcesCommandTest_AddWithProtocolVersion_WritesProtocolVersion()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                TestDirectory workingPath = pathContext.WorkingDirectory;
                SimpleTestSettingsContext settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    @"https://source.test",
                    "-ConfigFile",
                    settings.ConfigPath,
                    "-ProtocolVersion",
                    "3",
                    "-ForceEnglishOutput"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(workingPath, null, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("3", sourceItem.ProtocolVersion);
            }
        }

        [Fact]
        public void SourcesCommandTest_AddLocalSourceWithProtocolVersion_DoesNotWriteProtocolVersion()
        {
            var source = RuntimeEnvironmentHelper.IsWindows
                ? @"c:\path\to\packages"
                : "/path/to/packages";

            using (var pathContext = new SimpleTestPathContext())
            {
                TestDirectory workingPath = pathContext.WorkingDirectory;
                SimpleTestSettingsContext settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    settings.ConfigPath,
                    "-ProtocolVersion",
                    "3",
                    "-ForceEnglishOutput"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(workingPath, null, null);
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
        public void SourcesCommandTest_AddWithProtocolVersion_ValidateProtocolVersion(string protocolVersion, bool shouldSucceed)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    "https://source.test",
                    "-ConfigFile",
                    settings.ConfigPath,
                    "-ProtocolVersion",
                    protocolVersion,
                    "-ForceEnglishOutput"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                if (shouldSucceed)
                {
                    Util.VerifyResultSuccess(result);
                }
                else
                {
                    var expectedErrorMessage = "The protocol version specified is invalid.";
                    Util.VerifyResultFailure(result, expectedErrorMessage);
                }
            }
        }

        [Fact]
        public void SourcesCommandTest_UpdateWithProtocolVersion_WritesProtocolVersion()
        {
            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test.initial"" />
  </packageSources>
</configuration>";
                Util.CreateFile(configFileDirectory, configFileName, nugetConfig);

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    @"https://source.test",
                    "-ConfigFile",
                    configFilePath,
                    "-ProtocolVersion",
                    "3",
                    "-ForceEnglishOutput"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    configFileDirectory,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(configFileDirectory, configFileName, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                Assert.Single(packageSourcesSection.Items);
                SourceItem sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");
                Assert.Equal("3", sourceItem.ProtocolVersion);
            }
        }

        [Fact]
        public void SourcesCommandTest_UpdateLocalSourceWithProtocolVersion_DoesNotWriteProtocolVersion()
        {
            var source = RuntimeEnvironmentHelper.IsWindows
                ? @"c:\path\to\packages"
                : "/path/to/packages";

            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test.initial"" />
  </packageSources>
</configuration>";
                Util.CreateFile(configFileDirectory, configFileName, nugetConfig);

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    source,
                    "-ConfigFile",
                    configFilePath,
                    "-ProtocolVersion",
                    "3",
                    "-ForceEnglishOutput"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    configFileDirectory,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                ISettings loadedSettings = Configuration.Settings.LoadDefaultSettings(configFileDirectory, configFileName, null);
                SettingSection packageSourcesSection = loadedSettings.GetSection("packageSources");
                Assert.Single(packageSourcesSection.Items);
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
        public void SourcesCommandTest_UpdateWithProtocolVersion_ValidateProtocolVersion(string protocolVersion, bool shouldSucceed)
        {
            using (TestDirectory configFileDirectory = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                var nugetConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://source.test.initial"" />
  </packageSources>
</configuration>";
                Util.CreateFile(configFileDirectory, configFileName, nugetConfig);

                // Arrange
                var args = new string[] {
                    "sources",
                    "Update",
                    "-Name",
                    "test_source",
                    "-Source",
                    "https://source.test",
                    "-ConfigFile",
                    configFilePath,
                    "-ProtocolVersion",
                    protocolVersion,
                    "-ForceEnglishOutput"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    configFileDirectory,
                    string.Join(" ", args));

                // Assert
                if (shouldSucceed)
                {
                    Util.VerifyResultSuccess(result);
                }
                else
                {
                    var expectedErrorMessage = "The protocol version specified is invalid.";
                    Util.VerifyResultFailure(result, expectedErrorMessage);
                }
            }
        }

        [Fact]
        public void SourcesCommandTest_EnableSource()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var configFileDirectory = TestDirectory.Create())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""https://test_source"" />
  </packageSources>
  <disabledPackageSources>
    <add key=""test_source"" value=""true"" />
    <add key=""Microsoft and .NET"" value=""true"" />
  </disabledPackageSources>
</configuration>");

                var args = new string[] {
                    "sources",
                    "Enable",
                    "-Name",
                    "test_source",
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                var settings = Configuration.Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);
                var packageSourceProvider = new Configuration.PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                var source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("https://test_source", source.Source);
                Assert.False(source.IsEnabled);

                // Main Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                settings = Configuration.Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                var disabledSourcesSection = settings.GetSection("disabledPackageSources");
                var disabledSources = disabledSourcesSection?.Items.Select(c => c as AddItem).Where(c => c != null).ToList();
                Assert.Single(disabledSources);
                var disabledSource = disabledSources.Single();
                Assert.Equal("Microsoft and .NET", disabledSource.Key);

                packageSourceProvider = new Configuration.PackageSourceProvider(settings);
                sources = packageSourceProvider.LoadPackageSources().ToList();

                var testSources = sources.Where(s => s.Name == "test_source");
                Assert.Single(testSources);
                source = testSources.Single();

                Assert.Equal("test_source", source.Name);
                Assert.Equal("https://test_source", source.Source);
                Assert.True(source.IsEnabled, "Source is not enabled");
            }
        }

        [Fact]
        public void SourcesCommandTest_DisableSource()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var configFileDirectory = TestDirectory.Create())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""test_source"" value=""http://test_source"" />
  </packageSources>
</configuration>");

                var args = new string[] {
                    "sources",
                    "Disable",
                    "-Name",
                    "test_source",
                    "-ConfigFile",
                    configFilePath
                };

                // Act
                var settings = Configuration.Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                var packageSourceProvider = new Configuration.PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Single(sources);

                var source = sources.Single();
                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://test_source", source.Source);
                Assert.True(source.IsEnabled);

                // Main Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                settings = Configuration.Settings.LoadDefaultSettings(
                    configFileDirectory,
                    configFileName,
                    null);

                packageSourceProvider = new Configuration.PackageSourceProvider(settings);
                sources = packageSourceProvider.LoadPackageSources().ToList();

                var testSources = sources.Where(s => s.Name == "test_source");
                Assert.Single(testSources);
                source = testSources.Single();

                Assert.Equal("test_source", source.Name);
                Assert.Equal("http://test_source", source.Source);
                Assert.False(source.IsEnabled, "Source is not disabled");
            }
        }

        [Theory]
        [InlineData("sources a b")]
        [InlineData("sources a b c")]
        [InlineData("sources B a -ConfigFile file.txt -Name x -Source y")]
        public void SourcesCommandTest_Failure_InvalidArguments(string cmd)
        {
            Util.TestCommandInvalidArguments(cmd);
        }

        [Fact]
        public void TestVerbosityQuiet_DoesNotShowInfoMessages()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var settings = pathContext.Settings;

                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var args = new string[] {
                    "sources",
                    "Add",
                    "-Name",
                    "test_source",
                    "-Source",
                    "https://test_source",
                    "-Verbosity",
                    "Quiet"
                };

                // Act
                CommandRunnerResult result = CommandRunner.Run(nugetexe, workingPath, string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);
                // Ensure that no messages are shown with Verbosity as Quiet
                Assert.Equal(string.Empty, result.Output);
                var loadedSettings = Configuration.Settings.LoadDefaultSettings(workingPath, null, null);
                var packageSourcesSection = loadedSettings.GetSection("packageSources");
                var sourceItem = packageSourcesSection?.GetFirstItemWithAttribute<SourceItem>("key", "test_source");

                Assert.Equal("https://test_source", sourceItem.GetValueAsPath());
            }
        }

        [Fact]
        public void SourcesList__LocalizatedPackagesourceKeys_ConsideredDiffererent()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var configFileDirectory = TestDirectory.Create())
            {
                var configFileName = "nuget.config";
                var configFilePath = Path.Combine(configFileDirectory, configFileName);

                Util.CreateFile(configFileDirectory, configFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""encyclopaedia"" value=""https://source.test1"" />
    <add key=""Encyclopaedia"" value=""https://source.test2"" />
    <add key=""encyclopædia"" value=""https://source.test3"" />
    <add key=""Encyclopædia"" value=""https://source.test4"" />
  </packageSources>
</configuration>");

                var args = new string[] {
                    "sources",
                    "list",
                    "-ConfigFile",
                    configFilePath
                };

                // Main Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                // test to ensure detailed format is the default
                Assert.True(result.Output.StartsWith("Registered Sources:"));
                Assert.True(result.ExitCode == 0);
                Assert.Contains("encyclopaedia [Enabled]", result.Output);
                Assert.Contains("encyclopædia [Enabled]", result.Output);
                Assert.DoesNotContain("Encyclopaedia", result.Output);
                Assert.DoesNotContain("Encyclopædia", result.Output);
            }
        }
    }
}
