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
