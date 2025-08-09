// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
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
        var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
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
        var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
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
        var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
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
        var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packageToUpdate.Should().BeNull();
        logger.Invocations.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task RequestPackageNotReferencedByProject_ReturnsNullAndLogsError()
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
        var (packageToUpdate, frameworks) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
            [package],
            packageSpec,
            versionChooser.Object,
            NullSettings.Instance,
            logger.Object,
            CancellationToken.None);

        // Assert
        packageToUpdate.Should().BeNull();
        frameworks.Should().BeNull();

        // Verify that an error was logged
        logger.Verify(
            l => l.LogMinimal(
                It.Is<string>(message => message.Contains("NotReferenced.Package") && message.Contains("not referenced")),
                It.IsAny<ConsoleColor>()),
            Times.Once);
    }
}
