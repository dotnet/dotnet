// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using NuGet.Common;

namespace NuGet.Shared
{
    internal static class NuGetFeatureFlags
    {
        internal const string UseSystemTextJsonDeserializationSwitchName = "NuGet.UseSystemTextJsonDeserialization";
        internal const string UseSystemTextJsonDeserializationEnvVar = "NUGET_USE_SYSTEM_TEXT_JSON_DESERIALIZATION";

        internal const string ControlPowerShellScriptExecutionSwitchName = "NuGet.ControlPowerShellScriptExecution";
        internal const string ControlPowerShellScriptExecutionEnvVar = "NUGET_CONTROL_POWERSHELL_SCRIPT_EXECUTION";

        internal static bool IsControlPowerShellScriptExecutionEnabled(IEnvironmentVariableReader? env = null)
        {
            string? value = (env ?? EnvironmentVariableWrapper.Instance).GetEnvironmentVariable(ControlPowerShellScriptExecutionEnvVar);
            if (value == "0" || value == "1")
            {
                return value == "1";
            }

            return AppContext.TryGetSwitch(ControlPowerShellScriptExecutionSwitchName, out bool enabled) && enabled;
        }

        /// <summary>Feature switch for System.Text.Json deserialization. Defaults to <see langword="false"/> (Newtonsoft is the default).</summary>
        /// <remarks>Cached per assembly to keep the serializer choice stable even if VS publishes the switch later.</remarks>
        [FeatureSwitchDefinition(UseSystemTextJsonDeserializationSwitchName)]
        internal static bool UseSystemTextJsonDeserializationFeatureSwitch { get; } =
            AppContext.TryGetSwitch(UseSystemTextJsonDeserializationSwitchName, out bool value) && value;

        /// <summary>Returns <see langword="true"/> when env var <c>NUGET_USE_SYSTEM_TEXT_JSON_DESERIALIZATION</c> is <c>true</c>.</summary>
        /// <param name="env">
        /// Pass <see langword="null"/> (or omit) in production code to use the cached <see cref="Lazy{T}"/> value,
        /// avoiding repeated allocations on .NET Framework. Pass an explicit <see cref="IEnvironmentVariableReader"/>
        /// only in tests to override the value.
        /// </param>
        internal static bool IsSystemTextJsonDeserializationEnabledByEnvironment(IEnvironmentVariableReader? env = null)
        {
            if (env is null)
            {
                return SystemTextJsonEnvironmentCache.Value.Value;
            }

            string? envValue = env.GetEnvironmentVariable(UseSystemTextJsonDeserializationEnvVar);
            return string.Equals(envValue, "true", StringComparison.OrdinalIgnoreCase);
        }

        private static class SystemTextJsonEnvironmentCache
        {
            internal static Lazy<bool> Value =
                new Lazy<bool>(() => IsSystemTextJsonDeserializationEnabledByEnvironment(EnvironmentVariableWrapper.Instance));

            static SystemTextJsonEnvironmentCache()
            {
                StaticState.BuildEnded += ResetCache;
            }

            private static void ResetCache() =>
                Value = new Lazy<bool>(() => IsSystemTextJsonDeserializationEnabledByEnvironment(EnvironmentVariableWrapper.Instance));
        }
    }
}
