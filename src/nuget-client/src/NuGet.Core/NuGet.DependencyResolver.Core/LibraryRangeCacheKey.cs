// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Shared;

namespace NuGet.DependencyResolver
{
    /// <summary>
    /// Helper type to hold a library range and framework.
    /// </summary>
    public readonly struct LibraryRangeCacheKey : IEquatable<LibraryRangeCacheKey>
    {
        public LibraryRangeCacheKey(LibraryRange range, NuGetFramework framework, string? alias)
        {
            Framework = framework;
            Alias = alias ?? string.Empty; // alias may be passed as both null or empty and we need to treat them equivalently.
            LibraryRange = range;
        }

        /// <summary>
        /// Target framework
        /// </summary>
        public NuGetFramework Framework { get; }

        /// <summary>
        /// Library range information.
        /// </summary>
        public LibraryRange LibraryRange { get; }

        public string? Alias { get; }


        public override bool Equals(object? obj)
        {
            return obj is LibraryRangeCacheKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();
            combiner.AddObject(LibraryRange);
            combiner.AddObject(Framework);
            combiner.AddObject(Alias);
            return combiner.CombinedHash;
        }

        public bool Equals(LibraryRangeCacheKey other)
        {
            return LibraryRange.Equals(other.LibraryRange)
                && Framework.Equals(other.Framework)
                && StringComparer.OrdinalIgnoreCase.Equals(Alias, other.Alias);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", LibraryRange, Framework);
        }

        public static bool operator ==(LibraryRangeCacheKey left, LibraryRangeCacheKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LibraryRangeCacheKey left, LibraryRangeCacheKey right)
        {
            return !(left == right);
        }
    }
}
