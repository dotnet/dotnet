// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace NuGet.Repositories
{
    /// <summary>
    /// Represents a package installed to the user global folder, or a fallback folder.
    /// Components of the package are cached and across all restores.
    /// </summary>
    public class LocalPackageInfo
    {
        private readonly Lazy<NuspecReader> _nuspec;
        private readonly Lazy<IReadOnlyList<string>> _files;
        private readonly Lazy<string> _sha512;
        private readonly Lazy<RuntimeGraph> _runtimeGraph;

        public LocalPackageInfo(
            string packageId,
            NuGetVersion version,
            string path,
            string manifestPath,
            string zipPath,
            string sha512Path,
            Lazy<NuspecReader> nuspec,
            Lazy<IReadOnlyList<string>> files,
            Lazy<string> sha512,
            Lazy<RuntimeGraph> runtimeGraph)
        {
            Id = packageId;
            Version = version;
            ExpandedPath = path;
            ManifestPath = manifestPath;
            ZipPath = zipPath;
            Sha512Path = sha512Path;
            _nuspec = nuspec;
            _files = files;
            _sha512 = sha512;
            _runtimeGraph = runtimeGraph;
        }

        public string Id { get; }

        public NuGetVersion Version { get; }

        public string ExpandedPath { get; set; }

        public string ManifestPath { get; }

        public string ZipPath { get; }

        public string Sha512Path { get; }

        /// <summary>
        /// Caches the nuspec reader.
        /// If the nuspec does not exist this will throw a friendly exception.
        /// </summary>
        public NuspecReader Nuspec => _nuspec.Value;

        /// <summary>
        /// Package files with OPC files filtered out.
        /// Cached to avoid reading the same files multiple times.
        /// </summary>
        public IReadOnlyList<string> Files => _files.Value;

        /// <summary>
        /// SHA512 of the package.
        /// </summary>
        public string Sha512 => _sha512.Value;

        /// <summary>
        /// runtime.json
        /// </summary>
        /// <remarks>Returns null if runtime.json does not exist in the package.</remarks>
        public RuntimeGraph RuntimeGraph => _runtimeGraph.Value;

        public override string ToString()
        {
            return Id + " " + Version + " (" + (ManifestPath ?? ZipPath) + ")";
        }
    }
}
