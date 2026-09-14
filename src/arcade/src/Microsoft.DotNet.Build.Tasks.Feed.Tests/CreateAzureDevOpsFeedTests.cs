// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Build.Tasks.Feed.Tests.TestDoubles;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests;

public class CreateAzureDevOpsFeedTests
{
    private const string FeedUrl = "https://fakefeed.azure.com/nuget/v3/index.json";

    [Fact]
    public async Task WaitForFeedReadyRetriesUntilPublishingCredentialCanReadFeed()
    {
        using var httpClient = FakeHttpClient.WithResponses(
            new HttpResponseMessage(HttpStatusCode.Forbidden),
            new HttpResponseMessage(HttpStatusCode.Forbidden),
            new HttpResponseMessage(HttpStatusCode.OK));
        var retryHandler = new MockRetryHandler(maxAttempts: 3);
        var buildEngine = new MockBuildEngine();
        var task = new CreateAzureDevOpsFeed { BuildEngine = buildEngine };

        bool result = await task.WaitForFeedReadyAsync(FeedUrl, httpClient, retryHandler);

        result.Should().BeTrue();
        retryHandler.ActualAttempts.Should().Be(3);
        buildEngine.BuildErrorEvents.Should().BeEmpty();
    }

    [Fact]
    public async Task WaitForFeedReadyFailsAfterRetriesAreExhausted()
    {
        using var httpClient = FakeHttpClient.WithResponses(
            new HttpResponseMessage(HttpStatusCode.Forbidden),
            new HttpResponseMessage(HttpStatusCode.Forbidden));
        var retryHandler = new MockRetryHandler(maxAttempts: 2);
        var buildEngine = new MockBuildEngine();
        var task = new CreateAzureDevOpsFeed { BuildEngine = buildEngine };

        bool result = await task.WaitForFeedReadyAsync(FeedUrl, httpClient, retryHandler);

        result.Should().BeFalse();
        retryHandler.ActualAttempts.Should().Be(2);
        buildEngine.BuildErrorEvents.Should().ContainSingle();
    }
}
