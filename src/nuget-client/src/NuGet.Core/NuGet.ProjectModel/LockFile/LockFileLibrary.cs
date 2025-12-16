// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public sealed record class LockFileLibrary : IEquatable<LockFileLibrary>
    {
        private static IList<string> EmptyFiles = ImmutableArray<string>.Empty;

        public string Name { get; init; }

        public string Type { get; init; }

        public NuGetVersion Version { get; init; }

        public bool IsServiceable { get; init; }

        public string Sha512 { get; init; }

        public IList<string> Files { get; init; } = EmptyFiles;

        /// <summary>
        /// Relative path to the project.json file for projects
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// Relative path to the msbuild project file. Ex: xproj, csproj
        /// </summary>
        public string MSBuildProject { get; init; }

        public bool HasTools { get; init; }

        public bool Equals(LockFileLibrary other)
        {
            if (other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Path, other.Path, StringComparison.Ordinal)
                && string.Equals(MSBuildProject, other.MSBuildProject, StringComparison.Ordinal)
                && IsServiceable == other.IsServiceable
                && HasTools == other.HasTools
                && string.Equals(Sha512, other.Sha512, StringComparison.Ordinal)
                && Version == other.Version)
            {
                return Files.OrderedEquals(other.Files, s => s, StringComparer.Ordinal, StringComparer.Ordinal);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(Name);
            combiner.AddStringIgnoreCase(Type);
            combiner.AddObject(Sha512);
            combiner.AddObject(IsServiceable);
            combiner.AddObject(HasTools);
            combiner.AddObject(Version);
            combiner.AddObject(Path);
            combiner.AddObject(MSBuildProject);
            combiner.AddUnorderedSequence(Files);

            return combiner.CombinedHash;
        }
    }
}
