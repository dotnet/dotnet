// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
#if !NET472_OR_GREATER
using Microsoft.DotNet.Maestro.Client;
using Microsoft.DotNet.Maestro.Client.Models;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class PublishArtifactsInManifestV3 : PublishArtifactsInManifestBase
    {
        /// <summary>
        /// Comma separated list of Maestro++ Channel IDs to which the build should
        /// be assigned to once the assets are published.
        /// </summary>
        [Required]
        public string TargetChannels { get; set; }

        public bool PublishInstallersAndChecksums { get; set; }

        public string PdbArtifactsBasePath { get; set; }

        public string MsdlToken { get; set; }

        public string SymWebToken { get; set; }

        public string SymbolPublishingExclusionsFile { get; set; }

        public bool PublishSpecialClrFiles { get; set; }

        public bool AllowFeedOverrides { get; set; }

        public ITaskItem[] FeedKeys { get; set; }
        public ITaskItem[] FeedSasUris { get; set; }

        public ITaskItem[] FeedOverrides { get; set; }

        public override bool Execute()
        {
            ExecuteAsync().GetAwaiter().GetResult();
            return !Log.HasLoggedErrors;
        }

        public override async Task<bool> ExecuteAsync()
        {
            if (AnyMissingRequiredProperty())
            {
                Log.LogError("Missing required properties. Aborting execution.");
                return false;
            }

            try
            {
                List<int> targetChannelsIds = new List<int>();

                foreach (var channelIdStr in TargetChannels.Split('-'))
                {
                    if (!int.TryParse(channelIdStr, out var channelId))
                    {
                        Log.LogError(
                            $"Value '{channelIdStr}' isn't recognized as a valid Maestro++ channel ID. To add a channel refer to https://github.com/dotnet/arcade/blob/master/Documentation/CorePackages/Publishing.md#how-to-add-a-new-channel-to-use-v3-publishing.");
                        continue;
                    }

                    targetChannelsIds.Add(channelId);
                }

                if (Log.HasLoggedErrors)
                {
                    Log.LogError(
                        $"Could not parse the target channels list '{TargetChannels}'. It should be a comma separated list of integers.");
                    return false;
                }

                SplitArtifactsInCategories(BuildModel);

                if (Log.HasLoggedErrors)
                {
                    return false;
                }

                // Fetch Maestro record of the build. We're going to use it to get the BAR ID
                // of the assets being published so we can add a new location for them.
                IMaestroApi client = MaestroApiFactory.GetAuthenticated(
                    MaestroApiEndpoint,
                    BuildAssetRegistryToken,
                    MaestroApiFederatedToken,
                    MaestroManagedIdentityId,
                    disableInteractiveAuth: !AllowInteractiveAuthentication);

                Maestro.Client.Models.Build buildInformation = await client.Builds.GetBuildAsync(BARBuildId);
                ReadOnlyDictionary<string, Asset> buildAssets = CreateBuildAssetDictionary(buildInformation);

                if (Log.HasLoggedErrors)
                {
                    return false;
                }

                foreach (var targetChannelId in targetChannelsIds.Distinct())
                {
                    TargetChannelConfig targetChannelConfig = PublishingConstants.ChannelInfos
                        .Where(ci =>
                            ci.Id == targetChannelId &&
                            ci.PublishingInfraVersion == PublishingInfraVersion.Latest)
                        .FirstOrDefault();

                    // Invalid channel ID was supplied
                    if (targetChannelConfig.Equals(default(TargetChannelConfig)))
                    {
                        Log.LogError($"Channel with ID '{targetChannelId}' is not configured to be published to.");
                        return false;
                    }

                    if (await client.Channels.GetChannelAsync(targetChannelId) == null)
                    {
                        Log.LogError($"Channel with ID '{targetChannelId}' does not exist in BAR.");
                        return false;
                    }

                    Log.LogMessage(MessageImportance.High, $"Publishing to this target channel: {targetChannelConfig}");

                    List<string> shortLinkUrls = new List<string>();

                    foreach (string akaMSChannelName in targetChannelConfig.AkaMSChannelNames)
                    {
                        shortLinkUrls.Add($"dotnet/{akaMSChannelName}/{BuildQuality}");
                    }

                    // If there are no channel names, default to dotnet/
                    if (!targetChannelConfig.AkaMSChannelNames.Any())
                    {
                        shortLinkUrls.Add("dotnet/");
                    }

                    var targetFeedsSetup = new SetupTargetFeedConfigV3(
                        targetChannelConfig: targetChannelConfig,
                        isInternalBuild: targetChannelConfig.IsInternal,
                        isStableBuild: BuildModel.Identity.IsStable,
                        repositoryName: BuildModel.Identity.Name,
                        commitSha: BuildModel.Identity.Commit,
                        publishInstallersAndChecksums: PublishInstallersAndChecksums,
                        feedKeys: FeedKeys,
                        feedSasUris: FeedSasUris,
                        feedOverrides: AllowFeedOverrides ? FeedOverrides : Array.Empty<ITaskItem>(),
                        latestLinkShortUrlPrefixes: shortLinkUrls,
                        buildEngine: BuildEngine,
                        targetChannelConfig.SymbolTargetType,
                        filesToExclude: targetChannelConfig.FilenamesToExclude,
                        flatten: targetChannelConfig.Flatten,
                        log: Log);

                    var targetFeedConfigs = targetFeedsSetup.Setup();

                    // No target feeds to publish to, very likely this is an error
                    if (!targetFeedConfigs.Any())
                    {
                        Log.LogError($"No target feeds were found to publish the assets to.");
                        return false;
                    }

                    foreach (var feedConfig in targetFeedConfigs)
                    {
                        Log.LogMessage(MessageImportance.High, $"Target feed config: {feedConfig}");

                        TargetFeedContentType categoryKey = feedConfig.ContentType;
                        if (!FeedConfigs.TryGetValue(categoryKey, out _))
                        {
                            FeedConfigs[categoryKey] = new HashSet<TargetFeedConfig>();
                        }

                        FeedConfigs[categoryKey].Add(feedConfig);
                    }
                }

                CheckForStableAssetsInNonIsolatedFeeds();

                if (Log.HasLoggedErrors)
                {
                    return false;
                }

                string temporarySymbolsLocation = "";
                if (!UseStreamingPublishing)
                {
                    temporarySymbolsLocation =
                        Path.GetFullPath(Path.Combine(BlobAssetsBasePath, @"..\", "tempSymbols"));

                    EnsureTemporaryDirectoryExists(temporarySymbolsLocation);
                    DeleteTemporaryFiles(temporarySymbolsLocation);

                    // Copying symbol files to temporary location is required because the symUploader API needs read/write access to the files,
                    // since we publish blobs and symbols in parallel this will cause IO exceptions.
                    CopySymbolFilesToTemporaryLocation(BuildModel, temporarySymbolsLocation);
                }

                using var clientThrottle = new SemaphoreSlim(MaxClients, MaxClients);

                await Task.WhenAll(new Task[]
                {
                    HandlePackagePublishingAsync(buildAssets, clientThrottle),
                    HandleBlobPublishingAsync(buildAssets, clientThrottle),
                    HandleSymbolPublishingAsync(
                        PdbArtifactsBasePath,
                        MsdlToken,
                        SymWebToken,
                        SymbolPublishingExclusionsFile,
                        PublishSpecialClrFiles,
                        buildAssets,
                        clientThrottle,
                        temporarySymbolsLocation)
                });

                DeleteTemporaryFiles(temporarySymbolsLocation);
                DeleteTemporaryDirectory(temporarySymbolsLocation);

                await PersistPendingAssetLocationAsync(client);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            if (!Log.HasLoggedErrors)
            {
                Log.LogMessage(MessageImportance.High, "Publishing finished with success.");
            }

            return !Log.HasLoggedErrors;
        }


        /// <summary>
        /// Copying symbol files to temporary location.
        /// </summary>
        /// <param name="buildModel"></param>
        /// <param name="symbolTemporaryLocation"></param>
        private void CopySymbolFilesToTemporaryLocation(BuildModel buildModel, string symbolTemporaryLocation)
        {
            foreach (var blobAsset in buildModel.Artifacts.Blobs)
            {
                if (GeneralUtils.IsSymbolPackage(blobAsset.Id))
                {
                    var sourceFile = Path.Combine(BlobAssetsBasePath, Path.GetFileName(blobAsset.Id));
                    var destinationFile = Path.Combine(symbolTemporaryLocation, Path.GetFileName(blobAsset.Id));
                    File.Copy(sourceFile, destinationFile);
                    Log.LogMessage(MessageImportance.Low,
                        $"Successfully copied file {sourceFile} to {destinationFile}.");
                }
            }
        }

        public string GetFeed(string feed, string feedOverride)
        {
            return (AllowFeedOverrides && !string.IsNullOrEmpty(feedOverride)) ? feedOverride : feed;
        }

        public PublishArtifactsInManifestV3(AssetPublisherFactory assetPublisherFactory = null) : base(assetPublisherFactory)
        {
        }
    }
}
#else
public class PublishArtifactsInManifestV3 : Microsoft.Build.Utilities.Task
{
    public override bool Execute() => throw new NotSupportedException("PublishArtifactsInManifestV3 depends on Maestro.Client, which has discontinued support for desktop frameworks.");
}
#endif
