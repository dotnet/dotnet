// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Protocol.Core.Types;

namespace NuGet.Indexing
{
    /// <summary>
    /// A strategy of creating a "merged" package metadata object out of two coming from different sources.
    /// </summary>
    public interface IPackageSearchMetadataSplicer
    {
        /// <summary>
        /// Given two instances of package metadata with the same id create merged one.
        /// </summary>
        /// <param name="lhs">A package metadata</param>
        /// <param name="rhs">Another package metadata with the same id</param>
        /// <returns>Unified package metadata object aggregating attributes from both input objects</returns>
        IPackageSearchMetadata MergeEntries(IPackageSearchMetadata lhs, IPackageSearchMetadata rhs);
    }
}
