// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

public class GetLatestVersionAsyncTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance, virtualProjectBuilder: null);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public async Task GetLatestVersionAsync_WithMultipleVersions_ReturnsHighestStableVersion()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        var packageId = "TestPackage.A";

        var packages = new[]
        {
            new SimpleTestPackageContext(packageId, "1.0.0"),
            new SimpleTestPackageContext(packageId, "2.0.0"),
            new SimpleTestPackageContext(packageId, "2.1.0-beta"),
        };

        await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packages);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.GetLatestVersionAsync(
            packageId,
            includePrerelease: false,
            allowedSources: null,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().Be(new NuGetVersion("2.0.0"));
        result.VersionInCooldown.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestVersionAsync_WithPrereleaseVersions_WhenIncludePrereleaseTrue_ReturnsPrereleaseVersion()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        var packageId = "TestPackage.B";

        var packages = new[]
        {
            new SimpleTestPackageContext(packageId, "1.0.0"),
            new SimpleTestPackageContext(packageId, "2.0.0-beta")
        };

        await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packages);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.GetLatestVersionAsync(
            packageId,
            includePrerelease: true,
            allowedSources: null,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().Be(new NuGetVersion("2.0.0-beta"));
        result.VersionInCooldown.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestVersionAsync_WithNonExistentPackage_ReturnsNull()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.GetLatestVersionAsync(
            "NonExistent.Package",
            includePrerelease: false,
            allowedSources: null,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().BeNull();
        result.VersionInCooldown.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestVersionAsync_WithAllowedSources_RespectsSourceFilter()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        var packageId = "TestPackage.D";

        var source1 = Path.Combine(testContext.SolutionRoot, "source1");
        var source2 = Path.Combine(testContext.SolutionRoot, "source2");
        Directory.CreateDirectory(source1);
        Directory.CreateDirectory(source2);

        var packagesSource1 = new[]
        {
            new SimpleTestPackageContext(packageId, "1.0.0")
        };
        await SimpleTestPackageUtility.CreatePackagesAsync(source1, packagesSource1);

        var packagesSource2 = new[]
        {
            new SimpleTestPackageContext(packageId, "2.0.0")
        };
        await SimpleTestPackageUtility.CreatePackagesAsync(source2, packagesSource2);

        var nugetConfig = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""source1"" value=""{source1}"" />
    <add key=""source2"" value=""{source2}"" />
  </packageSources>
</configuration>";
        File.WriteAllText(Path.Combine(testContext.SolutionRoot, "nuget.config"), nugetConfig);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act - only use source1
        var result = await packageUpdateIO.GetLatestVersionAsync(
            packageId,
            includePrerelease: false,
            allowedSources: new[] { "source1" },
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().Be(new NuGetVersion("1.0.0"));
        result.VersionInCooldown.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestVersionAsync_WithMinPublishAge_ReturnsHighestVersionOutsideCooldown()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        const string packageId = "TestPackage.Cooldown";
        DateTimeOffset utcNow = new(2026, 9, 11, 0, 0, 0, TimeSpan.Zero);

        await SimpleTestPackageUtility.CreatePackagesAsync(
            testContext.PackageSource,
            new SimpleTestPackageContext(packageId, "1.0.0"),
            new SimpleTestPackageContext(packageId, "2.0.0"));

        File.SetLastWriteTimeUtc(
            Path.Combine(testContext.PackageSource, $"{packageId}.1.0.0.nupkg"),
            utcNow.AddHours(-25).UtcDateTime);
        File.SetLastWriteTimeUtc(
            Path.Combine(testContext.PackageSource, $"{packageId}.2.0.0.nupkg"),
            utcNow.AddHours(-23).UtcDateTime);

        WriteNuGetConfig(testContext, minPublishAgeHours: 24);
        using PackageUpdateIO packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot, () => utcNow);

        // Act
        PackageVersionLookupResult result = await packageUpdateIO.GetLatestVersionAsync(
            packageId,
            includePrerelease: false,
            allowedSources: null,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().Be(new NuGetVersion("1.0.0"));
        result.VersionInCooldown.Should().Be(new NuGetVersion("2.0.0"));
    }

    [Fact]
    public async Task GetLatestVersionAsync_WhenAllVersionsAreInCooldown_ReturnsNull()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        const string packageId = "TestPackage.AllInCooldown";
        DateTimeOffset utcNow = new(2026, 9, 11, 0, 0, 0, TimeSpan.Zero);

        await SimpleTestPackageUtility.CreatePackagesAsync(
            testContext.PackageSource,
            new SimpleTestPackageContext(packageId, "1.0.0"));

        File.SetLastWriteTimeUtc(
            Path.Combine(testContext.PackageSource, $"{packageId}.1.0.0.nupkg"),
            utcNow.AddHours(-23).UtcDateTime);

        WriteNuGetConfig(testContext, minPublishAgeHours: 24);
        using PackageUpdateIO packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot, () => utcNow);

        // Act
        PackageVersionLookupResult result = await packageUpdateIO.GetLatestVersionAsync(
            packageId,
            includePrerelease: false,
            allowedSources: null,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().BeNull();
        result.VersionInCooldown.Should().Be(new NuGetVersion("1.0.0"));
    }

    [Fact]
    public async Task GetLatestVersionAsync_WithMatchingException_IgnoresCooldown()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        const string packageId = "TestPackage.Cooldown";
        DateTimeOffset utcNow = new(2026, 9, 11, 0, 0, 0, TimeSpan.Zero);

        await SimpleTestPackageUtility.CreatePackagesAsync(
            testContext.PackageSource,
            new SimpleTestPackageContext(packageId, "1.0.0"),
            new SimpleTestPackageContext(packageId, "2.0.0"));

        File.SetLastWriteTimeUtc(
            Path.Combine(testContext.PackageSource, $"{packageId}.1.0.0.nupkg"),
            utcNow.AddHours(-25).UtcDateTime);
        File.SetLastWriteTimeUtc(
            Path.Combine(testContext.PackageSource, $"{packageId}.2.0.0.nupkg"),
            utcNow.AddHours(-23).UtcDateTime);

        WriteNuGetConfig(testContext, minPublishAgeHours: 24, exceptionPattern: "TestPackage.*");
        using PackageUpdateIO packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot, () => utcNow);

        // Act
        PackageVersionLookupResult result = await packageUpdateIO.GetLatestVersionAsync(
            packageId,
            includePrerelease: false,
            allowedSources: null,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().Be(new NuGetVersion("2.0.0"));
        result.VersionInCooldown.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestVersionAsync_WhenAnySourceAllowsVersion_ReturnsVersion()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        const string packageId = "TestPackage.MultipleSources";
        DateTimeOffset utcNow = new(2026, 9, 11, 0, 0, 0, TimeSpan.Zero);
        string cooledSource = Path.Combine(testContext.SolutionRoot, "cooled");
        string uncooledSource = Path.Combine(testContext.SolutionRoot, "uncooled");
        Directory.CreateDirectory(cooledSource);
        Directory.CreateDirectory(uncooledSource);

        var package = new SimpleTestPackageContext(packageId, "2.0.0");
        await SimpleTestPackageUtility.CreatePackagesAsync(cooledSource, package);
        await SimpleTestPackageUtility.CreatePackagesAsync(uncooledSource, package);
        File.SetLastWriteTimeUtc(
            Path.Combine(cooledSource, $"{packageId}.2.0.0.nupkg"),
            utcNow.AddHours(-23).UtcDateTime);

        File.WriteAllText(
            Path.Combine(testContext.SolutionRoot, "nuget.config"),
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <configuration>
              <packageSources>
                <clear />
                <add key="cooled" value="{cooledSource}" minPublishAgeHours="24" />
                <add key="uncooled" value="{uncooledSource}" />
              </packageSources>
            </configuration>
            """);

        using PackageUpdateIO packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot, () => utcNow);

        // Act
        PackageVersionLookupResult result = await packageUpdateIO.GetLatestVersionAsync(
            packageId,
            includePrerelease: false,
            allowedSources: null,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Version.Should().Be(new NuGetVersion("2.0.0"));
        result.VersionInCooldown.Should().BeNull();
    }

    [Fact]
    public void GetPackageVersionsWithCooldownStatus_WithMissingPublishDate_Throws()
    {
        // Arrange
        const string packageId = "TestPackage.MissingDate";
        var packageSource = new PackageSource("https://contoso.test/v3/index.json", "contoso")
        {
            MinPublishAge = TimeSpan.FromHours(24)
        };
        var metadata = new Mock<IPackageSearchMetadata>();
        metadata.SetupGet(m => m.Identity).Returns(new PackageIdentity(packageId, new NuGetVersion("2.0.0")));
        metadata.SetupGet(m => m.Published).Returns((DateTimeOffset?)null);

        // Act
        Action action = () => PackageUpdateIO.GetPackageVersionsWithCooldownStatus(
            [metadata.Object],
            packageSource,
            packageId,
            isCooldownExempt: false,
            new DateTimeOffset(2026, 9, 11, 0, 0, 0, TimeSpan.Zero)).ToList();

        // Assert
        action.Should().Throw<PackageUpdateException>()
            .WithMessage("*contoso*TestPackage.MissingDate*2.0.0*");
    }

    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot, Func<DateTimeOffset> utcNow)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance, virtualProjectBuilder: null);
        return new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance, utcNow);
    }

    private static void WriteNuGetConfig(SimpleTestPathContext testContext, int minPublishAgeHours, string? exceptionPattern = null)
    {
        string exceptions = exceptionPattern is null
            ? string.Empty
            : $"""
                <minPublishAgeExceptions>
                  <package pattern="{exceptionPattern}" />
                </minPublishAgeExceptions>
              """;

        File.WriteAllText(
            Path.Combine(testContext.SolutionRoot, "nuget.config"),
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <configuration>
              <packageSources>
                <clear />
                <add key="test" value="{testContext.PackageSource}" minPublishAgeHours="{minPublishAgeHours}" />
              </packageSources>
            {exceptions}
            </configuration>
            """);
    }
}
