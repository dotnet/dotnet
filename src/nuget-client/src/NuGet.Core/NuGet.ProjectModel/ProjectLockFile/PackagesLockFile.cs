// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class PackagesLockFile : IEquatable<PackagesLockFile>
    {
        public int Version { get; set; }

        public string Path { get; set; }

        public IList<PackagesLockFileTarget> Targets { get; set; } = new List<PackagesLockFileTarget>();

        public PackagesLockFile(int version)
        {
            Version = version;
        }

        public PackagesLockFile()
        {
            Version = PackagesLockFileFormat.Version;
        }

        public bool Equals(PackagesLockFile other)
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
                EqualityUtility.SequenceEqualWithNullCheck(Targets, other.Targets);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackagesLockFile);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Version);
            combiner.AddSequence(Targets);

            return combiner.CombinedHash;
        }
    }
}
