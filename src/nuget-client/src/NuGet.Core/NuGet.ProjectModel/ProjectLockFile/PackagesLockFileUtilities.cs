// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel.ProjectLockFile;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public static class PackagesLockFileUtilities
    {
        public static bool IsNuGetLockFileEnabled(PackageSpec project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            var restorePackagesWithLockFile = project.RestoreMetadata?.RestoreLockProperties.RestorePackagesWithLockFile;
            return MSBuildStringUtility.IsTrue(restorePackagesWithLockFile) || File.Exists(GetNuGetLockFilePath(project));
        }

        public static string GetNuGetLockFilePath(PackageSpec project)
        {
            if (project.RestoreMetadata == null || project.BaseDirectory == null)
            {
                // RestoreMetadata or project BaseDirectory is not set which means it's probably called through test.
                return null;
            }

            var path = project.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath;

            if (!string.IsNullOrEmpty(path))
            {
                return Path.Combine(project.BaseDirectory, path);
            }

            var projectName = Path.GetFileNameWithoutExtension(project.RestoreMetadata.ProjectPath);
            return GetNuGetLockFilePath(project.BaseDirectory, projectName);
        }

        public static string GetNuGetLockFilePath(string baseDirectory, string projectName)
        {
            if (!string.IsNullOrEmpty(projectName))
            {
                var path = Path.Combine(baseDirectory, "packages." + projectName.Replace(' ', '_') + ".lock.json");

                if (File.Exists(path))
                {
                    return path;
                }
            }

            return Path.Combine(baseDirectory, PackagesLockFileFormat.LockFileName);
        }

        [Obsolete("This method is obsolete. Call IsLockFileValid instead.")]
        public static bool IsLockFileStillValid(DependencyGraphSpec dgSpec, PackagesLockFile nuGetLockFile)
        {
            return IsLockFileValid(dgSpec, nuGetLockFile).IsValid;
        }

        /// <summary>
        /// The lock file will get invalidated if one or more of the below are true
        ///     1. The target frameworks list of the current project was updated.
        ///     2. The runtime list of the current project waw updated.
        ///     3. The packages of the current project were updated.
        ///     4. The packages of the dependent projects were updated.
        ///     5. The framework list of the dependent projects were updated with frameworks incompatible with the main project framework.
        ///     6. If the version of the <paramref name="nuGetLockFile"/> is larger than the current tools <see cref="PackagesLockFileFormat.PackagesLockFileVersion"/>.
        /// </summary>
        /// <param name="dgSpec">The <see cref="DependencyGraphSpec"/> for the new project defintion.</param>
        /// <param name="nuGetLockFile">The current <see cref="PackagesLockFile"/>.</param>
        /// <returns>Returns LockFileValidityWithInvalidReasons object with IsValid set to true if the lock file is valid false otherwise.
        /// The second return type is a localized message that indicates in further detail the reason for the inconsistency.</returns>
        public static LockFileValidationResult IsLockFileValid(DependencyGraphSpec dgSpec, PackagesLockFile nuGetLockFile)
        {
            if (dgSpec == null)
                throw new ArgumentNullException(nameof(dgSpec));

            if (nuGetLockFile == null)
                throw new ArgumentNullException(nameof(nuGetLockFile));

            List<string> invalidReasons = new List<string>();

            // Current tools know how to read only previous formats including the current
            if (PackagesLockFileFormat.PackagesLockFileVersion < nuGetLockFile.Version)
            {
                invalidReasons.Add(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackagesLockFile_IncompatibleLockFileVersion,
                            PackagesLockFileFormat.PackagesLockFileVersion
                            ));

                return new LockFileValidationResult(false, invalidReasons);
            }

            var uniqueName = dgSpec.Restore.First();
            var project = dgSpec.GetProjectSpec(uniqueName);

            // Validate all the direct dependencies
            NuGetFramework[] lockFileFrameworks = nuGetLockFile.Targets
                .Where(t => t.TargetFramework != null)
                .Select(t => t.TargetFramework)
                .Distinct()
                .ToArray();

            if (project.TargetFrameworks.Count != lockFileFrameworks.Length)
            {
                invalidReasons.Add(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackagesLockFile_MismatchedTargetFrameworks,
                            string.Join(",", project.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName())),
                            string.Join(",", lockFileFrameworks.Select(e => e.GetShortFolderName()))
                            ));
            }
            else
            {
                // Validate the runtimes for the current project did not change.
                var projectRuntimesKeys = project.RuntimeGraph.Runtimes.Select(r => r.Key).Where(k => k != null);
                var lockFileRuntimes = nuGetLockFile.Targets.Select(t => t.RuntimeIdentifier).Where(r => r != null).Distinct();

                if (!projectRuntimesKeys.OrderedEquals(
                            lockFileRuntimes,
                            x => x,
                            StringComparer.InvariantCultureIgnoreCase,
                            StringComparer.InvariantCultureIgnoreCase))
                {
                    invalidReasons.Add(string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.PackagesLockFile_RuntimeIdentifiersChanged,
                                    string.Join(";", projectRuntimesKeys.OrderBy(e => e)),
                                    string.Join(";", lockFileRuntimes.OrderBy(e => e))
                                    ));
                }

                foreach (var framework in project.TargetFrameworks)
                {
                    var target = nuGetLockFile.Targets.FirstOrDefault(
                        t => EqualityUtility.EqualsWithNullCheck(t.TargetFramework, framework.FrameworkName));

                    if (target == null)
                    {
                        // a new target found in the dgSpec so invalidate existing lock file.
                        invalidReasons.Add(string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.PackagesLockFile_NewTargetFramework,
                                    framework.FrameworkName.GetShortFolderName())
                                );

                        continue;
                    }

                    IEnumerable<LockFileDependency> directDependencies = target.Dependencies.Where(dep => dep.Type == PackageDependencyType.Direct);

                    (var hasProjectDependencyChanged, var pmessage) = HasDirectPackageDependencyChanged(framework.Dependencies, directDependencies, target.TargetFramework);
                    if (hasProjectDependencyChanged)
                    {
                        // lock file is out of sync
                        invalidReasons.Add(pmessage);
                    }

                    var transitiveDependenciesEnforcedByCentralVersions = target.Dependencies.Where(dep => dep.Type == PackageDependencyType.CentralTransitive).ToList();
                    var transitiveDependencies = target.Dependencies.Where(dep => dep.Type == PackageDependencyType.Transitive).ToList();

                    (var hasTransitiveDependencyChanged, var tmessage) = HasProjectTransitiveDependencyChanged(framework.CentralPackageVersions, transitiveDependenciesEnforcedByCentralVersions, transitiveDependencies);
                    if (hasTransitiveDependencyChanged)
                    {
                        // lock file is out of sync
                        invalidReasons.Add(tmessage);
                    }
                }

                // Validate all P2P references
                foreach (var restoreMetadataFramework in project.RestoreMetadata.TargetFrameworks)
                {
                    var target = nuGetLockFile.Targets.FirstOrDefault(
                        t => EqualityUtility.EqualsWithNullCheck(t.TargetFramework, restoreMetadataFramework.FrameworkName));

                    if (target == null)
                        continue;

                    var queue = new Queue<Tuple<string, string>>();
                    var visitedP2PReference = new HashSet<string>();

                    foreach (var projectReference in restoreMetadataFramework.ProjectReferences)
                    {
                        if (visitedP2PReference.Add(projectReference.ProjectUniqueName))
                        {
                            PackageSpec spec = dgSpec.GetProjectSpec(projectReference.ProjectUniqueName);
                            queue.Enqueue(new Tuple<string, string>(spec.Name, projectReference.ProjectUniqueName));

                            while (queue.Count > 0)
                            {
                                var projectNames = queue.Dequeue();
                                var p2pUniqueName = projectNames.Item2;
                                var p2pProjectName = projectNames.Item1;
                                var projectDependency = target.Dependencies.FirstOrDefault(
                                    dep => dep.Type == PackageDependencyType.Project &&
                                    StringComparer.OrdinalIgnoreCase.Equals(dep.Id, p2pProjectName));

                                if (projectDependency == null)
                                {
                                    // new direct project dependency.
                                    // If there are changes in the P2P2P references, they will be caught in HasP2PDependencyChanged.
                                    invalidReasons.Add(string.Format(
                                            CultureInfo.CurrentCulture,
                                            Strings.PackagesLockFile_ProjectReferenceAdded,
                                            p2pProjectName,
                                            target.TargetFramework.GetShortFolderName()
                                            ));

                                    continue;
                                }

                                var p2pSpec = dgSpec.GetProjectSpec(p2pUniqueName);

                                if (p2pSpec != null)
                                {
                                    TargetFrameworkInformation p2pSpecTargetFrameworkInformation = default;
                                    if (p2pSpec.RestoreMetadata.ProjectStyle == ProjectStyle.PackagesConfig || p2pSpec.RestoreMetadata.ProjectStyle == ProjectStyle.Unknown)
                                    {
                                        // Skip compat check and dependency check for non PR projects.
                                        // Projects that are not PR do not undergo compat checks by NuGet and do not contribute anything transitively.
                                        p2pSpecTargetFrameworkInformation = p2pSpec.TargetFrameworks.FirstOrDefault();
                                    }
                                    else
                                    {
                                        // This does not consider ATF.
                                        p2pSpecTargetFrameworkInformation = NuGetFrameworkUtility.GetNearest(p2pSpec.TargetFrameworks, restoreMetadataFramework.FrameworkName, e => e.FrameworkName);
                                    }
                                    // No compatible framework found
                                    if (p2pSpecTargetFrameworkInformation != null)
                                    {
                                        // We need to compare the main framework only. Ignoring fallbacks.
                                        var p2pSpecProjectRestoreMetadataFrameworkInfo = p2pSpec.RestoreMetadata.TargetFrameworks.FirstOrDefault(
                                            t => NuGetFramework.Comparer.Equals(p2pSpecTargetFrameworkInformation.FrameworkName, t.FrameworkName));

                                        if (p2pSpecProjectRestoreMetadataFrameworkInfo != null)
                                        {
                                            (var hasChanged, var message) = HasP2PDependencyChanged(p2pSpecTargetFrameworkInformation.Dependencies, p2pSpecProjectRestoreMetadataFrameworkInfo.ProjectReferences, projectDependency, dgSpec);

                                            if (hasChanged)
                                            {
                                                // P2P transitive package dependencies have changed                                            
                                                invalidReasons.Add(message);
                                            }

                                            foreach (var reference in p2pSpecProjectRestoreMetadataFrameworkInfo.ProjectReferences)
                                            {
                                                // Do not add private assets for processing.
                                                if (visitedP2PReference.Add(reference.ProjectUniqueName) && reference.PrivateAssets != LibraryIncludeFlags.All)
                                                {
                                                    var referenceSpec = dgSpec.GetProjectSpec(reference.ProjectUniqueName);
                                                    queue.Enqueue(new Tuple<string, string>(referenceSpec.Name, reference.ProjectUniqueName));
                                                }
                                            }
                                        }
                                        else // This should never happen.
                                        {
                                            throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.PackagesLockFile_RestoreMetadataMissingTfms));
                                        }
                                    }
                                    else
                                    {
                                        invalidReasons.Add(string.Format(
                                               CultureInfo.CurrentCulture,
                                               Strings.PackagesLockFile_ProjectReferenceHasNoCompatibleTargetFramework,
                                               p2pProjectName,
                                               restoreMetadataFramework.FrameworkName.GetShortFolderName()
                                               ));
                                    }
                                }
                                else // This can't happen. When adding the queue, the referenceSpec HAS to be discovered. If the project is otherwise missing, it will be discovered in HasP2PDependencyChanged
                                {
                                    throw new Exception(string.Format(
                                        CultureInfo.CurrentCulture,
                                        Strings.PackagesLockFile_UnableToLoadPackagespec,
                                        p2pUniqueName));
                                }
                            }
                        }
                    }
                }
            }

            bool isLockFileValid = invalidReasons.Count == 0;

            return new LockFileValidationResult(isLockFileValid, invalidReasons);
        }

        /// <summary>Compares two lock files to check if the structure is the same (all values are the same, other
        /// than SHA hash), and matches dependencies so the caller can easily compare SHA hashes.</summary>
        /// <param name="expected">The expected lock file structure. Usuaully generated from the project.</param>
        /// <param name="actual">The lock file that was loaded from the file on disk.</param>
        /// <returns>A <see cref="LockFileValidityWithMatchedResults"/>.</returns>
        public static LockFileValidityWithMatchedResults IsLockFileStillValid(PackagesLockFile expected, PackagesLockFile actual)
        {
            if (expected == null)
            {
                throw new ArgumentNullException(nameof(expected));
            }
            if (actual == null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            // do quick checks for obvious structure differences
            if (expected.Version != actual.Version)
            {
                return LockFileValidityWithMatchedResults.Invalid;
            }

            if (expected.Targets.Count != actual.Targets.Count)
            {
                return LockFileValidityWithMatchedResults.Invalid;
            }

            foreach (var expectedTarget in expected.Targets)
            {
                PackagesLockFileTarget actualTarget = null;

                for (var i = 0; i < actual.Targets.Count; i++)
                {
                    if (actual.Targets[i].TargetFramework == expectedTarget.TargetFramework)
                    {
                        if (actualTarget == null)
                        {
                            actualTarget = actual.Targets[i];
                        }
                        else
                        {
                            // more than 1? possible bug or bad hand edited lock file.
                            return LockFileValidityWithMatchedResults.Invalid;
                        }
                    }

                    if (actualTarget == null)
                    {
                        return LockFileValidityWithMatchedResults.Invalid;
                    }

                    if (actualTarget.Dependencies.Count != expectedTarget.Dependencies.Count)
                    {
                        return LockFileValidityWithMatchedResults.Invalid;
                    }
                }
            }

            // no obvious structure difference, so start trying to match individual dependencies
            var matchedDependencies = new List<KeyValuePair<LockFileDependency, LockFileDependency>>();
            var isLockFileStillValid = true;
            var dependencyComparer = LockFileDependencyComparerWithoutContentHash.Default;

            foreach (PackagesLockFileTarget expectedTarget in expected.Targets)
            {
                PackagesLockFileTarget actualTarget = actual.Targets.Single(t => t.TargetFramework == expectedTarget.TargetFramework);

                // Duplicate dependencies list so we can remove matches to validate that all dependencies were matched
                var actualDependencies = new Dictionary<LockFileDependency, LockFileDependency>(
                    actualTarget.Dependencies.Count,
                    dependencyComparer);
                foreach (LockFileDependency actualDependency in actualTarget.Dependencies)
                {
                    actualDependencies.Add(actualDependency, actualDependency);
                }

                foreach (LockFileDependency expectedDependency in expectedTarget.Dependencies)
                {
                    if (actualDependencies.TryGetValue(expectedDependency, out var actualDependency))
                    {
                        matchedDependencies.Add(new KeyValuePair<LockFileDependency, LockFileDependency>(expectedDependency, actualDependency));
                        actualDependencies.Remove(actualDependency);
                    }
                    else
                    {
                        return LockFileValidityWithMatchedResults.Invalid;
                    }
                }

                if (actualDependencies.Count != 0)
                {
                    return LockFileValidityWithMatchedResults.Invalid;
                }
            }

            return new LockFileValidityWithMatchedResults(isLockFileStillValid, matchedDependencies);
        }

        private static (bool, string) HasDirectPackageDependencyChanged(IEnumerable<LibraryDependency> newDependencies, IEnumerable<LockFileDependency> lockFileDependencies, NuGetFramework nuGetFramework)
        {
            // If the count is not the same, something has changed.
            // Otherwise the N^2 walk below determines whether anything has changed.
            var newPackageDependencies = newDependencies.Where(dep => dep.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package);

            var newPackageDependenciesCount = newPackageDependencies.Count();
            var lockFileDependenciesCount = lockFileDependencies.Count();

            if (newPackageDependenciesCount != lockFileDependenciesCount)
            {
                return (true,
                           string.Format(
                               CultureInfo.CurrentCulture,
                               Strings.PackagesLockFile_PackageReferencesHaveChanged,
                               nuGetFramework.GetShortFolderName(),
                               lockFileDependenciesCount > 0 ? string.Join(", ", lockFileDependencies.Select(e => e.Id + ":" + e.RequestedVersion.ToNormalizedString()).OrderBy(dep => dep)) : Strings.None,
                               newPackageDependenciesCount > 0 ? string.Join(", ", newPackageDependencies.Select(e => e.LibraryRange.Name + ":" + e.LibraryRange.VersionRange.ToNormalizedString()).OrderBy(dep => dep)) : Strings.None)
                           );
            }

            foreach (var dependency in newPackageDependencies)
            {
                var lockFileDependency = lockFileDependencies.FirstOrDefault(d => StringComparer.OrdinalIgnoreCase.Equals(d.Id, dependency.Name));

                if (lockFileDependency == null)
                {
                    // dependency has changed and lock file is out of sync.
                    return (true,
                               string.Format(
                                   CultureInfo.CurrentCulture,
                                   Strings.PackagesLockFile_PackageReferenceAdded,
                                   dependency.Name,
                                   nuGetFramework.GetShortFolderName())
                            );
                }
                if (!EqualityUtility.EqualsWithNullCheck(lockFileDependency.RequestedVersion, dependency.LibraryRange.VersionRange))
                {
                    // dependency has changed and lock file is out of sync.
                    return (true,
                               string.Format(
                                   CultureInfo.CurrentCulture,
                                   Strings.PackagesLockFile_PackageReferenceVersionChanged,
                                   dependency.Name,
                                   lockFileDependency.RequestedVersion.ToNormalizedString(),
                                   dependency.LibraryRange.VersionRange.ToNormalizedString())
                            );
                }
            }

            // no dependency changed. Lock file is still valid.
            return (false, string.Empty);
        }

        private static (bool, string) HasP2PDependencyChanged(IEnumerable<LibraryDependency> newDependencies, IEnumerable<ProjectRestoreReference> projectRestoreReferences, LockFileDependency projectDependency, DependencyGraphSpec dgSpec)
        {
            // If the count is not the same, something has changed.
            // Otherwise we N^2 walk below determines whether anything has changed.
            var transitivelyFlowingDependencies = newDependencies.Where(
                dep => dep.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package && dep.SuppressParent != LibraryIncludeFlags.All);

            var transitivelyFlowingProjectReferences = projectRestoreReferences.Where(e => e.PrivateAssets != LibraryIncludeFlags.All);

            var transitiveDependencies = transitivelyFlowingDependencies.Count() + transitivelyFlowingProjectReferences.Count();

            if (transitiveDependencies != projectDependency.Dependencies.Count)
            {
                return (true,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackagesLockFile_ProjectReferencesHasChange,
                            projectDependency.Id,
                            transitiveDependencies > 0 ? string.Join(",", transitivelyFlowingDependencies.Select(dep => dep.Name).Concat(projectRestoreReferences.Select(dep => dep.ProjectUniqueName)).OrderBy(dep => dep)) : Strings.None,
                            projectDependency.Dependencies.Count > 0 ? string.Join(",", projectDependency.Dependencies.Select(dep => dep.Id).OrderBy(dep => dep)) : Strings.None
                            )
                        );
            }

            foreach (var dependency in transitivelyFlowingDependencies)
            {
                var matchedP2PLibrary = projectDependency.Dependencies.FirstOrDefault(dep => StringComparer.OrdinalIgnoreCase.Equals(dep.Id, dependency.Name));

                if (matchedP2PLibrary == null || !EqualityUtility.EqualsWithNullCheck(matchedP2PLibrary.VersionRange, dependency.LibraryRange.VersionRange))
                {
                    // P2P dependency has changed and lock file is out of sync.
                    return (true,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackagesLockFile_ProjectReferenceDependenciesHasChanged,
                            projectDependency.Id
                            )
                        );
                }
            }

            foreach (var dependency in transitivelyFlowingProjectReferences)
            {
                var referenceSpec = dgSpec.GetProjectSpec(dependency.ProjectUniqueName);
                var matchedP2PLibrary = projectDependency.Dependencies.FirstOrDefault(dep => StringComparer.OrdinalIgnoreCase.Equals(dep.Id, referenceSpec.Name));

                if (matchedP2PLibrary == null) // Do not check the version for the projects, or else https://github.com/nuget/home/issues/7935
                {
                    // P2P dependency has changed and lock file is out of sync.
                    return (true,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackagesLockFile_ProjectReferenceDependenciesHasChanged,
                            projectDependency.Id
                            )
                        );
                }
            }

            // no dependency changed. Lock file is still valid.
            return (false, string.Empty);
        }

        /// <summary>
        /// The method will return true if:
        /// 1. If a transitive dependency from the lock file is now added to the central file.
        ///     or
        /// 1. If there is a mistmatch between the RequestedVersion of a lock file dependency marked as CentralTransitive and the the version specified in the central package management file.
        ///     or
        /// 2. If a central version that is a transitive dependency is removed from CPVM the lock file is invalidated.
        /// </summary>
        private static (bool, string) HasProjectTransitiveDependencyChanged(
            IDictionary<string, CentralPackageVersion> centralPackageVersions,
            IList<LockFileDependency> lockFileCentralTransitiveDependencies,
            IList<LockFileDependency> lockTransitiveDependencies)
        {
            // Transitive dependencies moved to be centraly managed will invalidate the lock file
            LockFileDependency dependency = lockTransitiveDependencies.FirstOrDefault(dep => centralPackageVersions.ContainsKey(dep.Id));

            if (dependency != null)
            {
                return (true,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackagesLockFile_ProjectTransitiveDependencyChanged,
                            dependency.Id
                            )
                        );
            }

            foreach (var lockFileDependencyEnforcedByCPV in lockFileCentralTransitiveDependencies)
            {
                if (centralPackageVersions.TryGetValue(lockFileDependencyEnforcedByCPV.Id, out var centralPackageVersion))
                {
                    if (centralPackageVersion != null && !EqualityUtility.EqualsWithNullCheck(lockFileDependencyEnforcedByCPV.RequestedVersion, centralPackageVersion.VersionRange))
                    {
                        return (true,
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.PackagesLockFile_ProjectTransitiveDependencyVersionChanged,
                                    lockFileDependencyEnforcedByCPV.RequestedVersion.ToNormalizedString(),
                                    centralPackageVersion.VersionRange.ToNormalizedString()
                                    )
                                );
                    }
                    continue;
                }

                // The central version was removed
                return (true,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackagesLockFile_CentralPackageVersionRemoved,
                            lockFileDependencyEnforcedByCPV.Id
                            )
                        );
            }

            return (false, string.Empty);
        }

        /// <summary>
        /// A class to return information about lock file validity
        /// </summary>
        public class LockFileValidityWithMatchedResults
        {
            /// <summary>
            /// True if the lock file had the expected structure (all values expected, other than content hash)
            /// </summary>
            public bool IsValid { get; }

            /// <summary>
            /// A list of matched dependencies, so content sha can easily be checked.
            /// </summary>
            public IReadOnlyList<KeyValuePair<LockFileDependency, LockFileDependency>> MatchedDependencies { get; }

            public LockFileValidityWithMatchedResults(bool isValid, IReadOnlyList<KeyValuePair<LockFileDependency, LockFileDependency>> matchedDependencies)
            {
                IsValid = isValid;
                MatchedDependencies = matchedDependencies;
            }

            public static readonly LockFileValidityWithMatchedResults Invalid =
                new LockFileValidityWithMatchedResults(isValid: false, matchedDependencies: null);
        }
    }

    /// <summary>
    /// A class to return information about lock file validity with invalid reasons.
    /// </summary>
    public class LockFileValidationResult
    {
        /// <summary>
        /// True if the packages.lock.json file dependencies match project.assets.json file dependencies
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// A list of reasons why lock file is invalid
        /// </summary>
        public IReadOnlyList<string> InvalidReasons { get; }

        public LockFileValidationResult(bool isValid, IReadOnlyList<string> invalidReasons)
        {
            IsValid = isValid;
            InvalidReasons = invalidReasons;
        }
    }
}
