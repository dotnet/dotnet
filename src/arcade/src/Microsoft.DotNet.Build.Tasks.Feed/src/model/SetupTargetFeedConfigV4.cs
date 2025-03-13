// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class SetupTargetFeedConfigV4 : SetupTargetFeedConfigBase
    {
        private readonly TargetChannelConfig _targetChannelConfig;

        private IBuildEngine BuildEngine { get; }

        private string StablePackagesFeed { get; set; }

        private string StableSymbolsFeed { get; set; }

        private SymbolPublishVisibility SymbolServerVisibility { get; }

        private bool Flatten { get; }

        public TaskLoggingHelper Log { get; }

        public string AzureDevOpsOrg => "dnceng";

        private readonly BuildModel _buildModel;

        public SetupTargetFeedConfigV4(
            TargetChannelConfig targetChannelConfig,
            BuildModel buildModel,
            bool isInternalBuild,
            string repositoryName,
            string commitSha,
            ITaskItem[] feedKeys,
            ITaskItem[] feedSasUris,
            ITaskItem[] feedOverrides,
            ImmutableList<string> latestLinkShortUrlPrefixes,
            IBuildEngine buildEngine,
            SymbolPublishVisibility symbolPublishVisibility,
            string stablePackagesFeed = null,
            string stableSymbolsFeed = null,
            ImmutableList<string> filesToExclude = null,
            bool flatten = true,
            TaskLoggingHelper log = null)
            : base(isInternalBuild: isInternalBuild,
                   repositoryName: repositoryName,
                   commitSha: commitSha,
                   publishInstallersAndChecksums: true,
                   installersTargetStaticFeed: null,
                   installersAzureAccountKey: null,
                   checksumsTargetStaticFeed: null,
                   checksumsAzureAccountKey: null,
                   azureDevOpsStaticShippingFeed: null,
                   azureDevOpsStaticTransportFeed: null,
                   azureDevOpsStaticSymbolsFeed: null,
                   latestLinkShortUrlPrefixes: latestLinkShortUrlPrefixes,
                   azureDevOpsFeedsKey: null)
        {
            _targetChannelConfig = targetChannelConfig;
            _buildModel = buildModel;
            BuildEngine = buildEngine;
            StableSymbolsFeed = stableSymbolsFeed;
            StablePackagesFeed = stablePackagesFeed;
            SymbolServerVisibility = symbolPublishVisibility;
            Flatten = flatten;
            FeedKeys = feedKeys.ToImmutableDictionary(i => i.ItemSpec, i => i.GetMetadata("Key"));
            FeedSasUris = feedSasUris.ToImmutableDictionary(i => i.ItemSpec, i => ConvertFromBase64(i.GetMetadata("Base64Uri")));
            FeedOverrides = feedOverrides.ToImmutableDictionary(i => i.ItemSpec, i => i.GetMetadata("Replacement"));
            AzureDevOpsFeedsKey = FeedKeys.TryGetValue("https://pkgs.dev.azure.com/dnceng", out string key) ? key : null;
            Log = log;
        }

        private static string ConvertFromBase64(string value)
        {
            if (value == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public ImmutableDictionary<string, string> FeedOverrides { get; set; }

        public ImmutableDictionary<string, string> FeedSasUris { get; set; }

        public ImmutableDictionary<string, string> FeedKeys { get; set; }

        public override List<TargetFeedConfig> Setup()
        {
            return Feeds().Distinct().ToList();
        }

        private IEnumerable<TargetFeedConfig> Feeds()
        {
            // We create stable feeds if any of the package assets within the build
            // could be stable. In V4, build stability is ignored.

            bool generateStableFeeds = _buildModel.Artifacts.Packages.Any(p => p.CouldBeStable.HasValue && p.CouldBeStable.Value == true);

            if (generateStableFeeds)
            {
                CreateStablePackagesFeedIfNeeded();

                yield return new TargetFeedConfig(
                    TargetFeedContentType.Package,
                    StablePackagesFeed,
                    FeedType.AzDoNugetFeed,
                    AzureDevOpsFeedsKey,
                    LatestLinkShortUrlPrefixes,
                    _targetChannelConfig.AkaMSCreateLinkPatterns,
                    _targetChannelConfig.AkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.CouldBeStable,
                    symbolPublishVisibility: SymbolServerVisibility,
                    isolated: true,
                    @internal: IsInternalBuild,
                    flatten: Flatten);
            }

            foreach (var spec in _targetChannelConfig.TargetFeeds)
            {
                foreach (var type in spec.ContentTypes)
                {
                    var oldFeed = spec.FeedUrl;
                    var feed = GetFeedOverride(oldFeed);
                    if (type is TargetFeedContentType.Package &&
                        spec.Assets == AssetSelection.NonShippingOnly &&
                        FeedOverrides.TryGetValue("transport-packages", out string newFeed))
                    {
                        feed = newFeed;
                    }
                    else if (type is TargetFeedContentType.Package &&
                        spec.Assets == AssetSelection.ShippingOnly &&
                        FeedOverrides.TryGetValue("shipping-packages", out newFeed))
                    {
                        feed = newFeed;
                    }
                    var key = GetFeedKey(feed);
                    var sasUri = GetFeedSasUri(feed);

                    var feedType = feed.StartsWith("https://pkgs.dev.azure.com")
                        ? FeedType.AzDoNugetFeed : FeedType.AzureStorageContainer;

                    // If no SAS is specified, then the default azure credential will be used.
                    if (feedType == FeedType.AzDoNugetFeed && string.IsNullOrEmpty(key))
                    {
                        Log?.LogError($"No key found for {feed}, unable to publish to it.");
                        continue;
                    }

                    yield return new TargetFeedConfig(
                        type,
                        feed,
                        feedType,
                        sasUri ?? key,
                        LatestLinkShortUrlPrefixes,
                        _targetChannelConfig.AkaMSCreateLinkPatterns,
                        _targetChannelConfig.AkaMSDoNotCreateLinkPatterns,
                        spec.Assets,
                        false,
                        IsInternalBuild,
                        false,
                        SymbolServerVisibility,
                        flatten: Flatten
                    );
                }
            }
        }

        /// <summary>
        /// Create the stable packages feed if one is not already explicitly provided
        /// </summary>
        /// <exception cref="Exception">Throws if the feed cannot be created</exception>
        private void CreateStablePackagesFeedIfNeeded()
        {
            if (string.IsNullOrEmpty(StablePackagesFeed))
            {
                var packagesFeedTask = new CreateAzureDevOpsFeed()
                {
                    BuildEngine = BuildEngine,
                    AzureDevOpsOrg = AzureDevOpsOrg,
                    AzureDevOpsProject = IsInternalBuild ? "internal" : "public",
                    AzureDevOpsPersonalAccessToken = AzureDevOpsFeedsKey,
                    RepositoryName = RepositoryName,
                    CommitSha = CommitSha
                };

                if (!packagesFeedTask.Execute())
                {
                    throw new Exception($"Problems creating an AzureDevOps feed for repository '{RepositoryName}' and commit '{CommitSha}'.");
                }

                StablePackagesFeed = packagesFeedTask.TargetFeedURL;
            }
        }

        private string GetFeedOverride(string feed)
        {
            foreach (var prefix in FeedOverrides.Keys.OrderByDescending(f => f.Length))
            {
                if (feed.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return FeedOverrides[prefix];
                }
            }

            return feed;
        }

        private string GetFeedSasUri(string feed)
        {
            foreach (var prefix in FeedSasUris.Keys.OrderByDescending(f => f.Length))
            {
                if (feed.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return FeedSasUris[prefix];
                }
            }

            return null;
        }

        private string GetFeedKey(string feed)
        {
            foreach (var prefix in FeedKeys.Keys.OrderByDescending(f => f.Length))
            {
                if (feed.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return FeedKeys[prefix];
                }
            }

            return null;
        }
    }
}
