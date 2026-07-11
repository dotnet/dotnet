// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.DotNet.Build.Tasks.FileCatalog
{
    /// <summary>
    /// A single file (or in-memory blob) to include in a catalog. Only the SHA-1 and SHA-256
    /// hashes of the content are recorded in the catalog, so an entry stores just those hashes
    /// (never the raw content), keeping memory use bounded regardless of file size.
    /// </summary>
    public sealed class CatalogEntry
    {
        /// <summary>
        /// Creates an entry whose hashes are computed by streaming <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">Path on disk to read for the file hashes.</param>
        /// <param name="name">
        /// Optional logical name for the entry. Defaults to <see cref="Path.GetFileName(string)"/>
        /// of <paramref name="filePath"/>. The name is not embedded in the catalog (makecat
        /// V2 catalogs do not carry per-member file names); it is retained only for diagnostics.
        /// </param>
        public static CatalogEntry FromFile(string filePath, string? name = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path must not be null or empty.", nameof(filePath));
            }

            // Stream the file through both hash algorithms in a single pass so the full
            // content is never buffered in memory (catalog generation only needs the hashes).
            using IncrementalHash sha1 = IncrementalHash.CreateHash(HashAlgorithmName.SHA1);
            using IncrementalHash sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            using (FileStream stream = File.OpenRead(filePath))
            {
                byte[] buffer = ArrayPool<byte>.Shared.Rent(81920);
                try
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        sha1.AppendData(buffer, 0, read);
                        sha256.AppendData(buffer, 0, read);
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }

            return new CatalogEntry(name ?? Path.GetFileName(filePath), sha1.GetHashAndReset(), sha256.GetHashAndReset());
        }

        /// <summary>
        /// Creates an entry from in-memory content, computing its hashes immediately.
        /// </summary>
        public CatalogEntry(string name, ReadOnlyMemory<byte> content)
            : this(name, SHA1.HashData(content.Span), SHA256.HashData(content.Span))
        {
        }

        private CatalogEntry(string name, byte[] sha1, byte[] sha256)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name must not be null or empty.", nameof(name));
            }

            Name = name;
            Sha1 = sha1;
            Sha256 = sha256;
        }

        /// <summary>Logical name for the entry (typically the file name). Not embedded in the catalog.</summary>
        public string Name { get; }

        /// <summary>The raw SHA-1 hash of the content, used as a legacy catalog lookup key.</summary>
        public byte[] Sha1 { get; }

        /// <summary>The raw SHA-256 hash of the content, recorded in the catalog.</summary>
        public byte[] Sha256 { get; }
    }
}
