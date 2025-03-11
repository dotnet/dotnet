// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Azure;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Arcade.Test.Common;
using FluentAssertions;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class DownloadFileTests
    {
        private const string _testTextFile = "test.txt";
        private const string _testSymbolPackage = "test-package-a.1.0.0.symbols.nupkg";

        [Fact]
        public void DownloadFileAsyncSucceedsForValidUrl_BlobArtifact()
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
                AzureDevOpsOrg = "dnceng",
                BuildId = "1234",
                AzureDevOpsProject = "blah"
            };

            var jsonContent = JsonContent.Create(
                new
                {
                    count = 1,
                    value = new[]
                    {
                            new
                            {
                                id = "1234",
                                name = "BlobArtifacts",
                                resource = new
                                {
                                    type = "Container",
                                    data = "#/123456/BlobArtifacts",
                                }
                            }
                    }
                });

            var testFile = Path.Combine("Symbols", _testTextFile);
            var fileResponseContent = TestInputs.ReadAllBytes(testFile);
            var fileResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileResponseContent)
            };

            var artifactResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = jsonContent
            };

            // Create a series of fake http responses. First
            // there will be a response from the artifact API, which will be used
            // to determine that the file is a blob artifact, and the correct container ID
            Dictionary<string, IEnumerable<HttpResponseMessage>> fakeHttpResponses = new Dictionary<string, IEnumerable<HttpResponseMessage>>
                {
                    { "https://dev.azure.com/dnceng/blah/_apis/build/builds/1234/artifacts?api-version=6.0", new[] { artifactResponse } },
                    { "https://dev.azure.com/dnceng/_apis/resources/Containers/123456?itemPath=BlobArtifacts%2Ftest.txt&isShallow=true&api-version=4.1-preview.4", new[] { fileResponse } }
                };

            using HttpClient client = FakeHttpClient.WithResponsesGivenUris(fakeHttpResponses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var test = publishTask.DownloadFileAsync(
                client,
                PublishArtifactsInManifestBase.BlobArtifactsArtifactName,
                _testTextFile,
                path);

            ValidateNoRemainingResponses(fakeHttpResponses, client);

            File.Exists(path).Should().BeTrue();
            publishTask.DeleteTemporaryFiles(path);
            publishTask.DeleteTemporaryDirectory(path);
        }

        [Fact]
        public void DownloadFileAsyncSucceedsForValidUrl_PipelineArtifact()
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
                AzureDevOpsOrg = "dnceng",
                BuildId = "1234",
                AzureDevOpsProject = "blah"
            };

            var jsonContent = JsonContent.Create(
                new
                {
                    count = 1,
                    value = new[]
                    {
                            new
                            {
                                id = "1234",
                                name = "PackageArtifacts",
                                resource = new
                                {
                                    type = "PipelineArtifact",
                                    data = "HASHHASHHASH",
                                    downloadUrl = "https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=zip"
                                }
                            }
                    }
                });

            var testFile = Path.Combine("Symbols", _testTextFile);
            var fileResponseContent = TestInputs.ReadAllBytes(testFile);
            var fileResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileResponseContent)
            };

            var artifactResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = jsonContent
            };

            // Create a series of fake http responses. First
            // there will be a response from the artifact API, which will be used
            // to determine that the file is a blob artifact, and the correct container ID
            Dictionary<string, IEnumerable<HttpResponseMessage>> fakeHttpResponses = new Dictionary<string, IEnumerable<HttpResponseMessage>>
                {
                    { "https://dev.azure.com/dnceng/blah/_apis/build/builds/1234/artifacts?api-version=6.0", new[] { artifactResponse } },
                    { "https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=file&subPath=%2Ftest.txt", new[] { fileResponse } }
                };

            using HttpClient client = FakeHttpClient.WithResponsesGivenUris(fakeHttpResponses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var test = publishTask.DownloadFileAsync(
                client,
                PublishArtifactsInManifestBase.PackageArtifactsArtifactName,
                _testTextFile,
                path);

            ValidateNoRemainingResponses(fakeHttpResponses, client);

            File.Exists(path).Should().BeTrue();
            publishTask.DeleteTemporaryFiles(path);
            publishTask.DeleteTemporaryDirectory(path);
        }

        [Fact]
        public void DownloadFileAsyncSucceedsForValidUrl_CachedURLHelper()
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
                AzureDevOpsOrg = "dnceng",
                BuildId = "1234",
                AzureDevOpsProject = "blah"
            };

            var jsonContent = JsonContent.Create(
                new
                {
                    count = 1,
                    value = new[]
                    {
                            new
                            {
                                id = "1234",
                                name = "PackageArtifacts",
                                resource = new
                                {
                                    type = "PipelineArtifact",
                                    data = "HASHHASHHASH",
                                    downloadUrl = "https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=zip"
                                }
                            }
                    }
                });

            var testFile = Path.Combine("Symbols", _testTextFile);
            var fileResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(TestInputs.ReadAllBytes(Path.Combine("Symbols", _testTextFile)))
            };

            var nextFileResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(TestInputs.ReadAllBytes(Path.Combine("Symbols", _testSymbolPackage)))
            };

            var artifactResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = jsonContent
            };

            // Next response returns 404
            var nextArtifactResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            // Create a series of fake http responses. First
            // there will be a response from the artifact API, which will be used
            // to determine that the file is a blob artifact, and the correct container ID
            Dictionary<string, IEnumerable<HttpResponseMessage>> fakeHttpResponses = new Dictionary<string, IEnumerable<HttpResponseMessage>>
                {
                    { "https://dev.azure.com/dnceng/blah/_apis/build/builds/1234/artifacts?api-version=6.0", new[] { artifactResponse, nextArtifactResponse } },
                    { "https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=file&subPath=%2Ftest.txt", new[] { fileResponse, nextFileResponse } },
                    { "https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=file&subPath=%2Ftest-package-a.1.0.0.symbols.nupkg", new[] { nextFileResponse } }
                };

            using HttpClient client = FakeHttpClient.WithResponsesGivenUris(fakeHttpResponses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var test = publishTask.DownloadFileAsync(
                client,
                PublishArtifactsInManifestBase.PackageArtifactsArtifactName,
                _testTextFile,
                path);

            File.Exists(path).Should().BeTrue();

            var path2 = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var test2 = publishTask.DownloadFileAsync(
                client,
                PublishArtifactsInManifestBase.PackageArtifactsArtifactName,
                "test-package-a.1.0.0.symbols.nupkg",
                path2);

            File.Exists(path2).Should().BeTrue();
            ValidateNoRemainingResponses(fakeHttpResponses, client);

            publishTask.DeleteTemporaryFiles(path);
            publishTask.DeleteTemporaryDirectory(path);
        }

        private static void ValidateNoRemainingResponses(Dictionary<string, IEnumerable<HttpResponseMessage>> fakeHttpResponses, HttpClient client)
        {
            // Ensure that if we send to any of our URIs, we get an exception thrown for no remaining responses:
            foreach (var uri in fakeHttpResponses.Keys)
            {
                FluentActions.Invoking(() => client.GetAsync(uri)).Should().ThrowAsync<InvalidOperationException>().WithMessage("Unexpected end of response sequence*");
            }
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        public async Task DownloadFailure_FailedToAccessArtifactAPI(HttpStatusCode httpStatus)
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
                AzureDevOpsOrg = "dnceng",
                BuildId = "1234",
                AzureDevOpsProject = "blah",
                RetryHandler = new ExponentialRetry() { MaxAttempts = 3, DelayBase = 1 }
            };
            var testFile = Path.Combine("Symbols", _testTextFile);
            var responseContent = TestInputs.ReadAllBytes(testFile);

            var responses = new[]
            {
                    new HttpResponseMessage(httpStatus),
                    new HttpResponseMessage(httpStatus),
                    new HttpResponseMessage(httpStatus),
                };
            using HttpClient client = FakeHttpClient.WithResponses(responses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var actualError = await FluentActions.Invoking(() => publishTask.DownloadFileAsync(
                    client,
                    PublishArtifactsInManifestBase.BlobArtifactsArtifactName,
                    _testTextFile,
                    path))
                .Should().ThrowAsync<Exception>();
            actualError.WithMessage($"Failed to construct download URL helper after {publishTask.RetryHandler.MaxAttempts} attempts.  See inner exception for details*");
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        public async Task DownloadFailure_FailedToDownloadFileAfterArtifactAPISuccess(HttpStatusCode httpStatus)
        {
            var buildEngine = new MockBuildEngine();
            var publishTask = new PublishArtifactsInManifestV3
            {
                BuildEngine = buildEngine,
                AzureDevOpsOrg = "dnceng",
                BuildId = "1234",
                AzureDevOpsProject = "blah",
                RetryHandler = new ExponentialRetry() { MaxAttempts = 3, DelayBase = 1 }
            };

            var jsonContent = JsonContent.Create(
                new
                {
                    count = 1,
                    value = new[]
                    {
                            new
                            {
                                id = "1234",
                                name = "BlobArtifacts",
                                resource = new
                                {
                                    type = "Container",
                                    data = "#/123456/BlobArtifacts",
                                }
                            }
                    }
                });

            var testFile = Path.Combine("Symbols", _testTextFile);
            var fileResponseContent = TestInputs.ReadAllBytes(testFile);
            var fileResponse = new HttpResponseMessage(httpStatus);

            var artifactResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = jsonContent
            };

            // Create a series of fake http responses. First
            // there will be a response from the artifact API, which will be used
            // to determine that the file is a blob artifact, and the correct container ID
            Dictionary<string, IEnumerable<HttpResponseMessage>> fakeHttpResponses = new Dictionary<string, IEnumerable<HttpResponseMessage>>
                {
                    { "https://dev.azure.com/dnceng/blah/_apis/build/builds/1234/artifacts?api-version=6.0", new[] { artifactResponse } },
                    { "https://dev.azure.com/dnceng/_apis/resources/Containers/123456?itemPath=BlobArtifacts%2Ftest.txt&isShallow=true&api-version=4.1-preview.4",
                        new[]
                        {
                            new HttpResponseMessage(httpStatus),
                            new HttpResponseMessage(httpStatus),
                            new HttpResponseMessage(httpStatus),
                        }
                    }
                };

            using HttpClient client = FakeHttpClient.WithResponsesGivenUris(fakeHttpResponses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            var actualError = await FluentActions.Invoking(() => publishTask.DownloadFileAsync(
                    client,
                    PublishArtifactsInManifestBase.BlobArtifactsArtifactName,
                    _testTextFile,
                    path))
                .Should().ThrowAsync<Exception>();
            actualError.WithMessage($"Failed to download '{path}' after {publishTask.RetryHandler.MaxAttempts} attempts. See inner exception for details.");

            ValidateNoRemainingResponses(fakeHttpResponses, client);
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
                AzureDevOpsOrg = "dnceng",
                BuildId = "1234",
                AzureDevOpsProject = "blah",
                RetryHandler = new ExponentialRetry() { MaxAttempts = 3, DelayBase = 1 }
            };

            var jsonContent = JsonContent.Create(
                new
                {
                    count = 1,
                    value = new[]
                    {
                            new
                            {
                                id = "1234",
                                name = "BlobArtifacts",
                                resource = new
                                {
                                    type = "Container",
                                    data = "#/123456/BlobArtifacts",
                                }
                            }
                    }
                });

            var testFile = Path.Combine("Symbols", _testTextFile);
            var fileResponseContent = TestInputs.ReadAllBytes(testFile);
            var fileResponse = new HttpResponseMessage(httpStatus);

            var artifactResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = jsonContent
            };

            // Create a series of fake http responses. First
            // there will be a response from the artifact API, which will be used
            // to determine that the file is a blob artifact, and the correct container ID
            Dictionary<string, IEnumerable<HttpResponseMessage>> fakeHttpResponses = new Dictionary<string, IEnumerable<HttpResponseMessage>>
                {
                    { "https://dev.azure.com/dnceng/blah/_apis/build/builds/1234/artifacts?api-version=6.0", new[] { artifactResponse } },
                    { "https://dev.azure.com/dnceng/_apis/resources/Containers/123456?itemPath=BlobArtifacts%2Ftest.txt&isShallow=true&api-version=4.1-preview.4",
                        new[]
                        {
                            new HttpResponseMessage(httpStatus),
                            new HttpResponseMessage(HttpStatusCode.OK)
                        }
                    }
                };

            using HttpClient client = FakeHttpClient.WithResponsesGivenUris(fakeHttpResponses);
            var path = TestInputs.GetFullPath(Guid.NewGuid().ToString());

            await publishTask.DownloadFileAsync(
                client,
                PublishArtifactsInManifestBase.BlobArtifactsArtifactName,
                _testTextFile,
                path);
            File.Exists(path).Should().BeTrue();
            ValidateNoRemainingResponses(fakeHttpResponses, client);
            publishTask.DeleteTemporaryFiles(path);
            publishTask.DeleteTemporaryDirectory(path);
        }
    }
}
