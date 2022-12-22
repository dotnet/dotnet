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
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// Represents the parameters needed to decrypt a JSON Web Token
    /// </summary>
    internal class JwtTokenDecryptionParameters
    {
        /// <summary>
        /// Gets or sets signature algorithm that was used to create the signature.
        /// </summary>
        public string Alg { get; set; }

        /// <summary>
        /// Gets or sets the AuthenticationTag from the original raw data of this instance when it was created.
        /// </summary>
        public string AuthenticationTag { get; set; }

        /// <summary>
        /// Gets or sets the Ciphertext from the original raw data of this instance when it was created.
        /// </summary>
        public string Ciphertext { get; set; }

        /// <summary>
        /// Gets or sets the function used to attempt decompression with.
        /// </summary>
        public Func<byte[], string, string> DecompressionFunction { get; set; }

        /// <summary>
        /// Gets or sets the encryption algorithm (Enc) of the token.
        /// </summary>
        public string Enc { get; set; }

        /// <summary>
        /// Gets the EncodedHeader from the original raw data of this instance when it was created.
        /// </summary>
        public string EncodedHeader { get; set; }

        /// <summary>
        /// Gets or sets the EncodedHeader from the original raw data of this instance when it was created.
        /// </summary>
        public string EncodedToken { get; set; }

        /// <summary>
        /// Gets or sets the InitializationVector from the original raw data of this instance when it was created.
        /// </summary>
        public string InitializationVector { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="SecurityKey"/>s to attempt to decrypt with.
        /// </summary>
        public IEnumerable<SecurityKey> Keys { get; set; }

        /// <summary>
        /// Gets or sets the 'value' of the 'zip' claim.
        /// </summary>
        public string Zip { get; set; }
    }
}
