// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Configuration;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Update.PackageUpdateCommandRunnerTests;

using Pkg = XPlat.Commands.Package.PackageWithVersionRange;

public class MultiProjectTests
{
    [Fact]
    public async Task ProjectWithP2PReference_SinglePackage_UpdatesCorrectPackageVersion()
    {
        // Arrange
        var solutionDirectory = RuntimeEnvironmentHelper.IsWindows
            ? @"S:\path\to\repo\src"
            : @"/path/to/repo/src";
        var project1Path = Path.Combine(solutionDirectory, "ConsoleApp1", "ConsoleApp1.csproj");
        var project1 = new TestPackageSpecFactory(project1Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")])
                   .WithItem("ProjectReference", @"..\ClassLib1\ClassLib1.csproj", []);
        }).Build();

        var project2Path = Path.Combine(solutionDirectory, "ClassLib1", "ClassLib1.csproj");
        var project2 = new TestPackageSpecFactory(project2Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0");
        }).Build();

        DependencyGraphSpec dgSpec = new DependencyGraphSpec();
        dgSpec.AddProject(project1);
        dgSpec.AddProject(project2);
        dgSpec.AddRestore(project1Path);

        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "Test.Package", VersionRange = new VersionRange(new NuGetVersion("1.2.3")) }
        };

        TestData testData = InitTest(project1Path, packagesToUpdate, dgSpec);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        // Updated project1, but not project2
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project1Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Test.Package" && p.NewVersion.ToString() == "[1.2.3, )"),
            It.IsAny<ILogger>()),
            Times.Once);
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project2Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.IsAny<PackageUpdateCommandRunner.PackageToUpdate>(),
            It.IsAny<ILogger>()),
            Times.Never);

        // preview restore must have both projects, but only project1 for restore
        testData.IoMock.Verify(x => x.PreviewUpdatePackageReferenceAsync(
            It.Is<DependencyGraphSpec>(d => d.Projects.Count == 2 && d.Restore.Count == 1 && d.Restore[0] == project1Path),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        testData.LoggerMock.Verify(x => x.LogMinimal(
            It.Is<string>(s => s.Contains(string.Format(Strings.PackageUpdate_FinalSummary, 1, 1))),
            It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    private TestData InitTest(
        string updateTarget,
        IReadOnlyList<Pkg> packagesToUpdate,
        DependencyGraphSpec dgSpec,
        bool restoreSuccessful = true,
        bool disablePackageSourceMapping = true)
    {
        var commandArgs = new PackageUpdateArgs
        {
            Project = updateTarget,
            Packages = packagesToUpdate,
            Interactive = false,
            LogLevel = LogLevel.Information,
            Vulnerable = false,
        };

        var loggerMock = new Mock<ILoggerWithColor>();
        ILoggerWithColor logger = loggerMock.Object;

        var restoreResultMock = new Mock<IPackageUpdateIO.RestoreResult>();
        restoreResultMock.SetupGet(x => x.Success).Returns(restoreSuccessful);
        var restoreResult = restoreResultMock.Object;

        var ioMock = new Mock<IPackageUpdateIO>();

        ioMock.Setup(x => x.GetDependencyGraphSpec(commandArgs.Project)).Returns(dgSpec);

        ioMock.Setup(x => x.PreviewUpdatePackageReferenceAsync(
            It.IsAny<DependencyGraphSpec>(),
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

        if (disablePackageSourceMapping)
        {
            var noMappings = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);
            var packageSourceMapping = new PackageSourceMapping(noMappings);
            ioMock.Setup(x => x.GetPackageSourceMapping()).Returns(packageSourceMapping);
        }

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
