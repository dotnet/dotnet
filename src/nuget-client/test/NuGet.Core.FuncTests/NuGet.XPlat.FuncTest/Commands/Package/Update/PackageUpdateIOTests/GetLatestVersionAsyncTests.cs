// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

public class GetLatestVersionAsyncTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
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
        result.Should().NotBeNull();
        result.Should().Be(new NuGetVersion("2.0.0"));
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
        result.Should().NotBeNull();
        result.Should().Be(new NuGetVersion("2.0.0-beta"));
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
        result.Should().BeNull();
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
        result.Should().NotBeNull();
        result.Should().Be(new NuGetVersion("1.0.0"));
    }
}
