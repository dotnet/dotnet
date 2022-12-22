// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Common;
using Test.Utility;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio.Common.Test
{
    [Collection(MockedVS.Collection)]
    public class NuGetFeatureFlagServiceTests
    {
        private GlobalServiceProvider _globalProvider;

        public NuGetFeatureFlagServiceTests(GlobalServiceProvider sp)
        {
            sp.Reset();
            _globalProvider = sp;
            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(ThreadHelper.JoinableTaskFactory);
        }

        [Fact]
        public void Constructor_WithNullWrapper_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetFeatureFlagService(null, AsyncServiceProvider.GlobalProvider));
        }

        [Fact]
        public void Constructor_WithNullAsyncServiceProvider_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetFeatureFlagService(new TestEnvironmentVariableReader(new Dictionary<string, string>()), null));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]

        public async Task IsFeatureEnabledAsync_WithoutFlag_ReturnsDefaultValueFromConstant(bool featureFlagDefault)
        {
            var featureFlagConstant = new NuGetFeatureFlagConstants("featureFlag", "featureEnvVar", defaultState: featureFlagDefault);
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            Mock.Get(vsFeatureFlags)
                .Setup(x => x.IsFeatureEnabled(
                    It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(featureFlagDefault);

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);
            var service = new NuGetFeatureFlagService(new EnvironmentVariableWrapper(), AsyncServiceProvider.GlobalProvider);
            (await service.IsFeatureEnabledAsync(featureFlagConstant)).Should().Be(featureFlagDefault);
        }

        [Fact]
        public async Task IsFeatureEnabledAsync_WithEnabledFeatureFlagAndForcedEnabledEnvVar_ReturnsTrue()
        {
            var featureFlagConstant = new NuGetFeatureFlagConstants("featureFlag", "featureEnvVar", defaultState: false);
            var envVars = new Dictionary<string, string>()
            {
                { featureFlagConstant.EnvironmentVariable, "1" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            Mock.Get(vsFeatureFlags)
                .Setup(x => x.IsFeatureEnabled(
                    It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(true);

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);
            var service = new NuGetFeatureFlagService(envVarWrapper, AsyncServiceProvider.GlobalProvider);
            (await service.IsFeatureEnabledAsync(featureFlagConstant)).Should().Be(true);

        }

        [Theory]
        [InlineData("2")]
        [InlineData("randomValue")]
        public async Task IsFeatureEnabledAsync_WithEnvVarWithIncorrectValue_WithEnvironmentVariable__ReturnsFalse(string value)
        {
            var featureFlagConstant = new NuGetFeatureFlagConstants("featureFlag", "featureEnvVar", defaultState: false);
            var envVars = new Dictionary<string, string>()
            {
                { featureFlagConstant.EnvironmentVariable, value },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            Mock.Get(vsFeatureFlags)
                .Setup(x => x.IsFeatureEnabled(
                    It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(false);

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);
            var service = new NuGetFeatureFlagService(envVarWrapper, AsyncServiceProvider.GlobalProvider);
            (await service.IsFeatureEnabledAsync(featureFlagConstant)).Should().Be(false);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task IsFeatureEnabledAsync_WithEnvVarNotSetWithEnabledFeatureFromWithFeatureFlagService_ReturnsExpectedResult(bool isFeatureEnabled, bool expectedResult)
        {
            var featureFlagConstant = new NuGetFeatureFlagConstants("featureFlag", "featureEnvVar", defaultState: false);
            var envVarWrapper = new TestEnvironmentVariableReader(new Dictionary<string, string>());
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            Mock.Get(vsFeatureFlags)
                .Setup(x => x.IsFeatureEnabled(
                    It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(isFeatureEnabled);

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);
            var service = new NuGetFeatureFlagService(envVarWrapper, AsyncServiceProvider.GlobalProvider);
            (await service.IsFeatureEnabledAsync(featureFlagConstant)).Should().Be(expectedResult);
        }

        [Fact]
        public async Task IsFeatureEnabledAsync_WithEnvVarEnabled_WithFeatureFlagServiceDisabled_ReturnsTrue()
        {
            var featureFlagConstant = new NuGetFeatureFlagConstants("featureFlag", "featureEnvVar", defaultState: false);
            var envVars = new Dictionary<string, string>()
            {
                { featureFlagConstant.EnvironmentVariable, "1" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            Mock.Get(vsFeatureFlags)
                .Setup(x => x.IsFeatureEnabled(
                    It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(false);

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);
            var service = new NuGetFeatureFlagService(envVarWrapper, AsyncServiceProvider.GlobalProvider);
            (await service.IsFeatureEnabledAsync(featureFlagConstant)).Should().Be(true);
        }

        [Fact]
        public async Task IsFeatureEnabledAsync_WithEnvVarDisabled_WithFeatureFlagServiceEnabled_ReturnsFalse()
        {
            var featureFlagConstant = new NuGetFeatureFlagConstants("featureFlag", "featureEnvVar", defaultState: false);
            var envVars = new Dictionary<string, string>()
            {
                { featureFlagConstant.EnvironmentVariable, "0" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            Mock.Get(vsFeatureFlags)
                .Setup(x => x.IsFeatureEnabled(
                    It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(true);

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);
            var service = new NuGetFeatureFlagService(envVarWrapper, AsyncServiceProvider.GlobalProvider);
            (await service.IsFeatureEnabledAsync(featureFlagConstant)).Should().Be(false);
        }

        [Fact]
        public async Task IsFeatureEnabledAsync_WithNullEnvironmentVariableForConstant_HandlesGracefully()
        {
            var featureFlagConstant = new NuGetFeatureFlagConstants("featureFlag", null, defaultState: false);
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);
            var service = new NuGetFeatureFlagService(new EnvironmentVariableWrapper(), AsyncServiceProvider.GlobalProvider);
            (await service.IsFeatureEnabledAsync(featureFlagConstant)).Should().Be(false);
        }

        [Fact]
        public async Task IsFeatureEnabledAsync_MultipleFeaturesOverriddenWithDifferentEnvVars_DoNotConflict()
        {
            var forcedOff = new NuGetFeatureFlagConstants("TestExp1", "TEST_EXP_1", defaultState: false);
            var forcedOn = new NuGetFeatureFlagConstants("TestExp2", "TEST_EXP_2", defaultState: false);
            var noOverride = new NuGetFeatureFlagConstants("TestExp3", "TEST_EXP_3", defaultState: false);
            var envVars = new Dictionary<string, string>()
            {
                { forcedOn.EnvironmentVariable, "1" },
                { forcedOff.EnvironmentVariable, "0" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var vsFeatureFlags = Mock.Of<IVsFeatureFlags>();

            Mock.Get(vsFeatureFlags)
                .Setup(x => x.IsFeatureEnabled(
                    It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(true);

            _globalProvider.AddService(typeof(SVsFeatureFlags), vsFeatureFlags);

            var service = new NuGetFeatureFlagService(envVarWrapper, AsyncServiceProvider.GlobalProvider);

            (await service.IsFeatureEnabledAsync(forcedOff)).Should().BeFalse();
            (await service.IsFeatureEnabledAsync(forcedOn)).Should().BeTrue();
            (await service.IsFeatureEnabledAsync(noOverride)).Should().BeTrue();
        }
    }
}
