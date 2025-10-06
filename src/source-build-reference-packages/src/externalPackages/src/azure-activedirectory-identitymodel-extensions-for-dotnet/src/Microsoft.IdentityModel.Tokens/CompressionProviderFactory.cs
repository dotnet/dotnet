// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
        /// Initializes a new instance of the <see cref="CompressionProviderFactory"/> class.
        /// </summary>
        public CompressionProviderFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionProviderFactory"/> class.
        /// </summary>
        /// <param name="other">The <see cref="CompressionProviderFactory"/> to copy from.</param>
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
        /// Determines whether the specified algorithm is supported.
        /// </summary>
        /// <param name="algorithm">The name of the cryptographic algorithm.</param>
        /// <returns><see langword="true"/> if the algorithm is supported; otherwise, <see langword="false"/>.</returns>
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
        /// Creates a <see cref="ICompressionProvider"/> for a specified compression algorithm.
        /// </summary>
        /// <param name="algorithm">The compression algorithm.</param>
        /// <returns>An instance of <see cref="ICompressionProvider"/> for the specified algorithm.</returns>
        public ICompressionProvider CreateCompressionProvider(string algorithm)
        {
            return CreateCompressionProvider(algorithm, TokenValidationParameters.DefaultMaximumTokenSizeInBytes);
        }

        /// <summary>
        /// Creates a <see cref="ICompressionProvider"/> for a specific compression algorithm with a maximum deflate size limit.
        /// </summary>
        /// <param name="algorithm">The compression algorithm.</param>
        /// <param name="maximumDeflateSize">The maximum size limit (in characters) for deflate compression processing.</param>
        /// <returns>A <see cref="ICompressionProvider"/> instance.</returns>
        public ICompressionProvider CreateCompressionProvider(string algorithm, int maximumDeflateSize)
        {
            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (CustomCompressionProvider != null && CustomCompressionProvider.IsSupportedAlgorithm(algorithm))
                return CustomCompressionProvider;

            if (algorithm.Equals(CompressionAlgorithms.Deflate))
                return new DeflateCompressionProvider { MaximumDeflateSize = maximumDeflateSize };

            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm))));
        }
    }
}
