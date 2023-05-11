// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Shared;

using FallbackList = System.Collections.Generic.IReadOnlyList<NuGet.Frameworks.NuGetFramework>;

namespace NuGet.Frameworks
{
    public class FallbackFramework : NuGetFramework, IEquatable<FallbackFramework>
    {
        /// <summary>
        /// List framework to fall back to.
        /// </summary>
        public FallbackList Fallback { get; }

        private int? _hashCode;

        public FallbackFramework(NuGetFramework framework, FallbackList fallbackFrameworks)
            : base(framework)
        {
            if (framework == null)
            {
                throw new ArgumentNullException(nameof(framework));
            }

            if (fallbackFrameworks == null)
            {
                throw new ArgumentNullException(nameof(fallbackFrameworks));
            }

            if (fallbackFrameworks.Count == 0)
            {
                throw new ArgumentException("Empty fallbackFrameworks is invalid", nameof(fallbackFrameworks));
            }

            Fallback = fallbackFrameworks;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FallbackFramework);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                var combiner = new HashCodeCombiner();

                combiner.AddObject(Comparer.GetHashCode(this));

                foreach (var each in Fallback)
                {
                    combiner.AddObject(Comparer.GetHashCode(each));
                }

                _hashCode = combiner.CombinedHash;
            }

            return _hashCode.Value;
        }

        public bool Equals(FallbackFramework? other)
        {
            if (other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            return NuGetFramework.Comparer.Equals(this, other)
                && Fallback.SequenceEqual(other.Fallback);
        }
    }
}
