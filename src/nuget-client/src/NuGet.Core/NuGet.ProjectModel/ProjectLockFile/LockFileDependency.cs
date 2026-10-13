// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.ProjectModel.ProjectLockFile;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public class LockFileDependency : IEquatable<LockFileDependency>
    {
        /// <summary>
        /// The package or project ID represented by this lock file entry.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The exact package version selected during restore. Project dependencies do not record a resolved version
        /// in the packages lock file.
        /// </summary>
        public NuGetVersion? ResolvedVersion { get; set; }

        /// <summary>
        /// The version range requested by the project. This is <see langword="null"/> when no version was requested,
        /// such as for transitive and project dependencies.
        /// </summary>
        public VersionRange? RequestedVersion { get; set; }

        /// <summary>
        /// The hash used to verify the restored package content. This is <see langword="null"/> for project dependencies.
        /// </summary>
        public string? ContentHash { get; set; }

        public PackageDependencyType Type { get; set; }

        /// <summary>
        /// The direct dependencies recorded for this package or project entry.
        /// </summary>
        public IList<PackageDependency> Dependencies { get; set; } = new List<PackageDependency>();

        public bool Equals(LockFileDependency? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return LockFileDependencyComparerWithoutContentHash.Default.Equals(this, other) &&
                ContentHash == other.ContentHash;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LockFileDependency);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();
            combiner.AddObject(LockFileDependencyComparerWithoutContentHash.Default.GetHashCode(this));
            combiner.AddObject(ContentHash);
            return combiner.CombinedHash;
        }
    }
}
