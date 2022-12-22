// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using NuGet.Commands;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Utility class for showing error after project has been retargeted,
    /// if there are installed packages incompatible with the new target framework.
    /// </summary>
    public static class ProjectRetargetingUtility
    {
        /// <summary>
        ///  Get the list of packages to be reinstalled in the project. This can be run right after a project is retargeted or during every build
        /// </summary>
        /// <param name="project">NuGet project that the packages were installed to</param>
        /// <returns>List of package identities to be reinstalled</returns>
        public static async Task<IList<PackageIdentity>> GetPackagesToBeReinstalled(NuGetProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var sortedPackages = new List<PackageIdentity>();
            var installedRefs = await project.GetInstalledPackagesAsync(CancellationToken.None);

            if (installedRefs != null && installedRefs.Any())
            {
                var targetFramework = project.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
                return await GetPackagesToBeReinstalledAsync(targetFramework, installedRefs);
            }

            return new List<PackageIdentity>();
        }

        /// <summary>
        /// Get the list of packages to be reinstalled given the project framework and packageReferences
        /// </summary>
        /// <param name="projectFramework">Current target framework of the project</param>
        /// <param name="packageReferences">List of package references in the project from which packages to be reinstalled are determined</param>
        /// <returns>List of package identities to be reinstalled</returns>
        public static async Task<List<PackageIdentity>> GetPackagesToBeReinstalledAsync(NuGetFramework projectFramework, IEnumerable<Packaging.PackageReference> packageReferences)
        {
            Debug.Assert(projectFramework != null);
            Debug.Assert(packageReferences != null);

            var packagesToBeReinstalled = new List<PackageIdentity>();
            var sourceRepositoryProvider = await ServiceLocator.GetComponentModelServiceAsync<ISourceRepositoryProvider>();
            var solutionManager = await ServiceLocator.GetComponentModelServiceAsync<ISolutionManager>();
            var settings = await ServiceLocator.GetComponentModelServiceAsync<Configuration.ISettings>();
            var deleteOnRestartManager = await ServiceLocator.GetComponentModelServiceAsync<IDeleteOnRestartManager>();
            var restoreProgressReporter = await ServiceLocator.GetComponentModelServiceAsync<IRestoreProgressReporter>();
            var packageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                settings,
                solutionManager,
                deleteOnRestartManager,
                restoreProgressReporter);

            foreach (var packageReference in packageReferences)
            {
                var identity = packageReference.PackageIdentity;
                if (identity != null && ShouldPackageBeReinstalled(projectFramework, packageReference.TargetFramework, identity, packageManager))
                {
                    packagesToBeReinstalled.Add(identity);
                }
            }

            return packagesToBeReinstalled;
        }

        /// <summary>
        /// Determines if package needs to be reinstalled for the new target framework of the project
        /// </summary>
        /// <param name="newProjectFramework">current target framework of the project</param>
        /// <param name="oldProjectFramework">target framework of the project against which the package was installed</param>
        /// <param name="package">package for which reinstallation is being determined</param>
        /// <returns>whether the package identity needs to be reinstalled</returns>
        private static bool ShouldPackageBeReinstalled(NuGetFramework newProjectFramework, NuGetFramework oldProjectFramework, PackageIdentity package, NuGetPackageManager packageManager)
        {
            Debug.Assert(newProjectFramework != null);
            Debug.Assert(oldProjectFramework != null);
            Debug.Assert(package != null);

            // Default to true, i.e. package does not need to be reinstalled.
            bool isAllItemsCompatible = true;

            var packageFilePath = packageManager?.PackagesFolderNuGetProject?.GetInstalledPackageFilePath(package);

            if (string.IsNullOrEmpty(packageFilePath))
            {
                return false;
            }

            try
            {
                using (var packageReader = new PackageArchiveReader(packageFilePath))
                {
                    var libItemGroups = packageReader.GetLibItems();
                    var referenceItemGroups = packageReader.GetReferenceItems();
                    var frameworkReferenceGroups = packageReader.GetFrameworkItems();
                    var contentFileGroups = packageReader.GetContentItems();
                    var buildFileGroups = packageReader.GetBuildItems();
                    var toolItemGroups = packageReader.GetToolItems();

                    isAllItemsCompatible = IsNearestFrameworkSpecificGroupEqual(libItemGroups, newProjectFramework, oldProjectFramework)
                        && IsNearestFrameworkSpecificGroupEqual(referenceItemGroups, newProjectFramework, oldProjectFramework)
                        && IsNearestFrameworkSpecificGroupEqual(frameworkReferenceGroups, newProjectFramework, oldProjectFramework)
                        && IsNearestFrameworkSpecificGroupEqual(contentFileGroups, newProjectFramework, oldProjectFramework)
                        && IsNearestFrameworkSpecificGroupEqual(buildFileGroups, newProjectFramework, oldProjectFramework)
                        && IsNearestFrameworkSpecificGroupEqual(toolItemGroups, newProjectFramework, oldProjectFramework);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.WriteErrorToActivityLog(ex);
            }

            // When all items are not compatible, the installed package should be retargeted.
            return !isAllItemsCompatible;
        }

        /// <summary>
        /// Determines if the targetframework the package was installed against is compatible with the project's new target framework
        /// </summary>
        /// <param name="items">lib/ref/content/build items in the packages</param>
        /// <param name="newProjectFramework">The project's new target framework</param>
        /// <param name="oldProjectFramework">The target framework that the package was installed against</param>
        private static bool IsNearestFrameworkSpecificGroupEqual(IEnumerable<FrameworkSpecificGroup> items, NuGetFramework newProjectFramework, NuGetFramework oldProjectFramework)
        {
            if (items.Any())
            {
                var newNearestFramework = NuGetFrameworkUtility.GetNearest(items, newProjectFramework);
                var oldNearestFramework = NuGetFrameworkUtility.GetNearest(items, oldProjectFramework);

                if (newNearestFramework != null && oldNearestFramework != null)
                {
                    return newNearestFramework.Equals(oldNearestFramework);
                }

                if (newNearestFramework == null && oldNearestFramework == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // If there are no items, return true, i.e. does not need to retarget.
            return true;
        }

        /// <summary>
        /// Marks the packages to be reinstalled on the projects' packages.config
        /// </summary>
        public static async Task MarkPackagesForReinstallation(NuGetProject project, IList<PackageIdentity> packagesToBeReinstalled)
        {
            Debug.Assert(project != null);
            Debug.Assert(packagesToBeReinstalled != null);

            var installedPackageReferences = (await project.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
            var packageReferencesToUpdateReinstall = new Dictionary<Packaging.PackageReference, Packaging.PackageReference>();

            if (installedPackageReferences != null && installedPackageReferences.Any())
            {
                foreach (var packageReference in installedPackageReferences)
                {
                    bool markForReinstall = packagesToBeReinstalled.Any(p => p.Equals(packageReference.PackageIdentity));

                    // Determine if requireReinstallation attribute needs to be updated.
                    if (packageReference.RequireReinstallation ^ markForReinstall)
                    {
                        var newPackageReference = new Packaging.PackageReference(packageReference.PackageIdentity, packageReference.TargetFramework,
                            packageReference.IsUserInstalled, packageReference.IsDevelopmentDependency, markForReinstall);

                        packageReferencesToUpdateReinstall.Add(packageReference, newPackageReference);
                    }
                }

                var projectFullPath = project.GetMetadata<string>(NuGetProjectMetadataKeys.FullPath);
                var packagesConfigFullPath = Path.Combine(projectFullPath ?? string.Empty, ProjectManagement.Constants.PackageReferenceFile);

                // Create new file or overwrite existing file
                if (File.Exists(packagesConfigFullPath))
                {
                    try
                    {
                        using (var writer = new PackagesConfigWriter(packagesConfigFullPath, createNew: false))
                        {
                            foreach (var entry in packageReferencesToUpdateReinstall)
                            {
                                writer.UpdatePackageEntry(entry.Key, entry.Value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHelper.WriteErrorToActivityLog(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of package references that were marked for reinstallation in packages.config of the project
        /// </summary>
        public static IList<Packaging.PackageReference> GetPackageReferencesMarkedForReinstallation(NuGetProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var projectFullPath = project.GetMetadata<string>(NuGetProjectMetadataKeys.FullPath);
            var packagesConfigFullPath = Path.Combine(projectFullPath ?? string.Empty, NuGet.ProjectManagement.Constants.PackageReferenceFile);

            if (File.Exists(packagesConfigFullPath))
            {
                using (var stream = File.OpenRead(packagesConfigFullPath))
                {
                    var reader = new PackagesConfigReader(stream);
                    IEnumerable<Packaging.PackageReference> packageReferences = reader?.GetPackages();
                    return packageReferences.Where(p => p.RequireReinstallation).ToList();
                }
            }

            return new List<Packaging.PackageReference>();
        }

        /// <summary>
        /// True if the project is non-null, and does not contain a project.json file
        /// </summary>
        public static bool IsProjectRetargetable(NuGetProject project)
        {
            // Skip projects with project.json
            return project != null && !(project is INuGetIntegratedProject);
        }

        /// <summary>
        /// Determines if NuGet is used in the project. Currently, it is determined by checking if packages.config is part of the project
        /// </summary>
        /// <param name="project">The project which is checked to see if NuGet is used in it</param>
        public static async Task<bool> IsNuGetInUseAsync(Project project)
        {
            return await EnvDTEProjectUtility.IsSupportedAsync(project) && File.Exists(await project.GetPackagesConfigFullPathAsync());
        }
    }
}
