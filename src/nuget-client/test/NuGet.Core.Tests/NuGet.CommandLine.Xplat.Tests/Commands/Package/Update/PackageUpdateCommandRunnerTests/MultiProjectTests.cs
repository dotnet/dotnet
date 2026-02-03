// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Configuration;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Model;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Update.PackageUpdateCommandRunnerTests;

using Pkg = XPlat.Commands.Package.PackageWithVersionRange;
using Strings = NuGet.CommandLine.XPlat.Strings;

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

    [Fact]
    public async Task UpgradeAllPackages_TwoProjectsWithSamePackage_UpdatesBothProjects()
    {
        // Arrange
        var solutionDirectory = RuntimeEnvironmentHelper.IsWindows
            ? @"S:\path\to\repo\src"
            : @"/path/to/repo/src";
        var project1Path = Path.Combine(solutionDirectory, "ConsoleApp1", "ConsoleApp1.csproj");
        var project1 = new TestPackageSpecFactory(project1Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
        }).Build();

        var project2Path = Path.Combine(solutionDirectory, "ClassLib1", "ClassLib1.csproj");
        var project2 = new TestPackageSpecFactory(project2Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
        }).Build();

        DependencyGraphSpec dgSpec = new DependencyGraphSpec();
        dgSpec.AddProject(project1);
        dgSpec.AddProject(project2);
        dgSpec.AddRestore(project1Path);
        dgSpec.AddRestore(project2Path);

        // no packages defined means update all packages
        var packagesToUpdate = new List<Pkg>();

        var latestPackageVersions = new Dictionary<string, string>
        {
            { "Test.Package", "2.0.0" }
        };

        TestData testData = InitTest(project1Path, packagesToUpdate, dgSpec, latestPackageVersions: latestPackageVersions);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        // Both projects should be updated
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project1Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Test.Package" && p.NewVersion.ToString() == "[2.0.0, )"),
            It.IsAny<ILogger>()),
            Times.Once);
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project2Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Test.Package" && p.NewVersion.ToString() == "[2.0.0, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        // preview restore must have both projects
        testData.IoMock.Verify(x => x.PreviewUpdatePackageReferenceAsync(
            It.Is<DependencyGraphSpec>(d => d.Projects.Count == 2 && d.Restore.Count == 2),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        int packagesScanned = dgSpec.Projects
            .SelectMany(p => p.TargetFrameworks)
            .SelectMany(tf => tf.Dependencies)
            .Select(d => d.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();
        int packagesUpdated = latestPackageVersions.Count;
        testData.LoggerMock.Verify(x => x.LogMinimal(
            It.Is<string>(s => s.Contains(string.Format(Strings.PackageUpdate_FinalSummary, packagesUpdated, packagesScanned))),
            It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    [Fact]
    public async Task UpgradePackageA_TwoProjectsWithPackageAOneWithout_UpdatesOnlyPackageA()
    {
        // Arrange
        var solutionDirectory = RuntimeEnvironmentHelper.IsWindows
            ? @"S:\path\to\repo\src"
            : @"/path/to/repo/src";
        var project1Path = Path.Combine(solutionDirectory, "ConsoleApp1", "ConsoleApp1.csproj");
        var project1 = new TestPackageSpecFactory(project1Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "PackageA", [new("Version", "1.0.0")]);
        }).Build();

        var project2Path = Path.Combine(solutionDirectory, "ClassLib1", "ClassLib1.csproj");
        var project2 = new TestPackageSpecFactory(project2Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "PackageA", [new("Version", "1.0.0")]);
        }).Build();

        var project3Path = Path.Combine(solutionDirectory, "ClassLib2", "ClassLib2.csproj");
        var project3 = new TestPackageSpecFactory(project3Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "PackageB", [new("Version", "2.0.0")]);
        }).Build();

        DependencyGraphSpec dgSpec = new DependencyGraphSpec();
        dgSpec.AddProject(project1);
        dgSpec.AddProject(project2);
        dgSpec.AddProject(project3);
        dgSpec.AddRestore(project1Path);
        dgSpec.AddRestore(project2Path);
        dgSpec.AddRestore(project3Path);

        var packagesToUpdate = new List<Pkg>
        {
            new Pkg { Id = "PackageA", VersionRange = new VersionRange(new NuGetVersion("3.0.0")) }
        };

        TestData testData = InitTest(project1Path, packagesToUpdate, dgSpec);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        // Projects 1 and 2 should be updated with PackageA
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project1Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "PackageA" && p.NewVersion.ToString() == "[3.0.0, )"),
            It.IsAny<ILogger>()),
            Times.Once);
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project2Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "PackageA" && p.NewVersion.ToString() == "[3.0.0, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        // Project 3 should not be updated (it doesn't have PackageA)
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project3Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.IsAny<PackageUpdateCommandRunner.PackageToUpdate>(),
            It.IsAny<ILogger>()),
            Times.Never);

        // preview restore must have all three projects
        testData.IoMock.Verify(x => x.PreviewUpdatePackageReferenceAsync(
            It.Is<DependencyGraphSpec>(d => d.Projects.Count == 3 && d.Restore.Count == 3),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        int packagesScanned = testData.CommandArgs.Packages.Count;
        testData.LoggerMock.Verify(x => x.LogMinimal(
            It.Is<string>(s => s.Contains(string.Format(Strings.PackageUpdate_FinalSummary, 1, packagesScanned))),
            It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    [Fact]
    public async Task UpgradeVulnerable_TwoProjects_OneVulnerableOneNot_UpdatesOnlyVulnerablePackageToLowestSafeVersion()
    {
        // Arrange
        var solutionDirectory = RuntimeEnvironmentHelper.IsWindows
            ? @"S:\path\to\repo\src"
            : @"/path/to/repo/src";
        var project1Path = Path.Combine(solutionDirectory, "ConsoleApp1", "ConsoleApp1.csproj");
        var project1 = new TestPackageSpecFactory(project1Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "VulnerablePackage", [new("Version", "1.0.0")]);
        }).Build();

        var project2Path = Path.Combine(solutionDirectory, "ClassLib1", "ClassLib1.csproj");
        var project2 = new TestPackageSpecFactory(project2Path, builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "SafePackage", [new("Version", "1.0.0")]);
        }).Build();

        DependencyGraphSpec dgSpec = new DependencyGraphSpec();
        dgSpec.AddProject(project1);
        dgSpec.AddProject(project2);
        dgSpec.AddRestore(project1Path);
        dgSpec.AddRestore(project2Path);

        var packagesToUpdate = new List<Pkg>();

        TestData testData = InitTest(project1Path, packagesToUpdate, dgSpec);
        testData = testData with
        {
            CommandArgs = testData.CommandArgs with { Vulnerable = true }
        };

        var knownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
        {
            new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "VulnerablePackage",
                    new List<PackageVulnerabilityInfo>
                    {
                        new PackageVulnerabilityInfo(
                            new Uri("https://cve.test/vulnerability1"),
                            PackageVulnerabilitySeverity.High,
                            VersionRange.Parse("[1.0.0, 2.0.0)"))
                    }
                }
            }
        };

        testData.IoMock.Setup(x => x.GetKnownVulnerabilitiesAsync(
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(knownVulnerabilities);

        testData.IoMock.Setup(x => x.GetNonVulnerableAsync(
            "VulnerablePackage",
            It.IsAny<IReadOnlyList<string>>(),
            new NuGetVersion("1.0.0"),
            It.IsAny<ILogger>(),
            knownVulnerabilities,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("2.0.0"));

        var lockFile = new LockFile
        {
            Version = 3,
            PackageSpec = project1,
            LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1902, "Package 'VulnerablePackage' 1.0.0 has a known moderate severity vulnerability")
                {
                    LibraryId = "VulnerablePackage"
                }
            },
            Libraries = new List<LockFileLibrary>
            {
                new LockFileLibrary
                {
                    Name = "VulnerablePackage",
                    Version = new NuGetVersion("1.0.0"),
                    Type = "package"
                }
            },
            Targets = new List<LockFileTarget>
            {
                new LockFileTarget
                {
                    TargetFramework = NuGetFramework.Parse("net9.0"),
                    Libraries = new List<LockFileTargetLibrary>
                    {
                        new LockFileTargetLibrary
                        {
                            Name = "VulnerablePackage",
                            Version = new NuGetVersion("1.0.0"),
                            Type = "package"
                        }
                    }
                }
            }
        };

        var project2LockFile = new LockFile
        {
            Version = 3,
            PackageSpec = project2,
            LogMessages = new List<IAssetsLogMessage>(),
            Libraries = new List<LockFileLibrary>
            {
                new LockFileLibrary
                {
                    Name = "SafePackage",
                    Version = new NuGetVersion("1.0.0"),
                    Type = "package"
                }
            },
            Targets = new List<LockFileTarget>
            {
                new LockFileTarget
                {
                    TargetFramework = NuGetFramework.Parse("net9.0"),
                    Libraries = new List<LockFileTargetLibrary>
                    {
                        new LockFileTargetLibrary
                        {
                            Name = "SafePackage",
                            Version = new NuGetVersion("1.0.0"),
                            Type = "package"
                        }
                    }
                }
            }
        };

        testData.IoMock.Setup(x => x.GetProjectAssetsFileAsync(
            It.IsAny<DependencyGraphSpec>(),
            project1Path,
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(lockFile);

        testData.IoMock.Setup(x => x.GetProjectAssetsFileAsync(
            It.IsAny<DependencyGraphSpec>(),
            project2Path,
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(project2LockFile);

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        // Only project1 should be updated with VulnerablePackage to the safe version 2.0.0
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project1Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "VulnerablePackage" && p.NewVersion.ToString() == "[2.0.0, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        // Project2 should not be updated (SafePackage is not vulnerable)
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.Is<PackageSpec>(p => p.FilePath == project2Path),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.IsAny<PackageUpdateCommandRunner.PackageToUpdate>(),
            It.IsAny<ILogger>()),
            Times.Never);

        // Verify GetNonVulnerableAsync was called to find the lowest safe version
        testData.IoMock.Verify(x => x.GetNonVulnerableAsync(
            "VulnerablePackage",
            It.IsAny<IReadOnlyList<string>>(),
            new NuGetVersion("1.0.0"),
            It.IsAny<ILogger>(),
            knownVulnerabilities,
            It.IsAny<CancellationToken>()),
            Times.Once);

        var summaryMessage = string.Format(Strings.PackageUpdate_FinalSummary, 1, 2);
        testData.LoggerMock.Verify(x => x.LogMinimal(
            It.Is<string>(s => s.Contains(summaryMessage)),
            It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    private TestData InitTest(
        string updateTarget,
        IReadOnlyList<Pkg> packagesToUpdate,
        DependencyGraphSpec dgSpec,
        bool restoreSuccessful = true,
        bool disablePackageSourceMapping = true,
        Dictionary<string, string>? latestPackageVersions = null)
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

        if (latestPackageVersions is not null)
        {
            ioMock.Setup(x => x.GetLatestVersionAsync(
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<IReadOnlyList<string>>(),
                It.IsAny<ILogger>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((string id, bool prerelease, IReadOnlyList<string> sources, ILogger logger, CancellationToken ct) =>
                {
                    if (latestPackageVersions.TryGetValue(id, out var versionString))
                    {
                        var version = NuGetVersion.Parse(versionString);
                        return version;
                    }
                    return null;
                });
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
