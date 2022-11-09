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

using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Compression provider factory for compression and decompression.
    /// </summary>
    public class CompressionProviderFactory
    {
        private static CompressionProviderFactory _default;

        /// <summary>
        /// Static constructor that initializes the default <see cref="CompressionProviderFactory"/>.
        /// </summary>
        static CompressionProviderFactory()
        {
            Default = new CompressionProviderFactory();
        }

        /// <summary>
        /// Default constructor for <see cref="CompressionProviderFactory"/>.
        /// </summary>
        public CompressionProviderFactory()
        {
        }

        /// <summary>
        /// Constructor that creates a deep copy of given <see cref="CompressionProviderFactory"/> object.
        /// </summary>
        /// <param name="other"><see cref="CompressionProviderFactory"/> to copy from.</param>
        public CompressionProviderFactory(CompressionProviderFactory other)
        {
            if (other == null)
                throw LogHelper.LogArgumentNullException(nameof(other));

            CustomCompressionProvider = other.CustomCompressionProvider;
        }

        /// <summary>
        /// Returns the default <see cref="CompressionProviderFactory"/> instance.
        /// </summary>
        public static CompressionProviderFactory Default
        {
            get => _default;
            set => _default = value ?? throw LogHelper.LogArgumentNullException(nameof(Default));
        }

        /// <summary>
        /// Extensibility point for custom compression support application wide.
        /// </summary>
        public ICompressionProvider CustomCompressionProvider { get; set; }

        /// <summary>
        /// Answers if an algorithm is supported.
        /// </summary>
        /// <param name="algorithm">the name of the crypto algorithm.</param>
        /// <returns>true if the algorithm is supported, false otherwise.</returns>
        public virtual bool IsSupportedAlgorithm(string algorithm)
        {
            if (CustomCompressionProvider != null && CustomCompressionProvider.IsSupportedAlgorithm(algorithm))
                return true;

            return IsSupportedCompressionAlgorithm(algorithm);
        }

        private static bool IsSupportedCompressionAlgorithm(string algorithm)
        {
            return CompressionAlgorithms.Deflate.Equals(algorithm);
        }

        /// <summary>
        /// Returns a <see cref="ICompressionProvider"/> for a specific algorithm.
        /// </summary>
        /// <param name="algorithm">the decompression algorithm.</param>
        /// <returns>a <see cref="ICompressionProvider"/>.</returns>
        public ICompressionProvider CreateCompressionProvider(string algorithm)
        {
            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (CustomCompressionProvider != null && CustomCompressionProvider.IsSupportedAlgorithm(algorithm))
                return CustomCompressionProvider;

            if (algorithm.Equals(CompressionAlgorithms.Deflate))
                return new DeflateCompressionProvider();

            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm))));
        }
    }
}

