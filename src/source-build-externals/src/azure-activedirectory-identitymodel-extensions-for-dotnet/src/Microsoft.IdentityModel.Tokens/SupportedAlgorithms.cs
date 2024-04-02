// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Defines the default set of algorithms this library supports
    /// </summary>
    internal static class SupportedAlgorithms
    {
        private const int RsaMinKeySize = 2048;

        internal static readonly ICollection<string> EcdsaSigningAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.EcdsaSha256,
            SecurityAlgorithms.EcdsaSha256Signature,
            SecurityAlgorithms.EcdsaSha384,
            SecurityAlgorithms.EcdsaSha384Signature,
            SecurityAlgorithms.EcdsaSha512,
            SecurityAlgorithms.EcdsaSha512Signature
        };

        internal static readonly ICollection<string> HashAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.Sha256,
            SecurityAlgorithms.Sha256Digest,
            SecurityAlgorithms.Sha384,
            SecurityAlgorithms.Sha384Digest,
            SecurityAlgorithms.Sha512,
            SecurityAlgorithms.Sha512Digest
        };

        // doubles as RsaKeyWrapAlgorithms
        internal static readonly ICollection<string> RsaEncryptionAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.RsaOAEP,
            SecurityAlgorithms.RsaPKCS1,
            SecurityAlgorithms.RsaOaepKeyWrap
        };

        internal static readonly ICollection<string> RsaSigningAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.RsaSha256,
            SecurityAlgorithms.RsaSha256Signature,
            SecurityAlgorithms.RsaSha384,
            SecurityAlgorithms.RsaSha384Signature,
            SecurityAlgorithms.RsaSha512,
            SecurityAlgorithms.RsaSha512Signature
        };

        internal static readonly ICollection<string> RsaPssSigningAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.RsaSsaPssSha256,
            SecurityAlgorithms.RsaSsaPssSha256Signature,
            SecurityAlgorithms.RsaSsaPssSha384,
            SecurityAlgorithms.RsaSsaPssSha384Signature,
            SecurityAlgorithms.RsaSsaPssSha512,
            SecurityAlgorithms.RsaSsaPssSha512Signature
        };

        internal static readonly ICollection<string> SymmetricEncryptionAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.Aes128CbcHmacSha256,
            SecurityAlgorithms.Aes192CbcHmacSha384,
            SecurityAlgorithms.Aes256CbcHmacSha512,
            SecurityAlgorithms.Aes128Gcm,
            SecurityAlgorithms.Aes192Gcm,
            SecurityAlgorithms.Aes256Gcm
        };

        internal static readonly ICollection<string> SymmetricKeyWrapAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.Aes128KW,
            SecurityAlgorithms.Aes128KeyWrap,
            SecurityAlgorithms.Aes192KW,
            SecurityAlgorithms.Aes192KeyWrap,
            SecurityAlgorithms.Aes256KW,
            SecurityAlgorithms.Aes256KeyWrap,
            SecurityAlgorithms.EcdhEsA128kw,
            SecurityAlgorithms.EcdhEsA192kw,
            SecurityAlgorithms.EcdhEsA256kw
        };

        internal static readonly ICollection<string> SymmetricSigningAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.HmacSha256,
            SecurityAlgorithms.HmacSha256Signature,
            SecurityAlgorithms.HmacSha384,
            SecurityAlgorithms.HmacSha384Signature,
            SecurityAlgorithms.HmacSha512,
            SecurityAlgorithms.HmacSha512Signature
        };

        internal static readonly ICollection<string> EcdsaWrapAlgorithms = new Collection<string>
        {
            SecurityAlgorithms.EcdhEsA128kw,
            SecurityAlgorithms.EcdhEsA192kw,
            SecurityAlgorithms.EcdhEsA256kw
        };

