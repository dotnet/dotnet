// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Microsoft.IdentityModel.Protocols.OpenIdConnect
// Range: 21000 - 21999

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // properties, configuration 
        internal const string IDX21105 = "IDX21105: NonceLifetime must be greater than zero. value: '{0}'";
        internal const string IDX21106 = "IDX21106: Error in deserializing to json: '{0}'";

        // protocol validation
        internal const string IDX21300 = "IDX21300: The hash claim: '{0}' in the id_token did not validate with against: '{1}', algorithm: '{2}'.";
        internal const string IDX21301 = "IDX21301: The algorithm: '{0}' specified in the jwt header could not be used to create a '{1}'. See inner exception for details.";
        internal const string IDX21302 = "IDX21302: The algorithm: '{0}' specified in the jwt header is not supported.";
        internal const string IDX21303 = "IDX21303: Validating hash of OIDC protocol message. Expected: '{0}'.";
        internal const string IDX21304 = "IDX21304: Validating 'c_hash' using id_token and code.";
        internal const string IDX21305 = "IDX21305: OpenIdConnectProtocolValidationContext.ProtocolMessage.Code is null, there is no 'code' in the OpenIdConnect Response to validate.";
        internal const string IDX21306 = "IDX21306: The 'c_hash' claim was not a string in the 'id_token', but a 'code' was in the OpenIdConnectMessage, 'id_token': '{0}'.";
        internal const string IDX21307 = "IDX21307: The 'c_hash' claim was not found in the id_token, but a 'code' was in the OpenIdConnectMessage, id_token: '{0}'";
        internal const string IDX21308 = "IDX21308: 'azp' claim exists in the 'id_token' but 'client_id' is null. Cannot validate the 'azp' claim.";
        internal const string IDX21309 = "IDX21309: Validating 'at_hash' using id_token and access_token.";
        internal const string IDX21310 = "IDX21310: OpenIdConnectProtocolValidationContext.ProtocolMessage.AccessToken is null, there is no 'token' in the OpenIdConnect Response to validate.";
        internal const string IDX21311 = "IDX21311: The 'at_hash' claim was not a string in the 'id_token', but an 'access_token' was in the OpenIdConnectMessage, 'id_token': '{0}'.";
        internal const string IDX21312 = "IDX21312: The 'at_hash' claim was not found in the 'id_token', but a 'access_token' was in the OpenIdConnectMessage, 'id_token': '{0}'.";
        internal const string IDX21313 = "IDX21313: The id_token: '{0}' is not valid. Delegate threw exception, see inner exception for more details.";
        internal const string IDX21314 = "IDX21314: OpenIdConnectProtocol requires the jwt token to have an '{0}' claim. The jwt did not contain an '{0}' claim, jwt: '{1}'.";
        internal const string IDX21315 = "IDX21315: RequireAcr is 'true' (default is 'false') but jwt.PayLoad.Acr is 'null or whitespace', jwt: '{0}'.";
        internal const string IDX21316 = "IDX21316: RequireAmr is 'true' (default is 'false') but jwt.PayLoad.Amr is 'null or whitespace', jwt: '{0}'.";
        internal const string IDX21317 = "IDX21317: RequireAuthTime is 'true' (default is 'false') but jwt.PayLoad.AuthTime is 'null or whitespace', jwt: '{0}'.";
        internal const string IDX21318 = "IDX21318: RequireAzp is 'true' (default is 'false') but jwt.PayLoad.Azp is 'null or whitespace', jwt: '{0}'.";
        internal const string IDX21319 = "IDX21319: Validating the nonce claim found in the id_token.";
        internal const string IDX21320 = "IDX21320: RequireNonce is '{0}'. OpenIdConnectProtocolValidationContext.Nonce and OpenIdConnectProtocol.ValidatedIdToken.Nonce are both null or empty. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'.";
        internal const string IDX21321 = "IDX21321: The 'nonce' found in the jwt token did not match the expected nonce.\nexpected: '{0}'\nfound in jwt: '{1}'.\njwt: '{2}'.";
        internal const string IDX21322 = "IDX21322: RequireNonce is false, validationContext.Nonce is null and there is no 'nonce' in the OpenIdConnect Response to validate.";
        internal const string IDX21323 = "IDX21323: RequireNonce is '{0}'. OpenIdConnectProtocolValidationContext.Nonce was null, OpenIdConnectProtocol.ValidatedIdToken.Payload.Nonce was not null. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'. Note if a 'nonce' is found it will be evaluated.";
        internal const string IDX21324 = "IDX21324: The 'nonce' has expired: '{0}'. Time from 'nonce' (UTC): '{1}', Current Time (UTC): '{2}'. NonceLifetime is: '{3}'.";
        internal const string IDX21325 = "IDX21325: The 'nonce' did not contain a timestamp: '{0}'.\nFormat expected is: <epochtime>.<noncedata>.";
        internal const string IDX21326 = "IDX21326: The 'nonce' timestamp could not be converted to a positive integer (greater than 0).\ntimestamp: '{0}'\nnonce: '{1}'.";
        internal const string IDX21327 = "IDX21327: The 'nonce' timestamp: '{0}', could not be converted to a DateTime using DateTime.FromBinary({0}).\nThe value must be between: '{1}' and '{2}'.";
        internal const string IDX21328 = "IDX21328: Generating nonce for openIdConnect message.";
        internal const string IDX21329 = "IDX21329: RequireState is '{0}' but the OpenIdConnectProtocolValidationContext.State is null. State cannot be validated.";
        internal const string IDX21330 = "IDX21330: RequireState is '{0}', the OpenIdConnect Request contained 'state', but the Response does not contain 'state'.";
        internal const string IDX21331 = "IDX21331: The 'state' parameter in the message: '{0}', does not equal the 'state' in the context: '{1}'.";
        internal const string IDX21332 = "IDX21332: OpenIdConnectProtocolValidationContext.ValidatedIdToken is null. There is no 'id_token' to validate against.";
        internal const string IDX21333 = "IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate.";
        internal const string IDX21334 = "IDX21334: Both 'id_token' and 'code' are null in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Authorization Endpoint. Cannot process the message.";
        internal const string IDX21335 = "IDX21335: 'refresh_token' cannot be present in a response message received from Authorization Endpoint.";
        internal const string IDX21336 = "IDX21336: Both 'id_token' and 'access_token' should be present in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Token Endpoint. Cannot process the message.";
        internal const string IDX21337 = "IDX21337: OpenIdConnectProtocolValidationContext.UserInfoEndpointResponse is null or empty, there is no OpenIdConnect Response to validate.";
        internal const string IDX21338 = "IDX21338: Subject claim present in 'id_token': '{0}' does not match the claim received from UserInfo Endpoint: '{1}'.";
        internal const string IDX21339 = "IDX21339: The 'id_token' contains multiple audiences but 'azp' claim is missing.";
        internal const string IDX21340 = "IDX21340: The 'id_token' contains 'azp' claim but its value is not equal to Client Id. 'azp': '{0}'. clientId: '{1}'.";
        internal const string IDX21341 = "IDX21341: 'RequireState' = false, OpenIdConnectProtocolValidationContext.State is null and there is no 'state' in the OpenIdConnect response to validate.";
        internal const string IDX21342 = "IDX21342: 'RequireStateValidation' = false, not validating the state.";
        internal const string IDX21343 = "IDX21343: Unable to parse response from UserInfo endpoint: '{0}'";
        internal const string IDX21345 = "IDX21345: OpenIdConnectProtocolValidationContext.UserInfoEndpointResponse does not contain a 'sub' claim, cannot validate.";
        internal const string IDX21346 = "IDX21346: OpenIdConnectProtocolValidationContext.ValidatedIdToken does not contain a 'sub' claim, cannot validate.";
        internal const string IDX21347 = "IDX21347: Validating the 'c_hash' failed, see inner exception.";
        internal const string IDX21348 = "IDX21348: Validating the 'at_hash' failed, see inner exception.";
        internal const string IDX21349 = "IDX21349: RequireNonce is '{0}'. OpenIdConnectProtocolValidationContext.Nonce was not null, OpenIdConnectProtocol.ValidatedIdToken.Payload.Nonce was null or empty. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'. Note if a 'nonce' is found it will be evaluated.";
        internal const string IDX21350 = "IDX21350: The algorithm specified in the jwt header is null or empty.";

        // configuration retrieval errors
        internal const string IDX21806 = "IDX21806: Deserializing json string into json web keys.";
        internal const string IDX21808 = "IDX21808: Deserializing json into OpenIdConnectConfiguration object: '{0}'.";
        internal const string IDX21809 = "IDX21809: Serializing OpenIdConfiguration object to json string.";
        internal const string IDX21811 = "IDX21811: Deserializing the string: '{0}' obtained from metadata endpoint into openIdConnectConfiguration object.";
        internal const string IDX21812 = "IDX21812: Retrieving json web keys from: '{0}'.";
        internal const string IDX21813 = "IDX21813: Deserializing json web keys: '{0}'.";
        internal const string IDX21815 = "IDX21815: Error deserializing json: '{0}' into '{1}'.";
        internal const string IDX21816 = "IDX21816: The number of signing keys must be greater or equal to '{0}'. Value: '{1}'.";
        internal const string IDX21817 = "IDX21817: The OpenIdConnectConfiguration did not contain any JsonWebKeys. This is required to validate the configuration.";
        internal const string IDX21818 = "IDX21818: The OpenIdConnectConfiguration's valid signing keys cannot be less than {0}. Values: {1}. Invalid keys: {2}";
#pragma warning restore 1591
    }
}
