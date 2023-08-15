// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NuGet.Common;
using NuGet.PackageManagement.Telemetry;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Xunit;
using ContractsItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;

namespace NuGet.PackageManagement.Test.Telemetry
{
    public class NavigatedTelemetryEventTests
    {
        private TelemetryEvent _lastTelemetryEvent;

        [Fact]
        public void Constructor_WithValidProperties_CreatedWithoutPiiData()
        {
            // Arrange
            SetupTelemetryListener();

            // Arbitrary values chosen here.
            NavigationType navigationType = NavigationType.Hyperlink;
            NavigationOrigin navigationOrigin = NavigationOrigin.Options_PackageSourceMapping_RemoveAll;

            var evt = new NavigatedTelemetryEvent(navigationType, navigationOrigin);

            // Act
            TelemetryActivity.NuGetTelemetryService.EmitTelemetryEvent(evt);

            // Assert
            Assert.NotNull(_lastTelemetryEvent);
            Assert.Equal(2, _lastTelemetryEvent.Count);
            Assert.Equal(navigationType, _lastTelemetryEvent[NavigatedTelemetryEvent.NavigationTypePropertyName]);
            Assert.Equal(navigationOrigin, _lastTelemetryEvent[NavigatedTelemetryEvent.OriginPropertyName]);
            Assert.Empty(_lastTelemetryEvent.GetPiiData());
        }

        [Fact]
        public void CreateWithExternalLink_WithValidProperties_CreatedWithoutPiiData()
        {
            // Arrange
            SetupTelemetryListener();

            HyperlinkType hyperlinkTab = HyperlinkType.DeprecationAlternativeDetails;
            ContractsItemFilter currentTab = ContractsItemFilter.UpdatesAvailable;
            bool isSolutionView = true;

            var evt = NavigatedTelemetryEvent.CreateWithExternalLink(hyperlinkTab, currentTab, isSolutionView);

            // Act
            TelemetryActivity.NuGetTelemetryService.EmitTelemetryEvent(evt);

            // Assert
            Assert.NotNull(_lastTelemetryEvent);
            Assert.Equal(5, _lastTelemetryEvent.Count);
            Assert.Equal(NavigationType.Hyperlink, _lastTelemetryEvent[NavigatedTelemetryEvent.NavigationTypePropertyName]);
            Assert.Equal(NavigationOrigin.PMUI_ExternalLink, _lastTelemetryEvent[NavigatedTelemetryEvent.OriginPropertyName]);
            Assert.Equal(hyperlinkTab, _lastTelemetryEvent[NavigatedTelemetryEvent.HyperLinkTypePropertyName]);
            Assert.Equal(currentTab, _lastTelemetryEvent[NavigatedTelemetryEvent.CurrentTabPropertyName]);
            Assert.Equal(isSolutionView, _lastTelemetryEvent[NavigatedTelemetryEvent.IsSolutionViewPropertyName]);
            Assert.Empty(_lastTelemetryEvent.GetPiiData());
        }

        [Fact]
        public void CreateWithAddPackageSourceMapping_WithValidProperties_CreatedWithoutPiiData()
        {
            // Arrange
            SetupTelemetryListener();

            int sourcesCount = 3;
            bool isGlobbing = false;

            var evt = NavigatedTelemetryEvent.CreateWithAddPackageSourceMapping(sourcesCount, isGlobbing);

            // Act
            TelemetryActivity.NuGetTelemetryService.EmitTelemetryEvent(evt);

            // Assert
            Assert.NotNull(_lastTelemetryEvent);
            Assert.Equal(4, _lastTelemetryEvent.Count);
            Assert.Equal(NavigationType.Button, _lastTelemetryEvent[NavigatedTelemetryEvent.NavigationTypePropertyName]);
            Assert.Equal(NavigationOrigin.Options_PackageSourceMapping_Add, _lastTelemetryEvent[NavigatedTelemetryEvent.OriginPropertyName]);
            Assert.Equal(sourcesCount, _lastTelemetryEvent[NavigatedTelemetryEvent.SourcesCountPropertyName]);
            Assert.Equal(isGlobbing, _lastTelemetryEvent[NavigatedTelemetryEvent.IsGlobbingPropertyName]);
            Assert.Empty(_lastTelemetryEvent.GetPiiData());
        }

        [Theory]
        [InlineData(PackageSourceMappingStatus.Unspecified)]
        [InlineData(PackageSourceMappingStatus.Mapped)]
        public void CreateWithPMUIConfigurePackageSourceMapping_WithValidProperties_CreatedWithoutPiiData(PackageSourceMappingStatus packageSourceMappingStatus)
        {
            // Arrange
            SetupTelemetryListener();

            ContractsItemFilter currentTab = ContractsItemFilter.UpdatesAvailable;
            bool isSolutionView = true;

            var evt = NavigatedTelemetryEvent.CreateWithPMUIConfigurePackageSourceMapping(currentTab, isSolutionView, packageSourceMappingStatus);

            // Act
            TelemetryActivity.NuGetTelemetryService.EmitTelemetryEvent(evt);

            // Assert
            Assert.NotNull(_lastTelemetryEvent);
            Assert.Equal(5, _lastTelemetryEvent.Count);

            Assert.Equal(NavigationType.Button, _lastTelemetryEvent[NavigatedTelemetryEvent.NavigationTypePropertyName]);
            Assert.Equal(NavigationOrigin.PMUI_PackageSourceMapping_Configure, _lastTelemetryEvent[NavigatedTelemetryEvent.OriginPropertyName]);
            Assert.Equal(currentTab, _lastTelemetryEvent[NavigatedTelemetryEvent.CurrentTabPropertyName]);
            Assert.Equal(isSolutionView, _lastTelemetryEvent[NavigatedTelemetryEvent.IsSolutionViewPropertyName]);
            Assert.Equal(packageSourceMappingStatus, _lastTelemetryEvent[NavigatedTelemetryEvent.PackageSourceMappingStatusPropertyName]);
            Assert.Empty(_lastTelemetryEvent.GetPiiData());
        }

