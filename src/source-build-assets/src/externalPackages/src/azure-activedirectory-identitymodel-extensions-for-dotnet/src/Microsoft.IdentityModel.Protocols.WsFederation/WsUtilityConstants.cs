// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Constants for WsUtility.
    /// </summary>
    public static class WsUtility
    {
        #pragma warning disable 1591

        public const string Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        public const string PreferredPrefix = "wsu";

        /// <summary>
        /// Elements that are in the WsUtility ns
        /// </summary>
        public static class Elements
        {
            public const string Created = "Created";
            public const string Expires = "Expires";
        }

        #pragma warning restore 1591
    }
}
 
