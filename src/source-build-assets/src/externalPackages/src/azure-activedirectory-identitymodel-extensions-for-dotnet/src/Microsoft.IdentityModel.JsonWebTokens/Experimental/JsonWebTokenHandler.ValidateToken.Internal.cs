// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

#nullable enable
namespace Microsoft.IdentityModel.JsonWebTokens
{
    public partial class JsonWebTokenHandler : TokenHandler, IResultBasedValidation
    {
        /// <inheritdoc/>
        internal override async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                return ValidationError.NullParameter(
                    nameof(token),
                    ValidationError.GetCurrentStackFrame());
            }

            if (validationParameters is null)
            {
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());
            }

            if (token.Length > MaximumTokenSizeInBytes)
            {
                return new ValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10209,
                        LogHelper.MarkAsNonPII(token.Length),
                        LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes)),
                    ValidationFailureType.SecurityTokenTooLarge,
                    ValidationError.GetCurrentStackFrame());
            }

            ValidationResult<SecurityToken, ValidationError> readResult = ReadToken(token, callContext);
            if (readResult.Succeeded)
            {
                ValidationResult<ValidatedToken, ValidationError> validationResult = await ValidateTokenAsync(
                    readResult.Result!,
                    validationParameters,
                    callContext,
                    cancellationToken)
                    .ConfigureAwait(false);

                if (validationResult.Succeeded)
                    return validationResult; // No need to unwrap and re-wrap the result.

                return validationResult.Error!.AddStackFrame(ValidationError.GetCurrentStackFrame());
            }

            return readResult.Error!.AddCurrentStackFrame();
        }

        /// <inheritdoc/>
        internal override async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            if (token is null)
            {
                return ValidationError.NullParameter(
                    nameof(token),
                    ValidationError.GetCurrentStackFrame());
            }

            if (validationParameters is null)
            {
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());
            }

            if (token is not JsonWebToken jsonWebToken)
            {
                return new ValidationError(
                    new MessageDetail(TokenLogMessages.IDX10001, nameof(token), nameof(JsonWebToken)),
                    ValidationFailureType.SecurityTokenNotExpectedType,
                    ValidationError.GetCurrentStackFrame());
            }

            BaseConfiguration? currentConfiguration =
                await GetCurrentConfigurationAsync(validationParameters, cancellationToken).ConfigureAwait(false);

            ValidationResult<ValidatedToken, ValidationError> result = jsonWebToken.IsEncrypted ?
                await ValidateJWEAsync(jsonWebToken, validationParameters, currentConfiguration, callContext, cancellationToken).ConfigureAwait(false) :
                await ValidateJWSAsync(jsonWebToken, validationParameters, currentConfiguration, callContext, cancellationToken).ConfigureAwait(false);

            if (validationParameters.ConfigurationManager is null)
            {
                if (result.Succeeded)
                    return result;

                return result.Error!.AddStackFrame(ValidationError.GetCurrentStackFrame());
            }

            if (result.Succeeded)
            {
                // Set current configuration as LKG if it exists.
                if (currentConfiguration is not null)
                    validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;

                return result;
            }

            if (TokenUtilities.IsRecoverableFailureType(result.Error!.FailureType, (currentConfiguration != null && currentConfiguration.TokenDecryptionKeys.Count > 0)))
            {
                // If we were still unable to validate, attempt to refresh the configuration and validate using it
                // but ONLY if the currentConfiguration is not null. We want to avoid refreshing the configuration on
                // retrieval error as this case should have already been hit before. This refresh handles the case
                // where a new valid configuration was somehow published during validation time.
                if (currentConfiguration is not null)
                {
                    validationParameters.ConfigurationManager.RequestRefresh();
                    validationParameters.RefreshBeforeValidation = true;
                    BaseConfiguration lastConfig = currentConfiguration;
                    currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);

                    // Only try to re-validate using the newly obtained config if it doesn't reference equal the previously used configuration.
                    if (lastConfig != currentConfiguration)
                    {
                        result = jsonWebToken.IsEncrypted ?
                            await ValidateJWEAsync(jsonWebToken, validationParameters, currentConfiguration, callContext, cancellationToken).ConfigureAwait(false) :
                            await ValidateJWSAsync(jsonWebToken, validationParameters, currentConfiguration, callContext, cancellationToken).ConfigureAwait(false);

                        if (result.Succeeded)
                        {
                            validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;
                            return result;
                        }
                    }
                }

                if (validationParameters.ConfigurationManager.UseLastKnownGoodConfiguration)
                {
                    validationParameters.RefreshBeforeValidation = false;
                    validationParameters.ValidateWithLKG = true;
                    ValidationFailureType failureType = result.Error!.FailureType;

                    BaseConfiguration[] validConfigurations = validationParameters.ConfigurationManager.GetValidLkgConfigurations();
                    for (int i = 0; i < validConfigurations.Length; i++)
                    {
                        BaseConfiguration lkgConfiguration = validConfigurations[i];
                        if (TokenUtilities.IsRecoverableConfigurationAndExceptionType(
                            jsonWebToken.Kid, currentConfiguration, lkgConfiguration, failureType))
                        {
                            result = jsonWebToken.IsEncrypted ?
                                await ValidateJWEAsync(jsonWebToken, validationParameters, lkgConfiguration, callContext, cancellationToken).ConfigureAwait(false) :
                                await ValidateJWSAsync(jsonWebToken, validationParameters, lkgConfiguration, callContext, cancellationToken).ConfigureAwait(false);

                            if (result.Succeeded)
                                return result;
                        }
                    }
                }
            }

            // If we reach this point, the token validation failed and we should return the error.
            return result.Error!.AddCurrentStackFrame();
        }

        private async ValueTask<ValidationResult<ValidatedToken, ValidationError>> ValidateJWEAsync(
            JsonWebToken jwtToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            ValidationResult<string, ValidationError> decryptionResult = DecryptToken(
                jwtToken, validationParameters, configuration, callContext);
            if (!decryptionResult.Succeeded)
            {
                return decryptionResult.Error!.AddCurrentStackFrame();
            }

            ValidationResult<SecurityToken, ValidationError> readResult = ReadToken(decryptionResult.Result!, callContext);
            if (!readResult.Succeeded)
            {
                return readResult.Error!.AddCurrentStackFrame();
            }

            JsonWebToken decryptedToken = (readResult.Result as JsonWebToken)!;
            ValidationResult<ValidatedToken, ValidationError> validationResult =
                await ValidateJWSAsync(decryptedToken!, validationParameters, configuration, callContext, cancellationToken)
                .ConfigureAwait(false);

            if (!validationResult.Succeeded)
            {
                return validationResult.Error!.AddCurrentStackFrame();
            }

            JsonWebToken jsonWebToken = (validationResult.Result!.SecurityToken as JsonWebToken)!;

            jwtToken.InnerToken = jsonWebToken;
            jwtToken.Payload = jsonWebToken.Payload;

            return validationResult;
        }

        private async ValueTask<ValidationResult<ValidatedToken, ValidationError>> ValidateJWSAsync(
            JsonWebToken jsonWebToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            DateTime? expires = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Exp) ? jsonWebToken.ValidTo : null;
            DateTime? notBefore = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Nbf) ? jsonWebToken.ValidFrom : null;

            ValidationResult<ValidatedLifetime, ValidationError> lifetimeResult =
                Validators.ValidateLifetimeInternal(
                    notBefore,
                    expires,
                    jsonWebToken,
                    validationParameters,
                    callContext);

            if (!lifetimeResult.Succeeded)
                return lifetimeResult.Error!.AddCurrentStackFrame();

            if (jsonWebToken.Audiences is not IList<string> tokenAudiences)
                tokenAudiences = [.. jsonWebToken.Audiences];

            ValidationResult<string, ValidationError> audienceResult =
                Validators.ValidateAudienceInternal(
                    tokenAudiences,
                    jsonWebToken,
                    validationParameters,
                    callContext);

            if (!audienceResult.Succeeded)
                return audienceResult.Error!.AddCurrentStackFrame();

            ValidationResult<ValidatedIssuer, ValidationError> issuerResult =
                await Validators.ValidateIssuerInternalAsync(
                    jsonWebToken.Issuer,
                    jsonWebToken,
                    validationParameters,
                    callContext,
                    cancellationToken).ConfigureAwait(false);

            if (!issuerResult.Succeeded)
                return issuerResult.Error!.AddCurrentStackFrame();

            ValidationResult<DateTime?, ValidationError>? tokenReplayResult =
                Validators.ValidateTokenReplayInternal(
                    expires,
                    jsonWebToken.EncodedToken,
                    validationParameters,
                    callContext);

            if (!tokenReplayResult.Value.Succeeded)
                return tokenReplayResult.Value.Error!.AddCurrentStackFrame();

            ValidationResult<ValidatedTokenType, ValidationError> tokenTypeResult =
                Validators.ValidateTokenTypeInternal(
                    jsonWebToken.Typ,
                    jsonWebToken,
                    validationParameters,
                    callContext);

            if (!tokenTypeResult.Succeeded)
                return tokenTypeResult.Error!.AddCurrentStackFrame();

            ValidationResult<string, ValidationError> algorithmResult =
                Validators.ValidateAlgorithmInternal(
                    jsonWebToken.Alg,
                    jsonWebToken,
                    validationParameters,
                    callContext);

            if (!algorithmResult.Succeeded)
                return algorithmResult.Error!.AddCurrentStackFrame();

            // The signature validation delegate is yet to be migrated to ValidationParameters.
            ValidationResult<SecurityKey, ValidationError> signatureResult =
                ValidateSignature(
                    jsonWebToken,
                    validationParameters,
                    configuration,
                    callContext);

            if (!signatureResult.Succeeded)
                return signatureResult.Error!.AddCurrentStackFrame();

            ValidationResult<ValidatedSignatureKey, ValidationError> signatureKeyResult =
                Validators.ValidateSignatureKeyInternal(
                    jsonWebToken.SigningKey,
                    jsonWebToken,
                    validationParameters,
                    callContext);

            if (!signatureKeyResult.Succeeded)
                return signatureKeyResult.Error!.AddCurrentStackFrame();

            // actor validation
            ValidationResult<ValidatedToken, ValidationError>? actorResult = null;
            if (validationParameters.ValidateActor && !string.IsNullOrWhiteSpace(jsonWebToken.Actor))
            {
                ValidationResult<SecurityToken, ValidationError> readResult = ReadToken(jsonWebToken.Actor, callContext);
                if (!readResult.Succeeded)
                    return readResult.Error!.AddCurrentStackFrame();

                if (validationParameters.ActorValidationParameters is null)
                    return ValidationError.NullParameter(
                        nameof(validationParameters.ActorValidationParameters),
                        ValidationError.GetCurrentStackFrame());

                // TODO - what if actor token is encrypted?
                JsonWebToken actorToken = (readResult.Result as JsonWebToken)!;
                actorResult = await ValidateJWSAsync(
                    actorToken,
                    validationParameters.ActorValidationParameters,
                    configuration,
                    callContext,
                    cancellationToken).ConfigureAwait(false);

                if (!actorResult.Value.Succeeded)
                    return actorResult.Value.Error!.AddCurrentStackFrame();
            }

            return new ValidatedToken(jsonWebToken, this, validationParameters)
            {
                ValidatedLifetime = lifetimeResult.Result,
                ValidatedAlgorithm = algorithmResult.Result,
                ValidatedAudience = audienceResult.Result,
                ValidatedIssuer = issuerResult.Result,
                ActorValidationResult = actorResult?.Result,
                ValidatedTokenType = tokenTypeResult.Result,
                ValidatedSignatureKey = signatureResult.Result
            };
        }

        private static async Task<BaseConfiguration?> GetCurrentConfigurationAsync(ValidationParameters validationParameters, CancellationToken cancellationToken)
        {
            BaseConfiguration? currentConfiguration = null;
            if (validationParameters.ConfigurationManager is not null)
            {
                try
                {
                    currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(cancellationToken).ConfigureAwait(false);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    // The exception is tracked and dismissed as the ValidationParameters may have the issuer
                    // and signing key set directly on them, allowing the library to continue with token validation.
                    // TODO: Move to CallContext.
                    //if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    //    LogHelper.LogWarning(LogHelper.FormatInvariant(TokenLogMessages.IDX10261, validationParameters.ConfigurationManager.MetadataAddress, ex.ToString()));
                }
            }

            return currentConfiguration;
        }

        #region Explicit Interface Implementations
        async Task<ValidationResult<ValidatedToken, ValidationError>> IResultBasedValidation.ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return await ValidateTokenAsync(
                token,
                validationParameters,
                callContext,
                default).ConfigureAwait(false);
        }

        async Task<ValidationResult<ValidatedToken, ValidationError>> IResultBasedValidation.ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await ValidateTokenAsync(
                token,
                validationParameters,
                callContext,
                cancellationToken).ConfigureAwait(false);
        }

        async Task<ValidationResult<ValidatedToken, ValidationError>> IResultBasedValidation.ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return await ValidateTokenAsync(
                token,
                validationParameters,
                callContext,
                default).ConfigureAwait(false);
        }

        async Task<ValidationResult<ValidatedToken, ValidationError>> IResultBasedValidation.ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await ValidateTokenAsync(
                token,
                validationParameters,
                callContext,
                cancellationToken).ConfigureAwait(false);
        }
        #endregion
    }
}
#nullable restore
