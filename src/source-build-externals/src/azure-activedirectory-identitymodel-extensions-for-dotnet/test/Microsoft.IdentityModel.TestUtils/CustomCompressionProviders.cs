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
