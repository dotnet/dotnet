// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class RestoreTransitiveLoggingTests
    {
        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceNoWarnsVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[AssemblyName=Test] -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceNoWarnsAndParentProjectContainsAssemblyNameVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.Properties.Add("AssemblyName", "TestAssemblyName");
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //                                          -> PkgY          -> PkgZ v 1.0.1
        //                                          -> PkgZ v 1.0.0
        public async Task GivenAProjectReferenceDoesNotNoWarnForAllWarningsVerifyWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced indirectly and Created in the source
                var packageZ11 = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.1"
                };

                // Referenced directly and Created in the source
                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                // Referenced directly and Created in the source
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageY.Dependencies.Add(packageZ11);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageZ);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageY);

                // B -> X
                // B -> Y -> Z v1.0.1
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(packageY);
                projectB.AddPackageToAllFrameworks(packageZ);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1605");
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[TreatWarningsAsErrors true] -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //                                          -> PkgY          -> PkgZ v 1.0.1
        //                                          -> PkgZ v 1.0.0
        public async Task GivenAProjectReferenceDoesNotNoWarnForAllWarningsAndDirectTreatWarningsAsErrorsVerifyErrorAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced indirectly and Created in the source
                var packageZ11 = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.1"
                };

                // Referenced directly and Created in the source
                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                // Referenced directly and Created in the source
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageY.Dependencies.Add(packageZ11);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageZ);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageY);

                // B -> X
                // B -> Y -> Z v1.0.1
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(packageY);
                projectB.AddPackageToAllFrameworks(packageZ);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1605");
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[WarningsAsErrors NU1605, NU1603] -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //                                              -> PkgY          -> PkgZ v 1.0.1
        //                                              -> PkgZ v 1.0.0
        public async Task GivenAProjectReferenceDoesNotNoWarnForAllWarningsAndDirectWarningsAsErrorsVerifyErrorAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced indirectly and Created in the source
                var packageZ11 = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.1"
                };

                // Referenced directly and Created in the source
                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                // Referenced directly and Created in the source
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageY.Dependencies.Add(packageZ11);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageZ);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageY);

                // B -> X
                // B -> Y -> Z v1.0.1
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(packageY);
                projectB.AddPackageToAllFrameworks(packageZ);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("WarningsAsErrors", "NU1603; NU1605");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1605");
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[WarnAsError NU1603] -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceNoWarnsAndDirectWarnAsErrorVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("WarningsAsErrors", "NU1603; NU1701");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[TreatWarningsAsError true] -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceNoWarnsAndDirectTreatWarningsAsErrorVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[WarnAsError NU1603] -> ProjB -> PkgX[NU1603]
        public async Task GivenAProjectReferenceDoesNotNoWarnAndDirectWarnAsErrorVerifyErrorAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("WarningsAsErrors", "NU1603; NU1701");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[TreatWarningsAsError true] -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //                                                                     -> PkgY[NU1603]
        public async Task GivenAProjectReferenceDoesNotNoWarnForAllReferencesAndDirectTreatWarningsAsErrorVerifyErrorAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced but not created
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageY11 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageY11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(packageY);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[TreatWarningsAsError true] -> ProjB[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        //                                                                            -> PkgY[NU1603]
        public async Task GivenAProjectReferenceNoWarnsForAllReferencesAndDirectTreatWarningsAsErrorVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced but not created
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageY11 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageY11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(packageY);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[TreatWarningsAsError true] -> ProjB -> PkgX[NU1603]
        public async Task GivenAProjectReferenceDoesNotNoWarnAndDirectTreatWarningsAsErrorVerifyErrorAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Properties.Add("TreatWarningsAsErrors", "true");
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> PkgY[NU1603]
        public async Task GivenAProjectReferenceNoWarnsVerifyWarningForDirectReferenceAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced but not created
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageY11 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageY11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                // A -> Y
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddPackageToAllFrameworks(packageY);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                GetSubstringCount(r.AllOutput, "NU1603", ignoreCase: false).Should().Be(1);
            }
        }

        [Fact]
        // Tests ProjA[PkgY NoWarn NU1603] -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //                                 -> PkgY[NU1603]
        public async Task GivenAProjectReferenceNoWarnsAndDirectReferenceNoWarnsVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced but not created
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageY11 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageY11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                // A -> Y
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddPackageToAllFrameworks(packageY);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[net461] -> ProjB[netstandard2.0][PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceWithFallBackFrameworkNoWarnsVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netstandard2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[net45] -> ProjB[net461][PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceWithIncompatibleFrameworkNoWarnsVerifyNoWarningAsync()
        {
            // Arrange         
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1201");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //                ToolY
        public async Task GivenAProjectReferenceNoWarnsVerifyNoWarningWithToolAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced but not created
                var toolY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1",
                    PackageType = PackageType.DotnetCliTool
                };

                // Created in the source
                var toolY101 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1",
                    PackageType = PackageType.DotnetCliTool
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, toolY101);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddPackageToAllFrameworks(toolY);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[net461] -> ProjB[netstandard2.0][ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        //                                                                         -> ToolY[NU1603]
        public async Task GivenAProjectReferenceWithToolAndProjectWideNoWarnsVerifyNoWarningAsync()
        {
            // Arrange         
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netstandard2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                };

                // Created in the source
                var packageX101 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced but not created
                var toolY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1",
                    PackageType = PackageType.DotnetCliTool
                };

                // Created in the source
                var toolY101 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1",
                    PackageType = PackageType.DotnetCliTool
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX101);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, toolY101);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(toolY);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[net461] -> ProjB[netstandard2.0][PkgX, ToolY NoWarn NU1603] -> PkgX[NU1603]
        //                                                                         -> ToolY[NU1603]
        public async Task GivenAProjectReferenceWithToolAndPackageSpecificNoWarnsVerifyNoWarningAsync()
        {
            // Arrange         
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netstandard2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX101 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Referenced but not created
                var toolY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1",
                    PackageType = PackageType.DotnetCliTool,
                    NoWarn = "NU1603"
                };

                // Created in the source
                var toolY101 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1",
                    PackageType = PackageType.DotnetCliTool
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX101);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, toolY101);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(toolY);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA[net461] -> ProjB[netstandard2.0][ProjectWide NoWarn NU1603] -> ToolY -> PkgX[NU1603]
        public async Task GivenAProjectReferenceWithToolBringingTransitivePackageNoWarnsVerifyNoWarningAsync()
        {
            // Arrange         
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netstandard2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                };

                // Created in the source
                var packageX101 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                // Created in the source
                var toolY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.1",
                    PackageType = PackageType.DotnetCliTool,
                    Dependencies = new List<SimpleTestPackageContext> { packageX }
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX101);
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, toolY);

                // B -> X
                projectB.AddPackageToAllFrameworks(toolY);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceNoWarnsProjectWideVerifyNoWarningAsync()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenAProjectReferenceWithDifferentFrameworkNoWarnsVerifyNoWarningAsync()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp1.1"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB -> ProjC[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenATransitiveProjectReferenceNoWarnsVerifyNoWarningAsync()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // C -> X
                projectC.AddPackageToAllFrameworks(packageX);
                projectC.Save();

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB -> ProjC[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenATransitiveProjectReferenceNoWarnsProjectWideVerifyNoWarningAsync()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // C -> X
                projectC.AddPackageToAllFrameworks(packageX);
                projectC.Properties.Add("NoWarn", "NU1603");
                projectC.Save();

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);
                projectB.Save();

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> PkgX[NU1603]
        public async Task GivenAProjectReferenceNoWarnsButDirectReferenceGeneratesWarningVerifyWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectB.Save();

                // A -> B
                // A -> X
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddPackageToAllFrameworks(packageX);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        //             -> PkgX[NU1603]
        public async Task GivenAProjectReferenceNoWarnsProjectWideButDirectReferenceGeneratesWarningVerifyWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // A -> B
                // A -> X
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddPackageToAllFrameworks(packageX);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenMultipleProjectReferencesNoWarnVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectB.Save();

                // C -> X
                projectC.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectC.Save();

                // A -> B
                // A -> C
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenMultipleProjectReferencesNoWarnProjectWideVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // C -> X
                projectC.AddPackageToAllFrameworks(packageX);
                projectC.Properties.Add("NoWarn", "NU1603");
                projectC.Save();

                // A -> B
                // A -> C
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC[ProjectWide NoWarn NU1605] -> PkgX[NU1603]
        public async Task GivenMultipleProjectReferencesNoWarnDifferentWarningsProjectWideVerifyWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // C -> X
                projectC.AddPackageToAllFrameworks(packageX);
                projectC.Properties.Add("NoWarn", "NU1605");
                projectC.Save();

                // A -> B
                // A -> C
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                GetSubstringCount(r.AllOutput, "NU1603", ignoreCase: false).Should().Be(2);
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[ProjectWide NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC[PkgX NoWarn NU1603]        -> PkgX[NU1603]
        public async Task GivenMultipleProjectReferencesNoWarnMixedVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };


                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Properties.Add("NoWarn", "NU1603");
                projectB.Save();

                // C -> X
                projectC.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectC.Save();

                // A -> B
                // A -> C
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC                     -> PkgX[NU1603]
        public async Task GivenMultipleProjectReferencesAndOnePathWarnsVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectB.Save();

                // C -> X
                projectC.AddPackageToAllFrameworks(packageX);
                projectC.Save();

                // A -> B
                // A -> C
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> PkgX[NU1603]
        public async Task GivenMultipleProjectReferencesNoWarnButDirectReferenceWarnsVerifyWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectB.Save();

                // C -> X
                projectC.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectC.Save();

                // A -> B
                // A -> C
                // A -> X
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.AddPackageToAllFrameworks(packageX);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                GetSubstringCount(r.AllOutput, "NU1603", ignoreCase: false).Should().Be(1);
            }
        }

        [Fact]
        // Tests ProjA[PkgX NoWarn NU1603] -> ProjB -> PkgX[NU1603]
        //                                 -> ProjC -> PkgX[NU1603]
        //                                 -> PkgX[NU1603]
        public async Task GivenMultipleProjectReferencesWarnButDirectReferenceNoWarnsVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.Save();

                // C -> X
                projectC.AddPackageToAllFrameworks(packageX);
                projectC.Save();

                // A -> B
                // A -> C
                // A -> X
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                GetSubstringCount(r.AllOutput, "NU1603", ignoreCase: false).Should().Be(2);
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC                     -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        public async Task GivenSinglePointOfReferenceNoWarnsVerifyNoWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectB.Save();

                // C -> B
                projectC.AddProjectToAllFrameworks(projectB);
                projectC.Save();

                // A -> B
                // A -> C
                // A -> X
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        [Fact]
        // Tests ProjA -> ProjB[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjC[PkgX NoWarn NU1603] -> PkgX[NU1603]
        //             -> ProjD                     -> ProjE        -> ProjF -> PkgX[NU1603]
        public async Task GivenOneLongPathWarnsVerifyWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectD = SimpleTestProjectContext.CreateNETCore(
                    "d",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectE = SimpleTestProjectContext.CreateNETCore(
                    "e",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));

                var projectF = SimpleTestProjectContext.CreateNETCore(
                    "f",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netcoreapp2.0"));


                // Referenced but not created
                var packageXWithNoWarn = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    NoWarn = "NU1603"
                };

                // Referenced but not created
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // Created in the source
                var packageX11 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.1"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX11);

                // B -> X
                projectB.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectB.Save();

                // C -> B
                projectC.AddPackageToAllFrameworks(packageXWithNoWarn);
                projectC.Save();

                // F -> X
                projectF.AddPackageToAllFrameworks(packageX);
                projectF.Save();

                // E -> F
                projectE.AddProjectToAllFrameworks(projectF);
                projectE.Save();

                // D -> E
                projectD.AddProjectToAllFrameworks(projectE);
                projectD.Save();

                // A -> B
                // A -> C
                // A -> D
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddProjectToAllFrameworks(projectC);
                projectA.AddProjectToAllFrameworks(projectD);
                projectA.Save();

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Projects.Add(projectD);
                solution.Projects.Add(projectE);
                solution.Projects.Add(projectF);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().Contain("NU1603");
            }
        }

        // Densely connected solutions containing 5, 10, 20 and 50 projects

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        public async Task GivenDenseSolutionWithMultiplePathsVerifyNoWarnAsync(int count)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projects = new List<SimpleTestProjectContext>();
                var referencedPackages = new List<SimpleTestPackageContext>();
                var createdPackages = new List<SimpleTestPackageContext>();

                for (var i = 0; i < count; i++)
                {
                    // Referenced but not created
                    var packagewithNoWarn = new SimpleTestPackageContext()
                    {
                        Id = "package_" + i,
                        Version = "1.0.0",
                        NoWarn = "NU1603"
                    };

                    // Created in the source
                    var package = new SimpleTestPackageContext()
                    {
                        Id = "package_" + i,
                        Version = "1.0.1"
                    };

                    await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, package);

                    referencedPackages.Add(packagewithNoWarn);
                    createdPackages.Add(package);
                }

                for (var i = 0; i < count; i++)
                {
                    var project = SimpleTestProjectContext.CreateNETCore(
                        "project_" + i,
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net461"));

                    projects.Add(project);
                }

                for (var i = 1; i < projects.Count(); i++)
                {
                    var project = projects[i];
                    project.AddPackageToAllFrameworks(referencedPackages[i]);
                }

                for (var i = 0; i < projects.Count() - 1; i++)
                {
                    var projectA = projects[i];
                    for (var j = i + 1; j < projects.Count(); j++)
                    {
                        var projectB = projects[j];
                        projectA.AddProjectToAllFrameworks(projectB);
                    }
                }

                foreach (var project in projects)
                {
                    project.Save();
                    solution.Projects.Add(project);
                }

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                r.AllOutput.Should().NotContain("NU1603");
            }
        }

        private static int GetSubstringCount(string str, string substr, bool ignoreCase)
        {
            var splitChars = new char[] { '.', '?', '!', ' ', ';', ':', ',', '\r', '\n' };
            var words = str.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            var comparisonType = ignoreCase ?
                StringComparison.OrdinalIgnoreCase :
                StringComparison.Ordinal;

            return words
                .Count(word => string.Equals(word, substr, comparisonType));
        }
    }
}
