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
    /// Represents a symmetric security key.
    /// </summary>
    public class SymmetricSecurityKey : SecurityKey
    {
        int _keySize;
        byte[] _key;

        internal SymmetricSecurityKey(JsonWebKey webKey)
            : base(webKey)
        {
            if (string.IsNullOrEmpty(webKey.K))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10703, LogHelper.MarkAsNonPII(typeof(SymmetricSecurityKey)))));

            _key = Base64UrlEncoder.DecodeBytes(webKey.K);
            _keySize = _key.Length * 8;
            webKey.ConvertedSecurityKey = this;
        }

        /// <summary>
        /// Returns a new instance of <see cref="SymmetricSecurityKey"/> instance.
        /// </summary>
        /// <param name="key">The byte array of the key.</param>
        public SymmetricSecurityKey(byte[] key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (key.Length == 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10703, LogHelper.MarkAsNonPII(typeof(SymmetricSecurityKey)))));

            _key = key.CloneByteArray();
            _keySize = _key.Length * 8;
        }

        /// <summary>
        /// Gets the key size.
        /// </summary>
        public override int KeySize
        {
            get { return _keySize; }
        }

        /// <summary>
        /// Gets the byte array of the key.
        /// </summary>
        public virtual byte[] Key
        {
            get { return _key.CloneByteArray(); }
        }

        /// <summary>
        /// Determines whether the <see cref="SymmetricSecurityKey"/> can compute a JWK thumbprint.
        /// </summary>
        /// <returns><c>true</c> if JWK thumbprint can be computed; otherwise, <c>false</c>.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override bool CanComputeJwkThumbprint()
        {
            return true;
        }

        /// <summary>
        /// Computes a sha256 hash over the <see cref="SymmetricSecurityKey"/>.
        /// </summary>
        /// <returns>A JWK thumbprint.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override byte[] ComputeJwkThumbprint()
        {
            var canonicalJwk = $@"{{""{JsonWebKeyParameterNames.K}"":""{Base64UrlEncoder.Encode(Key)}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.Octet}""}}";
            return Utility.GenerateSha256Hash(canonicalJwk);
        }
    }
}
