// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Specific scope values that are interesting to OpenID Connect.  See https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
    /// </summary>
    /// <remarks>Can be used to determine the scope by consumers of an <see cref="OpenIdConnectMessage"/>.
    /// For example: OpenIdConnectMessageTests.Publics() sets <see cref="OpenIdConnectMessage.Scope"/>
    /// to <see cref="OpenIdConnectScope.OpenIdProfile"/>.</remarks>
    public static class OpenIdConnectScope
    {
        /// <summary>
        /// Indicates <c>address</c> scope see: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims.
        /// </summary>
        public const string Address = "address";

        /// <summary>
        /// Indicates <c>email</c> scope see: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// Indicates <c>offline_access</c> scope see: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims.
        /// </summary>
        public const string OfflineAccess = "offline_access";

        /// <summary>
        /// Indicates <c>openid</c> scope see: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims.
        /// </summary>
        public const string OpenId = "openid";

        /// <summary>
        /// Indicates <c>openid</c> and <c>profile</c> scope see: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims.
        /// </summary>
        public const string OpenIdProfile = "openid profile";

        /// <summary>
        /// Indicates <c>phone</c> scope see: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims.
        /// </summary>
        public const string Phone = "phone";

        /// <summary>
        /// Indicates <c>profile</c> scope see: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims.
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// Indicates <c>user_impersonation</c> scope for Azure Active Directory.
        /// </summary>
        public const string UserImpersonation = "user_impersonation";
    }
}
