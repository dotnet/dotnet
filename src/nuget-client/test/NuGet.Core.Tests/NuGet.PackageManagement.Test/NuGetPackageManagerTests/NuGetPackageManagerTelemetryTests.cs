// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

#if NETFRAMEWORK

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio.Telemetry;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.Test.NuGetPackageManagerTests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class TelemetryTests
    {
        // Following are the various sets of packages that are small in size. To be used by the functional tests
        private readonly List<PackageIdentity> _packageWithDependents = new List<PackageIdentity>
            {
                new PackageIdentity("jQuery", new NuGetVersion("1.4.4")),
                new PackageIdentity("jQuery", new NuGetVersion("1.6.4")),
                new PackageIdentity("jQuery.Validation", new NuGetVersion("1.13.1")),
                new PackageIdentity("jQuery.UI.Combined", new NuGetVersion("1.11.2"))
            };

        private readonly XunitLogger _logger;

        public TelemetryTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
        }

        [Fact]
        public async Task PreviewInstallPackage_PackagesConfig_RaiseTelemetryEvents()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), [new PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0)))], true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), [], true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var nugetProject = solutionManager.AddNewMSBuildProject();

                // Main Act
                var target = new PackageIdentity("a", new NuGetVersion(1, 0, 0));

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    nugetProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                telemetryEvents.Count.Should().BeGreaterThan(0);
                VerifyPreviewActionsTelemetryEvents_PackagesConfig(telemetryEvents.Select(p => (string)p["SubStepName"]));
            }
        }

        [Fact]
        public async Task PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEvents()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestVSSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(createEmpty: true);

                // Main Act
                var target = _packageWithDependents[0];

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    buildIntegratedProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation"));
                Assert.Equal(1, telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName));

                Assert.Contains(telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName), p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && (y.EndsWith(".csproj") || y.EndsWith("proj"))));
            }
        }



        [Fact]
        public async Task PreviewInstallPackage_VersionNotInRange_RaiseTelemetryEventsWithErrorCodeNU1102()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var solutionManager = new TestSolutionManager(pathContext);

            var projectName = "testproj";
            var projectTargetFramework = NuGetFramework.Parse("uap10.0");
            var projectFolder = new DirectoryInfo(Path.Combine(solutionManager.SolutionDirectory, projectName));

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            var testLogger = new TestLogger();

            var nuGetVersioningPackageContext = new SimpleTestPackageContext("NuGet.Versioning", "1.0.7");
            nuGetVersioningPackageContext.AddFile("lib/uap10.0/NuGet.Versioning.dll");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, nuGetVersioningPackageContext);
            PackageSource packageSource = new PackageSource(pathContext.PackageSource);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(packageSource);
            var sources = sourceRepositoryProvider.GetRepositories();
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);

            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                settings: testSettings,
                solutionManager,
                new TestDeleteOnRestartManager());

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "uap10.0");

            PackageSpecOperations.AddOrUpdateDependency(packageSpec, new PackageDependency("NuGet.Versioning", VersionRange.Parse("99.0.0")));

            var testNuGetProjectContext = new TestNuGetProjectContext();
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            solutionManager.NuGetProjects.Add(buildIntegratedProject);

            // Act
            var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("99.9.9"));

            await nuGetPackageManager.PreviewInstallPackageAsync(
                buildIntegratedProject,
                target,
                new ResolutionContext(),
                nugetProjectContext,
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                CancellationToken.None);

            // Assert
            telemetryEvents.Count.Should().BeGreaterThanOrEqualTo(2);
            telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation").Should().Be(2);
            telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName).Should().Be(1);

            telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName).Should().Contain(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);

            string expectedLogCode = NuGetLogCode.NU1102.ToString();
            telemetryEvents
                .Where(p => p.Name == "ProjectRestoreInformation")
                .ElementAt(1)["ErrorCodes"].Should().NotBeNull()
                .And.Subject.ToString().Should().NotBeNull()
                .And.Subject.Should().Be(expectedLogCode);

            List<KeyValuePair<string, object>> projectFilePaths = telemetryEvents
                .Where(p => p.Name == "ProjectRestoreInformation")
                .SelectMany(x => x.GetPiiData())
                .Where(x => x.Key == "ProjectFilePath")
                .ToList();

            // All of these events should be referencing the same path.
            projectFilePaths.Should().HaveCount(2);
            string singleFilePath = projectFilePaths.Distinct().Should().ContainSingle().Subject.Value.ToString();
            string singlePath = Path.GetDirectoryName(singleFilePath);
            singlePath.Should().Be(msBuildNuGetProjectSystem.ProjectFullPath);
        }

        [Fact]
        public async Task PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEventsWithWarningCode()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var solutionManager = new TestSolutionManager(pathContext);

            var projectName = "testproj";
            var projectTargetFramework = NuGetFramework.Parse("uap10.0");
            var projectFolder = new DirectoryInfo(Path.Combine(solutionManager.SolutionDirectory, projectName));

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            var testLogger = new TestLogger();

            var nuGetVersioningPackageContext = new SimpleTestPackageContext("NuGet.Versioning", "1.0.7");
            nuGetVersioningPackageContext.AddFile("lib/uap10.0/NuGet.Versioning.dll");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, nuGetVersioningPackageContext);
            PackageSource packageSource = new PackageSource(pathContext.PackageSource);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(packageSource);
            var sources = sourceRepositoryProvider.GetRepositories();
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);

            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                settings: testSettings,
                solutionManager,
                new TestDeleteOnRestartManager());

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "uap10.0");

            PackageSpecOperations.AddOrUpdateDependency(packageSpec, new PackageDependency("NuGet.Versioning", VersionRange.Parse("(, 1.0.0)")));

            var testNuGetProjectContext = new TestNuGetProjectContext();
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            solutionManager.NuGetProjects.Add(buildIntegratedProject);

            // Act
            var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.6.9"));

            await nuGetPackageManager.PreviewInstallPackageAsync(
                buildIntegratedProject,
                target,
                new ResolutionContext(),
                nugetProjectContext,
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                CancellationToken.None);

            // Assert
            telemetryEvents.Count.Should().BeGreaterThanOrEqualTo(2);
            telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation").Should().Be(2);
            telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName).Should().Be(1);

            telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName).Should().Contain(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);
            var eventsProjectRestoreInformation = telemetryEvents
                .Where(p => p.Name == "ProjectRestoreInformation").ToList();
            eventsProjectRestoreInformation.Should().HaveCountGreaterThanOrEqualTo(1);

            string expectedLogCode = NuGetLogCode.NU1604.ToString();

            List<string> warningCodes = eventsProjectRestoreInformation
                .Select(item => item["WarningCodes"]?.ToString())
                .Where(warningCode => warningCode is not null)
                .ToList();

            string because = $"Expected Warning Code {expectedLogCode} to be present in ProjectRestoreInformation telemetry event";
            warningCodes.Should().NotBeNull(because);
            warningCodes.Should().HaveCount(1, because);
            warningCodes[0].Should().Be(expectedLogCode, because);

            List<KeyValuePair<string, object>> projectFilePaths = telemetryEvents
                .Where(p => p.Name == "ProjectRestoreInformation")
                .SelectMany(x => x.GetPiiData())
                .Where(x => x.Key == "ProjectFilePath")
                .ToList();

            // All of these events should be referencing the same path.
            projectFilePaths.Should().HaveCount(2);
            string singleFilePath = projectFilePaths.Distinct().Should().ContainSingle().Subject.Value.ToString();
            string singlePath = Path.GetDirectoryName(singleFilePath);
            singlePath.Should().Be(msBuildNuGetProjectSystem.ProjectFullPath);
        }

        [Fact]
        public async Task PreviewUpdatePackage_PackagesConfig_RaiseTelemetryEvents()
        {
            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackage1 = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            var installedPackage2 = new PackageIdentity("b", new NuGetVersion(1, 0, 0));

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(installedPackage1, fwk45, true),
                new NuGet.Packaging.PackageReference(installedPackage2, fwk45, true)
            };

            var nuGetProject = new TestNuGetProject(installedPackages);

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                // Main Act
                var target = new PackageIdentity("a", new NuGetVersion(2, 0, 0));

                await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<PackageIdentity> { target },
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(3, telemetryEvents.Count);
                VerifyPreviewActionsTelemetryEvents_PackagesConfig(telemetryEvents.Select(p => (string)p["SubStepName"]));
            }
        }

        [Fact]
        public async Task ExecuteNuGetProjectActions_PackagesConfig_RaiseTelemetryEvents()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var nugetProject = solutionManager.AddNewMSBuildProject();
                var target = _packageWithDependents[0];

                var projectActions = new List<NuGetProjectAction>();
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        target,
                        sourceRepositoryProvider.GetRepositories().First(),
                        nugetProject));

                // Act
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    new List<NuGetProject>() { nugetProject },
                    projectActions,
                    nugetProjectContext,
                    NullSourceCacheContext.Instance,
                    CancellationToken.None);

                // Assert
                Assert.Equal(5, telemetryEvents.Count);
                Assert.Equal(1, telemetryEvents.Count(p => p.Name == "PackagePreFetcherInformation"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "PackageExtractionInformation"));
                Assert.Equal(1, telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName));
                Assert.Contains(telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName), p => (string)p["SubStepName"] == TelemetryConstants.ExecuteActionStepName);
            }
        }

        [Fact]
        public async Task ExecuteNuGetProjectActions_BuildIntegrated_RaiseTelemetryEvents()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var solutionManager = new TestSolutionManager(pathContext);

            var projectName = "testproj";
            var projectTargetFramework = NuGetFramework.Parse("uap10.0");
            var projectFolder = new DirectoryInfo(Path.Combine(solutionManager.SolutionDirectory, projectName));

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            var testLogger = new TestLogger();

            var nuGetVersioningPackageContext = new SimpleTestPackageContext("NuGet.Versioning", "1.0.0");
            nuGetVersioningPackageContext.AddFile("lib/uap10.0/NuGet.Versioning.dll");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, nuGetVersioningPackageContext);
            PackageSource packageSource = new PackageSource(pathContext.PackageSource);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(packageSource);
            var sources = sourceRepositoryProvider.GetRepositories();
            var testSettings = TestSourceRepositoryUtility.PopulateSettingsWithSources(sourceRepositoryProvider, pathContext.WorkingDirectory);

            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                settings: testSettings,
                solutionManager,
                new TestDeleteOnRestartManager());

            // Create a PackageSpec for the PackageReference project.
            var packageSpec = ProjectTestHelpers.GetPackageSpec(
                testSettings,
                projectName,
                pathContext.SolutionRoot,
                framework: "uap10.0");

            var testNuGetProjectContext = new TestNuGetProjectContext();
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                projectTargetFramework,
                testNuGetProjectContext,
                projectFullPath: Path.GetDirectoryName(packageSpec.FilePath),
                projectName);

            var buildIntegratedProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);
            solutionManager.NuGetProjects.Add(buildIntegratedProject);

            // Act
            var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("1.0.0"));

            await nuGetPackageManager.PreviewInstallPackageAsync(
                buildIntegratedProject,
                target,
                new ResolutionContext(),
                nugetProjectContext,
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                CancellationToken.None);

            // Assert
            telemetryEvents.Count.Should().BeGreaterThanOrEqualTo(2);
            telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation").Should().Be(2);
            telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName).Should().Be(1);

            telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName).Should().Contain(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);
            telemetryEvents
                .Where(p => p.Name == "ProjectRestoreInformation")
                .ElementAt(1)["ErrorCodes"].Should().BeNull();

            List<KeyValuePair<string, object>> projectFilePaths = telemetryEvents
                .Where(p => p.Name == "ProjectRestoreInformation")
                .SelectMany(x => x.GetPiiData())
                .Where(x => x.Key == "ProjectFilePath")
                .ToList();

            // All of these events should be referencing the same path.
            projectFilePaths.Should().HaveCount(2);
            string singleFilePath = projectFilePaths.Distinct().Should().ContainSingle().Subject.Value.ToString();
            string singlePath = Path.GetDirectoryName(singleFilePath);
            singlePath.Should().Be(msBuildNuGetProjectSystem.ProjectFullPath);
        }

        private void VerifyPreviewActionsTelemetryEvents_PackagesConfig(IEnumerable<string> actual)
        {
            Assert.Contains(TelemetryConstants.GatherDependencyStepName, actual);
            Assert.Contains(TelemetryConstants.ResolveDependencyStepName, actual);
            Assert.Contains(TelemetryConstants.ResolvedActionsStepName, actual);
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

        private class TestNuGetVSTelemetryService : NuGetVSTelemetryService
        {
            private ITelemetrySession _telemetrySession;
            private XunitLogger _logger;

            public TestNuGetVSTelemetryService(ITelemetrySession telemetrySession, XunitLogger logger)
            {
                _telemetrySession = telemetrySession ?? throw new ArgumentNullException(nameof(telemetrySession));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public override void EmitTelemetryEvent(TelemetryEvent telemetryData)
            {
                if (telemetryData == null)
                {
                    throw new ArgumentNullException(nameof(telemetryData));
                }

                lock (_logger)
                {
                    var operationId = telemetryData["OperationId"];
                    var parentId = telemetryData["ParentId"];

                    _logger.LogInformation("--------------------------");
                    _logger.LogInformation($"Name: {telemetryData.Name}");
                    _logger.LogInformation($"OperationId: {operationId}");
                    _logger.LogInformation($"ParentId: {parentId}");
                    _logger.LogInformation($"Json: {telemetryData.ToJson()}");
                    _logger.LogInformation($"Stack: {Environment.StackTrace}");
                    _logger.LogInformation("--------------------------");
                }

                _telemetrySession.PostEvent(telemetryData);
            }
        }
    }
}
#endif