#if NET461 || NET462 || NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER
        /// <summary>
        /// Creating a Signature requires the use of a <see cref="HashAlgorithm"/>.
        /// This method returns the <see cref="HashAlgorithmName"/>
        /// that describes the <see cref="HashAlgorithm"/>to use when generating a Signature.
        /// </summary>
        /// <param name="algorithm">The SignatureAlgorithm in use.</param>
        /// <returns>The <see cref="HashAlgorithmName"/> to use.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="algorithm"/> is not supported.</exception>
        internal static HashAlgorithmName GetHashAlgorithmName(string algorithm)
        {
            if (string.IsNullOrWhiteSpace(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            switch (algorithm)
            {
                case SecurityAlgorithms.EcdsaSha256:
                case SecurityAlgorithms.EcdsaSha256Signature:
                case SecurityAlgorithms.RsaSha256:
                case SecurityAlgorithms.RsaSha256Signature:
                case SecurityAlgorithms.RsaSsaPssSha256:
                case SecurityAlgorithms.RsaSsaPssSha256Signature:
                    return HashAlgorithmName.SHA256;

                case SecurityAlgorithms.EcdsaSha384:
                case SecurityAlgorithms.EcdsaSha384Signature:
                case SecurityAlgorithms.RsaSha384:
                case SecurityAlgorithms.RsaSha384Signature:
                case SecurityAlgorithms.RsaSsaPssSha384:
                case SecurityAlgorithms.RsaSsaPssSha384Signature:
                    return HashAlgorithmName.SHA384;

                case SecurityAlgorithms.EcdsaSha512:
                case SecurityAlgorithms.EcdsaSha512Signature:
                case SecurityAlgorithms.RsaSha512:
                case SecurityAlgorithms.RsaSha512Signature:
                case SecurityAlgorithms.RsaSsaPssSha512:
                case SecurityAlgorithms.RsaSsaPssSha512Signature:
                    return HashAlgorithmName.SHA512;
            }

            throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(algorithm), LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm))));
        }
