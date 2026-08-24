// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Credentials
{
    /// <summary>
    /// Contains constants used for credential providers.
    /// </summary>
    public static class CredentialsConstants
    {
        /// <summary>
        /// Default timeout in seconds for the credential provider.
        /// </summary>
        public static readonly int ProviderTimeoutSecondsDefault = 300;

        /// <summary>
        /// Environment variable for the credential provider timeout in seconds.
        /// </summary>
        public static readonly string ProviderTimeoutSecondsEnvar = "NUGET_CREDENTIAL_PROVIDER_TIMEOUT_SECONDS";

        /// <summary>
        /// Setting name for the credential provider timeout.
        /// </summary>
        public static readonly string ProviderTimeoutSecondsSetting = "CredentialProvider.Timeout";
    }
}
