// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.Packaging
{
    public sealed class FrameworkReference : IEquatable<FrameworkReference>, IComparer<FrameworkReference>, IComparable<FrameworkReference>
    {
        public string Name { get; }

        public FrameworkReference(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Compare(FrameworkReference x, FrameworkReference y)
        {
            return ComparisonUtility.FrameworkReferenceNameComparer.Compare(x.Name, y.Name);
        }

        public bool Equals(FrameworkReference other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ComparisonUtility.FrameworkReferenceNameComparer.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FrameworkReference);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();
            combiner.AddObject(Name, ComparisonUtility.FrameworkReferenceNameComparer);
            return combiner.CombinedHash;
        }

        public int CompareTo(FrameworkReference other)
        {
            return Compare(this, other);
        }
    }
}
