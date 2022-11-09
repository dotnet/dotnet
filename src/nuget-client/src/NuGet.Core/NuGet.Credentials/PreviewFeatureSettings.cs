// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Credentials
{
    /// <summary>
    /// Settings for in-flight features not ready to be turned on permanently
    /// </summary>
    public static class PreviewFeatureSettings
    {
        public const string DefaultCredentialsAfterCredentialProvidersEnvironmentVariableName
            = "NUGET_CREDENTIAL_PROVIDER_OVERRIDE_DEFAULT";

        /// <summary>
        /// Use DefaultNetworkCredentialsCredentialProvider after plugin credential providers to handle using the user's
        /// ambient Windows credentials, instead of support baked into HttpSourceCredentials
        /// </summary>
        public static bool DefaultCredentialsAfterCredentialProviders { get; set; }
            = GetFlagFromEnvironmentVariable(DefaultCredentialsAfterCredentialProvidersEnvironmentVariableName);

        private static bool GetFlagFromEnvironmentVariable(string variableName)
        {
            bool flag;
            var flagString = Environment.GetEnvironmentVariable(variableName);
            return bool.TryParse(flagString, out flag) && flag;
        }
    }
}
