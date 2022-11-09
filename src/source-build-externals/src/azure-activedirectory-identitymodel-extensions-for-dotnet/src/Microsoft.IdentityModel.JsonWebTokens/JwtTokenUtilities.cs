//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// A class which contains useful methods for processing JWT tokens.
    /// </summary>
    public class JwtTokenUtilities
    {
        /// <summary>
        /// Regex that is used to figure out if a token is in JWS format.
        /// </summary>
        public static Regex RegexJws = new Regex(JwtConstants.JsonCompactSerializationRegex, RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));

        /// <summary>
        /// Regex that is used to figure out if a token is in JWE format.
        /// </summary>
        public static Regex RegexJwe = new Regex(JwtConstants.JweCompactSerializationRegex, RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));

        internal static IList<string> DefaultHeaderParameters = new List<string>()
        {
            JwtHeaderParameterNames.Alg,
            JwtHeaderParameterNames.Kid,
            JwtHeaderParameterNames.X5t,
            JwtHeaderParameterNames.Enc,
            JwtHeaderParameterNames.Zip
        };

        /// <summary>
        /// Produces a signature over the <paramref name="input"/>.
        /// </summary>
        /// <param name="input">String to be signed</param>
        /// <param name="signingCredentials">The <see cref="SigningCredentials"/> that contain crypto specs used to sign the token.</param>
        /// <returns>The bse64urlendcoded signature over the bytes obtained from UTF8Encoding.GetBytes( 'input' ).</returns>
        /// <exception cref="ArgumentNullException">'input' or 'signingCredentials' is null.</exception>
        public static string CreateEncodedSignature(string input, SigningCredentials signingCredentials)
        {
            if (input == null)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            var cryptoProviderFactory = signingCredentials.CryptoProviderFactory ?? signingCredentials.Key.CryptoProviderFactory;
            var signatureProvider = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm);
            if (signatureProvider == null)
                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10637, signingCredentials.Key == null ? "Null" : signingCredentials.Key.ToString(), LogHelper.MarkAsNonPII(signingCredentials.Algorithm))));

            try
            {
                LogHelper.LogVerbose(LogMessages.IDX14200);
                return Base64UrlEncoder.Encode(signatureProvider.Sign(Encoding.UTF8.GetBytes(input)));
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }

        /// <summary>
        /// Produces a signature over the <paramref name="input"/>.
        /// </summary>
        /// <param name="input">String to be signed</param>
        /// <param name="signingCredentials">The <see cref="SigningCredentials"/> that contain crypto specs used to sign the token.</param>
        /// <param name="cacheProvider">should the <see cref="SignatureProvider"/> be cached.</param>
        /// <returns>The bse64urlendcoded signature over the bytes obtained from UTF8Encoding.GetBytes( 'input' ).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> or <paramref name="signingCredentials"/> is null.</exception>
        public static string CreateEncodedSignature(string input, SigningCredentials signingCredentials, bool cacheProvider)
        {
            if (input == null)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            var cryptoProviderFactory = signingCredentials.CryptoProviderFactory ?? signingCredentials.Key.CryptoProviderFactory;
            var signatureProvider = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm, cacheProvider);
            if (signatureProvider == null)
                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10637, signingCredentials.Key == null ? "Null" : signingCredentials.Key.ToString(), LogHelper.MarkAsNonPII(signingCredentials.Algorithm))));

            try
            {
                LogHelper.LogVerbose(LogHelper.FormatInvariant(LogMessages.IDX14201, LogHelper.MarkAsNonPII(cacheProvider)));
                return Base64UrlEncoder.Encode(signatureProvider.Sign(Encoding.UTF8.GetBytes(input)));
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }

        /// <summary>
        /// Decompress JWT token bytes.
        /// </summary>
        /// <param name="tokenBytes"></param>
        /// <param name="algorithm"></param>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/> is null.</exception>
        /// <exception cref="NotSupportedException">if the decompression <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="SecurityTokenDecompressionFailedException">if decompression using <paramref name="algorithm"/> fails.</exception>
        /// <returns>Decompressed JWT token</returns>
        internal static string DecompressToken(byte[] tokenBytes, string algorithm)
        {
            if (tokenBytes == null)
                throw LogHelper.LogArgumentNullException(nameof(tokenBytes));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (!CompressionProviderFactory.Default.IsSupportedAlgorithm(algorithm))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10682, LogHelper.MarkAsNonPII(algorithm))));

            var compressionProvider = CompressionProviderFactory.Default.CreateCompressionProvider(algorithm);

            var decompressedBytes = compressionProvider.Decompress(tokenBytes);

            return decompressedBytes != null ? Encoding.UTF8.GetString(decompressedBytes) : throw LogHelper.LogExceptionMessage(new SecurityTokenDecompressionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10679, LogHelper.MarkAsNonPII(algorithm))));
        }

        /// <summary>
        /// Decrypts a Json Web Token.
        /// </summary>
        /// <param name="jwtToken">The Json Web Token</param>
        /// <param name="validationParameters">The validation parameters containing cryptographic material.</param>
        /// <param name="decryptionParameters">The decryption parameters container.</param>
        /// <returns>The decrypted, and if the 'zip' claim is set, decompressed string representation of the token.</returns>
        internal static string DecryptJwtToken(
            SecurityToken jwtToken,
            TokenValidationParameters validationParameters,
            JwtTokenDecryptionParameters decryptionParameters)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (decryptionParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(decryptionParameters));

            bool decryptionSucceeded = false;
            bool algorithmNotSupportedByCryptoProvider = false;
            byte[] decryptedTokenBytes = null;

            // keep track of exceptions thrown, keys that were tried
            var exceptionStrings = new StringBuilder();
            var keysAttempted = new StringBuilder();
            foreach (SecurityKey key in decryptionParameters.Keys)
            {
                var cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
                if (cryptoProviderFactory == null)
                {
                    LogHelper.LogWarning(TokenLogMessages.IDX10607, key);
                    continue;
                }

                if (!cryptoProviderFactory.IsSupportedAlgorithm(decryptionParameters.Enc, key))
                {
                    algorithmNotSupportedByCryptoProvider = true;
                    LogHelper.LogWarning(TokenLogMessages.IDX10611, LogHelper.MarkAsNonPII(decryptionParameters.Enc), key);
                    continue;
                }

                try
                {
                    Validators.ValidateAlgorithm(decryptionParameters.Enc, key, jwtToken, validationParameters);
                    decryptedTokenBytes = DecryptToken(cryptoProviderFactory, key, decryptionParameters);
                    X509SecurityKey x509Key = key as X509SecurityKey;
                    if (x509Key != null)
                        LogHelper.LogInformation(TokenLogMessages.IDX10903, x509Key.Certificate?.Thumbprint);

                    decryptionSucceeded = true;
                    break;
                }
                catch (Exception ex)
                {
                    exceptionStrings.AppendLine(ex.ToString());
                }

                if (key != null)
                    keysAttempted.AppendLine(key.ToString());
            }

            ValidateDecryption(decryptionParameters, decryptionSucceeded, algorithmNotSupportedByCryptoProvider, exceptionStrings, keysAttempted);

            if (string.IsNullOrEmpty(decryptionParameters.Zip))
                return Encoding.UTF8.GetString(decryptedTokenBytes);

            try
            {
                return decryptionParameters.DecompressionFunction(decryptedTokenBytes, decryptionParameters.Zip);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecompressionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10679, decryptionParameters.Zip), ex));
            }
        }

        private static void ValidateDecryption(JwtTokenDecryptionParameters decryptionParameters, bool decryptionSucceeded, bool algorithmNotSupportedByCryptoProvider, StringBuilder exceptionStrings, StringBuilder keysAttempted)
        {
            if (!decryptionSucceeded && keysAttempted.Length > 0)
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10603, keysAttempted, exceptionStrings, decryptionParameters.EncodedToken)));

            if (!decryptionSucceeded && algorithmNotSupportedByCryptoProvider)
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10619, LogHelper.MarkAsNonPII(decryptionParameters.Alg), LogHelper.MarkAsNonPII(decryptionParameters.Enc))));

            if (!decryptionSucceeded)
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10609, decryptionParameters.EncodedToken)));
        }

        private static byte[] DecryptToken(CryptoProviderFactory cryptoProviderFactory, SecurityKey key, JwtTokenDecryptionParameters decryptionParameters)
        {
            using (AuthenticatedEncryptionProvider decryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(key, decryptionParameters.Enc))
            {
                if (decryptionProvider == null)
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10610, key, LogHelper.MarkAsNonPII(decryptionParameters.Enc))));

                return decryptionProvider.Decrypt(
                    Base64UrlEncoder.DecodeBytes(decryptionParameters.Ciphertext),
                    Encoding.ASCII.GetBytes(decryptionParameters.EncodedHeader),
                    Base64UrlEncoder.DecodeBytes(decryptionParameters.InitializationVector),
                    Base64UrlEncoder.DecodeBytes(decryptionParameters.AuthenticationTag));
            }
        }

        /// <summary>
        /// Generates key bytes.
        /// </summary>
        public static byte[] GenerateKeyBytes(int sizeInBits)
        {
            byte[] key = null;
            if (sizeInBits != 256 && sizeInBits != 384 && sizeInBits != 512)
                throw LogHelper.LogExceptionMessage(new ArgumentException(TokenLogMessages.IDX10401, nameof(sizeInBits)));

            using (var aes = Aes.Create())
            {
                int halfSizeInBytes = sizeInBits >> 4;
                key = new byte[halfSizeInBytes << 1];
                aes.KeySize = sizeInBits >> 1;
                // The design of AuthenticatedEncryption needs two keys of the same size - generate them, each half size of what's required
                aes.GenerateKey();
                Array.Copy(aes.Key, key, halfSizeInBytes);
                aes.GenerateKey();
                Array.Copy(aes.Key, 0, key, halfSizeInBytes, halfSizeInBytes);
            }

            return key;
        }

        internal static SecurityKey GetSecurityKey(
            EncryptingCredentials encryptingCredentials,
            CryptoProviderFactory cryptoProviderFactory,
            IDictionary<string, object> additionalHeaderClaims,
            out byte[] wrappedKey)
        {
            SecurityKey securityKey = null;
            KeyWrapProvider kwProvider = null;
            wrappedKey = null;

            // if direct algorithm, look for support
            if (JwtConstants.DirectKeyUseAlg.Equals(encryptingCredentials.Alg))
            {
                if (!cryptoProviderFactory.IsSupportedAlgorithm(encryptingCredentials.Enc, encryptingCredentials.Key))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10615, LogHelper.MarkAsNonPII(encryptingCredentials.Enc), encryptingCredentials.Key)));

                securityKey = encryptingCredentials.Key;
            }
