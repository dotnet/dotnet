// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NuGet.Common
{
    /// <summary>
    /// CryptoHashProvider helps calculate or verify hash based on SHA256 or SHA512 algorithms
    /// </summary>
    public class CryptoHashProvider
    {
        /// <summary>
        /// Server token used to represent that the hash being used is SHA 512
        /// </summary>
        private const string SHA512HashAlgorithm = "SHA512";

        /// <summary>
        /// Server token used to represent that the hash being used is SHA 256
        /// </summary>
        private const string SHA256HashAlgorithm = "SHA256";

        private readonly string _hashAlgorithm;

        /// <summary>
        /// Creates an instance of CryptoHashProvider. Since the algorithm is not specified, SHA512 is assumed
        /// </summary>
        public CryptoHashProvider()
            : this(null)
        {
        }

        /// <summary>
        /// Creates an instance of CryptoHashProvider using the hashAlgorithm
        /// </summary>
        public CryptoHashProvider(string? hashAlgorithm)
        {
            if (string.IsNullOrEmpty(hashAlgorithm))
            {
                hashAlgorithm = SHA512HashAlgorithm;
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            // null annotation on string.IsNullOrEmpty missing before .NET 3.0
            else if (!hashAlgorithm.Equals(SHA512HashAlgorithm, StringComparison.OrdinalIgnoreCase)
                     &&
                     !hashAlgorithm.Equals(SHA256HashAlgorithm, StringComparison.OrdinalIgnoreCase))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                // Only support a vetted list of hash algorithms.
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedHashAlgorithm, hashAlgorithm), nameof(hashAlgorithm));
            }

            _hashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        /// Calculates the hash for a given stream
        /// </summary>
        public byte[] CalculateHash(Stream stream)
        {
            using (var hashAlgorithm = CryptoHashUtility.GetHashAlgorithm(_hashAlgorithm))
            {
                return hashAlgorithm.ComputeHash(stream);
            }
        }

        /// <summary>
        /// Calculates the hash for a byte array
        /// </summary>
        public byte[] CalculateHash(byte[] data)
        {
            using (var hashAlgorithm = CryptoHashUtility.GetHashAlgorithm(_hashAlgorithm))
            {
                return hashAlgorithm.ComputeHash(data);
            }
        }

        /// <summary>
        /// Verifies the hash for the given data and hash
        /// </summary>
        public bool VerifyHash(byte[] data, byte[] hash)
        {
            var dataHash = CalculateHash(data);
            return Enumerable.SequenceEqual(dataHash, hash);
        }
    }
}
