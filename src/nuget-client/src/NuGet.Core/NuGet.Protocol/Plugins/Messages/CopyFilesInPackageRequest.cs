// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to copy files in a package to a specified destination.
    /// </summary>
    public sealed class CopyFilesInPackageRequest
    {
        /// <summary>
        /// Gets the destination folder path.
        /// </summary>
        [JsonRequired]
        public string DestinationFolderPath { get; }

        /// <summary>
        /// Gets the files in the package to be copied.
        /// </summary>
        [JsonRequired]
        public IEnumerable<string> FilesInPackage { get; }

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
        /// Initializes a new <see cref="CopyFilesInPackageRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source repository location.</param>
        /// <param name="packageId">The package ID.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <param name="filesInPackage">The files in the package to be copied.</param>
        /// <param name="destinationFolderPath">The destination folder path.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageSourceRepository" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageId" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageVersion" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filesInPackage" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="destinationFolderPath" />
        /// is either <see langword="null" /> or an empty string.</exception>
        [JsonConstructor]
        public CopyFilesInPackageRequest(
            string packageSourceRepository,
            string packageId,
            string packageVersion,
            IEnumerable<string> filesInPackage,
            string destinationFolderPath)
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

            if (filesInPackage == null || !filesInPackage.Any())
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(filesInPackage));
            }

            if (string.IsNullOrEmpty(destinationFolderPath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(destinationFolderPath));
            }

            PackageId = packageId;
            PackageVersion = packageVersion;
            PackageSourceRepository = packageSourceRepository;
            FilesInPackage = filesInPackage;
            DestinationFolderPath = destinationFolderPath;
        }
    }
}