#if NET472 || NET6_0
            else if (SupportedAlgorithms.EcdsaWrapAlgorithms.Contains(encryptingCredentials.Alg))
            {
                // on decryption we get the public key from the EPK value see: https://datatracker.ietf.org/doc/html/rfc7518#appendix-C
                string apu = null, apv = null;
                if (additionalHeaderClaims != null && additionalHeaderClaims.Count > 0)
                {
                    if (additionalHeaderClaims.TryGetValue(JwtHeaderParameterNames.Apu, out object objApu))
                        apu = objApu?.ToString();

                    if (additionalHeaderClaims.TryGetValue(JwtHeaderParameterNames.Apv, out object objApv))
                        apv = objApv?.ToString();
                }

                EcdhKeyExchangeProvider ecdhKeyExchangeProvider = new EcdhKeyExchangeProvider(encryptingCredentials.Key as ECDsaSecurityKey, encryptingCredentials.KeyExchangePublicKey, encryptingCredentials.Alg, encryptingCredentials.Enc);
                SecurityKey kdf = ecdhKeyExchangeProvider.GenerateKdf(apu, apv);
                kwProvider = cryptoProviderFactory.CreateKeyWrapProvider(kdf, ecdhKeyExchangeProvider.GetEncryptionAlgorithm());

                // only 128, 384 and 512 AesKeyWrap for CEK algorithm
                if (SecurityAlgorithms.Aes128KW.Equals(kwProvider.Algorithm, StringComparison.Ordinal))
                    securityKey = new SymmetricSecurityKey(GenerateKeyBytes(256));
                else if (SecurityAlgorithms.Aes192KW.Equals(kwProvider.Algorithm, StringComparison.Ordinal))
                    securityKey = new SymmetricSecurityKey(GenerateKeyBytes(384));
                else if (SecurityAlgorithms.Aes256KW.Equals(kwProvider.Algorithm, StringComparison.Ordinal))
                    securityKey = new SymmetricSecurityKey(GenerateKeyBytes(512));
                else
                    throw LogHelper.LogExceptionMessage(
                        new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10617, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes128KW), LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes192KW), LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes256KW), LogHelper.MarkAsNonPII(kwProvider.Algorithm))));

                wrappedKey = kwProvider.WrapKey(((SymmetricSecurityKey)securityKey).Key);
            }
