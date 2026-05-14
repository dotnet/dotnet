// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Shared;

using FallbackList = System.Collections.Generic.IReadOnlyList<NuGet.Frameworks.NuGetFramework>;

namespace NuGet.Frameworks
{
    /// <summary>
    /// AssetTargetFallbackFramework only fallback when zero assets are selected. These do not 
    /// auto fallback during GetNearest as FallbackFramework would.
    /// </summary>
    public class AssetTargetFallbackFramework : NuGetFramework, IEquatable<AssetTargetFallbackFramework>
    {
        private int? _hashCode;

        /// <summary>
        /// List framework to fall back to.
        /// </summary>
        public FallbackList Fallback { get; }

        /// <summary>
        /// Root project framework.
        /// </summary>
        public NuGetFramework RootFramework { get; }

        public AssetTargetFallbackFramework(NuGetFramework framework, FallbackList fallbackFrameworks)
            : base(ValidateFrameworkArgument(framework))
        {
            if (fallbackFrameworks == null)
            {
                throw new ArgumentNullException(nameof(fallbackFrameworks));
            }

            if (fallbackFrameworks.Count == 0)
            {
                throw new ArgumentException("Empty fallbackFrameworks is invalid", nameof(fallbackFrameworks));
            }

            Fallback = fallbackFrameworks;
            RootFramework = framework;
        }


        private static NuGetFramework ValidateFrameworkArgument(NuGetFramework framework)
        {
            if (framework is null)
            {
                throw new ArgumentNullException(nameof(framework));
            }
            return framework;
        }

        /// <summary>
        /// Create a FallbackFramework from the current AssetTargetFallbackFramework.
        /// </summary>
        public FallbackFramework AsFallbackFramework()
        {
            return new FallbackFramework(RootFramework, Fallback);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetTargetFallbackFramework);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                var combiner = new HashCodeCombiner();

                // Ensure that this is different from a FallbackFramework;
                combiner.AddStringIgnoreCase("assettargetfallback");

                combiner.AddObject(Comparer.GetHashCode(this));

                foreach (var each in Fallback)
                {
                    combiner.AddObject(Comparer.GetHashCode(each));
                }

                _hashCode = combiner.CombinedHash;
            }

            return _hashCode.Value;
        }

        public bool Equals(AssetTargetFallbackFramework? other)
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
