// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class LockFileDependencyIdVersionComparer : IEqualityComparer<LockFileDependency>
    {
        public static LockFileDependencyIdVersionComparer Default { get; } = new LockFileDependencyIdVersionComparer();

        public bool Equals(LockFileDependency x, LockFileDependency y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return StringComparer.OrdinalIgnoreCase.Equals(x.Id, y.Id) &&
                EqualityUtility.EqualsWithNullCheck(x.ResolvedVersion, y.ResolvedVersion);
        }

        public int GetHashCode(LockFileDependency obj)
        {
            var combiner = new HashCodeCombiner();
            combiner.AddObject(obj.Id);
            combiner.AddObject(obj.ResolvedVersion);
            return combiner.CombinedHash;
        }
    }
}
