// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackageSpecTests

    {
        private BuildOptions CreateBuildOptions()
        {
            var outputName = "OutputName";
            var originalBuildOptions = new BuildOptions();
            originalBuildOptions.OutputName = outputName;
            return originalBuildOptions;
        }

        [Fact]
        public void BuildOptionsCloneTest()
        {
            //Set up
            var originalBuildOptions = CreateBuildOptions();

            // Act
            var clonedBuildOptions = originalBuildOptions.Clone();

            //Assert
            Assert.Equal(originalBuildOptions.OutputName, clonedBuildOptions.OutputName);
            Assert.False(object.ReferenceEquals(originalBuildOptions, clonedBuildOptions));
        }

        [Fact]
        public void IncludeExcludeFilesCloneTest()
        {
            //Set up
            var exclude = new List<string>() { "Exlclude0" };
            var include = new List<string>() { "Include0" };
            var includeFiles = new List<string>() { "IncludeFiles0" };
            var excludeFiles = new List<string>() { "ExlcludeFiles0" };

            var files = new IncludeExcludeFiles();
            files.Exclude = exclude;
            files.Include = include;
            files.IncludeFiles = includeFiles;
            files.ExcludeFiles = excludeFiles;

            // Act
            var clone = files.Clone();
            //Assert

            Assert.Equal(files.Exclude, clone.Exclude);
            Assert.Equal(files.Include, clone.Include);
            Assert.Equal(files.IncludeFiles, clone.IncludeFiles);
            Assert.Equal(files.ExcludeFiles, clone.ExcludeFiles);

            // Act again
            exclude.Add("Extra Exclude");

            //Assert
            Assert.Equal(2, files.Exclude.Count);
            Assert.NotEqual(files.Exclude, clone.Exclude);
        }

        [Fact]
        public void PackOptionsCloneTest()
        {
            //Set up
            var originalPackOptions = new PackOptions();
            var originalPackageName = "PackageA";
            var packageTypes = new List<NuGet.Packaging.Core.PackageType>() { new Packaging.Core.PackageType(originalPackageName, new System.Version("1.0.0")) };

            var exclude = new List<string>() { "Exlclude0" };
            var include = new List<string>() { "Include0" };
            var includeFiles = new List<string>() { "IncludeFiles0" };
            var excludeFiles = new List<string>() { "ExlcludeFiles0" };

            var files = new IncludeExcludeFiles();
            files.Exclude = exclude;
            files.Include = include;
            files.IncludeFiles = includeFiles;
            files.ExcludeFiles = excludeFiles;

            originalPackOptions.PackageType = packageTypes;
            originalPackOptions.IncludeExcludeFiles = files;

            // Act
            var clone = originalPackOptions.Clone();

            // Assert
            Assert.Equal(originalPackOptions, clone);

            // Act again
            packageTypes.Clear();

            // Assert
            Assert.NotEqual(originalPackOptions, clone);
            Assert.Equal(originalPackageName, clone.PackageType[0].Name);

            // Arrange again
            originalPackOptions.Mappings.Add("randomString", files);

            // Act again
            var cloneWithMappings = originalPackOptions.Clone();

            // Assert
            Assert.Equal(originalPackOptions, cloneWithMappings);

            // Act again
            originalPackOptions.Mappings.Clear();

            // Assert
            Assert.NotEqual(originalPackOptions, cloneWithMappings);
            Assert.Equal(1, cloneWithMappings.Mappings.Count);
        }

        internal static LibraryDependency CreateLibraryDependency()
        {
            var dependency = new LibraryDependency(
                libraryRange: new LibraryRange(Guid.NewGuid().ToString(), LibraryDependencyTarget.Package),
                includeType: LibraryIncludeFlags.None,
                suppressParent: LibraryIncludeFlags.ContentFiles,
                noWarn: new List<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1001, NuGetLogCode.NU1002 },
                autoReferenced: false,
                generatePathProperty: false,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuffff",
                versionOverride: null);

            return dependency;
        }

        [Obsolete]
        private IncludeExcludeFiles CreateIncludeExcludeFiles()
        {
            var files = new IncludeExcludeFiles();
            files.Exclude = new List<string>() { "Exlclude0" };
            files.Include = new List<string>() { "Include0" };
            files.IncludeFiles = new List<string>() { "IncludeFiles0" };
            files.ExcludeFiles = new List<string>() { "ExlcludeFiles0" };
            return files;
        }

        [Obsolete]
        private PackOptions CreatePackOptions()
        {
            var originalPackOptions = new PackOptions();
            originalPackOptions.PackageType = new List<PackageType>() { new PackageType("PackageA", new Version("1.0.0")) };
            originalPackOptions.IncludeExcludeFiles = CreateIncludeExcludeFiles();
            return originalPackOptions;
        }

        private PackageSpec CreatePackageSpec()
        {
            var originalTargetFrameworkInformation = CreateTargetFrameworkInformation();
            var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { originalTargetFrameworkInformation });
            packageSpec.RestoreMetadata = CreateProjectRestoreMetadata();
            packageSpec.FilePath = "FilePath";
            packageSpec.Name = "Name";
            packageSpec.Title = "Title";
            packageSpec.Version = new NuGetVersion("1.0.0");
