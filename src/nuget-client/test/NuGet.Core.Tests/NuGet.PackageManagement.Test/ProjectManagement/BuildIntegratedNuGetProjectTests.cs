// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Versioning;
using Xunit;

namespace ProjectManagement.Test
{
    public class BuildIntegratedNuGetProjectTests
    {
        [Fact]
        public void BuildIntegratedNuGetProject_SortDependenciesWithProjects()
        {
            // Arrange
            var lockFile = new LockFile();
            var target = new LockFileTarget();
            lockFile.Targets.Add(target);

            var targetA = new LockFileTargetLibrary()
            {
                Name = "a",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };
            targetA.Dependencies.Add(new PackageDependency("b"));

            var targetB = new LockFileTargetLibrary()
            {
                Name = "b",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "project"
            };
            targetA.Dependencies.Add(new PackageDependency("c"));

            var targetC = new LockFileTargetLibrary()
            {
                Name = "c",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };

            target.Libraries.Add(targetA);
            target.Libraries.Add(targetC);
            target.Libraries.Add(targetB);

            // Act
            var ordered = BuildIntegratedProjectUtility.GetOrderedLockFileDependencies(lockFile)
                .OrderBy(lib => lib.Name, StringComparer.Ordinal)
                .ToList();

            // Assert
            Assert.Equal(3, ordered.Count);
            Assert.Equal("a", ordered[0].Name);
            Assert.Equal("b", ordered[1].Name);
            Assert.Equal("c", ordered[2].Name);
        }

        [Fact]
        public void BuildIntegratedNuGetProject_SortDependenciesWithProjects_GetPackagesOnly()
        {
            // Arrange
            var lockFile = new LockFile();
            var target = new LockFileTarget();
            lockFile.Targets.Add(target);

            var targetA = new LockFileTargetLibrary()
            {
                Name = "a",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };
            targetA.Dependencies.Add(new PackageDependency("b"));

            var targetB = new LockFileTargetLibrary()
            {
                Name = "b",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "project"
            };
            targetA.Dependencies.Add(new PackageDependency("c"));

            var targetC = new LockFileTargetLibrary()
            {
                Name = "c",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };

            target.Libraries.Add(targetA);
            target.Libraries.Add(targetC);
            target.Libraries.Add(targetB);

            // Act
            var ordered = BuildIntegratedProjectUtility.GetOrderedLockFilePackageDependencies(lockFile)
                .OrderBy(lib => lib.Id, StringComparer.Ordinal)
                .ToList();

            // Assert
            Assert.Equal(2, ordered.Count);
            Assert.Equal("a", ordered[0].Id);
            // skip b
            Assert.Equal("c", ordered[1].Id);
        }
    }
}
