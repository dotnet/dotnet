// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.RuntimeModel
{
    /// <summary>
    /// A package dependency for a specific RID.
    /// </summary>
    /// <remarks>
    /// Immutable.
    /// </remarks>
    public sealed class RuntimePackageDependency : IEquatable<RuntimePackageDependency>
    {
        /// <summary>
        /// Dependency package id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Dependency version constraint.
        /// </summary>
        public VersionRange VersionRange { get; }

        public RuntimePackageDependency(string id, VersionRange versionRange)
        {
            Id = id;
            VersionRange = versionRange;
        }

        [Obsolete("This type is immutable, so there is no need or point to clone it.")]
        public RuntimePackageDependency Clone()
        {
            return this;
        }

        public override string ToString()
        {
            return $"{Id} {VersionRange}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RuntimePackageDependency);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(Id);
            combiner.AddObject(VersionRange);

            return combiner.CombinedHash;
        }

        public bool Equals(RuntimePackageDependency other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return other != null &&
                string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase) &&
                VersionRange.Equals(other.VersionRange);
        }
    }
}
