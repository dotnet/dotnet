// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft;

namespace NuGet.VisualStudio.SolutionExplorer.Models
{
    /// <summary>
    /// Immutable snapshot of data captured from <c>project.assets.json</c> that relates to a specific target.
    /// </summary>
    internal sealed class AssetsFileTarget
    {
        /// <summary>
        /// Gets the target framework moniker, such as <c>.NETFramework,Version=v4.8</c> or <c>.NETStandard,Version=v1.3</c>.
        /// </summary>
        public string TargetFrameworkMoniker { get; }

        /// <summary>
        /// Gets diagnostic messages for this target. Often empty.
        /// </summary>
        public ImmutableArray<AssetsFileLogMessage> Logs { get; }

        /// <summary>
        /// Stores data about libraries (packages/projects), keyed by name.
        /// </summary>
        public ImmutableDictionary<string, AssetsFileTargetLibrary> LibraryByName { get; }

        /// <summary>
        /// The snapshot to which this target data belongs.
        /// </summary>
        private readonly AssetsFileDependenciesSnapshot _snapshot;

        /// <summary>
        /// Lazily populated cache of back-references in the dependencies graph, mapping libraries to their ancestor(s).
        /// Created by <see cref="TryGetDependents"/>, which is used during computation of Solution Explorer search results.
        /// </summary>
        private IReadOnlyDictionary<string, ImmutableArray<AssetsFileTargetLibrary>>? _dependentsByLibrary;

        /// <summary>
        /// Lazily populated cache of dependencies as <see cref="AssetsFileTargetLibrary"/> objects.
        /// </summary>
        private readonly Dictionary<(string LibraryName, string? Version), ImmutableArray<AssetsFileTargetLibrary>> _dependenciesByNameAndVersion = new Dictionary<(string LibraryName, string? Version), ImmutableArray<AssetsFileTargetLibrary>>();

        public AssetsFileTarget(AssetsFileDependenciesSnapshot snapshot, string targetFrameworkMoniker, ImmutableArray<AssetsFileLogMessage> logs, ImmutableDictionary<string, AssetsFileTargetLibrary> libraryByName)
        {
            Requires.NotNull(snapshot, nameof(snapshot));
            Requires.NotNullOrWhiteSpace(targetFrameworkMoniker, nameof(targetFrameworkMoniker));
            Requires.Argument(!logs.IsDefault, nameof(logs), "Must not be default");
            Requires.NotNull(libraryByName, nameof(libraryByName));

            TargetFrameworkMoniker = targetFrameworkMoniker;
            _snapshot = snapshot;
            Logs = logs;
            LibraryByName = libraryByName;
        }

        /// <summary>
        /// Gets the set of dependents (parents) of <paramref name="libraryName"/>.
        /// </summary>
        /// <returns><see langword="true"/> if dependents were found, otherwise <see langword="false"/>.</returns>
        public bool TryGetDependents(string libraryName, out ImmutableArray<AssetsFileTargetLibrary> dependents)
        {
            // Defer construction of dependents collection until needed. It's only needed for Solution Explorer search.
            if (_dependentsByLibrary == null)
            {
                var dependentsByLibrary = new Dictionary<string, ImmutableArray<AssetsFileTargetLibrary>.Builder>(LibraryByName.Count);

                foreach ((_, AssetsFileTargetLibrary library) in LibraryByName)
                {
                    foreach (string dependency in library.Dependencies)
                    {
                        GetBuilder(dependency).Add(library);
                    }
                }

                Volatile.Write(ref _dependentsByLibrary, dependentsByLibrary.ToDictionary(pair => pair.Key, pair => pair.Value.ToImmutable()));

                ImmutableArray<AssetsFileTargetLibrary>.Builder GetBuilder(string library)
                {
                    if (!dependentsByLibrary.TryGetValue(library, out ImmutableArray<AssetsFileTargetLibrary>.Builder builder))
                    {
                        dependentsByLibrary[library] = builder = ImmutableArray.CreateBuilder<AssetsFileTargetLibrary>();
                    }

                    return builder;
                }
            }

            return _dependentsByLibrary!.TryGetValue(libraryName, out dependents);
        }

        /// <summary>
        /// Gets the set of dependencies (children) of <paramref name="libraryName"/>, optionally restricted by <paramref name="version"/>.
        /// </summary>
        /// <returns><see langword="true"/> if dependencies were found, otherwise <see langword="false"/>.</returns>
        public bool TryGetDependencies(string libraryName, string? version, out ImmutableArray<AssetsFileTargetLibrary> dependencies)
        {
            if (!LibraryByName.TryGetValue(libraryName, out AssetsFileTargetLibrary? library))
            {
                dependencies = default;
                return false;
            }

            if (version != null && library.Version != version)
            {
                dependencies = default;
                return false;
            }

            lock (_dependenciesByNameAndVersion)
            {
                (string libraryName, string? version) key = (libraryName, version);

                if (!_dependenciesByNameAndVersion.TryGetValue(key, out dependencies))
                {
                    ImmutableArray<AssetsFileTargetLibrary>.Builder builder = ImmutableArray.CreateBuilder<AssetsFileTargetLibrary>(library.Dependencies.Length);

                    foreach (string dependencyName in library.Dependencies)
                    {
                        // That there are rare cases where an advertised library is not detailed in the assets file.
                        // For example "NETStandard.Library" as a dependency of a package brought in via a project will
                        // not cause details NETStandard.Library to be included in the grandparent's assets file.
                        // Such libraries are excluded.
                        if (LibraryByName.TryGetValue(dependencyName, out AssetsFileTargetLibrary? dependency))
                        {
                            builder.Add(dependency);
                        }
                    }

                    dependencies = builder.Count != library.Dependencies.Length
                        ? builder.ToImmutable()
                        : builder.MoveToImmutable();
                    _dependenciesByNameAndVersion[key] = dependencies;
                }
            }

            return true;
        }

        public bool TryGetPackage(string packageId, [NotNullWhen(returnValue: true)] out AssetsFileTargetLibrary? assetsFileLibrary)
        {
            Requires.NotNull(packageId, nameof(packageId));

            if (LibraryByName.TryGetValue(packageId, out assetsFileLibrary) &&
                assetsFileLibrary.Type is AssetsFileLibraryType.Package or AssetsFileLibraryType.Unknown)
            {
                return true;
            }

            assetsFileLibrary = null;
            return false;
        }

        public bool TryGetProject(string projectId, [NotNullWhen(returnValue: true)] out AssetsFileTargetLibrary? assetsFileLibrary)
        {
            Requires.NotNull(projectId, nameof(projectId));

            if (LibraryByName.TryGetValue(projectId, out assetsFileLibrary) &&
                assetsFileLibrary.Type is AssetsFileLibraryType.Project or AssetsFileLibraryType.Unknown)
            {
                return true;
            }

            assetsFileLibrary = null;
            return false;
        }

        public bool TryResolvePackagePath(string packageId, string version, out string? fullPath)
        {
            return _snapshot.TryResolvePackagePath(packageId, version, out fullPath);
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("Target \"").Append(TargetFrameworkMoniker).Append("\" ");
            s.Append(LibraryByName.Count).Append(LibraryByName.Count == 1 ? " library" : " libraries");
            s.Append(Logs.Length).Append(Logs.Length == 1 ? " log" : " logs");
            return s.ToString();
        }
    }
}
