// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Shared;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackageSpecReferenceDependencyProviderTests
    {
        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, true)]
        public void GetLibrary__AddsCentralPackageVersionsIfDefined(bool cpvmEnabled, bool CentralPackageTransitivePinningEnabled, bool useLegacyDependencyGraphResolution)
        {
            // Arrange
            var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("2.0.0"));
            var centralVersionBar = new CentralPackageVersion("bar", VersionRange.Parse("2.0.0"));

            var dependencyFoo = new LibraryDependency(
                libraryRange: new LibraryRange("foo", versionRange: cpvmEnabled ? centralVersionFoo.VersionRange : null, LibraryDependencyTarget.Package),
                includeType: LibraryIncludeFlags.All,
                suppressParent: LibraryIncludeFlags.None,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: cpvmEnabled,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);

            var tfi = CreateTargetFrameworkInformation([dependencyFoo], new List<CentralPackageVersion>() { centralVersionFoo, centralVersionBar });
            var dependencyGraphSpec = CreateDependencyGraphSpecWithCentralDependencies(cpvmEnabled, CentralPackageTransitivePinningEnabled, true, tfi);
            var packSpec = dependencyGraphSpec.Projects[0];

            var dependencyProvider = new PackageSpecReferenceDependencyProvider([new ExternalProjectReference(packSpec.Name, packSpec, packSpec.FilePath, [])], NullLogger.Instance, useLegacyDependencyGraphResolution);
            // Act
            var libraryRange = new LibraryRange(packSpec.Name, versionRange: null, LibraryDependencyTarget.Project);

            var dependencies = dependencyProvider.GetLibrary(libraryRange, tfi.FrameworkName, tfi.TargetAlias).Dependencies.AsList();
            // Assert
            if (cpvmEnabled && CentralPackageTransitivePinningEnabled && useLegacyDependencyGraphResolution)
            {
                Assert.Equal(2, dependencies.Count);
                var barDep = dependencies.First(d => d.Name == "bar");
                Assert.NotNull(barDep);
                Assert.True(barDep.VersionCentrallyManaged);
                Assert.False(barDep.AutoReferenced);
                Assert.Equal(LibraryDependencyReferenceType.None, barDep.ReferenceType);
                Assert.Equal("[2.0.0, )", barDep.LibraryRange.VersionRange.ToNormalizedString());

                var fooDep = dependencies.First(d => d.Name == "foo");
                Assert.NotNull(fooDep);
                Assert.False(fooDep.AutoReferenced);
                Assert.True(fooDep.VersionCentrallyManaged);
                Assert.Equal(LibraryDependencyReferenceType.Direct, fooDep.ReferenceType);
                Assert.Equal("[2.0.0, )", fooDep.LibraryRange.VersionRange.ToNormalizedString());
            }
            else
            {
                Assert.Equal(1, dependencies.Count);
                var fooDep = dependencies.First(d => d.Name == "foo");
                Assert.NotNull(fooDep);
                Assert.Equal(fooDep.VersionCentrallyManaged, cpvmEnabled);
                Assert.Equal(fooDep.LibraryRange.VersionRange != null, cpvmEnabled);
                Assert.Equal(LibraryDependencyReferenceType.Direct, fooDep.ReferenceType);
            }
        }

        [Fact]
        public void GetLibrary_WithAssetTargetFallback_AndDependencyResolutionVariableSpecified_ReturnsCorrectDependencies()
        {
            // Arrange
            int dependencyCount = 1;
            var net60Framework = FrameworkConstants.CommonFrameworks.Net60;
            var net472Framework = FrameworkConstants.CommonFrameworks.Net472;
            var packageSpec = ProjectTestHelpers.GetPackageSpec(rootPath: "C:\\", projectName: "A", framework: net472Framework.GetShortFolderName(), dependencyName: "x");

            var dependencyProvider = new PackageSpecReferenceDependencyProvider([new ExternalProjectReference(packageSpec.Name, packageSpec, packageSpec.FilePath, [])], NullLogger.Instance);
            var assetTargetFallback = new AssetTargetFallbackFramework(net60Framework, new List<NuGetFramework> { net472Framework });
            var libraryRange = new LibraryRange(packageSpec.Name, versionRange: null, LibraryDependencyTarget.Project);

            // Act
            var dependencies = dependencyProvider.GetLibrary(libraryRange, assetTargetFallback, null).Dependencies.AsList();

            // Assert
            dependencies.Should().HaveCount(dependencyCount);
            dependencies[0].Name.Should().Be("x");
        }

        private static TargetFrameworkInformation CreateTargetFrameworkInformation(ImmutableArray<LibraryDependency> dependencies, List<CentralPackageVersion> centralVersionsDependencies)
        {
            NuGetFramework nugetFramework = new NuGetFramework("net40");
            var centralPackageVersions = centralVersionsDependencies.ToDictionary(cvd => cvd.Name, StringComparer.OrdinalIgnoreCase);

            TargetFrameworkInformation tfi = new TargetFrameworkInformation()
            {
                AssetTargetFallback = true,
                CentralPackageVersions = centralPackageVersions,
                Warn = false,
                FrameworkName = nugetFramework,
                Dependencies = dependencies,
            };

            return tfi;
        }

        private static DependencyGraphSpec CreateDependencyGraphSpecWithCentralDependencies(bool cpvmEnabled, bool tdpEnabled, bool legacyAlgorithmEnabled, params TargetFrameworkInformation[] tfis)
        {
            var packageSpec = new PackageSpec(tfis)
            {
                Name = "a",
                FilePath = "a.csproj",
            }.WithTestRestoreMetadata();

            packageSpec.RestoreMetadata.CentralPackageVersionsEnabled = cpvmEnabled;
            packageSpec.RestoreMetadata.CentralPackageTransitivePinningEnabled = tdpEnabled;
            packageSpec.RestoreMetadata.UseLegacyDependencyResolver = legacyAlgorithmEnabled;

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore("a");
            dgSpec.AddProject(packageSpec);
            return dgSpec;
        }
    }
}
