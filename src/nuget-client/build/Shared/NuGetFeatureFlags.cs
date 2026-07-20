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

        static NuGetFeatureFlags()
        {
            StaticState.StartMSBuildRestoreTasks += ResetCache;
        }

        private static Lazy<bool> _isSystemTextJsonDeserializationEnabledByEnvironment =
            new Lazy<bool>(() => IsSystemTextJsonDeserializationEnabledByEnvironment(EnvironmentVariableWrapper.Instance));

        /// <summary>Re-reads <c>NUGET_USE_SYSTEM_TEXT_JSON_DESERIALIZATION</c> from the current environment.</summary>
        internal static void ResetCache() =>
            _isSystemTextJsonDeserializationEnabledByEnvironment =
                new Lazy<bool>(() => IsSystemTextJsonDeserializationEnabledByEnvironment(EnvironmentVariableWrapper.Instance));

        /// <summary>Feature switch for System.Text.Json deserialization. Defaults to <see langword="false"/> (Newtonsoft is the default).</summary>
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
                return _isSystemTextJsonDeserializationEnabledByEnvironment.Value;
            }

            string? envValue = env.GetEnvironmentVariable(UseSystemTextJsonDeserializationEnvVar);
            return string.Equals(envValue, "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
