// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Utilities;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests;

public class AzureStorageAssetPublisherTests
{
    [Theory]
    [InlineData(409, "BlobAlreadyExists")]
    [InlineData(409, "BlobImmutableDueToLegalHold")]
    [InlineData(412, "ConditionNotMet")]
    public async Task IdenticalBlobCreatedConcurrentlyIsAccepted(int status, string errorCode)
    {
        string file = Path.GetTempFileName();

        try
        {
            await File.WriteAllTextAsync(file, "asset contents");
            byte[] contentHash = MD5.HashData(await File.ReadAllBytesAsync(file));
            var blobClient = new Mock<BlobClient>();
            blobClient
                .SetupGet(client => client.Uri)
                .Returns(new Uri("https://example.blob.core.windows.net/assets/test.bin"));

            blobClient
                .Setup(client => client.ExistsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(false, Mock.Of<Response>()));
            blobClient
                .Setup(client => client.UploadAsync(
                    file,
                    It.Is<BlobUploadOptions>(uploadOptions =>
                        uploadOptions.Conditions.IfNoneMatch == ETag.All),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(
                    status,
                    "The blob was created concurrently.",
                    errorCode,
                    null));
            blobClient
                .Setup(client => client.GetPropertiesAsync(
                    It.IsAny<BlobRequestConditions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(
                    BlobsModelFactory.BlobProperties(contentHash: contentHash),
                    Mock.Of<Response>()));

            var buildEngine = new MockBuildEngine();
            var task = new StubTask { BuildEngine = buildEngine };
            var publisher = new TestAzureStorageAssetPublisher(
                new TaskLoggingHelper(task),
                blobClient.Object);

            await publisher.PublishAssetAsync(
                file,
                "test.bin",
                new PushOptions
                {
                    AllowOverwrite = false,
                    PassIfExistingItemIdentical = true
                });

            Assert.Empty(buildEngine.BuildErrorEvents);
            blobClient.VerifyAll();
        }
        finally
        {
            File.Delete(file);
        }
    }

    [Fact]
    public async Task DifferentBlobCreatedConcurrentlyIsRejected()
    {
        string file = Path.GetTempFileName();

        try
        {
            await File.WriteAllTextAsync(file, "asset contents");
            var blobClient = new Mock<BlobClient>();
            blobClient
                .SetupGet(client => client.Uri)
                .Returns(new Uri("https://example.blob.core.windows.net/assets/test.bin"));

            blobClient
                .Setup(client => client.ExistsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(false, Mock.Of<Response>()));
            blobClient
                .Setup(client => client.UploadAsync(
                    file,
                    It.IsAny<BlobUploadOptions>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(
                    412,
                    "The condition specified using HTTP conditional headers is not met.",
                    "ConditionNotMet",
                    null));
            blobClient
                .Setup(client => client.GetPropertiesAsync(
                    It.IsAny<BlobRequestConditions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(
                    BlobsModelFactory.BlobProperties(contentHash: new byte[16]),
                    Mock.Of<Response>()));

            var buildEngine = new MockBuildEngine();
            var task = new StubTask { BuildEngine = buildEngine };
            var publisher = new TestAzureStorageAssetPublisher(
                new TaskLoggingHelper(task),
                blobClient.Object);

            await publisher.PublishAssetAsync(
                file,
                "test.bin",
                new PushOptions
                {
                    AllowOverwrite = false,
                    PassIfExistingItemIdentical = true
                });

            Assert.Single(buildEngine.BuildErrorEvents);
            Assert.Contains("already exists with different contents", buildEngine.BuildErrorEvents[0].Message);
            blobClient.VerifyAll();
        }
        finally
        {
            File.Delete(file);
        }
    }

    [Fact]
    public async Task ConcurrentBlobInspectionFailureIsLogged()
    {
        string file = Path.GetTempFileName();

        try
        {
            var blobClient = new Mock<BlobClient>();
            blobClient
                .SetupGet(client => client.Uri)
                .Returns(new Uri("https://example.blob.core.windows.net/assets/test.bin"));

            blobClient
                .Setup(client => client.ExistsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(false, Mock.Of<Response>()));
            blobClient
                .Setup(client => client.UploadAsync(
                    file,
                    It.IsAny<BlobUploadOptions>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(
                    412,
                    "The condition specified using HTTP conditional headers is not met.",
                    "ConditionNotMet",
                    null));
            blobClient
                .Setup(client => client.GetPropertiesAsync(
                    It.IsAny<BlobRequestConditions>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(
                    503,
                    "The service is temporarily unavailable."));

            var buildEngine = new MockBuildEngine();
            var task = new StubTask { BuildEngine = buildEngine };
            var publisher = new TestAzureStorageAssetPublisher(
                new TaskLoggingHelper(task),
                blobClient.Object);

            await publisher.PublishAssetAsync(
                file,
                "test.bin",
                new PushOptions
                {
                    AllowOverwrite = false,
                    PassIfExistingItemIdentical = true
                });

            Assert.Single(buildEngine.BuildErrorEvents);
            Assert.Contains("Unexpected exception publishing file", buildEngine.BuildErrorEvents[0].Message);
            Assert.Contains("The service is temporarily unavailable", buildEngine.BuildErrorEvents[0].Message);
            blobClient.VerifyAll();
        }
        finally
        {
            File.Delete(file);
        }
    }

    [Fact]
    public async Task ExistingBlobInspectionFailureIsLogged()
    {
        string file = Path.GetTempFileName();

        try
        {
            var blobClient = new Mock<BlobClient>();
            blobClient
                .SetupGet(client => client.Uri)
                .Returns(new Uri("https://example.blob.core.windows.net/assets/test.bin"));

            blobClient
                .Setup(client => client.ExistsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));
            blobClient
                .Setup(client => client.GetPropertiesAsync(
                    It.IsAny<BlobRequestConditions>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(
                    503,
                    "The service is temporarily unavailable."));

            var buildEngine = new MockBuildEngine();
            var task = new StubTask { BuildEngine = buildEngine };
            var publisher = new TestAzureStorageAssetPublisher(
                new TaskLoggingHelper(task),
                blobClient.Object);

            await publisher.PublishAssetAsync(
                file,
                "test.bin",
                new PushOptions
                {
                    AllowOverwrite = false,
                    PassIfExistingItemIdentical = true
                });

            Assert.Single(buildEngine.BuildErrorEvents);
            Assert.Contains("Unexpected exception publishing file", buildEngine.BuildErrorEvents[0].Message);
            Assert.Contains("The service is temporarily unavailable", buildEngine.BuildErrorEvents[0].Message);
            blobClient.VerifyAll();
        }
        finally
        {
            File.Delete(file);
        }
    }

    private sealed class TestAzureStorageAssetPublisher : AzureStorageAssetPublisher
    {
        private readonly BlobClient _blobClient;

        public TestAzureStorageAssetPublisher(TaskLoggingHelper log, BlobClient blobClient)
            : base(log)
        {
            _blobClient = blobClient;
        }

        public override BlobClient CreateBlobClient(string blobPath) => _blobClient;
    }
}
