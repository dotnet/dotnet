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
    public class BoundaryConditionTests : IClassFixture<SetupNugetSourcesFixture>, IDisposable
    {
        private readonly ScriptRunner _scriptRunner;
        private readonly string _testOutputDirectory;

        public BoundaryConditionTests(SetupNugetSourcesFixture fixture)
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
        public async Task EmptyConfiguration_FailsWithoutPackageSourcesSection()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));

            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(1, "Script should fail when packageSources section is missing");

            // Check both output and error for the message (scripts may write to stdout instead of stderr)
            var errorMessage = string.IsNullOrEmpty(result.error) ? result.output : result.error;
            errorMessage.Should().Contain("packageSources section", "should report missing packageSources section error");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            // Config should remain unchanged when script fails
            modifiedConfig.Should().BeEquivalentTo(originalConfig, "config should not be modified when script fails");
        }

        [Fact]
        public async Task ConfigWithoutPackageSourcesSection_FailsWithoutPackageSourcesSection()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <disabledPackageSources>
    <add key=""darc-int-dotnet-runtime-67890"" value=""true"" />
  </disabledPackageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));

            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(1, "Script should fail when packageSources section is missing");
            // Check both output and error for the message (scripts may write to stdout instead of stderr)
            var errorMessage = string.IsNullOrEmpty(result.error) ? result.output : result.error;
            errorMessage.Should().Contain("packageSources section", "should report missing packageSources section error");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            // Config should remain unchanged when script fails
            modifiedConfig.Should().BeEquivalentTo(originalConfig, "config should not be modified when script fails");
        }

        [Fact]
        public async Task ConfigWithMissingDisabledPackageSourcesSection_StillAddsInternalFeeds()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""dotnet-public"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json"" />
    <add key=""dotnet6"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json"" />
  </packageSources>
  <!-- No disabledPackageSources section -->
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));
            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(0, "Script should succeed, but got error: {result.error}");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            // Should still add internal feeds
            modifiedConfig.ShouldContainPackageSource("dotnet6-internal");
            modifiedConfig.ShouldContainPackageSource("dotnet6-internal-transport");
        }

        [Fact]
        public async Task NonExistentConfigFile_ReturnsError()
        {
            // Arrange
            var nonExistentPath = Path.Combine(_testOutputDirectory, "nonexistent.config");
            // Act
            var result = await _scriptRunner.RunScript(nonExistentPath);

            // Assert
            result.exitCode.Should().Be(1, "should return error code for nonexistent file");
            // Check both output and error for the message (scripts may write to stdout instead of stderr)
            var errorMessage = string.IsNullOrEmpty(result.error) ? result.output : result.error;
            errorMessage.Should().Contain("Couldn't find the NuGet config file", "should report missing file error");
        }

        [Fact]
        public async Task ConfigWithOnlyDisabledSources_FailsWithoutPackageSourcesSection()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <disabledPackageSources>
    <add key=""darc-int-dotnet-roslyn-12345"" value=""true"" />
    <add key=""some-other-disabled"" value=""true"" />
  </disabledPackageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));
            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(1, "Script should fail when packageSources section is missing");
            // Check both output and error for the message (scripts may write to stdout instead of stderr)
            var errorMessage = string.IsNullOrEmpty(result.error) ? result.output : result.error;
            errorMessage.Should().Contain("packageSources section", "should report missing packageSources section error");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            // Config should remain unchanged when script fails
            modifiedConfig.Should().BeEquivalentTo(originalConfig, "config should not be modified when script fails");
        }

        [Fact]
        public async Task ConfigWithEmptyPackageSourcesSection_HandlesCorrectly()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <!-- Empty packageSources section -->
  </packageSources>
  <disabledPackageSources>
    <add key=""darc-int-dotnet-roslyn-12345"" value=""true"" />
  </disabledPackageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));
            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(0, "Script should succeed, but got error: {result.error}");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            // Should enable darc-int feeds but not add any dotnet internal feeds since no dotnet feeds exist
            modifiedConfig.ShouldNotBeDisabled("darc-int-dotnet-roslyn-12345", "should enable darc-int feed");
            modifiedConfig.GetPackageSourceCount().Should().Be(0, "should not add dotnet internal feeds without dotnet public feeds");
        }
    }
}


