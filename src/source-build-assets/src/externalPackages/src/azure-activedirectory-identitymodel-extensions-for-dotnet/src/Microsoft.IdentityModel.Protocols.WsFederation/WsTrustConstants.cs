// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Constants for WsTrust.
    /// Attributes and Elements are almost the same across all versions 2005, 1.3, 1.4
    /// </summary>
    public static class WsTrustConstants
    {
        #pragma warning disable 1591

        /// <summary>
        /// Elements that can be in a WsTrust message
        /// </summary>
        public static class Elements
        {
            public const string KeyType = "KeyType";
            public const string Lifetime = "Lifetime";
            public const string RequestedAttachedReference = "RequestedAttachedReference";
            public const string RequestedSecurityToken = "RequestedSecurityToken";
            public const string RequestSecurityTokenResponse = "RequestSecurityTokenResponse";
            public const string RequestSecurityTokenResponseCollection = "RequestSecurityTokenResponseCollection";
            public const string RequestType = "RequestType";
            public const string SecurityTokenReference = "SecurityTokenReference";
            public const string RequestedUnattachedReference = "RequestedUnattachedReference";
            public const string TokenType = "TokenType";
        }

        /// <summary>
        /// Namespaces that can be in a WsTrust message
        /// </summary>
        public static class Namespaces
        {
            public const string WsTrust2005 = "http://schemas.xmlsoap.org/ws/2005/02/trust";
            public const string WsTrust1_3 = "http://docs.oasis-open.org/ws-sx/ws-trust/200512";
            public const string WsTrust1_4 = "http://docs.oasis-open.org/ws-sx/ws-trust/200802";
        }

        #pragma warning restore 1591
    }
}
 
