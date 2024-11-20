// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace ProjectManagement.Test
{
    public class BuildIntegratedNuGetProjectTests
    {
        [Fact]
        public async Task BuildIntegratedNuGetProject_GetPackageSpecForRestore_NoReferences()
        {
            // Arrange
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var projectJsonPath = Path.Combine(randomProjectFolderPath, "project.json");
                CreateConfigJson(projectJsonPath);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(projectJsonPath, projectFilePath);

                var referenceContext = new DependencyGraphCacheContext(new TestLogger(), NullSettings.Instance);

                // Act
                var actual = (await buildIntegratedProject.GetPackageSpecsAsync(referenceContext)).SingleOrDefault();

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(msBuildNuGetProjectSystem.ProjectName, actual.Name);
                Assert.Equal(projectJsonPath, actual.FilePath);
                Assert.NotNull(actual.RestoreMetadata);
                Assert.Equal(ProjectStyle.ProjectJson, actual.RestoreMetadata.ProjectStyle);
                Assert.Equal(projectFilePath, actual.RestoreMetadata.ProjectPath);
                Assert.Equal(msBuildNuGetProjectSystem.ProjectName, actual.RestoreMetadata.ProjectName);
                Assert.Equal(projectFilePath, actual.RestoreMetadata.ProjectUniqueName);
                Assert.Equal(1, actual.TargetFrameworks.Count);
                Assert.Equal(projectTargetFramework, actual.TargetFrameworks[0].FrameworkName);
                Assert.Empty(actual.TargetFrameworks[0].Imports);

                Assert.Empty(actual.Dependencies);
                Assert.Empty(actual.TargetFrameworks[0].Dependencies);
                Assert.Empty(actual.RestoreMetadata.TargetFrameworks.SelectMany(e => e.ProjectReferences));
            }
        }

        [Fact]
        public void BuildIntegratedNuGetProject_SortDependenciesWithProjects()
        {
            // Arrange
            var lockFile = new LockFile();
            var target = new LockFileTarget();
            lockFile.Targets.Add(target);

            var targetA = new LockFileTargetLibrary()
            {
                Name = "a",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };
            targetA.Dependencies.Add(new PackageDependency("b"));

            var targetB = new LockFileTargetLibrary()
            {
                Name = "b",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "project"
            };
            targetA.Dependencies.Add(new PackageDependency("c"));

            var targetC = new LockFileTargetLibrary()
            {
                Name = "c",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };

            target.Libraries.Add(targetA);
            target.Libraries.Add(targetC);
            target.Libraries.Add(targetB);

            // Act
            var ordered = BuildIntegratedProjectUtility.GetOrderedLockFileDependencies(lockFile)
                .OrderBy(lib => lib.Name, StringComparer.Ordinal)
                .ToList();

            // Assert
            Assert.Equal(3, ordered.Count);
            Assert.Equal("a", ordered[0].Name);
            Assert.Equal("b", ordered[1].Name);
            Assert.Equal("c", ordered[2].Name);
        }

        [Fact]
        public void BuildIntegratedNuGetProject_SortDependenciesWithProjects_GetPackagesOnly()
        {
            // Arrange
            var lockFile = new LockFile();
            var target = new LockFileTarget();
            lockFile.Targets.Add(target);

            var targetA = new LockFileTargetLibrary()
            {
                Name = "a",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };
            targetA.Dependencies.Add(new PackageDependency("b"));

            var targetB = new LockFileTargetLibrary()
            {
                Name = "b",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "project"
            };
            targetA.Dependencies.Add(new PackageDependency("c"));

            var targetC = new LockFileTargetLibrary()
            {
                Name = "c",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            };

            target.Libraries.Add(targetA);
            target.Libraries.Add(targetC);
            target.Libraries.Add(targetB);

            // Act
            var ordered = BuildIntegratedProjectUtility.GetOrderedLockFilePackageDependencies(lockFile)
                .OrderBy(lib => lib.Id, StringComparer.Ordinal)
                .ToList();

            // Assert
            Assert.Equal(2, ordered.Count);
            Assert.Equal("a", ordered[0].Id);
            // skip b
            Assert.Equal("c", ordered[1].Id);
        }

        [Fact]
        public async Task TestBuildIntegratedNuGetPackageSpecNameMatchesFilePath_ProjectNameJson()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
            using (var randomTestPackageSourcePath = TestDirectory.Create())
            using (var randomPackagesFolderPath = TestDirectory.Create())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var randomConfig = Path.Combine(randomProjectFolderPath, "fileName.project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();

                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                    projectTargetFramework,
                    testNuGetProjectContext,
                    randomProjectFolderPath,
                    "msbuildName");

                var projectFilePath = Path.Combine(randomProjectFolderPath, "fileName.csproj");

                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var spec = await buildIntegratedProject.GetPackageSpecsAsync(new DependencyGraphCacheContext());

                // Assert
                Assert.Equal(projectFilePath, buildIntegratedProject.MSBuildProjectPath);
                Assert.Equal("fileName", buildIntegratedProject.ProjectName);
                Assert.Equal("fileName", spec.Single().Name);
            }
        }

        [Fact]
        public async Task TestBuildIntegratedNuGetPackageSpecNameMatchesFilePath()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
            using (var randomTestPackageSourcePath = TestDirectory.Create())
            using (var randomPackagesFolderPath = TestDirectory.Create())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();

                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                    projectTargetFramework,
                    testNuGetProjectContext,
                    randomProjectFolderPath,
                    "msbuildName");

                var projectFilePath = Path.Combine(randomProjectFolderPath, "fileName.csproj");

                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var spec = await buildIntegratedProject.GetPackageSpecsAsync(new DependencyGraphCacheContext());

                // Assert
                Assert.Equal(projectFilePath, buildIntegratedProject.MSBuildProjectPath);
                Assert.Equal("fileName", buildIntegratedProject.ProjectName);
                Assert.Equal("fileName", spec.Single().Name);
            }
        }

        [Fact]
        public async Task TestBuildIntegratedNuGetProjectInstallPackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
            using (var randomTestPackageSourcePath = TestDirectory.Create())
            using (var randomPackagesFolderPath = TestDirectory.Create())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyContentPackage(randomTestPackageSourcePath,
                    packageIdentity.Id, packageIdentity.Version.ToNormalizedString());
                using (var packageStream = GetDownloadResourceResult(randomTestPackageSourcePath.Path, packageFileInfo))
                {
                    // Act
                    await buildIntegratedProject.InstallPackageAsync(
                        packageIdentity.Id,
                        new VersionRange(packageIdentity.Version),
                        testNuGetProjectContext,
                        installationContext: null,
                        token: token);
                }

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(randomConfig));
                // Check the number of packages and packages returned by project after the installation
                var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackages.Count);
                Assert.Equal(packageIdentity, installedPackages[0].PackageIdentity);
            }
        }

        [Fact]
        public async Task TestBuildIntegratedNuGetProjectUninstallPackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
            var packageIdentity2 = new PackageIdentity("packageB", new NuGetVersion("1.0.0"));
            using (var randomTestPackageSourcePath = TestDirectory.Create())
            using (var randomPackagesFolderPath = TestDirectory.Create())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyContentPackage(randomTestPackageSourcePath,
                    packageIdentity.Id, packageIdentity.Version.ToNormalizedString());
                var packageFileInfo2 = TestPackagesGroupedByFolder.GetLegacyContentPackage(randomTestPackageSourcePath,
                    packageIdentity.Id, packageIdentity.Version.ToNormalizedString());
                using (var packageStream = GetDownloadResourceResult(randomTestPackageSourcePath.Path, packageFileInfo))
                {
                    await buildIntegratedProject.InstallPackageAsync(
                        packageIdentity.Id,
                        new VersionRange(packageIdentity.Version),
                        testNuGetProjectContext,
                        installationContext: null,
                        token: token);
                    await buildIntegratedProject.InstallPackageAsync(
                        packageIdentity2.Id,
                        new VersionRange(packageIdentity2.Version),
                        testNuGetProjectContext,
                        installationContext: null,
                        token: token);

                    // Act
                    await buildIntegratedProject.UninstallPackageAsync(packageIdentity2, new TestNuGetProjectContext(), CancellationToken.None);
                }

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(randomConfig));
                // Check the number of packages and packages returned by project after the installation
                var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackages.Count);
                Assert.Equal(packageIdentity, installedPackages[0].PackageIdentity);
            }
        }

        [Fact]
        public async Task TestBuildIntegratedNuGetProjectUninstallAllPackages()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
            var packageIdentity2 = new PackageIdentity("packageB", new NuGetVersion("1.0.0"));
            using (var randomTestPackageSourcePath = TestDirectory.Create())
            using (var randomPackagesFolderPath = TestDirectory.Create())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyContentPackage(randomTestPackageSourcePath,
                    packageIdentity.Id, packageIdentity.Version.ToNormalizedString());
                var packageFileInfo2 = TestPackagesGroupedByFolder.GetLegacyContentPackage(randomTestPackageSourcePath,
                    packageIdentity.Id, packageIdentity.Version.ToNormalizedString());
                using (var packageStream = GetDownloadResourceResult(randomTestPackageSourcePath.Path, packageFileInfo))
                {
                    await buildIntegratedProject.InstallPackageAsync(
                        packageIdentity.Id,
                        new VersionRange(packageIdentity.Version),
                        testNuGetProjectContext,
                        installationContext: null,
                        token: token);
                    await buildIntegratedProject.InstallPackageAsync(
                        packageIdentity2.Id,
                        new VersionRange(packageIdentity2.Version),
                        testNuGetProjectContext,
                        installationContext: null,
                        token: token);

                    // Act
                    await buildIntegratedProject.UninstallPackageAsync(packageIdentity2, new TestNuGetProjectContext(), CancellationToken.None);
                    await buildIntegratedProject.UninstallPackageAsync(packageIdentity, new TestNuGetProjectContext(), CancellationToken.None);
                }

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(randomConfig));
                // Check the number of packages and packages returned by project after the installation
                var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, installedPackages.Count);
            }
        }

        [Fact]
        public async Task BuildIntegratedNuGetProject_GetPackageSpec_WithImports()
        {
            // Arrange

            var configJson = @"
                {
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                        ""dotnet"": {
                            ""imports"": ""portable-net452+win81"",
                            ""warn"": false
                        }
                    }
                }";

            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var projectJsonPath = Path.Combine(randomProjectFolderPath, "project.json");
                File.WriteAllText(projectJsonPath, configJson);

                var FallbackTargetFramework = NuGetFramework.Parse("portable-net452+win81");
                var projectTargetFramework = NuGetFramework.Parse("dotnet");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(projectJsonPath, projectFilePath);

                var referenceContext = new DependencyGraphCacheContext(new TestLogger(), NullSettings.Instance);

                // Act
                var actual = (await buildIntegratedProject.GetPackageSpecsAsync(referenceContext)).SingleOrDefault();

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(msBuildNuGetProjectSystem.ProjectName, actual.Name);
                Assert.Equal(projectJsonPath, actual.FilePath);
                Assert.NotNull(actual.RestoreMetadata);
                Assert.Equal(ProjectStyle.ProjectJson, actual.RestoreMetadata.ProjectStyle);
                Assert.Equal(projectFilePath, actual.RestoreMetadata.ProjectPath);
                Assert.Equal(msBuildNuGetProjectSystem.ProjectName, actual.RestoreMetadata.ProjectName);
                Assert.Equal(projectFilePath, actual.RestoreMetadata.ProjectUniqueName);
                Assert.Equal(1, actual.TargetFrameworks.Count);
                Assert.Equal(projectTargetFramework, actual.TargetFrameworks[0].FrameworkName);
                Assert.Equal(1, actual.TargetFrameworks[0].Imports.Length);
                Assert.Equal(FallbackTargetFramework, actual.TargetFrameworks[0].Imports[0]);

                Assert.Empty(actual.Dependencies);
                Assert.Empty(actual.TargetFrameworks[0].Dependencies);
                Assert.Empty(actual.RestoreMetadata.TargetFrameworks.SelectMany(e => e.ProjectReferences));
            }
        }

        [Fact]
        public async Task BuildIntegratedNuGetProject_GetPackageSpec_UAPWithImports()
        {
            // Arrange

            var configJson = @"
                {
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                        ""uap10.0"": {
                            ""imports"": ""netstandard1.3"",
                            ""warn"": false
                        }
                    }
                }";

            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var projectJsonPath = Path.Combine(randomProjectFolderPath, "project.json");
                File.WriteAllText(projectJsonPath, configJson);

                var FallbackTargetFramework = NuGetFramework.Parse("netstandard1.3");
                var projectTargetFramework = NuGetFramework.Parse("uap10.0");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(projectJsonPath, projectFilePath);

                var referenceContext = new DependencyGraphCacheContext(new TestLogger(), NullSettings.Instance);

                // Act
                var actual = (await buildIntegratedProject.GetPackageSpecsAsync(referenceContext)).SingleOrDefault();

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(msBuildNuGetProjectSystem.ProjectName, actual.Name);
                Assert.Equal(projectJsonPath, actual.FilePath);
                Assert.NotNull(actual.RestoreMetadata);
                Assert.Equal(ProjectStyle.ProjectJson, actual.RestoreMetadata.ProjectStyle);
                Assert.Equal(projectFilePath, actual.RestoreMetadata.ProjectPath);
                Assert.Equal(msBuildNuGetProjectSystem.ProjectName, actual.RestoreMetadata.ProjectName);
                Assert.Equal(projectFilePath, actual.RestoreMetadata.ProjectUniqueName);
                Assert.Equal(1, actual.TargetFrameworks.Count);
                Assert.Equal(projectTargetFramework, actual.TargetFrameworks[0].FrameworkName);
                Assert.Equal(1, actual.TargetFrameworks[0].Imports.Length);
                Assert.Equal(FallbackTargetFramework, actual.TargetFrameworks[0].Imports[0]);

                Assert.Empty(actual.Dependencies);
                Assert.Empty(actual.TargetFrameworks[0].Dependencies);
                Assert.Empty(actual.RestoreMetadata.TargetFrameworks.SelectMany(e => e.ProjectReferences));
            }
        }

        private static void CreateConfigJson(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(BasicConfig.ToString());
            }
        }

        private static DownloadResourceResult GetDownloadResourceResult(string source, FileInfo fileInfo)
        {
            return new DownloadResourceResult(fileInfo.OpenRead(), source);
        }

        private static JObject BasicConfig
        {
            get
            {
                var json = new JObject();

                var frameworks = new JObject();
                frameworks["netcore50"] = new JObject();

                json["frameworks"] = frameworks;

                json.Add("runtimes", JObject.Parse("{ \"uap10-x86\": { }, \"uap10-x86-aot\": { } }"));

                return json;
            }
        }
    }
}
