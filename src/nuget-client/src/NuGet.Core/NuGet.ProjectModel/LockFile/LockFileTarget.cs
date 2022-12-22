// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class LockFileTarget : IEquatable<LockFileTarget>
    {
        public NuGetFramework TargetFramework { get; set; }

        public string RuntimeIdentifier { get; set; }

        public string Name => TargetFramework + (string.IsNullOrEmpty(RuntimeIdentifier) ? "" : "/" + RuntimeIdentifier);

        public IList<LockFileTargetLibrary> Libraries { get; set; } = new List<LockFileTargetLibrary>();

        public bool Equals(LockFileTarget other)
        {
            if (other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (NuGetFramework.Comparer.Equals(TargetFramework, other.TargetFramework)
                && string.Equals(RuntimeIdentifier, other.RuntimeIdentifier, StringComparison.Ordinal)
                && string.Equals(Name, other.Name, StringComparison.Ordinal))
            {
                return Libraries.OrderedEquals(other.Libraries, library => library.Name, StringComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LockFileTarget);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(TargetFramework);
            combiner.AddObject(RuntimeIdentifier);
            combiner.AddObject(Name);

            foreach (var library in Libraries.OrderBy(library => library.Name, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(library);
            }

            return combiner.CombinedHash;
        }
    }
}
