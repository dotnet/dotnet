// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace NuGet.Common
{
    public static class CryptoHashUtility
    {
        private const string SHA256_OID = "2.16.840.1.101.3.4.2.1";
        private const string SHA384_OID = "2.16.840.1.101.3.4.2.2";
        private const string SHA512_OID = "2.16.840.1.101.3.4.2.3";
        private const string SHA256_RSA_OID = "1.2.840.113549.1.1.11";
        private const string SHA384_RSA_OID = "1.2.840.113549.1.1.12";
        private const string SHA512_RSA_OID = "1.2.840.113549.1.1.13";

        /// <summary>
        /// Compute the hash as a base64 encoded string.
        /// </summary>
        /// <remarks>Closes the stream by default.</remarks>
        /// <param name="hashAlgorithm">Algorithm to use for hashing.</param>
        /// <param name="data">Stream to hash.</param>
        public static string ComputeHashAsBase64(this HashAlgorithm hashAlgorithm, Stream data)
        {
            return ComputeHashAsBase64(hashAlgorithm, data, leaveStreamOpen: false);
        }

        /// <summary>
        /// Compute the hash as a base64 encoded string.
        /// </summary>
        /// <param name="hashAlgorithm">Algorithm to use for hashing.</param>
        /// <param name="data">Stream to hash.</param>
        /// <param name="leaveStreamOpen">If false the stream will be closed.</param>
        /// <returns>A base64 encoded hash string.</returns>
        public static string ComputeHashAsBase64(this HashAlgorithm hashAlgorithm, Stream data, bool leaveStreamOpen)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            string hash;

            try
            {
                hash = Convert.ToBase64String(hashAlgorithm.ComputeHash(data));
            }
            finally
            {
                if (!leaveStreamOpen)
                {
                    data.Dispose();
                }
            }

            return hash;
        }

        /// <summary>
        /// Compute the hash as a byte[].
        /// </summary>
        public static byte[] ComputeHash(this HashAlgorithmName hashAlgorithmName, byte[] data)
        {
            using (var provider = hashAlgorithmName.GetHashProvider())
            {
                return provider.ComputeHash(data);
            }
        }

        /// <summary>
        /// Compute the hash as a byte[].
        /// </summary>
        /// <remarks>Closes the stream by default.</remarks>
        /// <param name="hashAlgorithm">Algorithm to use for hashing.</param>
        /// <param name="data">Stream to hash.</param>
        /// <returns>A hash byte[].</returns>
        public static byte[] ComputeHash(this HashAlgorithm hashAlgorithm, Stream data)
        {
            return ComputeHash(hashAlgorithm, data, leaveStreamOpen: false);
        }

        /// <summary>
        /// Compute the hash as a byte[].
        /// </summary>
        /// <param name="hashAlgorithm">Algorithm to use for hashing.</param>
        /// <param name="data">Stream to hash.</param>
        /// <param name="leaveStreamOpen">If false the stream will be closed.</param>
        /// <returns>A hash byte[].</returns>
        public static byte[] ComputeHash(this HashAlgorithm hashAlgorithm, Stream data, bool leaveStreamOpen)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] hash;

            try
            {
                hash = hashAlgorithm.ComputeHash(data);
            }
            finally
            {
                if (!leaveStreamOpen)
                {
                    data.Dispose();
                }
            }

            return hash;
        }

        public static HashAlgorithm GetHashAlgorithm(string hashAlgorithmName)
        {
            if (hashAlgorithmName == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithmName));
            }

            Enum.TryParse<HashAlgorithmName>(hashAlgorithmName, ignoreCase: true, result: out var result);

            return GetHashAlgorithm(result);
        }

        public static HashAlgorithmName GetHashAlgorithmName(string hashAlgorithm)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (Enum.TryParse<HashAlgorithmName>(hashAlgorithm, ignoreCase: true, result: out var result))
            {
                return result;
            }

            return HashAlgorithmName.Unknown;
        }

        public static HashAlgorithm GetHashAlgorithm(HashAlgorithmName hashAlgorithmName)
        {
            return hashAlgorithmName.GetHashProvider();
        }

        public static HashAlgorithm GetHashProvider(this HashAlgorithmName hashAlgorithmName)
        {
#if !IS_CORECLR
            if (AllowFipsAlgorithmsOnly.Value)
            {
                // FIPs
                switch (hashAlgorithmName)
                {
                    case HashAlgorithmName.SHA256:
                        return new SHA256CryptoServiceProvider();
                    case HashAlgorithmName.SHA384:
                        return new SHA384CryptoServiceProvider();
                    case HashAlgorithmName.SHA512:
                        return new SHA512CryptoServiceProvider();
                }
            }
            else
            {
                // Non-FIPS
                switch (hashAlgorithmName)
                {
                    case HashAlgorithmName.SHA256:
                        return new SHA256Managed();
                    case HashAlgorithmName.SHA384:
                        return new SHA384Managed();
                    case HashAlgorithmName.SHA512:
                        return new SHA512Managed();
                }
            }
#else
            switch (hashAlgorithmName)
            {
                case HashAlgorithmName.SHA256:
                    return SHA256.Create();
                case HashAlgorithmName.SHA384:
                    return SHA384.Create();
                case HashAlgorithmName.SHA512:
                    return SHA512.Create();
            }
#endif

            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedHashAlgorithmName, hashAlgorithmName),
                nameof(hashAlgorithmName));
        }

