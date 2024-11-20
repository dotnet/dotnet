// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NuGet.Commands.Test;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;
using PackagesLockFileBuilder = NuGet.ProjectModel.Test.Builders.PackagesLockFileBuilder;

namespace NuGet.ProjectModel.Test.ProjectLockFile
{
    public class LockFileUtilitiesTests
    {
        [Fact]
        public void IsLockFileStillValid_DifferentVersions_AreNotEqual()
        {
            var x = new PackagesLockFileBuilder().Build();
            var y = new PackagesLockFileBuilder()
                .WithVersion(PackagesLockFileFormat.Version + 1)
                .Build();

            var actual = PackagesLockFileUtilities.IsLockFileStillValid(x, y);
            Assert.False(actual.IsValid);

            actual = PackagesLockFileUtilities.IsLockFileStillValid(y, x);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void IsLockFileStillValid_DifferentTargetCounts_AreNotEqual()
        {
            var x = new PackagesLockFileBuilder()
                .WithTarget(target => target.WithFramework(CommonFrameworks.NetStandard20))
                .WithTarget(target => target.WithFramework(CommonFrameworks.NetCoreApp22))
                .Build();
            var y = new PackagesLockFileBuilder()
                .WithTarget(target => target.WithFramework(CommonFrameworks.NetStandard20))
                .Build();

            var actual = PackagesLockFileUtilities.IsLockFileStillValid(x, y);
            Assert.False(actual.IsValid);

            actual = PackagesLockFileUtilities.IsLockFileStillValid(y, x);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void IsLockFileStillValid_DifferentTargets_AreNotEqual()
        {
            var x = new PackagesLockFileBuilder()
                .WithTarget(target => target.WithFramework(CommonFrameworks.NetStandard20))
                .Build();
            var y = new PackagesLockFileBuilder()
                .WithTarget(target => target.WithFramework(CommonFrameworks.NetCoreApp22))
                .Build();

            var actual = PackagesLockFileUtilities.IsLockFileStillValid(x, y);
            Assert.False(actual.IsValid);

            actual = PackagesLockFileUtilities.IsLockFileStillValid(y, x);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void IsLockFileStillValid_DifferentDependencyCounts_AreNotEqual()
        {
            var x = new PackagesLockFileBuilder()
                .WithTarget(target => target
                    .WithFramework(CommonFrameworks.NetStandard20)
                    .WithDependency(dep => dep.WithId("PackageA")))
                .Build();
            var y = new PackagesLockFileBuilder()
                .WithTarget(target => target
                    .WithFramework(CommonFrameworks.NetStandard20)
                    .WithDependency(dep => dep.WithId("PackageA"))
                    .WithDependency(dep => dep.WithId("PackageB")))
                .Build();

            var actual = PackagesLockFileUtilities.IsLockFileStillValid(x, y);
            Assert.False(actual.IsValid);

            actual = PackagesLockFileUtilities.IsLockFileStillValid(y, x);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void IsLockFileStillValid_DifferentDependency_AreNotEqual()
        {
            var x = new PackagesLockFileBuilder()
                .WithTarget(target => target
                    .WithFramework(CommonFrameworks.NetStandard20)
                    .WithDependency(dep => dep.WithId("PackageA")))
                .Build();
            var y = new PackagesLockFileBuilder()
                .WithTarget(target => target
                    .WithFramework(CommonFrameworks.NetStandard20)
                    .WithDependency(dep => dep.WithId("PackageB")))
                .Build();

            var actual = PackagesLockFileUtilities.IsLockFileStillValid(x, y);
            Assert.False(actual.IsValid);

            actual = PackagesLockFileUtilities.IsLockFileStillValid(y, x);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void IsLockFileStillValid_MatchesDependencies_AreEqual()
        {
            var x = new PackagesLockFileBuilder()
                .WithTarget(target => target
                    .WithFramework(CommonFrameworks.NetStandard20)
                    .WithDependency(dep => dep
                        .WithId("PackageA")
                        .WithContentHash("ABC"))
                    .WithDependency(dep => dep
                        .WithId("PackageB")
                        .WithContentHash("123")))
                .Build();
            var y = new PackagesLockFileBuilder()
                .WithTarget(target => target
                    .WithFramework(CommonFrameworks.NetStandard20)
                    .WithDependency(dep => dep
                        .WithId("PackageA")
                        .WithContentHash("XYZ"))
                    .WithDependency(dep => dep
                        .WithId("PackageB")
                        .WithContentHash("890")))
                .Build();

            var actual = PackagesLockFileUtilities.IsLockFileStillValid(x, y);
            Assert.True(actual.IsValid);
            Assert.NotNull(actual.MatchedDependencies);
            Assert.Equal(2, actual.MatchedDependencies.Count);
            var depKvp = actual.MatchedDependencies.Single(d => d.Key.Id == "PackageA");
            Assert.Equal("ABC", depKvp.Key.ContentHash);
            Assert.Equal("XYZ", depKvp.Value.ContentHash);
            depKvp = actual.MatchedDependencies.Single(d => d.Key.Id == "PackageB");
            Assert.Equal("123", depKvp.Key.ContentHash);
            Assert.Equal("890", depKvp.Value.ContentHash);

            actual = PackagesLockFileUtilities.IsLockFileStillValid(y, x);
            Assert.True(actual.IsValid);
            Assert.NotNull(actual.MatchedDependencies);
            Assert.Equal(2, actual.MatchedDependencies.Count);
            depKvp = actual.MatchedDependencies.Single(d => d.Key.Id == "PackageA");
            Assert.Equal("ABC", depKvp.Value.ContentHash);
            Assert.Equal("XYZ", depKvp.Key.ContentHash);
            depKvp = actual.MatchedDependencies.Single(d => d.Key.Id == "PackageB");
            Assert.Equal("123", depKvp.Value.ContentHash);
            Assert.Equal("890", depKvp.Key.ContentHash);
        }

        [Fact]
        public void IsLockFileStillValid_DifferentTargetFrameworksInDgspec_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";

            var dependency1 = new LibraryDependency(
                new LibraryRange("library1", versionRange: VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var tfm1 = new TargetFrameworkInformation
            {
                Dependencies = [dependency1],
                FrameworkName = framework
            };

            var tfm2 = new TargetFrameworkInformation
            {
                FrameworkName = CommonFrameworks.NetCoreApp31
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm1, tfm2 })
            {
                RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = false }
            };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("library1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("otherDep")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);
            Assert.False(actual.IsValid);
            Assert.Contains("The project target frameworks are different than the lock file's target frameworks. " +
                "Lock file target frameworks: netstandard2.0,netcoreapp3.1. Project target frameworks netstandard2.0.", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_NewTargetFrameworksInDgspec_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";

            var dependency1 = new LibraryDependency(
                new LibraryRange("library1", versionRange: VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var tfm = new TargetFrameworkInformation
            {
                Dependencies = [dependency1],
                FrameworkName = framework
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm })
            {
                RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = false }
            };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetCoreApp31)
                        .WithDependency(dep => dep
                        .WithId("library1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("otherDep")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);
            Assert.False(actual.IsValid);
            Assert.Contains("The project target framework netstandard2.0 was not found in the lock file.", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_RemovedCentralTransitivePackageVersions_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";
            var cpvm1 = new CentralPackageVersion("cpvm1", VersionRange.Parse("1.0.0"));
            var cpvm2 = new CentralPackageVersion("cpvm2", VersionRange.Parse("1.0.0"));
            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: cpvm1.VersionRange, LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: true,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var centralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            {
                { "cpvm1", cpvm1 },
                { "cpvm2", cpvm2 },
            };
            ImmutableArray<LibraryDependency> dependencies = [dependency1];

            var tfm = new TargetFrameworkInformation()
            {
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                FrameworkName = framework
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = true };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("cpvm2")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.CentralTransitive))
                        .WithDependency(dep => dep
                        .WithId("cpvm3")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.CentralTransitive)))
                        .Build();

            // The central package version cpvm3 it was removed
            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);
            Assert.False(actual.IsValid);
            Assert.Contains("Central package management file(s) doesn't contain version range for cpvm3 package " +
                "which is specified as CentralTransitive dependency in the lock file.", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_DifferentCentralTransitivePackageVersions_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";
            var cpvm1 = new CentralPackageVersion("cpvm1", VersionRange.Parse("1.0.0"));
            var cpvm2 = new CentralPackageVersion("cpvm2", VersionRange.Parse("2.0.0"));
            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: cpvm1.VersionRange, LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: true,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var centralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            {
                { "cpvm1", cpvm1 },
                { "cpvm2", cpvm2 },
            };
            ImmutableArray<LibraryDependency> dependencies = [dependency1];

            var tfm = new TargetFrameworkInformation()
            {
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                FrameworkName = framework,
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = true };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("cpvm2")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.CentralTransitive)))
                        .Build();

            // The central package version cpvm2 has version changed
            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);
            Assert.False(actual.IsValid);
            Assert.Contains("Mistmatch between the requestedVersion of a lock file dependency marked as CentralTransitive and the the version specified in the central package management file. " +
                "Lock file version [1.0.0, ), central package management version [2.0.0, ).", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_DifferentDirectPackageVersions_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";
            var cpvm1 = new CentralPackageVersion("cpvm1", VersionRange.Parse("2.0.0"));
            var cpvm2 = new CentralPackageVersion("cpvm2", VersionRange.Parse("1.0.0"));
            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: cpvm1.VersionRange, LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: true,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var centralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            {
                { "cpvm1", cpvm1 },
                { "cpvm2", cpvm2 },
            };
            ImmutableArray<LibraryDependency> dependencies = [dependency1];

            var tfm = new TargetFrameworkInformation()
            {
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                FrameworkName = framework
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = true };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("cpvm2")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.CentralTransitive)))
                        .Build();

