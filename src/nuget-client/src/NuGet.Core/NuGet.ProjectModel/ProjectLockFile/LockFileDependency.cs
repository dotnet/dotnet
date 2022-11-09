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
        public string Id { get; set; }

        public NuGetVersion ResolvedVersion { get; set; }

        public VersionRange RequestedVersion { get; set; }

        public string ContentHash { get; set; }

        public PackageDependencyType Type { get; set; }

        public IList<PackageDependency> Dependencies { get; set; } = new List<PackageDependency>();

        public bool Equals(LockFileDependency other)
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

        public override bool Equals(object obj)
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
