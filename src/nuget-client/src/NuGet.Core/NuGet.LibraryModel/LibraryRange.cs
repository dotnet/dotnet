// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.LibraryModel
{
    public class LibraryRange : IEquatable<LibraryRange>
    {
        public LibraryRange()
        {
        }

        [SetsRequiredMembers]
        public LibraryRange(string name) : this(name, null, LibraryDependencyTarget.All)
        {
        }

        [SetsRequiredMembers]
        public LibraryRange(string name, LibraryDependencyTarget typeConstraint) : this(name, null, typeConstraint)
        {
        }

        [SetsRequiredMembers]
        public LibraryRange(string name, VersionRange? versionRange, LibraryDependencyTarget typeConstraint)
        {
            Name = name;
            VersionRange = versionRange;
            TypeConstraint = typeConstraint;
        }

        [SetsRequiredMembers]
        public LibraryRange(LibraryRange other)
        {
            Name = other.Name;
            VersionRange = other.VersionRange;
            TypeConstraint = other.TypeConstraint;
        }

        public required string Name { get; init; }

        // Null is used for all, CLI still has code expecting this
        public VersionRange? VersionRange { get; init; }

        public LibraryDependencyTarget TypeConstraint { get; init; } = LibraryDependencyTarget.All;

        public override string ToString()
        {
            var output = Name;
            if (VersionRange != null)
            {
                output = $"{output} {VersionRange.ToNonSnapshotRange().PrettyPrint()}";
            }

            // Append the type constraint in a user friendly way if one exists
            var contraintString = string.Empty;

            switch (TypeConstraint)
            {
                case LibraryDependencyTarget.Reference:
                    contraintString = LibraryType.Reference;
                    break;
                case LibraryDependencyTarget.ExternalProject:
                    contraintString = LibraryType.ExternalProject;
                    break;
                case LibraryDependencyTarget.Project:
                case LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject:
                    contraintString = LibraryType.Project;
                    break;
            }

            if (!string.IsNullOrEmpty(contraintString))
            {
                output = $"{contraintString}/{output}";
            }

            return output;
        }

        public string ToLockFileDependencyGroupString()
        {
            StringBuilder sb = StringBuilderPool.Shared.Rent(256);

            sb.Append(Name);

            if (VersionRange != null)
            {
                if (VersionRange.HasLowerBound)
                {
                    if (VersionRange.IsMinInclusive)
                    {
                        sb.Append(" >= ");
                    }
                    else
                    {
                        sb.Append(" > ");
                    }

                    if (VersionRange.IsFloating)
                    {
                        VersionRange.Float.ToString(sb);
                    }
                    else
                    {
                        sb.Append(VersionRange.MinVersion.ToNormalizedString());
                    }
                }

                if (VersionRange.HasUpperBound)
                {
                    sb.Append(VersionRange.IsMaxInclusive ? " <= " : " < ");
                    sb.Append(VersionRange.MaxVersion.ToNormalizedString());
                }
            }

            return StringBuilderPool.Shared.ToStringAndReturn(sb);
        }

        /// <summary>
        /// True if the type constraint allows the flag.
        /// </summary>
        public bool TypeConstraintAllows(LibraryDependencyTarget flag)
        {
            return (TypeConstraint & flag) == flag;
        }

        /// <summary>
        /// True if any flags are allowed by the constraint.
        /// </summary>
        public bool TypeConstraintAllowsAnyOf(LibraryDependencyTarget flag)
        {
            return (TypeConstraint & flag) != LibraryDependencyTarget.None;
        }

        public bool Equals(LibraryRange? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return TypeConstraint == other.TypeConstraint
                && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
                && Equals(VersionRange, other.VersionRange);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LibraryRange);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(Name);
            combiner.AddObject(VersionRange);
            combiner.AddObject((int)TypeConstraint);

            return combiner.CombinedHash;
        }

        public static bool operator ==(LibraryRange? left, LibraryRange? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LibraryRange? left, LibraryRange? right)
        {
            return !Equals(left, right);
        }
    }
}
