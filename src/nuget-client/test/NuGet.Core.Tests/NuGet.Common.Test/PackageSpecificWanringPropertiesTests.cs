// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Commands;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class PackageSpecificWarningPropertiesTests
    {

        [Fact]
        public void PackageSpecificWarningProperties_DefaultValue()
        {

            // Arrange
            var properties = new PackageSpecificWarningProperties();
            var libraryId = "test_libraryId";
            var targetFramework = NuGetFramework.Parse("net45");

            // Assert
            Assert.False(properties.Contains(NuGetLogCode.NU1500, libraryId, targetFramework));
        }

        [Fact]
        public void PackageSpecificWarningProperties_AddsValue()
        {

            // Arrange
            var code = NuGetLogCode.NU1500;
            var libraryId = "test_libraryId";
            var targetFramework = NuGetFramework.Parse("net45");
            var properties = new PackageSpecificWarningProperties();
            properties.Add(code, libraryId, targetFramework);

            // Assert
            Assert.True(properties.Contains(code, libraryId, targetFramework));
            Assert.False(properties.Contains(code, libraryId, NuGetFramework.Parse("random_target_graph")));
        }

        [Fact]
        public void PackageSpecificWarningProperties_AddsRangeValueWithGlobalTFM()
        {

            // Arrange
            var codes = new List<NuGetLogCode> { NuGetLogCode.NU1500, NuGetLogCode.NU1601, NuGetLogCode.NU1701 };
            var libraryId = "test_libraryId";
            var targetFramework = NuGetFramework.Parse("net45");
            var properties = new PackageSpecificWarningProperties();
            properties.AddRangeOfCodes(codes, libraryId, targetFramework);

            // Assert
            foreach (var code in codes)
            {
                Assert.False(properties.Contains(code, libraryId, NuGetFramework.Parse("random_target_graph")));
                Assert.True(properties.Contains(code, libraryId, targetFramework));
            }
        }

        [Fact]
        public void PackageSpecificWarningProperties_CreatesPackageSpecificWarningPropertiesWithUnconditionalDependencies()
        {

            // Arrange
            var net45Framework = NuGetFramework.Parse("net45");
            var netcoreappFramework = NuGetFramework.Parse("netcoreapp1.1");
            var libraryId = "test_library";
            var libraryVersion = "1.0.0";
            var NoWarnList = new List<NuGetLogCode>
            {
                NuGetLogCode.NU1603,
                NuGetLogCode.NU1605
            };

            var targetFrameworkInformation = new List<TargetFrameworkInformation>
            {
                new TargetFrameworkInformation()
                {
                    FrameworkName = net45Framework
                },
                new TargetFrameworkInformation()
                {
                    FrameworkName = netcoreappFramework
                }
            };

            var packageSpec = new PackageSpec(targetFrameworkInformation)
            {
                Dependencies = new List<LibraryDependency>
                {
                    new LibraryDependency ()
                    {
                        LibraryRange = new LibraryRange
                        {
                            Name = libraryId,
                            TypeConstraint = LibraryDependencyTarget.Package,
                            VersionRange = VersionRange.Parse(libraryVersion)
                        },
                        NoWarn = NoWarnList
                    }
                }
            };

            // Act
            var warningProperties = PackageSpecificWarningProperties.CreatePackageSpecificWarningProperties(packageSpec);

            // Assert
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1603, libraryId, net45Framework));
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1603, libraryId, netcoreappFramework));
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1605, libraryId, net45Framework));
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1605, libraryId, netcoreappFramework));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, libraryId, NuGetFramework.Parse("random_framework")));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1605, libraryId, NuGetFramework.Parse("random_framework")));
        }

        [Fact]
        public void PackageSpecificWarningProperties_CreatesPackageSpecificWarningPropertiesWithFrameworkConditionalDependencies()
        {

            // Arrange
            var net45Framework = NuGetFramework.Parse("net45");
            var netcoreappFramework = NuGetFramework.Parse("netcoreapp1.1");
            var NoWarnList = new List<NuGetLogCode>
            {
                NuGetLogCode.NU1603,
                NuGetLogCode.NU1605
            };

            var dependency1 = new LibraryDependency()
            {
                LibraryRange = new LibraryRange
                {
                    Name = "test_library_1",
                    TypeConstraint = LibraryDependencyTarget.Package,
                    VersionRange = VersionRange.Parse("1.0.0")
                },
                NoWarn = new List<NuGetLogCode>
                {
                    NuGetLogCode.NU1603,
                    NuGetLogCode.NU1107
                }
            };

            var dependency2 = new LibraryDependency()
            {
                LibraryRange = new LibraryRange
                {
                    Name = "test_library_2",
                    TypeConstraint = LibraryDependencyTarget.Package,
                    VersionRange = VersionRange.Parse("1.0.0")
                },
                NoWarn = new List<NuGetLogCode>
                {
                    NuGetLogCode.NU1603,
                    NuGetLogCode.NU1605
                }
            };

            var targetFrameworkInformation = new List<TargetFrameworkInformation>
            {
                new TargetFrameworkInformation()
                {
                    FrameworkName = net45Framework,
                    Dependencies = new List<LibraryDependency>
                    {
                        dependency1
                    }
                },
                new TargetFrameworkInformation()
                {
                    FrameworkName = netcoreappFramework,
                    Dependencies = new List<LibraryDependency>
                    {
                        dependency2
                    }
                }
            };

            var packageSpec = new PackageSpec(targetFrameworkInformation);

            // Act
            var warningProperties = PackageSpecificWarningProperties.CreatePackageSpecificWarningProperties(packageSpec);

            // Assert
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_1", net45Framework));
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1107, "test_library_1", net45Framework));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library", net45Framework));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1701, "test_library_1", net45Framework));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_1", netcoreappFramework));

            Assert.True(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_2", netcoreappFramework));
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1605, "test_library_2", netcoreappFramework));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library", netcoreappFramework));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1701, "test_library_2", netcoreappFramework));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_2", net45Framework));
        }
    }
}
