// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Validators
{
    /// <summary>
    /// A generic class for additional validation checks on <see cref="SecurityToken"/> issued by the Microsoft identity platform (AAD).
    /// </summary>
    public static class AadTokenValidationParametersExtension
    {
        /// <summary>
        /// Enables the validation of the issuer of the signing keys used by the Microsoft identity platform (AAD) against the issuer of the token.
        /// </summary>
        /// <param name="tokenValidationParameters">The <see cref="TokenValidationParameters"/> that are used to validate the token.</param>
        public static void EnableAadSigningKeyIssuerValidation(this TokenValidationParameters tokenValidationParameters)
        {
            if (tokenValidationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(tokenValidationParameters));

            var userProvidedIssuerSigningKeyValidatorUsingConfiguration = tokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration;
            var userProvidedIssuerSigningKeyValidator = tokenValidationParameters.IssuerSigningKeyValidator;

            tokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, tvp, config) =>
            {
                ValidateIssuerSigningKey(securityKey, securityToken, config);

                // preserve and run provided logic
                if (userProvidedIssuerSigningKeyValidatorUsingConfiguration != null)
                    return userProvidedIssuerSigningKeyValidatorUsingConfiguration(securityKey, securityToken, tvp, config);

                if (userProvidedIssuerSigningKeyValidator != null)
                    return userProvidedIssuerSigningKeyValidator(securityKey, securityToken, tvp);

                return ValidateIssuerSigningKeyCertificate(securityKey, tvp);
            };
        }

        /// <summary>
        /// Validates the issuer signing key.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated, could be a JwtSecurityToken or JsonWebToken.</param>
        /// <param name="configuration">The <see cref="OpenIdConnectConfiguration"/> provided.</param>
        /// <returns><c>true</c> if the issuer signing key is valid; otherwise, <c>false</c>.</returns>
        internal static bool ValidateIssuerSigningKey(SecurityKey securityKey, SecurityToken securityToken, BaseConfiguration configuration)
        {
            if (securityKey == null)
                return true;

            if (securityToken == null)
                throw LogHelper.LogArgumentNullException(nameof(securityToken));

            var openIdConnectConfiguration = configuration as OpenIdConnectConfiguration;
            if (openIdConnectConfiguration == null)
                return true;

            var matchedKeyFromConfig = openIdConnectConfiguration.JsonWebKeySet?.Keys.FirstOrDefault(x => x != null && x.Kid == securityKey.KeyId);
            if (matchedKeyFromConfig != null && matchedKeyFromConfig.AdditionalData.TryGetValue(OpenIdProviderMetadataNames.Issuer, out object value))
            {
                var signingKeyIssuer = value as string;
                if (string.IsNullOrWhiteSpace(signingKeyIssuer))
                    return true;

                var tenantIdFromToken = GetTid(securityToken);
                if (string.IsNullOrEmpty(tenantIdFromToken))
                {
                    if (AppContextSwitches.DontFailOnMissingTid)
                        return true; 
                    
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogMessages.IDX40009));
                }

                var tokenIssuer = securityToken.Issuer;

#if NET6_0_OR_GREATER
                if (!string.IsNullOrEmpty(tokenIssuer) && !tokenIssuer.Contains(tenantIdFromToken, StringComparison.Ordinal))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogHelper.FormatInvariant(LogMessages.IDX40004, LogHelper.MarkAsNonPII(tokenIssuer), LogHelper.MarkAsNonPII(tenantIdFromToken))));

                // creating an effectiveSigningKeyIssuer is required as signingKeyIssuer might contain {tenantid}
                var effectiveSigningKeyIssuer = signingKeyIssuer.Replace(AadIssuerValidator.TenantIdTemplate, tenantIdFromToken, StringComparison.Ordinal);
                var v2TokenIssuer = openIdConnectConfiguration.Issuer?.Replace(AadIssuerValidator.TenantIdTemplate, tenantIdFromToken, StringComparison.Ordinal);
