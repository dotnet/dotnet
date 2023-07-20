// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.Packaging
{
    public class NupkgMetadataFile : IEquatable<NupkgMetadataFile>
    {
        public int Version { get; set; } = NupkgMetadataFileFormat.Version;

        public string ContentHash { get; set; }

        public string Source { get; set; }

        public bool Equals(NupkgMetadataFile other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Version == other.Version &&
                StringComparer.Ordinal.Equals(ContentHash, other.ContentHash) &&
                StringComparer.Ordinal.Equals(Source, other.Source);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NupkgMetadataFile);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Version);
            combiner.AddObject(ContentHash);
            combiner.AddObject(Source);

            return combiner.CombinedHash;
        }
    }
}
