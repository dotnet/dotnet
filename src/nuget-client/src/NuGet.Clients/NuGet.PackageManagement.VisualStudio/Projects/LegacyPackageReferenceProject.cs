// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio.Utility;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.RuntimeModel;
using NuGet.Shared;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// An implementation of <see cref="NuGetProject"/> that interfaces with VS project APIs to coordinate
    /// packages in a legacy CSProj with package references.
    /// </summary>
    public sealed class LegacyPackageReferenceProject : PackageReferenceProject<Dictionary<string, ProjectInstalledPackage>, KeyValuePair<string, ProjectInstalledPackage>>
    {
        private readonly IVsProjectAdapter _vsProjectAdapter;
        private readonly IVsProjectThreadingService _threadingService;

        public NuGetFramework TargetFramework { get; }

        public LegacyPackageReferenceProject(
            IVsProjectAdapter vsProjectAdapter,
            string projectId,
            INuGetProjectServices projectServices,
            IVsProjectThreadingService threadingService)
            : base(vsProjectAdapter.ProjectName,
                vsProjectAdapter.UniqueName,
                vsProjectAdapter.FullProjectPath)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.NotNullOrEmpty(projectId);
            Assumes.Present(projectServices);
            Assumes.Present(threadingService);

            _vsProjectAdapter = vsProjectAdapter;
            _threadingService = threadingService;

            ProjectStyle = ProjectStyle.PackageReference;

            InternalMetadata.Add(NuGetProjectMetadataKeys.Name, ProjectName);
            InternalMetadata.Add(NuGetProjectMetadataKeys.UniqueName, ProjectUniqueName);
            InternalMetadata.Add(NuGetProjectMetadataKeys.FullPath, ProjectFullPath);
            InternalMetadata.Add(NuGetProjectMetadataKeys.ProjectId, projectId);

            ProjectServices = projectServices;
        }

        public LegacyPackageReferenceProject(
            IVsProjectAdapter vsProjectAdapter,
            string projectId,
            INuGetProjectServices projectServices,
            IVsProjectThreadingService threadingService,
            NuGetFramework targetFramework)
            : this(vsProjectAdapter,
                projectId,
                projectServices,
                threadingService)
        {
            Assumes.NotNull(targetFramework);
            TargetFramework = targetFramework;
        }

        #region BuildIntegratedNuGetProject

        public override async Task<string> GetCacheFilePathAsync()
        {
            return GetCacheFilePath(await GetMSBuildProjectExtensionsPathAsync());
        }

        private static string GetCacheFilePath(string msbuildProjectExtensionsPath)
        {
            return NoOpRestoreUtilities.GetProjectCacheFilePath(cacheRoot: msbuildProjectExtensionsPath);
        }

        protected override async Task<string> GetAssetsFilePathAsync(bool shouldThrow)
        {
            var msbuildProjectExtensionsPath = await GetMSBuildProjectExtensionsPathAsync(shouldThrow);
            if (msbuildProjectExtensionsPath == null)
            {
                return null;
            }

            return Path.Combine(msbuildProjectExtensionsPath, LockFileFormat.AssetsFileName);
        }

        #endregion BuildIntegratedNuGetProject

        #region IDependencyGraphProject

        public override string MSBuildProjectPath => ProjectFullPath;

        public override async Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
        {
            PackageSpec packageSpec;
            if (context == null || !context.PackageSpecCache.TryGetValue(MSBuildProjectPath, out packageSpec))
            {
                packageSpec = await GetPackageSpecAsync(context.Settings);
                if (packageSpec == null)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentCulture, Strings.ProjectNotLoaded_RestoreFailed, ProjectName));
                }
                context?.PackageSpecCache.Add(ProjectFullPath, packageSpec);
            }

            return (new[] { packageSpec }, null);
        }

        private async Task<Dictionary<string, CentralPackageVersion>> GetCentralPackageVersionsAsync()
        {
            IEnumerable<(string PackageId, string Version)> packageVersions =
                        (await _vsProjectAdapter.GetBuildItemInformationAsync(ProjectBuildProperties.PackageVersion, ProjectBuildProperties.Version))
                        .Select(item => (PackageId: item.ItemId, Version: item.ItemMetadata.FirstOrDefault()));

            return packageVersions
                .Select(item => ToCentralPackageVersion(item.PackageId, item.Version))
                .Distinct(CentralPackageVersionNameComparer.Default)
                .ToDictionary(cpv => cpv.Name);
        }


        private CentralPackageVersion ToCentralPackageVersion(string packageId, string version)
        {
            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (string.IsNullOrEmpty(version))
            {
                return new CentralPackageVersion(packageId, VersionRange.All);
            }

            return new CentralPackageVersion(packageId, VersionRange.Parse(version));
        }

        #endregion

        #region NuGetProject

        public override async Task<bool> InstallPackageAsync(
            string packageId,
            VersionRange range,
            INuGetProjectContext _,
            BuildIntegratedInstallationContext __,
            CancellationToken token)
        {
            var dependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                    name: packageId,
                    versionRange: range,
                    typeConstraint: LibraryDependencyTarget.Package),
                SuppressParent = __.SuppressParent,
                IncludeType = __.IncludeType
            };

            await ProjectServices.References.AddOrUpdatePackageReferenceAsync(dependency, token);

            return true;
        }

        public override async Task AddFileToProjectAsync(string filePath)
        {
            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            EnvDTEProjectUtility.EnsureCheckedOutIfExists(_vsProjectAdapter.Project, _vsProjectAdapter.ProjectDirectory, filePath);

            var isFileExistsInProject = await EnvDTEProjectUtility.ContainsFileAsync(_vsProjectAdapter.Project, filePath);

            if (!isFileExistsInProject)
            {
                await AddProjectItemAsync(filePath);
            }
        }

        private async Task AddProjectItemAsync(string filePath)
        {
            var folderPath = Path.GetDirectoryName(filePath);
            var fullPath = filePath;

            string projectDirectory = _vsProjectAdapter.ProjectDirectory;
            if (filePath.Contains(projectDirectory))
            {
                // folderPath should always be relative to ProjectDirectory so if filePath already contains
                // ProjectDirectory then get a relative path and construct folderPath to get the appropriate
                // ProjectItems from dte where you have to add this file.
                var relativeLockFilePath = FileSystemUtility.GetRelativePath(projectDirectory, filePath);
                folderPath = Path.GetDirectoryName(relativeLockFilePath);
            }
            else
            {
                // get the fullPath wrt ProjectDirectory
                fullPath = FileSystemUtility.GetFullPath(projectDirectory, filePath);
            }

            var container = await EnvDTEProjectUtility.GetProjectItemsAsync(_vsProjectAdapter.Project, folderPath, createIfNotExists: true);

            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            container.AddFromFileCopy(fullPath);
        }

        public override async Task<bool> UninstallPackageAsync(
            PackageIdentity packageIdentity, INuGetProjectContext _, CancellationToken token)
        {
            await ProjectServices.References.RemovePackageReferenceAsync(packageIdentity.Id);

            return true;
        }

        #endregion

        private async Task<string> GetMSBuildProjectExtensionsPathAsync(bool shouldThrow = true)
        {
            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var msbuildProjectExtensionsPath = _vsProjectAdapter.GetMSBuildProjectExtensionsPath();

            if (string.IsNullOrEmpty(msbuildProjectExtensionsPath))
            {
                if (shouldThrow)
                {
                    throw new InvalidDataException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.MSBuildPropertyNotFound,
                        ProjectBuildProperties.MSBuildProjectExtensionsPath,
                        _vsProjectAdapter.ProjectDirectory));
                }

                return null;
            }

            return msbuildProjectExtensionsPath;
        }

        private static string GetPropertySafe(IVsProjectBuildProperties projectBuildProperties, string propertyName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var value = projectBuildProperties.GetPropertyValueWithDteFallback(propertyName);

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return value;
        }

        private string GetPackagesPath(ISettings settings)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var packagePath = GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RestorePackagesPath);

            if (string.IsNullOrWhiteSpace(packagePath))
            {
                return SettingsUtility.GetGlobalPackagesFolder(settings);
            }

            return UriUtility.GetAbsolutePathFromFile(ProjectFullPath, packagePath);
        }

        private IList<PackageSource> GetSources(ISettings settings)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var sources = MSBuildStringUtility.Split(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RestoreSources)).AsEnumerable();

            if (ShouldReadFromSettings(sources))
            {
                sources = SettingsUtility.GetEnabledSources(settings).Select(e => e.Source);
            }
            else
            {
                sources = VSRestoreSettingsUtilities.HandleClear(sources);
            }

            // Add additional sources
            sources = sources.Concat(MSBuildStringUtility.Split(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RestoreAdditionalProjectSources)));

            return sources.Select(e => new PackageSource(UriUtility.GetAbsolutePathFromFile(ProjectFullPath, e))).ToList();
        }

        private IList<string> GetFallbackFolders(ISettings settings)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var fallbackFolders = MSBuildStringUtility.Split(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RestoreFallbackFolders)).AsEnumerable();

            if (ShouldReadFromSettings(fallbackFolders))
            {
                fallbackFolders = SettingsUtility.GetFallbackPackageFolders(settings);
            }
            else
            {
                fallbackFolders = VSRestoreSettingsUtilities.HandleClear(fallbackFolders);
            }

            // Add additional fallback folders
            fallbackFolders = fallbackFolders.Concat(MSBuildStringUtility.Split(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RestoreAdditionalProjectFallbackFolders)));

            return fallbackFolders.Select(e => UriUtility.GetAbsolutePathFromFile(ProjectFullPath, e)).ToList();
        }

        private static bool ShouldReadFromSettings(IEnumerable<string> values)
        {
            return !values.Any();
        }

        private IList<string> GetConfigFilePaths(ISettings settings)
        {
            return settings.GetConfigFilePaths();
        }

        /// <summary>
        /// Emulates a JSON deserialization from project.json to PackageSpec in a post-project.json world
        /// </summary>
        private async Task<PackageSpec> GetPackageSpecAsync(ISettings settings)
        {
            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var projectReferences = await ProjectServices
                .ReferencesReader
                .GetProjectReferencesAsync(NullLogger.Instance, CancellationToken.None);

            var targetFramework = await _vsProjectAdapter.GetTargetFrameworkAsync();

            var packageReferences = (await ProjectServices
                .ReferencesReader
                .GetPackageReferencesAsync(targetFramework, CancellationToken.None))
                .ToList();

            var packageTargetFallback = MSBuildStringUtility.Split(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.PackageTargetFallback))
                .Select(NuGetFramework.Parse)
                .ToList();

            var assetTargetFallback = MSBuildStringUtility.Split(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.AssetTargetFallback))
                .Select(NuGetFramework.Parse)
                .ToList();

            var projectTfi = new TargetFrameworkInformation
            {
                FrameworkName = targetFramework,
                Dependencies = packageReferences,
            };

            bool isCpvmEnabled = MSBuildStringUtility.IsTrue(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.ManagePackageVersionsCentrally));
            if (isCpvmEnabled)
            {
                // Add the central version information and merge the information to the package reference dependencies
                projectTfi.CentralPackageVersions.AddRange(await GetCentralPackageVersionsAsync());
                LibraryDependency.ApplyCentralVersionInformation(projectTfi.Dependencies, projectTfi.CentralPackageVersions);
            }

            // Apply fallback settings
            AssetTargetFallbackUtility.ApplyFramework(projectTfi, packageTargetFallback, assetTargetFallback);

            // Build up runtime information.

            var runtimes = GetRuntimeIdentifiers(
                GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RuntimeIdentifier),
                GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RuntimeIdentifiers));
            var supports = GetRuntimeSupports(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RuntimeSupports));
            var runtimeGraph = new RuntimeGraph(runtimes, supports);

            // In legacy CSProj, we only have one target framework per project
            var tfis = new TargetFrameworkInformation[] { projectTfi };

            var projectName = ProjectName ?? ProjectUniqueName;

            string specifiedPackageId = _vsProjectAdapter.BuildProperties.GetPropertyValueWithDteFallback(ProjectBuildProperties.PackageId);

            if (!string.IsNullOrWhiteSpace(specifiedPackageId))
            {
                projectName = specifiedPackageId;
            }
            else
            {
                string specifiedAssemblyName = _vsProjectAdapter.BuildProperties.GetPropertyValueWithDteFallback(ProjectBuildProperties.AssemblyName);

                if (!string.IsNullOrWhiteSpace(specifiedAssemblyName))
                {
                    projectName = specifiedAssemblyName;
                }
            }

            string enableAudit = _vsProjectAdapter.BuildProperties.GetPropertyValue(ProjectBuildProperties.NuGetAudit);
            string auditLevel = _vsProjectAdapter.BuildProperties.GetPropertyValue(ProjectBuildProperties.NuGetAuditLevel);
            RestoreAuditProperties auditProperties = !string.IsNullOrEmpty(enableAudit) || !string.IsNullOrEmpty(auditLevel)
                ? new RestoreAuditProperties()
                {
                    EnableAudit = enableAudit,
                    AuditLevel = auditLevel
                }
                : null;

            var msbuildProjectExtensionsPath = await GetMSBuildProjectExtensionsPathAsync();
            return new PackageSpec(tfis)
            {
                Name = projectName,
                Version = new NuGetVersion(_vsProjectAdapter.Version),
                FilePath = ProjectFullPath,
                RuntimeGraph = runtimeGraph,
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = msbuildProjectExtensionsPath,
                    ProjectPath = ProjectFullPath,
                    ProjectName = projectName,
                    ProjectUniqueName = ProjectFullPath,
                    OriginalTargetFrameworks = tfis
                        .Select(tfi => tfi.FrameworkName.GetShortFolderName())
                        .ToList(),
                    TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                    {
                        new ProjectRestoreMetadataFrameworkInfo(tfis[0].FrameworkName)
                        {
                            ProjectReferences = projectReferences?.ToList()
                        }
                    },
                    SkipContentFileWrite = true,
                    CacheFilePath = GetCacheFilePath(msbuildProjectExtensionsPath),
                    PackagesPath = GetPackagesPath(settings),
                    Sources = GetSources(settings),
                    FallbackFolders = GetFallbackFolders(settings),
                    ConfigFilePaths = GetConfigFilePaths(settings),
                    ProjectWideWarningProperties = WarningProperties.GetWarningProperties(
                        treatWarningsAsErrors: GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.TreatWarningsAsErrors),
                        noWarn: GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.NoWarn),
                        warningsAsErrors: GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.WarningsAsErrors),
                        warningsNotAsErrors: GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.WarningsNotAsErrors)),
                    RestoreLockProperties = new RestoreLockProperties(
                        GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RestorePackagesWithLockFile),
                        GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.NuGetLockFilePath),
                        MSBuildStringUtility.IsTrue(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.RestoreLockedMode))),
                    CentralPackageVersionsEnabled = isCpvmEnabled,
                    CentralPackageVersionOverrideDisabled = GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.CentralPackageVersionOverrideEnabled).EqualsFalse(),
                    CentralPackageTransitivePinningEnabled = MSBuildStringUtility.IsTrue(GetPropertySafe(_vsProjectAdapter.BuildProperties, ProjectBuildProperties.CentralPackageTransitivePinningEnabled)),
                    RestoreAuditProperties = auditProperties,
                }
            };
        }

        internal static IEnumerable<RuntimeDescription> GetRuntimeIdentifiers(string unparsedRuntimeIdentifer, string unparsedRuntimeIdentifers)
        {
            var runtimes = Enumerable.Empty<string>();

            if (unparsedRuntimeIdentifer != null)
            {
                runtimes = runtimes.Concat(new[] { unparsedRuntimeIdentifer });
            }

            if (unparsedRuntimeIdentifers != null)
            {
                runtimes = runtimes.Concat(unparsedRuntimeIdentifers.Split(';'));
            }

            runtimes = runtimes
                .Select(x => x.Trim())
                .Distinct(StringComparer.Ordinal)
                .Where(x => !string.IsNullOrEmpty(x));

            return runtimes
                .Select(runtime => new RuntimeDescription(runtime));
        }

        internal static IEnumerable<CompatibilityProfile> GetRuntimeSupports(string unparsedRuntimeSupports)
        {
            if (unparsedRuntimeSupports == null)
            {
                return Enumerable.Empty<CompatibilityProfile>();
            }

            return unparsedRuntimeSupports
                .Split(';')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(support => new CompatibilityProfile(support));
        }

        /// <inheritdoc/>
        protected override Task<PackageSpec> GetPackageSpecAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return GetPackageSpecAsync(NullSettings.Instance);
        }

        protected override IEnumerable<PackageReference> ResolvedInstalledPackagesList(IEnumerable<LibraryDependency> libraries, NuGetFramework targetFramework, IList<LockFileTarget> targets, Dictionary<string, ProjectInstalledPackage> installedPackages)
        {
            return GetPackageReferences(libraries, targetFramework, installedPackages, targets);
        }

        protected override IReadOnlyList<PackageReference> ResolvedTransitivePackagesList(NuGetFramework targetFramework, IList<LockFileTarget> targets, Dictionary<string, ProjectInstalledPackage> installedPackages, Dictionary<string, ProjectInstalledPackage> transitivePackages)
        {
            return GetTransitivePackageReferences(targetFramework, installedPackages, transitivePackages, targets);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, ProjectInstalledPackage> GetCollectionCopy(Dictionary<string, ProjectInstalledPackage> collection) => new(collection);
    }
}
