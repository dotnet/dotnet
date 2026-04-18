// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using System;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// A custom compression provider class implementing <see cref="ICompressionProvider"/>.
    /// </summary>
    public class SampleCustomCompressionProvider : ICompressionProvider
    {
        public SampleCustomCompressionProvider(string algorithm)
        {
            Algorithm = algorithm;

            if (!IsSupportedAlgorithm(algorithm))
                throw new NotSupportedException($"Algorithm '{algorithm}' is not supported.");
        }

        public string Algorithm { get; set; }

        public byte[] Compress(byte[] value)
        {
            // just return the same bytes that were passed in
            return value;
        }

        public byte[] Decompress(byte[] value)
        {
            // just return the same bytes that were passed in
            return value;
        }

        public bool IsSupportedAlgorithm(string algorithm)
        {
            return algorithm != null && algorithm.Equals(Algorithm);
        }
    }

    /// <summary>
    /// A custom compression provider class implementing <see cref="ICompressionProvider"/>, 
    /// which accepts any algorithm but always return null for decompression and compression.
    /// </summary>
    public class SampleCustomCompressionProviderDecompressAndCompressAlwaysFail : ICompressionProvider
    {
        public SampleCustomCompressionProviderDecompressAndCompressAlwaysFail(string algorithm)
        {
            Algorithm = algorithm;
        }

        public string Algorithm { get; set; }

        public byte[] Compress(byte[] value)
        {
            return null;
        }

        public byte[] Decompress(byte[] value)
        {
            return null;
        }

        public bool IsSupportedAlgorithm(string algorithm)
        {
            return true;
        }
    }
}
