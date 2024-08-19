// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    /// <summary>
    /// Represents a node in the package dependency graph.
    /// </summary>
    internal class DependencyNode
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public HashSet<DependencyNode> Children { get; set; }

        public DependencyNode(string id, string version)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Children = new HashSet<DependencyNode>(new DependencyNodeComparer());
        }

        public override int GetHashCode()
        {
            var hashCodeCombiner = new HashCodeCombiner();
            hashCodeCombiner.AddObject(Id);
            hashCodeCombiner.AddObject(Version);
            hashCodeCombiner.AddUnorderedSequence(Children);
            return hashCodeCombiner.CombinedHash;
        }
    }

    internal class DependencyNodeComparer : IEqualityComparer<DependencyNode>
    {
        public bool Equals(DependencyNode? x, DependencyNode? y)
        {
            if (x == null || y == null)
                return false;

            return string.Equals(x.Id, y.Id, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(DependencyNode obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
