//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

// Microsoft.IdentityModel.JsonWebTokens
// Range: 14000 - 14999

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
        #pragma warning disable 1591

        // signature creation / validation
        internal const string IDX14000 = "IDX14000: Signing JWT is not supported for: Algorithm: '{0}', SecurityKey: '{1}'.";

        // JWT messages
        internal const string IDX14100 = "IDX14100: JWT is not well formed: '{0}'.\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EndcodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.";
        internal const string IDX14101 = "IDX14101: Unable to decode the payload '{0}' as Base64Url encoded string. jwtEncodedString: '{1}'.";
        internal const string IDX14102 = "IDX14102: Unable to decode the header '{0}' as Base64Url encoded string. jwtEncodedString: '{1}'.";
        internal const string IDX14103 = "IDX14103: Failed to create the token encryption provider.";
        internal const string IDX14105 = "IDX14105: Header.Cty != null, assuming JWS. Cty: '{0}'.";
        // internal const string IDX14106 = "IDX14106:";
        internal const string IDX14107 = "IDX14107: Token string does not match the token formats: JWE (header.encryptedKey.iv.ciphertext.tag) or JWS (header.payload.signature)";
        internal const string IDX14111 = "IDX14111: JWT: '{0}' must have three segments (JWS) or five segments (JWE).";
        internal const string IDX14112 = "IDX14112: Only a single 'Actor' is supported. Found second claim of type: '{0}', value: '{1}'";
        internal const string IDX14113 = "IDX14113: A duplicate value for 'SecurityTokenDescriptor.{0}' exists in 'SecurityTokenDescriptor.Claims'. \nThe value of 'SecurityTokenDescriptor.{0}' is used.";
        internal const string IDX14114 = "IDX14114: Both '{0}.{1}' and '{0}.{2}' are null or empty.";
        // internal const string IDX14115 = "IDX14115:";
        internal const string IDX14116 = "IDX14116: ''{0}' cannot contain the following claims: '{1}'. These values are added by default (if necessary) during security token creation.";

        // logging
        internal const string IDX14200 = "IDX14200: Creating raw signature using the signature credentials.";
        internal const string IDX14201 = "IDX14201: Creating raw signature using the signature credentials. Caching SignatureProvider: '{0}'.";

        // parsing
        internal const string IDX14300 = "IDX14300: Could not parse '{0}' : '{1}' as a '{2}'.";
        internal const string IDX14301 = "IDX14301: Unable to parse the header into a JSON object. \nHeader: '{0}'.";
        internal const string IDX14302 = "IDX14302: Unable to parse the payload into a JSON object. \nPayload: '{0}'.";
        internal const string IDX14303 = "IDX14303: Claim with name '{0}' does not exist in the header.";
        internal const string IDX14304 = "IDX14304: Claim with name '{0}' does not exist in the payload.";
        internal const string IDX14305 = "IDX14305: Unable to convert the '{0}' claim to the following type: '{1}'. Claim type was: '{2}'.";
        internal const string IDX14306 = "IDX14306: JWE Ciphertext cannot be an empty string.";
        internal const string IDX14307 = "IDX14307: An exception has been caught while parsing the payload. Exception: {0}, Payload: {1}";
#pragma warning restore 1591
    }
}
