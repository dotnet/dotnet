// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class RestoreMessageTest
    {
        [Fact]
        public async Task GivenAProjectIsUsedOverAPackageVerifyNoDowngradeWarningAsync()
        {

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                var packageX = new SimpleTestPackageContext("x", "9.0.0");
                var packageB = new SimpleTestPackageContext("b", "9.0.0");
                packageX.Dependencies.Add(packageB);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX, packageB);

                // PackageB will be overridden by ProjectB
                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, projectA.ProjectPath);
                var output = r.Output + " " + r.Errors;
                var reader = new LockFileFormat();
                var lockFileObj = reader.Read(projectA.AssetsFileOutputPath);

                // Assert
                Assert.NotNull(lockFileObj);
                Assert.Equal(0, lockFileObj.LogMessages.Count());
                Assert.DoesNotContain("downgrade", output, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public async Task GivenAPackageDowngradeVerifyDowngradeWarningAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var packageB = new SimpleTestPackageContext("b", "9.0.0");
                var packageI1 = new SimpleTestPackageContext("i", "9.0.0");
                var packageI2 = new SimpleTestPackageContext("i", "1.0.0");
                packageB.Dependencies.Add(packageI1);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageB, packageI1, packageI2);

                projectA.AddPackageToAllFrameworks(packageB);
                projectA.AddPackageToAllFrameworks(packageI2);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, projectA.ProjectPath);
                var output = r.Output + " " + r.Errors;
                var reader = new LockFileFormat();
                var lockFileObj = reader.Read(projectA.AssetsFileOutputPath);

                // Assert
                Assert.NotNull(lockFileObj);
                Assert.Equal(1, lockFileObj.LogMessages.Count());
                Assert.Contains("Detected package downgrade: i from 9.0.0 to 1.0.0",
                    lockFileObj.LogMessages.First().Message,
                    StringComparison.OrdinalIgnoreCase);
                Assert.Contains("Detected package downgrade: i from 9.0.0 to 1.0.0",
                    output,
                    StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void GivenAnUnknownPackageVerifyError()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var packageB = new SimpleTestPackageContext("b", "9.0.0");

                projectA.AddPackageToAllFrameworks(packageB);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, projectA.ProjectPath, expectedExitCode: 1);
                var output = r.Output + " " + r.Errors;
                var reader = new LockFileFormat();
                var lockFileObj = reader.Read(projectA.AssetsFileOutputPath);

                // Assert
                Assert.NotNull(lockFileObj);
                Assert.Equal(1, lockFileObj.LogMessages.Count());
                Assert.Contains("Unable to find package b. No packages exist with this id in source(s): source",
                    lockFileObj.LogMessages.First().Message,
                    StringComparison.OrdinalIgnoreCase);
                Assert.Contains("Unable to find package b. No packages exist with this id in source(s): source",
                    output,
                    StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public async Task GivenAPackageWithAHigherMinClientVersionVerifyErrorCodeDisplayedAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var packageB = new SimpleTestPackageContext("b", "1.0.0")
                {
                    MinClientVersion = "99.0.0"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageB);

                projectA.AddPackageToAllFrameworks(packageB);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, projectA.ProjectPath, expectedExitCode: 1);
                var output = r.Output + " " + r.Errors;

                // Assert
                Assert.Contains("NU1401", output, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
