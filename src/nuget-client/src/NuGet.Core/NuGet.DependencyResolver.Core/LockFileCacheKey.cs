// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;
using NuGet.Shared;

namespace NuGet.DependencyResolver
{
    /// <summary>
    /// Helper class to hold lock file libraries per TFM/RID combo.
    /// </summary>
    public class LockFileCacheKey : IEquatable<LockFileCacheKey>
    {
        /// <summary>
        /// Target framework.
        /// </summary>
        public NuGetFramework TargetFramework { get; }

        /// <summary>	
        /// Null for RIDless graphs.	
        /// </summary>	
        public string? RuntimeIdentifier { get; }

        public string TargetAlias { get; }

        public LockFileCacheKey(NuGetFramework framework, string? runtimeIdentifier, string? targetAlias)
        {
            TargetFramework = framework;
            RuntimeIdentifier = runtimeIdentifier;
            TargetAlias = targetAlias ?? string.Empty; // alias may be passed as both null or empty and we need to treat them equivalently.
        }

        /// <summary>
        /// Full framework name.
        /// </summary>
        public string Name => GetNameString(TargetFramework.DotNetFrameworkName, RuntimeIdentifier);

        public bool Equals(LockFileCacheKey? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return StringComparer.Ordinal.Equals(RuntimeIdentifier, other.RuntimeIdentifier)
                && NuGetFramework.Comparer.Equals(TargetFramework, other.TargetFramework)
                && StringComparer.OrdinalIgnoreCase.Equals(TargetAlias, other.TargetAlias);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LockFileCacheKey);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(TargetFramework);
            combiner.AddObject(RuntimeIdentifier);
            combiner.AddStringIgnoreCase(TargetAlias);

            return combiner.CombinedHash;
        }

        public override string ToString()
        {
            return Name;
        }

        private static string GetNameString(string framework, string? runtime)
        {
            if (!string.IsNullOrEmpty(runtime))
            {
                return $"{framework}/{runtime}";
            }

            return framework;
        }
    }
}
