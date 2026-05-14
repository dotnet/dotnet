// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// FrameworkName/RuntimeIdentifier combination
    /// </summary>
    public class PackagesLockFileTarget : IEquatable<PackagesLockFileTarget>
    {
        /// <summary>
        /// Target framework.
        /// </summary>
        public NuGetFramework TargetFramework { get; set; }

        /// <summary>	
        /// Null for RIDless graphs.	
        /// </summary>	
        public string RuntimeIdentifier { get; set; }

        /// <summary>
        /// Target alias for version 3 lock files when multiple aliases exist for the same framework.
        /// It is very important that the alias is *not* set when there are no duplicate frameworks, since this drives the format of the lock file.
        /// </summary>
        public string TargetAlias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<LockFileDependency> Dependencies { get; set; } = new List<LockFileDependency>();

        /// <summary>
        /// Full framework name.
        /// </summary>
        public string Name => GetNameString(TargetFramework, RuntimeIdentifier, TargetAlias);

        public bool Equals(PackagesLockFileTarget other)
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
                && StringComparer.OrdinalIgnoreCase.Equals(TargetAlias, other.TargetAlias)
                && NuGetFramework.Comparer.Equals(TargetFramework, other.TargetFramework)
                && EqualityUtility.SequenceEqualWithNullCheck(Dependencies, other.Dependencies);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackagesLockFileTarget);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(TargetFramework);
            combiner.AddObject(RuntimeIdentifier);
            combiner.AddObject(TargetAlias);
            combiner.AddSequence(Dependencies);

            return combiner.CombinedHash;
        }

        private static string GetNameString(NuGetFramework framework, string runtime, string alias)
        {
            string frameworkString;

            // We already shipped net5.0 being listed as .NETCoreApp,Version=5.0, so it would be a breaking change
            // to change it. Similarly for all earlier framework identifiers.
            // Since net5.0-windows didn't work properly, we can use the "friendly" (folder name) for net5 with a platform.
            // For net6 and higher, always use friendly folder format, since this is the preferred customer-facing
            // format (from dotnet/design's net5 spec).
            // Packages lock files are checked into source control, hence customers will see it in their diffs.
            if (string.Equals(framework.Framework, FrameworkConstants.FrameworkIdentifiers.NetCoreApp, StringComparison.OrdinalIgnoreCase)
                && (
                    (framework.Version.Major >= 6)
                    || (framework.Version.Major == 5 && framework.HasPlatform)
                   )
                )
            {
                frameworkString = framework.ToString();
            }
            else
            {
                frameworkString = framework.DotNetFrameworkName;
            }

            // For version 3, use alias/framework/rid format when alias is present
            if (!string.IsNullOrEmpty(alias))
            {
                if (!string.IsNullOrEmpty(runtime))
                {
                    return $"{alias}/{runtime}";
                }
                return $"{alias}";
            }

            // Version 1 & 2 : framework or framework/rid
            if (!string.IsNullOrEmpty(runtime))
            {
                return $"{frameworkString}/{runtime}";
            }

            return frameworkString;
        }
    }
}
