// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
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
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject();

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
                Assert.Equal(3, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation"));
                Assert.Equal(1, telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName));

                Assert.Contains(telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName), p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task PreviewInstallPackage_VersionNotInRange_RaiseTelemetryEventsWithErrorCodeNU1102(bool errorCodeExistsInJson)
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
            using var solutionManager = new TestSolutionManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                solutionManager,
                new TestDeleteOnRestartManager());

            JObject dependenciesJObject = null;
            if (errorCodeExistsInJson)
            {
                dependenciesJObject = new JObject()
                {
                    new JProperty("NuGet.Frameworks", "99.0.0")
                };
            }
            else
            {
                dependenciesJObject = new JObject();
            }

            var json = new JObject
            {
                ["dependencies"] = dependenciesJObject,
                ["frameworks"] = new JObject
                {
                    ["net46"] = new JObject()
                }
            };

            var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

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
            Assert.Equal(3, telemetryEvents.Count);
            Assert.Equal(2, telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation"));
            Assert.Equal(1, telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName));

            Assert.Contains(telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName), p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);

            Assert.True((string)telemetryEvents
                .Last(p => p.Name == "ProjectRestoreInformation")["ErrorCodes"] == NuGetLogCode.NU1102.ToString());

            var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
            Assert.Equal(2, projectFilePaths.Count());
            Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/10093")]
        public async Task PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEventsWithWarningCode()
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
            var telemetryService = new TestNuGetVSTelemetryService(telemetrySession.Object, _logger);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var json = new JObject
                {
                    ["dependencies"] = new JObject(),
                    ["frameworks"] = new JObject
                    {
                        ["net46"] = new JObject()
                    }
                };

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

                // Act
                var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.6.9"));

                lock (_logger)
                {
                    // telemetry count has been flaky, these xunit logs should help track the extra source of events on CI
                    // for issue https://github.com/NuGet/Home/issues/7105
                    _logger.LogInformation("Begin PreviewInstallPackageAsync");
                }

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    buildIntegratedProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                lock (_logger)
                {
                    _logger.LogInformation("End PreviewInstallPackageAsync");
                }

                // Assert
                Assert.Equal(19, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "GenerateRestoreGraph"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "GenerateAssetsFile"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "ValidateRestoreGraphs"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "CreateRestoreResult"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "RestoreNoOpInformation"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "CreateRestoreTargetGraph"));
                Assert.Equal(1, telemetryEvents.Count(p => p.Name == "NugetActionSteps"));

                Assert.Contains(telemetryEvents.Where(p => p.Name == "NugetActionSteps"), p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);

                Assert.True((string)telemetryEvents
                    .Last(p => p.Name == "ProjectRestoreInformation")["WarningCodes"] == NuGetLogCode.NU1603.ToString());

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }

        [Fact]
        public async Task PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEventsWithDupedWarningCodes()
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
            var telemetryService = new TestNuGetVSTelemetryService(telemetrySession.Object, _logger);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var json = new JObject
                {
                    ["dependencies"] = new JObject()
                    {
                        new JProperty("NuGet.Frameworks", "4.6.9")
                    },
                    ["frameworks"] = new JObject
                    {
                        ["net46"] = new JObject()
                    }
                };

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

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
                Assert.Equal(7, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation"));
                Assert.Equal(1, telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName));

                Assert.Contains(telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName), p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);

                Assert.True((string)telemetryEvents
                    .Last(p => p.Name == "ProjectRestoreInformation")["WarningCodes"] == NuGetLogCode.NU1603.ToString());

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
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

            using (var settingsdir = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                var settings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                foreach (var source in sourceRepositoryProvider.GetRepositories())
                {
                    settings.AddOrUpdate(ConfigurationConstants.PackageSources, source.PackageSource.AsSourceItem());
                }

                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    settings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var buildIntegratedProject = testSolutionManager.AddBuildIntegratedProject();

                var packageIdentity = _packageWithDependents[0];

                var projectActions = new List<NuGetProjectAction>();
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        packageIdentity,
                        sourceRepositoryProvider.GetRepositories().First(),
                        buildIntegratedProject));

                // Act
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    new List<NuGetProject>() { buildIntegratedProject },
                    projectActions,
                    nugetProjectContext,
                    NullSourceCacheContext.Instance,
                    token);

                // Assert
                Assert.Equal(24, telemetryEvents.Count);

                Assert.Equal(2, telemetryEvents.Count(p => p.Name == "ProjectRestoreInformation"));
                Assert.Equal(2, telemetryEvents.Count(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName));

                Assert.Contains(telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName), p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName);
                Assert.Contains(telemetryEvents.Where(p => p.Name == ActionTelemetryStepEvent.NugetActionStepsEventName), p => (string)p["SubStepName"] == TelemetryConstants.ExecuteActionStepName);

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }

        private void VerifyPreviewActionsTelemetryEvents_PackagesConfig(IEnumerable<string> actual)
        {
            Assert.Contains(TelemetryConstants.GatherDependencyStepName, actual);
            Assert.Contains(TelemetryConstants.ResolveDependencyStepName, actual);
            Assert.Contains(TelemetryConstants.ResolvedActionsStepName, actual);
        }

        private static void AddToPackagesFolder(PackageIdentity package, string root)
        {
            var dir = Path.Combine(root, $"{package.Id}.{package.Version.ToString()}");
            Directory.CreateDirectory(dir);

            var context = new SimpleTestPackageContext()
            {
                Id = package.Id,
                Version = package.Version.ToString()
            };

            context.AddFile("lib/net45/a.dll");
            SimpleTestPackageUtility.CreateOPCPackage(context, dir);
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

        private static void Expected(List<Tuple<PackageIdentity, NuGetProjectActionType>> expected, string id, NuGetVersion oldVersion, NuGetVersion newVersion)
        {
            expected.Add(Tuple.Create(new PackageIdentity(id, oldVersion), NuGetProjectActionType.Uninstall));
            expected.Add(Tuple.Create(new PackageIdentity(id, newVersion), NuGetProjectActionType.Install));
        }

        private static void Expected(List<Tuple<PackageIdentity, NuGetProjectActionType>> expected, string id, NuGetVersion newVersion)
        {
            expected.Add(Tuple.Create(new PackageIdentity(id, newVersion), NuGetProjectActionType.Install));
        }

        private static bool Compare(
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> lhs,
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> rhs)
        {
            var ok = true;
            ok &= RhsContainsAllLhs(lhs, rhs);
            ok &= RhsContainsAllLhs(rhs, lhs);
            return ok;
        }

        private static bool RhsContainsAllLhs(
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> lhs,
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> rhs)
        {
            foreach (var item in lhs)
            {
                if (!rhs.Contains(item, new ActionComparer()))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool PreviewResultsCompare(
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> lhs,
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> rhs)
        {
            var ok = true;
            ok &= RhsContainsAllLhs(lhs, rhs);
            ok &= RhsContainsAllLhs(rhs, lhs);
            return ok;
        }

        private static bool RhsContainsAllLhs(
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> lhs,
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> rhs)
        {
            foreach (var item in lhs)
            {
                if (!rhs.Contains(item, new PreviewResultComparer()))
                {
                    return false;
                }
            }
            return true;
        }

        private class ActionComparer : IEqualityComparer<Tuple<PackageIdentity, NuGetProjectActionType>>
        {
            public bool Equals(Tuple<PackageIdentity, NuGetProjectActionType> x, Tuple<PackageIdentity, NuGetProjectActionType> y)
            {
                var f1 = x.Item1.Equals(y.Item1);
                var f2 = x.Item2 == y.Item2;
                return f1 && f2;
            }

            public int GetHashCode(Tuple<PackageIdentity, NuGetProjectActionType> obj)
            {
                return obj.GetHashCode();
            }
        }

        private class PreviewResultComparer : IEqualityComparer<Tuple<TestNuGetProject, PackageIdentity>>
        {
            public bool Equals(Tuple<TestNuGetProject, PackageIdentity> x, Tuple<TestNuGetProject, PackageIdentity> y)
            {
                var f1 = x.Item1.Metadata[NuGetProjectMetadataKeys.Name].ToString().Equals(
                    y.Item1.Metadata[NuGetProjectMetadataKeys.Name].ToString());
                var f2 = x.Item2.Equals(y.Item2);
                return f1 && f2;
            }

            public int GetHashCode(Tuple<TestNuGetProject, PackageIdentity> obj)
            {
                return obj.GetHashCode();
            }
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
