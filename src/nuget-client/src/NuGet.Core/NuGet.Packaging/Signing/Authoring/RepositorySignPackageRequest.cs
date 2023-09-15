// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public sealed class RepositorySignPackageRequest : SignPackageRequest
    {
        /// <summary>
        /// Gets the repository V3 service index URL.
        /// </summary>
        public Uri V3ServiceIndexUrl { get; }

        /// <summary>
        /// Gets a read-only list of package owners.
        /// </summary>
        public IReadOnlyList<string> PackageOwners { get; }

        /// <summary>
        /// Gets the signature type.
        /// </summary>
        public override SignatureType SignatureType => SignatureType.Repository;

        /// <summary>
        /// Instantiates a new instance of the <see cref="RepositorySignPackageRequest" /> class.
        /// </summary>
        /// <param name="certificate">The signing certificate.</param>
        /// <param name="signatureHashAlgorithm">The signature hash algorithm.</param>
        /// <param name="timestampHashAlgorithm">The timestamp hash algorithm.</param>
        /// <param name="v3ServiceIndexUrl">The V3 service index URL.</param>
        /// <param name="packageOwners">A read-only list of package owners.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="certificate" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="signatureHashAlgorithm" />
        /// is invalid.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="timestampHashAlgorithm" />
        /// is invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="v3ServiceIndexUrl" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="v3ServiceIndexUrl" />
        /// is neither absolute nor HTTPS.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageOwners" />
        /// is either empty or contains an invalid value.</exception>
        public RepositorySignPackageRequest(
            X509Certificate2 certificate,
            HashAlgorithmName signatureHashAlgorithm,
            HashAlgorithmName timestampHashAlgorithm,
            Uri v3ServiceIndexUrl,
            IReadOnlyList<string> packageOwners)
            : base(
                  certificate,
                  signatureHashAlgorithm,
                  timestampHashAlgorithm)
        {
            if (v3ServiceIndexUrl == null)
            {
                throw new ArgumentNullException(nameof(v3ServiceIndexUrl));
            }

            if (!v3ServiceIndexUrl.IsAbsoluteUri)
            {
                throw new ArgumentException(Strings.InvalidUrl, nameof(v3ServiceIndexUrl));
            }

            if (!string.Equals(v3ServiceIndexUrl.Scheme, "https", StringComparison.Ordinal))
            {
                throw new ArgumentException(Strings.InvalidUrl, nameof(v3ServiceIndexUrl));
            }

            if (packageOwners != null)
            {
                if (packageOwners.Any(packageOwner => string.IsNullOrWhiteSpace(packageOwner)))
                {
                    throw new ArgumentException(Strings.NuGetPackageOwnersInvalidValue, nameof(packageOwners));
                }

                if (!packageOwners.Any())
                {
                    packageOwners = null;
                }
            }

            V3ServiceIndexUrl = v3ServiceIndexUrl;
            PackageOwners = packageOwners;
        }
    }
}
