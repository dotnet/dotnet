// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    /// <summary>
    /// Represents a node in the package dependency graph.
    /// </summary>
    internal abstract record DependencyNode(string Id, HashSet<DependencyNode> Children)
    {
        public abstract bool Equals(DependencyNode? other);

        public abstract override int GetHashCode();
    }

    /// <summary>
    /// Represents a project node in the dependency graph.
    /// </summary>
    internal record ProjectNode(string Id, HashSet<DependencyNode> Children)
        : DependencyNode(Id, Children)
    {
        public virtual bool Equals(ProjectNode? other)
        {
            if (other == null)
                return false;

            return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase)
                && Children.SetEquals(other.Children);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id, StringComparer.OrdinalIgnoreCase);
            foreach (var child in Children.OrderBy(c => c.Id, StringComparer.OrdinalIgnoreCase))
            {
                hash.Add(child);
            }
            return hash.ToHashCode();
        }
    }

    /// <summary>
    /// Represents a package node in the dependency graph.
    /// </summary>
    internal record PackageNode(string Id, NuGetVersion ResolvedVersion, VersionRange RequestedVersion, HashSet<DependencyNode> Children)
        : DependencyNode(Id, Children)
    {
        public virtual bool Equals(PackageNode? other)
        {
            if (other == null)
                return false;

            return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase)
                && ResolvedVersion.Equals(other.ResolvedVersion)
                && RequestedVersion.Equals(other.RequestedVersion)
                && Children.SetEquals(other.Children);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id, StringComparer.OrdinalIgnoreCase);
            hash.Add(ResolvedVersion);
            hash.Add(RequestedVersion);
            foreach (var child in Children.OrderBy(c => c.Id, StringComparer.OrdinalIgnoreCase))
            {
                hash.Add(child);
            }
            return hash.ToHashCode();
        }
    }
}
