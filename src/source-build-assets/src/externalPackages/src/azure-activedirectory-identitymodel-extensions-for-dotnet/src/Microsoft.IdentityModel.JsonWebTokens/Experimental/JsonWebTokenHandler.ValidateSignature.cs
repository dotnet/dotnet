// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

#nullable enable
namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <remarks>This partial class contains methods and logic related to the validation of tokens' signatures.</remarks>
    public partial class JsonWebTokenHandler : TokenHandler
    {
        /// <summary>
        /// Validates the JWT signature.
        /// </summary>
        /// <param name="jwtToken">The JWT token to validate.</param>
        /// <param name="validationParameters">The parameters used for validation.</param>
        /// <param name="configuration">The optional configuration used for validation.</param>
        /// <param name="callContext">The context in which the method is called.</param>
        /// <returns>A <see cref="ValidationResult{SecurityKey, ValidationError}"/> with the <see cref="SecurityKey"/> that signed the tokenif valid or a <see cref="ValidationError"/>.</returns>
        internal ValidationResult<SecurityKey, ValidationError> ValidateSignature(
            JsonWebToken jwtToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            if (jwtToken is null)
                return ValidationError.NullParameter(
                    nameof(jwtToken),
                    ValidationError.GetCurrentStackFrame());

            if (validationParameters is null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            // Delegate is set by the user, we call it and return the result.
            if (validationParameters.SignatureValidator is not null)
            {
                try
                {
                    ValidationResult<SecurityKey, ValidationError> signatureValidationResult =
                        validationParameters.SignatureValidator(
                            jwtToken,
                            validationParameters,
                            configuration,
                            callContext);

                    return signatureValidationResult;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return new SignatureValidationError(
                        new MessageDetail(TokenLogMessages.IDX10272),
                        SignatureValidationFailure.ValidatorThrew,
                        ValidationError.GetCurrentStackFrame(),
                        ex);
                }
            }

            // If the user wants to accept unsigned tokens, they must implement the delegate.
            if (!jwtToken.IsSigned)
                return new SignatureValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10504,
                        LogHelper.MarkAsSecurityArtifact(
                            jwtToken.EncodedToken,
                            JwtTokenUtilities.SafeLogJwtToken)),
                    SignatureValidationFailure.TokenIsNotSigned,
                    ValidationError.GetCurrentStackFrame());

            SecurityKey? key = null;
            if (validationParameters.SignatureKeyResolver is not null)
            {
                key = validationParameters.SignatureKeyResolver(
                    jwtToken.EncodedToken,
                    jwtToken,
                    jwtToken.Kid,
                    validationParameters,
                    configuration,
                    callContext);
            }
            else
            {
                // Resolve the key using the token's 'kid' and 'x5t' headers.
                // Fall back to the validation parameters' keys if configuration keys are not set.
                key = JwtTokenUtilities.ResolveTokenSigningKey(jwtToken.Kid, jwtToken.X5t, configuration?.SigningKeys)
                    ?? JwtTokenUtilities.ResolveTokenSigningKey(jwtToken.Kid, jwtToken.X5t, validationParameters.SigningKeys);
            }

            if (key is not null)
                return ValidateSignatureWithKey(jwtToken, validationParameters, key, callContext);

            if (validationParameters.TryAllSigningKeys)
                return ValidateSignatureUsingAllKeys(jwtToken, validationParameters, configuration, callContext);

            RecordSignatureValidationTelemetry(
                TelemetryClient,
                TelemetryConstants.SignatureValidationErrors.SigningKeyNotFound,
                jwtToken,
                key: null);

            // kid was NOT found, no matching keys available.
            if (string.IsNullOrEmpty(jwtToken.Kid))
            {
                return new SignatureValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10526,
                        LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                        LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                        LogHelper.MarkAsSecurityArtifact(jwtToken.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)),
                    SignatureValidationFailure.SigningKeyNotFound,
                    ValidationError.GetCurrentStackFrame());
            }

            // kid was found, no matching keys available.
            return new SignatureValidationError(
                new MessageDetail(
                    TokenLogMessages.IDX10527,
                    LogHelper.MarkAsNonPII(jwtToken.Kid),
                    LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                    LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                    LogHelper.MarkAsSecurityArtifact(jwtToken.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)),
                SignatureValidationFailure.SigningKeyNotFound,
                ValidationError.GetCurrentStackFrame());
        }

        private ValidationResult<SecurityKey, ValidationError> ValidateSignatureUsingAllKeys(
            JsonWebToken jwtToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            bool keysTried = false;
            bool kidExists = !string.IsNullOrEmpty(jwtToken.Kid);

            IEnumerable<SecurityKey>? keys = TokenUtilities.GetAllSigningKeys(configuration, validationParameters, callContext);
            StringBuilder? exceptionStrings = null;
            StringBuilder? keysAttempted = null;

            foreach (SecurityKey key in keys)
            {
                if (key is null)
                    continue; // skip null keys

                keysTried = true;

                // Validate the signature with each key.
                ValidationResult<SecurityKey, ValidationError> result = ValidateSignatureWithKey(
                    jwtToken,
                    validationParameters,
                    key,
                    callContext);

                if (result.Succeeded)
                    return result;

                if (result.Error is ValidationError validationError)
                {
                    exceptionStrings ??= new StringBuilder();
                    keysAttempted ??= new StringBuilder();
                    exceptionStrings.AppendLine(validationError.MessageDetail.Message);
                    keysAttempted.AppendLine(key.ToString());
                }
            }

            if (keysTried)
            {
                if (kidExists)
                {
                    return new SignatureValidationError(
                        new MessageDetail(
                            TokenLogMessages.IDX10522,
                            LogHelper.MarkAsNonPII(jwtToken.Kid),
                            LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                            LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                            LogHelper.MarkAsSecurityArtifact(jwtToken.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)),
                        SignatureValidationFailure.SigningKeyNotFound,
                        ValidationError.GetCurrentStackFrame());
                }
                else
                {
                    return new SignatureValidationError(
                        new MessageDetail(
                            TokenLogMessages.IDX10523,
                            LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                            LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                            LogHelper.MarkAsSecurityArtifact(jwtToken.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)),
                        SignatureValidationFailure.SigningKeyNotFound,
                        ValidationError.GetCurrentStackFrame());
                }
            }

            if (kidExists)
            {
                // No keys were attempted, return the error.
                // This is the case where the user specified a kid, but no keys were found.
                // This is not an error, but a warning that no keys were found for the specified kid.
                return new SignatureValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10524,
                        LogHelper.MarkAsNonPII(jwtToken.Kid),
                        LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                        LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                        LogHelper.MarkAsSecurityArtifact(jwtToken.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)),
                    SignatureValidationFailure.SigningKeyNotFound,
                    ValidationError.GetCurrentStackFrame());
            }

            return new SignatureValidationError(
                new MessageDetail(
                    TokenLogMessages.IDX10525,
                    LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                    LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                    LogHelper.MarkAsSecurityArtifact(jwtToken.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)),
                SignatureValidationFailure.SigningKeyNotFound,
                ValidationError.GetCurrentStackFrame());
        }

        private ValidationResult<SecurityKey, ValidationError> ValidateSignatureWithKey(
            JsonWebToken jsonWebToken,
            ValidationParameters validationParameters,
            SecurityKey key,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext)
