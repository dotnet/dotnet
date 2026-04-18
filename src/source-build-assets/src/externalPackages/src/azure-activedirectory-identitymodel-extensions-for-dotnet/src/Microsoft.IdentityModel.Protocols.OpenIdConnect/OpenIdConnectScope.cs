// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Defines scopes for OpenID Connect. For details, See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
    /// </summary>
    /// <remarks>
    /// Can be used to determine the scope in an <see cref="OpenIdConnectMessage"/>.
    /// </remarks>
    public static class OpenIdConnectScope
    {
        /// <summary>
        /// Indicates the <c>address</c> scope. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
        /// </summary>
        public const string Address = "address";

        /// <summary>
        /// Indicates the <c>email</c> scope. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// Indicates the <c>offline_access</c> scope. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
        /// </summary>
        public const string OfflineAccess = "offline_access";

        /// <summary>
        /// Indicates the <c>openid</c> scope. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
        /// </summary>
        public const string OpenId = "openid";

        /// <summary>
        /// Indicates both <c>openid</c> and <c>profile</c> scopes. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
        /// </summary>
        public const string OpenIdProfile = "openid profile";

        /// <summary>
        /// Indicates the <c>phone</c> scope. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
        /// </summary>
        public const string Phone = "phone";

        /// <summary>
        /// Indicates the <c>profile</c> scope. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims"/>.
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// Indicates the <c>user_impersonation</c> scope for Microsoft Entra ID.
        /// </summary>
        public const string UserImpersonation = "user_impersonation";
    }
}
