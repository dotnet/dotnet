// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package;
using NuGet.CommandLine.XPlat.Commands.Package.PackageDownload;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests;

public class PackageDownloadRunnerTests
{
    public static IEnumerable<object[]> PackageTestData()
    {
        // Basic stable explicit versions
        yield return new object[]
        {
            new List<(string, string)>
            {
                ("Contoso.Core", "1.0.0"),
                ("Contoso.Core", "1.1.0"),
                ("Contoso.Core", "2.0.0-beta")
            },                                      // source packages
            new List<(string, string)>
            {
                ("Contoso.Core", "1.1.0"),
                ("Contoso.Core", "1.0.0"),
            },                                      // argument packages
            false,                               // enablePrerelease
            "myOutput",                          // output directory subpath
            new List<(string, string)>
            {
                ("Contoso.Core", "1.1.0"),
                ("Contoso.Core", "1.0.0")
            }                                    // expected
        };

        // Basic stable explicit versions with different ids
        yield return new object[]
        {
            new List<(string, string)>
            {
                ("Contoso.Core", "1.0.0"),
                ("Contoso.Core.Utils", "1.1.0"),
                ("Contoso.Core", "2.0.0-beta")
            },                                      // source packages
            new List<(string, string)>
            {
                ("Contoso.Core.Utils", "1.1.0"),
                ("Contoso.Core", "1.0.0"),
            },                                      // argument packages
            false,                               // enablePrerelease
            "myOutput",                          // output directory subpath
            new List<(string, string)>
            {
                ("Contoso.Core.Utils", "1.1.0"),
                ("Contoso.Core", "1.0.0")
            }                                    // expected
        };

        // Mixed casing on the ID in the *download* argument 
        yield return new object[]
        {
            new List<(string, string)>
            {
                ("Contoso.Core", "1.1.0")
            },                                       // source packages      
            new List<(string, string)>
            {
                ("contoso.core", "1.1.0")
            },                                      // download argument
            false,                                  // enablePrerelease
            "",                                     // output directory subpath
            new List<(string, string)>
            {
                ("Contoso.Core", "1.1.0")
            },                                      // expected
        };

        // prerelease with IncludePrerelease == true
        yield return new object[]
        {
            new List<(string, string)>
            {
                ("Contoso.Preview", "1.3.0"),
                ("Contoso.Preview", "2.0.0-beta.2")
            },                                          // source packages
            new List<(string, string)>
            {
                ("Contoso.Preview", null),
            },                                          // download argument
            true,                                    // enablePrerelease
            "AnotherSubPath",                          // output directory subpath
            new List<(string, string)>
            {
                ("Contoso.Preview", "2.0.0-beta.2")
            },                                          // expected
        };

        // chose stable with IncludePrerelease == false
        yield return new object[]
        {
            new List<(string, string)>
            {
                ("Contoso.Preview", "1.3.2"),
                ("Contoso.Preview", "1.3.0"),
                ("Contoso.Preview", "2.0.0-beta.2")
            },                                                // source packages
            new List<(string, string)>
            {
                ("Contoso.Preview", null)
            },                                              // download argument
            false,                                           // enablePrerelease
            "SubPath",                                       // output directory subpath
            new List <(string, string) > {
                ("Contoso.Preview", "1.3.2")
            }                                                // expected
        };
    }

    [Theory]
    [MemberData(nameof(PackageTestData))]
    public async Task RunAsync_ExplicitVersionFromLocalFolderSource_SucceedsAsync(
        IReadOnlyList<(string, string)> sourcePackages,
        IReadOnlyList<(string, string)> argumentPackages,
        bool enablePrerelease,
        string outputDirectorySubPath,
        IReadOnlyList<(string, string)> expectedPackages)
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        var sourceDir = context.PackageSource;
        var outputDir = Path.Combine(context.WorkingDirectory, outputDirectorySubPath);

        foreach (var (id, version) in sourcePackages)
        {
            await SimpleTestPackageUtility.CreateFullPackageAsync(sourceDir, id, version);
        }

        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var settings = new Mock<ISettings>(MockBehavior.Loose);
        List<PackageWithNuGetVersion> packages = [];

        foreach (var (id, version) in argumentPackages)
        {
            packages.Add(new PackageWithNuGetVersion
            {
                Id = id,
                NuGetVersion = version == null ? null : NuGetVersion.Parse(version)
            });
        }

