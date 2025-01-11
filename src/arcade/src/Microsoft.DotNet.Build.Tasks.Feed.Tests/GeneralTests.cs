// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.DotNet.Arcade.Test.Common;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.Build.Tasks.Feed.Tests.TestDoubles;
using Xunit;
using static Microsoft.DotNet.Build.Tasks.Feed.GeneralUtils;
using static Microsoft.DotNet.Build.CloudTestTasks.AzureStorageUtils;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class GeneralTests
    {
        private const string dummyFeedUrl = "https://fakefeed.azure.com/nuget/v3/index.json";

        [Fact]
        public void ChannelConfigsHaveAllConfigs()
        {
            foreach (var channelConfig in PublishingConstants.ChannelInfos)
            {
                channelConfig.Id.Should().BeGreaterThan(0);
                channelConfig.TargetFeeds.Should().NotBeEmpty();
                foreach (TargetFeedContentType type in Enum.GetValues(typeof(TargetFeedContentType)))
                {
                    if (type == TargetFeedContentType.None)
                        continue;
                    channelConfig.TargetFeeds.Should().Contain(f => f.ContentTypes.Contains(type));
                }
            }
        }

        [Theory]
        [InlineData("foo/bar/baz/bop.symbols.nupkg", true)]
        [InlineData("foo/bar/baz/bop.symbols.nupkg.sha512", false)]
        [InlineData("foo/bar/baz/bip.snupkg.sha512", false)]
        [InlineData("foo/bar/baz/bip.snupkg", true)]
        [InlineData("foo/bar/baz/bip.SNUpkg", true)]
        [InlineData("foo/bar/baz/bop.SYMBOLS.nupkg", true)]
        [InlineData("foo/bar/symbols.nupkg/bop.nupkg", false)]
        public void IsSymbolPackage(string package, bool isSymbolPackage)
        {
            GeneralUtils.IsSymbolPackage(package).Should().Be(isSymbolPackage);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, true)]
        [InlineData(HttpStatusCode.Accepted, true)]
        [InlineData(HttpStatusCode.BadRequest, false)]
        [InlineData(HttpStatusCode.Forbidden, false)]
        [InlineData(HttpStatusCode.InternalServerError, null)]
        public async Task IsFeedPublicShouldCorrectlyInterpretFeedResponseStatusCode(
            HttpStatusCode feedResponseStatusCode,
            bool? expectedResult)
        {
            using var httpClient = FakeHttpClient.WithResponses(
                new HttpResponseMessage(feedResponseStatusCode));
            var retryHandler = new MockRetryHandler();

            var result = await GeneralUtils.IsFeedPublicAsync(
                dummyFeedUrl,
                httpClient,
                new Microsoft.Build.Utilities.TaskLoggingHelper(new StubTask()),
                retryHandler);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, 1)] // do not retry on 2xx
        [InlineData(HttpStatusCode.BadRequest, 1)] // do not retry on 4xx
        [InlineData(HttpStatusCode.InternalServerError, 2)] // retry on 5xx
        public async Task IsFeedPublicShouldRetryFailedRequests(
            HttpStatusCode initialResponseStatusCode,
            int expectedAttemptCount)
        {
            var responses = new[]
            {
                new HttpResponseMessage(initialResponseStatusCode),
                new HttpResponseMessage(HttpStatusCode.OK)
            };

            using var httpClient = FakeHttpClient.WithResponses(responses);

            var retryHandler = new MockRetryHandler(maxAttempts: 2);

            await GeneralUtils.IsFeedPublicAsync(
                dummyFeedUrl,
                httpClient,
                new Microsoft.Build.Utilities.TaskLoggingHelper(new StubTask()),
                retryHandler);

            retryHandler.ActualAttempts.Should().Be(expectedAttemptCount);
        }

        [Theory]
        [InlineData("", HttpStatusCode.NotFound, PackageFeedStatus.DoesNotExist)]
        [InlineData("test-package-b", HttpStatusCode.OK, PackageFeedStatus.ExistsAndDifferent)]
        [InlineData("test-package-a", HttpStatusCode.OK, PackageFeedStatus.ExistsAndIdenticalToLocal)]
        [InlineData("", HttpStatusCode.InternalServerError, PackageFeedStatus.Unknown)]
        public async Task CompareLocalPackageToFeedPackageShouldCorrectlyInterpretFeedResponse(
            string feedResponseContentName,
            HttpStatusCode feedResponseStatusCode,
            PackageFeedStatus expectedResult)
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.zip"));
            var packageContentUrl = $"https://fakefeed.azure.com/nuget/v3/{feedResponseContentName}.nupkg";
            var taskLoggingHelper = new Microsoft.Build.Utilities.TaskLoggingHelper(new StubTask());
            var retryHandler = new MockRetryHandler();

            var response = new HttpResponseMessage(feedResponseStatusCode);
            if (!string.IsNullOrEmpty(feedResponseContentName))
            {
                var content = TestInputs.ReadAllBytes(Path.Combine("Nupkgs", $"{feedResponseContentName}.zip"));
                response.Content = new ByteArrayContent(content);
            };

            var httpClient = FakeHttpClient.WithResponses(response);

            var result = await CompareLocalPackageToFeedPackage(
                localPackagePath,
                packageContentUrl,
                httpClient,
                taskLoggingHelper,
                retryHandler);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, 1)] // do not retry on 2xx
        [InlineData(HttpStatusCode.NotFound, 1)] // do not retry on 404
        [InlineData(HttpStatusCode.BadRequest, 2)] // retry on 4xx
        [InlineData(HttpStatusCode.InternalServerError, 2)] // retry on 5xx
        public async Task CompareLocalPackageToFeedPackageShouldRetryFailedRequests(
            HttpStatusCode initialResponseStatusCode,
            int expectedAttemptCount)
        {
            var testPackageName = Path.Combine("Nupkgs", "test-package-a.zip");
            var localPackagePath = TestInputs.GetFullPath(testPackageName);
            var packageContentUrl = "https://fakefeed.azure.com/nuget/v3/test-package-a.zip";
            var taskLoggingHelper = new Microsoft.Build.Utilities.TaskLoggingHelper(new StubTask());

            var retryHandler = new MockRetryHandler(maxAttempts: 2);

            var responseContent = TestInputs.ReadAllBytes(testPackageName);
            var responses = new[]
            {
                new HttpResponseMessage(initialResponseStatusCode)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(responseContent)
                }
            };

            var httpClient = FakeHttpClient.WithResponses(responses);

            await CompareLocalPackageToFeedPackage(
                localPackagePath,
                packageContentUrl,
                httpClient,
                taskLoggingHelper,
                retryHandler);

            retryHandler.ActualAttempts.Should().Be(expectedAttemptCount);
        }

        [Fact]
        public void TargetChannelConfig_DefaultAreEqual_Test()
        {
            // Remember:
            //      default(TargetChannelConfig)
            // is not the same as
            //      new TargetChannelConfig(default, default, ...)
            // The latter uses the constructor, the former does not.

            TargetChannelConfig defaultLeft = default;
            TargetChannelConfig defaultRight = default;

            Func<bool> action = () => defaultLeft.Equals(defaultRight);

            action.Should().NotThrow();

            bool actualResult = action();

            actualResult.Should().BeTrue();
        }

        [Fact]
        public void TargetChannelConfig_TargetFeeds_EqualTest()
        {
            TargetChannelConfig left = new(
                id: default,
                isInternal: default,
                publishingInfraVersion: default,
                akaMSChannelNames: default,
                akaMSCreateLinkPatterns: default,
                akaMSDoNotCreateLinkPatterns: default,
                targetFeeds: new TargetFeedSpecification[]
                {
                    new (new[] { TargetFeedContentType.Deb }, dummyFeedUrl, AssetSelection.ShippingOnly)  
                },
                symbolTargetType: default,
                flatten: default);

            TargetChannelConfig right = new(
                id: default,
                isInternal: default,
                publishingInfraVersion: default,
                akaMSChannelNames: default,
                akaMSCreateLinkPatterns: default,
                akaMSDoNotCreateLinkPatterns: default,
                targetFeeds: new TargetFeedSpecification[]
                {
                    new (new[] { TargetFeedContentType.Deb }, dummyFeedUrl, AssetSelection.ShippingOnly) 
                },
                symbolTargetType: default,
                flatten: default);

            bool actualResult = left.Equals(right);

            actualResult.Should().BeTrue();
        }

        [Fact]
        public void TargetChannelConfig_TargetFeeds_UnequalTest()
        {
            TargetChannelConfig left = new(
                id: default,
                isInternal: default,
                publishingInfraVersion: default,
                akaMSChannelNames: default,
                akaMSCreateLinkPatterns: default,
                akaMSDoNotCreateLinkPatterns: default,
                targetFeeds: new TargetFeedSpecification[]
                {
                    new (new[] { TargetFeedContentType.Deb }, dummyFeedUrl, AssetSelection.ShippingOnly)
                },
                symbolTargetType: default,
                flatten: default);

            TargetChannelConfig right = new(
                id: default,
                isInternal: default,
                publishingInfraVersion: default,
                akaMSChannelNames: default,
                akaMSCreateLinkPatterns: default,
                akaMSDoNotCreateLinkPatterns: default,
                targetFeeds: Enumerable.Empty<TargetFeedSpecification>(),
                symbolTargetType: default,
                flatten: default);

            bool actualResult = left.Equals(right);

            actualResult.Should().BeFalse();
        }
    }
}
