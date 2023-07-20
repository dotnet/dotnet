// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Shared;

namespace NuGet.RuntimeModel
{
    /// <remarks>
    /// Immutable.
    /// </remarks>
    public sealed class RuntimeDependencySet : IEquatable<RuntimeDependencySet>
    {
        private static readonly IReadOnlyDictionary<string, RuntimePackageDependency> EmptyDependencies = new Dictionary<string, RuntimePackageDependency>();

        /// <summary>
        /// Package Id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Package dependencies
        /// </summary>
        public IReadOnlyDictionary<string, RuntimePackageDependency> Dependencies { get; }

        public RuntimeDependencySet(string id)
            : this(id, (IReadOnlyDictionary<string, RuntimePackageDependency>)null)
        {
        }

        public RuntimeDependencySet(string id, IEnumerable<RuntimePackageDependency> dependencies)
            : this(id, dependencies?.ToDictionary(d => d.Id, StringComparer.OrdinalIgnoreCase))
        {
        }

        private RuntimeDependencySet(string id, IReadOnlyDictionary<string, RuntimePackageDependency> dependencies)
        {
            Id = id;
            Dependencies = dependencies is null or { Count: 0 } ? EmptyDependencies : dependencies;
        }

        public bool Equals(RuntimeDependencySet other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return string.Equals(other.Id, Id, StringComparison.OrdinalIgnoreCase)
                && Dependencies.OrderedEquals(other.Dependencies, p => p.Key, StringComparer.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RuntimeDependencySet);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();
            combiner.AddObject(Id, StringComparer.OrdinalIgnoreCase);
            combiner.AddDictionary(Dependencies);
            return combiner.CombinedHash;
        }

        [Obsolete("This type is immutable, so there is no need or point to clone it.")]
        public RuntimeDependencySet Clone()
        {
            return this;
        }

        public override string ToString()
        {
            return $"{Id} -> {string.Join(",", Dependencies.Select(d => d.Value.Id + " " + d.Value.VersionRange.ToString()))}";
        }
    }
}
