// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public sealed class AuthorSignPackageRequest : SignPackageRequest
    {
        /// <summary>
        /// Gets the signature type.
        /// </summary>
        public override SignatureType SignatureType => SignatureType.Author;

        /// <summary>
        /// Instantiates a new instance of the <see cref="AuthorSignPackageRequest" /> class.
        /// </summary>
        /// <param name="certificate">The signing certificate.</param>
        /// <param name="hashAlgorithm">The signature and timestamp hash algorithm.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="certificate" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="hashAlgorithm" />
        /// is invalid.</exception>
        public AuthorSignPackageRequest(
            X509Certificate2 certificate,
            HashAlgorithmName hashAlgorithm)
            : base(
                  certificate,
                  hashAlgorithm,
                  hashAlgorithm)
        {
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="AuthorSignPackageRequest" /> class.
        /// </summary>
        /// <param name="certificate">The signing certificate.</param>
        /// <param name="signatureHashAlgorithm">The signature hash algorithm.</param>
        /// <param name="timestampHashAlgorithm">The timestamp hash algorithm.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="certificate" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="signatureHashAlgorithm" />
        /// is invalid.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="timestampHashAlgorithm" />
        /// is invalid.</exception>
        public AuthorSignPackageRequest(
            X509Certificate2 certificate,
            HashAlgorithmName signatureHashAlgorithm,
            HashAlgorithmName timestampHashAlgorithm)
            : base(
                  certificate,
                  signatureHashAlgorithm,
                  timestampHashAlgorithm)
        {
        }
    }
}