        var args = new PackageDownloadArgs()
        {
            Packages = packages,
            OutputDirectory = outputDir,
            IncludePrerelease = enablePrerelease,
        };

        // Act
        var result = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            [new(sourceDir)],
            settings.Object,
            CancellationToken.None);

        // Assert
        foreach (var (expectedId, expectedVersion) in expectedPackages)
        {
            result.Should().Be(PackageDownloadRunner.ExitCodeSuccess);
            var installDir = Path.Combine(outputDir, expectedId.ToLowerInvariant(), expectedVersion);
            Directory.Exists(installDir).Should().BeTrue();
            Directory.EnumerateFiles(installDir, "*.nupkg").Any().Should().BeTrue();
            File.Exists(Path.Combine(installDir, $"{expectedId.ToLowerInvariant()}.{expectedVersion}.nupkg")).Should().BeTrue();
        }
    }

    [Fact]
    public async Task RunAsync_NoVersion_PicksHighestAcrossMultipleSources()
    {
        // Arrange
        using var contextA = new SimpleTestPathContext();
        using var contextB = new SimpleTestPathContext();
        var srcA = contextA.PackageSource;
        var srcB = contextB.PackageSource;
        var outputDir = contextA.WorkingDirectory;

        var id = "Contoso.Toolkit";
        await SimpleTestPackageUtility.CreateFullPackageAsync(srcA, id, "1.1.0");
        await SimpleTestPackageUtility.CreateFullPackageAsync(srcB, id, "1.2.0");

        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var settings = new Mock<ISettings>(MockBehavior.Loose);

        var args = new PackageDownloadArgs()
        {
            Packages = [new PackageWithNuGetVersion { Id = id, NuGetVersion = null }],
            OutputDirectory = outputDir,
        };

        // Act
        var result = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            [new PackageSource(srcA), new PackageSource(srcB)],
            settings.Object,
            CancellationToken.None);

        // Assert
        result.Should().Be(PackageDownloadRunner.ExitCodeSuccess);

        var chosen = Path.Combine(outputDir, id.ToLowerInvariant(), "1.2.0");
        Directory.Exists(chosen).Should().BeTrue("should choose the highest version found across all sources");
        File.Exists(Path.Combine(chosen, $"{id.ToLowerInvariant()}.1.2.0.nupkg")).Should().BeTrue();
    }

    public static IEnumerable<object[]> ShortCircuitsPackageData =>
        [
        // single package already installed
        [
            new []
            {
                ("Contoso.Utils", "3.4.5")          // source packages
            },
            new[]
            {
                new PackageWithNuGetVersion              // argument packages
                {
                    Id = "Contoso.Utils",
                    NuGetVersion = NuGetVersion.Parse("3.4.5")
                }
            },
            new []
            {
                ("Contoso.Utils", "3.4.5")               // expected packages
            }
        ],

        // multiple packages already installed
        [
            new []
            {
                ("Contoso.Utils", "3.4.5"),            // source packages
                ("Contoso.Core", "3.0.5")
            },
            new []
            {
                new PackageWithNuGetVersion            // argument packages
                {
                    Id = "Contoso.Utils",
                    NuGetVersion = NuGetVersion.Parse("3.4.5")
                },
                new PackageWithNuGetVersion
                {
                    Id = "Contoso.Core",
                    NuGetVersion = NuGetVersion.Parse("3.0.5")
                }
            },
            new []
            {
                ("Contoso.Utils", "3.4.5"),             // expected packages
                ("Contoso.Core", "3.0.5")
            }
        ],

        // no version specified, but latest version already installed
        [
            new []
            {
                ("Contoso.Utils", "3.4.5"),            // source packages
                ("Contoso.Core", "3.0.5")
            },
            new []
            {
                new PackageWithNuGetVersion             // argument packages
                {
                    Id = "Contoso.Utils",
                    NuGetVersion = null
            },
                new PackageWithNuGetVersion
                {
                    Id = "Contoso.Core",
                    NuGetVersion = null
                }
            },
            new []
            {
                ("Contoso.Utils", "3.4.5"),              // expected packages
                ("Contoso.Core", "3.0.5")
            }
        ]];

    [Theory]
    [MemberData(nameof(ShortCircuitsPackageData))]
    internal async Task RunAsync_VersionAlreadyInstalled_ShortCircuitsAndSucceeds(
        IReadOnlyList<(string, string)> sourcePackages,
        PackageWithNuGetVersion[] packageDownloadArgs,
        IReadOnlyList<(string, string)> expectedPackages)
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        var sourceDir = context.PackageSource;
        var outputDir = context.WorkingDirectory;

        List<PackageWithNuGetVersion> packagesToInstall = [];

        foreach (var (id, version) in sourcePackages)
        {
            await SimpleTestPackageUtility.CreateFullPackageAsync(sourceDir, id, version);
        }

        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var settings = new Mock<ISettings>(MockBehavior.Loose);

        // First run: install explicit version
        var args1 = new PackageDownloadArgs()
        {
            Packages = packageDownloadArgs,
            LogLevel = LogLevel.Verbose,
            OutputDirectory = outputDir,
        };

        var first = await PackageDownloadRunner.RunAsync(
            args1,
            logger.Object,
            [new PackageSource(sourceDir)],
            settings.Object,
            CancellationToken.None);
        first.Should().Be(ExitCodes.Success);

        // Second run: should short-circuit because already installed
        var args2 = new PackageDownloadArgs()
        {
            Packages = packageDownloadArgs,
            OutputDirectory = outputDir,
        };

        // Act
        var second = await PackageDownloadRunner.RunAsync(
            args2,
            logger.Object,
            [new PackageSource(sourceDir)],
            settings.Object,
            CancellationToken.None);

        // Assert
        foreach (var (id, version) in expectedPackages)
        {
            second.Should().Be(PackageDownloadRunner.ExitCodeSuccess);
            var installDir = Path.Combine(outputDir, id.ToLowerInvariant(), version);
            Directory.Exists(installDir).Should().BeTrue();
            File.Exists(Path.Combine(installDir, $"{id.ToLowerInvariant()}.{version}.nupkg")).Should().BeTrue();
        }

        logger.Verify(l => l.LogMinimal(It.Is<string>(s => s.Contains("Skipping", StringComparison.OrdinalIgnoreCase))), Times.AtLeastOnce);
    }

    [Fact]
    public async Task RunAsync_WhenAllowInsecureConnectionsFalse_RejectsHttpSource()
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        var httpSource = "http://contoso/v3/index.json";
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var settings = new Mock<ISettings>(MockBehavior.Loose);

        var args = new PackageDownloadArgs()
        {
            Packages = [new PackageWithNuGetVersion { Id = "Contoso.Lib", NuGetVersion = null }],
        };

        // Act
        var result = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            [new PackageSource(httpSource)],
            settings.Object,
            CancellationToken.None);

        // Assert
        result.Should().Be(PackageDownloadRunner.ExitCodeError);
        logger.Verify(l => l.LogError(It.Is<string>(s => s.Contains(httpSource, StringComparison.OrdinalIgnoreCase))), Times.AtLeastOnce);
    }

    [Fact]
    public async Task RunAsync_PackageDoesNotExist_ReturnsError()
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        var id = "Missing.Package";
        var v = "9.9.9";
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var settings = new Mock<ISettings>(MockBehavior.Loose);

        var args = new PackageDownloadArgs()
        {
            Packages = [new PackageWithNuGetVersion { Id = id, NuGetVersion = NuGetVersion.Parse(v) }],
            OutputDirectory = context.WorkingDirectory,
        };

        // Act
        var result = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            [new PackageSource(context.PackageSource)],
            settings.Object,
            CancellationToken.None);

        // Assert
        result.Should().Be(PackageDownloadRunner.ExitCodeError);
        logger.Verify(l => l.LogError(It.IsAny<string>()), Times.AtLeastOnce);

        File.Exists(Path.Combine(context.WorkingDirectory, $"{id.ToLowerInvariant()}.{v}.nupkg"))
            .Should().BeFalse("Package does not exist in sources");
    }

    public static IEnumerable<object[]> Cases()
    {
        // Parameters:
        // A-packages, B-packages, sourceMappings, sourcesArgs, downloadId, downloadVersion,
        //expectSuccess, expectedInstalled

        // --source specified, mapping ignored, package only in A -> success
        yield return new object[]
        {
            new List<(string,string)> { ("Contoso.Lib", "1.0.0") }, // A
            new List<(string,string)>(),                            // B
            new List<(string,string)> { ("B", "Contoso.*") },       // mapping ignored
            new List<string> { "A" },                               // --source A
            "Contoso.Lib", "1.0.0",                                  // downloadId, downloadVersion
            true,                                                   // expect success
            ("Contoso.Lib", "1.0.0")                                // expectedInstalled
        };

        // no --source, mapping -> B, package only in B -> success
        yield return new object[]
        {
            new List<(string,string)>(),                            // A
            new List<(string,string)> { ("Contoso.Mapped", "2.0.0") }, // B
            new List<(string,string)> { ("B", "Contoso.*") },       // mapping -> B
            null,                                                   // no --source
            "Contoso.Mapped", "2.0.0",                              // downloadId, downloadVersion
            true,                                                   // expect success
            ("Contoso.Mapped", "2.0.0")                             // expectedInstalled
        };

        // no --source, mapping -> A, package only in B -> fail
        yield return new object[]
        {
            new List<(string,string)>(),                            // A
            new List<(string,string)> { ("Contoso.Mapped", "2.0.0") },
            new List<(string,string)> { ("A", "Contoso.*") },       // mapped to A
            null,
            "Contoso.Mapped", "2.0.0",
            false,
            null!
        };

        // no --source, mapping -> A&B, package in both A and B. Latest version from A installed
        yield return new object[]
        {
            new List<(string,string)> { ("Contoso.Mapped", "3.0.0") },                            // A
            new List<(string,string)> { ("Contoso.Mapped", "2.0.0") },                           // B
            new List<(string,string)> { ("A", "Contoso.*"), ("B", "Contoso.*") }, // mapped to A&B
            null,
            "Contoso.Mapped", null,
            true,
            ("Contoso.Mapped", "3.0.0")
        };

        // no --source, mapping -> A&B, package in both A and B. Latest version from B installed
        yield return new object[]
        {
            new List<(string,string)> { ("Contoso.Mapped", "2.0.0") },                            // A
            new List<(string,string)> { ("Contoso.Mapped", "3.0.0") },                           // B
            new List<(string,string)> { ("A", "Contoso.*"), ("B", "Contoso.*") }, // mapped to A&B
            null,
            "Contoso.Mapped", null,
            true,
            ("Contoso.Mapped", "3.0.0")
        };
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task RunAsync_WithSourceMapping(
        IReadOnlyList<(string id, string version)> sourceAPackages,
        IReadOnlyList<(string id, string version)> sourceBPackages,
        IReadOnlyList<(string source, string pattern)> sourceMappings,
        IReadOnlyList<string> sourcesArgs,
        string downloadId,
        string downloadVersion,
        bool expectSuccess,
        (string id, string version)? expectedInstalled)
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        string srcADirectory = Path.Combine(context.PackageSource, "SourceA");
        string srcBDirectory = Path.Combine(context.PackageSource, "SourceB");

        foreach (var (id, ver) in sourceAPackages)
        {
            await SimpleTestPackageUtility.CreateFullPackageAsync(srcADirectory, id, ver);
        }

        foreach (var (id, ver) in sourceBPackages)
        {
            await SimpleTestPackageUtility.CreateFullPackageAsync(srcBDirectory, id, ver);
        }

        // sources
        context.Settings.AddSource("A", srcADirectory);
        context.Settings.AddSource("B", srcBDirectory);

        // mapping
        foreach (var (src, pattern) in sourceMappings)
        {
            context.Settings.AddPackageSourceMapping(src, pattern);
        }

        var settings = Settings.LoadSettingsGivenConfigPaths([context.Settings.ConfigPath]);

        var packageSources = new List<PackageSource>
        {
            new(srcADirectory, "A"),
            new(srcBDirectory, "B")
        };

        // args
        var args = new PackageDownloadArgs
        {
            Packages =
            [
                new PackageWithNuGetVersion
                {
                    Id = downloadId,
                    NuGetVersion = downloadVersion is null ? null : NuGetVersion.Parse(downloadVersion)
                }
            ],
            OutputDirectory = context.WorkingDirectory,
            Sources = sourcesArgs == null ? [] : sourcesArgs.ToList()
        };

        string capturedLogs = string.Empty;
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        logger
            .Setup(l => l.LogError(It.IsAny<string>()))
            .Callback<string>(msg => capturedLogs += msg + Environment.NewLine);

        // Act
        var exit = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            packageSources,
            settings,
            CancellationToken.None);

        // Assert
        if (expectSuccess)
        {
            exit.Should().Be(PackageDownloadRunner.ExitCodeSuccess, because: capturedLogs);
            expectedInstalled.Should().NotBeNull();

            var (expId, expVer) = expectedInstalled!.Value;
            var installDir = Path.Combine(context.WorkingDirectory, expId.ToLowerInvariant(), expVer);
            Directory.Exists(installDir).Should().BeTrue();
            File.Exists(Path.Combine(installDir, $"{expId.ToLowerInvariant()}.{expVer}.nupkg")).Should().BeTrue();
        }
        else
        {
            exit.Should().Be(PackageDownloadRunner.ExitCodeError);
        }
    }

    [Fact]
    public async Task RunAsync_WhenSourceMappingMapsToInsecureSource_AndAllowInsecureConnectionsFalse_LogsErrorForMappedSource()
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        PackageSource source = new("http://contoso.test/v3/index.json", "InsecureMapped");
        context.Settings.AddSource(source.Name, source.SourceUri.OriginalString);
        context.Settings.AddPackageSourceMapping(source.Name, "Contoso.*");

        var settings = Settings.LoadSettingsGivenConfigPaths([context.Settings.ConfigPath]);

        var args = new PackageDownloadArgs
        {
            Packages =
            [
                new PackageWithNuGetVersion
                {
                    Id = "Contoso.Insecure",
                    NuGetVersion = NuGetVersion.Parse("1.0.0")
                }
            ],
            OutputDirectory = context.WorkingDirectory,
            AllowInsecureConnections = false,
        };

        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var packageSources = new List<PackageSource>
        {
            source
        };

        string expectedError = string.Format(
            CultureInfo.CurrentCulture,
            Strings.Error_HttpServerUsage,
            "package download",
            source);

        // Act
        var exit = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            packageSources,
            settings,
            CancellationToken.None);

        // Assert
        exit.Should().Be(PackageDownloadRunner.ExitCodeError);
        logger.Verify(
            l => l.LogError(expectedError),
            Times.Once);

    }

    [Fact]
    public async Task RunAsync_WhenSourceMappingMapsToSecureLocalSource_DoesNotLogErrorEvenWhenOtherSourceIsHttp()
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        var localSourcePath = context.PackageSource;
        var insecureSourceUrl = "http://contoso.test/v3/index.json";
        var id = "Contoso.LocalOnly";
        var version = "1.0.0";
        await SimpleTestPackageUtility.CreateFullPackageAsync(localSourcePath, id, version);
        context.Settings.AddSource("LocalSecure", localSourcePath);
        context.Settings.AddSource("InsecureOther", insecureSourceUrl);

        // map package to the secure local source only
        context.Settings.AddPackageSourceMapping("LocalSecure", "Contoso.*");
        var settings = Settings.LoadSettingsGivenConfigPaths([context.Settings.ConfigPath]);

        var args = new PackageDownloadArgs
        {
            Packages =
            [
                new PackageWithNuGetVersion
                {
                    Id = id,
                    NuGetVersion = NuGetVersion.Parse(version)
                }
            ],
            OutputDirectory = context.WorkingDirectory,
            AllowInsecureConnections = false,
        };

        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);

        var packageSources = new List<PackageSource>
        {
            new(localSourcePath, "LocalSecure"),
            new(insecureSourceUrl, "InsecureOther"),
        };

        // Act
        var exit = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            packageSources,
            settings,
            CancellationToken.None);

        // Assert
        exit.Should().Be(PackageDownloadRunner.ExitCodeSuccess);
        var installDir = Path.Combine(context.WorkingDirectory, id.ToLowerInvariant(), version);
        Directory.Exists(installDir).Should().BeTrue();
        File.Exists(Path.Combine(installDir, $"{id.ToLowerInvariant()}.{version}.nupkg")).Should().BeTrue();
    }

    [Fact]
    public async Task RunAsync_WhenSourceMappingEnabled_PackageMapsToNoSource_Fails()
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        PackageSource source = new(context.PackageSource, "source");
        context.Settings.AddSource(source.Name, source.SourceUri.OriginalString);
        context.Settings.AddPackageSourceMapping(source.Name, "Contoso.Not.*");
        var settings = Settings.LoadSettingsGivenConfigPaths([context.Settings.ConfigPath]);

        // package
        var id = "Contoso.Package";
        var version = "1.0.0";

        string expectedError = string.Format(
            CultureInfo.CurrentCulture,
            Strings.PackageDownloadCommand_PackageSourceMapping_NoSourcesMapped,
            id,
            source);

        // arguments
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        var packageSources = new List<PackageSource>
        {
            source
        };
        var args = new PackageDownloadArgs
        {
            Packages =
            [
                new PackageWithNuGetVersion
                {
                    Id = id,
                    NuGetVersion = NuGetVersion.Parse(version)
                }
            ],
            OutputDirectory = context.WorkingDirectory,
        };

        // Act
        var exit = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            packageSources,
            settings,
            CancellationToken.None);

        // Assert
        exit.Should().Be(PackageDownloadRunner.ExitCodeError);
        logger.Verify(
            l => l.LogError(expectedError),
            Times.Once);
    }

    [Fact]
    public async Task RunAsync_WhenSourceMappingMapsToInsecureSource_AndAllowInsecureConnectionsTrue_DownloadsWithoutError()
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        string srcDirectory = Path.Combine(context.PackageSource, "HttpSource");
        Directory.CreateDirectory(srcDirectory);

        var id = "Contoso.HttpMapped";
        var version = "1.0.0";
        await SimpleTestPackageUtility.CreateFullPackageAsync(srcDirectory, id, version);
        using var server = new FileSystemBackedV3MockServer(srcDirectory);
        server.Start();

        var mappedHttpSource = server.ServiceIndexUri;
        context.Settings.AddSource("MappedHttp", mappedHttpSource);

        context.Settings.AddPackageSourceMapping("MappedHttp", "Contoso.*");
        var settings = Settings.LoadSettingsGivenConfigPaths([context.Settings.ConfigPath]);

        var args = new PackageDownloadArgs
        {
            Packages =
            [
                new PackageWithNuGetVersion
                {
                    Id = id,
                    NuGetVersion = NuGetVersion.Parse(version)
                }
            ],
            OutputDirectory = context.WorkingDirectory,
            AllowInsecureConnections = true,
        };

        string capturedErrors = string.Empty;
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        logger
            .Setup(l => l.LogError(It.IsAny<string>()))
            .Callback<string>(msg => capturedErrors += msg + Environment.NewLine);

        var packageSources = new List<PackageSource>
        {
            new(mappedHttpSource, "MappedHttp"),
        };

        // Act
        var exit = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            packageSources,
            settings,
            CancellationToken.None);

        server.Stop();

        // Assert
        exit.Should().Be(PackageDownloadRunner.ExitCodeSuccess, because: capturedErrors);

        var installDir = Path.Combine(context.WorkingDirectory, id.ToLowerInvariant(), version);
        Directory.Exists(installDir).Should().BeTrue();
        File.Exists(Path.Combine(installDir, $"{id.ToLowerInvariant()}.{version}.nupkg")).Should().BeTrue();
    }

    [Fact]
    public async Task RunAsync_WhenMappedSourceMissing_LogsVerbose()
    {
        // Arrange
        using var context = new SimpleTestPathContext();
        string srcADirectory = Path.Combine(context.PackageSource, "SourceA");

        await SimpleTestPackageUtility.CreateFullPackageAsync(srcADirectory, "Contoso.Utils", "1.0.0");
        context.Settings.AddSource("A", srcADirectory);

        // Map the package to a NON-EXISTING source name "MissingSource"
        context.Settings.AddPackageSourceMapping("MissingSource", "Contoso.*");
        var settings = Settings.LoadSettingsGivenConfigPaths([context.Settings.ConfigPath]);

        var args = new PackageDownloadArgs
        {
            Packages =
            [
                new PackageWithNuGetVersion
                {
                    Id = "Contoso.Utils",
                    NuGetVersion = NuGetVersion.Parse("1.0.0")
                }
            ],
            OutputDirectory = context.WorkingDirectory,
            AllowInsecureConnections = true,
            Sources = []
        };
        string capturedVerbose = string.Empty;
        var logger = new Mock<ILoggerWithColor>(MockBehavior.Loose);
        logger.Setup(l => l.LogVerbose(It.IsAny<string>()))
              .Callback<string>(msg => capturedVerbose += msg + Environment.NewLine);

        // Act
        var exit = await PackageDownloadRunner.RunAsync(
            args,
            logger.Object,
            [new(srcADirectory, "A")],
            settings,
            CancellationToken.None);

        // Assert
        var expected = string.Format(
            CultureInfo.CurrentCulture,
            Strings.PackageDownloadCommand_PackageSourceMapping_NoSuchSource,
            "MissingSource",
            "Contoso.Utils");

        capturedVerbose.Should().Contain(expected, because: "a package mapped to a non-existing source name must log a verbose diagnostic");
    }
}
