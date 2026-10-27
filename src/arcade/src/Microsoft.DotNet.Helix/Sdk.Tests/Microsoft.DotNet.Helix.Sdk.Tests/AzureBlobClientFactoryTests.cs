// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using AwesomeAssertions;
using Microsoft.DotNet.Helix.JobMonitor;
using Xunit;

#nullable enable
namespace Microsoft.DotNet.Helix.Sdk.Tests;

public class AzureBlobClientFactoryTests
{
    private readonly AzureBlobClientFactory _factory = new();

    [Theory]
    [InlineData("https://helixresults.blob.core.windows.net/results/job/file.txt")]
    [InlineData("https://helixresults.blob.core.windows.net/results/job/file.txt?sv=2021&sig=orig")]
    public void CreateBlobClient_WithoutSasToken_ReturnsClientWithOriginalUri(string blobUri)
    {
        var client = _factory.CreateBlobClient(blobUri);

        client.Uri.Should().Be(new Uri(blobUri));
    }

    [Fact]
    public void CreateBlobClient_WithBareUrlAndSasToken_PreservesBareUri()
    {
        string blobUri = "https://helixresults.blob.core.windows.net/results/job/file.txt";
        string sasToken = "sp=r&st=2026-10-16&se=2026-10-17&spr=https&sv=2021-06-08&sr=c&sig=mock123";

        var client = _factory.CreateBlobClient(blobUri, sasToken);

        client.Uri.Should().Be(new Uri(blobUri));
        client.Uri.Query.Should().BeEmpty();
    }

    [Fact]
    public void CreateBlobClient_WithExistingQueryAndSasToken_StripsQuery()
    {
        string baseUri = "https://helixresults.blob.core.windows.net/results/job/file.txt";
        string blobUri = $"{baseUri}?oldQuery=val&sig=old";
        string sasToken = "sp=r&st=2026-10-16&se=2026-10-17&spr=https&sv=2021-06-08&sr=c&sig=new123";

        var client = _factory.CreateBlobClient(blobUri, sasToken);

        client.Uri.Should().Be(new Uri(baseUri));
        client.Uri.Query.Should().BeEmpty();
    }

    [Fact]
    public void CreateBlobClient_WithSasHavingLeadingQuestionMark_TrimsLeadingQuestionMarkAndStripsQuery()
    {
        string baseUri = "https://helixresults.blob.core.windows.net/results/job/file.txt";
        string blobUri = $"{baseUri}?old=val";
        string sasToken = "?sp=r&st=2026-10-16&se=2026-10-17&spr=https&sv=2021-06-08&sr=c&sig=new123";

        var client = _factory.CreateBlobClient(blobUri, sasToken);

        client.Uri.Should().Be(new Uri(baseUri));
        client.Uri.Query.Should().BeEmpty();
    }

    [Fact]
    public void CreateBlobClient_WithMultipleQuestionMarks_StripsFromFirstQuestionMark()
    {
        string baseUri = "https://helixresults.blob.core.windows.net/results/job/file.txt";
        string blobUri = $"{baseUri}?key=val?extra=1";
        string sasToken = "sp=r&sig=abc";

        var client = _factory.CreateBlobClient(blobUri, sasToken);

        client.Uri.Should().Be(new Uri(baseUri));
        client.Uri.Query.Should().BeEmpty();
    }

    [Fact]
    public void CreateBlobClient_WithContainerUriAndLeadingQuestionMarkSas_TrimsLeadingQuestionMark()
    {
        var containerUri = new Uri("https://helixresults.blob.core.windows.net/results");
        string blobName = "test.txt";
        string sasToken = "?sp=r&sig=abc";

        var client = _factory.CreateBlobClient(containerUri, blobName, sasToken);

        client.Uri.Should().Be(new Uri($"https://helixresults.blob.core.windows.net/results/{blobName}"));
    }
}
