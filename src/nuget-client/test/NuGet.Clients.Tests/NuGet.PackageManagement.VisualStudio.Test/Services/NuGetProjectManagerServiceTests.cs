// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using FluentAssertions;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.References;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using StreamJsonRpc;
using Test.Utility;
using Test.Utility.VisualStudio;
using Xunit;
using Xunit.Abstractions;
using static NuGet.PackageManagement.VisualStudio.Test.ProjectFactories;
using PackageReference = NuGet.Packaging.PackageReference;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public sealed class NuGetProjectManagerServiceTests : MockedVSCollectionTests, IDisposable
    {
        private NuGetPackageManager _packageManager;
        private NuGetProjectManagerService _projectManager;
        private readonly TestNuGetProjectContext _projectContext;
        private TestSharedServiceState _sharedState;
        private TestVsSolutionManager _solutionManager;
        private NuGetProjectManagerServiceState _state;
        private TestDirectory _testDirectory;
        private readonly IVsProjectThreadingService _threadingService;
        private readonly TestLogger _logger;
        private readonly Mock<IOutputConsoleProvider> _outputConsoleProviderMock;
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;

        public NuGetProjectManagerServiceTests(GlobalServiceProvider globalServiceProvider, ITestOutputHelper output)
            : base(globalServiceProvider)
        {
            _projectContext = new TestNuGetProjectContext();
            _threadingService = new TestProjectThreadingService(NuGetUIThreadHelper.JoinableTaskFactory);

            var componentModel = new Mock<IComponentModel>();
            componentModel.Setup(x => x.GetService<INuGetProjectContext>()).Returns(_projectContext);
            AddService<SComponentModel>(Task.FromResult((object)componentModel.Object));

            var mockOutputConsoleUtility = OutputConsoleUtility.GetMock();
            _outputConsoleProviderMock = mockOutputConsoleUtility.mockIOutputConsoleProvider;
            _outputConsoleProvider = new Lazy<IOutputConsoleProvider>(() => _outputConsoleProviderMock.Object);

            _logger = new TestLogger(output);
        }

        public void Dispose()
        {
            _testDirectory?.Dispose();
            _solutionManager?.Dispose();
            _state?.Dispose();
            _projectManager?.Dispose();
        }

        [Fact]
        public async Task GetInstallActionsAsync_WhenProjectNotFound_Throws()
        {
            Initialize();

            await PerformOperationAsync(async (projectManager) =>
            {
                string[] projectIds = new[] { "a" };
                var packageIdentity = new PackageIdentity(id: "b", NuGetVersion.Parse("1.0.0"));
                string[] packageSourceNames = new[] { TestSourceRepositoryUtility.V3PackageSource.Name };

                LocalRpcException exception = await Assert.ThrowsAsync<LocalRpcException>(
                    () => projectManager.GetInstallActionsAsync(
                        projectIds,
                        packageIdentity,
                        VersionConstraints.None,
                        includePrerelease: true,
                        DependencyBehavior.Lowest,
                        packageSourceNames,
                        CancellationToken.None).AsTask());

                string expectedMessage = $"A project with ID '{projectIds.Single()}' was not found.\r\nParameter name: projectIds";
                Assert.StartsWith(expectedMessage, exception.Message);
                Assert.Equal((int)RemoteErrorCode.RemoteError, exception.ErrorCode);
                Assert.IsType<RemoteError>(exception.ErrorData);

                var remoteError = (RemoteError)exception.ErrorData;
                string expectedProjectContextLogMessage = exception.InnerException.ToString();

                Assert.Null(remoteError.ActivityLogMessage);
                Assert.Equal(NuGetLogCode.Undefined, remoteError.LogMessage.Code);
                Assert.Equal(LogLevel.Error, remoteError.LogMessage.Level);
                Assert.Equal(expectedMessage, remoteError.LogMessage.Message);
                Assert.Null(remoteError.LogMessage.ProjectPath);
                Assert.InRange(remoteError.LogMessage.Time, DateTimeOffset.UtcNow.AddSeconds(-10), DateTimeOffset.UtcNow.AddSeconds(1));
                Assert.Equal(WarningLevel.Severe, remoteError.LogMessage.WarningLevel);
                Assert.Null(remoteError.LogMessages);
                Assert.Equal(expectedProjectContextLogMessage, remoteError.ProjectContextLogMessage);
                Assert.Equal(typeof(ArgumentException).FullName, remoteError.TypeName);
            });
        }

        [Fact]
        public async Task GetInstallActionsAsync_WithPackageReferenceProject_WhenUpdatingPackage_ReturnsCorrectActions()
        {
            const string projectName = "a";
            string projectId = Guid.NewGuid().ToString();
            var projectSystemCache = new ProjectSystemCache();

            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                var packageV1 = new SimpleTestPackageContext(packageId: "b", version: "1.0.0");
                var packageV2 = new SimpleTestPackageContext(packageV1.Id, version: "2.0.0");
                string packageSourceDirectoryPath = Path.Combine(testDirectory, "packageSource");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSourceDirectoryPath,
                    PackageSaveMode.Defaultv3,
                    packageV1,
                    packageV2);

                var packageSource = new PackageSource(packageSourceDirectoryPath);
                var packageSources = new List<PackageSource>() { packageSource };

                Initialize(packageSources);

                var unconfiguredProject = new Mock<UnconfiguredProject>();
                var configuredProject = new Mock<ConfiguredProject>();
                var projectServices = new Mock<ConfiguredProjectServices>();
                var packageReferencesService = new Mock<IPackageReferencesService>();
                var result = new Mock<IUnresolvedPackageReference>();

                unconfiguredProject.Setup(x => x.GetSuggestedConfiguredProjectAsync())
                    .ReturnsAsync(configuredProject.Object);

                configuredProject.SetupGet(x => x.Services)
                    .Returns(projectServices.Object);

                projectServices.SetupGet(x => x.PackageReferences)
                    .Returns(packageReferencesService.Object);

                packageReferencesService.Setup(x => x.AddAsync(It.IsNotNull<string>(), It.IsNotNull<string>()))
                    .ReturnsAsync(new AddReferenceResult<IUnresolvedPackageReference>(result.Object, added: true));

                var nuGetProjectServices = new Mock<INuGetProjectServices>();

                nuGetProjectServices.SetupGet(x => x.ScriptService)
                    .Returns(Mock.Of<IProjectScriptHostService>());

                PackageSpec packageSpec = ProjectTestHelpers.GetPackageSpec(projectName, testDirectory);
                var projectFullPath = packageSpec.RestoreMetadata.ProjectPath;

                var project = new CpsPackageReferenceProject(
                    projectName: projectName,
                    projectUniqueName: projectFullPath,
                    projectFullPath: projectFullPath,
                    projectSystemCache,
                    unconfiguredProject.Object,
                    nuGetProjectServices.Object,
                    projectId);

                DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
                projectRestoreInfo.AddProject(packageSpec);
                var projectNames = new ProjectNames(
                    fullName: projectFullPath,
                    uniqueName: projectFullPath,
                    shortName: projectName,
                    customUniqueName: projectName,
                    projectId: projectId);
                projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, Array.Empty<IAssetsLogMessage>());

                _solutionManager.NuGetProjects.Add(project);

                string[] projectIds = new[] { projectId };
                string[] packageSourceNames = new[] { packageSource.Name };

                await PerformOperationAsync(async (projectManager) =>
                {
                    IReadOnlyList<ProjectAction> actions = await projectManager.GetInstallActionsAsync(
                        projectIds,
                        packageV1.Identity,
                        VersionConstraints.None,
                        includePrerelease: true,
                        DependencyBehavior.Lowest,
                        packageSourceNames,
                        CancellationToken.None);

                    Assert.NotEmpty(actions);
                    Assert.Equal(1, actions.Count);

                    ProjectAction action = actions[0];

                    Assert.Equal(packageV1.Identity, action.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, action.ProjectActionType);
                    Assert.Equal(projectId, action.ProjectId);

                    Assert.Equal(1, action.ImplicitActions.Count);

                    ImplicitProjectAction implicitAction = action.ImplicitActions[0];

                    Assert.Equal(packageV1.Identity, implicitAction.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, implicitAction.ProjectActionType);

                    await projectManager.ExecuteActionsAsync(actions, CancellationToken.None);

                    AddPackageDependency(projectSystemCache, projectNames, packageSpec, packageV1);
                });

                await PerformOperationAsync(async (projectManager) =>
                {
                    IReadOnlyList<ProjectAction> actions = await projectManager.GetInstallActionsAsync(
                        projectIds,
                        packageV2.Identity,
                        VersionConstraints.None,
                        includePrerelease: true,
                        DependencyBehavior.Lowest,
                        packageSourceNames,
                        CancellationToken.None);

                    Assert.NotEmpty(actions);
                    Assert.Equal(1, actions.Count);

                    ProjectAction action = actions[0];

                    Assert.Equal(packageV2.Identity, action.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, action.ProjectActionType);
                    Assert.Equal(projectId, action.ProjectId);

                    Assert.Equal(2, action.ImplicitActions.Count);

                    ImplicitProjectAction implicitAction = action.ImplicitActions[0];

                    Assert.Equal(packageV1.Identity, implicitAction.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Uninstall, implicitAction.ProjectActionType);

                    implicitAction = action.ImplicitActions[1];

                    Assert.Equal(packageV2.Identity, implicitAction.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, implicitAction.ProjectActionType);
                });
            }
        }

        [Fact]
        public async Task GetInstallActionsAsync_WithPackagesConfigProject_WhenUpdatingPackage_ReturnsCorrectActions()
        {
            const string projectName = "a";
            string projectId = Guid.NewGuid().ToString();

            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                var packageV1 = new SimpleTestPackageContext(packageId: "b", version: "1.0.0");
                var packageV2 = new SimpleTestPackageContext(packageV1.Id, version: "2.0.0");
                string packageSourceDirectoryPath = Path.Combine(testDirectory, "packageSource");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSourceDirectoryPath,
                    PackageSaveMode.Defaultv3,
                    packageV1,
                    packageV2);

                var packageSource = new PackageSource(packageSourceDirectoryPath);
                var packageSources = new List<PackageSource>() { packageSource };

                Initialize(packageSources);

                string projectFullPath = Path.Combine(testDirectory.Path, $"{projectName}.csproj");
                NuGetFramework targetFramework = NuGetFramework.Parse("net46");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(targetFramework, new TestNuGetProjectContext());
                var project = new TestMSBuildNuGetProject(msBuildNuGetProjectSystem, testDirectory.Path, projectFullPath, projectId);

                _solutionManager.NuGetProjects.Add(project);

                string[] projectIds = new[] { projectId };
                string[] packageSourceNames = new[] { packageSource.Name };

                await PerformOperationAsync(async (projectManager) =>
                {
                    IReadOnlyList<ProjectAction> actions = await projectManager.GetInstallActionsAsync(
                        projectIds,
                        packageV1.Identity,
                        VersionConstraints.None,
                        includePrerelease: true,
                        DependencyBehavior.Lowest,
                        packageSourceNames,
                        CancellationToken.None);

                    Assert.NotEmpty(actions);
                    Assert.Equal(1, actions.Count);

                    ProjectAction action = actions[0];

                    Assert.Equal(packageV1.Identity, action.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, action.ProjectActionType);
                    Assert.Equal(projectId, action.ProjectId);

                    Assert.Empty(action.ImplicitActions);

                    await projectManager.ExecuteActionsAsync(actions, CancellationToken.None);
                });

                await PerformOperationAsync(async (projectManager) =>
                {
                    IReadOnlyList<ProjectAction> actions = await projectManager.GetInstallActionsAsync(
                        projectIds,
                        packageV2.Identity,
                        VersionConstraints.None,
                        includePrerelease: true,
                        DependencyBehavior.Lowest,
                        packageSourceNames,
                        CancellationToken.None);

                    Assert.NotEmpty(actions);
                    Assert.Equal(2, actions.Count);

                    ProjectAction action = actions[0];

                    Assert.Equal(packageV1.Identity, action.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Uninstall, action.ProjectActionType);
                    Assert.Equal(projectId, action.ProjectId);

                    action = actions[1];

                    Assert.Equal(packageV2.Identity, action.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, action.ProjectActionType);
                    Assert.Equal(projectId, action.ProjectId);
                });
            }
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WhenProjectReturnsNullPackageReference_NullIsRemoved()
        {
            const string projectName = "a";
            string projectId = Guid.NewGuid().ToString();

            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                Initialize();

                string projectFullPath = Path.Combine(testDirectory.Path, $"{projectName}.csproj");
                NuGetFramework targetFramework = NuGetFramework.Parse("net46");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(targetFramework, new TestNuGetProjectContext());
                var project = new TestMSBuildNuGetProject(msBuildNuGetProjectSystem, testDirectory.Path, projectFullPath, projectId);
                var packageReference = new PackageReference(
                    new PackageIdentity(id: "b", NuGetVersion.Parse("1.0.0")),
                    targetFramework);
                project.InstalledPackageReferences = Task.FromResult<IEnumerable<PackageReference>>(new[]
                {
                    null,
                    packageReference
                });

                _solutionManager.NuGetProjects.Add(project);

                var telemetrySession = new Mock<ITelemetrySession>();
                var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();

                telemetrySession
                    .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                    .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

                TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

                IReadOnlyCollection<IPackageReferenceContextInfo> packages = await _projectManager.GetInstalledPackagesAsync(
                    new[] { projectId },
                    CancellationToken.None);

                Assert.Equal(1, packages.Count);
                IPackageReferenceContextInfo expected = PackageReferenceContextInfo.Create(packageReference);
                IPackageReferenceContextInfo actual = packages.Single();

                Assert.Equal(expected.AllowedVersions, actual.AllowedVersions);
                Assert.Equal(expected.Framework, actual.Framework);
                Assert.Equal(expected.Identity, actual.Identity);
                Assert.Equal(expected.IsAutoReferenced, actual.IsAutoReferenced);
                Assert.Equal(expected.IsDevelopmentDependency, actual.IsDevelopmentDependency);
                Assert.Equal(expected.IsUserInstalled, actual.IsUserInstalled);

                Assert.Equal(1, telemetryEvents.Count);
            }
        }

        [Fact]
        public async Task GetUpdateActionsAsync_WithPackageReferenceProject_WhenUpdatingPackage_ReturnsCorrectActions()
        {
            const string projectName = "a";
            string projectId = Guid.NewGuid().ToString();
            var projectSystemCache = new ProjectSystemCache();

            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                var packageV1 = new SimpleTestPackageContext(packageId: "b", version: "1.0.0");
                var packageV2 = new SimpleTestPackageContext(packageV1.Id, version: "2.0.0");
                string packageSourceDirectoryPath = Path.Combine(testDirectory, "packageSource");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSourceDirectoryPath,
                    PackageSaveMode.Defaultv3,
                    packageV1,
                    packageV2);

                var packageSource = new PackageSource(packageSourceDirectoryPath);
                var packageSources = new List<PackageSource>() { packageSource };

                Initialize(packageSources);

                var unconfiguredProject = new Mock<UnconfiguredProject>();
                var configuredProject = new Mock<ConfiguredProject>();
                var projectServices = new Mock<ConfiguredProjectServices>();
                var packageReferencesService = new Mock<IPackageReferencesService>();
                var result = new Mock<IUnresolvedPackageReference>();

                unconfiguredProject.Setup(x => x.GetSuggestedConfiguredProjectAsync())
                    .ReturnsAsync(configuredProject.Object);

                configuredProject.SetupGet(x => x.Services)
                    .Returns(projectServices.Object);

                projectServices.SetupGet(x => x.PackageReferences)
                    .Returns(packageReferencesService.Object);

                packageReferencesService.Setup(x => x.AddAsync(It.IsNotNull<string>(), It.IsNotNull<string>()))
                    .ReturnsAsync(new AddReferenceResult<IUnresolvedPackageReference>(result.Object, added: true));

                var nuGetProjectServices = new Mock<INuGetProjectServices>();

                nuGetProjectServices.SetupGet(x => x.ScriptService)
                    .Returns(Mock.Of<IProjectScriptHostService>());

                PackageSpec packageSpec = ProjectTestHelpers.GetPackageSpec(projectName, testDirectory);
                var projectFullPath = packageSpec.RestoreMetadata.ProjectPath;

                var project = new CpsPackageReferenceProject(
                    projectName: projectName,
                    projectUniqueName: projectFullPath,
                    projectFullPath: projectFullPath,
                    projectSystemCache,
                    unconfiguredProject.Object,
                    nuGetProjectServices.Object,
                    projectId);


                DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
                projectRestoreInfo.AddProject(packageSpec);
                var projectNames = new ProjectNames(
                    fullName: projectFullPath,
                    uniqueName: projectFullPath,
                    shortName: projectName,
                    customUniqueName: projectName,
                    projectId: projectId);
                projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, Array.Empty<IAssetsLogMessage>());

                _solutionManager.NuGetProjects.Add(project);

                string[] projectIds = new[] { projectId };
                string[] packageSourceNames = new[] { packageSource.Name };

                await PerformOperationAsync(async (projectManager) =>
                {
                    IReadOnlyList<ProjectAction> actions = await projectManager.GetInstallActionsAsync(
                        projectIds,
                        packageV1.Identity,
                        VersionConstraints.None,
                        includePrerelease: true,
                        DependencyBehavior.Lowest,
                        packageSourceNames,
                        CancellationToken.None);

                    Assert.NotEmpty(actions);
                    Assert.Equal(1, actions.Count);

                    ProjectAction action = actions[0];

                    Assert.Equal(packageV1.Identity, action.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, action.ProjectActionType);
                    Assert.Equal(projectId, action.ProjectId);

                    Assert.Equal(1, action.ImplicitActions.Count);

                    ImplicitProjectAction implicitAction = action.ImplicitActions[0];

                    Assert.Equal(packageV1.Identity, implicitAction.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, implicitAction.ProjectActionType);

                    await projectManager.ExecuteActionsAsync(actions, CancellationToken.None);

                    AddPackageDependency(projectSystemCache, projectNames, packageSpec, packageV1);
                });

                await PerformOperationAsync(async (projectManager) =>
                {
                    IReadOnlyList<ProjectAction> actions = await projectManager.GetUpdateActionsAsync(
                        projectIds,
                        new[] { packageV2.Identity },
                        VersionConstraints.None,
                        includePrelease: true,
                        DependencyBehavior.Lowest,
                        packageSourceNames,
                        CancellationToken.None);

                    Assert.NotEmpty(actions);
                    Assert.Equal(1, actions.Count);

                    ProjectAction action = actions[0];

                    Assert.Equal(packageV2.Identity, action.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, action.ProjectActionType);
                    Assert.Equal(projectId, action.ProjectId);

                    Assert.Equal(2, action.ImplicitActions.Count);

                    ImplicitProjectAction implicitAction = action.ImplicitActions[0];

                    Assert.Equal(packageV1.Identity, implicitAction.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Uninstall, implicitAction.ProjectActionType);

                    implicitAction = action.ImplicitActions[1];

                    Assert.Equal(packageV2.Identity, implicitAction.PackageIdentity);
                    Assert.Equal(NuGetProjectActionType.Install, implicitAction.ProjectActionType);
                });
            }
        }

        [Fact]
        private async Task GetInstalledAndTransitivePackagesAsync_TransitiveOriginsWithLegacyPackageReferenceProject_OneTransitiveOriginAsync()
        {
            // packageA_2.15.3 -> packageB_1.0.0 -> packageC_2.1.43

            string projectId = Guid.NewGuid().ToString();

            using TestDirectory testDirectory = TestDirectory.Create();
            // Arrange
            LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, projectId, "[1.0.0, )", _threadingService);

            NullSettings settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            var packageSpecs = await testProject.GetPackageSpecsAsync(context);

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            packagesDir.Create();
            packageSource.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            Initialize(sources);

            _solutionManager.NuGetProjects.Add(testProject);

            var logger = new TestLogger();
            var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
            {
                LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
            };

            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageC", "2.1.43");
            await SimpleTestPackageUtility.CreateFullPackageAsync(
                packageSource.FullName,
                "packageB",
                "1.0.0",
                new PackageDependency[]
                {
                        new PackageDependency("packageC", VersionRange.Parse("2.1.43"))
                });
            await SimpleTestPackageUtility.CreateFullPackageAsync(
                packageSource.FullName,
                "packageA",
                "2.15.3",
                new PackageDependency[]
                {
                        new PackageDependency("packageB", VersionRange.Parse("1.0.0"))
                });

            var command = new RestoreCommand(request);
            RestoreResult result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);
            Assert.True(result.Success);

            // Act
            var installedAndTransitive = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { projectId }, includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            installedAndTransitive.InstalledPackages.Should().HaveCount(1);
            installedAndTransitive.TransitivePackages.Should().HaveCount(2);

            var transitiveOrigin = new PackageIdentity("packageA", new NuGetVersion("2.15.3"));
            ITransitivePackageReferenceContextInfo transitivePackageB = installedAndTransitive.TransitivePackages.Where(pkg => pkg.Identity.Id == "packageB").Single();
            IPackageReferenceContextInfo transitiveOriginB = transitivePackageB.TransitiveOrigins.Single();
            Assert.Equal(transitiveOrigin, transitiveOriginB.Identity);

            ITransitivePackageReferenceContextInfo transitivePackageC = installedAndTransitive.TransitivePackages.Where(pkg => pkg.Identity.Id == "packageC").Single();
            IPackageReferenceContextInfo transitiveOriginC = transitivePackageC.TransitiveOrigins.Single();
            Assert.Equal(transitiveOrigin, transitiveOriginC.Identity);
        }

        [Fact]
        private async Task GetInstalledAndTransitivePackagesAsync_WithTransitivePackageNotRestored_NoTransitivePackageInfoAsync()
        {
            // packageA_1.0.0 -> packageB_2.0.0

            var projectSystemCache = new ProjectSystemCache();
            var projectAdapter = Mock.Of<IVsProjectAdapter>();
            var projectName = "projectA";

            using var pathContext = new SimpleTestPathContext();
            Initialize();

            string projectFullPath = Path.Combine(pathContext.SolutionRoot, projectName, $"{projectName}.csproj");

            var prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            // This test PackageSpec makes the project look NuGet-restored 
            PackageSpec packageSpec = GetPackageSpec(projectName, projectFullPath, "[1.0.0, )");

            // Packages
            await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, "packageA", "1.0.0",
                new[]
                {
                    new PackageDependency("packageB", VersionRange.Parse("2.0.0"))
                });
            await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, "packageB", "2.0.0");

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            _solutionManager.NuGetProjects.Add(prProject);

            // Act I: We will not have transitive packages data

            var installedProject1 = await prProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);
            Assert.NotEmpty(installedProject1.InstalledPackages);
            Assert.Empty(installedProject1.TransitivePackages);

            var installedService1 = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { projectName }, CancellationToken.None);
            Assert.NotEmpty(installedService1.InstalledPackages);
            Assert.Empty(installedService1.TransitivePackages);

            // Now, make a NuGet-Restore
            var pajFilepath = Path.Combine(Path.GetDirectoryName(projectFullPath), "project.assets.json");
            TestRestoreRequest restoreRequest = ProjectTestHelpers.CreateRestoreRequest(packageSpec, pathContext, _logger);
            restoreRequest.LockFilePath = pajFilepath;
            restoreRequest.ProjectStyle = ProjectStyle.PackageReference;
            var command = new RestoreCommand(restoreRequest);
            var resultA = await command.ExecuteAsync();
            await resultA.CommitAsync(_logger, CancellationToken.None);
            Assert.True(resultA.Success);
            Assert.True(File.Exists(pajFilepath));

            // Act II: From this point, we will have transitive packages

            var installedProject2 = await prProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);
            Assert.NotEmpty(installedProject2.InstalledPackages);
            Assert.NotEmpty(installedProject2.TransitivePackages);

            var installedService2 = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { projectName }, CancellationToken.None);
            Assert.NotEmpty(installedService2.InstalledPackages);
            Assert.NotEmpty(installedProject2.TransitivePackages);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        private async Task GetInstalledAndTransitivePackagesAsync_TransitiveOriginsWithLegacyPackageReferenceProject_MultipleOriginsAsync(bool useSameVersions)
        {
            // case useSameversions = true
            // packageX_3.0.0 -> packageD_0.1.1
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1
            // case useSameversions = false
            // packageX_3.0.0 -> packageD_0.1.2
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1

            string projectId = Guid.NewGuid().ToString();

            using TestDirectory testDirectory = TestDirectory.Create();
            // Setup
            var onedep = new[]
            {
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            "packageA",
                            VersionRange.Parse("[2.0.0, )"),
                            LibraryDependencyTarget.Package)
                    },
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            "packageX",
                            VersionRange.Parse("[3.0.0, )"),
                            LibraryDependencyTarget.Package)
                    },
                };

            LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, projectId, _threadingService, onedep);

            NullSettings settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            var packageSpecs = await testProject.GetPackageSpecsAsync(context);

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            packagesDir.Create();
            packageSource.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            Initialize(sources);

            _solutionManager.NuGetProjects.Add(testProject);

            var logger = _logger;
            var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
            {
                LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
            };

            await CreatePackages(packageSource.FullName, useSameVersions);

            // Act
            var command = new RestoreCommand(request);
            RestoreResult result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);
            Assert.True(result.Success);
            var installedAndTransitive = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { projectId }, includeTransitiveOrigins: true, CancellationToken.None);

            // Verify transitive package B
            var transitivePackageB = installedAndTransitive.TransitivePackages.Where(pkg => pkg.Identity.Id == "packageB").First();
            Assert.NotNull(transitivePackageB);
            Assert.Equal(1, transitivePackageB.TransitiveOrigins.Count());
            var transitiveOriginB = transitivePackageB.TransitiveOrigins.First();
            Assert.Equal("packageA", transitiveOriginB.Identity.Id);
            Assert.Equal(new NuGetVersion("2.0.0"), transitiveOriginB.Identity.Version);

            // Verify transitive package C
            var transitivePackageC = installedAndTransitive.TransitivePackages.Where(pkg => pkg.Identity.Id == "packageC").First();
            Assert.NotNull(transitivePackageC);
            Assert.Equal(1, transitivePackageC.TransitiveOrigins.Count());
            var transitiveOriginC = transitivePackageC.TransitiveOrigins.First();
            Assert.Equal("packageA", transitiveOriginC.Identity.Id);
            Assert.Equal(new NuGetVersion("2.0.0"), transitiveOriginC.Identity.Version);

            // Verify transitive package D
            var transitivePackageD = installedAndTransitive.TransitivePackages.Where(pkg => pkg.Identity.Id == "packageD").First();
            Assert.NotNull(transitivePackageD);
            Assert.Equal(2, transitivePackageD.TransitiveOrigins.Count()); // Two top dependencies
            Assert.Collection(transitivePackageD.TransitiveOrigins,
                x => Assert.Equal(x.Identity, new PackageIdentity("packageA", NuGetVersion.Parse("2.0.0"))),
                x => Assert.Equal(x.Identity, new PackageIdentity("packageX", NuGetVersion.Parse("3.0.0"))));
        }

        [Fact]
        private async Task GetInstalledAndTransitivePackagesAsync_WithCpsPackageReferenceProject_OneTransitiveReferenceAsync()
        {
            // packageA_2.0.0 -> packageB_1.0.0

            string projectName = Guid.NewGuid().ToString();
            string projectId = projectName;
            var projectSystemCache = new ProjectSystemCache();
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();

            using TestDirectory testDirectory = TestDirectory.Create();
            Initialize();

            // Prepare: Create project
            string projectFullPath = Path.Combine(testDirectory.Path, $"{projectName}.csproj");

            var prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath,
                projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            PackageSpec packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            packagesDir.Create();
            packageSource.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            var logger = new TestLogger();
            var pajFilepath = Path.Combine(testDirectory, "project.assets.json");
            var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
            {
                LockFilePath = pajFilepath
            };

            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageB", "1.0.0");
            await SimpleTestPackageUtility.CreateFullPackageAsync(
                packageSource.FullName,
                "packageA",
                "2.0.0",
                new PackageDependency[]
                {
                        new PackageDependency("packageB", VersionRange.Parse("1.0.0"))
                });

            _solutionManager.NuGetProjects.Add(prProject);

            // Prepare: Create telemetry
            var telemetrySession = new Mock<ITelemetrySession>();
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();

            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            // Prepare: Force a nuget Restore
            var command = new RestoreCommand(request);
            // Force writing project.assets.json
            var result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            Assert.True(result.Success);
            Assert.True(File.Exists(pajFilepath));

            // Act
            var installedAndTransitive = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { projectId }, includeTransitiveOrigins: true, CancellationToken.None);

            var packagesB = installedAndTransitive
                .TransitivePackages
                .First(pkg => pkg.Identity.Id == "packageB")
                .TransitiveOrigins;

            // Verify
            Assert.Equal(1, packagesB.Count());
            Assert.Collection(packagesB,
                pkg => AssertElement(pkg, "packageA", "2.0.0"));
        }

        [Fact]
        private async Task GetInstalledAndTransitivePackagesAsync_TransitiveOriginsWithCpsPackageReferenceProjectAndMultipleCalls_SucceedsAsync()
        {
            // packageX_3.0.0 -> packageD_0.1.1
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1

            string projectName = Guid.NewGuid().ToString();
            string projectId = projectName;
            var projectSystemCache = new ProjectSystemCache();
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();

            using TestDirectory testDirectory = TestDirectory.Create();
            Initialize();

            // Prepare: Create project
            string projectFullPath = Path.Combine(testDirectory.Path, $"{projectName}.csproj");

            var prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath,
                projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            string referenceSpec = $@"
                {{
                    ""frameworks"":
                    {{
                        ""net5.0"":
                        {{
                            ""dependencies"":
                            {{
                                ""packageA"":
                                {{
                                    ""version"": ""2.0.0"",
                                    ""target"": ""Package""
                                }},
                                ""packageX"":
                                {{
                                    ""version"": ""3.0.0"",
                                    ""target"": ""Package""
                                }}
                            }}
                        }}
                    }}
                }}";
            PackageSpec packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, projectFullPath).WithTestRestoreMetadata();

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            packagesDir.Create();
            packageSource.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            var logger = new TestLogger();
            var pajFilepath = Path.Combine(testDirectory, "project.assets.json");
            var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
            {
                LockFilePath = pajFilepath
            };

            await CreatePackages(packageSource.FullName);

            _solutionManager.NuGetProjects.Add(prProject);

            // Prepare: Create telemetry
            var telemetrySession = new Mock<ITelemetrySession>();
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();

            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            // Prepare: Force a nuget Restore
            var command = new RestoreCommand(request);
            // Force writing project.assets.json
            var result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            Assert.True(result.Success);
            Assert.True(File.Exists(pajFilepath));

            // Act
            var installedAndTransitive = await _projectManager.GetInstalledAndTransitivePackagesAsync(
                new[] { projectId },
                includeTransitiveOrigins: true,
                CancellationToken.None);

            Assert.Equal(2, installedAndTransitive.InstalledPackages.Count);
            Assert.Equal(3, installedAndTransitive.TransitivePackages.Count);

            // Act I
            var topPackagesB = installedAndTransitive
                .TransitivePackages
                .First(x => x.Identity.Id == "packageB")
                .TransitiveOrigins;

            // Verify I
            Assert.NotNull(topPackagesB);
            Assert.Equal(1, topPackagesB.Count()); // only one top dependency
            var dep = topPackagesB.First();
            Assert.Equal("packageA", dep.Identity.Id);
            Assert.Equal(new NuGetVersion("2.0.0"), dep.Identity.Version);

            // Act II
            var topPackagesD = installedAndTransitive
                .TransitivePackages
                .First(x => x.Identity.Id == "packageD")
                .TransitiveOrigins;

            // Verify II
            Assert.NotNull(topPackagesD);
            Assert.Equal(2, topPackagesD.Count()); // two top packages
            Assert.Collection(topPackagesD,
                x => AssertElement(x, "packageA", "2.0.0"),
                x => AssertElement(x, "packageX", "3.0.0"));

            // Act III: Call to another APIs
            IReadOnlyCollection<IPackageReferenceContextInfo> installed = await _projectManager.GetInstalledPackagesAsync(new[] { projectId }, CancellationToken.None);

            // Verify III
            Assert.Equal(2, installed.Count);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        private async Task GetInstalledAndTransitivePackagesAsync_TransitiveOriginsWithCpsPackageReferenceProjectAndMultitargeting_SucceedsAsync(bool useSameVersions)
        {
            // useSameVersion = true
            // net5.0:
            // packageX_3.0.0 -> packageD_0.1.1
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1
            // net472:
            // packageX_3.0.0 -> packageD_0.1.1

            // useSameVersion = false
            // net5.0:
            // packageX_3.0.0 -> packageD_0.1.2
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1
            // net472:
            // packageX_4.0.0 -> packageD_0.1.2

            string projectName = Guid.NewGuid().ToString();
            string projectId = projectName;
            var projectSystemCache = new ProjectSystemCache();
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();

            using var testDirectory = TestDirectory.Create();
            Initialize();

            // Prepare: Create project
            string projectFullPath = Path.Combine(testDirectory.Path, $"{projectName}.csproj");

            var prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath,
                projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            string referenceSpec = $@"
                {{
                    ""frameworks"":
                    {{
                        ""net5.0"":
                        {{
                            ""dependencies"":
                            {{
                                ""packageA"":
                                {{
                                    ""version"": ""2.0.0"",
                                    ""target"": ""Package""
                                }},
                                ""packageX"":
                                {{
                                    ""version"": ""3.0.0"",
                                    ""target"": ""Package""
                                }}
                            }}
                        }},
                        ""net472"":
                        {{
                            ""dependencies"":
                            {{
                                ""packageX"":
                                {{
                                    ""version"": ""{(useSameVersions ? "3.0.0" : "4.0.0")}"",
                                    ""target"": ""Package""
                                }}
                            }}
                        }}
                    }}
                }}";
            PackageSpec packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, projectFullPath).WithTestRestoreMetadata();

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            packagesDir.Create();
            packageSource.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            var logger = new TestLogger();
            var pajFilepath = Path.Combine(testDirectory, "project.assets.json");
            var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
            {
                LockFilePath = pajFilepath
            };

            await CreatePackages(packageSource.FullName, useSameVersions);

            _solutionManager.NuGetProjects.Add(prProject);

            // Prepare: Create telemetry
            var telemetrySession = new Mock<ITelemetrySession>();
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();

            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            // Prepare: Force a nuget Restore
            var command = new RestoreCommand(request);
            // Force writing project.assets.json
            var result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            Assert.True(result.Success);
            Assert.True(File.Exists(pajFilepath));

            var installedAndTransitive = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { projectId }, includeTransitiveOrigins: true, CancellationToken.None);

            // Act I
            var topPackagesB = installedAndTransitive
                .TransitivePackages
                .First(pkg => pkg.Identity.Id == "packageB")
                .TransitiveOrigins;

            // Verify I
            Assert.Equal(1, topPackagesB.Count()); // only one framework/RID pair
            Assert.Collection(topPackagesB,
                x => AssertElement(x, "packageA", "2.0.0"));

            // Act II
            var topPackagesD = installedAndTransitive
                .TransitivePackages
                .First(pkg => pkg.Identity.Id == "packageD")
                .TransitiveOrigins;


            // Verify II
            Assert.Equal(2, topPackagesD.Count()); // multitargeting: 2 keys
            if (useSameVersions)
            {
                Assert.Collection(topPackagesD,
                    x => AssertElement(x, "packageA", "2.0.0"),
                    x => AssertElement(x, "packageX", "3.0.0"));
            }
            else
            {
                Assert.Collection(topPackagesD,
                    x => AssertElement(x, "packageA", "2.0.0", "net5.0"),
                    x => AssertElement(x, "packageX", "3.0.0", "net5.0")); // multitargeting brings this version
            }
        }

        [Fact]
        private async Task GetInstalledAndTransitivePackagesAsync_InvalidInput_ThrowsAsync()
        {
            Initialize();

            Exception ex1 = await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                // Throws Assumes.InternalErrorException, but that class is private
                _ = await _projectManager.GetInstalledAndTransitivePackagesAsync(null, CancellationToken.None);
            });
            Assert.Equal(ex1.GetType().FullName, "Microsoft.Assumes+InternalErrorException");

            Exception ex2 = await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                // Throws Assumes.InternalErrorException, but that class is private
                _ = await _projectManager.GetInstalledAndTransitivePackagesAsync(new string[] { }, CancellationToken.None);
            });
            Assert.Equal(ex2.GetType().FullName, "Microsoft.Assumes+InternalErrorException");

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Project not found
                _ = await _projectManager.GetInstalledAndTransitivePackagesAsync(new string[] { "abc" }, CancellationToken.None);
            });
        }

        [Fact]
        private async Task GetInstalledAndTransitivePackagesAsync_WithCancellationToken_ThrowsAsync()
        {
            Initialize();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                using var cts = new CancellationTokenSource();
                cts.Cancel();
                _ = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { "aProjectId" }, cts.Token);
            });
        }

        [Fact]
        private async Task GetInstalledAndTransitivePackagesAsync_TransitiveOriginsWithCpsPackageReferenceProjectAndMultitargetingMultipleCalls_MergedResultsAsync()
        {
            // net5.0:
            // packageX_3.0.0 -> packageD_0.1.1
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1
            //
            // net472:
            // packageX_3.0.0 -> packageD_0.1.1

            string projectName = Guid.NewGuid().ToString();
            string projectId = projectName;
            var projectSystemCache = new ProjectSystemCache();
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();

            using TestDirectory testDirectory = TestDirectory.Create();
            Initialize();

            // Prepare: Create project
            string projectFullPath = Path.Combine(testDirectory.Path, $"{projectName}.csproj");

            var prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath,
                projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            string referenceSpec = $@"
                {{
                    ""frameworks"":
                    {{
                        ""net5.0"":
                        {{
                            ""dependencies"":
                            {{
                                ""packageA"":
                                {{
                                    ""version"": ""2.0.0"",
                                    ""target"": ""Package""
                                }},
                                ""packageX"":
                                {{
                                    ""version"": ""3.0.0"",
                                    ""target"": ""Package""
                                }}
                            }}
                        }},
                        ""net472"":
                        {{
                            ""dependencies"":
                            {{
                                ""packageX"":
                                {{
                                    ""version"": ""3.0.0"",
                                    ""target"": ""Package""
                                }}
                            }}
                        }}
                    }},
                    ""runtimes"":
                    {{
                        ""win10-x86"": {{}},
                        ""win7-x86"": {{}},
                        ""win8-x86"": {{}}
                    }}
                }}";
            PackageSpec packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, projectFullPath).WithTestRestoreMetadata();

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            packagesDir.Create();
            packageSource.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            var logger = new TestLogger();
            var pajFilepath = Path.Combine(testDirectory, "project.assets.json");
            var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
            {
                LockFilePath = pajFilepath
            };

            await CreatePackages(packageSource.FullName);

            _solutionManager.NuGetProjects.Add(prProject);

            // Prepare: Create telemetry
            var telemetrySession = new Mock<ITelemetrySession>();
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();

            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            // Prepare: Force a nuget Restore
            var command = new RestoreCommand(request);
            // Force writing project.assets.json
            var result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            Assert.True(result.Success);
            Assert.True(File.Exists(pajFilepath));

            // Act
            var installedAndTransitive = await _projectManager.GetInstalledAndTransitivePackagesAsync(new[] { projectId }, includeTransitiveOrigins: true, CancellationToken.None);

            // Act I
            var topPackagesB = installedAndTransitive
                .TransitivePackages
                .First(pkg => pkg.Identity.Id == "packageB")
                .TransitiveOrigins;

            // Verify I
            Assert.NotNull(topPackagesB);
            Assert.NotEmpty(topPackagesB);
            Assert.Equal(1, topPackagesB.Count()); // 3 fw/RID pairs + 1 fw/null-RID pair = 4 elements
            Assert.Collection(topPackagesB,
                x => AssertElement(x, "packageA", "2.0.0"));

            // Act II
            var topPackagesD = installedAndTransitive
                .TransitivePackages
                .First(pkg => pkg.Identity.Id == "packageD")
                .TransitiveOrigins;

            // Verify II
            Assert.NotNull(topPackagesD);
            Assert.Equal(2, topPackagesD.Count()); // Multitargeting, merged into two dependencies
            Assert.Collection(topPackagesD,
                x => AssertElement(x, "packageA", "2.0.0"),
                x => AssertElement(x, "packageX", "3.0.0"));
        }

        [Fact]
        private async Task GetPackageFoldersAsync_InvalidInput_ThrowsAsync()
        {
            Initialize();

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _projectManager.GetPackageFoldersAsync(null, CancellationToken.None);
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _projectManager.GetPackageFoldersAsync(new[] { "unknownProject" }, CancellationToken.None);
            });
        }

        [Fact]
        private async Task GetPackageFoldersAsync_WithCancellationToken_ThowsAsync()
        {
            Initialize();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                using var cts = new CancellationTokenSource();
                cts.Cancel();
                await _projectManager.GetPackageFoldersAsync(new[] { "unknownProject" }, cts.Token);
            });
        }

        [Fact]
        private async Task GetPackageFoldersAsync_CpsProject_ReturnsPackageFolderAsync()
        {
            string projectName = Guid.NewGuid().ToString();
            string projectId = projectName;
            var projectSystemCache = new ProjectSystemCache();
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();

            using var pathContext = new SimpleTestPathContext();
            Initialize();

            // Prepare: Create project
            string projectFullPath = Path.Combine(pathContext.SolutionRoot, projectName, $"{projectName}.csproj");

            CpsPackageReferenceProject prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            string referenceSpec = $@"
                {{
                    ""frameworks"":
                    {{
                        ""net6.0"":
                        {{
                            ""dependencies"":
                            {{
                            }}
                        }}
                    }}
                }}";
            PackageSpec packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, projectFullPath).WithTestRestoreMetadata();

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            _solutionManager.NuGetProjects.Add(prProject);

            // Perform NuGet restore
            var pajFilepath = Path.Combine(Path.GetDirectoryName(projectFullPath), "project.assets.json");
            TestRestoreRequest restoreRequest = ProjectTestHelpers.CreateRestoreRequest(packageSpec, pathContext, _logger); // Adds 1 source
            restoreRequest.LockFilePath = pajFilepath;
            restoreRequest.ProjectStyle = ProjectStyle.PackageReference;
            var command = new RestoreCommand(restoreRequest);
            var resultA = await command.ExecuteAsync();
            await resultA.CommitAsync(_logger, CancellationToken.None);
            Assert.True(resultA.Success);
            Assert.True(File.Exists(pajFilepath));

            // Act
            IReadOnlyCollection<string> folders = await _projectManager.GetPackageFoldersAsync(new[] { projectId }, CancellationToken.None);

            // Assert
            Assert.Equal(1, folders.Count); // only globalPackagesFolder is listed
        }

        [Fact]
        private async Task GetPackageFoldersAsync_LegacyProject_ReturnsPackageFolderAsync()
        {
            string projectId = Guid.NewGuid().ToString();

            using TestDirectory testDirectory = TestDirectory.Create();
            // Arrange
            LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, projectId, "[1.0.0, )", _threadingService);

            NullSettings settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(_logger, settings);

            var packageSpecs = await testProject.GetPackageSpecsAsync(context);

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            packagesDir.Create();
            packageSource.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            Initialize(sources);

            _solutionManager.NuGetProjects.Add(testProject);

            var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, _logger)
            {
                LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
            };

            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "1.0.0", new PackageDependency[] { });

            var command = new RestoreCommand(request);
            RestoreResult result = await command.ExecuteAsync();
            await result.CommitAsync(_logger, CancellationToken.None);
            Assert.True(result.Success);

            // Act
            var folders = await _projectManager.GetPackageFoldersAsync(new[] { projectId }, CancellationToken.None);

            // Assert
            Assert.Equal(1, folders.Count);
        }

        [Fact]
        private async Task GetPackageFoldersAsync_LegacyProjectWithFallbackFolder_ReturnsPackageFoldersAsync()
        {
            string projectId = Guid.NewGuid().ToString();

            using TestDirectory testDirectory = TestDirectory.Create();
            // Arrange
            LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, projectId, "[1.0.0, )", _threadingService);

            NullSettings settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(_logger, settings);

            var packageSpecs = await testProject.GetPackageSpecsAsync(context);

            // Package directories
            var sources = new List<PackageSource>();
            var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
            var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
            var fallbackFolder = new DirectoryInfo(Path.Combine(testDirectory, "fallbackFolder"));
            packagesDir.Create();
            packageSource.Create();
            fallbackFolder.Create();
            sources.Add(new PackageSource(packageSource.FullName));

            Initialize(sources);

            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "1.0.0", new PackageDependency[] { });

            _solutionManager.NuGetProjects.Add(testProject);

            var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, new[] { fallbackFolder.FullName }, _logger)
            {
                LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
            };

            var command = new RestoreCommand(request);
            RestoreResult result = await command.ExecuteAsync();
            await result.CommitAsync(_logger, CancellationToken.None);
            Assert.True(result.Success);

            // Act
            var folders = await _projectManager.GetPackageFoldersAsync(new[] { projectId }, CancellationToken.None);

            // Assert
            Assert.Equal(2, folders.Count);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        private async Task GetCentralPackageVersionsManagmentEnabled_SucceedsAsync(bool isCentralPackageVersionsEnabled)
        {
            string projectName = Guid.NewGuid().ToString();
            string projectId = projectName;
            var projectSystemCache = new ProjectSystemCache();
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();

            using var pathContext = new SimpleTestPathContext();
            Initialize();

            // Prepare: Create project
            string projectFullPath = Path.Combine(pathContext.SolutionRoot, projectName, $"{projectName}.csproj");

            CpsPackageReferenceProject prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            string referenceSpec = $@"
                {{
                    ""frameworks"":
                    {{
                        ""net6.0"":
                        {{
                            ""dependencies"":
                            {{
                            }}
                        }}
                    }}
                }}";
            PackageSpec packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, projectFullPath).WithTestRestoreMetadata();
            packageSpec.RestoreMetadata.CentralPackageVersionsEnabled = isCentralPackageVersionsEnabled;

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            _solutionManager.NuGetProjects.Add(prProject);

            // Act
            bool isCentralPackageManagmentEnabled = await _projectManager.IsCentralPackageManagementEnabledAsync(projectId, CancellationToken.None);

            // Assert
            Assert.Equal(isCentralPackageVersionsEnabled, isCentralPackageManagmentEnabled);
        }

        [Fact]
        private async Task GetPackageFoldersAsync_CpsProjectWithFallbackFolder_ReturnsPackageFoldersAsync()
        {
            string projectName = Guid.NewGuid().ToString();
            string projectId = projectName;
            var projectSystemCache = new ProjectSystemCache();
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();

            using var pathContext = new SimpleTestPathContext();
            Initialize();

            // Prepare: Create project
            string projectFullPath = Path.Combine(pathContext.SolutionRoot, projectName, $"{projectName}.csproj");

            CpsPackageReferenceProject prProject = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectSystemCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            string referenceSpec = $@"
                {{
                    ""frameworks"":
                    {{
                        ""net6.0"":
                        {{
                            ""dependencies"":
                            {{
                            }}
                        }}
                    }}
                }}";
            PackageSpec packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, projectFullPath).WithTestRestoreMetadata();

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            projectSystemCache.AddProject(projectNames, projectAdapter, prProject).Should().BeTrue();

            _solutionManager.NuGetProjects.Add(prProject);

            var sources = new List<PackageSource>();


            // Perform NuGet restore
            var pajFilepath = Path.Combine(Path.GetDirectoryName(projectFullPath), "project.assets.json");
            var request = new TestRestoreRequest(packageSpec, sources, pathContext.PackageSource, new[] { pathContext.FallbackFolder }, _logger)
            {
                LockFilePath = pajFilepath,
                ProjectStyle = ProjectStyle.PackageReference
            };
            var command = new RestoreCommand(request);
            var resultA = await command.ExecuteAsync();
            await resultA.CommitAsync(_logger, CancellationToken.None);
            Assert.True(resultA.Success);
            Assert.True(File.Exists(pajFilepath));

            // Act
            IReadOnlyCollection<string> folders = await _projectManager.GetPackageFoldersAsync(new[] { projectId }, CancellationToken.None);

            // Assert
            Assert.Equal(2, folders.Count);
        }

        private void AssertElement(IPackageReferenceContextInfo pkg, string id, string version, string framework = null)
        {
            Assert.Equal(id, pkg.Identity.Id);
            Assert.Equal(NuGetVersion.Parse(version), pkg.Identity.Version);
            if (!string.IsNullOrEmpty(framework))
            {
                Assert.Equal(NuGetFramework.Parse(framework), pkg.Framework);
            }
        }

        private static void AddPackageDependency(ProjectSystemCache projectSystemCache, ProjectNames projectNames, PackageSpec packageSpec, SimpleTestPackageContext package)
        {
            var dependency = new LibraryDependency()
            {
                LibraryRange = new LibraryRange(
                    name: package.Id,
                    versionRange: new VersionRange(package.Identity.Version),
                    typeConstraint: LibraryDependencyTarget.Package)
            };

            packageSpec.TargetFrameworks.First().Dependencies.Add(dependency);
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            projectSystemCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, Array.Empty<IAssetsLogMessage>());
        }

        private void Initialize(IReadOnlyList<PackageSource> packageSources = null)
        {
            SourceRepositoryProvider sourceRepositoryProvider;

            if (packageSources == null || packageSources.Count == 0)
            {
                sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            }
            else
            {
                sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(packageSources);
            }

            _solutionManager = new TestVsSolutionManager();
            _testDirectory = TestDirectory.Create();
            ISettings testSettings = CreateSettings(sourceRepositoryProvider, _testDirectory);
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            _packageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                _solutionManager,
                deleteOnRestartManager);
            _state = new NuGetProjectManagerServiceState();
            _sharedState = new TestSharedServiceState(
                new Microsoft.VisualStudio.Threading.AsyncLazy<NuGetPackageManager>(
                    () => Task.FromResult(_packageManager)),
                new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolutionManager>(
                    () => Task.FromResult<IVsSolutionManager>(_solutionManager)),
                sourceRepositoryProvider,
                new Microsoft.VisualStudio.Threading.AsyncLazy<IReadOnlyCollection<SourceRepository>>(
                    () => Task.FromResult<IReadOnlyCollection<SourceRepository>>(sourceRepositoryProvider.GetRepositories().ToList())));
            _projectManager = new NuGetProjectManagerService(
                default(ServiceActivationOptions),
                Mock.Of<IServiceBroker>(),
                new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                _state,
                _sharedState);
        }

        private async Task PerformOperationAsync(Func<NuGetProjectManagerService, Task> testAsync)
        {
            await _projectManager.BeginOperationAsync(CancellationToken.None);

            try
            {
                await testAsync(_projectManager);
            }
            finally
            {
                await _projectManager.EndOperationAsync(CancellationToken.None);
            }
        }

        private static ISettings CreateSettings(
            SourceRepositoryProvider sourceRepositoryProvider,
            TestDirectory settingsDirectory)
        {
            var settings = new Settings(settingsDirectory);

            foreach (SourceRepository packageSource in sourceRepositoryProvider.GetRepositories())
            {
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, packageSource.PackageSource.AsSourceItem());
            }

            return settings;
        }

        private static async Task CreatePackages(string packageSourceDir, bool useSameVersions = true)
        {
            // Package Graph. -> means 'depends on'
            //
            // case useSameversions = true
            // packageX_3.0.0 -> packageD_0.1.1
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1
            // case useSameversions = false
            // packageX_4.0.0 -> packageD_0.1.2
            // packageX_3.0.0 -> packageD_0.1.2
            // packageA_2.0.0 -> packageB_1.0.0 -> packageC_0.0.1
            //                                  -> packageD_0.1.1

            if (!useSameVersions)
            {
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSourceDir, "packageD", "0.1.2");
            }
            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSourceDir, "packageD", "0.1.1");
            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSourceDir, "packageC", "0.0.1");
            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSourceDir, "packageB", "1.0.0",
                new PackageDependency[]
                {
                        new PackageDependency("packageC", VersionRange.Parse("0.0.1")),
                        new PackageDependency("packageD", VersionRange.Parse("0.1.1")),
                });
            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSourceDir, "packageA", "2.0.0",
                new PackageDependency[]
                {
                        new PackageDependency("packageB", VersionRange.Parse("1.0.0"))
                });
            await SimpleTestPackageUtility.CreateFullPackageAsync(packageSourceDir, "packageX", "3.0.0",
                new PackageDependency[]
                {
                        new PackageDependency("packageD", VersionRange.Parse(useSameVersions? "0.1.1" : "0.1.2"))
                });

            if (!useSameVersions)
            {
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSourceDir, "packageX", "4.0.0",
                new PackageDependency[]
                {
                        new PackageDependency("packageD", VersionRange.Parse(useSameVersions? "0.1.1" : "0.1.2"))
                });
            }
        }

        private sealed class TestMSBuildNuGetProject : MSBuildNuGetProject
        {
            internal Task<IEnumerable<PackageReference>> InstalledPackageReferences { get; set; }

            public TestMSBuildNuGetProject(
                IMSBuildProjectSystem msbuildProjectSystem,
                string folderNuGetProjectPath,
                string packagesConfigFolderPath,
                string projectId) : base(
                    msbuildProjectSystem,
                    folderNuGetProjectPath,
                    packagesConfigFolderPath)
            {
                InternalMetadata[NuGetProjectMetadataKeys.ProjectId] = projectId;
            }

            public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
            {
                return InstalledPackageReferences ?? base.GetInstalledPackagesAsync(token);
            }
        }

        private sealed class TestVsSolutionManager : IVsSolutionManager, IDisposable
        {
            private readonly TestDirectory _directory;

            public List<NuGetProject> NuGetProjects { get; set; } = new List<NuGetProject>();

            public string DefaultNuGetProjectName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public bool IsInitialized => throw new NotImplementedException();

            public Task InitializationTask { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public string SolutionDirectory => _directory.Path;

            public CancellationToken VsShutdownToken => CancellationToken.None;

            public bool IsSolutionOpen => throw new NotImplementedException();

            public INuGetProjectContext NuGetProjectContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

#pragma warning disable CS0067
            public event EventHandler SolutionOpening;
            public event EventHandler SolutionOpened;
            public event EventHandler SolutionClosing;
            public event EventHandler SolutionClosed;
            public event EventHandler<NuGetEventArgs<string>> AfterNuGetCacheUpdated;
            public event EventHandler<NuGetProjectEventArgs> NuGetProjectAdded;
            public event EventHandler<NuGetProjectEventArgs> NuGetProjectRemoved;
            public event EventHandler<NuGetProjectEventArgs> NuGetProjectRenamed;
            public event EventHandler<NuGetProjectEventArgs> NuGetProjectUpdated;
            public event EventHandler<NuGetProjectEventArgs> AfterNuGetProjectRenamed;
            public event EventHandler<ActionsExecutedEventArgs> ActionsExecuted;
#pragma warning restore CS0067

            internal TestVsSolutionManager()
            {
                _directory = TestDirectory.Create();
            }

            public void Dispose()
            {
                _directory.Dispose();

                GC.SuppressFinalize(this);
            }

            public Task<bool> DoesNuGetSupportsAnyProjectAsync()
            {
                throw new NotImplementedException();
            }

            public void EnsureSolutionIsLoaded()
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<IVsProjectAdapter>> GetAllVsProjectAdaptersAsync()
            {
                throw new NotImplementedException();
            }

            public Task<NuGetProject> GetDefaultNuGetProjectAsync()
            {
                throw new NotImplementedException();
            }

            public Task<NuGetProject> GetNuGetProjectAsync(string nuGetProjectSafeName)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetNuGetProjectSafeNameAsync(NuGetProject nuGetProject)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<NuGetProject>> GetNuGetProjectsAsync()
            {
                return Task.FromResult<IEnumerable<NuGetProject>>(NuGetProjects);
            }

            public Task<NuGetProject> GetOrCreateProjectAsync(Project project, INuGetProjectContext projectContext)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetSolutionFilePathAsync()
            {
                throw new NotImplementedException();
            }

            public Task<IVsProjectAdapter> GetVsProjectAdapterAsync(string name)
            {
                throw new NotImplementedException();
            }

            public Task<IVsProjectAdapter> GetVsProjectAdapterAsync(NuGetProject project)
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsAllProjectsNominatedAsync()
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsSolutionAvailableAsync()
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsSolutionFullyLoadedAsync()
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsSolutionOpenAsync()
            {
                throw new NotImplementedException();
            }

            public void OnActionsExecuted(IEnumerable<ResolvedAction> actions)
            {
                throw new NotImplementedException();
            }

            public Task<NuGetProject> UpgradeProjectToPackageReferenceAsync(NuGetProject project)
            {
                throw new NotImplementedException();
            }

            public IReadOnlyList<object> GetAllProjectRestoreInfoSources()
            {
                throw new NotImplementedException();
            }

            public Task<string> GetSolutionDirectoryAsync()
            {
                return Task.FromResult(_directory.Path);
            }
        }
    }
}
