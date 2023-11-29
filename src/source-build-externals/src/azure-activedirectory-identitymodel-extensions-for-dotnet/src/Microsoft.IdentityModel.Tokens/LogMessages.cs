// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Microsoft.IdentityModel.Tokens
// Range: 10000 - 10999

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // general
        // public const string IDX10000 = "IDX10000:";

        // properties, configuration 
        public const string IDX10101 = "IDX10101: MaximumTokenSizeInBytes must be greater than zero. value: '{0}'";
        public const string IDX10100 = "IDX10100: ClockSkew must be greater than TimeSpan.Zero. value: '{0}'";
        public const string IDX10102 = "IDX10102: NameClaimType cannot be null or whitespace.";
        public const string IDX10103 = "IDX10103: RoleClaimType cannot be null or whitespace.";
        public const string IDX10104 = "IDX10104: TokenLifetimeInMinutes must be greater than zero. value: '{0}'";
        public const string IDX10105 = "IDX10105: ClaimValue that is a collection of collections is not supported. Such ClaimValue is found for ClaimType : '{0}'";
        //public const string IDX10106 = "IDX10106:";
        public const string IDX10107 = "IDX10107: When setting RefreshInterval, the value must be greater than MinimumRefreshInterval: '{0}'. value: '{1}'.";
        public const string IDX10108 = "IDX10108: When setting AutomaticRefreshInterval, the value must be greater than MinimumAutomaticRefreshInterval: '{0}'. value: '{1}'.";
        public const string IDX10109 = "IDX10109: Warning: Claims is being accessed without first reading the properties TokenValidationResult.IsValid or TokenValidationResult.Exception. This could be a potential security issue.";
        public const string IDX10110 = "IDX10110: When setting LastKnownGoodLifetime, the value must be greater than or equal to zero. value: '{0}'.";

        // token validation
        public const string IDX10204 = "IDX10204: Unable to validate issuer. validationParameters.ValidIssuer is null or whitespace AND validationParameters.ValidIssuers is null or empty.";
        public const string IDX10205 = "IDX10205: Issuer validation failed. Issuer: '{0}'. Did not match: validationParameters.ValidIssuer: '{1}' or validationParameters.ValidIssuers: '{2}' or validationParameters.ConfigurationManager.CurrentConfiguration.Issuer: '{3}'. For more details, see https://aka.ms/IdentityModel/issuer-validation. ";
        public const string IDX10206 = "IDX10206: Unable to validate audience. The 'audiences' parameter is empty.";
        public const string IDX10207 = "IDX10207: Unable to validate audience. The 'audiences' parameter is null.";
        public const string IDX10208 = "IDX10208: Unable to validate audience. validationParameters.ValidAudience is null or whitespace and validationParameters.ValidAudiences is null.";
        public const string IDX10209 = "IDX10209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.";
        public const string IDX10211 = "IDX10211: Unable to validate issuer. The 'issuer' parameter is null or whitespace";
        public const string IDX10214 = "IDX10214: Audience validation failed. Audiences: '{0}'. Did not match: validationParameters.ValidAudience: '{1}' or validationParameters.ValidAudiences: '{2}'.";
        public const string IDX10222 = "IDX10222: Lifetime validation failed. The token is not yet valid. ValidFrom (UTC): '{0}', Current time (UTC): '{1}'.";
        public const string IDX10223 = "IDX10223: Lifetime validation failed. The token is expired. ValidTo (UTC): '{0}', Current time (UTC): '{1}'.";
        public const string IDX10224 = "IDX10224: Lifetime validation failed. The NotBefore (UTC): '{0}' is after Expires (UTC): '{1}'.";
        public const string IDX10225 = "IDX10225: Lifetime validation failed. The token is missing an Expiration Time. Tokentype: '{0}'.";
        public const string IDX10227 = "IDX10227: TokenValidationParameters.TokenReplayCache is not null, indicating to check for token replay but the security token has no expiration time: token '{0}'.";
        public const string IDX10228 = "IDX10228: The securityToken has previously been validated, securityToken: '{0}'.";
        public const string IDX10229 = "IDX10229: TokenValidationParameters.TokenReplayCache was unable to add the securityToken: '{0}'.";
        public const string IDX10230 = "IDX10230: Lifetime validation failed. Delegate returned false, securitytoken: '{0}'.";
        public const string IDX10231 = "IDX10231: Audience validation failed. Delegate returned false, securitytoken: '{0}'.";
        public const string IDX10232 = "IDX10232: IssuerSigningKey validation failed. Delegate returned false, securityKey: '{0}'.";
        public const string IDX10233 = "IDX10233: ValidateAudience property on ValidationParameters is set to false. Exiting without validating the audience.";
        public const string IDX10234 = "IDX10234: Audience Validated.Audience: '{0}'";
        public const string IDX10235 = "IDX10235: ValidateIssuer property on ValidationParameters is set to false. Exiting without validating the issuer.";
        public const string IDX10236 = "IDX10236: Issuer Validated.Issuer: '{0}'";
        public const string IDX10237 = "IDX10237: ValidateIssuerSigningKey property on ValidationParameters is set to false. Exiting without validating the issuer signing key.";
        public const string IDX10238 = "IDX10238: ValidateLifetime property on ValidationParameters is set to false. Exiting without validating the lifetime.";
        public const string IDX10239 = "IDX10239: Lifetime of the token is valid.";
        public const string IDX10240 = "IDX10240: No token replay is detected.";
        public const string IDX10241 = "IDX10241: Security token validated. token: '{0}'.";
        public const string IDX10242 = "IDX10242: Security token: '{0}' has a valid signature.";
        public const string IDX10243 = "IDX10243: Reading issuer signing keys from validation parameters.";
        public const string IDX10244 = "IDX10244: Issuer is null or empty. Using runtime default for creating claims '{0}'.";
        public const string IDX10245 = "IDX10245: Creating claims identity from the validated token: '{0}'.";
        public const string IDX10246 = "IDX10246: ValidateTokenReplay property on ValidationParameters is set to false. Exiting without validating the token replay.";
        // public const string IDX10247 = "IDX10247:";
        public const string IDX10248 = "IDX10248: X509SecurityKey validation failed. The associated certificate is not yet valid. ValidFrom (UTC): '{0}', Current time (UTC): '{1}'.";
        public const string IDX10249 = "IDX10249: X509SecurityKey validation failed. The associated certificate has expired. ValidTo (UTC): '{0}', Current time (UTC): '{1}'.";
        public const string IDX10250 = "IDX10250: The associated certificate is valid. ValidFrom (UTC): '{0}', Current time (UTC): '{1}'.";
        public const string IDX10251 = "IDX10251: The associated certificate is valid. ValidTo (UTC): '{0}', Current time (UTC): '{1}'.";
        public const string IDX10252 = "IDX10252: RequireSignedTokens property on ValidationParameters is set to false and the issuer signing key is null. Exiting without validating the issuer signing key.";
        public const string IDX10253 = "IDX10253: RequireSignedTokens property on ValidationParameters is set to true, but the issuer signing key is null.";
        public const string IDX10254 = "IDX10254: '{0}.{1}' failed. The virtual method '{2}.{3}' returned null. If this method was overridden, ensure a valid '{4}' is returned.";
        public const string IDX10255 = "IDX10255: TypeValidator property on ValidationParameters is null and ValidTypes is either null or empty. Exiting without validating the token type.";
        public const string IDX10256 = "IDX10256: Unable to validate the token type. TokenValidationParameters.ValidTypes is set, but the 'typ' header claim is null or empty.";
        public const string IDX10257 = "IDX10257: Token type validation failed. Type: '{0}'. Did not match: validationParameters.TokenTypes: '{1}'.";
        public const string IDX10258 = "IDX10258: Token type validated. Type: '{0}'.";
        // public const string IDX10260 = "IDX10260:";
        public const string IDX10261 = "IDX10261: Unable to retrieve configuration from authority: '{0}'. \nProceeding with token validation in case the relevant properties have been set manually on the TokenValidationParameters. Exception caught: \n {1}. See https://aka.ms/validate-using-configuration-manager for additional information.";
        public const string IDX10262 = "IDX10262: One of the issuers in TokenValidationParameters.ValidIssuers was null or an empty string. See https://aka.ms/wilson/tokenvalidation for details.";
        //public const string IDX10263 = "IDX10263: Unable to re-validate with ConfigurationManager.LastKnownGoodConfiguration as it is expired.";
        public const string IDX10264 = "IDX10264: Reading issuer signing keys from validation parameters and configuration.";
        public const string IDX10265 = "IDX10265: Reading issuer signing keys from configuration.";

        // 10500 - SignatureValidation
        public const string IDX10500 = "IDX10500: Signature validation failed. No security keys were provided to validate the signature.";
        //public const string IDX10501 = "IDX10501: Signature validation failed. Unable to match key: \nkid: '{0}'. \nNumber of keys in TokenValidationParameters: '{1}'. \nNumber of keys in Configuration: '{2}'. \nExceptions caught:\n '{3}'. \ntoken: '{4}'.";
        public const string IDX10503 = "IDX10503: Signature validation failed. Token does not have a kid. Keys tried: '{0}'. Number of keys in TokenValidationParameters: '{1}'. \nNumber of keys in Configuration: '{2}'. \nExceptions caught:\n '{3}'.\ntoken: '{4}'. See https://aka.ms/IDX10503 for details.";
        public const string IDX10504 = "IDX10504: Unable to validate signature, token does not have a signature: '{0}'.";
        public const string IDX10505 = "IDX10505: Signature validation failed. The user defined 'Delegate' specified on TokenValidationParameters returned null when validating token: '{0}'.";
        public const string IDX10506 = "IDX10506: Signature validation failed. The user defined 'Delegate' specified on TokenValidationParameters did not return a '{0}', but returned a '{1}' when validating token: '{2}'.";
        // public const string IDX10507 = "IDX10507:";
        public const string IDX10508 = "IDX10508: Signature validation failed. Signature is improperly formatted.";
        public const string IDX10509 = "IDX10509: Token validation failed. The user defined 'Delegate' set on TokenValidationParameters.TokenReader did not return a '{0}', but returned a '{1}' when reading token: '{2}'.";
        public const string IDX10510 = "IDX10510: Token validation failed. The user defined 'Delegate' set on TokenValidationParameters.TokenReader returned null when reading token: '{0}'.";
        public const string IDX10511 = "IDX10511: Signature validation failed. Keys tried: '{0}'. \nNumber of keys in TokenValidationParameters: '{1}'. \nNumber of keys in Configuration: '{2}'. \nMatched key was in '{3}'. \nkid: '{4}'. \nExceptions caught:\n '{5}'.\ntoken: '{6}'. See https://aka.ms/IDX10511 for details.";
        public const string IDX10512 = "IDX10512: Signature validation failed. Token does not have KeyInfo. Keys tried: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'.";
        //public const string IDX10513 = "IDX10513: Signature validation failed. Unable to match key: \nKeyInfo: '{0}'.\nExceptions caught:\n '{1}'. \ntoken: '{2}'.";
        public const string IDX10514 = "IDX10514: Signature validation failed. Keys tried: '{0}'. \nKeyInfo: '{1}'. \nExceptions caught:\n '{2}'.\ntoken: '{3}'.";
        //public const string IDX10515 = "IDX10515: Signature validation failed. Unable to match key: \nKeyInfo: '{0}'.\nExceptions caught:\n '{1}'. \ntoken: '{2}'. Valid Lifetime: '{3}'. Valid Issuer: '{4}'";
        //public const string IDX10516 = "IDX10516: Signature validation failed. Unable to match key: \nkid: '{0}'. \nNumber of keys in TokenValidationParameters: '{1}'. \nNumber of keys in Configuration: '{2}'. \nExceptions caught:\n '{3}'. \ntoken: '{4}'. Valid Lifetime: '{5}'. Valid Issuer: '{6}'";

        // encryption / decryption
        // public const string IDX10600 = "IDX10600:";
        // public const string IDX10601 = "IDX10601:";
        public const string IDX10603 = "IDX10603: Decryption failed. Keys tried: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'";
        // public const string IDX10604 = "IDX10604:";
        // public const string IDX10605 = "IDX10605:";
        // public const string IDX10606 = "IDX10606:";
        public const string IDX10607 = "IDX10607: Decryption skipping key: '{0}', both validationParameters.CryptoProviderFactory and key.CryptoProviderFactory are null.";
        // public const string IDX10608 = "IDX10608:";
        public const string IDX10609 = "IDX10609: Decryption failed. No Keys tried: token: '{0}'.";
        public const string IDX10610 = "IDX10610: Decryption failed. Could not create decryption provider. Key: '{0}', Algorithm: '{1}'.";
        public const string IDX10611 = "IDX10611: Decryption failed. Encryption is not supported for: Algorithm: '{0}', SecurityKey: '{1}'.";
        public const string IDX10612 = "IDX10612: Decryption failed. Header.Enc is null or empty, it must be specified.";
        // public const string IDX10613 = "IDX10613:";
        // public const string IDX10614 = "IDX10614:";
        public const string IDX10615 = "IDX10615: Encryption failed. No support for: Algorithm: '{0}', SecurityKey: '{1}'.";
        public const string IDX10616 = "IDX10616: Encryption failed. EncryptionProvider failed for: Algorithm: '{0}', SecurityKey: '{1}'. See inner exception.";
        public const string IDX10617 = "IDX10617: Encryption failed. Keywrap is only supported for: '{0}', '{1}' and '{2}'. The content encryption specified is: '{3}'.";
        public const string IDX10618 = "IDX10618: Key unwrap failed using decryption Keys: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'.";
        public const string IDX10619 = "IDX10619: Decryption failed. Algorithm: '{0}'. Either the Encryption Algorithm: '{1}' or none of the Security Keys are supported by the CryptoProviderFactory.";
        public const string IDX10620 = "IDX10620: Unable to obtain a CryptoProviderFactory, both EncryptingCredentials.CryptoProviderFactory and EncryptingCredentials.Key.CrypoProviderFactory are null.";
        //public const string IDX10903 = "IDX10903: Token decryption succeeded. With thumbprint: '{0}'.";
        public const string IDX10904 = "IDX10904: Token decryption key : '{0}' found in TokenValidationParameters.";
        public const string IDX10905 = "IDX10905: Token decryption key : '{0}' found in Configuration/Metadata.";

        // Formating
        public const string IDX10400 = "IDX10400: Unable to decode: '{0}' as Base64url encoded string.";
        public const string IDX10401 = "IDX10401: Invalid requested key size. Valid key sizes are: 256, 384, and 512.";

        // Crypto Errors
        public const string IDX10621 = "IDX10621: '{0}' supports: '{1}' of types: '{2}' or '{3}'. SecurityKey received was of type '{4}'.";
        // public const string IDX10622 = "IDX10622:";
        // public const string IDX10623 = "IDX10623:";
        // public const string IDX10624 = "IDX10624:";
        // public const string IDX10627 = "IDX10627:";
        public const string IDX10628 = "IDX10628: Cannot set the MinimumSymmetricKeySizeInBits to less than '{0}'.";
        public const string IDX10630 = "IDX10630: The '{0}' for signing cannot be smaller than '{1}' bits. KeySize: '{2}'.";
        public const string IDX10631 = "IDX10631: The '{0}' for verifying cannot be smaller than '{1}' bits. KeySize: '{2}'.";
        public const string IDX10634 = "IDX10634: Unable to create the SignatureProvider.\nAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms";
        // public const string IDX10635 = "IDX10635:";
        public const string IDX10636 = "IDX10636: CryptoProviderFactory.CreateForVerifying returned null for key: '{0}', signatureAlgorithm: '{1}'.";
        public const string IDX10637 = "IDX10637: CryptoProviderFactory.CreateForSigning returned null for key: '{0}', signatureAlgorithm: '{1}'.";
        public const string IDX10638 = "IDX10638: Cannot create the SignatureProvider, 'key.HasPrivateKey' is false, cannot create signatures. Key: {0}.";
        public const string IDX10640 = "IDX10640: Algorithm is not supported: '{0}'.";
        // public const string IDX10641 = "IDX10641:";
        public const string IDX10642 = "IDX10642: Creating signature using the input: '{0}'.";
        public const string IDX10643 = "IDX10643: Comparing the signature created over the input with the token signature: '{0}'.";
        // public const string IDX10644 = "IDX10644:";
        public const string IDX10645 = "IDX10645: Elliptical Curve not supported for curveId: '{0}'";
        public const string IDX10646 = "IDX10646: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}', Key: '{1}'), but Create.(algorithm, args) as '{2}' == NULL.";
        public const string IDX10647 = "IDX10647: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}'), but Create.(algorithm, args) as '{1}' == NULL.";
        // public const string IDX10648 = "IDX10648:";
        public const string IDX10649 = "IDX10649: Failed to create a SymmetricSignatureProvider for the algorithm '{0}'.";
        public const string IDX10650 = "IDX10650: Failed to verify ciphertext with aad '{0}'; iv '{1}'; and authenticationTag '{2}'.";
        // public const string IDX10651 = "IDX10651:";
        public const string IDX10652 = "IDX10652: The algorithm '{0}' is not supported.";
        public const string IDX10653 = "IDX10653: The encryption algorithm '{0}' requires a key size of at least '{1}' bits. Key '{2}', is of size: '{3}'.";
        public const string IDX10654 = "IDX10654: Decryption failed. Cryptographic operation exception: '{0}'.";
        public const string IDX10655 = "IDX10655: '{0}' must be greater than 1, was: '{1}'";
        // public const string IDX10656 = "IDX10656:";
        public const string IDX10657 = "IDX10657: The SecurityKey provided for the symmetric key wrap algorithm cannot be converted to byte array. Type is: '{0}'.";
        public const string IDX10658 = "IDX10658: WrapKey failed, exception from cryptographic operation: '{0}'";
        public const string IDX10659 = "IDX10659: UnwrapKey failed, exception from cryptographic operation: '{0}'";
        // public const string IDX10660 = "IDX10660:";
        public const string IDX10661 = "IDX10661: Unable to create the KeyWrapProvider.\nKeyWrapAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported.";
        public const string IDX10662 = "IDX10662: The KeyWrap algorithm '{0}' requires a key size of '{1}' bits. Key '{2}', is of size:'{3}'.";
        public const string IDX10663 = "IDX10663: Failed to create symmetric algorithm with SecurityKey: '{0}', KeyWrapAlgorithm: '{1}'.";
        public const string IDX10664 = "IDX10664: The length of input must be a multiple of 64 bits. The input size is: '{0}' bits.";
        public const string IDX10665 = "IDX10665: Data is not authentic";
        public const string IDX10666 = "IDX10666: Unable to create KeyedHashAlgorithm for algorithm '{0}'.";
        public const string IDX10667 = "IDX10667: Unable to obtain required byte array for KeyHashAlgorithm from SecurityKey: '{0}'.";
        public const string IDX10668 = "IDX10668: Unable to create '{0}', algorithm '{1}'; key: '{2}' is not supported.";
        public const string IDX10669 = "IDX10669: Failed to create symmetric algorithm.";
        // public const string IDX10670 = "IDX10670:";
        // public const string IDX10671 = "IDX10671:";
        // public const string IDX10672 = "IDX10672:";
        // public const string IDX10673 = "IDX10673:";
        public const string IDX10674 = "IDX10674: JsonWebKeyConverter does not support SecurityKey of type: {0}";
        public const string IDX10675 = "IDX10675: Cannot create a ECDsa object from the '{0}', the bytes from the decoded value of '{1}' must be less than the size associated with the curve: '{2}'. Size was: '{3}'.";
        // public const string IDX10676 = "IDX10676:";
        // public const string IDX10677 = "IDX10677:";
        // public const string IDX10678 = "IDX10678:";
        public const string IDX10679 = "IDX10679: Failed to decompress using algorithm '{0}'.";
        public const string IDX10680 = "IDX10680: Failed to compress using algorithm '{0}'.";
        // public const string IDX10681 = "IDX10681:";
        public const string IDX10682 = "IDX10682: Compression algorithm '{0}' is not supported.";
        // public const string IDX10683 = "IDX10683:";
        public const string IDX10684 = "IDX10684: Unable to convert the JsonWebKey to an AsymmetricSecurityKey. Algorithm: '{0}', Key: '{1}'.";
        public const string IDX10685 = "IDX10685: Unable to Sign, Internal SignFunction is not available.";
        public const string IDX10686 = "IDX10686: Unable to Verify, Internal VerifyFunction is not available.";
        //public const string IDX10688 = "IDX10688:"
        public const string IDX10689 = "IDX10689: Unable to create an ECDsa object. See inner exception for more details.";
        public const string IDX10690 = "IDX10690: ECDsa creation is not supported by the current platform. For more details, see https://aka.ms/IdentityModel/create-ecdsa";
        //public const string IDX10691 = "IDX10691:"
        //public const string IDX10692 = "IDX10692: The RSASS-PSS signature algorithm is not available on the .NET 4.5 target. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms";
        public const string IDX10693 = "IDX10693: RSACryptoServiceProvider doesn't support the RSASSA-PSS signature algorithm. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms";
        public const string IDX10694 = "IDX10694: JsonWebKeyConverter threw attempting to convert JsonWebKey: '{0}'. Exception: '{1}'.";
        public const string IDX10695 = "IDX10695: Unable to create a JsonWebKey from an ECDsa object. Required ECParameters structure is not supported by .NET Framework < 4.7.";
        public const string IDX10696 = "IDX10696: The algorithm '{0}' is not in the user-defined accepted list of algorithms.";
        public const string IDX10697 = "IDX10697: The user defined 'Delegate' AlgorithmValidator specified on TokenValidationParameters returned false when validating Algorithm: '{0}', SecurityKey: '{1}'.";
        public const string IDX10698 = "IDX10698: The SignatureProviderObjectPoolCacheSize must be greater than 0. Value: '{0}'.";
        public const string IDX10699 = "IDX10699: Unable to remove SignatureProvider with cache key: {0} from the InMemoryCryptoProviderCache. Exception: '{1}'.";

        // security keys
        public const string IDX10700 = "IDX10700: {0} is unable to use 'rsaParameters'. {1} is null.";
        //public const string IDX10701 = "IDX10701:"
        //public const string IDX10702 = "IDX10702:"
        public const string IDX10703 = "IDX10703: Cannot create a '{0}', key length is zero.";
        public const string IDX10704 = "IDX10704: Cannot verify the key size. The SecurityKey is not or cannot be converted to an AsymmetricSecuritKey. SecurityKey: '{0}'.";
        public const string IDX10705 = "IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.";
        public const string IDX10706 = "IDX10706: Cannot create a JWK thumbprint, '{0}' must be one of the following: '{1}'.";
        public const string IDX10707 = "IDX10707: Cannot create a JSON representation of an asymmetric public key, '{0}' must be one of the following: '{1}'.";
        public const string IDX10708 = "IDX10708: Cannot create a JSON representation of an EC public key, '{0}' is null or empty.";
        public const string IDX10709 = "IDX10709: Cannot create a JSON representation of an RSA public key, '{0}' is null or empty.";
        public const string IDX10710 = "IDX10710: Computing a JWK thumbprint is supported only on SymmetricSecurityKey, JsonWebKey, RsaSecurityKey, X509SecurityKey, and ECDsaSecurityKey.";
        public const string IDX10711 = "IDX10711: Unable to Decrypt, Internal DecryptionFunction is not available.";
        public const string IDX10712 = "IDX10712: Unable to Encrypt, Internal EncryptionFunction is not available.";
        public const string IDX10713 = "IDX10713: Encrytion/Decryption using algorithm '{0}' is only supported on Windows platform.";
        public const string IDX10714 = "IDX10714: Unable to perform the decryption. There is a authentication tag mismatch.";
        public const string IDX10715 = "IDX10715: Encryption using algorithm: '{0}' is not supported.";
        public const string IDX10716 = "IDX10716: '{0}' must be greater than 0, was: '{1}'";
        public const string IDX10717 = "IDX10717: '{0} + {1}' must not be greater than {2}, '{3} + {4} > {5}'.";
        public const string IDX10718 = "IDX10718: AlgorithmToValidate is not supported: '{0}'. Algorithm '{1}'.";
        public const string IDX10719 = "IDX10719: SignatureSize (in bytes) was expected to be '{0}', was '{1}'.";
        public const string IDX10720 = "IDX10720: Unable to create KeyedHashAlgorithm for algorithm '{0}', the key size must be greater than: '{1}' bits, key has '{2}' bits.";

        // Json specific errors
        //public const string IDX10801 = "IDX10801:"
        //public const string IDX10802 = "IDX10802:"
        //public const string IDX10804 = "IDX10804:"
        public const string IDX10805 = "IDX10805: Error deserializing json: '{0}' into '{1}'.";
        public const string IDX10806 = "IDX10806: Deserializing json: '{0}' into '{1}'.";
        //public const string IDX10807 = "IDX10807:"
        public const string IDX10808 = "IDX10808: The 'use' parameter of a JsonWebKey: '{0}' was expected to be 'sig' or empty, but was '{1}'.";
        //public const string IDX10809 = "IDX10809:"
        public const string IDX10810 = "IDX10810: Unable to convert the JsonWebKey: '{0}' to a X509SecurityKey, RsaSecurityKey or ECDSASecurityKey.";
        //public const string IDX10811 = "IDX10811:"
        public const string IDX10812 = "IDX10812: Unable to create a {0} from the properties found in the JsonWebKey: '{1}'.";
        public const string IDX10813 = "IDX10813: Unable to create a {0} from the properties found in the JsonWebKey: '{1}', Exception '{2}'.";
        public const string IDX10814 = "IDX10814: Unable to create a {0} from the properties found in the JsonWebKey: '{1}'. Missing: '{2}'.";
        public const string IDX10815 = "IDX10815: Depth of JSON: '{0}' exceeds max depth of '{1}'.";
        public const string IDX10816 = "IDX10816: Decompressing would result in a token with a size greater than allowed. Maximum size allowed: '{0}'.";

        // Base64UrlEncoding
        public const string IDX10820 = "IDX10820: Invalid character found in Base64UrlEncoding. Character: '{0}', Encoding: '{1}'.";
        public const string IDX10821 = "IDX10821: Incorrect padding detected in Base64UrlEncoding. Encoding: '{0}'.";

        //EventBasedLRUCache errors
        public const string IDX10900 = "IDX10900: EventBasedLRUCache._eventQueue encountered an error while processing a cache operation. Exception '{0}'.";
        public const string IDX10901 = "IDX10901: CryptoProviderCacheOptions.SizeLimit must be greater than 10. Value: '{0}'";
        public const string IDX10902 = "IDX10902: Object disposed exception in '{0}': '{1}'";

        // Crypto Errors
        public const string IDX11000 = "IDX11000: Cannot create EcdhKeyExchangeProvider. '{0}'\'s Curve '{1}' does not match with '{2}'\'s curve '{3}'.";
        public const string IDX11001 = "IDX11001: Cannot generate KDF. '{0}':'{1}' and '{2}':'{3}' must be different.";
        public const string IDX11002 = "IDX11002: Cannot create the EcdhKeyExchangeProvider. Unable to obtain ECParameters from {0}. Verify the SecurityKey is an ECDsaSecurityKey or JsonWebKey and that properties Crv, X, Y, and D (if used for a private key) are contained in the provided SecurityKey.";

        // Json parsing errors
        public const string IDX11020 = "IDX11020: The JSON value of type: '{0}', could not be converted to '{1}'. Reading: '{2}.{3}', Position: '{4}', CurrentDepth: '{5}', BytesConsumed: '{6}'.";
        public const string IDX11022 = "IDX11022: Expecting json reader to be positioned on '{0}', reader was positioned at: '{1}', Reading: '{2}.{3}', Position: '{4}', CurrentDepth: '{5}', BytesConsumed: '{6}'.";
        public const string IDX11023 = "IDX11023: Expecting json reader to be positioned on '{0}', reader was positioned at: '{1}', Reading: '{2}', Position: '{3}', CurrentDepth: '{4}', BytesConsumed: '{5}'.";
        public const string IDX11025 = "IDX11025: Cannot serialize object of type: '{0}' into property: '{1}'.";

#pragma warning restore 1591
    }
}
