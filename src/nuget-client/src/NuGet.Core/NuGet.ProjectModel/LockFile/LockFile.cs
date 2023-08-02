// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

        public bool IsValidForPackageSpec(PackageSpec spec)
        {
            return IsValidForPackageSpec(spec, Version);
        }

        public bool IsValidForPackageSpec(PackageSpec spec, int requestLockFileVersion)
        {
            if (Version != requestLockFileVersion)
            {
                return false;
            }

            if (!ValidateDependencies(spec))
            {
                return false;
            }

            return true;
        }

        private bool ValidateDependencies(PackageSpec spec)
        {
            var actualTargetFrameworks = spec.TargetFrameworks;

            // The lock file should contain dependencies for each framework plus dependencies shared by all frameworks
            if (ProjectFileDependencyGroups.Count != actualTargetFrameworks.Count() + 1)
            {
                return false;
            }

            foreach (var group in ProjectFileDependencyGroups)
            {
                IOrderedEnumerable<string> actualDependencies;
                var expectedDependencies = @group.Dependencies.OrderBy(x => x, StringComparer.Ordinal);

                // If the framework name is empty, the associated dependencies are shared by all frameworks
                if (string.IsNullOrEmpty(@group.FrameworkName))
                {
                    actualDependencies = spec.Dependencies
                        .Select(x => x.LibraryRange.ToLockFileDependencyGroupString())
                        .OrderBy(x => x, StringComparer.Ordinal);
                }
                else
                {
                    var framework = actualTargetFrameworks.FirstOrDefault(f => string.Equals(
                                f.FrameworkName.DotNetFrameworkName,
                                @group.FrameworkName,
                                StringComparison.OrdinalIgnoreCase));

                    if (framework == null)
                    {
                        return false;
                    }

                    actualDependencies = framework
                        .Dependencies
                        .Select(d => d.LibraryRange.ToLockFileDependencyGroupString())
                        .OrderBy(x => x, StringComparer.Ordinal);
                }

                if (!actualDependencies.SequenceEqual(expectedDependencies))
                {
                    return false;
                }
            }
            return true;
        }

        public LockFileTarget GetTarget(NuGetFramework framework, string runtimeIdentifier)
        {
            return Targets.FirstOrDefault(t =>
                t.TargetFramework.Equals(framework) &&
                ((string.IsNullOrEmpty(runtimeIdentifier) && string.IsNullOrEmpty(t.RuntimeIdentifier) ||
                 string.Equals(runtimeIdentifier, t.RuntimeIdentifier, StringComparison.OrdinalIgnoreCase))));
        }

        public LockFileTarget GetTarget(string frameworkAlias, string runtimeIdentifier)
        {
            var framework = PackageSpec.TargetFrameworks.FirstOrDefault(tfi => tfi.TargetAlias.Equals(frameworkAlias, StringComparison.OrdinalIgnoreCase))?.FrameworkName;

            if (framework != null)
            {
                return GetTarget(framework, runtimeIdentifier);
            }
            return null;
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
