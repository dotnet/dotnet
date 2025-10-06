// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Defines the <see cref="SecurityKey"/>, algorithm and digest for digital signatures.
    /// </summary>
    public class SigningCredentials
    {
        private string _algorithm;
        private string _digest;
        private SecurityKey _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningCredentials"/> class.
        /// </summary>
        /// <param name="certificate"><see cref="X509Certificate2"/> that will be used for signing.</param>
        /// <remarks>Algorithm will be set to <see cref="SecurityAlgorithms.RsaSha256"/>.
        /// the 'digest method' if needed may be implied from the algorithm. For example <see cref="SecurityAlgorithms.RsaSha256"/> implies Sha256.</remarks>
        /// <exception cref="ArgumentNullException">if 'key' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'algorithm' is null or empty.</exception>
        protected SigningCredentials(X509Certificate2 certificate)
        {
            if (certificate == null)
                throw LogHelper.LogArgumentNullException(nameof(certificate));

            Key = new X509SecurityKey(certificate);
            Algorithm = SecurityAlgorithms.RsaSha256;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningCredentials"/> class.
        /// </summary>
        /// <param name="certificate"><see cref="X509Certificate2"/> that will be used for signing.</param>
        /// <param name="algorithm">The signature algorithm to be used.</param>
        /// <remarks>the 'digest method' if needed may be implied from the algorithm. For example <see cref="SecurityAlgorithms.RsaSha256"/> implies Sha256.</remarks>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'algorithm' is null or empty.</exception>
        protected SigningCredentials(X509Certificate2 certificate, string algorithm)
        {
            if (certificate == null)
                throw LogHelper.LogArgumentNullException(nameof(certificate));

            Key = new X509SecurityKey(certificate);
            Algorithm = algorithm;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SigningCredentials"/> class.
        /// </summary>
        /// <param name="key"><see cref="SecurityKey"/>.</param>
        /// <param name="algorithm">The signature algorithm to be used.</param>
        /// <remarks>the 'digest method' if needed may be implied from the algorithm. For example <see cref="SecurityAlgorithms.HmacSha256Signature"/> implies Sha256.</remarks>
        /// <exception cref="ArgumentNullException">if 'key' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'algorithm' is null or empty.</exception>
        public SigningCredentials(SecurityKey key, string algorithm)
        {
            Key = key;
            Algorithm = algorithm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningCredentials"/> class.
        /// </summary>
        /// <param name="key"><see cref="SecurityKey"/>.</param>
        /// <param name="algorithm">The signature algorithm to be used.</param>
        /// <param name="digest">The digest algorithm to be used.</param>
        /// <exception cref="ArgumentNullException">if 'key' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'algorithm' is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if 'digest' is null or empty.</exception>
        public SigningCredentials(SecurityKey key, string algorithm, string digest)
        {
            Key = key;
            Algorithm = algorithm;
            Digest = digest;
        }

        /// <summary>
        /// Gets the signature algorithm.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null or empty.</exception>
        public string Algorithm
        {
            get => _algorithm;
            private set => _algorithm = string.IsNullOrEmpty(value) ? throw LogHelper.LogArgumentNullException("algorithm") : value;
        }

        /// <summary>
        /// Gets the digest algorithm.
        /// </summary>
        public string Digest
        {
            get => _digest;
            private set => _digest = string.IsNullOrEmpty(value) ? throw LogHelper.LogArgumentNullException("digest") : value;
        }

        /// <summary>
        /// Users can override the default <see cref="CryptoProviderFactory"/> with this property. This factory will be used for creating signature providers.
        /// </summary>
        /// <remarks>This will have precedence over <see cref="SecurityKey.CryptoProviderFactory"/></remarks>
        public CryptoProviderFactory CryptoProviderFactory { get; set; }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> used for signature creation or validation.
        /// </summary>
        public SecurityKey Key
        {
            get => _key;
            private set => _key = value ?? throw LogHelper.LogArgumentNullException("key");
        }

        /// <summary>
        /// Gets the key id associated with <see cref="SecurityKey"/>.
        /// </summary>
        public string Kid
        {
            get => Key.KeyId;
        }
    }
}
