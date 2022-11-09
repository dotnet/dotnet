// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.Threading;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent the view of packages installed to project(s)
    /// </summary>
    public class PowerShellInstalledPackage : PowerShellPackage
    {
        public string ProjectName { get; set; }

        /// <summary>
        /// Get the view of installed packages. Use for Get-Package command.
        /// </summary>
        internal static List<PowerShellInstalledPackage> GetPowerShellPackageView(
            Dictionary<NuGetProject, IEnumerable<PackageReference>> dictionary,
            ISettings settings)
        {
            var views = new List<PowerShellInstalledPackage>();

            foreach (var entry in dictionary)
            {
                var nugetProject = entry.Key;
                var projectName = entry.Key.GetMetadata<string>(NuGetProjectMetadataKeys.Name);

                FolderNuGetProject packageFolderProject = null;
                FallbackPackagePathResolver fallbackResolver = null;

                // Build a project-specific strategy for resolving a package .nupkg path.
                if (nugetProject is INuGetIntegratedProject) // This is technically incorrect for DNX projects,
                                                             // however since that experience is deprecated we don't
                                                             // care.
                {
                    var pathContext = NuGetPathContext.Create(settings);
                    fallbackResolver = new FallbackPackagePathResolver(pathContext);
                }
                else
                {
                    var project = nugetProject as MSBuildNuGetProject;

                    if (project != null)
                    {
                        packageFolderProject = project.FolderNuGetProject;
                    }
                }

                // entry.Value is an empty list if no packages are installed.
                foreach (var package in entry.Value)
                {
                    string installPackagePath = null;
                    string licenseUrl = null;

                    // Try to get the path to the package .nupkg on disk.
                    if (fallbackResolver != null)
                    {
                        var packageInfo = fallbackResolver.GetPackageInfo(
                            package.PackageIdentity.Id,
                            package.PackageIdentity.Version);

                        installPackagePath = packageInfo?.PathResolver.GetPackageFilePath(
                            package.PackageIdentity.Id,
                            package.PackageIdentity.Version);
                    }
                    else if (packageFolderProject != null)
                    {
                        installPackagePath = packageFolderProject.GetInstalledPackageFilePath(package.PackageIdentity);
                    }

                    // Try to read out the license URL.
                    using (var reader = GetPackageReader(installPackagePath))
                    using (var nuspecStream = reader?.GetNuspec())
                    {
                        if (nuspecStream != null)
                        {
                            var nuspecReader = new NuspecReader(nuspecStream);
                            licenseUrl = nuspecReader.GetLicenseUrl();
                        }
                    }

                    var view = new PowerShellInstalledPackage
                    {
                        Id = package.PackageIdentity.Id,
                        AsyncLazyVersions = new AsyncLazy<IEnumerable<NuGetVersion>>(
                            () => Task.FromResult<IEnumerable<NuGetVersion>>(new[] { package.PackageIdentity.Version }),
                            NuGetUIThreadHelper.JoinableTaskFactory),
                        ProjectName = projectName,
                        LicenseUrl = licenseUrl
                    };

                    views.Add(view);
                }
            }

            return views;
        }

        private static PackageReaderBase GetPackageReader(string installPath)
        {
            if (installPath == null)
            {
                return null;
            }

            var nupkg = new FileInfo(installPath);

            if (nupkg.Exists)
            {
                return new PackageArchiveReader(nupkg.OpenRead());
            }

            return null;
        }
    }
}
