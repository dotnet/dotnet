// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal enum UpdateLevel { NoUpdate, Major, Minor, Patch };

    /// <summary>
    /// A class to simplify holding all of the information
    /// about a package reference when using list
    /// </summary>
    [DebuggerDisplay("{Name} {OriginalRequestedVersion}")]
    internal class InstalledPackageReference
    {
        internal string Name { get; }
        internal string OriginalRequestedVersion { get; set; }
        internal IPackageSearchMetadata ResolvedPackageMetadata { get; set; }
        internal IPackageSearchMetadata LatestPackageMetadata { get; set; }
        internal bool AutoReference { get; set; }
        internal UpdateLevel UpdateLevel { get; set; }
        internal bool IsVersionOverride { get; set; }

        /// <summary>
        /// A constructor that takes a name of a package
        /// </summary>
        /// <param name="name">The name of the package</param>
        public InstalledPackageReference(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
