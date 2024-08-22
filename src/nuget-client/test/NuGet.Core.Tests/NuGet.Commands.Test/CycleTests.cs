// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class CycleTests
    {
        [Fact]
        public async Task Cycle_PackageWithSameNameAsProjectVerifyCycleDetectedAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                spec1.TargetFrameworks[0].Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("projectA", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var specs = new[] { spec1 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("projectA", NuGetVersion.Parse("1.0.0")));

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.False(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains("Cycle detected", string.Join(Environment.NewLine, logger.ErrorMessages));
            }
        }

        [Fact]
        public async Task Cycle_ProjectWithSameNameAsProjectVerifyCycleDetectedAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");

                var specs = new[] { spec1, spec2 };

                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Link projects
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName,
                });

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("projectA", NuGetVersion.Parse("1.0.0")));

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.False(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains("Cycle detected", string.Join(Environment.NewLine, logger.ErrorMessages));
            }
        }

        [Fact]
        public async Task Cycle_PackageWithSameNameAsProjectVerifyCycleDetectedTwoLevelsDownAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                spec1.TargetFrameworks[0].Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var specs = new[] { spec1 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // A -> X -> A
                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                var projectAPkg = new SimpleTestPackageContext("projectA", "1.0.0");

                packageX.Dependencies.Add(projectAPkg);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX, projectAPkg);

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.False(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains("Cycle detected", string.Join(Environment.NewLine, logger.ErrorMessages));
            }
        }

        [Fact]
        public async Task Cycle_PackageWithSameNameAsProjectVerifyCycleDetectedAtEndAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                spec1.TargetFrameworks[0].Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var specs = new[] { spec1 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // A -> X -> Y -> Z -> Z
                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                var packageY = new SimpleTestPackageContext("y", "1.0.0");
                var packageZ = new SimpleTestPackageContext("z", "1.0.0");

                packageX.Dependencies.Add(packageY);
                packageY.Dependencies.Add(packageZ);
                packageZ.Dependencies.Add(packageZ);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX, packageY, packageZ);

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.False(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains("Cycle detected", string.Join(Environment.NewLine, logger.ErrorMessages));
            }
        }

        [Fact]
        public async Task Cycle_PackageCircularDependencyVerifyCycleDetectedAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                spec1.TargetFrameworks[0].Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("X", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var specs = new[] { spec1 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                var packageX = new SimpleTestPackageContext("x", "1.0.0");
                var packageY = new SimpleTestPackageContext("y", "1.0.0");
                var packageZ = new SimpleTestPackageContext("z", "1.0.0");

                // X -> Y -> Z -> X
                packageX.Dependencies.Add(packageY);
                packageY.Dependencies.Add(packageZ);
                packageZ.Dependencies.Add(packageX);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX, packageY, packageZ);

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.False(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains("Cycle detected", string.Join(Environment.NewLine, logger.ErrorMessages));
            }
        }

        [Theory]
        [InlineData("projectA")]
        [InlineData("projectB")]
        [InlineData("prOJecta")]
        [InlineData("prOJectB")]
        public async Task Cycle_TransitiveProjectWithSameNameAsPackageVerifyCycleDetectedAsync(string packageId)
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "projectB", framework: "netstandard1.6");

                spec2.TargetFrameworks[0].Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange(packageId, VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var specs = new[] { spec1, spec2 };

                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Link projects
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName,
                });

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("projectA", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("projectB", NuGetVersion.Parse("1.0.0")));

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec1);
                dgFile.AddProject(spec2);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.False(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains("Cycle detected", string.Join(Environment.NewLine, logger.ErrorMessages));
            }
        }
    }
}