            // The central package version cpvm2 has version changed
            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);
            Assert.False(actual.IsValid);
            Assert.Contains("The package reference cpvm1 version has changed from [1.0.0, ) to [2.0.0, ).", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_TransitiveVersionsMovedToCentralFile_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";
            var cpvm1 = new CentralPackageVersion("cpvm1", VersionRange.Parse("1.0.0"));
            var cpvm2 = new CentralPackageVersion("cpvm2", VersionRange.Parse("1.0.0"));
            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: cpvm1.VersionRange, LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: true,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var centralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            {
                { "cpvm1", cpvm1 },
                { "cpvm2", cpvm2 },
            };
            ImmutableArray<LibraryDependency> dependencies = [dependency1];

            var tfm = new TargetFrameworkInformation()
            {
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                FrameworkName = framework,
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = true };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("cpvm2")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            // The central package version cpvm2 has was changed from transitive to central 
            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);
            Assert.False(actual.IsValid);
            Assert.Contains("Transitive dependency cpvm2 moved to be centraly managed invalidated the lock file.", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_NoChangeInCentralTransitivePackageVersions_DoesNotInvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";
            var cpvm1 = new CentralPackageVersion("cpvm1", VersionRange.Parse("1.0.0"));
            var cpvm2 = new CentralPackageVersion("cpvm2", VersionRange.Parse("1.0.0"));
            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: cpvm1.VersionRange, LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: true,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var centralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            {
                { "cpvm1", cpvm1 },
                { "cpvm2", cpvm2 },
            };
            ImmutableArray<LibraryDependency> dependencies = [dependency1];

            var tfm = new TargetFrameworkInformation()
            {
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                FrameworkName = framework
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = true };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("cpvm2")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.CentralTransitive))
                        .WithDependency(dep => dep
                        .WithId("otherDep")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            // Nothing changed in central package versions
            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);
            Assert.True(actual.IsValid);
            Assert.Empty(actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_TransitiveDependencyNotCentrallyManaged_DoesNotInvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";
            var cpvm1 = new CentralPackageVersion("cpvm1", VersionRange.Parse("1.0.0"));
            var cpvm2 = new CentralPackageVersion("cpvm2", VersionRange.Parse("1.0.0"));
            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: cpvm1.VersionRange, LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: true,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var centralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            {
                { "cpvm1", cpvm1 },
                { "cpvm2", cpvm2 },
            };
            ImmutableArray<LibraryDependency> dependencies = [dependency1];

            var tfm = new TargetFrameworkInformation()
            {
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                FrameworkName = framework
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = true };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile1 = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("otherDep")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            var lockFile2 = new PackagesLockFileBuilder()
                        .WithVersion(PackagesLockFileFormat.PackagesLockFileVersion)
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("otherDep")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            // Nothing changed
            // different versions of lock file versions are handled

            var actual1 = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile1);
            var actual2 = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile2);

            Assert.True(actual1.IsValid);
            Assert.Empty(actual1.InvalidReasons);
            Assert.True(actual2.IsValid);
            Assert.Empty(actual2.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_NoCentralPackageVersions_DoesNotInvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";

            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var tfm = new TargetFrameworkInformation()
            {
                Dependencies = [dependency1],
                FrameworkName = framework
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = false };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("otherDep")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            // Nothing changed in central package versions
            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.True(actual.IsValid);
            Assert.Empty(actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_VersionValidationCheck()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard20;
            var projectName = "project";

            var dependency1 = new LibraryDependency(
                new LibraryRange("cpvm1", versionRange: VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var tfm = new TargetFrameworkInformation()
            {
                Dependencies = [dependency1],
                FrameworkName = framework
            };

            var project = new PackageSpec(new List<TargetFrameworkInformation>() { tfm });
            project.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = projectName, CentralPackageVersionsEnabled = false };

            DependencyGraphSpec dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore(projectName);
            dgSpec.AddProject(project);

            var lockFile = new PackagesLockFileBuilder()
                        .WithVersion(PackagesLockFileFormat.PackagesLockFileVersion + 1)
                        .WithTarget(target => target
                        .WithFramework(CommonFrameworks.NetStandard20)
                        .WithDependency(dep => dep
                        .WithId("cpvm1")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Direct))
                        .WithDependency(dep => dep
                        .WithId("otherDep")
                        .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        .WithType(PackageDependencyType.Transitive)))
                        .Build();

            // Due to the increased version in the lock file the lock should be invalid
            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.False(actual.IsValid);
            Assert.Contains("Version specified in the packages.lock.json file is incompatible. Current tooling supports version " +
                $"{PackagesLockFileFormat.PackagesLockFileVersion} and previous formats.", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_WithNewProjectDependency_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.Net50;
            var frameworkShortName = framework.GetShortFolderName();
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackageSpec("B", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectC = ProjectTestHelpers.GetPackageSpec("C", framework: frameworkShortName).WithTestRestoreMetadata();

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            // A -> C
            projectA = projectA.WithTestProjectReference(projectC);

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB, projectC);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.False(actual.IsValid);
            Assert.Contains("A new project reference to C was found for net5.0 target framework.", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_WithNewP2PDependency_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.Net50;
            var frameworkShortName = framework.GetShortFolderName();
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackageSpec("B", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectC = ProjectTestHelpers.GetPackageSpec("C", framework: frameworkShortName).WithTestRestoreMetadata();

            // B -> C
            projectB = projectB.WithTestProjectReference(projectC);

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB, projectC);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.False(actual.IsValid);
            Assert.Contains(string.Format("The project reference B has changed. Current dependencies: {0} lock file's dependencies: None.", projectC.FilePath), actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_WithChangeInP2PDependency_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.Net50;
            var frameworkShortName = framework.GetShortFolderName();
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackageSpec("B", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectD = ProjectTestHelpers.GetPackageSpec("D", framework: frameworkShortName).WithTestRestoreMetadata();

            // B -> D
            projectB = projectB.WithTestProjectReference(projectD);

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB, projectD);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                            .WithDependency(new PackageDependency("C", VersionRange.Parse("1.0.0"))))
                        .WithDependency(dep => dep
                            .WithId("C")
                            .WithType(PackageDependencyType.Project)))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.False(actual.IsValid);
            Assert.Contains("The project references B whose dependencies has changed.", actual.InvalidReasons);
        }

        [Fact]
        public void IsLockFileStillValid_WithChangeInP2PPackageDependency_InvalidateLockFile()
        {
            // Arrange
            var framework = CommonFrameworks.Net50;
            var frameworkShortName = framework.GetShortFolderName();
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackageSpec("B", framework: frameworkShortName).WithTestRestoreMetadata();

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            var packageC = new LibraryDependency(
                new LibraryRange("packageC", versionRange: VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package),
                includeType: LibraryIncludeFlags.All,
                suppressParent: LibraryIncludeFlagUtils.DefaultSuppressParent,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                libraryDependencyReferenceType: LibraryDependencyReferenceType.Direct,
                aliases: null,
                versionOverride: null);

            var projectBFirstTargetFramework = projectB.TargetFrameworks.First();
            var projectBFirstTargetFrameworkDependencies = projectBFirstTargetFramework.Dependencies.Add(packageC);
            projectBFirstTargetFramework = new TargetFrameworkInformation(projectBFirstTargetFramework) { Dependencies = projectBFirstTargetFrameworkDependencies };
            projectB.TargetFrameworks[0] = projectBFirstTargetFramework;

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                            .WithDependency(new PackageDependency("packageB", VersionRange.Parse("1.0.0"))))
                        .WithDependency(dep => dep
                            .WithId("packageB")
                            .WithType(PackageDependencyType.Transitive)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.False(actual.IsValid);
            Assert.Contains("The project references B whose dependencies has changed.", actual.InvalidReasons);
        }

        /// <summary>
        /// A -> B (PrivateAssets)-> C
        /// A has packages lock file enabled. Locked should succeed and ignore `C`.
        /// </summary>
        [Fact]
        public void IsLockFileStillValid_WithProjectToProjectWithPrivateAssets_IgnoresSuppressedDependencies()
        {
            // Arrange
            var framework = CommonFrameworks.Net50;
            var frameworkShortName = framework.GetShortFolderName();
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackageSpec("B", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectC = ProjectTestHelpers.GetPackageSpec("C", framework: frameworkShortName).WithTestRestoreMetadata();

            // B (PrivateAssets.All) -> C 
            projectB = projectB.WithTestProjectReference(projectC, privateAssets: LibraryIncludeFlags.All);

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB, projectC);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.True(actual.IsValid);
            Assert.Empty(actual.InvalidReasons);
        }

        /// <summary>
        /// A -> B (PrivateAssets)-> C -> PackageC
        /// A -> D -> C -> PackageC
        /// </summary>
        [Fact]
        public void IsLockFileStillValid_WithProjectToProject_MultipleEdgesWithDifferentPrivateAssets_IncludesAllDependencies()
        {
            // Arrange
            var framework = CommonFrameworks.Net50;
            var frameworkShortName = framework.GetShortFolderName();
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackageSpec("B", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectC = ProjectTestHelpers.GetPackageSpec("C", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectD = ProjectTestHelpers.GetPackageSpec("D", framework: frameworkShortName).WithTestRestoreMetadata();

            var packageC = new LibraryDependency(
                new LibraryRange("packageC", versionRange: VersionRange.Parse("2.0.0"), LibraryDependencyTarget.Package),
                includeType: LibraryIncludeFlags.All,
                suppressParent: LibraryIncludeFlagUtils.DefaultSuppressParent,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                libraryDependencyReferenceType: LibraryDependencyReferenceType.Direct,
                aliases: null,
                versionOverride: null);

            // B (PrivateAssets.All) -> C 
            projectB = projectB.WithTestProjectReference(projectC, privateAssets: LibraryIncludeFlags.All);

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            // A -> D
            projectA = projectA.WithTestProjectReference(projectD);

            // D -> C
            projectD = projectD.WithTestProjectReference(projectC);

            // C -> PackageC
            var projectCTargetFrameworkDependencies = projectC.TargetFrameworks.First().Dependencies.Add(packageC);
            projectC.TargetFrameworks[0] = new TargetFrameworkInformation(projectC.TargetFrameworks[0]) { Dependencies = projectCTargetFrameworkDependencies };

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB, projectC, projectD);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0")))
                        .WithDependency(dep => dep
                            .WithId("C")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                            .WithDependency(new PackageDependency("packageC", VersionRange.Parse("2.0.0"))))
                        .WithDependency(dep => dep
                            .WithId("D")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                            .WithDependency(new PackageDependency("C", VersionRange.Parse("1.0.0"))))
                        .WithDependency(dep => dep
                            .WithId("packageC")
                            .WithType(PackageDependencyType.Transitive)
                            .WithRequestedVersion(VersionRange.Parse("2.0.0"))
                            .WithResolvedVersion(NuGetVersion.Parse("2.0.0"))
                        ))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.True(actual.IsValid);
            Assert.Empty(actual.InvalidReasons);
        }

        // <summary>
        /// A(PR) -> B (PC) -> C(PR) -> PackageC
        /// </summary>
        [Fact]
        public void IsLockFileStillValid_WithProjectToProjectPackagesConfig_IncludesAllDependencies()
        {
            // Arrange
            var framework = CommonFrameworks.Net50;
            var frameworkShortName = framework.GetShortFolderName();
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackagesConfigPackageSpec("B");
            var projectC = ProjectTestHelpers.GetPackageSpec("C", framework: frameworkShortName).WithTestRestoreMetadata();

            var packageC = new LibraryDependency(
                new LibraryRange("packageC", versionRange: VersionRange.Parse("2.0.0"), LibraryDependencyTarget.Package),
                includeType: LibraryIncludeFlags.All,
                suppressParent: LibraryIncludeFlagUtils.DefaultSuppressParent,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                libraryDependencyReferenceType: LibraryDependencyReferenceType.Direct,
                aliases: null,
                versionOverride: null);

            // B -> C 
            projectB = projectB.WithTestProjectReference(projectC);

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            // C -> PackageC
            var projectCTargetFrameworkDependencies = projectC.TargetFrameworks.First().Dependencies.Add(packageC);
            projectC.TargetFrameworks[0] = new TargetFrameworkInformation(projectC.TargetFrameworks[0]) { Dependencies = projectCTargetFrameworkDependencies };

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB, projectC);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                            .WithDependency(new PackageDependency("C", VersionRange.Parse("1.0.0"))))
                        .WithDependency(dep => dep
                            .WithId("C")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                            .WithDependency(new PackageDependency("packageC", VersionRange.Parse("2.0.0"))))
                        .WithDependency(dep => dep
                            .WithId("packageC")
                            .WithType(PackageDependencyType.Transitive)
                            .WithRequestedVersion(VersionRange.Parse("2.0.0"))
                            .WithResolvedVersion(NuGetVersion.Parse("2.0.0"))
                        ))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.True(actual.IsValid);
            Assert.Empty(actual.InvalidReasons);
        }

        // <summary>
        /// A(PR) -> B (PC), where A and B have incompatible frameworks
        /// </summary>
        [Fact]
        public void IsLockFileStillValid_WithProjectToProjectPackagesConfig_WithIncompatibleFrameworks_IgnoresCompatiblityChecksAndSucceeds()
        {
            // Arrange
            var framework = CommonFrameworks.NetStandard;
            var frameworkShortName = framework.GetShortFolderName();
            var incompatibleFramework = CommonFrameworks.Net46;
            var projectA = ProjectTestHelpers.GetPackageSpec("A", framework: frameworkShortName).WithTestRestoreMetadata();
            var projectB = ProjectTestHelpers.GetPackagesConfigPackageSpec("B", framework: incompatibleFramework.GetShortFolderName());

            // A -> B
            projectA = projectA.WithTestProjectReference(projectB);

            var dgSpec = ProjectTestHelpers.GetDGSpecForFirstProject(projectA, projectB);

            var lockFile = new PackagesLockFileBuilder()
                        .WithTarget(target => target
                        .WithFramework(framework)
                        .WithDependency(dep => dep
                            .WithId("B")
                            .WithType(PackageDependencyType.Project)
                            .WithRequestedVersion(VersionRange.Parse("1.0.0"))
                        ))
                        .Build();

            var actual = PackagesLockFileUtilities.IsLockFileValid(dgSpec, lockFile);

            Assert.True(actual.IsValid);
            Assert.Empty(actual.InvalidReasons);
        }
    }
}
