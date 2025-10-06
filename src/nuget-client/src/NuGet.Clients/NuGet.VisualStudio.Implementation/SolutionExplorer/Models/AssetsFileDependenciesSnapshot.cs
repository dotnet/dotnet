// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.SolutionExplorer.Models
{
    /// <summary>
    /// Snapshot of data captured from <c>project.assets.json</c>. Immutable.
    /// </summary>
    internal sealed class AssetsFileDependenciesSnapshot
    {
        /// <summary>
        /// Gets the singleton empty instance.
        /// </summary>
        public static AssetsFileDependenciesSnapshot Empty { get; } = new(null, null);

        /// <summary>
        /// Shared object for parsing the lock file. May be used in parallel.
        /// </summary>
        private static readonly LockFileFormat LockFileFormat = new();

        public ImmutableDictionary<string, AssetsFileTarget> DataByTarget { get; }

        /// <summary>
        /// The <c>packageFolders</c> array from the assets file. The first is the 'user package folder',
        /// and any others are 'fallback package folders'.
        /// </summary>
        private readonly ImmutableArray<string> _packageFolders;

        /// <summary>
        /// Lazily populated instance of a NuGet type that performs package path resolution. May be used in parallel.
        /// </summary>
        private FallbackPackagePathResolver? _packagePathResolver;

        /// <summary>
        /// Produces an updated snapshot by reading the <c>project.assets.json</c> file at <paramref name="path"/>.
        /// If the file could not be read, or no changes are detected, the current snapshot (this) is returned.
        /// </summary>
        public AssetsFileDependenciesSnapshot UpdateFromAssetsFile(string path)
        {
            Requires.NotNull(path, nameof(path));

            try
            {
                // Parse the file
                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete, 4096 * 10, FileOptions.SequentialScan);

                LockFile lockFile = LockFileFormat.Read(fileStream, path);

                return new AssetsFileDependenciesSnapshot(lockFile, this);
            }
            catch
            {
                return this;
            }
        }

        // For testing only
        internal static AssetsFileDependenciesSnapshot FromLockFile(LockFile lockFile)
        {
            return new(lockFile, Empty);
        }

        private AssetsFileDependenciesSnapshot(LockFile? lockFile, AssetsFileDependenciesSnapshot? previous)
        {
            if (lockFile == null)
            {
                DataByTarget = ImmutableDictionary<string, AssetsFileTarget>.Empty;
                return;
            }

            Assumes.NotNull(previous);

            _packageFolders = lockFile.PackageFolders.Select(pf => pf.Path).ToImmutableArray();

            ImmutableDictionary<string, AssetsFileTarget>.Builder dataByTarget = ImmutableDictionary.CreateBuilder<string, AssetsFileTarget>(StringComparer.OrdinalIgnoreCase);

            foreach (LockFileTarget lockFileTarget in lockFile.Targets)
            {
                if (lockFileTarget.RuntimeIdentifier != null)
                {
                    // Skip "target/rid"s and only consume actual targets
                    continue;
                }

                string targetAlias = GetTargetAlias(lockFileTarget.Name);

                previous.DataByTarget.TryGetValue(targetAlias, out AssetsFileTarget? previousTarget);

                ImmutableArray<AssetsFileLogMessage> logMessages = ParseLogMessages(lockFile, previousTarget, lockFileTarget.Name);

                dataByTarget.Add(
                    targetAlias,
                    new AssetsFileTarget(
                        this,
                        targetAlias,
                        logMessages,
                        ParseLibraries(lockFile, lockFileTarget, logMessages)));
            }

            DataByTarget = dataByTarget.ToImmutable();
            return;

            string GetTargetAlias(string lockFileTargetName)
            {
                // In some places, the target alias specified in the project file (e.g. "net472") will not
                // match the target name used throughout the lock file (e.g. ".NETFramework,Version=v4.7.2").
                // The dependencies tree only uses the target alias (what's in the project file) so we need
                // to map back to that. See https://github.com/dotnet/project-system/issues/6832.

                if (lockFile.PackageSpec.TargetFrameworks.Any(t => t.TargetAlias == lockFileTargetName))
                {
                    // The target name used in the assets file matches the target alias in the project file.
                    return lockFileTargetName;
                }

                // The target name used in the assets file does NOT match any target alias in the project.
                // Attempt to find the name used in the project.
                foreach (TargetFrameworkInformation targetInfo in lockFile.PackageSpec.TargetFrameworks)
                {
                    if (targetInfo.FrameworkName.DotNetFrameworkName == lockFileTargetName)
                    {
                        // We found a match, so return the alias.
                        return targetInfo.TargetAlias;
                    }
                }

                // No match was found. Not ideal. Nothing to do but return the original value.
                return lockFileTargetName;
            }

            static ImmutableArray<AssetsFileLogMessage> ParseLogMessages(LockFile lockFile, AssetsFileTarget? previousTarget, string target)
            {
                if (lockFile.LogMessages.Count == 0)
                {
                    return ImmutableArray<AssetsFileLogMessage>.Empty;
                }

                // Filter log messages to our target
                ImmutableArray<AssetsFileLogMessage> previousLogs = previousTarget?.Logs ?? ImmutableArray<AssetsFileLogMessage>.Empty;
                ImmutableArray<AssetsFileLogMessage>.Builder builder = ImmutableArray.CreateBuilder<AssetsFileLogMessage>();

                int j = 0;
                foreach (IAssetsLogMessage logMessage in lockFile.LogMessages)
                {
                    if (!logMessage.TargetGraphs.Contains(target))
                    {
                        continue;
                    }

                    j++;

                    if (j < previousLogs.Length && previousLogs[j].Equals(logMessage, lockFile.PackageSpec.FilePath))
                    {
                        // Unchanged, so use previous value
                        builder.Add(previousLogs[j]);
                    }
                    else
                    {
                        builder.Add(new AssetsFileLogMessage(lockFile.PackageSpec.FilePath, logMessage));
                    }
                }

                return builder.ToImmutable();
            }
        }

        internal static ImmutableDictionary<string, AssetsFileTargetLibrary> ParseLibraries(LockFile lockFile, LockFileTarget lockFileTarget, ImmutableArray<AssetsFileLogMessage> logMessages)
        {
            var levelByLibrary = BuildLevelByLibrary();

            ImmutableDictionary<string, AssetsFileTargetLibrary>.Builder builder = ImmutableDictionary.CreateBuilder<string, AssetsFileTargetLibrary>(StringComparer.OrdinalIgnoreCase);

            foreach (LockFileTargetLibrary lockFileLibrary in lockFileTarget.Libraries)
            {
                LogLevel? logLevel = null;

                if (lockFileLibrary.Name is not null && levelByLibrary.TryGetValue(lockFileLibrary.Name, out var level))
                {
                    logLevel = level;
                }

                if (AssetsFileTargetLibrary.TryCreate(lockFile, lockFileLibrary, logLevel, out AssetsFileTargetLibrary? library))
                {
                    builder.Add(library.Name, library);
                }
            }

            // If a non-existent library is referenced, it will have an error log message, but no entry in "libraries".
            // We want to show a diagnostic node beneath such nodes in the tree, so need to create a dummy library entry,
            // otherwise there's nothing to attach that diagnostic to.
            foreach (AssetsFileLogMessage message in logMessages)
            {
                string libraryName = message.LibraryName;

                if (!builder.ContainsKey(libraryName))
                {
                    builder.Add(libraryName, AssetsFileTargetLibrary.CreatePlaceholder(libraryName));
                }
            }

            return builder.ToImmutable();

            Dictionary<string, LogLevel> BuildLevelByLibrary()
            {
                Dictionary<string, HashSet<string>> ancestorsByLibrary = new(StringComparer.OrdinalIgnoreCase);

                // Build a map from child-to-ancestors
                foreach (LockFileTargetLibrary lockFileLibrary in lockFileTarget.Libraries)
                {
                    string? parentId = lockFileLibrary.Name;

                    if (parentId is null)
                    {
                        continue;
                    }

                    foreach (var dep in lockFileLibrary.Dependencies)
                    {
                        string childId = dep.Id;

                        if (!ancestorsByLibrary.TryGetValue(childId, out HashSet<string>? ancestors))
                        {
                            ancestors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            ancestorsByLibrary.Add(childId, ancestors);
                        }

                        ancestors.Add(parentId);
                    }
                }

                // Walk up the tree from each log message's library, propagating the highest level found.
                Dictionary<string, LogLevel> levelByLibrary = new(StringComparer.OrdinalIgnoreCase);

                foreach (AssetsFileLogMessage message in logMessages)
                {
                    Integrate(message.LibraryName, message.Level, visited: []);
                }

                return levelByLibrary;

                void Integrate(string id, LogLevel level, HashSet<string> visited)
                {
                    if (!visited.Add(id))
                    {
                        // Avoid infinite recursion.
                        return;
                    }

                    if (!levelByLibrary.TryGetValue(id, out LogLevel currentLevel))
                    {
                        // No level yet, so set it.
                        levelByLibrary[id] = level;
                    }
                    else
                    {
                        // Higher level is more severe.
                        // - If we already have a higher level, don't change it.
                        // - If the level matches, we will have already propagated it to ancestors, so can return.
                        if (currentLevel >= level)
                        {
                            return;
                        }
                    }

                    if (ancestorsByLibrary.TryGetValue(id, out HashSet<string>? ancestors))
                    {
                        foreach (string ancestor in ancestors)
                        {
                            Integrate(ancestor, level, visited);
                        }
                    }
                }
            }
        }

        public bool TryGetTarget(string? target, [NotNullWhen(returnValue: true)] out AssetsFileTarget? targetData)
        {
            if (DataByTarget.Count == 0)
            {
                targetData = null;
                return false;
            }

            if (target == null)
            {
                if (DataByTarget.Count != 1)
                {
                    // This is unexpected
                    Debug.Fail("No target specified, yet more than one target exists");
                    targetData = null;
                    return false;
                }

                targetData = DataByTarget.First().Value;
            }
            else if (!DataByTarget.TryGetValue(target, out targetData))
            {
                targetData = null;
                return false;
            }

            return true;
        }

        public bool TryResolvePackagePath(string packageId, string version, out string? fullPath)
        {
            Requires.NotNull(packageId, nameof(packageId));
            Requires.NotNull(version, nameof(version));

            if (_packageFolders.IsEmpty)
            {
                fullPath = null;
                return false;
            }

            try
            {
                _packagePathResolver ??= new FallbackPackagePathResolver(_packageFolders[0], _packageFolders.Skip(1));

                fullPath = _packagePathResolver.GetPackageDirectory(packageId, version);
                return true;
            }
            catch
            {
                fullPath = null;
                return false;
            }
        }

        public override string ToString() => $"{DataByTarget.Count} target{(DataByTarget.Count == 1 ? string.Empty : "s")}";
    }
}
