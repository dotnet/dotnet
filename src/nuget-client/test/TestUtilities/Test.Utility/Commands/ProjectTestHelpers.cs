// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.RuntimeModel;
using NuGet.Test.Utility;

namespace NuGet.Commands.Test
{
    public static class ProjectTestHelpers
    {
        /// <summary>
        /// Create a restore request for the specs. Restore only the first one.
        /// </summary>
        public static async Task<RestoreRequest> GetRequestAsync(
            RestoreArgs restoreContext,
            params PackageSpec[] projects)
        {
            var dgSpec = GetDGSpec(projects);

            var dgProvider = new DependencyGraphSpecRequestProvider(
                new RestoreCommandProvidersCache(),
                dgSpec);

            var requests = await dgProvider.CreateRequests(restoreContext);
            return requests.Single().Request;
        }

        /// <summary>
        /// Create a dg file for the specs. Restore only the first one.
        /// </summary>
        public static DependencyGraphSpec GetDGSpec(params PackageSpec[] projects)
        {
            var dgSpec = new DependencyGraphSpec();

            var project = EnsureRestoreMetadata(projects.First());

            dgSpec.AddProject(project);
            dgSpec.AddRestore(project.RestoreMetadata.ProjectUniqueName);

            foreach (var child in projects.Skip(1))
            {
                dgSpec.AddProject(EnsureRestoreMetadata(child));
            }

            return dgSpec;
        }

        /// <summary>
        /// Creates a dg specs with all PackageReference and project.json projects to be restored.
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>
        public static DependencyGraphSpec GetDGSpecFromPackageSpecs(params PackageSpec[] projects)
        {
            var dgSpec = new DependencyGraphSpec();
            foreach (var project in projects)
            {
                dgSpec.AddProject(project);
                if (project.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference ||
                    project.RestoreMetadata.ProjectStyle == ProjectStyle.ProjectJson)
                {
                    dgSpec.AddRestore(project.RestoreMetadata.ProjectUniqueName);
                }
            }
            return dgSpec;
        }

        /// <summary>
        /// Add restore metadata only if not already set.
        /// Sets the project style to PackageReference.
        /// </summary>
        public static PackageSpec EnsureRestoreMetadata(this PackageSpec spec)
        {
            if (string.IsNullOrEmpty(spec.RestoreMetadata?.ProjectUniqueName))
            {
                return spec.WithTestRestoreMetadata();
            }

            return spec;
        }

        /// <summary>
        /// Add restore metadata only if not already set.
        /// Sets the project style to PackageReference.
        /// </summary>
        public static PackageSpec EnsureProjectJsonRestoreMetadata(this PackageSpec spec)
        {
            if (string.IsNullOrEmpty(spec.RestoreMetadata?.ProjectUniqueName))
            {
                return spec.WithProjectJsonTestRestoreMetadata();
            }

            return spec;
        }

        public static PackageSpec WithTestProjectReference(this PackageSpec parent, PackageSpec child, params NuGetFramework[] frameworks)
        {
            return parent.WithTestProjectReference(child, privateAssets: LibraryIncludeFlagUtils.DefaultSuppressParent, frameworks);
        }

        public static PackageSpec WithTestProjectReference(this PackageSpec parent, PackageSpec child, LibraryIncludeFlags privateAssets, params NuGetFramework[] frameworks)
        {
            var spec = parent.Clone();

            if (frameworks.Length == 0)
            {
                // Use all frameworks if none were given
                frameworks = spec.TargetFrameworks.Select(e => e.FrameworkName).ToArray();
            }

            foreach (var framework in spec
                .RestoreMetadata
                .TargetFrameworks
                .Where(e => frameworks.Contains(e.FrameworkName)))
            {
                framework.ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectUniqueName = child.RestoreMetadata.ProjectUniqueName,
                    ProjectPath = child.RestoreMetadata.ProjectPath,
                    PrivateAssets = privateAssets,
                });
            }

