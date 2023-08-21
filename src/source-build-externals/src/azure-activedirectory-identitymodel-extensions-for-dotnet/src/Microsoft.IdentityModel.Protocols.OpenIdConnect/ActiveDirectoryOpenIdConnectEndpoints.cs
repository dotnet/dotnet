// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Well known endpoints for AzureActiveDirectory
    /// </summary>
    public static class ActiveDirectoryOpenIdConnectEndpoints
    {
#pragma warning disable 1591
        public const string Authorize = "oauth2/authorize";
        public const string Logout = "oauth2/logout";
        public const string Token = "oauth2/token";
#pragma warning restore 1591
    }
}
