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
    public class HyperlinkClickedTelemetryEventTests
    {
        public static IEnumerable<object[]> GetData()
        {
            var allData = new List<object[]>();

            foreach (var hyperlinkType in Enum.GetValues(typeof(HyperlinkType)))
            {
                foreach (var filter in Enum.GetValues(typeof(ContractsItemFilter)))
                {
                    allData.Add(new object[] { hyperlinkType, filter, true, "a search query" });
                    allData.Add(new object[] { hyperlinkType, filter, false, "a search query" });
                }
            }

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void HyperlinkClicked_RoundTrip_Succeeds(HyperlinkType hyperlinkTab, ContractsItemFilter currentTab, bool isSolutionView, string searchQuery)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            _ = telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            var evt = new HyperlinkClickedTelemetryEvent(hyperlinkTab, currentTab, isSolutionView, searchQuery);

            // Act
            service.EmitTelemetryEvent(evt);

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            Assert.Equal(hyperlinkTab, lastTelemetryEvent[HyperlinkClickedTelemetryEvent.HyperLinkTypePropertyName]);
            Assert.Equal(currentTab, lastTelemetryEvent[HyperlinkClickedTelemetryEvent.CurrentTabPropertyName]);
            Assert.Equal(isSolutionView, lastTelemetryEvent[HyperlinkClickedTelemetryEvent.IsSolutionViewPropertyName]);
            Assert.Equal(searchQuery, lastTelemetryEvent.GetPiiData().Where(x => x.Key == HyperlinkClickedTelemetryEvent.AlternativePackageIdPropertyName).Select(x => x.Value).First());
        }

        [Fact]
        public void HyperlinkClicked_CorrelatesSearchSelectionAndAction_Succeeds()
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

            var evtHyperlink = new HyperlinkClickedTelemetryEvent(
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
            var pkgAdded = new TelemetryEvent(eventName: null);
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
            var packageIdHyperlink = hyperlinkEmitted.GetPiiData().First(x => x.Key == HyperlinkClickedTelemetryEvent.AlternativePackageIdPropertyName).Value;
            var packageIdSearch = searchEmitted.GetPiiData().First(x => x.Key == "PackageId").Value;
            var packageIdsAction = (IEnumerable<TelemetryEvent>)actionEmitted.ComplexData["AddedPackages"];
            var packageIds = packageIdsAction.Select(x => x.GetPiiData().First(x => x.Key == "id").Value);
            Assert.Equal(packageIdHyperlink, packageIdSearch);
            Assert.Contains(packageIdHyperlink, packageIds);
        }
    }
}