#endif

        /// <summary>
        /// Creating a Signature requires the use of a <see cref="HashAlgorithm"/>.
        /// This method returns the HashAlgorithm string that is associated with a SignatureAlgorithm.
        /// </summary>
        /// <param name="algorithm">The SignatureAlgorithm of interest.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/>is null or whitespace.</exception>
        /// <exception cref="ArgumentException">if <paramref name="algorithm"/> is not supported.</exception>
        internal static string GetDigestFromSignatureAlgorithm(string algorithm)
        {
            if (string.IsNullOrWhiteSpace(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            switch (algorithm)
            {
                case SecurityAlgorithms.EcdsaSha256:
                case SecurityAlgorithms.HmacSha256:
                case SecurityAlgorithms.RsaSha256:
                case SecurityAlgorithms.RsaSsaPssSha256:
                    return SecurityAlgorithms.Sha256;

                case SecurityAlgorithms.EcdsaSha256Signature:
                case SecurityAlgorithms.HmacSha256Signature:
                case SecurityAlgorithms.RsaSha256Signature:
                case SecurityAlgorithms.RsaSsaPssSha256Signature:
                    return SecurityAlgorithms.Sha256Digest;

                case SecurityAlgorithms.EcdsaSha384:
                case SecurityAlgorithms.HmacSha384:
                case SecurityAlgorithms.RsaSha384:
                case SecurityAlgorithms.RsaSsaPssSha384:
                    return SecurityAlgorithms.Sha384;

                case SecurityAlgorithms.EcdsaSha384Signature:
                case SecurityAlgorithms.HmacSha384Signature:
                case SecurityAlgorithms.RsaSha384Signature:
                case SecurityAlgorithms.RsaSsaPssSha384Signature:
                    return SecurityAlgorithms.Sha384Digest;

                case SecurityAlgorithms.EcdsaSha512:
                case SecurityAlgorithms.HmacSha512:
                case SecurityAlgorithms.RsaSha512:
                case SecurityAlgorithms.RsaSsaPssSha512:
                    return SecurityAlgorithms.Sha512;

                case SecurityAlgorithms.EcdsaSha512Signature:
                case SecurityAlgorithms.HmacSha512Signature:
                case SecurityAlgorithms.RsaSha512Signature:
                case SecurityAlgorithms.RsaSsaPssSha512Signature:
                    return SecurityAlgorithms.Sha512Digest;
            }

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm)), nameof(algorithm)));
        }

        /// <summary>
        /// Checks if an 'algorithm, key' pair is supported.
        /// </summary>
        /// <param name="algorithm">the algorithm to check.</param>
        /// <param name="key">the <see cref="SecurityKey"/>.</param>
        /// <returns>true if 'algorithm, key' pair is supported.</returns>
        public static bool IsSupportedAlgorithm(string algorithm, SecurityKey key)
        {
            if (key as RsaSecurityKey != null)
                return IsSupportedRsaAlgorithm(algorithm, key);

            if (key is X509SecurityKey x509Key)
            {
                // only RSA keys are supported
                if (x509Key.PublicKey as RSA == null)
                    return false;

                return IsSupportedRsaAlgorithm(algorithm, key);
            }

            if (key is JsonWebKey jsonWebKey)
            {
                if (JsonWebAlgorithmsKeyTypes.RSA.Equals(jsonWebKey.Kty))
                    return IsSupportedRsaAlgorithm(algorithm, key);
                else if (JsonWebAlgorithmsKeyTypes.EllipticCurve.Equals(jsonWebKey.Kty))
                    return IsSupportedEcdsaAlgorithm(algorithm);
                else if (JsonWebAlgorithmsKeyTypes.Octet.Equals(jsonWebKey.Kty))
                    return IsSupportedSymmetricAlgorithm(algorithm);

                return false;
            }

            if (key is ECDsaSecurityKey)
                return IsSupportedEcdsaAlgorithm(algorithm);

            if (key as SymmetricSecurityKey != null)
                return IsSupportedSymmetricAlgorithm(algorithm);

            return false;
        }

        internal static bool IsSupportedEncryptionAlgorithm(string algorithm, SecurityKey key)
        {
            if (key == null)
                return false;

            if (string.IsNullOrEmpty(algorithm))
                return false;

            if (!(IsAesCbc(algorithm) || IsAesGcm(algorithm)))
                return false;

            if (key is SymmetricSecurityKey)
                return true;

            if (key is JsonWebKey jsonWebKey)
                return (jsonWebKey.K != null && jsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.Octet);

            return false;
        }

        internal static bool IsAesGcm(string algorithm)
        {
            if (string.IsNullOrEmpty(algorithm))
                return false;

            return algorithm.Equals(SecurityAlgorithms.Aes128Gcm)
               || algorithm.Equals(SecurityAlgorithms.Aes192Gcm)
               || algorithm.Equals(SecurityAlgorithms.Aes256Gcm);
        }

        internal static bool IsAesCbc(string algorithm)
        {
            if (string.IsNullOrEmpty(algorithm))
                return false;

            return algorithm.Equals(SecurityAlgorithms.Aes128CbcHmacSha256)
               || algorithm.Equals(SecurityAlgorithms.Aes192CbcHmacSha384)
               || algorithm.Equals(SecurityAlgorithms.Aes256CbcHmacSha512);
        }

        private static bool IsSupportedEcdsaAlgorithm(string algorithm)
        {
            return EcdsaSigningAlgorithms.Contains(algorithm);
        }

        internal static bool IsSupportedHashAlgorithm(string algorithm)
        {
            return HashAlgorithms.Contains(algorithm);
        }

        internal static bool IsSupportedRsaKeyWrap(string algorithm, SecurityKey key)
        {
            if (key == null)
                return false;

            if (string.IsNullOrEmpty(algorithm))
                return false;

            if (!RsaEncryptionAlgorithms.Contains(algorithm))
                return false;

            if (key is RsaSecurityKey || key is X509SecurityKey || (key is JsonWebKey rsaJsonWebKey && rsaJsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.RSA))
                return key.KeySize >= RsaMinKeySize;

            return false;
        }

        internal static bool IsSupportedSymmetricKeyWrap(string algorithm, SecurityKey key)
        {
            if (key == null)
                return false;

            if (string.IsNullOrEmpty(algorithm))
                return false;

            if (!SymmetricKeyWrapAlgorithms.Contains(algorithm))
                return false;

            return (key is SymmetricSecurityKey || (key is JsonWebKey jsonWebKey && jsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.Octet));
        }

        internal static bool IsSupportedRsaAlgorithm(string algorithm, SecurityKey key)
        {
            return RsaSigningAlgorithms.Contains(algorithm)
                || RsaEncryptionAlgorithms.Contains(algorithm)
                || (RsaPssSigningAlgorithms.Contains(algorithm) && IsSupportedRsaPss(key));
        }

        private static bool IsSupportedRsaPss(SecurityKey key)
        {
            // RSACryptoServiceProvider doesn't support RSA-PSS
            if (key is RsaSecurityKey rsa && rsa.Rsa is RSACryptoServiceProvider)
            {
                LogHelper.LogInformation(LogMessages.IDX10693);
                return false;
            }
            else if (key is X509SecurityKey x509SecurityKey && x509SecurityKey.PublicKey is RSACryptoServiceProvider)
            {
                LogHelper.LogInformation(LogMessages.IDX10693);
                return false;
            }
            else
            {
                return true;
            }
        }

        internal static bool IsSupportedSymmetricAlgorithm(string algorithm)
        {
            return SymmetricEncryptionAlgorithms.Contains(algorithm)
                || SymmetricKeyWrapAlgorithms.Contains(algorithm)
                || SymmetricSigningAlgorithms.Contains(algorithm);
        }

        /// <summary>
        /// Returns the maximum size in bytes for a supported signature algorithms.
        /// The key size affects the signature size for asymmetric algorithms.
        /// </summary>
        /// <param name="algorithm">The security algorithm to find the maximum size.</param>
        /// <returns>Set size for known algorithms, 2K default.</returns>
        internal static int GetMaxByteCount(string algorithm) => algorithm switch
        {
            SecurityAlgorithms.HmacSha256 or
            SecurityAlgorithms.HmacSha256Signature => 32,

            SecurityAlgorithms.HmacSha384 or
            SecurityAlgorithms.HmacSha384Signature => 48,

            SecurityAlgorithms.HmacSha512 or
            SecurityAlgorithms.HmacSha512Signature => 64,

            SecurityAlgorithms.EcdsaSha256 or
            SecurityAlgorithms.EcdsaSha256Signature or
            SecurityAlgorithms.EcdsaSha384 or
            SecurityAlgorithms.EcdsaSha384Signature or
            SecurityAlgorithms.RsaSha256 or
            SecurityAlgorithms.RsaSha256Signature or
            SecurityAlgorithms.RsaSsaPssSha256 or
            SecurityAlgorithms.RsaSsaPssSha256Signature or
            SecurityAlgorithms.RsaSha384 or
            SecurityAlgorithms.RsaSsaPssSha384 or
            SecurityAlgorithms.RsaSsaPssSha384Signature or
            SecurityAlgorithms.RsaSha384Signature => 512,

            SecurityAlgorithms.EcdsaSha512 or
            SecurityAlgorithms.EcdsaSha512Signature or
            SecurityAlgorithms.RsaSha512 or
            SecurityAlgorithms.RsaSsaPssSha512 or
            SecurityAlgorithms.RsaSsaPssSha512Signature or
            SecurityAlgorithms.RsaSha512Signature => 1024,

            // if we don't know the algorithm, report 2K twice as big as any known algorithm.
            _ => 2048,
        };
    }
}