#endif
            else
            {
                if (!cryptoProviderFactory.IsSupportedAlgorithm(encryptingCredentials.Alg, encryptingCredentials.Key))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10615, LogHelper.MarkAsNonPII(encryptingCredentials.Alg), encryptingCredentials.Key)));

                // only 128, 384 and 512 AesCbcHmac for CEK algorithm
                if (SecurityAlgorithms.Aes128CbcHmacSha256.Equals(encryptingCredentials.Enc))
                    securityKey = new SymmetricSecurityKey(GenerateKeyBytes(256));
                else if (SecurityAlgorithms.Aes192CbcHmacSha384.Equals(encryptingCredentials.Enc))
                    securityKey = new SymmetricSecurityKey(GenerateKeyBytes(384));
                else if (SecurityAlgorithms.Aes256CbcHmacSha512.Equals(encryptingCredentials.Enc))
                    securityKey = new SymmetricSecurityKey(GenerateKeyBytes(512));
                else
                    throw LogHelper.LogExceptionMessage(
                        new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10617, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes128CbcHmacSha256), LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes192CbcHmacSha384), LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes256CbcHmacSha512), LogHelper.MarkAsNonPII(encryptingCredentials.Enc))));

                kwProvider = cryptoProviderFactory.CreateKeyWrapProvider(encryptingCredentials.Key, encryptingCredentials.Alg);
                wrappedKey = kwProvider.WrapKey(((SymmetricSecurityKey)securityKey).Key);
            }

            return securityKey;
        }

        /// <summary>
        /// Gets all decryption keys.
        /// </summary>
        public static IEnumerable<SecurityKey> GetAllDecryptionKeys(TokenValidationParameters validationParameters)
        {
            if (validationParameters == null)
                throw new ArgumentNullException(nameof(validationParameters));

            var decryptionKeys = new Collection<SecurityKey>();
            if (validationParameters.TokenDecryptionKey != null)
                decryptionKeys.Add(validationParameters.TokenDecryptionKey);

            if (validationParameters.TokenDecryptionKeys != null)
                foreach (SecurityKey key in validationParameters.TokenDecryptionKeys)
                    decryptionKeys.Add(key);

            return decryptionKeys;

        }


        /// <summary>
        /// Gets the <see cref="DateTime"/> using the number of seconds from 1970-01-01T0:0:0Z (UTC)
        /// </summary>
        /// <param name="key">Claim in the payload that should map to an integer, float, or string.</param>
        /// <param name="payload">The payload that contains the desired claim value.</param>
        /// <remarks>If the claim is not found, the function returns: <see cref="DateTime.MinValue"/>
        /// </remarks>
        /// <exception cref="FormatException">If the value of the claim cannot be parsed into a long.</exception>
        /// <returns>The <see cref="DateTime"/> representation of a claim.</returns>
        internal static DateTime GetDateTime(string key, JObject payload)
        {
            if (!payload.TryGetValue(key, out var jToken))
                return DateTime.MinValue;

            return EpochTime.DateTime(Convert.ToInt64(Math.Truncate(Convert.ToDouble(ParseTimeValue(jToken, key), CultureInfo.InvariantCulture))));
        }

        private static long ParseTimeValue(JToken jToken, string claimName)
        {
            if (jToken.Type == JTokenType.Integer || jToken.Type == JTokenType.Float)
            {
                return (long)jToken;
            }
            else if (jToken.Type == JTokenType.String)
            {
                if (long.TryParse((string)jToken, out long resultLong))
                    return resultLong;

                if (float.TryParse((string)jToken, out float resultFloat))
                    return (long)resultFloat;

                if (double.TryParse((string)jToken, out double resultDouble))
                    return (long)resultDouble;
            }

            throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant(LogMessages.IDX14300, LogHelper.MarkAsNonPII(claimName), jToken.ToString(), LogHelper.MarkAsNonPII(typeof(long)))));
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when validating the signature of a token.
        /// </summary>
        /// <param name="kid">The <see cref="string"/> kid field of the token being validated</param>
        /// <param name="x5t">The <see cref="string"/> x5t field of the token being validated</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> required for validation.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> that will be used along with the <see cref="TokenValidationParameters"/> to resolve the signing key</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then null is returned</remarks>
        internal static SecurityKey ResolveTokenSigningKey(string kid, string x5t, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            if (configuration?.SigningKeys == null)
                return null;

            if (!string.IsNullOrEmpty(kid))
            {
                foreach (SecurityKey signingKey in configuration.SigningKeys)
                {
                    if (signingKey != null && string.Equals(signingKey.KeyId, kid, signingKey is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        return signingKey;
                }
            }

            if (!string.IsNullOrEmpty(x5t))
            {
                foreach (SecurityKey signingKey in configuration.SigningKeys)
                {
                    if (signingKey != null && string.Equals(signingKey.KeyId, x5t))
                        return signingKey;
                }
            }

            return ResolveTokenSigningKey(kid, x5t, validationParameters);
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when validating the signature of a token.
        /// </summary>
        /// <param name="kid">The <see cref="string"/> kid field of the token being validated</param>
        /// <param name="x5t">The <see cref="string"/> x5t field of the token being validated</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/>  required for validation.</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then null is returned</remarks>
        internal static SecurityKey ResolveTokenSigningKey(string kid, string x5t, TokenValidationParameters validationParameters)
        {
            if (!string.IsNullOrEmpty(kid))
            {
                if (validationParameters.IssuerSigningKey != null
                    && string.Equals(validationParameters.IssuerSigningKey.KeyId, kid, validationParameters.IssuerSigningKey is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                    return validationParameters.IssuerSigningKey;

                if (validationParameters.IssuerSigningKeys != null)
                {
                    foreach (SecurityKey signingKey in validationParameters.IssuerSigningKeys)
                    {
                        if (signingKey != null && string.Equals(signingKey.KeyId, kid, signingKey is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        {
                            return signingKey;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(x5t))
            {
                if (validationParameters.IssuerSigningKey != null)
                {
                    if (string.Equals(validationParameters.IssuerSigningKey.KeyId, x5t, validationParameters.IssuerSigningKey is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        return validationParameters.IssuerSigningKey;

                    X509SecurityKey x509Key = validationParameters.IssuerSigningKey as X509SecurityKey;
                    if (x509Key != null && string.Equals(x509Key.X5t, x5t, StringComparison.OrdinalIgnoreCase))
                        return validationParameters.IssuerSigningKey;
                }

                if (validationParameters.IssuerSigningKeys != null)
                {
                    foreach (SecurityKey signingKey in validationParameters.IssuerSigningKeys)
                    {
                        if (signingKey != null && string.Equals(signingKey.KeyId, x5t))
                        {
                            return signingKey;
                        }
                    }
                }
            }

            return null;
        }
    }
}