#else
                if (!string.IsNullOrEmpty(tokenIssuer) && !tokenIssuer.Contains(tenantIdFromToken))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogHelper.FormatInvariant(LogMessages.IDX40004, LogHelper.MarkAsNonPII(tokenIssuer), LogHelper.MarkAsNonPII(tenantIdFromToken))));

                // creating an effectiveSigningKeyIssuer is required as signingKeyIssuer might contain {tenantid}
                var effectiveSigningKeyIssuer = signingKeyIssuer.Replace(AadIssuerValidator.TenantIdTemplate, tenantIdFromToken);
                var v2TokenIssuer = openIdConnectConfiguration.Issuer?.Replace(AadIssuerValidator.TenantIdTemplate, tenantIdFromToken);
#endif

                // comparing effectiveSigningKeyIssuer with v2TokenIssuer is required as well because of the following scenario:
                // 1. service trusts /common/v2.0 endpoint 
                // 2. service receives a v1 token that has issuer like sts.windows.net
                // 3. signing key issuers will never match sts.windows.net as v1 endpoint doesn't have issuers attached to keys
                // v2TokenIssuer is the representation of Token.Issuer (if it was a v2 issuer)
                if (effectiveSigningKeyIssuer != tokenIssuer && effectiveSigningKeyIssuer != v2TokenIssuer)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogHelper.FormatInvariant(LogMessages.IDX40005, LogHelper.MarkAsNonPII(tokenIssuer), LogHelper.MarkAsNonPII(effectiveSigningKeyIssuer))));
            }

            return true;
        }

        private static string GetTid(SecurityToken securityToken)
        {
            switch (securityToken)
            {
                case JsonWebToken jsonWebToken:
                    if (jsonWebToken.TryGetPayloadValue<string>(AadIssuerValidatorConstants.Tid, out string tid))
                    {
                        EnforceSingleClaimCaseInsensitive(jsonWebToken.PayloadClaimNames, AadIssuerValidatorConstants.Tid);
                        return tid;
                    }

                    return string.Empty;

                case JwtSecurityToken jwtSecurityToken:
                    if ((jwtSecurityToken.Payload.TryGetValue(AadIssuerValidatorConstants.Tid, out object tidObject) && tidObject is string jwtTid))
                    {
                        EnforceSingleClaimCaseInsensitive(jwtSecurityToken.Payload.Keys, AadIssuerValidatorConstants.Tid);
                        return jwtTid;
                    }

                    return string.Empty;

                default:
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogMessages.IDX40010));
            }
        }

        private static void EnforceSingleClaimCaseInsensitive(IEnumerable<string> keys, string claimType)
        {
            bool claimSeen = false;
            foreach (var key in keys)
            {
                if (string.Equals(key, claimType, StringComparison.OrdinalIgnoreCase))
                {
                    if (claimSeen)
                        throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogHelper.FormatInvariant(LogMessages.IDX40011, claimType)));

                    claimSeen = true;
                }
            }
        }

        /// <summary>
        /// Validates the issuer signing key certificate.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> that are used to validate the token.</param>
        /// <returns><c>true</c> if the issuer signing key certificate is valid; otherwise, <c>false</c>.</returns>
        internal static bool ValidateIssuerSigningKeyCertificate(SecurityKey securityKey, TokenValidationParameters validationParameters)
        {
            if (!validationParameters.RequireSignedTokens && securityKey == null)
            {
                LogHelper.LogInformation(Tokens.LogMessages.IDX10252);
                return true;
            }
            else if (securityKey == null)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(securityKey), LogMessages.IDX40007));
            }

            if (!validationParameters.ValidateIssuerSigningKey)
            {
                LogHelper.LogVerbose(Tokens.LogMessages.IDX10237);
                return true;
            }

            Tokens.Validators.ValidateIssuerSigningKeyLifeTime(securityKey, validationParameters);

            return true;
        }
    }
}
