// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// id, version, original PackageReference if available.
    /// </summary>
    public sealed class PackageCollectionItem : PackageIdentity
    {
        /// <summary>
        /// Installed package references.
        /// </summary>
        public IReadOnlyCollection<IPackageReferenceContextInfo> PackageReferences { get; }

        public PackageCollectionItem(string id, NuGetVersion version, IEnumerable<IPackageReferenceContextInfo> installedReferences)
            : base(id, version)
        {
            PackageReferences = installedReferences?.ToList()
                ?? (IReadOnlyCollection<IPackageReferenceContextInfo>)Array.Empty<IPackageReferenceContextInfo>();
        }
    }
}
