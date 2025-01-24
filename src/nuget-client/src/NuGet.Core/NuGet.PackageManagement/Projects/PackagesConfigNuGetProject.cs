// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Represents a NuGet project as represented by packages.config
    /// </summary>
    public class PackagesConfigNuGetProject : NuGetProject
    {
        private readonly object _configLock = new object();

        public string FullPath
        {
            get
            {
                if (UsingPackagesProjectNameConfigPath)
                {
                    return PackagesProjectNameConfigPath;
                }
                return PackagesConfigPath;
            }
        }

        private bool UsingPackagesProjectNameConfigPath { get; set; }

        /// <summary>
        /// Represents the full path to "packages.config"
        /// </summary>
        private string PackagesConfigPath { get; }

        /// <summary>
        /// Represents the full path to "packages.'projectName'.config"
        /// </summary>
        private string PackagesProjectNameConfigPath { get; }

        private NuGetFramework TargetFramework { get; }

        public PackagesConfigNuGetProject(string folderPath, Dictionary<string, object> metadata)
            : base(metadata)
        {
            if (folderPath == null)
            {
                throw new ArgumentNullException(nameof(folderPath));
            }

            // In ProjectFactory.cs, an instance of _project is initialized as a dynamic through
            // Activator.CreateInstance.  It has a Dictionary<string, object> which is later
            // provided to this constructor.  Occasionally, the value is a string, occasionally, it
            // is a NuGetFramework.  Not knowing why that's happening, we can protect against it here.
            // https://github.com/NuGet/Home/issues/4491

            var oFramework = GetMetadata<object>(NuGetProjectMetadataKeys.TargetFramework);
            TargetFramework = oFramework is string
                ? NuGetFramework.ParseFrameworkName((string)oFramework, DefaultFrameworkNameProvider.Instance)
                : (NuGetFramework)oFramework;

            PackagesConfigPath = Path.Combine(folderPath, "packages.config");

            var projectName = GetMetadata<string>(NuGetProjectMetadataKeys.Name);
            PackagesProjectNameConfigPath = Path.Combine(folderPath, "packages." + projectName + ".config");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override async Task<bool> InstallPackageAsync(
            PackageIdentity packageIdentity,
            DownloadResourceResult downloadResourceResult,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            var isDevelopmentDependency = await CheckDevelopmentDependencyAsync(downloadResourceResult, token);
            var newPackageReference = new PackageReference(
                packageIdentity,
                TargetFramework,
                userInstalled: true,
                developmentDependency: isDevelopmentDependency,
                requireReinstallation: false);
            var installedPackagesList = GetInstalledPackagesList();

            try
            {
                // Avoid modifying the packages.config file while it is being read
                // This can happen when VS API calls are made to get the list of
                // all installed packages.
                lock (_configLock)
                {
                    // Packages.config exist at full path
                    if (installedPackagesList.Any())
                    {
                        var packageReferenceWithSameId = installedPackagesList.FirstOrDefault(
                            p => p.PackageIdentity.Id.Equals(packageIdentity.Id, StringComparison.OrdinalIgnoreCase));

                        if (packageReferenceWithSameId != null)
                        {
                            if (packageReferenceWithSameId.PackageIdentity.Equals(packageIdentity))
                            {
                                nuGetProjectContext.Log(MessageLevel.Warning, Strings.PackageAlreadyExistsInPackagesConfig, packageIdentity, Path.GetFileName(FullPath));
                                return false;
                            }

                            // Higher version of an installed package is being installed. Remove old and add new
                            using (var writer = new PackagesConfigWriter(FullPath, createNew: false))
                            {
                                writer.UpdatePackageEntry(packageReferenceWithSameId, newPackageReference);
                            }
                        }
                        else
                        {
                            using (var writer = new PackagesConfigWriter(FullPath, createNew: false))
                            {

                                if (nuGetProjectContext.OriginalPackagesConfig == null)
                                {
                                    // Write a new entry
                                    writer.AddPackageEntry(newPackageReference);
                                }
                                else
                                {
                                    // Update the entry based on the original entry if it exists
                                    writer.UpdateOrAddPackageEntry(nuGetProjectContext.OriginalPackagesConfig, newPackageReference);
                                }
                            }
                        }
                    }
                    // Create new packages.config file and add the package entry
                    else
                    {
                        using (var writer = new PackagesConfigWriter(FullPath, createNew: true))
                        {
                            if (nuGetProjectContext.OriginalPackagesConfig == null)
                            {
                                // Write a new entry
                                writer.AddPackageEntry(newPackageReference);
                            }
                            else
                            {
                                // Update the entry based on the original entry if it exists
                                writer.UpdateOrAddPackageEntry(nuGetProjectContext.OriginalPackagesConfig, newPackageReference);
                            }
                        }
                    }
                }
            }
            catch (PackagesConfigWriterException ex)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ErrorWritingPackagesConfig,
                    FullPath,
                    ex.Message));
            }

            nuGetProjectContext.Log(MessageLevel.Info, Strings.AddedPackageToPackagesConfig, packageIdentity, Path.GetFileName(FullPath));
            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            var installedPackagesList = GetInstalledPackagesList();
            var packageReference = installedPackagesList.FirstOrDefault(p => p.PackageIdentity.Equals(packageIdentity));
            if (packageReference == null)
            {
                nuGetProjectContext.Log(MessageLevel.Warning, Strings.PackageDoesNotExisttInPackagesConfig, packageIdentity, Path.GetFileName(FullPath));
                return TaskResult.False;
            }

            try
            {
                if (installedPackagesList.Any())
                {
                    // Matching packageReference is found and is the only entry
                    // Then just delete the packages.config file 
                    if (installedPackagesList.Count == 1 && nuGetProjectContext.ActionType == NuGetActionType.Uninstall)
                    {
                        FileSystemUtility.DeleteFile(FullPath, nuGetProjectContext);
                    }
                    else
                    {
                        // Remove the package reference from packages.config file
                        using (var writer = new PackagesConfigWriter(FullPath, createNew: false))
                        {
                            writer.RemovePackageEntry(packageReference);
                        }
                    }
                }
            }
            catch (PackagesConfigWriterException ex)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ErrorWritingPackagesConfig,
                    FullPath,
                    ex.Message));
            }

            nuGetProjectContext.Log(MessageLevel.Info, Strings.RemovedPackageFromPackagesConfig, packageIdentity, Path.GetFileName(FullPath));
            return TaskResult.True;
        }

        public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
        {
            return Task.FromResult<IEnumerable<PackageReference>>(GetInstalledPackagesList());
        }

        /// <summary>
        /// Checks if there is a packages.config or packages.'projectName'.config file in the current project.
        /// </summary>
        /// <returns></returns>
        public bool PackagesConfigExists()
        {
            UpdateFullPath();
            return File.Exists(FullPath);
        }

        /// <summary>
        /// Retrieve the packages.config XML.
        /// This will return null if the file does not exist.
        /// </summary>
        public XDocument GetPackagesConfig()
        {
            UpdateFullPath();

            try
            {
                var share = FileShare.ReadWrite | FileShare.Delete;

                // Try to read the config file up to 3 times
                for (var i = 0; i < FileUtility.MaxTries; i++)
                {
                    try
                    {
                        // Avoid opening packages.config while an update or install is moving the file
                        lock (_configLock)
                        {
                            if (File.Exists(FullPath))
                            {
                                using (var stream = new FileStream(FullPath, FileMode.Open, FileAccess.Read, share))
                                {
                                    return XDocument.Load(stream);
                                }
                            }
                            else
                            {
                                // File does not exist
                                break;
                            }
                        }
                    }
                    catch (Exception ex) when ((i < (FileUtility.MaxTries - 1))
                        && (ex is IOException || ex is UnauthorizedAccessException))
                    {
                        // Retry incase the file temporarily does not exist due to an install or update
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex) when
                (ex is IOException || ex is UnauthorizedAccessException || ex is XmlException)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ErrorLoadingPackagesConfig,
                    FullPath,
                    ex.Message));
            }

            return null;
        }

        private void UpdateFullPath()
        {
            if (UsingPackagesProjectNameConfigPath
                && !File.Exists(PackagesProjectNameConfigPath)
                && File.Exists(PackagesConfigPath))
            {
                UsingPackagesProjectNameConfigPath = false;
            }
            else if (!File.Exists(PackagesConfigPath)
                     && File.Exists(PackagesProjectNameConfigPath))
            {
                UsingPackagesProjectNameConfigPath = true;
            }
        }

        private List<PackageReference> GetInstalledPackagesList()
        {
            try
            {
                var xml = GetPackagesConfig();

                if (xml != null)
                {
                    var reader = new PackagesConfigReader(xml);
                    return reader.GetPackages().ToList();
                }
            }
            catch (Exception ex)
                when (ex is XmlException ||
                        ex is PackagesConfigReaderException)
            {
                throw new InvalidOperationException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.ErrorLoadingPackagesConfig,
                        FullPath,
                        ex.Message));
            }

            return new List<PackageReference>();
        }

        private static async Task<bool> CheckDevelopmentDependencyAsync(
            DownloadResourceResult downloadResourceResult,
            CancellationToken token)
        {
            var isDevelopmentDependency = false;

            // Catch any exceptions while fetching DevelopmentDependency element from nuspec file.
            // So it can continue to write the packages.config file.
            try
            {
                if (downloadResourceResult.PackageReader != null)
                {
                    isDevelopmentDependency = await downloadResourceResult.PackageReader.GetDevelopmentDependencyAsync(token);
                }
                else
                {
                    using (var packageReader = new PackageArchiveReader(
                        downloadResourceResult.PackageStream,
                        leaveStreamOpen: true))
                    {
                        var nuspecReader = new NuspecReader(packageReader.GetNuspec());
                        isDevelopmentDependency = nuspecReader.GetDevelopmentDependency();
                    }
                }
            }
            catch { }

            return isDevelopmentDependency;
        }
    }
}
