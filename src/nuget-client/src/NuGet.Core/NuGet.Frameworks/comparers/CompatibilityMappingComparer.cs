// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.Frameworks
{
    public class CompatibilityMappingComparer : IEqualityComparer<OneWayCompatibilityMappingEntry>
    {
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

            var comparer = new FrameworkRangeComparer();

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
            var comparer = new FrameworkRangeComparer();

            combiner.AddObject(comparer.GetHashCode(obj.TargetFrameworkRange));
            combiner.AddObject(comparer.GetHashCode(obj.SupportedFrameworkRange));

            return combiner.CombinedHash;
        }
    }
}
