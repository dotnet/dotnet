// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Arcade.Test.Common;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.Maestro.Client.Models;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class PublishToSymbolServerTest
    {
        private const string MsdlToken = "msdlToken";
        private const string SymWebToken = "SymWebToken";
        private const string TargetUrl = "TargetUrl";
        private const string Msdl = "https://microsoftpublicsymbols.artifacts.visualstudio.com/DefaultCollection";
        private const string SymWeb = "https://microsoft.artifacts.visualstudio.com/DefaultCollection";

        [Theory]
        [InlineData(SymbolTargetType.Msdl, Msdl)]
        [InlineData(SymbolTargetType.SymWeb, SymWeb)]
        public void PublishToSymbolServersTest(SymbolTargetType symbolTargetType , string symbolServer)
        {
            var publishTask = new PublishArtifactsInManifestV3();
            var feedConfigsForSymbols = new HashSet<TargetFeedConfig>();
            feedConfigsForSymbols.Add(new TargetFeedConfig(
                TargetFeedContentType.Symbols,
                "TargetUrl",
                FeedType.AzDoNugetFeed,
                MsdlToken,
                new List<string>(),
                AssetSelection.All,
                isolated: true,
                @internal: false,
                allowOverwrite: true,
                symbolTargetType));
            Dictionary<string, string> test =
                publishTask.GetTargetSymbolServers(feedConfigsForSymbols, MsdlToken, SymWebToken);
            Assert.True(
                test.ContainsKey(symbolServer));
            Assert.True(test.Count == 1);
        }

        [Fact]
        public void PublishToBothSymbolServerTest()
        {
            var publishTask = new PublishArtifactsInManifestV3();
            var feedConfigsForSymbols = new HashSet<TargetFeedConfig>();
            feedConfigsForSymbols.Add(new TargetFeedConfig(
                TargetFeedContentType.Symbols,
                "testUrl",
                FeedType.AzDoNugetFeed,
                SymWebToken,
                new List<string>(),
                AssetSelection.All,
                isolated: true,
                @internal: false,
                allowOverwrite: true,
                SymbolTargetType.SymWeb));
            feedConfigsForSymbols.Add(new TargetFeedConfig(
                TargetFeedContentType.Symbols,
                TargetUrl,
                FeedType.AzDoNugetFeed,
                MsdlToken,
                new List<string>(),
                AssetSelection.All,
                isolated: true,
                @internal: false,
                allowOverwrite: true,
                SymbolTargetType.Msdl));
            Dictionary<string, string> test =
                publishTask.GetTargetSymbolServers(feedConfigsForSymbols, MsdlToken, SymWebToken);
            Assert.True(
                test.ContainsKey(Msdl));
            Assert.True(test.ContainsKey(SymWeb));
            Assert.True(test.Count == 2);
        }

        [Fact]
        public async Task TemporarySymbolDirectoryDoesNotExists()
        {
            var buildEngine = new MockBuildEngine();
            var task = new PublishArtifactsInManifestV3()
            {
                BuildEngine = buildEngine,
            };
            var path = TestInputs.GetFullPath("Symbol");
            var buildAsset = new Dictionary<string, Asset>().AsReadOnly();
            await task.HandleSymbolPublishingAsync(path, MsdlToken, SymWebToken, "", false, buildAsset, null, path);
            Assert.True(task.Log.HasLoggedErrors);
        }

        [Fact]
        public void TemporarySymbolsDirectoryTest()
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3()
            {
                BuildEngine = buildEngine,
            };
            var path = TestInputs.GetFullPath("Test");
            publishTask.EnsureTemporaryDirectoryExists(path);
            Assert.True(Directory.Exists(path));
            publishTask.DeleteTemporaryDirectory(path);
            Assert.False(Directory.Exists(path));
        }

        [Fact]
        public void PublishSymbolApiIsCalledTest()
        {
            var path = TestInputs.GetFullPath("Symbols");
            string[] fileEntries = Directory.GetFiles(path);
            var feedConfigsForSymbols = new HashSet<TargetFeedConfig>();
            feedConfigsForSymbols.Add(new TargetFeedConfig(
                TargetFeedContentType.Symbols,
                TargetUrl,
                FeedType.AzDoNugetFeed,
                SymWebToken,
                new List<string>(),
                AssetSelection.All,
                isolated: true,
                @internal: false,
                allowOverwrite: true,
                SymbolTargetType.SymWeb));
            Assert.True(PublishSymbolsHelper.PublishAsync(
                log: null,
                symbolServerPath: path,
                personalAccessToken: SymWebToken,
                inputPackages: fileEntries,
                inputFiles: fileEntries,
                packageExcludeFiles: null,
                expirationInDays: 365,
                convertPortablePdbsToWindowsPdbs: false,
                publishSpecialClrFiles: false,
                pdbConversionTreatAsWarning: null,
                treatPdbConversionIssuesAsInfo: false,
                dryRun: false,
                timer: false,
                verboseLogging: false).IsCompleted);
        }

        [Fact]
        public void DownloadFileAsyncSucceedsForValidUrl()
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
            };

            var testFile = Path.Combine("Symbols", "test.txt");
            var responseContent = TestInputs.ReadAllBytes(testFile);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(responseContent)
            };

            using HttpClient client = FakeHttpClient.WithResponses(response);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var test = publishTask.DownloadFileAsync(
                client,
                PublishArtifactsInManifestBase.ArtifactName.BlobArtifacts,
                "1234",
                "test.txt",
                path);

            Assert.True(File.Exists(path));
            publishTask.DeleteTemporaryFiles(path);
            publishTask.DeleteTemporaryDirectory(path);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        public async Task DownloadFileAsyncFailsForInValidUrlTest(HttpStatusCode httpStatus)
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
            };
            var testFile = Path.Combine("Symbols", "test.txt");
            var responseContent = TestInputs.ReadAllBytes(testFile);
            publishTask.RetryHandler = new ExponentialRetry() { MaxAttempts = 3, DelayBase = 1 };

            var responses = new[]
            {
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                }
            };
            using HttpClient client = FakeHttpClient.WithResponses(responses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var actualError = await Assert.ThrowsAsync<Exception>(() =>
                publishTask.DownloadFileAsync(
                    client,
                    PublishArtifactsInManifestBase.ArtifactName.BlobArtifacts,
                    "1234",
                    "test.txt",
                    path));
            Assert.Contains($"Failed to download local file '{path}' after {publishTask.RetryHandler.MaxAttempts} attempts.  See inner exception for details.", actualError.Message);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        public async Task DownloadFailureWhenStatusCodeIsInvalid(HttpStatusCode httpStatus)
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
            };
            var testFile = Path.Combine("Symbols", "test.txt");
            var responseContent = TestInputs.ReadAllBytes(testFile);
            publishTask.RetryHandler = new ExponentialRetry() { MaxAttempts = 3, DelayBase = 1 };

            var responses = new[]
            {
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                }
            };
            using HttpClient client = FakeHttpClient.WithResponses(responses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var actualError = await Assert.ThrowsAsync<Exception>(() =>
                publishTask.DownloadFileAsync(
                    client,
                    PublishArtifactsInManifestBase.ArtifactName.BlobArtifacts,
                    "1234",
                    "test.txt",
                    path));
            Assert.Contains($"Failed to download local file '{path}' after {publishTask.RetryHandler.MaxAttempts} attempts.  See inner exception for details.", actualError.Message);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        public async Task DownloadFileSuccessfulAfterRetryTest(HttpStatusCode httpStatus)
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
            };
            var testFile = Path.Combine("Symbols", "test.txt");
            var responseContent = TestInputs.ReadAllBytes(testFile);
            publishTask.RetryHandler = new ExponentialRetry() { MaxAttempts = 2, DelayBase = 1 };

            var responses = new[]
            {
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(responseContent)
                }
            };
            using HttpClient client = FakeHttpClient.WithResponses(responses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            await publishTask.DownloadFileAsync(
                client,
                PublishArtifactsInManifestBase.ArtifactName.BlobArtifacts,
                "1234",
                "test.txt",
                path);
            Assert.True(File.Exists(path));
            publishTask.DeleteTemporaryFiles(path);
            publishTask.DeleteTemporaryDirectory(path);
        }

        [Theory]
        [InlineData(PublishArtifactsInManifestBase.ArtifactName.BlobArtifacts, "1")]
        [InlineData(PublishArtifactsInManifestBase.ArtifactName.PackageArtifacts, "1234")]
        public async Task GetContainerIdToDownloadArtifactAsync(PublishArtifactsInManifestBase.ArtifactName artifactName, string containerId)
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
            };
            publishTask.BuildId = "1243456";
            var testPackageName = Path.Combine("Symbols", "test.txt");
            var responseContent = TestInputs.ReadAllBytes(testPackageName);
            var responses = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(responseContent)
            };

            using HttpClient client = FakeHttpClient.WithResponses(responses);
            var test = await publishTask.GetContainerIdAsync(
                client,
                artifactName);
            Assert.Equal(containerId, test);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task ErrorAfterMaxRetriesToGetContainerId(HttpStatusCode httpStatus)
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
            };
            publishTask.BuildId = "1243456";
            publishTask.RetryHandler = new ExponentialRetry() {MaxAttempts = 3, DelayBase = 1};

            var testPackageName = Path.Combine("Symbols", "test.txt");
            var responseContent = TestInputs.ReadAllBytes(testPackageName);
            var responses = new[]
            {
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                },
                new HttpResponseMessage(httpStatus)
                {
                    Content = new ByteArrayContent(responseContent)
                }
            };

            using HttpClient client = FakeHttpClient.WithResponses(responses);

            var actualError = await Assert.ThrowsAsync<Exception>(() =>
                publishTask.GetContainerIdAsync(
                    client,
                    PublishArtifactsInManifestBase.ArtifactName.BlobArtifacts));
            Assert.Contains($"Failed to get container id after {publishTask.RetryHandler.MaxAttempts} attempts.  See inner exception for details,", actualError.Message);
        }
    }
}
