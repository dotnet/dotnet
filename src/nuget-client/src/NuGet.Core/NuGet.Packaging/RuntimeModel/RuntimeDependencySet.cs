// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NuGet.Shared;

namespace NuGet.RuntimeModel
{
    public class RuntimeDependencySet : IEquatable<RuntimeDependencySet>
    {
        /// <summary>
        /// Package Id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Package dependencies
        /// </summary>
        public IReadOnlyDictionary<string, RuntimePackageDependency> Dependencies { get; }

        public RuntimeDependencySet(string id)
            : this(id, Enumerable.Empty<RuntimePackageDependency>())
        {
        }

        public RuntimeDependencySet(string id, IEnumerable<RuntimePackageDependency> dependencies)
        {
            Id = id;
            Dependencies = new ReadOnlyDictionary<string, RuntimePackageDependency>(dependencies.ToDictionary(d => d.Id, StringComparer.OrdinalIgnoreCase));
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

        public RuntimeDependencySet Clone()
        {
            return new RuntimeDependencySet(Id, Dependencies.Values.Select(d => d.Clone()));
        }

        public override string ToString()
        {
            return $"{Id} -> {string.Join(",", Dependencies.Select(d => d.Value.Id + " " + d.Value.VersionRange.ToString()))}";
        }
    }
}
