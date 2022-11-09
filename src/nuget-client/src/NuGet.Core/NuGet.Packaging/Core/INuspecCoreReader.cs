// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// A basic nuspec reader that understands ONLY the id, version, and min client version of a package.
    /// </summary>
    /// <remarks>Higher level concepts used for normal development nupkgs should go at a higher level</remarks>
    public interface INuspecCoreReader
    {
        /// <summary>
        /// Package Id
        /// </summary>
        /// <returns></returns>
        string GetId();

        /// <summary>
        /// Package Version
        /// </summary>
        NuGetVersion GetVersion();

        /// <summary>
        /// Minimum client version needed to consume the package.
        /// </summary>
        NuGetVersion GetMinClientVersion();

        /// <summary>
        /// Gets zero or more package types from the .nuspec.
        /// </summary>
        IReadOnlyList<PackageType> GetPackageTypes();

        /// <summary>
        /// Id and Version of a package.
        /// </summary>
        PackageIdentity GetIdentity();

        /// <summary>
        /// Package metadata in the nuspec
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> GetMetadata();
    }
}
