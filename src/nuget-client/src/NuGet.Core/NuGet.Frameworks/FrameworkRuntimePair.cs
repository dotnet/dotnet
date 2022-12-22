// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Shared;

namespace NuGet.Frameworks
{
    public class FrameworkRuntimePair : IEquatable<FrameworkRuntimePair>, IComparable<FrameworkRuntimePair>
    {
        public NuGetFramework Framework
        {
            get { return _framework; }
        }

        public string RuntimeIdentifier
        {
            get { return _runtimeIdentifier; }
        }

        public string Name
        {
            get { return _name; }
        }

        private readonly NuGetFramework _framework;
        private readonly string _runtimeIdentifier;
        private readonly string _name;

        public FrameworkRuntimePair(NuGetFramework framework, string runtimeIdentifier)
        {
            _framework = framework;
            _runtimeIdentifier = runtimeIdentifier ?? string.Empty;
            _name = GetName(framework, runtimeIdentifier);
        }

        public bool Equals(FrameworkRuntimePair other)
        {
            return other != null &&
                Equals(Framework, other.Framework) &&
                string.Equals(RuntimeIdentifier, other.RuntimeIdentifier, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
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

        public int CompareTo(FrameworkRuntimePair other)
        {
            var fxCompare = string.Compare(Framework.GetShortFolderName(), other.Framework.GetShortFolderName(), StringComparison.Ordinal);
            if (fxCompare != 0)
            {
                return fxCompare;
            }
            return string.Compare(RuntimeIdentifier, other.RuntimeIdentifier, StringComparison.Ordinal);
        }

        public static string GetName(NuGetFramework framework, string runtimeIdentifier)
        {
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

        public static string GetTargetGraphName(NuGetFramework framework, string runtimeIdentifier)
        {
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
