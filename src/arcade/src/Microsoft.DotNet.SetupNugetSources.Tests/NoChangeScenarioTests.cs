// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.DotNet.XUnitExtensions;
using Xunit;

namespace Microsoft.DotNet.SetupNugetSources.Tests
{
    public class NoChangeScenarioTests : IClassFixture<SetupNugetSourcesFixture>, IDisposable
    {
        private readonly ScriptRunner _scriptRunner;
        private readonly string _testOutputDirectory;

        public NoChangeScenarioTests(SetupNugetSourcesFixture fixture)
        {
            _testOutputDirectory = Path.Combine(Path.GetTempPath(), "SetupNugetSourcesTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testOutputDirectory);
            _scriptRunner = fixture.ScriptRunner;
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_testOutputDirectory))
                {
                    Directory.Delete(_testOutputDirectory, true);
                }
            }
            catch { }
        }



        [Fact]
        public async Task BasicConfig_NoChanges()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""dotnet-public"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json"" />
  </packageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));

            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(0, "script should succeed, but got error: {result.error}");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));
            modifiedConfig.ShouldBeSemanticallySame(originalConfig, "basic config with no special feeds should not be modified");
        }

        [Fact]
        public async Task ConfigWithNonDotNetFeeds_NoChanges()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""dotnet-public"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json"" />
    <add key=""private-feed"" value=""https://example.com/nuget/v3/index.json"" />
    <add key=""company-internal"" value=""https://company.example.com/nuget"" />
  </packageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));
            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(0, "Script should succeed, but got error: {result.error}");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));
            modifiedConfig.ShouldBeSemanticallySame(originalConfig, "config with non-dotnet feeds should not be modified");
        }
    }
}


