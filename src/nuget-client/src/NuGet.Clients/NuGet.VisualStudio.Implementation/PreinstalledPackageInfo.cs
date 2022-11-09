// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using NuGet.Versioning;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// The information that represents a single preinstalled package (already on disk).
    /// </summary>
    internal sealed class PreinstalledPackageInfo
    {
        /// <summary>
        /// Information for a single preinstalled package that will have its assembly references added and its
        /// dependencies ignored.
        /// </summary>
        /// <param name="id">The package Id.</param>
        /// <param name="version">The package version.</param>
        public PreinstalledPackageInfo(string id, string version)
            :
                this(id, version, skipAssemblyReferences: false, ignoreDependencies: true)
        {
        }

        /// <summary>
        /// Information for a single preinstalled package.
        /// </summary>
        /// <param name="id">The package Id.</param>
        /// <param name="version">The package version.</param>
        /// <param name="skipAssemblyReferences">
        /// A boolean indicating whether assembly references from the package
        /// should be skipped.
        /// </param>
        /// <param name="ignoreDependencies">
        /// A boolean indicating whether dependencies from the package should be
        /// ignored.
        /// </param>
        public PreinstalledPackageInfo(string id, string version, bool skipAssemblyReferences, bool ignoreDependencies)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(id));
            Debug.Assert(!String.IsNullOrWhiteSpace(version));

            Id = id;
            Version = NuGetVersion.Parse(version);
            SkipAssemblyReferences = skipAssemblyReferences;
            IgnoreDependencies = ignoreDependencies;
        }

        public string Id { get; private set; }
        public NuGetVersion Version { get; private set; }
        public bool SkipAssemblyReferences { get; private set; }
        public bool IgnoreDependencies { get; private set; }
    }
}
