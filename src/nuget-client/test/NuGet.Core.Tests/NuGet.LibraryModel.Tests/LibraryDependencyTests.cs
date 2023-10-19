// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Versioning;
using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class LibraryDependencyTests
    {
        [Fact]
        public void LibraryDependency_Clone_Equals()
        {
            // Arrange
            var target = GetTarget();

            // Act
            var clone = target.Clone();

            // Assert
            Assert.NotSame(target, clone);
            Assert.Equal(target, clone);
        }

        [Fact]
        public void LibraryDependency_Clone_ClonesLibraryRange()
        {
            // Arrange
            var target = GetTarget();

            // Act
            var clone = target.Clone();
            clone.LibraryRange.Name = "SomethingElse";

            // Assert
            Assert.NotSame(target.LibraryRange, clone.LibraryRange);
            Assert.NotEqual(target.LibraryRange.Name, clone.LibraryRange.Name);
        }

        [Fact]
        public void LibraryDependency_ApplyCentralVersionInformation_NullArgumentCheck()
        {
            // Arrange
            List<LibraryDependency> packageReferences = new List<LibraryDependency>();
            Dictionary<string, CentralPackageVersion> centralPackageVersions = new Dictionary<string, CentralPackageVersion>();

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => LibraryDependency.ApplyCentralVersionInformation(null!, centralPackageVersions));
            Assert.Throws<ArgumentNullException>(() => LibraryDependency.ApplyCentralVersionInformation(packageReferences, null!));
        }

        [Fact]
        public void LibraryDependency_ApplyCentralVersionInformation_CPVIsMergedTpPackageVersions()
        {
            var dep1 = new LibraryDependency()
            {
                LibraryRange = new LibraryRange() { Name = "fooMerged" },
            };
            var dep2 = new LibraryDependency()
            {
                LibraryRange = new LibraryRange() { Name = "barNotMerged", VersionRange = VersionRange.Parse("1.0.0") },
            };
            var dep3 = new LibraryDependency()
            {
                LibraryRange = new LibraryRange() { Name = "bazNotMerged" },
                AutoReferenced = true
            };
            List<LibraryDependency> deps = new List<LibraryDependency>() { dep1, dep2, dep3 };

            var cpv1 = new CentralPackageVersion(dep1.Name.ToLower(), VersionRange.Parse("2.0.0"));
            var cpv2 = new CentralPackageVersion(dep2.Name.ToLower(), VersionRange.Parse("2.0.0"));
            var cpv3 = new CentralPackageVersion(dep3.Name.ToLower(), VersionRange.Parse("2.0.0"));
            Dictionary<string, CentralPackageVersion> cpvs = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            { [cpv1.Name] = cpv1, [cpv2.Name] = cpv2, [cpv3.Name] = cpv3 };

            // Act
            LibraryDependency.ApplyCentralVersionInformation(deps, cpvs);

            // Assert
            Assert.True(dep1.VersionCentrallyManaged);
            Assert.False(dep2.VersionCentrallyManaged);
            Assert.False(dep3.VersionCentrallyManaged);

            Assert.Equal("[2.0.0, )", dep1.LibraryRange.VersionRange!.ToNormalizedString());
            Assert.Equal("[1.0.0, )", dep2.LibraryRange.VersionRange!.ToNormalizedString());
            Assert.Null(dep3.LibraryRange.VersionRange);
        }

        public LibraryDependency GetTarget()
        {
            return new LibraryDependency
            {
                IncludeType = LibraryIncludeFlags.Build | LibraryIncludeFlags.Compile,
                LibraryRange = new LibraryRange
                {
                    Name = "SomeLibrary",
                    TypeConstraint = LibraryDependencyTarget.ExternalProject | LibraryDependencyTarget.WinMD,
                    VersionRange = new VersionRange(new NuGetVersion("4.0.0-rc2"))
                },
                SuppressParent = LibraryIncludeFlags.Analyzers | LibraryIncludeFlags.ContentFiles,
                Aliases = "stuff",
            };
        }
    }
}
