// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class NuGetFeatureFlagsTests
    {
        [Theory]
        [InlineData(null, false, false)]
        [InlineData(null, true, true)]
        [InlineData("1", false, true)]
        [InlineData("0", true, false)]
        [InlineData("invalid", false, false)]
        [InlineData("invalid", true, true)]
        public void IsControlPowerShellScriptExecutionEnabled_WithEnvironmentOverrideAndAppContext_ReturnsExpectedState(
            string? value, bool switchEnabled, bool expected)
        {
            var environment = new TestEnvironmentVariableReader(new Dictionary<string, string?>
            {
                { NuGetFeatureFlags.ControlPowerShellScriptExecutionEnvVar, value }
            });
            AppContext.TryGetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, out bool originalSwitch);
            try
            {
                AppContext.SetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, switchEnabled);
                bool actual = NuGetFeatureFlags.IsControlPowerShellScriptExecutionEnabled(environment);

                Assert.Equal(expected, actual);
            }
            finally
            {
                AppContext.SetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, originalSwitch);
            }
        }

        [Fact]
        public void IsControlPowerShellScriptExecutionEnabled_WhenAppContextSwitchChanges_ReturnsCurrentState()
        {
            var environment = new TestEnvironmentVariableReader(new Dictionary<string, string>());
            AppContext.TryGetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, out bool originalSwitch);
            try
            {
                AppContext.SetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, false);
                Assert.False(NuGetFeatureFlags.IsControlPowerShellScriptExecutionEnabled(environment));

                AppContext.SetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, true);
                Assert.True(NuGetFeatureFlags.IsControlPowerShellScriptExecutionEnabled(environment));

                AppContext.SetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, false);
                Assert.False(NuGetFeatureFlags.IsControlPowerShellScriptExecutionEnabled(environment));
            }
            finally
            {
                AppContext.SetSwitch(NuGetFeatureFlags.ControlPowerShellScriptExecutionSwitchName, originalSwitch);
            }
        }
    }
}
