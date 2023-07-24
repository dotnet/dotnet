// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Compression provider interface.
    /// </summary>
    public interface ICompressionProvider
    {
        /// <summary>
        /// Gets the compression algorithm.
        /// </summary>
        string Algorithm { get; }

        /// <summary>
        /// Called to determine if an algorithm is supported.
        /// </summary>
        /// <param name="algorithm">the algorithm that defines the compression method.</param>
        /// <returns>true if supported</returns>
        bool IsSupportedAlgorithm(string algorithm);

        /// <summary>
        /// Decompress.
        /// </summary>
        /// <param name="value">the value to decompress.</param>
        byte[] Decompress(byte[] value);

        /// <summary>
        /// Compress.
        /// </summary>
        /// <param name="value">the value to decompress.</param>
        byte[] Compress(byte[] value);
    }
}
