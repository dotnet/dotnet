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
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// A class for properties that are used for token encryption.
    /// </summary>
    public class EncryptingCredentials
    {
        private string _alg;
        private string _enc;
        private SecurityKey _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptingCredentials"/> class.
        /// </summary>
        /// <param name="certificate"><see cref="X509Certificate2"/>.</param>
        /// <param name="alg">A key wrap algorithm to use when encrypting a session key.</param>
        /// <param name="enc">Data encryption algorithm to apply.</param>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'alg' is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if 'enc' is null or empty.</exception>
        protected EncryptingCredentials(X509Certificate2 certificate, string alg, string enc)
        {
            if (certificate == null)
                throw LogHelper.LogArgumentNullException(nameof(certificate));

            Key = new X509SecurityKey(certificate);
            Alg = alg;
            Enc = enc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptingCredentials"/> class.
        /// </summary>
        /// <param name="key"><see cref="SecurityKey"/> to use when encrypting a session key.</param>
        /// <param name="alg">A key wrap algorithm to use when encrypting a session key.</param>
        /// <param name="enc">Data encryption algorithm to apply.</param>
        /// <exception cref="ArgumentNullException">if 'key' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'alg' is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if 'enc' is null or empty.</exception>
        public EncryptingCredentials(SecurityKey key, string alg, string enc)
        {
            Key = key;
            Alg = alg;
            Enc = enc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptingCredentials"/> class.
        /// </summary>
        /// <remarks> Used in scenarios when a key represents a 'shared' symmetric key.
        /// For example, SAML 2.0 Assertion will be encrypted using a provided symmetric key
        /// which won't be serialized to a SAML token.
        /// </remarks>
        /// <param name="key"><see cref="SymmetricSecurityKey"/> to apply.</param>
        /// <param name="enc">Data encryption algorithm to apply.</param>
        /// <exception cref="ArgumentException">If the <see cref="SecurityKey"/> is not a <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="ArgumentNullException">if 'enc' is null or empty.</exception>
        public EncryptingCredentials(SymmetricSecurityKey key, string enc)
            : this(key, SecurityAlgorithms.None, enc)
        {
        }

        /// <summary>
        /// Gets the key wrap algorithm used for session key encryption.
        /// </summary>
        public string Alg
        {
            get => _alg;
            private set => _alg = string.IsNullOrEmpty(value) ? throw LogHelper.LogArgumentNullException("alg") : value;
        }

        /// <summary>
        /// Gets the data encryption algorithm.
        /// </summary>
        public string Enc
        {
            get => _enc;
            private set => _enc = string.IsNullOrEmpty(value) ? throw LogHelper.LogArgumentNullException("enc") : value;
        }

        /// <summary>
        /// Public key used in Key Agreement Algorithms
        /// </summary>
        public SecurityKey KeyExchangePublicKey { get; set; }

        /// <summary>
        /// Users can override the default <see cref="CryptoProviderFactory"/> with this property. This factory will be used for creating encryption providers.
        /// </summary>
        public CryptoProviderFactory CryptoProviderFactory { get; set; }

        /// <summary>
        /// Gets or sets a bool that controls if the encrypted token creation will set default 'cty' if not specified.
        /// <remarks>
        /// Applies to only JWT tokens.
        /// </remarks>
        /// </summary>
        public bool SetDefaultCtyClaim { get; set; } = true;

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> used for encryption.
        /// </summary>
        public SecurityKey Key
        {
            get => _key;
            private set => _key = value ?? throw LogHelper.LogArgumentNullException("key");
        }
    }
}
