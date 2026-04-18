// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Constants for WsFederation.
    /// As defined in the http://docs.oasis-open.org/wsfed/federation/v1.2/os/ws-federation-1.2-spec-os.html
    /// </summary>
    public static class WsFederationConstants
    {
        #pragma warning disable 1591

        public const string MetadataNamespace = "urn:oasis:names:tc:SAML:2.0:metadata";
        public const string Namespace =  "http://docs.oasis-open.org/wsfed/federation/200706";
        public const string PreferredPrefix = "fed";

        /// <summary>
        /// Constants for WsFederation actions.
        /// </summary>
        public static class WsFederationActions
        {
            public const string Attribute = "wattr1.0";
            public const string Pseudonym = "wpseudo1.0";
            public const string SignIn = "wsignin1.0";
            public const string SignOut = "wsignout1.0";
            public const string SignOutCleanup = "wsignoutcleanup1.0";
        }

        /// <summary>
        /// Constants for WsFederation Fault codes.
        /// </summary>
        public static class WsFederationFaultCodes
        {
            public const string AlreadySignedIn = "AlreadySignedIn";
            public const string BadRequest = "BadRequest";
            public const string IssuerNameNotSupported = "IssuerNameNotSupported";
            public const string NeedFresherCredentials = "NeedFresherCredentials";
            public const string NoMatchInScope = "NoMatchInScope";
            public const string NoPseudonymInScope = "NoPseudonymInScope";
            public const string NotSignedIn = "NotSignedIn";
            public const string RstParameterNotAccepted = "RstParameterNotAccepted";
            public const string SpecificPolicy = "SpecificPolicy";
            public const string UnsupportedClaimsDialect = "UnsupportedClaimsDialect";
            public const string UnsupportedEncoding = "UnsupportedEncoding";
        }

        /// <summary>
        /// Defines the WsFederation Constants
        /// </summary>
        public static class WsFederationParameterNames
        {
            public const string Wa = "wa";
            public const string Wattr = "wattr";
            public const string Wattrptr = "wattrptr";
            public const string Wauth = "wauth";
            public const string Wct = "wct";
            public const string Wctx = "wctx";
            public const string Wencoding = "wencoding";
            public const string Wfed = "wfed";
            public const string Wfresh = "wfresh";
            public const string Whr = "whr";
            public const string Wp = "wp";
            public const string Wpseudo = "wpseudo";
            public const string Wpseudoptr = "wpseudoptr";
            public const string Wreply = "wreply";
            public const string Wreq = "wreq";
            public const string Wreqptr = "wreqptr";
            public const string Wres = "wres";
            public const string Wresult = "wresult";
            public const string Wresultptr = "wresultptr";
            public const string Wtrealm = "wtrealm";
        }

        /// <summary>
        /// Attributes for WsFederation metadata xml.
        /// </summary>
        public static class Attributes
        {
            public const string EntityId = "entityID";
            public const string Id = "ID";
            public const string ProtocolSupportEnumeration = "protocolSupportEnumeration";
            public const string Type = "type";
            public const string Use = "use";
        }

        /// <summary>
        /// Elements for WsFederation metadata xml.
        /// </summary>
        public static class Elements
        {
            public const string EntityDescriptor = "EntityDescriptor";
            public const string IdpssoDescriptor = "IDPSSODescriptor";
            public const string KeyDescriptor = "KeyDescriptor";
            public const string RoleDescriptor = "RoleDescriptor";
            public const string PassiveRequestorEndpoint = "PassiveRequestorEndpoint";
            public const string SecurityTokenServiceEndpoint = "SecurityTokenServiceEndpoint";
            public const string SpssoDescriptor = "SPSSODescriptor";
        }

        /// <summary>
        /// Namespaces for WsFederation metadata xml.
        /// </summary>
        public static class Namespaces
        {

        }

        /// <summary>
        /// Types for WsFederation metadata xml.
        /// </summary>
        public static class Types
        {
            public const string ApplicationServiceType = "ApplicationServiceType";
            public const string SecurityTokenServiceType = "SecurityTokenServiceType";
        }

        /// <summary>
        /// Defines the key use value in key descriptor for WsFederation metadata xml.
        /// </summary>
        public static class KeyUse
        {
            public const string Signing = "signing";
        }

        /// <summary>
        /// xmlns string.
        /// </summary>
        internal static string Xmlns = "xmlns";

        /// <summary>
        /// Prefix names.
        /// </summary>
        internal static class Prefixes
        {
            public const string Fed = "fed";
            public const string Xsi = "xsi";
            public const string Wsa = "wsa";
            public const string Md = "md";
        }

        #pragma warning restore 1591
    }
}
 
