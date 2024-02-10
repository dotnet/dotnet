// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

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
    }
}
