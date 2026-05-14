// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Validates the cryptographic algorithm used.
    /// </summary>
    /// <param name="algorithm">The algorithm to be validated.</param>
    /// <param name="securityKey">The <see cref="SecurityKey"/> used to sign the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns><see langword="true"/> if the algorithm is valid; otherwise, <see langword="false"/>.</returns>
    public delegate bool AlgorithmValidator(string algorithm, SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the audiences found in the security token.
    /// </summary>
    /// <param name="audiences">The audiences found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns><see langword="true"/> if the audience is valid; otherwise, <see langword="false"/>.</returns>
    public delegate bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Resolves the signing key used for validating a token's signature.
    /// </summary>
    /// <param name="token">The string representation of the token being validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated, which may be null.</param>
    /// <param name="kid">The key identifier, which may be null.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>The <see cref="SecurityKey"/> used to validate the signature.</returns>
    /// <remarks>If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, <see cref="IssuerSigningKeyResolverUsingConfiguration"/> takes priority.</remarks>
    public delegate IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);

    /// <summary>
    /// Resolves the signing key using additional configuration.
    /// </summary>
    /// <param name="token">The string representation of the token being validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated, which may be null.</param>
    /// <param name="kid">The key identifier, which may be null.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <param name="configuration">The configuration required for validation.</param>
    /// <returns>The <see cref="SecurityKey"/> used to validate the signature.</returns>
    /// <remarks>If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, <see cref="IssuerSigningKeyResolverUsingConfiguration"/> takes priority.</remarks>
    public delegate IEnumerable<SecurityKey> IssuerSigningKeyResolverUsingConfiguration(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Validates the signing key used for the security token.
    /// </summary>
    /// <param name="securityKey">The <see cref="SecurityKey"/> used to sign the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns><see langword="true"/> if the signing key is valid; otherwise, <see langword="false"/>.</returns>
    public delegate bool IssuerSigningKeyValidator(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the signing key using additional configuration.
    /// </summary>
    /// <param name="securityKey">The <see cref="SecurityKey"/> used to sign the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <param name="configuration">The configuration required for validation.</param>
    /// <returns><see langword="true"/> if the signing key is valid; otherwise, <see langword="false"/>.</returns>
    public delegate bool IssuerSigningKeyValidatorUsingConfiguration(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Validates the issuer of the security token.
    /// </summary>
    /// <param name="issuer">The issuer to be validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>The validated issuer to use when creating claims.</returns>
    /// <remarks>The delegate should return a non-null string that represents the issuer. If null, a default value will be used. If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, <see cref="IssuerValidatorUsingConfiguration"/> takes priority.</remarks>
    public delegate string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the issuer using additional configuration.
    /// </summary>
    /// <param name="issuer">The issuer to be validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <param name="configuration">The configuration required for validation.</param>
    /// <returns>The validated issuer to use when creating claims.</returns>
    /// <remarks>The delegate should return a non-null string that represents the issuer. If null, a default value will be used. If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, <see cref="IssuerValidatorUsingConfiguration"/> takes priority.</remarks>
    public delegate string IssuerValidatorUsingConfiguration(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Asynchronously validates the issuer of the security token.
    /// </summary>
    /// <param name="issuer">The issuer to be validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>A task that represents the asynchronous operation, containing the validated issuer to use when creating claims.</returns>
    /// <remarks>The delegate should return a non-null string that represents the issuer. If null, a default value will be used. <see cref="IssuerValidatorAsync"/> will be called before <see cref="IssuerSigningKeyValidatorUsingConfiguration"/> or <see cref="IssuerSigningKeyValidator"/> if set.</remarks>
    internal delegate ValueTask<string> IssuerValidatorAsync(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the lifetime of the security token.
    /// </summary>
    /// <param name="notBefore">The 'not before' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="expires">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns><see langword="true"/> if the lifetime is valid; otherwise, <see langword="false"/>.</returns>
    public delegate bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the replay of the security token.
    /// </summary>
    /// <param name="expirationTime">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns><see langword="true"/> if the token replay is valid; otherwise, <see langword="false"/>.</returns>
    public delegate bool TokenReplayValidator(DateTime? expirationTime, string securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the signature of the security token.
    /// </summary>
    /// <param name="token">The security token with a signature.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>The validated <see cref="SecurityToken"/>.</returns>
    public delegate SecurityToken SignatureValidator(string token, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the signature using additional configuration.
    /// </summary>
    /// <param name="token">The security token with a signature.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <param name="configuration">The configuration required for validation.</param>
    /// <returns>The validated <see cref="SecurityToken"/>.</returns>
    public delegate SecurityToken SignatureValidatorUsingConfiguration(string token, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Reads the security token.
    /// </summary>
    /// <param name="token">The security token.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>The read <see cref="SecurityToken"/>.</returns>
    public delegate SecurityToken TokenReader(string token, TokenValidationParameters validationParameters);

    /// <summary>
    /// Resolves the decryption key for the security token.
    /// </summary>
    /// <param name="token">The string representation of the token to be decrypted.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> to be decrypted, which is null by default.</param>
    /// <param name="kid">The key identifier, which may be null.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>The <see cref="SecurityKey"/> used to decrypt the token.</returns>
    public delegate IEnumerable<SecurityKey> TokenDecryptionKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);

    /// <summary>
    /// Validates the type of the security token.
    /// </summary>
    /// <param name="type">The token type to be validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>The actual token type, which may be the same as <paramref name="type"/> or a different value if the token type was resolved from a different location.</returns>
    public delegate string TypeValidator(string type, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Transforms the security token before signature validation.
    /// </summary>
    /// <param name="token">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <returns>The transformed <see cref="SecurityToken"/>.</returns>
    public delegate SecurityToken TransformBeforeSignatureValidation(SecurityToken token, TokenValidationParameters validationParameters);
}
