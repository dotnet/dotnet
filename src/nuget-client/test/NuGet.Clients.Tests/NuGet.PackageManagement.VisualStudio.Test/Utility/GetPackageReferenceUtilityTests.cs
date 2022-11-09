// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using NuGet.PackageManagement.VisualStudio.Utility;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class GetPackageReferenceUtilityTests
    {
        [Theory]
        [InlineData("a", "1.0", "b", "1.0")]
        [InlineData("a", "1.0", "b", null)]
        [InlineData("09a", "1.0", "b", null)]
        [InlineData("a", "1.0", "a", "2.0")]
        public void PackageReferenceMergeComparer_AscendingPackageIdentities_ReturnLessThanZero(string id1, string version1, string id2, string version2)
        {
            // Arrange
            PackageReference first = CreatePackageReference(id1, version1, "net6.0");
            PackageReference second = CreatePackageReference(id2, version2, framework: null);

            // Act
            int result = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(first, second);

            // Assert
            Assert.True(result < 0);
        }

        [Theory]
        [InlineData("b", "1.0", "a", "1.0")]
        [InlineData("a", "2.0", "a", "1.0")]
        public void PackageReferenceMergeComparer_DescendingPackageIdentities_ReturnGreaterThanZero(string id1, string version1, string id2, string version2)
        {
            // Arrange
            PackageReference first = CreatePackageReference(id1, version1, "net6.0");
            PackageReference second = CreatePackageReference(id2, version2, framework: null);

            // Act
            int result = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(first, second);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void PackageReferenceMergeComparer_EqualPackageIdentities_ReturnsZero()
        {
            // Arrange
            PackageReference first = CreatePackageReference("Abc", "1.0", "net6.0");
            PackageReference second = CreatePackageReference("abc", "1.0", framework: null);

            // Act
            int result = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(first, second);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void PackageReferenceMergeComparer_AnyNullArgument_ReturnsExpectedResult()
        {
            // Arrange
            PackageReference testPkgReference = CreatePackageReference("Abc", "1.0", "net6.0");

            // Act and Assert 1: Second is null
            int result1 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(testPkgReference, null);
            Assert.True(result1 > 0);

            // Act and Assert 2: First is null
            int result2 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(null, testPkgReference);
            Assert.True(result2 < 0);

            // Act and Assert 3: Both are null
            int result3 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(null, null);
            Assert.True(result3 == 0);

            var prWithNullIdentity = new PackageReference(null, null);

            // Act and Assert 4: Second with null PackageIdentity
            int result4 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(testPkgReference, prWithNullIdentity);
            Assert.True(result4 > 0);

            // Act and Assert 5: First with null PackageIdentity
            int result5 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(prWithNullIdentity, testPkgReference);
            Assert.True(result5 < 0);

            // Act and Assert 6: First with null PackageIdentity and null
            int result6 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(prWithNullIdentity, null);
            Assert.Equal(0, result6);

            PackageReference pkgRefWithNullVersion1 = CreatePackageReference("nullVersion", null, framework: null);
            PackageReference pkgRefWithNullVersion2 = CreatePackageReference("otherVersion", null, framework: null);

            // Act and Assert 7: First Null PackageIdentity and Second with null version, compare only with package IDs
            int result7 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(prWithNullIdentity, pkgRefWithNullVersion1);
            Assert.True(result7 < 0);

            // Act and Assert 8: Only package ID's on both
            int result8 = GetPackageReferenceUtility.PackageReferenceMergeComparer.Compare(pkgRefWithNullVersion1, pkgRefWithNullVersion2);
            Assert.True(result8 < 0);
        }

        private static PackageReference CreatePackageReference(string id, string version, NuGetFramework fw) => new PackageReference(CreatePackageIdentity(id, version), fw);

        private static PackageReference CreatePackageReference(string id, string version, string framework)
        {
            NuGetFramework fw = string.IsNullOrEmpty(framework) ? null : NuGetFramework.Parse(framework);
            return CreatePackageReference(id, version, fw);
        }

        private static PackageIdentity CreatePackageIdentity(string id, string version)
        {
            NuGetVersion ver = string.IsNullOrEmpty(version) ? null : NuGetVersion.Parse(version);
            return new PackageIdentity(id, ver);
        }
    }
}
