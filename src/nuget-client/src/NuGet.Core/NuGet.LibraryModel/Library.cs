// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.LibraryModel
{
    public class Library
    {
        public static readonly IEqualityComparer<Library> IdentityComparer = new LibraryIdentityComparer();

        public LibraryRange LibraryRange { get; set; }
        public LibraryIdentity Identity { get; set; }
        public IEnumerable<LibraryDependency> Dependencies { get; set; }
        public bool Resolved { get; set; } = true;
        public string Path { get; set; }

        public IDictionary<string, object> Items { get; set; } = new Dictionary<string, object>();

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
            if (Identity == null)
            {
                return LibraryRange?.ToString();
            }

            return Identity + " (" + LibraryRange + ")";
        }

        private class LibraryIdentityComparer : IEqualityComparer<Library>
        {
            public bool Equals(Library x, Library y)
            {
                return x.Identity.Equals(y.Identity);
            }

            public int GetHashCode(Library obj)
            {
                return obj.Identity.GetHashCode();
            }
        }
    }
}
