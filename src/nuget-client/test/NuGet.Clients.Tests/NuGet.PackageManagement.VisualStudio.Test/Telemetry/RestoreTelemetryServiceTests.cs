// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NuGet.Common;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class RestoreTelemetryServiceTests
    {
        [Theory]
        [InlineData(false, RestoreOperationSource.OnBuild, NuGetOperationStatus.Succeeded, ExplicitRestoreReason.None)]
        [InlineData(false, RestoreOperationSource.Explicit, NuGetOperationStatus.Succeeded, ExplicitRestoreReason.ProjectRetargeting)]
        [InlineData(false, RestoreOperationSource.OnBuild, NuGetOperationStatus.NoOp, ExplicitRestoreReason.None)]
        [InlineData(false, RestoreOperationSource.Explicit, NuGetOperationStatus.NoOp, ExplicitRestoreReason.MissingPackagesBanner)]
        [InlineData(false, RestoreOperationSource.OnBuild, NuGetOperationStatus.Failed, ExplicitRestoreReason.None)]
        [InlineData(false, RestoreOperationSource.Explicit, NuGetOperationStatus.Failed, ExplicitRestoreReason.RestoreSolutionPackages)]
        [InlineData(true, RestoreOperationSource.OnBuild, NuGetOperationStatus.Succeeded, ExplicitRestoreReason.None)]
        [InlineData(true, RestoreOperationSource.Explicit, NuGetOperationStatus.Succeeded, ExplicitRestoreReason.ProjectRetargeting)]
        [InlineData(true, RestoreOperationSource.OnBuild, NuGetOperationStatus.NoOp, ExplicitRestoreReason.None)]
        [InlineData(true, RestoreOperationSource.Explicit, NuGetOperationStatus.NoOp, ExplicitRestoreReason.MissingPackagesBanner)]
        [InlineData(true, RestoreOperationSource.OnBuild, NuGetOperationStatus.Failed, ExplicitRestoreReason.None)]
        [InlineData(true, RestoreOperationSource.Explicit, NuGetOperationStatus.Failed, ExplicitRestoreReason.RestoreSolutionPackages)]
        public void RestoreTelemetryService_EmitRestoreEvent_OperationSucceed(bool forceRestore, RestoreOperationSource source, NuGetOperationStatus status, ExplicitRestoreReason explicitRestoreReason)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var noopProjectsCount = 0;

            if (status == NuGetOperationStatus.NoOp)
            {
                noopProjectsCount = 1;
            }

            var operationId = Guid.NewGuid().ToString();

            var restoreTelemetryData = new RestoreTelemetryEvent(
                operationId,
                projectIds: new[] { Guid.NewGuid().ToString() },
                forceRestore: forceRestore,
                source: source,
                startTime: DateTimeOffset.Now.AddSeconds(-3),
                status: status,
                packageCount: 2,
                noOpProjectsCount: noopProjectsCount,
                upToDateProjectsCount: 5,
                unknownProjectsCount: 0,
                projectJsonProjectsCount: 0,
                packageReferenceProjectsCount: 0,
                legacyPackageReferenceProjectsCount: 0,
                cpsPackageReferenceProjectsCount: 0,
                dotnetCliToolProjectsCount: 0,
                packagesConfigProjectsCount: 1,
                endTime: DateTimeOffset.Now,
                duration: 2.10,
                additionalTrackingData: new Dictionary<string, object>()
                {
                    { nameof(RestoreTelemetryEvent.IsSolutionLoadRestore), true },
                    { nameof(ExplicitRestoreReason), explicitRestoreReason }
                },
                new IntervalTracker("Activity"),
                isPackageSourceMappingEnabled: false,
                httpFeedsCount: 1,
                localFeedsCount: 2,
                hasNuGetOrg: true,
                hasVSOfflineFeed: false);
            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            // Act
            service.EmitTelemetryEvent(restoreTelemetryData);

            // Assert
            VerifyTelemetryEventData(operationId, restoreTelemetryData, lastTelemetryEvent);
        }

        [Theory]
        [InlineData("")]
        [InlineData("nuget.org,nuget")]
        [InlineData(" nuget.org , nuget | privateRepository , private* ")]
        public void RestoreTelemetryService_EmitRestoreEvent_IntervalsAreCaptured(string _packageSourceMapping)
        {
            // Arrange
            var first = "first";
            var second = "second";
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);
            var tracker = new IntervalTracker("Activity");

            using (tracker.Start(first))
            {
            }

            using (tracker.Start(second))
            {
            }

            var operationId = Guid.NewGuid().ToString();
            var packageSourceMapping = string.IsNullOrEmpty(_packageSourceMapping) ? null : PackageSourceMappingUtility.GetPackageSourceMapping(_packageSourceMapping);
            bool isPackageSourceMappingEnabled = packageSourceMapping?.IsEnabled ?? false;

            var restoreTelemetryData = new RestoreTelemetryEvent(
                operationId,
                projectIds: new[] { Guid.NewGuid().ToString() },
                forceRestore: false,
                source: RestoreOperationSource.OnBuild,
                startTime: DateTimeOffset.Now.AddSeconds(-3),
                status: NuGetOperationStatus.Succeeded,
                packageCount: 1,
                noOpProjectsCount: 0,
                upToDateProjectsCount: 0,
                unknownProjectsCount: 0,
                projectJsonProjectsCount: 0,
                packageReferenceProjectsCount: 1,
                legacyPackageReferenceProjectsCount: 0,
                cpsPackageReferenceProjectsCount: 1,
                dotnetCliToolProjectsCount: 0,
                packagesConfigProjectsCount: 0,
                endTime: DateTimeOffset.Now,
                duration: 2.10,
                additionalTrackingData: new Dictionary<string, object>()
                {
                    { nameof(RestoreTelemetryEvent.IsSolutionLoadRestore), true }
                },
                tracker,
                isPackageSourceMappingEnabled: isPackageSourceMappingEnabled,
                httpFeedsCount: 1,
                localFeedsCount: 2,
                hasNuGetOrg: true,
                hasVSOfflineFeed: false);
            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            // Act
            service.EmitTelemetryEvent(restoreTelemetryData);

            // Assert

            Assert.NotNull(lastTelemetryEvent);
            Assert.Equal(RestoreTelemetryEvent.RestoreActionEventName, lastTelemetryEvent.Name);
            Assert.Equal(27, lastTelemetryEvent.Count);

            Assert.Equal(restoreTelemetryData.OperationSource.ToString(), lastTelemetryEvent["OperationSource"].ToString());

            Assert.Equal(restoreTelemetryData.NoOpProjectsCount, (int)lastTelemetryEvent["NoOpProjectsCount"]);
            Assert.Equal(restoreTelemetryData[first], lastTelemetryEvent[first]);
            Assert.Equal(restoreTelemetryData[second], lastTelemetryEvent[second]);
        }

        private void VerifyTelemetryEventData(string operationId, RestoreTelemetryEvent expected, TelemetryEvent actual)
        {
            Assert.NotNull(actual);
            Assert.Equal(RestoreTelemetryEvent.RestoreActionEventName, actual.Name);
            Assert.Equal(26, actual.Count);

            Assert.Equal(expected.OperationSource.ToString(), actual["OperationSource"].ToString());

            Assert.Equal(expected.NoOpProjectsCount, (int)actual["NoOpProjectsCount"]);
            Assert.Equal(expected.ForceRestore, (bool)actual["ForceRestore"]);
            Assert.Equal(expected.IsSolutionLoadRestore, (bool)actual["IsSolutionLoadRestore"]);
            Assert.Equal(expected.UpToDateProjectCount, (int)actual["UpToDateProjectCount"]);
            Assert.Equal(expected.PackageReferenceProjectsCount, (int)actual["PackageReferenceProjectsCount"]);
            Assert.Equal(expected.ProjectJsonProjectsCount, (int)actual["ProjectJsonProjectsCount"]);
            Assert.Equal(expected.PackagesConfigProjectsCount, (int)actual["PackagesConfigProjectsCount"]);
            Assert.Equal(expected.DotnetCliToolProjectsCount, (int)actual["DotnetCliToolProjectsCount"]);
            Assert.Equal(expected.UnknownProjectsCount, (int)actual["UnknownProjectsCount"]);
            Assert.Equal(expected.LegacyPackageReferenceProjectsCount, (int)actual["LegacyPackageReferenceProjectsCount"]);
            Assert.Equal(expected.CpsPackageReferenceProjectsCount, (int)actual["CpsPackageReferenceProjectsCount"]);
            Assert.Equal(expected[RestoreTelemetryEvent.NumHTTPFeeds], (int)actual["NumHTTPFeeds"]);
            Assert.Equal(expected[RestoreTelemetryEvent.NumLocalFeeds], (int)actual["NumLocalFeeds"]);
            Assert.Equal(expected[RestoreTelemetryEvent.NuGetOrg], (bool)actual["NuGetOrg"]);
            Assert.Equal(expected[RestoreTelemetryEvent.VsOfflinePackages], (bool)actual["VsOfflinePackages"]);
            Assert.Equal(1, (int)actual["NumHTTPFeeds"]);
            Assert.Equal(2, (int)actual["NumLocalFeeds"]);
            Assert.True((bool)actual["NuGetOrg"]);
            Assert.False((bool)actual["VsOfflinePackages"]);
            Assert.Equal(expected["ExplicitRestoreReason"], actual["ExplicitRestoreReason"]);
            AssertProjectsCount(expected);

            TestTelemetryUtility.VerifyTelemetryEventData(operationId, expected, actual);
        }

        private void AssertProjectsCount(RestoreTelemetryEvent t)
        {
            Assert.True(t.ProjectsCount >= t.PackageReferenceProjectsCount + t.ProjectJsonProjectsCount + t.PackagesConfigProjectsCount + t.DotnetCliToolProjectsCount + t.UnknownProjectsCount);
            Assert.True(t.PackageReferenceProjectsCount >= t.LegacyPackageReferenceProjectsCount + t.CpsPackageReferenceProjectsCount);
        }
    }
}
