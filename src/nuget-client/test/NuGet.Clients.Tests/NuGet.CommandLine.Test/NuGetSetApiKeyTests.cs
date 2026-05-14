// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Configuration;
using NuGet.Shared;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetSetApiKeyTests
    {
        private static readonly string NuGetExePath = Util.GetNuGetExePath();

        [Fact]
        public void SetApiKey_DefaultSource()
        {
            using (var testFolder = TestDirectory.Create())
            {
                var configFile = Path.Combine(testFolder, "nuget.config");
                Util.CreateFile(configFile, "<configuration/>");

                var testApiKey = Guid.NewGuid().ToString();

                // Act
                var result = CommandRunner.Run(
                    NuGetExePath,
                    testFolder,
                    $"setApiKey {testApiKey} -ConfigFile {configFile}");

                // Assert
                Assert.True(0 == result.ExitCode, $"{result.Output} {result.Errors}");
                Assert.Contains($"The API Key '{testApiKey}' was saved for the NuGet gallery (https://www.nuget.org)", result.Output);
                Assert.DoesNotContain($"symbol", result.Output);

                var settings = Configuration.Settings.LoadDefaultSettings(
                    Path.GetDirectoryName(configFile),
                    Path.GetFileName(configFile),
                    null);

                var actualApiKey = SettingsUtility.GetDecryptedValueForAddItem(settings, ConfigurationConstants.ApiKeys, NuGetConstants.DefaultGalleryServerUrl);
                Assert.NotNull(actualApiKey);
                Assert.Equal(testApiKey, actualApiKey);
                XElement apiKeySection = SimpleTestSettingsContext.GetOrAddSection(XmlUtility.Load(configFile), ConfigurationConstants.ApiKeys);
                Assert.Equal(1, apiKeySection.Elements().Count());
            }
        }

        [Theory]
        [InlineData("setapikey")]
        [InlineData("setApiKey k1 k2")]
        [InlineData("setapikey a -ConfigFile b c d")]
        public void SetApiKey_Failure_InvalidArguments(string cmd)
        {
            Util.TestCommandInvalidArguments(cmd);
        }

        [Theory]
        [InlineData("http://nuget.org/api/v2")]
        [InlineData("https://NUGET.ORG/api/v2")]
        [InlineData("http://www.nuget.org/api/v2")]
        [InlineData("https://WWW.NUGET.ORG/api/v2")]
        [InlineData("http://api.nuget.org/v3/index.json")]
        [InlineData("https://api.nuget.org/v3/index.json")]
        [InlineData("http://some.nuget.org")]
        [InlineData("https://some.nuget.org/")]
        [InlineData("https://nuget.contoso.org/")]
        [InlineData("https://nuget.contoso.org/v3/api/v2")]
        [InlineData("https://nuget.contoso.org/v3/index.json")]
        [InlineData("https://randomnuget.org/v3/index.json")]
        [InlineData("https://randomnuget.org/v2")]
        public void SetApiKey_WithSpecifiedSource_SetApiKeyBySourceKey(string serverUri)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Add source into NuGet.Config file
                SimpleTestSettingsContext settings = pathContext.Settings;
                var packageSourcesSection = SimpleTestSettingsContext.GetOrAddSection(settings.XML, ConfigurationConstants.PackageSources);
                string sourceKey = serverUri.Contains("random") ? "random" : serverUri.Contains("contos") ? "contoso" : "nuget.org";
                SimpleTestSettingsContext.AddEntry(packageSourcesSection, sourceKey, serverUri);
                settings.Save();

                var testApiKey = Guid.NewGuid().ToString();

                // Act
                var result = CommandRunner.Run(
                    NuGetExePath,
                    pathContext.WorkingDirectory,
                    $"setApiKey {testApiKey} -Source {sourceKey} -ConfigFile {settings.ConfigPath}");

                var iSettings = Configuration.Settings.LoadDefaultSettings(
                    Path.GetDirectoryName(settings.ConfigPath),
                    Path.GetFileName(settings.ConfigPath),
                    null);

                // Assert
                Assert.True(0 == result.ExitCode, $"{result.Output} {result.Errors}");
                Assert.Contains($"The API Key '{testApiKey}' was saved for '{serverUri}'", result.Output);
                Assert.DoesNotContain($"symbol", result.Output);

                var actualApiKey = SettingsUtility.GetDecryptedValueForAddItem(iSettings, ConfigurationConstants.ApiKeys, serverUri);
                Assert.Equal(testApiKey, actualApiKey);
                XElement apiKeySection = SimpleTestSettingsContext.GetOrAddSection(XmlUtility.Load(settings.ConfigPath), ConfigurationConstants.ApiKeys);
                Assert.Equal(1, apiKeySection.Elements().Count());
            }
        }

        [Theory]
        [InlineData("http://nuget.org/api/v2")]
        [InlineData("https://NUGET.ORG/api/v2")]
        [InlineData("http://www.nuget.org/api/v2")]
        [InlineData("https://WWW.NUGET.ORG/api/v2")]
        [InlineData("http://api.nuget.org/v3/index.json")]
        [InlineData("https://api.nuget.org/v3/index.json")]
        [InlineData("http://some.nuget.org")]
        [InlineData("https://some.nuget.org/")]
        [InlineData("https://nuget.contoso.org/")]
        [InlineData("https://nuget.contoso.org/v3/api/v2")]
        [InlineData("https://nuget.contoso.org/v3/index.json")]
        [InlineData("https://randomnuget.org/v3/index.json")]
        [InlineData("https://randomnuget.org/v2")]
        public void SetApiKey_WithSpecifiedSource_SetApiKeyBySourceUri(string serverUri)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Add source into NuGet.Config file
                SimpleTestSettingsContext settings = pathContext.Settings;
                var packageSourcesSection = SimpleTestSettingsContext.GetOrAddSection(settings.XML, ConfigurationConstants.PackageSources);
                string sourceKey = serverUri.Contains("random") ? "random" : serverUri.Contains("contos") ? "contoso" : "nuget.org";
                SimpleTestSettingsContext.AddEntry(packageSourcesSection, sourceKey, serverUri);
                settings.Save();

                var testApiKey = Guid.NewGuid().ToString();

                // Act
                var result = CommandRunner.Run(
                    NuGetExePath,
                    pathContext.WorkingDirectory,
                    $"setApiKey {testApiKey} -Source {serverUri} -ConfigFile {settings.ConfigPath}");

                var iSettings = Configuration.Settings.LoadDefaultSettings(
                    Path.GetDirectoryName(settings.ConfigPath),
                    Path.GetFileName(settings.ConfigPath),
                    null);

                // Assert
                Assert.True(0 == result.ExitCode, $"{result.Output} {result.Errors}");
                Assert.Contains($"The API Key '{testApiKey}' was saved for '{serverUri}'", result.Output);
                Assert.DoesNotContain($"symbol", result.Output);

                var actualApiKey = SettingsUtility.GetDecryptedValueForAddItem(iSettings, ConfigurationConstants.ApiKeys, serverUri);
                Assert.Equal(testApiKey, actualApiKey);
                XElement apiKeySection = SimpleTestSettingsContext.GetOrAddSection(XmlUtility.Load(settings.ConfigPath), ConfigurationConstants.ApiKeys);
                Assert.Equal(1, apiKeySection.Elements().Count());
            }
        }
    }
}
