// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.Test
{
    public class RestoreLoggingTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RestoreLoggingTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task RestoreLogging_VerifyNU1605DowngradeWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var packageZ1 = new SimpleTestPackageContext("z", "1.5.0");
                var packageZ2 = new SimpleTestPackageContext("z", "2.0.0");
                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                packageX.Dependencies.Add(packageZ2);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX, packageZ1, packageZ2, packageZ1);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var doc = projectA.GetXML();

                // z 1.*
                ProjectFileUtils.AddItem(doc,
                    "PackageReference", "z",
                    NuGetFramework.Parse("netcoreapp1.0"),
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { "Version", "1.*" } });

                // x *
                ProjectFileUtils.AddItem(doc,
                    "PackageReference", "x",
                    NuGetFramework.Parse("netcoreapp1.0"),
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { "Version", "*" } });

                doc.Save(projectA.ProjectPath);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                var message = projectA.AssetsFile.LogMessages.Single(e => e.Code == NuGetLogCode.NU1605);
                message.Level.Should().Be(LogLevel.Warning);

                // Verify message contains the actual 1.5.0 version instead of the lower bound of 1.0.0.
                message.Message.Should().Contain("Detected package downgrade: z from 2.0.0 to 1.5.0. Reference the package directly from the project to select a different version.");

                // Verify that x display the version instead of the range which is >= 0.0.0
                message.Message.Should().Contain("a -> x 1.0.0 -> z (>= 2.0.0)");

                // Verify non-snapshot range is displayed for the downgradedBy path.
                message.Message.Should().Contain("a -> z (>= 1.0.0)");
            }
        }

        [Fact]
        public async Task RestoreLogging_VerifyNU1608MessageAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var packageX = new SimpleTestPackageContext("x", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>x</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""z"" version=""[1.0.0]"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageZ1 = new SimpleTestPackageContext("z", "1.0.0");
                var packageZ2 = new SimpleTestPackageContext("z", "2.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX, packageZ1, packageZ2);

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageZ2);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);
                var log = projectA.AssetsFile.LogMessages.SingleOrDefault(e => e.Code == NuGetLogCode.NU1608);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1107");
                r.AllOutput.Should().Contain("NU1608");
                log.FilePath.Should().Be(projectA.ProjectPath);
                log.LibraryId.Should().Be("z");
                log.Level.Should().Be(LogLevel.Warning);
                log.TargetGraphs.Select(e => string.Join(",", e)).Should().Contain(netcoreapp1.DotNetFrameworkName);
                log.Message.Should().Contain("Detected package version outside of dependency constraint: x 1.0.0 requires z (= 1.0.0) but version z 2.0.0 was resolved.");
            }
        }

        [Fact]
        public async Task RestoreLogging_MissingNuspecInSource_FailsWithNU5037Async()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX);

                File.Delete(Path.Combine(pathContext.PackageSource, packageX.Id, packageX.Version, packageX.Id + NuGetConstants.ManifestExtension));

                var logger = new TestLogger();
                // Set-up command.
                var request = ProjectTestHelpers.CreateRestoreRequest(
                        pathContext,
                        logger,
                        ProjectTestHelpers.GetPackageSpec("Project1", pathContext.SolutionRoot, framework: "net5.0", dependencyName: packageX.Id));
                var command = new NuGet.Commands.RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();

                // Assert
                result.Success.Should().BeFalse(because: logger.ShowMessages());
                result.LockFile.LogMessages.Select(e => e.Code).Should().AllBeEquivalentTo(NuGetLogCode.NU5037);
                logger.ShowMessages().Should().Contain("The package is missing the required nuspec file. Path: " + Path.Combine(pathContext.PackageSource, packageX.Id, packageX.Version));
            }
        }

        [Fact]
        public async Task RestoreLogging_MissingNuspecInGlobalPackages_FailsWithNU5037Async()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = FrameworkConstants.CommonFrameworks.NetCoreApp10;

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var packageX = new SimpleTestPackageContext("x", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX);
                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act                
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);
                //delete the project.assets file to avoid no-op restore
                File.Delete(projectA.AssetsFileOutputPath);
                File.Delete(Path.Combine(pathContext.UserPackagesFolder, packageX.Id, packageX.Version, packageX.Id + NuGetConstants.ManifestExtension));
                r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("The package is missing the required nuspec file. Path: " + Path.Combine(pathContext.UserPackagesFolder, packageX.Id, packageX.Version));
            }
        }

        [Fact]
        public async Task RestoreLogging_VerifyNU1107DoesNotDisplayNU1608AlsoAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);
                projectA.Properties.Add("WarningsAsErrors", "NU1608");

                var packageX = new SimpleTestPackageContext("x", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>x</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""z"" version=""[1.0.0]"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageY = new SimpleTestPackageContext("y", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>y</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""z"" version=""[2.0.0]"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageZ1 = new SimpleTestPackageContext("z", "1.0.0");
                var packageZ2 = new SimpleTestPackageContext("z", "2.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX, packageY, packageZ1, packageZ2);

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1107");
                r.AllOutput.Should().NotContain("NU1608");
            }
        }

        [Fact]
        public void RestoreLogging_VerifyCompatErrorNU1201Properties()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);
                var log = projectA.AssetsFile.LogMessages.SingleOrDefault(e => e.Code == NuGetLogCode.NU1201 && e.TargetGraphs.All(g => !g.Contains("/")));

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1201");
                log.FilePath.Should().Be(projectA.ProjectPath);
                log.LibraryId.Should().Be("b");
                log.Level.Should().Be(LogLevel.Error);
                log.TargetGraphs.Should().BeEquivalentTo(new[] { netcoreapp1.DotNetFrameworkName });
                log.Message.Should().Be("Project b is not compatible with netcoreapp1.0 (.NETCoreApp,Version=v1.0). Project b supports: netcoreapp2.0 (.NETCoreApp,Version=v2.0)");
            }
        }

        [Fact]
        public async Task RestoreLogging_VerifyCompatErrorNU1202PropertiesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                packageX.Files.Clear();
                packageX.AddFile("lib/netcoreapp2.0/a.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);
                var log = projectA.AssetsFile.LogMessages.SingleOrDefault(e => e.Code == NuGetLogCode.NU1202 && e.TargetGraphs.All(g => !g.Contains("/")));

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1202");
                log.FilePath.Should().Be(projectA.ProjectPath);
                log.LibraryId.Should().Be("x");
                log.Level.Should().Be(LogLevel.Error);
                log.TargetGraphs.Should().BeEquivalentTo(new[] { netcoreapp1.DotNetFrameworkName });
                log.Message.Should().Be("Package x 1.0.0 is not compatible with netcoreapp1.0 (.NETCoreApp,Version=v1.0). Package x 1.0.0 supports: netcoreapp2.0 (.NETCoreApp,Version=v2.0)");
            }
        }

        [Fact]
        public async Task RestoreLogging_VerifyCompatErrorNU1203PropertiesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);
                projectA.Properties.Add("ValidateRuntimeIdentifierCompatibility", "true");
                projectA.Properties.Add("RuntimeIdentifiers", "win10-x64");

                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                packageX.Files.Clear();
                packageX.AddFile("ref/netcoreapp1.0/a.dll"); // ref with a missing runtime

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);
                var log = projectA.AssetsFile.LogMessages.OrderBy(e => e.Message, StringComparer.Ordinal).FirstOrDefault(e => e.Code == NuGetLogCode.NU1203);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1203");
                log.FilePath.Should().Be(projectA.ProjectPath);
                log.LibraryId.Should().Be("x");
                log.Level.Should().Be(LogLevel.Error);
                log.TargetGraphs.Single().Should().Contain(netcoreapp1.DotNetFrameworkName);
                log.Message.Should().Contain("x 1.0.0 provides a compile-time reference assembly for a on .NETCoreApp,Version=v1.0, but there is no run-time assembly compatible with");
            }
        }

        [Fact]
        public async Task RestoreLogging_VerifyCircularDependencyErrorNU1106PropertiesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                var packageY = new SimpleTestPackageContext("y", "1.0.0");
                packageX.Dependencies.Add(packageY);
                packageY.Dependencies.Add(packageX);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX, packageY);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);
                var log = projectA.AssetsFile.LogMessages.SingleOrDefault(e => e.Code == NuGetLogCode.NU1108 && e.TargetGraphs.All(g => !g.Contains("/")));

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1108");
                log.FilePath.Should().Be(projectA.ProjectPath);
                log.LibraryId.Should().Be("x");
                log.Level.Should().Be(LogLevel.Error);
                log.TargetGraphs.Single().Should().Contain(netcoreapp1.DotNetFrameworkName);
                log.Message.Should().Contain("a -> x 1.0.0 -> y 1.0.0 -> x (>= 1.0.0)");
            }
        }

        [Fact]
        public async Task RestoreLogging_VerifyConflictErrorNU1107PropertiesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var packageX = new SimpleTestPackageContext("x", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>x</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""z"" version=""[1.0.0]"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageY = new SimpleTestPackageContext("y", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>y</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""z"" version=""[2.0.0]"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageZ1 = new SimpleTestPackageContext("z", "1.0.0");
                var packageZ2 = new SimpleTestPackageContext("z", "2.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX, packageY, packageZ1, packageZ2);

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);
                var log = projectA.AssetsFile.LogMessages.SingleOrDefault(e => e.Code == NuGetLogCode.NU1107 && e.TargetGraphs.All(g => !g.Contains("/")));

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1107");
                log.FilePath.Should().Be(projectA.ProjectPath);
                log.LibraryId.Should().Be("z");
                log.Level.Should().Be(LogLevel.Error);
                log.TargetGraphs.Single().Should().Contain(netcoreapp1.DotNetFrameworkName);
                log.Message.Should().Contain("Version conflict detected for z");
                log.Message.Should().Contain("a -> y 1.0.0 -> z (= 2.0.0)");
                log.Message.Should().Contain("a -> x 1.0.0 -> z (= 1.0.0).");
            }
        }

        [Fact]
        public async Task RestoreLogging_VerifyConflictErrorNU1107IsResolvedByTopLevelReferenceAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1);

                var packageX = new SimpleTestPackageContext("x", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>x</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""z"" version=""[1.0.0]"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageY = new SimpleTestPackageContext("y", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>y</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""z"" version=""[2.0.0]"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageZ1 = new SimpleTestPackageContext("z", "1.0.0");
                var packageZ2 = new SimpleTestPackageContext("z", "2.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX, packageY, packageZ1, packageZ2);

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);

                // This reference solves the conflict
                projectA.AddPackageToAllFrameworks(packageZ1);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreLogging_WarningsContainNuGetLogCodesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("WARNING: NU1603:");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_WarningsAsErrorsFailsRestoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Theory]
        [InlineData("NU1603")]
        [InlineData("$(NoWarn);NU1603")]
        [InlineData("NU1603;$(NoWarn);")]
        [InlineData("NU1603;NU1701")]
        [InlineData("NU1603,NU1701")]
        public async Task RestoreLogging_NetCore_NoWarnRemovesWarningAsync(string noWarn)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("NoWarn", noWarn);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Theory]
        [InlineData("NU1603")]
        [InlineData("$(NoWarn);NU1603")]
        [InlineData("NU1603;$(NoWarn);")]
        [InlineData("NU1603;NU1701")]
        [InlineData("NU1603,NU1701")]
        public async Task RestoreLogging_NetCore_WarningsAsErrorsForSpecificWarningFailsAsync(string warnAsError)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "false");
                projectA.Properties.Add("WarningsAsErrors", warnAsError);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_WarningsAsErrorsForSpecificWarningOfAnotherTypeIgnoredAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "false");
                projectA.Properties.Add("WarningsAsErrors", "NU1602;NU1701");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
                r.AllOutput.Should().NotContain("NU1602");
                r.AllOutput.Should().NotContain("NU1701");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_NoWarnWithTreatWarningsAsErrorRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Properties.Add("NoWarn", "NU1603");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_DifferentNoWarnWithTreatWarningsAsErrorFailsRestoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Properties.Add("NoWarn", "NU1107");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_NoWarnWithWarnSpecificAsErrorRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("WarningsAsErrors", "NU1603");
                projectA.Properties.Add("NoWarn", "NU1603");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_DifferentNoWarnWithWarnSpecificAsErrorFailsRestoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("WarningsAsErrors", "NU1603");
                projectA.Properties.Add("NoWarn", "NU1107");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_PackageSpecificNoWarnRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_WithMultiTargeting_AllTfmPackageSpecificNoWarnRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net45 = NuGetFramework.Parse("net45");
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2,
                    net45);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_WithMultiTargeting_PartialTfmPackageSpecificNoWarnRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net45 = NuGetFramework.Parse("net45");
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2,
                    net45);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToFramework("net45", packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_PackageSpecificDifferentNoWarnDoesNotRemoveWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1107"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
            }
        }


        [Fact]
        public async Task RestoreLogging_NetCore_PackageSpecificNoWarnAndTreatWarningsAsErrorsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_NetCore_PackageSpecificNoWarnAndTreatSpecificWarningsAsErrorsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("WarningsAsErrors", "NU1603");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_WarningsContainNuGetLogCodesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("WARNING: NU1603:");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_WarningsAsErrorsFailsRestoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Theory]
        [InlineData("NU1603")]
        [InlineData("$(NoWarn);NU1603")]
        [InlineData("NU1603;$(NoWarn);")]
        [InlineData("NU1603;NU1701")]
        [InlineData("NU1603,NU1701")]
        public async Task RestoreLogging_Legacy_NoWarnRemovesWarningAsync(string noWarn)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("NoWarn", noWarn);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Theory]
        [InlineData("NU1603")]
        [InlineData("$(NoWarn);NU1603")]
        [InlineData("NU1603;$(NoWarn);")]
        [InlineData("NU1603;NU1701")]
        [InlineData("NU1603,NU1701")]
        public async Task RestoreLogging_Legacy_WarningsAsErrorsForSpecificWarningFailsAsync(string warnAsError)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "false");
                projectA.Properties.Add("WarningsAsErrors", warnAsError);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_WarningsAsErrorsForSpecificWarningOfAnotherTypeIgnoredAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "false");
                projectA.Properties.Add("WarningsAsErrors", "NU1602;NU1701");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
                r.AllOutput.Should().NotContain("NU1602");
                r.AllOutput.Should().NotContain("NU1701");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_NoWarnWithTreatWarningsAsErrorRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Properties.Add("NoWarn", "NU1603");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_DifferentNoWarnWithTreatWarningsAsErrorFailsRestoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Properties.Add("NoWarn", "NU1107");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_NoWarnWithWarnSpecificAsErrorRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("WarningsAsErrors", "NU1603");
                projectA.Properties.Add("NoWarn", "NU1603");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_DifferentNoWarnWithWarnSpecificAsErrorFailsRestoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("WarningsAsErrors", "NU1603");
                projectA.Properties.Add("NoWarn", "NU1107");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_PackageSpecificNoWarnRemovesWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_PackageSpecificDifferentNoWarnDoesNotRemoveWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1107"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
            }
        }


        [Fact]
        public async Task RestoreLogging_Legacy_PackageSpecificNoWarnAndTreatWarningsAsErrorsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("TreatWarningsAsErrors", "true");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public async Task RestoreLogging_Legacy_PackageSpecificNoWarnAndTreatSpecificWarningsAsErrorsAsync()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("WarningsAsErrors", "NU1603");

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX9 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "9.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX9);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        public void RestoreLogging_PackagesLockFile_InvalidInputError()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestorePackagesWithLockFile", "false");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                File.Create(projectA.NuGetLockFileOutputPath).Close();

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1005");
            }
        }

        [Fact]
        public async Task RestoreLogging_PackagesLockFile_RestoreLockedModeErrorAsync()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0",
                };

                projectA.AddPackageToAllFrameworks(packageX);

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX, packageY);

                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, testOutputHelper: _testOutputHelper);

                projectA.AddPackageToAllFrameworks(packageY);
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();

                // Act
                r = Util.RestoreSolution(pathContext, expectedExitCode: 1, testOutputHelper: _testOutputHelper);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1004");
                var logCodes = projectA.AssetsFile.LogMessages.Select(e => e.Code);
                logCodes.Should().Contain(NuGetLogCode.NU1004);
            }
        }
    }
}
