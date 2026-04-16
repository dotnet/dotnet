// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Microsoft.IdentityModel.Protocols.WsFederation
// Range: 22000 - 22999

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // general
        internal const string IDX22000 = "IDX22000: The parameter '{0}' cannot be a 'null' or an empty object.";

        // wsfederation messages
        internal const string IDX22900 = "IDX22900: Building wsfederation message from query string: '{0}'.";
        internal const string IDX22901 = "IDX22901: Building wsfederation message from uri: '{0}'.";
        internal const string IDX22902 = "IDX22902: Token is not found in Wresult";
        internal const string IDX22903 = "IDX22903: Multiple tokens were found in the RequestSecurityTokenCollection. Only a single token is supported.";
        internal const string IDX22904 = "IDX22904: Wresult does not contain a 'RequestedSecurityToken' element.";

        // xml metadata messages
        internal const string IDX22800 = "IDX22800: Exception thrown while reading WsFederationMetadata. Element '{0}'. Caught exception: '{1}'.";
        internal const string IDX22801 = "IDX22801: 'entityID' attribute is not found in EntityDescriptor element in metadata file.";
        internal const string IDX22802 = "IDX22802: Current name '{0} and namespace '{1}' do not match the expected name '{2}' and namespace '{3}'.";
        internal const string IDX22803 = "IDX22803: Token reference address is missing in 'PassiveRequestorEndpoint' in metadata file.";
        internal const string IDX22804 = "IDX22804: 'SecurityTokenServiceTypeRoleDescriptor' is expected.";
        internal const string IDX22806 = "IDX22806: Key descriptor for signing is missing in 'SecurityTokenServiceTypeRoleDescriptor'.";
        internal const string IDX22807 = "IDX22807: Token endpoint is missing in 'SecurityTokenServiceTypeRoleDescriptor'.";
        internal const string IDX22808 = "IDX22808: 'Use' attribute is missing in KeyDescriptor.";
        internal const string IDX22810 = "IDX22810: 'Issuer' value is missing in wsfederationconfiguration.";
        internal const string IDX22811 = "IDX22811: 'TokenEndpoint' value is missing in wsfederationconfiguration.";
        internal const string IDX22812 = "IDX22812: Element: '{0}' was an empty element. 'TokenEndpoint' value is missing in wsfederationconfiguration.";
        internal const string IDX22813 = "IDX22813: 'ActiveTokenEndpoint' is missing in 'SecurityTokenServiceTypeRoleDescriptor'.";
        internal const string IDX22814 = "IDX22814: Token reference address is missing in 'SecurityTokenServiceEndpoint' in metadata.";

        // WsFederationConfigurationValidator messages
        internal const string IDX22700 = "IDX22700: The Issuer property is null or empty.";
        internal const string IDX22701 = "IDX22701: The Signature property is null.";
        internal const string IDX22702 = "IDX22702: The Signature's KeyInfo property is null.";
        internal const string IDX22703 = "IDX22703: The Signature's SignatureValue property is null or empty.";
        internal const string IDX22704 = "IDX22704: The Signature.SignedInfo property is null or empty.";
        internal const string IDX22705 = "IDX22705: The Signature.SignedInfo.SignatureMethod property is null or empty.";
        internal const string IDX22706 = "IDX22706: The Signature.SignedInfo.References property is null or an empty collection.";
        internal const string IDX22707 = "IDX22707: The ActiveTokenEndpoint property is not defined.";
        internal const string IDX22708 = "IDX22708: The ActiveTokenEndpoint property is not a valid URI.";
        internal const string IDX22709 = "IDX22709: The TokenEndpoint property is not defined.";
        internal const string IDX22710 = "IDX22710: The TokenEndpoint property is not a valid URI.";
        internal const string IDX22711 = "IDX22711: The SigningKeys is null or an empty collection.";
        internal const string IDX22712 = "IDX22712: Could not identify the thumbprint of the key used to sign the metadata.";
        internal const string IDX22713 = "IDX22713: Metadata signature validation failed.";

#pragma warning restore 1591
    }
}
