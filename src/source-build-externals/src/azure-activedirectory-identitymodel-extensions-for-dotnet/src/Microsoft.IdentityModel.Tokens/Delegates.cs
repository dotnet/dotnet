// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Definition for AlgorithmValidator
    /// </summary>
    /// <param name="algorithm">The algorithm to validate.</param>
    /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns><c>true</c> if the algorithm is considered valid</returns>
    public delegate bool AlgorithmValidator(string algorithm, SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for AudienceValidator.
    /// </summary>
    /// <param name="audiences">The audiences found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>true if the audience is considered valid.</returns>
    public delegate bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerSigningKeyResolver.
    /// </summary>
    /// <param name="token">The <see cref="string"/> representation of the token that is being validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated. It may be null.</param>
    /// <param name="kid">A key identifier. It may be null.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>A <see cref="SecurityKey"/> to use when validating a signature.</returns>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerSigningKeyResolverUsingConfiguration.
    /// </summary>
    /// <param name="token">The <see cref="string"/> representation of the token that is being validated.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated. It may be null.</param>
    /// <param name="kid">A key identifier. It may be null.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration"><see cref="BaseConfiguration"/> required for validation.</param>
    /// <returns>A <see cref="SecurityKey"/> to use when validating a signature.</returns>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate IEnumerable<SecurityKey> IssuerSigningKeyResolverUsingConfiguration(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for IssuerSigningKeyValidator.
    /// </summary>
    /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate bool IssuerSigningKeyValidator(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerSigningKeyValidatorUsingConfiguration.
    /// </summary>
    /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration"><see cref="BaseConfiguration"/> required for validation.</param>
    /// <remarks> If both <see cref="IssuerSigningKeyResolverUsingConfiguration"/> and <see cref="IssuerSigningKeyResolver"/> are set, IssuerSigningKeyResolverUsingConfiguration takes
    /// priority.</remarks>
    public delegate bool IssuerSigningKeyValidatorUsingConfiguration(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for IssuerValidator.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
    /// <remarks>The delegate should return a non null string that represents the 'issuer'. If null a default value will be used.
    /// If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, IssuerValidatorUsingConfiguration takes
    /// priority.</remarks>
    public delegate string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for IssuerValidatorUsingConfiguration.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration"><see cref="BaseConfiguration"/> required for validation.</param>
    /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
    /// <remarks>The delegate should return a non null string that represents the 'issuer'. If null a default value will be used.
    /// If both <see cref="IssuerValidatorUsingConfiguration"/> and <see cref="IssuerValidator"/> are set, IssuerValidatorUsingConfiguration takes
    /// priority.
    /// </remarks>
    public delegate string IssuerValidatorUsingConfiguration(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for IssuerValidatorAsync. Left internal for now while we work out the details of async validation for all delegates.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
    /// <remarks>The delegate should return a non null string that represents the 'issuer'. If null a default value will be used.
    /// <see cref="IssuerValidatorAsync"/> if set, will be called before <see cref="IssuerSigningKeyValidatorUsingConfiguration"/> or <see cref="IssuerSigningKeyValidator"/>
    /// </remarks>
    internal delegate ValueTask<string> IssuerValidatorAsync(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for LifetimeValidator.
    /// </summary>
    /// <param name="notBefore">The 'notBefore' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="expires">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    public delegate bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TokenReplayValidator.
    /// </summary>
    /// <param name="expirationTime">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns></returns>
    public delegate bool TokenReplayValidator(DateTime? expirationTime, string securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for SignatureValidator.
    /// </summary>
    /// <param name="token">A securityToken with a signature.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    public delegate SecurityToken SignatureValidator(string token, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for SignatureValidator.
    /// </summary>
    /// <param name="token">A securityToken with a signature.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="configuration">The <see cref="BaseConfiguration"/> that is required for validation.</param>
    public delegate SecurityToken SignatureValidatorUsingConfiguration(string token, TokenValidationParameters validationParameters, BaseConfiguration configuration);

    /// <summary>
    /// Definition for TokenReader.
    /// </summary>
    /// <param name="token">A securityToken with a signature.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    public delegate SecurityToken TokenReader(string token, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TokenDecryptionKeyResolver.
    /// </summary>
    /// <param name="token">The <see cref="string"/> representation of the token to be decrypted.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> to be decrypted. The runtime by default passes null.</param>
    /// <param name="kid">A key identifier. It may be null.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>A <see cref="SecurityKey"/> to use when decrypting the token.</returns>
    public delegate IEnumerable<SecurityKey> TokenDecryptionKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TypeValidator.
    /// </summary>
    /// <param name="type">The token type to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>The actual token type, that may be the same as <paramref name="type"/> or a different value if the token type was resolved from a different location.</returns>
    public delegate string TypeValidator(string type, SecurityToken securityToken, TokenValidationParameters validationParameters);

    /// <summary>
    /// Definition for TransformBeforeSignatureValidation.
    /// </summary>
    /// <param name="token">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <returns>A transformed <see cref="SecurityToken"/>.</returns>
    public delegate SecurityToken TransformBeforeSignatureValidation(SecurityToken token, TokenValidationParameters validationParameters);
}
