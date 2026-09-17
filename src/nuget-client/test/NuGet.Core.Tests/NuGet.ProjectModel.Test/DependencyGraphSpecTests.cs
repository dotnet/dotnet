// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class DependencyGraphSpecTests
    {
        private const string DgSpecWithCentralDependencies = "DependencyGraphSpec_CentralVersionDependencies.json";
        private const string Project1Json = "project1.json";
        private const string Project2Json = "project2.json";
        private const string Test1Dg = "test1.dg";
        private const string Test2Dg = "test2.dg";
        private const string Test3Dg = "test3.dg";

        private const string PackageSpecName = "x";
        private const string PackageSpecPath = @"c:\fake\project.json";

        [Fact]
        public void GetParents_WhenCalledOnChild_ReturnsParents()
        {
            // Arrange
            string jsonContent = GetResourceAsJson(Test1Dg);
            using var testDirectory = TestDirectory.Create();
            var jsonPath = Path.Combine(testDirectory.Path, "dg.json");
            File.WriteAllText(jsonPath, jsonContent);

            // Act
            var dg = DependencyGraphSpec.Load(jsonPath);

            var xParents = dg.GetParents("A55205E7-4D08-4672-8011-0925467CC45F");
            var yParents = dg.GetParents("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F");
            var zParents = dg.GetParents("44B29B8D-8413-42D2-8DF4-72225659619B");

            // Assert
            Assert.Equal(0, xParents.Count);
            Assert.Equal(1, yParents.Count);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", yParents.Single());

            Assert.Equal(1, zParents.Count);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", zParents.Single());
        }

        [Fact]
        public void GetClosure_WhenClosureExists_ReturnsClosure()
        {
            // Arrange
            string jsonContent = GetResourceAsJson(Test1Dg);
            using var testDirectory = TestDirectory.Create();
            var jsonPath = Path.Combine(testDirectory.Path, "dg.json");
            File.WriteAllText(jsonPath, jsonContent);

            // Act
            var dg = DependencyGraphSpec.Load(jsonPath);

            var xClosure = dg.GetClosure("A55205E7-4D08-4672-8011-0925467CC45F").OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.Ordinal).ToList();
            var yClosure = dg.GetClosure("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F").OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.Ordinal).ToList();
            var zClosure = dg.GetClosure("44B29B8D-8413-42D2-8DF4-72225659619B").OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.Ordinal).ToList();

            // Assert
            Assert.Equal(3, xClosure.Count);
            Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B", xClosure[0].RestoreMetadata.ProjectUniqueName);
            Assert.Equal("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F", xClosure[1].RestoreMetadata.ProjectUniqueName);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", xClosure[2].RestoreMetadata.ProjectUniqueName);

            Assert.Equal(1, yClosure.Count);
            Assert.Equal("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F", yClosure.Single().RestoreMetadata.ProjectUniqueName);

            Assert.Equal(1, zClosure.Count);
            Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B", zClosure.Single().RestoreMetadata.ProjectUniqueName);
        }

        [PlatformFact(Platform.Windows)]
        public void GetClosure_WhenClosureExistsCaseInsensitively_ReturnsClosure()
        {
            // Arrange
            string jsonContent = GetResourceAsJson(Test3Dg);
            using var testDirectory = TestDirectory.Create();
            var jsonPath = Path.Combine(testDirectory.Path, "dg.json");
            File.WriteAllText(jsonPath, jsonContent);

            // Act
            var dg = DependencyGraphSpec.Load(jsonPath);

            var xClosure = dg.GetClosure("A55205E7-4D08-4672-8011-0925467CC45F").OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.OrdinalIgnoreCase).ToList();
            var yClosure = dg.GetClosure("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F").OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.OrdinalIgnoreCase).ToList();

            // Assert
            Assert.Equal(3, xClosure.Count);
            Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B", xClosure[0].RestoreMetadata.ProjectUniqueName);
            Assert.Equal("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F", xClosure[1].RestoreMetadata.ProjectUniqueName);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", xClosure[2].RestoreMetadata.ProjectUniqueName);

            Assert.Equal(1, yClosure.Count);
            Assert.Equal("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F", yClosure.Single().RestoreMetadata.ProjectUniqueName);
        }

        [Fact]
        public void GetClosure_WhenProjectHasToolReferences_ReturnsClosure()
        {
            // Arrange
            string jsonContent = GetResourceAsJson(Test2Dg);
            using var testDirectory = TestDirectory.Create();
            var jsonPath = Path.Combine(testDirectory.Path, "dg.json");
            File.WriteAllText(jsonPath, jsonContent);

            // Act
            var dg = DependencyGraphSpec.Load(jsonPath);

            var childProject = @"f:\validation\test\dg\Project.Core\Project.Core\Project.Core.csproj";
            var parentProject = @"f:\validation\test\dg\Project.Core\Project\Project.csproj";
            var tool = @"atool-netcoreapp2.0-[1.0.0, )";

            var childClosure = dg.GetClosure(childProject).OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.Ordinal).ToList();
            var parentClosure = dg.GetClosure(parentProject).OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.Ordinal).ToList();
            var toolClosure = dg.GetClosure(tool).OrderBy(e => e.RestoreMetadata.ProjectUniqueName, StringComparer.Ordinal).ToList();

            // Assert
            Assert.Equal(2, parentClosure.Count);
            Assert.Equal(childProject, parentClosure[0].RestoreMetadata.ProjectUniqueName);
            Assert.Equal(parentProject, parentClosure[1].RestoreMetadata.ProjectUniqueName);

            Assert.Equal(1, childClosure.Count);
            Assert.Equal(childProject, childClosure.Single().RestoreMetadata.ProjectUniqueName);

            Assert.Equal(1, toolClosure.Count);
            Assert.Equal(tool, toolClosure.Single().RestoreMetadata.ProjectUniqueName);
        }

        [Fact]
        public void DefaultConstructor_Always_CreatesEmptyDgSpec()
        {
            // Arrange && Act
            var dg = new DependencyGraphSpec();

            // Assert
            Assert.Equal(0, dg.Restore.Count);
            Assert.Equal(0, dg.Projects.Count);
        }

        [Theory]
        [InlineData("")]
        [InlineData("[]")]
        public void Load_WithPath_WhenJsonIsInvalidDgSpec_Throws(string json)
        {
            using (Test test = Test.Create(json))
            {
                InvalidDataException exception = Assert.Throws<InvalidDataException>(
                    () => DependencyGraphSpec.Load(test.FilePath));

                Assert.Null(exception.InnerException);
            }
        }

        [Fact]
        public void Load_WithPath_WhenJsonStartsWithComment_SkipsComment()
        {
            var json = @"/*
*/
{
}";

            using (Test test = Test.Create(json))
            {
                DependencyGraphSpec dgSpec = DependencyGraphSpec.Load(test.FilePath);

                Assert.NotNull(dgSpec);
            }
        }

        [Fact]
        public void DependencyGraphSpec_ReadMSBuildMetadata()
        {
            // Arrange
            string json = GetResourceAsJson(Project1Json);

            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, PackageSpecName, PackageSpecPath);
            var msbuildMetadata = spec.RestoreMetadata;

            // Assert
            Assert.NotNull(msbuildMetadata);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", msbuildMetadata.ProjectUniqueName);
            Assert.Equal("c:\\x\\x.csproj", msbuildMetadata.ProjectPath);
            Assert.Equal(PackageSpecName, msbuildMetadata.ProjectName);
            Assert.Equal("c:\\x\\project.json", msbuildMetadata.ProjectJsonPath);
            Assert.Equal(ProjectStyle.PackageReference, msbuildMetadata.ProjectStyle);
            Assert.Equal("c:\\packages", msbuildMetadata.PackagesPath);
            Assert.Equal("https://api.nuget.org/v3/index.json", string.Join("|", msbuildMetadata.Sources.Select(s => s.Source)));
            Assert.Equal("c:\\fallback1|c:\\fallback2", string.Join("|", msbuildMetadata.FallbackFolders));
            Assert.Equal("c:\\nuget.config|d:\\nuget.config", string.Join("|", msbuildMetadata.ConfigFilePaths));
            Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B|c:\\a\\a.csproj|78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F|c:\\b\\b.csproj", string.Join("|", msbuildMetadata.TargetFrameworks.Single().ProjectReferences.Select(e => $"{e.ProjectUniqueName}|{e.ProjectPath}")));
            Assert.True(msbuildMetadata.CrossTargeting);
            Assert.True(msbuildMetadata.LegacyPackagesDirectory);
        }

        [Fact]
        public void DependencyGraphSpec_ReadMSBuildMetadata_WithProperDefaults()
        {
            // Arrange
            string json = GetResourceAsJson(Project2Json);

            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, PackageSpecName, PackageSpecPath);
            var msbuildMetadata = spec.RestoreMetadata;

            // Assert
            Assert.NotNull(msbuildMetadata);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", msbuildMetadata.ProjectUniqueName);
            Assert.Equal("c:\\x\\x.csproj", msbuildMetadata.ProjectPath);
            Assert.Equal(PackageSpecName, msbuildMetadata.ProjectName);
            Assert.Equal("c:\\x\\project.json", msbuildMetadata.ProjectJsonPath);
            Assert.Equal(ProjectStyle.PackageReference, msbuildMetadata.ProjectStyle);
            Assert.Equal("c:\\packages", msbuildMetadata.PackagesPath);
            Assert.Equal("https://api.nuget.org/v3/index.json", string.Join("|", msbuildMetadata.Sources.Select(s => s.Source)));
            Assert.Equal("c:\\fallback1|c:\\fallback2", string.Join("|", msbuildMetadata.FallbackFolders));
            Assert.Equal("c:\\nuget.config|e:\\nuget.config", string.Join("|", msbuildMetadata.ConfigFilePaths));
            Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B|c:\\a\\a.csproj|78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F|c:\\b\\b.csproj", string.Join("|", msbuildMetadata.TargetFrameworks.Single().ProjectReferences.Select(e => $"{e.ProjectUniqueName}|{e.ProjectPath}")));
            Assert.False(msbuildMetadata.CrossTargeting);
            Assert.False(msbuildMetadata.LegacyPackagesDirectory);
        }

        [Fact]
        public void DependencyGraphSpec_VerifyMSBuildMetadataObject()
        {
            // Arrange && Act
            var msbuildMetadata = new ProjectRestoreMetadata();

            msbuildMetadata.ProjectUniqueName = "A55205E7-4D08-4672-8011-0925467CC45F";
            msbuildMetadata.ProjectPath = "c:\\x\\x.csproj";
            msbuildMetadata.ProjectName = PackageSpecName;
            msbuildMetadata.ProjectJsonPath = "c:\\x\\project.json";
            msbuildMetadata.ProjectStyle = ProjectStyle.PackageReference;
            msbuildMetadata.PackagesPath = "c:\\packages";
            msbuildMetadata.Sources = new[] { new PackageSource("https://api.nuget.org/v3/index.json") };

            var tfmGroup = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45"));

            msbuildMetadata.TargetFrameworks.Add(tfmGroup);

            tfmGroup.ProjectReferences.Add(new ProjectRestoreReference()
            {
                ProjectUniqueName = "44B29B8D-8413-42D2-8DF4-72225659619B",
                ProjectPath = "c:\\a\\a.csproj"
            });

            tfmGroup.ProjectReferences.Add(new ProjectRestoreReference()
            {
                ProjectUniqueName = "78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F",
                ProjectPath = "c:\\b\\b.csproj"
            });

            msbuildMetadata.FallbackFolders.Add("c:\\fallback1");
            msbuildMetadata.FallbackFolders.Add("c:\\fallback2");

            msbuildMetadata.ConfigFilePaths.Add("c:\\nuget.config");
            msbuildMetadata.ConfigFilePaths.Add("d:\\nuget.config");


            // Assert
            Assert.NotNull(msbuildMetadata);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", msbuildMetadata.ProjectUniqueName);
            Assert.Equal("c:\\x\\x.csproj", msbuildMetadata.ProjectPath);
            Assert.Equal(PackageSpecName, msbuildMetadata.ProjectName);
            Assert.Equal("c:\\x\\project.json", msbuildMetadata.ProjectJsonPath);
            Assert.Equal(ProjectStyle.PackageReference, msbuildMetadata.ProjectStyle);
            Assert.Equal("c:\\packages", msbuildMetadata.PackagesPath);
            Assert.Equal("https://api.nuget.org/v3/index.json", string.Join("|", msbuildMetadata.Sources.Select(s => s.Source)));
            Assert.Equal("c:\\fallback1|c:\\fallback2", string.Join("|", msbuildMetadata.FallbackFolders));
            Assert.Equal("c:\\nuget.config|d:\\nuget.config", string.Join("|", msbuildMetadata.ConfigFilePaths));
            Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B|c:\\a\\a.csproj|78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F|c:\\b\\b.csproj", string.Join("|", msbuildMetadata.TargetFrameworks.Single().ProjectReferences.Select(e => $"{e.ProjectUniqueName}|{e.ProjectPath}")));
        }

        [Fact]
        public void DependencyGraphSpec_RoundTripMSBuildMetadata()
        {
            // Arrange
            var frameworks = new List<TargetFrameworkInformation>();
            frameworks.Add(new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            });

            var spec = new PackageSpec(frameworks);
            spec.Version = NuGetVersion.Parse("24.5.1.2-alpha.1.2+a.b.c");
            var msbuildMetadata = new ProjectRestoreMetadata();
            spec.RestoreMetadata = msbuildMetadata;

            msbuildMetadata.ProjectUniqueName = "A55205E7-4D08-4672-8011-0925467CC45F";
            msbuildMetadata.ProjectPath = "c:\\x\\x.csproj";
            msbuildMetadata.ProjectName = PackageSpecName;
            msbuildMetadata.ProjectJsonPath = "c:\\x\\project.json";
            msbuildMetadata.ProjectStyle = ProjectStyle.PackageReference;
            msbuildMetadata.PackagesPath = "c:\\packages";
            msbuildMetadata.Sources = new[] { new PackageSource("https://api.nuget.org/v3/index.json") };

            var tfmGroup = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45"));
            msbuildMetadata.TargetFrameworks.Add(tfmGroup);

            tfmGroup.ProjectReferences.Add(new ProjectRestoreReference()
            {
                ProjectUniqueName = "44B29B8D-8413-42D2-8DF4-72225659619B",
                ProjectPath = "c:\\a\\a.csproj"
            });

            tfmGroup.ProjectReferences.Add(new ProjectRestoreReference()
            {
                ProjectUniqueName = "78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F",
                ProjectPath = "c:\\b\\b.csproj"
            });

            msbuildMetadata.FallbackFolders.Add("c:\\fallback1");
            msbuildMetadata.FallbackFolders.Add("c:\\fallback2");


            msbuildMetadata.ConfigFilePaths.Add("c:\\nuget.config");
            msbuildMetadata.ConfigFilePaths.Add("d:\\nuget.config");

            msbuildMetadata.CrossTargeting = true;
            msbuildMetadata.LegacyPackagesDirectory = true;

            // Act
            PackageSpec readSpec = PackageSpecTestUtility.RoundTrip(spec, PackageSpecName, PackageSpecPath);
            ProjectRestoreMetadata msbuildMetadata2 = readSpec.RestoreMetadata;

            // Assert
            Assert.NotNull(msbuildMetadata2);
            Assert.Equal("A55205E7-4D08-4672-8011-0925467CC45F", msbuildMetadata2.ProjectUniqueName);
            Assert.Equal("c:\\x\\x.csproj", msbuildMetadata2.ProjectPath);
            Assert.Equal(PackageSpecName, msbuildMetadata2.ProjectName);
            Assert.Equal("c:\\x\\project.json", msbuildMetadata2.ProjectJsonPath);
            Assert.Equal(ProjectStyle.PackageReference, msbuildMetadata2.ProjectStyle);
            Assert.Equal("c:\\packages", msbuildMetadata2.PackagesPath);
            Assert.Equal("https://api.nuget.org/v3/index.json", string.Join("|", msbuildMetadata.Sources.Select(s => s.Source)));
            Assert.Equal("c:\\fallback1|c:\\fallback2", string.Join("|", msbuildMetadata2.FallbackFolders));
            Assert.Equal("c:\\nuget.config|d:\\nuget.config", string.Join("|", msbuildMetadata.ConfigFilePaths));
            Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B|c:\\a\\a.csproj|78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F|c:\\b\\b.csproj", string.Join("|", msbuildMetadata2.TargetFrameworks.Single().ProjectReferences.Select(e => $"{e.ProjectUniqueName}|{e.ProjectPath}")));
            Assert.True(msbuildMetadata.CrossTargeting);
            Assert.True(msbuildMetadata.LegacyPackagesDirectory);

            // Verify build metadata is not lost.
            Assert.Equal("24.5.1.2-alpha.1.2+a.b.c", readSpec.Version.ToFullString());
        }

        [Fact]
        public void DependencyGraphSpec_RoundTripMSBuildMetadata_ProjectReferenceFlags()
        {
            // Arrange
            var frameworks = new List<TargetFrameworkInformation>();
            frameworks.Add(new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            });

            var spec = new PackageSpec(frameworks);
            var msbuildMetadata = new ProjectRestoreMetadata();
            spec.RestoreMetadata = msbuildMetadata;

            msbuildMetadata.ProjectUniqueName = "A55205E7-4D08-4672-8011-0925467CC45F";
            msbuildMetadata.ProjectPath = "c:\\x\\x.csproj";
            msbuildMetadata.ProjectName = PackageSpecName;
            msbuildMetadata.ProjectStyle = ProjectStyle.PackageReference;

            var tfmGroup = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45"));
            var tfmGroup2 = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netstandard1.3"));

            msbuildMetadata.TargetFrameworks.Add(tfmGroup);
            msbuildMetadata.TargetFrameworks.Add(tfmGroup2);

            var ref1 = new ProjectRestoreReference()
            {
                ProjectUniqueName = "44B29B8D-8413-42D2-8DF4-72225659619B",
                ProjectPath = "c:\\a\\a.csproj",
                IncludeAssets = LibraryIncludeFlags.Build,
                ExcludeAssets = LibraryIncludeFlags.Compile,
                PrivateAssets = LibraryIncludeFlags.Runtime
            };

            var ref2 = new ProjectRestoreReference()
            {
                ProjectUniqueName = "78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F",
                ProjectPath = "c:\\b\\b.csproj"
            };

            tfmGroup.ProjectReferences.Add(ref1);
            tfmGroup.ProjectReferences.Add(ref2);

            tfmGroup2.ProjectReferences.Add(ref1);
            tfmGroup2.ProjectReferences.Add(ref2);

            // Act
            PackageSpec readSpec = PackageSpecTestUtility.RoundTrip(spec, PackageSpecName, PackageSpecPath);

            // Assert
            Assert.Equal(2, readSpec.RestoreMetadata.TargetFrameworks.Count);

            foreach (var framework in readSpec.RestoreMetadata.TargetFrameworks)
            {
                var references = framework.ProjectReferences.OrderBy(e => e.ProjectUniqueName).ToArray();
                Assert.Equal("44B29B8D-8413-42D2-8DF4-72225659619B", references[0].ProjectUniqueName);
                Assert.Equal(LibraryIncludeFlags.Build, references[0].IncludeAssets);
                Assert.Equal(LibraryIncludeFlags.Compile, references[0].ExcludeAssets);
                Assert.Equal(LibraryIncludeFlags.Runtime, references[0].PrivateAssets);

                Assert.Equal("78A6AD3F-9FA5-47F6-A54E-84B46A48CB2F", references[1].ProjectUniqueName);
                Assert.Equal(LibraryIncludeFlags.All, references[1].IncludeAssets);
                Assert.Equal(LibraryIncludeFlags.None, references[1].ExcludeAssets);
                Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, references[1].PrivateAssets);
            }
        }

        [Fact]
        public void Save_WithNonEmptyDgSpec_SerializesCorrectly()
        {
            string expectedJson = GetResourceAsJson("DependencyGraphSpec_Save_SerializesMembersAsJson.json");
            DependencyGraphSpec dependencyGraphSpec = CreateDependencyGraphSpec();
            string actualJson = GetJson(dependencyGraphSpec);

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void Save_WithCentralVersionDependencies_SerializesMembersAsJson()
        {
            // Arrange
            string expectedJson = GetResourceAsJson(DgSpecWithCentralDependencies);

            // Act
            DependencyGraphSpec dependencyGraphSpec = CreateDependencyGraphSpecWithCentralDependencies(CreateTargetFrameworkInformation());
            string actualJson = GetJson(dependencyGraphSpec);

            // Assert
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetHash_WithDifferentProjectPathCasings_UsesFileSystemCaseSensitivity(bool useLegacyHashFunction)
        {
            PackageSpec firstProject = CreatePackageSpecWithPaths(Path.Combine(Path.GetTempPath(), "Projects"));
            PackageSpec secondProject = CreatePackageSpecWithPaths(Path.Combine(Path.GetTempPath(), "projects"));
            DependencyGraphSpec first = ProjectTestHelpers.GetDGSpecForFirstProject(firstProject);
            DependencyGraphSpec second = ProjectTestHelpers.GetDGSpecForFirstProject(secondProject);
            string firstJson = GetJson(first);
            string secondJson = GetJson(second);

            string firstHash = GetHash(first, useLegacyHashFunction);
            string secondHash = GetHash(second, useLegacyHashFunction);

            Assert.Equal(PathUtility.IsFileSystemCaseInsensitive, StringComparer.Ordinal.Equals(firstHash, secondHash));
            Assert.Equal(firstJson, GetJson(first));
            Assert.Equal(secondJson, GetJson(second));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetHash_WithDifferentReferencedProjectPathCasings_UsesFileSystemCaseSensitivity(bool useLegacyHashFunction)
        {
            PackageSpec firstRoot = CreatePackageSpecWithPaths(Path.Combine(Path.GetTempPath(), "Root"));
            PackageSpec secondRoot = firstRoot.Clone();
            PackageSpec firstChild = CreatePackageSpecWithPaths(Path.Combine(Path.GetTempPath(), "Referenced"), "Child");
            PackageSpec secondChild = CreatePackageSpecWithPaths(Path.Combine(Path.GetTempPath(), "referenced"), "Child");
            firstRoot = firstRoot.WithTestProjectReference(firstChild);
            secondRoot = secondRoot.WithTestProjectReference(secondChild);
            DependencyGraphSpec first = ProjectTestHelpers.GetDGSpecForFirstProject(firstRoot, firstChild);
            DependencyGraphSpec second = ProjectTestHelpers.GetDGSpecForFirstProject(secondRoot, secondChild);

            DependencyGraphSpec firstClosure = first.WithProjectClosure(firstRoot.RestoreMetadata.ProjectUniqueName);
            DependencyGraphSpec secondClosure = second.WithProjectClosure(secondRoot.RestoreMetadata.ProjectUniqueName);
            Assert.Equal(2, firstClosure.Projects.Count);
            Assert.Equal(2, secondClosure.Projects.Count);
            string firstJson = GetJson(firstClosure);
            string secondJson = GetJson(secondClosure);

            string firstHash = GetHash(firstClosure, useLegacyHashFunction);
            string secondHash = GetHash(secondClosure, useLegacyHashFunction);

            Assert.Equal(PathUtility.IsFileSystemCaseInsensitive, StringComparer.Ordinal.Equals(firstHash, secondHash));
            Assert.Equal(firstJson, GetJson(firstClosure));
            Assert.Equal(secondJson, GetJson(secondClosure));
        }

        [Theory]
        [InlineData(false, "ProjectName")]
        [InlineData(true, "ProjectName")]
        [InlineData(false, "Source")]
        [InlineData(true, "Source")]
        [InlineData(false, "PackagePath")]
        [InlineData(true, "PackagePath")]
        [InlineData(false, "OutputPath")]
        [InlineData(true, "OutputPath")]
        [InlineData(false, "DependencyVersion")]
        [InlineData(true, "DependencyVersion")]
        [InlineData(false, "ProjectJsonPath")]
        [InlineData(true, "ProjectJsonPath")]
        [InlineData(false, "PackagesPath")]
        [InlineData(true, "PackagesPath")]
        [InlineData(false, "ConfigFilePath")]
        [InlineData(true, "ConfigFilePath")]
        [InlineData(false, "FallbackFolder")]
        [InlineData(true, "FallbackFolder")]
        [InlineData(false, "LockFilePath")]
        [InlineData(true, "LockFilePath")]
        [InlineData(false, "RuntimeIdentifierGraphPath")]
        [InlineData(true, "RuntimeIdentifierGraphPath")]
        [InlineData(false, "AbsolutePath")]
        [InlineData(true, "AbsolutePath")]
        public void GetHash_WithOtherRestoreInputChanges_ReturnsDifferentHashes(bool useLegacyHashFunction, string changedValue)
        {
            PackageSpec first = CreatePackageSpecWithPaths(Path.Combine(Path.GetTempPath(), "Projects"));
            PackageSpec second = first.Clone();

            switch (changedValue)
            {
                case "ProjectName":
                    second.RestoreMetadata.ProjectName = "project";
                    break;
                case "Source":
                    second.RestoreMetadata.Sources = [new PackageSource("https://example.test/feed/index.json")];
                    break;
                case "PackagePath":
                    second.RestoreMetadata.Files = [new ProjectRestoreMetadataFile("lib/net10.0/project.dll", first.RestoreMetadata.Files[0].AbsolutePath)];
                    break;
                case "OutputPath":
                    second.RestoreMetadata.OutputPath = Path.Combine(Path.GetTempPath(), "OtherOutput");
                    break;
                case "DependencyVersion":
                    second.TargetFrameworks[0] = new TargetFrameworkInformation(second.TargetFrameworks[0])
                    {
                        Dependencies = [new LibraryDependency
                        {
                            LibraryRange = new LibraryRange("Package", VersionRange.Parse("2.0.0"), LibraryDependencyTarget.Package)
                        }]
                    };
                    break;
                case "ProjectJsonPath":
                    second.RestoreMetadata.ProjectJsonPath = second.RestoreMetadata.ProjectJsonPath.ToUpperInvariant();
                    break;
                case "PackagesPath":
                    second.RestoreMetadata.PackagesPath = second.RestoreMetadata.PackagesPath.ToUpperInvariant();
                    break;
                case "ConfigFilePath":
                    second.RestoreMetadata.ConfigFilePaths[0] = second.RestoreMetadata.ConfigFilePaths[0].ToUpperInvariant();
                    break;
                case "FallbackFolder":
                    second.RestoreMetadata.FallbackFolders[0] = second.RestoreMetadata.FallbackFolders[0].ToUpperInvariant();
                    break;
                case "LockFilePath":
                    second.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                        "true", second.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath.ToUpperInvariant(), false);
                    break;
                case "RuntimeIdentifierGraphPath":
                    second.TargetFrameworks[0] = new TargetFrameworkInformation(second.TargetFrameworks[0])
                    {
                        RuntimeIdentifierGraphPath = second.TargetFrameworks[0].RuntimeIdentifierGraphPath.ToUpperInvariant()
                    };
                    break;
                case "AbsolutePath":
                    second.RestoreMetadata.Files = [new ProjectRestoreMetadataFile(
                        first.RestoreMetadata.Files[0].PackagePath, first.RestoreMetadata.Files[0].AbsolutePath.ToUpperInvariant())];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(changedValue));
            }

            string firstHash = GetHash(ProjectTestHelpers.GetDGSpecForFirstProject(first), useLegacyHashFunction);
            string secondHash = GetHash(ProjectTestHelpers.GetDGSpecForFirstProject(second), useLegacyHashFunction);

            Assert.NotEqual(firstHash, secondHash);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetHash_WithCentralPackageVersionsAndPackagesToPrune_IgnoresDictionaryCreationOrder(bool useLegacyHashFunction)
        {
            // Arrange
            DependencyGraphSpec first = CreateDependencyGraphSpecWithCentralDependenciesAndPruning(
                centralPackageVersionNames: ["PackageB", "PackageA"],
                packagesToPruneNames: ["TransitiveB", "TransitiveA"]);

            DependencyGraphSpec second = CreateDependencyGraphSpecWithCentralDependenciesAndPruning(
                centralPackageVersionNames: ["PackageA", "PackageB"],
                packagesToPruneNames: ["TransitiveA", "TransitiveB"]);

            // Act
            string firstHash = GetHash(first, useLegacyHashFunction);
            string secondHash = GetHash(second, useLegacyHashFunction);

            // Assert
            Assert.Equal(firstHash, secondHash);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetHash_WithPackagesToPruneInDifferentJsonOrder_IgnoresInputOrder(bool useLegacyHashFunction)
        {
            // Arrange
            DependencyGraphSpec first = CreateDependencyGraphSpecFromJson(@"
            {
              ""restore"": {
                ""projectUniqueName"": ""Project1"",
                ""centralPackageVersionsManagementEnabled"": true
              },
              ""frameworks"": {
                ""net8.0"": {
                  ""dependencies"": {
                    ""PackageA"": {
                      ""target"": ""Package"",
                      ""version"": ""[1.0.0,)"",
                      ""versionCentrallyManaged"": true
                    }
                  },
                  ""centralPackageVersions"": {
                    ""PackageA"": ""[1.0.0]"",
                    ""PackageB"": ""[2.0.0]""
                  },
                  ""packagesToPrune"": {
                    ""TransitiveB"": ""(,2.0.0]"",
                    ""TransitiveA"": ""(,1.0.0]""
                  }
                }
              }
            }");

            DependencyGraphSpec second = CreateDependencyGraphSpecFromJson(@"
            {
              ""restore"": {
                ""projectUniqueName"": ""Project1"",
                ""centralPackageVersionsManagementEnabled"": true
              },
              ""frameworks"": {
                ""net8.0"": {
                  ""dependencies"": {
                    ""PackageA"": {
                      ""target"": ""Package"",
                      ""version"": ""[1.0.0,)"",
                      ""versionCentrallyManaged"": true
                    }
                  },
                  ""centralPackageVersions"": {
                    ""PackageA"": ""[1.0.0]"",
                    ""PackageB"": ""[2.0.0]""
                  },
                  ""packagesToPrune"": {
                    ""TransitiveA"": ""(,1.0.0]"",
                    ""TransitiveB"": ""(,2.0.0]""
                  }
                }
              }
            }");

            // Act
            string firstHash = GetHash(first, useLegacyHashFunction);
            string secondHash = GetHash(second, useLegacyHashFunction);

            // Assert
            Assert.Equal(firstHash, secondHash);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetHash_WithCentralPackageVersionsInDifferentJsonOrder_IgnoresInputOrder(bool useLegacyHashFunction)
        {
            // Arrange
            DependencyGraphSpec first = CreateDependencyGraphSpecFromJson(@"
            {
              ""restore"": {
                ""projectUniqueName"": ""Project1"",
                ""centralPackageVersionsManagementEnabled"": true
              },
              ""frameworks"": {
                ""net8.0"": {
                  ""dependencies"": {
                    ""PackageA"": {
                      ""target"": ""Package"",
                      ""version"": ""[1.0.0,)"",
                      ""versionCentrallyManaged"": true
                    }
                  },
                  ""centralPackageVersions"": {
                    ""PackageB"": ""[2.0.0]"",
                    ""PackageA"": ""[1.0.0]""
                  },
                  ""packagesToPrune"": {
                    ""TransitiveA"": ""(,1.0.0]"",
                    ""TransitiveB"": ""(,2.0.0]""
                  }
                }
              }
            }");

            DependencyGraphSpec second = CreateDependencyGraphSpecFromJson(@"
            {
              ""restore"": {
                ""projectUniqueName"": ""Project1"",
                ""centralPackageVersionsManagementEnabled"": true
              },
              ""frameworks"": {
                ""net8.0"": {
                  ""dependencies"": {
                    ""PackageA"": {
                      ""target"": ""Package"",
                      ""version"": ""[1.0.0,)"",
                      ""versionCentrallyManaged"": true
                    }
                  },
                  ""centralPackageVersions"": {
                    ""PackageA"": ""[1.0.0]"",
                    ""PackageB"": ""[2.0.0]""
                  },
                  ""packagesToPrune"": {
                    ""TransitiveA"": ""(,1.0.0]"",
                    ""TransitiveB"": ""(,2.0.0]""
                  }
                }
              }
            }");

            // Act
            string firstHash = GetHash(first, useLegacyHashFunction);
            string secondHash = GetHash(second, useLegacyHashFunction);

            // Assert
            Assert.Equal(firstHash, secondHash);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetHash_WithDifferentRestoreEnableAnalyzerAssetsValues_ReturnsDifferentHashes(bool useLegacyHashFunction)
        {
            // Arrange
            DependencyGraphSpec first = CreateDependencyGraphSpec();
            DependencyGraphSpec second = CreateDependencyGraphSpec();
            first.GetProjectSpec("a").TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = NuGetFramework.Parse("net5.0") });
            second.GetProjectSpec("a").TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = NuGetFramework.Parse("net5.0") });
            second.GetProjectSpec("a").RestoreMetadata.RestoreEnableAnalyzerAssets = true;

            // Act
            string firstHash = GetHash(first, useLegacyHashFunction);
            string secondHash = GetHash(second, useLegacyHashFunction);

            // Assert
            Assert.NotEqual(firstHash, secondHash);
        }

        [Fact]
        public void AddProject_WhenRestoreMetadataIsNull_AddsProject()
        {
            var expectedResult = new PackageSpec();
            var dgSpec = new DependencyGraphSpec();

            dgSpec.AddProject(expectedResult);

            Assert.Collection(
                dgSpec.Projects,
                actualResult => Assert.Same(expectedResult, actualResult));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddProject_DoesNotClone(bool cpvmEnabled)
        {
            // Arrange
            var dependencyFoo = new LibraryDependency()
            {
                LibraryRange = new LibraryRange("foo", versionRange: cpvmEnabled ? null : VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package),
                VersionCentrallyManaged = cpvmEnabled
            };

            var centralVersions = cpvmEnabled
                ? new List<CentralPackageVersion>() { new CentralPackageVersion("foo", VersionRange.Parse("1.0.0")) }
                : new List<CentralPackageVersion>();

            var tfi = CreateTargetFrameworkInformation(
                [dependencyFoo],
                centralVersions);

            var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { tfi });
            packageSpec.RestoreMetadata = new ProjectRestoreMetadata()
            {
                ProjectUniqueName = "a",
                CentralPackageVersionsEnabled = cpvmEnabled
            };

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore("a");
            dgSpec.AddProject(packageSpec);

            // Act
            var packageSpecFromDGSpec = dgSpec.GetProjectSpec("a");

            // Assert
            Assert.True(packageSpec.Equals(packageSpecFromDGSpec));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateFromClosure_WhenProjectUniqueNameIsNullOrEmpty_Throws(string projectUniqueName)
        {
            IReadOnlyList<PackageSpec> closure = new[] { new PackageSpec() };
            var dgSpec = new DependencyGraphSpec();

            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => dgSpec.CreateFromClosure(projectUniqueName, closure));

            Assert.Equal("projectUniqueName", exception.ParamName);
        }

        [Fact]
        public void CreateFromClosure_WhenClosureIsNull_Throws()
        {
            var dgSpec = new DependencyGraphSpec();

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => dgSpec.CreateFromClosure(PackageSpecName, closure: null));

            Assert.Equal("closure", exception.ParamName);
        }

        [Fact]
        public void CreateFromClosure_WhenReadOnlyIsTrue_ReturnsSameClosure()
        {
            var expectedResult = new PackageSpec()
            {
                RestoreMetadata = new ProjectRestoreMetadata()
            };
            IReadOnlyList<PackageSpec> closure = new[] { expectedResult };
            var dgSpec = new DependencyGraphSpec(isReadOnly: true);

            DependencyGraphSpec newDgSpec = dgSpec.CreateFromClosure(PackageSpecName, closure);

            Assert.Collection(
                newDgSpec.Restore,
                actualResult => Assert.Equal(PackageSpecName, actualResult));

            Assert.Collection(
                newDgSpec.Projects,
                actualResult => Assert.Same(expectedResult, actualResult));
        }

        [Fact]
        public void CreateFromClosure_WhenReadOnlyIsFalse_ReturnsClonedClosure()
        {
            var expectedResult = new PackageSpec()
            {
                RestoreMetadata = new ProjectRestoreMetadata()
            };
            IReadOnlyList<PackageSpec> closure = new[] { expectedResult };
            var dgSpec = new DependencyGraphSpec(isReadOnly: false);

            DependencyGraphSpec newDgSpec = dgSpec.CreateFromClosure(PackageSpecName, closure);

            Assert.Collection(
                newDgSpec.Restore,
                actualResult => Assert.Equal(PackageSpecName, actualResult));

            Assert.Collection(
                newDgSpec.Projects,
                actualResult =>
                {
                    Assert.True(expectedResult.Equals(actualResult));
                    Assert.NotSame(expectedResult, actualResult);
                });
        }

        private static PackageSpec CreatePackageSpecWithPaths(string directory, string projectName = "Project")
        {
            string projectPath = Path.Combine(directory, projectName + ".csproj");
            string restoreSettingsDirectory = Path.Combine(Path.GetTempPath(), "RestoreSettings");
            var framework = new TargetFrameworkInformation
            {
                FrameworkName = NuGetFramework.Parse("net10.0"),
                RuntimeIdentifierGraphPath = Path.Combine(restoreSettingsDirectory, "RuntimeIdentifierGraph.json"),
                Dependencies = [new LibraryDependency
                {
                    LibraryRange = new LibraryRange("Package", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                }]
            };

            return new PackageSpec([framework])
            {
                Name = projectName,
                FilePath = projectPath,
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    ProjectUniqueName = projectPath,
                    ProjectName = projectName,
                    ProjectPath = projectPath,
                    ProjectJsonPath = Path.Combine(restoreSettingsDirectory, "project.json"),
                    OutputPath = Path.Combine(directory, "obj"),
                    PackagesPath = Path.Combine(restoreSettingsDirectory, "Packages"),
                    ConfigFilePaths = [Path.Combine(restoreSettingsDirectory, "NuGet.Config")],
                    FallbackFolders = [Path.Combine(restoreSettingsDirectory, "Fallback")],
                    Sources = [new PackageSource("https://example.test/Feed/index.json")],
                    TargetFrameworks = [new ProjectRestoreMetadataFrameworkInfo(framework.FrameworkName)],
                    Files = [new ProjectRestoreMetadataFile("lib/net10.0/Project.dll", Path.Combine(restoreSettingsDirectory, "bin", "Project.dll"))],
                    RestoreLockProperties = new RestoreLockProperties("true", Path.Combine(restoreSettingsDirectory, "packages.lock.json"), false)
                }
            };
        }

        private static DependencyGraphSpec CreateDependencyGraphSpec()
        {
            var dgSpec = new DependencyGraphSpec();

            dgSpec.AddRestore("b");
            dgSpec.AddRestore("a");
            dgSpec.AddRestore("c");

            dgSpec.AddProject(new PackageSpec() { RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = "b" } });
            dgSpec.AddProject(new PackageSpec() { RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = "a" } });
            dgSpec.AddProject(new PackageSpec() { RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = "c" } });

            return dgSpec;
        }

        private static DependencyGraphSpec CreateDependencyGraphSpecWithCentralDependencies(params TargetFrameworkInformation[] tfis)
        {
            var packageSpec = new PackageSpec(tfis);
            packageSpec.RestoreMetadata = new ProjectRestoreMetadata() { ProjectUniqueName = "a", CentralPackageVersionsEnabled = true };

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddRestore("a");
            dgSpec.AddProject(packageSpec);
            return dgSpec;
        }

        private static DependencyGraphSpec CreateDependencyGraphSpecWithCentralDependenciesAndPruning(
            IReadOnlyList<string> centralPackageVersionNames,
            IReadOnlyList<string> packagesToPruneNames)
        {
            string centralPackageVersions = string.Join(
                $",{Environment.NewLine}",
                centralPackageVersionNames.Select(packageId => $@"                    ""{packageId}"": ""[1.0.0]"""));
            string packagesToPrune = string.Join(
                $",{Environment.NewLine}",
                packagesToPruneNames.Select(packageId => $@"                    ""{packageId}"": ""(,1.0.0]"""));

            string rootProject = $@"
            {{
              ""restore"": {{
                ""projectUniqueName"": ""Project1"",
                ""centralPackageVersionsManagementEnabled"": true
              }},
              ""frameworks"": {{
                ""net8.0"": {{
                  ""dependencies"": {{
                    ""PackageA"": {{
                      ""version"": ""[1.0.0,)"",
                      ""target"": ""Package"",
                      ""versionCentrallyManaged"": true
                    }}
                  }},
                  ""centralPackageVersions"": {{
{centralPackageVersions}
                  }},
                  ""packagesToPrune"": {{
{packagesToPrune}
                  }}
                }}
              }}
            }}";

            return CreateDependencyGraphSpecFromJson(rootProject);
        }

        private static TargetFrameworkInformation CreateTargetFrameworkInformation()
        {
            var nugetFramework = new NuGetFramework("net40");

            var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("1.0.0"));
            var centralVersionBar = new CentralPackageVersion("bar", VersionRange.Parse("2.0.0"));

            var dependencyFoo = new LibraryDependency(
                new LibraryRange("foo", versionRange: centralVersionFoo.VersionRange, LibraryDependencyTarget.Package),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                noWarn: [],
                autoReferenced: false,
                generatePathProperty: true,
                versionCentrallyManaged: true,
                LibraryDependencyReferenceType.Direct,
                aliases: null,
                versionOverride: null);

            var assetTargetFallback = true;
            var warn = false;

            var centralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase)
            {
                {centralVersionFoo.Name, centralVersionFoo },
                { centralVersionBar.Name, centralVersionBar },
            };

            ImmutableArray<LibraryDependency> dependencies = [dependencyFoo];

            var tfi = new TargetFrameworkInformation()
            {
                AssetTargetFallback = assetTargetFallback,
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                Warn = warn,
                FrameworkName = nugetFramework,
            };

            return tfi;
        }

        private static TargetFrameworkInformation CreateTargetFrameworkInformation(ImmutableArray<LibraryDependency> dependencies, List<CentralPackageVersion> centralVersionsDependencies)
        {
            var nugetFramework = new NuGetFramework("net40");
            var centralPackageVersions = centralVersionsDependencies.ToDictionary(cvd => cvd.Name, StringComparer.OrdinalIgnoreCase);

            var tfi = new TargetFrameworkInformation()
            {
                AssetTargetFallback = true,
                CentralPackageVersions = centralPackageVersions,
                Dependencies = dependencies,
                FrameworkName = nugetFramework,
                Warn = false,
            };

            return tfi;
        }

        private static DependencyGraphSpec CreateDependencyGraphSpecFromJson(string json)
        {
            PackageSpec packageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", @"c:\", json);
            return ProjectTestHelpers.GetDGSpecForFirstProject(packageSpec);
        }

        private static string GetHash(DependencyGraphSpec dgSpec, bool useLegacyHashFunction)
        {
            return useLegacyHashFunction
                ? dgSpec.GetHash(() => new Sha512HashFunction())
                : dgSpec.GetHash(() => new FnvHash64Function());
        }

        private static string GetJson(DependencyGraphSpec dgSpec)
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                string filePath = Path.Combine(testDirectory.Path, "out.json");

                dgSpec.Save(filePath);

                return File.ReadAllText(filePath);
            }
        }

        private static string GetResourceAsJson(string fileName)
        {
            var resourceName = $"NuGet.ProjectModel.Test.compiler.resources.{fileName}";

            return ResourceTestUtility.GetResource(resourceName, typeof(DependencyGraphSpecTests));
        }

        private sealed class Test : IDisposable
        {
            internal static string DefaultFileName = "dg.spec";

            internal TestDirectory Directory { get; }
            internal string FilePath { get; }

            private bool _isDisposed;

            private Test(TestDirectory directory, string filePath)
            {
                Directory = directory;
                FilePath = filePath;
            }

            internal static Test Create(string json = null)
            {
                TestDirectory directory = TestDirectory.Create();
                string filePath = Path.Combine(directory.Path, DefaultFileName);

                File.WriteAllText(filePath, json ?? string.Empty);

                return new Test(directory, filePath);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Directory.Dispose();

                    _isDisposed = true;
                }
            }
        }
    }
}
