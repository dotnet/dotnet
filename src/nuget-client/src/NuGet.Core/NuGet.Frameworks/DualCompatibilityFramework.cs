// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.Frameworks
{
    /// <summary>
    /// Represents a framework that behaves as 2 potentially independent frameworks.
    /// Ex. C++/CLI can support both .NET 5.0 and native.
    /// This type is immutable.
    /// </summary>
    public class DualCompatibilityFramework : NuGetFramework
    {
        /// <summary>
        /// The root framework. Any compatibility checks should be performed against this framework first.
        /// </summary>
        public NuGetFramework RootFramework { get; }

        /// <summary>
        /// The secondary framework. If the root framework compatibility checks fail, then the compat checks should be performed against this framework next.
        /// </summary>
        public NuGetFramework SecondaryFramework { get; }

        private int? _hashCode;
        private FallbackFramework? _fallbackFramework;

        /// <summary>
        /// Multiple compatbility 
        /// </summary>
        /// <param name="framework">Root framework. Never <see langword="null"/>. </param>
        /// <param name="secondaryFramework">Secondary framework. Never <see langword="null"/>. </param>
        /// <exception cref="ArgumentNullException">if either <paramref name="framework"/> or <paramref name="secondaryFramework"/> are <see langword="null"/>.</exception>
        public DualCompatibilityFramework(NuGetFramework framework, NuGetFramework secondaryFramework)
            : base(ValidateFrameworkArgument(framework))
        {
            if (secondaryFramework == null)
            {
                throw new ArgumentNullException(nameof(secondaryFramework));
            }

            SecondaryFramework = secondaryFramework;
            RootFramework = framework;
        }

        /// <summary>
        /// Create a FallbackFramework from the current DualCompatibilityFramework.
        /// </summary>
        public FallbackFramework AsFallbackFramework()
        {
            if (_fallbackFramework == null)
            {
                _fallbackFramework = new FallbackFramework(RootFramework, new NuGetFramework[] { SecondaryFramework });
            }

            return _fallbackFramework;
        }

        private static NuGetFramework ValidateFrameworkArgument(NuGetFramework framework)
        {
            if (framework == null)
            {
                throw new ArgumentNullException(nameof(framework));
            }
            return framework;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DualCompatibilityFramework);
        }

        public bool Equals(DualCompatibilityFramework? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Comparer.Equals(RootFramework, other.RootFramework)
                && Comparer.Equals(SecondaryFramework, other.SecondaryFramework);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                var combiner = new HashCodeCombiner();
                // Ensure that this is different from AssetTargetFallback & FallbackFramework;
                combiner.AddStringIgnoreCase(nameof(DualCompatibilityFramework));
                combiner.AddObject(Comparer.GetHashCode(RootFramework));
                combiner.AddObject(Comparer.GetHashCode(SecondaryFramework));
                _hashCode = combiner.CombinedHash;
            }

            return _hashCode.Value;
        }

    }
}
