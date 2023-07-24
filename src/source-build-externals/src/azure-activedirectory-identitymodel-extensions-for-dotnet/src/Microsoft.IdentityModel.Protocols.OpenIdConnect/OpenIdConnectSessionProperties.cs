// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Defines a set of properties names 
    /// </summary>
    public static class OpenIdConnectSessionProperties
    {
        /// <summary>
        /// Property defined for 'check_session_iframe'.
        /// </summary>
        public const string CheckSessionIFrame = ".checkSessionIFrame";
   
        /// <summary>
        /// Property defined for 'redirect_uri' set in the request for a 'code'
        /// </summary>
        public const string RedirectUri = ".redirect_uri";

        /// <summary>
        /// Property defined for 'session state'
        /// </summary>
        public const string SessionState = ".sessionState";

    }
}
