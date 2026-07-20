
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// A class which contains useful methods for processing saml tokens.
    /// </summary>
    internal partial class SamlTokenUtilities
    {
        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when validating the signature of a token.
        /// </summary>
        /// <param name="keyInfo">The <see cref="KeyInfo"/> field of the token being validated</param>
        /// <param name="signingKeys">A collection of <see cref="SecurityKey"/> a signing key to be resolved from.</param>
        /// <returns>A <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then <see langword="null"/> is returned.</remarks>
        internal static SecurityKey ResolveTokenSigningKey(KeyInfo keyInfo, IEnumerable<SecurityKey> signingKeys)
        {
            if (keyInfo == null)
                return null;

            if (signingKeys == null)
                return null;

            foreach (SecurityKey signingKey in signingKeys)
            {
                if (signingKey != null)
                {
                    if (keyInfo.MatchesKey(signingKey))
                        return signingKey;
                }
            }

            return null;
        }

        internal static string SafeLogSamlToken(object obj)
        {
            if (obj == null)
                return string.Empty;

            // TODO - remove signature, if present, from the token.
            // For now, we just return the type of the object.
            return obj.GetType().ToString();
        }

        private static void RecordSignatureValidationTelemetry(
            Microsoft.IdentityModel.Telemetry.ITelemetryClient telemetryClient,
            string errorType,
            SecurityToken securityToken,
            string algorithm,
            SecurityKey key)
        {
            if (CryptoTelemetry.RecordSignatureValidationTelemetry)
            {
                telemetryClient.IncrementSignatureValidationCounter(
                    errorType,
                    securityToken.Issuer,
                    algorithm,
                    key);
            }
        }

        internal static ValidationResult<SecurityKey, ValidationError> ValidateSignature(
            SecurityToken securityToken,
            Signature signature,
            string canonicalString,
            ValidationParameters validationParameters,
            BaseConfiguration configuration,
            CallContext callContext,
            Microsoft.IdentityModel.Telemetry.ITelemetryClient telemetryClient)
        {
            if (securityToken is null)
            {
                return ValidationError.NullParameter(
                    nameof(securityToken),
                    ValidationError.GetCurrentStackFrame());
            }

            if (validationParameters is null)
            {
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());
            }

            // Delegate is set by the user, we call it and return the result.
            if (validationParameters.SignatureValidator is not null)
            {
                try
                {
                    return validationParameters.SignatureValidator(
                        securityToken,
                        validationParameters,
                        configuration,
                        callContext);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return new SignatureValidationError(
                        new MessageDetail(Tokens.LogMessages.IDX10272),
                        SignatureValidationFailure.ValidatorThrew,
                        ValidationError.GetCurrentStackFrame(),
                        ex);
                }
            }

            // If the user wants to accept unsigned tokens, they must set validationParameters.SignatureValidator
            if (signature is null)
                return new SignatureValidationError(
                    new MessageDetail(
                        Tokens.LogMessages.IDX10504,
                        LogHelper.MarkAsSecurityArtifact(
                            canonicalString,
                            SamlTokenUtilities.SafeLogSamlToken)),
                    SignatureValidationFailure.TokenIsNotSigned,
                    ValidationError.GetCurrentStackFrame());

            SecurityKey key = null;
            if (validationParameters.SignatureKeyResolver is not null)
            {
                key = validationParameters.SignatureKeyResolver(
                    canonicalString,
                    securityToken,
                    signature.KeyInfo?.Id,
                    validationParameters,
                    configuration,
                    callContext);
            }
            else
            {
                key = ResolveTokenSigningKey(signature.KeyInfo, configuration?.SigningKeys)
                    ?? ResolveTokenSigningKey(signature.KeyInfo, validationParameters.SigningKeys);
            }

            using (var memoryStream = new MemoryStream())
            {
                signature.SignedInfo.GetCanonicalBytes(memoryStream);
                byte[] canonicalBytes = memoryStream.ToArray();
                byte[] signatureValueBytes = Convert.FromBase64String(signature.SignatureValue);

                if (key is not null)
                {
                    return ValidateSignatureWithKey(
                        signatureValueBytes,
                        canonicalBytes,
                        securityToken,
                        signature,
                        validationParameters,
                        key,
                        callContext,
                        telemetryClient);

                }
                else if (validationParameters.TryAllSigningKeys)
                    return ValidateSignatureUsingAllKeys(
                        canonicalBytes,
                        signatureValueBytes,
                        securityToken,
                        signature,
                        canonicalString,
                        validationParameters,
                        configuration,
                        callContext,
                        telemetryClient);
            }

            if (signature.KeyInfo == null)
            {
                return new SignatureValidationError(
                    new MessageDetail(
                        Tokens.LogMessages.IDX10526,
                        LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                        LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                        LogHelper.MarkAsSecurityArtifact(canonicalString, SafeLogSamlToken)),
                    SignatureValidationFailure.SigningKeyNotFound,
                    ValidationError.GetCurrentStackFrame());
            }

            // If the kid was specified, but no key was found, return an error.
            return new SignatureValidationError(
                new MessageDetail(
                    Tokens.LogMessages.IDX10527,
                    LogHelper.MarkAsNonPII(signature.KeyInfo.Id),
                    LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                    LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                    LogHelper.MarkAsSecurityArtifact(canonicalString, SafeLogSamlToken)),
                SignatureValidationFailure.SigningKeyNotFound,
                ValidationError.GetCurrentStackFrame());
        }

        private static ValidationResult<SecurityKey, ValidationError> ValidateSignatureWithKey(
            byte[] signatureBytes,
            byte[] canonicalBytes,
            SecurityToken securityToken,
            Signature signature,
            ValidationParameters validationParameters,
            SecurityKey key,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext,
#pragma warning restore CA1801 // Review unused parameters
            Microsoft.IdentityModel.Telemetry.ITelemetryClient telemetryClient)
        {
            // TODO - this is not an AlgorithmValidationFailure, but a CryptoProviderFactory failure.
            // TODO we need tests across token handlers
            CryptoProviderFactory cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
            if (!cryptoProviderFactory.IsSupportedAlgorithm(signature.SignedInfo.SignatureMethod, key))
            {
                return new SignatureValidationError(
                    new MessageDetail(
                        Tokens.LogMessages.IDX10652,
                        LogHelper.MarkAsNonPII(signature.SignedInfo.SignatureMethod),
                        key),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    ValidationError.GetCurrentStackFrame());
            }

            SignatureProvider signatureProvider = cryptoProviderFactory.CreateForVerifying(key, signature.SignedInfo.SignatureMethod);
            try
            {
                if (signatureProvider == null)
                    return new SignatureValidationError(
                        new MessageDetail(
                            Tokens.LogMessages.IDX10636,
                            LogHelper.MarkAsNonPII(key?.KeyId ?? "Null"),
                            LogHelper.MarkAsNonPII(signature.SignedInfo.SignatureMethod)),
                        ValidationFailureType.CryptoProviderReturnedNull,
                        ValidationError.GetCurrentStackFrame());

                if (!signatureProvider.Verify(canonicalBytes, signatureBytes))
                {
                    RecordSignatureValidationTelemetry(
                        telemetryClient,
                        TelemetryConstants.SignatureValidationErrors.SignatureVerificationFailed,
                        securityToken,
                        signature.SignedInfo.SignatureMethod,
                        key);

                    return new SignatureValidationError(
                        new MessageDetail(
                            Tokens.LogMessages.IDX10520,
                            LogHelper.MarkAsNonPII(key.ToString())),
                        SignatureValidationFailure.ValidationFailed,
                        ValidationError.GetCurrentStackFrame());
                }

                var result = signature.SignedInfo.Verify(cryptoProviderFactory, callContext);
                if (result == null)
                {
                    RecordSignatureValidationTelemetry(
                        telemetryClient,
                        TelemetryConstants.SignatureValidationErrors.None,
                        securityToken,
                        signature.SignedInfo.SignatureMethod,
                        key);

                    securityToken.SigningKey = key;
                    return key;
                }

                RecordSignatureValidationTelemetry(
                    telemetryClient,
                    TelemetryConstants.SignatureValidationErrors.SignatureVerificationFailed,
                    securityToken,
                    signature.SignedInfo.SignatureMethod,
                    key);

                return result;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                RecordSignatureValidationTelemetry(
                    telemetryClient,
                    TelemetryConstants.SignatureValidationErrors.SignatureVerificationFailed,
                    securityToken,
                    signature.SignedInfo.SignatureMethod,
                    key);

                return new SignatureValidationError(
                    new MessageDetail(
                        Tokens.LogMessages.IDX10521,
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

        private static ValidationResult<SecurityKey, ValidationError> ValidateSignatureUsingAllKeys(
            byte[] signatureValueBytes,
            byte[] canonicalBytes,
            SecurityToken securityToken,
            Signature signature,
            string canonicalString,
            ValidationParameters validationParameters,
            BaseConfiguration configuration,
            CallContext callContext,
            Microsoft.IdentityModel.Telemetry.ITelemetryClient telemetryClient)
        {
            bool keysTried = false;
            bool kidExists = !string.IsNullOrEmpty(signature?.KeyInfo?.Id);

            IEnumerable<SecurityKey> keys = TokenUtilities.GetAllSigningKeys(configuration, validationParameters, callContext);
            StringBuilder exceptionStrings = null;
            StringBuilder keysAttempted = null;

            foreach (SecurityKey key in keys)
            {
                if (key is null)
                    continue; // skip null keys

                keysTried = true;

                ValidationResult<SecurityKey, ValidationError> result = ValidateSignatureWithKey(
                    signatureValueBytes,
                    canonicalBytes,
                    securityToken,
                    signature,
                    validationParameters,
                    key,
                    callContext,
                    telemetryClient);

                if (result.Succeeded)
                {
                    securityToken.SigningKey = key;
                    return result;
                }

                if (result.Error is SignatureValidationError signatureValidationError)
                {
                    exceptionStrings ??= new StringBuilder();
                    keysAttempted ??= new StringBuilder();
                    exceptionStrings.AppendLine(signatureValidationError.MessageDetail.Message);
                    keysAttempted.AppendLine(key.ToString());
                }
            }

            if (keysTried)
            {
                if (kidExists)
                {
                    return new SignatureValidationError(
                        new MessageDetail(
                            Tokens.LogMessages.IDX10522,
                            LogHelper.MarkAsNonPII(signature?.KeyInfo?.Id),
                            LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                            LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                            LogHelper.MarkAsSecurityArtifact(canonicalString, SafeLogSamlToken)),
                        SignatureValidationFailure.SigningKeyNotFound,
                        ValidationError.GetCurrentStackFrame());
                }
                else
                {
                    return new SignatureValidationError(
                        new MessageDetail(
                            Tokens.LogMessages.IDX10523,
                            LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                            LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                            LogHelper.MarkAsSecurityArtifact(canonicalString, SafeLogSamlToken)),
                        SignatureValidationFailure.SigningKeyNotFound,
                        ValidationError.GetCurrentStackFrame());
                }
            }

            if (kidExists)
            {
                return new SignatureValidationError(
                    new MessageDetail(
                        Tokens.LogMessages.IDX10526,
                        LogHelper.MarkAsNonPII(signature?.KeyInfo?.Id),
                        LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                        LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                        LogHelper.MarkAsSecurityArtifact(canonicalString, SafeLogSamlToken)),
                    SignatureValidationFailure.SigningKeyNotFound,
                    ValidationError.GetCurrentStackFrame());
            }

            return new SignatureValidationError(
                new MessageDetail(
                    Tokens.LogMessages.IDX10525,
                    LogHelper.MarkAsNonPII(validationParameters.SigningKeys.Count),
                    LogHelper.MarkAsNonPII(configuration?.SigningKeys?.Count ?? 0),
                    LogHelper.MarkAsSecurityArtifact(canonicalString, SafeLogSamlToken)),
                SignatureValidationFailure.SigningKeyNotFound,
                ValidationError.GetCurrentStackFrame());
        }
    }
}
