// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

#nullable enable
namespace Microsoft.IdentityModel.JsonWebTokens
{
    public partial class JsonWebTokenHandler : TokenHandler
    {
        /// <summary>
        /// Decrypts a JWE using the keys from <paramref name="validationParameters"/> and returns the clear text.
        /// </summary>
        /// <param name="jwtToken">The JWE that contains the cypher text.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> to be used for validating the token.</param>
        /// <param name="callContext">A <see cref="CallContext"/> that contains call information.</param>
        /// <returns>An <see cref="ValidationResult{TResult, TError}"/> with ValidationResult.Result containing the clear text or a <see cref="ValidationError"/>.</returns>
        internal ValidationResult<string, ValidationError> DecryptToken(
            JsonWebToken jwtToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext? callContext)
        {
            if (jwtToken == null)
            {
                return ValidationError.NullParameter(
                    nameof(jwtToken),
                    ValidationError.GetCurrentStackFrame());
            }

            if (validationParameters == null)
            {
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());
            }

            if (string.IsNullOrEmpty(jwtToken.Enc))
            {
                return new ValidationError(
                    new MessageDetail(TokenLogMessages.IDX10612),
                    ValidationFailureType.TokenDecryptionFailed,
                    ValidationError.GetCurrentStackFrame());
            }

            (IList<SecurityKey>? ContentEncryptionKeys, ValidationError? ValidationError) result =
                GetContentEncryptionKeys(jwtToken, validationParameters, configuration, callContext);

            if (result.ValidationError != null)
                return result.ValidationError.AddCurrentStackFrame();

            if (result.ContentEncryptionKeys == null || result.ContentEncryptionKeys.Count == 0)
            {
                return new ValidationError(
                    new MessageDetail(
                        TokenLogMessages.IDX10609,
                        LogHelper.MarkAsSecurityArtifact(jwtToken, JwtTokenUtilities.SafeLogJwtToken)),
                    ValidationFailureType.TokenDecryptionFailed,
                    ValidationError.GetCurrentStackFrame());
            }

            var decryptionParameters = CreateJwtTokenDecryptionParameters(jwtToken, result.ContentEncryptionKeys);

            return JwtTokenUtilities.DecryptJwtToken(
                jwtToken,
                validationParameters,
                decryptionParameters,
                callContext);
        }

