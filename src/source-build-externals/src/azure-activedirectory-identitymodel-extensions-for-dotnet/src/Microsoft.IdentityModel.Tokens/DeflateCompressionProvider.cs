//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using Microsoft.IdentityModel.Logging;
using System;
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
        /// Decompress the value using DEFLATE algorithm.
        /// </summary>
        /// <param name="value">the bytes to decompress.</param>
        /// <returns>the decompressed bytes.</returns>
        public byte[] Decompress(byte[] value)
        {
            if (value == null)
                throw LogHelper.LogArgumentNullException(nameof(value));

            using (var inputStream = new MemoryStream(value))
            {
                using (var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(deflateStream, Encoding.UTF8))
                    {
                        return Encoding.UTF8.GetBytes(reader.ReadToEnd());
                    }
                }
            }
        }

        /// <summary>
        /// Compress the value using the DEFLATE algorithm.
        /// </summary>
        /// <param name="value">the bytes to compress.</param>
        /// <returns>the compressed bytes.</returns>
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
        /// Answers if a compression algorithm is supported.
        /// </summary>
        /// <param name="algorithm">the name of the compression algorithm.</param>
        /// <returns>true if the compression algorithm is supported, false otherwise.</returns>
        public bool IsSupportedAlgorithm(string algorithm)
        {
            return Algorithm.Equals(algorithm);
        }
    }
}
