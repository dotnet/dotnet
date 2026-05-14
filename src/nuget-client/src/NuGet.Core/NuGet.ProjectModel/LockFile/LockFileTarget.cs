// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class LockFileTarget : IEquatable<LockFileTarget>
    {
        public NuGetFramework TargetFramework { get; set; }

        public string RuntimeIdentifier { get; set; }

        public string TargetAlias { get; set; }

        private string _name;
        public string Name
        {
            get
            {
                _name ??= TargetFramework + (string.IsNullOrEmpty(RuntimeIdentifier) ? "" : "/" + RuntimeIdentifier);
                return _name;
            }
            init
            {
                _name = value;
            }
        }

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
                && string.Equals(Name, other.Name, StringComparison.Ordinal)
                && string.Equals(TargetAlias, other.TargetAlias, StringComparison.Ordinal))
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
            combiner.AddObject(TargetAlias);
            combiner.AddUnorderedSequence(Libraries);

            return combiner.CombinedHash;
        }
    }
}
