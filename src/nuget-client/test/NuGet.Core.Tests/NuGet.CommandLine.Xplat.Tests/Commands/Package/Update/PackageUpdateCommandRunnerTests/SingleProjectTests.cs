// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Configuration;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Model;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Update.PackageUpdateCommandRunnerTests;

using Pkg = XPlat.Commands.Package.PackageWithVersionRange;
using Strings = NuGet.CommandLine.XPlat.Strings;

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
    public async Task PackageNotReferenced_ReturnsNothingToUpdateExitCode()
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
        exitCode.Should().Be(PackageUpdateCommandRunner.ExitCodes.NoPackagesNeedUpdating);

        testData.IoMock.Verify(x => x.PreviewUpdatePackageReferenceAsync(
            It.IsAny<DependencyGraphSpec>(),
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ProjectWithPackageSourceMapping_ErrorWhenPackageNameIsNotMapped(bool omitPackageName)
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
        }).Build();
        var packagesToUpdate = new List<Pkg>();

        TestData testData = InitTest(packagesToUpdate, packageSpec, disablePackageSourceMapping: false);
        if (!omitPackageName)
        {
            testData = testData with
            {
                CommandArgs = testData.CommandArgs with
                {
                    Packages = new List<Pkg>
                    {
                        new Pkg { Id = "Test.Package", VersionRange = null }
                    }
                }
            };
        }

        testData.IoMock.Setup(io => io.GetPackageSourceMapping())
            .Returns(new PackageSourceMapping(new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "contoso.*", new List<string> { "contoso-packages" } }
            }));

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(PackageUpdateCommandRunner.ExitCodes.Error);
        testData.LoggerMock.Verify(x => x.LogError(Messages.Error_PackageSourceMappingNotFound("Test.Package")),
            Times.Once);
        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.IsAny<PackageUpdateCommandRunner.PackageToUpdate>(),
            It.IsAny<ILogger>()),
            Times.Never);
    }

    [Theory]
    [InlineData("all")]
    [InlineData("direct")]
    public async Task VulnerablePackage_UpdatesToNonVulnerableVersion(string auditMode)
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFrameworks", "net9.0;net8.0")
                   .WithProperty("NuGetAudit", "true")
                   .WithProperty("NuGetAuditMode", auditMode);
        })
            .WithInnerBuild(builder =>
            {
                builder.WithProperty("TargetFramework", "net9.0")
                       .WithProperty("RuntimeIdentifiers", "winx64;linux-x64")
                       .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
            })
            .WithInnerBuild(builder =>
            {
                builder.WithProperty("TargetFramework", "net8.0")
                       .WithProperty("RuntimeIdentifiers", "winx64;linux-x64")
                       .WithItem("PackageReference", "Second.Package", [new("Version", "1.0.0")]);
            }).Build();

        var packagesToUpdate = new List<Pkg>();

        TestData testData = InitTest(packagesToUpdate, packageSpec);
        testData = testData with
        {
            CommandArgs = testData.CommandArgs with { Vulnerable = true }
        };

        // Mock the GetProjectAssetsFileAsync to return a LockFile with vulnerability log messages
        var lockFile = new LockFile
        {
            Version = 3,
            PackageSpec = packageSpec
        };

        // Add libraries to the lock file
        lockFile.Libraries.Add(new LockFileLibrary
        {
            Name = "Test.Package",
            Version = new NuGetVersion("1.0.0"),
            Type = "package"
        });

        lockFile.Libraries.Add(new LockFileLibrary
        {
            Name = "Second.Package",
            Version = new NuGetVersion("1.0.0"),
            Type = "package"
        });

        // Add a transitive package
        lockFile.Libraries.Add(new LockFileLibrary
        {
            Name = "Transitive.Package",
            Version = new NuGetVersion("1.0.0"),
            Type = "package"
        });

        // Add target for net9.0 with Test.Package
        var targetNet9 = new LockFileTarget
        {
            TargetFramework = packageSpec.TargetFrameworks[0].FrameworkName
        };
        targetNet9.Libraries.Add(new LockFileTargetLibrary
        {
            Name = "Test.Package",
            Version = new NuGetVersion("1.0.0"),
            Type = "package"
        });
        targetNet9.Libraries.Add(new LockFileTargetLibrary
        {
            Name = "Transitive.Package",
            Version = new NuGetVersion("1.0.0"),
            Type = "package"
        });
        lockFile.Targets.Add(targetNet9);

        // Add target for net8.0 with Second.Package
        var targetNet8 = new LockFileTarget
        {
            TargetFramework = packageSpec.TargetFrameworks[1].FrameworkName
        };
        targetNet8.Libraries.Add(new LockFileTargetLibrary
        {
            Name = "Second.Package",
            Version = new NuGetVersion("1.0.0"),
            Type = "package"
        });
        targetNet8.Libraries.Add(new LockFileTargetLibrary
        {
            Name = "Transitive.Package",
            Version = new NuGetVersion("1.0.0"),
            Type = "package"
        });
        lockFile.Targets.Add(targetNet8);

        // Add vulnerability log messages
        lockFile.LogMessages.Add(new AssetsLogMessage(
            LogLevel.Warning,
            NuGetLogCode.NU1903,
            "Test.Package has a known high severity vulnerability")
        {
            LibraryId = "Test.Package"
        });

        lockFile.LogMessages.Add(new AssetsLogMessage(
            LogLevel.Warning,
            NuGetLogCode.NU1903,
            "Second.Package has a known high severity vulnerability")
        {
            LibraryId = "Second.Package"
        });

        if (auditMode.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            lockFile.LogMessages.Add(new AssetsLogMessage(
                LogLevel.Warning,
                NuGetLogCode.NU1903,
                "Transitive.Package has a known high severity vulnerability")
            {
                LibraryId = "Transitive.Package"
            });
        }

        testData.IoMock.Setup(x => x.GetProjectAssetsFileAsync(
            It.IsAny<DependencyGraphSpec>(),
            It.IsAny<string>(),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(lockFile);

        // Mock GetKnownVulnerabilitiesAsync to return vulnerability data
        var vulnerabilityInfo = new PackageVulnerabilityInfo(
            new Uri("https://example.com/advisory"),
            PackageVulnerabilitySeverity.High,
            VersionRange.Parse("[1.0.0]"));

        var knownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
        {
            new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Test.Package", new List<PackageVulnerabilityInfo> { vulnerabilityInfo } },
                { "Second.Package", new List<PackageVulnerabilityInfo> { vulnerabilityInfo } },
                { "Transitive.Package", new List<PackageVulnerabilityInfo> { vulnerabilityInfo } }
            }
        };

        testData.IoMock.Setup(x => x.GetKnownVulnerabilitiesAsync(
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(knownVulnerabilities);

        testData.IoMock.Setup(x => x.GetNonVulnerableAsync(
            "Test.Package",
            It.IsAny<IReadOnlyList<string>>(),
            new NuGetVersion("1.0.0"),
            It.IsAny<ILogger>(),
            It.IsAny<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("1.2.3"));
        testData.IoMock.Setup(x => x.GetNonVulnerableAsync(
            "Second.Package",
            It.IsAny<IReadOnlyList<string>>(),
            new NuGetVersion("1.0.0"),
            It.IsAny<ILogger>(),
            It.IsAny<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("2.1.0"));
        testData.IoMock.Setup(x => x.GetNonVulnerableAsync(
            "Transitive.Package",
            It.IsAny<IReadOnlyList<string>>(),
            new NuGetVersion("1.0.0"),
            It.IsAny<ILogger>(),
            It.IsAny<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("2.0.0"));

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(0);

        testData.IoMock.Verify(x => x.GetKnownVulnerabilitiesAsync(
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        testData.IoMock.Verify(x => x.GetNonVulnerableAsync(
            "Test.Package",
            It.IsAny<IReadOnlyList<string>>(),
            new NuGetVersion("1.0.0"),
            It.IsAny<ILogger>(),
            It.IsAny<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        testData.IoMock.Verify(x => x.GetNonVulnerableAsync(
            "Second.Package",
            It.IsAny<IReadOnlyList<string>>(),
            new NuGetVersion("1.0.0"),
            It.IsAny<ILogger>(),
            It.IsAny<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        if (auditMode.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            testData.IoMock.Verify(x => x.GetNonVulnerableAsync(
                "Transitive.Package",
                It.IsAny<IReadOnlyList<string>>(),
                new NuGetVersion("1.0.0"),
                It.IsAny<ILogger>(),
                It.IsAny<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Test.Package" && p.NewVersion.ToString() == "[1.2.3, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        testData.IoMock.Verify(x => x.UpdatePackageReference(
            It.IsAny<PackageSpec>(),
            It.IsAny<IPackageUpdateIO.RestoreResult>(),
            It.IsAny<List<string>>(),
            It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Second.Package" && p.NewVersion.ToString() == "[2.1.0, )"),
            It.IsAny<ILogger>()),
            Times.Once);

        if (auditMode.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            testData.IoMock.Verify(x => x.UpdatePackageReference(
                It.IsAny<PackageSpec>(),
                It.IsAny<IPackageUpdateIO.RestoreResult>(),
                It.IsAny<List<string>>(),
                It.Is<PackageUpdateCommandRunner.PackageToUpdate>(p => p.Id == "Transitive.Package" && p.NewVersion.ToString() == "[2.0.0, )"),
                It.IsAny<ILogger>()),
                Times.Once);
        }

        int packageCount =
            auditMode.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? 3
            : 2;
        testData.LoggerMock.Verify(x => x.LogMinimal(
            It.Is<string>(s => s.Contains(string.Format(Strings.PackageUpdate_FinalSummary, packageCount, packageCount))),
            It.IsAny<ConsoleColor>()),
            Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ProjectWithPackageSourceMapping_PassesMappedSourcesToGetLatestVersion(bool omitPackageName)
    {
        // Arrange
        var packageSpec = new TestPackageSpecFactory(builder =>
        {
            builder.WithProperty("TargetFramework", "net9.0")
                   .WithItem("PackageReference", "Test.Package", [new("Version", "1.0.0")]);
        }).Build();
        var packagesToUpdate = new List<Pkg>();

        TestData testData = InitTest(packagesToUpdate, packageSpec, disablePackageSourceMapping: false);
        if (!omitPackageName)
        {
            testData = testData with
            {
                CommandArgs = testData.CommandArgs with
                {
                    Packages = new List<Pkg>
                    {
                        new Pkg { Id = "Test.Package", VersionRange = null }
                    }
                }
            };
        }

        List<string> mappedSources = ["package-feed"];
        testData.IoMock.Setup(io => io.GetPackageSourceMapping())
            .Returns(new PackageSourceMapping(new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "package-feed", ["test.package"]}
            }));

        testData.IoMock.Setup(x => x.GetLatestVersionAsync("Test.Package",
            It.IsAny<bool>(),
            It.IsAny<IReadOnlyList<string>>(),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NuGetVersion("1.2.3"));

        // Act
        int exitCode = await RunCommand(testData, CancellationToken.None);

        // Assert
        exitCode.Should().Be(PackageUpdateCommandRunner.ExitCodes.Success);

        // Make sure the mapped source was passed to GetLatestVersionAsync
        testData.IoMock.Verify(x => x.GetLatestVersionAsync(
            "Test.Package",
            false,
            It.Is<IReadOnlyList<string>>(sources => sources.Count == 1 && sources[0] == "package-feed"),
            It.IsAny<ILogger>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private TestData InitTest(
        IReadOnlyList<Pkg> packagesToUpdate,
        PackageSpec project,
        bool restoreSuccessful = true,
        bool disablePackageSourceMapping = true)
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
