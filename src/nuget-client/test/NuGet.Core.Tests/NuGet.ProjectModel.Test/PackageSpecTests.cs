// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackageSpecTests
    {
        internal static LibraryDependency CreateLibraryDependency()
        {
            var dependency = new LibraryDependency(
                libraryRange: new LibraryRange(Guid.NewGuid().ToString(), LibraryDependencyTarget.Package),
                includeType: LibraryIncludeFlags.None,
                suppressParent: LibraryIncludeFlags.ContentFiles,
                noWarn: [NuGetLogCode.NU1000, NuGetLogCode.NU1001, NuGetLogCode.NU1002],
                autoReferenced: false,
                generatePathProperty: false,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuffff",
                versionOverride: null);

            return dependency;
        }

        private PackageSpec CreatePackageSpec()
        {
            var originalTargetFrameworkInformation = CreateTargetFrameworkInformation();
            var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { originalTargetFrameworkInformation });
            packageSpec.RestoreMetadata = CreateProjectRestoreMetadata();
            packageSpec.FilePath = "FilePath";
            packageSpec.Name = "Name";
            packageSpec.Version = new NuGetVersion("1.0.0");
            packageSpec.RuntimeGraph = CreateRuntimeGraph();
            packageSpec.RestoreSettings = CreateProjectRestoreSettings();
            return packageSpec;
        }

        public static RuntimeGraph CreateRuntimeGraph()
        {
            var runtimeDescription = new RuntimeDescription(Guid.NewGuid().ToString());
            var compatibilityProfile = CreateCompatibilityProfile("CompatibilityProfile");
            return new RuntimeGraph(new RuntimeDescription[] { runtimeDescription }, new CompatibilityProfile[] { compatibilityProfile });
        }

        [Theory]
        [InlineData("ModifyOriginalTargetFrameworkInformationAdd", true)]
        [InlineData("ModifyOriginalTargetFrameworkInformationEdit", true)]
        [InlineData("ModifyRestoreMetadata", true)]
        [InlineData("ModifyVersion", true)]
        [InlineData("ModifyRuntimeGraph", true)]
        [InlineData("ModifyRestoreSettings", false)]
        public void PackageSpecCloneTest(string methodName, bool validateJson)
        {
            // Arrange
            var packageSpec = CreatePackageSpec();
            var clonedPackageSpec = packageSpec.Clone();

            //Preconditions
            Assert.Equal(packageSpec, clonedPackageSpec);

            JObject originalJObject = packageSpec.ToJObject();
            JObject clonedJObject = clonedPackageSpec.ToJObject();

            Assert.Equal(originalJObject.ToString(), clonedJObject.ToString());
            Assert.False(ReferenceEquals(packageSpec, clonedPackageSpec));

            // Act
            var methodInfo = typeof(PackageSpecModify).GetMethod(methodName);
            methodInfo.Invoke(null, new object[] { packageSpec });

            // Assert
            if (validateJson)
            {
                Assert.NotEqual(packageSpec, clonedPackageSpec);

                originalJObject = packageSpec.ToJObject();
                clonedJObject = clonedPackageSpec.ToJObject();

                Assert.NotEqual(originalJObject.ToString(), clonedJObject.ToString());
            }
            else
            {
                Assert.Equal(packageSpec, clonedPackageSpec);

            }

            Assert.False(object.ReferenceEquals(packageSpec, clonedPackageSpec));

        }

        public class PackageSpecModify
        {
            public static void ModifyOriginalTargetFrameworkInformationAdd(PackageSpec packageSpec)
            {
                packageSpec.TargetFrameworks.Add(CreateTargetFrameworkInformation("net40"));
            }

            public static void ModifyOriginalTargetFrameworkInformationEdit(PackageSpec packageSpec)
            {
                var newImports = packageSpec.TargetFrameworks[0].Imports.Add(NuGetFramework.Parse("net461"));
                packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Imports = newImports };
            }

            public static void ModifyRestoreMetadata(PackageSpec packageSpec)
            {
                var newImports = packageSpec.TargetFrameworks[0].Imports.Add(NuGetFramework.Parse("net461"));
                packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Imports = newImports };
            }

            public static void ModifyVersion(PackageSpec packageSpec)
            {
                packageSpec.Version = new NuGetVersion("2.0.0");
            }

            public static void ModifyRuntimeGraph(PackageSpec packageSpec)
            {
                packageSpec.RuntimeGraph.Supports["CompatibilityProfile"].RestoreContexts.Add(CreateFrameworkRuntimePair(rid: "win10-x64"));
            }

            public static void ModifyRestoreSettings(PackageSpec packageSpec)
            {
                packageSpec.RestoreSettings.HideWarningsAndErrors = false;
            }
        }

        private ProjectRestoreMetadata CreateProjectRestoreMetadata()
        {
            var projectRestoreMetadataFrameworkInfo = CreateProjectRestoreMetadataFrameworkInfo();

            var allWarningsAsErrors = true;
            var noWarn = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1500 };
            var warningsAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1501 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1005 };
            var warningProperties = new WarningProperties(allWarningsAsErrors: allWarningsAsErrors, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors);

            var originalProjectRestoreMetadata = new ProjectRestoreMetadata();
            originalProjectRestoreMetadata.ProjectStyle = ProjectStyle.PackageReference;
            originalProjectRestoreMetadata.ProjectPath = "ProjectPath";
            originalProjectRestoreMetadata.ProjectJsonPath = "ProjectJsonPath";
            originalProjectRestoreMetadata.OutputPath = "OutputPath";
            originalProjectRestoreMetadata.ProjectName = "ProjectName";
            originalProjectRestoreMetadata.ProjectUniqueName = "ProjectUniqueName";
            originalProjectRestoreMetadata.PackagesPath = "PackagesPath";
            originalProjectRestoreMetadata.CacheFilePath = "CacheFilePath";
            originalProjectRestoreMetadata.CrossTargeting = true;
            originalProjectRestoreMetadata.LegacyPackagesDirectory = true;
            originalProjectRestoreMetadata.ValidateRuntimeAssets = true;
            originalProjectRestoreMetadata.SkipContentFileWrite = true;
            originalProjectRestoreMetadata.TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>() { projectRestoreMetadataFrameworkInfo };
            originalProjectRestoreMetadata.Sources = new List<PackageSource>() { new PackageSource("http://api.nuget.org/v3/index.json") };
            originalProjectRestoreMetadata.FallbackFolders = new List<string>() { "fallback1" };
            originalProjectRestoreMetadata.ConfigFilePaths = new List<string>() { "config1" };
            originalProjectRestoreMetadata.OriginalTargetFrameworks = new List<string>() { "net45" };
            originalProjectRestoreMetadata.Files = new List<ProjectRestoreMetadataFile>() { new ProjectRestoreMetadataFile("packagePath", "absolutePath") };
            originalProjectRestoreMetadata.ProjectWideWarningProperties = warningProperties;
            originalProjectRestoreMetadata.SdkAnalysisLevel = new NuGetVersion("9.0.100");
            originalProjectRestoreMetadata.UsingMicrosoftNETSdk = false;

            return originalProjectRestoreMetadata;
        }

        private static ProjectRestoreMetadataFrameworkInfo CreateProjectRestoreMetadataFrameworkInfo(string frameworkName = "net461", string alias = "net461")
        {
            var projectReference = new ProjectRestoreReference();
            projectReference.ProjectPath = "Path";
            projectReference.ProjectUniqueName = "ProjectUniqueName";
            projectReference.IncludeAssets = LibraryIncludeFlags.All;
            projectReference.ExcludeAssets = LibraryIncludeFlags.Analyzers;
            projectReference.PrivateAssets = LibraryIncludeFlags.Build;
            var nugetFramework = NuGetFramework.Parse(frameworkName);
            var originalPRMFI = new ProjectRestoreMetadataFrameworkInfo(nugetFramework);
            originalPRMFI.TargetAlias = alias ?? Guid.NewGuid().ToString();
            originalPRMFI.ProjectReferences = new List<ProjectRestoreReference>() { projectReference };
            return originalPRMFI;
        }

        [Fact]
        public void ProjectRestoreMetadataCloneTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();
            // Act

            var happyClone = originalProjectRestoreMetadata.Clone();

            // Assert
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeSourcesTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.Sources.Clear();

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(1, happyClone.Sources.Count);
        }

        [Fact]
        public void ProjectRestoreMetadataEqualityAccountsForDuplicates()
        {
            // Arrange
            var left = CreateProjectRestoreMetadata();
            var right = CreateProjectRestoreMetadata();
            left.Sources = new List<PackageSource>() { new PackageSource("http://api.nuget.org/v3/index.json"), new PackageSource("C:\\source"), new PackageSource("http://api.nuget.org/v3/index.json") };
            right.Sources = new List<PackageSource>() { new PackageSource("C:\\source"), new PackageSource("http://api.nuget.org/v3/index.json"), new PackageSource("http://api.nuget.org/v3/index.json") };

            // Act & Assert
            Assert.Equal(left, right);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeCentralPackageVersionsEnabledTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.CentralPackageVersionsEnabled = !originalProjectRestoreMetadata.CentralPackageVersionsEnabled;

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(originalProjectRestoreMetadata.CentralPackageVersionsEnabled, !happyClone.CentralPackageVersionsEnabled);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeCentralPackageFloatingVersionsEnabledTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.CentralPackageFloatingVersionsEnabled = !originalProjectRestoreMetadata.CentralPackageFloatingVersionsEnabled;

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(originalProjectRestoreMetadata.CentralPackageFloatingVersionsEnabled, !happyClone.CentralPackageFloatingVersionsEnabled);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeCentralPackageVersionOverrideDisabledTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.CentralPackageVersionOverrideDisabled = !originalProjectRestoreMetadata.CentralPackageVersionOverrideDisabled;

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(originalProjectRestoreMetadata.CentralPackageVersionOverrideDisabled, !happyClone.CentralPackageVersionOverrideDisabled);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeCentralPackageTransitivePinningEnabledTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.CentralPackageTransitivePinningEnabled = !originalProjectRestoreMetadata.CentralPackageTransitivePinningEnabled;

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(originalProjectRestoreMetadata.CentralPackageTransitivePinningEnabled, !happyClone.CentralPackageTransitivePinningEnabled);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeFallbackFoldersTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.FallbackFolders.Clear();

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(1, happyClone.FallbackFolders.Count);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeConfigFilePathsTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.ConfigFilePaths.Clear();

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(1, happyClone.ConfigFilePaths.Count);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeOriginalTargetFrameworksTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.OriginalTargetFrameworks.Clear();

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(1, happyClone.OriginalTargetFrameworks.Count);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeFilesTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.Files.Clear();

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.Equal(1, happyClone.Files.Count);
        }

        [Fact]
        public void ProjectRestoreMetadataCloneChangeProjectWideWarningPropertiesTest()
        {
            // Arrange
            var originalProjectRestoreMetadata = CreateProjectRestoreMetadata();

            // Preconditions
            var happyClone = originalProjectRestoreMetadata.Clone();
            Assert.Equal(originalProjectRestoreMetadata, happyClone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadata, happyClone));

            // Act
            originalProjectRestoreMetadata.ProjectWideWarningProperties.AllWarningsAsErrors = false;

            // Assert
            Assert.NotEqual(originalProjectRestoreMetadata, happyClone);
            Assert.True(happyClone.ProjectWideWarningProperties.AllWarningsAsErrors);
        }

        [Fact]
        public void ProjectRestoreMetadataFileCloneTest()
        {
            // Arrange
            var originalProjectRestoreMetadataFile = new ProjectRestoreMetadataFile("packagePath", "absolutePath");

            // Act
            var clone = originalProjectRestoreMetadataFile.Clone();

            // Assert
            Assert.Equal(originalProjectRestoreMetadataFile, clone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreMetadataFile, clone));
        }

        [Fact]
        public void ProjectRestoreMetadataFrameworkInfoCloneTest()
        {
            //Set up
            var projectReference = new ProjectRestoreReference();
            projectReference.ProjectPath = "Path";
            projectReference.ProjectUniqueName = "ProjectUniqueName";
            projectReference.IncludeAssets = LibraryModel.LibraryIncludeFlags.All;
            projectReference.ExcludeAssets = LibraryModel.LibraryIncludeFlags.Analyzers;
            projectReference.PrivateAssets = LibraryModel.LibraryIncludeFlags.Build;

            var nugetFramework = NuGetFramework.Parse("net461");

            var originalPRMFI = new ProjectRestoreMetadataFrameworkInfo(nugetFramework);
            originalPRMFI.ProjectReferences = new List<ProjectRestoreReference>() { projectReference };

            // Act
            var clone = originalPRMFI.Clone();

            // Assert
            Assert.Equal(clone, originalPRMFI);
            Assert.False(object.ReferenceEquals(originalPRMFI, clone));

            // Act
            projectReference.ProjectPath = "NewPath";

            // Assert
            Assert.NotEqual(clone, originalPRMFI);
        }

        [Fact]
        public void ProjectRestoreReferenceCloneTest()
        {
            // Arrange
            var originalProjectRestoreReference = new ProjectRestoreReference();
            originalProjectRestoreReference.ProjectPath = "Path";
            originalProjectRestoreReference.ProjectUniqueName = "ProjectUniqueName";
            originalProjectRestoreReference.IncludeAssets = LibraryModel.LibraryIncludeFlags.All;
            originalProjectRestoreReference.ExcludeAssets = LibraryModel.LibraryIncludeFlags.Analyzers;
            originalProjectRestoreReference.PrivateAssets = LibraryModel.LibraryIncludeFlags.Build;

            // Act
            var clone = originalProjectRestoreReference.Clone();

            // Assert
            Assert.Equal(originalProjectRestoreReference, clone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreReference, clone));
        }

        private ProjectRestoreSettings CreateProjectRestoreSettings()
        {
            var prs = new ProjectRestoreSettings();
            prs.HideWarningsAndErrors = true;
            prs.SdkVersion = new NuGetVersion(10, 0, 100);
            return prs;
        }

        [Fact]
        public void ProjectRestoreSettingsCloneTest()
        {
            // Arrange
            var originalProjectRestoreSettings = CreateProjectRestoreSettings();

            // Act
            var clone = originalProjectRestoreSettings.Clone();

            // Assert
            Assert.Equal(originalProjectRestoreSettings, clone);
            Assert.False(object.ReferenceEquals(originalProjectRestoreSettings, clone));
        }

        internal static TargetFrameworkInformation CreateTargetFrameworkInformation(string tfm = "net461", string alias = null)
        {
            var framework = NuGetFramework.Parse(tfm);
            var dependency = new LibraryDependency(
                libraryRange: new LibraryRange("Dependency", LibraryDependencyTarget.Package),
                includeType: LibraryIncludeFlags.None,
                suppressParent: LibraryIncludeFlags.ContentFiles,
                noWarn: [NuGetLogCode.NU1000, NuGetLogCode.NU1001],
                autoReferenced: false,
                generatePathProperty: false,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);
            var imports = NuGetFramework.Parse("net45"); // This makes no sense in the context of fallback, just for testing :)

            var originalTargetFrameworkInformation = new TargetFrameworkInformation()
            {
                AssetTargetFallback = false,
                CentralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
                {
                    { "CVD", new CentralPackageVersion("CVD", VersionRange.Parse("1.0.0")) },
                },
                Dependencies = [dependency],
                DownloadDependencies = [new DownloadDependency("X", VersionRange.Parse("1.0.0"))],
                FrameworkName = framework,
                FrameworkReferences = [
                    new FrameworkDependency("frameworkRef", FrameworkDependencyFlags.All),
                    new FrameworkDependency("FrameworkReference", FrameworkDependencyFlags.None)
                ],
                Imports = [imports],
                RuntimeIdentifierGraphPath = @"path/to/dotnet/sdk/3.0.100/runtime.json",
                TargetAlias = alias ?? Guid.NewGuid().ToString()
            };

            return originalTargetFrameworkInformation;
        }

        [Fact]
        public void TargetFrameworkInformationCloneTest()
        {
            // Arrange
            var originalTargetFrameworkInformation = CreateTargetFrameworkInformation();

            // Act
            var clone = new TargetFrameworkInformation(originalTargetFrameworkInformation);

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, clone);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, clone));
            Assert.Equal(originalTargetFrameworkInformation.GetHashCode(), clone.GetHashCode());

            // Act
            originalTargetFrameworkInformation = new TargetFrameworkInformation(originalTargetFrameworkInformation) { Imports = [] };

            // Assert
            Assert.NotEqual(originalTargetFrameworkInformation, clone);
            Assert.Equal(1, clone.Imports.Length);

            //Act
            var cloneToTestDependencies = new TargetFrameworkInformation(originalTargetFrameworkInformation);

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestDependencies);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestDependencies));

            // Act
            originalTargetFrameworkInformation = new TargetFrameworkInformation(originalTargetFrameworkInformation) { Dependencies = [] };

            // Assert
            Assert.NotEqual(originalTargetFrameworkInformation, cloneToTestDependencies);
            Assert.Equal(1, cloneToTestDependencies.Dependencies.Length);

            //Act
            var cloneToTestDownloadDependencies = new TargetFrameworkInformation(originalTargetFrameworkInformation);

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestDownloadDependencies);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestDownloadDependencies));

            // Act
            originalTargetFrameworkInformation = new TargetFrameworkInformation(originalTargetFrameworkInformation) { DownloadDependencies = [] };

            //Assert
            Assert.NotEqual(originalTargetFrameworkInformation, cloneToTestDownloadDependencies);
            Assert.Equal(1, cloneToTestDownloadDependencies.DownloadDependencies.Length);

            //Setup
            var cloneToTestFrameworkReferenceEquality = new TargetFrameworkInformation(originalTargetFrameworkInformation)
            {
                FrameworkReferences = [
                    new FrameworkDependency("frameworkRef", FrameworkDependencyFlags.All),
                    new FrameworkDependency("frameworkReference", FrameworkDependencyFlags.None)
                ]
            };

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestFrameworkReferenceEquality);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestFrameworkReferenceEquality));

            //Act
            var cloneToTestFrameworkReferences = new TargetFrameworkInformation(originalTargetFrameworkInformation);

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestFrameworkReferences);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestFrameworkReferences));

            // Act
            originalTargetFrameworkInformation = new TargetFrameworkInformation(originalTargetFrameworkInformation) { FrameworkReferences = [] };

            //Assert
            Assert.NotEqual(originalTargetFrameworkInformation, cloneToTestFrameworkReferences);
            Assert.Equal(2, cloneToTestFrameworkReferences.FrameworkReferences.Count);

            var cloneToTestRuntimeIdentifierGraphPath = new TargetFrameworkInformation(originalTargetFrameworkInformation);

            // Act
            originalTargetFrameworkInformation = new TargetFrameworkInformation(originalTargetFrameworkInformation) { RuntimeIdentifierGraphPath = "new/path/to/runtime.json" };

            //Assert
            Assert.NotEqual(originalTargetFrameworkInformation, cloneToTestRuntimeIdentifierGraphPath);
        }

        [Fact]
        public void WarningPropertiesCloneTest()
        {
            // Arrange
            var allWarningsAsErrors = false;
            var noWarn = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1500 };
            var warningsAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1001, NuGetLogCode.NU1501 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { NuGetLogCode.NU1005 };
            var originalWarningProperties = new WarningProperties(allWarningsAsErrors: allWarningsAsErrors, warningsAsErrors: warningsAsErrors, noWarn: noWarn, warningsNotAsErrors: warningsNotAsErrors);

            //Act
            var clone = originalWarningProperties.Clone();

            // Assert
            Assert.Equal(originalWarningProperties, clone);
            Assert.False(object.ReferenceEquals(originalWarningProperties, clone));
            Assert.False(object.ReferenceEquals(noWarn, clone.NoWarn));

            // Act again
            noWarn.Clear();

            //Assert again
            Assert.NotEqual(originalWarningProperties, clone);
            Assert.Equal(2, clone.NoWarn.Count);
        }

        private static FrameworkRuntimePair CreateFrameworkRuntimePair(string tfm = "net461", string rid = "win-x64")
        {
            return new FrameworkRuntimePair(NuGetFramework.Parse(tfm), rid);
        }

        private static CompatibilityProfile CreateCompatibilityProfile(string name, string tfm = "net461")
        {
            return new CompatibilityProfile(name, new FrameworkRuntimePair[] { CreateFrameworkRuntimePair(tfm, "win-x64"), CreateFrameworkRuntimePair(tfm, "win-x86") });
        }

        [Fact]
        public void CompatibilityProfileCloneTest()
        {
            // Setup
            var compat = CreateCompatibilityProfile("bla");
            //Act
            var clone = compat.Clone();
            //Assert
            Assert.Equal(compat, clone);
            Assert.False(object.ReferenceEquals(compat, clone));
            Assert.Same(compat.RestoreContexts[0], clone.RestoreContexts[0]); // FRP is immutable so the instance is reused

            // Act - Change the list of compat
            compat.RestoreContexts.Add(CreateFrameworkRuntimePair(rid: "win10-x64"));

            // Assert
            Assert.NotEqual(compat, clone);
            Assert.Equal(3, compat.RestoreContexts.Count);
            Assert.Equal(2, clone.RestoreContexts.Count);
        }

        [Fact]
        public void PackageSpec_Equals_WithTargetFrameworkInformationOutOfOrder_ReturnsTrue()
        {
            var leftSide = new PackageSpec(new List<TargetFrameworkInformation>()
            {
                CreateTargetFrameworkInformation("net461", "net461"),
                CreateTargetFrameworkInformation("netcoreapp2.0", "netcoreapp2.0"),
            })
            {
                RestoreMetadata = CreateProjectRestoreMetadata(),
                RestoreSettings = CreateProjectRestoreSettings()
            };

            var rightSide = new PackageSpec(new List<TargetFrameworkInformation>()
            {
                CreateTargetFrameworkInformation("netcoreapp2.0", "netcoreapp2.0"),
                CreateTargetFrameworkInformation("net461", "net461"),
            })
            {
                RestoreMetadata = CreateProjectRestoreMetadata(),
                RestoreSettings = CreateProjectRestoreSettings()
            };

            leftSide.Should().Be(rightSide);
        }


        [Fact]
        public void PackageSpec_Equals_WithProjectRestoreMetadataFrameworkInfoOutOfOrder_ReturnsTrue()
        {
            var leftSide = new PackageSpec(new List<TargetFrameworkInformation>())
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>()
                    {
                        CreateProjectRestoreMetadataFrameworkInfo("net461", "net461"),
                        CreateProjectRestoreMetadataFrameworkInfo("netcoreapp2.0", "netcoreapp2.0"),
                    }
                },
                RestoreSettings = CreateProjectRestoreSettings()
            };

            var rightSide = new PackageSpec(new List<TargetFrameworkInformation>())
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>()
                    {
                        CreateProjectRestoreMetadataFrameworkInfo("netcoreapp2.0", "netcoreapp2.0"),
                        CreateProjectRestoreMetadataFrameworkInfo("net461", "net461")
                    }
                },
                RestoreSettings = CreateProjectRestoreSettings()
            };

            leftSide.Should().Be(rightSide);
        }
    }
}
