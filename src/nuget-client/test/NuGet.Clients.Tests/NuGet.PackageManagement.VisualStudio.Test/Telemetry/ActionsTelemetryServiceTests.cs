// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NuGet.Common;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class ActionsTelemetryServiceTests
    {
        [Theory]
        [InlineData(NuGetProjectActionType.Install, OperationSource.UI)]
        [InlineData(NuGetProjectActionType.Update, OperationSource.UI)]
        [InlineData(NuGetProjectActionType.Uninstall, OperationSource.UI)]
        [InlineData(NuGetProjectActionType.Install, OperationSource.PMC)]
        [InlineData(NuGetProjectActionType.Update, OperationSource.PMC)]
        [InlineData(NuGetProjectActionType.Uninstall, OperationSource.PMC)]
        public void ActionsTelemetryService_EmitActionEvent_OperationSucceed(NuGetProjectActionType operationType, OperationSource source)
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
                operationType: operationType,
                source: source,
                startTime: DateTimeOffset.Now.AddSeconds(-3),
                status: NuGetOperationStatus.Succeeded,
                packageCount: 1,
                endTime: DateTimeOffset.Now,
                duration: 2.10,
                isPackageSourceMappingEnabled: true);

            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            // Act
            service.EmitTelemetryEvent(actionTelemetryData);

            // Assert
            VerifyTelemetryEventData(operationId, actionTelemetryData, lastTelemetryEvent);
        }

        [Theory]
        [InlineData(NuGetProjectActionType.Install, OperationSource.UI)]
        [InlineData(NuGetProjectActionType.Update, OperationSource.UI)]
        [InlineData(NuGetProjectActionType.Uninstall, OperationSource.UI)]
        [InlineData(NuGetProjectActionType.Install, OperationSource.PMC)]
        [InlineData(NuGetProjectActionType.Update, OperationSource.PMC)]
        [InlineData(NuGetProjectActionType.Uninstall, OperationSource.PMC)]
        public void ActionsTelemetryService_EmitActionEvent_OperationFailed(NuGetProjectActionType operationType, OperationSource source)
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
                operationType: operationType,
                source: source,
                startTime: DateTimeOffset.Now.AddSeconds(-2),
                status: NuGetOperationStatus.Failed,
                packageCount: 1,
                endTime: DateTimeOffset.Now,
                duration: 1.30,
                isPackageSourceMappingEnabled: false);

            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            // Act
            service.EmitTelemetryEvent(actionTelemetryData);

            // Assert
            VerifyTelemetryEventData(operationId, actionTelemetryData, lastTelemetryEvent);
        }

        [Theory]
        [InlineData(NuGetProjectActionType.Install)]
        [InlineData(NuGetProjectActionType.Update)]
        [InlineData(NuGetProjectActionType.Uninstall)]
        public void ActionsTelemetryService_EmitActionEvent_OperationNoOp(NuGetProjectActionType operationType)
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
                operationType: operationType,
                source: OperationSource.PMC,
                startTime: DateTimeOffset.Now.AddSeconds(-1),
                status: NuGetOperationStatus.NoOp,
                packageCount: 1,
                endTime: DateTimeOffset.Now,
                duration: .40,
                isPackageSourceMappingEnabled: false);
            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            // Act
            service.EmitTelemetryEvent(actionTelemetryData);

            // Assert
            VerifyTelemetryEventData(operationId, actionTelemetryData, lastTelemetryEvent);
        }

        [Theory]
        [MemberData(nameof(GetStepNames))]
        public void ActionsTelemetryService_EmitActionStepsEvent(string stepName)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var duration = 1.12;
            var service = new NuGetVSTelemetryService(telemetrySession.Object);

            var parentId = Guid.NewGuid().ToString();

            // Act
            service.EmitTelemetryEvent(new ActionTelemetryStepEvent(parentId, stepName, duration));

            // Assert
            Assert.NotNull(lastTelemetryEvent);
            Assert.Equal(ActionTelemetryStepEvent.NugetActionStepsEventName, lastTelemetryEvent.Name);
            Assert.Equal(3, lastTelemetryEvent.Count);

            Assert.Equal(parentId, lastTelemetryEvent["ParentId"].ToString());
            Assert.Equal(stepName, lastTelemetryEvent["SubStepName"].ToString());
            Assert.Equal(duration, (double)lastTelemetryEvent["Duration"]);
        }

        public static IEnumerable<object[]> GetStepNames()
        {
            yield return new[] { TelemetryConstants.PreviewBuildIntegratedStepName };
            yield return new[] { TelemetryConstants.GatherDependencyStepName };
            yield return new[] { TelemetryConstants.ResolveDependencyStepName };
            yield return new[] { TelemetryConstants.ResolvedActionsStepName };
            yield return new[] { TelemetryConstants.ExecuteActionStepName };
        }

        private void VerifyTelemetryEventData(string operationId, VSActionsTelemetryEvent expected, TelemetryEvent actual)
        {
            Assert.NotNull(actual);
            Assert.Equal(ActionsTelemetryEvent.NugetActionEventName, actual.Name);
            Assert.Equal(11, actual.Count);

            Assert.Equal(expected.OperationType.ToString(), actual["OperationType"].ToString());
            Assert.Equal(expected.Source.ToString(), actual["Source"].ToString());

            TestTelemetryUtility.VerifyTelemetryEventData(operationId, expected, actual);
        }
    }
}
