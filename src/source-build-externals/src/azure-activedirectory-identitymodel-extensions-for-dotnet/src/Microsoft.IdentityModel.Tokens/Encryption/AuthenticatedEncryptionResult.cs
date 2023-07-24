// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the results of <see cref="AuthenticatedEncryptionProvider.Encrypt(byte[], byte[])"/> operation.
    /// </summary>
    public class AuthenticatedEncryptionResult
    {
        /// <summary>
        /// Initializes a new <see cref="AuthenticatedEncryptionResult"/>
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> used during <see cref="AuthenticatedEncryptionProvider.Encrypt(byte[], byte[])"/></param>
        /// <param name="ciphertext">protected text.</param>
        /// <param name="iv">the initialization vector used.</param>
        /// <param name="authenticationTag">the bytes that need be passed to <see cref="AuthenticatedEncryptionProvider.Decrypt(byte[], byte[], byte[], byte[])"/>.</param>
        public AuthenticatedEncryptionResult(SecurityKey key, byte[] ciphertext, byte[] iv, byte[] authenticationTag)
        {
            Key = key;
            Ciphertext = ciphertext;
            IV = iv;
            AuthenticationTag = authenticationTag;
        }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/>.
        /// </summary>
        public SecurityKey Key { get; private set; }

        /// <summary>
        /// Gets the Ciphertext.
        /// </summary>
        public byte[] Ciphertext { get; private set; }

        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] IV { get; private set; }

        /// <summary>
        /// Gets the authentication tag
        /// </summary>
        public byte[] AuthenticationTag { get; private set; }
    }
}