            return spec;
        }

        /// <summary>
        /// Add fake PackageReference restore metadata.
        /// This resembles the .NET Core based projects (<see cref="ProjectRestoreSettings"/>.
        /// </summary>
        public static PackageSpec WithTestRestoreMetadata(this PackageSpec spec)
        {
            var updated = spec.Clone();
            var packageSpecFile = new FileInfo(spec.FilePath);

            var projectDir = (packageSpecFile.Attributes & FileAttributes.Directory) == FileAttributes.Directory && !spec.FilePath.EndsWith(".csproj") ?
                packageSpecFile.FullName :
                packageSpecFile.Directory.FullName;

            var projectPath = Path.Combine(projectDir, spec.Name + ".csproj");
            updated.FilePath = projectPath;

            updated.RestoreMetadata = new ProjectRestoreMetadata();
            updated.RestoreMetadata.CrossTargeting = updated.TargetFrameworks.Count > 1;
            updated.RestoreMetadata.OriginalTargetFrameworks = updated.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName()).ToList();
            updated.RestoreMetadata.OutputPath = projectDir;
            updated.RestoreMetadata.ProjectStyle = ProjectStyle.PackageReference;
            updated.RestoreMetadata.ProjectName = spec.Name;
            updated.RestoreMetadata.ProjectUniqueName = projectPath;
            updated.RestoreMetadata.ProjectPath = projectPath;
            updated.RestoreMetadata.ConfigFilePaths = new List<string>();
            updated.RestoreMetadata.CentralPackageVersionsEnabled = spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false;
            updated.RestoreMetadata.CentralPackageTransitivePinningEnabled = spec.RestoreMetadata?.CentralPackageTransitivePinningEnabled ?? false;

            updated.RestoreMetadata.RestoreAuditProperties = new RestoreAuditProperties()
            {
                EnableAudit = bool.FalseString
            };

            // Update the Target Alias.
            foreach (var framework in updated.TargetFrameworks)
            {
                if (string.IsNullOrEmpty(framework.TargetAlias))
                {
                    framework.TargetAlias = framework.FrameworkName.GetShortFolderName();
                }
            }
            foreach (var framework in updated.TargetFrameworks)
            {
                updated.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(framework.FrameworkName) { TargetAlias = framework.TargetAlias });
            }
            return updated;
        }

        private static PackageSpec WithProjectJsonTestRestoreMetadata(this PackageSpec spec)
        {
            var updated = spec.Clone();
            var metadata = new ProjectRestoreMetadata();
            updated.RestoreMetadata = metadata;

            var msbuildProjectFilePath = Path.Combine(Path.GetDirectoryName(spec.FilePath), spec.Name + ".csproj");
            var msbuildProjectExtensionsPath = Path.Combine(Path.GetDirectoryName(spec.FilePath), "obj");
            metadata.ProjectStyle = ProjectStyle.ProjectJson;
            metadata.OutputPath = msbuildProjectExtensionsPath;
            metadata.ProjectPath = msbuildProjectFilePath;
            metadata.ProjectJsonPath = spec.FilePath;
            metadata.ProjectName = spec.Name;
            metadata.ProjectUniqueName = msbuildProjectFilePath;
            metadata.CacheFilePath = NoOpRestoreUtilities.GetProjectCacheFilePath(msbuildProjectExtensionsPath);
            metadata.ConfigFilePaths = new List<string>();
            metadata.RestoreAuditProperties = new RestoreAuditProperties()
            {
                EnableAudit = bool.FalseString
            };

            foreach (var framework in updated.TargetFrameworks)
            {
                metadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(framework.FrameworkName) { });
            }

            return updated;
        }

        /// <summary>
        /// Creates a restore request, with the only source being the source from the <paramref name="pathContext"/>.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="pathContext"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static TestRestoreRequest CreateRestoreRequest(PackageSpec spec, SimpleTestPathContext pathContext, ILogger logger)
        {
            var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };
            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(spec);
            dgSpec.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

            return new TestRestoreRequest(spec, sources, pathContext.UserPackagesFolder, logger)
            {
                LockFilePath = Path.Combine(spec.FilePath, LockFileFormat.AssetsFileName),
                DependencyGraphSpec = dgSpec,
            };
        }

        public static TestRestoreRequest CreateRestoreRequest(PackageSpec projectToRestore, IEnumerable<PackageSpec> packageSpecsClosure, SimpleTestPathContext pathContext, ILogger logger)
        {
            var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };
            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(projectToRestore);
            dgSpec.AddRestore(projectToRestore.RestoreMetadata.ProjectUniqueName);

            foreach (var spec in packageSpecsClosure)
            {
                dgSpec.AddProject(spec);
            }

            var externalClosure = DependencyGraphSpecRequestProvider.GetExternalClosure(dgSpec, projectToRestore.RestoreMetadata.ProjectUniqueName).ToList();

            return new TestRestoreRequest(projectToRestore, sources, pathContext.UserPackagesFolder, logger)
            {
                LockFilePath = Path.Combine(projectToRestore.FilePath, LockFileFormat.AssetsFileName),
                DependencyGraphSpec = dgSpec,
                ExternalProjects = externalClosure,
            };
        }

        public static RuntimeGraph GetRuntimeGraph(IEnumerable<string> runtimeIdentifiers, IEnumerable<string> runtimeSupports)
        {
            var runtimes = runtimeIdentifiers?
                .Distinct(StringComparer.Ordinal)
                .Select(rid => new RuntimeDescription(rid))
                .ToList()
                ?? Enumerable.Empty<RuntimeDescription>();

            var supports = runtimeSupports?
                .Distinct(StringComparer.Ordinal)
                .Select(s => new CompatibilityProfile(s))
                .ToList()
                ?? Enumerable.Empty<CompatibilityProfile>();

            return new RuntimeGraph(runtimes, supports);
        }

        public static PackageSpec WithPackagesConfigRestoreMetadata(this PackageSpec spec)
        {
            var updated = spec.Clone();
            var packageSpecFile = new FileInfo(spec.FilePath);
            var projectDir = packageSpecFile.Directory.FullName;

            var projectPath = Path.Combine(projectDir, spec.Name + ".csproj");
            updated.FilePath = projectPath;

            updated.RestoreMetadata = new PackagesConfigProjectRestoreMetadata();
            updated.RestoreMetadata.OutputPath = projectDir;
            updated.RestoreMetadata.ProjectStyle = ProjectStyle.PackagesConfig;
            updated.RestoreMetadata.ProjectName = spec.Name;
            updated.RestoreMetadata.ProjectUniqueName = projectPath;
            updated.RestoreMetadata.ProjectPath = projectPath;
            updated.RestoreMetadata.ConfigFilePaths = new List<string>();
            (updated.RestoreMetadata as PackagesConfigProjectRestoreMetadata).PackagesConfigPath = Path.GetFullPath(Path.Combine(projectDir, "../packages"));

            foreach (var framework in updated.TargetFrameworks)
            {
                updated.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(framework.FrameworkName));
            }
            return updated;
        }

        /// <summary>
        /// Returns a PackageReference spec.
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="rootPath">Root path, normally solution root. The project is gonna be "located" at rootPath/projectName/projectName.csproj </param>
        /// <param name="framework">framework</param>
        /// <param name="useAssetTargetFallback">Whether to use ATF. Default is false.</param>
        /// <param name="assetTargetFallbackFrameworks">ATF string.</param>
        /// <returns>Returns a PackageReference spec with all details similar to what a spec from a nomination would contain.</returns>
        public static PackageSpec GetPackageSpec(string projectName, string rootPath = @"C:\", string framework = "net5.0", bool useAssetTargetFallback = false, string assetTargetFallbackFrameworks = "")
        {
            var actualAssetTargetFallback = GetFallbackString(useAssetTargetFallback, assetTargetFallbackFrameworks);

            const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""TARGET_FRAMEWORK"": {
                            ""dependencies"": {
                            }
                            ASSET_TARGET_FALLBACK
                        }
                    }
                }";

            var spec = referenceSpec.Replace("TARGET_FRAMEWORK", framework).Replace("ASSET_TARGET_FALLBACK", actualAssetTargetFallback);
            return GetPackageSpecWithProjectNameAndSpec(projectName, rootPath, spec);
        }

        /// <summary>
        /// Returns a PackageReference spec.
        /// </summary>
        /// <param name="settings">Settings to be used for the restore metadata.</param>
        /// <param name="projectName">Project name</param>
        /// <param name="rootPath">Root path, normally solution root. The project is gonna be "located" at rootPath/projectName/projectName.csproj </param>
        /// <param name="framework">framework</param>
        /// <returns>Returns a PackageReference spec with all details similar to what a spec after the post processing before restore would look like.
        /// The RestoreMetadata has the settings, sources etc set based on the ISettings provided.
        /// </returns>
        public static PackageSpec GetPackageSpec(ISettings settings, string projectName, string rootPath = @"C:\", string framework = "net5.0")
        {
            var packageSpec = GetPackageSpec(projectName, rootPath, framework);

            packageSpec.RestoreMetadata.ConfigFilePaths = settings.GetConfigFilePaths();
            packageSpec.RestoreMetadata.Sources = SettingsUtility.GetEnabledSources(settings).ToList();
            packageSpec.RestoreMetadata.FallbackFolders = SettingsUtility.GetFallbackPackageFolders(settings).ToList();
            packageSpec.RestoreMetadata.PackagesPath = SettingsUtility.GetGlobalPackagesFolder(settings);

            return packageSpec;
        }

        public static PackageSpec GetPackageSpec(string projectName, string rootPath, string framework, string dependencyName, bool useAssetTargetFallback = false, string assetTargetFallbackFrameworks = "", bool asAssetTargetFallback = true)
        {
            var actualAssetTargetFallback = GetFallbackString(useAssetTargetFallback, assetTargetFallbackFrameworks, asAssetTargetFallback);

            const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""TARGET_FRAMEWORK"": {
                            ""dependencies"": {
                                ""DEPENDENCY_NAME"" : ""1.0.0""
                            }
                            ASSET_TARGET_FALLBACK
                        }
                    }
                }";

            var spec = referenceSpec.Replace("TARGET_FRAMEWORK", framework).Replace("DEPENDENCY_NAME", dependencyName).Replace("ASSET_TARGET_FALLBACK", actualAssetTargetFallback);
            return GetPackageSpecWithProjectNameAndSpec(projectName, rootPath, spec);
        }

        private static string GetFallbackString(bool useAssetTargetFallback, string assetTargetFallbackFrameworks, bool asAssetTargetFallback = true)
        {
            const string assetTargetFallback = @",
                            ""assetTargetFallback"" : ATF_VALUE,
                            ""imports"" : [ ""ASSET_TARGET_FALLBACK_FRAMEWORK_LIST"" ],
                            ""warn"" : true
                        ";
            var actualAssetTargetFallback = useAssetTargetFallback ?
                assetTargetFallback.Replace("ASSET_TARGET_FALLBACK_FRAMEWORK_LIST", assetTargetFallbackFrameworks)
                    .Replace("ATF_VALUE", asAssetTargetFallback.ToString().ToLowerInvariant()) :
                string.Empty;
            return actualAssetTargetFallback;
        }

        private static PackageSpec GetPackageSpecWithProjectNameAndSpec(string projectName, string rootPath, string spec)
        {
            var packageSpec = JsonPackageSpecReader.GetPackageSpec(spec, projectName, Path.Combine(rootPath, projectName, projectName)).WithTestRestoreMetadata();
            return packageSpec;
        }

        public static PackageSpec GetPackagesConfigPackageSpec(string projectName, string rootPath = @"C:\", string framework = "net472")
        {
            const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""TARGET_FRAMEWORK"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";

            var spec = referenceSpec.Replace("TARGET_FRAMEWORK", framework);
            return JsonPackageSpecReader.GetPackageSpec(spec, projectName, Path.Combine(rootPath, projectName, projectName)).WithPackagesConfigRestoreMetadata();
        }
    }
}
