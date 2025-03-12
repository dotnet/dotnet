// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Models.Darc;
using Microsoft.DotNet.ProductConstructionService.Client;
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MSBuild = Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class PublishBuildToMaestro : MSBuildTaskBase, ICancelableTask
    {
        [Required]
        public string ManifestsPath { get; set; }

        public string BuildAssetRegistryToken { get; set; }

        [Required]
        public string MaestroApiEndpoint { get; set; }

        private bool IsStableBuild { get; set; } = false;

        public bool AllowInteractive { get; set; } = false;

        public string RepoRoot { get; set; }

        public string AssetVersion { get; set; }

        [Output]
        public int BuildId { get; set; }

        private const string SearchPattern = "*.xml";
        private const string MergedManifestFileName = "MergedManifest.xml";
        private const string NoCategory = "NONE";
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private string _gitHubRepository = "";
        private string _gitHubBranch = "";

        // Set up proxy objects to allow unit test mocking
        internal IVersionIdentifierProxy _versionIdentifier = new VersionIdentifierProxy();
        internal IGetEnvProxy _getEnvProxy = new GetEnvProxy();
        private IBuildModelFactory _buildModelFactory;
        private IFileSystem _fileSystem;

        public const string NonShippingAttributeName = "NonShipping";
        public const string DotNetReleaseShippingAttributeName = "DotNetReleaseShipping";
        public const string CategoryAttributeName = "Category";

        public void Cancel()
        {
            _tokenSource.Cancel();
        }

        public bool ExecuteTask(IBuildModelFactory buildModelFactory,
                                IFileSystem fileSystem)
        {
            _buildModelFactory = buildModelFactory;
            _fileSystem = fileSystem;

            ExecuteAsync().GetAwaiter().GetResult();
            return !Log.HasLoggedErrors;
        }

        public async Task ExecuteAsync()
        {
            await PushMetadataAsync(_tokenSource.Token);
        }

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.TryAddSingleton<IBuildModelFactory, BuildModelFactory>();
            collection.TryAddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>();
            collection.TryAddSingleton<IPdbArtifactModelFactory, PdbArtifactModelFactory>();
            collection.TryAddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>();
            collection.TryAddSingleton<INupkgInfoFactory, NupkgInfoFactory>();
            collection.TryAddSingleton<IPackageArchiveReaderFactory, PackageArchiveReaderFactory>();
            collection.TryAddSingleton<IFileSystem, FileSystem>();
            collection.TryAddSingleton(Log);
        }

        public async Task<bool> PushMetadataAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                Log.LogMessage(MessageImportance.High, "Starting build metadata push to the Build Asset Registry...");

                if (!Directory.Exists(ManifestsPath))
                {
                    Log.LogError($"Required folder '{ManifestsPath}' does not exist.");
                }
                else
                {
                    //get the list of manifests
                    List<BuildModel> parsedManifests = LoadBuildModels(ManifestsPath, cancellationToken);

                    if (parsedManifests.Count == 0)
                    {
                        Log.LogError(
                            $"No manifests found matching the search pattern {SearchPattern} in {ManifestsPath}");
                        return !Log.HasLoggedErrors;
                    }

                    var mergedManifest = _buildModelFactory.CreateMergedModel(parsedManifests);

                    // Update the merged manifest with any missing manifest build data based on the environment.
                    mergedManifest.Identity.AzureDevOpsAccount = mergedManifest.Identity.AzureDevOpsAccount ?? GetAzDevAccount();
                    mergedManifest.Identity.AzureDevOpsProject = mergedManifest.Identity.AzureDevOpsProject ?? GetAzDevProject();
                    mergedManifest.Identity.AzureDevOpsBuildNumber = mergedManifest.Identity.AzureDevOpsBuildNumber ?? GetAzDevBuildNumber();
                    mergedManifest.Identity.AzureDevOpsBuildId = mergedManifest.Identity.AzureDevOpsBuildId ?? GetAzDevBuildId();
                    mergedManifest.Identity.AzureDevOpsRepository = mergedManifest.Identity.AzureDevOpsRepository ?? GetAzDevRepository();
                    mergedManifest.Identity.AzureDevOpsBranch = mergedManifest.Identity.AzureDevOpsBranch ?? GetAzDevBranch();
                    mergedManifest.Identity.AzureDevOpsBuildDefinitionId = mergedManifest.Identity.AzureDevOpsBuildDefinitionId ?? GetAzDevBuildDefinitionId();

                    string mergedManifestPath = Path.Combine(GetAzDevStagingDirectory(), MergedManifestFileName);

                    //add manifest as an asset to the buildModel
                    var mergedManifestAsset = AddManifestAsAsset(mergedManifest, mergedManifestPath);

                    // Write the merged manifest
                    _fileSystem.WriteToFile(mergedManifestPath, mergedManifest.ToXml().ToString());

                    Log.LogMessage(MessageImportance.High,
                                $"##vso[artifact.upload containerfolder=BlobArtifacts;artifactname=BlobArtifacts]{mergedManifestPath}");

                    // populate buildData and assetData using merged manifest data 
                    BuildData buildData = GetMaestroBuildDataFromMergedManifest(mergedManifest, mergedManifestAsset, cancellationToken);

                    IProductConstructionServiceApi client = PcsApiFactory.GetAuthenticated(
                        MaestroApiEndpoint,
                        BuildAssetRegistryToken,
                        managedIdentityId: null,
                        !AllowInteractive);

                    var deps = await GetBuildDependenciesAsync(client, cancellationToken);
                    Log.LogMessage(MessageImportance.High, "Calculated Dependencies:");
                    foreach (var dep in deps)
                    {
                        Log.LogMessage(MessageImportance.High, $"    {dep.BuildId}, IsProduct: {dep.IsProduct}");
                    }

                    buildData.Dependencies = deps;
                    LookupForMatchingGitHubRepository(mergedManifest.Identity);
                    buildData.GitHubBranch = _gitHubBranch;
                    buildData.GitHubRepository = _gitHubRepository;

                    ProductConstructionService.Client.Models.Build recordedBuild = await client.Builds.CreateAsync(buildData, cancellationToken);
                    BuildId = recordedBuild.Id;

                    Log.LogMessage(MessageImportance.High,
                        $"Metadata has been pushed. Build id in the Build Asset Registry is '{recordedBuild.Id}'");
                    Console.WriteLine($"##vso[build.addbuildtag]BAR ID - {recordedBuild.Id}");

                    // Only 'create' the AzDO (VSO) variables if running in an AzDO build
                    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILD_BUILDID")))
                    {
                        IEnumerable<DefaultChannel> defaultChannels =
                            await GetBuildDefaultChannelsAsync(client, recordedBuild);

                        var targetChannelIds = new HashSet<int>(defaultChannels.Select(dc => dc.Channel.Id));

                        var defaultChannelsStr = "[" + string.Join("][", targetChannelIds) + "]";
                        Log.LogMessage(MessageImportance.High,
                            $"Determined build will be added to the following channels: {defaultChannelsStr}");

                        Console.WriteLine($"##vso[task.setvariable variable=BARBuildId]{recordedBuild.Id}");
                        Console.WriteLine($"##vso[task.setvariable variable=DefaultChannels]{defaultChannelsStr}");
                        Console.WriteLine($"##vso[task.setvariable variable=IsStableBuild]{IsStableBuild}");
                    }
                }
            }
            catch (Exception exc)
            {
                Log.LogErrorFromException(exc, true, true, null);
            }

            return !Log.HasLoggedErrors;
        }

        private async Task<IEnumerable<DefaultChannel>> GetBuildDefaultChannelsAsync(IProductConstructionServiceApi client,
            ProductConstructionService.Client.Models.Build recordedBuild)
        {
            IEnumerable<DefaultChannel> defaultChannels = await client.DefaultChannels.ListAsync(
                branch: recordedBuild.GetBranch(),
                channelId: null,
                enabled: true,
                repository: recordedBuild.GetRepository()
            );

            Log.LogMessage(MessageImportance.High, "Found the following default channels:");
            foreach (var defaultChannel in defaultChannels)
            {
                Log.LogMessage(
                    MessageImportance.High,
                    $"    {defaultChannel.Repository}@{defaultChannel.Branch} " +
                    $"=> ({defaultChannel.Channel.Id}) {defaultChannel.Channel.Name}");
            }

            return defaultChannels;
        }

        private async Task<List<BuildRef>> GetBuildDependenciesAsync(
            IProductConstructionServiceApi client,
            CancellationToken cancellationToken)
        {
            var logger = new MSBuildLogger(Log);
            var local = new Local(new RemoteTokenProvider(), logger, RepoRoot);
            IEnumerable<DependencyDetail> dependencies = await local.GetDependenciesAsync();
            var builds = new Dictionary<int, bool>();
            var assetCache = new Dictionary<(string name, string version, string commit), int>();
            var buildCache = new Dictionary<int, ProductConstructionService.Client.Models.Build>();
            foreach (var dep in dependencies)
            {
                var buildId = await GetBuildId(dep, client, buildCache, assetCache, cancellationToken);
                if (buildId == null)
                {
                    Log.LogMessage(
                        MessageImportance.High,
                        $"Asset '{dep.Name}@{dep.Version}' not found in BAR, most likely this is an external dependency, ignoring...");
                    continue;
                }

                Log.LogMessage(
                    MessageImportance.Normal,
                    $"Dependency '{dep.Name}@{dep.Version}' found in build {buildId.Value}");

                var isProduct = dep.Type == DependencyType.Product;

                if (!builds.ContainsKey(buildId.Value))
                {
                    builds[buildId.Value] = isProduct;
                }
                else
                {
                    builds[buildId.Value] = isProduct || builds[buildId.Value];
                }
            }

            return builds.Select(t => new BuildRef(t.Key, t.Value, 0)).ToList();
        }

        private static async Task<int?> GetBuildId(DependencyDetail dep, IProductConstructionServiceApi client,
            Dictionary<int, ProductConstructionService.Client.Models.Build> buildCache,
            Dictionary<(string name, string version, string commit), int> assetCache,
            CancellationToken cancellationToken)
        {
            if (assetCache.TryGetValue((dep.Name, dep.Version, dep.Commit), out int value))
            {
                return value;
            }

            var assets = client.Assets.ListAssetsAsync(name: dep.Name, version: dep.Version,
                cancellationToken: cancellationToken);
            List<Asset> matchingAssetsFromSameSha = new List<Asset>();

            // Filter out those assets which do not have matching commits
            await foreach (Asset asset in assets)
            {
                if (!buildCache.TryGetValue(asset.BuildId, out ProductConstructionService.Client.Models.Build producingBuild))
                {
                    producingBuild = await client.Builds.GetBuildAsync(asset.BuildId, cancellationToken);
                    buildCache.Add(asset.BuildId, producingBuild);
                }

                if (producingBuild.Commit == dep.Commit)
                {
                    matchingAssetsFromSameSha.Add(asset);
                }
            }

            var buildId = matchingAssetsFromSameSha.OrderByDescending(a => a.Id).FirstOrDefault()?.BuildId;
            if (!buildId.HasValue)
            {
                return null;
            }

            // Commonly, if a repository has a dependency on an asset from a build, more dependencies will be to that same build
            // lets fetch all assets from that build to save time later.
            var build = await client.Builds.GetBuildAsync(buildId.Value, cancellationToken);
            foreach (var asset in build.Assets)
            {
                if (!assetCache.ContainsKey((asset.Name, asset.Version, build.Commit)))
                {
                    assetCache.Add((asset.Name, asset.Version, build.Commit), build.Id);
                }
            }

            return buildId;
        }

        private string GetVersion(string assetId)
        {
            return _versionIdentifier.GetVersion(assetId);
        }

        internal List<BuildModel> LoadBuildModels(
            string manifestsFolderPath,
            CancellationToken cancellationToken)
        {
            return Directory.GetFiles(manifestsFolderPath, SearchPattern, SearchOption.AllDirectories)
                .Select(manifest => _buildModelFactory.ManifestFileToModel(manifest))
                .ToList();
        }


        internal BuildData GetMaestroBuildDataFromMergedManifest(
            BuildModel buildModel,
            BlobArtifactModel mergedManifestAsset,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var assets = new List<AssetData>();

            IsStableBuild = buildModel.Identity.IsStable;

            // The AzureDevOps properties can be null in the Manifest, but maestro needs them. Read them from the environment if they are null in the manifest.
            var buildInfo = new BuildData(
                commit: buildModel.Identity.Commit,
                azureDevOpsAccount: buildModel.Identity.AzureDevOpsAccount,
                azureDevOpsProject: buildModel.Identity.AzureDevOpsProject,
                azureDevOpsBuildNumber: buildModel.Identity.AzureDevOpsBuildNumber,
                azureDevOpsRepository: buildModel.Identity.AzureDevOpsRepository,
                azureDevOpsBranch: buildModel.Identity.AzureDevOpsBranch,
                stable: buildModel.Identity.IsStable,
                released: false)
            {
                Assets = new List<AssetData>(),
                AzureDevOpsBuildId = buildModel.Identity.AzureDevOpsBuildId,
                AzureDevOpsBuildDefinitionId = buildModel.Identity.AzureDevOpsBuildDefinitionId,
                GitHubRepository = buildModel.Identity.Name,
                GitHubBranch = buildModel.Identity.Branch,
            };

            foreach (var package in buildModel.Artifacts.Packages)
            {
                AddAsset(
                    assets,
                    package.Id,
                    package.Version,
                    buildModel.Identity.InitialAssetsLocation,
                    (buildModel.Identity.InitialAssetsLocation == null) ? LocationType.NugetFeed : LocationType.Container,
                    package.NonShipping);
            }

            foreach (var blob in buildModel.Artifacts.Blobs)
            {
                string version = string.Empty;

                // The merged manifest will not have an identifiable version number,
                // and really we don't need to identify the version of it anyway,
                // since we don't need to create stable asset links for it.
                if (blob != mergedManifestAsset)
                {
                    version = GetVersion(blob.Id);

                    if (string.IsNullOrEmpty(version))
                    {
                        Log.LogWarning($"Version could not be extracted from '{blob.Id}'");
                        version = string.Empty;
                    }
                }

                AddAsset(
                    assets,
                    blob.Id,
                    version,
                    buildModel.Identity.InitialAssetsLocation,
                    LocationType.Container,
                    blob.NonShipping);
            }

            // At some point, maybe we want to include PDBs? No version information, but they do have locations which I suppose are
            // somewhat useful for tracking. They're not blobs though.

            buildInfo.Assets = buildInfo.Assets.Concat(assets).ToList();

            return buildInfo;
        }

        private string GetAzDevAccount()
        {
            var uri = new Uri(_getEnvProxy.GetEnv("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI"));
            if (uri.Host == "dev.azure.com")
            {
                return uri.AbsolutePath.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).First();
            }

            return uri.Host.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).First();
        }

        private string GetAzDevProject()
        {
            return _getEnvProxy.GetEnv("SYSTEM_TEAMPROJECT");
        }

        private string GetAzDevBuildNumber()
        {
            return _getEnvProxy.GetEnv("BUILD_BUILDNUMBER");
        }

        private string GetAzDevRepository()
        {
            return _getEnvProxy.GetEnv("BUILD_REPOSITORY_URI");
        }

        private string GetAzDevRepositoryName()
        {
            return _getEnvProxy.GetEnv("BUILD_REPOSITORY_NAME");
        }

        private string GetAzDevBranch()
        {
            return _getEnvProxy.GetEnv("BUILD_SOURCEBRANCH");
        }

        private int GetAzDevBuildId()
        {
            return int.Parse(_getEnvProxy.GetEnv("BUILD_BUILDID"));
        }

        private int GetAzDevBuildDefinitionId()
        {
            return int.Parse(_getEnvProxy.GetEnv("SYSTEM_DEFINITIONID"));
        }

        private string GetAzDevCommit()
        {
            return _getEnvProxy.GetEnv("BUILD_SOURCEVERSION");
        }

        private string GetAzDevStagingDirectory()
        {
            return _getEnvProxy.GetEnv("BUILD_STAGINGDIRECTORY");
        }

        /// <summary>
        ///     Add a new asset to the list of assets that will be uploaded to BAR
        /// </summary>
        /// <param name="assets">List of assets</param>
        /// <param name="assetName">Name of new asset</param>
        /// <param name="version">Version of asset</param>
        /// <param name="location">Location of asset</param>
        /// <param name="locationType">Type of location</param>
        /// <param name="nonShipping">If true, the asset is not intended for end customers</param>
        internal static void AddAsset(List<AssetData> assets, string assetName, string version, string location,
            LocationType locationType, bool nonShipping)
        {
            assets.Add(new AssetData(nonShipping)
            {
                Locations = location == null
                    ? null
                    : new List<AssetLocationData>() { new AssetLocationData(locationType) { Location = location } },
                Name = assetName,
                Version = version,
            });
        }

        /// <summary>
        /// When we flow dependencies we expect source and target repos to be the same i.e github.com or dev.azure.com/dnceng. 
        /// When this task is executed the repository is an Azure DevOps repository even though the real source is GitHub 
        /// since we just mirror the code. When we detect an Azure DevOps repository we check if the latest commit exists in 
        /// GitHub to determine if the source is GitHub or not. If the commit exists in the repo we transform the Url from 
        /// Azure DevOps to GitHub. If not we continue to work with the original Url.
        /// </summary>
        /// <returns></returns>
        private void LookupForMatchingGitHubRepository(BuildIdentity buildIdentity)
        {
            if (buildIdentity == null)
            {
                throw new ArgumentNullException(nameof(buildIdentity));
            }

            using (var client = new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true }))
            {
                string repoIdentity = string.Empty;
                string gitHubHost = "github.com";

                if (!Uri.TryCreate(buildIdentity.AzureDevOpsRepository, UriKind.Absolute, out Uri repoAddr))
                {
                    throw new Exception($"Can't parse the repository URL: {buildIdentity.AzureDevOpsRepository}");
                }

                if (repoAddr.Host.Equals(gitHubHost, StringComparison.OrdinalIgnoreCase))
                {
                    repoIdentity = repoAddr.AbsolutePath.Trim('/');
                }
                else
                {
                    repoIdentity = GetGithubRepoName(buildIdentity.AzureDevOpsRepository);
                }

                client.BaseAddress = new Uri($"https://api.{gitHubHost}");
                client.DefaultRequestHeaders.Add("User-Agent", "PushToBarTask");

                HttpResponseMessage response =
                    client.GetAsync($"/repos/{repoIdentity}/commits/{buildIdentity.Commit}").Result;

                if (response.IsSuccessStatusCode)
                {
                    _gitHubRepository = $"https://github.com/{repoIdentity}";
                    _gitHubBranch = buildIdentity.AzureDevOpsBranch;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                        || response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        throw new HttpRequestException($"API rate limit exceeded, HttpResponse: {response.StatusCode} {responseBody}. Please retry");
                    }
                    Log.LogMessage(MessageImportance.High,
                        $" Unable to translate AzDO to GitHub URL. HttpResponse: {response.StatusCode} {response.ReasonPhrase} for repoIdentity: {repoIdentity} and commit: {buildIdentity.Commit}.");
                    _gitHubRepository = null;
                    _gitHubBranch = null;
                }
            }
        }

        public static string GetRepoName(string repoUrl)
        {
            // In case the URL comes in ending with a '/', prevent an indexing exception
            repoUrl = repoUrl.TrimEnd('/');

            string[] segments = repoUrl.Split('/');
            string repoName = segments[segments.Length - 1].ToLower();

            if (repoUrl.Contains("DevDiv", StringComparison.OrdinalIgnoreCase)
                && repoName.EndsWith("-Trusted", StringComparison.OrdinalIgnoreCase))
            {
                repoName = repoName.Remove(repoName.LastIndexOf("-trusted"));
            }

            return repoName;
        }

        /// <summary>
        /// Get repo name from the Azure DevOps repo url
        /// </summary>
        /// <param name="repoUrl"></param>
        /// <returns></returns>
        public static string GetGithubRepoName(string repoUrl)
        {
            var repoName = GetRepoName(repoUrl);

            StringBuilder builder = new StringBuilder(repoName);
            int index = repoName.IndexOf('-');

            if (index > -1)
            {
                builder[index] = '/';
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates a merged manifest blob
        /// </summary>
        /// <param name="mergedModel">Merged build manifest model</param>
        /// <param name="manifestFileName">Merged manifest file name</param>
        /// <returns>A blob with data about the merged manifest</returns>
        internal BlobArtifactModel AddManifestAsAsset(BuildModel mergedModel, string manifestFileName)
        {
            string repoName = mergedModel.Identity.Name ?? GetRepoName(mergedModel.Identity.AzureDevOpsRepository);
            string buildNumber = mergedModel.Identity.AzureDevOpsBuildNumber;
            string id = $"assets/manifests/{repoName}/{buildNumber}/{Path.GetFileName(manifestFileName)}";

            var mergedManifestAsset = new BlobArtifactModel()
            {
                Id = $"{id}",
                NonShipping = true,
                RepoOrigin = repoName,
            };

            mergedModel.Artifacts.Blobs.Add(mergedManifestAsset);

            return mergedManifestAsset;
        }
    }
}
