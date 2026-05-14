// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class ManifestVersionUtilityTest
    {
        [Fact]
        public void GetManifestVersionReturns1IfPackageTypesAreSet()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description",
                PackageTypes = new[]
                {
                    new PackageType("Bar", new System.Version(2, 0))
                }
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(1, version);
        }

        [Fact]
        public void GetManifestVersionReturns1IfNoNewPropertiesAreSet()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description"
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(1, version);
        }

        [Fact]
        public void GetManifestVersionReturns1IfFrameworkAssemblyHasValues()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description",
                FrameworkReferences = new List<FrameworkAssemblyReference> {
                    new FrameworkAssemblyReference("System.Data.dll", new [] { NuGetFramework.AnyFramework })
                }
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(1, version);
        }

        [Fact]
        public void GetManifestVersionReturns2IfCopyrightIsSet()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description",
                Copyright = "© Outercurve Foundation"
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(2, version);
        }

        [Fact]
        public void GetManifestVersionReturns2IfFrameworkAssemblyAndReferencesAreSet()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description",
                FrameworkReferences = new List<FrameworkAssemblyReference> {
                    new FrameworkAssemblyReference("System.Data.dll", new [] { NuGetFramework.AnyFramework })
                },
                PackageAssemblyReferences = new List<PackageReferenceSet> {
                    new PackageReferenceSet(new [] { "Foo.dll" })
                }
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(2, version);
        }

        [Fact]
        public void GetManifestVersionConsidersEmptyLists()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description",
                FrameworkReferences = new List<FrameworkAssemblyReference>(),
                PackageAssemblyReferences = new List<PackageReferenceSet>
                {
                }
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(1, version);
        }

        [Fact]
        public void GetManifestVersionReturns2IfReleaseNotesIsPresent()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description",
                ReleaseNotes = "Notes.txt"
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(2, version);
        }

        [Fact]
        public void GetManifestVersionIgnoresEmptyStrings()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "A, B" },
                Description = "Description",
                ReleaseNotes = ""
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(1, version);
        }

        [Fact]
        public void GetManifestVersionReturns3IfUsingSemanticVersioning()
        {
            // Arrange
            var metadata = new ManifestMetadata
            {
                Id = "Foo",
                Version = NuGetVersion.Parse("1.0.0-alpha"),
                Authors = new[] { "A, B" },
                Description = "Description"
            };

            // Act
            var version = ManifestVersionUtility.GetManifestVersion(metadata);

            // Assert
            Assert.Equal(3, version);
        }
    }
}
