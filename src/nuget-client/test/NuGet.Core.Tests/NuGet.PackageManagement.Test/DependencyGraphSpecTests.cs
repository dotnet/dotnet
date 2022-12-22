// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.ProjectModel;
using Xunit;

namespace NuGet.Test
{
    public class DependencyGraphSpecTests
    {
        [Fact]
        public void WithReplacedSpec()
        {
            // Arrange
            var packageSpecA = new PackageSpec();
            packageSpecA.Title = "A";
            packageSpecA.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = "a", CentralPackageVersionsEnabled = false };
            var dgSpec = new DependencyGraphSpec();
            var packageSpecB = new PackageSpec();
            packageSpecB.Title = "B";
            packageSpecB.RestoreMetadata = new ProjectRestoreMetadata()
            {
                ProjectUniqueName = "BBB"
            };
            var packageSpecC = new PackageSpec();
            packageSpecC.Title = "C";
            packageSpecC.RestoreMetadata = new ProjectRestoreMetadata()
            {
                ProjectUniqueName = "CCC"
            };

            // Act
            dgSpec = dgSpec.WithReplacedSpec(packageSpecA);
            dgSpec = dgSpec.WithReplacedSpec(packageSpecB);
            dgSpec = dgSpec.WithReplacedSpec(packageSpecC);

            // Assert
            Assert.Equal(dgSpec.Projects.Count, 3);
            Assert.Equal(dgSpec.Restore.Count, 1);
        }

        [Fact]
        public void WithReplacedPackageSpecs_WithASinglePackageSpec_Succeeds()
        {
            // Arrange
            var packageSpecA = new PackageSpec
            {
                Title = "A",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectUniqueName = "a",
                    CentralPackageVersionsEnabled = false
                }
            };
            var packageSpecB = new PackageSpec
            {
                Title = "B",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectUniqueName = "BBB"
                }
            };
            var packageSpecC = new PackageSpec
            {
                Title = "C",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectUniqueName = "CCC"
                }
            };
            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(packageSpecA);
            dgSpec.AddProject(packageSpecB);
            dgSpec.AddProject(packageSpecC);
            dgSpec.AddRestore(packageSpecA.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddRestore(packageSpecB.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddRestore(packageSpecC.RestoreMetadata.ProjectUniqueName);

            // Create an updated packageSpecA
            var updatedPackageA = packageSpecA.Clone();
            updatedPackageA.RestoreMetadata.ConfigFilePaths.Add("/samplePath");
            var newNugetPackageSpecs = new List<PackageSpec>()
            {
                updatedPackageA
            };

            // Preconditions
            dgSpec.Projects.Should().HaveCount(3);
            dgSpec.Restore.Should().HaveCount(3);

            // Act
            var dgSpecWithReplacedPackageA = dgSpec.WithPackageSpecs(newNugetPackageSpecs);

            // Assert
            dgSpecWithReplacedPackageA.Projects.Should().HaveCount(3);
            dgSpecWithReplacedPackageA.Restore.Should().HaveCount(1);

            var packageSpecInAFromDgSpec = dgSpecWithReplacedPackageA.Projects.Single(e => e.Title.Equals("A"));
            packageSpecInAFromDgSpec.Should().Be(updatedPackageA);
            dgSpecWithReplacedPackageA.Restore.Single().Should().Be(updatedPackageA.RestoreMetadata.ProjectUniqueName);
        }
    }
}
