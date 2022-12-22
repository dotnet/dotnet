// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// SignedCms.ContentInfo.Content for the primary signature.
    /// </summary>
    public sealed class SignatureContent
    {
        private readonly SigningSpecifications _signingSpecifications;

        /// <summary>
        /// Hashing algorithm used.
        /// </summary>
        public HashAlgorithmName HashAlgorithm { get; }

        /// <summary>
        /// Base64 package stream hash.
        /// </summary>
        public string HashValue { get; }

        public SignatureContent(
            SigningSpecifications signingSpecifications,
            HashAlgorithmName hashAlgorithm,
            string hashValue)
        {
            if (signingSpecifications == null)
            {
                throw new ArgumentNullException(nameof(signingSpecifications));
            }

            if (string.IsNullOrEmpty(hashValue))
            {
                throw new ArgumentException(Strings.StringCannotBeNullOrEmpty, nameof(hashValue));
            }

            _signingSpecifications = signingSpecifications;
            HashAlgorithm = hashAlgorithm;
            HashValue = hashValue;
        }

        /// <summary>
        /// Write the content to a stream.
        /// </summary>
        private void Save(Stream stream)
        {
            using (var writer = new KeyPairFileWriter(stream, _signingSpecifications.Encoding, leaveOpen: true))
            {
                writer.WritePair("Version", _signingSpecifications.Version);
                writer.WriteSectionBreak();
                writer.WritePair(CryptoHashUtility.ConvertToOidString(HashAlgorithm) + "-Hash", HashValue);
                writer.WriteSectionBreak();
            }
        }

        /// <summary>
        /// Write the content to byte array.
        /// </summary>
        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                Save(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Load from a byte array.
        /// </summary>
        public static SignatureContent Load(byte[] bytes, SigningSpecifications signingSpecifications)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (signingSpecifications == null)
            {
                throw new ArgumentNullException(nameof(signingSpecifications));
            }

            using (var ms = new MemoryStream(bytes))
            {
                ms.Position = 0;
                return Load(ms, signingSpecifications);
            }
        }

        /// <summary>
        /// Load content from a stream.
        /// </summary>
        private static SignatureContent Load(Stream stream, SigningSpecifications signingSpecifications)
        {
            var hashAlgorithm = HashAlgorithmName.Unknown;
            string hash = null;

            using (var reader = new KeyPairFileReader(stream, signingSpecifications.Encoding))
            {
                // Read header-section.
                var properties = reader.ReadSection();

                ThrowIfEmpty(properties);
                ThrowIfSignatureFormatVersionIsUnsupported(properties, signingSpecifications);

                // Read only the first section.
                properties = reader.ReadSection();

                ThrowIfEmpty(properties);

                foreach (var property in properties)
                {
                    if (TryReadPackageHashProperty(property, signingSpecifications, out hashAlgorithm))
                    {
                        hash = property.Value;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(hash))
                {
                    throw new SignatureException(Strings.UnableToReadPackageHashInformation);
                }
            }

            return new SignatureContent(signingSpecifications, hashAlgorithm, hash);
        }

        private static void ThrowIfEmpty(Dictionary<string, string> properties)
        {
            if (properties.Count == 0)
            {
                throw new SignatureException(Strings.InvalidSignatureContent);
            }
        }

        private static bool TryReadPackageHashProperty(
            KeyValuePair<string, string> property,
            SigningSpecifications signingSpecifications,
            out HashAlgorithmName hashAlgorithmName)
        {
            hashAlgorithmName = HashAlgorithmName.Unknown;

            if (property.Key.EndsWith("-Hash", StringComparison.Ordinal))
            {
                foreach (var hashAlgorithmOid in signingSpecifications.AllowedHashAlgorithmOids)
                {
                    if (property.Key == $"{hashAlgorithmOid}-Hash")
                    {
                        hashAlgorithmName = CryptoHashUtility.OidToHashAlgorithmName(hashAlgorithmOid);

                        return true;
                    }
                }
            }

            return false;
        }

        private static void ThrowIfSignatureFormatVersionIsUnsupported(Dictionary<string, string> properties, SigningSpecifications signingSpecifications)
        {
            const string Version = "Version";

            string signatureFormatVersion;
            if (!properties.TryGetValue(Version, out signatureFormatVersion))
            {
                throw new SignatureException(Strings.InvalidSignatureContent);
            }

            if (signingSpecifications.Version != signatureFormatVersion)
            {
                throw new SignatureException(NuGetLogCode.NU3007, Strings.UnsupportedSignatureFormatVersion);
            }
        }
    }
}
