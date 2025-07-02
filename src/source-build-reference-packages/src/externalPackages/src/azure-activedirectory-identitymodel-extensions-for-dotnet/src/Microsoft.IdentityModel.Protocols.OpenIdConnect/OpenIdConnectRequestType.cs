// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Defines request types for OpenID Connect.
    /// </summary>
    /// <remarks>
    /// Can be used to determine the message type in an <see cref="OpenIdConnectMessage"/>.
    /// </remarks>
    public enum OpenIdConnectRequestType
    {
        /// <summary>
        /// Indicates an Authentication Request. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest"/>.
        /// </summary>
        Authentication,

        /// <summary>
        /// Indicates a Logout Request. See: <see href="https://openid.net/specs/openid-connect-frontchannel-1_0.html#RPLogout"/>.
        /// </summary>
        Logout,

        /// <summary>
        /// Indicates a Token Request. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#TokenRequest"/>.
        /// </summary>
        Token,
    }
}
