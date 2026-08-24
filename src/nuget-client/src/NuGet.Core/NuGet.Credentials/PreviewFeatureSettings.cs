// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.Credentials
{
    /// <summary>
    /// Settings for in-flight features not ready to be turned on permanently
    /// </summary>
    public static class PreviewFeatureSettings
    {
        static PreviewFeatureSettings()
        {
            StaticState.BuildEnded += ResetCache;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string DefaultCredentialsAfterCredentialProvidersEnvironmentVariableName
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            = "NUGET_CREDENTIAL_PROVIDER_OVERRIDE_DEFAULT";

        internal static IEnvironmentVariableReader environmentVariableReader { get; set; } = EnvironmentVariableWrapper.Instance;

        private static bool? s_defaultCredentialsAfterCredentialProviders;

        /// <summary>
        /// Use DefaultNetworkCredentialsCredentialProvider after plugin credential providers to handle using the user's
        /// ambient Windows credentials, instead of support baked into HttpSourceCredentials
        /// </summary>
        public static bool DefaultCredentialsAfterCredentialProviders
        {
            get
            {
                // Computed on first use rather than in the reset, so a process reused across builds reads the
                // environment of the build that uses it.
                s_defaultCredentialsAfterCredentialProviders ??= GetFlagFromEnvironmentVariable(DefaultCredentialsAfterCredentialProvidersEnvironmentVariableName);
                return s_defaultCredentialsAfterCredentialProviders.Value;
            }
            set => s_defaultCredentialsAfterCredentialProviders = value;
        }

        /// <summary>Discards the cached flag so it is re-read from the environment on next use.</summary>
        internal static void ResetCache() => s_defaultCredentialsAfterCredentialProviders = null;

        private static bool GetFlagFromEnvironmentVariable(string variableName)
        {
            bool flag;
            var flagString = environmentVariableReader.GetEnvironmentVariable(variableName);
            return bool.TryParse(flagString, out flag) && flag;
        }
    }
}
