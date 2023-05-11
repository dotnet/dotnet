// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Shared;

namespace NuGet.Frameworks
{
    public class FrameworkRuntimePair : IEquatable<FrameworkRuntimePair>, IComparable<FrameworkRuntimePair>
    {
        public NuGetFramework Framework { get; }

        public string RuntimeIdentifier { get; }

        public string Name { get; }

        public FrameworkRuntimePair(NuGetFramework framework, string? runtimeIdentifier)
        {
            Framework = framework ?? throw new ArgumentNullException(nameof(framework));
            RuntimeIdentifier = runtimeIdentifier ?? string.Empty;
            Name = GetName(framework, runtimeIdentifier);
        }

        public bool Equals(FrameworkRuntimePair? other)
        {
            return other != null &&
                Equals(Framework, other.Framework) &&
                string.Equals(RuntimeIdentifier, other.RuntimeIdentifier, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FrameworkRuntimePair);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.GetHashCode(Framework, RuntimeIdentifier);
        }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0}~{1}",
                Framework.GetShortFolderName(),
                RuntimeIdentifier);
        }

        public FrameworkRuntimePair Clone()
        {
            return new FrameworkRuntimePair(Framework, RuntimeIdentifier);
        }

        public int CompareTo(FrameworkRuntimePair? other)
        {
            if (other == null) return 1;

            var fxCompare = string.Compare(Framework.GetShortFolderName(), other.Framework.GetShortFolderName(), StringComparison.Ordinal);
            if (fxCompare != 0)
            {
                return fxCompare;
            }
            return string.Compare(RuntimeIdentifier, other.RuntimeIdentifier, StringComparison.Ordinal);
        }

        public static string GetName(NuGetFramework framework, string? runtimeIdentifier)
        {
            if (framework is null) throw new ArgumentNullException(nameof(framework));

            if (string.IsNullOrEmpty(runtimeIdentifier))
            {
                return framework.ToString();
            }
            else
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} ({1})",
                    framework,
                    runtimeIdentifier);
            }
        }

        public static string GetTargetGraphName(NuGetFramework framework, string? runtimeIdentifier)
        {
            if (framework is null) throw new ArgumentNullException(nameof(framework));

            if (string.IsNullOrEmpty(runtimeIdentifier))
            {
                return framework.ToString();
            }
            else
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}/{1}",
                    framework,
                    runtimeIdentifier);
            }
        }
    }
}
