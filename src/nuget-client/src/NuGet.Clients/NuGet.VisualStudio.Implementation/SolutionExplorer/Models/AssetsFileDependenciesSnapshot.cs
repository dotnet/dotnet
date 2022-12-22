// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft;
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
        public static AssetsFileDependenciesSnapshot Empty { get; } = new AssetsFileDependenciesSnapshot(null, null);

        /// <summary>
        /// Shared object for parsing the lock file. May be used in parallel.
        /// </summary>
        private static readonly LockFileFormat LockFileFormat = new LockFileFormat();

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

                previous.DataByTarget.TryGetValue(lockFileTarget.Name, out AssetsFileTarget? previousTarget);

                dataByTarget.Add(
                    lockFileTarget.Name,
                    new AssetsFileTarget(
                        this,
                        lockFileTarget.Name,
                        ParseLogMessages(lockFile, previousTarget, lockFileTarget.Name),
                        ParseLibraries(lockFile, lockFileTarget)));
            }

            DataByTarget = dataByTarget.ToImmutable();
            return;

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

                    if (j < previousLogs.Length && previousLogs[j].Equals(logMessage))
                    {
                        // Unchanged, so use previous value
                        builder.Add(previousLogs[j]);
                    }
                    else
                    {
                        builder.Add(new AssetsFileLogMessage(logMessage));
                    }
                }

                return builder.ToImmutable();
            }
        }

        internal static ImmutableDictionary<string, AssetsFileTargetLibrary> ParseLibraries(LockFile lockFile, LockFileTarget lockFileTarget)
        {
            ImmutableDictionary<string, AssetsFileTargetLibrary>.Builder builder = ImmutableDictionary.CreateBuilder<string, AssetsFileTargetLibrary>(StringComparer.OrdinalIgnoreCase);

            foreach (LockFileTargetLibrary lockFileLibrary in lockFileTarget.Libraries)
            {
                if (AssetsFileTargetLibrary.TryCreate(lockFile, lockFileLibrary, out AssetsFileTargetLibrary? library))
                {
                    builder.Add(library.Name, library);
                }
            }

            return builder.ToImmutable();
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
