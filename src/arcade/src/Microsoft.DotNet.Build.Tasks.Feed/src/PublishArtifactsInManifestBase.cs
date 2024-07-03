// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
#if !NET472_OR_GREATER
using Microsoft.DotNet.Maestro.Client;
using Microsoft.DotNet.Maestro.Client.Models;
#endif
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Newtonsoft.Json;
using NuGet.Versioning;
using static Microsoft.DotNet.Build.Tasks.Feed.GeneralUtils;
using static Microsoft.DotNet.Build.CloudTestTasks.AzureStorageUtils;
using MsBuildUtils = Microsoft.Build.Utilities;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.DotNet.Build.Tasks.Feed
{

#if !NET472_OR_GREATER
    /// <summary>
    /// The intended use of this task is to push artifacts described in
    /// a build manifest to a static package feed.
    /// </summary>
    public abstract class PublishArtifactsInManifestBase : Microsoft.Build.Utilities.Task
    {
        public AssetPublisherFactory AssetPublisherFactory { get; }

        /// <summary>
        /// Full path to the folder containing blob assets.
        /// </summary>
        public string BlobAssetsBasePath { get; set; }

        /// <summary>
        /// Full path to the folder containing package assets.
        /// </summary>
        public string PackageAssetsBasePath { get; set; }

        /// <summary>
        /// ID of the build (in BAR/Maestro) that produced the artifacts being published.
        /// This might change in the future as we'll probably fetch this ID from the manifest itself.
        /// </summary>
        public int BARBuildId { get; set; }

        /// <summary>
        /// Access point to the Maestro API to be used for accessing BAR.
        /// </summary>
        public string MaestroApiEndpoint { get; set; }

        /// <summary>
        /// Authentication token to be used when interacting with Maestro API.
        /// Deprecated and should not be used anymore.
        /// </summary>
        public string BuildAssetRegistryToken { get; set; }

        /// <summary>
        /// Federated token to be used in edge cases when this task cannot be called from within an AzureCLI task directly.
        /// The token is obtained in a previous AzureCLI@2 step and passed as a secret to this task.
        /// </summary>
        public string MaestroApiFederatedToken { get; set; }

        /// <summary>
        /// Managed identity to be used to authenticate with Maestro API in case the regular Azure CLI or token is not available.
        /// </summary>
        public string MaestroManagedIdentityId { get; set; }

        /// <summary>
        /// When running this task locally, allow the interactive browser-based authentication against Maestro.
        /// </summary>
        public bool AllowInteractiveAuthentication { get; set; }

        /// <summary>
        /// Directory where "nuget.exe" is installed. This will be used to publish packages.
        /// </summary>
        [Required]
        public string NugetPath { get; set; }

        /// <summary>
        /// We are setting StreamingPublishingMaxClients=16 and NonStreamingPublishingMaxClients=12 through publish-asset.yml as we were hitting OOM issue 
        /// https://github.com/dotnet/core-eng/issues/13098 for more details.
        /// </summary>
        public int StreamingPublishingMaxClients { get; set; }
        public int NonStreamingPublishingMaxClients { get; set; }

        /// <summary>
        /// Maximum number of parallel uploads for the upload tasks.
        /// For streaming publishing, 20 is used as the most optimal.
        /// For non-streaming publishing, 16 is used (there are multiple sets of 16-parallel uploads)
        ///
        /// NOTE: Due to the desire to run on hosted agents and drastic memory changes in these VMs 
        /// (see https://github.com/dotnet/core-eng/issues/13098 for details) these numbers are 
        /// currently reduced below optimal to prevent OOM.
        /// </summary>
        public int MaxClients { get { return UseStreamingPublishing ? StreamingPublishingMaxClients : NonStreamingPublishingMaxClients; } }

        /// <summary>
        /// Whether this build is internal or not. If true, extra checks are done to avoid accidental
        /// publishing of assets to public feeds or storage accounts.
        /// </summary>
        public bool InternalBuild { get; set; } = false;

        /// <summary>
        /// If true, safety checks only print messages and do not error
        /// - Internal asset to public feed
        /// - Stable packages to non-isolated feeds
        /// </summary>
        public bool SkipSafetyChecks { get; set; } = false;

        /// <summary>
        /// Which build model (i.e., parsed build manifest) the publishing task will operate on.
        /// This is set by the main publishing task before dispatching the execution to one of
        /// the version specific publishing task.
        /// </summary>
        public BuildModel BuildModel { get; set; }

        public string AkaMSClientId { get; set; }

        public string AkaMSClientSecret { get; set; }

        public X509Certificate2 AkaMSClientCertificate { get; set; }

        public string AkaMSTenant { get; set; }

        public string AkaMsOwners { get; set; }

        public string AkaMSCreatedBy { get; set; }

        public string AkaMSGroupOwner { get; set; }

        public string ManagedIdentityClientId { get; set; }

        public string BuildQuality { get; set; }

        public string AzdoApiToken { get; set; }

        public string ArtifactsBasePath { get; set; }

        public string AzureDevOpsFeedsApiVersion { get; set; } = "6.0";

        public string AzureApiVersionForFileDownload { get; set; } = "4.1-preview.4";

        public string AzureProject { get; set; }

        public string BuildId { get; set; }

        public string AzureDevOpsOrg { get; set; }

        private readonly string AzureDevOpsBaseUrl = $"https://dev.azure.com";

        /// <summary>
        /// Instead of relying on pre-downloaded artifacts, 'stream' artifacts in from the input build.
        /// Artifacts are downloaded one by one from the input build, and then immediately published and deleted.
        /// This allows for faster publishing by utilizing both upload and download pipes at the same time,
        /// and reduces maximum disk usage.
        /// This is not appplicable if the input build does not contain the artifacts for publishing
        /// (e.g. when publishing post-build signed assets)
        /// </summary>
        public bool UseStreamingPublishing { get; set; }

        public readonly Dictionary<TargetFeedContentType, HashSet<TargetFeedConfig>> FeedConfigs =
            new Dictionary<TargetFeedContentType, HashSet<TargetFeedConfig>>();

        private readonly Dictionary<TargetFeedContentType, HashSet<PackageArtifactModel>> PackagesByCategory =
            new Dictionary<TargetFeedContentType, HashSet<PackageArtifactModel>>();

        private readonly Dictionary<TargetFeedContentType, HashSet<BlobArtifactModel>> BlobsByCategory =
            new Dictionary<TargetFeedContentType, HashSet<BlobArtifactModel>>();

        private readonly ConcurrentDictionary<(int AssetId, string AssetLocation, LocationType LocationType), ValueTuple> NewAssetLocations =
            new ConcurrentDictionary<(int AssetId, string AssetLocation, LocationType LocationType), ValueTuple>();

        // Matches versions such as 1.0.0.1234
        private static readonly string FourPartVersionPattern = @"\d+\.\d+\.\d+\.\d+";

        private static Regex FourPartVersionRegex = new Regex(FourPartVersionPattern);

        private const string SymwebServerPath = "https://microsoft.artifacts.visualstudio.com/DefaultCollection";

        private const string MsdlServerPath = "https://microsoftpublicsymbols.artifacts.visualstudio.com/DefaultCollection";

        private const int ExpirationInDays = 3650;

        public int TimeoutInMinutes { get; set; } = 5;

        protected LatestLinksManager LinkManager { get; set; } = null;

        /// <summary>
        /// For functions where retry is possible, max number of retries to perform
        /// </summary>
        public int MaxRetryCount { get; set; } = 5;

        /// <summary>
        /// For functions where retry is possible, base value for waiting between retries (may be multiplied in 2nd-Nth retry)
        /// </summary>
        public int RetryDelayMilliseconds { get; set; } = 5000;

        public ExponentialRetry RetryHandler = new ExponentialRetry
        {
            MaxAttempts = 5,
            DelayBase = 2.5 // 2.5 ^ 5 = ~1.5 minutes max wait between retries
        };

        public enum ArtifactName
        {
            [Description("PackageArtifacts")]
            PackageArtifacts,

            [Description("BlobArtifacts")]
            BlobArtifacts
        }

        private int TimeoutInSeconds = 300;

        protected PublishArtifactsInManifestBase(AssetPublisherFactory assetPublisherFactory = null)
        {
            AssetPublisherFactory = assetPublisherFactory ?? new AssetPublisherFactory(Log);
        }

        public override bool Execute()
        {
            return ExecuteAsync().GetAwaiter().GetResult();
        }

        public abstract Task<bool> ExecuteAsync();

        /// <summary>
        ///     Lookup an asset in the build asset dictionary by name and version
        /// </summary>
        /// <param name="name">Name of asset</param>
        /// <param name="version">Version of asset</param>
        /// <returns>Asset if one with the name and version exists, null otherwise</returns>
        private Asset LookupAsset(string name, string version, ReadOnlyDictionary<string, Asset> buildAssets)
        {
            if (!buildAssets.TryGetValue(name, out Asset assetWithName))
            {
                return null;
            }
            if (assetWithName.Version == version)
            {
                return assetWithName;
            }
            return null;
        }

        /// <summary>
        ///     Lookup an asset in the build asset dictionary by name only.
        ///     This is for blob lookup purposes.
        /// </summary>
        /// <param name="name">Name of asset</param>
        /// <returns>
        ///     Asset if one with the name exists and is the only asset with the name.
        ///     Throws if there is more than one asset with that name.
        /// </returns>
        private Asset LookupAsset(string name, ReadOnlyDictionary<string, Asset> buildAssets)
        {
            if (!buildAssets.TryGetValue(name, out Asset assetWithName))
            {
                return null;
            }
            return assetWithName;
        }

        /// <summary>
        ///     Build up a map of asset name -> asset list so that we can avoid n^2 lookups when processing assets.
        ///     We use name only because blobs are only looked up by id (version is not recorded in the manifest).
        ///     This could mean that there might be multiple versions of the same asset in the build.
        /// </summary>
        /// <param name="buildInformation">Build information</param>
        /// <returns>Map of asset name -> list of assets with that name.</returns>
        protected ReadOnlyDictionary<string, Asset> CreateBuildAssetDictionary(Maestro.Client.Models.Build buildInformation)
        {
            Dictionary<string, Asset> buildAssets = new Dictionary<string, Asset>();

            foreach (var asset in buildInformation.Assets)
            {
                if (buildAssets.ContainsKey(asset.Name))
                {
                    Log.LogError($"Asset '{asset.Name}' is specified twice in the build information. Assets should not be duplicated.");
                }
                else
                {
                    buildAssets.Add(asset.Name, asset);
                }
            }

            return buildAssets.AsReadOnly();
        }

        /// <summary>
        ///   Records the association of Asset -> AssetLocation to be persisted later in BAR.
        /// </summary>
        /// <param name="assetId">Id of the asset (i.e., name of the package) whose the location should be updated.</param>
        /// <param name="assetVersion">Version of the asset whose the location should be updated.</param>
        /// <param name="buildAssets">List of BAR Assets for the build that's being modified.</param>
        /// <param name="feedConfig">Configuration of where the asset was published.</param>
        /// <param name="assetLocationType">Type of feed location that is being added.</param>
        /// <returns>True if that asset didn't have the informed location recorded already.</returns>
        private bool TryAddAssetLocation(string assetId, string assetVersion, ReadOnlyDictionary<string, Asset> buildAssets, TargetFeedConfig feedConfig, LocationType assetLocationType)
        {
            Asset assetRecord = string.IsNullOrEmpty(assetVersion) ?
                LookupAsset(assetId, buildAssets) :
                LookupAsset(assetId, assetVersion, buildAssets);

            if (assetRecord == null)
            {
                string versionMsg = string.IsNullOrEmpty(assetVersion) ? string.Empty : $"and version {assetVersion}";
                Log.LogError($"Asset with Id {assetId} {versionMsg} isn't registered on the BAR Build with ID {BARBuildId}");
                return false;
            }

            return NewAssetLocations.TryAdd((assetRecord.Id, feedConfig.SafeTargetURL, assetLocationType), ValueTuple.Create());
        }

        /// <summary>
        ///   Persist in BAR all pending associations of Asset -> AssetLocation stored in `NewAssetLocations`.
        /// </summary>
        /// <param name="client">Maestro++ API client</param>
        protected async Task PersistPendingAssetLocationAsync(IMaestroApi client)
        {
            Log.LogMessage(MessageImportance.High, "\nPersisting new locations of assets in the Build Asset Registry.");

            var updates = NewAssetLocations.Keys.Select(nal => new AssetAndLocation(nal.AssetId, (LocationType)nal.LocationType)
            {
                Location = nal.AssetLocation
            }).ToImmutableList();

            await client.Assets.BulkAddLocationsAsync(updates);

            Log.LogMessage(MessageImportance.High, "\nCompleted persisting of new asset locations...");
        }

        /// <summary>
        /// Protect against accidental publishing of internal assets to non-internal feeds.
        /// </summary>
        protected void CheckForInternalBuildsOnPublicFeeds(TargetFeedConfig feedConfig)
        {
            // If separated out for clarity.
            if (!SkipSafetyChecks)
            {
                if (InternalBuild && !feedConfig.Internal)
                {
                    Log.LogError($"Use of non-internal feed '{feedConfig.TargetURL}' is invalid for an internal build. This can be overridden with '{nameof(SkipSafetyChecks)}= true'");
                }
            }
        }

        /// <summary>
        ///  Run a check to verify that stable assets are not published to
        ///  locations they should not be published.
        ///  
        /// This is only done for packages since feeds are
        /// immutable.
        /// </summary>
        public void CheckForStableAssetsInNonIsolatedFeeds()
        {
            if (BuildModel.Identity.IsReleaseOnlyPackageVersion || SkipSafetyChecks)
            {
                return;
            }

            foreach (var packagesPerCategory in PackagesByCategory)
            {
                var category = packagesPerCategory.Key;
                var packages = packagesPerCategory.Value;

                if (FeedConfigs.TryGetValue(category, out HashSet<TargetFeedConfig> feedConfigsForCategory))
                {
                    foreach (var feedConfig in feedConfigsForCategory)
                    {
                        // Look at the version numbers. If any of the packages here are stable and about to be published to a
                        // non-isolated feed, then issue an error. Isolated feeds may recieve all packages.
                        if (feedConfig.Isolated)
                        {
                            continue;
                        }

                        HashSet<PackageArtifactModel> filteredPackages = FilterPackages(packages, feedConfig);

                        foreach (var package in filteredPackages)
                        {
                            // Special case. Four part versions should publish to non-isolated feeds
                            if (FourPartVersionRegex.IsMatch(package.Version))
                            {
                                continue;
                            }
                            if (!NuGetVersion.TryParse(package.Version, out NuGetVersion version))
                            {
                                Log.LogError($"Package '{package.Id}' has invalid version '{package.Version}'");
                            }
                            // We want to avoid pushing non-final bits with final version numbers to feeds that are in general
                            // use by the public. This is for technical (can't overwrite the original packages) reasons as well as 
                            // to avoid confusion. Because .NET core generally brands its "final" bits without prerelease version
                            // suffixes (e.g. 3.0.0-preview1), test to see whether a prerelease suffix exists.
                            else if (!version.IsPrerelease)
                            {
                                Log.LogError($"Package '{package.Id}' has stable version '{package.Version}' but is targeted at a non-isolated feed '{feedConfig.TargetURL}'");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Publishes files to symbol server(s) one by one using Azure api to download files
        /// </summary>
        /// <param name="pdbArtifactsBasePath">Path to dll and pdb files</param>
        /// <param name="msdlToken">Token to authenticate msdl</param>
        /// <param name="symWebToken">Token to authenticate symweb</param>
        /// <param name="symbolPublishingExclusionsFile">Right now we do not add any files to this, so this is going to be null</param>
        /// <param name="publishSpecialClrFiles">If true, the special coreclr module indexed files like DBI, DAC and SOS are published</param>
        /// <param name="clientThrottle">To avoid starting too many processes</param>
        /// <returns>Task</returns>
        public async Task PublishSymbolsUsingStreamingAsync(
            string pdbArtifactsBasePath,
            string msdlToken,
            string symWebToken,
            string symbolPublishingExclusionsFile,
            bool publishSpecialClrFiles,
            ReadOnlyDictionary<string, Asset> buildAssets,
            SemaphoreSlim clientThrottle)
        {
            bool failed = false;
            Log.LogMessage(MessageImportance.High,
                $"Performing symbol publishing... \nExpirationInDays : {ExpirationInDays} \nConvertPortablePdbsToWindowsPdb : false \ndryRun: false ");
            var symbolCategory = TargetFeedContentType.Symbols;

            using HttpClient httpClient = CreateAzdoClient(AzureDevOpsOrg, false, AzureProject);
            string containerId = await GetContainerIdAsync(httpClient, ArtifactName.BlobArtifacts);

            HashSet<string> symbolsToPublish = new HashSet<string>();

            //Get all the symbol file names
            foreach (var asset in buildAssets)
            {
                var name = asset.Key;
                if (GeneralUtils.IsSymbolPackage(name))
                {
                    symbolsToPublish.Add(Path.GetFileName(name));
                }
            }

            Log.LogMessage(MessageImportance.High, $"Total number of symbol files : {symbolsToPublish.Count}");

            HashSet<TargetFeedConfig> feedConfigsForSymbols = FeedConfigs[symbolCategory];
            Dictionary<string, string> serversToPublish =
                GetTargetSymbolServers(feedConfigsForSymbols, msdlToken, symWebToken);
            HashSet<string> excludeFiles = null;

            if (File.Exists(symbolPublishingExclusionsFile))
            {
                Log.LogMessage(MessageImportance.Normal, $"SymbolPublishingExclusionFile exists");
                string[] files = File.ReadAllLines(symbolPublishingExclusionsFile);
                excludeFiles = new HashSet<string>();

                foreach (var file in files)
                {
                    if (!string.IsNullOrEmpty(file))
                    {
                        Log.LogMessage(MessageImportance.Normal, $"Exclude the file {file} from publishing to symbol server");
                        excludeFiles.Add(file);
                    }
                }
            }
            else
            {
                Log.LogMessage(MessageImportance.Normal, $"SymbolPublishingExclusionFile was not found at ${symbolPublishingExclusionsFile} ");
            }

            if (symbolsToPublish != null && symbolsToPublish.Any())
            {
                using (HttpClient client = CreateAzdoClient(AzureDevOpsOrg, true))
                {
                    client.Timeout = TimeSpan.FromSeconds(TimeoutInSeconds);
                    await Task.WhenAll(symbolsToPublish.Select(async symbol =>
                    {
                        try
                        {
                            await clientThrottle.WaitAsync();
                            string temporarySymbolsDirectory = CreateTemporaryDirectory();
                            string localSymbolPath = Path.Combine(temporarySymbolsDirectory, symbol);
                            Log.LogMessage(MessageImportance.High, $"Downloading symbol : {symbol} to {localSymbolPath}");

                            Stopwatch gatherDownloadTime = Stopwatch.StartNew();
                            await DownloadFileAsync(
                                client,
                                ArtifactName.BlobArtifacts,
                                containerId,
                                symbol,
                                localSymbolPath);

                            gatherDownloadTime.Stop();
                            Log.LogMessage(MessageImportance.High, $"Time taken to download file to '{localSymbolPath}' is {gatherDownloadTime.ElapsedMilliseconds / 1000.0} (seconds)");
                            Log.LogMessage(MessageImportance.High, $"Successfully downloaded symbol : {symbol} to {localSymbolPath}");

                            List<string> symbolFiles = new List<string>();
                            symbolFiles.Add(localSymbolPath);

                            foreach (var server in serversToPublish)
                            {
                                var serverPath = server.Key;
                                var token = server.Value;
                                Log.LogMessage(MessageImportance.High, $"Publishing symbol file {symbol} to {serverPath}:");
                                Stopwatch gatherSymbolPublishingTime = Stopwatch.StartNew();

                                try
                                {
                                    // ConvertPortablePdbsToWindowsPdbs is always set to false, 
                                    // because this is an expensive task so this is done in the staging pipeline post signing.
                                    await PublishSymbolsHelper.PublishAsync(
                                        log: Log,
                                        symbolServerPath: serverPath,
                                        personalAccessToken: token,
                                        inputPackages: symbolFiles,
                                        inputFiles: null,
                                        packageExcludeFiles: excludeFiles,
                                        expirationInDays: ExpirationInDays,
                                        convertPortablePdbsToWindowsPdbs: false,
                                        publishSpecialClrFiles: publishSpecialClrFiles,
                                        pdbConversionTreatAsWarning: null,
                                        treatPdbConversionIssuesAsInfo: false,
                                        dryRun: false,
                                        timer: false,
                                        verboseLogging: true);
                                }
                                catch (Exception ex)
                                {
                                    failed = true;
                                    Log.LogError(ex.Message);
                                }

                                gatherSymbolPublishingTime.Stop();
                                Log.LogMessage(MessageImportance.High,
                                    $"Symbol publishing for {symbol} took {gatherSymbolPublishingTime.ElapsedMilliseconds / 1000.0} (seconds)");
                            }

                            if (failed)
                            {
                                return;
                            }

                            DeleteTemporaryDirectory(temporarySymbolsDirectory);
                        }
                        finally
                        {
                            clientThrottle.Release();
                        }
                    }));

                    Log.LogMessage(MessageImportance.High, "Successfully published to symbol servers.");
                }
            }
            else
            {
                Log.LogMessage(MessageImportance.High, $"No symbol files to upload.");
            }

            // publishing pdb artifacts 
            IEnumerable<string> filesToSymbolServer = null;
            if (Directory.Exists(pdbArtifactsBasePath))
            {
                var pdbEntries = System.IO.Directory.EnumerateFiles(
                    pdbArtifactsBasePath,
                    "*.pdb",
                    System.IO.SearchOption.AllDirectories);
                var dllEntries = System.IO.Directory.EnumerateFiles(
                    pdbArtifactsBasePath,
                    "*.dll",
                    System.IO.SearchOption.AllDirectories);
                filesToSymbolServer = pdbEntries.Concat(dllEntries);
            }

            if (filesToSymbolServer != null && filesToSymbolServer.Any())
            {
                foreach (var server in serversToPublish)
                {
                    var serverPath = server.Key;
                    var token = server.Value;
                    Log.LogMessage(MessageImportance.High, $"Publishing pdbFiles to {serverPath}:");

                    try
                    {
                        // ConvertPortablePdbsToWindowsPdbs is always set to false,
                        // because this is an expensive task so this is done in the staging pipeline post signing.
                        await PublishSymbolsHelper.PublishAsync(
                            log: Log,
                            symbolServerPath: serverPath,
                            personalAccessToken: token,
                            inputPackages: null,
                            inputFiles: filesToSymbolServer,
                            packageExcludeFiles: excludeFiles,
                            expirationInDays: ExpirationInDays,
                            convertPortablePdbsToWindowsPdbs: false,
                            publishSpecialClrFiles: publishSpecialClrFiles,
                            pdbConversionTreatAsWarning: null,
                            treatPdbConversionIssuesAsInfo: false,
                            dryRun: false,
                            timer: false,
                            verboseLogging: false);
                    }
                    catch (Exception ex)
                    {
                        Log.LogError(ex.Message);
                    }
                }

                Log.LogMessage(MessageImportance.High, $"Successfully published pdb files");
            }

            else
            {
                Log.LogMessage(MessageImportance.Low, $"No pdb files to upload to symbol server.");
            }
        }

        /// <summary>
        /// Decides how to publish the symbol, dll and pdb files
        /// </summary>
        /// <param name="pdbArtifactsBasePath">Path to dll and pdb files</param>
        /// <param name="msdlToken">Token to authenticate msdl</param>
        /// <param name="symWebToken">Token to authenticate symweb</param>
        /// <param name="symbolPublishingExclusionsFile">Right now we do not add any files to this, so this is going to be null</param>
        /// <param name="temporarySymbolsLocation">Path to Symbol.nupkgs</param>
        /// <param name="clientThrottle">To avoid starting too many processes</param>
        /// <param name="publishSpecialClrFiles">If true, the special coreclr module indexed files like DBI, DAC and SOS are published</param>
        public async Task HandleSymbolPublishingAsync(
            string pdbArtifactsBasePath,
            string msdlToken,
            string symWebToken,
            string symbolPublishingExclusionsFile,
            bool publishSpecialClrFiles,
            ReadOnlyDictionary<string, Asset> buildAssets,
            SemaphoreSlim clientThrottle = null,
            string temporarySymbolsLocation = null)
        {
            if (UseStreamingPublishing)
            {
                await PublishSymbolsUsingStreamingAsync(
                    pdbArtifactsBasePath,
                    msdlToken,
                    symWebToken,
                    symbolPublishingExclusionsFile,
                    publishSpecialClrFiles,
                    buildAssets,
                    clientThrottle);
            }
            else
            {
                await PublishSymbolsfromBlobArtifactsAsync(
                    pdbArtifactsBasePath,
                    msdlToken,
                    symWebToken,
                    symbolPublishingExclusionsFile,
                    publishSpecialClrFiles,
                    temporarySymbolsLocation);
            }
        }

        /// <summary>
        /// Publishes symbol, dll and pdb files to symbol server.
        /// </summary>
        /// <param name="pdbArtifactsBasePath">Path to dll and pdb files</param>
        /// <param name="msdlToken">Token to authenticate msdl</param>
        /// <param name="symWebToken">Token to authenticate symweb</param>
        /// <param name="symbolPublishingExclusionsFile">Right now we do not add any files to this, so this is going to be null</param>
        /// <param name="temporarySymbolsLocation">Path to Symbol.nupkgs</param>
        /// <param name="publishSpecialClrFiles">If true, the special coreclr module indexed files like DBI, DAC and SOS are published</param>
        public async Task PublishSymbolsfromBlobArtifactsAsync(
            string pdbArtifactsBasePath,
            string msdlToken,
            string symWebToken,
            string symbolPublishingExclusionsFile,
            bool publishSpecialClrFiles,
            string temporarySymbolsLocation = null)
        {
            if (Directory.Exists(temporarySymbolsLocation))
            {
                Log.LogMessage(MessageImportance.High, "Publishing Symbols to Symbol server: ");
                string[] fileEntries = Directory.GetFiles(temporarySymbolsLocation);

                var category = TargetFeedContentType.Symbols;

                HashSet<TargetFeedConfig> feedConfigsForSymbols = FeedConfigs[category];

                Dictionary<string, string> serversToPublish =
                    GetTargetSymbolServers(feedConfigsForSymbols, msdlToken, symWebToken);

                IEnumerable<string> filesToSymbolServer = null;

                if (Directory.Exists(pdbArtifactsBasePath))
                {
                    var pdbEntries = System.IO.Directory.EnumerateFiles(
                        pdbArtifactsBasePath,
                        "*.pdb",
                        System.IO.SearchOption.AllDirectories);
                    var dllEntries = System.IO.Directory.EnumerateFiles(
                        pdbArtifactsBasePath,
                        "*.dll",
                        System.IO.SearchOption.AllDirectories);
                    filesToSymbolServer = pdbEntries.Concat(dllEntries);
                }

                foreach (var server in serversToPublish)
                {
                    var serverPath = server.Key;
                    var token = server.Value; ;
                    Log.LogMessage(MessageImportance.High,
                        $"Performing symbol publishing...\nSymbolServerPath : ${serverPath} \nExpirationInDays : {ExpirationInDays} \nConvertPortablePdbsToWindowsPdb : false \ndryRun: false \nTotal number of symbol files : {fileEntries.Length} ");

                    try
                    {
                        // ConvertPortablePdbsToWindowsPdbs is always set to false,
                        // because this is an expensive task so this is done in the staging pipeline post signing.
                        await PublishSymbolsHelper.PublishAsync(
                            log: Log,
                            symbolServerPath: serverPath,
                            personalAccessToken: token,
                            inputPackages: fileEntries,
                            inputFiles: filesToSymbolServer,
                            packageExcludeFiles: null,
                            expirationInDays: ExpirationInDays,
                            convertPortablePdbsToWindowsPdbs: false,
                            publishSpecialClrFiles: publishSpecialClrFiles,
                            pdbConversionTreatAsWarning: null,
                            treatPdbConversionIssuesAsInfo: false,
                            dryRun: false,
                            timer: false,
                            verboseLogging: false);
                    }
                    catch (Exception ex)
                    {
                        Log.LogError(ex.Message);
                    }

                    Log.LogMessage(MessageImportance.High, $"Successfully published to ${serverPath}.");
                }
            }
            else
            {
                Log.LogError("Temporary symbols directory does not exists.");
            }
        }

        /// <summary>
        /// Get the Symbol Server to publish
        /// </summary>
        /// <param name="feedConfigsForSymbols"></param>
        /// <param name="msdlToken"></param>
        /// <param name="symWebToken"></param>
        /// <returns>A map of symbol server path => token to the symbol server</returns>
        public Dictionary<string, string> GetTargetSymbolServers(HashSet<TargetFeedConfig> feedConfigsForSymbols, string msdlToken, string symWebToken)
        {
            Dictionary<string, string> serversToPublish = new Dictionary<string, string>();
            if (feedConfigsForSymbols.Any(x => (x.SymbolTargetType & SymbolTargetType.Msdl) != SymbolTargetType.None))
            {
                serversToPublish.Add(MsdlServerPath, msdlToken);
            }
            if (feedConfigsForSymbols.Any(x => (x.SymbolTargetType & SymbolTargetType.SymWeb) != SymbolTargetType.None))
            {
                serversToPublish.Add(SymwebServerPath, symWebToken);
            }
            return serversToPublish;
        }

        /// <summary>
        ///     Handle package publishing for all the feed configs.
        /// </summary>
        /// <param name="client">Maestro API client</param>
        /// <param name="buildAssets">Assets information about build being published.</param>
        /// <param name="clientThrottle">To avoid starting too many processes</param>
        /// <returns>Task</returns>
        protected async Task HandlePackagePublishingAsync(ReadOnlyDictionary<string, Asset> buildAssets, SemaphoreSlim clientThrottle = null)
        {
            List<Task> publishTasks = new List<Task>();

            // Just log a empty line for better visualization of the logs
            Log.LogMessage(MessageImportance.High, "\nBegin publishing of packages: ");

            foreach (var packagesPerCategory in PackagesByCategory)
            {
                var category = packagesPerCategory.Key;
                var packages = packagesPerCategory.Value;

                if (FeedConfigs.TryGetValue(category, out HashSet<TargetFeedConfig> feedConfigsForCategory))
                {
                    foreach (var feedConfig in feedConfigsForCategory)
                    {
                        HashSet<PackageArtifactModel> filteredPackages = FilterPackages(packages, feedConfig);

                        foreach (var package in filteredPackages)
                        {
                            string isolatedString = feedConfig.Isolated ? "Isolated" : "Non-Isolated";
                            string internalString = feedConfig.Internal ? $", Internal" : ", Public";
                            string shippingString = package.NonShipping ? "NonShipping" : "Shipping";
                            Log.LogMessage(MessageImportance.High,
                                $"Package {package.Id}@{package.Version} ({shippingString}) should go to {feedConfig.TargetURL} ({isolatedString}{internalString})");
                        }

                        switch (feedConfig.Type)
                        {
                            case FeedType.AzDoNugetFeed:
                                publishTasks.Add(
                                    PublishPackagesToAzDoNugetFeedAsync(
                                        filteredPackages,
                                        buildAssets,
                                        feedConfig,
                                        clientThrottle));
                                break;
                            default:
                                Log.LogError(
                                    $"Unknown target feed type for category '{category}': '{feedConfig.Type}'.");
                                break;
                        }
                    }
                }
                else
                {
                    Log.LogError($"No target feed configuration found for artifact category: '{category}'.");
                }
            }

            await Task.WhenAll(publishTasks);

            Log.LogMessage(MessageImportance.High, "\nCompleted publishing of packages: ");
        }

        private HashSet<PackageArtifactModel> FilterPackages(HashSet<PackageArtifactModel> packages, TargetFeedConfig feedConfig)
        {
            return feedConfig.AssetSelection switch
            {
                AssetSelection.All => packages,
                AssetSelection.NonShippingOnly => packages.Where(p => p.NonShipping).ToHashSet(),
                AssetSelection.ShippingOnly => packages.Where(p => !p.NonShipping).ToHashSet(),

                // Throw NIE here instead of logging an error because error would have already been logged in the
                // parser for the user.
                _ => throw new NotImplementedException($"Unknown asset selection type '{feedConfig.AssetSelection}'")
            };
        }

        /// <summary>
        /// Creates Azdo client
        /// </summary>
        /// <param name="accountName">Name of the org</param>
        /// <param name="setAutomaticDecompression">If client is used for downloading the artifact then AutomaticDecompression has to be set, else it is not required</param>
        /// <param name="projectName">Azure devOps project name</param>
        /// <param name="versionOverride">Default version is 6.0</param>
        /// <returns>HttpClient</returns>
        public HttpClient CreateAzdoClient(
            string accountName,
            bool setAutomaticDecompression,
            string projectName = null,
            string versionOverride = null)
        {
            HttpClientHandler handler = new HttpClientHandler { CheckCertificateRevocationList = true };
            if (setAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            string address = $"https://dev.azure.com/{accountName}/";

            if (!string.IsNullOrEmpty(projectName))
            {
                address += $"{projectName}/";
            }

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(address)
            };
            client.Timeout = TimeSpan.FromSeconds(TimeoutInSeconds);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "",
                    AzdoApiToken))));
            client.DefaultRequestHeaders.Add(
                "Accept",
                $"application/xml;api-version={versionOverride ?? AzureDevOpsFeedsApiVersion}");
            return client;
        }

        /// <summary>
        /// Gets the container Id, that is going to be used in another API call to download the assets
        /// ContainerId is the same for PackageArtifacts and BlobArtifacts
        /// </summary>
        /// <param name="artifactName">If it is PackageArtifacts or BlobArtifacts</param>
        /// <returns>ContainerId</returns>
        public async Task<string> GetContainerIdAsync(HttpClient client, ArtifactName artifactName)
        {
            string uri =
                 $"{AzureDevOpsBaseUrl}/{AzureDevOpsOrg}/{AzureProject}/_apis/build/builds/{BuildId}/artifacts?api-version={AzureDevOpsFeedsApiVersion}";
            Exception mostRecentlyCaughtException = null;
            string containerId = "";
            bool success = await RetryHandler.RunAsync(async attempt =>
            {
                try
                {
                    CancellationTokenSource timeoutTokenSource =
                        new CancellationTokenSource(TimeSpan.FromMinutes(TimeoutInMinutes));

                    using HttpRequestMessage getMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                    using HttpResponseMessage response = await client.GetAsync(uri, timeoutTokenSource.Token);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    BuildArtifacts buildArtifacts = JsonConvert.DeserializeObject<BuildArtifacts>(responseBody);

                    foreach (var artifact in buildArtifacts.value)
                    {
                        ArtifactName name;
                        if (Enum.TryParse<ArtifactName>(artifact.name, out name) && name == artifactName)
                        {
                            string[] segment = artifact.resource.data.Split('/');
                            containerId = segment[1];
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception toStore) when (toStore is HttpRequestException || toStore is TaskCanceledException)
                {
                    mostRecentlyCaughtException = toStore;
                    return false;
                }
            }).ConfigureAwait(false);

            if (string.IsNullOrEmpty(containerId))
            {
                Log.LogError("Container Id does not exists");
            }

            if (!success)
            {
                throw new Exception(
                    $"Failed to get container id after {RetryHandler.MaxAttempts} attempts.  See inner exception for details, {mostRecentlyCaughtException}");
            }

            return containerId;
        }

        /// <summary>
        /// Download artifact file using Azure API
        /// </summary>
        /// <param name="client">Azdo client</param>
        /// <param name="artifactName">If it is PackageArtifacts or BlobArtifacts</param>
        /// <param name="containerId">ContainerId where the packageArtifact and BlobArtifacts are stored</param>
        /// <param name="fileName">Name the file we are trying to download</param>
        /// <param name="path">Path where the file is being downloaded</param>
        public async Task DownloadFileAsync(
            HttpClient client,
            ArtifactName artifactName,
            string containerId,
            string fileName,
            string path)
        {
            string uri =
                $"{AzureDevOpsBaseUrl}/{AzureDevOpsOrg}/_apis/resources/Containers/{containerId}?itemPath=/{artifactName}/{fileName}&isShallow=true&api-version={AzureApiVersionForFileDownload}";
            Exception mostRecentlyCaughtException = null;
            bool success = await RetryHandler.RunAsync(async attempt =>
            {
                try
                {
                    Log.LogMessage(MessageImportance.Low, $"Download file uri = {uri}");

                    CancellationTokenSource timeoutTokenSource =
                        new CancellationTokenSource(TimeSpan.FromMinutes(TimeoutInMinutes));

                    using HttpRequestMessage getMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                    using HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, timeoutTokenSource.Token);

                    response.EnsureSuccessStatusCode();

                    using var fs = new FileStream(
                        path,
                        FileMode.Create,
                        FileAccess.ReadWrite,
                        FileShare.ReadWrite);
                    using var stream = await response.Content.ReadAsStreamAsync(timeoutTokenSource.Token);

                    try
                    {
                        await stream.CopyToAsync(fs, timeoutTokenSource.Token);
                    }
                    finally
                    {
                        stream.Close();
                        fs.Close();
                    }

                    return true;
                }
                catch (Exception toStore)
                {
                    mostRecentlyCaughtException = toStore;
                    return false;
                }
            }).ConfigureAwait(false);

            if (!success)
            {
                throw new Exception(
                    $"Failed to download local file '{path}' after {RetryHandler.MaxAttempts} attempts.  See inner exception for details.", mostRecentlyCaughtException);
            }
        }

        protected async Task HandleBlobPublishingAsync(ReadOnlyDictionary<string, Asset> buildAssets, SemaphoreSlim clientThrottle = null)
        {
            List<Task> publishTasks = new List<Task>();

            // Just log a empty line for better visualization of the logs
            Log.LogMessage(MessageImportance.High, "\nBegin publishing of blobs: ");

            foreach (var blobsPerCategory in BlobsByCategory)
            {
                var category = blobsPerCategory.Key;
                var blobs = blobsPerCategory.Value;

                if (FeedConfigs.TryGetValue(category, out HashSet<TargetFeedConfig> feedConfigsForCategory))
                {
                    foreach (var feedConfig in feedConfigsForCategory)
                    {
                        HashSet<BlobArtifactModel> filteredBlobs = FilterBlobs(blobs, feedConfig);

                        foreach (var blob in filteredBlobs)
                        {
                            string isolatedString = feedConfig.Isolated ? "Isolated" : "Non-Isolated";
                            string internalString = feedConfig.Internal ? ", Internal" : ", Public";
                            string shippingString = blob.NonShipping ? "NonShipping" : "Shipping";
                            Log.LogMessage(MessageImportance.High,
                                $"Blob {blob.Id} ({shippingString}) should go to {feedConfig.SafeTargetURL} ({isolatedString}{internalString})");
                        }

                        var assetsToPublish = new HashSet<string>(filteredBlobs.Select(b => b.Id));
                        var publisher = AssetPublisherFactory.CreateAssetPublisher(feedConfig, this);
                        publishTasks.Add(
                            PublishAssetsAsync(
                                publisher,
                                assetsToPublish,
                                buildAssets,
                                feedConfig,
                                clientThrottle));
                    }
                }
                else
                {
                    Log.LogError($"No target feed configuration found for artifact category: '{category}'.");
                }
            }

            await Task.WhenAll(publishTasks);

            Log.LogMessage(MessageImportance.High, "\nCompleted publishing of blobs: ");
        }

        /// <summary>
        ///     Filter the blobs by the feed config information
        /// </summary>
        /// <param name="blobs"></param>
        /// <param name="feedConfig"></param>
        /// <returns></returns>
        private HashSet<BlobArtifactModel> FilterBlobs(HashSet<BlobArtifactModel> blobs, TargetFeedConfig feedConfig)
        {
            return feedConfig.AssetSelection switch
            {
                AssetSelection.All => blobs,
                AssetSelection.NonShippingOnly => blobs.Where(p => p.NonShipping).ToHashSet(),
                AssetSelection.ShippingOnly => blobs.Where(p => !p.NonShipping).ToHashSet(),

                // Throw NYI here instead of logging an error because error would have already been logged in the
                // parser for the user.
                _ => throw new NotImplementedException("Unknown asset selection type '{feedConfig.AssetSelection}'")
            };
        }

        /// <summary>
        ///     Split the artifacts into categories.
        ///     
        ///     Categories are either specified explicitly when publishing (with the asset attribute "Category", separated by ';'),
        ///     or they are inferred based on the extension of the asset.
        /// </summary>
        /// <param name="buildModel"></param>
        public void SplitArtifactsInCategories(BuildModel buildModel)
        {
            foreach (var packageAsset in buildModel.Artifacts.Packages)
            {
                string categories = string.Empty;

                if (!packageAsset.Attributes.TryGetValue("Category", out categories))
                {
                    // Package artifacts don't have extensions. They are always nupkgs.
                    // Set the category explicitly to "PACKAGE"
                    categories = GeneralUtils.PackagesCategory;
                }

                foreach (var category in categories.Split(';'))
                {
                    if (!Enum.TryParse(category, ignoreCase: true, out TargetFeedContentType categoryKey))
                    {
                        Log.LogError($"Invalid target feed config category '{category}'.");
                        continue;
                    }

                    if (PackagesByCategory.ContainsKey(categoryKey))
                    {
                        PackagesByCategory[categoryKey].Add(packageAsset);
                    }
                    else
                    {
                        PackagesByCategory[categoryKey] = new HashSet<PackageArtifactModel>() { packageAsset };
                    }
                }
            }

            foreach (var blobAsset in buildModel.Artifacts.Blobs)
            {
                string categories = string.Empty;

                if (!blobAsset.Attributes.TryGetValue("Category", out categories) || string.Equals(categories, "NONE", StringComparison.OrdinalIgnoreCase))
                {
                    categories = GeneralUtils.InferCategory(blobAsset.Id, Log);
                }

                foreach (var category in categories.Split(';'))
                {
                    if (!Enum.TryParse(category, ignoreCase: true, out TargetFeedContentType categoryKey))
                    {
                        Log.LogError($"Invalid target feed config category '{category}'.");
                        continue;
                    }

                    if (BlobsByCategory.ContainsKey(categoryKey))
                    {
                        BlobsByCategory[categoryKey].Add(blobAsset);
                    }
                    else
                    {
                        BlobsByCategory[categoryKey] = new HashSet<BlobArtifactModel>() { blobAsset };
                    }
                }
            }
        }

        private async Task PublishPackagesFromPackageArtifactsToAzDoNugetFeedAsync(
            HashSet<PackageArtifactModel> packagesToPublish,
            ReadOnlyDictionary<string, Asset> buildAssets,
            TargetFeedConfig feedConfig)
        {
            await PushNugetPackagesAsync(packagesToPublish, feedConfig, maxClients: MaxClients,
                async (feed, httpClient, package, feedAccount, feedVisibility, feedName) =>
                {
                    string localPackagePath =
                        Path.Combine(PackageAssetsBasePath, $"{package.Id}.{package.Version}.nupkg");
                    if (!File.Exists(localPackagePath))
                    {
                        Log.LogError($"Could not locate '{package.Id}.{package.Version}' at '{localPackagePath}'");
                        return;
                    }

                    TryAddAssetLocation(
                        package.Id,
                        package.Version,
                        buildAssets,
                        feedConfig,
                        LocationType.NugetFeed);

                    await PushNugetPackageAsync(
                        feed,
                        httpClient,
                        localPackagePath,
                        package.Id,
                        package.Version,
                        feedAccount,
                        feedVisibility,
                        feedName);
                });
        }

        private async Task PublishPackagesUsingStreamingToAzdoNugetAsync(
            HashSet<PackageArtifactModel> packagesToPublish,
            ReadOnlyDictionary<string, Asset> buildAssets,
            TargetFeedConfig feedConfig,
            SemaphoreSlim clientThrottle)
        {
            bool failed = false;
            using HttpClient httpClient = CreateAzdoClient(AzureDevOpsOrg, false, AzureProject);
            string containerId = await GetContainerIdAsync(httpClient, ArtifactName.PackageArtifacts);

            using HttpClient client = CreateAzdoClient(AzureDevOpsOrg, true);

            await Task.WhenAll(packagesToPublish.Select(async package =>
            {
                try
                {
                    await clientThrottle.WaitAsync();
                    var packageFilename = $"{package.Id}.{package.Version}.nupkg";
                    string temporaryPackageDirectory =
                        Path.GetFullPath(Path.Combine(ArtifactsBasePath, Guid.NewGuid().ToString()));
                    EnsureTemporaryDirectoryExists(temporaryPackageDirectory);
                    string localPackagePath = Path.Combine(temporaryPackageDirectory, packageFilename);
                    Log.LogMessage(MessageImportance.Low,
                        $"Downloading package : {packageFilename} to {localPackagePath}");

                    Stopwatch gatherPackageDownloadTime = Stopwatch.StartNew();
                    await DownloadFileAsync(
                        client,
                        ArtifactName.PackageArtifacts,
                        containerId,
                        packageFilename,
                        localPackagePath);

                    if (!File.Exists(localPackagePath))
                    {
                        failed = true;
                        Log.LogError(
                            $"Could not locate '{package.Id}.{package.Version}' at '{localPackagePath}'");
                        return;
                    }

                    gatherPackageDownloadTime.Stop();

                    if (failed)
                    {
                        return;
                    }

                    Log.LogMessage(MessageImportance.Low, $"Time taken to download file to '{localPackagePath}' is {gatherPackageDownloadTime.ElapsedMilliseconds / 1000.0} (seconds)");
                    Log.LogMessage(MessageImportance.Low,
                        $"Successfully downloaded package : {packageFilename} to {localPackagePath}");

                    TryAddAssetLocation(
                        package.Id,
                        package.Version,
                        buildAssets,
                        feedConfig,
                        LocationType.NugetFeed);

                    using HttpClient httpClient = new HttpClient(new HttpClientHandler
                    {
                        CheckCertificateRevocationList = true
                    });

                    httpClient.Timeout = TimeSpan.FromSeconds(TimeoutInSeconds);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(
                            Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", feedConfig.Token))));

                    Stopwatch gatherPackagePublishingTime = Stopwatch.StartNew();
                    await PushPackageToNugetFeed(httpClient, feedConfig, localPackagePath, package.Id, package.Version);
                    gatherPackagePublishingTime.Stop();
                    Log.LogMessage(MessageImportance.Low, $"Publishing package {localPackagePath} took {gatherPackagePublishingTime.ElapsedMilliseconds / 1000.0} (seconds)");

                    DeleteTemporaryDirectory(localPackagePath);
                }
                finally
                {
                    clientThrottle.Release();
                }
            }));
        }

        private async Task PublishPackagesToAzDoNugetFeedAsync(
            HashSet<PackageArtifactModel> packagesToPublish,
            ReadOnlyDictionary<string, Asset> buildAssets,
            TargetFeedConfig feedConfig,
            SemaphoreSlim clientThrottle)
        {
            if (UseStreamingPublishing)
            {
                await PublishPackagesUsingStreamingToAzdoNugetAsync(packagesToPublish, buildAssets, feedConfig, clientThrottle);
            }
            else
            {
                await PublishPackagesFromPackageArtifactsToAzDoNugetFeedAsync(packagesToPublish, buildAssets, feedConfig);
            }
        }

        /// <summary>
        ///     Push nuget packages to the azure devops feed.
        /// </summary>
        /// <param name="packagesToPublish">List of packages to publish</param>
        /// <param name="feedConfig">Information about feed to publish to</param>
        public async Task PushNugetPackagesAsync<T>(
            HashSet<T> packagesToPublish,
            TargetFeedConfig feedConfig,
            int maxClients,
            Func<TargetFeedConfig, HttpClient, T, string, string, string, Task> packagePublishAction)
        {
            if (!packagesToPublish.Any())
            {
                return;
            }

            var parsedUri = Regex.Match(feedConfig.TargetURL, PublishingConstants.AzDoNuGetFeedPattern);
            if (!parsedUri.Success)
            {
                Log.LogError(
                    $"Azure DevOps NuGetFeed was not in the expected format '{PublishingConstants.AzDoNuGetFeedPattern}'");
                return;
            }

            string feedAccount = parsedUri.Groups["account"].Value;
            string feedVisibility = parsedUri.Groups["visibility"].Value;
            string feedName = parsedUri.Groups["feed"].Value;

            using var clientThrottle = new SemaphoreSlim(maxClients, maxClients);

            using (HttpClient httpClient = new HttpClient(new HttpClientHandler
            { CheckCertificateRevocationList = true }))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(TimeoutInSeconds);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", feedConfig.Token))));

                await Task.WhenAll(packagesToPublish.Select(async packageToPublish =>
                {
                    try
                    {
                        // Wait to avoid starting too many processes.
                        await clientThrottle.WaitAsync();
                        await packagePublishAction(
                            feedConfig,
                            httpClient,
                            packageToPublish,
                            feedAccount,
                            feedVisibility,
                            feedName);
                    }
                    finally
                    {
                        clientThrottle.Release();
                    }
                }));
            }
        }

        /// <summary>
        ///     Push a single package to an Azure DevOps nuget feed.
        /// </summary>
        /// <param name="feedConfig">Infos about the target feed</param>
        /// <param name="packageToPublish">Package to push</param>
        /// <returns>Task</returns>
        /// <remarks>
        ///     This method attempts to take the most efficient path to push the package.
        ///     
        ///     There are four cases:
        ///         - Package does not exist on the target feed, and is pushed normally
        ///         - Package exists with bitwise-identical contents
        ///         - Package exists with non-matching contents
        ///         - Azure DevOps is having some issue meaning we didn't succeed to publish at first.
        ///         
        ///     The second case is by far the most common. So, we first attempt to push the 
        ///     package normally using nuget.exe. If this fails, this could mean any number of 
        ///     things (like failed auth). But in normal circumstances, this might mean the 
        ///     package already exists. This either means that we are attempting to push the 
        ///     same package, or attemtping to push a different package with the same id and 
        ///     version. The second case is an error, as Azure DevOps feeds are immutable, 
        ///     the former is simply a case where we should continue onward.
        ///     
        ///     To handle the third case we rely on the call to compare file contents 
        ///     `CompareLocalPackageToFeedPackage` to return PackageFeedStatus.DoesNotExist,
        ///     meaning that it got a 404 when looking up the file in the feed - to trigger 
        ///     a retry on the publish operation. This was implemented this way because we 
        ///     didn't want to rely on parsing the output of the push operation - which does 
        ///     a call to `nuget.exe` behind the scenes.
        /// </remarks>
        public async Task PushNugetPackageAsync(
            TargetFeedConfig feedConfig,
            HttpClient client,
            string localPackageLocation,
            string id,
            string version,
            string feedAccount,
            string feedVisibility,
            string feedName,
            Func<string, string, HttpClient, MsBuildUtils.TaskLoggingHelper, Task<PackageFeedStatus>> CompareLocalPackageToFeedPackageCallBack = null,
            Func<string, string, Task<ProcessExecutionResult>> RunProcessAndGetOutputsCallBack = null
            )
        {
            // Using these callbacks we can mock up functionality when testing.
            CompareLocalPackageToFeedPackageCallBack ??= CompareLocalPackageToFeedPackage;
            RunProcessAndGetOutputsCallBack ??= GeneralUtils.RunProcessAndGetOutputsAsync;
            ProcessExecutionResult nugetResult = null;
            var packageStatus = PackageFeedStatus.Unknown;

            try
            {
                Log.LogMessage(MessageImportance.Normal, $"Pushing local package {localPackageLocation} to target feed {feedConfig.TargetURL}");
                int attemptIndex = 0;

                do
                {
                    attemptIndex++;
                    // The feed key when pushing to AzDo feeds is "AzureDevOps" (works with the credential helper).
                    nugetResult = await RunProcessAndGetOutputsCallBack(NugetPath, $"push \"{localPackageLocation}\" -Source \"{feedConfig.TargetURL}\" -NonInteractive -ApiKey AzureDevOps -Verbosity quiet");

                    if (nugetResult.ExitCode == 0)
                    {
                        // We have just pushed this package so we know it exists and is identical to our local copy
                        packageStatus = PackageFeedStatus.ExistsAndIdenticalToLocal;
                        break;
                    }

                    Log.LogMessage(MessageImportance.Low, $"Attempt # {attemptIndex} failed to push {localPackageLocation}, attempting to determine whether the package already existed on the feed with the same content. Nuget exit code = {nugetResult.ExitCode}");

                    string packageContentUrl = $"https://pkgs.dev.azure.com/{feedAccount}/{feedVisibility}_apis/packaging/feeds/{feedName}/nuget/packages/{id}/versions/{version}/content";
                    packageStatus = await CompareLocalPackageToFeedPackageCallBack(localPackageLocation, packageContentUrl, client, Log);

                    switch (packageStatus)
                    {
                        case PackageFeedStatus.ExistsAndIdenticalToLocal:
                            {
                                Log.LogMessage(MessageImportance.Normal, $"Package '{localPackageLocation}' already exists on '{feedConfig.TargetURL}' but has the same content; skipping push");
                                break;
                            }
                        case PackageFeedStatus.ExistsAndDifferent:
                            {
                                Log.LogError($"Package '{localPackageLocation}' already exists on '{feedConfig.TargetURL}' with different content.");
                                break;
                            }
                        default:
                            {
                                // For either case (unknown exception or 404, we will retry the push and check again.  Linearly increase back-off time on each retry.
                                Log.LogMessage(MessageImportance.Low, $"Hit error checking package status after failed push: '{packageStatus}'. Will retry after {RetryDelayMilliseconds * attemptIndex} ms.");
                                await Task.Delay(RetryDelayMilliseconds * attemptIndex).ConfigureAwait(false);
                                break;
                            }
                    }
                }
                while (packageStatus != PackageFeedStatus.ExistsAndIdenticalToLocal && // Success
                       packageStatus != PackageFeedStatus.ExistsAndDifferent &&        // Give up: Non-retriable error
                       attemptIndex <= MaxRetryCount);                                              // Give up: Too many retries

                if (packageStatus != PackageFeedStatus.ExistsAndIdenticalToLocal)
                {
                    Log.LogError($"Failed to publish package '{id}@{version}' to '{feedConfig.TargetURL}' after {MaxRetryCount} attempts. (Final status: {packageStatus})");
                }
                else
                {
                    Log.LogMessage($"Succeeded publishing package '{localPackageLocation}' to feed {feedConfig.TargetURL}");
                }
            }
            catch (Exception e)
            {
                Log.LogError($"Unexpected exception pushing package '{id}@{version}': {e.Message}");
            }

            if (packageStatus != PackageFeedStatus.ExistsAndIdenticalToLocal && nugetResult?.ExitCode != 0)
            {
                Log.LogError($"Output from nuget.exe: {Environment.NewLine}StdOut:{Environment.NewLine}{nugetResult.StandardOut}{Environment.NewLine}StdErr:{Environment.NewLine}{nugetResult.StandardError}");
            }
        }

        /// <summary>
        /// Creates a temporary directory
        /// </summary>
        /// <returns>Path to the directory created</returns>
        public string CreateTemporaryDirectory()
        {
            string temporaryDirectory =
                Path.GetFullPath(Path.Combine(ArtifactsBasePath, Guid.NewGuid().ToString()));
            EnsureTemporaryDirectoryExists(temporaryDirectory);
            return temporaryDirectory;
        }

        private async Task PublishAssetsAsync(IAssetPublisher assetPublisher, HashSet<string> assetsToPublish,
            ReadOnlyDictionary<string, Asset> buildAssets,
            TargetFeedConfig feedConfig,
            SemaphoreSlim clientThrottle)
        {
            if (UseStreamingPublishing)
            {
                await PublishAssetsUsingStreamingPublishingAsync(assetPublisher, assetsToPublish, buildAssets, feedConfig, clientThrottle);
            }
            else
            {
                await PublishAssetsWithoutStreamingPublishingAsync(assetPublisher, assetsToPublish, buildAssets, feedConfig);
            }

            if (feedConfig.Type == FeedType.AzureStorageContainer && 
                feedConfig.LatestLinkShortUrlPrefixes.Any())
            {

                if (LinkManager == null)
                {
                    // If there is a client cert supplied, use that.
                    // Otherwise, use the client secret.
                    if (AkaMSClientCertificate != null)
                    {
                        LinkManager = new LatestLinksManager(
                            AkaMSClientId,
                            AkaMSClientCertificate,
                            AkaMSTenant,
                            AkaMSGroupOwner,
                            AkaMSCreatedBy,
                            AkaMsOwners,
                            Log);
                    }
                    else if (!string.IsNullOrEmpty(AkaMSClientSecret))
                    {
                        LinkManager = new LatestLinksManager(
                            AkaMSClientId,
                            AkaMSClientSecret,
                            AkaMSTenant,
                            AkaMSGroupOwner,
                            AkaMSCreatedBy,
                            AkaMsOwners,
                            Log);
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot create latest links for feed config without aka.ms authentication information");
                    }
                }

                // The latest links should be updated only after the publishing is complete, to avoid
                // dead links in the interim.
                await LinkManager.CreateOrUpdateLatestLinksAsync(
                    assetsToPublish,
                    feedConfig,
                    feedConfig.Type == FeedType.AzureStorageContainer
                        ? 0
                        : PublishingConstants.ExpectedFeedUrlSuffix.Length);
            }
        }

        private async Task PublishAssetsUsingStreamingPublishingAsync(
            IAssetPublisher assetPublisher,
            HashSet<string> assetsToPublish,
            ReadOnlyDictionary<string, Asset> buildAssets,
            TargetFeedConfig feedConfig,
            SemaphoreSlim clientThrottle)
        {
            bool failed = false;
            using HttpClient httpClient = CreateAzdoClient(AzureDevOpsOrg, false, AzureProject);
            string containerId = await GetContainerIdAsync(httpClient, ArtifactName.BlobArtifacts);

            var pushOptions = new PushOptions
            {
                AllowOverwrite = feedConfig.AllowOverwrite,
                PassIfExistingItemIdentical = true,
            };
            using HttpClient client = CreateAzdoClient(AzureDevOpsOrg, true, AzureProject);

            await Task.WhenAll(assetsToPublish.Select(async asset =>
            {
                using (await SemaphoreLock.LockAsync(clientThrottle))
                {
                    string temporaryBlobDirectory = CreateTemporaryDirectory();
                    var fileName = Path.GetFileName(asset);
                    var localBlobPath = Path.Combine(temporaryBlobDirectory, fileName);
                    Log.LogMessage(MessageImportance.Low, $"Downloading blob : {fileName} to {localBlobPath}");

                    Stopwatch gatherBlobDownloadTime = Stopwatch.StartNew();
                    await DownloadFileAsync(
                        client,
                        ArtifactName.BlobArtifacts,
                        containerId,
                        fileName,
                        localBlobPath);
                    gatherBlobDownloadTime.Stop();
                    Log.LogMessage(MessageImportance.Low, $"Time taken to download file to '{localBlobPath}' is {gatherBlobDownloadTime.ElapsedMilliseconds / 1000.0} (seconds)");

                    if (!File.Exists(localBlobPath))
                    {
                        failed = true;
                        Log.LogError($"Could not locate '{asset} at '{localBlobPath}'");
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.Low,
                            $"Successfully downloaded blob : {fileName} to {localBlobPath}");

                        TryAddAssetLocation(
                            asset,
                            assetVersion: null,
                            buildAssets,
                            feedConfig,
                            assetPublisher.LocationType);

                        Stopwatch gatherBlobPublishingTime = Stopwatch.StartNew();
                        await assetPublisher.PublishAssetAsync(localBlobPath, asset, pushOptions, null);
                        gatherBlobPublishingTime.Stop();
                        Log.LogMessage(MessageImportance.Low, $"Publishing {localBlobPath} completed in {gatherBlobPublishingTime.ElapsedMilliseconds / 1000.0} (seconds)");
                    }

                    if (failed)
                    {
                        return;
                    }

                    DeleteTemporaryDirectory(temporaryBlobDirectory);
                }
            }));
        }

        private async Task PublishAssetsWithoutStreamingPublishingAsync(
            IAssetPublisher assetPublisher,
            HashSet<string> assetsToPublish,
            ReadOnlyDictionary<string, Asset> buildAssets,
            TargetFeedConfig feedConfig)
        {
            bool failed = false;
            var assets = assetsToPublish
                .Select(asset =>
                {
                    var fileName = Path.GetFileName(asset);
                    var localBlobPath = Path.Combine(BlobAssetsBasePath, fileName);

                    if (!File.Exists(localBlobPath))
                    {
                        failed = true;
                        Log.LogError($"Could not locate '{asset} at '{localBlobPath}'");
                    }

                    return (localBlobPath, id: asset);
                })
                .ToArray();

            if (failed)
            {
                return;
            }

            var pushOptions = new PushOptions
            {
                AllowOverwrite = feedConfig.AllowOverwrite,
                PassIfExistingItemIdentical = true
            };

            foreach (var asset in assetsToPublish)
            {
                TryAddAssetLocation(
                    asset,
                    assetVersion: null,
                    buildAssets,
                    feedConfig,
                    LocationType.Container);
            }

            using var clientThrottle = new SemaphoreSlim(MaxClients, MaxClients);
            await Task.WhenAll(assets.Select(asset =>
                assetPublisher.PublishAssetAsync(asset.localBlobPath, asset.id, pushOptions, clientThrottle)));
        }

        private async Task PushPackageToNugetFeed(
            HttpClient httpClient,
            TargetFeedConfig feedConfig,
            string localPackagePath,
            string id,
            string version)
        {
            var parsedUri = Regex.Match(feedConfig.TargetURL, PublishingConstants.AzDoNuGetFeedPattern);

            if (!parsedUri.Success)
            {
                Log.LogError(
                    $"Azure DevOps NuGetFeed was not in the expected format '{PublishingConstants.AzDoNuGetFeedPattern}'");
                return;
            }

            string feedAccount = parsedUri.Groups["account"].Value;
            string feedVisibility = parsedUri.Groups["visibility"].Value;
            string feedName = parsedUri.Groups["feed"].Value;

            await PushNugetPackageAsync(
                feedConfig,
                httpClient,
                localPackagePath,
                id,
                version,
                feedAccount,
                feedVisibility,
                feedName);
        }

        /// <summary>
        /// Create Temporary directory if it does not exists.
        /// </summary>
        /// <param name="temporaryLocation"></param>
        public void EnsureTemporaryDirectoryExists(string temporaryLocation)
        {
            if (!Directory.Exists(temporaryLocation))
            {
                Directory.CreateDirectory(temporaryLocation);
            }
        }

        /// <summary>
        /// Delete the files after publishing, this is part of cleanup
        /// </summary>
        /// <param name="temporaryLocation"></param>
        public void DeleteTemporaryFiles(string temporaryLocation)
        {
            try
            {
                if (Directory.Exists(temporaryLocation))
                {
                    string[] fileEntries = Directory.GetFiles(temporaryLocation);
                    foreach (var file in fileEntries)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogWarning(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the temporary folder, this is part of clean up
        /// </summary>
        /// <param name="temporaryLocation"></param>
        public void DeleteTemporaryDirectory(string temporaryLocation)
        {
            var attempts = 0;
            if (Directory.Exists(temporaryLocation))
            {
                do
                {
                    try
                    {
                        attempts++;
                        Log.LogMessage(MessageImportance.Low, $"Deleting directory : {temporaryLocation}");
                        Directory.Delete(temporaryLocation, true);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (attempts == MaxRetryCount)
                            Log.LogMessage(MessageImportance.Low, $"Unable to delete the directory because of {ex.Message} after {attempts} attempts.");
                    }
                    Log.LogMessage(MessageImportance.Low, $"Retrying to delete {temporaryLocation}, attempt number {attempts}");
                    Task.Delay(RetryDelayMilliseconds).Wait();
                }
                while (true);
            }
        }

        protected bool AnyMissingRequiredProperty()
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                RequiredAttribute attribute =
                    (RequiredAttribute)Attribute.GetCustomAttribute(prop, typeof(RequiredAttribute));

                if (attribute != null)
                {
                    string value = prop.GetValue(this, null)?.ToString();

                    if (string.IsNullOrEmpty(value))
                    {
                        Log.LogError($"The property {prop.Name} is required but doesn't have a value set.");
                    }
                }
            }

            AnyMissingRequiredBaseProperties();

            return Log.HasLoggedErrors;
        }

        protected bool AnyMissingRequiredBaseProperties()
        {
            if (string.IsNullOrEmpty(BlobAssetsBasePath))
            {
                Log.LogError($"The property {nameof(BlobAssetsBasePath)} is required but doesn't have a value set.");
            }

            if (string.IsNullOrEmpty(PackageAssetsBasePath))
            {
                Log.LogError($"The property {nameof(PackageAssetsBasePath)} is required but doesn't have a value set.");
            }

            if (BARBuildId == 0)
            {
                Log.LogError($"The property {nameof(BARBuildId)} is required but doesn't have a value set.");
            }

            if (string.IsNullOrEmpty(MaestroApiEndpoint))
            {
                Log.LogError($"The property {nameof(MaestroApiEndpoint)} is required but doesn't have a value set.");
            }

            if (UseStreamingPublishing && string.IsNullOrEmpty(AzdoApiToken))
            {
                Log.LogError($"The property {nameof(AzdoApiToken)} is required when using streaming publishing, but doesn't have a value set.");
            }

            if (UseStreamingPublishing && string.IsNullOrEmpty(ArtifactsBasePath))
            {
                Log.LogError($"The property {nameof(ArtifactsBasePath)} is required when using streaming publishing, but doesn't have a value set.");
            }
            return Log.HasLoggedErrors;
        }
    }
}
#else
    public abstract class PublishArtifactsInManifestBase : Microsoft.Build.Utilities.Task
    {
        public override bool Execute() => throw new NotSupportedException("PublishArtifactsInManifestBase depends on Maestro.Client, which has discontinued support for desktop frameworks.");

        public abstract Task<bool> ExecuteAsync();

        public Task PushNugetPackagesAsync<T>(
            HashSet<T> packagesToPublish,
            TargetFeedConfig feedConfig,
            int maxClients,
            Func<TargetFeedConfig, HttpClient, T, string, string, string, Task> packagePublishAction)  
            => throw new NotSupportedException("PublishArtifactsInManifestBase depends on Maestro.Client, which has discontinued support for desktop frameworks.");

        public Task PushNugetPackageAsync(
            TargetFeedConfig feedConfig,
            HttpClient client,
            string localPackageLocation,
            string id,
            string version,
            string feedAccount,
            string feedVisibility,
            string feedName,
            Func<string, string, HttpClient, MsBuildUtils.TaskLoggingHelper, Task<PackageFeedStatus>> CompareLocalPackageToFeedPackageCallBack = null,
            Func<string, string, Task<ProcessExecutionResult>> RunProcessAndGetOutputsCallBack = null
            ) => throw new NotSupportedException("PublishArtifactsInManifestBase depends on Maestro.Client, which has discontinued support for desktop frameworks.");
    }
}
#endif

