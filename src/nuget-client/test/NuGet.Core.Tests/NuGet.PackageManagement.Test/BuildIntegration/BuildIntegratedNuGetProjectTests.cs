// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.VisualStudio;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class BuildIntegratedNuGetProjectTests
    {
        [Fact]
        public async Task BuildIntegratedNuGetProject_IsRestoreRequiredChangedSha512()
        {
            // Arrange
            var projectName = "testproj";

            using (var solutionManager = new TestSolutionManager())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(solutionManager.SolutionDirectory, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);
                var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

                JsonConfigUtility.AddDependency(json, new PackageDependency("nuget.versioning", VersionRange.Parse("1.0.7")));

                using (var writer = new StreamWriter(projectConfig.FullName))
                {
                    writer.Write(json.ToString());
                }

                var sources = new List<SourceRepository> { };
                var testLogger = new TestLogger();
                var settings = Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config");
                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);

                solutionManager.NuGetProjects.Add(project);

                var restoreContext = new DependencyGraphCacheContext(testLogger, settings);
                var providersCache = new RestoreCommandProvidersCache();
                var dgSpec1 = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext);

                await DependencyGraphRestoreUtility.RestoreAsync(
                    solutionManager,
                    dgSpec1,
                    restoreContext,
                    providersCache,
                    (c) => { },
                    sources,
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                var dgSpec2 = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext);
                var noOpRestoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                    solutionManager,
                    dgSpec2,
                    restoreContext,
                    providersCache,
                    (c) => { },
                    sources,
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                foreach (var restoreSummary in noOpRestoreSummaries)
                {
                    Assert.True(restoreSummary.NoOpRestore);
                }

                var resolver = new VersionFolderPathResolver(solutionManager.GlobalPackagesFolder);
                var hashPath = resolver.GetHashPath("nuget.versioning", NuGetVersion.Parse("1.0.7"));
                var nupkgMetadataPath = resolver.GetNupkgMetadataPath("nuget.versioning", NuGetVersion.Parse("1.0.7"));

                File.Delete(hashPath);
                File.Delete(nupkgMetadataPath);

                var restoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                    solutionManager,
                    await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                    restoreContext,
                    new RestoreCommandProvidersCache(),
                    (c) => { },
                    sources,
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                foreach (var restoreSummary in restoreSummaries)
                {
                    Assert.True(restoreSummary.Success);
                    Assert.False(restoreSummary.NoOpRestore);
                }
            }
        }

        [Fact]
        public async Task BuildIntegratedNuGetProject_RestoreFailed_PersistDGSpecFile()
        {
            // Arrange
            var projectName = "testproj";

            using (var packagesFolder = TestDirectory.Create())
            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);

                var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

                // invalid version for nuget.versioning package which will make this restore fail.
                JsonConfigUtility.AddDependency(json, new PackageDependency("nuget.versioning", VersionRange.Parse("3000.0.0")));

                using (var writer = new StreamWriter(projectConfig.FullName))
                {
                    writer.Write(json.ToString());
                }

                var sources = new List<SourceRepository> { };

                var testLogger = new TestLogger();
                var settings = new Settings(rootFolder);
                settings.AddOrUpdate(ConfigurationConstants.Config, new AddItem("globalPackagesFolder", packagesFolder));

                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);

                using (var solutionManager = new TestSolutionManager())
                {
                    solutionManager.NuGetProjects.Add(project);

                    var restoreContext = new DependencyGraphCacheContext(testLogger, settings);
                    var providersCache = new RestoreCommandProvidersCache();
                    var dgSpec = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext);

                    var restoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                        solutionManager,
                        dgSpec,
                        restoreContext,
                        providersCache,
                        (c) => { },
                        sources,
                        Guid.Empty,
                        false,
                        true,
                        testLogger,
                        CancellationToken.None);

                    foreach (var restoreSummary in restoreSummaries)
                    {
                        Assert.False(restoreSummary.Success);
                    }
                }
            }
        }

        [Fact]
        public async Task BuildIntegratedNuGetProject_IsRestoreRequiredMissingPackage()
        {
            // Arrange
            var projectName = "testproj";

            using (var solutionManager = new TestSolutionManager())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(solutionManager.SolutionDirectory, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);

                var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

                JsonConfigUtility.AddDependency(json, new NuGet.Packaging.Core.PackageDependency("nuget.versioning", VersionRange.Parse("1.0.7")));

                using (var writer = new StreamWriter(projectConfig.FullName))
                {
                    writer.Write(json.ToString());
                }

                var sources = new List<SourceRepository> { };

                var projectTargetFramework = NuGetFramework.Parse("uap10.0");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext());
                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);
                solutionManager.NuGetProjects.Add(project);

                var testLogger = new TestLogger();
                var settings = Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config");
                var restoreContext = new DependencyGraphCacheContext(testLogger, settings);
                var providersCache = new RestoreCommandProvidersCache();

                await DependencyGraphRestoreUtility.RestoreAsync(
                    solutionManager,
                    await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                    restoreContext,
                    providersCache,
                    (c) => { },
                    sources,
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                var noOpRestoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                    solutionManager,
                    await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                    restoreContext,
                    providersCache,
                    (c) => { },
                    sources,
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                foreach (var restoreSummary in noOpRestoreSummaries)
                {
                    Assert.True(restoreSummary.NoOpRestore);
                }

                var resolver = new VersionFolderPathResolver(solutionManager.GlobalPackagesFolder);
                var pathToDelete = resolver.GetInstallPath("nuget.versioning", NuGetVersion.Parse("1.0.7"));

                TestFileSystemUtility.DeleteRandomTestFolder(pathToDelete);

                var restoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                    solutionManager,
                    await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                    restoreContext,
                    new RestoreCommandProvidersCache(),
                    (c) => { },
                    sources,
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                foreach (var restoreSummary in restoreSummaries)
                {
                    Assert.True(restoreSummary.Success);
                    Assert.False(restoreSummary.NoOpRestore);
                }
            }
        }

        [Fact]
        public async Task BuildIntegratedNuGetProject_IsRestoreNotRequiredWithFloatingVersion()
        {
            // Arrange
            var projectName = "testproj";
            using (var packagesFolder = TestDirectory.Create())
            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);

                var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

                json.Add("dependencies", JObject.Parse("{ \"nuget.versioning\": \"1.0.*\" }"));

                using (var writer = new StreamWriter(projectConfig.FullName))
                {
                    writer.Write(json.ToString());
                }

                var sources = new List<SourceRepository>
                {
                    Repository.Factory.GetVisualStudio("https://www.nuget.org/api/v2/")
                };

                var projectTargetFramework = NuGetFramework.Parse("uap10.0");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext());
                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);

                var testLogger = new TestLogger();
                var settings = new Settings(rootFolder);
                settings.AddOrUpdate(ConfigurationConstants.Config, new AddItem("globalPackagesFolder", packagesFolder));

                using (var solutionManager = new TestSolutionManager())
                {
                    solutionManager.NuGetProjects.Add(project);

                    var restoreContext = new DependencyGraphCacheContext(testLogger, settings);
                    var providersCache = new RestoreCommandProvidersCache();

                    await DependencyGraphRestoreUtility.RestoreAsync(
                        solutionManager,
                        await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                        restoreContext,
                        providersCache,
                        (c) => { },
                        sources,
                        Guid.Empty,
                        false,
                        true,
                        testLogger,
                        CancellationToken.None);

                    var noOpRestoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                        solutionManager,
                        await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                        restoreContext,
                        providersCache,
                        (c) => { },
                        sources,
                        Guid.Empty,
                        false,
                        true,
                        testLogger,
                        CancellationToken.None);

                    foreach (var restoreSummary in noOpRestoreSummaries)
                    {
                        Assert.True(restoreSummary.NoOpRestore);
                    }
                }
            }
        }

        [Fact]
        public async Task BuildIntegratedNuGetProject_IsRestoreRequiredWithNoChanges()
        {
            // Arrange
            var projectName = "testproj";
            using (var packagesFolder = TestDirectory.Create())
            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);

                var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

                JsonConfigUtility.AddDependency(json, new NuGet.Packaging.Core.PackageDependency("nuget.versioning", VersionRange.Parse("1.0.7")));

                using (var writer = new StreamWriter(projectConfig.FullName))
                {
                    writer.Write(json.ToString());
                }

                var sources = new List<SourceRepository>
                {
                    Repository.Factory.GetVisualStudio("https://www.nuget.org/api/v2/")
                };

                var projectTargetFramework = NuGetFramework.Parse("uap10.0");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext());
                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);

                var effectiveGlobalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(NullSettings.Instance);

                using (var solutionManager = new TestSolutionManager())
                {
                    solutionManager.NuGetProjects.Add(project);

                    var testLogger = new TestLogger();
                    var settings = new Settings(rootFolder);
                    settings.AddOrUpdate(ConfigurationConstants.Config, new AddItem("globalPackagesFolder", packagesFolder));

                    var providersCache = new RestoreCommandProvidersCache();
                    var restoreContext = new DependencyGraphCacheContext(testLogger, settings);

                    await DependencyGraphRestoreUtility.RestoreAsync(
                        solutionManager,
                        await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                        restoreContext,
                        providersCache,
                        (c) => { },
                        sources,
                        Guid.Empty,
                        false,
                        true,
                        testLogger,
                        CancellationToken.None);

                    var noOpRestoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                        solutionManager,
                        await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                        restoreContext,
                        providersCache,
                        (c) => { },
                        sources,
                        Guid.Empty,
                        false,
                        true,
                        testLogger,
                        CancellationToken.None);

                    foreach (var restoreSummary in noOpRestoreSummaries)
                    {
                        Assert.True(restoreSummary.NoOpRestore);
                    }
                }
            }
        }

        [Fact(Skip = "Add nuget.config to bring in fallback folders")]
        public async Task BuildIntegratedNuGetProject_IsRestoreRequiredWithNoChangesFallbackFolder()
        {
            // Arrange
            var projectName = "testproj";

            using (var globalFolder = TestDirectory.Create())
            using (var fallbackFolder = TestDirectory.Create())
            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);

                var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

                JsonConfigUtility.AddDependency(json, new NuGet.Packaging.Core.PackageDependency("nuget.versioning", VersionRange.Parse("1.0.7")));

                using (var writer = new StreamWriter(projectConfig.FullName))
                {
                    writer.Write(json.ToString());
                }

                var sources = new List<SourceRepository>
                {
                    Repository.Factory.GetVisualStudio("https://www.nuget.org/api/v2/")
                };

                var projectTargetFramework = NuGetFramework.Parse("uap10.0");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext());
                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);

                // Restore to the fallback folder
                using (var solutionManager = new TestSolutionManager())
                {
                    solutionManager.NuGetProjects.Add(project);

                    var testLogger = new TestLogger();

                    var restoreContext = new DependencyGraphCacheContext(testLogger, NullSettings.Instance);

                    await DependencyGraphRestoreUtility.RestoreAsync(
                        solutionManager,
                        await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                        restoreContext,
                        new RestoreCommandProvidersCache(),
                        (c) => { },
                        sources,
                        Guid.Empty,
                        false,
                        true,
                        testLogger,
                        CancellationToken.None);

                    var packageFolders = new List<string> { globalFolder, fallbackFolder };

                    // Act
                    //var actual = await project.IsRestoreRequired(
                    //    packageFolders.Select(p => new VersionFolderPathResolver(p)),
                    //    new HashSet<PackageIdentity>(),
                    //    restoreContext);

                    // Assert
                    //Assert.False(actual);
                }
            }
        }

        [Fact(Skip = "Add nuget.config to bring in fallback folders")]
        public void BuildIntegratedNuGetProject_IsRestoreRequiredWithNoChangesFallbackFolderIgnoresOtherHashes()
        {
            // Arrange
            //var projectName = "testproj";

            //using (var globalFolder = TestDirectory.Create())
            //using (var fallbackFolder = TestDirectory.Create())
            //using (var rootFolder = TestDirectory.Create())
            //{
            //    var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
            //    projectFolder.Create();
            //    var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
            //    var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

            //    BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);

            //    var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

            //    JsonConfigUtility.AddDependency(json, new PackageDependency("nuget.versioning", VersionRange.Parse("1.0.7")));

            //    using (var writer = new StreamWriter(projectConfig.FullName))
            //    {
            //        writer.Write(json.ToString());
            //    }

            //    var sources = new List<SourceRepository>
            //    {
            //        Repository.Factory.GetVisualStudio("https://www.nuget.org/api/v2/")
            //    };

            //    var projectTargetFramework = NuGetFramework.Parse("uap10.0");
            //    var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext());
            //    var project = new ProjectJsonBuildIntegratedNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName, msBuildNuGetProjectSystem);

            //    // Restore to the fallback folder
            //    var result = await BuildIntegratedRestoreUtility.RestoreAsync(project,
            //        BuildIntegrationTestUtility.GetExternalProjectReferenceContext(),
            //        sources,
            //        fallbackFolder,
            //        Enumerable.Empty<string>(),
            //        CancellationToken.None);

            //    // Restore to global folder
            //    result = await BuildIntegratedRestoreUtility.RestoreAsync(project,
            //        BuildIntegrationTestUtility.GetExternalProjectReferenceContext(),
            //        sources,
            //        globalFolder,
            //        Enumerable.Empty<string>(),
            //        CancellationToken.None);

            //    var resolver = new VersionFolderPathResolver(fallbackFolder);
            //    var hashPath = resolver.GetHashPath("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            //    File.WriteAllText(hashPath, "AA00F==");

            //    var packageFolders = new List<string>() { globalFolder, fallbackFolder };

            //    var context = BuildIntegrationTestUtility.GetExternalProjectReferenceContext();

            //    // Act
            //    var actual = project.IsRestoreRequired(
            //        packageFolders.Select(p => new VersionFolderPathResolver(p)),
            //        new HashSet<PackageIdentity>(),
            //        context);

            //    // Assert
            //    Assert.False(actual);
            //}
        }
    }
}
