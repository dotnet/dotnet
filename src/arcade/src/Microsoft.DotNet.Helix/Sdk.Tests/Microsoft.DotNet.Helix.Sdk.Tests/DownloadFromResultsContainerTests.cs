// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using AwesomeAssertions;
using Xunit;

#nullable enable
namespace Microsoft.DotNet.Helix.Sdk.Tests;

public class DownloadFromResultsContainerTests
{
    [Theory]
    [InlineData("https://helixresults.blob.core.windows.net/results/job/console.log", null)]
    [InlineData("https://helixresults.blob.core.windows.net/results/job/console.log", "")]
    [InlineData("https://helixresults.blob.core.windows.net/results/job/console.log?sv=2021&sig=orig", null)]
    [InlineData("https://helixresults.blob.core.windows.net/results/job/console.log?sv=2021&sig=orig", "")]
    public void GetBlobUriAndSas_WhenResultsSasIsNullOrEmpty_ReturnsOriginalUriAndNullSas(string link, string? resultsSas)
    {
        var (blobUri, sasToken) = DownloadFromResultsContainer.GetBlobUriAndSas(link, resultsSas!);

        blobUri.Should().Be(new Uri(link));
        sasToken.Should().BeNull();
    }

    [Fact]
    public void GetBlobUriAndSas_WhenLinkHasNoQueryAndResultsSasProvided_DoesNotThrowAndReturnsCleanUri()
    {
        string link = "https://helixresults.blob.core.windows.net/results/job/console.log";
        string resultsSas = "sp=r&st=2026-10-15&se=2026-10-16&spr=https&sv=2021-06-08&sr=c&sig=mock123";

        var (blobUri, sasToken) = DownloadFromResultsContainer.GetBlobUriAndSas(link, resultsSas);

        blobUri.Should().Be(new Uri("https://helixresults.blob.core.windows.net/results/job/console.log"));
        sasToken.Should().Be(resultsSas);
    }

    [Fact]
    public void GetBlobUriAndSas_WhenLinkHasQueryAndResultsSasProvided_StripsQueryAndReturnsSas()
    {
        string link = "https://helixresults.blob.core.windows.net/results/job/console.log?old=query&sig=old";
        string resultsSas = "sp=r&st=2026-10-15&se=2026-10-16&spr=https&sv=2021-06-08&sr=c&sig=new123";

        var (blobUri, sasToken) = DownloadFromResultsContainer.GetBlobUriAndSas(link, resultsSas);

        blobUri.Should().Be(new Uri("https://helixresults.blob.core.windows.net/results/job/console.log"));
        sasToken.Should().Be(resultsSas);
    }

    [Fact]
    public void GetBlobUriAndSas_WhenResultsSasHasLeadingQuestionMark_TrimsLeadingQuestionMark()
    {
        string link = "https://helixresults.blob.core.windows.net/results/job/console.log?old=query";
        string resultsSas = "?sp=r&st=2026-10-15&se=2026-10-16&spr=https&sv=2021-06-08&sr=c&sig=new123";

        var (blobUri, sasToken) = DownloadFromResultsContainer.GetBlobUriAndSas(link, resultsSas);

        blobUri.Should().Be(new Uri("https://helixresults.blob.core.windows.net/results/job/console.log"));
        sasToken.Should().Be("sp=r&st=2026-10-15&se=2026-10-16&spr=https&sv=2021-06-08&sr=c&sig=new123");
    }

    [Fact]
    public void GetBlobUriAndSas_WhenLinkHasMultipleQuestionMarks_StripsFromFirstQuestionMark()
    {
        string link = "https://helixresults.blob.core.windows.net/results/job/console.log?key=val?extra=1";
        string resultsSas = "sp=r&sig=abc";

        var (blobUri, sasToken) = DownloadFromResultsContainer.GetBlobUriAndSas(link, resultsSas);

        blobUri.Should().Be(new Uri("https://helixresults.blob.core.windows.net/results/job/console.log"));
        sasToken.Should().Be("sp=r&sig=abc");
    }
}
