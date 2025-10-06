// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectManagement.Projects
{
    /// <summary>
    /// A NuGet integrated MSBuild project.k
    /// These projects contain a project.json
    /// </summary>
    public class ProjectJsonNuGetProject : BuildIntegratedNuGetProject
    {
        private readonly FileInfo _jsonConfig;
        private readonly string _projectName;

        /// <summary>
        /// Project.json based project system.
        /// </summary>
        /// <param name="jsonConfig">Path to project.json.</param>
        /// <param name="msBuildProjectPath">Path to the msbuild project file.</param>
        public ProjectJsonNuGetProject(
            string jsonConfig,
            string msBuildProjectPath)
        {
            if (jsonConfig == null)
            {
                throw new ArgumentNullException(nameof(jsonConfig));
            }

            if (msBuildProjectPath == null)
            {
                throw new ArgumentNullException(nameof(msBuildProjectPath));
            }

            _jsonConfig = new FileInfo(jsonConfig);

            MSBuildProjectPath = msBuildProjectPath;
            ProjectStyle = ProjectStyle.ProjectJson;

            _projectName = Path.GetFileNameWithoutExtension(msBuildProjectPath);

            if (string.IsNullOrEmpty(_projectName))
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Strings.InvalidProjectName, MSBuildProjectPath));
            }

            JObject projectJson;
            var targetFrameworks = Enumerable.Empty<NuGetFramework>();

            try
            {
                projectJson = GetJson();
                targetFrameworks = JsonConfigUtility.GetFrameworks(projectJson);
            }
            catch (InvalidOperationException)
            {
                // Ignore a bad project.json when constructing the project, and treat it as unsupported.
            }

            // Default to unsupported if anything unexpected is returned
            var targetFramework = NuGetFramework.UnsupportedFramework;

            // Having more than one framework is not supported, but we pick the first as fallback
            // We will eventually support more than one framework ala projectK.
            if (targetFrameworks.Count() == 1)
            {
                targetFramework = targetFrameworks.First();
            }

            InternalMetadata.Add(NuGetProjectMetadataKeys.TargetFramework, targetFramework);
            InternalMetadata.Add(NuGetProjectMetadataKeys.Name, _projectName);
            InternalMetadata.Add(NuGetProjectMetadataKeys.FullPath, msBuildProjectPath);

            var supported = new List<FrameworkName>
            {
                new FrameworkName(targetFramework.DotNetFrameworkName)
            };

            InternalMetadata.Add(NuGetProjectMetadataKeys.SupportedFrameworks, supported);
        }

        public override Task<string> GetAssetsFilePathAsync()
        {
            return Task.FromResult(ProjectJsonPathUtilities.GetLockFilePath(JsonConfigPath));
        }

        public override Task<string> GetAssetsFilePathOrNullAsync()
        {
            // project.json projects can always have their lock file path determined.
            return GetAssetsFilePathAsync();
        }

        public override Task AddFileToProjectAsync(string filePath)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// project.json path
        /// </summary>
        public string JsonConfigPath => _jsonConfig.FullName;

        public override string MSBuildProjectPath { get; }

        /// <summary>
        /// Project name
        /// </summary>
        public override string ProjectName => _projectName;

        protected virtual Task UpdateInternalTargetFrameworkAsync()
        {
            // Extending class will implement the functionality
            return Task.CompletedTask;
        }

        public override async Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
        {
            var packages = new List<PackageReference>();

            //  Find all dependencies and convert them into packages.config style references
            foreach (var dependency in JsonConfigUtility.GetDependencies(await GetJsonAsync()))
            {
                // Use the minimum version of the range for the identity
                var identity = new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion);

                // Pass the actual version range as the allowed range
                packages.Add(new PackageReference(identity,
                    targetFramework: null,
                    userInstalled: true,
                    developmentDependency: false,
                    requireReinstallation: false,
                    allowedVersions: dependency.VersionRange));
            }

            return packages;
        }

        protected virtual Task<string> GetMSBuildProjectExtensionsPathAsync()
        {
            // Extending class will implement the functionality.
            return TaskResult.Null<string>();
        }

        public override async Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
        {
            var (dgSpecs, _) = await GetPackageSpecsAndAdditionalMessagesAsync(context);
            return dgSpecs;
        }

        public override async Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
        {
            PackageSpec packageSpec = null;
            if (context == null || !context.PackageSpecCache.TryGetValue(MSBuildProjectPath, out packageSpec))
            {
                packageSpec = JsonPackageSpecReader.GetPackageSpec(ProjectName, JsonConfigPath);
                if (packageSpec == null)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentCulture, Strings.ProjectNotLoaded_RestoreFailed, ProjectName));
                }
                var metadata = new ProjectRestoreMetadata();
                packageSpec.RestoreMetadata = metadata;

                metadata.ProjectStyle = ProjectStyle.ProjectJson;
                metadata.OutputPath = await GetMSBuildProjectExtensionsPathAsync();
                metadata.ProjectPath = MSBuildProjectPath;
                metadata.ProjectJsonPath = packageSpec.FilePath;
                metadata.ProjectName = packageSpec.Name;
                metadata.ProjectUniqueName = MSBuildProjectPath;
                metadata.CacheFilePath = await GetCacheFilePathAsync();

                // Reload the target framework from csproj and update the target framework in packageSpec for restore
                await UpdateInternalTargetFrameworkAsync();

                if (TryGetInternalFramework(out var targetFramework))
                {
                    var nuGetFramework = targetFramework as NuGetFramework;
                    if (IsUAPFramework(nuGetFramework))
                    {
                        // Ensure the project json has only one target framework
                        if (packageSpec.TargetFrameworks != null && packageSpec.TargetFrameworks.Count == 1)
                        {
                            var tfi = packageSpec.TargetFrameworks[0];
                            if (tfi.Imports.Length > 0)
                            {
                                if (tfi.AssetTargetFallback)
                                {
                                    nuGetFramework = new AssetTargetFallbackFramework(nuGetFramework, tfi.Imports.AsList());
                                }
                                else
                                {
                                    nuGetFramework = new FallbackFramework(nuGetFramework, tfi.Imports.AsList());
                                }
                            }
                            packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(tfi) { FrameworkName = nuGetFramework };
                        }
                    }
                }

                var references = (await ProjectServices
                    .ReferencesReader
                    .GetProjectReferencesAsync(context?.Logger ?? NullLogger.Instance, CancellationToken.None))
                    .ToList();

                if (references != null && references.Count > 0)
                {
                    // Add msbuild reference groups for each TFM in the project
                    foreach (var framework in packageSpec.TargetFrameworks.Select(e => e.FrameworkName))
                    {
                        metadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(framework));
                    }

                    foreach (var reference in references)
                    {
                        // This reference applies to all frameworks
                        // Include/exclude flags may be applied later when merged with project.json
                        // Add the reference for all TFM groups, there are no conditional project
                        // references in UWP. There should also be just one TFM.
                        foreach (var frameworkInfo in metadata.TargetFrameworks)
                        {
                            frameworkInfo.ProjectReferences.Add(reference);
                        }
                    }
                }
                // Write restore settings to the package spec.
                // For project.json these properties may not come from the project file.
                var settings = context?.Settings ?? NullSettings.Instance;
                packageSpec.RestoreMetadata.PackagesPath = SettingsUtility.GetGlobalPackagesFolder(settings);
                packageSpec.RestoreMetadata.Sources = SettingsUtility.GetEnabledSources(settings).AsList();
                packageSpec.RestoreMetadata.FallbackFolders = SettingsUtility.GetFallbackPackageFolders(settings).AsList();
                packageSpec.RestoreMetadata.ConfigFilePaths = settings.GetConfigFilePaths();

                context?.PackageSpecCache.Add(MSBuildProjectPath, packageSpec);
            }

            return (new[] { packageSpec }, null);
        }

        public async override Task<bool> InstallPackageAsync(
            string packageId,
            VersionRange range,
            INuGetProjectContext nuGetProjectContext,
            BuildIntegratedInstallationContext installationContext,
            CancellationToken token)
        {
            var dependency = new PackageDependency(packageId, range);

            var json = await GetJsonAsync();

            JsonConfigUtility.AddDependency(json, dependency);

            await UpdateFrameworkAsync(json);

            await SaveJsonAsync(json);

            return true;
        }

        /// <summary>
        /// Uninstall a package from the config file.
        /// </summary>
        public async Task<bool> RemoveDependencyAsync(string packageId,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            var json = await GetJsonAsync();

            JsonConfigUtility.RemoveDependency(json, packageId);

            await UpdateFrameworkAsync(json);

            await SaveJsonAsync(json);

            return true;
        }

        public override async Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            return await RemoveDependencyAsync(packageIdentity.Id, nuGetProjectContext, token);
        }

        protected bool IsUAPFramework(NuGetFramework framework)
        {
            return string.Equals("uap", framework.Framework, StringComparison.OrdinalIgnoreCase);
        }

        private JObject GetJson()
        {
            try
            {
                return FileUtility.SafeRead(JsonConfigPath, (stream, filePath) =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return JObject.Parse(reader.ReadToEnd());
                    }
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Strings.ErrorLoadingPackagesConfig, _jsonConfig.FullName, ex.Message), ex);
            }
        }

        private async Task<JObject> GetJsonAsync()
        {
            try
            {
                return await FileUtility.SafeReadAsync(JsonConfigPath, async (stream, filePath) =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return JObject.Parse(await reader.ReadToEndAsync());
                    }
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Strings.ErrorLoadingPackagesConfig, _jsonConfig.FullName, ex.Message), ex);
            }
        }

        private async Task SaveJsonAsync(JObject json)
        {
            await FileUtility.ReplaceAsync(async (outputPath) =>
            {
                using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
                {
                    await writer.WriteAsync(json.ToString());
                }
            },
            JsonConfigPath);
        }

        private async Task UpdateFrameworkAsync(JObject json)
        {
            // Update the internal target framework with TPMinV from csproj
            await UpdateInternalTargetFrameworkAsync();

            if (TryGetInternalFramework(out var newTargetFrameworkObject))
            {
                var frameworks = JsonConfigUtility.GetFrameworks(json);
                var newTargetFramework = newTargetFrameworkObject as NuGetFramework;
                if (IsUAPFramework(newTargetFramework)
                    && frameworks.Count() == 1
                    && frameworks.Single() != newTargetFramework)
                {
                    // project.json can have only one target framework
                    JsonConfigUtility.ClearFrameworks(json);
                    JsonConfigUtility.AddFramework(json, newTargetFramework);
                }
            }
        }

        private bool TryGetInternalFramework(out object internalTargetFramework)
        {
            return InternalMetadata.TryGetValue(NuGetProjectMetadataKeys.TargetFramework, out internalTargetFramework);
        }

        // Overriding class wil implement the method
        public override Task<string> GetCacheFilePathAsync()
        {
            return TaskResult.Null<string>();
        }

        public override Task<bool> UninstallPackageAsync(string packageId, BuildIntegratedInstallationContext installationContext, CancellationToken token)
        {
            return RemoveDependencyAsync(packageId, null, token);
        }
    }
}
