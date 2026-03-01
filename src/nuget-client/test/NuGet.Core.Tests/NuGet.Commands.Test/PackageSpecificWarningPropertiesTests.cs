// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using NuGet.Commands;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Versioning;
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
            var frameworkString = "net45";

            // Assert
            Assert.False(properties.Contains(NuGetLogCode.NU1500, libraryId, frameworkString));
        }

        [Fact]
        public void PackageSpecificWarningProperties_AddsValue()
        {

            // Arrange
            var code = NuGetLogCode.NU1500;
            var libraryId = "test_libraryId";
            var frameworkString = "net45";
            var properties = new PackageSpecificWarningProperties();
            properties.Add(code, libraryId, frameworkString);

            // Assert
            Assert.True(properties.Contains(code, libraryId, frameworkString));
            Assert.False(properties.Contains(code, libraryId, "random_target_graph"));
        }

        [Fact]
        public void PackageSpecificWarningProperties_AddsRangeValueWithGlobalTFM()
        {

            // Arrange
            ImmutableArray<NuGetLogCode> codes = [NuGetLogCode.NU1500, NuGetLogCode.NU1601, NuGetLogCode.NU1701];
            var libraryId = "test_libraryId";
            var frameworkString = "net45";
            var properties = new PackageSpecificWarningProperties();
            properties.AddRangeOfCodes(codes, libraryId, frameworkString);

            // Assert
            foreach (var code in codes)
            {
                Assert.False(properties.Contains(code, libraryId, "random_target_graph"));
                Assert.True(properties.Contains(code, libraryId, frameworkString));
            }
        }

        [Fact]
        public void PackageSpecificWarningProperties_CreatesPackageSpecificWarningPropertiesWithFrameworkConditionalDependencies()
        {

            // Arrange
            var net45Framework = NuGetFramework.Parse("net45");
            var netcoreappFramework = NuGetFramework.Parse("netcoreapp1.1");
            var net45Alias = "net45";
            var netcoreappAlias = "netcoreapp1.1";
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
                NoWarn = [NuGetLogCode.NU1603, NuGetLogCode.NU1107]
            };

            var dependency2 = new LibraryDependency()
            {
                LibraryRange = new LibraryRange
                {
                    Name = "test_library_2",
                    TypeConstraint = LibraryDependencyTarget.Package,
                    VersionRange = VersionRange.Parse("1.0.0")
                },
                NoWarn = [NuGetLogCode.NU1603, NuGetLogCode.NU1605]
            };

            var targetFrameworkInformation = new List<TargetFrameworkInformation>
            {
                new TargetFrameworkInformation()
                {
                    FrameworkName = net45Framework,
                    TargetAlias = net45Alias,
                    Dependencies = [dependency1]
                },
                new TargetFrameworkInformation()
                {
                    FrameworkName = netcoreappFramework,
                    TargetAlias = netcoreappAlias,
                    Dependencies = [dependency2]
                }
            };

            var packageSpec = new PackageSpec(targetFrameworkInformation);

            // Act
            var warningProperties = PackageSpecificWarningProperties.CreatePackageSpecificWarningProperties(packageSpec);

            // Assert
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_1", net45Alias));
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1107, "test_library_1", net45Alias));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library", net45Alias));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1701, "test_library_1", net45Alias));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_1", netcoreappAlias));

            Assert.True(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_2", netcoreappAlias));
            Assert.True(warningProperties.Contains(NuGetLogCode.NU1605, "test_library_2", netcoreappAlias));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library", netcoreappAlias));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1701, "test_library_2", netcoreappAlias));
            Assert.False(warningProperties.Contains(NuGetLogCode.NU1603, "test_library_2", net45Alias));
        }
    }
}
