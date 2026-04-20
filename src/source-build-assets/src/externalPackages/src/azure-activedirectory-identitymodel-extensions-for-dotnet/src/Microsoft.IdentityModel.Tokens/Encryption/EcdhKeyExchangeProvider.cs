// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
#if NET472 || NET6_0_OR_GREATER
    /// <summary>
    /// Provides a Security Key that can be used as Content Encryption Key (CEK) for use with a JWE
    /// </summary>
    public class EcdhKeyExchangeProvider
    {
        /// <summary>
        /// Number of bits in the desired output key
        /// </summary>
        public int KeyDataLen { get; set; }

        private ECDiffieHellman _ecdhPublic;
        private ECDiffieHellman _ecdhPrivate;
        private ECParameters _ecParamsPublic;
        private ECParameters _ecParamsPrivate;
        private string _algorithmId;

        /// <summary>
        /// Initializes a new instance of <see cref="EcdhKeyExchangeProvider"/> used for CEKs
        /// <param name="privateKey">The <see cref="SecurityKey"/> that will be used for cryptographic operations and represents the private key.</param>
        /// <param name="publicKey">The <see cref="SecurityKey"/> that will be used for cryptographic operations and represents the public key.</param>
        /// <param name="alg">alg header parameter value.</param>
        /// <param name="enc">enc header parameter value.</param>
        /// </summary>
        public EcdhKeyExchangeProvider(SecurityKey privateKey, SecurityKey publicKey, string alg, string enc)
        {
            if (privateKey == null)
                throw LogHelper.LogArgumentNullException(nameof(privateKey));

            if (publicKey is null)
                throw LogHelper.LogArgumentNullException(nameof(publicKey));

            ValidateAlgAndEnc(alg, enc);
            SetKeyDataLenAndEncryptionAlgorithm(alg, enc);
            _ecParamsPublic = GetECParametersFromKey(publicKey, false, nameof(publicKey));
            _ecParamsPrivate = GetECParametersFromKey(privateKey, true, nameof(privateKey));
            ValidateCurves(nameof(privateKey), nameof(publicKey));
            _ecdhPublic = ECDiffieHellman.Create(_ecParamsPublic);
            _ecdhPrivate = ECDiffieHellman.Create(_ecParamsPrivate);
        }

        /// <summary>
        /// Generates the KDF
        /// </summary>
        /// <param name="apu">Agreement PartyUInfo (optional). When used, the PartyVInfo value contains information about the producer,
        /// represented as a base64url-encoded string.</param>
        /// <param name="apv">Agreement PartyVInfo (optional). When used, the PartyUInfo value contains information about the recipient,
        /// represented as a base64url-encoded string.</param>
        /// <returns>Returns <see cref="SecurityKey"/> that represents the key generated</returns>
        public SecurityKey GenerateKdf(string apu = null, string apv = null)
        {
            //The "apu" and "apv" values MUST be distinct when used (per rfc7518 section 4.6.2) https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.2
            if (!string.IsNullOrEmpty(apu) && !string.IsNullOrEmpty(apv) && apu.Equals(apv))
                throw LogHelper.LogArgumentException<ArgumentException>(
                    nameof(apu),
                    LogHelper.FormatInvariant(
                        LogMessages.IDX11001,
                        LogHelper.MarkAsNonPII(nameof(apu)),
                        LogHelper.MarkAsNonPII(apu),
                        LogHelper.MarkAsNonPII(nameof(apv)),
                        LogHelper.MarkAsNonPII(apv))
                    );

            int kdfLength = KeyDataLen / 8; // number of octets
            // prepend bytes that represent n = ceiling of (keydatalen / hashlen), see section 5.8.1.1: https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
            // hashlen is always 256 for ecdh-es, see: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.2
            // for supported algorithms it is always '1', for saml might be different
            byte[] prepend = new byte[4] { 0, 0, 0, 1 };
            SetAppendBytes(apu, apv, out byte[] append);
            byte[] kdf = new byte[kdfLength];

            // JWA's spec https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.2 specifies SHA256, saml might be different
            byte[] derivedKey = _ecdhPrivate.DeriveKeyFromHash(_ecdhPublic.PublicKey, HashAlgorithmName.SHA256, prepend, append);
            Array.Copy(derivedKey, kdf, kdfLength);

            return new SymmetricSecurityKey(kdf);
        }

        private void SetAppendBytes(string apu, string apv, out byte[] append)
        {
            byte[] encBytes = Encoding.ASCII.GetBytes(_algorithmId);
            byte[] apuBytes = Base64UrlEncoder.DecodeBytes(string.IsNullOrEmpty(apu) ? string.Empty : apu);
            byte[] apvBytes = Base64UrlEncoder.DecodeBytes(string.IsNullOrEmpty(apv) ? string.Empty : apv);
            byte[] numOctetsEnc = BitConverter.GetBytes(encBytes.Length);
            byte[] numOctetsApu = BitConverter.GetBytes(apuBytes.Length);
            byte[] numOctetsApv = BitConverter.GetBytes(apvBytes.Length);
            byte[] keyDataLengthBytes = BitConverter.GetBytes(KeyDataLen);

            if (BitConverter.IsLittleEndian)
            {
                // these representations need to be big-endian
                Array.Reverse(numOctetsEnc);
                Array.Reverse(numOctetsApu);
                Array.Reverse(numOctetsApv);
                Array.Reverse(keyDataLengthBytes);
            }

            append = Concat(numOctetsEnc, encBytes, numOctetsApu, apuBytes, numOctetsApv, apvBytes, keyDataLengthBytes);
        }

        private void SetKeyDataLenAndEncryptionAlgorithm(string alg, string enc = null)
        {
            if (SecurityAlgorithms.EcdhEs.Equals(alg, StringComparison.InvariantCulture))
            {
                _algorithmId = enc;
                if (SecurityAlgorithms.Aes128Gcm.Equals(enc, StringComparison.InvariantCulture))
                    KeyDataLen = 128;
                else if (SecurityAlgorithms.Aes192Gcm.Equals(enc, StringComparison.InvariantCulture))
                    KeyDataLen = 192;
                else if (SecurityAlgorithms.Aes256Gcm.Equals(enc, StringComparison.InvariantCulture))
                    KeyDataLen = 256;
                else if (SecurityAlgorithms.Aes128CbcHmacSha256.Equals(enc, StringComparison.InvariantCulture))
                    KeyDataLen = 128;
                else if (SecurityAlgorithms.Aes192CbcHmacSha384.Equals(enc, StringComparison.InvariantCulture))
                    KeyDataLen = 192;
                else if (SecurityAlgorithms.Aes256CbcHmacSha512.Equals(enc, StringComparison.InvariantCulture))
                    KeyDataLen = 256;
            }
            else
            {
                _algorithmId = alg;

                if (SecurityAlgorithms.EcdhEsA128kw.Equals(alg, StringComparison.InvariantCulture))
                    KeyDataLen = 128;
                else if (SecurityAlgorithms.EcdhEsA192kw.Equals(alg, StringComparison.InvariantCulture))
                    KeyDataLen = 192;
                else if (SecurityAlgorithms.EcdhEsA256kw.Equals(alg, StringComparison.InvariantCulture))
                    KeyDataLen = 256;
            }
        }

        private static void ValidateAlgAndEnc(string alg, string enc)
        {
            if (string.IsNullOrEmpty(alg))
                throw LogHelper.LogArgumentNullException(alg);
            if (string.IsNullOrEmpty(enc))
                throw LogHelper.LogArgumentNullException(enc);

            if (!SupportedAlgorithms.EcdsaWrapAlgorithms.Contains(alg) && !SecurityAlgorithms.EcdhEs.Equals(alg, StringComparison.InvariantCulture))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(alg))));

            if (!SupportedAlgorithms.SymmetricEncryptionAlgorithms.Contains(enc))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10715, LogHelper.MarkAsNonPII(enc))));
        }

        private void ValidateCurves(string privateKeyArgName, string publicKeyArgName)
        {
            if (_ecParamsPrivate.Curve.Equals(_ecParamsPublic.Curve))
            {
                throw LogHelper.LogArgumentException<ArgumentException>(
                    privateKeyArgName,
                    LogHelper.FormatInvariant(
                        LogMessages.IDX11000,
                        LogHelper.MarkAsNonPII(privateKeyArgName),
                        LogHelper.MarkAsNonPII(_ecParamsPrivate.Curve.ToString()),
                        LogHelper.MarkAsNonPII(publicKeyArgName),
                        LogHelper.MarkAsNonPII(_ecParamsPublic.Curve.ToString()))
                    );
            }
        }

        private static ECParameters GetECParametersFromKey(SecurityKey key, bool isPrivate, string nameOfKey)
        {
            if (key is ECDsaSecurityKey ecdsaKey)
            {
                return ecdsaKey.ECDsa.ExportParameters(isPrivate);
            }
            else if (key is JsonWebKey jwk
                && JsonWebKeyConverter.TryConvertToECDsaSecurityKey(jwk, out SecurityKey securityKey))
            {
                return ((ECDsaSecurityKey)securityKey).ECDsa.ExportParameters(isPrivate);
            }
            else
            {
                throw LogHelper.LogArgumentException<ArgumentException>(
                    nameOfKey,
                    LogHelper.FormatInvariant(LogMessages.IDX11002, LogHelper.MarkAsNonPII(nameOfKey)));
            }
        }

        private static byte[] Concat(params byte[][] arrays)
        {
            int outputLength = 0;
            foreach (byte[] arr in arrays)
                outputLength += arr.Length;

            byte[] output = new byte[outputLength];
            int x = 0;
            foreach (byte[] arr in arrays)
            {
                Array.Copy(arr, 0, output, x, arr.Length);
                x += arr.Length;
            }

            return output;
        }

        internal string GetEncryptionAlgorithm()
        {
            if (_algorithmId.Equals(SecurityAlgorithms.EcdhEsA128kw, StringComparison.Ordinal))
                return SecurityAlgorithms.Aes128KW;
            if (_algorithmId.Equals(SecurityAlgorithms.EcdhEsA192kw, StringComparison.Ordinal))
                return SecurityAlgorithms.Aes192KW;
            if (_algorithmId.Equals(SecurityAlgorithms.EcdhEsA256kw, StringComparison.Ordinal))
                return SecurityAlgorithms.Aes256KW;
            return _algorithmId;
        }
    }
#endif
}
