// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Microsoft.IdentityModel.JsonWebTokens
// Range: 14000 - 14999

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
        // signature creation / validation
        internal const string IDX14000 = "IDX14000: Signature validation of this JWT is not supported for: Algorithm: '{0}', SecurityKey: '{1}'.";

        // JWT messages
        internal const string IDX14100 = "IDX14100: JWT is not well formed, there are no dots (.).\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EndcodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.";
        internal const string IDX14101 = "IDX14101: Unable to decode the payload '{0}' as Base64Url encoded string.";
        internal const string IDX14102 = "IDX14102: Unable to decode the header '{0}' as Base64Url encoded string.";
        internal const string IDX14103 = "IDX14103: Failed to create the token encryption provider.";
        //internal const string IDX14105 = "IDX14105:";
        // internal const string IDX14106 = "IDX14106:";
        internal const string IDX14107 = "IDX14107: Token string does not match the token formats: JWE (header.encryptedKey.iv.ciphertext.tag) or JWS (header.payload.signature)";
        //internal const string IDX14111 = "IDX14111: JWT: '{0}' must have three segments (JWS) or five segments (JWE).";
        internal const string IDX14112 = "IDX14112: Only a single 'Actor' is supported. Found second claim of type: '{0}'";
        internal const string IDX14113 = "IDX14113: A duplicate value for 'SecurityTokenDescriptor.{0}' exists in 'SecurityTokenDescriptor.Claims'. \nThe value of 'SecurityTokenDescriptor.{0}' is used.";
        internal const string IDX14114 = "IDX14114: Both '{0}.{1}' and '{0}.{2}' are null or empty.";
        // internal const string IDX14115 = "IDX14115:";
        internal const string IDX14116 = "IDX14116: '{0}' cannot contain the following claims: '{1}'. These values are added by default (if necessary) during security token creation.";
        // number of sections 'dots' is not correct
        internal const string IDX14120 = "IDX14120: JWT is not well formed, there is only one dot (.).\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EndcodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.";
        internal const string IDX14121 = "IDX14121: JWT is not a well formed JWE, there must be four dots (.).\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EndcodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.";
        internal const string IDX14122 = "IDX14122: JWT is not a well formed JWE, there are more than four dots (.) a JWE can have at most 4 dots.\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EndcodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.";

        // logging
        internal const string IDX14200 = "IDX14200: Creating raw signature using the signature credentials.";
        internal const string IDX14201 = "IDX14201: Creating raw signature using the signature credentials. Caching SignatureProvider: '{0}'.";

        // parsing
        //internal const string IDX14300 = "IDX14300: Could not parse '{0}' : '{1}' as a '{2}'.";
        //internal const string IDX14301 = "IDX14301: Unable to parse the header into a JSON object. \nHeader: '{0}'.";
        //internal const string IDX14302 = "IDX14302: Unable to parse the payload into a JSON object. \nPayload: '{0}'.";
        //internal const string IDX14303 = "IDX14303: Claim with name '{0}' does not exist in the header.";
        internal const string IDX14304 = "IDX14304: Claim with name '{0}' does not exist in the JsonClaimSet.";
        internal const string IDX14305 = "IDX14305: Unable to convert the '{0}' json property to the following type: '{1}'. Property type was: '{2}'. Value: '{3}'.";
        internal const string IDX14306 = "IDX14306: JWE Ciphertext cannot be an empty string.";
        internal const string IDX14307 = "IDX14307: JWE header is missing.";
        internal const string IDX14308 = "IDX14308: JWE initialization vector is missing.";
        internal const string IDX14309 = "IDX14309: Unable to decode the initialization vector as Base64Url encoded string.";
        internal const string IDX14310 = "IDX14310: JWE authentication tag is missing.";
        internal const string IDX14311 = "IDX14311: Unable to decode the authentication tag as a Base64Url encoded string.";
        internal const string IDX14312 = "IDX14312: Unable to decode the cipher text as a Base64Url encoded string.";
    }
}
