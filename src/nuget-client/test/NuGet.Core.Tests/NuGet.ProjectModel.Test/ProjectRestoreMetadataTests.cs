// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class ProjectRestoreMetadataTests
    {
        [Fact]
        public void Equals_WithSameObject_ReturnsTrue()
        {
            // Arrange
            var metadata = CreateProjectRestoreMetadata();

            // Act & Assert
            metadata.Equals(metadata).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithNullObject_ReturnsFalse()
        {
            // Arrange
            var metadata = CreateProjectRestoreMetadata();

            // Act & Assert
            metadata.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithSameCaseInPaths_ReturnsTrue()
        {
            // Arrange
            var metadata1 = CreateProjectRestoreMetadata();
            var metadata2 = metadata1.Clone();

            metadata2.ProjectPath = "ProjectPath";
            metadata2.ProjectJsonPath = "ProjectJsonPath";
            metadata2.OutputPath = "OutputPath";
            metadata2.ProjectName = "ProjectName";
            metadata2.ProjectUniqueName = "ProjectUniqueName";
            metadata2.PackagesPath = "PackagesPath";
            metadata2.ConfigFilePaths = new List<string>() { "config1" };
            metadata2.FallbackFolders = new List<string>() { "fallback1" };

            // Act & Assert
            metadata1.Equals(metadata2).Should().BeTrue();
        }

        [PlatformFact(Platform.Windows, Platform.Darwin)]
        public void Equals_WithDifferentCaseInPathsOnWindowsAndMac_ReturnsTrue()
        {
            // Arrange
            var metadata1 = CreateProjectRestoreMetadata();
            var metadata2 = metadata1.Clone();

            metadata2.ProjectPath = "projectPath";
            metadata2.ProjectJsonPath = "projectJsonPath";
            metadata2.OutputPath = "outputPath";
            metadata2.ProjectName = "projectName";
            metadata2.ProjectUniqueName = "projectUniqueName";
            metadata2.PackagesPath = "packagesPath";
            metadata2.CacheFilePath = "cacheFilePath";
            metadata2.ConfigFilePaths = new List<string>() { "Config1" };
            metadata2.FallbackFolders = new List<string>() { "Fallback1" };

            // Act & Assert
            metadata1.Equals(metadata2).Should().BeTrue();
        }

        [PlatformFact(Platform.Linux)]
        public void Equals_WithDifferentCaseInPathsOnLinux_ReturnsFalse()
        {
            // Arrange
            var metadata1 = CreateProjectRestoreMetadata();
            var metadata2 = metadata1.Clone();

            metadata2.ProjectPath = "projectPath";
            metadata2.ProjectJsonPath = "projectJsonPath";
            metadata2.OutputPath = "outputPath";
            metadata2.ProjectName = "projectName";
            metadata2.ProjectUniqueName = "projectUniqueName";
            metadata2.PackagesPath = "packagesPath";
            metadata2.CacheFilePath = "cacheFilePath";
            metadata2.ConfigFilePaths = new List<string>() { "Config1" };
            metadata2.FallbackFolders = new List<string>() { "Fallback1" };

            // Act & Assert
            metadata1.Equals(metadata2).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithDifferentRestoreLockProperties_ReturnsFalse()
        {
            // Arrange
            var metadata1 = CreateProjectRestoreMetadata();
            var metadata2 = metadata1.Clone();
            var metadata3 = metadata1.Clone();
            var metadata4 = metadata1.Clone();

            metadata2.RestoreLockProperties = new RestoreLockProperties("false", null, false);
            metadata3.RestoreLockProperties = new RestoreLockProperties("true", "tempPath", false);
            metadata4.RestoreLockProperties = new RestoreLockProperties("true", null, true);

            // Act & Assert
            metadata1.Equals(metadata2).Should().BeFalse();
            metadata1.Equals(metadata3).Should().BeFalse();
            metadata1.Equals(metadata4).Should().BeFalse();
        }

        private ProjectRestoreMetadata CreateProjectRestoreMetadata()
        {
            var projectReference = new ProjectRestoreReference
            {
                ProjectPath = "Path",
                ProjectUniqueName = "ProjectUniqueName",
                IncludeAssets = LibraryIncludeFlags.All,
                ExcludeAssets = LibraryIncludeFlags.Analyzers,
                PrivateAssets = LibraryIncludeFlags.Build
            };

            var nugetFramework = NuGetFramework.Parse("net461");
            var originalPRMFI = new ProjectRestoreMetadataFrameworkInfo(nugetFramework)
            {
                ProjectReferences = new List<ProjectRestoreReference>() { projectReference }
            };

            var targetframeworks = new List<ProjectRestoreMetadataFrameworkInfo>() { originalPRMFI };
            var allWarningsAsErrors = true;
            var noWarn = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1500 };
            var warningsAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1501 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1802, NuGetLogCode.NU1803 };
            var warningProperties = new WarningProperties(allWarningsAsErrors: allWarningsAsErrors, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors);
            var restoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile: "true", nuGetLockFilePath: null, restoreLockedMode: false);
            var originalProjectRestoreMetadata = new ProjectRestoreMetadata
            {
                ProjectStyle = ProjectStyle.PackageReference,
                ProjectPath = "ProjectPath",
                ProjectJsonPath = "ProjectJsonPath",
                OutputPath = "OutputPath",
                ProjectName = "ProjectName",
                ProjectUniqueName = "ProjectUniqueName",
                PackagesPath = "PackagesPath",
                CacheFilePath = "CacheFilePath",
                CrossTargeting = true,
                LegacyPackagesDirectory = true,
                ValidateRuntimeAssets = true,
                SkipContentFileWrite = true,
                TargetFrameworks = targetframeworks,
                Sources = new List<PackageSource>() { new PackageSource("http://api.nuget.org/v3/index.json") },
                FallbackFolders = new List<string>() { "fallback1" },
                ConfigFilePaths = new List<string>() { "config1" },
                OriginalTargetFrameworks = new List<string>() { "net45" },
                Files = new List<ProjectRestoreMetadataFile>() { new ProjectRestoreMetadataFile("packagePath", "absolutePath") },
                ProjectWideWarningProperties = warningProperties,
                RestoreLockProperties = restoreLockProperties
            };

            return originalProjectRestoreMetadata;
        }

        [Theory]
        [InlineData("true", "path", true, "true", "path", true, true)]
        [InlineData("false", "path", true, "true", "path", true, false)]
        [InlineData("true", "path", true, "true", "path2", true, false)]
        [InlineData("true", "path", true, "true", "path", false, false)]
        public void Equals_WithRestoreLockProperties(string leftRPWLF, string leftLockFilePath, bool leftRestoreLockedMode,
            string rightRPWLF, string rightLockFilePath, bool rightRestoreLockedMode,
            bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                RestoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile: leftRPWLF, nuGetLockFilePath: leftLockFilePath, restoreLockedMode: leftRestoreLockedMode)
            };
            var rightSide = new ProjectRestoreMetadata
            {
                RestoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile: rightRPWLF, nuGetLockFilePath: rightLockFilePath, restoreLockedMode: rightRestoreLockedMode)
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(ProjectStyle.PackageReference, ProjectStyle.PackageReference, true)]
        [InlineData(ProjectStyle.PackageReference, ProjectStyle.DotnetCliTool, false)]
        public void Equals_WithProjectStyle(ProjectStyle left, ProjectStyle right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                ProjectStyle = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ProjectStyle = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(@"C:\path", @"C:\path", true)]
        [InlineData(@"C:\path;C:\path\2", @"C:\path\2;C:\path", true)]
        [InlineData(@"C:\path;C:\path\2;C:\path\3", @"C:\path\2;C:\path", false)]
        public void Equals_WithFallbackFolders(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                FallbackFolders = left.Split(';')
            };
            var rightSide = new ProjectRestoreMetadata
            {
                FallbackFolders = right.Split(';')
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(@"C:\path", @"C:\path", true)]
        [InlineData(@"C:\path;C:\path\2", @"C:\path\2;C:\path", true)]
        [InlineData(@"C:\path;C:\path\2;C:\path\3", @"C:\path\2;C:\path", false)]
        public void Equals_WithConfigFilePaths(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                ConfigFilePaths = left.Split(';')
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ConfigFilePaths = right.Split(';')
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net462", "net462", true)]
        [InlineData("net462", "net4.6.2", false)]
        [InlineData("net462;net461", "net462;net461", true)]
        [InlineData("net5", "NET5", true)]
        [InlineData("net5;net462", "net462;NET5.0", false)]

        public void Equals_WithOriginalTargetFrameworks(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                OriginalTargetFrameworks = left.Split(';')
            };
            var rightSide = new ProjectRestoreMetadata
            {
                OriginalTargetFrameworks = right.Split(';')
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithCrossTargeting(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CrossTargeting = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CrossTargeting = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithLegacyPackagesDirectory(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                LegacyPackagesDirectory = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                LegacyPackagesDirectory = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithValidateRuntimeAssets(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                ValidateRuntimeAssets = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ValidateRuntimeAssets = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithCentralPackageVersionsEnabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionsEnabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionsEnabled = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithCentralPackageFloatingVersionsEnabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageFloatingVersionsEnabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageFloatingVersionsEnabled = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithCentralPackageVersionOverrideDisabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionOverrideDisabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionOverrideDisabled = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithCentralPackageTransitivePinningEnabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageTransitivePinningEnabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageTransitivePinningEnabled = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithSkipContentFileWrite(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                SkipContentFileWrite = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                SkipContentFileWrite = right
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Fact]
        public void Equals_WithEquivalentWarningProperties_ReturnsTrue()
        {
            var allWarningsAsErrors = true;
            var noWarn = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1500 };
            var warningsAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1501 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1802, NuGetLogCode.NU1803 };

            var leftSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: allWarningsAsErrors, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: allWarningsAsErrors, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            AssertEquality(leftSide, rightSide);
        }

        [Fact]
        public void Equals_WithDifferentWarningProperties_ReturnsTrue()
        {
            var noWarn = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1500 };
            var warningsAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1501 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1802, NuGetLogCode.NU1803 };

            var leftSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: true, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: false, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            AssertEquality(expected: false, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net462", "net461;net472", false)]
        [InlineData("net462;net472", "net472;net462", true)]
        [InlineData("net462;net472;net5.0", "net472;net462", false)]

        public void Equals_WithTargetFrameworks(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                TargetFrameworks = left.Split(';').Select(e => new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse(e))
                {
                    TargetAlias = e,
                    ProjectReferences = new List<ProjectRestoreReference>() { new ProjectRestoreReference() { ProjectPath = "path" } }
                }).ToList()
            };
            var rightSide = new ProjectRestoreMetadata
            {
                TargetFrameworks = right.Split(';').Select(e => new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse(e))
                {
                    TargetAlias = e,
                    ProjectReferences = new List<ProjectRestoreReference>() { new ProjectRestoreReference() { ProjectPath = "path" } }
                }).ToList()
            };
            AssertEquality(expected, leftSide, rightSide);
        }


        [Theory]
        [InlineData(@"C:\path", @"C:\path", true)]
        [InlineData(@"C:\path;C:\path\2", @"C:\path\2;C:\path", true)]
        [InlineData(@"C:\path;C:\PATH\2", @"C:\PATH\2;C:\path", true)]
        [InlineData(@"C:\path;C:\path\2;C:\path\3", @"C:\path\2;C:\path", false)]
        public void Equals_WithSources(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                Sources = left.Split(';').Select(e => new PackageSource(e)).ToList()
            };
            var rightSide = new ProjectRestoreMetadata
            {
                Sources = right.Split(';').Select(e => new PackageSource(e)).ToList()
            };
            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("true", "path", true, "true", "path", true, true)]
        [InlineData("false", "path", true, "true", "path", true, false)]
        [InlineData("true", "path", true, "true", "path2", true, false)]
        [InlineData("true", "path", true, "true", "path", false, false)]
        public void HashCode_WithRestoreLockProperties(string leftRPWLF, string leftLockFilePath, bool leftRestoreLockedMode,
            string rightRPWLF, string rightLockFilePath, bool rightRestoreLockedMode,
            bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                RestoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile: leftRPWLF, nuGetLockFilePath: leftLockFilePath, restoreLockedMode: leftRestoreLockedMode)
            };
            var rightSide = new ProjectRestoreMetadata
            {
                RestoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile: rightRPWLF, nuGetLockFilePath: rightLockFilePath, restoreLockedMode: rightRestoreLockedMode)
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(ProjectStyle.PackageReference, ProjectStyle.PackageReference, true)]
        [InlineData(ProjectStyle.PackageReference, ProjectStyle.DotnetCliTool, false)]
        public void HashCode_WithProjectStyle(ProjectStyle left, ProjectStyle right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                ProjectStyle = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ProjectStyle = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(@"C:\path", @"C:\path", true)]
        [InlineData(@"C:\path;C:\path\2", @"C:\path\2;C:\path", true)]
        [InlineData(@"C:\path;C:\path\2;C:\path\3", @"C:\path\2;C:\path", false)]
        public void HashCode_WithFallbackFolders(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                FallbackFolders = left.Split(';')
            };
            var rightSide = new ProjectRestoreMetadata
            {
                FallbackFolders = right.Split(';')
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(@"C:\path", @"C:\path", true)]
        [InlineData(@"C:\path;C:\path\2", @"C:\path\2;C:\path", true)]
        [InlineData(@"C:\path;C:\path\2;C:\path\3", @"C:\path\2;C:\path", false)]
        public void HashCode_WithConfigFilePaths(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                ConfigFilePaths = left.Split(';')
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ConfigFilePaths = right.Split(';')
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net462", "net462", true)]
        [InlineData("net462", "net4.6.2", false)]
        [InlineData("net462;net461", "net462;net461", true)]
        [InlineData("net5", "NET5", true)]
        [InlineData("net5;net462", "net462;NET5.0", false)]

        public void HashCode_WithOriginalTargetFrameworks(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                OriginalTargetFrameworks = left.Split(';')
            };
            var rightSide = new ProjectRestoreMetadata
            {
                OriginalTargetFrameworks = right.Split(';')
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithCrossTargeting(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CrossTargeting = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CrossTargeting = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithLegacyPackagesDirectory(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                LegacyPackagesDirectory = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                LegacyPackagesDirectory = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithValidateRuntimeAssets(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                ValidateRuntimeAssets = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ValidateRuntimeAssets = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithCentralPackageVersionOverrideDisabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionOverrideDisabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionOverrideDisabled = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithCentralPackageTransitivePinningEnabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageTransitivePinningEnabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageTransitivePinningEnabled = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithCentralPackageVersionsEnabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionsEnabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageVersionsEnabled = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithCentralPackageFloatingVersionsEnabled(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                CentralPackageFloatingVersionsEnabled = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                CentralPackageFloatingVersionsEnabled = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithSkipContentFileWrite(bool left, bool right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                SkipContentFileWrite = left
            };
            var rightSide = new ProjectRestoreMetadata
            {
                SkipContentFileWrite = right
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        [Fact]
        public void HashCode_WithEquivalentWarningProperties_ReturnsTrue()
        {
            var allWarningsAsErrors = true;
            var noWarn = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1500 };
            var warningsAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1501 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1802, NuGetLogCode.NU1803 };

            var leftSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: allWarningsAsErrors, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: allWarningsAsErrors, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            AssertHashCode(leftSide, rightSide);
        }

        [Fact]
        public void HashCode_WithDifferentWarningProperties_ReturnsTrue()
        {
            var noWarn = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1500 };
            var warningsAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1501 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1802, NuGetLogCode.NU1803 };

            var leftSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: true, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            var rightSide = new ProjectRestoreMetadata
            {
                ProjectWideWarningProperties = new WarningProperties(allWarningsAsErrors: false, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors)
            };
            AssertHashCode(expected: false, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net462", "net461;net472", false)]
        [InlineData("net462;net472", "net472;net462", true)]
        [InlineData("net462;net472;net5.0", "net472;net462", false)]

        public void HashCode_WithTargetFrameworks(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                TargetFrameworks = left.Split(';').Select(e => new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse(e))
                {
                    TargetAlias = e,
                    ProjectReferences = new List<ProjectRestoreReference>() { new ProjectRestoreReference() { ProjectPath = "path" } }
                }).ToList()
            };
            var rightSide = new ProjectRestoreMetadata
            {
                TargetFrameworks = right.Split(';').Select(e => new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse(e))
                {
                    TargetAlias = e,
                    ProjectReferences = new List<ProjectRestoreReference>() { new ProjectRestoreReference() { ProjectPath = "path" } }
                }).ToList()
            };
            AssertHashCode(expected, leftSide, rightSide);
        }


        [Theory]
        [InlineData(@"C:\path", @"C:\path", true)]
        [InlineData(@"C:\path;C:\path\2", @"C:\path\2;C:\path", true)]
        [InlineData(@"C:\path;C:\PATH\2", @"C:\PATH\2;C:\path", true)]
        [InlineData(@"C:\path;C:\path\2;C:\path\3", @"C:\path\2;C:\path", false)]
        public void HashCode_WithSources(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadata
            {
                Sources = left.Split(';').Select(e => new PackageSource(e)).ToList()
            };
            var rightSide = new ProjectRestoreMetadata
            {
                Sources = right.Split(';').Select(e => new PackageSource(e)).ToList()
            };
            AssertHashCode(expected, leftSide, rightSide);
        }

        private static void AssertEquality(ProjectRestoreMetadata leftSide, ProjectRestoreMetadata rightSide)
        {
            AssertEquality(true, leftSide, rightSide);
        }

        private static void AssertEquality(bool expected, ProjectRestoreMetadata leftSide, ProjectRestoreMetadata rightSide)
        {
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        private static void AssertHashCode(ProjectRestoreMetadata leftSide, ProjectRestoreMetadata rightSide)
        {
            AssertHashCode(true, leftSide, rightSide);
        }

        private static void AssertHashCode(bool expected, ProjectRestoreMetadata leftSide, ProjectRestoreMetadata rightSide)
        {
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }
    }
}
