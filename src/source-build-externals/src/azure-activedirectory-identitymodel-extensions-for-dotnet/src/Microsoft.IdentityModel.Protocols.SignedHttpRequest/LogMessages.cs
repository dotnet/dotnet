// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

//  Microsoft.IdentityModel.Protocols.SignedHttpRequest
// SignedHttpRequest range: 23000 - 23999.

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
        public const string IDX23001 = "IDX23001: HttpRequestUri must be absolute when creating or validating the 'u' claim. HttpRequestUri: '{0}'.";
        public const string IDX23002 = "IDX23002: The HTTP Method must be an uppercase HTTP verb. HttpMethod: '{0}'.";
        public const string IDX23003 = "IDX23003: The signed http request does not contain the '{0}' claim or the claim value is null. This claim is required to validate a signed http request.";
        public const string IDX23004 = "IDX23004: The following query parameters will not be processed as they are repeated: '{0}'.";
        public const string IDX23005 = "IDX23005: The following headers will not be processed as they are repeated: '{0}'.";
        public const string IDX23006 = "IDX23006: The address specified '{0}' is not valid as per the HTTPS scheme. Please specify an HTTPS address for security reasons. For testing with an HTTP address, set the RequireHttpsForJkuResourceRetrieval property on SignedHttpRequestValidationParameters to false.";
        public const string IDX23007 = "IDX23007: HttpRequestUri is an invalid relative URI: '{0}'.";
        public const string IDX23008 = "IDX23008: Exception caught while creating the '{0}' claim. Inner exception: '{1}'.";
        public const string IDX23009 = "IDX23009: Signed http request signature validation failed. Exceptions caught: '{0}'.";
        public const string IDX23010 = "IDX23010: Lifetime validation of the signed http request failed. Current time: '{0}' UTC, signed http request is valid until: '{1}' UTC.";
        public const string IDX23011 = "IDX23011: '{0}' claim validation failed. Expected value: '{1}', value found: '{2}'.";
        public const string IDX23012 = "IDX23012: '{0}' claim validation failed. Expected values: '{1}' or '{2}', value found: '{3}'.";
        public const string IDX23013 = "IDX23013: 'at' token validation failed. Inner exception: '{0}'.";
        public const string IDX23014 = "IDX23014: Unable to resolve a PoP key. The 'cnf' object must have one of the following claims: 'jwk', 'jwe', 'jku', 'kid'. The 'cnf' claim value: '{0}'.";
        public const string IDX23015 = "IDX23015: A security key resolved from the 'jwk' claim is not an asymmetric key. Resolved key type: '{0}'.";
        public const string IDX23016 = "IDX23016: Unable to convert the key found in the 'jwk' claim to a security key. JsonWebKey: '{0}'.";
        public const string IDX23017 = "IDX23017: No decryption keys found. Unable to decrypt a key found in the 'jwe' claim without decryption keys.";
        public const string IDX23018 = "IDX23018: Unable to decrypt a 'jwe' claim. Decryption keys used: '{0}'. Inner exception: '{1}'.";
        public const string IDX23019 = "IDX23019: A security key resolved from the 'jwe' claim is not a symmetric key. Resolved key type: '{0}'.";
        public const string IDX23021 = "IDX23021: Unable to resolve a PoP key from the 'jku' claim. Unable to match kid '{0}' against '{1}'.";
        public const string IDX23022 = "IDX23022: Exception caught while retrieving a jwk set from: '{0}'. Inner exception: '{1}'.";
        public const string IDX23023 = "IDX23023: Unable to resolve a PoP key using only the 'kid' claim. To utilize 'cnf' claim reference, a 'cnf' claim must be included as a root element of SignedHttpRequest. To manually resolve a PoP key using the 'kid', set the 'PopKeyResolverFromKeyIdentifierAsync' delegate on 'SignedHttpRequestValidationParameters'. For more details, see https://aka.ms/IdentityModel/SignedHttpRequest.";
        public const string IDX23024 = "IDX23024: Unable to parse the '{0}' claim: '{1}'. Inner exception: '{2}'.";
        public const string IDX23025 = "IDX23025: Exception caught while validating the '{0}' claim. Inner exception: '{1}'.";
        public const string IDX23026 = "IDX23026: The request contains unsigned headers and SignedHttpRequestValidationParameters.AcceptUnsignedHeaders is set to 'false'. Unsigned headers: '{0}'.";
        public const string IDX23027 = "IDX23027: Header: '{0}' was not found in the request headers: '{1}'. Unable to validate the 'h' claim.";
        public const string IDX23028 = "IDX23028: Query parameter: '{0}' was not found in the request query parameters: '{1}'. Unable to validate the 'q' claim.";
        public const string IDX23029 = "IDX23029: The request contains unsigned query parameters and SignedHttpRequestValidationParameters.AcceptUnsignedQueryParameters is set to 'false'. Unsigned query parameters: '{0}'.";
        public const string IDX23030 = "IDX23030: Unable to cast a '{0}' into a '{1}'. '{0}': '{2}'.";
        public const string IDX23031 = "IDX23031: Unable to resolve a PoP key from the 'jku' claim. GetPopKeysFromJkuAsync method returned no keys.";
        public const string IDX23032 = "IDX23032: SigningCredentials object has a key that is not a JsonWebKey or an AsymmetricKey. Unable to create a 'cnf' claim from '{0}'. Use 'SignedHttpRequestDescriptor.CnfClaimValue' to manually set a 'cnf' claim value, or set 'SignedHttpRequestCreationParameters.CreateCnf' flag to false.";
        public const string IDX23033 = "IDX23033: Unable to validate the 'cnf' claim reference. Thumbprint of the JWK used to sign the SignedHttpRequest (root 'cnf' claim) does not match the expected thumbprint ('at' -> 'cnf' -> 'kid'). Expected value: '{0}', actual value: '{1}'. Root 'cnf' claim value: '{2}'. For more details, see https://aka.ms/IdentityModel/SignedHttpRequest.";
        public const string IDX23034 = "IDX23034: Signed http request signature validation failed. SignedHttpRequest: '{0}'";
        public const string IDX23035 = "IDX23035: Unable to resolve a PoP key from the 'jku' claim. Multiple keys are found in the referenced JWK Set document and the 'cnf' claim doesn't contain a 'kid' value.";
        public const string IDX23036 = "IDX23036: Signed http request nonce validation failed. Exceptions caught: '{0}'.";
        public const string IDX23037 = "IDX23037: Resolving a PoP key from the 'jku' claim is not allowed. To allow it, set AllowResolvingPopKeyFromJku property on SignedHttpRequestValidationParameters to true and provide a list of trusted domains via AllowedDomainsForJkuRetrieval.";
        public const string IDX23038 = "IDX23038: Resolving a PoP key from the 'jku' claim is not allowed as '{0}' is not present in the list of allowed domains for 'jku' retrieval: '{1}'. If '{0}' belongs to a trusted domain, add it to AllowedDomainsForJkuRetrieval property on SignedHttpRequestValidationParameters.";
    }
}
