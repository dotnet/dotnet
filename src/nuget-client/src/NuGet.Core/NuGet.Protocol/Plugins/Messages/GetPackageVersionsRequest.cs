// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request for package versions.
    /// </summary>
    public sealed class GetPackageVersionsRequest
    {
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
        /// Initializes a new <see cref="GetPackageVersionsRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source repository location.</param>
        /// <param name="packageId">The package ID.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageSourceRepository" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageId" />
        /// is either <see langword="null" /> or an empty string.</exception>
        [JsonConstructor]
        public GetPackageVersionsRequest(
            string packageSourceRepository,
            string packageId)
        {
            if (string.IsNullOrEmpty(packageSourceRepository))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageSourceRepository));
            }

            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageId));
            }

            PackageSourceRepository = packageSourceRepository;
            PackageId = packageId;
        }
    }
}
