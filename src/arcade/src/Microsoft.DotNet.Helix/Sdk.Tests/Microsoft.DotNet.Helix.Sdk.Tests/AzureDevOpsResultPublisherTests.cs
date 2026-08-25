// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class AzureDevOpsResultPublisherTests
    {
        [Fact]
        public void AttachmentModeDefaultsToFailed()
        {
            var reportingParameters = new AzureDevOpsReportingParameters(
                new Uri("https://dev.azure.com/dnceng-public/"),
                "public",
                "123");

            Assert.Equal(TestResultAttachmentMode.Failed, reportingParameters.TestResultAttachmentMode);
            Assert.Equal(TestResultAttachmentMode.Failed, new JobMonitorOptions().TestResultAttachmentMode);
        }

        [Fact]
        public void JobMonitorUploadParallelismDefaultsToFortyEight()
        {
            Assert.Equal(48, new JobMonitorOptions().TestResultUploadParallelism);
        }

        [Theory]
        [InlineData("Passed", true)]
        [InlineData("NotExecuted", true)]
        [InlineData("Inconclusive", true)]
        [InlineData("Failed", false)]
        [InlineData("None", false)]
        public void ComputeAllPassed_SingleResult_OnlyFailedAndNoneCountAsFailure(string result, bool expectedAllPassed)
        {
            var results = new[] { new AggregatedResult(AggregationType.Single, "Test1", 1, result) };
            Assert.Equal(expectedAllPassed, AzureDevOpsResultPublisher.ComputeAllPassed(results));
        }

        [Fact]
        public void ComputeAllPassed_InconclusiveDataDrivenRollup_DoesNotFailTheWorkItem()
        {
            AggregatedResult[] results =
            [
                new(AggregationType.Single, "Test1", 1, "Passed"),
                new(AggregationType.DataDriven, "Test2", 1, "Inconclusive"),
            ];

            Assert.True(AzureDevOpsResultPublisher.ComputeAllPassed(results));
        }

        [Fact]
        public void ComputeAllPassed_AnyFailedResult_FailsTheWorkItem()
        {
            AggregatedResult[] results =
            [
                new(AggregationType.Single, "Test1", 1, "Passed"),
                new(AggregationType.DataDriven, "Test2", 1, "Failed"),
            ];

            Assert.False(AzureDevOpsResultPublisher.ComputeAllPassed(results));
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_BatchesByTopLevelResultCount()
        {
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport);
            AggregatedResult[] results =
            [
                CreateDataDrivenResult("First", 600),
                CreateDataDrivenResult("Second", 600),
            ];

            long uploadedCount = await publisher.UploadTestResultsWithCountAsync(results, new { });

            Assert.Equal(2, uploadedCount);
            Assert.Equal(new[] { 2 }, transport.RequestResultCounts);
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_SplitsMoreThanOneThousandTopLevelResults()
        {
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport);
            AggregatedResult[] results =
            [
                .. Enumerable.Range(0, 1001)
                    .Select(i => new AggregatedResult(AggregationType.Single, $"Test{i}", 1, "Passed"))
            ];

            long uploadedCount = await publisher.UploadTestResultsWithCountAsync(results, new { });

            Assert.Equal(1001, uploadedCount);
            Assert.Equal(new[] { 1000, 1 }, transport.RequestResultCounts);
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_SplitHierarchiesIncludeRootInNodeLimit()
        {
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport);

            long uploadedCount = await publisher.UploadTestResultsWithCountAsync(
                [CreateDataDrivenResult("Theory", 950)],
                new { });

            Assert.Equal(2, uploadedCount);
            Assert.Equal(new[] { 2 }, transport.RequestResultCounts);
            Assert.Equal(new[] { 950, 2 }, transport.RequestHierarchyNodeCounts.Single());
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_RecursivelySplitsOversizedNestedHierarchies()
        {
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport);
            var nested = CreateDataDrivenResult("Nested", 950);
            AggregatedResult[] results =
            [
                new(AggregationType.DataDriven, "Outer", 1, "Passed", [nested]),
            ];

            long uploadedCount = await publisher.UploadTestResultsWithCountAsync(results, new { });

            Assert.Equal(2, uploadedCount);
            Assert.Equal(new[] { 950, 4 }, transport.RequestHierarchyNodeCounts.Single());
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_DoesNotMaterializeAllConvertedResults()
        {
            var transport = new BlockingResultTransport();
            var publisher = CreatePublisher(transport);
            int enumerated = 0;

            IEnumerable<AggregatedResult> Results()
            {
                for (int i = 0; i < 2_000; i++)
                {
                    enumerated++;
                    yield return new AggregatedResult(AggregationType.Single, $"Test{i}", 1, "Passed");
                }
            }

            Task<long> upload = publisher.UploadTestResultsWithCountAsync(Results(), new { });
            await transport.FirstRequestStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));

            Assert.InRange(enumerated, 0, 1001);
            transport.ReleaseFirstRequest.SetResult();

            Assert.Equal(2_000, await upload);
            Assert.Equal(2, transport.RequestResultCounts.Count);
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_UsesSemanticAttachmentTransport()
        {
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport);
            var result = new AggregatedResult(
                AggregationType.Single,
                "Test",
                1,
                "Failed",
                attachments: [new TestResultAttachment("failure.txt", "details")]);

            Assert.Equal(1, await publisher.UploadTestResultsWithCountAsync([result], new { }));

            ResultAttachment attachment = Assert.Single(transport.Attachments);
            Assert.Equal(1, attachment.TestResultId);
            Assert.Null(attachment.TestSubResultId);
            Assert.Equal("failure.txt", attachment.FileName);
            Assert.Equal("details", System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(attachment.Stream)));
        }

        private static AzureDevOpsResultPublisher CreatePublisher(IAzureDevOpsResultTransport transport)
            => new(
                TestResultAttachmentMode.Failed,
                useFullyQualifiedTestName: false,
                NullLogger.Instance,
                transport);

        private static AggregatedResult CreateDataDrivenResult(string name, int subResultCount)
            => new(
                AggregationType.DataDriven,
                name,
                subResultCount,
                "Passed",
                [
                    .. Enumerable.Range(0, subResultCount)
                        .Select(i => new AggregatedResult(AggregationType.Single, $"{name}_{i}", 1, "Passed"))
                ]);

        private class RecordingResultTransport : IAzureDevOpsResultTransport
        {
            public List<int> RequestResultCounts { get; } = [];
            public List<int[]> RequestHierarchyNodeCounts { get; } = [];
            public List<ResultAttachment> Attachments { get; } = [];

            public virtual Task<string> PublishResultsAsync(object results, CancellationToken cancellationToken)
            {
                using JsonDocument requestBody = JsonDocument.Parse(JsonSerializer.Serialize(results));
                int resultCount = requestBody.RootElement.GetArrayLength();
                RequestResultCounts.Add(resultCount);
                RequestHierarchyNodeCounts.Add(
                    [.. requestBody.RootElement.EnumerateArray().Select(CountHierarchyNodes)]);

                return Task.FromResult(JsonSerializer.Serialize(new
                {
                    value = Enumerable.Range(1, resultCount).Select(id => new { id })
                }));
            }

            public Task UploadAttachmentAsync(
                long testResultId,
                long? testSubResultId,
                string fileName,
                string stream,
                CancellationToken cancellationToken)
            {
                Attachments.Add(new(testResultId, testSubResultId, fileName, stream));
                return Task.CompletedTask;
            }

            private static int CountHierarchyNodes(JsonElement result)
            {
                if (!result.TryGetProperty("SubResults", out JsonElement subResults) &&
                    !result.TryGetProperty("subResults", out subResults))
                {
                    return 1;
                }

                return subResults.ValueKind == JsonValueKind.Array
                    ? 1 + subResults.EnumerateArray().Sum(CountHierarchyNodes)
                    : 1;
            }
        }

        private sealed class BlockingResultTransport : RecordingResultTransport
        {
            public TaskCompletionSource FirstRequestStarted { get; } =
                new(TaskCreationOptions.RunContinuationsAsynchronously);
            public TaskCompletionSource ReleaseFirstRequest { get; } =
                new(TaskCreationOptions.RunContinuationsAsynchronously);
            private int _requestCount;

            public override async Task<string> PublishResultsAsync(object results, CancellationToken cancellationToken)
            {
                if (Interlocked.Increment(ref _requestCount) == 1)
                {
                    FirstRequestStarted.SetResult();
                    await ReleaseFirstRequest.Task.WaitAsync(cancellationToken);
                }

                return await base.PublishResultsAsync(results, cancellationToken);
            }
        }

        private sealed record ResultAttachment(
            long TestResultId,
            long? TestSubResultId,
            string FileName,
            string Stream);
    }
}
