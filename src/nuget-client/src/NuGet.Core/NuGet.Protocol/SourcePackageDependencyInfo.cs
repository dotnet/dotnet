// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Protocol.Core.Types
{
    public class SourcePackageDependencyInfo : PackageDependencyInfo
    {
        public SourcePackageDependencyInfo(
            string id,
            NuGetVersion version,
            IEnumerable<PackageDependency> dependencies,
            bool listed,
            SourceRepository source)
            : this(
                  new PackageIdentity(id, version),
                  dependencies,
                  listed,
                  source,
                  downloadUri: null,
                  packageHash: null)
        {
        }

        public SourcePackageDependencyInfo(
            string id,
            NuGetVersion version,
            IEnumerable<PackageDependency> dependencies,
            bool listed,
            SourceRepository source,
            Uri downloadUri,
            string packageHash)
            : this(
                  new PackageIdentity(id, version),
                  dependencies,
                  listed,
                  source,
                  downloadUri,
                  packageHash)
        {
        }

        public SourcePackageDependencyInfo(
            PackageIdentity identity,
            IEnumerable<PackageDependency> dependencies,
            bool listed,
            SourceRepository source,
            Uri downloadUri,
            string packageHash)
            : base(identity, dependencies)
        {
            Listed = listed;
            Source = source;
            DownloadUri = downloadUri;
            PackageHash = packageHash;
        }

        /// <summary>
        /// True if the package is listed and shown in search.
        /// </summary>
        /// <remarks>This property only applies to online sources.</remarks>
        public bool Listed { get; }

        /// <summary>
        /// Source repository the dependency information was retrieved from.
        /// </summary>
        public SourceRepository Source { get; }

        /// <summary>
        /// The HTTP, UNC, or local file URI to the package nupkg.
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri DownloadUri { get; }

        /// <summary>
        /// Package hash
        /// </summary>
        /// <remarks>Optional</remarks>
        public string PackageHash { get; }
    }
}
