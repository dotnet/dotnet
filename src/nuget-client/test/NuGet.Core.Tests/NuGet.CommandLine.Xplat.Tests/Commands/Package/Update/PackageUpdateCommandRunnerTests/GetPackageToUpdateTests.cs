// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Update.PackageUpdateCommandRunnerTests;

using Pkg = NuGet.CommandLine.XPlat.Commands.Package.Update.Package;

public class GetPackageToUpdateTests
{
    private readonly ITestOutputHelper _output;

    public GetPackageToUpdateTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task RequestSinglePackage_GetsRequestedVersion()
    {
        // Arrange
        Pkg package = new()
        {
            Id = "Contoso.Utils",
            VersionRange = new VersionRange(new NuGetVersion("1.2.3"))
        };

        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
            {
                builder.WithProperty("TargetFramework", "net9.0")
                       .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
            })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        var logger = new Mock<ILoggerWithColor>();

        // Act
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().HaveCount(1);
        var packageToUpdate = packagesToUpdate.First().Package;
        packageToUpdate.Should().NotBeNull();
        packageToUpdate.Id.Should().Be("Contoso.Utils");
        packageToUpdate.CurrentVersion.ToString().Should().Be("[1.0.0, )");
        packageToUpdate.NewVersion.ToString().Should().Be("[1.2.3, )");
        logger.Invocations.Count.Should().Be(0);
    }

    [Fact]
    public async Task RequestPackageWithoutVersion_GetsLatestVersion()
    {
        // Arrange
        Pkg package = new()
        {
            Id = "Contoso.Utils",
            VersionRange = null
        };

        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Contoso.Utils", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("3.4.5"));

        var sourceCacheContext = new SourceCacheContext();
        var logger = new Mock<ILoggerWithColor>();

        // Act
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().HaveCount(1);
        var packageToUpdate = packagesToUpdate.First().Package;
        packageToUpdate.Should().NotBeNull();
        packageToUpdate.Id.Should().Be("Contoso.Utils");
        packageToUpdate.CurrentVersion.ToString().Should().Be("[1.0.0, )");
        packageToUpdate.NewVersion.ToString().Should().Be("[3.4.5, )");
        logger.Invocations.Count.Should().Be(0);
    }

    [Fact]
    public async Task RequestSinglePackageWithRangeSyntax_GetsRequestedVersion()
    {
        // Arrange
        Pkg package = new()
        {
            Id = "Contoso.Utils",
            VersionRange = VersionRange.Parse("[1.2.3,2.0.0)")
        };

        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        var logger = new Mock<ILoggerWithColor>();

        // Act
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().HaveCount(1);
        var packageToUpdate = packagesToUpdate.First().Package;
        packageToUpdate.Should().NotBeNull();
        packageToUpdate.Id.Should().Be("Contoso.Utils");
        packageToUpdate.CurrentVersion.ToString().Should().Be("[1.0.0, )");
        packageToUpdate.NewVersion.ToString().Should().Be("[1.2.3, 2.0.0)");
        logger.Invocations.Count.Should().Be(0);
    }

    [Fact]
    public async Task RequestPackageNotOnSource_LogsError()
    {
        // Arrange
        Pkg package = new()
        {
            Id = "Contoso.Utils",
            VersionRange = null
        };

        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Contoso.Utils", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((NuGetVersion?)null);
        var logger = new Mock<ILoggerWithColor>();

        // Act
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().BeEmpty();
        logger.Invocations.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task RequestPackageNotReferencedByProject_ReturnsEmptyAndLogsError()
    {
        // Arrange
        Pkg package = new()
        {
            Id = "NotReferenced.Package",
            VersionRange = new VersionRange(new NuGetVersion("1.2.3"))
        };

        // Create a package spec that references a different package
        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        var logger = new Mock<ILoggerWithColor>();

        // Act
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().BeEmpty();

        // Verify that an error was logged
        logger.Verify(
            l => l.LogMinimal(
                It.Is<string>(message => message.Contains("NotReferenced.Package") && message.Contains("not referenced")),
                It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    [Fact]
    public async Task RequestMultiplePackages_GetsAllRequested()
    {
        // Arrange
        Pkg package1 = new()
        {
            Id = "Contoso.Utils",
            VersionRange = new VersionRange(new NuGetVersion("1.2.3"))
        };

        Pkg package2 = new()
        {
            Id = "Fabrikam.Tools",
            VersionRange = null // Should get latest version
        };

        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")])
                   .WithItem("PackageReference", "Fabrikam.Tools", [new("Version", "2.0.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Fabrikam.Tools", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("2.1.0"));

        var logger = new Mock<ILoggerWithColor>();

        // Act
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [package1, package2],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().HaveCount(2);

        var contosoUpdate = packagesToUpdate.First(p => p.Package.Id == "Contoso.Utils");
        contosoUpdate.Package.CurrentVersion.ToString().Should().Be("[1.0.0, )");
        contosoUpdate.Package.NewVersion.ToString().Should().Be("[1.2.3, )");

        var fabrikamUpdate = packagesToUpdate.First(p => p.Package.Id == "Fabrikam.Tools");
        fabrikamUpdate.Package.CurrentVersion.ToString().Should().Be("[2.0.0, )");
        fabrikamUpdate.Package.NewVersion.ToString().Should().Be("[2.1.0, )");

        logger.Invocations.Count.Should().Be(0);
    }

    [Fact]
    public async Task RequestMultiplePackagesWithOneError_ReturnsEmpty()
    {
        // Arrange
        Pkg package1 = new()
        {
            Id = "Contoso.Utils",
            VersionRange = new VersionRange(new NuGetVersion("1.2.3"))
        };

        Pkg package2 = new()
        {
            Id = "NotReferenced.Package",
            VersionRange = new VersionRange(new NuGetVersion("1.0.0"))
        };

        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        var logger = new Mock<ILoggerWithColor>();

        // Act
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [package1, package2],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().BeEmpty();
        logger.Invocations.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task NoPackagesProvided_BothPackagesHaveUpdates_UpdatesBothPackages()
    {
        // Arrange
        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package1", [new("Version", "1.0.0")])
                   .WithItem("PackageReference", "Test.Package2", [new("Version", "2.0.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Test.Package1", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("1.2.3"));
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Test.Package2", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("2.1.0"));

        var logger = new Mock<ILoggerWithColor>();

        // Act - Pass empty list to trigger "update all packages" behavior
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().HaveCount(2);

        var package1Update = packagesToUpdate.First(p => p.Package.Id == "Test.Package1");
        package1Update.Package.CurrentVersion.ToString().Should().Be("[1.0.0, )");
        package1Update.Package.NewVersion.ToString().Should().Be("[1.2.3, )");

        var package2Update = packagesToUpdate.First(p => p.Package.Id == "Test.Package2");
        package2Update.Package.CurrentVersion.ToString().Should().Be("[2.0.0, )");
        package2Update.Package.NewVersion.ToString().Should().Be("[2.1.0, )");

        logger.Invocations.Count.Should().Be(0);
    }

    [Fact]
    public async Task NoPackagesProvided_OnePackageAlreadyLatest_UpdatesOnlyOutdatedPackage()
    {
        // Arrange
        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package1", [new("Version", "1.0.0")])
                   .WithItem("PackageReference", "Test.Package2", [new("Version", "2.1.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Test.Package1", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("1.2.3"));
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Test.Package2", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("2.1.0")); // Same as current version

        var logger = new Mock<ILoggerWithColor>();

        // Act - Pass empty list to trigger "update all packages" behavior
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().HaveCount(1);

        var packageUpdate = packagesToUpdate.First();
        packageUpdate.Package.Id.Should().Be("Test.Package1");
        packageUpdate.Package.CurrentVersion.ToString().Should().Be("[1.0.0, )");
        packageUpdate.Package.NewVersion.ToString().Should().Be("[1.2.3, )");

        logger.Invocations.Count.Should().Be(0);
    }

    [Fact]
    public async Task NoPackagesProvided_AllPackagesUpToDate_ReturnsEmptyList()
    {
        // Arrange
        PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package1", [new("Version", "1.2.3")])
                   .WithItem("PackageReference", "Test.Package2", [new("Version", "2.1.0")]);
        })
            .Build();

        var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Test.Package1", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("1.2.3")); // Same as current version
        versionChooser
            .Setup(v => v.GetLatestVersionAsync("Test.Package2", It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("2.1.0")); // Same as current version

        var logger = new Mock<ILoggerWithColor>();

        // Act - Pass empty list to trigger "update all packages" behavior
        var packagesToUpdate = await PackageUpdateCommandRunner.GetPackagesToUpdateAsync(
            [],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packagesToUpdate.Should().BeEmpty();
        logger.Invocations.Count.Should().Be(0);
    }
}
