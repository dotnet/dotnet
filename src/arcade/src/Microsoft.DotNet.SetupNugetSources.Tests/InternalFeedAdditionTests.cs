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
    public class InternalFeedAdditionTests : IClassFixture<SetupNugetSourcesFixture>, IDisposable
    {
        private readonly ScriptRunner _scriptRunner;
        private readonly string _testOutputDirectory;

        public InternalFeedAdditionTests(SetupNugetSourcesFixture fixture)
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

        [Theory]
        [InlineData("dotnet5")]
        [InlineData("dotnet6")]
        [InlineData("dotnet7")]
        [InlineData("dotnet8")]
        [InlineData("dotnet9")]
        [InlineData("dotnet10")]
        public async Task ConfigWithSpecificDotNetVersion_AddsCorrespondingInternalFeeds(string dotnetVersion)
        {
            // Arrange
            var originalConfig = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""dotnet-public"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json"" />
    <add key=""{dotnetVersion}"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/{dotnetVersion}/nuget/v3/index.json"" />
  </packageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));
            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(0, "Script should succeed, but got error: {result.error}");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            modifiedConfig.ShouldContainPackageSource($"{dotnetVersion}-internal",
                $"https://pkgs.dev.azure.com/dnceng/internal/_packaging/{dotnetVersion}-internal/nuget/v3/index.json",
                $"should add {dotnetVersion}-internal feed");
            modifiedConfig.ShouldContainPackageSource($"{dotnetVersion}-internal-transport",
                $"https://pkgs.dev.azure.com/dnceng/internal/_packaging/{dotnetVersion}-internal-transport/nuget/v3/index.json",
                $"should add {dotnetVersion}-internal-transport feed");
        }

        [Fact]
        public async Task ConfigWithMultipleDotNetVersions_AddsAllInternalFeeds()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""dotnet-public"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json"" />
    <add key=""dotnet5"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet5/nuget/v3/index.json"" />
    <add key=""dotnet6"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json"" />
    <add key=""dotnet7"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json"" />
    <add key=""dotnet8"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8/nuget/v3/index.json"" />
    <add key=""dotnet9"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet9/nuget/v3/index.json"" />
    <add key=""dotnet10"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet10/nuget/v3/index.json"" />
  </packageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));
            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(0, "Script should succeed, but got error: {result.error}");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            // Should add internal feeds for all versions
            var versions = new[] { "dotnet5", "dotnet6", "dotnet7", "dotnet8", "dotnet9", "dotnet10" };
            foreach (var version in versions)
            {
                modifiedConfig.ShouldContainPackageSource($"{version}-internal",
                    $"https://pkgs.dev.azure.com/dnceng/internal/_packaging/{version}-internal/nuget/v3/index.json",
                    $"should add {version}-internal feed");
                modifiedConfig.ShouldContainPackageSource($"{version}-internal-transport",
                    $"https://pkgs.dev.azure.com/dnceng/internal/_packaging/{version}-internal-transport/nuget/v3/index.json",
                    $"should add {version}-internal-transport feed");
            }

            // Original count (7 sources) + 12 internal sources = 19 total
            modifiedConfig.GetPackageSourceCount().Should().Be(19, "should have all original sources plus internal feeds");
        }

        [Fact]
        public async Task ConfigWithExistingInternalFeed_DoesNotDuplicate()
        {
            // Arrange
            var originalConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""dotnet-public"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json"" />
    <add key=""dotnet6"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json"" />
    <add key=""dotnet6-internal"" value=""https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet6-internal/nuget/v3/index.json"" />
  </packageSources>
</configuration>";
            var configPath = Path.Combine(_testOutputDirectory, "nuget.config");
            await Task.Run(() => File.WriteAllText(configPath, originalConfig));
            // Act
            var result = await _scriptRunner.RunScript(configPath);

            // Assert
            result.exitCode.Should().Be(0, "Script should succeed, but got error: {result.error}");
            var modifiedConfig = await Task.Run(() => File.ReadAllText(configPath));

            // Should still contain the dotnet6-internal feed (only once)
            modifiedConfig.ShouldContainPackageSource("dotnet6-internal",
                "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet6-internal/nuget/v3/index.json",
                "existing internal feed should be preserved");

            // Should add the missing transport feed
            modifiedConfig.ShouldContainPackageSource("dotnet6-internal-transport",
                "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet6-internal-transport/nuget/v3/index.json",
                "should add missing transport feed");

            // Should have 4 total sources (3 original + 1 added transport)
            modifiedConfig.GetPackageSourceCount().Should().Be(4, "should not duplicate existing sources");
        }
    }
}


