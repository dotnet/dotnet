// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Shared;

namespace NuGet.Commands
{
    public class RestoreResult : IRestoreResult
    {
        public bool Success { get; }

        /// <summary>
        /// Gets the path that the lock file will be written to.
        /// </summary>
        public string LockFilePath { get; set; }

        /// <summary>
        /// Gets the resolved dependency graphs produced by the restore operation
        /// </summary>
        public IEnumerable<RestoreTargetGraph> RestoreGraphs { get; }

        public IEnumerable<CompatibilityCheckResult> CompatibilityCheckResults { get; }

        /// <summary>
        /// Props and targets files to be written to disk.
        /// </summary>
        public IEnumerable<MSBuildOutputFile> MSBuildOutputFiles { get; }

        /// <summary>
        /// Restore type.
        /// </summary>
        public ProjectStyle ProjectStyle { get; }

        /// <summary>
        /// Gets the lock file that was generated during the restore or, in the case of a locked lock file,
        /// was used to determine the packages to install during the restore.
        /// </summary>
        public virtual LockFile LockFile { get; }

        /// <summary>
        /// The existing lock file. This is null if no lock file was provided on the <see cref="RestoreRequest"/>.
        /// </summary>
        public virtual LockFile PreviousLockFile { get; }

        /// <summary>
        /// Restore time
        /// </summary>
        public TimeSpan ElapsedTime { get; }

        /// <summary>
        /// The log messages raised during this restore operation
        /// </summary>
        /// <remarks>The messages here are usually sources from the <see cref="LockFile"/> in full restores or <see cref="CacheFile"/> for no-op restores.</remarks>
        public virtual IList<IAssetsLogMessage> LogMessages { get; internal set; }

        /// <summary>
        ///  Cache File. The previous cache file for this project
        /// </summary>
        private CacheFile CacheFile { get; }

        /// <summary>
        /// Cache File path. The file path where the cache is written out
        /// </summary>
        protected string CacheFilePath { get; }

        /// <summary>
        /// New Packages lock file path
        /// </summary>
        private readonly string _newPackagesLockFilePath;

        /// <summary>
        /// NuGet lock file which is either generated or updated to lock down NuGet packages version
        /// </summary>
        internal PackagesLockFile _newPackagesLockFile { get; }


        private readonly string _dependencyGraphSpecFilePath;

        private readonly DependencyGraphSpec _dependencyGraphSpec;

        public RestoreResult(
            bool success,
            IEnumerable<RestoreTargetGraph> restoreGraphs,
            IEnumerable<CompatibilityCheckResult> compatibilityCheckResults,
            IEnumerable<MSBuildOutputFile> msbuildFiles,
            LockFile lockFile,
            LockFile previousLockFile,
            string lockFilePath,
            CacheFile cacheFile,
            string cacheFilePath,
            string packagesLockFilePath,
            PackagesLockFile packagesLockFile,
            string dependencyGraphSpecFilePath,
            DependencyGraphSpec dependencyGraphSpec,
            ProjectStyle projectStyle,
            TimeSpan elapsedTime)
        {
            Success = success;
            RestoreGraphs = restoreGraphs;
            CompatibilityCheckResults = compatibilityCheckResults;
            MSBuildOutputFiles = msbuildFiles;
            LockFile = lockFile;
            LockFilePath = lockFilePath;
            PreviousLockFile = previousLockFile;
            CacheFile = cacheFile;
            CacheFilePath = cacheFilePath;
            _newPackagesLockFilePath = packagesLockFilePath;
            _newPackagesLockFile = packagesLockFile;
            _dependencyGraphSpecFilePath = dependencyGraphSpecFilePath;
            _dependencyGraphSpec = dependencyGraphSpec;
            ProjectStyle = projectStyle;
            ElapsedTime = elapsedTime;
            LogMessages = lockFile?.LogMessages ?? new List<IAssetsLogMessage>();
        }

        /// <summary>
        /// Calculates the complete set of all packages installed by this operation
        /// </summary>
        /// <remarks>
        /// This requires quite a bit of iterating over the graph so the result should be cached
        /// </remarks>
        /// <returns>A set of libraries that were installed by this operation</returns>
        public virtual ISet<LibraryIdentity> GetAllInstalled()
        {
            return new HashSet<LibraryIdentity>(RestoreGraphs.Where(g => !g.InConflict).SelectMany(g => g.Install).Distinct().Select(m => m.Library));
        }

        /// <summary>
        /// Calculates the complete set of all unresolved dependencies for this operation
        /// </summary>
        /// <remarks>
        /// This requires quite a bit of iterating over the graph so the result should be cached
        /// </remarks>
        /// <returns>A set of dependencies that were unable to be resolved by this operation</returns>
        public ISet<LibraryRange> GetAllUnresolved()
        {
            return new HashSet<LibraryRange>(RestoreGraphs.SelectMany(g => g.Unresolved).Distinct());
        }

