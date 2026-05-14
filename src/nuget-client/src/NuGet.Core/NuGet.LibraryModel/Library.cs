// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NuGet.LibraryModel
{
    public class Library
    {
        public static readonly IEqualityComparer<Library> IdentityComparer = new LibraryIdentityComparer();

        public required LibraryRange LibraryRange { get; set; }
        public required LibraryIdentity Identity { get; set; }
        public required IEnumerable<LibraryDependency> Dependencies { get; set; }
        public bool Resolved { get; set; } = true;
        public string? Path { get; set; }

        public IDictionary<string, object> Items { get; set; } = new Dictionary<string, object>();

        public Library()
        {
        }

        [SetsRequiredMembers]
        public Library(LibraryRange libraryRange, LibraryIdentity identity, IEnumerable<LibraryDependency> dependencies)
        {
            LibraryRange = libraryRange ?? throw new ArgumentNullException(nameof(libraryRange));
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        public object this[string key]
        {
            get { return Items[key]; }
            set
            {
                if (value != null)
                {
                    Items[key] = value;
                }
            }
        }

        public override string ToString()
        {
            if (Identity is null)
            {
                if (LibraryRange is null)
                {
                    return typeof(Library).FullName!;
                }
                return LibraryRange.ToString();
            }

            return Identity + " (" + LibraryRange + ")";
        }

        private class LibraryIdentityComparer : IEqualityComparer<Library>
        {
            public bool Equals(Library? x, Library? y)
            {
                if (x is null && y is null) return true;
                if (x is null || y is null) return false;

                return x.Identity.Equals(y.Identity);
            }

            public int GetHashCode(Library obj)
            {
                return obj.Identity.GetHashCode();
            }
        }
    }
}
