// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.SolutionExplorer.Models
{
    /// <summary>
    /// Data about a library (package/project) in a given target, from <c>project.assets.json</c>. Immutable.
    /// </summary>
    internal sealed class AssetsFileTargetLibrary
    {
        public static bool TryCreate(LockFile lockFile, LockFileTargetLibrary lockFileLibrary, [NotNullWhen(returnValue: true)] out AssetsFileTargetLibrary? targetLibrary)
        {
            AssetsFileLibraryType type;
            if (lockFileLibrary.Type == "package")
            {
                type = AssetsFileLibraryType.Package;
            }
            else if (lockFileLibrary.Type == "project")
            {
                type = AssetsFileLibraryType.Project;
            }
            else
            {
                targetLibrary = null;
                return false;
            }

            LockFileLibrary? library = lockFile.Libraries.FirstOrDefault(lib => lib.Name == lockFileLibrary.Name);

            targetLibrary = new AssetsFileTargetLibrary(library, lockFileLibrary, type);
            return true;
        }

        private AssetsFileTargetLibrary(LockFileLibrary? library, LockFileTargetLibrary targetLibrary, AssetsFileLibraryType type)
        {
            Name = targetLibrary.Name;
            Version = targetLibrary.Version.ToNormalizedString();
            Type = type;

            Dependencies = targetLibrary.Dependencies.Select(dep => dep.Id).ToImmutableArray();

            CompileTimeAssemblies = targetLibrary.CompileTimeAssemblies
                .Where(a => a is not null && !IsPlaceholderFile(a.Path))
                .Select(a => a.Path)
                .ToImmutableArray();

            FrameworkAssemblies = targetLibrary.FrameworkAssemblies.ToImmutableArray();

            ContentFiles = targetLibrary.ContentFiles
                .Where(file => !IsPlaceholderFile(file.Path))
                .Select(file => new AssetsFileTargetLibraryContentFile(file))
                .ToImmutableArray();

            BuildFiles = targetLibrary.Build
                .Where(file => !IsPlaceholderFile(file.Path))
                .Select(file => file.Path)
                .ToImmutableArray();

            BuildMultiTargetingFiles = targetLibrary.BuildMultiTargeting
                .Where(file => !IsPlaceholderFile(file.Path))
                .Select(file => file.Path)
                .ToImmutableArray();

            DocumentationFiles = library?.Files
                .Where(IsDocumentationFile)
                .ToImmutableArray() ?? ImmutableArray<string>.Empty;

            return;

            static bool IsPlaceholderFile(string path)
            {
                if (path.EndsWith("_._", StringComparison.Ordinal))
                {
                    if (path.Length == 3)
                    {
                        return true;
                    }

                    char separator = path[path.Length - 4];
                    return separator == '\\' || separator == '/';
                }

                return false;
            }

            static bool IsDocumentationFile(string path)
            {
                // Content files are not considered documentation. They are displayed via different means.
                if (path.StartsWith("contentFiles/", StringComparison.OrdinalIgnoreCase))
                    return false;

                return path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                    || path.EndsWith(".md", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string Name { get; }
        public string Version { get; }
        public AssetsFileLibraryType Type { get; }
        public ImmutableArray<string> Dependencies { get; }
        public ImmutableArray<string> FrameworkAssemblies { get; }
        public ImmutableArray<string> CompileTimeAssemblies { get; }
        public ImmutableArray<AssetsFileTargetLibraryContentFile> ContentFiles { get; }
        public ImmutableArray<string> BuildFiles { get; }
        public ImmutableArray<string> BuildMultiTargetingFiles { get; }
        public ImmutableArray<string> DocumentationFiles { get; }

        public override string ToString() => $"{Type} {Name} ({Version}) {Dependencies.Length} {(Dependencies.Length == 1 ? "dependency" : "dependencies")}";
    }
}
