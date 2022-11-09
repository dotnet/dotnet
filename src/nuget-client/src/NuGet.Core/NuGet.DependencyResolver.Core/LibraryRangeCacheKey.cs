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
    /// Helper class to hold a library range and framework.
    /// </summary>
    public class LibraryRangeCacheKey : IEquatable<LibraryRangeCacheKey>
    {
        public LibraryRangeCacheKey(LibraryRange range, NuGetFramework framework)
        {
            Framework = framework;
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

        public override bool Equals(object obj)
        {
            return Equals(obj as LibraryRangeCacheKey);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.GetHashCode(LibraryRange, Framework);
        }

        public bool Equals(LibraryRangeCacheKey other)
        {
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (Object.ReferenceEquals(null, other))
            {
                return false;
            }

            return LibraryRange.Equals(other.LibraryRange)
                && Framework.Equals(other.Framework);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", LibraryRange, Framework);
        }
    }
}
