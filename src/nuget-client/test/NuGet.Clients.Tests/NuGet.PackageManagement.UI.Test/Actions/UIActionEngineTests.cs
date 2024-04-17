// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;
using ContractsItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;

namespace NuGet.PackageManagement.UI.Test
{
    [Collection(MockedVS.Collection)]
    public class UIActionEngineTests
    {
        public UIActionEngineTests(GlobalServiceProvider sp)
        {
            sp.Reset();
            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(ThreadHelper.JoinableTaskFactory);
        }

        [Fact]
        public async Task GetPreviewResultsAsync_WhenPackageIdentityIsSubclass_ItIsReplacedWithNewPackageIdentity()
        {
            string projectId = Guid.NewGuid().ToString();
            var packageIdentityA1 = new PackageIdentitySubclass(id: "a", NuGetVersion.Parse("1.0.0"));
            var packageIdentityA2 = new PackageIdentitySubclass(id: "a", NuGetVersion.Parse("2.0.0"));
            var packageIdentityB1 = new PackageIdentitySubclass(id: "b", NuGetVersion.Parse("3.0.0"));
            var packageIdentityB2 = new PackageIdentitySubclass(id: "b", NuGetVersion.Parse("4.0.0"));
            var uninstallAction = new ProjectAction(
                id: Guid.NewGuid().ToString(),
                projectId,
                packageIdentityA1,
                NuGetProjectActionType.Uninstall,
                implicitActions: new[]
                {
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        packageIdentityA1,
                        NuGetProjectActionType.Uninstall),
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        packageIdentityB1,
                        NuGetProjectActionType.Uninstall)
                });
            var installAction = new ProjectAction(
                id: Guid.NewGuid().ToString(),
                projectId,
                packageIdentityA2,
                NuGetProjectActionType.Install,
                implicitActions: new[]
                {
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        packageIdentityA2,
                        NuGetProjectActionType.Install),
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        packageIdentityB2,
                        NuGetProjectActionType.Install)
                });

            var mockUIController = new Mock<INuGetUI>();
            var mockUIContext = new Mock<INuGetUIContext>();
            mockUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns((PackageSourceMapping)null);
            mockUIController.Setup(uiController => uiController.UIContext).Returns(mockUIContext.Object);

            UIActionEngine uiActionEngine = CreateUIActionEngine();
            IReadOnlyList<PreviewResult> previewResults = await uiActionEngine.GetPreviewResultsAsync(
                Mock.Of<INuGetProjectManagerService>(),
                projectActions: new[] { uninstallAction, installAction },
                userAction: null,
                mockUIController.Object,
                CancellationToken.None);

            Assert.Equal(1, previewResults.Count);
            UpdatePreviewResult[] updatedResults = previewResults[0].Updated.ToArray();

            Assert.Equal(2, updatedResults.Length);

            UpdatePreviewResult updatedResult = updatedResults[0];

            Assert.False(updatedResult.Old.GetType().IsSubclassOf(typeof(PackageIdentity)));
            Assert.False(updatedResult.New.GetType().IsSubclassOf(typeof(PackageIdentity)));
            Assert.Equal("a.1.0.0 -> a.2.0.0", updatedResult.ToString());

            updatedResult = updatedResults[1];

            Assert.False(updatedResult.Old.GetType().IsSubclassOf(typeof(PackageIdentity)));
            Assert.False(updatedResult.New.GetType().IsSubclassOf(typeof(PackageIdentity)));
            Assert.Equal("b.3.0.0 -> b.4.0.0", updatedResult.ToString());
        }

        [Fact]
        public async Task GetPreviewResultsAsync_WithMultipleActions_SortsPackageIdentities()
        {
            string projectId = Guid.NewGuid().ToString();
            var packageIdentityA = new PackageIdentitySubclass(id: "a", NuGetVersion.Parse("1.0.0"));
            var packageIdentityB = new PackageIdentitySubclass(id: "b", NuGetVersion.Parse("2.0.0"));
            var packageIdentityC = new PackageIdentitySubclass(id: "c", NuGetVersion.Parse("3.0.0"));
            var installAction = new ProjectAction(
                id: Guid.NewGuid().ToString(),
                projectId,
                packageIdentityB,
                NuGetProjectActionType.Install,
                implicitActions: new[]
                {
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        packageIdentityA,
                        NuGetProjectActionType.Install),
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        packageIdentityB,
                        NuGetProjectActionType.Install),
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        packageIdentityC,
                        NuGetProjectActionType.Install)
                });

            var mockUIController = new Mock<INuGetUI>();
            var mockUIContext = new Mock<INuGetUIContext>();
            mockUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns((PackageSourceMapping)null);
            mockUIController.Setup(uiController => uiController.UIContext).Returns(mockUIContext.Object);

            UIActionEngine uiActionEngine = CreateUIActionEngine();
            IReadOnlyList<PreviewResult> previewResults = await uiActionEngine.GetPreviewResultsAsync(
                Mock.Of<INuGetProjectManagerService>(),
                projectActions: new[] { installAction },
                userAction: null,
                mockUIController.Object,
                CancellationToken.None);

            Assert.Equal(1, previewResults.Count);
            AccessiblePackageIdentity[] addedResults = previewResults[0].Added.ToArray();

            Assert.Equal(3, addedResults.Length);

            Assert.Equal(packageIdentityA.Id, addedResults[0].Id);
            Assert.Equal(packageIdentityB.Id, addedResults[1].Id);
            Assert.Equal(packageIdentityC.Id, addedResults[2].Id);
        }

        private UIActionEngine CreateUIActionEngine(ISettings settings = null)
        {
            settings = settings ?? Mock.Of<ISettings>();
            var sourceRepositoryProvider = Mock.Of<ISourceRepositoryProvider>();
            var packageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                Mock.Of<ISettings>(),
                TestDirectory.Create().Path);

            return new UIActionEngine(
                sourceRepositoryProvider,
                packageManager,
                Mock.Of<INuGetLockService>());
        }

        [Theory]
        [MemberData(nameof(GetInstallActionTestData))]
        public async Task CreateInstallAction_OnInstallingProject_WithNewSourceMapping_DoesNotLogTelemetry(ContractsItemFilter activeTab, bool isSolutionLevel, string packageIdToInstall, bool? expectedPackageWasTransitive)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            var sourceProvider = new Mock<ISourceRepositoryProvider>();
            var settings = new Mock<ISettings>();
            var nugetPM = new NuGetPackageManager(sourceProvider.Object, settings.Object, @"\packagesFolder");
            var lockService = new NuGetLockService(ThreadHelper.JoinableTaskContext);
            var uiEngine = new UIActionEngine(sourceProvider.Object, nugetPM, lockService);

            var installedAndTransitive = new InstalledAndTransitivePackages(
                new[] {
                    new PackageReferenceContextInfo(new PackageIdentity("installedA", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net472")),
                    new PackageReferenceContextInfo(new PackageIdentity("installedB", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net472"))
                },
                new[] {
                    new TransitivePackageReferenceContextInfo(new PackageIdentity("transitiveA", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net472"))
                });
            var prjMgrSvc = new Mock<INuGetProjectManagerService>();
            prjMgrSvc
                .Setup(mgr => mgr.GetInstalledAndTransitivePackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(installedAndTransitive));
            prjMgrSvc
                .Setup(mgr => mgr.GetInstalledAndTransitivePackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(installedAndTransitive));
            var dictMetadata = new Dictionary<string, object>
            {
                [NuGetProjectMetadataKeys.UniqueName] = "a",
                [NuGetProjectMetadataKeys.ProjectId] = "a"
            };
            ProjectMetadataContextInfo metadataCtxtInfo = ProjectMetadataContextInfo.Create(dictMetadata);
            prjMgrSvc
                .Setup(mgr => mgr.GetMetadataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(metadataCtxtInfo));
            var uiService = new Mock<INuGetUI>();
            var uiContext = new Mock<INuGetUIContext>();
            var projectContext = new Mock<INuGetProjectContext>();
            var serviceBroker = new Mock<IServiceBroker>();
            _ = serviceBroker.Setup(sb => sb.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(s => s == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetProjectManagerService>(prjMgrSvc.Object));
            uiContext.Setup(ctx => ctx.ServiceBroker).Returns(serviceBroker.Object);
            uiService.Setup(ui => ui.UIContext).Returns(uiContext.Object);
            uiService.Setup(ui => ui.ProjectContext).Returns(projectContext.Object);
            uiService.Setup(ui => ui.Settings).Returns(settings.Object);
            uiService.Setup(ui => ui.Projects).Returns(new[] { new ProjectContextInfo("a", ProjectModel.ProjectStyle.PackageReference, NuGetProjectKind.PackageReference) });

            var action = UserAction.CreateInstallAction(packageIdToInstall, NuGetVersion.Parse("1.0.0"), isSolutionLevel, activeTab, sourceMappingSourceName: "source");

            // Act
            await uiEngine.PerformInstallOrUninstallAsync(uiService.Object, action, CancellationToken.None);

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            Assert.IsType<NuGetOperationStatus>(lastTelemetryEvent[nameof(ActionEventBase.Status)]);
            Assert.Equal(NuGetProjectActionType.Install, lastTelemetryEvent[nameof(ActionsTelemetryEvent.OperationType)]);
            Assert.Equal(isSolutionLevel, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.IsSolutionLevel)]);
            Assert.Equal(activeTab, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.Tab)]);
            Assert.Equal(expectedPackageWasTransitive, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.PackageToInstallWasTransitive)]);
            // Package Source Mapping is considered enabled if mappings already existed when the action began.
            Assert.Equal(false, lastTelemetryEvent[VSActionsTelemetryEvent.PackageSourceMappingIsMappingEnabled]);
        }

        [Fact]
        public void AddUiActionEngineTelemetryProperties_AddsVulnerabilityInfo_Succeeds()
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var operationId = Guid.NewGuid().ToString();

            var actionTelemetryData = new VSActionsTelemetryEvent(
                operationId,
                projectIds: new[] { Guid.NewGuid().ToString() },
                operationType: NuGetProjectActionType.Install,
                source: OperationSource.PMC,
                startTime: DateTimeOffset.Now.AddSeconds(-1),
                status: NuGetOperationStatus.NoOp,
                packageCount: 1,
                endTime: DateTimeOffset.Now,
                duration: .40,
                isPackageSourceMappingEnabled: false);

            UIActionEngine.AddUiActionEngineTelemetryProperties(
                actionTelemetryEvent: actionTelemetryData,
                continueAfterPreview: true,
                acceptedLicense: true,
                userAction: UserAction.CreateInstallAction("mypackageId", new NuGetVersion(1, 0, 0), It.IsAny<bool>(), It.IsAny<ContractsItemFilter>()),
                selectedIndex: 0,
                recommendedCount: 0,
                recommendPackages: false,
                recommenderVersion: null,
                topLevelVulnerablePackagesCount: 3,
                topLevelVulnerablePackagesMaxSeverities: new List<int> { 1, 1, 3 }, // each package has its own max severity
                transitiveVulnerablePackagesCount: 2,
                transitiveVulnerablePackagesMaxSeverities: new List<int> { 2, 3 }, // each package has its own max severity
                existingPackages: null,
                addedPackages: null,
                removedPackages: null,
                updatedPackagesOld: null,
                updatedPackagesNew: null,
                targetFrameworks: null,
                countCreatedTopLevelSourceMappings: null,
                countCreatedTransitiveSourceMappings: null);

            // Act
            var service = new NuGetVSTelemetryService(telemetrySession.Object);
            service.EmitTelemetryEvent(actionTelemetryData);

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            Assert.NotNull(lastTelemetryEvent.ComplexData["TopLevelVulnerablePackagesMaxSeverities"] as List<int>);
            var topLevelPkgSeverities = lastTelemetryEvent.ComplexData["TopLevelVulnerablePackagesMaxSeverities"] as List<int>;
            Assert.Equal(lastTelemetryEvent["TopLevelVulnerablePackagesCount"], topLevelPkgSeverities.Count());
            Assert.Collection(topLevelPkgSeverities,
                item => Assert.Equal(1, item),
                item => Assert.Equal(1, item),
                item => Assert.Equal(3, item));
            Assert.Equal(3, topLevelPkgSeverities.Count());

            var transitivePkgSeverities = lastTelemetryEvent.ComplexData["TransitiveVulnerablePackagesMaxSeverities"] as List<int>;
            Assert.Equal(lastTelemetryEvent["TransitiveVulnerablePackagesCount"], transitivePkgSeverities.Count());
            Assert.Equal(lastTelemetryEvent["TransitiveVulnerablePackagesCount"], transitivePkgSeverities.Count());
            Assert.Collection(transitivePkgSeverities,
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item));
            Assert.Equal(2, transitivePkgSeverities.Count());

            Assert.Null(lastTelemetryEvent["CreatedTopLevelSourceMappingsCount"]);
            Assert.Null(lastTelemetryEvent["CreatedTransitiveSourceMappingsCount"]);
        }

        [Fact]
        public void ActionCreatingSourceMappings_TopLevelCountAndTransitiveCount_AddsValue()
        {
            // Arrange
            int expectedCountCreatedTopLevelSourceMappings = 42;
            int expectedCountCreatedTransitiveSourceMappings = 24;
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var operationId = Guid.NewGuid().ToString();

            var actionTelemetryData = new VSActionsTelemetryEvent(
                operationId,
                projectIds: new[] { Guid.NewGuid().ToString() },
                operationType: NuGetProjectActionType.Install,
                source: OperationSource.PMC,
                startTime: DateTimeOffset.Now.AddSeconds(-1),
                status: NuGetOperationStatus.NoOp,
                packageCount: 1,
                endTime: DateTimeOffset.Now,
                duration: .40,
                isPackageSourceMappingEnabled: false);

            UIActionEngine.AddUiActionEngineTelemetryProperties(
                actionTelemetryEvent: actionTelemetryData,
                continueAfterPreview: true,
                acceptedLicense: true,
                userAction: null,
                selectedIndex: 0,
                recommendedCount: 0,
                recommendPackages: false,
                recommenderVersion: null,
                topLevelVulnerablePackagesCount: 3,
                topLevelVulnerablePackagesMaxSeverities: new List<int> { 1, 1, 3 }, // each package has its own max severity
                transitiveVulnerablePackagesCount: 2,
                transitiveVulnerablePackagesMaxSeverities: new List<int> { 2, 3 }, // each package has its own max severity
                existingPackages: null,
                addedPackages: null,
                removedPackages: null,
                updatedPackagesOld: null,
                updatedPackagesNew: null,
                targetFrameworks: null,
                countCreatedTopLevelSourceMappings: expectedCountCreatedTopLevelSourceMappings,
                countCreatedTransitiveSourceMappings: expectedCountCreatedTransitiveSourceMappings);

            // Act
            var service = new NuGetVSTelemetryService(telemetrySession.Object);
            service.EmitTelemetryEvent(actionTelemetryData);

            // Assert
            Assert.Equal(expectedCountCreatedTopLevelSourceMappings, (int)lastTelemetryEvent["CreatedTopLevelSourceMappingsCount"]);
            Assert.Equal(expectedCountCreatedTransitiveSourceMappings, (int)lastTelemetryEvent["CreatedTransitiveSourceMappingsCount"]);
        }

        [Theory]
        [InlineData("packageA", "2.0.0", "*")]
        [InlineData("packageA", "2.0.0", "(1.0.0, )")]
        [InlineData("packageA", "2.0.0", null)]
        public void ToTelemetryPackage_Succeeds(string packageId, string packageVersion, string packageVersionRange)
        {
            TelemetryEvent telemetryEvent = UIActionEngine.ToTelemetryPackage(packageId, packageVersion, packageVersionRange);

            Assert.Equal(telemetryEvent.GetPiiData().First().Value.ToString(), VSTelemetryServiceUtility.NormalizePackageId(packageId));
            Assert.Equal(telemetryEvent["version"], packageVersion);
            Assert.Equal(telemetryEvent["versionRange"], packageVersionRange);
        }

        public static IEnumerable<object[]> GetTelemetryListTestData()
        {
            yield return new object[] { new List<Tuple<string, string>> { new Tuple<string, string>("packageA", "2.0.0") } };
            yield return new object[] { new List<Tuple<string, string>> { new Tuple<string, string>("packageB", "1.0.0") } };
        }

        [Theory]
        [MemberData(nameof(GetTelemetryListTestData))]
        public void ToTelemetryPackageList_Succeeds(List<Tuple<string, string>> packages)
        {
            List<TelemetryEvent> telemetryEvents = UIActionEngine.ToTelemetryPackageList(packages);

            Assert.Equal(packages.Count(), telemetryEvents.Count());

            for (int index = 0; index < telemetryEvents.Count(); index++)
            {
                Assert.Equal(telemetryEvents[index].GetPiiData().First().Value.ToString(), VSTelemetryServiceUtility.NormalizePackageId(packages[index].Item1));
                Assert.Equal(telemetryEvents[index]["version"], packages[index].Item2);
            }
        }


        public static IEnumerable<object[]> GetInstallActionTestData()
        {
            foreach (var activeTab in Enum.GetValues(typeof(ContractsItemFilter)))
            {
                yield return new object[] { activeTab, true, "transitiveA", null, }; // don't care in expectedValue in this case (solution PM UI)
                yield return new object[] { activeTab, false, "transitiveA", true, }; // installs a package that was a transitive dependency
                yield return new object[] { activeTab, false, "anotherPackage", false, }; // installs a package that was not a transitive dependency
            }
        }

        [Theory]
        [MemberData(nameof(GetInstallActionTestData))]
        public async Task CreateInstallAction_OnInstallingProject_EmitsPkgWasTransitiveTelemetryAndTabAndIsSolutionPropertiesAsync(ContractsItemFilter activeTab, bool isSolutionLevel, string packageIdToInstall, bool? expectedPackageWasTransitive)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            var sourceProvider = new Mock<ISourceRepositoryProvider>();
            var settings = new Mock<ISettings>();
            var nugetPM = new NuGetPackageManager(sourceProvider.Object, settings.Object, @"\packagesFolder");
            var lockService = new NuGetLockService(ThreadHelper.JoinableTaskContext);
            var uiEngine = new UIActionEngine(sourceProvider.Object, nugetPM, lockService);

            var installedAndTransitive = new InstalledAndTransitivePackages(
                new[] {
                    new PackageReferenceContextInfo(new PackageIdentity("installedA", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net472")),
                    new PackageReferenceContextInfo(new PackageIdentity("installedB", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net472"))
                },
                new[] {
                    new TransitivePackageReferenceContextInfo(new PackageIdentity("transitiveA", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net472"))
                });

            var list = new List<IPackageReferenceContextInfo>();
            list.AddRange(installedAndTransitive.InstalledPackages);
            list.AddRange(installedAndTransitive.TransitivePackages);
            var installedPackagesMergedWithTransitives = new ReadOnlyCollection<IPackageReferenceContextInfo>(list);

            var prjMgrSvc = new Mock<INuGetProjectManagerService>();
            prjMgrSvc.Setup(mgr => mgr.GetInstalledPackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackagesMergedWithTransitives));
            prjMgrSvc
                .Setup(mgr => mgr.GetInstalledAndTransitivePackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(installedAndTransitive));
            prjMgrSvc
                .Setup(mgr => mgr.GetInstalledAndTransitivePackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(installedAndTransitive));
            var dictMetadata = new Dictionary<string, object>
            {
                [NuGetProjectMetadataKeys.UniqueName] = "a",
                [NuGetProjectMetadataKeys.ProjectId] = "a"
            };
            ProjectMetadataContextInfo metadataCtxtInfo = ProjectMetadataContextInfo.Create(dictMetadata);
            prjMgrSvc
                .Setup(mgr => mgr.GetMetadataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(metadataCtxtInfo));
            var uiService = new Mock<INuGetUI>();
            var uiContext = new Mock<INuGetUIContext>();
            var projectContext = new Mock<INuGetProjectContext>();
            var serviceBroker = new Mock<IServiceBroker>();
            _ = serviceBroker.Setup(sb => sb.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(s => s == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetProjectManagerService>(prjMgrSvc.Object));
            uiContext.Setup(ctx => ctx.ServiceBroker).Returns(serviceBroker.Object);
            uiService.Setup(ui => ui.UIContext).Returns(uiContext.Object);
            uiService.Setup(ui => ui.ProjectContext).Returns(projectContext.Object);
            uiService.Setup(ui => ui.Settings).Returns(settings.Object);
            uiService.Setup(ui => ui.Projects).Returns(new[] { new ProjectContextInfo("a", ProjectModel.ProjectStyle.PackageReference, NuGetProjectKind.PackageReference) });

            var action = UserAction.CreateInstallAction(packageIdToInstall, NuGetVersion.Parse("1.0.0"), isSolutionLevel, activeTab);

            // Act
            await uiEngine.PerformInstallOrUninstallAsync(uiService.Object, action, CancellationToken.None);

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            Assert.IsType<NuGetOperationStatus>(lastTelemetryEvent[nameof(ActionEventBase.Status)]);
            Assert.Equal(NuGetProjectActionType.Install, lastTelemetryEvent[nameof(ActionsTelemetryEvent.OperationType)]);
            Assert.Equal(isSolutionLevel, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.IsSolutionLevel)]);
            Assert.Equal(activeTab, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.Tab)]);
            Assert.Equal(expectedPackageWasTransitive, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.PackageToInstallWasTransitive)]);
        }

        [Fact]
        public async Task PerformInstallOrUninstallAsync_TransitiveNotSourceMapped_Throws()
        {
            // Arrange
            string localPackageSourceName = null;
            string remotePackageSourceName = "sourceA";
            var projectId = Guid.NewGuid().ToString();

            // When checking remote sources, the Package Source Mapping object should be checked to see if it's enabled.
            var timesSourceMappingCalled = Times.Once();
            var throwOnShowError = true;

            // Configure Package Source Mappings.
            IReadOnlyDictionary<string, IReadOnlyList<string>> packageSourceMappingPatterns = new Dictionary<string, IReadOnlyList<string>>
            {
                { "anotherSource", new List<string>() { "a" } }
            };

            SetupUIServiceWithPackageSearchMetadata(
                localPackageSourceName,
                remotePackageSourceName,
                projectId,
                packageSourceMappingPatterns,
                throwOnShowError,
                out UIActionEngine uiActionEngine,
                out Mock<INuGetUI> mockUIService,
                out Mock<INuGetUIContext> mockNuGetUIContext);

            var action = UserAction.CreateInstallAction(
                packageId: "anotherPackage",
                NuGetVersion.Parse("1.0.0"),
                isSolutionLevel: false,
                activeTab: ContractsItemFilter.All);

            // Act
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => uiActionEngine.PerformInstallOrUninstallAsync(mockUIService.Object, action, CancellationToken.None));

            // Assert
            mockNuGetUIContext.Verify(uiContext => uiContext.PackageSourceMapping, timesSourceMappingCalled);
            Assert.Contains("Unable to find metadata of transitiveA.1.0.0", ex.Message);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task PerformInstallOrUninstallAsync_TransitiveFoundOnRemoteSource_Succeeds(bool isSourceMappingEnabled)
        {
            // Arrange
            string localPackageSourceName = null;
            string remotePackageSourceName = "sourceA";
            var projectId = Guid.NewGuid().ToString();

            // When checking remote sources, the Package Source Mapping object should be checked to see if it's enabled.
            var timesSourceMappingCalled = Times.Once();
            var throwOnShowError = true;

            // Configure Package Source Mappings.
            IReadOnlyDictionary<string, IReadOnlyList<string>> packageSourceMappingPatterns = null;
            if (isSourceMappingEnabled)
            {
                packageSourceMappingPatterns = new Dictionary<string, IReadOnlyList<string>>
                {
                    { remotePackageSourceName, new List<string>() { "transitiveA" } }
                };
            }

            SetupUIServiceWithPackageSearchMetadata(
                localPackageSourceName,
                remotePackageSourceName,
                projectId,
                packageSourceMappingPatterns,
                throwOnShowError,
                out UIActionEngine uiActionEngine,
                out Mock<INuGetUI> mockUIService,
                out Mock<INuGetUIContext> mockNuGetUIContext);

            var action = UserAction.CreateInstallAction(
                packageId: "anotherPackage",
                NuGetVersion.Parse("1.0.0"),
                isSolutionLevel: false,
                activeTab: ContractsItemFilter.All);

            // Act
            await uiActionEngine.PerformInstallOrUninstallAsync(mockUIService.Object, action, CancellationToken.None);

            // Assert
            mockNuGetUIContext.Verify(uiContext => uiContext.PackageSourceMapping, timesSourceMappingCalled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task PerformInstallOrUninstallAsync_TransitiveFoundOnLocalSource_SucceedsWithoutRemoteSources(bool isSourceMappingEnabled)
        {
            string localPackageSourceName = "sourceA";
            string remotePackageSourceName = "sourceB";

            var projectId = Guid.NewGuid().ToString();
            var timesSourceMappingCalled = Times.Never();
            var throwOnShowError = true;

            // Configure Package Source Mappings.
            IReadOnlyDictionary<string, IReadOnlyList<string>> packageSourceMappingPatterns = null;
            if (isSourceMappingEnabled)
            {
                packageSourceMappingPatterns = new Dictionary<string, IReadOnlyList<string>>
                {
                    { localPackageSourceName, new List<string>() { "transitiveA" } }
                };
            }

            SetupUIServiceWithPackageSearchMetadata(
                localPackageSourceName,
                remotePackageSourceName,
                projectId,
                packageSourceMappingPatterns,
                throwOnShowError,
                out UIActionEngine uiActionEngine,
                out Mock<INuGetUI> mockUIService,
                out Mock<INuGetUIContext> mockNuGetUIContext);

            var action = UserAction.CreateInstallAction(
                packageId: "anotherPackage",
                NuGetVersion.Parse("1.0.0"),
                isSolutionLevel: false,
                activeTab: ContractsItemFilter.All);

            // Act
            await uiActionEngine.PerformInstallOrUninstallAsync(mockUIService.Object, action, CancellationToken.None);

            // Assert
            mockNuGetUIContext.Verify(uiContext => uiContext.PackageSourceMapping, timesSourceMappingCalled);
        }

        private void SetupUIServiceWithPackageSearchMetadata(
            string localPackageSourceName,
            string remotePackageSourceName,
            string projectId,
            IReadOnlyDictionary<string, IReadOnlyList<string>> packageSourceMappingPatterns,
            bool throwOnShowError,
            out UIActionEngine uiActionEngine,
            out Mock<INuGetUI> uiService,
            out Mock<INuGetUIContext> uiContext)
        {
            var mockUiService = new Mock<INuGetUI>();
            uiContext = new Mock<INuGetUIContext>();
            var projectContext = new Mock<INuGetProjectContext>();
            var serviceBroker = new Mock<IServiceBroker>();
            var settings = new Mock<ISettings>();
            var lockService = new NuGetLockService(ThreadHelper.JoinableTaskContext);

            // Configure installed & transitive packages.
            PackageIdentity packageIdentityInstalledA = new("installedA", NuGetVersion.Parse("1.0.0"));
            PackageIdentity packageIdentityTransitiveA = new("transitiveA", NuGetVersion.Parse("1.0.0"));
            PackageReferenceContextInfo[] topLevelInstalledPackages = new[] {
                    new PackageReferenceContextInfo(packageIdentityInstalledA, NuGetFramework.Parse("net472")),
                    new PackageReferenceContextInfo(new PackageIdentity("installedB", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net472"))
                };

            TransitivePackageReferenceContextInfo[] transitiveInstalledPackages = new[] {
                    new TransitivePackageReferenceContextInfo(packageIdentityTransitiveA, NuGetFramework.Parse("net472"))
                };

            ConfigureNuGetUIWithPackageSourceMapping(uiContext, packageSourceMappingPatterns);
            SetupPackagesConfigProject(mockUiService, projectId);

            PackageSource localPackageSource = localPackageSourceName != null ? new(localPackageSourceName) : null;
            PackageSource remotePackageSource = remotePackageSourceName != null ? new(remotePackageSourceName) : null;
            Mock<PackageMetadataResource> mockPackageMetadataResource = new();
            ISourceRepositoryProvider sourceRepositoryProvider = SetupSourceRepositoryProvider(
                localPackageSource,
                remotePackageSource,
                mockPackageMetadataResource,
                out SourceRepository localSourceRepository,
                out _);

            PackageSource activePackageSource = remotePackageSource ?? localPackageSource;
            SetupActivePackageSource(activePackageSource, mockUiService);

            NuGetPackageManager packageManager = new(sourceRepositoryProvider, settings.Object, @"\packagesFolder");
            uiActionEngine = new(sourceRepositoryProvider, packageManager, lockService);

            SetupUIService(mockUiService, uiContext, projectContext, settings, throwOnShowError);
            SetupProjectInstallAction(
                projectId,
                topLevelPackage: packageIdentityInstalledA,
                implicitTransitivePackage: packageIdentityTransitiveA,
                out List<ProjectAction> listProjectActions);

            SetupProjectManagerService(serviceBroker, listProjectActions, topLevelInstalledPackages, transitiveInstalledPackages);
            SetupPackageSearchMetadata(mockPackageMetadataResource, packageIdentityInstalledA);
            SetupPackageSearchMetadata(mockPackageMetadataResource, packageIdentityTransitiveA);
            SetupProjectUpgraderService(serviceBroker);
            SetupUIContext(localSourceRepository, uiContext, serviceBroker);

            uiService = mockUiService;
        }

        private static void SetupUIContext(SourceRepository localSourceRepository, Mock<INuGetUIContext> uiContext, Mock<IServiceBroker> serviceBroker)
        {
            Mock<INuGetSearchService> reconnectingNuGetSearchService = new Mock<INuGetSearchService>();

            IReadOnlyList<SourceRepository> list = System.Collections.Immutable.ImmutableArray<SourceRepository>.Empty;
            if (localSourceRepository != null)
            {
                list = new List<SourceRepository>() { localSourceRepository }.AsReadOnly();
            }

            reconnectingNuGetSearchService.Setup(svc => svc.GetAllPackageFoldersAsync(It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyList<SourceRepository>>(list).AsTask());

            uiContext.Setup(ctx => ctx.NuGetSearchService).Returns(reconnectingNuGetSearchService.Object);
            uiContext.Setup(ctx => ctx.ServiceBroker).Returns(serviceBroker.Object);
        }

        private static void SetupProjectUpgraderService(Mock<IServiceBroker> serviceBroker)
        {
            var projectUpgraderService = new Mock<INuGetProjectUpgraderService>();
            projectUpgraderService.Setup(s => s.GetUpgradeableProjectsAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IProjectContextInfo>>(Mock.Of<IReadOnlyCollection<IProjectContextInfo>>()));

            _ = serviceBroker.Setup(sb => sb.GetProxyAsync<INuGetProjectUpgraderService>(
                    It.Is<ServiceJsonRpcDescriptor>(s => s == NuGetServices.ProjectUpgraderService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetProjectUpgraderService>(projectUpgraderService.Object));
        }

        private static void SetupProjectInstallAction(string projectId, PackageIdentity topLevelPackage, PackageIdentity implicitTransitivePackage, out List<ProjectAction> listProjectActions)
        {
            var projectAction = new ProjectAction(
                           id: Guid.NewGuid().ToString(),
                           projectId,
                           topLevelPackage,
                           projectActionType: NuGetProjectActionType.Install,
                           implicitActions: new[]
                           {
                    new ImplicitProjectAction(
                        id: Guid.NewGuid().ToString(),
                        implicitTransitivePackage,
                        NuGetProjectActionType.Install),
                           });

            listProjectActions = new List<ProjectAction>
            {
                projectAction
            };
        }

        private static void SetupUIService(Mock<INuGetUI> uiService, Mock<INuGetUIContext> uiContext, Mock<INuGetProjectContext> projectContext, Mock<ISettings> settings, bool throwOnShowError)
        {
            uiService.Setup(ui => ui.UIContext).Returns(uiContext.Object);
            uiService.Setup(ui => ui.ProjectContext).Returns(projectContext.Object);
            uiService.Setup(ui => ui.Settings).Returns(settings.Object);
            if (throwOnShowError)
            {
                uiService.Setup(ui => ui.ShowError(It.IsAny<Exception>())).Callback((Exception exception) =>
                {
                    throw exception;
                });
            }

            uiService.Setup(ui => ui.PromptForPackageManagementFormat(It.IsAny<PackageManagementFormat>())).Returns(true);
        }

        private static void SetupProjectManagerService(Mock<IServiceBroker> serviceBroker, List<ProjectAction> listProjectActions,
            PackageReferenceContextInfo[] topLevelInstalledPackages, TransitivePackageReferenceContextInfo[] transitiveInstalledPackages)
        {
            var mockNuGetProjectManagerService = new Mock<INuGetProjectManagerService>();

            InstalledAndTransitivePackages installedAndTransitive = new(topLevelInstalledPackages, transitiveInstalledPackages);

            mockNuGetProjectManagerService
                .Setup(mgr => mgr.GetInstalledAndTransitivePackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(installedAndTransitive));
            mockNuGetProjectManagerService
                .Setup(mgr => mgr.GetInstalledAndTransitivePackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(installedAndTransitive));

            mockNuGetProjectManagerService
                .Setup(mgr => mgr.GetInstallActionsAsync(It.IsAny<IReadOnlyCollection<string>>(),
                It.IsAny<PackageIdentity>(),
                It.IsAny<VersionConstraints>(),
                It.IsAny<bool>(),
                It.IsAny<Resolver.DependencyBehavior>(),
                It.IsAny<IReadOnlyList<string>>(),
                It.IsAny<VersionRange>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyList<ProjectAction>>(listProjectActions));

            var metadata = new Dictionary<string, object>
            {
                [NuGetProjectMetadataKeys.UniqueName] = "a",
                [NuGetProjectMetadataKeys.ProjectId] = "a"
            };

            ProjectMetadataContextInfo projectMetadataContextInfo = ProjectMetadataContextInfo.Create(metadata);
            mockNuGetProjectManagerService
                .Setup(mgr => mgr.GetMetadataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(projectMetadataContextInfo));

            List<IPackageReferenceContextInfo> installedAndTransitivePackageReferenceContextInfo =
                new List<IPackageReferenceContextInfo>(capacity: topLevelInstalledPackages.Length + transitiveInstalledPackages.Length);
            installedAndTransitivePackageReferenceContextInfo.AddRange(topLevelInstalledPackages);
            installedAndTransitivePackageReferenceContextInfo.AddRange(transitiveInstalledPackages);

            mockNuGetProjectManagerService.Setup(mgr => mgr.GetInstalledPackagesAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedAndTransitivePackageReferenceContextInfo));

            _ = serviceBroker.Setup(sb => sb.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(s => s == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetProjectManagerService>(mockNuGetProjectManagerService.Object));
        }

        private static void SetupActivePackageSource(PackageSource packageSource, Mock<INuGetUI> uiService)
        {
            Assumes.NotNull(packageSource);

            var packageSourceMoniker = new PackageSourceMoniker(
                packageSource.Name,
                packageSources: new List<PackageSourceContextInfo>() { PackageSourceContextInfo.Create(packageSource) },
                priorityOrder: 0);
            uiService.SetupGet(ui => ui.ActivePackageSourceMoniker).Returns(packageSourceMoniker);
        }

        private static ISourceRepositoryProvider SetupSourceRepositoryProvider(
            PackageSource localPackageSource,
            PackageSource remotePackageSource,
            Mock<PackageMetadataResource> mockPackageMetadataResource,
            out SourceRepository localSourceRepository,
            out SourceRepository remoteSourceRepository)
        {
            var mockSourceRepositoryProvider = new Mock<ISourceRepositoryProvider>();
            var packageMetadataResourceProvider = new Mock<INuGetResourceProvider>();
            var sourceRepositories = new List<SourceRepository>();
            localSourceRepository = null;
            remoteSourceRepository = null;

            packageMetadataResourceProvider
                .Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                    .Returns(() => Task.FromResult(Tuple.Create(true, (INuGetResource)mockPackageMetadataResource.Object)));
            packageMetadataResourceProvider
                .Setup(x => x.ResourceType)
                .Returns(typeof(PackageMetadataResource));

            if (localPackageSource != null)
            {
                localSourceRepository = SetupSourceRepository(localPackageSource, mockPackageMetadataResource, packageMetadataResourceProvider);
                sourceRepositories.Add(localSourceRepository);
            }

            if (remotePackageSource != null)
            {
                remoteSourceRepository = SetupSourceRepository(remotePackageSource, mockPackageMetadataResource, packageMetadataResourceProvider);
                sourceRepositories.Add(remoteSourceRepository);
            }

            mockSourceRepositoryProvider.Setup(p => p.GetRepositories()).Returns(sourceRepositories);

            return mockSourceRepositoryProvider.Object;
        }

        private static SourceRepository SetupSourceRepository(PackageSource remotePackageSource, Mock<PackageMetadataResource> mockPackageMetadataResource, Mock<INuGetResourceProvider> packageMetadataResourceProvider)
        {
            var mockRemoteSourceRepository = new Mock<SourceRepository>(remotePackageSource, new List<INuGetResourceProvider> { packageMetadataResourceProvider.Object });
            mockRemoteSourceRepository.SetupGet(m => m.PackageSource).Returns(remotePackageSource);

            // Required by UIActionEngine to check license metadata.
            mockRemoteSourceRepository.Setup(m => m.GetResource<PackageMetadataResource>(It.IsAny<CancellationToken>())).Returns(mockPackageMetadataResource.Object);
            return mockRemoteSourceRepository.Object;
        }

        private static void SetupPackageSearchMetadata(Mock<PackageMetadataResource> mockPackageMetadataResource, PackageIdentity packageIdentity)
        {
            IPackageSearchMetadata packageMetadata = PackageSearchMetadataBuilder.FromIdentity(packageIdentity).Build();
            mockPackageMetadataResource
                    .Setup(x => x.GetMetadataAsync(It.Is<PackageIdentity>(p => p.Id == packageIdentity.Id), It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(packageMetadata);
        }

        private static void SetupPackagesConfigProject(Mock<INuGetUI> uiService, string projectId)
        {
            var projectContextInfo = new ProjectContextInfo(projectId, ProjectModel.ProjectStyle.PackagesConfig, NuGetProjectKind.PackagesConfig);
            uiService.Setup(ui => ui.Projects).Returns(new[] { projectContextInfo });
        }

        protected void ConfigureNuGetUIWithPackageSourceMapping(
            Mock<INuGetUIContext> mockNuGetUIContext,
            IReadOnlyDictionary<string, IReadOnlyList<string>> packageSourceMappingPatterns)
        {
            if (packageSourceMappingPatterns is null)
            {
                packageSourceMappingPatterns = new Dictionary<string, IReadOnlyList<string>>();
            }

            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(packageSourceMappingPatterns);
            mockNuGetUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);
        }

        private sealed class PackageIdentitySubclass : PackageIdentity
        {
            public PackageIdentitySubclass(string id, NuGetVersion version)
                : base(id, version)
            {
            }

            public override string ToString()
            {
                return "If this displays, it is a bug";
            }
        }
    }
}
