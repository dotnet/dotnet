// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.LibraryModel;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Commands
{
    /// <summary>
    /// ResolvedDependencyKey represents a node in the graph, the edge containing
    /// the dependency constraint, and the child node that was resolved based 
    /// on this constraint.
    /// 
    /// (Parent Node) --(Range Constraint)--> (Resolved Child Node)
    /// </summary>
    public class ResolvedDependencyKey : IEquatable<ResolvedDependencyKey>
    {
        /// <summary>
        /// Parent node.
        /// </summary>
        public LibraryIdentity Parent { get; }

        /// <summary>
        /// Dependency range from the parent on the child.
        /// </summary>
        public VersionRange Range { get; }

        /// <summary>
        /// Child node.
        /// </summary>
        public LibraryIdentity Child { get; }

        public ResolvedDependencyKey(LibraryIdentity parent, VersionRange range, LibraryIdentity child)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Range = range ?? VersionRange.All;
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public bool Equals(ResolvedDependencyKey other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Parent.Equals(other.Parent)
                && Child.Equals(other.Child)
                && Range.Equals(other.Range);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ResolvedDependencyKey);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.GetHashCode(Parent, Range, Child);
        }
    }
}
