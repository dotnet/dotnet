// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Constants for WsAddressing.
    /// </summary>
    public static class WsAddressing
    {
#pragma warning disable 1591

        public const string Namespace = "http://www.w3.org/2005/08/addressing";
        public const string PreferredPrefix = "wsa";

        /// <summary>
        /// Elements that can be in a WsAddressing ns
        /// </summary>
        public static class Elements
        {
            public const string Address = "Address";
            public const string EndpointReference = "EndpointReference";
        }

        #pragma warning restore 1591
    }
}
 
