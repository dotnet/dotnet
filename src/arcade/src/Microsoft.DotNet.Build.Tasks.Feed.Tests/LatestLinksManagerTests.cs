// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.Build.Tasks.Feed.Tests.TestDoubles;
using Microsoft.DotNet.Deployment.Tasks.Links;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class LatestLinksManagerTests
    {
        [Theory]
        [InlineData("https://dotnetcli.blob.core.windows.net/test", "https://builds.dotnet.microsoft.com/test/")]
        [InlineData("https://dotnetbuilds.blob.core.windows.net/test/", "https://ci.dot.net/test/")]
        public void ComputeLatestLinkBase_WithValidFeedConfig_ReturnsExpectedBaseUrl(string safeTargetUrl, string expectedTargetUrl)
        {
            // Arrange
            var feedConfig = new TargetFeedConfig(
                contentType: TargetFeedContentType.Installer,
                targetURL: safeTargetUrl,
                type: FeedType.AzureStorageContainer,
                token: "dummyToken",
                latestLinkShortUrlPrefixes: ["prefix"],
                akaMSCreateLinkPatterns: [],
                akaMSDoNotCreateLinkPatterns: []
            );

            // Act
            LatestLinksManager.ComputeLatestLinkBase(feedConfig).Should().Be(expectedTargetUrl);
        }

        [Fact]
        public void ComputeLatestLinkBase_WithTrailingSlash_ReturnsExpectedBaseUrl()
        {
            // Arrange
            var feedConfig = new TargetFeedConfig(
                contentType: TargetFeedContentType.Installer,
                targetURL: "https://dotnetcli.blob.core.windows.net/test/",
                type: FeedType.AzureStorageContainer,
                token: "dummyToken",
                latestLinkShortUrlPrefixes: ["prefix"],
                akaMSCreateLinkPatterns: [],    
                akaMSDoNotCreateLinkPatterns: ImmutableList<Regex>.Empty
            );

            // Act
            LatestLinksManager.ComputeLatestLinkBase(feedConfig).Should().Be("https://builds.dotnet.microsoft.com/test/");
        }

        [Fact]
        public void ComputeLatestLinkBase_WithUnknownAuthority_ReturnsOriginalBaseUrl()
        {
            // Arrange
            var feedConfig = new TargetFeedConfig(
                contentType: TargetFeedContentType.Installer,
                targetURL: "https://unknown.blob.core.windows.net/test",
                type: FeedType.AzureStorageContainer,
                token: "dummyToken",
                latestLinkShortUrlPrefixes: [ "prefix" ],
                akaMSCreateLinkPatterns: [],
                akaMSDoNotCreateLinkPatterns: []
            );

            // Act
            LatestLinksManager.ComputeLatestLinkBase(feedConfig).Should().Be("https://unknown.blob.core.windows.net/test/");
        }

        [Fact]
        public void GetLatestLinksToCreate_Patterns()
        {
            var taskLoggingHelper = new Microsoft.Build.Utilities.TaskLoggingHelper(new StubTask());
            // Arrange
            var assetsToPublish = new HashSet<string>
            {
                "assets/symbols/Microsoft.stuff.symbols.nupkg",
                "assets/Microsoft.stuff.zip",
                "assets/Microsoft.stuff.zip.sha512",
                "assets/Microsoft.stuff.json",
                "assets/Microsoft.stuff.json.zip",
                "assets/Microsoft.stuff.sha512"
            };
            var feedConfig = new TargetFeedConfig(
                contentType: TargetFeedContentType.Other,
                targetURL: "https://example.com/feed",
                type: FeedType.AzureStorageContainer,
                token: "",
                latestLinkShortUrlPrefixes: ImmutableList.Create("prefix1", "prefix2"),
                akaMSCreateLinkPatterns: [new Regex(@"\.zip(\.sha512)?")],
                akaMSDoNotCreateLinkPatterns: [new Regex("json")],
                assetSelection: AssetSelection.All,
                isolated: false,
                @internal: false,
                allowOverwrite: false,
                symbolPublishVisibility: SymbolPublishVisibility.None,
                flatten: true
            );

            var manager = new LatestLinksManager("clientId", null, "tenant", "groupOwner", "createdBy", "owners", taskLoggingHelper);

            var links = manager.GetLatestLinksToCreate(assetsToPublish, feedConfig, "https://example.com/feed/");

            // Flattenned links should remove the path elements
            links.Should().BeEquivalentTo(new List<AkaMSLink>
            {
                new AkaMSLink("prefix1/Microsoft.stuff.zip", "https://example.com/feed/assets/Microsoft.stuff.zip"),
                new AkaMSLink("prefix2/Microsoft.stuff.zip", "https://example.com/feed/assets/Microsoft.stuff.zip"),
                new AkaMSLink("prefix1/Microsoft.stuff.zip.sha512", "https://example.com/feed/assets/Microsoft.stuff.zip.sha512"),
                new AkaMSLink("prefix2/Microsoft.stuff.zip.sha512", "https://example.com/feed/assets/Microsoft.stuff.zip.sha512")
            });
        }

        [Fact]
        public void GetLatestLinksToCreate_EmptyPatternsShouldCreateNoLinks()
        {
            var taskLoggingHelper = new Microsoft.Build.Utilities.TaskLoggingHelper(new StubTask());
            // Arrange
            var assetsToPublish = new HashSet<string> { "assets/symbols/Microsoft.stuff.symbols.nupkg", "assets/Microsoft.stuff.zip", "assets/Microsoft.stuff.json", "assets/Microsoft.stuff.json.zip" };
            var feedConfig = new TargetFeedConfig(
                contentType: TargetFeedContentType.Other,
                targetURL: "https://example.com/feed",
                type: FeedType.AzureStorageContainer,
                token: "",
                latestLinkShortUrlPrefixes: ImmutableList.Create("prefix1", "prefix2"),
                akaMSCreateLinkPatterns: [],
                akaMSDoNotCreateLinkPatterns: null,
                assetSelection: AssetSelection.All,
                isolated: false,
                @internal: false,
                allowOverwrite: false,
                symbolPublishVisibility: SymbolPublishVisibility.None,
                flatten: true
            );

            var manager = new LatestLinksManager("clientId", null, "tenant", "groupOwner", "createdBy", "owners", taskLoggingHelper);

            var links = manager.GetLatestLinksToCreate(assetsToPublish, feedConfig, "https://example.com/feed/");

            // Flattenned links should remove the path elements
            links.Should().BeEmpty();
        }

        [Fact]
        public void GetLatestLinksToCreate_OnlyIncludeShouldWork()
        {
            var taskLoggingHelper = new TaskLoggingHelper(new StubTask());
            // Arrange
            var assetsToPublish = new HashSet<string> { "assets/symbols/Microsoft.stuff.symbols.nupkg", "assets/Microsoft.stuff.zip", "assets/Microsoft.stuff.json", "assets/Microsoft.stuff.json.zip" };
            var feedConfig = new TargetFeedConfig(
                contentType: TargetFeedContentType.Other,
                targetURL: "https://example.com/feed",
                type: FeedType.AzureStorageContainer,
                token: "",
                latestLinkShortUrlPrefixes: ImmutableList.Create("prefix1", "prefix2"),
                akaMSCreateLinkPatterns: [new Regex(@"\.zip")],
                akaMSDoNotCreateLinkPatterns: [],
                assetSelection: AssetSelection.All,
                isolated: false,
                @internal: false,
                allowOverwrite: false,
                symbolPublishVisibility: SymbolPublishVisibility.None,
                flatten: true
            );

            var manager = new LatestLinksManager("clientId", null, "tenant", "groupOwner", "createdBy", "owners", taskLoggingHelper);

            var links = manager.GetLatestLinksToCreate(assetsToPublish, feedConfig, "https://example.com/feed/");

            // Flattenned links should remove the path elements
            // Flattenned links should remove the path elements
            links.Should().BeEquivalentTo(new List<AkaMSLink>
            {
                new AkaMSLink("prefix1/Microsoft.stuff.zip", "https://example.com/feed/assets/Microsoft.stuff.zip"),
                new AkaMSLink("prefix2/Microsoft.stuff.zip", "https://example.com/feed/assets/Microsoft.stuff.zip"),
                new AkaMSLink("prefix1/Microsoft.stuff.json.zip", "https://example.com/feed/assets/Microsoft.stuff.json.zip"),
                new AkaMSLink("prefix2/Microsoft.stuff.json.zip", "https://example.com/feed/assets/Microsoft.stuff.json.zip")
            });
        }

        [Fact]
        public void GetLatestLinksToCreate_NonFlattenedShouldNotFlatten()
        {
            var taskLoggingHelper = new TaskLoggingHelper(new StubTask());
            // Arrange
            var assetsToPublish = new HashSet<string> { "assets/symbols/Microsoft.stuff.symbols.nupkg", "bar/Microsoft.stuff.zip", "assets/Microsoft.stuff.json", "assets/plop/Microsoft.stuff.json.zip" };
            var feedConfig = new TargetFeedConfig(
                contentType: TargetFeedContentType.Other,
                targetURL: "https://example.com/feed",
                type: FeedType.AzureStorageContainer,
                token: "",
                latestLinkShortUrlPrefixes: ImmutableList.Create("prefix1", "prefix2"),
                akaMSCreateLinkPatterns: [new Regex(@"\.zip")],
                akaMSDoNotCreateLinkPatterns: [],
                assetSelection: AssetSelection.All,
                isolated: false,
                @internal: false,
                allowOverwrite: false,
                symbolPublishVisibility: SymbolPublishVisibility.None,
                flatten: false
            );

            var manager = new LatestLinksManager("clientId", null, "tenant", "groupOwner", "createdBy", "owners", taskLoggingHelper);

            var links = manager.GetLatestLinksToCreate(assetsToPublish, feedConfig, "https://example.com/feed/");

            // Flattenned links should remove the path elements
            // Flattenned links should remove the path elements
            links.Should().BeEquivalentTo(new List<AkaMSLink>
            {
                new AkaMSLink("prefix1/bar/Microsoft.stuff.zip", "https://example.com/feed/bar/Microsoft.stuff.zip"),
                new AkaMSLink("prefix2/bar/Microsoft.stuff.zip", "https://example.com/feed/bar/Microsoft.stuff.zip"),
                new AkaMSLink("prefix1/assets/plop/Microsoft.stuff.json.zip", "https://example.com/feed/assets/plop/Microsoft.stuff.json.zip"),
                new AkaMSLink("prefix2/assets/plop/Microsoft.stuff.json.zip", "https://example.com/feed/assets/plop/Microsoft.stuff.json.zip")
            });
        }
    }
}
