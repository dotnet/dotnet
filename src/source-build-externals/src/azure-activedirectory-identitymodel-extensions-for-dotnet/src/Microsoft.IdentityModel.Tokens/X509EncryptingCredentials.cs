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
    /// An <see cref="X509EncryptingCredentials"/> designed to construct <see cref="EncryptingCredentials"/> based on a x509 certificate.
    /// </summary>
    public class X509EncryptingCredentials : EncryptingCredentials
    {
        /// <summary>
        /// Designed to construct <see cref="EncryptingCredentials"/> based on a x509 certificate.
        /// </summary>
        /// <param name="certificate">A <see cref="X509Certificate2"/></param>
        /// <remarks>
        /// <see cref="SecurityAlgorithms.DefaultAsymmetricKeyWrapAlgorithm"/> will be used as the key wrap algorithm
        /// <see cref="SecurityAlgorithms.DefaultSymmetricEncryptionAlgorithm"/> will be used as the data encryption algorithm
        /// </remarks>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        public X509EncryptingCredentials(X509Certificate2 certificate)
            : this(certificate, SecurityAlgorithms.DefaultAsymmetricKeyWrapAlgorithm, SecurityAlgorithms.DefaultSymmetricEncryptionAlgorithm)
        {
        }

        /// <summary>
        /// Designed to construct <see cref="EncryptingCredentials"/> based on the x509 certificate, a key wrap algorithm, and data encryption algorithm.
        /// </summary>
        /// <param name="certificate">A <see cref="X509Certificate2"/></param>
        /// <param name="keyWrapAlgorithm">A key wrap algorithm</param>
        /// <param name="dataEncryptionAlgorithm">Data encryption algorithm</param>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'keyWrapAlgorithm' is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if 'dataEncryptionAlgorithm' is null or empty.</exception>
        public X509EncryptingCredentials(X509Certificate2 certificate, string keyWrapAlgorithm, string dataEncryptionAlgorithm)
            : base(certificate, keyWrapAlgorithm, dataEncryptionAlgorithm)
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
