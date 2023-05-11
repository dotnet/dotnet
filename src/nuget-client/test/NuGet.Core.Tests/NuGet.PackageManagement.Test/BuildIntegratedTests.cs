// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Test
{
    public class BuildIntegratedTests
    {
        [Fact]
        public void BuildIntegrated_VerifyGetAddedIsOnlyPackages()
        {
            // Arrange
            var lockFile = new LockFile();
            var lockFileEmpty = new LockFile();

            var targetEmpty = new LockFileTarget();
            lockFileEmpty.Targets.Add(targetEmpty);

            var target = new LockFileTarget();
            lockFile.Targets.Add(target);

            target.Libraries.Add(new LockFileTargetLibrary()
            {
                Name = "a",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "package"
            });

            target.Libraries.Add(new LockFileTargetLibrary()
            {
                Name = "b",
                Version = NuGetVersion.Parse("1.0.0"),
                Type = "project"
            });

            // Act
            var added = BuildIntegratedRestoreUtility.GetAddedPackages(lockFileEmpty, lockFile);

            // Assert
            Assert.Equal(1, added.Count);
            Assert.Equal("a", added.Single().Id);
        }

        // Verify that parent projects are restored when a child project is updated
        [Fact]
        public async Task TestPacManBuildIntegratedInstallPackageTransitive()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("1.0.7"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            var projectDirectories = new List<TestDirectory>();

            try
            {
                using (var settingsDirectory = TestDirectory.Create())
                using (var testSolutionManager = new TestSolutionManager())
                {
                    var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, settingsDirectory);
                    var deleteOnRestartManager = new TestDeleteOnRestartManager();
                    var nuGetPackageManager = new NuGetPackageManager(
                        sourceRepositoryProvider,
                        testSettings,
                        testSolutionManager,
                        deleteOnRestartManager);

                    var token = CancellationToken.None;

                    var testNuGetProjectContext = new TestNuGetProjectContext();
                    var projectTargetFramework = NuGetFramework.Parse("net452");

                    var configs = new List<string>();
                    var lockFiles = new List<string>();
                    var buildIntegratedProjects = new List<TestProjectJsonBuildIntegratedNuGetProject>();

                    // Create projects
                    for (var i = 0; i < 4; i++)
                    {
                        var directory = TestDirectory.Create();
                        projectDirectories.Add(directory);

                        var config = Path.Combine(directory, "project.json");

                        configs.Add(config);

                        GetBasicConfig(config);

                        var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                            projectTargetFramework,
                            testNuGetProjectContext,
                            directory,
                            $"testProjectName{i}");

                        var buildIntegratedProject = new TestProjectJsonBuildIntegratedNuGetProject(config, msBuildNuGetProjectSystem);

                        buildIntegratedProjects.Add(buildIntegratedProject);

                        lockFiles.Add(ProjectJsonPathUtilities.GetLockFilePath(config));

                        testSolutionManager.NuGetProjects.Add(buildIntegratedProject);
                    }

                    // Link projects
                    var reference0 = new TestExternalProjectReference(buildIntegratedProjects[0], buildIntegratedProjects[1]);
                    var reference1 = new TestExternalProjectReference(buildIntegratedProjects[1], buildIntegratedProjects[2]);
                    var reference2 = new TestExternalProjectReference(buildIntegratedProjects[2], buildIntegratedProjects[3]);
                    var reference3 = new TestExternalProjectReference(buildIntegratedProjects[3]);

                    var myProjDirectory = TestDirectory.Create();
                    projectDirectories.Add(myProjDirectory);

                    var myProjPath = Path.Combine(myProjDirectory, "myproj.csproj");

                    var normalProject = new TestNonBuildIntegratedNuGetProject()
                    {
                        MSBuildProjectPath = myProjPath,
                        PackageSpec = new PackageSpec(new List<TargetFrameworkInformation>()
                        {
                            new TargetFrameworkInformation()
                            {
                                FrameworkName = projectTargetFramework,
                            }
                        })
                        {
                            RestoreMetadata = new ProjectRestoreMetadata()
                            {
                                ProjectName = myProjPath,
                                ProjectUniqueName = myProjPath,
                                ProjectStyle = ProjectStyle.Unknown,
                                ProjectPath = myProjPath
                            },
                            Name = myProjPath,
                            FilePath = myProjPath
                        }
                    };

                    testSolutionManager.NuGetProjects.Add(normalProject);

                    var normalReference = new TestExternalProjectReference(normalProject);

                    buildIntegratedProjects[0].ProjectReferences.Add(reference1);
                    buildIntegratedProjects[0].ProjectReferences.Add(reference2);
                    buildIntegratedProjects[0].ProjectReferences.Add(reference3);
                    buildIntegratedProjects[0].ProjectReferences.Add(normalReference);

                    buildIntegratedProjects[1].ProjectReferences.Add(reference2);
                    buildIntegratedProjects[1].ProjectReferences.Add(reference3);
                    buildIntegratedProjects[1].ProjectReferences.Add(normalReference);

                    buildIntegratedProjects[2].ProjectReferences.Add(reference3);
                    buildIntegratedProjects[2].ProjectReferences.Add(normalReference);

                    var message = string.Empty;

                    var format = new LockFileFormat();

                    // Act
                    await nuGetPackageManager.InstallPackageAsync(buildIntegratedProjects[2], packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    var parsedLockFiles = new List<LockFile>();

                    for (var i = 0; i < 3; i++)
                    {
                        var lockFile = format.Read(lockFiles[i]);
                        parsedLockFiles.Add(lockFile);
                    }

                    // Assert
                    Assert.NotNull(parsedLockFiles[0].GetLibrary("NuGet.Versioning", NuGetVersion.Parse("1.0.7")));
                    Assert.NotNull(parsedLockFiles[1].GetLibrary("NuGet.Versioning", NuGetVersion.Parse("1.0.7")));
                    Assert.NotNull(parsedLockFiles[2].GetLibrary("NuGet.Versioning", NuGetVersion.Parse("1.0.7")));
                    Assert.False(File.Exists(lockFiles[3]));
                }
            }
            finally
            {
                foreach (TestDirectory projectDirectory in projectDirectories)
                {
                    projectDirectory.Dispose();
                }
            }
        }

        // Verify that parent projects are restored when a child project is updated
        [Fact]
        public async Task TestPacManBuildIntegratedInstallPackageTransitive_VerifyCacheInvalidated()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("3.3.0"));
            var packageIdentity2 = new PackageIdentity("NuGet.Configuration", NuGetVersion.Parse("3.3.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            var projectDirectories = new List<TestDirectory>();
            var logger = new TestLogger();

            try
            {
                using (var settingsDirectory = TestDirectory.Create())
                using (var testSolutionManager = new TestSolutionManager())
                {
                    var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, settingsDirectory);
                    var deleteOnRestartManager = new TestDeleteOnRestartManager();
                    var nuGetPackageManager = new NuGetPackageManager(
                        sourceRepositoryProvider,
                        testSettings,
                        testSolutionManager,
                        deleteOnRestartManager);

                    var token = CancellationToken.None;

                    var testNuGetProjectContext = new TestNuGetProjectContext();
                    var projectTargetFramework = NuGetFramework.Parse("net452");

                    var configs = new List<string>();
                    var lockFiles = new List<string>();
                    var buildIntegratedProjects = new List<TestProjectJsonBuildIntegratedNuGetProject>();

                    // Create projects
                    for (var i = 0; i < 4; i++)
                    {
                        var directory = TestDirectory.Create();
                        projectDirectories.Add(directory);

                        var config = Path.Combine(directory, "project.json");

                        configs.Add(config);

                        GetBasicConfig(config);

                        var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                            projectTargetFramework,
                            testNuGetProjectContext,
                            directory,
                            $"testProjectName{i}");

                        var buildIntegratedProject = new TestProjectJsonBuildIntegratedNuGetProject(config, msBuildNuGetProjectSystem);
                        buildIntegratedProject.IsCacheEnabled = true;

                        buildIntegratedProjects.Add(buildIntegratedProject);

                        lockFiles.Add(ProjectJsonPathUtilities.GetLockFilePath(config));

                        testSolutionManager.NuGetProjects.Add(buildIntegratedProject);
                    }

                    // Link projects
                    var reference0 = new TestExternalProjectReference(buildIntegratedProjects[0], buildIntegratedProjects[1]);
                    var reference1 = new TestExternalProjectReference(buildIntegratedProjects[1], buildIntegratedProjects[2]);
                    var reference2 = new TestExternalProjectReference(buildIntegratedProjects[2], buildIntegratedProjects[3]);
                    var reference3 = new TestExternalProjectReference(buildIntegratedProjects[3]);

                    var myProjDirectory = TestDirectory.Create();
                    projectDirectories.Add(myProjDirectory);

                    var myProjPath = Path.Combine(myProjDirectory, "myproj.csproj");

                    var normalProject = new TestNonBuildIntegratedNuGetProject()
                    {
                        MSBuildProjectPath = myProjPath,
                        PackageSpec = new PackageSpec(new List<TargetFrameworkInformation>()
                        {
                            new TargetFrameworkInformation()
                            {
                                FrameworkName = projectTargetFramework,
                            }
                        })
                        {
                            RestoreMetadata = new ProjectRestoreMetadata()
                            {
                                ProjectName = myProjPath,
                                ProjectUniqueName = myProjPath,
                                ProjectStyle = ProjectStyle.Unknown,
                                ProjectPath = myProjPath
                            },
                            Name = myProjPath,
                            FilePath = myProjPath
                        }
                    };

                    testSolutionManager.NuGetProjects.Add(normalProject);

                    var normalReference = new TestExternalProjectReference(normalProject);

                    buildIntegratedProjects[0].ProjectReferences.Add(reference1);
                    buildIntegratedProjects[0].ProjectReferences.Add(reference2);
                    buildIntegratedProjects[0].ProjectReferences.Add(reference3);
                    buildIntegratedProjects[0].ProjectReferences.Add(normalReference);

                    buildIntegratedProjects[1].ProjectReferences.Add(reference2);
                    buildIntegratedProjects[1].ProjectReferences.Add(reference3);
                    buildIntegratedProjects[1].ProjectReferences.Add(normalReference);

                    buildIntegratedProjects[2].ProjectReferences.Add(reference3);
                    buildIntegratedProjects[2].ProjectReferences.Add(normalReference);

                    var message = string.Empty;

                    var format = new LockFileFormat();

                    // Restore and build cache
                    var restoreContext = new DependencyGraphCacheContext(logger, testSettings);

                    // Act
                    await nuGetPackageManager.InstallPackageAsync(buildIntegratedProjects[2], packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // Install again
                    await nuGetPackageManager.InstallPackageAsync(buildIntegratedProjects[2], packageIdentity2, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    var parsedLockFiles = new List<LockFile>();

                    for (var i = 0; i < 3; i++)
                    {
                        var lockFile = format.Read(lockFiles[i]);
                        parsedLockFiles.Add(lockFile);
                    }

                    // Assert
                    Assert.NotNull(parsedLockFiles[0].GetLibrary("NuGet.Configuration", NuGetVersion.Parse("3.3.0")));
                    Assert.NotNull(parsedLockFiles[1].GetLibrary("NuGet.Configuration", NuGetVersion.Parse("3.3.0")));
                    Assert.NotNull(parsedLockFiles[2].GetLibrary("NuGet.Configuration", NuGetVersion.Parse("3.3.0")));
                }
            }
            finally
            {
                foreach (TestDirectory projectDirectory in projectDirectories)
                {
                    projectDirectory.Dispose();
                }
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedInstallPackageWithReadMeFile()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("elmah", new NuGetVersion("1.2.2"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net452");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                testNuGetProjectContext.TestExecutionContext = new TestExecutionContext(packageIdentity);
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                // Act
                // Set the direct install on the execution context of INuGetProjectContext before installing a package
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity,
                    new ResolutionContext(), testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.Equal(packageIdentity, installedPackages.First().PackageIdentity);
                Assert.True(File.Exists(lockFile));
                Assert.Equal(1, testNuGetProjectContext.TestExecutionContext.FilesOpened.Count);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedInstallPackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                // Act
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.Equal(packageIdentity, installedPackages.First().PackageIdentity);
                Assert.True(File.Exists(lockFile));

                installationCompatibility.Verify(
                    x => x.EnsurePackageCompatibility(
                        buildIntegratedProject,
                        It.Is<INuGetPathContext>(y => y.UserPackageFolder != null),
                        It.Is<IEnumerable<NuGetProjectAction>>(
                            y => y.Count() == 1 &&
                            y.First().NuGetProjectActionType == NuGetProjectActionType.Install &&
                            y.First().PackageIdentity == packageIdentity),
                        It.IsAny<RestoreResult>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedInstallAndRollbackPackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("91.0.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net45");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                    projectTargetFramework,
                    testNuGetProjectContext,
                    randomProjectFolderPath);

                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                // Act
                var rollback = false;

                try
                {
                    await nuGetPackageManager.InstallPackageAsync(
                        buildIntegratedProject,
                        packageIdentity,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);
                }
                catch (InvalidOperationException)
                {
                    // Catch rollback
                    rollback = true;
                }

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.True(rollback);
                Assert.Equal(0, installedPackages.Count());
                Assert.False(File.Exists(lockFile));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedInstallAndRollbackPackageVerifyAdditionalMessages()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("91.0.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net45");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                    projectTargetFramework,
                    testNuGetProjectContext,
                    randomProjectFolderPath);

                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                // Act
                var messages = new List<ILogMessage>();

                try
                {
                    await nuGetPackageManager.InstallPackageAsync(
                        buildIntegratedProject,
                        packageIdentity,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);
                }
                catch (PackageReferenceRollbackException ex)
                {
                    messages.AddRange(ex.LogMessages);
                }

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                messages.Count.Should().Be(1);
                messages[0].Message.Should().Contain("Unable to find package nuget.core with version (>= 91.0.0)");
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdateAndRollbackPackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("91.0.0"));
            var packageIdentity2 = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.5"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            var lockFiles = new List<string>();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);

                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net45");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                    projectTargetFramework,
                    testNuGetProjectContext,
                    randomProjectFolderPath);

                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(
                        buildIntegratedProject,
                        packageIdentity2,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                // Act
                var rollback = false;

                try
                {
                    await nuGetPackageManager.InstallPackageAsync(
                        buildIntegratedProject,
                        packageIdentity,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);
                }
                catch (InvalidOperationException)
                {
                    // Catch rollback
                    rollback = true;
                }

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.True(rollback);
                Assert.Equal(packageIdentity2, installedPackages.Single().PackageIdentity);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedInstallMultiplePackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            var packageIdentity2 = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.4"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                File.Delete(lockFile);

                // Act
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity2, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Assert
                Assert.Equal(2, installedPackages.Count());
                Assert.Equal(packageIdentity2, installedPackages.First().PackageIdentity);
                Assert.Equal(packageIdentity, installedPackages.Skip(1).First().PackageIdentity);
                Assert.True(File.Exists(lockFile));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedInstallUpdatedPackage()
        {
            // Arrange
            var versioning107 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, versioning105, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                // Act
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, versioning107, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.Equal(1, installedPackages.Count());
                Assert.Equal(versioning107, installedPackages.Single().PackageIdentity);
                Assert.True(File.Exists(lockFile));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdatePackageToHighest()
        {
            // Arrange
            var nugetVersioningId = "Nuget.Versioning";
            var oldJson = new PackageIdentity(nugetVersioningId, NuGetVersion.Parse("3.5.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                var framework = "net472";
                GetBasicConfig(randomConfig, framework);

                var projectTargetFramework = NuGetFramework.Parse(framework);
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, oldJson, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                // Act
                var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    nugetVersioningId,
                    new List<NuGetProject> { buildIntegratedProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    buildIntegratedProject,
                    actions,
                    new TestNuGetProjectContext(),
                    NullSourceCacheContext.Instance,
                    CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.Equal(1, actions.Count());
                Assert.True(actions.First() is BuildIntegratedProjectAction);
                Assert.Equal(1, installedPackages.Count());
                Assert.True(installedPackages.Single().PackageIdentity.Version > oldJson.Version);
                Assert.True(File.Exists(lockFile));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdateMultipleAndRollback()
        {
            // Arrange
            // This package is not compatible with netcore50 and will cause the rollback.
            var oldVersioning = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("1.0.5"));

            // This package is compatible.
            var oldJson = new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("6.0.8"));

            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);

                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    new TestDeleteOnRestartManager());

                var projectJson = Path.Combine(randomProjectFolderPath, "project.json");
                CreateConfigJson(projectJson);

                var projectTargetFramework = NuGetFramework.Parse("net45");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(projectJson, projectFilePath);

                await nuGetPackageManager.InstallPackageAsync(
                    buildIntegratedProject,
                    oldVersioning,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.InstallPackageAsync(
                    buildIntegratedProject,
                    oldJson,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<NuGetProject> { buildIntegratedProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                var originalProjectJson = File.ReadAllText(projectJson);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<PackageReferenceRollbackException>(
                    () => nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        buildIntegratedProject,
                        actions,
                        new TestNuGetProjectContext(),
                        NullSourceCacheContext.Instance,
                        CancellationToken.None));

                var rollbackProjectJson = File.ReadAllText(projectJson);

                Assert.Equal(originalProjectJson, rollbackProjectJson);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);

                var rollbackVersioning = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == "NuGet.Versioning");
                Assert.NotNull(rollbackVersioning);
                Assert.Equal(oldVersioning, rollbackVersioning.PackageIdentity);

                var rollbackJson = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == "Newtonsoft.Json");
                Assert.NotNull(rollbackJson);
                Assert.Equal(oldJson, rollbackJson.PackageIdentity);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdatePackageAll()
        {
            // Arrange
            var nugetVersioningId = "NuGet.Versioning";
            var mvvmLightId = "MvvmLight";
            var oldMvvm = new PackageIdentity(mvvmLightId, NuGetVersion.Parse("4.2.32.7"));
            var oldJson = new PackageIdentity(nugetVersioningId, NuGetVersion.Parse("3.5.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                var framework = "net472";
                GetBasicConfig(randomConfig, framework);

                var projectTargetFramework = NuGetFramework.Parse(framework);
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                using (var cacheContext = new SourceCacheContext())
                {
                    var downloadContext = new PackageDownloadContext(cacheContext);

                    await nuGetPackageManager.InstallPackageAsync(
                        buildIntegratedProject,
                        oldMvvm,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        downloadContext,
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    await nuGetPackageManager.InstallPackageAsync(
                        buildIntegratedProject,
                        oldJson,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        downloadContext,
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    // Act
                    var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        new List<NuGetProject> { buildIntegratedProject },
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        buildIntegratedProject,
                        actions,
                        new TestNuGetProjectContext(),
                        cacheContext,
                        CancellationToken.None);

                    var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                    var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                    // Assert
                    Assert.Equal(1, actions.Count());
                    Assert.IsType<BuildIntegratedProjectAction>(actions.First());
                    Assert.Equal(NuGetProjectActionType.Install, actions.First().NuGetProjectActionType);
                    Assert.Equal(2, installedPackages.Count());

                    var newMvvm = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == mvvmLightId);
                    Assert.NotNull(newMvvm);
                    Assert.True(newMvvm.PackageIdentity.Version > oldMvvm.Version);

                    var newJson = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == nugetVersioningId);
                    Assert.NotNull(newJson);
                    Assert.True(newJson.PackageIdentity.Version > oldJson.Version);

                    Assert.True(File.Exists(lockFile));
                }
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdatePackageAllNoop()
        {
            // Arrange
            var oldJson = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("3.5.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var settingsDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, settingsDirectory);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                var framework = "net472";
                GetBasicConfig(randomConfig, framework);

                var projectTargetFramework = NuGetFramework.Parse(framework);
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;
                using (var cacheContext = new SourceCacheContext())
                {
                    var downloadContext = new PackageDownloadContext(cacheContext);

                    await nuGetPackageManager.InstallPackageAsync(
                        buildIntegratedProject,
                        oldJson,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        downloadContext,
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    // Update to the latest
                    var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        new List<NuGetProject> { buildIntegratedProject },
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        buildIntegratedProject,
                        actions,
                        new TestNuGetProjectContext(),
                        cacheContext,
                        CancellationToken.None);

                    // Act
                    actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        new List<NuGetProject> { buildIntegratedProject },
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        buildIntegratedProject,
                        actions,
                        new TestNuGetProjectContext(),
                        cacheContext,
                        CancellationToken.None);

                    var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                    var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                    // Assert
                    Assert.Equal(0, actions.Count());
                }
            }
        }

        private ISettings PopulateSettingsWithSources(SourceRepositoryProvider sourceRepositoryProvider, TestDirectory settingsDirectory)
        {
            var settings = new Settings(settingsDirectory);

            foreach (var source in sourceRepositoryProvider.GetRepositories())
            {
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, source.PackageSource.AsSourceItem());
            }

            return settings;
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdatePackageToExactVersion()
        {
            // Arrange
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
            var versioning107 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, versioning105, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                // Act
                var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    versioning107,
                    new List<NuGetProject> { buildIntegratedProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    buildIntegratedProject,
                    actions,
                    new TestNuGetProjectContext(),
                    NullSourceCacheContext.Instance,
                    CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.Equal(1, actions.Count());
                Assert.True(actions.First() is BuildIntegratedProjectAction);
                Assert.Equal(1, installedPackages.Count());
                Assert.True(installedPackages.Single().PackageIdentity.Version > versioning105.Version);
                Assert.True(File.Exists(lockFile));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdatePackageToExactVersionMulti()
        {
            // Arrange
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
            var versioning107 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            var json604 = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.4"));
            var json606 = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.6"));
            var am330 = new PackageIdentity("automapper", NuGetVersion.Parse("3.3.0"));

            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();

                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(
                    buildIntegratedProject,
                    versioning105,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.InstallPackageAsync(
                    buildIntegratedProject,
                    json604,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.InstallPackageAsync(
                    buildIntegratedProject,
                    am330,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                var targets = new List<PackageIdentity> { versioning107, json606 };

                // Act
                var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targets,
                    new List<NuGetProject> { buildIntegratedProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    buildIntegratedProject,
                    actions,
                    new TestNuGetProjectContext(),
                    NullSourceCacheContext.Instance,
                    CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.Equal(1, actions.Count());
                Assert.True(actions.First() is BuildIntegratedProjectAction);

                Assert.Equal(3, installedPackages.Count());

                foreach (var installed in installedPackages)
                {
                    if (installed.PackageIdentity.Id.Equals("nuget.versioning", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.True(installed.PackageIdentity.Version > versioning105.Version);
                    }
                    else if (installed.PackageIdentity.Id.Equals("newtonsoft.json", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.True(installed.PackageIdentity.Version > json604.Version);
                    }
                    else if (installed.PackageIdentity.Id.Equals("automapper", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.True(installed.PackageIdentity.Version == am330.Version);
                    }
                }

                Assert.True(File.Exists(lockFile));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdatePackageDowngrade()
        {
            // Arrange
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
            var versioning101 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.1"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, versioning105, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                // Act
                var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    versioning101,
                    new List<NuGetProject> { buildIntegratedProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    buildIntegratedProject,
                    actions,
                    new TestNuGetProjectContext(),
                    NullSourceCacheContext.Instance,
                    CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                // Assert
                Assert.Equal(1, actions.Count());
                Assert.Equal(1, installedPackages.Count());
                Assert.Equal("1.0.1", installedPackages.Single().PackageIdentity.Version.ToNormalizedString());
                Assert.True(File.Exists(lockFile));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUninstallPackageThatDoesNotExist()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.8"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                var message = string.Empty;

                // Act
                try
                {
                    await nuGetPackageManager.UninstallPackageAsync(buildIntegratedProject, packageIdentity.Id,
                        new UninstallationContext(), new TestNuGetProjectContext(), token);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                // Assert
                Assert.Equal("Package 'newtonsoft.json' to be uninstalled could not be found in project 'TestProjectName'", message);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUninstallPackageNoRollback()
        {
            // uninstall json.net from a project where a parent depends on it
            // this should result in the item being removed from project.json, but still existing in the lock file

            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                var basicConfig = BasicConfig;
                var dependencies = basicConfig["dependencies"] as JObject;
                dependencies.Add(new JProperty("bad2l3kj42lk4234234", "99999.9.9"));
                dependencies.Add(new JProperty("nuget.versioning", "1.0.7"));

                using (var writer = new StreamWriter(randomConfig))
                {
                    writer.Write(basicConfig.ToString());
                }

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                // Check that there are no packages returned by PackagesConfigProject
                var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, installedPackages.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.UninstallPackageAsync(buildIntegratedProject, "nuget.versioning",
                    new UninstallationContext(), new TestNuGetProjectContext(), token);

                // Assert
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackages.Count);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUninstallPackage()
        {
            // uninstall json.net from a project where a parent depends on it
            // this should result in the item being removed from project.json, but still existing in the lock file

            // Arrange
            var packageIdentity = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.8"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                CreateConfigJson(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("netcore50");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                await buildIntegratedProject.InstallPackageAsync(
                    "dotnetrdf",
                    VersionRange.Parse("1.0.8.3533"),
                    new TestNuGetProjectContext(),
                    null,
                    token);
                await buildIntegratedProject.InstallPackageAsync(
                    "newtonsoft.json",
                    VersionRange.Parse("6.0.8"),
                    new TestNuGetProjectContext(),
                    null,
                    token);

                // Check that there are no packages returned by PackagesConfigProject
                var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, installedPackages.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.UninstallPackageAsync(buildIntegratedProject, packageIdentity.Id,
                    new UninstallationContext(), new TestNuGetProjectContext(), token);

                // Assert
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackages.Count);
                Assert.Equal(packageIdentity.Id, "newtonsoft.json", StringComparer.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUninstallPackageVerifyRemovalFromLockFile()
        {
            // Arrange
            var packageIdentityA = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.8"));
            var packageIdentityB = new PackageIdentity("entityframework", NuGetVersion.Parse("6.1.3"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, testSettings, testSolutionManager, deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net452");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var projectFilePath = Path.Combine(randomProjectFolderPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
                var buildIntegratedProject = new ProjectJsonNuGetProject(randomConfig, projectFilePath);

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject,
                    packageIdentityA,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject,
                    packageIdentityB,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                var lockFileFormat = new LockFileFormat();
                var lockFilePath = ProjectJsonPathUtilities.GetLockFilePath(buildIntegratedProject.JsonConfigPath);

                var lockFile = lockFileFormat.Read(lockFilePath);
                var entityFrameworkTargets = lockFile.Targets.SelectMany(target => target.Libraries)
                    .Where(library => string.Equals(library.Name, packageIdentityB.Id, StringComparison.OrdinalIgnoreCase));

                // Check that there are no packages returned by PackagesConfigProject
                var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, installedPackages.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);
                Assert.True(entityFrameworkTargets.Any());

                // Act
                await nuGetPackageManager.UninstallPackageAsync(buildIntegratedProject, packageIdentityB.Id,
                    new UninstallationContext(), new TestNuGetProjectContext(), token);

                lockFile = lockFileFormat.Read(lockFilePath);
                entityFrameworkTargets = lockFile.Targets.SelectMany(target => target.Libraries)
                    .Where(library => string.Equals(library.Name, packageIdentityB.Id, StringComparison.OrdinalIgnoreCase));

                // Assert
                installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackages.Count);
                Assert.Equal(packageIdentityA.Id, "newtonsoft.json", StringComparer.OrdinalIgnoreCase);
                Assert.Equal(0, entityFrameworkTargets.Count());
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedInstallPackageWithInitPS1()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.3"));
            var dependencyIdentity = new PackageIdentity("Microsoft.Web.Xdt", NuGetVersion.Parse("2.1.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, testSettings, testSolutionManager, deleteManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net452");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var buildIntegratedProject = new TestProjectJsonBuildIntegratedNuGetProject(randomConfig, msBuildNuGetProjectSystem);

                var message = string.Empty;

                // Act
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                // Assert
                Assert.Equal(2, buildIntegratedProject.ExecuteInitScriptAsyncCalls.Count);
                Assert.True(buildIntegratedProject.ExecuteInitScriptAsyncCalls.Contains(packageIdentity),
                    string.Join("|", buildIntegratedProject.ExecuteInitScriptAsyncCalls));
                Assert.True(buildIntegratedProject.ExecuteInitScriptAsyncCalls.Contains(dependencyIdentity),
                    string.Join("|", buildIntegratedProject.ExecuteInitScriptAsyncCalls));
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUninstallPackageDoesNotCallInitPs1()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.3"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var testDeleteManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, testSettings, testSolutionManager, testDeleteManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net452");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var buildIntegratedProject = new TestProjectJsonBuildIntegratedNuGetProject(randomConfig, msBuildNuGetProjectSystem);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                buildIntegratedProject.ExecuteInitScriptAsyncCalls.Clear();

                // Act
                await nuGetPackageManager.UninstallPackageAsync(buildIntegratedProject, packageIdentity.Id,
                    new UninstallationContext(), new TestNuGetProjectContext(), token);

                // Assert
                Assert.Equal(0, buildIntegratedProject.ExecuteInitScriptAsyncCalls.Count);
            }
        }

        [Fact]
        public async Task TestPacManBuildIntegratedUpdatePackageCallsInitPs1OnNewPackages()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.3"));
            var updateIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.5"));
            var dependencyIdentity = new PackageIdentity("Microsoft.Web.Xdt", NuGetVersion.Parse("2.1.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, testSettings, testSolutionManager, deleteOnRestartManager);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");
                var token = CancellationToken.None;

                GetBasicConfig(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net452");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var buildIntegratedProject = new TestProjectJsonBuildIntegratedNuGetProject(randomConfig, msBuildNuGetProjectSystem);

                var message = string.Empty;

                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                buildIntegratedProject.ExecuteInitScriptAsyncCalls.Clear();

                // Act
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, updateIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                // Assert
                Assert.Equal(1, buildIntegratedProject.ExecuteInitScriptAsyncCalls.Count);
                Assert.True(buildIntegratedProject.ExecuteInitScriptAsyncCalls.Contains(updateIdentity));
            }
        }

        [Fact]
        public async Task TestPacMan_BuildIntegrated_PreviewUpdatesAsync_NoUpdatesAvailable()
        {
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

                using (var testSolutionManager = new TestSolutionManager())
                using (var randomProjectFolderPath = TestDirectory.Create())
                {
                    var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                    var token = CancellationToken.None;
                    var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, includePrelease: true, includeUnlisted: true, versionConstraints: VersionConstraints.None);
                    var testNuGetProjectContext = new TestNuGetProjectContext();
                    var deleteOnRestartManager = new TestDeleteOnRestartManager();
                    var nuGetPackageManager = new NuGetPackageManager(
                        sourceRepositoryProvider,
                        testSettings,
                        testSolutionManager,
                        deleteOnRestartManager);
                    var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                    var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                    var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");

                    GetBasicConfig(randomConfig);

                    var projectTargetFramework = NuGetFramework.Parse("net452");
                    var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                    var buildIntegratedProject = new TestProjectJsonBuildIntegratedNuGetProject(randomConfig, msBuildNuGetProjectSystem);

                    var packageContext = new SimpleTestPackageContext("packageA", "1.0.0-beta1");
                    packageContext.AddFile("lib/net45/a.dll");
                    SimpleTestPackageUtility.CreateOPCPackage(packageContext, packageSource);

                    // Install
                    await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, "packageA",
                        resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                    // Pre-Assert
                    var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                    Assert.Equal(1, installedPackages.Count);

                    // Main Act
                    var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        "packageA",
                        new List<NuGetProject> { buildIntegratedProject },
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    // Assert
                    Assert.False(actions.Any());
                }
            }
        }

        [Fact]
        public async Task TestPacMan_BuildIntegrated_PreviewUpdatesAsync_WithStrictVersionRange()
        {
            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("packageA", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("packageA", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
            };

            // Arrange
            var sourceRepositoryProvider = CreateSource(packages);

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, includePrelease: true, includeUnlisted: true, versionConstraints: VersionConstraints.None);
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var randomConfig = Path.Combine(randomProjectFolderPath, "project.json");

                BasicConfigWithPackage(randomConfig);

                var projectTargetFramework = NuGetFramework.Parse("net46");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, randomProjectFolderPath);
                var buildIntegratedProject = new TestProjectJsonBuildIntegratedNuGetProject(randomConfig, msBuildNuGetProjectSystem);

                var packageIdentity = new PackageIdentity("packageA", new NuGetVersion(1, 0, 0));
                var testLogger = new TestLogger();
                var restoreContext = new DependencyGraphCacheContext(testLogger, testSettings);
                var providersCache = new RestoreCommandProvidersCache();
                var dgSpec1 = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(testSolutionManager, restoreContext);

                await DependencyGraphRestoreUtility.RestoreAsync(
                    testSolutionManager,
                    dgSpec1,
                    restoreContext,
                    providersCache,
                    (c) => { },
                    sourceRepositoryProvider.GetRepositories(),
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                Assert.Equal(packageIdentity, installedPackages.First().PackageIdentity);

                // Main Act
                var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    "packageA",
                    new List<NuGetProject> { buildIntegratedProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.False(actions.Any());
            }
        }

        private SourceRepositoryProvider CreateSource(List<SourcePackageDependencyInfo> packages)
        {
            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new Configuration.PackageSource("http://temp");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            return new SourceRepositoryProvider(packageSourceProvider, resourceProviders);
        }

        private static void CreateConfigJson(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(BasicConfig.ToString());
            }
        }

        private static JObject BasicConfig
        {
            get
            {
                var json = new JObject();

                var frameworks = new JObject();
                frameworks["netcore50"] = new JObject();

                json["dependencies"] = new JObject();

                json["frameworks"] = frameworks;

                json.Add("runtimes", JObject.Parse("{ \"uap10-x86\": { }, \"uap10-x86-aot\": { } }"));

                return json;
            }
        }

        private static void GetBasicConfig(string path, string framework = "net46")
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(GetBasicConfigForFramework(framework).ToString());
            }
        }

        private static void BasicConfigWithPackage(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(ConfigWithPackage.ToString());
            }
        }

        private static JObject GetBasicConfigForFramework(string framework)
        {
            var json = new JObject();

            var frameworks = new JObject();
            frameworks[framework] = new JObject();

            json["dependencies"] = new JObject();

            json["frameworks"] = frameworks;

            return json;
        }

        private static JObject ConfigWithPackage
        {
            get
            {
                var json = new JObject();

                var frameworks = new JObject();
                frameworks["net46"] = new JObject();

                var deps = new JObject();
                var prop = new JProperty("packageA", "[1.0.0]");
                deps.Add(prop);

                json["dependencies"] = deps;

                json["frameworks"] = frameworks;

                return json;
            }
        }

        private class TestProjectJsonBuildIntegratedNuGetProject
            : ProjectJsonNuGetProject
            , INuGetProjectServices
            , IProjectScriptHostService
            , IProjectSystemReferencesReader
        {
            public HashSet<PackageIdentity> ExecuteInitScriptAsyncCalls { get; }
                = new HashSet<PackageIdentity>(PackageIdentity.Comparer);

            public List<TestExternalProjectReference> ProjectReferences { get; }
                = new List<TestExternalProjectReference>();

            public bool IsCacheEnabled { get; set; }

            [Obsolete]
            public IProjectBuildProperties BuildProperties => throw new NotImplementedException();

            public IProjectSystemCapabilities Capabilities => throw new NotImplementedException();

            public IProjectSystemReferencesReader ReferencesReader => this;

            public IProjectSystemReferencesService References => throw new NotImplementedException();

            public IProjectSystemService ProjectSystem => throw new NotImplementedException();

            public IProjectScriptHostService ScriptService => this;

            public TestProjectJsonBuildIntegratedNuGetProject(
                string jsonConfig,
                IMSBuildProjectSystem msbuildProjectSystem)
                : base(jsonConfig, msbuildProjectSystem.ProjectFileFullPath)
            {
                ProjectServices = this;
            }

            public override async Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
            {
                if (IsCacheEnabled)
                {
                    if (context.PackageSpecCache.TryGetValue(MSBuildProjectPath, out var cachedResult))
                    {
                        return new[] { cachedResult };
                    }
                }

                return await base.GetPackageSpecsAsync(context);
            }

            public T GetGlobalService<T>() where T : class
            {
                throw new NotImplementedException();
            }

            public Task ExecutePackageScriptAsync(PackageIdentity packageIdentity, string packageInstallPath, string scriptRelativePath, INuGetProjectContext projectContext, bool throwOnFailure, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public Task<bool> ExecutePackageInitScriptAsync(PackageIdentity packageIdentity, string packageInstallPath, INuGetProjectContext projectContext, bool throwOnFailure, CancellationToken token)
            {
                ExecuteInitScriptAsyncCalls.Add(packageIdentity);
                return Task.FromResult(true);
            }

            public Task<IEnumerable<LibraryDependency>> GetPackageReferencesAsync(NuGetFramework targetFramework, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(ILogger logger, CancellationToken token)
            {
                var projectRefs = ProjectReferences.Select(e => new ProjectRestoreReference()
                {
                    ProjectUniqueName = e.MSBuildProjectPath,
                    ProjectPath = e.MSBuildProjectPath,
                });

                return Task.FromResult(projectRefs);
            }
        }

        private class TestNonBuildIntegratedNuGetProject : NuGetProject, IDependencyGraphProject
        {
            public List<TestExternalProjectReference> ProjectReferences { get; }
                = new List<TestExternalProjectReference>();

            public string MSBuildProjectPath { get; set; }

            public DateTimeOffset LastModified { get; set; }

            public PackageSpec PackageSpec { get; set; }

            public TestNonBuildIntegratedNuGetProject()
                : base()
            {
            }

            public Task<IReadOnlyList<IDependencyGraphProject>> GetDirectProjectReferencesAsync(DependencyGraphCacheContext context)
            {
                return Task.FromResult<IReadOnlyList<IDependencyGraphProject>>(ProjectReferences
                    .Select(e => e.Project)
                    .Where(e => e != null)
                    .ToList());
            }

            public Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
            {
                return Task.FromResult<IReadOnlyList<PackageSpec>>(new List<PackageSpec>() { PackageSpec });
            }

            public Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
            {
                return Task.FromResult<(IReadOnlyList<PackageSpec>, IReadOnlyList<IAssetsLogMessage>)>((new List<PackageSpec>() { PackageSpec }, null));
            }

            public Task<DependencyGraphSpec> GetDependencyGraphSpecAsync(DependencyGraphCacheContext context)
            {
                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(PackageSpec);
                dgSpec.AddRestore(PackageSpec.RestoreMetadata.ProjectUniqueName);

                return Task.FromResult(dgSpec);
            }

            public Task<bool> IsRestoreRequired(IEnumerable<VersionFolderPathResolver> pathResolvers, ISet<PackageIdentity> packagesChecked, DependencyGraphCacheContext context)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> InstallPackageAsync(PackageIdentity packageIdentity, DownloadResourceResult downloadResourceResult, INuGetProjectContext nuGetProjectContext, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }

        private ExternalProjectReference CreateReference(string name)
        {
            return new ExternalProjectReference(name, null, null, Enumerable.Empty<string>());
        }

        private class TestExternalProjectReference
        {
            public IDependencyGraphProject Project { get; set; }

            public IDependencyGraphProject[] Children { get; set; }

            public TestExternalProjectReference(
                IDependencyGraphProject project,
                params IDependencyGraphProject[] children)
            {
                Project = project;
                Children = children;
                MSBuildProjectPath = project.MSBuildProjectPath;
            }

            public string MSBuildProjectPath { get; set; }
        }
    }
}
