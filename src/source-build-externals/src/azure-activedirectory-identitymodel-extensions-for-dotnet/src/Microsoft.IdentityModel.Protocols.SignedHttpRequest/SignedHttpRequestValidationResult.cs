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

using System;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Contains artifacts obtained when a SignedHttpRequest is validated.
    /// </summary>
    public class SignedHttpRequestValidationResult
    {
        /// <summary>
        /// Gets or sets the access token validation result.
        /// </summary>
        public TokenValidationResult AccessTokenValidationResult { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Exception"/> that occurred during validation of the SignedHttpRequest.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// True if the SignedHttpRequest was successfully validated, false otherwise.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets SignedHttpRequest in its original encoded form.
        /// </summary>
        public string SignedHttpRequest { get; set; }

        /// <summary>
        /// Gets or sets the validated SignedHttpRequest.
        /// </summary>
        public SecurityToken ValidatedSignedHttpRequest { get; set; }
    }
}
