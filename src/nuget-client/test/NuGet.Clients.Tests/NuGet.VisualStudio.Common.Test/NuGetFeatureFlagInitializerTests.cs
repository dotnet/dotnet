// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    [Collection(MockedVS.Collection)]
    public class NuGetFeatureFlagInitializerTests
    {
        public NuGetFeatureFlagInitializerTests(GlobalServiceProvider globalServiceProvider)
        {
            globalServiceProvider.Reset();
            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(ThreadHelper.JoinableTaskFactory);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task InitializeAsync_PublishesPowerShellScriptExecutionFlag(bool flagEnabled)
        {
            var featureFlags = new Mock<INuGetFeatureFlagService>(MockBehavior.Strict);
            featureFlags.Setup(x => x.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.ControlPowerShellScriptExecution))
                .ReturnsAsync(flagEnabled);
            var initializer = new NuGetFeatureFlagInitializer(
                featureFlags.Object, Mock.Of<INuGetExperimentationService>());

            string switchName = NuGetFeatureFlagConstants.ControlPowerShellScriptExecution.Name;
            AppContext.TryGetSwitch(switchName, out bool previousValue);
            try
            {
                AppContext.SetSwitch(switchName, !flagEnabled);

                await initializer.InitializeAsync();

                AppContext.TryGetSwitch(switchName, out bool enabled).Should().BeTrue();
                enabled.Should().Be(flagEnabled);
            }
            finally
            {
                AppContext.SetSwitch(switchName, previousValue);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InitializeAsync_WhenLookupFails_LeavesSwitchesUnchangedWithoutRetry(bool experimentFails)
        {
            var failure = new InvalidOperationException("Feature lookup failed.");
            var experimentationService = new Mock<INuGetExperimentationService>(MockBehavior.Strict);
            var experiment = experimentationService.Setup(
                x => x.IsExperimentEnabled(ExperimentationConstants.UseSystemTextJsonDeserialization));
            if (experimentFails)
            {
                experiment.Throws(failure);
            }
            else
            {
                experiment.Returns(true);
            }

            var featureFlags = new Mock<INuGetFeatureFlagService>(MockBehavior.Strict);
            featureFlags.Setup(x => x.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.ControlPowerShellScriptExecution))
                .ThrowsAsync(failure);
            var initializer = new NuGetFeatureFlagInitializer(featureFlags.Object, experimentationService.Object);
            string jsonSwitchName = ExperimentationConstants.UseSystemTextJsonDeserialization.FlightFlag;
            string powerShellSwitchName = NuGetFeatureFlagConstants.ControlPowerShellScriptExecution.Name;
            AppContext.TryGetSwitch(jsonSwitchName, out bool previousJsonValue);
            AppContext.TryGetSwitch(powerShellSwitchName, out bool previousPowerShellValue);
            try
            {
                AppContext.SetSwitch(jsonSwitchName, false);
                AppContext.SetSwitch(powerShellSwitchName, true);

                await initializer.InitializeAsync();
                await initializer.InitializeAsync();

                AppContext.TryGetSwitch(jsonSwitchName, out bool jsonEnabled);
                jsonEnabled.Should().BeFalse();
                AppContext.TryGetSwitch(powerShellSwitchName, out bool powerShellEnabled);
                powerShellEnabled.Should().BeTrue();
                experimentationService.Verify(
                    x => x.IsExperimentEnabled(ExperimentationConstants.UseSystemTextJsonDeserialization), Times.Once);
                featureFlags.Verify(
                    x => x.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.ControlPowerShellScriptExecution),
                    experimentFails ? Times.Never() : Times.Once());
            }
            finally
            {
                AppContext.SetSwitch(jsonSwitchName, previousJsonValue);
                AppContext.SetSwitch(powerShellSwitchName, previousPowerShellValue);
            }
        }

        [Fact]
        public async Task InitializeAsync_ConcurrentCallers_WaitForSharedInitializationAndPublishBothSwitchesOnce()
        {
            var experimentationService = new Mock<INuGetExperimentationService>(MockBehavior.Strict);
            experimentationService.Setup(x => x.IsExperimentEnabled(ExperimentationConstants.UseSystemTextJsonDeserialization))
                .Returns(true);
            var flagAvailable = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var featureFlags = new Mock<INuGetFeatureFlagService>(MockBehavior.Strict);
            featureFlags.Setup(x => x.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.ControlPowerShellScriptExecution))
                .Returns(flagAvailable.Task);
            var initializer = new NuGetFeatureFlagInitializer(featureFlags.Object, experimentationService.Object);

            string jsonSwitchName = ExperimentationConstants.UseSystemTextJsonDeserialization.FlightFlag;
            string powerShellSwitchName = NuGetFeatureFlagConstants.ControlPowerShellScriptExecution.Name;
            AppContext.TryGetSwitch(jsonSwitchName, out bool previousJsonValue);
            AppContext.TryGetSwitch(powerShellSwitchName, out bool previousValue);
            try
            {
                AppContext.SetSwitch(jsonSwitchName, false);
                AppContext.SetSwitch(powerShellSwitchName, false);

                Task firstCaller = initializer.InitializeAsync();
                Task secondCaller = initializer.InitializeAsync();

                firstCaller.IsCompleted.Should().BeFalse();
                secondCaller.IsCompleted.Should().BeFalse();
                AppContext.TryGetSwitch(powerShellSwitchName, out bool enabled);
                enabled.Should().BeFalse();

                flagAvailable.SetResult(true);
                await Task.WhenAll(firstCaller, secondCaller);
                await initializer.InitializeAsync();

                AppContext.TryGetSwitch(powerShellSwitchName, out enabled).Should().BeTrue();
                enabled.Should().BeTrue();
                AppContext.TryGetSwitch(jsonSwitchName, out bool jsonEnabled).Should().BeTrue();
                jsonEnabled.Should().BeTrue();
                experimentationService.Verify(
                    x => x.IsExperimentEnabled(ExperimentationConstants.UseSystemTextJsonDeserialization), Times.Once);
                featureFlags.Verify(
                    x => x.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.ControlPowerShellScriptExecution), Times.Once);
            }
            finally
            {
                flagAvailable.TrySetCanceled();
                AppContext.SetSwitch(jsonSwitchName, previousJsonValue);
                AppContext.SetSwitch(powerShellSwitchName, previousValue);
            }
        }
    }
}
