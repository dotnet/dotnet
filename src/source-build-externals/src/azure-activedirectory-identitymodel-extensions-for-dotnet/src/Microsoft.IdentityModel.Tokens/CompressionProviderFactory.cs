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
            return CreateCompressionProvider(algorithm, TokenValidationParameters.DefaultMaximumTokenSizeInBytes);
        }

        /// <summary>
        /// Returns a <see cref="ICompressionProvider"/> for a specific algorithm.
        /// </summary>
        /// <param name="algorithm">the decompression algorithm.</param>
        /// <param name="maximumDeflateSize">the maximum deflate size in chars that will be processed.</param>
        /// <returns>a <see cref="ICompressionProvider"/>.</returns>
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

