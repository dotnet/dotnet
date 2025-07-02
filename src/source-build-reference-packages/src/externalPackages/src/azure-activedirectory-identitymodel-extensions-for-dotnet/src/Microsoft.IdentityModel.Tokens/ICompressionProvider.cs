// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides methods for compressing and decompressing data.
    /// </summary>
    public interface ICompressionProvider
    {
        /// <summary>
        /// Gets the compression algorithm used by the provider.
        /// </summary>
        string Algorithm { get; }

        /// <summary>
        /// Determines if a specified algorithm is supported by the provider.
        /// </summary>
        /// <param name="algorithm">The compression algorithm to check.</param>
        /// <returns><see langword="true"/> if the algorithm is supported; otherwise, <see langword="false"/>.</returns>
        bool IsSupportedAlgorithm(string algorithm);

        /// <summary>
        /// Decompresses the specified byte array.
        /// </summary>
        /// <param name="value">The byte array to decompress.</param>
        /// <returns>A byte array containing the decompressed data.</returns>
        byte[] Decompress(byte[] value);

        /// <summary>
        /// Compresses the specified byte array.
        /// </summary>
        /// <param name="value">The byte array to compress.</param>
        /// <returns>A byte array containing the compressed data.</returns>
        byte[] Compress(byte[] value);
    }
}
