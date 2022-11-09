// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.Packaging.Core
{
    public class RepositoryMetadata : IEquatable<RepositoryMetadata>
    {
        public RepositoryMetadata()
        {

        }
        public RepositoryMetadata(string type, string url, string branch, string commit)
        {
            Type = type;
            Url = url;
            Branch = branch;
            Commit = commit;
        }

        public string Type { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Branch { get; set; } = string.Empty;

        public string Commit { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return Equals(obj as RepositoryMetadata);
        }

        public static bool operator ==(RepositoryMetadata a, RepositoryMetadata b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(RepositoryMetadata a, RepositoryMetadata b)
        {
            return !(a == b);
        }

        public bool Equals(RepositoryMetadata other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase) &&
                Url == other.Url &&
                Branch == other.Branch &&
                Commit == other.Commit;
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Type, StringComparer.OrdinalIgnoreCase);
            combiner.AddObject(Url);
            combiner.AddObject(Branch);
            combiner.AddObject(Commit);

            return combiner.CombinedHash;
        }
    }
}
