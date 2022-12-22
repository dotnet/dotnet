// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.SolutionRestoreManager
{
    [Export(typeof(ISolutionRestoreChecker))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SolutionUpToDateChecker : ISolutionRestoreChecker
    {
        private IList<string> _failedProjects = new List<string>();
        private DependencyGraphSpec _cachedDependencyGraphSpec;
        private Dictionary<string, RestoreData> _restoreData = new Dictionary<string, RestoreData>();

        public void SaveRestoreStatus(IReadOnlyList<RestoreSummary> restoreSummaries)
        {
            if (restoreSummaries == null)
            {
                throw new ArgumentNullException(nameof(restoreSummaries));
            }

            _failedProjects.Clear();

            foreach (var summary in restoreSummaries)
            {
                if (summary.Success)
                {
                    var packageSpec = _cachedDependencyGraphSpec.GetProjectSpec(summary.InputPath);
                    GetOutputFilePaths(packageSpec, out string assetsFilePath, out string cacheFilePath, out string targetsFilePath, out string propsFilePath, out string lockFilePath);
                    var messages = !packageSpec.RestoreSettings.HideWarningsAndErrors && summary.Errors.Count > 0 ?
                            summary.Errors :
                            null;

                    _restoreData[summary.InputPath] = new RestoreData()
                    {
                        _lastAssetsFileWriteTime = GetLastWriteTime(assetsFilePath),
                        _lastCacheFileWriteTime = GetLastWriteTime(cacheFilePath),
                        _lastTargetsFileWriteTime = GetLastWriteTime(targetsFilePath),
                        _lastPropsFileWriteTime = GetLastWriteTime(propsFilePath),
                        _lastLockFileWriteTime = GetLastWriteTime(lockFilePath),
                        _globalPackagesFolderCreationTime = GetCreationTime(packageSpec.RestoreMetadata.PackagesPath),
                        _messages = messages
                    };
                }
                else
                {
                    _failedProjects.Add(summary.InputPath);
                }
            }
        }

        // The algorithm here is a 2 pass. In reality the 2nd pass can do a lot but for huge benefits :)
        // Pass #1
        // We check all the specs against the cached ones if any. Any project with a change in the spec is considered dirty.
        // If a project had previously been restored and it failed, it is considered dirty.
        // Every project that is considered to have a dirty spec will be important in pass #2.
        // In the first pass, we also validate the outputs for the projects. Note that these are independent and project specific. Outputs not being up to date it irrelevant for transitivity.
        // Pass #2
        // For every project with a dirty spec (the outputs don't matter here), we want to ensure that its parent projects are marked as dirty as well.
        // This is a bit more expensive since PackageSpecs do not retain pointers to the projects that reference them as ProjectReference.
        // Finally we only update the cache specs if Pass #1 determined that there are projects that are not up to date.
        // Result
        // Lastly all the projects marked as having dirty specs & dirty outputs are returned.
        // Before we return the list of projects that are not up to date, we always make sure to replay the warnings for the up to date projects.
        public IEnumerable<string> PerformUpToDateCheck(DependencyGraphSpec dependencyGraphSpec, ILogger logger)
        {
            if (dependencyGraphSpec == null)
            {
                throw new ArgumentNullException(nameof(dependencyGraphSpec));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (_cachedDependencyGraphSpec != null)
            {
                var dirtySpecs = new List<string>();
                var dirtyOutputs = new List<string>();
                bool hasDirtyNonTransitiveSpecs = false;

                // Pass #1. Validate all the data (i/o)
                // 1a. Validate the package specs (references & settings)
                // 1b. Validate the expected outputs (assets file, nuget.g.*, lock file)
                var unloadedProjects = _restoreData.Keys.ToHashSet();
                foreach (var project in dependencyGraphSpec.Projects)
                {
                    var projectUniqueName = project.RestoreMetadata.ProjectUniqueName;
                    var cache = _cachedDependencyGraphSpec.GetProjectSpec(projectUniqueName);

                    if (cache == null || !project.Equals(cache))
                    {
                        dirtySpecs.Add(projectUniqueName);
                    }
                    unloadedProjects.Remove(projectUniqueName);

                    if (project.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference ||
                        project.RestoreMetadata.ProjectStyle == ProjectStyle.ProjectJson)
                    {
                        if (!_failedProjects.Contains(projectUniqueName) && _restoreData.TryGetValue(projectUniqueName, out RestoreData restoreData))
                        {
                            GetOutputFilePaths(project, out string assetsFilePath, out string cacheFilePath, out string targetsFilePath, out string propsFilePath, out string lockFilePath);
                            if (!AreOutputsUpToDate(assetsFilePath, cacheFilePath, targetsFilePath, propsFilePath, lockFilePath, project.RestoreMetadata.PackagesPath, restoreData))
                            {
                                dirtyOutputs.Add(projectUniqueName);
                            }
                        }
                        else
                        {
                            dirtyOutputs.Add(projectUniqueName);
                        }
                    }
                    else
                    {
                        hasDirtyNonTransitiveSpecs = true;
                    }
                }

                // Remove the cached data of the unloaded projects if any.
                foreach (var project in unloadedProjects)
                {
                    _restoreData.Remove(project);
                }

                // Fast path. Skip Pass #2
                if (dirtySpecs.Count == 0 && dirtyOutputs.Count == 0)
                {
                    ReplayAllWarnings(_restoreData, (string projectName) => true, logger);
                    return Enumerable.Empty<string>();
                }
                // Update the cache before Pass #2
                _cachedDependencyGraphSpec = dependencyGraphSpec;

                // Pass #2 For any dirty specs discrepancies, mark them and their parents as needing restore.
                var dirtyProjects = GetParents(dirtySpecs, dependencyGraphSpec);

                // All dirty projects + projects with outputs that need to be restored
                // - the projects that are non transitive that never needed restore anyways, hence the intersection with the provider restore projects!
                var resultSpecs = dirtyProjects.Union(dirtyOutputs);
                if (hasDirtyNonTransitiveSpecs)
                {
                    resultSpecs = dependencyGraphSpec.Restore.Intersect(resultSpecs);
                }

                ReplayAllWarnings(_restoreData, (string projectName) => !resultSpecs.Contains(projectName), logger);
                return resultSpecs;
            }
            else
            {
                _cachedDependencyGraphSpec = dependencyGraphSpec;

                return dependencyGraphSpec.Restore;
            }
        }

        private void ReplayAllWarnings(Dictionary<string, RestoreData> restoreData, Func<string, bool> shouldReplayWarnings, ILogger logger)
        {
            foreach (var restoreOutputs in restoreData)
            {
                if (shouldReplayWarnings(restoreOutputs.Key) && restoreOutputs.Value._messages != null)
                {
                    foreach (var logMessage in restoreOutputs.Value._messages)
                    {
                        logger.Log(logMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Given a list of project unique names, goes through the dg spec and returns the current projects + all their parents
        /// </summary>
        /// <param name="DirtySpecs">The projects for which we need to find the parents</param>
        /// <param name="dependencyGraphSpec">The dependency graph spec contain the projects passed in dirty specs at the minimum.</param>
        /// <returns>The list of the projects passed in and their parents in the dependencyGraphSpec</returns>
        internal static IList<string> GetParents(List<string> DirtySpecs, DependencyGraphSpec dependencyGraphSpec)
        {
            var projectsByUniqueName = dependencyGraphSpec.Projects
                .ToDictionary(t => t.RestoreMetadata.ProjectUniqueName, t => t, PathUtility.GetStringComparerBasedOnOS());

            var DirtyProjects = new HashSet<string>(DirtySpecs, PathUtility.GetStringComparerBasedOnOS());

            var sortedProjects = DependencyGraphSpec.SortPackagesByDependencyOrder(dependencyGraphSpec.Projects);

            foreach (var project in sortedProjects)
            {
                if (!DirtyProjects.Contains(project.RestoreMetadata.ProjectUniqueName))
                {
                    var projectReferences = GetPackageSpecDependencyIds(project);

                    foreach (var projectReference in projectReferences)
                    {
                        if (DirtyProjects.Contains(projectReference))
                        {
                            DirtyProjects.Add(project.RestoreMetadata.ProjectUniqueName);
                        }
                    }
                }
            }
            return DirtyProjects.ToList();
        }

        private static string[] GetPackageSpecDependencyIds(PackageSpec spec)
        {
            return spec.RestoreMetadata
                .TargetFrameworks
                .SelectMany(r => r.ProjectReferences)
                .Select(r => r.ProjectUniqueName)
                .Distinct(PathUtility.GetStringComparerBasedOnOS())
                .ToArray();
        }

        internal static void GetOutputFilePaths(PackageSpec packageSpec, out string assetsFilePath, out string cacheFilePath, out string targetsFilePath, out string propsFilePath, out string lockFilePath)
        {
            assetsFilePath = GetAssetsFilePath(packageSpec);
            cacheFilePath = NoOpRestoreUtilities.GetProjectCacheFilePath(packageSpec.RestoreMetadata.OutputPath);
            targetsFilePath = BuildAssetsUtils.GetMSBuildFilePath(packageSpec, BuildAssetsUtils.TargetsExtension);
            propsFilePath = BuildAssetsUtils.GetMSBuildFilePath(packageSpec, BuildAssetsUtils.PropsExtension);
            lockFilePath = packageSpec.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference ?
                PackagesLockFileUtilities.GetNuGetLockFilePath(packageSpec) :
                null;
        }

        private static bool AreOutputsUpToDate(string assetsFilePath, string cacheFilePath, string targetsFilePath, string propsFilePath, string lockFilePath, string globalPackagesFolderPath, RestoreData outputWriteTime)
        {
            DateTime currentAssetsFileWriteTime = GetLastWriteTime(assetsFilePath);
            DateTime currentCacheFilePath = GetLastWriteTime(cacheFilePath);
            DateTime currentTargetsFilePath = GetLastWriteTime(targetsFilePath);
            DateTime currentPropsFilePath = GetLastWriteTime(propsFilePath);
            DateTime currentLockFilePath = GetLastWriteTime(lockFilePath);
            DateTime globalPackagesFolderCreationTime = GetCreationTime(globalPackagesFolderPath);

            return outputWriteTime._lastAssetsFileWriteTime.Equals(currentAssetsFileWriteTime) &&
                   outputWriteTime._lastCacheFileWriteTime.Equals(currentCacheFilePath) &&
                   outputWriteTime._lastTargetsFileWriteTime.Equals(currentTargetsFilePath) &&
                   outputWriteTime._lastPropsFileWriteTime.Equals(currentPropsFilePath) &&
                   outputWriteTime._lastLockFileWriteTime.Equals(currentLockFilePath) &&
                   outputWriteTime._globalPackagesFolderCreationTime.Equals(globalPackagesFolderCreationTime);
        }

        private static DateTime GetLastWriteTime(string assetsFilePath)
        {
            if (!string.IsNullOrWhiteSpace(assetsFilePath))
            {
                var fileInfo = new FileInfo(assetsFilePath);
                if (fileInfo.Exists)
                {
                    return fileInfo.LastWriteTimeUtc;
                }
            }
            return default;
        }

        private static DateTime GetCreationTime(string assetsFilePath)
        {
            if (!string.IsNullOrWhiteSpace(assetsFilePath))
            {
                var fileInfo = new FileInfo(assetsFilePath);
                if (fileInfo.Exists || ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory))
                {
                    return fileInfo.CreationTimeUtc;
                }
            }
            return default;
        }

        private static string GetAssetsFilePath(PackageSpec packageSpec)
        {
            if (packageSpec.RestoreMetadata?.ProjectStyle == ProjectStyle.PackageReference)
            {
                return Path.Combine(
                    packageSpec.RestoreMetadata.OutputPath,
                    LockFileFormat.AssetsFileName);
            }
            else if (packageSpec.RestoreMetadata?.ProjectStyle == ProjectStyle.ProjectJson)
            {
                return ProjectJsonPathUtilities.GetLockFilePath(packageSpec.FilePath);
            }
            return null;
        }

        public void CleanCache()
        {
            _failedProjects.Clear();
            _cachedDependencyGraphSpec = null;
            _restoreData.Clear();
        }

        internal struct RestoreData
        {
            internal DateTime _lastAssetsFileWriteTime;
            internal DateTime _lastCacheFileWriteTime;
            internal DateTime _lastTargetsFileWriteTime;
            internal DateTime _lastPropsFileWriteTime;
            internal DateTime _lastLockFileWriteTime;
            internal DateTime _globalPackagesFolderCreationTime;
            internal IReadOnlyList<IRestoreLogMessage> _messages;
        }
    }
}
