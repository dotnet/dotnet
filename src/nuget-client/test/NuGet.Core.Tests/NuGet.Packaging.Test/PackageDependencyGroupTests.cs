// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;
using NuGet.Protocol;

namespace NuGet.Packaging.Test
{
    public class PackageDependencyGroupTests
    {
        [Fact]
        public void PackageDependencyGroup_Equals_SameVersionAndPackages()
        {
            var a = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            Assert.Equal(a, b);
        }

        [Fact]
        public void PackageDependencyGroup_Equals_SameVersionAndPackages_DifferentOrder()
        {
            var a = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                });

            Assert.Equal(a, b);
        }

        [Fact]
        public void PackageDependencyGroup_Equals_Same()
        {
            var a = new PackageDependencyGroup(
                 FrameworkConstants.CommonFrameworks.Net472,
                 new PackageDependency[] { new PackageDependency("a", new VersionRange(NuGetVersion.Parse("1.0.0"))) });
            var b = a;

            Assert.Equal(a, b);
        }

        [Fact]
        public void PackageDependencyGroup_Equals_DifferentVersion()
        {
            var a = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.NetStandard21,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void PackageDependencyGroup_Equals_DifferentPackages()
        {
            var a = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("e", new VersionRange(NuGetVersion.Parse("3.0.0"))),
                    new PackageDependency("f", new VersionRange(NuGetVersion.Parse("4.0.0"))),
                });

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void PackageDependencyGroup_Equals_Null()
        {
            var a = new PackageDependencyGroup(
                  FrameworkConstants.CommonFrameworks.Net472,
                  new PackageDependency[] { new PackageDependency("a", new VersionRange(NuGetVersion.Parse("1.0.0"))) });

            Assert.NotEqual(a, null);
        }

        [Fact]
        public void PackageDependencyGroup_GetHashCode_SameVersionAndPackages()
        {
            var a = new PackageDependencyGroup(
               FrameworkConstants.CommonFrameworks.Net472,
               new PackageDependency[]
               {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
               });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var aHashCode = a.GetHashCode();
            var bHashCode = b.GetHashCode();

            Assert.Equal(aHashCode, bHashCode);
        }

        [Fact]
        public void PackageDependencyGroup_GetHashCode_DifferentVersion()
        {
            var a = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.NetStandard21,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var aHashCode = a.GetHashCode();
            var bHashCode = b.GetHashCode();

            Assert.NotEqual(aHashCode, bHashCode);
        }

        [Fact]
        public void PackageDependencyGroup_GetHashCode_DifferentPackages()
        {
            var a = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("e", new VersionRange(NuGetVersion.Parse("3.0.0"))),
                    new PackageDependency("f", new VersionRange(NuGetVersion.Parse("4.0.0"))),
                });

            var aHashCode = a.GetHashCode();
            var bHashCode = b.GetHashCode();

            Assert.NotEqual(aHashCode, bHashCode);
        }

        [Fact]
        public void PackageDependencyGroup_GetHashCode_SameVersionAndPackages_DifferentOrder()
        {
            var a = new PackageDependencyGroup(
               FrameworkConstants.CommonFrameworks.Net472,
               new PackageDependency[]
               {
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
               });

            var b = new PackageDependencyGroup(
                FrameworkConstants.CommonFrameworks.Net472,
                new PackageDependency[]
                {
                    new PackageDependency("d", new VersionRange(NuGetVersion.Parse("2.0.0"))),
                    new PackageDependency("c", new VersionRange(NuGetVersion.Parse("1.0.0"))),
                });

            var aHashCode = a.GetHashCode();
            var bHashCode = b.GetHashCode();

            Assert.Equal(aHashCode, bHashCode);
        }

        [Fact]
        public void PackageDependencyGroup_Deserialize_ReturnsExpected()
        {
            // Arrange
            const string id = "PackageA";
            var range = new VersionRange(new NuGetVersion(1, 4, 1));
            var expectedPackageDependency = new PackageDependency(id, range);
            IEnumerable<PackageDependency> expectedTargetFramework = new List<PackageDependency>() { expectedPackageDependency };

            var targetFramework = NuGetFramework.Parse(".NETStandard1.0");

            // Act
            using var stringReader = new StringReader(PackageRegistrationDependencyGroupsJson);
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read();

            var serializer = JsonSerializer.Create(JsonExtensions.ObjectSerializationSettings);
            PackageDependencyGroup actualPackageDependencies = serializer.Deserialize<PackageDependencyGroup>(jsonReader);

            // Assert
            Assert.Equal(expectedTargetFramework, actualPackageDependencies.Packages);
            Assert.Equal(targetFramework, actualPackageDependencies.TargetFramework);
        }

        [Fact]
        public void PackageDependencyGroup_NoTargetFramework_Deserialize_ReturnsExpected()
        {
            // Arrange
            const string id = "PackageA";
            var range = new VersionRange(new NuGetVersion(1, 4, 1));
            var expectedPackageDependency = new PackageDependency(id, range);
            IEnumerable<PackageDependency> expectedPackages = new List<PackageDependency>() { expectedPackageDependency };

            var expectedTargetFramework = NuGetFramework.AnyFramework;

            // Act
            using var stringReader = new StringReader(PackageRegistrationDependencyGroupsJson_NoTargetFramework);
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read();

            var serializer = JsonSerializer.Create(JsonExtensions.ObjectSerializationSettings);
            PackageDependencyGroup actualPackageDependencies = serializer.Deserialize<PackageDependencyGroup>(jsonReader);

            // Assert
            Assert.Equal(expectedPackages, actualPackageDependencies.Packages);
            Assert.Equal(expectedTargetFramework, actualPackageDependencies.TargetFramework);
        }

        [Fact]
        public void PackageDependencyGroup_NoDependencies_Deserialize_ReturnsExpected()
        {
            // Arrange
            const string id = "PackageA";
            var version = new VersionRange(new NuGetVersion(1, 4, 1));
            var expectedPackageDependency = new PackageDependency(id, version);
            IEnumerable<PackageDependency> packages = new List<PackageDependency>() { };

            var targetFramework = NuGetFramework.Parse(".NETStandard1.0");

            // Act
            using var stringReader = new StringReader(PackageRegistrationDependencyGroupsJson_NoDependencies);
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read();

            var serializer = JsonSerializer.Create(JsonExtensions.ObjectSerializationSettings);
            PackageDependencyGroup actualPackageDependencies = serializer.Deserialize<PackageDependencyGroup>(jsonReader);

            // Assert
            Assert.Equal(packages, actualPackageDependencies.Packages);
            Assert.Equal(targetFramework, actualPackageDependencies.TargetFramework);
        }

        private const string PackageRegistrationDependencyGroupsJson = @"{
            ""targetFramework"": "".NETStandard1.0"",
            ""dependencies"": [
                {
                 ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.24.00/PackageA.1.6.0.json#dependencygroup/jquery"",
                 //  comment to test that they are ignored
                 ""@type"": ""PackageDependency"",
                 ""id"": ""PackageA"",
                 ""range"": ""[1.4.1, )"",
                 ""registration"": ""https://api.nuget.org/v3/registration0/PackageA/index.json""
                }
            ]
        }";

        private const string PackageRegistrationDependencyGroupsJson_NoTargetFramework = @"{
            ""dependencies"": [
                {
                 ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.24.00/PackageA.1.6.0.json#dependencygroup/jquery"",
                 //  comment to test that they are ignored
                 ""@type"": ""PackageDependency"",
                 ""id"": ""PackageA"",
                 ""range"": ""[1.4.1, )"",
                 ""registration"": ""https://api.nuget.org/v3/registration0/PackageA/index.json""
                }
            ]
        }";

        private const string PackageRegistrationDependencyGroupsJson_NoDependencies = @"{
            ""targetFramework"": "".NETStandard1.0"",
        }";
    }
}
