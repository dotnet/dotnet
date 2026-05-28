// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class HelixServiceTests
    {
        [Fact]
        public async Task GetJobsForBuildAsync_PassesSourceThroughAndFiltersByBuildId()
        {
            var api = CreateApi();
            string capturedSource = null;
            int? capturedCount = null;
            api.Job
                .Setup(j => j.ListAsync(null, It.IsAny<int?>(), null, null, It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
                .Callback<string, int?, string, string, string, string, CancellationToken>((_, count, _, _, source, _, _) =>
                {
                    capturedCount = count;
                    capturedSource = source;
                })
                .ReturnsAsync(ImmutableList.Create(
                    Job("running-job", finished: null, new JObject
                    {
                        ["BuildId"] = "123",
                        ["TestRunName"] = "custom run",
                        ["System.StageName"] = "test stage",
                    }),
                    Job("finished-job", finished: "2026-04-30T00:00:00Z", new JObject
                    {
                        ["BuildId"] = "123",
                    }),
                    Job("wrong-build", finished: null, new JObject
                    {
                        ["BuildId"] = "999",
                    }),
                    Job("missing-properties", finished: null, properties: null),
                    Job("non-object-properties", finished: null, new JArray())));

            HelixService service = CreateService(api.Api.Object);

            IReadOnlyList<HelixJobInfo> jobs = await service.GetJobsForBuildAsync(
                source: "pr/public/dotnet/runtime/refs/pull/42/merge",
                buildId: "123",
                CancellationToken.None);

            Assert.Equal("pr/public/dotnet/runtime/refs/pull/42/merge", capturedSource);
            Assert.Equal(100_000, capturedCount);
            Assert.Equal(2, jobs.Count);
            Assert.Equal("running-job", jobs[0].JobName);
            Assert.Equal("running", jobs[0].Status);
            Assert.Equal("custom run", jobs[0].TestRunName);
            Assert.Equal("test stage", jobs[0].StageName);
            Assert.Equal("finished-job", jobs[1].JobName);
            Assert.Equal("finished", jobs[1].Status);
            Assert.Equal("run on", jobs[1].TestRunName);
            Assert.Null(jobs[1].StageName);
        }

        [Fact]
        public async Task GetJobsForBuildAsync_RequiresNonEmptySource()
        {
            HelixService service = CreateService(CreateApi().Api.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetJobsForBuildAsync(source: "", buildId: "123", CancellationToken.None));
        }

        [Fact]
        public async Task DownloadTestResultsAsync_FiltersFilesUsesFileSystemAndContinuesAfterDownloadFailure()
        {
            var api = CreateApi();
            api.Job
                .Setup(j => j.ResultsAsync("job:name", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new JobResultsUri { ResultsUriRSAS = "?resultSas" });
            api.WorkItem
                .Setup(w => w.ListFilesAsync("work:item", "job:name", false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ImmutableList.Create(
                    new UploadedFile("logs/console.txt", "https://storage/logs/console.txt"),
                    new UploadedFile("nested/testResults.xml", "https://storage/nested/testResults.xml"),
                    new UploadedFile("failed.trx", "https://storage/failed.trx")));
            api.WorkItem
                .Setup(w => w.ListFilesAsync("no-results", "job:name", false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ImmutableList.Create(new UploadedFile("artifact.bin", "https://storage/artifact.bin")));

            var blobClientFactory = new FakeBlobClientFactory();
            blobClientFactory.FailDownloadsFor.Add("https://storage/failed.trx");
            var fileSystem = new MockFileSystem(directorySeparator: Path.DirectorySeparatorChar.ToString());

            IReadOnlyList<WorkItemTestResults> results = await CreateService(api.Api.Object, blobClientFactory, fileSystem)
                .DownloadTestResultsAsync("job:name", ["work:item", "no-results"], "work", CancellationToken.None);

            WorkItemTestResults result = Assert.Single(results);
            Assert.Equal("job:name", result.JobName);
            Assert.Equal("work:item", result.WorkItemName);
            string jobDirectory = fileSystem.PathCombine("work", SanitizeForCurrentPlatform("job:name"));
            string workItemDirectory = fileSystem.PathCombine(jobDirectory, SanitizeForCurrentPlatform("work:item"));
            string expectedResultFile = fileSystem.PathCombine(workItemDirectory, NormalizeForCurrentPlatform("nested/testResults.xml"));
            Assert.Equal([expectedResultFile], result.TestResultFiles);
            Assert.Contains(jobDirectory, fileSystem.Directories);
            Assert.Contains(workItemDirectory, fileSystem.Directories);
            Assert.Contains(fileSystem.GetDirectoryName(expectedResultFile), fileSystem.Directories);
            Assert.Equal(
                [new DownloadCall("https://storage/nested/testResults.xml", "?resultSas", expectedResultFile),
                 new DownloadCall("https://storage/failed.trx", "?resultSas", fileSystem.PathCombine(workItemDirectory, "failed.trx"))],
                blobClientFactory.Downloads);
        }

        [Fact]
        public async Task CancelJobAsync_DelegatesToHelixApiJobCancelAsync()
        {
            var api = CreateApi();
            using var cts = new CancellationTokenSource();

            api.Job
                .Setup(j => j.CancelAsync("job-to-cancel", null, cts.Token))
                .Returns(Task.CompletedTask);

            await CreateService(api.Api.Object).CancelJobAsync("job-to-cancel", cts.Token);

            api.Job.Verify(j => j.CancelAsync("job-to-cancel", null, cts.Token), Times.Once);
        }

        [Fact]
        public async Task ResubmitWorkItemsAsync_ReturnsNullWhenRequiredJobDetailsAreMissing()
        {
            var api = CreateApi();
            api.Job
                .Setup(j => j.DetailsAsync("original-job", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new JobDetails("https://storage/job-list.json", null, "original-job", "wait", "source", "type", "build")
                {
                    QueueId = null,
                });

            HelixJobInfo result = await CreateService(api.Api.Object)
                .ResubmitWorkItemsAsync(new HelixJobInfo("original-job", "finished"), [WorkItem("missing")], CancellationToken.None);

            Assert.Null(result);
            api.Storage.Verify(s => s.NewAsync(It.IsAny<ContainerCreationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ResubmitWorkItemsAsync_ReturnsNullWhenJobListIsInvalidJson()
        {
            var api = CreateApi();
            api.Job
                .Setup(j => j.DetailsAsync("original-job", It.IsAny<CancellationToken>()))
                .ReturnsAsync(JobDetails());
            var blobClientFactory = new FakeBlobClientFactory
            {
                DownloadedText = "not json",
            };

            HelixJobInfo result = await CreateService(api.Api.Object, blobClientFactory)
                .ResubmitWorkItemsAsync(new HelixJobInfo("original-job", "finished"), [WorkItem("work-a")], CancellationToken.None);

            Assert.Null(result);
            api.Storage.Verify(s => s.NewAsync(It.IsAny<ContainerCreationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ResubmitWorkItemsAsync_UploadsFilteredJobListAndCreatesJobWithCopiedMetadata()
        {
            var api = CreateApi();
            api.Job
                .Setup(j => j.DetailsAsync("original-job", It.IsAny<CancellationToken>()))
                .ReturnsAsync(JobDetails());
            api.Storage
                .Setup(s => s.NewAsync(It.IsAny<ContainerCreationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ContainerInformation(
                    DateTimeOffset.Parse("2026-04-30T00:00:00Z"),
                    DateTimeOffset.Parse("2026-05-30T00:00:00Z"),
                    "creator",
                    "container",
                    "account",
                    Guid.Empty,
                    "region")
                {
                    ReadToken = "?read",
                    WriteToken = "?write",
                });

            JobCreationRequest capturedRequest = null;
            string capturedIdempotencyKey = null;
            api.Job
                .Setup(j => j.NewAsync(It.IsAny<JobCreationRequest>(), It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
                .Callback<JobCreationRequest, string, bool?, CancellationToken>((request, idempotencyKey, _, _) =>
                {
                    capturedRequest = request;
                    capturedIdempotencyKey = idempotencyKey;
                })
                .ReturnsAsync(new JobCreationResult("new-job", "summary", "results", null));

            var blobClientFactory = new FakeBlobClientFactory
            {
                DownloadedText = """
                [
                  { "WorkItemId": "work-a", "Command": "run a", "PayloadUri": "payload-a" },
                  { "WorkItemId": "work-b", "Command": "run b", "PayloadUri": "payload-b" },
                  { "WorkItemId": "other", "Command": "run other", "PayloadUri": "payload-other" }
                ]
                """,
                UploadedBlobUri = new Uri("https://account.blob.core.windows.net/container/job-list.json"),
            };

            HelixJobInfo result = await CreateService(api.Api.Object, blobClientFactory)
                .ResubmitWorkItemsAsync(new HelixJobInfo("original-job", "finished"), [WorkItem("WORK-A"), WorkItem("work-b")], CancellationToken.None);

            Assert.Equal("new-job", result.JobName);
            Assert.Equal("running", result.Status);
            Assert.Equal("custom run", result.TestRunName);
            Assert.Equal("test stage", result.StageName);
            Assert.Equal("original-job", result.PreviousHelixJobName);

            Assert.Equal("https://storage/job-list.json", blobClientFactory.DownloadedTextUri);
            UploadCall upload = Assert.Single(blobClientFactory.Uploads);
            Assert.Equal(new Uri("https://account.blob.core.windows.net/container"), upload.ContainerUri);
            Assert.Equal("?write", upload.SasToken);
            Assert.StartsWith("job-list-", upload.BlobName, StringComparison.Ordinal);
            Assert.EndsWith(".json", upload.BlobName, StringComparison.Ordinal);
            JArray uploadedEntries = JArray.Parse(upload.Content);
            Assert.Equal(["work-a", "work-b"], uploadedEntries.Select(e => (string)e["WorkItemId"]).ToArray());

            Assert.NotNull(capturedRequest);
            Assert.False(string.IsNullOrEmpty(capturedIdempotencyKey));
            Assert.Equal("helix-type", capturedRequest.Type);
            Assert.Equal("queue-id", capturedRequest.QueueId);
            Assert.Equal("https://account.blob.core.windows.net/container/job-list.json?read", capturedRequest.ListUri);
            Assert.Equal("source", capturedRequest.Source);
            Assert.Equal("creator", capturedRequest.Creator);
            Assert.Equal("123", capturedRequest.Properties["BuildId"]);
            Assert.Equal("custom run", capturedRequest.Properties["TestRunName"]);
            Assert.Equal("test stage", capturedRequest.Properties["System.StageName"]);
            Assert.Equal("original-job", capturedRequest.Properties[HelixJobInfo.PreviousHelixJobNamePropertyName]);
            Assert.Equal("""{"nested":true}""", capturedRequest.Properties["ObjectProperty"]);

            api.Storage.Verify(s => s.NewAsync(
                It.Is<ContainerCreationRequest>(r => r.ExpirationInDays == 30 && r.DesiredName == "joblists" && r.TargetQueue == "queue-id"),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static HelixService CreateService(IHelixApi api, IBlobClientFactory blobClientFactory = null, IFileSystem fileSystem = null)
            => new(
                api,
                NullLogger.Instance,
                blobClientFactory ?? new FakeBlobClientFactory(),
                fileSystem ?? new MockFileSystem(directorySeparator: Path.DirectorySeparatorChar.ToString()));

        private static ApiMocks CreateApi()
        {
            var api = new Mock<IHelixApi>(MockBehavior.Strict);
            var job = new Mock<IJob>(MockBehavior.Strict);
            var workItem = new Mock<IWorkItem>(MockBehavior.Strict);
            var storage = new Mock<IStorage>(MockBehavior.Strict);
            api.SetupGet(a => a.Job).Returns(job.Object);
            api.SetupGet(a => a.WorkItem).Returns(workItem.Object);
            api.SetupGet(a => a.Storage).Returns(storage.Object);
            return new ApiMocks(api, job, workItem, storage);
        }

        private static JobSummary Job(string name, string finished, JToken properties)
            => new("details", name, "wait", "source", "type", "build")
            {
                Finished = finished,
                Properties = properties,
            };

        private static JobDetails JobDetails()
            => new("https://storage/job-list.json", null, "original-job", "wait", "source", "helix-type", "build")
            {
                Creator = "creator",
                QueueId = "queue-id",
                Properties = new JObject
                {
                    ["BuildId"] = "123",
                    ["TestRunName"] = "custom run",
                    ["System.StageName"] = "test stage",
                    ["ObjectProperty"] = new JObject
                    {
                        ["nested"] = true,
                    },
                    ["NullProperty"] = JValue.CreateNull(),
                },
            };

        private static WorkItemSummary WorkItem(string name)
            => new($"details/{name}", "original-job", name, "Finished") { ExitCode = 1 };

        private static string SanitizeForCurrentPlatform(string value)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(invalidChar, '-');
            }

            return value;
        }

        private static string NormalizeForCurrentPlatform(string path)
            => path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        private sealed record ApiMocks(
            Mock<IHelixApi> Api,
            Mock<IJob> Job,
            Mock<IWorkItem> WorkItem,
            Mock<IStorage> Storage);

        private sealed record DownloadCall(string BlobUri, string ResultsSas, string DestinationFile);

        private sealed record UploadCall(Uri ContainerUri, string BlobName, string SasToken, string Content);

        private sealed class FakeBlobClientFactory : IBlobClientFactory
        {
            public List<DownloadCall> Downloads { get; } = [];

            public HashSet<string> FailDownloadsFor { get; } = new(StringComparer.OrdinalIgnoreCase);

            public string DownloadedText { get; set; } = "[]";

            public string DownloadedTextUri { get; private set; }

            public List<UploadCall> Uploads { get; } = [];

            public Uri UploadedBlobUri { get; set; } = new("https://storage/uploaded-job-list.json");

            public IBlobClient CreateBlobClient(string blobUri, string sasToken = null)
            {
                return new FakeBlobClient(new Uri(blobUri), this, blobUri, sasToken);
            }

            public IBlobClient CreateBlobClient(Uri containerUri, string blobName, string sasToken)
            {
                return new FakeBlobClient(UploadedBlobUri, this, containerUri, sasToken, blobName);
            }

            private sealed class FakeBlobClient : IBlobClient
            {
                private readonly FakeBlobClientFactory _factory;
                private readonly string _blobUri;
                private readonly string _sasToken;
                private readonly Uri _containerUri;
                private readonly string _blobName;

                public FakeBlobClient(Uri uri, FakeBlobClientFactory factory, string blobUri, string sasToken)
                {
                    Uri = uri;
                    _factory = factory;
                    _blobUri = blobUri;
                    _sasToken = sasToken;
                }

                public FakeBlobClient(Uri uri, FakeBlobClientFactory factory, Uri containerUri, string sasToken, string blobName)
                {
                    Uri = uri;
                    _factory = factory;
                    _containerUri = containerUri;
                    _sasToken = sasToken;
                    _blobName = blobName;
                }

                public Uri Uri { get; }

                public Task DownloadToAsync(string destinationFile, CancellationToken cancellationToken)
                {
                    _factory.Downloads.Add(new DownloadCall(_blobUri, _sasToken, destinationFile));
                    if (_factory.FailDownloadsFor.Contains(_blobUri))
                    {
                        throw new InvalidOperationException("Injected download failure.");
                    }

                    return Task.CompletedTask;
                }

                public Task<BinaryData> DownloadContentAsync(CancellationToken cancellationToken)
                {
                    _factory.DownloadedTextUri = _blobUri;
                    return Task.FromResult(BinaryData.FromString(_factory.DownloadedText));
                }

                public Task UploadAsync(BinaryData content, bool overwrite, CancellationToken cancellationToken)
                {
                    Assert.True(overwrite);
                    _factory.Uploads.Add(new UploadCall(_containerUri, _blobName, _sasToken, content.ToString()));
                    return Task.CompletedTask;
                }
            }
        }
    }
}
