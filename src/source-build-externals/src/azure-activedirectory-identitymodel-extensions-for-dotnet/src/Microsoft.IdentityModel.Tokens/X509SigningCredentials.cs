// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
