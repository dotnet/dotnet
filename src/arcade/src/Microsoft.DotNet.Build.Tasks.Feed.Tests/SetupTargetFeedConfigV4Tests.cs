// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using FluentAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;
using Xunit.Abstractions;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class SetupTargetFeedConfigV4Tests
    {
        private const string LatestLinkShortUrlPrefix = "LatestLinkShortUrlPrefix";
        private const string BuildQuality = "quality";
        private const string AzureDevOpsFeedsKey = "AzureDevOpsFeedsKey";

        private const string StablePackageFeed = "StablePackageFeed";
        private const string StableSymbolsFeed = "StableSymbolsFeed";

        private const string ChecksumsTargetStaticFeed = "ChecksumsTargetStaticFeed";
        private const string ChecksumsTargetStaticFeedKey = "ChecksumsTargetStaticFeedKey";

        private const string InstallersTargetStaticFeed = "InstallersTargetStaticFeed";
        private const string InstallersTargetStaticFeedKey = "InstallersTargetStaticFeedKey";

        private const string AzureDevOpsStaticShippingFeed = "AzureDevOpsStaticShippingFeed";
        private const string AzureDevOpsStaticTransportFeed = "AzureDevOpsStaticTransportFeed";
        private const string AzureDevOpsStaticSymbolsFeed = "AzureDevOpsStaticSymbolsFeed";

        private static ImmutableList<string> FilesToExclude = ImmutableList.Create(
            "filename",
            "secondfilename"
        );

        private readonly List<TargetFeedContentType> InstallersAndSymbols = new List<TargetFeedContentType>() {
            TargetFeedContentType.OSX,
            TargetFeedContentType.Deb,
            TargetFeedContentType.Rpm,
            TargetFeedContentType.Node,
            TargetFeedContentType.BinaryLayout,
            TargetFeedContentType.Installer,
            TargetFeedContentType.Maven,
            TargetFeedContentType.VSIX,
            TargetFeedContentType.Badge,
            TargetFeedContentType.Symbols,
            TargetFeedContentType.Other
        };

        private readonly ITaskItem[] FeedKeys = GetFeedKeys();

        private static ITaskItem[] GetFeedKeys()
        {
            var installersKey = new TaskItem(PublishingConstants.FeedStagingForInstallers);
            installersKey.SetMetadata("Key", InstallersTargetStaticFeedKey);
            var checksumsKey = new TaskItem(PublishingConstants.FeedStagingForChecksums);
            checksumsKey.SetMetadata("Key", ChecksumsTargetStaticFeedKey);
            var azureDevops = new TaskItem("https://pkgs.dev.azure.com/dnceng");
            azureDevops.SetMetadata("Key", AzureDevOpsFeedsKey);
            return new ITaskItem[]
            {
                installersKey,
                checksumsKey,
                azureDevops,
            };
        }

        private const SymbolPublishVisibility symbolVisibility = SymbolPublishVisibility.Internal;

        private readonly ITestOutputHelper Output;

        public SetupTargetFeedConfigV4Tests(ITestOutputHelper output)
        {
            this.Output = output;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void StableFeeds(bool isInternalBuild)
        {
            BuildModel buildModel = new BuildModel(new BuildIdentity());
            buildModel.Artifacts.Packages.Add(new PackageArtifactModel() { CouldBeStable = true });

            var expectedFeeds = new List<TargetFeedConfig>();

            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Package,
                    StablePackageFeed,
                    FeedType.AzDoNugetFeed,
                    AzureDevOpsFeedsKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                    akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.CouldBeStable,
                    isolated: true,
                    @internal: isInternalBuild,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));

            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Package,
                    PublishingConstants.FeedDotNetEng,
                    FeedType.AzDoNugetFeed,
                    AzureDevOpsFeedsKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                    akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.ShippingOnly,
                    isolated: false,
                    @internal: isInternalBuild,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));

            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Package,
                    PublishingConstants.FeedDotNetEng,
                    FeedType.AzDoNugetFeed,
                    AzureDevOpsFeedsKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                    akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.NonShippingOnly,
                    isolated: false,
                    @internal: isInternalBuild,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));

            foreach (var contentType in InstallersAndSymbols)
            {
                expectedFeeds.Add(
                    new TargetFeedConfig(
                        contentType,
                        PublishingConstants.FeedStagingForInstallers,
                        FeedType.AzureStorageContainer,
                        InstallersTargetStaticFeedKey,
                        latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                        akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                        akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                        assetSelection: AssetSelection.All,
                        isolated: false,
                        @internal: isInternalBuild,
                        allowOverwrite: false,
                        symbolPublishVisibility: symbolVisibility));
            }
            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Checksum,
                    PublishingConstants.FeedStagingForChecksums,
                    FeedType.AzureStorageContainer,
                    ChecksumsTargetStaticFeedKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                    akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.All,
                    isolated: false,
                    @internal: isInternalBuild,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));


            var buildEngine = new MockBuildEngine();
            var channelConfig = PublishingConstants.ChannelInfos.First(c => c.Id == 2);
            var config = new SetupTargetFeedConfigV4(
                    channelConfig,
                    buildModel,
                    isInternalBuild,
                    repositoryName: "test-repo",
                    commitSha: "c0c0c0c0",
                    FeedKeys,
                    Array.Empty<ITaskItem>(),
                    Array.Empty<ITaskItem>(),
                    [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    buildEngine,
                    symbolVisibility,
                    StablePackageFeed,
                    StableSymbolsFeed,
                    filesToExclude: FilesToExclude
                );

            var actualFeeds = config.Setup();

            actualFeeds.Should().BeEquivalentTo(expectedFeeds);
        }

        [Fact]
        public void NonStableAndInternal()
        {
            BuildModel buildModel = new BuildModel(new BuildIdentity());

            var expectedFeeds = new List<TargetFeedConfig>();

            expectedFeeds.Add(new TargetFeedConfig(
                TargetFeedContentType.Package,
                PublishingConstants.FeedDotNetEng,
                FeedType.AzDoNugetFeed,
                AzureDevOpsFeedsKey,
                latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                assetSelection: AssetSelection.ShippingOnly,
                isolated: false,
                @internal: true,
                allowOverwrite: false,
                symbolPublishVisibility: symbolVisibility));

            expectedFeeds.Add(new TargetFeedConfig(
                TargetFeedContentType.Package,
                PublishingConstants.FeedDotNetEng,
                FeedType.AzDoNugetFeed,
                AzureDevOpsFeedsKey,
                latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                assetSelection: AssetSelection.NonShippingOnly,
                isolated: false,
                @internal: true,
                allowOverwrite: false,
                symbolPublishVisibility: symbolVisibility));

            foreach (var contentType in InstallersAndSymbols)
            {
                expectedFeeds.Add(
                    new TargetFeedConfig(
                        contentType,
                        PublishingConstants.FeedStagingForInstallers,
                        FeedType.AzureStorageContainer,
                        InstallersTargetStaticFeedKey,
                        latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                        akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                        akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                        assetSelection: AssetSelection.All,
                        isolated: false,
                        @internal: true,
                        allowOverwrite: false,
                        symbolPublishVisibility: symbolVisibility));
            }

            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Checksum,
                    PublishingConstants.FeedStagingForChecksums,
                    FeedType.AzureStorageContainer,
                    ChecksumsTargetStaticFeedKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                    akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.All,
                    isolated: false,
                    @internal: true,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));

            var buildEngine = new MockBuildEngine();
            var channelConfig = PublishingConstants.ChannelInfos.First(c => c.Id == 2);
            var config = new SetupTargetFeedConfigV4(
                    channelConfig,
                    buildModel,
                    isInternalBuild: true,
                    repositoryName: "test-repo",
                    commitSha: "c0c0c0c0",
                    FeedKeys,
                    Array.Empty<ITaskItem>(),
                    Array.Empty<ITaskItem>(),
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    buildEngine: buildEngine,
                    symbolVisibility
                );

            var actualFeeds = config.Setup();

            actualFeeds.Should().BeEquivalentTo(expectedFeeds);
        }

        [Fact]
        public void NonStableAndPublic()
        {
            BuildModel buildModel = new BuildModel(new BuildIdentity());
            buildModel.Artifacts.Packages.Add(new PackageArtifactModel() { CouldBeStable = false });

            var expectedFeeds = new List<TargetFeedConfig>();

            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Package,
                    PublishingConstants.FeedDotNetEng,
                    FeedType.AzDoNugetFeed,
                    AzureDevOpsFeedsKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                        akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.ShippingOnly,
                    isolated: false,
                    @internal: false,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));

            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Package,
                    PublishingConstants.FeedDotNetEng,
                    FeedType.AzDoNugetFeed,
                    AzureDevOpsFeedsKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                    akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.NonShippingOnly,
                    isolated: false,
                    @internal: false,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));

            foreach (var contentType in InstallersAndSymbols)
            {
                expectedFeeds.Add(
                    new TargetFeedConfig(
                        contentType,
                        PublishingConstants.FeedStagingForInstallers,
                        FeedType.AzureStorageContainer,
                        InstallersTargetStaticFeedKey,
                        latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                        akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                        akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                        assetSelection: AssetSelection.All,
                        isolated: false,
                        @internal: false,
                        allowOverwrite: false,
                        symbolPublishVisibility: symbolVisibility));
            }

            expectedFeeds.Add(
                new TargetFeedConfig(
                    TargetFeedContentType.Checksum,
                    PublishingConstants.FeedStagingForChecksums,
                    FeedType.AzureStorageContainer,
                    ChecksumsTargetStaticFeedKey,
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    akaMSCreateLinkPatterns: PublishingConstants.DefaultAkaMSCreateLinkPatterns,
                    akaMSDoNotCreateLinkPatterns: PublishingConstants.DefaultAkaMSDoNotCreateLinkPatterns,
                    assetSelection: AssetSelection.All,
                    isolated: false,
                    @internal: false,
                    allowOverwrite: false,
                    symbolPublishVisibility: symbolVisibility));

            var buildEngine = new MockBuildEngine();
            var channelConfig = PublishingConstants.ChannelInfos.First(c => c.Id == 2);
            var config = new SetupTargetFeedConfigV4(
                    channelConfig,
                    buildModel,
                    isInternalBuild: false,
                    repositoryName: "test-repo",
                    commitSha: "c0c0c0c0",
                    FeedKeys,
                    Array.Empty<ITaskItem>(),
                    Array.Empty<ITaskItem>(),
                    latestLinkShortUrlPrefixes: [$"{LatestLinkShortUrlPrefix}/{BuildQuality}"],
                    buildEngine: buildEngine,
                    symbolVisibility
                );

            var actualFeeds = config.Setup();

            actualFeeds.Should().BeEquivalentTo(expectedFeeds);
        }

        private bool AreEquivalent(List<TargetFeedConfig> expectedItems, List<TargetFeedConfig> actualItems)
        {
            if (expectedItems.Count() != actualItems.Count())
            {
                Output.WriteLine($"The expected items list has {expectedItems.Count()} item(s) but the list of actual items has {actualItems.Count()}.");

                return false;
            }

            foreach (var expected in expectedItems)
            {
                if (actualItems.Contains(expected) == false)
                {
                    Output.WriteLine($"Expected item was not found in the actual collection of items: {expected}");

                    Output.WriteLine("Actual items are: ");
                    foreach (var actual in actualItems)
                    {
                        Output.WriteLine($"\t {actual}");
                    }

                    return false;
                }
            }

            return true;
        }

        [Fact]
        public void MustHaveSeparateTargetFeedSpecificationsForShippingAndNonShipping()
        {
            Action shouldFail = () => new TargetFeedSpecification(new TargetFeedContentType[] { TargetFeedContentType.Package }, "FooFeed", AssetSelection.All);
            shouldFail.Should().Throw<ArgumentException>();

            Action shouldPassShippingOnly = () => new TargetFeedSpecification(new TargetFeedContentType[] { TargetFeedContentType.Package }, "FooFeed", AssetSelection.ShippingOnly);
            shouldPassShippingOnly.Should().NotThrow();

            Action shouldPassNonShippingOnly = () => new TargetFeedSpecification(new TargetFeedContentType[] { TargetFeedContentType.Package }, "FooFeed", AssetSelection.NonShippingOnly);
            shouldPassNonShippingOnly.Should().NotThrow();
        }
    }
}
