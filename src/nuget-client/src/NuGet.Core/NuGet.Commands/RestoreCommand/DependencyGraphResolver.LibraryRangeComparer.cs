// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using NuGet.LibraryModel;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Commands
{
    internal sealed partial class DependencyGraphResolver
    {
        /// <summary>
        /// Represents an <see cref="IEqualityComparer{T}" /> of <see cref="LibraryRange" /> that considers them to be equal based on the same functionality of <see cref="LibraryRange.ToString" />.
        /// </summary>
        public sealed class LibraryRangeComparer : IEqualityComparer<LibraryRange>
        {
            private LibraryRangeComparer()
            {
            }

            /// <summary>
            /// Gets an instance of <see cref="LibraryRangeComparer" />.
            /// </summary>
            public static LibraryRangeComparer Instance { get; } = new LibraryRangeComparer();

            public bool Equals(LibraryRange? x, LibraryRange? y)
            {
                if (x == null || y == null || x.VersionRange == null || y.VersionRange == null)
                {
                    return false;
                }

                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                // All of this logic is copied from LibraryRange.ToString()
                LibraryDependencyTarget typeConstraint1 = LibraryDependencyTarget.None;
                LibraryDependencyTarget typeConstraint2 = LibraryDependencyTarget.None;

                switch (x.TypeConstraint)
                {
                    case LibraryDependencyTarget.Reference:
                        typeConstraint1 = LibraryDependencyTarget.Reference;
                        break;

                    case LibraryDependencyTarget.ExternalProject:
                        typeConstraint1 = LibraryDependencyTarget.ExternalProject;
                        break;

                    case LibraryDependencyTarget.Project:
                    case LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject:
                        typeConstraint1 = LibraryDependencyTarget.Project;
                        break;
                }

                switch (y.TypeConstraint)
                {
                    case LibraryDependencyTarget.Reference:
                        typeConstraint2 = LibraryDependencyTarget.Reference;
                        break;

                    case LibraryDependencyTarget.ExternalProject:
                        typeConstraint2 = LibraryDependencyTarget.ExternalProject;
                        break;

                    case LibraryDependencyTarget.Project:
                    case LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject:
                        typeConstraint2 = LibraryDependencyTarget.Project;
                        break;
                }

                return typeConstraint1 == typeConstraint2
                    && x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase)
                    && x.VersionRange.Equals(y.VersionRange);
            }

            public int GetHashCode(LibraryRange obj)
            {
                LibraryDependencyTarget typeConstraint = LibraryDependencyTarget.None;

                switch (obj.TypeConstraint)
                {
                    case LibraryDependencyTarget.Reference:
                        typeConstraint = LibraryDependencyTarget.Reference;
                        break;

                    case LibraryDependencyTarget.ExternalProject:
                        typeConstraint = LibraryDependencyTarget.ExternalProject;
                        break;

                    case LibraryDependencyTarget.Project:
                    case LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject:
                        typeConstraint = LibraryDependencyTarget.Project;
                        break;
                }

                VersionRange versionRange = obj.VersionRange ?? VersionRange.None;

                HashCodeCombiner combiner = new();

                combiner.AddObject((int)typeConstraint);
                combiner.AddStringIgnoreCase(obj.Name);
                combiner.AddObject(versionRange);

                return combiner.CombinedHash;
            }
        }
    }
}
