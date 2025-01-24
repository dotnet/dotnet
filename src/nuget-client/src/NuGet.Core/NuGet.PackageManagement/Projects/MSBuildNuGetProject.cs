// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// This class represents a NuGetProject based on a .NET project. This also contains an instance of a
    /// FolderNuGetProject
    /// </summary>
    [DebuggerDisplay("{ProjectSystem.ProjectName} [{ProjectStyle}]")]
    public class MSBuildNuGetProject
        : NuGetProject
        , IDependencyGraphProject
    {
        private readonly IDictionary<FileTransformExtensions, IPackageFileTransformer> FileTransformers =
            new Dictionary<FileTransformExtensions, IPackageFileTransformer>
                {
                    { new FileTransformExtensions(".transform", ".transform"), new XmlTransformer(GetConfigMappings()) },
                    { new FileTransformExtensions(".pp", ".pp"), new Preprocessor() },
                    { new FileTransformExtensions(".install.xdt", ".uninstall.xdt"), new XdtTransformer() }
                };

        #region Events

        /// <summary>
        /// Event to be raised while installing a package
        /// </summary>
        public event EventHandler<PackageEventArgs> PackageInstalling;

        /// <summary>
        /// Event to be raised while installing a package
        /// </summary>
        public event EventHandler<PackageEventArgs> PackageInstalled;

        /// <summary>
        /// Event to be raised while installing a package
        /// </summary>
        public event EventHandler<PackageEventArgs> PackageUninstalling;

        /// <summary>
        /// Event to be raised while installing a package
        /// </summary>
        public event EventHandler<PackageEventArgs> PackageUninstalled;

        /// <summary>
        /// Event to be raised while added references to project
        /// </summary>
        public event EventHandler<PackageEventArgs> PackageReferenceAdded;

        /// <summary>
        /// Event to be raised while removed references from project
        /// </summary>
        public event EventHandler<PackageEventArgs> PackageReferenceRemoved;

        #endregion Events

        #region Properties

        public IMSBuildProjectSystem ProjectSystem { get; }

        public FolderNuGetProject FolderNuGetProject { get; }

        public PackagesConfigNuGetProject PackagesConfigNuGetProject { get; }

        public string MSBuildProjectPath => ProjectSystem.ProjectFileFullPath;

        #endregion Properties

        public MSBuildNuGetProject(
            IMSBuildProjectSystem msbuildNuGetProjectSystem,
            string folderNuGetProjectPath,
            string packagesConfigFolderPath)
        {
            if (folderNuGetProjectPath == null)
            {
                throw new ArgumentNullException(nameof(folderNuGetProjectPath));
            }

            if (packagesConfigFolderPath == null)
            {
                throw new ArgumentNullException(nameof(packagesConfigFolderPath));
            }

            ProjectStyle = ProjectStyle.PackagesConfig;

            ProjectSystem = msbuildNuGetProjectSystem ?? throw new ArgumentNullException(nameof(msbuildNuGetProjectSystem));
            FolderNuGetProject = new FolderNuGetProject(folderNuGetProjectPath);
            InternalMetadata.Add(NuGetProjectMetadataKeys.Name, ProjectSystem.ProjectName);
            InternalMetadata.Add(NuGetProjectMetadataKeys.TargetFramework, ProjectSystem.TargetFramework);
            InternalMetadata.Add(NuGetProjectMetadataKeys.FullPath, msbuildNuGetProjectSystem.ProjectFullPath);
            InternalMetadata.Add(NuGetProjectMetadataKeys.UniqueName, msbuildNuGetProjectSystem.ProjectUniqueName);
            PackagesConfigNuGetProject = new PackagesConfigNuGetProject(packagesConfigFolderPath, InternalMetadata);
        }

        public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
        {
            return PackagesConfigNuGetProject.GetInstalledPackagesAsync(token);
        }

        public bool DoesPackagesConfigExists()
        {
            return PackagesConfigNuGetProject.PackagesConfigExists();
        }

        public void AddBindingRedirects()
        {
            ProjectSystem.AddBindingRedirects();
        }

        private static bool IsBindingRedirectsDisabled(INuGetProjectContext nuGetProjectContext)
        {
            var msBuildNuGetProjectContext = nuGetProjectContext as IMSBuildNuGetProjectContext;
            return msBuildNuGetProjectContext != null && msBuildNuGetProjectContext.BindingRedirectsDisabled;
        }

        private static bool IsSkipAssemblyReferences(INuGetProjectContext nuGetProjectContext)
        {
            var msBuildNuGetProjectContext = nuGetProjectContext as IMSBuildNuGetProjectContext;
            return msBuildNuGetProjectContext != null && msBuildNuGetProjectContext.SkipAssemblyReferences;
        }

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

            if (downloadResourceResult == null)
            {
                throw new ArgumentNullException(nameof(downloadResourceResult));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            if (downloadResourceResult.Status != DownloadResourceResultStatus.AvailableWithoutStream &&
                !downloadResourceResult.PackageStream.CanSeek)
            {
                throw new ArgumentException(Strings.PackageStreamShouldBeSeekable);
            }

            // Step-1: Check if the package already exists after setting the nuGetProjectContext
            ProjectSystem.NuGetProjectContext = nuGetProjectContext;

            var packageReference = (await GetInstalledPackagesAsync(token))
                .FirstOrDefault(p => p.PackageIdentity.Equals(packageIdentity));
            if (packageReference != null)
            {
                nuGetProjectContext.Log(MessageLevel.Warning, Strings.PackageAlreadyExistsInProject,
                    packageIdentity, ProjectSystem.ProjectName);
                return false;
            }

            // Step-2: Create PackageArchiveReader using the PackageStream and obtain the various item groups
            if (downloadResourceResult.Status != DownloadResourceResultStatus.AvailableWithoutStream)
            {
                downloadResourceResult.PackageStream.Seek(0, SeekOrigin.Begin);
            }

            // These casts enforce use of -Async(...) methods.
            var packageReader = downloadResourceResult.PackageReader
                ?? new PackageArchiveReader(downloadResourceResult.PackageStream, leaveStreamOpen: true);
            IAsyncPackageContentReader packageContentReader = packageReader;
            IAsyncPackageCoreReader packageCoreReader = packageReader;

            var libItemGroups = await packageContentReader.GetLibItemsAsync(token);
            var referenceItemGroups = await packageContentReader.GetReferenceItemsAsync(token);
            var frameworkReferenceGroups = await packageContentReader.GetFrameworkItemsAsync(token);
            var contentFileGroups = await packageContentReader.GetContentItemsAsync(token);
            var buildFileGroups = await packageContentReader.GetBuildItemsAsync(token);
            var toolItemGroups = await packageContentReader.GetToolItemsAsync(token);

            // Step-3: Get the most compatible items groups for all items groups
            var hasCompatibleProjectLevelContent = false;

            var compatibleLibItemsGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(ProjectSystem.TargetFramework, libItemGroups);
            var compatibleReferenceItemsGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(ProjectSystem.TargetFramework, referenceItemGroups);
            var compatibleFrameworkReferencesGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(ProjectSystem.TargetFramework, frameworkReferenceGroups);
            var compatibleContentFilesGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(ProjectSystem.TargetFramework, contentFileGroups);
            var compatibleBuildFilesGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(ProjectSystem.TargetFramework, buildFileGroups);
            var compatibleToolItemsGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(ProjectSystem.TargetFramework, toolItemGroups);

            compatibleLibItemsGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleLibItemsGroup);
            compatibleReferenceItemsGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleReferenceItemsGroup);
            compatibleFrameworkReferencesGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleFrameworkReferencesGroup);
            compatibleContentFilesGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleContentFilesGroup);
            compatibleBuildFilesGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleBuildFilesGroup);
            compatibleToolItemsGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleToolItemsGroup);

            hasCompatibleProjectLevelContent = MSBuildNuGetProjectSystemUtility.IsValid(compatibleLibItemsGroup) ||
                                               MSBuildNuGetProjectSystemUtility.IsValid(compatibleFrameworkReferencesGroup) ||
                                               MSBuildNuGetProjectSystemUtility.IsValid(compatibleContentFilesGroup) ||
                                               MSBuildNuGetProjectSystemUtility.IsValid(compatibleBuildFilesGroup);

            // Check if package has any content for project
            var hasProjectLevelContent = libItemGroups.Any() || frameworkReferenceGroups.Any()
                                         || contentFileGroups.Any() || buildFileGroups.Any();
            var onlyHasCompatibleTools = false;
            var onlyHasDependencies = false;

            if (!hasProjectLevelContent)
            {
                // Since it does not have project-level content, check if it has dependencies or compatible tools
                // Note that we are not checking if it has compatible project level content, but, just that it has project level content
                // If the package has project-level content, but nothing compatible, we still need to throw
                // If a package does not have any project-level content, it can be a
                // Legacy solution level packages which only has compatible tools group
                onlyHasCompatibleTools = MSBuildNuGetProjectSystemUtility.IsValid(compatibleToolItemsGroup) && compatibleToolItemsGroup.Items.Any();
                if (!onlyHasCompatibleTools)
                {
                    // If it does not have compatible tool items either, check if it at least has dependencies
                    onlyHasDependencies = (await packageContentReader.GetPackageDependenciesAsync(token)).Any();
                }
            }
            else
            {
                var shortFramework = ProjectSystem.TargetFramework.GetShortFolderName();
                nuGetProjectContext.Log(MessageLevel.Debug, Strings.Debug_TargetFrameworkInfoPrefix, packageIdentity,
                    GetMetadata<string>(NuGetProjectMetadataKeys.Name), shortFramework);
            }

            // Step-4: Check if there are any compatible items in the package or that this is not a package with only tools group. If not, throw
            if (!hasCompatibleProjectLevelContent
                && !onlyHasCompatibleTools
                && !onlyHasDependencies)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                        Strings.UnableToFindCompatibleItems, packageIdentity.Id + " " + packageIdentity.Version.ToNormalizedString(), ProjectSystem.TargetFramework));
            }

            if (hasCompatibleProjectLevelContent)
            {
                var shortFramework = ProjectSystem.TargetFramework.GetShortFolderName();
                nuGetProjectContext.Log(MessageLevel.Debug, Strings.Debug_TargetFrameworkInfoPrefix, packageIdentity,
                    GetMetadata<string>(NuGetProjectMetadataKeys.Name), shortFramework);
            }
            else if (onlyHasCompatibleTools)
            {
                nuGetProjectContext.Log(MessageLevel.Info, Strings.AddingPackageWithOnlyToolsGroup, packageIdentity,
                    GetMetadata<string>(NuGetProjectMetadataKeys.Name));
            }
            else if (onlyHasDependencies)
            {
                nuGetProjectContext.Log(MessageLevel.Info, Strings.AddingPackageWithOnlyDependencies, packageIdentity,
                    GetMetadata<string>(NuGetProjectMetadataKeys.Name));
            }

            // Step-5: Raise PackageInstalling event
            // At this point, GetInstalledPath is pointless since the package is, likely, not already installed. It will be empty
            // Using PackagePathResolver.GetInstallPath would be wrong, since, package version from the nuspec is always used
            var packageEventArgs = new PackageEventArgs(FolderNuGetProject, packageIdentity, installPath: string.Empty);
            if (PackageInstalling != null)
            {
                PackageInstalling(this, packageEventArgs);
            }
            PackageEventsProvider.Instance.NotifyInstalling(packageEventArgs);

            // Step-6: Install package to FolderNuGetProject
            await FolderNuGetProject.InstallPackageAsync(packageIdentity, downloadResourceResult, nuGetProjectContext, token);

            // Step-7: Raise PackageInstalled event
            // Call GetInstalledPath to get the package installed path
            var packageInstallPath = FolderNuGetProject.GetInstalledPath(packageIdentity);
            packageEventArgs = new PackageEventArgs(FolderNuGetProject, packageIdentity, packageInstallPath);
            if (PackageInstalled != null)
            {
                PackageInstalled(this, packageEventArgs);
            }
            PackageEventsProvider.Instance.NotifyInstalled(packageEventArgs);

            // Step-8: MSBuildNuGetProjectSystem operations
            // Step-8.1: Add references to project
            if (!IsSkipAssemblyReferences(nuGetProjectContext) &&
                MSBuildNuGetProjectSystemUtility.IsValid(compatibleReferenceItemsGroup))
            {
                foreach (var referenceItem in compatibleReferenceItemsGroup.Items)
                {
                    if (IsAssemblyReference(referenceItem))
                    {
                        var referenceItemFullPath = Path.Combine(packageInstallPath, referenceItem);
                        var referenceName = Path.GetFileName(referenceItem);

                        if (await ProjectSystem.ReferenceExistsAsync(referenceName))
                        {
                            await ProjectSystem.RemoveReferenceAsync(referenceName);
                        }

                        await ProjectSystem.AddReferenceAsync(referenceItemFullPath);
                    }
                }
            }

            // Step-8.2: Add Frameworkreferences to project
            if (!IsSkipAssemblyReferences(nuGetProjectContext) &&
                MSBuildNuGetProjectSystemUtility.IsValid(compatibleFrameworkReferencesGroup))
            {
                foreach (var frameworkReference in compatibleFrameworkReferencesGroup.Items)
                {
                    if (!await ProjectSystem.ReferenceExistsAsync(frameworkReference))
                    {
                        await ProjectSystem.AddFrameworkReferenceAsync(frameworkReference, packageIdentity.Id);
                    }
                }
            }

            // Step-8.3: Add Content Files
            if (MSBuildNuGetProjectSystemUtility.IsValid(compatibleContentFilesGroup))
            {
                await MSBuildNuGetProjectSystemUtility.AddFilesAsync(
                    ProjectSystem,
                    packageCoreReader,
                    compatibleContentFilesGroup,
                    FileTransformers,
                    token);
            }

            // Step-8.4: Add Build imports
            if (MSBuildNuGetProjectSystemUtility.IsValid(compatibleBuildFilesGroup))
            {
                foreach (var buildImportFile in compatibleBuildFilesGroup.Items)
                {
                    var fullImportFilePath = Path.Combine(packageInstallPath, buildImportFile);
                    ProjectSystem.AddImport(fullImportFilePath,
                        fullImportFilePath.EndsWith(".props", StringComparison.OrdinalIgnoreCase) ? ImportLocation.Top : ImportLocation.Bottom);
                }
            }

            // Step-9: Install package to PackagesConfigNuGetProject
            await PackagesConfigNuGetProject.InstallPackageAsync(packageIdentity, downloadResourceResult, nuGetProjectContext, token);

            // Step-10: Add packages.config to MSBuildNuGetProject
            ProjectSystem.AddExistingFile(Path.GetFileName(PackagesConfigNuGetProject.FullPath));

            // Step 11: Raise PackageReferenceAdded event
            PackageReferenceAdded?.Invoke(this, packageEventArgs);
            PackageEventsProvider.Instance.NotifyReferenceAdded(packageEventArgs);

            // Step-12: Execute powershell script - install.ps1
            var anyFrameworkToolsGroup = toolItemGroups.FirstOrDefault(g => g.TargetFramework.Equals(NuGetFramework.AnyFramework));
            if (anyFrameworkToolsGroup != null)
            {
                var initPS1RelativePath = anyFrameworkToolsGroup.Items
                    .FirstOrDefault(p =>
                        p.StartsWith(PowerShellScripts.InitPS1RelativePath, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(initPS1RelativePath))
                {
                    initPS1RelativePath = PathUtility.ReplaceAltDirSeparatorWithDirSeparator(
                        initPS1RelativePath);
                    await ProjectServices.ScriptService.ExecutePackageScriptAsync(
                        packageIdentity,
                        packageInstallPath,
                        initPS1RelativePath,
                        nuGetProjectContext,
                        throwOnFailure: true,
                        token: token);
                }
            }

            if (MSBuildNuGetProjectSystemUtility.IsValid(compatibleToolItemsGroup))
            {
                var installPS1RelativePath = compatibleToolItemsGroup.Items.FirstOrDefault(
                    p => p.EndsWith(Path.DirectorySeparatorChar + PowerShellScripts.Install, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(installPS1RelativePath))
                {
                    await ProjectServices.ScriptService.ExecutePackageScriptAsync(
                        packageIdentity,
                        packageInstallPath,
                        installPS1RelativePath,
                        nuGetProjectContext,
                        throwOnFailure: true,
                        token: token);
                }
            }
            return true;
        }

        public override async Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            // Step-1: Check if the package already exists after setting the nuGetProjectContext
            ProjectSystem.NuGetProjectContext = nuGetProjectContext;

            var packageReference = (await GetInstalledPackagesAsync(token))
                .FirstOrDefault(p => p.PackageIdentity.Equals(packageIdentity));
            if (packageReference == null)
            {
                nuGetProjectContext.Log(MessageLevel.Warning, Strings.PackageDoesNotExistInProject,
                    packageIdentity, ProjectSystem.ProjectName);
                return false;
            }

            var packageTargetFramework = packageReference.TargetFramework ?? ProjectSystem.TargetFramework;
            var packageEventArgs = new PackageEventArgs(FolderNuGetProject,
                                            packageIdentity,
                                            FolderNuGetProject.GetInstalledPath(packageIdentity));

            if (PackageUninstalling != null)
            {
                PackageUninstalling(this, packageEventArgs);
            }

            PackageEventsProvider.Instance.NotifyUninstalling(packageEventArgs);

            var packagePath = FolderNuGetProject.GetInstalledPackageFilePath(packageIdentity);

            if (string.IsNullOrEmpty(packagePath))
            {
                nuGetProjectContext.Log(MessageLevel.Warning, Strings.PackageDoesNotExistInFolder,
                    packageIdentity, ProjectSystem.ProjectName);
                return false;
            }

            using var packageStream = File.OpenRead(packagePath);
            using var zipArchive = new ZipArchive(packageStream);
            using var packageReader = new PackageArchiveReader(zipArchive);

            // Step-2: Execute powershell script - uninstall.ps1
            var toolItemGroups = packageReader.GetToolItems();
            var compatibleToolItemsGroup = MSBuildNuGetProjectSystemUtility
                .GetMostCompatibleGroup(packageTargetFramework, toolItemGroups);
            compatibleToolItemsGroup = MSBuildNuGetProjectSystemUtility.Normalize(compatibleToolItemsGroup);

            if (MSBuildNuGetProjectSystemUtility.IsValid(compatibleToolItemsGroup))
            {
                var uninstallPS1RelativePath = compatibleToolItemsGroup.Items.FirstOrDefault(
                    p => p.EndsWith(Path.DirectorySeparatorChar + PowerShellScripts.Uninstall,
                                    StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(uninstallPS1RelativePath))
                {
                    var packageInstallPath = FolderNuGetProject.GetInstalledPath(
                        packageIdentity);
                    await ProjectServices.ScriptService.ExecutePackageScriptAsync(
                        packageIdentity,
                        packageInstallPath,
                        uninstallPS1RelativePath,
                        nuGetProjectContext,
                        throwOnFailure: false,
                        token: token);
                }
            }

            // Step-3: Obtain the various item groups
            // Get the package target framework instead of using project targetframework
            var referenceItemGroups = packageReader.GetReferenceItems();
            var contentFileGroups = packageReader.GetContentItems();
            var buildFileGroups = packageReader.GetBuildItems();

            // Step-4: Get the most compatible items groups for all items groups
            var compatibleReferenceItemsGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(packageTargetFramework, referenceItemGroups);

            var compatibleContentFilesGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(packageTargetFramework, contentFileGroups);

            var compatibleBuildFilesGroup =
                MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(packageTargetFramework, buildFileGroups);

            compatibleReferenceItemsGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleReferenceItemsGroup);
            compatibleContentFilesGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleContentFilesGroup);
            compatibleBuildFilesGroup
                = MSBuildNuGetProjectSystemUtility.Normalize(compatibleBuildFilesGroup);

            // Step-5: Remove package reference from packages.config
            await PackagesConfigNuGetProject.UninstallPackageAsync(packageIdentity, nuGetProjectContext, token);

            // Step-6: Remove packages.config from MSBuildNuGetProject if there are no packages
            //         OR Add it again (to ensure that Source Control works), when there are some packages
            if (!(await PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).Any())
            {
                ProjectSystem.RemoveFile(Path.GetFileName(PackagesConfigNuGetProject.FullPath));
            }
            else
            {
                ProjectSystem.AddExistingFile(Path.GetFileName(PackagesConfigNuGetProject.FullPath));
            }

            // Step-7: Uninstall package from the msbuild project
            // Step-7.1: Remove references
            if (MSBuildNuGetProjectSystemUtility.IsValid(compatibleReferenceItemsGroup))
            {
                foreach (var item in compatibleReferenceItemsGroup.Items)
                {
                    if (IsAssemblyReference(item))
                    {
                        await ProjectSystem.RemoveReferenceAsync(Path.GetFileName(item));
                    }
                }
            }

            // Step-7.2: Framework references are never removed. This is a no-op

            // Step-7.3: Remove content files
            if (MSBuildNuGetProjectSystemUtility.IsValid(compatibleContentFilesGroup))
            {
                var packagesPaths = (await GetInstalledPackagesAsync(token))
                    .Select(pr => FolderNuGetProject.GetInstalledPackageFilePath(pr.PackageIdentity))
                    .Where(path => !string.IsNullOrEmpty(path));

                await MSBuildNuGetProjectSystemUtility.DeleteFilesAsync(
                    ProjectSystem,
                    zipArchive,
                    packagesPaths,
                    compatibleContentFilesGroup,
                    FileTransformers,
                    token);
            }

            // Step-7.4: Remove build imports
            if (MSBuildNuGetProjectSystemUtility.IsValid(compatibleBuildFilesGroup))
            {
                foreach (var buildImportFile in compatibleBuildFilesGroup.Items)
                {
                    var fullImportFilePath = Path.Combine(FolderNuGetProject.GetInstalledPath(packageIdentity), buildImportFile);
                    ProjectSystem.RemoveImport(fullImportFilePath);
                }
            }


            // Step-7.5: Remove binding redirects. This is a no-op
            // Binding redirects will be removed when all packages have finished
            // uninstalling for performance reasons

            // Step-8: Raise PackageReferenceRemoved event
            if (PackageReferenceRemoved != null)
            {
                PackageReferenceRemoved(this, packageEventArgs);
            }

            PackageEventsProvider.Instance.NotifyReferenceRemoved(packageEventArgs);


            // Step-9: Uninstall package from the folderNuGetProject
            await FolderNuGetProject.UninstallPackageAsync(packageIdentity, nuGetProjectContext, token);

            // Step-10: Raise PackageUninstalled event
            if (PackageUninstalled != null)
            {
                PackageUninstalled(this, packageEventArgs);
            }

            PackageEventsProvider.Instance.NotifyUninstalled(packageEventArgs);

            return true;
        }

        public override Task PostProcessAsync(INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            if (!IsBindingRedirectsDisabled(nuGetProjectContext))
            {
                ProjectSystem.AddBindingRedirects();
            }
            return base.PostProcessAsync(nuGetProjectContext, token);
        }

        private static bool IsAssemblyReference(string filePath)
        {
            // assembly reference must be under lib/
            if (!filePath.StartsWith(PackagingConstants.Folders.Lib + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                && !filePath.StartsWith(PackagingConstants.Folders.Lib + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var fileName = Path.GetFileName(filePath);

            // if it's an empty folder, yes
            if (fileName == PackagingCoreConstants.EmptyFolder)
            {
                return true;
            }

            // Assembly reference must have a .dll|.exe|.winmd extension and is not a resource assembly;
            return !filePath.EndsWith(Constants.ResourceAssemblyExtension, StringComparison.OrdinalIgnoreCase) &&
                   Constants.AssemblyReferencesExtensions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase);
        }

        private static IDictionary<XName, Action<XElement, XElement>> GetConfigMappings()
        {
            // REVIEW: This might be an edge case, but we're setting this rule for all xml files.
            // If someone happens to do a transform where the xml file has a configSections node
            // we will add it first. This is probably fine, but this is a config specific scenario
            return new Dictionary<XName, Action<XElement, XElement>>
                {
                    { "configSections", (parent, element) => parent.AddFirst(element) }
                };
        }

        public async Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
        {
            var (dgSpec, _) = await GetPackageSpecsAndAdditionalMessagesAsync(context);
            return dgSpec;
        }

        public async Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Some projects like website project don't have project file.
            // Return empty list for this case.
            if (string.IsNullOrEmpty(ProjectSystem.ProjectFileFullPath))
            {
                return (new List<PackageSpec>(), null);
            }

            PackageSpec packageSpec = null;
            if (!context.PackageSpecCache.TryGetValue(ProjectSystem.ProjectFileFullPath, out packageSpec))
            {
                packageSpec = new PackageSpec(new List<TargetFrameworkInformation>
                {
                    new TargetFrameworkInformation()
                    {
                        FrameworkName = ProjectSystem.TargetFramework
                    }
                });
                packageSpec.Name = ProjectSystem.ProjectName;
                packageSpec.FilePath = ProjectSystem.ProjectFileFullPath;

                // A packages.config project does not follow the typical restore flow so there is no need to add package
                // dependencides to the package spec. Packages.config package restoration is done elsewhere.

                var metadata = new ProjectRestoreMetadata();
                packageSpec.RestoreMetadata = metadata;

                metadata.ProjectStyle = ProjectStyle.Unknown;
                metadata.ProjectPath = ProjectSystem.ProjectFileFullPath;
                metadata.ProjectName = ProjectSystem.ProjectName;
                metadata.ProjectUniqueName = ProjectSystem.ProjectFileFullPath;

                // Add framework group
                var frameworkGroup = new ProjectRestoreMetadataFrameworkInfo(ProjectSystem.TargetFramework);
                metadata.TargetFrameworks.Add(frameworkGroup);

                var references = (await ProjectServices
                    .ReferencesReader
                    .GetProjectReferencesAsync(context.Logger, CancellationToken.None))
                    .ToList();
                if (references != null && references.Count > 0)
                {
                    foreach (var reference in references)
                    {
                        // This reference applies to all frameworks
                        // Include/exclude flags are not possible for this project type
                        frameworkGroup.ProjectReferences.Add(reference);
                    }
                }

                context.PackageSpecCache.Add(MSBuildProjectPath, packageSpec);
            }

            return (new[] { packageSpec }, null);
        }
    }
}
