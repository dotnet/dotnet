// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.Frameworks
{
    public class CompatibilityMappingComparer : IEqualityComparer<OneWayCompatibilityMappingEntry>
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static CompatibilityMappingComparer Instance { get; } = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Use singleton CompatibilityMappingComparer.Instance instead")]
        public CompatibilityMappingComparer()
        {
        }

        public bool Equals(OneWayCompatibilityMappingEntry? x, OneWayCompatibilityMappingEntry? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null)
                || ReferenceEquals(y, null))
            {
                return false;
            }

            var comparer = FrameworkRangeComparer.Instance;

            return comparer.Equals(x.TargetFrameworkRange, y.TargetFrameworkRange)
                   && comparer.Equals(x.SupportedFrameworkRange, y.SupportedFrameworkRange);
        }

        public int GetHashCode(OneWayCompatibilityMappingEntry obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();
            var comparer = FrameworkRangeComparer.Instance;

            combiner.AddObject(comparer.GetHashCode(obj.TargetFrameworkRange));
            combiner.AddObject(comparer.GetHashCode(obj.SupportedFrameworkRange));

            return combiner.CombinedHash;
        }
    }
}
