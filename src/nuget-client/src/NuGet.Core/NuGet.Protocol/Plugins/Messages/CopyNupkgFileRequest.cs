// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to copy a .nupkg file.
    /// </summary>
    public sealed class CopyNupkgFileRequest
    {
        /// <summary>
        /// Gets the destination file path for the .nupkg file.
        /// </summary>
        [JsonRequired]
        public string DestinationFilePath { get; }

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
        /// Initializes a new <see cref="CopyNupkgFileRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source repository location.</param>
        /// <param name="packageId">The package ID.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <param name="destinationFilePath">The destination file path for the .nupkg file.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageSourceRepository" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageId" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageVersion" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="destinationFilePath" />
        /// is either <see langword="null" /> or an empty string.</exception>
        [JsonConstructor]
        public CopyNupkgFileRequest(
            string packageSourceRepository,
            string packageId,
            string packageVersion,
            string destinationFilePath)
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

            if (string.IsNullOrEmpty(destinationFilePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(destinationFilePath));
            }

            PackageId = packageId;
            PackageVersion = packageVersion;
            PackageSourceRepository = packageSourceRepository;
            DestinationFilePath = destinationFilePath;
        }
    }
}
