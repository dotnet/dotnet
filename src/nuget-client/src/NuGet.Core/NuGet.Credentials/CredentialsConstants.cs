// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Credentials
{
    public static class CredentialsConstants
    {
        public static readonly int ProviderTimeoutSecondsDefault = 300;

        public static readonly string ProviderTimeoutSecondsEnvar = "NUGET_CREDENTIAL_PROVIDER_TIMEOUT_SECONDS";

        public static readonly string ProviderTimeoutSecondsSetting = "CredentialProvider.Timeout";
    }
}