#if !IS_CORECLR
        // Read this value once.
        private static Lazy<bool> AllowFipsAlgorithmsOnly = new Lazy<bool>(() => ReadFipsConfigValue());

        /// <summary>
        /// Determines if we are to only allow Fips compliant algorithms.
        /// </summary>
        /// <remarks>
        /// CryptoConfig.AllowOnlyFipsAlgorithm does not exist in Mono.
        /// </remarks>
        private static bool ReadFipsConfigValue()
        {
            // Mono does not currently support this method. Have this in a separate method to avoid JITing exceptions.
            var cryptoConfig = typeof(CryptoConfig);

            if (cryptoConfig != null)
            {
                var allowOnlyFipsAlgorithmsProperty = cryptoConfig.GetProperty("AllowOnlyFipsAlgorithms", BindingFlags.Public | BindingFlags.Static);

                if (allowOnlyFipsAlgorithmsProperty != null)
                {
                    return (bool)allowOnlyFipsAlgorithmsProperty.GetValue(null, null);
                }
            }

            return false;
        }
#endif

        /// <summary>
        /// Extension method to convert NuGet.Common.HashAlgorithmName to System.Security.Cryptography.HashAlgorithmName
        /// </summary>
        /// <returns>System.Security.Cryptography.HashAlgorithmName equivalent of the NuGet.Common.HashAlgorithmName</returns>
        public static System.Security.Cryptography.HashAlgorithmName ConvertToSystemSecurityHashAlgorithmName(this HashAlgorithmName hashAlgorithmName)
        {
            switch (hashAlgorithmName)
            {
                case HashAlgorithmName.SHA256:
                    return System.Security.Cryptography.HashAlgorithmName.SHA256;
                case HashAlgorithmName.SHA384:
                    return System.Security.Cryptography.HashAlgorithmName.SHA384;
                case HashAlgorithmName.SHA512:
                    return System.Security.Cryptography.HashAlgorithmName.SHA512;
                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedHashAlgorithmName, hashAlgorithmName),
                        nameof(hashAlgorithmName));
            }
        }

        /// <summary>
        /// Extension method to convert NuGet.Common.HashAlgorithmName to an Oid string
        /// </summary>
        /// <returns>Oid string equivalent of the NuGet.Common.HashAlgorithmName</returns>
        public static string ConvertToOidString(this HashAlgorithmName hashAlgorithmName)
        {
            switch (hashAlgorithmName)
            {
                case HashAlgorithmName.SHA256:
                    return SHA256_OID;
                case HashAlgorithmName.SHA384:
                    return SHA384_OID;
                case HashAlgorithmName.SHA512:
                    return SHA512_OID;
                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedHashAlgorithmName, hashAlgorithmName),
                        nameof(hashAlgorithmName));
            }
        }

        /// <summary>
        /// Extension method to convert NuGet.Common.HashAlgorithmName to an OID
        /// </summary>
        /// <returns>OID equivalent of the NuGet.Common.HashAlgorithmName</returns>
        public static Oid ConvertToOid(this HashAlgorithmName hashAlgorithm)
        {
            var oidString = hashAlgorithm.ConvertToOidString();

            return new Oid(oidString);
        }

        /// <summary>
        /// Helper method to convert an Oid string to NuGet.Common.HashAlgorithmName
        /// </summary>
        /// <param name="oid">An oid string.</param>
        /// <returns>NuGet.Common.HashAlgorithmName equivalent of the oid string</returns>
        public static HashAlgorithmName OidToHashAlgorithmName(string oid)
        {
            switch (oid)
            {
                case SHA256_OID:
                    return HashAlgorithmName.SHA256;
                case SHA384_OID:
                    return HashAlgorithmName.SHA384;
                case SHA512_OID:
                    return HashAlgorithmName.SHA512;
                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedHashAlgorithmName, oid),
                        nameof(oid));
            }
        }

        /// <summary>
        /// Extension method to convert NuGet.Common.SignatureAlgorithmName to an Oid string
        /// </summary>
        /// <returns>Oid string equivalent of the NuGet.Common.SignatureAlgorithmName</returns>
        public static string ConvertToOidString(this SignatureAlgorithmName signatureAlgorithmName)
        {
            switch (signatureAlgorithmName)
            {
                case SignatureAlgorithmName.SHA256RSA:
                    return SHA256_RSA_OID;
                case SignatureAlgorithmName.SHA384RSA:
                    return SHA384_RSA_OID;
                case SignatureAlgorithmName.SHA512RSA:
                    return SHA512_RSA_OID;
                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedSignatureAlgorithmName, signatureAlgorithmName),
                        nameof(signatureAlgorithmName));
            }
        }
    }
}
