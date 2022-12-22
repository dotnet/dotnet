// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class CentralTransitiveDependencyGroupTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CentralTransitiveDependencyGroup_ConstructorNullArgumentCheck(bool nullFramework, bool nullDependencies)
        {
            // Arrange
            var nuGetFramework = nullFramework ? null : NuGetFramework.Parse("NETStandard2.0");
            var dependencies = nullDependencies ? null : Enumerable.Empty<LibraryDependency>();

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => new CentralTransitiveDependencyGroup(nuGetFramework, dependencies));
        }

        [Fact]
        public void CentralTransitiveDependencyGroup_GetProperties()
        {
            // Arrange
            var nuGetFramework = NuGetFramework.Parse("NETStandard2.0");
            var libraryDep = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                                "packageA",
                                VersionRange.Parse("1.0.0"),
                                LibraryDependencyTarget.Package)
            };
            var dependencies = new List<LibraryDependency>() { libraryDep };
            var centralTransitiveDependencyGroup = new CentralTransitiveDependencyGroup(nuGetFramework, dependencies);

            // Act
            var framework = centralTransitiveDependencyGroup.FrameworkName;
            var tDependencies = centralTransitiveDependencyGroup.TransitiveDependencies;

            // Assert
            Assert.Equal(dependencies, tDependencies);
            Assert.Equal(nuGetFramework.ToString(), framework);
        }

        [Fact]
        public void CentralTransitiveDependencyGroup_EqualObjects()
        {
            // Arrange
            var nuGetFramework = NuGetFramework.Parse("NETStandard2.0");
            var libraryDep = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                                "packageA",
                                VersionRange.Parse("1.0.0"),
                                LibraryDependencyTarget.Package)
            };
            var dependencies = new List<LibraryDependency>() { libraryDep };
            var centralTransitiveDependencyGroup1 = new CentralTransitiveDependencyGroup(nuGetFramework, dependencies);
            var centralTransitiveDependencyGroup2 = new CentralTransitiveDependencyGroup(nuGetFramework, dependencies);

            // Act = Assert
            Assert.True(centralTransitiveDependencyGroup1.Equals(centralTransitiveDependencyGroup1));
            Assert.True(centralTransitiveDependencyGroup1.Equals(centralTransitiveDependencyGroup2));
            Assert.Equal(centralTransitiveDependencyGroup1.GetHashCode(), centralTransitiveDependencyGroup2.GetHashCode());
        }

        [Fact]
        public void CentralTransitiveDependencyGroup_NotEqualObjects()
        {
            // Arrange
            var nuGetFramework1 = NuGetFramework.Parse("NETStandard2.0");
            var nuGetFramework2 = NuGetFramework.Parse("NETStandard3.0");
            var libraryDep1 = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                                "packageA",
                                VersionRange.Parse("1.0.0"),
                                LibraryDependencyTarget.Package)
            };
            var libraryDep2 = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                                "packageB",
                                VersionRange.Parse("1.0.0"),
                                LibraryDependencyTarget.Package)
            };
            var dependencies1 = new List<LibraryDependency>() { libraryDep1 };
            var dependencies2 = new List<LibraryDependency>() { libraryDep2 };

            var centralTransitiveDependencyGroup11 = new CentralTransitiveDependencyGroup(nuGetFramework1, dependencies1);
            var centralTransitiveDependencyGroup12 = new CentralTransitiveDependencyGroup(nuGetFramework1, dependencies2);
            var centralTransitiveDependencyGroup21 = new CentralTransitiveDependencyGroup(nuGetFramework2, dependencies1);
            var centralTransitiveDependencyGroup22 = new CentralTransitiveDependencyGroup(nuGetFramework2, dependencies2);

            // Act = Assert
            Assert.False(centralTransitiveDependencyGroup11.Equals(null));
            Assert.False(centralTransitiveDependencyGroup11.Equals(centralTransitiveDependencyGroup12));
            Assert.False(centralTransitiveDependencyGroup11.Equals(centralTransitiveDependencyGroup21));
            Assert.False(centralTransitiveDependencyGroup11.Equals(centralTransitiveDependencyGroup22));

            Assert.False(centralTransitiveDependencyGroup12.Equals(centralTransitiveDependencyGroup21));
            Assert.False(centralTransitiveDependencyGroup12.Equals(centralTransitiveDependencyGroup22));

            Assert.False(centralTransitiveDependencyGroup21.Equals(centralTransitiveDependencyGroup22));
        }
        [Fact]
        public void Equals_WithOutOfOrderDependencies_ReturnsTrue()
        {
            // Arrange
            var leftSide = new CentralTransitiveDependencyGroup(
                                NuGetFramework.Parse("net461"),
                                new LibraryDependency[]
                                {
                                    new LibraryDependency()
                                    {
                                        LibraryRange = new LibraryRange()
                                        {
                                            Name = "first"
                                        }
                                    },
                                    new LibraryDependency()
                                    {
                                        LibraryRange = new LibraryRange()
                                        {
                                            Name = "second"
                                        }
                                    }
                                });
            var rightSide = new CentralTransitiveDependencyGroup(
                                NuGetFramework.Parse("net461"),
                                new LibraryDependency[]
                                {
                                    new LibraryDependency()
                                    {
                                        LibraryRange = new LibraryRange()
                                        {
                                            Name = "second"
                                        }
                                    },
                                    new LibraryDependency()
                                    {
                                        LibraryRange = new LibraryRange()
                                        {
                                            Name = "first"
                                        }
                                    }
                                });

            leftSide.Should().Be(rightSide);
        }

    }
}
