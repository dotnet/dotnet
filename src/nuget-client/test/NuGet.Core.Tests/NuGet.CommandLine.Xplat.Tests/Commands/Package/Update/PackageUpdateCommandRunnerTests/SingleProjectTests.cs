// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
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

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Update.PackageUpdateCommandRunnerTests;

using Pkg = XPlat.Commands.Package.Update.Package;

public class SingleProjectTests
{
    [Fact]
    public async Task SingleTarget_SinglePackage_UpdatesCorrectPackageVersion()
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
        }).Build();

        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "Test.Package", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) }
        };

        TestData testData = InitTest(packagesToUpdate, packageSpec);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Test.Package" && p.NewVersion.ToString() == "[1.2.3, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        testData.LoggerMock.Verify(x => x.LogMinimal(
            It.Is<string>(s => s.Contains(string.Format(Strings.PackageUpdate_FinalSummary, 1, 1))),
            It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    [Fact]
    public async Task SingleTarget_MultiplePackages_UpdatesBothPackages()
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package1", [new("Version", "1.0.0")])
                   .WithItem("PackageReference", "Test.Package2", [new("Version", "2.0.0")]);
        }).Build();

        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "Test.Package1", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) },
            new Pkg { Id = "Test.Package2", VersionRange = new VersionRange(new NuGetVersion("2.1.0")) }
        };

        TestData testData = InitTest(packagesToUpdate, packageSpec);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Test.Package1" && p.NewVersion.ToString() == "[1.2.3, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Test.Package2" && p.NewVersion.ToString() == "[2.1.0, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        testData.LoggerMock.Verify(x => x.LogMinimal(
            It.Is<string>(s => s.Contains(string.Format(Strings.PackageUpdate_FinalSummary, 2, 2))),
            It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    [Fact]
    public async Task MultiTargetingWithConditionalPackage_SinglePackage_UpdatesExpectedPackage()
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFrameworks", "net9.0;netstandard2.0")
                   .WithItem("PackageReference", "Contoso.Tools", [new("Version", "1.0.0")]);
        })
            .WithInnerBuild(builder =>
            {
                builder.WithProperty("TargetFramework", "net9.0");
            })
            .WithInnerBuild(builder =>
            {
                builder.WithProperty("TargetFramework", "netstandard2.0")
                       .WithItem("PackageReference", "Contoso.Polyfill", [new("Version", "1.0.0")]);
            }).Build();

        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "Contoso.Polyfill", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) }
        };

        TestData testData = InitTest(packagesToUpdate, packageSpec);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Contoso.Polyfill" && p.NewVersion.ToString() == "[1.2.3, )"),
            It.IsAny<ILogger>()), Times.Once);
    }

    [Fact]
    public async Task MultiTargetingWithTfmAliases_SinglePackage_UpdatesExpectedPackage()
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFrameworks", "dev;experimental");
        })
            .WithInnerBuild(builder =>
            {
                builder
                .WithProperty("TargetFramework", "dev")
                .WithProperty("TargetFrameworkIdentifier", ".NETCoreApp")
                .WithProperty("TargetFrameworkVersion", "v8.0")
                .WithProperty("TargetFrameworkMoniker", ".NETCoreApp,Version=v8.0")
                .WithItem("PackageReference", "Contoso.Polyfill", [new("Version", "1.0.0")]);
            })
            .WithInnerBuild(builder =>
            {
                builder
                .WithProperty("TargetFramework", "experimental")
                .WithProperty("TargetFrameworkIdentifier", ".NETCoreApp")
                .WithProperty("TargetFrameworkVersion", "v9.0")
                .WithProperty("TargetFrameworkMoniker", ".NETCoreApp,Version=v9.0");
            }).Build();

        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "Contoso.Polyfill", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) }
        };

        TestData testData = InitTest(packagesToUpdate, packageSpec);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.Is<List<string>>(p => p.Count == 1 && p[0] == "dev"),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Contoso.Polyfill" && p.NewVersion.ToString() == "[1.2.3, )"),
            It.IsAny<ILogger>()), Times.Once);
    }

    [Fact]
    public async Task PackageNotReferenced_ReturnsErrorExitCode()
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
        }).Build();
        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "Non.Existent.Package", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) }
        };
        TestData testData = InitTest(packagesToUpdate, packageSpec);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(3);

        testData.IoMock.Verify(x => x.PreviewUpdatePackageReferenceAsync(
            It.IsAny<DependencyGraphSpec>(),
            It.IsAny<SourceCacheContext>(),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()), Times.Never);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.IsAny<PackageUpdateCommandRunner.PackageToUpdate>(),
            It.IsAny<ILogger>()), Times.Never);
    }

    [Fact]
    public async Task PreviewRestoreFails_ProjectIsNotUpdated()
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
        }).Build();
        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "Test.Package", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) }
        };
        TestData testData = InitTest(packagesToUpdate, packageSpec, restoreSuccessful: false);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(3);

        testData.IoMock.Verify(x => x.PreviewUpdatePackageReferenceAsync(
            It.IsAny<DependencyGraphSpec>(),
            It.IsAny<SourceCacheContext>(),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()), Times.Once);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.IsAny<PackageUpdateCommandRunner.PackageToUpdate>(),
            It.IsAny<ILogger>()), Times.Never);
    }

    [Fact]
    public async Task ProjectCantBeLoaded_ReturnsErrorExitCode()
    {
        // Arrange
        var commandArgs = new PackageUpdateArgs
        {
            Project = "nonexistent.csproj",
            Packages = new List<Pkg>
            {
                new Pkg { Id = "Test.Package", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) }
            },
            Interactive = false,
            LogLevel = LogLevel.Information,
            Vulnerable = false,
        };
        var loggerMock = new Mock<ILoggerWithColor>();
        ILoggerWithColor logger = loggerMock.Object;

        var ioMock = new Mock<IPackageUpdateIO>(MockBehavior.Strict);
        ioMock.Setup(x => x.GetDependencyGraphSpec(It.IsAny<string>())).Returns<DependencyGraphSpec>(null);

        TestData testData = new TestData
        {
            CommandArgs = commandArgs,
            IoMock = ioMock,
            LoggerMock = loggerMock
        };

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(3);
    }

    [Fact]
    public async Task ProjectWithAuditDisabled_UpdateVulnerableShowsErrorMessage()
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithProperty("NuGetAudit", "false");
        }).Build();
        var packagesToUpdate = new List<Pkg>();

        TestData testData = InitTest(packagesToUpdate, packageSpec);
        testData = testData with
        {
            CommandArgs = testData.CommandArgs with { Vulnerable = true }
        };

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(PackageUpdateCommandRunner.ExitCodes.InvalidArgs);
        testData.LoggerMock.Verify(x => x.LogError(It.Is<string>(s => s.Contains(Strings.PackageUpdate_AuditDisabled))),
            Times.Once);
    }

    private TestData InitTest(IReadOnlyList<Pkg> packagesToUpdate, PackageSpec project, bool restoreSuccessful = true)
    {
        var commandArgs = new PackageUpdateArgs
        {
            Project = project.FilePath,
            Packages = packagesToUpdate,
            Interactive = false,
            LogLevel = LogLevel.Information,
            Vulnerable = false,
        };

        var loggerMock = new Mock<ILoggerWithColor>();
        ILoggerWithColor logger = loggerMock.Object;


        var dgSpec = new DependencyGraphSpec();
        dgSpec.AddProject(project);
        dgSpec.AddRestore(project.RestoreMetadata.ProjectUniqueName);

        var restoreResultMock = new Mock<IPackageUpdateIO.RestoreResult>();
        restoreResultMock.SetupGet(x => x.Success).Returns(restoreSuccessful);
        var restoreResult = restoreResultMock.Object;

        var ioMock = new Mock<IPackageUpdateIO>();

        ioMock.Setup(x => x.LoadSettings(It.IsAny<string>()))
            .Returns(NullSettings.Instance);

        ioMock.Setup(x => x.GetDependencyGraphSpec(commandArgs.Project)).Returns(dgSpec);

        ioMock.Setup(x => x.PreviewUpdatePackageReferenceAsync(
            It.IsAny<DependencyGraphSpec>(),
            It.IsAny<SourceCacheContext>(),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(restoreResult);

        ioMock.Setup(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.IsAny<PackageUpdateCommandRunner.PackageToUpdate>(),
            It.IsAny<ILogger>()));

        ioMock.Setup(x => x.CommitAsync(
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var testData = new TestData
        {
            CommandArgs = commandArgs,
            IoMock = ioMock,
            LoggerMock = loggerMock
        };
        return testData;
    }

    private Task<int> RunCommand(TestData testData, CancellationToken cancellationToken)
    {
        return PackageUpdateCommandRunner.Run(
            testData.CommandArgs,
            testData.LoggerMock.Object,
            testData.IoMock.Object,
            cancellationToken);
    }

    record TestData
    {
        public required PackageUpdateArgs CommandArgs { get; init; }
        public required Mock<IPackageUpdateIO> IoMock { get; init; }
        public required Mock<ILoggerWithColor> LoggerMock { get; init; }
    }
}
