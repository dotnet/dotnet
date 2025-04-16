// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace NuGet.Commands.Restore.Utility
{
    /// <summary>
    /// <seealso cref="RestoreRequest"/> uses <see cref="PackageSpec"/> instances to represent project restore inputs.
    /// This class provides a single implementation of creating a <see cref="PackageSpec"/> from an <see cref="IProject"/>,
    /// which each restore entry point is responsible for creating an adapter for.
    /// </summary>
    /// <remarks>
    /// When modifying PackageSpecFactory with new properties or items, you still need to modify NuGet.targets for
    /// MSBuild restore, and the dotnet/project-system repo for SDK style projects in Visual Studio. In addition, it
    /// would be a good idea to manually test new features with non-SDK style projects in Visual Studio, and also static graph evaluation restores on the command line, to test all 4 restore entry points.
    /// </remarks>
    public static class PackageSpecFactory
    {
        /// <summary>
        /// Convert an MSBuild project to a PackageSpec.
        /// </summary>
        public static PackageSpec? GetPackageSpec(IProject project, ISettings settings)
        {
            (ProjectRestoreMetadata? restoreMetadata, List<TargetFrameworkInformation>? targetFrameworkInfos) = GetProjectRestoreMetadataAndTargetFrameworkInformation(project, settings);

            if (restoreMetadata == null || targetFrameworkInfos == null)
            {
                return null;
            }

            var packageSpec = new PackageSpec(targetFrameworkInfos)
            {
                FilePath = project.FullPath,
                Name = restoreMetadata.ProjectName,
                RestoreMetadata = restoreMetadata,
                RuntimeGraph = new RuntimeGraph(
                    MSBuildStringUtility.Split($"{project.OuterBuild.GetProperty("RuntimeIdentifiers")};{project.OuterBuild.GetProperty("RuntimeIdentifier")}")
                        .Concat(project.TargetFrameworks.Values.SelectMany(i => MSBuildStringUtility.Split($"{i.GetProperty("RuntimeIdentifiers")};{i.GetProperty("RuntimeIdentifier")}")))
                        .Distinct(StringComparer.Ordinal)
                        .Select(rid => new RuntimeDescription(rid))
                        .ToList(),
                    MSBuildStringUtility.Split(project.OuterBuild.GetProperty("RuntimeSupports"))
                        .Distinct(StringComparer.Ordinal)
                        .Select(s => new CompatibilityProfile(s))
                        .ToList()
                    ),
                Version = GetProjectVersion(project.OuterBuild)
            };

            return packageSpec;
        }

        /// <summary>
        /// Gets the version of the project.
        /// </summary>
        /// <param name="project">The <see cref="ITargetFramework" /> representing the project.</param>
        /// <returns>The <see cref="NuGetVersion" /> of the specified project if one was found, otherwise <see cref="PackageSpec.DefaultVersion" />.</returns>
        internal static NuGetVersion GetProjectVersion(ITargetFramework project)
        {
            string? version = project.GetProperty("PackageVersion") ?? project.GetProperty("Version");

            if (version == null)
            {
                return PackageSpec.DefaultVersion;
            }

            return NuGetVersion.Parse(version);
        }

        /// <summary>
        /// Gets the restore metadata and target framework information for the specified project.
        /// </summary>
        /// <param name="project">An <see cref="IProject" /> representing the project.</param>
        /// <param name="settings">The <see cref="ISettings" /> of the specified project.</param>
        /// <returns>A <see cref="Tuple" /> containing the <see cref="ProjectRestoreMetadata" /> and <see cref="List{TargetFrameworkInformation}" /> for the specified project.</returns>
        private static (ProjectRestoreMetadata? RestoreMetadata, List<TargetFrameworkInformation>? TargetFrameworkInfos) GetProjectRestoreMetadataAndTargetFrameworkInformation(IProject project, ISettings settings)
        {
            ITargetFramework outerBuild = project.OuterBuild;
            string projectName = GetProjectName(outerBuild);

            string? outputPath = GetRestoreOutputPath(project);

            (ProjectStyle projectStyle, string? packagesConfigFilePath) = GetProjectStyle(project);

            (bool isCentralPackageManagementEnabled, bool isCentralPackageVersionOverrideDisabled, bool isCentralPackageTransitivePinningEnabled, bool isCentralPackageFloatingVersionsEnabled) =
                GetCentralPackageManagementSettings(outerBuild, projectStyle);

            RestoreAuditProperties? auditProperties = GetRestoreAuditProperties(project);

            List<TargetFrameworkInformation> targetFrameworkInfos = GetTargetFrameworkInfos(project, isCentralPackageManagementEnabled);

            ProjectRestoreMetadata restoreMetadata;

            if (projectStyle == ProjectStyle.PackagesConfig)
            {
                restoreMetadata = new PackagesConfigProjectRestoreMetadata
                {
                    PackagesConfigPath = packagesConfigFilePath,
                    RepositoryPath = GetRepositoryPath(project, settings),
                    RestoreAuditProperties = auditProperties,
                };
            }
            else
            {
                restoreMetadata = new ProjectRestoreMetadata
                {
                    // CrossTargeting is on, even if the TargetFrameworks property has only 1 tfm.
                    CrossTargeting = (projectStyle == ProjectStyle.PackageReference || projectStyle == ProjectStyle.DotnetToolReference) && (
                        project.TargetFrameworks.Count > 1 || !string.IsNullOrWhiteSpace(project.OuterBuild.GetProperty("TargetFrameworks"))),
                    FallbackFolders = GetFallbackFolders(
                        outerBuild.GetProperty("MSBuildStartupDirectory"),
                        project.Directory,
                        SplitPropertyValueOrNull(outerBuild, "RestoreFallbackFolders"),
                        SplitPropertyValueOrNull(outerBuild, "RestoreFallbackFolders"),
                        project.TargetFrameworks.Values.SelectMany(i => MSBuildStringUtility.Split(i.GetProperty("RestoreAdditionalProjectFallbackFolders"))),
                        project.TargetFrameworks.Values.SelectMany(i => MSBuildStringUtility.Split(i.GetProperty("RestoreAdditionalProjectFallbackFoldersExcludes"))),
                        settings),
                    SkipContentFileWrite = IsLegacyProject(outerBuild),
                    ValidateRuntimeAssets = outerBuild.IsPropertyTrue("ValidateRuntimeIdentifierCompatibility"),
                    CentralPackageVersionsEnabled = isCentralPackageManagementEnabled && projectStyle == ProjectStyle.PackageReference,
                    CentralPackageFloatingVersionsEnabled = isCentralPackageFloatingVersionsEnabled,
                    CentralPackageVersionOverrideDisabled = isCentralPackageVersionOverrideDisabled,
                    CentralPackageTransitivePinningEnabled = isCentralPackageTransitivePinningEnabled,
                    RestoreAuditProperties = auditProperties
                };
            }

            restoreMetadata.CacheFilePath = NoOpRestoreUtilities.GetProjectCacheFilePath(outputPath, project.FullPath);
            restoreMetadata.ConfigFilePaths = settings.GetConfigFilePaths();
            restoreMetadata.OutputPath = outputPath;
            targetFrameworkInfos.ForEach(tfi =>
                restoreMetadata.OriginalTargetFrameworks.Add(
                        !string.IsNullOrEmpty(tfi.TargetAlias) ?
                            tfi.TargetAlias :
                            tfi.FrameworkName.GetShortFolderName()));
            restoreMetadata.PackagesPath = GetPackagesPath(project, settings);
            restoreMetadata.ProjectName = projectName;
            restoreMetadata.ProjectPath = project.FullPath;
            restoreMetadata.ProjectStyle = projectStyle;
            restoreMetadata.ProjectUniqueName = project.FullPath;
            restoreMetadata.ProjectWideWarningProperties = WarningProperties.GetWarningProperties(outerBuild.GetProperty("TreatWarningsAsErrors"), outerBuild.GetProperty("WarningsAsErrors"), outerBuild.GetProperty("NoWarn"), outerBuild.GetProperty("WarningsNotAsErrors"));
            restoreMetadata.RestoreLockProperties = new RestoreLockProperties(outerBuild.GetProperty("RestorePackagesWithLockFile"), outerBuild.GetProperty("NuGetLockFilePath"), outerBuild.IsPropertyTrue("RestoreLockedMode"));
            restoreMetadata.Sources = GetSources(project, settings);
            restoreMetadata.TargetFrameworks = GetProjectRestoreMetadataFrameworkInfos(targetFrameworkInfos, project.TargetFrameworks);
            restoreMetadata.UsingMicrosoftNETSdk = MSBuildRestoreUtility.GetUsingMicrosoftNETSdk(outerBuild.GetProperty("UsingMicrosoftNETSdk"));
            restoreMetadata.SdkAnalysisLevel = MSBuildRestoreUtility.GetSdkAnalysisLevel(outerBuild.GetProperty("SdkAnalysisLevel"));
            restoreMetadata.UseLegacyDependencyResolver = outerBuild.IsPropertyTrue("RestoreUseLegacyDependencyResolver");

            return (restoreMetadata, targetFrameworkInfos);

            static (ProjectStyle, string? packagesConfigPath) GetProjectStyle(IProject project)
            {
                ProjectStyle? projectStyleOrNull = GetProjectRestoreStyleFromProjectProperty(project.OuterBuild.GetProperty("RestoreProjectStyle"));
                bool hasPackageReferenceItems = project.TargetFrameworks.Values.Any(p => p.GetItems("PackageReference").Any());
                (ProjectStyle ProjectStyle, bool IsPackageReferenceCompatibleProjectStyle, string? PackagesConfigFilePath) projectStyleResult =
                    GetProjectRestoreStyle(
                        restoreProjectStyle: projectStyleOrNull,
                        hasPackageReferenceItems: hasPackageReferenceItems,
                        projectJsonPath: project.OuterBuild.GetProperty("_CurrentProjectJsonPath"),
                        projectDirectory: project.Directory,
                        projectName: project.OuterBuild.GetProperty("MSBuildProjectName"));

                return (projectStyleResult.ProjectStyle, projectStyleResult.PackagesConfigFilePath);
            }
        }

        /// <summary>
        /// Gets the target framework information for the specified project.  This includes the package references, package downloads, and framework references.
        /// </summary>
        /// <param name="project">An <see cref="IReadOnlyDictionary{NuGetFramework,ProjectInstance} "/> containing the projects by their target framework.</param>
        /// <param name="isCpvmEnabled">A flag that is true if the Central Package Management was enabled.</param>
        /// <returns>A <see cref="List{TargetFrameworkInformation}" /> containing the target framework information for the specified project.</returns>
        internal static List<TargetFrameworkInformation> GetTargetFrameworkInfos(IProject project, bool isCpvmEnabled)
        {
            var targetFrameworkInfos = new List<TargetFrameworkInformation>(project.TargetFrameworks.Count);

            foreach (var projectInnerNode in project.TargetFrameworks)
            {
                var msBuildProjectInstance = projectInnerNode.Value;
                var targetAlias = string.IsNullOrEmpty(projectInnerNode.Key) ? string.Empty : projectInnerNode.Key;

                NuGetFramework targetFramework = MSBuildProjectFrameworkUtility.GetProjectFramework(
                    projectFilePath: project.FullPath,
                    targetFrameworkMoniker: msBuildProjectInstance.GetProperty("TargetFrameworkMoniker"),
                    targetPlatformMoniker: msBuildProjectInstance.GetProperty("TargetPlatformMoniker"),
                    targetPlatformMinVersion: msBuildProjectInstance.GetProperty("TargetPlatformMinVersion"),
                    clrSupport: msBuildProjectInstance.GetProperty("CLRSupport"),
                    windowsTargetPlatformMinVersion: msBuildProjectInstance.GetProperty("WindowsTargetPlatformMinVersion"));

                var packageTargetFallback = MSBuildStringUtility.Split(msBuildProjectInstance.GetProperty("PackageTargetFallback")).Select(NuGetFramework.Parse).ToList();

                var assetTargetFallbackEnum = MSBuildStringUtility.Split(msBuildProjectInstance.GetProperty(nameof(TargetFrameworkInformation.AssetTargetFallback))).Select(NuGetFramework.Parse).ToList();

                AssetTargetFallbackUtility.EnsureValidFallback(packageTargetFallback, assetTargetFallbackEnum, project.FullPath);

                (targetFramework, ImmutableArray<NuGetFramework> imports, bool assetTargetFallback, bool warn) = AssetTargetFallbackUtility.GetFallbackFrameworkInformation(targetFramework, packageTargetFallback, assetTargetFallbackEnum);

                IReadOnlyDictionary<string, CentralPackageVersion>? centralPackageVersions = null;
                if (isCpvmEnabled)
                {
                    centralPackageVersions = GetCentralPackageVersions(msBuildProjectInstance);
                }

                var dependencies = GetPackageReferences(msBuildProjectInstance, isCpvmEnabled, centralPackageVersions);
                var prunedReferences = msBuildProjectInstance.IsPropertyTrue("RestoreEnablePackagePruning") ? GetPrunePackageReferences(msBuildProjectInstance) : [];

                var targetFrameworkInformation = new TargetFrameworkInformation()
                {
                    AssetTargetFallback = assetTargetFallback,
                    CentralPackageVersions = centralPackageVersions,
                    Dependencies = dependencies,
                    DownloadDependencies = GetPackageDownloads(msBuildProjectInstance).ToImmutableArray(),
                    FrameworkName = targetFramework,
                    Imports = imports,
                    FrameworkReferences = GetFrameworkReferences(msBuildProjectInstance),
                    PackagesToPrune = prunedReferences,
                    RuntimeIdentifierGraphPath = msBuildProjectInstance.GetProperty(nameof(TargetFrameworkInformation.RuntimeIdentifierGraphPath)),
                    TargetAlias = targetAlias,
                    Warn = warn
                };

                targetFrameworkInfos.Add(targetFrameworkInformation);
            }

            return targetFrameworkInfos;
        }

        private static RestoreAuditProperties? GetRestoreAuditProperties(IProject project)
        {
            string enableAudit = project.OuterBuild.GetProperty("NuGetAudit");
            string auditLevel = project.OuterBuild.GetProperty("NuGetAuditLevel");
            string auditMode = GetAuditMode(project);
            HashSet<string>? suppressionItems = GetAuditSuppressions(project.OuterBuild);

            if (enableAudit != null || auditLevel != null || auditMode != null
                || (suppressionItems != null && suppressionItems.Count > 0))
            {
                return new RestoreAuditProperties()
                {
                    EnableAudit = enableAudit,
                    AuditLevel = auditLevel,
                    AuditMode = auditMode,
                    SuppressedAdvisories = suppressionItems?.Count > 0 ? suppressionItems : null
                };
            }

            return null;

            // We want to set NuGetAuditMode to "all" if a multi-targeting project targets .NET 10 or higher.
            // However, that can only be done by an "inner build" evaulation, but we read other audit settings
            // from the project evaluation, not inner-builds. So, check the inner builds if any TFM sets mode
            // to "all", otherwise use the project's "outer build" mode.
            string GetAuditMode(IProject project)
            {
                foreach (var item in project.TargetFrameworks.NoAllocEnumerate())
                {
                    string auditMode = item.Value.GetProperty("NuGetAuditMode");
                    if (string.Equals(auditMode, "all", StringComparison.OrdinalIgnoreCase))
                    {
                        return auditMode;
                    }
                }

                string projectAuditMode = project.OuterBuild.GetProperty("NuGetAuditMode");
                return projectAuditMode;
            }
        }

        /// <summary>
        /// Gets a value indicating if the specified project is a legacy project.
        /// </summary>
        /// <param name="project">The <see cref="ITargetFramework" /> representing the project.</param>
        /// <returns><code>true</code> if the specified project is considered legacy, otherwise <code>false</code>.</returns>
        internal static bool IsLegacyProject(ITargetFramework project)
        {
            // We consider the project to be legacy if it does not specify TargetFramework or TargetFrameworks
            return project.GetProperty("TargetFramework") == null && project.GetProperty("TargetFrameworks") == null;
        }

        /// <summary>
        /// Determines the restore style of a project.
        /// </summary>
        /// <param name="restoreProjectStyle">An optional user supplied restore style.</param>
        /// <param name="hasPackageReferenceItems">A <see cref="bool"/> indicating whether or not the project has any PackageReference items.</param>
        /// <param name="projectJsonPath">An optional path to the project's project.json file.</param>
        /// <param name="projectDirectory">The full path to the project directory.</param>
        /// <param name="projectName">The name of the project file.</param>
        /// <returns>A <see cref="Tuple{ProjectStyle, Boolean}"/> containing the project style and a value indicating if the project is using a style that is compatible with PackageReference.
        /// If the value of <paramref name="restoreProjectStyle"/> is not empty and could not be parsed, <code>null</code> is returned.</returns>
        private static (ProjectStyle ProjectStyle, bool IsPackageReferenceCompatibleProjectStyle, string? PackagesConfigFilePath)
            GetProjectRestoreStyle(ProjectStyle? restoreProjectStyle, bool hasPackageReferenceItems, string projectJsonPath, string projectDirectory, string projectName)
        {
            ProjectStyle projectStyle;
            string? packagesConfigFilePath = null;

            // Allow a user to override by setting RestoreProjectStyle in the project.
            if (restoreProjectStyle.HasValue)
            {
                projectStyle = restoreProjectStyle.Value;
            }
            else if (hasPackageReferenceItems)
            {
                // If any PackageReferences exist treat it as PackageReference. This has priority over project.json.
                projectStyle = ProjectStyle.PackageReference;
            }
            else if (!string.IsNullOrWhiteSpace(projectJsonPath))
            {
                // If this is not a PackageReference project check if project.json or projectName.project.json exists.
                projectStyle = ProjectStyle.ProjectJson;
            }
            else if (ProjectHasPackagesConfigFile(projectDirectory, projectName, out packagesConfigFilePath))
            {
                // If this is not a PackageReference or ProjectJson project check if packages.config or packages.ProjectName.config exists
                projectStyle = ProjectStyle.PackagesConfig;
            }
            else
            {
                // This project is either a packages.config project or one that does not use NuGet at all.
                projectStyle = ProjectStyle.Unknown;
            }

            bool isPackageReferenceCompatibleProjectStyle = projectStyle == ProjectStyle.PackageReference || projectStyle == ProjectStyle.DotnetToolReference;

            return (projectStyle, isPackageReferenceCompatibleProjectStyle, packagesConfigFilePath);
        }

        /// <summary>
        /// Determines if the project has a packages.config file.
        /// </summary>
        /// <param name="projectDirectory">The full path of the project directory.</param>
        /// <param name="projectName">The name of the project file.</param>
        /// <param name="packagesConfigPath">Receives the full path to the packages.config file if one exists, otherwise <code>null</code>.</param>
        /// <returns><code>true</code> if a packages.config exists next to the project, otherwise <code>false</code>.</returns>
        private static bool ProjectHasPackagesConfigFile(string projectDirectory, string projectName, [NotNullWhen(true)] out string? packagesConfigPath)
        {
            if (string.IsNullOrWhiteSpace(projectDirectory))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(projectDirectory));
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(projectName));
            }

            packagesConfigPath = GetPackagesConfigFilePath(projectDirectory, projectName);

            return packagesConfigPath != null;
        }

        /// <summary>
        /// Gets the path to a packages.config for the specified project if one exists.
        /// </summary>
        /// <param name="projectDirectory">The full path to the project directory.</param>
        /// <param name="projectName">The name of the project file.</param>
        /// <returns>The path to the packages.config file if one exists, otherwise <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="projectDirectory" /> -or- <paramref name="projectName" /> is <see langword="null" />.</exception>
        private static string? GetPackagesConfigFilePath(string projectDirectory, string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectDirectory))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(projectDirectory));
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(projectName));
            }

            string packagesConfigPath = Path.Combine(projectDirectory, NuGetConstants.PackageReferenceFile);

            if (File.Exists(packagesConfigPath))
            {
                return packagesConfigPath;
            }

            packagesConfigPath = Path.Combine(projectDirectory, "packages." + projectName + ".config");

            if (File.Exists(packagesConfigPath))
            {
                return packagesConfigPath;
            }

            return null;
        }

        /// <summary>
        /// Gets the packages path for the specified project.
        /// </summary>
        /// <param name="project">The <see cref="IMSBuildItem" /> representing the project.</param>
        /// <param name="settings">The <see cref="ISettings" /> of the project.</param>
        /// <returns>The full path to the packages directory for the specified project.</returns>
        internal static string GetPackagesPath(IProject project, ISettings settings)
        {
            var packagesPath = GetValue(
                () => UriUtility.GetAbsolutePath(project.Directory, project.OuterBuild.GetProperty("RestorePackagesPath")),
                () => UriUtility.GetAbsolutePath(project.Directory, project.OuterBuild.GetProperty("RestorePackagesPath")),
                () => SettingsUtility.GetGlobalPackagesFolder(settings));

            // GetValue doesn't understand that the last func will always provide a non-null value.
            return packagesPath!;
        }

        /// <summary>
        /// Gets the package fallback folders for a project.
        /// </summary>
        /// <param name="startupDirectory">The start-up directory of the tool.</param>
        /// <param name="projectDirectory">The full path to the directory of the project.</param>
        /// <param name="fallbackFolders">A <see cref="T:string[]" /> containing the fallback folders for the project.</param>
        /// <param name="fallbackFoldersOverride">A <see cref="T:string[]" /> containing overrides for the fallback folders for the project.</param>
        /// <param name="additionalProjectFallbackFolders">An <see cref="IEnumerable{String}" /> containing additional fallback folders for the project.</param>
        /// <param name="additionalProjectFallbackFoldersExcludes">An <see cref="IEnumerable{String}" /> containing fallback folders to exclude.</param>
        /// <param name="settings">An <see cref="ISettings" /> object containing settings for the project.</param>
        /// <returns>A <see cref="T:string[]" /> containing the package fallback folders for the project.</returns>
        private static string[] GetFallbackFolders(string startupDirectory, string projectDirectory, string[]? fallbackFolders, string[]? fallbackFoldersOverride, IEnumerable<string> additionalProjectFallbackFolders, IEnumerable<string> additionalProjectFallbackFoldersExcludes, ISettings settings)
        {
            // Fallback folders
            var currentFallbackFolders = GetValue(
                () => fallbackFoldersOverride?.Select(e => UriUtility.GetAbsolutePath(startupDirectory, e)).ToArray(),
                () => MSBuildRestoreUtility.ContainsClearKeyword(fallbackFolders) ? Array.Empty<string>() : null,
                () => fallbackFolders?.Select(e => UriUtility.GetAbsolutePath(projectDirectory, e)).ToArray(),
                () => SettingsUtility.GetFallbackPackageFolders(settings).ToArray());

            // Append additional fallback folders after removing excluded folders
            var filteredAdditionalProjectFallbackFolders = MSBuildRestoreUtility.AggregateSources(
                    values: additionalProjectFallbackFolders,
                    excludeValues: additionalProjectFallbackFoldersExcludes);

            // GetValue doesn't understand that the last func will always provide a non-null value.
            return AppendItems(projectDirectory, currentFallbackFolders!, filteredAdditionalProjectFallbackFolders);
        }

        /// <summary>
        /// Determines the current settings for central package management for the specified project.
        /// </summary>
        /// <param name="project">The <see cref="ITargetFramework" /> to get the central package management settings from.</param>
        /// <param name="projectStyle">The <see cref="ProjectStyle?" /> of the specified project.  Specify <see langword="null" /> when the project does not define a restore style.</param>
        /// <returns>A <see cref="Tuple{T1, T2, T3, T4}" /> containing values indicating whether or not central package management is enabled, if the ability to override a package version </returns>
        private static (bool IsEnabled, bool IsVersionOverrideDisabled, bool IsCentralPackageTransitivePinningEnabled, bool isCentralPackageFloatingVersionsEnabled)
            GetCentralPackageManagementSettings(ITargetFramework project, ProjectStyle projectStyle)
        {
            if (projectStyle == ProjectStyle.PackageReference)
            {
                bool isEnabled = IsPropertyTrue(project, "_CentralPackageVersionsEnabled");
                bool isVersionOverrideDisabled = IsPropertyFalse(project, "CentralPackageVersionOverrideEnabled");
                bool isCentralPackageTransitivePinningEnabled = IsPropertyTrue(project, "CentralPackageTransitivePinningEnabled");
                bool isCentralPackageFloatingVersionsEnabled = IsPropertyTrue(project, "CentralPackageFloatingVersionsEnabled");
                return (isEnabled, isVersionOverrideDisabled, isCentralPackageTransitivePinningEnabled, isCentralPackageFloatingVersionsEnabled);
            }

            return (false, false, false, false);
        }

        /// <summary>
        /// Gets the name of the specified project.
        /// </summary>
        /// <param name="project">The <see cref="IMSBuildItem" /> representing the project.</param>
        /// <returns>The name of the specified project.</returns>
        private static string GetProjectName(ITargetFramework project)
        {
            string? packageId = project.GetProperty("PackageId");

            if (!string.IsNullOrWhiteSpace(packageId))
            {
                // If the PackageId property was specified, return that
                return packageId!;
            }

            string? assemblyName = project.GetProperty("AssemblyName");

            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                // If the AssemblyName property was specified, return that
                return assemblyName!;
            }

            // By default return the MSBuildProjectName which is a built-in property that represents the name of the project file without the file extension
            string? projectName = project.GetProperty("MSBuildProjectName");
            if (!string.IsNullOrWhiteSpace(projectName))
            {
                return projectName!;
            }

            throw new Exception("Something went wrong. MSBuildProjectName should always have a value, but did not.");
        }

        /// <summary>
        /// Try to parse the <paramref name="restoreProjectStyleProperty"/> and return the <see cref="ProjectStyle"/> value.
        /// </summary>
        /// <param name="restoreProjectStyleProperty">The value of the RestoreProjectStyle property value. It can be null.</param>
        /// <returns>The <see cref="ProjectStyle"/>. If the <paramref name="restoreProjectStyleProperty"/> is null the return vale will be null. </returns>
        private static ProjectStyle? GetProjectRestoreStyleFromProjectProperty(string? restoreProjectStyleProperty)
        {
            ProjectStyle projectStyle;
            // Allow a user to override by setting RestoreProjectStyle in the project.
            if (!string.IsNullOrWhiteSpace(restoreProjectStyleProperty))
            {
                if (!Enum.TryParse(restoreProjectStyleProperty, ignoreCase: true, out projectStyle))
                {
                    projectStyle = ProjectStyle.Unknown;
                }
                return projectStyle;
            }

            return null;
        }

        /// <summary>
        /// Gets the restore metadata framework information for the specified projects.
        /// </summary>
        /// <param name="projects">A <see cref="IReadOnlyDictionary{NuGetFramework,ProjectInstance}" /> representing the target frameworks and their corresponding projects.</param>
        /// <returns>A <see cref="List{ProjectRestoreMetadataFrameworkInfo}" /> containing the restore metadata framework information for the specified project.</returns>
        internal static List<ProjectRestoreMetadataFrameworkInfo> GetProjectRestoreMetadataFrameworkInfos(List<TargetFrameworkInformation> targetFrameworkInfos, IReadOnlyDictionary<string, ITargetFramework> projects)
        {
            var projectRestoreMetadataFrameworkInfos = new List<ProjectRestoreMetadataFrameworkInfo>(projects.Count);

            foreach (var targetFrameworkInfo in targetFrameworkInfos)
            {
                var project = projects[targetFrameworkInfo.TargetAlias];
                projectRestoreMetadataFrameworkInfos.Add(new ProjectRestoreMetadataFrameworkInfo(targetFrameworkInfo.FrameworkName)
                {
                    TargetAlias = targetFrameworkInfo.TargetAlias,
                    ProjectReferences = GetProjectReferences(project)
                });
            }

            return projectRestoreMetadataFrameworkInfos;
        }

        /// <summary>
        /// Gets the project references of the specified project.
        /// </summary>
        /// <param name="project">The <see cref="ITargetFramework" /> to get project references for.</param>
        /// <returns>A <see cref="List{ProjectRestoreReference}" /> containing the project references for the specified project.</returns>
        internal static List<ProjectRestoreReference> GetProjectReferences(ITargetFramework project)
        {
            // Get the unique list of ProjectReference items that have the ReferenceOutputAssembly metadata set to "true", ignoring duplicates
            var projectReferenceItems = project.GetItems("ProjectReference")
                .Where(i => i.IsMetadataTrue("ReferenceOutputAssembly", defaultValue: true))
                .Distinct(ProjectItemIdentityComparer.Default)
                .ToList();

            var projectReferences = new List<ProjectRestoreReference>(projectReferenceItems.Count);

            foreach (var projectReferenceItem in projectReferenceItems)
            {
                string? fullPath = projectReferenceItem.GetMetadata("FullPath");

                projectReferences.Add(new ProjectRestoreReference
                {
                    ExcludeAssets = GetLibraryIncludeFlags(projectReferenceItem.GetMetadata("ExcludeAssets"), LibraryIncludeFlags.None),
                    IncludeAssets = GetLibraryIncludeFlags(projectReferenceItem.GetMetadata("IncludeAssets"), LibraryIncludeFlags.All),
                    PrivateAssets = GetLibraryIncludeFlags(projectReferenceItem.GetMetadata("PrivateAssets"), LibraryIncludeFlagUtils.DefaultSuppressParent),
                    ProjectPath = fullPath,
                    ProjectUniqueName = fullPath
                });
            }

            return projectReferences;
        }

        /// <summary>
        /// Gets the package references for the specified project.
        /// </summary>
        /// <param name="project">The <see cref="ProjectInstance" /> to get package references for.</param>
        /// <param name="isCentralPackageVersionManagementEnabled">A flag for central package version management being enabled.</param>
        /// <returns>A <see cref="List{LibraryDependency}" /> containing the package references for the specified project.</returns>
        internal static ImmutableArray<LibraryDependency> GetPackageReferences(ITargetFramework project, bool isCentralPackageVersionManagementEnabled, IReadOnlyDictionary<string, CentralPackageVersion>? centralPackageVersions)
        {
            // Get the distinct PackageReference items, ignoring duplicates
            List<IItem> packageReferenceItems = GetDistinctItemsOrEmpty(project, "PackageReference").ToList();

            var libraryDependencies = new LibraryDependency[packageReferenceItems.Count];

            for (int i = 0; i < packageReferenceItems.Count; i++)
            {
                var packageReferenceItem = packageReferenceItems[i];
                bool autoReferenced = packageReferenceItem.IsMetadataTrue("IsImplicitlyDefined");
                string version = packageReferenceItem.GetMetadata("Version");

                VersionRange? versionRange = string.IsNullOrWhiteSpace(version) ? null : VersionRange.Parse(version);
                bool versionDefined = versionRange != null;
                if (versionRange == null && !isCentralPackageVersionManagementEnabled)
                {
                    versionRange = VersionRange.All;
                }

                string versionOverrideString = packageReferenceItem.GetMetadata("VersionOverride");
                var versionOverrideRange = string.IsNullOrWhiteSpace(versionOverrideString) ? null : VersionRange.Parse(versionOverrideString);

                CentralPackageVersion? centralPackageVersion = null;
                bool isCentrallyManaged = !versionDefined && !autoReferenced && isCentralPackageVersionManagementEnabled && versionOverrideRange == null && centralPackageVersions != null && centralPackageVersions.TryGetValue(packageReferenceItem.Identity, out centralPackageVersion);
                if (isCentrallyManaged)
                {
                    versionRange = centralPackageVersion!.VersionRange;
                }
                versionRange = versionOverrideRange ?? versionRange;

                ImmutableArray<NuGetLogCode> noWarn = MSBuildStringUtility.GetNuGetLogCodes(packageReferenceItem.GetMetadata("NoWarn"));

                libraryDependencies[i] = new LibraryDependency()
                {
                    AutoReferenced = autoReferenced,
                    GeneratePathProperty = packageReferenceItem.IsMetadataTrue("GeneratePathProperty"),
                    Aliases = packageReferenceItem.GetMetadata("Aliases"),
                    IncludeType = GetLibraryIncludeFlags(packageReferenceItem.GetMetadata("IncludeAssets"), LibraryIncludeFlags.All) & ~GetLibraryIncludeFlags(packageReferenceItem.GetMetadata("ExcludeAssets"), LibraryIncludeFlags.None),
                    LibraryRange = new LibraryRange(
                        packageReferenceItem.Identity,
                        versionRange,
                        LibraryDependencyTarget.Package),
                    SuppressParent = GetLibraryIncludeFlags(packageReferenceItem.GetMetadata("PrivateAssets"), LibraryIncludeFlagUtils.DefaultSuppressParent),
                    VersionOverride = versionOverrideRange,
                    NoWarn = noWarn,
                    VersionCentrallyManaged = isCentrallyManaged,
                };
            }

            return ImmutableCollectionsMarshal.AsImmutableArray(libraryDependencies);
        }

        internal static Dictionary<string, PrunePackageReference> GetPrunePackageReferences(ITargetFramework project)
        {
            var result = new Dictionary<string, PrunePackageReference>(StringComparer.OrdinalIgnoreCase);
            IEnumerable<IItem> PrunePackageReferences = GetDistinctItemsOrEmpty(project, "PrunePackageReference");

            foreach (var projectItemInstance in PrunePackageReferences)
            {
                string id = projectItemInstance.Identity;
                string versionString = projectItemInstance.GetMetadata("Version");
                result.Add(id, PrunePackageReference.Create(id, versionString));
            }

            return result;
        }

        /// <summary>
        /// Gets the package downloads for the specified project.
        /// </summary>
        /// <param name="project">The <see cref="ITargetFramework" /> to get package downloads for.</param>
        /// <returns>An <see cref="IEnumerable{DownloadDependency}" /> containing the package downloads for the specified project.</returns>
        internal static IEnumerable<DownloadDependency> GetPackageDownloads(ITargetFramework project)
        {
            // Get the distinct PackageDownload items, ignoring duplicates
            foreach (IItem projectItemInstance in GetDistinctItemsOrEmpty(project, "PackageDownload"))
            {
                string id = projectItemInstance.Identity;

                // PackageDownload items can contain multiple versions
                string versionRanges = projectItemInstance.GetMetadata("Version");
                if (string.IsNullOrEmpty(versionRanges))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PackageDownload_NoVersion, id));
                }

                foreach (var version in MSBuildStringUtility.Split(versionRanges))
                {
                    // Validate the version range
                    VersionRange versionRange = !string.IsNullOrWhiteSpace(version) ? VersionRange.Parse(version) : VersionRange.All;

                    if (!(versionRange.HasLowerAndUpperBounds && versionRange.MinVersion.Equals(versionRange.MaxVersion)))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PackageDownload_OnlyExactVersionsAreAllowed, id, versionRange.OriginalString));
                    }

                    yield return new DownloadDependency(id, versionRange);
                }
            }
        }

        /// <summary>
        /// Gets the framework references per target framework for the specified project.
        /// </summary>
        /// <param name="project">The <see cref="ITargetFramework" /> to get framework references for.</param>
        /// <returns>A <see cref="List{FrameworkDependency}" /> containing the framework references for the specified project.</returns>
        internal static IReadOnlyCollection<FrameworkDependency>? GetFrameworkReferences(ITargetFramework project)
        {
            // Get the unique FrameworkReference items, ignoring duplicates
            List<IItem> frameworkReferenceItems = GetDistinctItemsOrEmpty(project, "FrameworkReference").ToList();

            if (frameworkReferenceItems.Count == 0)
            {
                return null;
            }

            // For best performance, its better to create a list with the exact number of items needed rather than using a LINQ statement or AddRange.  This is because if the list
            // is not allocated with enough items, the list has to be grown which can slow things down
            var frameworkDependencies = new FrameworkDependency[frameworkReferenceItems.Count];

            for (int i = 0; i < frameworkReferenceItems.Count; i++)
            {
                var frameworkReferenceItem = frameworkReferenceItems[i];
                var privateAssets = MSBuildStringUtility.Split(frameworkReferenceItem.GetMetadata("PrivateAssets"));

                frameworkDependencies[i] = new FrameworkDependency(frameworkReferenceItem.Identity, FrameworkDependencyFlagsUtils.GetFlags(privateAssets));
            }

            return frameworkDependencies;
        }

        /// <summary>
        /// Gets the <see cref="LibraryIncludeFlags" /> for the specified value.
        /// </summary>
        /// <param name="value">A semicolon delimited list of include flags.</param>
        /// <param name="defaultValue">The default value ot return if the value contains no flags.</param>
        /// <returns>The <see cref="LibraryIncludeFlags" /> for the specified value, otherwise the <paramref name="defaultValue" />.</returns>
        private static LibraryIncludeFlags GetLibraryIncludeFlags(string value, LibraryIncludeFlags defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            string[] parts = MSBuildStringUtility.Split(value);

            return parts.Length > 0 ? LibraryIncludeFlagUtils.GetFlags(parts) : defaultValue;
        }

        /// <summary>
        /// Gets the repository path for the specified project.
        /// </summary>
        /// <param name="project">The <see cref="IMSBuildItem" /> representing the project.</param>
        /// <param name="settings">The <see cref="ISettings" /> of the specified project.</param>
        /// <returns>The repository path of the specified project.</returns>
        private static string GetRepositoryPath(IProject project, ISettings settings)
        {
            var path = GetValue(
                () => UriUtility.GetAbsolutePath(project.Directory, project.OuterBuild.GetProperty("RestoreRepositoryPath")),
                () => UriUtility.GetAbsolutePath(project.Directory, project.OuterBuild.GetProperty("RestoreRepositoryPath")),
                () => SettingsUtility.GetRepositoryPath(settings),
                () =>
                {
                    string solutionDir = project.OuterBuild.GetProperty("SolutionPath");

                    solutionDir = string.Equals(solutionDir, "*Undefined*", StringComparison.OrdinalIgnoreCase)
                        ? project.Directory
                        : Path.GetDirectoryName(solutionDir)!;

                    return UriUtility.GetAbsolutePath(solutionDir, PackagesConfig.PackagesNodeName);
                });

            // GetValue doesn't understand that the last func will always provide a non-null value.
            return path!;
        }

        /// <summary>
        /// Gets the centrally defined package version information.
        /// </summary>
        /// <param name="project">The <see cref="ProjectInstance" /> to get PackageVersion for.</param>
        /// <returns>An <see cref="IEnumerable{CentralPackageVersion}" /> containing the package versions for the specified project.</returns>
        private static Dictionary<string, CentralPackageVersion> GetCentralPackageVersions(ITargetFramework project)
        {
            var result = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase);
            IEnumerable<IItem> packageVersionItems = GetDistinctItemsOrEmpty(project, "PackageVersion");

            foreach (var projectItemInstance in packageVersionItems)
            {
                string id = projectItemInstance.Identity;
                string version = projectItemInstance.GetMetadata("Version");
                VersionRange versionRange = string.IsNullOrWhiteSpace(version) ? VersionRange.All : VersionRange.Parse(version);

                result.Add(id, new CentralPackageVersion(id, versionRange));
            }

            return result;
        }

        /// <summary>
        /// Gets the package sources of the specified project.
        /// </summary>
        /// <param name="project">An <see cref="IProject" /> representing the project..</param>
        /// <param name="settings">The <see cref="ISettings" /> of the specified project.</param>
        /// <returns>A <see cref="List{PackageSource}" /> object containing the packages sources for the specified project.</returns>
        internal static List<PackageSource> GetSources(IProject project, ISettings settings)
        {
            return GetSources(
                project.OuterBuild.GetProperty("OriginalMSBuildStartupDirectory"),
                project.Directory,
                project.OuterBuild.SplitPropertyValueOrNull("RestoreSources"),
                project.OuterBuild.SplitPropertyValueOrNull("RestoreSources"),
                project.TargetFrameworks.Values.SelectMany(i => MSBuildStringUtility.Split(i.GetProperty("RestoreAdditionalProjectSources"))),
                settings)
                .Select(i => new PackageSource(i))
                .ToList();
        }

        private static string[] GetSources(string startupDirectory, string projectDirectory, string[]? sources, string[]? sourcesOverride, IEnumerable<string> additionalProjectSources, ISettings settings)
        {
            // Sources
            var currentSources = GetValue(
                () => sourcesOverride?.Select(MSBuildRestoreUtility.FixSourcePath).Select(e => UriUtility.GetAbsolutePath(startupDirectory, e)).ToArray(),
                () => MSBuildRestoreUtility.ContainsClearKeyword(sources) ? Array.Empty<string>() : null,
                () => sources?.Select(MSBuildRestoreUtility.FixSourcePath).Select(e => UriUtility.GetAbsolutePath(projectDirectory, e)).ToArray(),
                () => (PackageSourceProvider.LoadPackageSources(settings)).Where(e => e.IsEnabled).Select(e => e.Source).ToArray());

            // Append additional sources
            // Escape strings to avoid xplat path issues with msbuild.
            var filteredAdditionalProjectSources = MSBuildRestoreUtility.AggregateSources(
                    values: additionalProjectSources,
                    excludeValues: Enumerable.Empty<string>())
                .Select(MSBuildRestoreUtility.FixSourcePath);

            // GetValue doesn't understand that the last func will always provide a non-null value.
            return AppendItems(projectDirectory, currentSources!, filteredAdditionalProjectSources);
        }

        private static string[] AppendItems(string projectDirectory, string[] current, IEnumerable<string>? additional)
        {
            if (additional == null || !additional.Any())
            {
                // noop
                return current;
            }

            IEnumerable<string> additionalAbsolute = additional.Select(e => UriUtility.GetAbsolutePath(projectDirectory, e)!);

            return current.Concat(additionalAbsolute).ToArray();
        }

        /// <summary>
        /// Return the value from the first function that returns non-null.
        /// </summary>
        private static T? GetValue<T>(params Func<T>[] funcs)
        {
            var result = default(T);

            // Run until a value is returned from a function.
#pragma warning disable CS8604 // Possible null reference argument. net framework and netstandard BCLs don't have nullable annotations
            for (var i = 0; EqualityComparer<T>.Default.Equals(result, default) && i < funcs.Length; i++)
#pragma warning restore CS8604 // Possible null reference argument.
            {
                result = funcs[i]();
            }

            return result;
        }

        private static HashSet<string>? GetAuditSuppressions(ITargetFramework project)
        {
            IEnumerable<string> suppressions = GetDistinctItemsOrEmpty(project, "NuGetAuditSuppress")
                                                    .Select(i => i.Identity);

            return new HashSet<string>(suppressions, StringComparer.Ordinal);
        }

        /// <summary>
        /// Returns the list of distinct items with the <paramref name="itemName"/> name.
        /// Two items are equal if they have the same <see cref="IItem.Identity"/>.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="itemName">The item name.</param>
        /// <returns>Returns the list of items with the <paramref name="itemName"/>. If the item does not exist it will return an empty list.</returns>
        private static IEnumerable<IItem> GetDistinctItemsOrEmpty(ITargetFramework project, string itemName)
        {
            return project.GetItems(itemName)?.Distinct(ProjectItemIdentityComparer.Default) ?? Enumerable.Empty<IItem>();
        }

        /// <summary>
        /// Gets the restore output path for the specified project.
        /// </summary>
        /// <param name="project">The <see cref="IProject" /> representing the project.</param>
        /// <returns>The full path to the restore output directory for the specified project if a value is specified, otherwise <code>null</code>.</returns>
        internal static string? GetRestoreOutputPath(IProject project)
        {
            string? outputPath = project.OuterBuild.GetProperty("RestoreOutputPath") ?? project.OuterBuild.GetProperty("MSBuildProjectExtensionsPath");

            return outputPath == null ? null : Path.GetFullPath(Path.Combine(project.Directory, outputPath));
        }

        internal static string[] GetTargetFrameworkStrings(ITargetFramework project)
        {
            string? targetFrameworks = project.GetProperty("TargetFrameworks");
            if (string.IsNullOrEmpty(targetFrameworks))
            {
                targetFrameworks = project.GetProperty("TargetFramework");
            }
            var projectFrameworkStrings = MSBuildStringUtility.Split(targetFrameworks);
            return projectFrameworkStrings;
        }

        internal static bool IsPropertyTrue(this ITargetFramework project, string propertyName, bool defaultValue = false)
        {
            string? value = project.GetProperty(propertyName);

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }

        internal static bool IsPropertyFalse(this ITargetFramework project, string propertyName, bool defaultValue = false)
        {
            string value = project.GetProperty(propertyName);

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase);
        }

        internal static bool IsMetadataTrue(this IItem item, string metadataName, bool defaultValue = false)
        {
            string? value = item.GetMetadata(metadataName);

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Splits the value of the specified property and returns an array if the property has a value, otherwise returns <code>null</code>.
        /// </summary>
        /// <param name="project">The <see cref="ITargetFramework" /> to get the property value from.</param>
        /// <param name="name">The name of the property to get the value of and split.</param>
        /// <returns>A <see cref="T:string[]" /> containing the split value of the property if the property had a value, otherwise <code>null</code>.</returns>
        private static string[]? SplitPropertyValueOrNull(this ITargetFramework project, string name)
        {
            string? value = project.GetProperty(name);

            return value is null ? null : MSBuildStringUtility.Split(value);
        }
    }
}
