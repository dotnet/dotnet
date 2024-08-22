// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Shared;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Helper class for calling the RestoreCommand
    /// </summary>
    public static class BuildIntegratedRestoreUtility
    {
        public static async Task ExecuteInitPs1ScriptsAsync(
            BuildIntegratedNuGetProject project,
            IEnumerable<PackageIdentity> packages,
            FallbackPackagePathResolver pathResolver,
            INuGetProjectContext projectContext,
            CancellationToken token)
        {
            // Find all dependencies in sorted order
            var sortedPackages = await BuildIntegratedProjectUtility.GetOrderedProjectPackageDependencies(project);

            // Keep track of the packages that need to be executed.
            var packagesToExecute = new HashSet<PackageIdentity>(packages, PackageIdentity.Comparer);

            // Use the ordered packages to run init.ps1 for the specified packages.
            foreach (var package in sortedPackages)
            {
                if (packagesToExecute.Remove(package))
                {
                    var packageInstallPath = pathResolver.GetPackageDirectory(package.Id, package.Version);

                    if (packageInstallPath == null)
                    {
                        continue;
                    }

                    await project
                        .ProjectServices
                        .ScriptService
                        .ExecutePackageInitScriptAsync(
                            package,
                            packageInstallPath,
                            projectContext,
                            throwOnFailure: false,
                            token: token);
                }
            }
        }

        /// <summary>
        /// Find all packages added to <paramref name="updatedLockFile"/>.
        /// </summary>
        public static IReadOnlyList<PackageIdentity> GetAddedPackages(
            LockFile originalLockFile,
            LockFile updatedLockFile)
        {
            IEnumerable<PackageIdentity> updatedPackages;
            if (updatedLockFile != null)
            {
                updatedPackages = updatedLockFile
                    .Targets
                    .SelectMany(target => target.Libraries)
                    .Where(library => library.Type == LibraryType.Package)
                    .Select(library => new PackageIdentity(library.Name, library.Version));
            }
            else
            {
                updatedPackages = Enumerable.Empty<PackageIdentity>();
            }

            IEnumerable<PackageIdentity> originalPackages;
            if (originalLockFile != null)
            {
                originalPackages = originalLockFile
                    .Targets
                    .SelectMany(target => target.Libraries)
                    .Where(library => library.Type == LibraryType.Package)
                    .Select(library => new PackageIdentity(library.Name, library.Version));
            }
            else
            {
                originalPackages = Enumerable.Empty<PackageIdentity>();
            }

            var results = updatedPackages
                .Except(originalPackages, PackageIdentity.Comparer)
                .ToList();

            return results;
        }

        /// <summary>
        /// Find the list of parent projects which directly or indirectly reference the child project.
        /// </summary>
        public static IReadOnlyList<BuildIntegratedNuGetProject> GetParentProjectsInClosure(
            IReadOnlyList<BuildIntegratedNuGetProject> projects,
            BuildIntegratedNuGetProject target,
            DependencyGraphSpec cache)
        {
            if (projects == null)
            {
                throw new ArgumentNullException(nameof(projects));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            var listOfParents = cache.GetParents(target.MSBuildProjectPath);

            var parentNuGetprojects = new HashSet<BuildIntegratedNuGetProject>();

            foreach (var parent in listOfParents)
            {
                // do not count the target as a parent
                var nugetProject = projects.FirstOrDefault(r => r.MSBuildProjectPath == parent);
                if (nugetProject != null && !nugetProject.Equals(target))
                {
                    parentNuGetprojects.Add(nugetProject);
                }
            }

            // sort parents by path to make this more deterministic during restores
            return parentNuGetprojects
                .OrderBy(parent => parent.MSBuildProjectPath, StringComparer.Ordinal)
                .ToList();
        }

        public static void UpdatePackageReferenceMetadata(
            LockFile lockFile,
            FallbackPackagePathResolver pathResolver,
            PackageIdentity package)
        {
            var info = pathResolver.GetPackageInfo(package.Id, package.Version);

            if (info == null)
            {
                // don't do anything if package was not resolved on disk
                return;
            }

            var nuspecFilePath = info.PathResolver.GetManifestFilePath(package.Id, package.Version);
            var nuspecReader = new NuspecReader(nuspecFilePath);
            var developmentDependency = nuspecReader.GetDevelopmentDependency();

            if (developmentDependency)
            {
                foreach (var frameworkInfo in lockFile.PackageSpec.TargetFrameworks
                    .OrderBy(framework => framework.FrameworkName.ToString(),
                        StringComparer.Ordinal))
                {
                    var dependency = frameworkInfo.Dependencies.First(dep => dep.Name.Equals(package.Id, StringComparison.OrdinalIgnoreCase));

                    if (dependency?.SuppressParent == LibraryIncludeFlagUtils.DefaultSuppressParent &&
                        dependency?.IncludeType == LibraryIncludeFlags.All)
                    {
                        var includeType = LibraryIncludeFlags.All & ~LibraryIncludeFlags.Compile;
                        dependency.SuppressParent = LibraryIncludeFlags.All;
                        dependency.IncludeType = includeType;

                        // update lock file target libraries
                        foreach (var target in lockFile.Targets
                            .Where(t => EqualityUtility.EqualsWithNullCheck(t.TargetFramework, frameworkInfo.FrameworkName)))
                        {
                            var targetLibrary = target.GetTargetLibrary(package.Id);

                            if (targetLibrary != null)
                            {
                                LockFileUtils.ExcludeItems(targetLibrary, includeType);
                            }
                        }
                    }
                }
            }
        }
    }
}
