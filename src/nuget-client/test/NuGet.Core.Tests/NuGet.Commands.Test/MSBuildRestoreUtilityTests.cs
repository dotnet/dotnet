// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    public class MSBuildRestoreUtilityTests
    {
        [PlatformFact(Platform.Windows)]
        public void MSBuildRestoreUtility_GivenDifferentProjectPathCasingsVerifyResult()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "b");

                var project1Path = Path.Combine(project1Root, "a-abcdefghijklmno.csproj");
                var project2Path = Path.Combine(project2Root, "b-abcdefghijklmno.csproj");

                var project1UniqueName = Path.Combine(project1Root, "a-abcdefghijklmno.csproj");
                var project2UniqueName = Path.Combine(project2Root, "b-abcdefghijklmno.csproj");

                var project1UniqueNameCasings = new[]
                {
                    Path.Combine(project1Root, "a-Abcdefghijklmno.csproj"),
                    Path.Combine(project1Root, "a-ABcdefghijklmno.csproj"),
                    Path.Combine(project1Root, "a-ABCdefghijklmno.csproj"),
                    Path.Combine(project1Root, "a-ABCDefghijklmno.csproj"),
                    Path.Combine(project1Root, "a-ABCDEfghijklmno.csproj"),
                };

                var project2UniqueNameCasings = new[]
                {
                    Path.Combine(project1Root, "b-Abcdefghijklmno.csproj"),
                    Path.Combine(project1Root, "b-ABcdefghijklmno.csproj"),
                    Path.Combine(project1Root, "b-ABCdefghijklmno.csproj"),
                    Path.Combine(project1Root, "b-ABCDefghijklmno.csproj"),
                    Path.Combine(project1Root, "b-ABCDEfghijklmno.csproj"),
                };

                var outputPath1 = Path.Combine(project1Root, "obj");
                var outputPath2 = Path.Combine(project2Root, "obj");

                // Exact unique name matches
                var itemsWithSameCasings = new List<IDictionary<string, string>>();

                // Unique names differ on casings
                var itemsWithDifferentCasings = new List<IDictionary<string, string>>();

                var projectAItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                };

                itemsWithSameCasings.Add(projectAItem);
                itemsWithDifferentCasings.Add(WithUniqueName(projectAItem, project1UniqueNameCasings[0]));

                var projectATfmInformationNetItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=v4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };

                itemsWithSameCasings.Add(projectATfmInformationNetItem);
                itemsWithDifferentCasings.Add(WithUniqueName(projectATfmInformationNetItem, project1UniqueNameCasings[0]));

                var projectATfmInformationNSItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };

                itemsWithSameCasings.Add(projectATfmInformationNSItem);
                itemsWithDifferentCasings.Add(WithUniqueName(projectATfmInformationNSItem, project1UniqueNameCasings[0]));

                var projectBItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "b" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath2 },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectPath", project2Path },
                    { "CrossTargeting", "true" },
                };

                itemsWithSameCasings.Add(projectBItem);
                itemsWithDifferentCasings.Add(WithUniqueName(projectBItem, project2UniqueNameCasings[0]));

                var projectBTfmInformationNetItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "net45" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.5" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=v4.5" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };

                itemsWithSameCasings.Add(projectBTfmInformationNetItem);
                itemsWithDifferentCasings.Add(WithUniqueName(projectBTfmInformationNetItem, project2UniqueNameCasings[0]));

                var projectBTfmInformationNSItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "netstandard1.0" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.0" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };

                itemsWithSameCasings.Add(projectBTfmInformationNSItem);
                itemsWithDifferentCasings.Add(WithUniqueName(projectBTfmInformationNSItem, project2UniqueNameCasings[0]));

                // A -> B
                var projectReference = new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectReferenceUniqueName", project2UniqueName },
                    { "ProjectPath", project2Path },
                    { "TargetFrameworks", "netstandard1.6" },
                    { "CrossTargeting", "true" },
                };

                itemsWithSameCasings.Add(projectReference);

                var projectReferenceDifferentCasings = WithUniqueName(projectReference, project1UniqueNameCasings[1]);
                projectReferenceDifferentCasings["ProjectReferenceUniqueName"] = project2UniqueNameCasings[1];
                projectReferenceDifferentCasings["ProjectPath"] = project2UniqueNameCasings[2];

                itemsWithDifferentCasings.Add(projectReferenceDifferentCasings);

                // Package references
                // A net46 -> X
                var packageXReference = new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0-beta.*" },
                    { "TargetFrameworks", "net46" },
                    { "CrossTargeting", "true" },
                };

                itemsWithSameCasings.Add(packageXReference);
                itemsWithDifferentCasings.Add(WithUniqueName(packageXReference, project1UniqueNameCasings[2]));

                // A netstandard1.6 -> Z
                var packageZReference = new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "Id", "z" },
                    { "VersionRange", "2.0.0" },
                    { "TargetFrameworks", "netstandard1.6" },
                    { "CrossTargeting", "true" },
                };

                itemsWithSameCasings.Add(packageZReference);
                itemsWithDifferentCasings.Add(WithUniqueName(packageZReference, project1UniqueNameCasings[3]));

                // B ALL -> Y
                var packageYReference = new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "Id", "y" },
                    { "VersionRange", "[1.0.0]" },
                    { "TargetFrameworks", "netstandard1.0;net45" },
                    { "CrossTargeting", "true" },
                };

                itemsWithSameCasings.Add(packageYReference);
                itemsWithDifferentCasings.Add(WithUniqueName(packageYReference, project2UniqueNameCasings[3]));

                var wrappedItemsWithSameCasings = itemsWithSameCasings.Select(CreateItems).ToList();
                var wrappedItemsWithDifferentCasings = itemsWithDifferentCasings.Select(CreateItems).ToList();

                // Act
                var dgSpecWithSameCasings = MSBuildRestoreUtility.GetDependencySpec(wrappedItemsWithSameCasings);
                var project1SpecWithSameCasings = dgSpecWithSameCasings.Projects.Single(e => e.Name == "a");
                var project2SpecWithSameCasings = dgSpecWithSameCasings.Projects.Single(e => e.Name == "b");

                var dgSpecWithDifferentCasings = MSBuildRestoreUtility.GetDependencySpec(wrappedItemsWithDifferentCasings);
                var project1SpecWithDifferentCasings = dgSpecWithDifferentCasings.Projects.Single(e => e.Name == "a");
                var project2SpecWithDifferentCasings = dgSpecWithDifferentCasings.Projects.Single(e => e.Name == "b");

                // Assert
                // Verify package dependencies and framework references are the same
                project1SpecWithSameCasings.TargetFrameworks.Should().BeEquivalentTo(project1SpecWithDifferentCasings.TargetFrameworks);
                project2SpecWithSameCasings.TargetFrameworks.Should().BeEquivalentTo(project2SpecWithDifferentCasings.TargetFrameworks);

                // Verify project references are the same
                var projectReferencesSame = project1SpecWithSameCasings.RestoreMetadata.TargetFrameworks[0].ProjectReferences.Select(e => e.ProjectPath.ToLowerInvariant());
                var projectReferencesDiff = project1SpecWithDifferentCasings.RestoreMetadata.TargetFrameworks[0].ProjectReferences.Select(e => e.ProjectPath.ToLowerInvariant());

                projectReferencesSame.Should().BeEquivalentTo(projectReferencesDiff);
            }
        }

        [PlatformFact(Platform.Linux)]
        public void MSBuildRestoreUtility_GivenDifferentProjectPathCasingsVerifyPackageReferencesAreCorrect()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "A");

                // Same path, different case on Linux
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var project2Path = Path.Combine(project2Root, "A.csproj");

                var project1UniqueName = project1Path;
                var project2UniqueName = project2Path;

                var outputPath1 = Path.Combine(project1Root, "obj");
                var outputPath2 = Path.Combine(project2Root, "obj");

                // Exact unique name matches
                var items = new List<IDictionary<string, string>>();

                var projectAItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a1" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectAItem);

                var projectATfmInformationNetItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=v4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };
                items.Add(projectATfmInformationNetItem);

                var projectATfmInformationNSItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };
                items.Add(projectATfmInformationNSItem);

                var projectA2Item = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a2" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath2 },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectPath", project2Path },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectA2Item);

                var project2TfmInformationNetItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=v4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };
                items.Add(project2TfmInformationNetItem);

                var project2TfmInformationNSItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };
                items.Add(project2TfmInformationNSItem);

                // Package references
                // A net46 -> X
                var packageXReference = new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0-beta.*" },
                    { "TargetFrameworks", "net46" },
                    { "CrossTargeting", "true" },
                };
                items.Add(packageXReference);

                // A net46 -> Z
                var packageZReference = new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "Id", "z" },
                    { "VersionRange", "2.0.0" },
                    { "TargetFrameworks", "net46" },
                    { "CrossTargeting", "true" },
                };
                items.Add(packageZReference);

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == "a1");
                var project2Spec = dgSpec.Projects.Single(e => e.Name == "a2");

                // Assert
                project1Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Select(e => e.Name).Single().Should().Be("x");
                project2Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Select(e => e.Name).Single().Should().Be("z");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void MSBuildRestoreUtility_NormalizeProjectReferencesVerifyResult()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "b");

                // Same path, different case on Linux
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var project2Path = Path.Combine(project2Root, "b.csproj");
                var project2PathAlt = Path.Combine(project2Root, "B.csproj");

                var project1UniqueName = project1Path;
                var project2UniqueName = project2Path;
                var project2UniqueNameAlt = project2PathAlt;

                var outputPath1 = Path.Combine(project1Root, "obj");
                var outputPath2 = Path.Combine(project2Root, "obj");

                // Exact unique name matches
                var items = new List<IDictionary<string, string>>();

                var projectAItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectAItem);

                var project1TfmInformationItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };
                items.Add(project1TfmInformationItem);

                var projectBItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "b" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath2 },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectPath", project2Path },
                    { "TargetFrameworks", "net46" },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectBItem);

                var project2TfmInformationItem = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };
                items.Add(project2TfmInformationItem);

                // A -> B
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectReferenceUniqueName", project2UniqueNameAlt },
                    { "ProjectPath", project2PathAlt },
                    { "TargetFrameworks", "net46" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var projectReferences = dgSpec.GetProjectSpec(project1UniqueName).RestoreMetadata.TargetFrameworks[0].ProjectReferences;

                // Assert
                projectReferences[0].ProjectUniqueName.Should().Be(project2UniqueName, "this should be normalized");
                projectReferences[0].ProjectPath.Should().Be(project2Path, "this should be normalized");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void MSBuildRestoreUtility_RemoveMissingProjectsVerifyCaseDoesNotMatter()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "b");

                // Same path, different case on Linux
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var project2Path = Path.Combine(project2Root, "b.csproj");
                var project2PathAlt = Path.Combine(project2Root, "B.csproj");

                var project1UniqueName = project1Path;
                var project2UniqueName = project2Path;
                var project2UniqueNameAlt = project2PathAlt;

                var outputPath1 = Path.Combine(project1Root, "obj");
                var outputPath2 = Path.Combine(project2Root, "obj");

                // Exact unique name matches
                var items = new List<IDictionary<string, string>>();

                var projectAItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectAItem);

                var projectBItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "b" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath2 },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectPath", project2Path },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectBItem);

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // A -> B
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectReferenceUniqueName", project2UniqueNameAlt },
                    { "ProjectPath", project2PathAlt },
                    { "TargetFrameworks", "net46" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var projectReferences = dgSpec.GetProjectSpec(project1UniqueName).RestoreMetadata.TargetFrameworks[0].ProjectReferences;

                // Assert
                projectReferences.Count.Should().Be(1);
            }
        }

        [PlatformFact(Platform.Linux)]
        public void MSBuildRestoreUtility_RemoveMissingProjectsVerifyCaseDoesMatter()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "b");

                // Same path, different case on Linux
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var project2Path = Path.Combine(project2Root, "b.csproj");
                var project2PathAlt = Path.Combine(project2Root, "B.csproj");

                var project1UniqueName = project1Path;
                var project2UniqueName = project2Path;
                var project2UniqueNameAlt = project2PathAlt;

                var outputPath1 = Path.Combine(project1Root, "obj");
                var outputPath2 = Path.Combine(project2Root, "obj");

                // Exact unique name matches
                var items = new List<IDictionary<string, string>>();

                var projectAItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectAItem);

                var projectBItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "b" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath2 },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectPath", project2Path },
                    { "CrossTargeting", "true" },
                };
                items.Add(projectBItem);

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // A -> B
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectReferenceUniqueName", project2UniqueNameAlt },
                    { "ProjectPath", project2PathAlt },
                    { "TargetFrameworks", "net46" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var projectReferences = dgSpec.GetProjectSpec(project1UniqueName).RestoreMetadata.TargetFrameworks[0].ProjectReferences;

                // Assert
                projectReferences.Should().BeEmpty();
            }
        }

        [Theory]
        [InlineData("a", "", "a")]
        [InlineData("a|b", "", "a|b")]
        [InlineData("a|b", "a|b", "")]
        [InlineData("a|b", "a|b|c", "")]
        [InlineData("", "a", "")]
        [InlineData("a", "A", "a")]
        public void MSBuildRestoreUtility_AggregateSources(string values, string exclude, string expected)
        {
            var inputValues = values.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var excludeValues = exclude.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var expectedValues = expected.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            MSBuildRestoreUtility.AggregateSources(inputValues, excludeValues)
                .Should().BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_VerifyInvalidProjectReferencesAreIgnored()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project2Root = Path.Combine(workingDir, "b");
                var project2Path = Path.Combine(project2Root, "b.csproj");
                var project3Root = Path.Combine(workingDir, "c");
                var project3Path = Path.Combine(project3Root, "c.csproj");
                var outputPath3 = Path.Combine(project3Root, "obj");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project2UniqueName = "C82C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "2.0.0-rc.2+a.b.c" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                    { "RestoreLegacyPackagesDirectory", "true" }
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "2.0.0-rc.2+a.b.c" },
                    { "ProjectName", "c" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath3 },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectPath", project3Path },
                    { "TargetFrameworks", "net46;netstandard16" },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                    { "RestoreLegacyPackagesDirectory", "true" }
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // A -> B
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectReferenceUniqueName", "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project2Path },
                    { "TargetFrameworks", "net46;netstandard1.6" },
                    { "CrossTargeting", "true" },
                });

                // A -> C
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectReferenceUniqueName", "C82C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project3Path },
                    { "TargetFrameworks", "net46;netstandard1.6" },
                    { "CrossTargeting", "true" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var projectReferences = dgSpec.Projects.Select(e => e.RestoreMetadata)
                    .SelectMany(e => e.TargetFrameworks)
                    .SelectMany(e => e.ProjectReferences)
                    .Select(e => e.ProjectPath)
                    .Distinct()
                    .ToList();

                // Assert
                Assert.Equal(1, projectReferences.Count);
                Assert.Equal(project3Path, projectReferences[0]);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpecVersion_UAP()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "b");

                var project1JsonPath = Path.Combine(project1Root, "project.json");
                var project2JsonPath = Path.Combine(project2Root, "project.json");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var project2Path = Path.Combine(project2Root, "b.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectJsonPath", project1JsonPath },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "ProjectJson" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project1Path },
                });

                var project1Json = @"
                {
                    ""version"": ""2.0.0-beta.1+build"",
                    ""description"": """",
                    ""authors"": [ ""author"" ],
                    ""tags"": [ """" ],
                    ""projectUrl"": """",
                    ""licenseUrl"": """",
                    ""frameworks"": {
                        ""net45"": {
                        }
                    }
                }";

                Directory.CreateDirectory(project1Root);
                File.WriteAllText(project1JsonPath, project1Json);

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == "a");

                // Assert
                Assert.Equal("2.0.0-beta.1+build", project1Spec.Version.ToFullString());
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_Tool()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "DotnetCliTool" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "netcoreapp1.0" },
                    { "CrossTargeting", "true" },
                });

                // Package reference
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0-beta.*" },
                    { "TargetFrameworks", "netcoreapp1.0" },
                    { "CrossTargeting", "true" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == "a");

                // Assert
                // Dependency counts
                Assert.Equal(1, project1Spec.GetTargetFramework(NuGetFramework.Parse("netcoreapp1.0")).Dependencies.Count);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyIncludeFlags()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package references
                // A net46 -> X
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                    { "TargetFrameworks", "net46" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });

                // A net46 -> Y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "Id", "y" },
                    { "VersionRange", "1.0.0" },
                    { "TargetFrameworks", "net46" },
                    { "ExcludeAssets", "build;compile" },
                });

                // A net46 -> Z
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "Id", "z" },
                    { "VersionRange", "1.0.0" },
                    { "TargetFrameworks", "net46" },
                    { "PrivateAssets", "all" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();
                var x = project1Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Single(e => e.Name == "x");
                var y = project1Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Single(e => e.Name == "y");
                var z = project1Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Single(e => e.Name == "z");

                // Assert
                // X
                Assert.Equal((LibraryIncludeFlags.Build | LibraryIncludeFlags.Compile), x.IncludeType);
                Assert.Equal((LibraryIncludeFlagUtils.DefaultSuppressParent), x.SuppressParent);

                // Y
                Assert.Equal(LibraryIncludeFlags.All & ~(LibraryIncludeFlags.Build | LibraryIncludeFlags.Compile), y.IncludeType);
                Assert.Equal((LibraryIncludeFlagUtils.DefaultSuppressParent), y.SuppressParent);

                // Z
                Assert.Equal(LibraryIncludeFlags.All, z.IncludeType);
                Assert.Equal(LibraryIncludeFlags.All, z.SuppressParent);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyBasicMetadata()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var configFilePath = Path.Combine(project1Root, "nuget.config");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "2.0.0-rc.2+a.b.c" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "ConfigFilePaths", configFilePath },
                    { "CrossTargeting", "true" },
                    { "RestoreLegacyPackagesDirectory", "true" }
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                // Assert
                Assert.Equal(project1Path, project1Spec.FilePath);
                Assert.Equal("a", project1Spec.Name);
                Assert.Equal("2.0.0-rc.2+a.b.c", project1Spec.Version.ToFullString());
                Assert.Equal(ProjectStyle.PackageReference, project1Spec.RestoreMetadata.ProjectStyle);
                Assert.Equal(project1UniqueName, project1Spec.RestoreMetadata.ProjectUniqueName);
                Assert.Equal(project1Path, project1Spec.RestoreMetadata.ProjectPath);
                Assert.Equal(0, project1Spec.RestoreMetadata.TargetFrameworks.SelectMany(e => e.ProjectReferences).Count());
                Assert.Null(project1Spec.RestoreMetadata.ProjectJsonPath);
                Assert.Equal("net46|netstandard1.6", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName())));
                Assert.Equal("net46|netstandard16", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.TargetAlias)));
                Assert.Equal("net46|netstandard16", string.Join("|", project1Spec.RestoreMetadata.OriginalTargetFrameworks));
                Assert.Equal(outputPath1, project1Spec.RestoreMetadata.OutputPath);
                Assert.Equal("https://nuget.org/a/index.json|https://nuget.org/b/index.json", string.Join("|", project1Spec.RestoreMetadata.Sources.Select(s => s.Source)));
                Assert.Equal(fallbackFolder, string.Join("|", project1Spec.RestoreMetadata.FallbackFolders));
                Assert.Equal(packagesFolder, string.Join("|", project1Spec.RestoreMetadata.PackagesPath));
                Assert.Equal(configFilePath, string.Join("|", project1Spec.RestoreMetadata.ConfigFilePaths));
                Assert.Equal(0, project1Spec.RuntimeGraph.Runtimes.Count);
                Assert.Equal(0, project1Spec.RuntimeGraph.Supports.Count);
                Assert.True(project1Spec.RestoreMetadata.CrossTargeting);
                Assert.True(project1Spec.RestoreMetadata.LegacyPackagesDirectory);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyDefaultVersion()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var configFilePath = Path.Combine(project1Root, "nuget.config");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "ConfigFilePaths", configFilePath },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                    { "RestoreLegacyPackagesDirectory", "true" }
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                // Assert
                Assert.Equal("1.0.0", project1Spec.Version.ToFullString());
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyInvalidVersionThrowsOnParse()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var configFilePath = Path.Combine(project1Root, "nuget.config");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "notaversionstring" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "net46;netstandard16" },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "ConfigFilePaths", configFilePath },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                    { "RestoreLegacyPackagesDirectory", "true" }
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act && Assert
                Assert.Throws<ArgumentException>(() => MSBuildRestoreUtility.GetDependencySpec(wrappedItems));
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreNonCrossTargeting()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var configFilePath = Path.Combine(project1Root, "nuget.config");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName",  project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "netstandard16" },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "ConfigFilePaths", configFilePath },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                // Assert
                Assert.Equal(project1Path, project1Spec.FilePath);
                Assert.Equal("a", project1Spec.Name);
                Assert.Equal(ProjectStyle.PackageReference, project1Spec.RestoreMetadata.ProjectStyle);
                Assert.Equal("netstandard1.6", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName())));
                Assert.Equal("netstandard16", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.TargetAlias)));
                Assert.Equal("netstandard16", string.Join("|", project1Spec.RestoreMetadata.OriginalTargetFrameworks));
                Assert.False(project1Spec.RestoreMetadata.CrossTargeting);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreNonLegacyPackagesDirectory()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "netstandard16" },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                // Assert
                Assert.Equal(project1Path, project1Spec.FilePath);
                Assert.Equal("a", project1Spec.Name);
                Assert.Equal(ProjectStyle.PackageReference, project1Spec.RestoreMetadata.ProjectStyle);
                Assert.Equal("netstandard1.6", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName())));
                Assert.Equal("netstandard16", string.Join("|", project1Spec.RestoreMetadata.OriginalTargetFrameworks));
                Assert.Equal("netstandard16", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.TargetAlias)));
                Assert.False(project1Spec.RestoreMetadata.LegacyPackagesDirectory);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyImports()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "portable-net45+win8;dnxcore50;;" },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                var nsTFM = project1Spec.GetTargetFramework(NuGetFramework.Parse("netstandard16"));
                var netTFM = project1Spec.GetTargetFramework(NuGetFramework.Parse("net46"));

                // Assert
                Assert.Equal(2, nsTFM.Imports.Count);
                Assert.Equal(0, netTFM.Imports.Count);

                Assert.Equal(NuGetFramework.Parse("portable-net45+win8"), nsTFM.Imports[0]);
                Assert.Equal(NuGetFramework.Parse("dnxcore50"), nsTFM.Imports[1]);

                // Verify fallback framework
                var fallbackFramework = (FallbackFramework)project1Spec.TargetFrameworks
                    .Single(e => e.FrameworkName.GetShortFolderName() == "netstandard1.6")
                    .FrameworkName;

                // net46 does not have imports
                var fallbackFrameworkNet45 = project1Spec.TargetFrameworks
                    .Single(e => e.FrameworkName.GetShortFolderName() == "net46")
                    .FrameworkName
                    as FallbackFramework;

                Assert.Null(fallbackFrameworkNet45);
                Assert.Equal(2, fallbackFramework.Fallback.Count);
                Assert.Equal(NuGetFramework.Parse("portable-net45+win8"), fallbackFramework.Fallback[0]);
                Assert.Equal(NuGetFramework.Parse("dnxcore50"), fallbackFramework.Fallback[1]);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyImportsEmpty()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                var nsTFM = project1Spec.GetTargetFramework(NuGetFramework.Parse("netstandard16"));
                var netTFM = project1Spec.GetTargetFramework(NuGetFramework.Parse("net46"));

                // Assert
                Assert.Equal(0, nsTFM.Imports.Count);
                Assert.Equal(0, netTFM.Imports.Count);

                // Verify no fallback frameworks
                var fallbackFrameworks = project1Spec.TargetFrameworks.Select(e => e.FrameworkName as FallbackFramework);
                Assert.True(fallbackFrameworks.All(e => e == null));
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyWhitespaceRemoved()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "  a\n  " },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json; https://nuget.org/b/index.json\n" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46\n" },
                    { "TargetFrameworkIdentifier", ".NETFramework\n" },
                    { "TargetFrameworkVersion", "v4.6\n" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6\n" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "   portable-net45+win8  ;   dnxcore50\n   ; ;  " },
                    { "TargetFramework", " netstandard16\n  " },
                    { "TargetFrameworkIdentifier", ".NETStandard\n" },
                    { "TargetFrameworkVersion", "v1.6\n" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6\n" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                var nsTFM = project1Spec.GetTargetFramework(NuGetFramework.Parse("netstandard16"));
                var netTFM = project1Spec.GetTargetFramework(NuGetFramework.Parse("net46"));

                // Assert
                Assert.Equal("a", project1Spec.RestoreMetadata.ProjectName);
                Assert.Equal(2, nsTFM.Imports.Count);
                Assert.Equal(0, netTFM.Imports.Count);

                Assert.Equal(NuGetFramework.Parse("portable-net45+win8"), nsTFM.Imports[0]);
                Assert.Equal(NuGetFramework.Parse("dnxcore50"), nsTFM.Imports[1]);

                // Verify fallback framework
                var fallbackFramework = (FallbackFramework)project1Spec.TargetFrameworks
                    .Single(e => e.FrameworkName.GetShortFolderName() == "netstandard1.6")
                    .FrameworkName;

                // net46 does not have imports
                var fallbackFrameworkNet45 = project1Spec.TargetFrameworks
                    .Single(e => e.FrameworkName.GetShortFolderName() == "net46")
                    .FrameworkName
                    as FallbackFramework;

                Assert.Null(fallbackFrameworkNet45);
                Assert.Equal(2, fallbackFramework.Fallback.Count);
                Assert.Equal(NuGetFramework.Parse("portable-net45+win8"), fallbackFramework.Fallback[0]);
                Assert.Equal(NuGetFramework.Parse("dnxcore50"), fallbackFramework.Fallback[1]);

                // Verify original frameworks are trimmed
                Assert.Equal("net46", project1Spec.RestoreMetadata.OriginalTargetFrameworks[0]);
                Assert.Equal("netstandard16", project1Spec.RestoreMetadata.OriginalTargetFrameworks[1]);
                Assert.Equal("net46|netstandard16", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.TargetAlias)));
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyRuntimes()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "RuntimeIdentifiers", "win7-x86;linux-x64" },
                    { "RuntimeSupports", "net46.app;win8.app" },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                // Assert
                Assert.Equal(2, project1Spec.RuntimeGraph.Runtimes.Count);
                Assert.Equal(2, project1Spec.RuntimeGraph.Supports.Count);
                Assert.Equal("win7-x86", project1Spec.RuntimeGraph.Runtimes["win7-x86"].RuntimeIdentifier);
                Assert.Equal("linux-x64", project1Spec.RuntimeGraph.Runtimes["linux-x64"].RuntimeIdentifier);
                Assert.Equal("net46.app", project1Spec.RuntimeGraph.Supports["net46.app"].Name);
                Assert.Equal("win8.app", project1Spec.RuntimeGraph.Supports["win8.app"].Name);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyRuntimes_Duplicates()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "RuntimeIdentifiers", "win7-x86;linux-x64;win7-x86;linux-x64" },
                    { "RuntimeSupports", "net46.app;win8.app;net46.app;win8.app" },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", "netstandard16" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                // Assert
                Assert.Equal(2, project1Spec.RuntimeGraph.Runtimes.Count);
                Assert.Equal(2, project1Spec.RuntimeGraph.Supports.Count);
                Assert.Equal("win7-x86", project1Spec.RuntimeGraph.Runtimes["win7-x86"].RuntimeIdentifier);
                Assert.Equal("linux-x64", project1Spec.RuntimeGraph.Runtimes["linux-x64"].RuntimeIdentifier);
                Assert.Equal("net46.app", project1Spec.RuntimeGraph.Supports["net46.app"].Name);
                Assert.Equal("win8.app", project1Spec.RuntimeGraph.Supports["win8.app"].Name);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCore_Conditionals()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "b");

                var project1Path = Path.Combine(project1Root, "a.csproj");
                var project2Path = Path.Combine(project2Root, "b.csproj");

                var outputPath1 = Path.Combine(project1Root, "obj");
                var outputPath2 = Path.Combine(project2Root, "obj");

                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project2UniqueName = "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "b" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath2 },
                    { "ProjectUniqueName", project2UniqueName },
                    { "ProjectPath", project2Path },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", "netstandard1.0" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.0" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project2UniqueName },
                    { "TargetFramework", "net45" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.5" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.5" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // A -> B
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectReferenceUniqueName", "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project2Path },
                    { "TargetFrameworks", "netstandard1.6" },
                    { "CrossTargeting", "true" },
                });

                // Package references
                // A net46 -> X
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0-beta.*" },
                    { "TargetFrameworks", "net46" },
                    { "CrossTargeting", "true" },
                });

                // A netstandard1.6 -> Z
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "Id", "z" },
                    { "VersionRange", "2.0.0" },
                    { "TargetFrameworks", "netstandard1.6" },
                    { "CrossTargeting", "true" },
                });

                // B ALL -> Y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "Id", "y" },
                    { "VersionRange", "[1.0.0]" },
                    { "TargetFrameworks", "netstandard1.0;net45" },
                    { "CrossTargeting", "true" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == "a");
                var project2Spec = dgSpec.Projects.Single(e => e.Name == "b");

                var msbuildDependency = project1Spec.RestoreMetadata.TargetFrameworks
                    .Single(e => e.FrameworkName.Equals(NuGetFramework.Parse("netstandard1.6")))
                    .ProjectReferences
                    .Single();

                // Assert
                // Verify p2p reference
                Assert.Equal("AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A", msbuildDependency.ProjectUniqueName);
                Assert.Equal(project2Path, msbuildDependency.ProjectPath);
                Assert.Equal(LibraryIncludeFlags.All, msbuildDependency.IncludeAssets);
                Assert.Equal(LibraryIncludeFlags.None, msbuildDependency.ExcludeAssets);
                Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, msbuildDependency.PrivateAssets);
                Assert.Equal("netstandard1.6", string.Join("|", project1Spec.RestoreMetadata.TargetFrameworks
                    .Where(e => e.ProjectReferences.Count > 0)
                    .Select(e => e.FrameworkName.GetShortFolderName())
                    .OrderBy(s => s, StringComparer.Ordinal)));

                // Dependency counts
                Assert.Equal(0, project1Spec.Dependencies.Count);
                Assert.Equal(0, project2Spec.Dependencies.Count);

                Assert.Equal(1, project1Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Count);
                Assert.Equal(1, project1Spec.GetTargetFramework(NuGetFramework.Parse("netstandard1.6")).Dependencies.Count);

                Assert.Equal(1, project2Spec.GetTargetFramework(NuGetFramework.Parse("net45")).Dependencies.Count);
                Assert.Equal(1, project2Spec.GetTargetFramework(NuGetFramework.Parse("netstandard1.0")).Dependencies.Count);

                // Verify dependencies
                var xDep = project1Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Single(e => e.Name == "x");
                var zDep = project1Spec.GetTargetFramework(NuGetFramework.Parse("netstandard1.6")).Dependencies.Single(e => e.Name == "z");

                var yDep1 = project2Spec.GetTargetFramework(NuGetFramework.Parse("netstandard1.0")).Dependencies.Single();
                var yDep2 = project2Spec.GetTargetFramework(NuGetFramework.Parse("net45")).Dependencies.Single();

                Assert.Equal("x", xDep.Name);
                Assert.Equal(VersionRange.Parse("1.0.0-beta.*"), xDep.LibraryRange.VersionRange);
                Assert.Equal(LibraryDependencyTarget.Package, xDep.LibraryRange.TypeConstraint);
                Assert.Equal(LibraryIncludeFlags.All, xDep.IncludeType);
                Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, xDep.SuppressParent);

                Assert.Equal("z", zDep.Name);
                Assert.Equal(VersionRange.Parse("2.0.0"), zDep.LibraryRange.VersionRange);
                Assert.Equal(LibraryDependencyTarget.Package, zDep.LibraryRange.TypeConstraint);
                Assert.Equal(LibraryIncludeFlags.All, zDep.IncludeType);
                Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, zDep.SuppressParent);

                Assert.Equal("y", yDep1.Name);
                Assert.Equal(VersionRange.Parse("[1.0.0]"), yDep1.LibraryRange.VersionRange);
                Assert.Equal(LibraryDependencyTarget.Package, yDep1.LibraryRange.TypeConstraint);
                Assert.Equal(LibraryIncludeFlags.All, yDep1.IncludeType);
                Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, yDep1.SuppressParent);

                Assert.Equal(yDep1, yDep2);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCore_VerifyDuplicateItemsAreIgnored()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectRoot = Path.Combine(workingDir, "a");
                var projectPath = Path.Combine(projectRoot, "a.csproj");
                var outputPath = Path.Combine(projectRoot, "obj");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                var specItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", projectPath },
                    { "CrossTargeting", "true" },
                };

                // Add each item twice
                items.Add(specItem);
                items.Add(specItem);

                // A -> B
                var projectRef = new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectReferenceUniqueName", "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", "otherProjectPath.csproj" },
                    { "TargetFrameworks", "netstandard1.6" },
                    { "CrossTargeting", "true" },
                };

                items.Add(projectRef);
                items.Add(projectRef);

                // Package references
                // A netstandard1.6 -> Z
                var packageRef1 = new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "Id", "z" },
                    { "VersionRange", "2.0.0" },
                    { "TargetFrameworks", "netstandard1.6" },
                    { "CrossTargeting", "true" },
                };

                items.Add(packageRef1);
                items.Add(packageRef1);

                // B ALL -> Y
                var packageRef2 = new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "Id", "y" },
                    { "VersionRange", "[1.0.0]" },
                    { "TargetFrameworks", "netstandard1.6;net46" },
                    { "CrossTargeting", "true" },
                };

                items.Add(packageRef2);
                items.Add(packageRef2);

                // TFM info
                var tfmInfoNS = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "portable-net45+win8;dnxcore50;;" },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };

                items.Add(tfmInfoNS);
                items.Add(tfmInfoNS);

                var tfmInfoNet = new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                };

                items.Add(tfmInfoNet);
                items.Add(tfmInfoNet);

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var projectSpec = dgSpec.Projects.Single(e => e.Name == "a");

                // Assert
                Assert.Equal(0, projectSpec.Dependencies.Count);
                Assert.Equal(1, dgSpec.Projects.Count);
                Assert.Equal("y", string.Join("|", projectSpec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.Select(e => e.Name)));
                Assert.Equal("z|y", string.Join("|", projectSpec.GetTargetFramework(NuGetFramework.Parse("netstandard1.6")).Dependencies.Select(e => e.Name)));
                Assert.Equal(2, projectSpec.RestoreMetadata.TargetFrameworks.Count);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCore_IgnoreBadItemWithMismatchedIds()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectRoot = Path.Combine(workingDir, "a");
                var projectPath = Path.Combine(projectRoot, "a.csproj");
                var outputPath = Path.Combine(projectRoot, "obj");

                var items = new List<IDictionary<string, string>>();
                var project1UniqueName = "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var specItem = new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", projectPath },
                    { "TargetFrameworks", "net46;netstandard1.6" },
                    { "CrossTargeting", "true" },
                };

                items.Add(specItem);

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", "netstandard1.6" },
                    { "TargetFrameworkIdentifier", ".NETStandard" },
                    { "TargetFrameworkVersion", "v1.6" },
                    { "TargetFrameworkMoniker", ".NETStandard,Version=v1.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // A -> B
                var projectRef = new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    // This ID does not match the project!
                    { "ProjectUniqueName", "BB2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectReferenceUniqueName", "CC2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", "otherProjectPath.csproj" },
                    { "TargetFrameworks", "netstandard1.6" },
                    { "CrossTargeting", "true" },
                };

                items.Add(projectRef);

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var projectSpec = dgSpec.Projects.Single(e => e.Name == "a");

                // Assert
                Assert.NotNull(projectSpec);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_UAP_P2P()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project2Root = Path.Combine(workingDir, "b");

                var project1JsonPath = Path.Combine(project1Root, "project.json");
                var project2JsonPath = Path.Combine(project2Root, "project.json");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var project2Path = Path.Combine(project2Root, "b.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectJsonPath", project1JsonPath },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "ProjectJson" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project1Path },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectJsonPath", project2JsonPath },
                    { "ProjectName", "b" },
                    { "ProjectStyle", "ProjectJson" },
                    { "ProjectUniqueName", "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project2Path },
                });

                // A -> B
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectReference" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectReferenceUniqueName", "AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project2Path },
                });

                var project1Json = @"
                {
                    ""version"": ""1.0.0"",
                    ""description"": """",
                    ""authors"": [ ""author"" ],
                    ""tags"": [ """" ],
                    ""projectUrl"": """",
                    ""licenseUrl"": """",
                    ""frameworks"": {
                        ""net45"": {
                        }
                    }
                }";

                var project2Json = @"
                {
                    ""version"": ""1.0.0"",
                    ""description"": """",
                    ""authors"": [ ""author"" ],
                    ""tags"": [ """" ],
                    ""projectUrl"": """",
                    ""licenseUrl"": """",
                    ""frameworks"": {
                        ""net45"": {
                        }
                    }
                }";

                Directory.CreateDirectory(project1Root);
                Directory.CreateDirectory(project2Root);

                File.WriteAllText(project1JsonPath, project1Json);
                File.WriteAllText(project2JsonPath, project2Json);

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == "a");
                var project2Spec = dgSpec.Projects.Single(e => e.Name == "b");

                var allDependencies1 = project1Spec.Dependencies.Concat(project1Spec.TargetFrameworks.Single().Dependencies).ToList();
                var allDependencies2 = project2Spec.Dependencies.Concat(project2Spec.TargetFrameworks.Single().Dependencies).ToList();
                var msbuildDependency = project1Spec.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Single();

                // Assert
                Assert.Equal("AA2C20DE-DFF9-4BD0-B90A-BD3201AA351A", msbuildDependency.ProjectUniqueName);
                Assert.Equal(project2Path, msbuildDependency.ProjectPath);
                Assert.Equal(LibraryIncludeFlags.All, msbuildDependency.IncludeAssets);
                Assert.Equal(LibraryIncludeFlags.None, msbuildDependency.ExcludeAssets);
                Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, msbuildDependency.PrivateAssets);
                Assert.Equal("net45", string.Join("|", project1Spec.RestoreMetadata.TargetFrameworks
                    .Select(e => e.FrameworkName.GetShortFolderName())
                    .OrderBy(s => s, StringComparer.Ordinal)));

                Assert.Equal(0, allDependencies2.Count);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_UAP_VerifyMetadata()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectJsonPath = Path.Combine(workingDir, "project.json");
                var projectPath = Path.Combine(workingDir, "a.csproj");

                var items = new List<IDictionary<string, string>>();
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectJsonPath", projectJsonPath },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "ProjectJson" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", projectPath },
                });

                var projectJson = @"
                {
                    ""version"": ""1.0.0"",
                    ""description"": """",
                    ""authors"": [ ""author"" ],
                    ""tags"": [ """" ],
                    ""projectUrl"": """",
                    ""licenseUrl"": """",
                    ""frameworks"": {
                        ""net45"": {
                        }
                    }
                }";

                File.WriteAllText(projectJsonPath, projectJson);

                // Act
                var spec = MSBuildRestoreUtility.GetPackageSpec(items.Select(CreateItems));

                // Assert
                Assert.Equal(projectJsonPath, spec.FilePath);
                Assert.Equal("a", spec.Name);
                Assert.Equal(ProjectStyle.ProjectJson, spec.RestoreMetadata.ProjectStyle);
                Assert.Equal("482C20DE-DFF9-4BD0-B90A-BD3201AA351A", spec.RestoreMetadata.ProjectUniqueName);
                Assert.Equal(projectPath, spec.RestoreMetadata.ProjectPath);
                Assert.Equal(0, spec.RestoreMetadata.TargetFrameworks.SelectMany(e => e.ProjectReferences).Count());
                Assert.Equal(projectJsonPath, spec.RestoreMetadata.ProjectJsonPath);
                Assert.Equal(NuGetFramework.Parse("net45"), spec.TargetFrameworks.Single().FrameworkName);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_UAP_IgnoresUnexpectedProperties()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectJsonPath = Path.Combine(workingDir, "project.json");
                var projectPath = Path.Combine(workingDir, "a.csproj");

                var items = new List<IDictionary<string, string>>();
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectJsonPath", projectJsonPath },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "ProjectJson" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", projectPath },
                    { "CrossTargeting", "true" },
                    { "RestoreLegacyPackagesDirectory", "true" },
                });

                var projectJson = @"
                {
                    ""version"": ""1.0.0"",
                    ""description"": """",
                    ""authors"": [ ""author"" ],
                    ""tags"": [ """" ],
                    ""projectUrl"": """",
                    ""licenseUrl"": """",
                    ""frameworks"": {
                        ""net45"": {
                        }
                    }
                }";

                File.WriteAllText(projectJsonPath, projectJson);

                // Act
                var spec = MSBuildRestoreUtility.GetPackageSpec(items.Select(CreateItems));

                // Assert
                Assert.False(spec.RestoreMetadata.CrossTargeting);
                Assert.False(spec.RestoreMetadata.LegacyPackagesDirectory);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_PackagesConfigProject()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectPath = Path.Combine(workingDir, "a.csproj");
                var packagesConfigPath = Path.Combine(workingDir, "packages.config");

                var items = new List<IDictionary<string, string>>();
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", projectPath },
                    { "ProjectStyle", "PackagesConfig" },
                    { "TargetFramework", "net462" },
                    { "ProjectName", "a" },
                    { "PackagesConfigPath", packagesConfigPath },
                    { "RestorePackagesWithLockFile", "true" },
                    { "NuGetLockFilePath", "custom.lock.json" },
                    { "RestoreLockedMode", "true" }
                });

                // Act
                var spec = MSBuildRestoreUtility.GetPackageSpec(items.Select(CreateItems));

                // Assert
                Assert.Equal(projectPath, spec.FilePath);
                Assert.Equal("a", spec.Name);
                Assert.IsType<PackagesConfigProjectRestoreMetadata>(spec.RestoreMetadata);
                var restoreMetadata = (PackagesConfigProjectRestoreMetadata)spec.RestoreMetadata;
                Assert.Equal(ProjectStyle.PackagesConfig, restoreMetadata.ProjectStyle);
                Assert.Equal("482C20DE-DFF9-4BD0-B90A-BD3201AA351A", restoreMetadata.ProjectUniqueName);
                Assert.Equal(projectPath, restoreMetadata.ProjectPath);
                Assert.Equal(packagesConfigPath, restoreMetadata.PackagesConfigPath);
                Assert.Equal("true", restoreMetadata.RestoreLockProperties.RestorePackagesWithLockFile);
                Assert.Equal("custom.lock.json", restoreMetadata.RestoreLockProperties.NuGetLockFilePath);
                Assert.True(restoreMetadata.RestoreLockProperties.RestoreLockedMode);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NonNuGetProject()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectPath = Path.Combine(workingDir, "a.csproj");

                var items = new List<IDictionary<string, string>>();
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", projectPath },
                    { "TargetFrameworks", "net462" },
                    { "ProjectName", "a" },
                    { "CrossTargeting", "true" },
                });

                // Act
                var spec = MSBuildRestoreUtility.GetPackageSpec(items.Select(CreateItems));

                // Assert
                Assert.Equal(projectPath, spec.FilePath);
                Assert.Equal("a", spec.Name);
                Assert.Equal(ProjectStyle.Unknown, spec.RestoreMetadata.ProjectStyle);
                Assert.Equal("482C20DE-DFF9-4BD0-B90A-BD3201AA351A", spec.RestoreMetadata.ProjectUniqueName);
                Assert.Equal(projectPath, spec.RestoreMetadata.ProjectPath);
                Assert.Equal(NuGetFramework.Parse("net462"), spec.TargetFrameworks.Single().FrameworkName);
                Assert.Equal(0, spec.RestoreMetadata.TargetFrameworks.SelectMany(e => e.ProjectReferences).Count());
                Assert.Null(spec.RestoreMetadata.ProjectJsonPath);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_LegacyPackageReference_DependenciesArePerFramework()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                // Assert
                Assert.Equal(project1Path, project1Spec.FilePath);
                Assert.Equal("a", project1Spec.Name);
                Assert.Equal(ProjectStyle.PackageReference, project1Spec.RestoreMetadata.ProjectStyle);
                Assert.Equal("net46", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName())));
                Assert.Equal("net46", string.Join("|", project1Spec.RestoreMetadata.OriginalTargetFrameworks));
                Assert.Equal("net46", string.Join("|", project1Spec.TargetFrameworks.Select(e => e.TargetAlias)));
                Assert.Equal("x", project1Spec.GetTargetFramework(NuGetFramework.Parse("net46")).Dependencies.SingleOrDefault().Name);
                Assert.Empty(project1Spec.Dependencies);
            }
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData(null, "")]
        [InlineData(";;;;;;", "")]
        [InlineData("\n", "")]
        [InlineData(" ;\n;\t;;  \n ", "")]
        [InlineData("a;b;c", "a|b|c")]
        [InlineData(" a ; b ; c ", "a|b|c")]
        [InlineData("a;c \n ", "a|c")]
        public void MSBuildRestoreUtility_Split(string input, string expected)
        {
            // Arrange && Act
            var parts = MSBuildStringUtility.Split(input);
            var output = string.Join("|", parts);

            // Assert
            Assert.Equal(expected, output);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData(" ", null)]
        [InlineData(null, null)]
        [InlineData("\n", null)]
        [InlineData(" a ; b ; c ", "a ; b ; c")]
        [InlineData(" a;c\n ", "a;c")]
        public void MSBuildRestoreUtility_GetProperty_Trim(string input, string expected)
        {
            // Arrange
            var item = new MSBuildItem("a", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "key", input }
            });

            // Act
            var trimmed = item.GetProperty("key");
            var raw = item.GetProperty("key", trim: false);

            // Assert
            Assert.Equal(expected, trimmed);

            // Verify the value was not changed when it was stored
            Assert.Equal(input, raw);
        }

        [Theory]
        [InlineData("a", false)]
        [InlineData("", false)]
        [InlineData("cLear", false)]
        [InlineData("cLear;clear", false)]
        [InlineData("cLear;a", true)]
        [InlineData("a;CLEAR", true)]
        [InlineData("a;CLEAR;CLEAR", true)]
        public void MSBuildRestoreUtility_HasInvalidClear(string input, bool expected)
        {
            Assert.Equal(expected, MSBuildRestoreUtility.HasInvalidClear(MSBuildStringUtility.Split(input)));
        }

        [Theory]
        [InlineData("a", false)]
        [InlineData("", false)]
        [InlineData("c;lear", false)]
        [InlineData("a;b", false)]
        [InlineData("cLear", true)]
        [InlineData("cLear;clear", true)]
        [InlineData("cLear;a", true)]
        [InlineData("a;CLEAR", true)]
        [InlineData("a;CLEAR;CLEAR", true)]
        public void MSBuildRestoreUtility_ContainsClearKeyword(string input, bool expected)
        {
            Assert.Equal(expected, MSBuildRestoreUtility.ContainsClearKeyword(MSBuildStringUtility.Split(input)));
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyProjectWideWarningProperties()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var configFilePath = Path.Combine(project1Root, "nuget.config");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();


                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "2.0.0-rc.2+a.b.c" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "ConfigFilePaths", configFilePath },
                    { "TreatWarningsAsErrors", "true" },
                    { "WarningsAsErrors", "NU1001;NU1002" },
                    { "NoWarn", "NU1100;NU1101" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();
                var props = project1Spec.RestoreMetadata.ProjectWideWarningProperties;

                // Assert
                props.AllWarningsAsErrors.Should().BeTrue();
                props.NoWarn.Should().BeEquivalentTo(new[] { NuGetLogCode.NU1100, NuGetLogCode.NU1101 });
                props.WarningsAsErrors.Should().BeEquivalentTo(new[] { NuGetLogCode.NU1001, NuGetLogCode.NU1002 });
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyEmptyProjectWideWarningProperties()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var configFilePath = Path.Combine(project1Root, "nuget.config");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();


                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "2.0.0-rc.2+a.b.c" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "ConfigFilePaths", configFilePath },
                    { "TreatWarningsAsErrors", "" },
                    { "WarningsAsErrors", "" },
                    { "NoWarn", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();
                var props = project1Spec.RestoreMetadata.ProjectWideWarningProperties;

                // Assert
                props.AllWarningsAsErrors.Should().BeFalse();
                props.NoWarn.Should().BeEmpty();
                props.WarningsAsErrors.Should().BeEmpty();
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyPackageWarningProperties()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var uniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(CreateProject(project1Root, uniqueName));

                // Package references
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", uniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                    { "CrossTargeting", "true" },
                    { "NoWarn", "NU1001;NU1002" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", uniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();
                var packageDependency = project1Spec.TargetFrameworks[0].Dependencies[0];

                // Assert
                packageDependency.NoWarn.Should().BeEquivalentTo(new[] { NuGetLogCode.NU1001, NuGetLogCode.NU1002 });
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyEmptyPackageWarningProperties()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var uniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(CreateProject(project1Root, uniqueName));

                // Package references
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", uniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", uniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();
                var packageDependency = project1Spec.TargetFrameworks[0].Dependencies[0];

                // Assert
                packageDependency.NoWarn.Should().BeEmpty();
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_NetCoreVerifyAutoReferencedTrue()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var uniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";

                var items = new List<IDictionary<string, string>>();

                items.Add(CreateProject(project1Root, uniqueName));

                // Package references
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", uniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                    { "IsImplicitlyDefined", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", uniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();
                var packageDependency = project1Spec.TargetFrameworks[0].Dependencies[0];

                // Assert
                packageDependency.AutoReferenced.Should().BeTrue();
            }
        }

        [Theory]
        [InlineData("NU1107")]
        [InlineData(";NU1107")]
        [InlineData(";NU1107;")]
        [InlineData("$(AnotherProperty);NU1107;")]
        [InlineData("NU1107;1607")]
        [InlineData("NU1107;random;values;are;here")]
        [InlineData("NU1107;CSC1607")]
        [InlineData("NU1107;MSB1607")]
        [InlineData("NU1107;1607;600")]
        [InlineData("NU1107;1607;0;-1;abc123")]
        [InlineData(",NU1107")]
        [InlineData(",NU1107,")]
        [InlineData("$(AnotherProperty),NU1107,")]
        [InlineData("NU1107,1607")]
        [InlineData("NU1107,random,values,are,here")]
        [InlineData("NU1107,CSC1607")]
        [InlineData("NU1107,MSB1607")]
        [InlineData("NU1107,1607,600")]
        [InlineData("NU1107,1607,0,-1,abc123")]
        [InlineData(", NU1107   ,")]
        [InlineData(" NU1107   ")]
        [InlineData("$(AnotherProperty), NU1107   ,")]
        [InlineData(" NU1107   ,1607")]
        [InlineData(" NU1107   ,random,values,are,here")]
        [InlineData(" NU1107   ,CSC1607")]
        [InlineData(" NU1107   ,MSB1607")]
        [InlineData(" NU1107   ,1607,600")]
        [InlineData(" NU1107   ,1607,0,-1,abc123")]
        [InlineData("; NU1107   ;")]
        [InlineData(" NU1107   ;1607")]
        [InlineData(" NU1107   ;random;values;are;here")]
        [InlineData(" NU1107   ;CSC1607")]
        [InlineData(" NU1107   ;MSB1607")]
        [InlineData(" NU1107   ;1607;600")]
        [InlineData(" NU1107   ;1607;0;-1;abc123")]
        [InlineData(" NU1107   ,1607;0;-1,abc123")]
        [InlineData(" NU1107  \t ;1607;0;-1;abc123")]
        [InlineData(" NU1107  \t\r\n ,\t1607;0;-1,abc123")]
        public void MSBuildRestoreUtility_GetNuGetLogCodes_ParsesPropertyWithOneCode(string property)
        {
            // Arrange && Act
            var codes = MSBuildStringUtility.GetNuGetLogCodes(property);

            // Assert
            codes.Should().NotBeNull();
            codes.Should().BeEquivalentTo(new[] { NuGetLogCode.NU1107 });
        }

        [Theory]
        [InlineData("NU1107;NU1701")]
        [InlineData(";NU1107;NU1701")]
        [InlineData(";NU1107;;NU1701")]
        [InlineData("NU1107;nu1701")]
        [InlineData(";NU1701;$(AnotherProperty);NU1107;")]
        [InlineData("$(AnotherProperty);NU1701;NU1107;")]
        [InlineData("NU1107;1607;NU1701")]
        [InlineData("NU1107;random;values;are;here;NU1701")]
        [InlineData("NU1107;CSC1607;NU1701")]
        [InlineData("NU1107;MSB1607;NU1701")]
        [InlineData("NU1107;1607;600;NU1701")]
        [InlineData("NU1107;1607;0;-1;abc123;NU1701")]
        [InlineData("NU1107,NU1701")]
        [InlineData(",NU1107,NU1701")]
        [InlineData(",NU1107,,NU1701")]
        [InlineData("NU1107,nu1701")]
        [InlineData(",NU1701,$(AnotherProperty),NU1107,")]
        [InlineData("$(AnotherProperty),NU1701,NU1107,")]
        [InlineData("NU1107,1607,NU1701")]
        [InlineData("NU1107,random,values,are,here,NU1701")]
        [InlineData("NU1107,CSC1607,NU1701")]
        [InlineData("NU1107,MSB1607,NU1701")]
        [InlineData("NU1107,1607,600,NU1701")]
        [InlineData("NU1107,1607,0,-1,abc123,NU1701")]
        [InlineData("         NU1107     	,NU1701")]
        [InlineData(",         NU1107     	,NU1701")]
        [InlineData(",         NU1107     	,,NU1701")]
        [InlineData("         NU1107     	,nu1701")]
        [InlineData(",NU1701,$(AnotherProperty),         NU1107     	,")]
        [InlineData("$(AnotherProperty),NU1701,         NU1107     	,")]
        [InlineData("         NU1107     	,1607,NU1701")]
        [InlineData("         NU1107   \t  	,random,values,are,here,NU1701")]
        [InlineData("         NU1107     	,CSC1607,NU1701")]
        [InlineData("         NU1107     	,MSB1607,NU1701")]
        [InlineData("         NU1107    \t 	,1607,600,NU1701")]
        [InlineData("         NU1107    \t 	,1607,0,-1,abc123,NU1701")]
        [InlineData("         NU1107    \n\t 	,1607,0,-1,abc123,NU1701")]
        [InlineData("         NU1107    \n\t\r 	,1607,0,-1,abc123,NU1701")]
        [InlineData("         NU1107    \n\t\r 	,1607,0,-1;abc123,NU1701")]
        public void MSBuildRestoreUtility_GetNuGetLogCodes_ParsesPropertyWithMultipleCodes(string property)
        {
            // Arrange && Act
            var codes = MSBuildStringUtility.GetNuGetLogCodes(property);

            // Assert
            codes.Should().NotBeNull();
            codes.Should().BeEquivalentTo(new[] { NuGetLogCode.NU1107, NuGetLogCode.NU1701 });
        }

        [Theory]
        [InlineData("NU9999")]
        [InlineData("NU 1607")]
        [InlineData("NU1 607")]
        [InlineData("NU1107a")]
        [InlineData("1607")]
        [InlineData("random;values;are;here")]
        [InlineData("CSC1607")]
        [InlineData("MSB1607")]
        [InlineData("1607;600")]
        [InlineData("1607;0;-1;abc123")]
        [InlineData("$(NoWarn);0;-1;abc123")]
        [InlineData("")]
        public void MSBuildRestoreUtility_GetNuGetLogCodes_DoesNotParseInvalidCodes(string property)
        {
            // Arrange && Act
            var codes = MSBuildStringUtility.GetNuGetLogCodes(property);

            // Assert
            codes.Should().NotBeNull();
            codes.Should().BeEmpty();
        }

        [Theory]
        [InlineData("/tmp/test/", "/tmp/test/")]
        [InlineData("/tmp/test", "/tmp/test")]
        [InlineData("tmp/test", "tmp/test")]
        [InlineData("tmp", "tmp")]
        [InlineData("http:", "http:")]
        [InlineData("https:", "https:")]
        [InlineData("file:", "file:")]
        [InlineData("http:/", "http:/")]
        [InlineData("https:/", "https:/")]
        [InlineData("file:/", "file:/")]
        [InlineData("http://", "http://")]
        [InlineData("https://", "https://")]
        [InlineData("file://", "file://")]
        [InlineData("http://a", "http://a")]
        [InlineData("https://a", "https://a")]
        [InlineData("http:/a", "http://a")]
        [InlineData("https:/a", "https://a")]
        [InlineData("HTtP:/a", "HTtP://a")]
        [InlineData("HTTPs:/a", "HTTPs://a")]
        [InlineData("http:///", "http:///")]
        [InlineData("https:///", "https:///")]
        [InlineData("file:///", "file:///")]
        [InlineData("HTTPS:/api.NUGET.org/v3/index.json", "HTTPS://api.NUGET.org/v3/index.json")]
        [InlineData(@"C:\source\", @"C:\source\")]
        [InlineData(@"\\share\", @"\\share\")]
        public void MSBuildRestoreUtility_FixSourcePath(string input, string expected)
        {
            MSBuildRestoreUtility.FixSourcePath(input).Should().Be(expected);
        }

        [Fact]
        public void MSBuildRestoreUtility_ReplayWarningsAndErrors_Minimal()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var lockFile = new LockFile()
            {
                LogMessages = new List<IAssetsLogMessage>()
                {
                    new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Test Warning"),
                    new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Test Error")
                }
            };

            // Act
            var codes = MSBuildRestoreUtility.ReplayWarningsAndErrorsAsync(lockFile.LogMessages, logger.Object);

            // Assert
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Warning && l.Message == "Test Warning" && l.Code == NuGetLogCode.NU1500)),
                Times.Once);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Error && l.Message == "Test Error" && l.Code == NuGetLogCode.NU1000)),
                Times.Once);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Debug)), Times.Never);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Information)), Times.Never);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Minimal)), Times.Never);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Verbose)), Times.Never);
        }

        [Fact]
        public void MSBuildRestoreUtility_ReplayWarningsAndErrors_Full()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var number = 10;
            var targetGraphs = new List<string>() { "target1", "target2" };
            var lockFile = new LockFile()
            {
                LogMessages = new List<IAssetsLogMessage>()
                {
                    new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Test Warning")
                    {
                        EndColumnNumber = number,
                        EndLineNumber = number,
                        StartColumnNumber = number,
                        StartLineNumber = number,
                        FilePath = "Warning File Path",
                        LibraryId = "Warning Package",
                        ProjectPath = "Warning Project Path",
                        TargetGraphs = targetGraphs,
                        WarningLevel = WarningLevel.Important
                    },
                    new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Test Error")
                    {
                        EndColumnNumber = number,
                        EndLineNumber = number,
                        StartColumnNumber = number,
                        StartLineNumber = number,
                        FilePath = "Error File Path",
                        LibraryId = "Error Package",
                        ProjectPath = "Error Project Path",
                        TargetGraphs = targetGraphs,
                        WarningLevel = WarningLevel.Important
                    },
                }
            };

            // Act
            var codes = MSBuildRestoreUtility.ReplayWarningsAndErrorsAsync(lockFile.LogMessages, logger.Object);

            // Assert
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Warning &&
            l.Message == "Test Warning" &&
            l.Code == NuGetLogCode.NU1500 &&
            l.EndColumnNumber == number &&
            l.StartColumnNumber == number &&
            l.EndLineNumber == number &&
            l.StartLineNumber == number &&
            l.FilePath == "Warning File Path" &&
            l.LibraryId == "Warning Package" &&
            l.ProjectPath == "Warning Project Path" &&
            l.TargetGraphs.SequenceEqual(targetGraphs) &&
            l.WarningLevel == WarningLevel.Important)),
                Times.Once);

            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Error &&
            l.Message == "Test Error" &&
            l.Code == NuGetLogCode.NU1000 &&
            l.EndColumnNumber == number &&
            l.StartColumnNumber == number &&
            l.EndLineNumber == number &&
            l.StartLineNumber == number &&
            l.FilePath == "Error File Path" &&
            l.LibraryId == "Error Package" &&
            l.ProjectPath == "Error Project Path" &&
            l.TargetGraphs.SequenceEqual(targetGraphs))),
                Times.Once);

            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Debug)), Times.Never);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Information)), Times.Never);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Minimal)), Times.Never);
            logger.Verify(x => x.LogAsync(It.Is<RestoreLogMessage>(l => l.Level == LogLevel.Verbose)), Times.Never);
        }

        [PlatformFact(Platform.Windows)]
        public void MSBuildRestoreUtility_FixSourcePath_VerifyDoubleSlashWindows()
        {
            var input = "file:/C:\tmp";

            MSBuildRestoreUtility.FixSourcePath(input).Should().Be("file://C:\tmp");
        }

        [PlatformFact(Platform.Linux, Platform.Darwin)]
        public void MSBuildRestoreUtility_FixSourcePath_VerifyTripleSlashOnNonWindows()
        {
            var input = "file:/tmp/test/";

            MSBuildRestoreUtility.FixSourcePath(input).Should().Be("file:///tmp/test/");
        }

        [Theory]
        [InlineData("true", "c:\\temp\\nuget.lock.json", "true")]
        [InlineData(null, "c:\\temp\\nuget.lock.json", "false")]
        [InlineData("false", null, null)]
        public void MSBuildRestoreUtility_GetPackageSpec_NuGetLockFileProperties(
            string RestoreWithLockFile,
            string NuGetLockFilePath,
            string LockedMode)
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");

                var items = new List<IDictionary<string, string>>();


                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "2.0.0-rc.2+a.b.c" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", "482C20DE-DFF9-4BD0-B90A-BD3201AA351A" },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "net46" },
                    { "RestorePackagesWithLockFile", RestoreWithLockFile },
                    { "NuGetLockFilePath", NuGetLockFilePath },
                    { "RestoreLockedMode", LockedMode }
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();
                var lockedModeBool = string.IsNullOrEmpty(LockedMode) ? false : bool.Parse(LockedMode);

                // Assert
                project1Spec.RestoreMetadata.RestoreLockProperties.RestorePackagesWithLockFile.Should().Be(RestoreWithLockFile);
                project1Spec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath.Should().Be(NuGetLockFilePath);
                project1Spec.RestoreMetadata.RestoreLockProperties.RestoreLockedMode.Should().Be(lockedModeBool);
            }
        }

        [Theory]
        [InlineData("[1.0.0]")]
        [InlineData("[1.0.0, 1.0.0]")]
        [InlineData("[1.0.0];[1.0.0]")]
        public void MSBuildRestoreUtility_AddPackageDownloads_SinglePackageSingleVersion(string versionString)
        {
            // Arrange
            var targetFramework = FrameworkConstants.CommonFrameworks.NetStandard20;
            var alias = "netstandard2.0";
            var spec = MSBuildRestoreUtility.GetPackageSpec(new[]
            {
                CreateItems(
                new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", "a" },
                    { "TargetFrameworks", alias },
                    { "CrossTargeting", "true" },
                }),
                CreateItems(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", "a" },
                    { "TargetFramework", alias },
                    { "TargetFrameworkIdentifier", targetFramework.Framework },
                    { "TargetFrameworkVersion", $"v{targetFramework.Version.ToString(2)}" },
                    { "TargetFrameworkMoniker", $"{targetFramework.Framework},Version={targetFramework.Version.ToString(2)}" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                })
            });

            var packageX = new Mock<IMSBuildItem>();
            packageX.Setup(p => p.GetProperty("Type")).Returns("DownloadDependency");
            packageX.Setup(p => p.GetProperty("Id")).Returns("x");
            packageX.Setup(p => p.GetProperty("VersionRange")).Returns(versionString);
            packageX.Setup(p => p.GetProperty("TargetFrameworks")).Returns(targetFramework.GetShortFolderName());

            var msbuildItems = new[]
            {
                packageX.Object
            };

            // Act
            MSBuildRestoreUtility.AddPackageDownloads(spec, msbuildItems);

            // Assert
            var framework = spec.GetTargetFramework(targetFramework);
            Assert.Equal(1, framework.DownloadDependencies.Count);
            Assert.Equal("x", framework.DownloadDependencies[0].Name);
            Assert.Equal("[1.0.0]", framework.DownloadDependencies[0].VersionRange.ToShortString());
        }

        [Theory]
        [InlineData("[1.0.0];[2.0.0]")]
        [InlineData(";[1.0.0];;[2.0.0];")]
        public void MSBuildRestoreUtility_AddPackageDownloads_SinglePackageMultipleVersions(string versionString)
        {
            // Arrange
            var targetFramework = FrameworkConstants.CommonFrameworks.NetStandard20;
            var alias = "netstandard2.0";
            var spec = MSBuildRestoreUtility.GetPackageSpec(new[]
            {
                CreateItems(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", "a" },
                    { "TargetFrameworks", alias },
                    { "CrossTargeting", "true" },
                }),
                CreateItems(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", "a" },
                    { "TargetFramework", alias },
                    { "TargetFrameworkIdentifier", targetFramework.Framework },
                    { "TargetFrameworkVersion", $"v{targetFramework.Version.ToString(2)}" },
                    { "TargetFrameworkMoniker", $"{targetFramework.Framework},Version={targetFramework.Version.ToString(2)}" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                })
                });

            var packageX = new Mock<IMSBuildItem>();
            packageX.Setup(p => p.GetProperty("Type")).Returns("DownloadDependency");
            packageX.Setup(p => p.GetProperty("Id")).Returns("x");
            packageX.Setup(p => p.GetProperty("VersionRange")).Returns(versionString);
            packageX.Setup(p => p.GetProperty("TargetFrameworks")).Returns(targetFramework.GetShortFolderName());

            var msbuildItems = new[]
            {
                packageX.Object
            };

            // Act
            MSBuildRestoreUtility.AddPackageDownloads(spec, msbuildItems);

            // Assert
            var framework = spec.GetTargetFramework(targetFramework);
            Assert.Equal(2, framework.DownloadDependencies.Count);
            Assert.Equal(1, framework.DownloadDependencies.Count(d => d.Name == "x" && d.VersionRange.ToShortString() == "[1.0.0]"));
            Assert.Equal(1, framework.DownloadDependencies.Count(d => d.Name == "x" && d.VersionRange.ToShortString() == "[2.0.0]"));
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("[1.0.0, )")]
        [InlineData("(, 1.0.0]")]
        [InlineData("(, 1.0.0)")]
        [InlineData("[1.0.0, 2.0.0]")]
        [InlineData("[1.0.0, 2.0.0)")]
        [InlineData("[1.0.0];2.0.0")]
        public void MSBuildRestoreUtility_AddPackageDownloads_InvalidVersionRange(string versionString)
        {
            // Arrange
            var targetFramework = FrameworkConstants.CommonFrameworks.NetStandard20;
            var alias = "netstandard2.0";

            var spec = MSBuildRestoreUtility.GetPackageSpec(new[]
            {
                CreateItems(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", "a" },
                    { "TargetFrameworks", alias },
                    { "CrossTargeting", "true" },
                }),
                CreateItems(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", "a" },
                    { "TargetFramework", alias },
                    { "TargetFrameworkIdentifier", targetFramework.Framework },
                    { "TargetFrameworkVersion", $"v{targetFramework.Version.ToString(2)}" },
                    { "TargetFrameworkMoniker", $"{targetFramework.Framework},Version={targetFramework.Version.ToString(2)}" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                })
                });

            var packageX = new Mock<IMSBuildItem>();
            packageX.Setup(p => p.GetProperty("Type")).Returns("DownloadDependency");
            packageX.Setup(p => p.GetProperty("Id")).Returns("x");
            packageX.Setup(p => p.GetProperty("VersionRange")).Returns(versionString);
            packageX.Setup(p => p.GetProperty("TargetFrameworks")).Returns(targetFramework.GetShortFolderName());

            var msbuildItems = new[]
            {
                packageX.Object
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MSBuildRestoreUtility.AddPackageDownloads(spec, msbuildItems));
        }

        [Fact]
        public void MSBuildRestoreUtility_AddPackageDownloads_NoVersion_ThrowsException()
        {
            // Arrange
            PackageSpec spec = MSBuildRestoreUtility.GetPackageSpec(new[] { CreateItems(new Dictionary<string, string>()) });
            Mock<IMSBuildItem> packageX = new Mock<IMSBuildItem>();
            const string packageId = "x";
            packageX.Setup(p => p.GetProperty("Type")).Returns("DownloadDependency");
            packageX.Setup(p => p.GetProperty("Id")).Returns(packageId);

            IMSBuildItem[] msbuildItems = new[]
            {
                packageX.Object
            };

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(() => MSBuildRestoreUtility.AddPackageDownloads(spec, msbuildItems));
            string expectedMessage = string.Format(CultureInfo.CurrentCulture, Strings.Error_PackageDownload_NoVersion, packageId);
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void MSBuildRestoreUtility_GetDependencySpec_CentralVersionIsMergedWhenCPVMEnabled()
        {
            var projectName = "acpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                    { "_CentralPackageVersionsEnabled", "true"}
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "netcoreapp3.0" },
                    { "TargetFrameworkIdentifier", ".NETCoreApp" },
                    { "TargetFrameworkVersion", "v3.0" },
                    { "TargetFrameworkMoniker", "NETCoreApp,Version=3.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package reference
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });

                // Package reference with version
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "1.2.1" },
                    { "IsImplicitlyDefined", "true" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                // Central Version for the package above and another one for a package y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "2.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "z" },
                    { "VersionRange", "3.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == projectName);

                // Assert
                Assert.Equal(1, project1Spec.TargetFrameworks.Count());
                Assert.Equal(2, project1Spec.TargetFrameworks.First().Dependencies.Count);
                Assert.Equal(3, project1Spec.TargetFrameworks.First().CentralPackageVersions.Count);

                var dependencyX = project1Spec.TargetFrameworks.First().Dependencies.Where(d => d.Name == "x").First();
                var dependencyY = project1Spec.TargetFrameworks.First().Dependencies.Where(d => d.Name == "y").First();

                Assert.Equal("[1.0.0, )", dependencyX.LibraryRange.VersionRange.ToNormalizedString());
                Assert.Equal(LibraryIncludeFlags.Compile | LibraryIncludeFlags.Build, dependencyX.IncludeType);
                Assert.Equal("[1.2.1, )", dependencyY.LibraryRange.VersionRange.ToNormalizedString());

                var centralDependencyX = project1Spec.TargetFrameworks.First().CentralPackageVersions["x"];
                var centralDependencyY = project1Spec.TargetFrameworks.First().CentralPackageVersions["y"];
                var centralDependencyZ = project1Spec.TargetFrameworks.First().CentralPackageVersions["Z"];

                Assert.Equal("x", centralDependencyX.Name);
                Assert.Equal("[1.0.0, )", centralDependencyX.VersionRange.ToNormalizedString());

                Assert.Equal("y", centralDependencyY.Name);
                Assert.Equal("[2.0.0, )", centralDependencyY.VersionRange.ToNormalizedString());

                Assert.Equal("z", centralDependencyZ.Name);
                Assert.Equal("[3.0.0, )", centralDependencyZ.VersionRange.ToNormalizedString());

                Assert.True(project1Spec.RestoreMetadata.CentralPackageVersionsEnabled);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetDependencySpec_HandlesDuplicatesWhenCPVMEnabled()
        {
            // Arrange
            using var workingDir = TestDirectory.Create();
            const string projectName = "acpvm";
            const string projectUniqueName = "21031AA5-93B3-4230-BA20-0EF4CFCEDAAB";
            var project1Root = Path.Combine(workingDir, projectName);
            var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

            var items = new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string>()
                    {
                        { "Type", "ProjectSpec" },
                        { "ProjectName", projectName },
                        { "ProjectStyle", "PackageReference" },
                        { "ProjectUniqueName", projectUniqueName },
                        { "ProjectPath", project1Path },
                        { "CrossTargeting", "true" },
                        { "_CentralPackageVersionsEnabled", "true"}
                    },
                    new Dictionary<string, string>()
                    {
                        { "Type", "TargetFrameworkInformation" },
                        { "AssetTargetFallback", "" },
                        { "PackageTargetFallback", "" },
                        { "ProjectUniqueName", projectUniqueName },
                        { "TargetFramework", "netcoreapp3.0" },
                        { "TargetFrameworkIdentifier", ".NETCoreApp" },
                        { "TargetFrameworkVersion", "v3.0" },
                        { "TargetFrameworkMoniker", "NETCoreApp,Version=3.0" },
                        { "TargetPlatformIdentifier", "" },
                        { "TargetPlatformMoniker", "" },
                        { "TargetPlatformVersion", "" },
                    },
                    // Package reference
                    new Dictionary<string, string>()
                    {
                        { "Type", "Dependency" },
                        { "ProjectUniqueName", projectUniqueName },
                        { "Id", "x" },
                        { "TargetFrameworks", "netcoreapp3.0" },
                        { "IncludeAssets", "build;compile" },
                        { "CrossTargeting", "true" },
                    },
                    // Duplicate central package versions
                    new Dictionary<string, string>()
                    {
                        { "Type", "CentralPackageVersion" },
                        { "ProjectUniqueName", projectUniqueName },
                        { "Id", "x" },
                        { "VersionRange", "1.0.0" },
                        { "TargetFrameworks", "netcoreapp3.0" },
                    },
                    new Dictionary<string, string>()
                    {
                        { "Type", "CentralPackageVersion" },
                        { "ProjectUniqueName", projectUniqueName },
                        { "Id", "x" },
                        { "VersionRange", "2.0.0" },
                        { "TargetFrameworks", "netcoreapp3.0" },
                    }
                };

            var wrappedItems = items.Select(CreateItems).ToList();

            // Act
            var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
            var project1Spec = dgSpec.Projects.Single(e => e.Name == projectName);

            // Assert
            Assert.Equal(1, project1Spec.TargetFrameworks.Count());
            Assert.Equal(1, project1Spec.TargetFrameworks.First().Dependencies.Count);
            Assert.Equal(1, project1Spec.TargetFrameworks.First().CentralPackageVersions.Count);

            var dependencyX = project1Spec.TargetFrameworks.First().Dependencies.Where(d => d.Name == "x").First();

            Assert.Equal("[2.0.0, )", dependencyX.LibraryRange.VersionRange.ToNormalizedString());
            Assert.Equal(LibraryIncludeFlags.Compile | LibraryIncludeFlags.Build, dependencyX.IncludeType);

            var centralDependencyX = project1Spec.TargetFrameworks.First().CentralPackageVersions["x"];

            Assert.Equal("x", centralDependencyX.Name);
            Assert.Equal("[2.0.0, )", centralDependencyX.VersionRange.ToNormalizedString());

            Assert.True(project1Spec.RestoreMetadata.CentralPackageVersionsEnabled);
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_CPVM_EnabledLegacyProjectsMergesCentralVersions()
        {
            var projectName = "alegacycpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "_CentralPackageVersionsEnabled", "true"}
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "net472" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.7.2" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=v4.7.2" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package reference
                // No TargetFrameworks metadata
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });


                // Central Version for the package above and another one for a package y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "2.0.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == projectName);

                // Assert
                Assert.Equal(1, project1Spec.TargetFrameworks.Count());
                Assert.Equal(1, project1Spec.TargetFrameworks.First().Dependencies.Count);
                Assert.Equal(2, project1Spec.TargetFrameworks.First().CentralPackageVersions.Count);

                Assert.Equal("[1.0.0, )", project1Spec.TargetFrameworks.First().Dependencies[0].LibraryRange.VersionRange.ToNormalizedString());
                Assert.Equal(LibraryIncludeFlags.Compile | LibraryIncludeFlags.Build, project1Spec.TargetFrameworks.First().Dependencies[0].IncludeType);

                Assert.Equal("x", project1Spec.TargetFrameworks.First().CentralPackageVersions["x"].Name);
                Assert.Equal("[1.0.0, )", project1Spec.TargetFrameworks.First().CentralPackageVersions["x"].VersionRange.ToNormalizedString());

                Assert.Equal("y", project1Spec.TargetFrameworks.First().CentralPackageVersions["y"].Name);
                Assert.Equal("[2.0.0, )", project1Spec.TargetFrameworks.First().CentralPackageVersions["y"].VersionRange.ToNormalizedString());

                Assert.True(project1Spec.RestoreMetadata.CentralPackageVersionsEnabled);
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("true", false)]
        [InlineData("invalid", false)]
        [InlineData("false", true)]
        public void MSBuildRestoreUtility_GetPackageSpec_CPVM_VersionOverrideCanBeDisabled(string isCentralPackageVersionOverrideEnabled, bool disabled)
        {
            var projectName = "alegacycpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "_CentralPackageVersionsEnabled", "true"},
                    { "CentralPackageVersionOverrideEnabled", isCentralPackageVersionOverrideEnabled }
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "net472" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.7.2" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=v4.7.2" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package reference
                // No TargetFrameworks metadata
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });


                // Central Version for the package above and another one for a package y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "2.0.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == projectName);

                // Assert
                Assert.Equal(1, project1Spec.TargetFrameworks.Count());
                Assert.Equal(1, project1Spec.TargetFrameworks.First().Dependencies.Count);
                Assert.Equal(2, project1Spec.TargetFrameworks.First().CentralPackageVersions.Count);

                Assert.Equal("[1.0.0, )", project1Spec.TargetFrameworks.First().Dependencies[0].LibraryRange.VersionRange.ToNormalizedString());
                Assert.Equal(LibraryIncludeFlags.Compile | LibraryIncludeFlags.Build, project1Spec.TargetFrameworks.First().Dependencies[0].IncludeType);

                Assert.Equal("x", project1Spec.TargetFrameworks.First().CentralPackageVersions["x"].Name);
                Assert.Equal("[1.0.0, )", project1Spec.TargetFrameworks.First().CentralPackageVersions["x"].VersionRange.ToNormalizedString());

                Assert.Equal("y", project1Spec.TargetFrameworks.First().CentralPackageVersions["y"].Name);
                Assert.Equal("[2.0.0, )", project1Spec.TargetFrameworks.First().CentralPackageVersions["y"].VersionRange.ToNormalizedString());

                Assert.True(project1Spec.RestoreMetadata.CentralPackageVersionsEnabled);

                if (disabled)
                {
                    Assert.True(project1Spec.RestoreMetadata.CentralPackageVersionOverrideDisabled);
                }
                else
                {
                    Assert.False(project1Spec.RestoreMetadata.CentralPackageVersionOverrideDisabled);
                }
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("                     ", false)]
        [InlineData("false", false)]
        [InlineData("invalid", false)]
        [InlineData("true", true)]
        public void MSBuildRestoreUtility_GetPackageSpec_CPVM_FloatingVersionsCanBeEnabled(string isCentralPackageFloatingVersionsEnabled, bool enabled)
        {
            var projectName = "alegacycpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "_CentralPackageVersionsEnabled", "true"},
                    { "CentralPackageFloatingVersionsEnabled", isCentralPackageFloatingVersionsEnabled }
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "net472" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.7.2" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=v4.7.2" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package reference
                // No TargetFrameworks metadata
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });


                // Central Version for the package above and another one for a package y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "2.0.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == projectName);

                // Assert
                Assert.Equal(1, project1Spec.TargetFrameworks.Count());
                Assert.Equal(1, project1Spec.TargetFrameworks.First().Dependencies.Count);
                Assert.Equal(2, project1Spec.TargetFrameworks.First().CentralPackageVersions.Count);

                Assert.Equal("[1.0.0, )", project1Spec.TargetFrameworks.First().Dependencies[0].LibraryRange.VersionRange.ToNormalizedString());
                Assert.Equal(LibraryIncludeFlags.Compile | LibraryIncludeFlags.Build, project1Spec.TargetFrameworks.First().Dependencies[0].IncludeType);

                Assert.Equal("x", project1Spec.TargetFrameworks.First().CentralPackageVersions["x"].Name);
                Assert.Equal("[1.0.0, )", project1Spec.TargetFrameworks.First().CentralPackageVersions["x"].VersionRange.ToNormalizedString());

                Assert.Equal("y", project1Spec.TargetFrameworks.First().CentralPackageVersions["y"].Name);
                Assert.Equal("[2.0.0, )", project1Spec.TargetFrameworks.First().CentralPackageVersions["y"].VersionRange.ToNormalizedString());

                Assert.True(project1Spec.RestoreMetadata.CentralPackageVersionsEnabled);

                if (enabled)
                {
                    Assert.True(project1Spec.RestoreMetadata.CentralPackageFloatingVersionsEnabled);
                }
                else
                {
                    Assert.False(project1Spec.RestoreMetadata.CentralPackageFloatingVersionsEnabled);
                }
            }
        }

        [Theory]
        [InlineData(ProjectStyle.DotnetCliTool)]
        [InlineData(ProjectStyle.DotnetToolReference)]
        [InlineData(ProjectStyle.PackagesConfig)]
        [InlineData(ProjectStyle.ProjectJson)]
        [InlineData(ProjectStyle.Standalone)]
        public void MSBuildRestoreUtility_GetPackageSpec_CPVM_OnlyPackageReferenceProjectsWillHaveCPVMEnabled(ProjectStyle projectStyle)
        {
            var projectName = "bcpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var projectSpec = CreateItems(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "bcpvm" },
                    { "ProjectStyle", projectStyle.ToString() },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                    { "_CentralPackageVersionsEnabled", "true"}
                });

                // Act + Assert
                Assert.False(MSBuildRestoreUtility.GetCentralPackageManagementSettings(projectSpec, projectStyle).IsEnabled);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetDependencySpec_CPVM_NotEnabledProjectDoesNotMergeVersions()
        {
            var projectName = "ccpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "Packagereference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "CrossTargeting", "true" },
                    { "_CentralPackageVersionsEnabled", "false"}
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "netcoreapp3.0" },
                    { "TargetFrameworkIdentifier", ".NETCoreApp" },
                    { "TargetFrameworkVersion", "v3.0" },
                    { "TargetFrameworkMoniker", "NETCoreApp,Version=3.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package reference
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });

                // Central Version for the package above and another one for a package y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "2.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "3.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == projectName);

                // Assert
                // Dependency counts
                Assert.Equal(1, project1Spec.TargetFrameworks.Count());
                Assert.Equal(1, project1Spec.TargetFrameworks.First().Dependencies.Count);
                Assert.Equal(0, project1Spec.TargetFrameworks.First().CentralPackageVersions.Count);
                Assert.Equal("(, )", project1Spec.TargetFrameworks.First().Dependencies.First().LibraryRange.VersionRange.ToNormalizedString());
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_CPVM_NoVersionChecks()
        {
            var projectName = "ccpvm2";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "Packagereference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "CrossTargeting", "true" },
                    { "_CentralPackageVersionsEnabled", "true"}
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "netcoreapp3.0" },
                    { "TargetFrameworkIdentifier", ".NETCoreApp" },
                    { "TargetFrameworkVersion", "v3.0" },
                    { "TargetFrameworkMoniker", "NETCoreApp,Version=3.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package reference
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "IncludeAssets", "build;compile" },
                    { "VersionRange", "" },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });

                // Central Version for the package above and another one for a package y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var packSpec = MSBuildRestoreUtility.GetPackageSpec(wrappedItems);

                // Assert
                Assert.Equal(1, packSpec.TargetFrameworks.Count());

                var dependencyX = packSpec.TargetFrameworks.First().Dependencies.Where(d => d.Name == "x").First();
                var dependencyY = packSpec.TargetFrameworks.First().Dependencies.Where(d => d.Name == "y").First();

                var centralDependencyX = packSpec.TargetFrameworks.First().CentralPackageVersions["x"];
                var centralDependencyY = packSpec.TargetFrameworks.First().CentralPackageVersions["Y"];

                Assert.Equal("(, )", dependencyX.LibraryRange.VersionRange.ToNormalizedString());
                Assert.Equal("(, )", dependencyY.LibraryRange.VersionRange.ToNormalizedString());
                Assert.Equal("(, )", centralDependencyX.VersionRange.ToNormalizedString());
                Assert.Equal("(, )", centralDependencyY.VersionRange.ToNormalizedString());
            }
        }

        [Theory]
        [InlineData("false", false)]
        [InlineData("true", true)]
        public void MSBuildRestoreUtility_GetPackageSpec_CPVM_TransitiveDependencyPinning(string value, bool expected)
        {
            var projectName = "ccpvm2";
            var projectStyle = ProjectStyle.PackageReference;
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var projectUniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var projectSpec = CreateItems(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "bcpvm" },
                    { "ProjectStyle", projectStyle.ToString() },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                    { "_CentralPackageVersionsEnabled", "true"},
                    { ProjectBuildProperties.CentralPackageTransitivePinningEnabled, value},
                });

                // Act
                var settings = MSBuildRestoreUtility.GetCentralPackageManagementSettings(projectSpec, projectStyle);

                // Assert
                Assert.Equal(expected, settings.IsCentralPackageTransitivePinningEnabled);
            }
        }

        /// <summary>
        /// Verifies that <see cref="MSBuildRestoreUtility.GetDependencySpec(IEnumerable{IMSBuildItem})" /> applies version overrides correctly depending on whether or not central package management is enabled.
        /// </summary>
        /// <param name="isCentralPackageManagementEnabled"><see langword="true" /> if central package management is enabled, otherwise <see langword="false" />.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MSBuildRestoreUtility_GetDependencySpec_VersionOverrideAppliesWhenCPVMEnabled(bool isCentralPackageManagementEnabled)
        {
            var projectName = "acpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                const string projectUniqueName = "3BA10BF9-98DF-4952-A062-651AEE292848";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                    { "_CentralPackageVersionsEnabled", isCentralPackageManagementEnabled.ToString()}
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "netcoreapp3.0" },
                    { "TargetFrameworkIdentifier", ".NETCoreApp" },
                    { "TargetFrameworkVersion", "v3.0" },
                    { "TargetFrameworkMoniker", "NETCoreApp,Version=3.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                // Package reference
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "VersionRange", isCentralPackageManagementEnabled ? null : "1.0.0" },
                    { "IncludeAssets", "build;compile" },
                    { "CrossTargeting", "true" },
                });

                // Package reference with version but is implicitly defined
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "1.2.1" },
                    { "IsImplicitlyDefined", "true" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                // Package reference with version
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "z" },
                    { "VersionRange", isCentralPackageManagementEnabled ? null : "3.0.0" },
                    { "VersionOverride", isCentralPackageManagementEnabled ? "9.9.9" : null },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                // Central Version for the package above and another one for a package y
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "1.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "y" },
                    { "VersionRange", "2.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });
                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "z" },
                    { "VersionRange", "3.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single(e => e.Name == projectName);

                TargetFrameworkInformation targetFrameworkInformation = project1Spec.TargetFrameworks.First();

                // Assert
                Assert.Equal(1, project1Spec.TargetFrameworks.Count());
                Assert.Equal(3, targetFrameworkInformation.Dependencies.Count);
                Assert.Equal(isCentralPackageManagementEnabled ? 3 : 0, targetFrameworkInformation.CentralPackageVersions.Count);

                var dependencyX = targetFrameworkInformation.Dependencies.First(d => d.Name == "x");
                var dependencyY = targetFrameworkInformation.Dependencies.First(d => d.Name == "y");
                var dependencyZ = targetFrameworkInformation.Dependencies.First(d => d.Name == "z");

                Assert.Equal(LibraryIncludeFlags.Compile | LibraryIncludeFlags.Build, dependencyX.IncludeType);

                if (isCentralPackageManagementEnabled)
                {
                    Assert.True(project1Spec.RestoreMetadata.CentralPackageVersionsEnabled);

                    Assert.True(dependencyX.VersionCentrallyManaged);
                    Assert.False(dependencyY.VersionCentrallyManaged);
                    Assert.False(dependencyZ.VersionCentrallyManaged);

                    Assert.Equal("[1.0.0, )", dependencyX.LibraryRange.VersionRange.ToNormalizedString());
                    Assert.Equal("[1.2.1, )", dependencyY.LibraryRange.VersionRange.ToNormalizedString());
                    Assert.Equal("[9.9.9, )", dependencyZ.LibraryRange.VersionRange.ToNormalizedString());

                    var centralDependencyX = targetFrameworkInformation.CentralPackageVersions["x"];
                    var centralDependencyY = targetFrameworkInformation.CentralPackageVersions["y"];
                    var centralDependencyZ = targetFrameworkInformation.CentralPackageVersions["Z"];

                    Assert.Equal("x", centralDependencyX.Name);
                    Assert.Equal("[1.0.0, )", centralDependencyX.VersionRange.ToNormalizedString());

                    Assert.Equal("y", centralDependencyY.Name);
                    Assert.Equal("[2.0.0, )", centralDependencyY.VersionRange.ToNormalizedString());

                    Assert.Equal("z", centralDependencyZ.Name);
                    Assert.Equal("[3.0.0, )", centralDependencyZ.VersionRange.ToNormalizedString());
                }
                else
                {
                    Assert.False(project1Spec.RestoreMetadata.CentralPackageVersionsEnabled);

                    Assert.Equal("[1.0.0, )", dependencyX.LibraryRange.VersionRange.ToNormalizedString());
                    Assert.Equal("[1.2.1, )", dependencyY.LibraryRange.VersionRange.ToNormalizedString());
                    Assert.Equal("[3.0.0, )", dependencyZ.LibraryRange.VersionRange.ToNormalizedString());

                    Assert.False(dependencyX.VersionCentrallyManaged);
                    Assert.False(dependencyY.VersionCentrallyManaged);
                    Assert.False(dependencyZ.VersionCentrallyManaged);
                }
            }
        }

        /// <summary>
        /// Verifies that <see cref="MSBuildRestoreUtility.GetDependencySpec(IEnumerable{IMSBuildItem})" /> throws a <see cref="ArgumentException" /> if PackageReference contains a value for VersionOverride is not a valid <see cref="VersionRange" />.
        /// </summary>
        [Fact]
        public void MSBuildRestoreUtility_GetDependencySpec_ThrowsArgumentExceptionWhenVersionOverrideIsInvalid()
        {
            var projectName = "acpvm";
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                const string projectUniqueName = "3BA10BF9-98DF-4952-A062-651AEE292848";
                var project1Root = Path.Combine(workingDir, projectName);
                var project1Path = Path.Combine(project1Root, $"{projectName}.csproj");

                var items = new List<IDictionary<string, string>>();

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", projectName },
                    { "ProjectStyle", "PackageReference" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "ProjectPath", project1Path },
                    { "CrossTargeting", "true" },
                    { "_CentralPackageVersionsEnabled", bool.TrueString}
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "TargetFramework", "netcoreapp3.0" },
                    { "TargetFrameworkIdentifier", ".NETCoreApp" },
                    { "TargetFrameworkVersion", "v3.0" },
                    { "TargetFrameworkMoniker", "NETCoreApp,Version=3.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "Dependency" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                    { "VersionOverride", "invalid" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "CentralPackageVersion" },
                    { "ProjectUniqueName", projectUniqueName },
                    { "Id", "x" },
                    { "VersionRange", "3.0.0" },
                    { "TargetFrameworks", "netcoreapp3.0" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Assert
                ArgumentException exception = Assert.Throws<ArgumentException>(() => MSBuildRestoreUtility.GetDependencySpec(wrappedItems));

                Assert.Equal("'invalid' is not a valid version string.", exception.Message);
            }
        }

        [Fact]
        public void GetPackageSpec_DootnetToolReference_WithTargetFrameworkInformation_Succeeds()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var uniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var outputPath = Path.Combine(workingDir, "a", "obj");
                var atf = FrameworkConstants.CommonFrameworks.Net462;
                var items = new List<IDictionary<string, string>>();
                var runtimeIdentifierGraphPath = Path.Combine(workingDir, "sdk", "runtime.json");

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a1" },
                    { "ProjectStyle", "DotnetToolReference" },
                    { "OutputPath", outputPath },
                    { "ProjectUniqueName", uniqueName },
                    { "ProjectPath", project1Root },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", atf.GetShortFolderName() },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", uniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", FrameworkConstants.FrameworkIdentifiers.NetCoreApp },
                    { "TargetFrameworkVersion", "v3.0" },
                    { "TargetFrameworkMoniker", $"{FrameworkConstants.FrameworkIdentifiers.NetCoreApp},Version=3.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                    { "RuntimeIdentifierGraphPath", runtimeIdentifierGraphPath }
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var targetFrameworkInformation = dgSpec.Projects.Single().TargetFrameworks.Single();

                // Assert
                targetFrameworkInformation.FrameworkName.Framework.Should().Be(FrameworkConstants.FrameworkIdentifiers.NetCoreApp);
                targetFrameworkInformation.AssetTargetFallback.Should().BeTrue();
                var assetTargetFallbackFramework = targetFrameworkInformation.FrameworkName as AssetTargetFallbackFramework;
                assetTargetFallbackFramework.Fallback.Should().HaveCount(1);
                assetTargetFallbackFramework.Fallback.Single().Should().Be(atf);
                targetFrameworkInformation.RuntimeIdentifierGraphPath.Should().Be(runtimeIdentifierGraphPath);
            }
        }

        [Fact]
        public void GetPackageSpec_WithRuntimeIdentifierGraphPath_Succeeds()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var uniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var runtimeIdentifierGraphPath = Path.Combine(workingDir, "sdk", "runtime.json");
                var items = new List<IDictionary<string, string>>();

                items.Add(CreateProject(project1Root, uniqueName));

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", uniqueName },
                    { "TargetFramework", "net46" },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.6" },
                    { "TargetFrameworkMoniker", ".NETFramework,Version=4.6" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                    { "RuntimeIdentifierGraphPath", runtimeIdentifierGraphPath }
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var targetFrameworkInformation = dgSpec.Projects.Single().TargetFrameworks.Single();

                // Assert
                targetFrameworkInformation.RuntimeIdentifierGraphPath.Should().Be(runtimeIdentifierGraphPath);
            }
        }

        [Fact]
        public void GetPackageSpec_TargetFrameworkInformationWithAlias_Succeeds()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var uniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var runtimeIdentifierGraphPath = Path.Combine(workingDir, "sdk", "runtime.json");
                var items = new List<IDictionary<string, string>>();
                var alias = "blabla";
                var framework = FrameworkConstants.CommonFrameworks.Net461;
                items.Add(CreateProject(project1Root, uniqueName));

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "AssetTargetFallback", "" },
                    { "PackageTargetFallback", "" },
                    { "ProjectUniqueName", uniqueName },
                    { "TargetFramework", alias },
                    { "TargetFrameworkIdentifier", framework.Framework },
                    { "TargetFrameworkVersion", $"v{framework.Version.ToString(2)}" },
                    { "TargetFrameworkMoniker", framework.DotNetFrameworkName },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                    { "RuntimeIdentifierGraphPath", runtimeIdentifierGraphPath }
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var targetFrameworkInformation = dgSpec.Projects.Single().TargetFrameworks.Single();

                // Assert
                targetFrameworkInformation.RuntimeIdentifierGraphPath.Should().Be(runtimeIdentifierGraphPath);
                targetFrameworkInformation.TargetAlias.Should().Be(alias);
                targetFrameworkInformation.FrameworkName.Equals(framework);
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_MultiTargettingWithNet5_UsesIndividualProperties()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                var net60Alias = "net5.0";
                var net50WithPlatformAlias = "net50-android21.0";

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", net50WithPlatformAlias },
                    { "TargetFrameworkIdentifier", ".NETCoreApp" },
                    { "TargetFrameworkVersion", "v5.0" },
                    { "TargetFrameworkMoniker", ".NETCoreApp,Version=v5.0" },
                    { "TargetPlatformIdentifier", "android" },
                    { "TargetPlatformVersion", "29.0" },
                    { "TargetPlatformMoniker", "android,Version=29.0" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", net60Alias },
                    { "TargetFrameworkIdentifier", ".NETCoreApp" },
                    { "TargetFrameworkVersion", "v6.0" },
                    { "TargetFrameworkMoniker", ".NETCoreApp,Version=v6.0" },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                var net60Framework = project1Spec.TargetFrameworks.Single(e => e.TargetAlias.Equals(net60Alias));
                var net50Android = project1Spec.TargetFrameworks.Single(e => e.TargetAlias.Equals(net50WithPlatformAlias));

                // Assert
                net60Framework.FrameworkName.Framework.Should().Be(FrameworkConstants.FrameworkIdentifiers.NetCoreApp);
                net60Framework.FrameworkName.Version.Should().Be(new Version("6.0.0.0"));
                net60Framework.FrameworkName.HasPlatform.Should().BeFalse();

                net50Android.FrameworkName.Framework.Should().Be(FrameworkConstants.FrameworkIdentifiers.NetCoreApp);
                net50Android.FrameworkName.Version.Should().Be(new Version("5.0.0.0"));
                net50Android.FrameworkName.HasPlatform.Should().BeTrue();
                net50Android.FrameworkName.Platform.Should().Be("android");
                net50Android.FrameworkName.PlatformVersion.Should().Be(new Version("29.0.0.0"));
            }
        }

        [Fact]
        public void MSBuildRestoreUtility_GetPackageSpec_SingleTargetingFrameworkWithProfile_UsesIndividualProperties()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var project1Root = Path.Combine(workingDir, "a");
                var project1Path = Path.Combine(project1Root, "a.csproj");
                var outputPath1 = Path.Combine(project1Root, "obj");
                var fallbackFolder = Path.Combine(project1Root, "fallback");
                var packagesFolder = Path.Combine(project1Root, "packages");
                var project1UniqueName = "482C20DE-DFF9-4BD0-B90A-BD3201AA351A";
                var items = new List<IDictionary<string, string>>();

                var alias = "net5.0";
                var profile = "Client";

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", project1UniqueName },
                    { "ProjectPath", project1Path },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "CrossTargeting", "true" },
                });

                items.Add(new Dictionary<string, string>()
                {
                    { "Type", "TargetFrameworkInformation" },
                    { "ProjectUniqueName", project1UniqueName },
                    { "PackageTargetFallback", "" },
                    { "TargetFramework", alias },
                    { "TargetFrameworkIdentifier", ".NETFramework" },
                    { "TargetFrameworkVersion", "v4.0" },
                    { "TargetFrameworkMoniker", $".NETFramework,Version=v4.0,Profile={profile}" },
                    { "TargetFrameworkProfile", profile },
                    { "TargetPlatformIdentifier", "" },
                    { "TargetPlatformMoniker", "" },
                    { "TargetPlatformVersion", "" },
                });

                var wrappedItems = items.Select(CreateItems).ToList();

                // Act
                var dgSpec = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);
                var project1Spec = dgSpec.Projects.Single();

                var net60Framework = project1Spec.TargetFrameworks.Single(e => e.TargetAlias.Equals(alias));

                // Assert
                net60Framework.FrameworkName.Framework.Should().Be(FrameworkConstants.FrameworkIdentifiers.Net);
                net60Framework.FrameworkName.Version.Should().Be(new Version("4.0.0.0"));
                net60Framework.FrameworkName.Profile.Should().Be(profile);
                net60Framework.FrameworkName.HasPlatform.Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("                     ", false)]
        [InlineData("false", false)]
        [InlineData("invalid", false)]
        [InlineData("true", true)]
        [InlineData("           true    ", true)]
        public void IsPropertyTrue_ReturnsExpectedValue(string value, bool expected)
        {
            const string propertyName = "Property1";

            MSBuildItem item = new("Item1", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [propertyName] = value
            });

            MSBuildRestoreUtility.IsPropertyTrue(item, propertyName).Should().Be(expected);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("                     ", false)]
        [InlineData("false", true)]
        [InlineData("   false     ", true)]
        [InlineData("invalid", false)]
        [InlineData("true", false)]
        [InlineData("           true    ", false)]
        public void IsPropertyFalse_ReturnsExpectedValue(string value, bool expected)
        {
            const string propertyName = "Property1";

            MSBuildItem item = new("Item1", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [propertyName] = value
            });

            MSBuildRestoreUtility.IsPropertyFalse(item, propertyName).Should().Be(expected);
        }

        private static IDictionary<string, string> CreateProject(string root, string uniqueName)
        {
            var project1Path = Path.Combine(root, "a.csproj");
            var outputPath1 = Path.Combine(root, "obj");
            var fallbackFolder = Path.Combine(root, "fallback");
            var packagesFolder = Path.Combine(root, "packages");
            var configFilePath = Path.Combine(root, "nuget.config");

            return new Dictionary<string, string>()
                {
                    { "Type", "ProjectSpec" },
                    { "Version", "2.0.0-rc.2+a.b.c" },
                    { "ProjectName", "a" },
                    { "ProjectStyle", "PackageReference" },
                    { "OutputPath", outputPath1 },
                    { "ProjectUniqueName", uniqueName },
                    { "ProjectPath", project1Path },
                    { "TargetFrameworks", "net46" },
                    { "Sources", "https://nuget.org/a/index.json;https://nuget.org/b/index.json" },
                    { "FallbackFolders", fallbackFolder },
                    { "PackagesPath", packagesFolder },
                    { "ConfigFilePaths", configFilePath },
                };
        }

        private IMSBuildItem CreateItems(IDictionary<string, string> properties)
        {
            return new MSBuildItem(Guid.NewGuid().ToString(), properties);
        }

        private Dictionary<string, string> WithUniqueName(Dictionary<string, string> item, string uniqueName)
        {
            var newItem = new Dictionary<string, string>(item, StringComparer.OrdinalIgnoreCase);
            newItem["ProjectUniqueName"] = uniqueName;
            return newItem;
        }
    }
}
