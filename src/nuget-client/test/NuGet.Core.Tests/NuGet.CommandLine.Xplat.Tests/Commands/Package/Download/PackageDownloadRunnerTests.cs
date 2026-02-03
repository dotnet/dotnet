// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package;
using NuGet.CommandLine.XPlat.Commands.Package.PackageDownload;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests;

public class PackageDownloadRunnerTests
{
    public sealed record SourceSpec(string Name, params string[] Versions);

    public sealed record ResolveScenario(
        string Id,
        IReadOnlyList<SourceSpec> Sources,
        string RequestedVersion,
        bool IncludePrerelease,
        string ExpectedSource,   // null => expect no resolution
        string ExpectedVersion); // null => expect no resolution

    public sealed class ResolveVersionData : TheoryData<ResolveScenario>
    {
        public ResolveVersionData()
        {
            // Exact version at first source -> returns early
            Add(new ResolveScenario(
                Id: "Contoso",
                Sources: [new SourceSpec("src", "1.2.3")],
                RequestedVersion: "1.2.3",
                IncludePrerelease: false,
                ExpectedSource: "src",
                ExpectedVersion: "1.2.3"));

            // Exact version missing in first, present in second -> pick second
            Add(new ResolveScenario(
                "Contoso",
                [new SourceSpec("srcA", "1.0.0"), new SourceSpec("srcB", "1.2.3")],
                "1.2.3",
                false,
                "srcB",
                "1.2.3"));

            // No version; exclude prerelease -> highest stable across sources
            Add(new ResolveScenario(
                "Contoso",
                [new SourceSpec("srcA", "1.0.0", "1.5.0-alpha"), new SourceSpec("srcB", "2.0.0")],
                null,
                false,
                "srcB",
                "2.0.0"));

            // No version; include prerelease -> highest including prerelease
            Add(new ResolveScenario(
                "Contoso",
                [new SourceSpec("srcA", "2.0.0"), new SourceSpec("srcB", "2.1.0-rc.1")],
                null,
                true,
                "srcB",
                "2.1.0-rc.1"));

            // No matches anywhere -> null
            Add(new ResolveScenario(
                "Contoso",
                [new SourceSpec("emptyA"), new SourceSpec("emptyB")],
                null,
                false,
                ExpectedSource: null,
                ExpectedVersion: null));

            // Exact version requested, but not found anywhere -> null
            Add(new ResolveScenario(
                "Contoso",
                [new SourceSpec("src", "1.0.0")],
                "1.2.3",
                false,
                null,
                null));
        }
    }

    [Theory]
    [ClassData(typeof(ResolveVersionData))]
    public async Task ResolvePackageDownloadVersion_Scenarios(ResolveScenario scenario)
    {
        // Arrange
        using var context = new SimpleTestPathContext();

        // Create source folders and packages
        var repos = new List<SourceRepository>();
        foreach (var src in scenario.Sources)
        {
            var folder = Path.Combine(context.WorkingDirectory, src.Name);
            Directory.CreateDirectory(folder);

            foreach (var version in src.Versions)
            {
                await SimpleTestPackageUtility.CreateFullPackageAsync(folder, scenario.Id, version);
            }

            repos.Add(Repository.Factory.GetCoreV3(new PackageSource(folder)));
        }

        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var package = new PackageWithNuGetVersion
        {
            Id = scenario.Id,
            NuGetVersion = scenario.RequestedVersion is null ? null : NuGetVersion.Parse(scenario.RequestedVersion)
        };

        // Act
        (NuGetVersion resolved, SourceRepository resolvedRepo) = await PackageDownloadRunner.ResolvePackageDownloadVersion(
            package,
            [.. repos],
            new SourceCacheContext(),
            logger.Object,
            includePrerelease: scenario.IncludePrerelease,
            CancellationToken.None);

        // Assert
        if (scenario.ExpectedVersion is null)
        {
            Assert.Null(resolved);
            Assert.Null(resolvedRepo);
            return;
        }

        Assert.Equal(new NuGetVersion(scenario.ExpectedVersion), resolved);
        Assert.NotNull(resolvedRepo);

        var expectedPath = Path.Combine(context.WorkingDirectory, scenario.ExpectedSource!);
        Assert.Equal(expectedPath, resolvedRepo.PackageSource.Source);
    }

