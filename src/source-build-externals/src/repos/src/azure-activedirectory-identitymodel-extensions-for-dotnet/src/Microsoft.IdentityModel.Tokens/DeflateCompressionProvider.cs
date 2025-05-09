// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Logging;
using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// A compression provider that supports compression and decompression using the <see cref="CompressionAlgorithms.Deflate"/> algorithm.
    /// </summary>
    public class DeflateCompressionProvider : ICompressionProvider
    {
        private int _maximumTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeflateCompressionProvider"/> class used to compress and decompress used the <see cref="CompressionAlgorithms.Deflate"/> algorithm.
        /// </summary>
        public DeflateCompressionProvider()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeflateCompressionProvider"/> class used to compress and decompress used the <see cref="CompressionAlgorithms.Deflate"/> algorithm.
        /// <param name="compressionLevel">The compression level to use when compressing.</param>
        /// </summary>
        public DeflateCompressionProvider(CompressionLevel compressionLevel)
        {
            CompressionLevel = compressionLevel;
        }

        /// <summary>
        /// Gets the compression algorithm.
        /// </summary>
        public string Algorithm => CompressionAlgorithms.Deflate;

        /// <summary>
        /// Specifies whether compression should emphasize speed or compression size.
        /// Set to <see cref="CompressionLevel.Optimal"/> by default.
        /// </summary>
        public CompressionLevel CompressionLevel { get; private set; } = CompressionLevel.Optimal;

        /// <summary>
        /// Gets and sets the maximum deflate size in chars that will be processed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is less than 1.</exception>
        public int MaximumDeflateSize
        {
            get => _maximumTokenSizeInBytes;
            set => _maximumTokenSizeInBytes = (value < 1) ?
                throw LogHelper.LogExceptionMessage(
                    new ArgumentOutOfRangeException(nameof(value),
                        LogHelper.FormatInvariant(LogMessages.IDX10101, LogHelper.MarkAsNonPII(value)))) : value;
        }

        /// <summary>
        /// Decompresses the value using Deflate algorithm.
        /// </summary>
        /// <param name="value">The bytes to decompress.</param>
        /// <returns>The decompressed bytes.</returns>
        public byte[] Decompress(byte[] value)
        {
            if (value == null)
                throw LogHelper.LogArgumentNullException(nameof(value));

            char[] chars = null;
            try
            {
                chars = ArrayPool<char>.Shared.Rent(MaximumDeflateSize);
                using (var inputStream = new MemoryStream(value))
                {
                    using (var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                    {
                        using (var reader = new StreamReader(deflateStream, Encoding.UTF8))
                        {
                            // if there is one more char to read, then the token is too large.
                            int bytesRead = reader.Read(chars, 0, MaximumDeflateSize);
                            if (reader.Peek() != -1)
                            {
                                throw LogHelper.LogExceptionMessage(
                                    new SecurityTokenDecompressionFailedException(
                                        LogHelper.FormatInvariant(
                                            LogMessages.IDX10816,
                                            LogHelper.MarkAsNonPII(MaximumDeflateSize))));
                            }

                            return Encoding.UTF8.GetBytes(chars, 0, bytesRead);
                        }
                    }
                }
            }
            finally
            {
                if (chars != null)
                    ArrayPool<char>.Shared.Return(chars);
            }
        }

        /// <summary>
        /// Compresses the value using the Deflate algorithm.
        /// </summary>
        /// <param name="value">The bytes to compress.</param>
        /// <returns>The compressed bytes.</returns>
        public byte[] Compress(byte[] value)
        {
            if (value == null)
                throw LogHelper.LogArgumentNullException(nameof(value));

            using (var output = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(output, CompressionLevel))
                {
                    using (var writer = new StreamWriter(deflateStream, Encoding.UTF8))
                    {
                        writer.Write(Encoding.UTF8.GetString(value));
                    }
                }

                return output.ToArray();
            }
        }

        /// <summary>
        /// Determines whether the specified compression algorithm is supported.
        /// </summary>
        /// <param name="algorithm">The name of the compression algorithm.</param>
        /// <returns><see langword="true"/> if the compression algorithm is supported; otherwise, <see langword="false"/>.</returns>
        public bool IsSupportedAlgorithm(string algorithm)
        {
            return Algorithm.Equals(algorithm);
        }
    }
}
