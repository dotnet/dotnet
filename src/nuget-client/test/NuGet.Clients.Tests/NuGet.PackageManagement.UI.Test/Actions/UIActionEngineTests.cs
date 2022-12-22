// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
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
            IReadOnlyList<PreviewResult> previewResults = await UIActionEngine.GetPreviewResultsAsync(
                Mock.Of<INuGetProjectManagerService>(),
                new[] { uninstallAction, installAction },
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
            IReadOnlyList<PreviewResult> previewResults = await UIActionEngine.GetPreviewResultsAsync(
                Mock.Of<INuGetProjectManagerService>(),
                new[] { installAction },
                CancellationToken.None);

            Assert.Equal(1, previewResults.Count);
            AccessiblePackageIdentity[] addedResults = previewResults[0].Added.ToArray();

            Assert.Equal(3, addedResults.Length);

            Assert.Equal(packageIdentityA.Id, addedResults[0].Id);
            Assert.Equal(packageIdentityB.Id, addedResults[1].Id);
            Assert.Equal(packageIdentityC.Id, addedResults[2].Id);
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
                operationType: NuGetOperationType.Install,
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
                existingPackages: null,
                addedPackages: null,
                removedPackages: null,
                updatedPackagesOld: null,
                updatedPackagesNew: null,
                targetFrameworks: null);

            // Act
            var service = new NuGetVSTelemetryService(telemetrySession.Object);
            service.EmitTelemetryEvent(actionTelemetryData);

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            Assert.NotNull(lastTelemetryEvent.ComplexData["TopLevelVulnerablePackagesMaxSeverities"] as List<int>);
            var pkgSeverities = lastTelemetryEvent.ComplexData["TopLevelVulnerablePackagesMaxSeverities"] as List<int>;
            Assert.Equal(lastTelemetryEvent["TopLevelVulnerablePackagesCount"], pkgSeverities.Count());
            Assert.Collection(pkgSeverities,
                item => Assert.Equal(1, item),
                item => Assert.Equal(1, item),
                item => Assert.Equal(3, item));
            Assert.Equal(3, pkgSeverities.Count());
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
        public async Task CreateInstallAction_OnInstallingProject_EmitsPkgWasTransitiveTelemetryAndTabAndIsSolutionPropertiesAsync(ContractsItemFilter activeTab, bool isSolutionLevel, string packageIdToInstall, bool? expectedPkgWasTransitive)
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

            var action = UserAction.CreateInstallAction(packageIdToInstall, NuGetVersion.Parse("1.0.0"), isSolutionLevel, activeTab);

            // Act
            await uiEngine.PerformInstallOrUninstallAsync(uiService.Object, action, CancellationToken.None);

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            // expect failed action because we mocked just enough objects to emit telemetry
            Assert.Equal(NuGetOperationStatus.Failed, lastTelemetryEvent[nameof(ActionEventBase.Status)]);
            Assert.Equal(NuGetOperationType.Install, lastTelemetryEvent[nameof(ActionsTelemetryEvent.OperationType)]);
            Assert.Equal(isSolutionLevel, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.IsSolutionLevel)]);
            Assert.Equal(activeTab, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.Tab)]);
            Assert.Equal(expectedPkgWasTransitive, lastTelemetryEvent[nameof(VSActionsTelemetryEvent.PackageToInstallWasTransitive)]);
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
