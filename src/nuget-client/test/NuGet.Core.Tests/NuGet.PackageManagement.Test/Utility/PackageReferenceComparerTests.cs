// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class PackageReferenceComparerTests
    {
        private static readonly PackageReferenceComparer Comparer = PackageReferenceComparer.Instance;
        private static readonly NuGetVersion Version100 = NuGetVersion.Parse("1.0.0");
        private static readonly NuGetVersion Version200 = NuGetVersion.Parse("2.0.0");
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "a", Version100);
        private static readonly PackageReference ReferenceA0 = new PackageReference(
            PackageIdentity,
            NuGetFramework.Parse("net50"),
            userInstalled: true,
            developmentDependency: true,
            requireReinstallation: true,
            VersionRange.All);
        private static readonly PackageReference ReferenceA1 = new PackageReference(
            PackageIdentity,
            NuGetFramework.Parse("net472"),
            userInstalled: false,
            developmentDependency: false,
            requireReinstallation: false,
            new VersionRange(minVersion: Version100, maxVersion: Version200));

        [Fact]
        public void Equals_WhenArgumentsAreNull_ReturnsTrue()
        {
            Assert.True(Comparer.Equals(x: null, y: null));
        }

        [Fact]
        public void Equals_WhenArgumentsAreSame_ReturnsTrue()
        {
            Assert.True(Comparer.Equals(ReferenceA0, ReferenceA0));
        }

        [Fact]
        public void Equals_WhenArgumentsAreEqual_ReturnsTrue()
        {
            Assert.True(Comparer.Equals(ReferenceA0, ReferenceA1));
        }

        [Fact]
        public void Equals_WhenArgumentsAreNotEqual_ReturnsFalse()
        {
            var referenceB = new PackageReference(
                new PackageIdentity(id: "b", ReferenceA0.PackageIdentity.Version),
                ReferenceA0.TargetFramework,
                ReferenceA0.IsUserInstalled,
                ReferenceA0.IsDevelopmentDependency,
                ReferenceA0.RequireReinstallation,
                ReferenceA0.AllowedVersions);

            Assert.False(Comparer.Equals(ReferenceA0, referenceB));
        }

        [Fact]
        public void GetHashCode_WhenArgumentIsValid_ReturnsHashCode()
        {
            int expectedResult = PackageIdentityComparer.Default.GetHashCode(PackageIdentity);
            int actualResult = Comparer.GetHashCode(ReferenceA0);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
