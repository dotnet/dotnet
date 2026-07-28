// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Validators
{
    /// <summary>
    /// A generic class for additional validation checks on <see cref="SecurityToken"/> issued by the Microsoft identity platform (AAD).
    /// </summary>
    internal static class AadValidationParametersExtension
    {
        /// <summary>
        /// Enables validation of the cloud instance of the Microsoft Entra ID token signing keys.
        /// </summary>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> that are used to validate the token.</param>
        internal static void EnableEntraIdSigningKeyCloudInstanceValidation(this ValidationParameters validationParameters)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            SignatureKeyValidationDelegate originalIssuerSigningKeyValidationDelegate = validationParameters.SignatureKeyValidator;

            SignatureKeyValidationDelegate cloudInstanceSigningKeyValidationDelegate = (securityKey, securityToken, validationParameters, callContext) =>
            {
                BaseConfiguration configuration = null;
                if (validationParameters.ConfigurationManager != null)
                    configuration = validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();

                ValidateSigningKeyCloudInstance(securityKey, configuration);

                // preserve and run provided logic
                if (originalIssuerSigningKeyValidationDelegate != null)
                    return originalIssuerSigningKeyValidationDelegate(securityKey, securityToken, validationParameters, callContext);

                return new ValidatedSignatureKey(DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
            };

            validationParameters.SignatureKeyValidator = cloudInstanceSigningKeyValidationDelegate;
        }

        /// <summary>
        /// Enables the validation of the issuer of the signing keys used by the Microsoft identity platform (AAD) against the issuer of the token.
        /// </summary>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> that are used to validate the token.</param>
        internal static void EnableAadSigningKeyIssuerValidation(this ValidationParameters validationParameters)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            SignatureKeyValidationDelegate issuerSigningKeyValidationDelegate = validationParameters.SignatureKeyValidator;

            validationParameters.SignatureKeyValidator = (securityKey, securityToken, vp, callContext) =>
            {
                BaseConfiguration baseConfiguration = null;
                if (vp.ConfigurationManager != null)
                    baseConfiguration = vp.ConfigurationManager.GetBaseConfigurationAsync(System.Threading.CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();

                AadTokenValidationParametersExtension.ValidateIssuerSigningKey(securityKey, securityToken, baseConfiguration);

                // preserve and run provided logic
                if (issuerSigningKeyValidationDelegate != null)
                    return issuerSigningKeyValidationDelegate(securityKey, securityToken, vp, callContext);

                return ValidateIssuerSigningKeyCertificate(securityKey, validationParameters);
            };
        }

        /// <summary>
        /// Validates the cloud instance of the signing key.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> provided.</param>
        internal static void ValidateSigningKeyCloudInstance(SecurityKey securityKey, BaseConfiguration configuration)
        {
            if (securityKey == null)
                return;

            if (configuration is not OpenIdConnectConfiguration openIdConnectConfiguration)
                return;

            JsonWebKey matchedKeyFromConfig = AadTokenValidationParametersExtension.GetJsonWebKeyBySecurityKey(openIdConnectConfiguration, securityKey);
            if (matchedKeyFromConfig != null && matchedKeyFromConfig.AdditionalData.TryGetValue(AadIssuerValidatorConstants.CloudInstanceNameKey, out object value))
            {
                string signingKeyCloudInstanceName = value as string;
                if (string.IsNullOrWhiteSpace(signingKeyCloudInstanceName))
                    return;

                if (openIdConnectConfiguration.AdditionalData.TryGetValue(AadIssuerValidatorConstants.CloudInstanceNameKey, out object configurationCloudInstanceNameObjectValue))
                {
                    string configurationCloudInstanceName = configurationCloudInstanceNameObjectValue as string;
                    if (string.IsNullOrWhiteSpace(configurationCloudInstanceName))
                        return;

                    if (!string.Equals(signingKeyCloudInstanceName, configurationCloudInstanceName, StringComparison.Ordinal))
                        throw LogHelper.LogExceptionMessage(
                            new SecurityTokenInvalidCloudInstanceException(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX40012,
                                    LogHelper.MarkAsNonPII(signingKeyCloudInstanceName),
                                    LogHelper.MarkAsNonPII(configurationCloudInstanceName)))
                            {
                                ConfigurationCloudInstanceName = configurationCloudInstanceName,
                                SigningKeyCloudInstanceName = signingKeyCloudInstanceName,
                                SigningKey = securityKey,
                            });
                }
            }
        }

        /// <summary>
        /// Validates the issuer signing key certificate.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> that are used to validate the token.</param>
        /// <returns><c>true</c> if the issuer signing key certificate is valid; otherwise, <c>false</c>.</returns>
        private static ValidationResult<ValidatedSignatureKey, ValidationError> ValidateIssuerSigningKeyCertificate(SecurityKey securityKey, ValidationParameters validationParameters)
        {
            if (securityKey == null)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(securityKey), LogMessages.IDX40007));
            }

            return Tokens.Validators.ValidateSignatureKey(securityKey, validationParameters, new CallContext());
        }
    }
}
