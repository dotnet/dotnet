// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NuGet.Common;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// Basic package reader that provides the identity, min client version, and file access.
    /// </summary>
    /// <remarks>Higher level concepts used for normal development nupkgs should go at a higher level</remarks>
    public interface IPackageCoreReader : IDisposable
    {
        /// <summary>
        /// Gets the package identity.
        /// </summary>
        /// <returns>A package identity.</returns>
        PackageIdentity GetIdentity();

        /// <summary>
        /// Gets the minimum client version needed to consume the package.
        /// </summary>
        /// <returns>A NuGet version.</returns>
        NuGetVersion GetMinClientVersion();

        /// <summary>
        /// Gets zero or more package types from the .nuspec.
        /// </summary>
        /// <returns>A readonly list of package types.</returns>
        IReadOnlyList<PackageType> GetPackageTypes();

        /// <summary>
        /// Gets a file stream from the package.
        /// </summary>
        /// <returns>A stream for a file in the package.</returns>
        Stream GetStream(string path);

        /// <summary>
        /// Gets all files in the package.
        /// </summary>
        /// <returns>An enumerable of files in the package.</returns>
        IEnumerable<string> GetFiles();

        /// <summary>
        /// Gets files in a folder in the package.
        /// </summary>
        /// <param name="folder">Folder path</param>
        /// <returns>An enumerable of files under specified folder.</returns>
        IEnumerable<string> GetFiles(string folder);

        /// <summary>
        /// Gets a nuspec stream.
        /// </summary>
        /// <returns>A stream for the nuspec.</returns>
        Stream GetNuspec();

        /// <summary>
        /// Gets a nuspec file path.
        /// </summary>
        /// <returns>The nuspec file path.</returns>
        string GetNuspecFile();

        /// <summary>
        /// Copies files from a package to a new location.
        /// </summary>
        /// <param name="destination">The destination folder path.</param>
        /// <param name="packageFiles">The package files to copy.</param>
        /// <param name="extractFile">A package file extraction delegate.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>An enumerable of paths of files copied to the destination.</returns>
        IEnumerable<string> CopyFiles(
            string destination,
            IEnumerable<string> packageFiles,
            ExtractPackageFileDelegate extractFile,
            ILogger logger,
            CancellationToken token);
    }
}
