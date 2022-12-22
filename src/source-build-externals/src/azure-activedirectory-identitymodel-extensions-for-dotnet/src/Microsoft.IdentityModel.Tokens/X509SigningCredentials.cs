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

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Defines the <see cref="X509Certificate2"/>, algorithm and digest for digital signatures.
    /// </summary>
    public class X509SigningCredentials : SigningCredentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="X509SigningCredentials"/> class.
        /// </summary>
        /// <param name="certificate"><see cref="X509Certificate2"/> that will be used for signing.</param>
        /// <remarks>Algorithm will be set to <see cref="SecurityAlgorithms.RsaSha256"/>.
        /// the 'digest method' if needed may be implied from the algorithm. For example <see cref="SecurityAlgorithms.RsaSha256"/> implies Sha256.</remarks>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        public X509SigningCredentials(X509Certificate2 certificate)
            : base(certificate)
        {
            Certificate = certificate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X509SigningCredentials"/> class.
        /// </summary>
        /// <param name="certificate">A <see cref="X509Certificate2"/> that will be used for signing.</param>
        /// <param name="algorithm">The signature algorithm to apply.</param>
        /// <remarks>the 'digest method' if needed may be implied from the algorithm. For example <see cref="SecurityAlgorithms.RsaSha256"/> implies Sha256.</remarks>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'algorithm' is null or empty.</exception>
        public X509SigningCredentials(X509Certificate2 certificate, string algorithm)
            :base(certificate, algorithm)
        {
            Certificate = certificate;
        }

        /// <summary>
        /// Gets the <see cref="X509Certificate2"/> used by this instance.
        /// </summary>
        public X509Certificate2 Certificate
        {
            get;
            private set;
        }
    }
}
