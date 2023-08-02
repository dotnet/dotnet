// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public class LockFileLibrary : IEquatable<LockFileLibrary>
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public NuGetVersion Version { get; set; }

        public bool IsServiceable { get; set; }

        public string Sha512 { get; set; }

        public IList<string> Files { get; set; } = new List<string>();

        /// <summary>
        /// Relative path to the project.json file for projects
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Relative path to the msbuild project file. Ex: xproj, csproj
        /// </summary>
        public string MSBuildProject { get; set; }

        public bool HasTools { get; set; }

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

        public override bool Equals(object obj)
        {
            return Equals(obj as LockFileLibrary);
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

        /// <summary>
        /// Makes a deep clone of the lock file library.
        /// </summary>
        /// <returns>The cloned lock file library.</returns>
        public LockFileLibrary Clone()
        {
            return new LockFileLibrary
            {
                Name = Name,
                Type = Type,
                Version = Version,
                IsServiceable = IsServiceable,
                HasTools = HasTools,
                Sha512 = Sha512,
                Files = Files != null ? new List<string>(Files) : null,
                Path = Path,
                MSBuildProject = MSBuildProject
            };
        }
    }
}