#pragma warning disable CS0612 // Type or member is obsolete
            packageSpec.HasVersionSnapshot = true;
            packageSpec.Description = "Description";
            packageSpec.Summary = "Summary";
            packageSpec.ReleaseNotes = "ReleaseNotes";
            packageSpec.Authors = new string[] { "Author1" };
            packageSpec.Owners = new string[] { "Owner1" };
            packageSpec.ProjectUrl = "ProjectUrl";
            packageSpec.IconUrl = "IconUrl";
            packageSpec.LicenseUrl = "LicenseUrl";
            packageSpec.Copyright = "Copyright";
            packageSpec.Language = "Language";
            packageSpec.RequireLicenseAcceptance = true;
            packageSpec.Tags = new string[] { "Tags" };
            packageSpec.BuildOptions = CreateBuildOptions();
            packageSpec.ContentFiles = new List<string>() { "contentFile1", "contentFile2" };

            packageSpec.Scripts.Add(Guid.NewGuid().ToString(), new List<string>() { Guid.NewGuid().ToString() });
            packageSpec.Scripts.Add(Guid.NewGuid().ToString(), new List<string>() { Guid.NewGuid().ToString() });

            packageSpec.PackInclude.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            packageSpec.PackOptions = CreatePackOptions();
#pragma warning restore CS0612 // Type or member is obsolete

            packageSpec.Dependencies = new List<LibraryDependency>() { CreateLibraryDependency(), CreateLibraryDependency() };
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
        [InlineData("ModifyAuthors", true)]
        [InlineData("ModifyOriginalTargetFrameworkInformationAdd", true)]
        [InlineData("ModifyOriginalTargetFrameworkInformationEdit", true)]
        [InlineData("ModifyRestoreMetadata", true)]
        [InlineData("ModifyVersion", true)]
        [InlineData("ModifyOwners", true)]
        [InlineData("ModifyTags", true)]
        [InlineData("ModifyBuildOptions", false)]
        [InlineData("ModifyContentFiles", true)]
        [InlineData("ModifyDependencies", true)]
        [InlineData("ModifyScriptsAdd", true)]
        [InlineData("ModifyScriptsEdit", true)]
        [InlineData("ModifyPackInclude", true)]
        [InlineData("ModifyPackOptions", true)]
        [InlineData("ModifyRuntimeGraph", true)]
        //[InlineData("ModifyRestoreSettings", true)] = Not really included in the equals and hash code comparisons
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
            Assert.NotEqual(packageSpec, clonedPackageSpec);

            if (validateJson)
            {
                originalJObject = packageSpec.ToJObject();
                clonedJObject = clonedPackageSpec.ToJObject();

                Assert.NotEqual(originalJObject.ToString(), clonedJObject.ToString());
            }

            Assert.False(object.ReferenceEquals(packageSpec, clonedPackageSpec));
        }

        public class PackageSpecModify
        {
            [Obsolete]
            public static void ModifyAuthors(PackageSpec packageSpec)
            {
                packageSpec.Authors[0] = "NewAuthor";
            }

            public static void ModifyOriginalTargetFrameworkInformationAdd(PackageSpec packageSpec)
            {
                packageSpec.TargetFrameworks.Add(CreateTargetFrameworkInformation("net40"));
            }

            public static void ModifyOriginalTargetFrameworkInformationEdit(PackageSpec packageSpec)
            {
                packageSpec.TargetFrameworks[0].Imports.Add(NuGetFramework.Parse("net461"));
            }

            public static void ModifyRestoreMetadata(PackageSpec packageSpec)
            {
                packageSpec.TargetFrameworks[0].Imports.Add(NuGetFramework.Parse("net461"));
            }

            public static void ModifyVersion(PackageSpec packageSpec)
            {
                packageSpec.Version = new Versioning.NuGetVersion("2.0.0");
            }

            [Obsolete]
            public static void ModifyOwners(PackageSpec packageSpec)
            {
                packageSpec.Owners[0] = "BetterOwner";
            }

            [Obsolete]
            public static void ModifyTags(PackageSpec packageSpec)
            {
                packageSpec.Tags[0] = "better tag!";
            }

            [Obsolete]
            public static void ModifyBuildOptions(PackageSpec packageSpec)
            {
                packageSpec.BuildOptions.OutputName = Guid.NewGuid().ToString();
            }

            [Obsolete]
            public static void ModifyContentFiles(PackageSpec packageSpec)
            {
                packageSpec.ContentFiles.Add("New fnacy content file");
            }

            public static void ModifyDependencies(PackageSpec packageSpec)
            {
                packageSpec.Dependencies.Add(CreateLibraryDependency());
            }

            [Obsolete]
            public static void ModifyScriptsAdd(PackageSpec packageSpec)
            {
                packageSpec.Scripts.Add(Guid.NewGuid().ToString(), new List<string>() { Guid.NewGuid().ToString() });
            }

            [Obsolete]
            public static void ModifyScriptsEdit(PackageSpec packageSpec)
            {
                var enumerator = packageSpec.Scripts.Keys.GetEnumerator();
                enumerator.MoveNext();
                var key = enumerator.Current;
                ((List<string>)packageSpec.Scripts[key]).Add(Guid.NewGuid().ToString());
            }

            [Obsolete]
            public static void ModifyPackInclude(PackageSpec packageSpec)
            {
                packageSpec.PackInclude.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }

            [Obsolete]
            public static void ModifyPackOptions(PackageSpec packageSpec)
            {
                ((List<PackageType>)packageSpec.PackOptions.PackageType).Add(PackageType.DotnetCliTool);
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
                noWarn: new List<NuGetLogCode>() { NuGetLogCode.NU1000, NuGetLogCode.NU1001 },
                autoReferenced: false,
                generatePathProperty: false,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "stuff",
                versionOverride: null);
            var imports = NuGetFramework.Parse("net45"); // This makes no sense in the context of fallback, just for testing :)

            var originalTargetFrameworkInformation = new TargetFrameworkInformation();
            originalTargetFrameworkInformation.TargetAlias = alias ?? Guid.NewGuid().ToString();
            originalTargetFrameworkInformation.FrameworkName = framework;
            originalTargetFrameworkInformation.Dependencies = new List<LibraryDependency>() { dependency };
            originalTargetFrameworkInformation.AssetTargetFallback = false;
            originalTargetFrameworkInformation.Imports = new List<NuGetFramework>() { imports };
            originalTargetFrameworkInformation.DownloadDependencies.Add(new DownloadDependency("X", VersionRange.Parse("1.0.0")));
            originalTargetFrameworkInformation.FrameworkReferences.Add(new FrameworkDependency("frameworkRef", FrameworkDependencyFlags.All));
            originalTargetFrameworkInformation.FrameworkReferences.Add(new FrameworkDependency("FrameworkReference", FrameworkDependencyFlags.None));
            originalTargetFrameworkInformation.RuntimeIdentifierGraphPath = @"path/to/dotnet/sdk/3.0.100/runtime.json";
            originalTargetFrameworkInformation.CentralPackageVersions.Add("CVD", new CentralPackageVersion("CVD", VersionRange.Parse("1.0.0")));
            return originalTargetFrameworkInformation;
        }

        [Fact]
        public void TargetFrameworkInformationCloneTest()
        {
            // Arrange
            var originalTargetFrameworkInformation = CreateTargetFrameworkInformation();

            // Act
            var clone = originalTargetFrameworkInformation.Clone();

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, clone);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, clone));
            Assert.Equal(originalTargetFrameworkInformation.GetHashCode(), clone.GetHashCode());

            // Act
            originalTargetFrameworkInformation.Imports.Clear();

            // Assert
            Assert.NotEqual(originalTargetFrameworkInformation, clone);
            Assert.Equal(1, clone.Imports.Count);

            //Act
            var cloneToTestDependencies = originalTargetFrameworkInformation.Clone();

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestDependencies);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestDependencies));

            // Act
            originalTargetFrameworkInformation.Dependencies.Clear();

            // Assert
            Assert.NotEqual(originalTargetFrameworkInformation, cloneToTestDependencies);
            Assert.Equal(1, cloneToTestDependencies.Dependencies.Count);

            //Act
            var cloneToTestDownloadDependencies = originalTargetFrameworkInformation.Clone();

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestDownloadDependencies);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestDownloadDependencies));

            // Act
            originalTargetFrameworkInformation.DownloadDependencies.Clear();

            //Assert
            Assert.NotEqual(originalTargetFrameworkInformation, cloneToTestDownloadDependencies);
            Assert.Equal(1, cloneToTestDownloadDependencies.DownloadDependencies.Count);

            //Setup
            var cloneToTestFrameworkReferenceEquality = originalTargetFrameworkInformation.Clone();
            cloneToTestFrameworkReferenceEquality.FrameworkReferences.Clear();
            cloneToTestFrameworkReferenceEquality.FrameworkReferences.Add(new FrameworkDependency("frameworkRef", FrameworkDependencyFlags.All));
            cloneToTestFrameworkReferenceEquality.FrameworkReferences.Add(new FrameworkDependency("frameworkReference", FrameworkDependencyFlags.None));

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestFrameworkReferenceEquality);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestFrameworkReferenceEquality));

            //Act
            var cloneToTestFrameworkReferences = originalTargetFrameworkInformation.Clone();

            // Assert
            Assert.Equal(originalTargetFrameworkInformation, cloneToTestFrameworkReferences);
            Assert.False(ReferenceEquals(originalTargetFrameworkInformation, cloneToTestFrameworkReferences));

            // Act
            originalTargetFrameworkInformation.FrameworkReferences.Clear();

            //Assert
            Assert.NotEqual(originalTargetFrameworkInformation, cloneToTestFrameworkReferences);
            Assert.Equal(2, cloneToTestFrameworkReferences.FrameworkReferences.Count);

            var cloneToTestRuntimeIdentifierGraphPath = originalTargetFrameworkInformation.Clone();

            // Act
            originalTargetFrameworkInformation.RuntimeIdentifierGraphPath = "new/path/to/runtime.json";

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

        [Fact]
        [Obsolete]
        public void FrameworkRuntimePairCloneTest()
        {
            //Setup
            var frp = CreateFrameworkRuntimePair();
            //Act
            var clone = frp.Clone();
            //Assert
            Assert.Same(frp, clone);
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

        [Obsolete]
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Clone_WhenIsDefaultVersionVaries_ReturnsEqualClone(bool expectedResult)
        {
            var packageSpec = new PackageSpec();

            packageSpec.IsDefaultVersion = expectedResult;

            Assert.Equal(expectedResult, packageSpec.IsDefaultVersion);

            PackageSpec clone = packageSpec.Clone();

            Assert.Equal(expectedResult, packageSpec.IsDefaultVersion);
            Assert.Equal(expectedResult, clone.IsDefaultVersion);
            Assert.True(packageSpec.Equals(clone));
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

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("b;a", "a;b", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;b", false)]
        public void PackageSpec_Equals_WithDependencies(string left, string right, bool expected)
        {
            var leftSide = new PackageSpec(new List<TargetFrameworkInformation>())
            {
                Dependencies = left.Split(';').Select(e =>
                    new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange(e, VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                    }
                    )
                .ToList()
            };

            var rightSide = new PackageSpec(new List<TargetFrameworkInformation>())
            {
                Dependencies = right.Split(';').Select(e =>
                    new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange(e, VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                    }
                    )
                .ToList()
            };
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);

            }
        }
    }
}
