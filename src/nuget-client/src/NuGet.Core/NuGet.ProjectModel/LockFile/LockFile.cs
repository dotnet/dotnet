// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public class LockFile : IEquatable<LockFile>
    {
        // Tools run under a hard coded framework.
        public static readonly NuGetFramework ToolFramework = FrameworkConstants.CommonFrameworks.NetCoreApp10;

        public static readonly char DirectorySeparatorChar = '/';

        // Set the version to the current default for new files.
        public int Version { get; set; } = LockFileFormat.Version;
        public string Path { get; set; }
        public IList<ProjectFileDependencyGroup> ProjectFileDependencyGroups { get; set; } = new List<ProjectFileDependencyGroup>();
        public IList<LockFileLibrary> Libraries { get; set; } = new List<LockFileLibrary>();
        public IList<LockFileTarget> Targets { get; set; } = new List<LockFileTarget>();
        public IList<LockFileItem> PackageFolders { get; set; } = new List<LockFileItem>();
        public IList<IAssetsLogMessage> LogMessages { get; set; } = new List<IAssetsLogMessage>();
        public PackageSpec PackageSpec { get; set; }
        public IList<CentralTransitiveDependencyGroup> CentralTransitiveDependencyGroups { get; set; } = new List<CentralTransitiveDependencyGroup>();

        public LockFileTarget GetTarget(NuGetFramework framework, string runtimeIdentifier)
        {
            return Targets.FirstOrDefault(t =>
                t.TargetFramework.Equals(framework) &&
                ((string.IsNullOrEmpty(runtimeIdentifier) && string.IsNullOrEmpty(t.RuntimeIdentifier) ||
                 string.Equals(runtimeIdentifier, t.RuntimeIdentifier, StringComparison.OrdinalIgnoreCase))));
        }

        public LockFileTarget GetTarget(string frameworkAlias, string runtimeIdentifier)
        {
            return Targets.FirstOrDefault(t =>
                t.TargetAlias.Equals(frameworkAlias) &&
                (string.IsNullOrEmpty(runtimeIdentifier) && string.IsNullOrEmpty(t.RuntimeIdentifier) ||
                 string.Equals(runtimeIdentifier, t.RuntimeIdentifier, StringComparison.OrdinalIgnoreCase)));
        }

        public LockFileLibrary GetLibrary(string name, NuGetVersion version)
        {
            return Libraries.FirstOrDefault(l =>
                string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase) &&
                l.Version.Equals(version));
        }

        public bool Equals(LockFile other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Version == other.Version
                && ProjectFileDependencyGroups.OrderedEquals(other.ProjectFileDependencyGroups, group => group.FrameworkName, StringComparer.OrdinalIgnoreCase)
                && Libraries.OrderedEquals(other.Libraries, library => library.Name, StringComparer.OrdinalIgnoreCase)
                && Targets.OrderedEquals(other.Targets, target => target.Name, StringComparer.Ordinal)
                && PackageFolders.SequenceEqual(other.PackageFolders)
                && EqualityUtility.EqualsWithNullCheck(PackageSpec, other.PackageSpec)
                && LogsEqual(other.LogMessages)
                && CentralTransitiveDependencyGroups.OrderedEquals(other.CentralTransitiveDependencyGroups, group => group.FrameworkName, StringComparer.OrdinalIgnoreCase);
        }

        private bool LogsEqual(IList<IAssetsLogMessage> otherLogMessages)
        {
            if (ReferenceEquals(LogMessages, otherLogMessages))
            {
                return true;
            }
            if (LogMessages.Count != otherLogMessages.Count)
            {
                return false;
            }


            var equals = true;

            var orderedLogMessages = LogMessages
                .OrderBy(m => m.Message, StringComparer.Ordinal)
                .ToArray();

            var orderedOtherLogMessages = otherLogMessages
                .OrderBy(m => m.Message, StringComparer.Ordinal)
                .ToArray();

            var length = orderedLogMessages.Length;

            for (var i = 0; i < length; i++)
            {
                equals &= orderedLogMessages[i].Equals(orderedOtherLogMessages[i]);

                if (!equals)
                {
                    break;
                }
            }

            return equals;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LockFile);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Version);
            combiner.AddUnorderedSequence(ProjectFileDependencyGroups);
            combiner.AddUnorderedSequence(Libraries);
            combiner.AddUnorderedSequence(Targets);
            combiner.AddSequence(PackageFolders); // ordered
            combiner.AddObject(PackageSpec);
            combiner.AddUnorderedSequence(LogMessages);
            combiner.AddUnorderedSequence(CentralTransitiveDependencyGroups);

            return combiner.CombinedHash;
        }
    }
}