#pragma warning restore CA1801 // Review unused parameters
        {
            CryptoProviderFactory cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
            if (!cryptoProviderFactory.IsSupportedAlgorithm(jsonWebToken.Alg, key))
            {
                RecordSignatureValidationTelemetry(
                    TelemetryClient,
                    TelemetryConstants.SignatureValidationErrors.AlgorithmNotSupported,
                    jsonWebToken,
                    key);

                return new SignatureValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10652,
                        LogHelper.MarkAsNonPII(jsonWebToken.Alg),
                        key),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    ValidationError.GetCurrentStackFrame());
            }

            SignatureProvider signatureProvider = cryptoProviderFactory.CreateForVerifying(key, jsonWebToken.Alg);
            try
            {
                if (signatureProvider == null)
                {
                    RecordSignatureValidationTelemetry(
                        TelemetryClient,
                        TelemetryConstants.SignatureValidationErrors.SignatureProviderCreationFailed,
                        jsonWebToken,
                        key);

                    return new SignatureValidationError(
                        new MessageDetail(
                            TokenLogMessages.IDX10636,
                            LogHelper.MarkAsNonPII(key?.KeyId ?? "Null"),
                            LogHelper.MarkAsNonPII(jsonWebToken.Alg)),
                        ValidationFailureType.CryptoProviderReturnedNull,
                        ValidationError.GetCurrentStackFrame());
                }

                bool valid = EncodingUtils.PerformEncodingDependentOperation<bool, string, int, SignatureProvider>(
                    jsonWebToken.EncodedToken,
                    0,
                    jsonWebToken.Dot2,
                    Encoding.UTF8,
                    jsonWebToken.EncodedToken,
                    jsonWebToken.Dot2,
                    signatureProvider,
                    ValidateSignature);

                if (valid)
                {
                    RecordSignatureValidationTelemetry(
                        TelemetryClient,
                        TelemetryConstants.SignatureValidationErrors.None,
                        jsonWebToken,
                        key);

                    jsonWebToken.SigningKey = key;
                    return key;
                }
                else
                {
                    RecordSignatureValidationTelemetry(
                        TelemetryClient,
                        TelemetryConstants.SignatureValidationErrors.SignatureVerificationFailed,
                        jsonWebToken,
                        key);

                    return new SignatureValidationError(
                        new MessageDetail(
                            TokenLogMessages.IDX10520,
                            LogHelper.MarkAsNonPII(key.ToString())),
                        SignatureValidationFailure.ValidationFailed,
                        ValidationError.GetCurrentStackFrame());
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                RecordSignatureValidationTelemetry(
                    TelemetryClient,
                    TelemetryConstants.SignatureValidationErrors.SignatureVerificationFailed,
                    jsonWebToken,
                    key);

                return new SignatureValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10521,
                        LogHelper.MarkAsNonPII(key.ToString()),
                        LogHelper.MarkAsNonPII(ex.Message)),
                    SignatureValidationFailure.ValidationFailed,
                    ValidationError.GetCurrentStackFrame(),
                    ex);
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }

        private static void PopulateFailedResults(
            KeyMatchFailedResult? failedResult,
            StringBuilder exceptionStrings,
            StringBuilder keysAttempted)
        {
            if (failedResult is KeyMatchFailedResult result)
            {
                for (int i = 0; i < result.KeysAttempted.Count; i++)
                {
                    exceptionStrings.AppendLine(result.FailedResults[i].MessageDetail.Message);
                    keysAttempted.AppendLine(result.KeysAttempted[i].ToString());
                }
            }
        }

        private struct KeyMatchFailedResult(
            IList<ValidationError> failedResults,
            IList<SecurityKey> keysAttempted)
        {
            public IList<ValidationError> FailedResults = failedResults;
            public IList<SecurityKey> KeysAttempted = keysAttempted;
        }
    }
}
#nullable restore
