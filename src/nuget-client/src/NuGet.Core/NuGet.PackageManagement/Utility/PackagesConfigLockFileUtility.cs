// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.PackageManagement.Utility
{
    public static class PackagesConfigLockFileUtility
    {
        private static readonly IComparer _dependencyComparer = new DependencyComparer();

        internal static void UpdateLockFile(
            MSBuildNuGetProject msbuildProject,
            List<NuGetProjectAction> actionsList,
            CancellationToken token)
        {
            if (msbuildProject == null)
            {
                // probably a `nuget.exe install` command, which doesn't support lock files until lock file arguments are implemented
                return;
            }

            var lockFileName = GetPackagesLockFilePath(msbuildProject);
            var lockFileExists = File.Exists(lockFileName);
            var enableLockFile = IsRestorePackagesWithLockFileEnabled(msbuildProject);
            if (enableLockFile == false && lockFileExists)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidLockFileInput, lockFileName);
                throw new InvalidOperationException(message);
            }
            else if (enableLockFile == true || lockFileExists)
            {
                var lockFile = GetLockFile(lockFileExists, lockFileName);
                lockFile.Targets[0].TargetFramework = msbuildProject.ProjectSystem.TargetFramework;
                var contentHashUtil = new PackagesConfigContentHashProvider(msbuildProject.FolderNuGetProject);
                ApplyChanges(lockFile, actionsList, contentHashUtil, token);
                PackagesLockFileFormat.Write(lockFileName, lockFile);

                // Add lock file to msbuild project, so it appears in solution explorer and is added to TFS source control.
                if (msbuildProject != null)
                {
                    var projectUri = new Uri(msbuildProject.MSBuildProjectPath);
                    var lockFileUri = new Uri(lockFileName);
                    var lockFileRelativePath = projectUri.MakeRelativeUri(lockFileUri).OriginalString;
                    msbuildProject.ProjectSystem.AddExistingFile(lockFileRelativePath);
                }
            }
        }

        internal static string GetPackagesLockFilePath(MSBuildNuGetProject msbuildProject)
        {
            var directory = (string)msbuildProject.Metadata["FullPath"];
            var msbuildProperty = msbuildProject.ProjectSystem?.GetPropertyValue("NuGetLockFilePath");
            var projectName = (string)msbuildProject.Metadata["UniqueName"];

            return GetPackagesLockFilePath(directory, msbuildProperty, projectName);
        }

        public static string GetPackagesLockFilePath(string projectPath, string nuGetLockFilePath, string projectName)
        {
            if (!string.IsNullOrWhiteSpace(nuGetLockFilePath))
            {
                return Path.Combine(projectPath, nuGetLockFilePath);
            }

            return PackagesLockFileUtilities.GetNuGetLockFilePath(projectPath, projectName);
        }

        public static IReadOnlyList<IRestoreLogMessage> ValidatePackagesConfigLockFiles(
            string projectFile,
            string packagesConfigFile,
            string projectName,
            string nuGetLockFilePath,
            string restorePackagesWithLockFile,
            NuGetFramework projectTfm,
            string packagesFolderPath,
            bool restoreLockedMode,
            CancellationToken token)
        {
            var lockFilePath = GetPackagesLockFilePath(Path.GetDirectoryName(packagesConfigFile), nuGetLockFilePath, projectName);
            var lockFileExists = File.Exists(lockFilePath);
            var lockFileOptIn = MSBuildStringUtility.GetBooleanOrNull(restorePackagesWithLockFile);
            var useLockFile = lockFileOptIn == true || lockFileExists;

            if (lockFileOptIn == false && lockFileExists)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidLockFileInput, lockFilePath);
                var errors = new List<IRestoreLogMessage>();
                var log = RestoreLogMessage.CreateError(NuGetLogCode.NU1005, message, packagesConfigFile);
                log.ProjectPath = projectFile ?? packagesConfigFile;
                errors.Add(log);
                return errors;
            }

            if (useLockFile)
            {
                PackagesLockFile projectLockFileEquivalent = PackagesConfigLockFileUtility.FromPackagesConfigFile(packagesConfigFile,
                    projectTfm,
                    packagesFolderPath,
                    token);

                if (!lockFileExists)
                {
                    PackagesLockFileFormat.Write(lockFilePath, projectLockFileEquivalent);
                    return null;
                }
                else
                {
                    PackagesLockFile lockFile = PackagesLockFileFormat.Read(lockFilePath);
                    PackagesLockFileUtilities.LockFileValidityWithMatchedResults comparisonResult = PackagesLockFileUtilities.IsLockFileStillValid(projectLockFileEquivalent, lockFile);
                    if (comparisonResult.IsValid)
                    {
                        // check sha hashes
                        bool allContentHashesMatch = comparisonResult.MatchedDependencies.All(pair => pair.Key.ContentHash == pair.Value.ContentHash);
                        if (allContentHashesMatch)
                        {
                            return null;
                        }
                        else
                        {
                            var errors = new List<IRestoreLogMessage>();
                            foreach (var difference in comparisonResult.MatchedDependencies.Where(kvp => kvp.Key.ContentHash != kvp.Value.ContentHash))
                            {
                                var message = string.Format(CultureInfo.CurrentCulture, Strings.Error_PackageValidationFailed, difference.Key.Id + "." + difference.Key.ResolvedVersion);
                                var log = RestoreLogMessage.CreateError(NuGetLogCode.NU1403, message, packagesConfigFile);
                                log.ProjectPath = projectFile ?? packagesConfigFile;
                                errors.Add(log);
                            }
                            return errors;
                        }
                    }
                    else
                    {
                        if (restoreLockedMode)
                        {
                            var errors = new List<IRestoreLogMessage>();
                            var log = RestoreLogMessage.CreateError(NuGetLogCode.NU1004, Strings.Error_RestoreInLockedModePackagesConfig, packagesConfigFile);
                            log.ProjectPath = projectFile ?? packagesConfigFile;
                            errors.Add(log);
                            return errors;
                        }
                        else
                        {
                            PackagesLockFileFormat.Write(lockFilePath, projectLockFileEquivalent);
                            return null;
                        }
                    }
                }
            }
            else
            {
                return null;
            }
        }


        private static bool? IsRestorePackagesWithLockFileEnabled(MSBuildNuGetProject msbuildProject)
        {
            var msbuildProperty = msbuildProject?.ProjectSystem?.GetPropertyValue("RestorePackagesWithLockFile");
            if (msbuildProperty is string restorePackagesWithLockFileValue)
            {
                if (bool.TryParse(restorePackagesWithLockFileValue, out var useLockFile))
                {
                    return useLockFile;
                }
            }

            return null;
        }

        internal static PackagesLockFile GetLockFile(bool lockFileExists, string lockFileName)
        {
            PackagesLockFile lockFile;

            if (lockFileExists)
            {
                lockFile = PackagesLockFileFormat.Read(lockFileName);
                lockFile.Version = PackagesLockFileFormat.Version;
                if (lockFile.Targets.Count == 0)
                {
                    lockFile.Targets.Add(new PackagesLockFileTarget());
                }
                else if (lockFile.Targets.Count > 1)
                {
                    // merge lists
                    while (lockFile.Targets.Count > 1)
                    {
                        var target = lockFile.Targets[1];

                        for (var i = 0; i < target.Dependencies.Count; i++)
                        {
                            lockFile.Targets[0].Dependencies.Add(target.Dependencies[i]);
                        }

                        lockFile.Targets.RemoveAt(1);
                    }
                }
            }
            else
            {
                lockFile = new PackagesLockFile();
                lockFile.Targets.Add(new PackagesLockFileTarget());
            }

            return lockFile;
        }

        internal static void ApplyChanges(
            PackagesLockFile lockFile,
            List<NuGetProjectAction> actionsList,
            IPackagesConfigContentHashProvider contentHashUtil,
            CancellationToken token)
        {
            RemoveUninstalledPackages(lockFile,
                actionsList.Where(a => a.NuGetProjectActionType == NuGetProjectActionType.Uninstall));
            AddInstalledPackages(lockFile,
                actionsList.Where(a => a.NuGetProjectActionType == NuGetProjectActionType.Install),
                contentHashUtil,
                token);
            ArrayList.Adapter((IList)lockFile.Targets[0].Dependencies).Sort(_dependencyComparer);
        }

        private static void RemoveUninstalledPackages(PackagesLockFile lockFile, IEnumerable<NuGetProjectAction> actionsList)
        {
            var dependencies = lockFile.Targets[0].Dependencies;
            foreach (var toUninstall in actionsList)
            {
                Debug.Assert(toUninstall.NuGetProjectActionType == NuGetProjectActionType.Uninstall);

                var toUninstallId = toUninstall.PackageIdentity.Id;

                for (var i = 0; i < dependencies.Count; i++)
                {
                    if (string.Equals(toUninstallId, dependencies[i].Id, StringComparison.InvariantCultureIgnoreCase))
                    {
                        dependencies.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private static void AddInstalledPackages(
            PackagesLockFile lockFile,
            IEnumerable<NuGetProjectAction> actionsList,
            IPackagesConfigContentHashProvider contentHashUtil,
            CancellationToken token)
        {
            foreach (var toInstall in actionsList)
            {
                Debug.Assert(toInstall.NuGetProjectActionType == NuGetProjectActionType.Install);

                var newDependency = new LockFileDependency
                {
                    Id = toInstall.PackageIdentity.Id,
                    ContentHash = contentHashUtil.GetContentHash(toInstall.PackageIdentity, token),
                    RequestedVersion = new VersionRange(toInstall.PackageIdentity.Version, includeMinVersion: true, toInstall.PackageIdentity.Version, includeMaxVersion: true),
                    ResolvedVersion = toInstall.PackageIdentity.Version,
                    Type = PackageDependencyType.Direct
                };

                lockFile.Targets[0].Dependencies.Add(newDependency);
            }
        }

        public static PackagesLockFile FromPackagesConfigFile(
            string pcFile,
            NuGetFramework projectTfm,
            string packagesFolderPath,
            CancellationToken token)
        {
            if (pcFile == null)
            {
                throw new ArgumentNullException(nameof(pcFile));
            }
            if (!File.Exists(pcFile))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Strings.Error_FileDoesNotExist, pcFile), pcFile);
            }
            if (projectTfm == null)
            {
                throw new ArgumentNullException(nameof(projectTfm));
            }
            if (packagesFolderPath == null)
            {
                throw new ArgumentNullException(nameof(packagesFolderPath));
            }
            if (!Directory.Exists(packagesFolderPath))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.CurrentCulture, Strings.Error_DirectoryDoesNotExist, packagesFolderPath));
            }

            var lockFile = new PackagesLockFile();
            var target = new PackagesLockFileTarget();
            lockFile.Targets.Add(target);
            target.TargetFramework = projectTfm;

            using (var stream = File.OpenRead(pcFile))
            {
                var contentHashUtil = new PackagesConfigContentHashProvider(new FolderNuGetProject(packagesFolderPath));

                var reader = new PackagesConfigReader(stream);
                foreach (var package in reader.GetPackages(allowDuplicatePackageIds: true))
                {
                    var dependency = new LockFileDependency
                    {
                        Id = package.PackageIdentity.Id,
                        ContentHash = contentHashUtil.GetContentHash(package.PackageIdentity, token),
                        RequestedVersion = new VersionRange(package.PackageIdentity.Version, includeMinVersion: true, package.PackageIdentity.Version, includeMaxVersion: true),
                        ResolvedVersion = package.PackageIdentity.Version,
                        Type = PackageDependencyType.Direct
                    };

                    target.Dependencies.Add(dependency);
                }
            }

            return lockFile;
        }

        private class DependencyComparer : IComparer<LockFileDependency>, IComparer
        {
            public int Compare(LockFileDependency x, LockFileDependency y)
            {
                if (x == null && y == null) return 0;
                if (x == null || y == null) return x == null ? -1 : 1;
                return string.Compare(x.Id, y.Id, StringComparison.InvariantCultureIgnoreCase);
            }

            public int Compare(object x, object y)
            {
                return Compare(x as LockFileDependency, y as LockFileDependency);
            }
        }
    }
}