    [Theory]
    [InlineData("1.0.0", true)]  // exact version specified -> should find even if unlisted
    [InlineData(null, false)]     // no exact version -> should return null when unlisted
    public async Task ResolvePackageDownloadVersion_UnlistedPackage_BehavesAsExpected(string requestedVersion, bool expectFound)
    {
        // Arrange
        using var pathContext = new SimpleTestPathContext();
        using var mockServer = new FileSystemBackedV3MockServer(pathContext.PackageSource);

        var cache = new SourceCacheContext();
        var logger = new Mock<ILoggerWithColor>();
        var token = CancellationToken.None;

        var sourceRepo = Repository.Factory.GetCoreV3(mockServer.ServiceIndexUri);

        // Create the package and unlist it on the mock server.
        var id = "Contoso.unlisted";
        var createdVersion = "1.0.0";
        var pkg = new SimpleTestPackageContext(id, createdVersion);
        await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, pkg);
        mockServer.UnlistedPackages.Add(pkg.Identity);

        mockServer.Start();

        var packageWithNuGetVersion = new PackageWithNuGetVersion
        {
            Id = id,
            NuGetVersion = requestedVersion is null ? null : NuGetVersion.Parse(requestedVersion)
        };

        // Act
        (NuGetVersion foundVersion, SourceRepository foundRepo) = await PackageDownloadRunner.ResolvePackageDownloadVersion(
            packageWithNuGetVersion,
            [sourceRepo],
            cache,
            logger.Object,
            includePrerelease: false,
            token);

        mockServer.Stop();

        // Assert
        if (expectFound)
        {
            foundVersion.Should().NotBeNull();
            foundVersion.OriginalVersion.Should().Be(requestedVersion);
            foundRepo.Should().NotBeNull();
            foundRepo.PackageSource.Name.Should().Be(sourceRepo.PackageSource.Name);
        }
        else
        {
            foundVersion.Should().BeNull();
            foundRepo.Should().BeNull();
        }
    }

    [Theory]
    [InlineData("A", "a")]
    [InlineData("a", "A")]
    [InlineData("SourceA", "sourcea")]
    [InlineData("SOURCEA", "sourcea")]
    public void GetMappedRepositories_WithVariousSourceCasing_ReturnsOnlyApplicableSources(string sourceNameConfig, string sourceNameMapped)
    {
        // Arrange
        var packageId = "Contoso.Package";
        var packageSource = new PackageSource(sourceNameConfig, sourceNameConfig);
        var repository = new SourceRepository(packageSource, Array.Empty<INuGetResourceProvider>());
        var allRepos = new List<SourceRepository> { repository };
        var mappedNames = new List<string> { sourceNameMapped };
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);

        // Act
        var result = PackageDownloadRunner.GetMappedRepositories(
            mappedNames,
            allRepos,
            packageId,
            logger.Object);

        // Assert
        result.Should().HaveCount(1);
        result[0].PackageSource.Name.Should().Be(sourceNameConfig);
    }

    [Fact]
    public void GetMappedRepositories_WhenSourceMissing_LogsVerbose()
    {
        // Arrange
        var packageId = "Contoso.Package";

        var existingRepo = new SourceRepository(new PackageSource("A", "A"), Array.Empty<INuGetResourceProvider>());

        var allRepos = new List<SourceRepository> { existingRepo };
        string mappedName = "MissingSource";
        var mappedNames = new List<string> { mappedName };

        string captured = "";
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        logger.Setup(l => l.LogVerbose(It.IsAny<string>()))
            .Callback<string>(msg => captured += msg);

        string expectedLog = string.Format(
            CultureInfo.CurrentCulture,
            XPlat.Strings.PackageDownloadCommand_PackageSourceMapping_NoSuchSource,
            mappedName,
            packageId);

        // Act
        var result = PackageDownloadRunner.GetMappedRepositories(
            mappedNames,
            allRepos,
            packageId,
            logger.Object);

        // Assert
        result.Should().BeEmpty();
        captured.Should().Contain(expectedLog);
    }

    [Fact]
    public void GetMappedRepositories_WhenAllMappedSourcesExist_ReturnsAllMatchedRepositories()
    {
        // Arrange
        var packageId = "Contoso.Package";

        var packageSourceA = new PackageSource("Source1", "Source1");
        var packageSourceB = new PackageSource("Source2", "Source2");

        var repoA = new SourceRepository(packageSourceA, Array.Empty<INuGetResourceProvider>());
        var repoB = new SourceRepository(packageSourceB, Array.Empty<INuGetResourceProvider>());

        var allRepos = new List<SourceRepository> { repoA, repoB };
        var mappedNames = new List<string> { "Source1", "source2" };

        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);

        // Act
        var result = PackageDownloadRunner.GetMappedRepositories(
            mappedNames,
            allRepos,
            packageId,
            logger.Object);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(packageSourceA.Name, result[0].PackageSource.Name);
        Assert.Equal(packageSourceB.Name, result[1].PackageSource.Name);
        logger.Verify(l => l.LogVerbose(It.IsAny<string>()), Times.Never);
    }
}
