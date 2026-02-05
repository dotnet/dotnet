// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

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
using NuGet.Commands.Test;
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
        public void GetAddedPackages_LockFileTargetLibraryProject_OnlyAddsPackage()
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

        [Fact]
        public async Task ChildProjectUpdated_ParentProjectsRestored()
        {
            using (var pathContext = new SimpleTestPathContext())
            using (var testSolutionManager = new TestSolutionManager(pathContext))
            {
                var package = new SimpleTestPackageContext("a", "1.0.7");
                var packageIdentity = package.Identity;
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, package);

                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
                var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);

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
                var buildIntegratedProjects = new List<TestPackageReferenceNuGetProject>();

                // Create projects
                for (var i = 0; i < 4; i++)
                {
                    var projectName = $"testProjectName{i}";
                    var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, projectName, rootPath: pathContext.SolutionRoot);
                    var directory = Path.GetDirectoryName(packageSpec.FilePath);
                    var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                        projectTargetFramework,
                        testNuGetProjectContext,
                        directory,
                        projectName);

                    var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

                    buildIntegratedProjects.Add(buildIntegratedProject);

                    lockFiles.Add(Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName));

                    testSolutionManager.NuGetProjects.Add(buildIntegratedProject);
                }
                var myProjDirectory = Path.Combine(pathContext.SolutionRoot, "myproj");

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

                // Link projects
                buildIntegratedProjects[0].AddProjectReference(buildIntegratedProjects[1]);
                buildIntegratedProjects[0].AddProjectReference(buildIntegratedProjects[2]);
                buildIntegratedProjects[0].AddProjectReference(buildIntegratedProjects[3]);
                buildIntegratedProjects[0].AddProjectReference(normalProject.PackageSpec);
                buildIntegratedProjects[1].AddProjectReference(buildIntegratedProjects[2]);
                buildIntegratedProjects[1].AddProjectReference(buildIntegratedProjects[3]);
                buildIntegratedProjects[1].AddProjectReference(normalProject.PackageSpec);
                buildIntegratedProjects[2].AddProjectReference(buildIntegratedProjects[3]);
                buildIntegratedProjects[2].AddProjectReference(normalProject.PackageSpec);
                buildIntegratedProjects[3].AddProjectReference(normalProject.PackageSpec);

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
                Assert.NotNull(parsedLockFiles[0].GetLibrary(packageIdentity.Id, packageIdentity.Version));
                Assert.NotNull(parsedLockFiles[1].GetLibrary(packageIdentity.Id, packageIdentity.Version));
                Assert.NotNull(parsedLockFiles[2].GetLibrary(packageIdentity.Id, packageIdentity.Version));
                Assert.False(File.Exists(lockFiles[3]));
            }
        }

        // Verify that parent projects are restored when a child project is updated
        [Fact]
        public async Task InstallPackageTransitive_VerifyCacheInvalidated()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            var packageIdentity = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("3.3.0"));
            var packageIdentity2 = new PackageIdentity("NuGet.Configuration", NuGetVersion.Parse("3.3.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            var packageContext = new SimpleTestPackageContext(packageIdentity);
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageContext);
            var projectDirectories = new List<TestDirectory>();
            var logger = new TestLogger();

            try
            {
                var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var token = CancellationToken.None;

                var testNuGetProjectContext = new TestNuGetProjectContext();
                testNuGetProjectContext.TestExecutionContext = new TestExecutionContext(packageIdentity);

                var projectTargetFramework = NuGetFramework.Parse("net452");
                var buildIntegratedProjects = new List<TestPackageReferenceNuGetProject>();

                // Create projects
                for (var i = 0; i < 4; i++)
                {
                    string projectName = $"testProjectName{i}";

                    // Create a PackageSpec for the PackageReference project.
                    var packageSpec = ProjectTestHelpers.GetPackageSpec(
                        testSettings,
                        projectName,
                        pathContext.SolutionRoot,
                        framework: "net452");

                    var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                        projectTargetFramework,
                        testNuGetProjectContext,
                        projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                        projectName
                        );

                    var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
                    testSolutionManager.NuGetProjects.Add(buildIntegratedProject);
                    buildIntegratedProject.IsCacheEnabled = true;

                    buildIntegratedProjects.Add(buildIntegratedProject);
                    testSolutionManager.NuGetProjects.Add(buildIntegratedProject);
                }

                // Link projects
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

                buildIntegratedProjects[0].AddProjectReference(buildIntegratedProjects[1]);
                buildIntegratedProjects[0].AddProjectReference(buildIntegratedProjects[2]);
                buildIntegratedProjects[0].AddProjectReference(buildIntegratedProjects[3]);
                buildIntegratedProjects[0].AddProjectReference(normalProject.PackageSpec);

                buildIntegratedProjects[1].AddProjectReference(buildIntegratedProjects[2]);
                buildIntegratedProjects[1].AddProjectReference(buildIntegratedProjects[3]);
                buildIntegratedProjects[1].AddProjectReference(buildIntegratedProjects[3]);
                buildIntegratedProjects[1].AddProjectReference(normalProject.PackageSpec);

                buildIntegratedProjects[2].AddProjectReference(buildIntegratedProjects[3]);
                buildIntegratedProjects[2].AddProjectReference(normalProject.PackageSpec);

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

                // Assert
                string assetsFilePath1 = await buildIntegratedProjects[0].GetAssetsFilePathAsync();
                string assetsFilePath2 = await buildIntegratedProjects[1].GetAssetsFilePathAsync();
                string assetsFilePath3 = await buildIntegratedProjects[2].GetAssetsFilePathAsync();

                File.Exists(assetsFilePath1).Should().BeTrue();
                File.Exists(assetsFilePath2).Should().BeTrue();
                File.Exists(assetsFilePath3).Should().BeTrue();

                LockFileFormat lockFileFormat = new LockFileFormat();
                LockFile lockFile1 = lockFileFormat.Read(assetsFilePath1);
                LockFile lockFile2 = lockFileFormat.Read(assetsFilePath2);
                LockFile lockFile3 = lockFileFormat.Read(assetsFilePath3);

                lockFile1.GetLibrary("NuGet.Configuration", NuGetVersion.Parse("3.3.0")).Should().NotBeNull();
                lockFile2.GetLibrary("NuGet.Configuration", NuGetVersion.Parse("3.3.0")).Should().NotBeNull();
                lockFile3.GetLibrary("NuGet.Configuration", NuGetVersion.Parse("3.3.0")).Should().NotBeNull();
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
        public async Task InstallPackageWithReadMeFile()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            var packageIdentity = new PackageIdentity("mypackage", new NuGetVersion("1.2.2"));
            var packageContext = new SimpleTestPackageContext(packageIdentity);
            packageContext.AddFile("Readme.txt", "This is a readme file for mypackage 1.2.2.");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageContext);
            string projectName = "project1";
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net452");

            var projectTargetFramework = NuGetFramework.Parse("net452");
            var testNuGetProjectContext = new TestNuGetProjectContext();
            testNuGetProjectContext.TestExecutionContext = new TestExecutionContext(packageIdentity);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            // Act
            // Set the direct install on the execution context of INuGetProjectContext before installing a package
            await nuGetPackageManager.InstallPackageAsync(
                buildIntegratedProject,
                packageIdentity,
                resolutionContext: new ResolutionContext(),
                testNuGetProjectContext,
                sourceRepositoryProvider.GetRepositories().First(),
                secondarySources: null,
                token: CancellationToken.None);

            var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
            var assetsFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

            // Assert
            packageSpec.RestoreMetadata.ProjectStyle.Should().Be(ProjectStyle.PackageReference);
            Assert.Equal(packageIdentity, installedPackages.First().PackageIdentity);
            Assert.True(File.Exists(assetsFile));
            Assert.Equal(1, testNuGetProjectContext.TestExecutionContext.FilesOpened.Count);
        }

        [Fact]
        public async Task InstallPackage()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testSolutionManager = new TestSolutionManager(pathContext))
            {
                var package = new SimpleTestPackageContext("a", "1.0.7");
                var packageIdentity = package.Identity;
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, package);
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));

                var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var token = CancellationToken.None;

                var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, "projectName");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(packageSpec.TargetFrameworks[0].FrameworkName, testNuGetProjectContext, packageSpec.FilePath);
                var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

                var message = string.Empty;

                // Act
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
                var lockFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

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
        public async Task InstallAndRollbackPackageVerifyAdditionalMessages()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, "project1", pathContext.SolutionRoot, framework: "net45");
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                packageSpec.TargetFrameworks[0].FrameworkName,
                new TestNuGetProjectContext(),
                Path.GetDirectoryName(packageSpec.FilePath),
                packageSpec.Name);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                new SimpleTestPackageContext(new PackageIdentity("nuget.core", NuGetVersion.Parse("10.0.0"))));

            // Act
            var messages = new List<ILogMessage>();

            try
            {
                await nuGetPackageManager.InstallPackageAsync(
                    buildIntegratedProject,
                    new PackageIdentity("nuget.core", NuGetVersion.Parse("91.0.0")),
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
            var lockFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

            // Assert
            messages.Count.Should().Be(1);
            messages[0].Message.Should().Contain("Unable to find package nuget.core with version (>= 91.0.0)");
        }

        [Fact]
        public async Task UpdateAndRollbackPackage()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("91.0.0"));
            var packageIdentity2 = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.5"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var projectName = "TestProjectName";

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageIdentity2);

            var lockFiles = new List<string>();

            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);

            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var token = CancellationToken.None;

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net452");

            var projectTargetFramework = NuGetFramework.Parse("net45");
            var testNuGetProjectContext = new TestNuGetProjectContext();
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

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

            // Assert
            Assert.True(rollback);
            Assert.Equal(packageIdentity2, installedPackages.Single().PackageIdentity);
        }

        [Fact]
        public async Task InstallMultiplePackage()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, "project1", pathContext.SolutionRoot);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                packageSpec.TargetFrameworks[0].FrameworkName,
                new TestNuGetProjectContext(),
                Path.GetDirectoryName(packageSpec.FilePath),
                packageSpec.Name);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            var package = new SimpleTestPackageContext("a", "1.0.7");
            var package2 = new SimpleTestPackageContext("b", "1.0.5");
            var packageIdentity = package.Identity;
            var packageIdentity2 = package2.Identity;
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, package, package2);

            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            var lockFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);
            File.Delete(lockFile);

            // Act
            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity2, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();

            // Assert
            Assert.Equal(2, installedPackages.Count);
            Assert.Equal(packageIdentity, installedPackages[0].PackageIdentity);
            Assert.Equal(packageIdentity2, installedPackages[1].PackageIdentity);
            Assert.True(File.Exists(lockFile));
        }

        [Fact]
        public async Task InstallUpdatedPackage()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var versioning107 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, versioning105, versioning107);

            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);
            var projectName = "TestProjectName";

            var token = CancellationToken.None;

            var projectTargetFramework = NuGetFramework.Parse("netcore50");
            var testNuGetProjectContext = new TestNuGetProjectContext();

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net452");

            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName
                );

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            var message = string.Empty;

            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, versioning105, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            // Act
            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, versioning107, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            // Assert
            var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None);
            string assetsFile = await buildIntegratedProject.GetAssetsFilePathAsync();
            File.Exists(assetsFile).Should().BeTrue();

            installedPackages.Count().Should().Be(1);
            installedPackages.Single().PackageIdentity.Should().Be(versioning107);
        }

        [Fact]
        public async Task UpdatePackageToHighest()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            var projectName = "TestProjectName";
            var nugetVersioningId = "Nuget.Versioning";
            var originalInstalledPackage = new PackageIdentity(nugetVersioningId, NuGetVersion.Parse("3.5.0"));
            var latestPackage = new PackageIdentity(nugetVersioningId, NuGetVersion.Parse("10.5.0"));
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, originalInstalledPackage, latestPackage);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var token = CancellationToken.None;

            var framework = "net472";
            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework);

            var projectTargetFramework = NuGetFramework.Parse(framework);
            var testNuGetProjectContext = new TestNuGetProjectContext();
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                        projectTargetFramework,
                        testNuGetProjectContext,
                        projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                        projectName);
            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            var sourceRepositories = sourceRepositoryProvider.GetRepositories();
            var message = string.Empty;

            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, originalInstalledPackage, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositories, sourceRepositories, token);

            // Act
            List<NuGetProjectAction> actions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                nugetVersioningId,
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                token)).ToList();

            await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                buildIntegratedProject,
                actions,
                new TestNuGetProjectContext(),
                NullSourceCacheContext.Instance,
                token);


            // Assert
            var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(token)).ToList();
            string assetsFile = await buildIntegratedProject.GetAssetsFilePathAsync();
            File.Exists(assetsFile).Should().BeTrue();

            installedPackages.Count.Should().Be(1);
            installedPackages[0].PackageIdentity.Version
                .Should().BeGreaterThan(originalInstalledPackage.Version);

            actions.Count.Should().Be(1);
            actions[0].Should().BeOfType<BuildIntegratedProjectAction>();
        }

        [Fact]
        public async Task UpdateMultipleAndRollback()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            // This latest version of this package is not compatible and will cause the rollback.
            var packageWithIncompatibleLatest = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("1.0.5"));
            var incompatibleLatestPackage = new SimpleTestPackageContext("NuGet.Versioning", "1.0.7");
            incompatibleLatestPackage.AddFile("lib/net9.0/NuGet.Versioning.dll");
            incompatibleLatestPackage.UseDefaultRuntimeAssemblies = false;

            // This package and its latest version are compatible.
            var packageWithCompatibleLatest = new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("6.0.8"));
            var compatiblePackageLatest = new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("7.0.8"));

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                packageWithCompatibleLatest,
                compatiblePackageLatest,
                packageWithIncompatibleLatest);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                incompatibleLatestPackage);

            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);

            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                new TestDeleteOnRestartManager());

            var projectTargetFramework = NuGetFramework.Parse("net45");
            var testNuGetProjectContext = new TestNuGetProjectContext();
            string projectName = $"testProjectName";

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net45");

            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            var sourceRepositories = sourceRepositoryProvider.GetRepositories();

            await nuGetPackageManager.InstallPackageAsync(
                buildIntegratedProject,
                packageWithIncompatibleLatest,
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                CancellationToken.None);

            await nuGetPackageManager.InstallPackageAsync(
                buildIntegratedProject,
                packageWithCompatibleLatest,
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                CancellationToken.None);

            var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                CancellationToken.None);

            // Act & Assert
            LockFileFormat lockFileFormat = new LockFileFormat();

            string assetsFilePathOriginal = await buildIntegratedProject.GetAssetsFilePathAsync();
            File.Exists(assetsFilePathOriginal).Should().BeTrue();
            LockFile originalAssetsFile = lockFileFormat.Read(assetsFilePathOriginal);

            var exception = await Assert.ThrowsAsync<PackageReferenceRollbackException>(
                () => nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    buildIntegratedProject,
                    actions,
                    new TestNuGetProjectContext(),
                    NullSourceCacheContext.Instance,
                    CancellationToken.None));

            string assetsFilePathRollback = await buildIntegratedProject.GetAssetsFilePathAsync();
            File.Exists(assetsFilePathRollback).Should().BeTrue();
            LockFile rollbackAssetsFile = lockFileFormat.Read(assetsFilePathRollback);

            originalAssetsFile.Should().Be(rollbackAssetsFile);

            var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();

            installedPackages.Count.Should().Be(2);

            var rollbackPackage = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == "NuGet.Versioning");
            rollbackPackage.Should().NotBeNull();
            rollbackPackage.PackageIdentity.Should().Be(packageWithIncompatibleLatest);

            var installedOriginalPackage = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == "Newtonsoft.Json");
            installedOriginalPackage.Should().NotBeNull();
            installedOriginalPackage.PackageIdentity.Should().Be(packageWithCompatibleLatest);
        }

        [Fact]
        public async Task UpdatePackageAll()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            string projectName = "TestProjectName";
            string packageId1 = "MvvmLight";
            string packageId2 = "NuGet.Versioning";

            var originalPackage1 = new PackageIdentity(packageId1, NuGetVersion.Parse("4.2.32.7"));
            var originalPackage2 = new PackageIdentity(packageId2, NuGetVersion.Parse("3.5.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var token = CancellationToken.None;

            var framework = "net472";
            var projectTargetFramework = NuGetFramework.Parse(framework);
            var testNuGetProjectContext = new TestNuGetProjectContext();

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);
            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            var message = string.Empty;

            using var cacheContext = new SourceCacheContext();
            var downloadContext = new PackageDownloadContext(cacheContext);
            var sourceRepositories = sourceRepositoryProvider.GetRepositories();

            await nuGetPackageManager.InstallPackageAsync(
                buildIntegratedProject,
                originalPackage1,
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                downloadContext,
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                token);

            await nuGetPackageManager.InstallPackageAsync(
                buildIntegratedProject,
                originalPackage2,
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                downloadContext,
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                token);

            // Act
            List<NuGetProjectAction> actions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                token)).ToList();

            await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                buildIntegratedProject,
                actions,
                new TestNuGetProjectContext(),
                cacheContext,
                token);

            List<PackageReference> installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
            var assetsFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

            // Assert
            File.Exists(assetsFile).Should().BeTrue();
            actions.Count.Should().Be(1);
            actions[0].Should().BeOfType<BuildIntegratedProjectAction>();
            actions[0].NuGetProjectActionType.Should().Be(NuGetProjectActionType.Install);

            installedPackages.Count.Should().Be(2);

            var updatedPackageId1 = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == packageId1);
            Assert.NotNull(updatedPackageId1);
            Assert.True(updatedPackageId1.PackageIdentity.Version > originalPackage1.Version);

            var updatedPackageId2 = installedPackages.FirstOrDefault(x => x.PackageIdentity.Id == packageId2);
            Assert.NotNull(updatedPackageId2);
            Assert.True(updatedPackageId2.PackageIdentity.Version > originalPackage2.Version);
        }

        [Fact]
        public async Task UpdatePackageAllNoop()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            string projectName = "TestProjectName";

            var originalPackage = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("3.5.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var token = CancellationToken.None;

            var framework = "net472";

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework);

            var projectTargetFramework = NuGetFramework.Parse(framework);
            var testNuGetProjectContext = new TestNuGetProjectContext();
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
               projectTargetFramework,
               testNuGetProjectContext,
               projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
               projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            using var cacheContext = new SourceCacheContext();
            var downloadContext = new PackageDownloadContext(cacheContext);

            var sourceRepositories = sourceRepositoryProvider.GetRepositories();

            await nuGetPackageManager.InstallPackageAsync(
                buildIntegratedProject,
                originalPackage,
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                downloadContext,
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                token);

            // Update to the latest
            var actionsFirstUpdate = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                token);

            await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                buildIntegratedProject,
                actionsFirstUpdate,
                new TestNuGetProjectContext(),
                cacheContext,
                token);

            // Act
            var actionsSecondUpdate = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                token);

            await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                buildIntegratedProject,
                actionsSecondUpdate,
                new TestNuGetProjectContext(),
                cacheContext,
                token);

            var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(token);

            // Assert
            actionsFirstUpdate.Count().Should().Be(1);
            actionsSecondUpdate.Count().Should().Be(0);
        }

        [Fact]
        public async Task UpdatePackageToExactVersion()
        {
            // Arrange
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
            var versioning107 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));

            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, "project1", pathContext.SolutionRoot);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                packageSpec.TargetFrameworks[0].FrameworkName,
                new TestNuGetProjectContext(),
                Path.GetDirectoryName(packageSpec.FilePath),
                packageSpec.Name);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                versioning105,
                versioning107);

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
            var lockFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

            // Assert
            Assert.Equal(1, actions.Count());
            Assert.True(actions.First() is BuildIntegratedProjectAction);
            Assert.Equal(1, installedPackages.Count());
            Assert.True(installedPackages.Single().PackageIdentity.Version > versioning105.Version);
            Assert.True(File.Exists(lockFile));
        }

        [Fact]
        public async Task UpdatePackageToExactVersionMulti()
        {
            // Arrange
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
            var versioning107 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.7"));
            var json604 = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.4"));
            var json606 = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.6"));
            var am330 = new PackageIdentity("automapper", NuGetVersion.Parse("3.3.0"));

            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, "project1", pathContext.SolutionRoot);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                packageSpec.TargetFrameworks[0].FrameworkName,
                new TestNuGetProjectContext(),
                Path.GetDirectoryName(packageSpec.FilePath),
                packageSpec.Name);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                versioning105,
                versioning107,
                json604,
                json606,
                am330);

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
            var lockFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

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

        [Fact]
        public async Task UpdatePackageDowngrade()
        {
            // Arrange
            var versioning105 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
            var versioning101 = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.1"));

            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, "project1", pathContext.SolutionRoot);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                packageSpec.TargetFrameworks[0].FrameworkName,
                new TestNuGetProjectContext(),
                Path.GetDirectoryName(packageSpec.FilePath),
                packageSpec.Name);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                versioning105,
                versioning101);

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
            var lockFile = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

            // Assert
            Assert.Equal(1, actions.Count());
            Assert.Equal(1, installedPackages.Count());
            Assert.Equal("1.0.1", installedPackages.Single().PackageIdentity.Version.ToNormalizedString());
            Assert.True(File.Exists(lockFile));
        }

        [Fact]
        public async Task UninstallPackageThatDoesNotExist()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.8"));
            var projectName = "TestProjectName";

            using (var pathContext = new SimpleTestPathContext())
            using (var testSolutionManager = new TestSolutionManager(pathContext))
            {
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
                var testSettings = Settings.LoadDefaultSettings(pathContext.SolutionRoot);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var token = CancellationToken.None;
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, projectName);
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(packageSpec.TargetFrameworks[0].FrameworkName, testNuGetProjectContext, packageSpec.FilePath);
                var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

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
        public async Task UninstallPackageNoRollback()
        {
            // uninstall the package from a project where a parent depends on it
            // this should result in the item being removed from project, but still existing in the assets file

            var projectName = "TestProjectName";

            using (var pathContext = new SimpleTestPathContext())
            using (var testSolutionManager = new TestSolutionManager(pathContext))
            {
                var packageIdentity = new SimpleTestPackageContext("bad2l3kj42lk4234234", "99999.9.9");
                var packageIdentity2 = new SimpleTestPackageContext("nuget.versioning", "1.0.7");
                packageIdentity.Dependencies.Add(packageIdentity2);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageIdentity, packageIdentity2);

                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
                var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);

                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var token = CancellationToken.None;

                var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, projectName, rootPath: pathContext.SolutionRoot);
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, new PackageDependency("bad2l3kj42lk4234234", VersionRange.Parse("99999.9.9")));
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, new PackageDependency("nuget.versioning", VersionRange.Parse("1.0.7")));

                var projectTargetFramework = packageSpec.TargetFrameworks[0].FrameworkName;
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, testNuGetProjectContext, packageSpec.FilePath);
                var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

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
        public async Task UninstallPackageVerifyRemovalFromLockFile()
        {
            // Arrange
            var packageIdentityA = new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.8"));
            var packageIdentityB = new PackageIdentity("entityframework", NuGetVersion.Parse("6.1.3"));

            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var packageSpec = ProjectTestHelpers.GetPackageSpec(testSettings, "project1", pathContext.SolutionRoot);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                packageSpec.TargetFrameworks[0].FrameworkName,
                new TestNuGetProjectContext(),
                Path.GetDirectoryName(packageSpec.FilePath),
                packageSpec.Name);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                packageIdentityA,
                packageIdentityB);

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
            var lockFilePath = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);

            var lockFile = lockFileFormat.Read(lockFilePath);
            var entityFrameworkTargets = lockFile.Targets.SelectMany(target => target.Libraries)
                .Where(library => string.Equals(library.Name, packageIdentityB.Id, StringComparison.OrdinalIgnoreCase));

            // Check that there are no packages returned by PackagesConfigProject
            var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
            Assert.Equal(2, installedPackages.Count);
            Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);
            Assert.True(entityFrameworkTargets.Any());

            // Act
            await nuGetPackageManager.UninstallPackageAsync(buildIntegratedProject, packageIdentityB.Id,
                new UninstallationContext(), new TestNuGetProjectContext(), CancellationToken.None);

            lockFile = lockFileFormat.Read(lockFilePath);
            entityFrameworkTargets = lockFile.Targets.SelectMany(target => target.Libraries)
                .Where(library => string.Equals(library.Name, packageIdentityB.Id, StringComparison.OrdinalIgnoreCase));

            // Assert
            installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
            Assert.Equal(1, installedPackages.Count);
            Assert.Equal(packageIdentityA.Id, "newtonsoft.json", StringComparer.OrdinalIgnoreCase);
            Assert.Equal(0, entityFrameworkTargets.Count());
        }

        [Fact]
        public async Task InstallPackageWithInitPS1()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            var projectName = "TestProjectName";
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.3"));
            var depedencyIdentity = new PackageIdentity("Microsoft.Web.Xdt", NuGetVersion.Parse("2.1.0"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var packageContext = new SimpleTestPackageContext(packageIdentity);
            var dependencyPackageContext = new SimpleTestPackageContext(depedencyIdentity);
            packageContext.Dependencies.Add(dependencyPackageContext);
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageContext, dependencyPackageContext);

            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, testSettings, testSolutionManager, deleteOnRestartManager);

            var token = CancellationToken.None;

            var projectTargetFramework = NuGetFramework.Parse("net452");
            var testNuGetProjectContext = new TestNuGetProjectContext();
            testNuGetProjectContext.TestExecutionContext = new TestExecutionContext(packageIdentity);

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net452");

            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName
                );

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            var message = string.Empty;

            // Act
            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), token);

            // Assert
            Assert.Equal(2, buildIntegratedProject.ExecuteInitScriptAsyncCalls.Count);
            Assert.True(buildIntegratedProject.ExecuteInitScriptAsyncCalls.Contains(packageIdentity),
                string.Join("|", buildIntegratedProject.ExecuteInitScriptAsyncCalls));
            buildIntegratedProject.ExecuteInitScriptAsyncCalls.Should().Contain(depedencyIdentity, because:
                string.Join("|", buildIntegratedProject.ExecuteInitScriptAsyncCalls));
        }

        [Fact]
        public async Task UninstallPackageDoesNotCallInitPs1()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            var projectName = "TestProjectName";

            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.3"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var testDeleteManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, testSettings, testSolutionManager, testDeleteManager);

            var token = CancellationToken.None;

            var projectTargetFramework = NuGetFramework.Parse("net452");
            var testNuGetProjectContext = new TestNuGetProjectContext();
            testNuGetProjectContext.TestExecutionContext = new TestExecutionContext(packageIdentity);

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net452");

            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            buildIntegratedProject.ExecuteInitScriptAsyncCalls.Clear();

            // Act
            await nuGetPackageManager.UninstallPackageAsync(buildIntegratedProject, packageIdentity.Id,
                new UninstallationContext(), new TestNuGetProjectContext(), token);

            // Assert
            buildIntegratedProject.ExecuteInitScriptAsyncCalls.Count.Should().Be(0);
        }

        [Fact]
        public async Task UpdatePackageCallsInitPs1OnNewPackages()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            string projectName = "TestProjectName";
            var packageIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.3"));
            var updateIdentity = new PackageIdentity("nuget.core", NuGetVersion.Parse("2.8.5"));
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var token = CancellationToken.None;

            var testNuGetProjectContext = new TestNuGetProjectContext();
            testNuGetProjectContext.TestExecutionContext = new TestExecutionContext(packageIdentity);

            var projectTargetFramework = NuGetFramework.Parse("net452");

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net452");

            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            buildIntegratedProject.ExecuteInitScriptAsyncCalls.Clear();

            // Act
            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, updateIdentity, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            // Assert
            buildIntegratedProject.ExecuteInitScriptAsyncCalls.Count.Should().Be(1);
            buildIntegratedProject.ExecuteInitScriptAsyncCalls.Should().Contain(updateIdentity);
        }

        [Fact]
        public async Task PreviewUpdatesAsync_NoUpdatesAvailable()
        {
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(pathContext);

            // Arrange
            string projectName = "TestProjectName";
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                new List<PackageSource>()
                {
                    new PackageSource(pathContext.PackageSource)
                });

            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);
            var token = CancellationToken.None;
            var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, includePrelease: true, includeUnlisted: true, versionConstraints: VersionConstraints.None);
            var testNuGetProjectContext = new TestNuGetProjectContext();
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);

            var projectTargetFramework = NuGetFramework.Parse("net452");

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "net452");

            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            testSolutionManager.NuGetProjects.Add(buildIntegratedProject);

            var packageContext = new SimpleTestPackageContext("packageA", "1.0.0-beta1");
            packageContext.AddFile("lib/net45/a.dll");
            SimpleTestPackageUtility.CreateOPCPackage(packageContext, pathContext.PackageSource);

            var sourceRepositories = sourceRepositoryProvider.GetRepositories();

            // Install
            await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, "packageA",
                resolutionContext, testNuGetProjectContext, sourceRepositories.First(), secondarySources: null, token);

            var installedPackages = (await buildIntegratedProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();

            // Main Act
            var actions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                "packageA",
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(),
                new TestNuGetProjectContext(),
                primarySources: sourceRepositories,
                secondarySources: sourceRepositories,
                CancellationToken.None);

            // Assert
            installedPackages.Count.Should().Be(1);
            actions.Should().BeEmpty();
        }

        [Fact]
        public async Task PreviewUpdatesAsync_WithStrictVersionRange()
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
                var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, randomProjectFolderPath);
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

        private static void BasicConfigWithPackage(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(ConfigWithPackage.ToString());
            }
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
                return TaskResult.True;
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

            public Task<IReadOnlyList<(string id, string[] metadata)>> GetItemsAsync(string itemTypeName, params string[] metadataNames)
            {
                throw new NotImplementedException();
            }
        }

        private class TestNonBuildIntegratedNuGetProject : NuGetProject, IDependencyGraphProject
        {
            public List<TestExternalProjectReference> ProjectReferences { get; }
                = new List<TestExternalProjectReference>();

            public string MSBuildProjectPath { get; set; }

            public DateTimeOffset LastModified { get; set; }

            public PackageSpec PackageSpec { get; set; }

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
    }
}
