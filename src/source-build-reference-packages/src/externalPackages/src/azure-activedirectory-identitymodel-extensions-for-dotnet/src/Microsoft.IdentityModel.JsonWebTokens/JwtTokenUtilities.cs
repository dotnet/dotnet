// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;

using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// A class which contains useful methods for processing JWT tokens.
    /// </summary>
    public partial class JwtTokenUtilities
    {
        private const int _regexMatchTimeoutMilliseconds = 100;
        private const string _unrecognizedEncodedToken = "UnrecognizedEncodedToken";

        /// <summary>
        /// Regex that is used to figure out if a token is in JWS format.
        /// </summary>
        public static Regex RegexJws = CreateJwsRegex();

        /// <summary>
        /// Regex that is used to figure out if a token is in JWE format.
        /// </summary>
        public static Regex RegexJwe = CreateJweRegex();

#if NET7_0_OR_GREATER
        [GeneratedRegex(JwtConstants.JsonCompactSerializationRegex, RegexOptions.CultureInvariant, _regexMatchTimeoutMilliseconds)]
        private static partial Regex CreateJwsRegex();
        [GeneratedRegex(JwtConstants.JweCompactSerializationRegex, RegexOptions.CultureInvariant, _regexMatchTimeoutMilliseconds)]
        private static partial Regex CreateJweRegex();
#else
        private static Regex CreateJwsRegex() => new Regex(JwtConstants.JsonCompactSerializationRegex, RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(_regexMatchTimeoutMilliseconds));
        private static Regex CreateJweRegex() => new Regex(JwtConstants.JweCompactSerializationRegex, RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(_regexMatchTimeoutMilliseconds));
#endif

        internal static List<string> DefaultHeaderParameters = new List<string>()
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
        /// <param name="input">The value to be signed.</param>
        /// <param name="signingCredentials">The <see cref="SigningCredentials"/> that contain crypto specs used to sign the token.</param>
        /// <returns>The base 64 url encoded signature over the bytes obtained from UTF8Encoding.GetBytes( 'input' ).</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> or <paramref name="signingCredentials"/> is null.</exception>
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
        /// <param name="input">The value to be signed.</param>
        /// <param name="signingCredentials">The <see cref="SigningCredentials"/> that contain crypto specs used to sign the token.</param>
        /// <param name="cacheProvider">Indicates whether the <see cref="SignatureProvider"/> should be cached.</param>
        /// <returns>The base 64 url encoded signature over the bytes obtained from UTF8Encoding.GetBytes( 'input' ).</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> or <paramref name="signingCredentials"/> is null.</exception>
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
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogHelper.FormatInvariant(LogMessages.IDX14201, LogHelper.MarkAsNonPII(cacheProvider)));

                return Base64UrlEncoder.Encode(signatureProvider.Sign(Encoding.UTF8.GetBytes(input)));
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }

        internal static byte[] CreateEncodedSignature(
            byte[] input,
            int offset,
            int count,
            SigningCredentials signingCredentials)
        {
            if (input == null)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signingCredentials == null)
                return null;

            var cryptoProviderFactory = signingCredentials.CryptoProviderFactory ?? signingCredentials.Key.CryptoProviderFactory;
            var signatureProvider = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm) ??
                throw LogHelper.LogExceptionMessage(
                    new InvalidOperationException(
                        LogHelper.FormatInvariant(
                            TokenLogMessages.IDX10637,
                            signingCredentials.Key == null ? "Null" : signingCredentials.Key.ToString(),
                            LogHelper.MarkAsNonPII(signingCredentials.Algorithm))));

            try
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogMessages.IDX14200);

                return signatureProvider.Sign(input, offset, count);
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Produces a signature over the <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The <see cref="ReadOnlySpan{Byte}"/> containing the bytes to be signed.</param>
        /// <param name="destination">destination for signature.</param>
        /// <param name="signingCredentials">The <see cref="SigningCredentials"/> that contain crypto specs used to sign the token.</param>
        /// <param name="bytesWritten">The number of bytes actually written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the signature was successfully written to <paramref name="destination"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signingCredentials"/> is null.</exception>
        internal static bool CreateSignature(
            ReadOnlySpan<byte> data,
            Span<byte> destination,
            SigningCredentials signingCredentials,
            out int bytesWritten)
        {
            bytesWritten = 0;
            if (signingCredentials == null)
                return false;

            var cryptoProviderFactory = signingCredentials.CryptoProviderFactory ?? signingCredentials.Key.CryptoProviderFactory;
            var signatureProvider = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm) ??
                throw LogHelper.LogExceptionMessage(
                    new InvalidOperationException(
                        LogHelper.FormatInvariant(
                            TokenLogMessages.IDX10637, signingCredentials.Key == null ? "Null" : signingCredentials.Key.ToString(),
                            LogHelper.MarkAsNonPII(signingCredentials.Algorithm))));

            try
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogMessages.IDX14200);

                return signatureProvider.Sign(data, destination, out bytesWritten);
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }
#endif

        /// <summary>
        /// Decompress JWT token bytes.
        /// </summary>
        /// <param name="tokenBytes">The JWT token bytes to be decompressed.</param>
        /// <param name="algorithm">The algorithm used for decompression.</param>
        /// <param name="maximumDeflateSize">The maximum allowable size for the decompressed data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="tokenBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null.</exception>
        /// <exception cref="NotSupportedException">Thrown if the decompression <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="SecurityTokenDecompressionFailedException">Thrown if decompression using <paramref name="algorithm"/> fails.</exception>
        /// <returns>The decompressed JWT token.</returns>
        internal static string DecompressToken(byte[] tokenBytes, string algorithm, int maximumDeflateSize)
        {
            if (tokenBytes == null)
                throw LogHelper.LogArgumentNullException(nameof(tokenBytes));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (!CompressionProviderFactory.Default.IsSupportedAlgorithm(algorithm))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10682, LogHelper.MarkAsNonPII(algorithm))));

            var compressionProvider = CompressionProviderFactory.Default.CreateCompressionProvider(algorithm, maximumDeflateSize);

            var decompressedBytes = compressionProvider.Decompress(tokenBytes);

            return decompressedBytes != null ? Encoding.UTF8.GetString(decompressedBytes) : throw LogHelper.LogExceptionMessage(new SecurityTokenDecompressionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10679, LogHelper.MarkAsNonPII(algorithm))));
        }

        /// <summary>
        /// Decrypts a JWT token.
        /// </summary>
        /// <param name="securityToken">The JWT token, could be a JwtSecurityToken or JsonWebToken.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <param name="decryptionParameters">The decryption parameters container.</param>
        /// <returns>The decrypted, and if the 'zip' claim is set, decompressed string representation of the token.</returns>
        internal static string DecryptJwtToken(
            SecurityToken securityToken,
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
            StringBuilder exceptionStrings = null;
            StringBuilder keysAttempted = null;
            string zipAlgorithm = null;
            foreach (SecurityKey key in decryptionParameters.Keys)
            {
                var cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
                if (cryptoProviderFactory == null)
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Warning))
                        LogHelper.LogWarning(TokenLogMessages.IDX10607, key);

                    continue;
                }

                try
                {
                    // The JsonWebTokenHandler will set the JsonWebToken and those values will be used.
                    // The JwtSecurityTokenHandler will calculate values and set the values on DecrytionParameters.

                    // JsonWebToken from JsonWebTokenHandler
                    if (securityToken is JsonWebToken jsonWebToken)
                    {
                        if (!cryptoProviderFactory.IsSupportedAlgorithm(jsonWebToken.Enc, key))
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Warning))
                                LogHelper.LogWarning(TokenLogMessages.IDX10611, LogHelper.MarkAsNonPII(decryptionParameters.Enc), key);

                            algorithmNotSupportedByCryptoProvider = true;
                            continue;
                        }

                        Validators.ValidateAlgorithm(jsonWebToken.Enc, key, securityToken, validationParameters);
                        decryptedTokenBytes = DecryptToken(
                            cryptoProviderFactory,
                            key,
                            jsonWebToken.Enc,
                            jsonWebToken.CipherTextBytes,
                            jsonWebToken.HeaderAsciiBytes,
                            jsonWebToken.InitializationVectorBytes,
                            jsonWebToken.AuthenticationTagBytes);

                        zipAlgorithm = jsonWebToken.Zip;
                        decryptionSucceeded = true;
                        break;
                    }
                    // JwtSecurityToken from JwtSecurityTokenHandler
                    else
                    {
                        if (!cryptoProviderFactory.IsSupportedAlgorithm(decryptionParameters.Enc, key))
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Warning))
                                LogHelper.LogWarning(TokenLogMessages.IDX10611, LogHelper.MarkAsNonPII(decryptionParameters.Enc), key);

                            algorithmNotSupportedByCryptoProvider = true;
                            continue;
                        }

                        Validators.ValidateAlgorithm(decryptionParameters.Enc, key, securityToken, validationParameters);
                        decryptedTokenBytes = DecryptToken(
                            cryptoProviderFactory,
                            key,
                            decryptionParameters.Enc,
                            decryptionParameters.CipherTextBytes,
                            decryptionParameters.HeaderAsciiBytes,
                            decryptionParameters.InitializationVectorBytes,
                            decryptionParameters.AuthenticationTagBytes);

                        zipAlgorithm = decryptionParameters.Zip;
                        decryptionSucceeded = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    (exceptionStrings ??= new StringBuilder()).AppendLine(ex.ToString());
                }

                if (key != null)
                    (keysAttempted ??= new StringBuilder()).AppendLine(key.ToString());
            }

            ValidateDecryption(decryptionParameters, decryptionSucceeded, algorithmNotSupportedByCryptoProvider, exceptionStrings, keysAttempted);
            try
            {
                if (string.IsNullOrEmpty(zipAlgorithm))
                    return Encoding.UTF8.GetString(decryptedTokenBytes);

                return decryptionParameters.DecompressionFunction(decryptedTokenBytes, zipAlgorithm, decryptionParameters.MaximumDeflateSize);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecompressionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10679, zipAlgorithm), ex));
            }
        }

        private static void ValidateDecryption(JwtTokenDecryptionParameters decryptionParameters, bool decryptionSucceeded, bool algorithmNotSupportedByCryptoProvider, StringBuilder exceptionStrings, StringBuilder keysAttempted)
        {
            if (!decryptionSucceeded && keysAttempted is not null)
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10603, keysAttempted, (object)exceptionStrings ?? "", LogHelper.MarkAsSecurityArtifact(decryptionParameters.EncodedToken, SafeLogJwtToken))));

            if (!decryptionSucceeded && algorithmNotSupportedByCryptoProvider)
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10619, LogHelper.MarkAsNonPII(decryptionParameters.Alg), LogHelper.MarkAsNonPII(decryptionParameters.Enc))));

            if (!decryptionSucceeded)
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10609, LogHelper.MarkAsSecurityArtifact(decryptionParameters.EncodedToken, SafeLogJwtToken))));
        }

        private static byte[] DecryptToken(CryptoProviderFactory cryptoProviderFactory, SecurityKey key, string encAlg, byte[] ciphertext, byte[] headerAscii, byte[] initializationVector, byte[] authenticationTag)
        {
            using (AuthenticatedEncryptionProvider decryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(key, encAlg))
            {
                if (decryptionProvider == null)
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10610, key, LogHelper.MarkAsNonPII(encAlg))));

                return decryptionProvider.Decrypt(
                    ciphertext,
                    headerAscii,
                    initializationVector,
                    authenticationTag);
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
#if NET472 || NET6_0_OR_GREATER
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

        internal static string SafeLogJwtToken(object obj)
        {
            if (obj == null)
                return string.Empty;

            // not a string, we do not know how to sanitize so we return a String which represents the object instance
            if (!(obj is string token))
                return obj.GetType().ToString();

            int lastDot = token.LastIndexOf(".");

            // no dots, not a JWT, we do not know how to sanitize so we return UnrecognizedEncodedToken
            if (lastDot == -1)
                return _unrecognizedEncodedToken;

            return token.Substring(0, lastDot);
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when validating the signature of a token.
        /// </summary>
        /// <param name="kid">The <see cref="string"/> kid field of the token being validated.</param>
        /// <param name="x5t">The <see cref="string"/> x5t field of the token being validated.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> that will be used along with the <see cref="TokenValidationParameters"/> to resolve the signing key.</param>
        /// <returns>A <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>Resolve the signing key using configuration then the validationParameters until a key is resolved. If key fails to resolve, then null is returned.</remarks>
        internal static SecurityKey ResolveTokenSigningKey(string kid, string x5t, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            return ResolveTokenSigningKey(kid, x5t, configuration?.SigningKeys) ?? ResolveTokenSigningKey(kid, x5t, ConcatSigningKeys(validationParameters));
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when validating the signature of a token.
        /// </summary>
        /// <param name="kid">The <see cref="string"/> kid field of the token being validated.</param>
        /// <param name="x5t">The <see cref="string"/> x5t field of the token being validated.</param>
        /// <param name="signingKeys">A collection of <see cref="SecurityKey"/> a signing key to be resolved from.</param>
        /// <returns>A <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then <see langword="null"/> is returned.</remarks>
        internal static SecurityKey ResolveTokenSigningKey(string kid, string x5t, IEnumerable<SecurityKey> signingKeys)
        {
            if (signingKeys == null)
                return null;

            foreach (SecurityKey signingKey in signingKeys)
            {
                if (signingKey != null)
                {
                    if (signingKey is X509SecurityKey x509Key)
                    {
                        if ((!string.IsNullOrEmpty(kid) && string.Equals(signingKey.KeyId, kid, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(x5t) && string.Equals(x509Key.X5t, x5t, StringComparison.OrdinalIgnoreCase)))
                        {
                            return signingKey;
                        }
                    }
                    else if (!string.IsNullOrEmpty(signingKey.KeyId))
                    {
                        if (string.Equals(signingKey.KeyId, kid) || string.Equals(signingKey.KeyId, x5t))
                        {
                            return signingKey;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Counts the number of JWT token segments.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <param name="maxCount">The maximum number of segments to count up to.</param>
        /// <returns>The number of segments up to <paramref name="maxCount"/>.</returns>
        internal static int CountJwtTokenPart(string token, int maxCount)
        {
            var count = 1;
            var index = 0;
            while (index < token.Length)
            {
                var dotIndex = token.IndexOf('.', index);
                if (dotIndex < 0)
                {
                    break;
                }
                count++;
                index = dotIndex + 1;
                if (count == maxCount)
                {
                    break;
                }
            }
            return count;
        }

        internal static IEnumerable<SecurityKey> ConcatSigningKeys(TokenValidationParameters tvp)
        {
            if (tvp == null)
                yield break;

            yield return tvp.IssuerSigningKey;
            if (tvp.IssuerSigningKeys != null)
            {
                foreach (var key in tvp.IssuerSigningKeys)
                {
                    yield return key;
                }
            }
        }

        // If a string is in IS8061 format, assume a DateTime is in UTC
        // Because this is a friend class, we can't remove this method without
        // breaking compatibility.
        internal static string GetStringClaimValueType(string str)
        {
            return GetStringClaimValueType(str, string.Empty);
        }

        internal static string GetStringClaimValueType(string str, string claimType)
        {
            if (!string.IsNullOrEmpty(claimType) && !AppContextSwitches.TryAllStringClaimsAsDateTime && JsonSerializerPrimitives.IsKnownToNotBeDateTime(claimType))
                return ClaimValueTypes.String;

            if (DateTime.TryParse(str, out DateTime dateTimeValue))
            {
                string dtUniversal = dateTimeValue.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
                if (dtUniversal.Equals(str, StringComparison.Ordinal))
                    return ClaimValueTypes.DateTime;
            }

            return ClaimValueTypes.String;
        }
    }
}

