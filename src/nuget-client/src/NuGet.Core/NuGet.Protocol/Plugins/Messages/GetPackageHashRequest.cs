// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to get a package hash.
    /// </summary>
    public sealed class GetPackageHashRequest
    {
        /// <summary>
        /// Gets the hash algorithm.
        /// </summary>
        [JsonRequired]
        public string HashAlgorithm { get; }

        /// <summary>
        /// Gets the package ID.
        /// </summary>
        [JsonRequired]
        public string PackageId { get; }

        /// <summary>
        /// Gets the package source repository location.
        /// </summary>
        [JsonRequired]
        public string PackageSourceRepository { get; }

        /// <summary>
        /// Gets the package version.
        /// </summary>
        [JsonRequired]
        public string PackageVersion { get; }

        /// <summary>
        /// Initializes a new <see cref="GetPackageHashRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source repository location.</param>
        /// <param name="packageId">The package ID.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageSourceRepository" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageId" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageVersion" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="hashAlgorithm" />
        /// is either <see langword="null" /> or an empty string.</exception>
        [JsonConstructor]
        public GetPackageHashRequest(
            string packageSourceRepository,
            string packageId,
            string packageVersion,
            string hashAlgorithm)
        {
            if (string.IsNullOrEmpty(packageSourceRepository))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageSourceRepository));
            }

            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageId));
            }

            if (string.IsNullOrEmpty(packageVersion))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageVersion));
            }

            if (string.IsNullOrEmpty(hashAlgorithm))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(hashAlgorithm));
            }

            PackageId = packageId;
            PackageVersion = packageVersion;
            PackageSourceRepository = packageSourceRepository;
            HashAlgorithm = hashAlgorithm;
        }
    }
}
