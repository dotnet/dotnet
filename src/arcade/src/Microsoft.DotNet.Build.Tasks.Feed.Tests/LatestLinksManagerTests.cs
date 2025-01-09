// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.DotNet.Build.Tasks.Feed;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using FluentAssertions;

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
                latestLinkShortUrlPrefixes: new List<string> { "prefix" }
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
                latestLinkShortUrlPrefixes: new List<string> { "prefix" }
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
                latestLinkShortUrlPrefixes: new List<string> { "prefix" }
            );

            // Act
            LatestLinksManager.ComputeLatestLinkBase(feedConfig).Should().Be("https://unknown.blob.core.windows.net/test/");
        }
    }
}
