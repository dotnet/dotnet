// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Shared;

namespace NuGet.Frameworks
{
    /// <summary>
    /// Internal cache key used to store framework compatibility.
    /// </summary>
    internal class CompatibilityCacheKey : IEquatable<CompatibilityCacheKey>
    {
        public NuGetFramework Target
        {
            get { return _target; }
        }

        public NuGetFramework Candidate
        {
            get { return _candidate; }
        }

        private readonly NuGetFramework _target;
        private readonly NuGetFramework _candidate;
        private readonly int _hashCode;

        public CompatibilityCacheKey(NuGetFramework target, NuGetFramework candidate)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate));
            }

            _target = target;
            _candidate = candidate;

            // This is designed to be cached, just get the hash up front
            var combiner = new HashCodeCombiner();
            combiner.AddObject(target);
            combiner.AddObject(candidate);
            _hashCode = combiner.CombinedHash;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public bool Equals(CompatibilityCacheKey? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Target.Equals(other.Target)
                && Candidate.Equals(other.Candidate);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CompatibilityCacheKey);
        }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0} -> {1}",
                Target.DotNetFrameworkName,
                Candidate.DotNetFrameworkName);
        }
    }
}