        internal (IList<SecurityKey>?, ValidationError?) GetContentEncryptionKeys(
            JsonWebToken jwtToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext? callContext)
        {
            IList<SecurityKey>? keys = null;

            // First we check to see if the caller has set a custom decryption resolver on VP for the call, if so any keys set on VP and keys in Configuration are ignored.
            // If no custom decryption resolver is set, we'll check to see if they've set some static decryption keys on VP. If a key is found, we ignore configuration.
            // If no key found in VP, we'll check the configuration.
            if (validationParameters.DecryptionKeyResolver != null)
            {
                keys = validationParameters.DecryptionKeyResolver(jwtToken.EncodedToken, jwtToken, jwtToken.Kid, validationParameters, callContext);
            }
            else
            {
                var key = ResolveTokenDecryptionKey(jwtToken, validationParameters, callContext);
                if (key is null && configuration is not null)
                {
                    key = ResolveTokenDecryptionKeyFromConfig(jwtToken, configuration);
                }

                if (key is not null)
                    keys = [key];
            }

            // on decryption for ECDH-ES, we get the public key from the EPK value see: https://datatracker.ietf.org/doc/html/rfc7518#appendix-C
            // we need the ECDSASecurityKey for the receiver, use TokenValidationParameters.TokenDecryptionKey

            // control gets here if:
            // 1. User specified delegate: DecryptionKeyResolver returned null
            // 2. ResolveTokenDecryptionKey returned null
            // 3. ResolveTokenDecryptionKeyFromConfig returned null
            // Try all the keys. This is the degenerate case, not concerned about perf.
            if (validationParameters.TryAllDecryptionKeys && keys.IsNullOrEmpty())
            {
                keys = validationParameters.DecryptionKeys;
                if (configuration != null)
                {
                    if (configuration.TokenDecryptionKeys is not List<SecurityKey> configurationKeys)
                        configurationKeys = configuration.TokenDecryptionKeys.ToList();

                    if (keys != null)
                    {
                        keys = keys.Concat(configurationKeys).ToList();
                    }
                    else
                        keys = configurationKeys;
                }

            }

            if (jwtToken.Alg.Equals(JwtConstants.DirectKeyUseAlg, StringComparison.Ordinal)
                || jwtToken.Alg.Equals(SecurityAlgorithms.EcdhEs, StringComparison.Ordinal))
                return (keys, null);

            if (keys is null)
                return (keys, null); // Cannot iterate over null.

            var unwrappedKeys = new List<SecurityKey>();

            // Pre-generate a placeholder key used as a fallback when an unwrap call
            // does not yield a usable key, so that downstream processing follows a
            // single uniform path regardless of which key was tried.
            int expectedCekSizeInBytes = GetExpectedCekSizeInBytes(jwtToken.Enc);
            byte[] fallbackCek = new byte[expectedCekSizeInBytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(fallbackCek);

            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];

                try
                {
#if NET472 || NET6_0_OR_GREATER
                    if (SupportedAlgorithms.EcdsaWrapAlgorithms.Contains(jwtToken.Alg))
                    {
                        // on decryption we get the public key from the EPK value see: https://datatracker.ietf.org/doc/html/rfc7518#appendix-C
                        jwtToken.TryGetHeaderValue(JwtHeaderParameterNames.Epk, out string epk);
                        ECDsaSecurityKey? publicKey = new ECDsaSecurityKey(new JsonWebKey(epk), false);
                        if (publicKey is not null)
                        {
                            var ecdhKeyExchangeProvider = new EcdhKeyExchangeProvider(
                                key as ECDsaSecurityKey,
                                publicKey,
                                jwtToken.Alg,
                                jwtToken.Enc);
                            jwtToken.TryGetHeaderValue(JwtHeaderParameterNames.Apu, out string apu);
                            jwtToken.TryGetHeaderValue(JwtHeaderParameterNames.Apv, out string apv);
                            SecurityKey kdf = ecdhKeyExchangeProvider.GenerateKdf(apu, apv);
                            var kwp = key.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(kdf, ecdhKeyExchangeProvider.GetEncryptionAlgorithm());
                            var unwrappedKey = kwp.UnwrapKey(Base64UrlEncoder.DecodeBytes(jwtToken.EncryptedKey));
                            unwrappedKeys.Add(new SymmetricSecurityKey(unwrappedKey));
                        }
                    }
                    else
#endif
#pragma warning disable IDE0055 // Fix formatting
                    if (key.CryptoProviderFactory.IsSupportedAlgorithm(jwtToken.Alg, key))
                    {
                        var kwp = key.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(key, jwtToken.Alg);
                        var unwrappedKey = kwp.UnwrapKey(jwtToken.EncryptedKeyBytes);
                        unwrappedKeys.Add(new SymmetricSecurityKey(unwrappedKey));
                    }
#pragma warning restore IDE0055 // Fix formatting
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Warning))
                        LogHelper.LogWarning(ex.ToString());

                    // Use the placeholder key so the downstream code path is the
                    // same regardless of whether unwrap succeeded.
                    unwrappedKeys.Add(new SymmetricSecurityKey(fallbackCek));
                }
            }

            return (unwrappedKeys, null);
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when decrypting a JWE.
        /// </summary>
        /// <param name="jwtToken">The <see cref="JsonWebToken"/> that is being decrypted.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The call context used for logging.</param>
        /// <returns>A <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then null is returned.</remarks>
        internal virtual SecurityKey? ResolveTokenDecryptionKey(
            JsonWebToken jwtToken,
            ValidationParameters validationParameters,
            CallContext? callContext)
        {
            if (jwtToken == null || validationParameters == null)
                return null;

            if (!string.IsNullOrEmpty(jwtToken.Kid) && validationParameters.DecryptionKeys != null)
            {
                for (int i = 0; i < validationParameters.DecryptionKeys.Count; i++)
                {
                    var key = validationParameters.DecryptionKeys[i];
                    if (key != null && string.Equals(key.KeyId, jwtToken.Kid, GetStringComparisonRuleIf509OrECDsa(key)))
                        return key;
                }
            }

            if (!string.IsNullOrEmpty(jwtToken.X5t) && validationParameters.DecryptionKeys != null)
            {
                for (int i = 0; i < validationParameters.DecryptionKeys.Count; i++)
                {
                    SecurityKey? key = validationParameters.DecryptionKeys[i];

                    if (key != null && string.Equals(key.KeyId, jwtToken.X5t, GetStringComparisonRuleIf509(key)))
                        return key;

                    if (key is X509SecurityKey x509Key && string.Equals(x509Key.X5t, jwtToken.X5t, StringComparison.OrdinalIgnoreCase))
                        return key;
                }
            }

            return null;
        }
    }
}
#nullable restore