        [Fact]
        public void CreateWithAlternatePackageNavigation_WithAlternatePackageId_GetPiiDataFound()
        {
            // Arrange
            HyperlinkType hyperlinkTab = HyperlinkType.DeprecationAlternativeDetails;
            ContractsItemFilter currentTab = ContractsItemFilter.UpdatesAvailable;
            bool isSolutionView = true;
            string alternativePackageId = "alternate package";

            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            _ = telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            var evt = NavigatedTelemetryEvent.CreateWithAlternatePackageNavigation(hyperlinkTab, currentTab, isSolutionView, alternativePackageId);

            // Act
            service.EmitTelemetryEvent(evt);

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            Assert.Equal(hyperlinkTab, lastTelemetryEvent[NavigatedTelemetryEvent.HyperLinkTypePropertyName]);
            Assert.Equal(currentTab, lastTelemetryEvent[NavigatedTelemetryEvent.CurrentTabPropertyName]);
            Assert.Equal(isSolutionView, lastTelemetryEvent[NavigatedTelemetryEvent.IsSolutionViewPropertyName]);
            Assert.Equal(alternativePackageId, lastTelemetryEvent.GetPiiData().Where(x => x.Key == NavigatedTelemetryEvent.AlternativePackageIdPropertyName).Select(x => x.Value).First());
        }

        [Fact]
        public void CreateWithAlternatePackageNavigation_CorrelatesSearchSelectionAndAction_Succeeds()
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            _ = telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            var testPackageId = "testPackage.id";
            var testPackageVersion = new NuGetVersion(1, 0, 0);

            var evtHyperlink = NavigatedTelemetryEvent.CreateWithAlternatePackageNavigation(
                HyperlinkType.DeprecationAlternativeDetails,
                ContractsItemFilter.All,
                isSolutionView: false,
                testPackageId);

            var evtSearch = new SearchSelectionTelemetryEvent(
                parentId: It.IsAny<Guid>(),
                recommendedCount: It.IsAny<int>(),
                itemIndex: It.IsAny<int>(),
                packageId: testPackageId,
                packageVersion: testPackageVersion,
                isPackageVulnerable: It.IsAny<bool>(),
                isPackageDeprecated: true,
                hasDeprecationAlternativePackage: true);

            var evtActions = new VSActionsTelemetryEvent(
                operationId: It.IsAny<string>(),
                projectIds: new[] { Guid.NewGuid().ToString() },
                operationType: NuGetOperationType.Install,
                source: OperationSource.PMC,
                startTime: DateTimeOffset.Now.AddSeconds(-1),
                status: NuGetOperationStatus.NoOp,
                packageCount: 1,
                endTime: DateTimeOffset.Now,
                duration: .40,
                isPackageSourceMappingEnabled: false);

            // Simulate UIActionEngine.AddUiActionEngineTelemetryProperties()
            var pkgAdded = new TelemetryEvent(eventName: string.Empty);
            pkgAdded.AddPiiData("id", VSTelemetryServiceUtility.NormalizePackageId(testPackageId));
            pkgAdded.AddPiiData("version", testPackageVersion.ToNormalizedString());

            var packages = new List<TelemetryEvent>
            {
                pkgAdded
            };

            evtActions.ComplexData["AddedPackages"] = packages;

            // Act
            service.EmitTelemetryEvent(evtHyperlink);
            var hyperlinkEmitted = lastTelemetryEvent;
            service.EmitTelemetryEvent(evtSearch);
            var searchEmitted = lastTelemetryEvent;
            service.EmitTelemetryEvent(evtActions);
            var actionEmitted = lastTelemetryEvent;

            // Assert
            var packageIdHyperlink = hyperlinkEmitted.GetPiiData().First(x => x.Key == NavigatedTelemetryEvent.AlternativePackageIdPropertyName).Value;
            var packageIdSearch = searchEmitted.GetPiiData().First(x => x.Key == "PackageId").Value;
            var packageIdsAction = (IEnumerable<TelemetryEvent>)actionEmitted.ComplexData["AddedPackages"];
            var packageIds = packageIdsAction.Select(x => x.GetPiiData().First(x => x.Key == "id").Value);
            Assert.Equal(packageIdHyperlink, packageIdSearch);
            Assert.Contains(packageIdHyperlink, packageIds);
        }

        private Mock<ITelemetrySession> SetupTelemetryListener()
        {
            var telemetrySession = new Mock<ITelemetrySession>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => _lastTelemetryEvent = x);
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;
            return telemetrySession;
        }
    }
}

