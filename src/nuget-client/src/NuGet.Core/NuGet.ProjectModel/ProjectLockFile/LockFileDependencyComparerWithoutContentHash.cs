// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.ProjectModel.ProjectLockFile
{
    public class LockFileDependencyComparerWithoutContentHash : IEqualityComparer<LockFileDependency>
    {
        public static LockFileDependencyComparerWithoutContentHash Default { get; } = new LockFileDependencyComparerWithoutContentHash();

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

            return LockFileDependencyIdVersionComparer.Default.Equals(x, y) &&
                x.Type == y.Type &&
                EqualityUtility.EqualsWithNullCheck(x.RequestedVersion, y.RequestedVersion) &&
                EqualityUtility.SequenceEqualWithNullCheck(x.Dependencies, y.Dependencies);
        }

        public int GetHashCode(LockFileDependency obj)
        {
            var combiner = new HashCodeCombiner();
            combiner.AddObject(LockFileDependencyIdVersionComparer.Default.GetHashCode(obj));
            combiner.AddObject(obj.RequestedVersion);
            combiner.AddSequence(obj.Dependencies);
            combiner.AddStruct(obj.Type);
            return combiner.CombinedHash;
        }
    }
}