        /// <summary>
        /// Commits the lock file contained in <see cref="LockFile"/> and the MSBuild targets/props to
        /// the local file system.
        /// </summary>
        /// <remarks>If <see cref="PreviousLockFile"/> and <see cref="LockFile"/> are identical
        ///  the file will not be written to disk.</remarks>
        public virtual async Task CommitAsync(ILogger log, CancellationToken token)
        {
            // Write the lock file
            var lockFileFormat = new LockFileFormat();

            var isTool = ProjectStyle == ProjectStyle.DotnetCliTool;

            // Commit the assets file to disk.
            await CommitAssetsFileAsync(
                lockFileFormat,
                result: this,
                log: log,
                toolCommit: isTool,
                token: token);

            //Commit the cache file to disk
            await CommitCacheFileAsync(
                log: log,
                toolCommit: isTool);

            // Commit the lock file to disk
            await CommitLockFileAsync(
                log: log,
                toolCommit: isTool);

            // Commit the dg spec file to disk
            await CommitDgSpecFileAsync(
                log: log,
                toolCommit: isTool);
        }

        private async Task CommitAssetsFileAsync(
            LockFileFormat lockFileFormat,
            IRestoreResult result,
            ILogger log,
            bool toolCommit,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Commit targets/props to disk before the assets file.
            // Visual Studio typically watches the assets file for changes
            // and begins a reload when that file changes.
            var buildFilesToWrite = result.MSBuildOutputFiles
                    .Where(e => BuildAssetsUtils.HasChanges(e.Content, e.Path, log));

            BuildAssetsUtils.WriteFiles(buildFilesToWrite, log);

            if (result.LockFile == null || result.LockFilePath == null)
            {
                // there is no assets file to be written so just return
                return;
            }

            // Avoid writing out the lock file if it is the same to avoid triggering an intellisense
            // update on a restore with no actual changes.
            if (result.PreviousLockFile == null
                || !result.PreviousLockFile.Equals(result.LockFile))
            {
                if (toolCommit)
                {
                    log.LogInformation(string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_ToolWritingAssetsFile,
                    result.LockFilePath));

                    await FileUtility.ReplaceWithLock(
                        (outputPath) => lockFileFormat.Write(outputPath, result.LockFile),
                        result.LockFilePath);
                }
                else
                {
                    log.LogInformation(string.Format(CultureInfo.CurrentCulture,
                        Strings.Log_WritingAssetsFile,
                        result.LockFilePath));

                    FileUtility.Replace(
                        (outputPath) => lockFileFormat.Write(outputPath, result.LockFile),
                        result.LockFilePath);
                }
            }
            else
            {
                if (toolCommit)
                {
                    log.LogInformation(string.Format(CultureInfo.CurrentCulture,
                        Strings.Log_ToolSkippingAssetsFile,
                        result.LockFilePath));
                }
                else
                {
                    log.LogInformation(string.Format(CultureInfo.CurrentCulture,
                        Strings.Log_SkippingAssetsFile,
                        result.LockFilePath));
                }
            }
        }

        private async Task CommitCacheFileAsync(ILogger log, bool toolCommit)
        {
            if (CacheFile != null && CacheFilePath != null)
            { // This is done to preserve the old behavior

                if (toolCommit)
                {
                    log.LogVerbose(string.Format(CultureInfo.CurrentCulture,
                            Strings.Log_ToolWritingCacheFile,
                            CacheFilePath));
                }
                else
                {
                    log.LogVerbose(string.Format(CultureInfo.CurrentCulture,
                            Strings.Log_WritingCacheFile,
                            CacheFilePath));
                }

                await FileUtility.ReplaceWithLock(
                   outPath => CacheFileFormat.Write(outPath, CacheFile),
                            CacheFilePath);
            }
        }

        private async Task CommitLockFileAsync(ILogger log, bool toolCommit)
        {
            // write packages lock file if it's not tool commit
            if (!toolCommit && _newPackagesLockFile != null && !string.IsNullOrEmpty(_newPackagesLockFilePath))
            {
                log.LogInformation(string.Format(CultureInfo.CurrentCulture,
                Strings.Log_WritingPackagesLockFile,
                _newPackagesLockFilePath));

                await FileUtility.ReplaceWithLock(
                    (outputPath) => PackagesLockFileFormat.Write(outputPath, _newPackagesLockFile),
                    _newPackagesLockFilePath);
            }
        }

        private async Task CommitDgSpecFileAsync(ILogger log, bool toolCommit)
        {
            if (!toolCommit && _dependencyGraphSpecFilePath != null && _dependencyGraphSpec != null)
            {
                log.LogVerbose($"Persisting dg to {_dependencyGraphSpecFilePath}");

                await FileUtility.ReplaceWithLock(
                    (outputPath) => _dependencyGraphSpec.Save(outputPath),
                    _dependencyGraphSpecFilePath);
            }
        }
    }
}
