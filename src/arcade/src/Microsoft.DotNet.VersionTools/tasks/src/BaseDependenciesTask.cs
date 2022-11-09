// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Microsoft.DotNet.VersionTools.BuildManifest;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.DotNet.VersionTools.Dependencies;
using Microsoft.DotNet.VersionTools.Dependencies.BuildManifest;
using Microsoft.DotNet.VersionTools.Dependencies.BuildOutput;
using Microsoft.DotNet.VersionTools.Dependencies.BuildOutput.OrchestratedBuild;
using Microsoft.DotNet.VersionTools.Dependencies.Submodule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.VersionTools
{
    public abstract class BaseDependenciesTask : Microsoft.Build.Utilities.Task
    {
        internal const string RawUrlMetadataName = "RawUrl";
        internal const string RawVersionsBaseUrlMetadataName = "RawVersionsBaseUrl";
        internal const string VersionsRepoDirMetadataName = "VersionsRepoDir";
        internal const string BuildInfoPathMetadataName = "BuildInfoPath";
        internal const string CurrentRefMetadataName = "CurrentRef";
        internal const string PackageIdMetadataName = "PackageId";
        internal const string VersionMetadataName = "Version";
        internal const string DependencyTypeMetadataName = "DependencyType";
        internal const string ReplacementSubstituteOldMetadataName = "ReplacementSubstituteOld";
        internal const string ReplacementSubstituteNewMetadataName = "ReplacementSubstituteNew";

        [Required]
        public ITaskItem[] DependencyInfo { get; set; }

        public ITaskItem[] ProjectJsonFiles { get; set; }

        public ITaskItem[] UpdateStep { get; set; }

        public string BuildInfoCacheDir { get; set; }

        /// <summary>
        /// GitHub personal authentication token (PAT). If no PAT is provided, API calls are
        /// performed anonymously. This works for operations that don't need any permissions, like
        /// fetching the latest dotnet/versions commit hash. It is always preferable to supply a PAT
        /// because the anonymous user rate limit is small and per-IP.
        /// </summary>
        public string GitHubAuthToken { get; set; }
        public string GitHubUser { get; set; }

        /// <summary>
        /// A potentially authenticated GitHub client. Only valid during TraceListenedExecute.
        /// </summary>
        protected GitHubClient GitHubClient { get; private set; }

        protected Dictionary<IDependencyInfo, ITaskItem> DependencyInfoConfigItems { get; } =
            new Dictionary<IDependencyInfo, ITaskItem>();

        public override bool Execute()
        {
            GitHubAuth auth = null;
            if (!string.IsNullOrEmpty(GitHubAuthToken))
            {
                auth = new GitHubAuth(GitHubAuthToken, GitHubUser);
            }

            using (GitHubClient = new GitHubClient(auth))
            {
                Trace.Listeners.MsBuildListenedInvoke(Log, TraceListenedExecute);
            }
            return !Log.HasLoggedErrors;
        }

        protected abstract void TraceListenedExecute();

        protected Regex CreateXmlUpdateRegex(string elementName, string contentGroupName) =>
            new Regex($@"<{elementName}>(?<{contentGroupName}>.*)</{elementName}>");

        protected Regex CreateMSBuildSdkUpdateRegex(string msbuildSdkName, string contentGroupName) =>
            new Regex($@"""{msbuildSdkName}""\s*:\s*""(?<{contentGroupName}>.*)""");

        protected IEnumerable<IDependencyUpdater> CreateUpdaters()
        {
            if (ProjectJsonFiles != null && ProjectJsonFiles.Any())
            {
                yield return new ProjectJsonUpdater(ProjectJsonFiles.Select(item => item.ItemSpec));
            }

            foreach (ITaskItem step in UpdateStep ?? Enumerable.Empty<ITaskItem>())
            {
                string type = step.GetMetadata("UpdaterType");
                switch (type)
                {
                    case "Xml":
                        yield return CreateXmlUpdater(step);
                        break;

                    case "MSBuildSdk":
                        yield return CreateMSBuildSdkUpdater(step);
                        break;

                    case "File":
                        yield return ConfigureFileUpdater(
                            new FilePackageUpdater
                            {
                                PackageId = GetRequiredMetadata(step, "PackageId"),
                                Path = GetRequiredMetadata(step, "Path"),
                            },
                            step);
                        break;

                    case "Tool versions":
                        yield return new ToolVersionsUpdater
                        {
                            Path = GetRequiredMetadata(step, "Path"),
                        };
                        break;

                    case "Submodule from package":
                        yield return new IndicatorPackageSubmoduleUpdater(
                            GetRequiredMetadata(step, "IndicatorPackage"))
                        {
                            PackageDownloadBaseUrl = GetRequiredMetadata(step, "PackageDownloadBaseUrl"),
                            Path = GetRequiredMetadata(step, "Path")
                        };
                        break;

                    case "Submodule from latest":
                        yield return new LatestCommitSubmoduleUpdater(
                            GetRequiredMetadata(step, "Repository"),
                            GetRequiredMetadata(step, "Ref"))
                        {
                            Path = GetRequiredMetadata(step, "Path")
                        };
                        break;

                    case "Submodule from orchestrated build":
                        yield return new OrchestratedBuildSubmoduleUpdater
                        {
                            Path = GetRequiredMetadata(step, "Path"),
                            BuildName = GetRequiredMetadata(step, "BuildName"),
                            GitUrl = GetRequiredMetadata(step, "GitUrl")
                        };
                        break;

                    case "Build attribute from orchestrated build":
                        yield return CreateOrchestratedBuildUpdater(
                            step,
                            OrchestratedBuildUpdateHelpers.BuildAttribute(
                                GetRequiredMetadata(step, "BuildName"),
                                GetRequiredMetadata(step, "AttributeName")));
                        break;

                    case "Orchestrated blob feed attribute":
                        yield return CreateOrchestratedBuildUpdater(
                            step,
                            OrchestratedBuildUpdateHelpers.OrchestratedFeedAttribute(
                                GetRequiredMetadata(step, "AttributeName")));
                        break;

                    case "Orchestrated blob feed package version":
                        yield return CreateOrchestratedBuildUpdater(
                            step,
                            OrchestratedBuildUpdateHelpers.OrchestratedFeedPackageVersion(
                                GetRequiredMetadata(step, "PackageId")));
                        break;

                    default:
                        throw new NotSupportedException(
                            $"Unsupported updater '{step.ItemSpec}': UpdaterType '{type}'.");
                }
            }
        }

        protected IEnumerable<IDependencyInfo> CreateLocalDependencyInfos()
        {
            return CreateDependencyInfos(false, null);
        }

        protected IEnumerable<IDependencyInfo> CreateDependencyInfos(
            bool remote,
            string versionsCommit)
        {
            foreach (ITaskItem info in DependencyInfo ?? Enumerable.Empty<ITaskItem>())
            {
                IDependencyInfo dependencyInfo;

                string type = info.GetMetadata("DependencyType");
                switch (type)
                {
                    case "Build":
                        SetVersionsCommitOverride(info, versionsCommit);
                        dependencyInfo = CreateBuildInfoDependency(info, BuildInfoCacheDir);
                        break;

                    case "Submodule":
                        dependencyInfo = SubmoduleDependencyInfo.Create(
                            GetRequiredMetadata(info, "Repository"),
                            GetRequiredMetadata(info, "Ref"),
                            GetRequiredMetadata(info, "Path"),
                            remote);
                        break;

                    case "Orchestrated build":
                        SetVersionsCommitOverride(info, versionsCommit);
                        dependencyInfo = OrchestratedBuildDependencyInfo.CreateAsync(
                            info.ItemSpec,
                            new GitHubProject(
                                GetRequiredMetadata(info, "VersionsRepo"),
                                GetRequiredMetadata(info, "VersionsRepoOwner")),
                            GetRequiredMetadata(info, CurrentRefMetadataName),
                            GetRequiredMetadata(info, "BasePath"),
                            new BuildManifestClient(GitHubClient)).Result;
                        break;

                    case "Orchestrated build file":
                        dependencyInfo = new OrchestratedBuildDependencyInfo(
                            info.ItemSpec,
                            OrchestratedBuildModel.Parse(
                                XElement.Parse(
                                    File.ReadAllText(
                                        GetRequiredMetadata(info, "Path")))));
                        break;

                    default:
                        throw new NotSupportedException(
                            $"Unsupported DependencyInfo '{info.ItemSpec}': DependencyType '{type}'.");
                }

                DependencyInfoConfigItems[dependencyInfo] = info;
                yield return dependencyInfo;
            }
        }

        private FileRegexUpdater CreateXmlUpdater(ITaskItem step)
        {
            string buildInfoName = step.GetMetadata("BuildInfoName");
            string packageId = step.GetMetadata("PackageId");

            FileRegexUpdater updater;

            if (!string.IsNullOrEmpty(buildInfoName))
            {
                updater = new FileRegexReleaseUpdater
                {
                    BuildInfoName = buildInfoName
                };
            }
            else
            {
                updater = new FileRegexPackageUpdater
                {
                    PackageId = packageId
                };
            }
            ConfigureFileRegexUpdater(updater, step);
            return updater;
        }

        private FileRegexUpdater CreateMSBuildSdkUpdater(ITaskItem step)
        {
            string packageId = step.GetMetadata("PackageId");

            var updater = new FileRegexPackageUpdater
            {
                PackageId = packageId
            };

            ConfigureFileRegexUpdater(updater, step);
            return updater;
        }

        private FileUpdater ConfigureFileUpdater(FileUpdater updater, ITaskItem step)
        {
            updater.SkipIfNoReplacementFound = string.Equals(
                step.GetMetadata(nameof(updater.SkipIfNoReplacementFound)),
                "true",
                StringComparison.OrdinalIgnoreCase);

            // GetMetadata doesn't return null: empty string whether or not metadata is assigned.
            string oldValue = step.GetMetadata(ReplacementSubstituteOldMetadataName);
            string newValue = step.GetMetadata(ReplacementSubstituteNewMetadataName);

            if (!string.IsNullOrEmpty(oldValue))
            {
                updater.ReplacementTransform = v => v.Replace(oldValue, newValue);
            }
            else if (!string.IsNullOrEmpty(newValue))
            {
                Log.LogError(
                    $"Metadata {ReplacementSubstituteNewMetadataName} supplied for updater " +
                    $"{step.ItemSpec} without {ReplacementSubstituteOldMetadataName}. " +
                    "It is impossible to replace the empty string with something.");
            }

            return updater;
        }

        private FileRegexUpdater ConfigureFileRegexUpdater(FileRegexUpdater updater, ITaskItem step)
        {
            updater.Path = step.GetMetadata("Path");

            string elementName = step.GetMetadata("ElementName");
            string manualRegex = step.GetMetadata("Regex");
            string msbuildSdkName = step.GetMetadata("MSBuildSdkName");
            if (!string.IsNullOrEmpty(elementName))
            {
                updater.Regex = CreateXmlUpdateRegex(elementName, nameof(elementName));
                updater.VersionGroupName = nameof(elementName);
            }
            else if (!string.IsNullOrEmpty(manualRegex))
            {
                updater.Regex = new Regex(manualRegex);
                updater.VersionGroupName = GetRequiredMetadata(step, "VersionGroupName");
            }
            else if (!string.IsNullOrEmpty(msbuildSdkName))
            {
                updater.Regex = CreateMSBuildSdkUpdateRegex(Regex.Escape(msbuildSdkName), nameof(msbuildSdkName));
                updater.VersionGroupName = nameof(msbuildSdkName);
            }
            else
            {
                throw new ArgumentException(
                    $"On '{step.ItemSpec}', did not find 'ElementName', 'Regex', or 'MSBuildSdkName' metadata.");
            }

            updater.SkipIfNoReplacementFound = string.Equals(
                step.GetMetadata(nameof(updater.SkipIfNoReplacementFound)),
                "true",
                StringComparison.OrdinalIgnoreCase);

            return updater;
        }

        private IDependencyUpdater CreateOrchestratedBuildUpdater(
            ITaskItem step,
            Func<OrchestratedBuildDependencyInfo[], DependencyReplacement> updater)
        {
            string path = step.GetMetadata("SingleLineFile");

            if (!string.IsNullOrEmpty(path))
            {
                return ConfigureFileUpdater(
                    new FileOrchestratedBuildCustomUpdater
                    {
                        GetDesiredValue = updater,
                        Path = path
                    },
                    step);
            }

            return ConfigureFileRegexUpdater(
                new FileRegexOrchestratedBuildCustomUpdater { GetDesiredValue = updater },
                step);
        }

        private static BuildDependencyInfo CreateBuildInfoDependency(ITaskItem item, string cacheDir)
        {
            BuildInfo info = CreateBuildInfo(item, cacheDir);

            bool updateStaticDependencies = item
                .GetMetadata("UpdateStableVersions")
                .Equals("true", StringComparison.OrdinalIgnoreCase);

            string[] disabledPackages = item
                .GetMetadata("DisabledPackages")
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            return new BuildDependencyInfo(
                info,
                updateStaticDependencies,
                disabledPackages);
        }

        private static BuildInfo CreateBuildInfo(ITaskItem item, string cacheDir)
        {
            string rawUrl = item.GetMetadata(RawUrlMetadataName);

            if (!string.IsNullOrEmpty(rawUrl))
            {
                return BuildInfo.Get(item.ItemSpec, rawUrl);
            }

            string rawVersionsBaseUrl = item.GetMetadata(RawVersionsBaseUrlMetadataName);
            string buildInfoPath = item.GetMetadata(BuildInfoPathMetadataName);
            string currentRef = item.GetMetadata(CurrentRefMetadataName);

            // Optional: override base url with a local directory.
            string versionsRepoDir = item.GetMetadata(VersionsRepoDirMetadataName);

            if (!string.IsNullOrEmpty(versionsRepoDir) &&
                !string.IsNullOrEmpty(buildInfoPath))
            {
                return BuildInfo.LocalFileGetAsync(
                    item.ItemSpec,
                    versionsRepoDir,
                    buildInfoPath,
                    // Don't fetch latest release file: it may not be present in build from source.
                    fetchLatestReleaseFile: false).Result;
            }

            if (!string.IsNullOrEmpty(rawVersionsBaseUrl) &&
                !string.IsNullOrEmpty(buildInfoPath) &&
                !string.IsNullOrEmpty(currentRef))
            {
                return BuildInfo.CachedGet(
                    item.ItemSpec,
                    rawVersionsBaseUrl,
                    currentRef,
                    buildInfoPath,
                    cacheDir);
            }

            string packageId = item.GetMetadata(PackageIdMetadataName);
            string version = item.GetMetadata(VersionMetadataName);

            if (!string.IsNullOrEmpty(packageId) &&
                !string.IsNullOrEmpty(version))
            {
                return new BuildInfo
                {
                    Name = item.ItemSpec,
                    LatestPackages = new Dictionary<string, string>
                    {
                        [packageId] = version
                    }
                };
            }

            throw new Exception($"Unable to create build info with '{item}'.");
        }

        private static string GetRequiredMetadata(ITaskItem item, string name)
        {
            string metadata = item.GetMetadata(name);
            if (string.IsNullOrEmpty(metadata))
            {
                throw new ArgumentException(
                    $"On '{item.ItemSpec}', did not find required '{name}' metadata.");
            }
            return metadata;
        }

        private static void SetVersionsCommitOverride(ITaskItem item, string versionsCommit)
        {
            if (versionsCommit != null)
            {
                ReplaceExistingMetadata(item, CurrentRefMetadataName, versionsCommit);
            }
        }

        private static void ReplaceExistingMetadata(ITaskItem item, string name, string value)
        {
            if (!string.IsNullOrEmpty(item.GetMetadata(name)))
            {
                item.SetMetadata(name, value);
            }
        }
    }
}
