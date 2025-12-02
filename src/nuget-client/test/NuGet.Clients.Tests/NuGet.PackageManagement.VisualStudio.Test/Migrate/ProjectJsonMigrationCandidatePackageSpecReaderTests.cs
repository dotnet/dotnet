// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Text;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement.VisualStudio.Migrate;
using NuGet.RuntimeModel;
using NuGet.Versioning;
using NuGet.LibraryModel;
using Xunit;
using FileFormatException = NuGet.ProjectModel.FileFormatException;
using NuGet.Shared;

namespace NuGet.PackageManagement.VisualStudio.Test.Migrate
{
    public class ProjectJsonMigrationCandidatePackageSpecReaderTests
    {
        [Fact]
        public void GetPackageSpec_RootPackageDependency_WithVersion()
        {
            var json = "{\"dependencies\": { \"packageA\": \"1.2.3\" }}";
            var spec = GetPackageSpec(json);
            var dep = Assert.Single(spec.Dependencies);
            Assert.Equal("packageA", dep.Name);
            Assert.Equal(NuGetVersion.Parse("1.2.3"), dep.LibraryRange.VersionRange.MinVersion);
        }

        [Fact]
        public void GetPackageSpec_PackageDependencyMissingVersion_Throws()
        {
            var json = "{ \"dependencies\": { \"packageA\": { \"target\": \"package\" } }, \"frameworks\": { \"net6.0\": {} }}";
            var ex = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));
            Assert.Contains("Dependency version", ex.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetPackageSpec_PackageDependencyEmptyVersion_Throws()
        {
            // Matches semantics from JsonPackageSpecReaderTests.PackageEmptyVersion but adapted to migration reader messages.
            var json = "{ \"dependencies\": { \"packageA\": { \"target\": \"package\", \"version\": \"\" } }, \"frameworks\": { \"net6.0\": {} }}";
            var ex = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));
            Assert.Contains("Dependency version", ex.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetPackageSpec_PackageDependencyWhitespaceVersion_Throws()
        {
            // Matches semantics from JsonPackageSpecReaderTests.PackageWhitespaceVersion but adapted to migration reader messages.
            var json = "{ \"dependencies\": { \"packageA\": { \"target\": \"package\", \"version\": \"   \" } }, \"frameworks\": { \"net6.0\": {} }}";
            var ex = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));
            Assert.Contains("not a valid version string", ex.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetPackageSpec_ProjectDependencyMissingVersion_AllVersionRange()
        {
            var json = "{ \"dependencies\": { \"projA\": { \"target\": \"project\" } }, \"frameworks\": { \"net6.0\": {} }}";
            var spec = GetPackageSpec(json);
            var dep = Assert.Single(spec.Dependencies);
            Assert.Equal(VersionRange.All, dep.LibraryRange.VersionRange);
        }

        [Fact]
        public void GetPackageSpec_FrameworkSpecificDependency()
        {
            var json = "{ \"frameworks\": { \"net6.0\": { \"dependencies\": { \"packageB\": \"2.0.0\" } }}}";
            var spec = GetPackageSpec(json);
            var tfm = Assert.Single(spec.TargetFrameworks);
            Assert.Equal(NuGetFramework.Parse("net6.0"), tfm.FrameworkName);
            var dep = Assert.Single(tfm.Dependencies);
            Assert.Equal("packageB", dep.Name);
            Assert.Equal(NuGetVersion.Parse("2.0.0"), dep.LibraryRange.VersionRange.MinVersion);
        }

        [Fact]
        public void GetPackageSpec_RuntimeIdentifiers()
        {
            var json = "{ \"runtimes\": { \"win-x64\": {}, \"linux-x64\": {} }}";
            var spec = GetPackageSpec(json);
            Assert.Equal(2, spec.RuntimeGraph.Runtimes.Count);
            Assert.Contains("win-x64", spec.RuntimeGraph.Runtimes.Keys);
            Assert.Contains("linux-x64", spec.RuntimeGraph.Runtimes.Keys);
        }

        [Fact]
        public void GetPackageSpec_WhenSupportsValueIsEmptyObject_ReturnsSupports()
        {
            var expectedResult = RuntimeGraph.Empty;
            const string json = "{\"supports\":{}}";
            var packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]

        public void GetPackageSpec_WhenSupportsValueIsValidWithCompatibilityProfilesAndFrameworkRuntimePairs_ReturnsSupports()
        {
            FrameworkRuntimePair[] restoreContexts = new[]
            {
                new FrameworkRuntimePair(NuGetFramework.Parse("net472"), "b"),
                new FrameworkRuntimePair(NuGetFramework.Parse("net48"), "c")
            };
            var profile = new CompatibilityProfile(name: "a", restoreContexts);
            var expectedResult = new RuntimeGraph(new[] { profile });
            var json = $"{{\"supports\":{{\"{profile.Name}\":{{" +
                $"\"{restoreContexts[0].Framework.GetShortFolderName()}\":\"{restoreContexts[0].RuntimeIdentifier}\"," +
                $"\"{restoreContexts[1].Framework.GetShortFolderName()}\":[\"{restoreContexts[1].RuntimeIdentifier}\"]}}}}}}";
            var packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_SupportsProfiles()
        {
            var json = "{ \"supports\": { \"net6-win\": { \"net6.0\": \"win-x64\" } }}";
            var spec = GetPackageSpec(json);
            Assert.Single(spec.RuntimeGraph.Supports);
            Assert.Contains("net6-win", spec.RuntimeGraph.Supports.Keys);
        }

        [Fact]

        public void GetPackageSpec_WhenSupportsValueIsValidWithCompatibilityProfiles_ReturnsSupports()
        {
            var profile = new CompatibilityProfile(name: "a");
            var expectedResult = new RuntimeGraph(new[] { profile });
            var json = $"{{\"supports\":{{\"{profile.Name}\":{{}}}}}}";
            var packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_DependencyWithNoWarn()
        {
            var json = "{ \"dependencies\": { \"packageA\": { \"target\": \"package\", \"version\": \"1.0.0\", \"noWarn\": [ \"NU1500\", \"NU1107\" ] } }, \"frameworks\": { \"net8.0\": {} }}";
            var spec = GetPackageSpec(json);
            var dep = Assert.Single(spec.Dependencies);
            Assert.Equal(2, dep.NoWarn.Length);
        }

        [Fact]
        public void GetPackageSpec_DependencyWithSingleNoWarn()
        {
            var json = "{ \"dependencies\": { \"packageA\": { \"target\": \"package\", \"version\": \"1.0.0\", \"noWarn\": [ \"NU1500\" ] } }, \"frameworks\": { \"net8.0\": {} } }";
            var spec = GetPackageSpec(json);
            var dep = Assert.Single(spec.Dependencies);
            Assert.Single(dep.NoWarn);
        }

        [Fact]
        public void GetPackageSpec_DependencyWithEmptyNoWarnList()
        {
            var json = "{ \"dependencies\": { \"packageA\": { \"target\": \"package\", \"version\": \"1.0.0\", \"noWarn\": [  ] } }, \"frameworks\": { \"net8.0\": {} } }";
            var spec = GetPackageSpec(json);
            var dep = Assert.Single(spec.Dependencies);
            Assert.Empty(dep.NoWarn);
        }

        [Fact]
        public void GetPackageSpec_DependencyExplicitIncludeOverridesType()
        {
            // Mirrors JsonPackageSpecReaderTests.PackageSpecReader_ExplicitIncludesOverrideTypePlatform.
            var json = "{ \"dependencies\": { \"redist\": { \"version\": \"1.0.0\", \"type\": \"platform\", \"include\": \"analyzers\" } }, \"frameworks\": { \"net8.0\": {} } }";
            var spec = GetPackageSpec(json);
            var dep = Assert.Single(spec.Dependencies);
            Assert.Equal(LibraryIncludeFlags.Analyzers, dep.IncludeType);
        }

        internal static PackageSpecProjectJsonMigrationCandidate GetPackageSpec(string json)
        {
            string name = null;
            string packageSpecPath = null;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return GetPackageSpecUtf8JsonStreamReader(stream, name, packageSpecPath, EnvironmentVariableWrapper.Instance, snapshotValue: null);
            }
        }

        private static PackageSpecProjectJsonMigrationCandidate GetPackageSpecUtf8JsonStreamReader(Stream stream, string name, string packageSpecPath, IEnvironmentVariableReader environmentVariableReader, string snapshotValue = null)
        {
            var reader = new Utf8JsonStreamReader(stream);
            var packageSpec = ProjectJsonMigrationCandidatePackageSpecReader.GetPackageSpec(ref reader, name, packageSpecPath, environmentVariableReader, snapshotValue);

            reader.Dispose();

            return packageSpec;
        }
    }
}
