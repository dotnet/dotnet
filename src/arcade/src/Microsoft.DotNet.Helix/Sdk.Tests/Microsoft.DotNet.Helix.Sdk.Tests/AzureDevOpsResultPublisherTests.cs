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
        private const int TestRunId = 123;

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
            Assert.Equal(expectedAllPassed, TestResultProcessor.ComputeAllPassed(results));
        }

        [Fact]
        public void ComputeAllPassed_InconclusiveDataDrivenRollup_DoesNotFailTheWorkItem()
        {
            AggregatedResult[] results =
            [
                new(AggregationType.Single, "Test1", 1, "Passed"),
                new(AggregationType.DataDriven, "Test2", 1, "Inconclusive"),
            ];

            Assert.True(TestResultProcessor.ComputeAllPassed(results));
        }

        [Fact]
        public void ComputeAllPassed_AnyFailedResult_FailsTheWorkItem()
        {
            AggregatedResult[] results =
            [
                new(AggregationType.Single, "Test1", 1, "Passed"),
                new(AggregationType.DataDriven, "Test2", 1, "Failed"),
            ];

            Assert.False(TestResultProcessor.ComputeAllPassed(results));
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

            long uploadedCount = await PublishAsync(publisher, results);

            Assert.Equal(2, uploadedCount);
            Assert.Equal(new[] { 2 }, transport.RequestResultCounts);
            Assert.Equal([TestRunId], transport.TestRunIds);
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

            long uploadedCount = await PublishAsync(publisher, results);

            Assert.Equal(1001, uploadedCount);
            Assert.Equal(new[] { 1000, 1 }, transport.RequestResultCounts);
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_SplitHierarchiesIncludeRootInNodeLimit()
        {
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport);

            long uploadedCount = await PublishAsync(
                publisher,
                [CreateDataDrivenResult("Theory", 950)]);

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

            long uploadedCount = await PublishAsync(publisher, results);

            Assert.Equal(2, uploadedCount);
            Assert.Equal(new[] { 950, 4 }, transport.RequestHierarchyNodeCounts.Single());
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_DoesNotMaterializeAllConvertedResults()
        {
            var transport = new BlockingResultTransport();
            var publisher = CreatePublisher(transport);
            var results = new TrackingResultList(2_000);

            Task<long> upload = PublishAsync(publisher, results);
            await transport.FirstRequestStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));

            Assert.InRange(results.EnumeratedCount, 0, 1001);
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

            Assert.Equal(1, await PublishAsync(publisher, [result]));

            ResultAttachment attachment = Assert.Single(transport.Attachments);
            Assert.Equal(1, attachment.TestResultId);
            Assert.Null(attachment.TestSubResultId);
            Assert.Equal("failure.txt", attachment.FileName);
            Assert.Equal("details", System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(attachment.Stream)));
        }

        [Fact]
        public async Task UploadTestResultsWithCountAsync_UsesStableWorkItemNameAsTestStorage()
        {
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport);
            var result = new AggregatedResult(AggregationType.Single, "Test", 1, "Passed");

            await PublishAsync(
                publisher,
                [result],
                "work-item",
                "job-a");
            await PublishAsync(
                publisher,
                [result],
                "work-item",
                "job-b");

            JsonElement firstResult = Assert.Single(transport.RequestBodies[0].EnumerateArray());
            JsonElement secondResult = Assert.Single(transport.RequestBodies[1].EnumerateArray());

            Assert.Equal("work-item", firstResult.GetProperty("AutomatedTestStorage").GetString());
            Assert.Equal("work-item", secondResult.GetProperty("AutomatedTestStorage").GetString());
            Assert.Contains("job-a", firstResult.GetProperty("Comment").GetString());
            Assert.Contains("job-b", secondResult.GetProperty("Comment").GetString());
        }

        [Fact]
        public async Task FullyQualifiedNames_DataDrivenRowsUseShortChildDisplayNames()
        {
            const string fullyQualifiedName =
                "Microsoft.DotNet.Cli.New.IntegrationTests.CommonTemplatesTests.FeaturesSupport";
            const string dataRowName = "FeaturesSupport(\"classlib\",True,\"netstandard2.0\")";
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport, useFullyQualifiedTestName: true);
            var result = new AggregatedResult(
                AggregationType.DataDriven,
                fullyQualifiedName,
                1,
                "Passed",
                [new AggregatedResult(
                    AggregationType.Single,
                    dataRowName,
                    1,
                    "Passed",
                    fullyQualifiedName: fullyQualifiedName)],
                fullyQualifiedName: fullyQualifiedName);

            await PublishAsync(publisher, [result]);

            JsonElement publishedTest = Assert.Single(Assert.Single(transport.RequestBodies).EnumerateArray());
            Assert.Equal(fullyQualifiedName, publishedTest.GetProperty("TestCaseTitle").GetString());
            JsonElement dataRow = Assert.Single(publishedTest.GetProperty("SubResults").EnumerateArray());
            Assert.Equal(dataRowName, dataRow.GetProperty("DisplayName").GetString());
        }

        [Fact]
        public async Task FullyQualifiedNames_DataDrivenRerunRowsOnlyShortenDirectChildren()
        {
            const string fullyQualifiedName = "Ns.MyTests.FeaturesSupport";
            const string dataRowName = "FeaturesSupport(\"classlib\")";
            var transport = new RecordingResultTransport();
            var publisher = CreatePublisher(transport, useFullyQualifiedTestName: true);
            var attempt = new AggregatedResult(
                AggregationType.Single,
                $"Attempt #1 - {dataRowName}",
                1,
                "Passed",
                attemptId: 1,
                fullyQualifiedName: fullyQualifiedName);
            var rerunRow = new AggregatedResult(
                AggregationType.Rerun,
                dataRowName,
                1,
                "Passed",
                [attempt],
                fullyQualifiedName: fullyQualifiedName);
            var result = new AggregatedResult(
                AggregationType.DataDriven,
                fullyQualifiedName,
                1,
                "Passed",
                [rerunRow],
                fullyQualifiedName: fullyQualifiedName);

            await PublishAsync(publisher, [result]);

            JsonElement publishedTest = Assert.Single(Assert.Single(transport.RequestBodies).EnumerateArray());
            JsonElement publishedRow = Assert.Single(publishedTest.GetProperty("SubResults").EnumerateArray());
            JsonElement publishedAttempt = Assert.Single(publishedRow.GetProperty("SubResults").EnumerateArray());
            Assert.Equal(dataRowName, publishedRow.GetProperty("DisplayName").GetString());
            Assert.Equal(
                $"{fullyQualifiedName} (Attempt #1 - {dataRowName})",
                publishedAttempt.GetProperty("DisplayName").GetString());
        }

        private static AzureDevOpsResultPublisher CreatePublisher(
            IAzureDevOpsResultTransport transport,
            bool useFullyQualifiedTestName = false)
            => new(
                NullLogger.Instance,
                useFullyQualifiedTestName,
                transport);

        private static Task<long> PublishAsync(
            AzureDevOpsResultPublisher publisher,
            IReadOnlyList<AggregatedResult> results,
            string workItemName = "work-item",
            string jobId = "job")
            => publisher.PublishAsync(
                TestRunId,
                new WorkItemTestResults(jobId, workItemName, []),
                new PreparedTestResults(results, AllPassed: true),
                CancellationToken.None);

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
            public List<int> TestRunIds { get; } = [];
            public List<int> RequestResultCounts { get; } = [];
            public List<int[]> RequestHierarchyNodeCounts { get; } = [];
            public List<JsonElement> RequestBodies { get; } = [];
            public List<ResultAttachment> Attachments { get; } = [];

            public virtual Task<string> PublishResultsAsync(
                int testRunId,
                object results,
                CancellationToken cancellationToken)
            {
                TestRunIds.Add(testRunId);
                using JsonDocument requestBody = JsonDocument.Parse(JsonSerializer.Serialize(results));
                RequestBodies.Add(requestBody.RootElement.Clone());
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
                int testRunId,
                long testResultId,
                long? testSubResultId,
                string fileName,
                string stream,
                CancellationToken cancellationToken)
            {
                TestRunIds.Add(testRunId);
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

            public override async Task<string> PublishResultsAsync(
                int testRunId,
                object results,
                CancellationToken cancellationToken)
            {
                if (Interlocked.Increment(ref _requestCount) == 1)
                {
                    FirstRequestStarted.SetResult();
                    await ReleaseFirstRequest.Task.WaitAsync(cancellationToken);
                }

                return await base.PublishResultsAsync(testRunId, results, cancellationToken);
            }
        }

        private sealed class TrackingResultList(int count) : IReadOnlyList<AggregatedResult>
        {
            public int Count => count;
            public int EnumeratedCount { get; private set; }

            public AggregatedResult this[int index] => CreateResult(index);

            public IEnumerator<AggregatedResult> GetEnumerator()
            {
                for (int i = 0; i < count; i++)
                {
                    EnumeratedCount++;
                    yield return CreateResult(i);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                => GetEnumerator();

            private static AggregatedResult CreateResult(int index)
                => new(AggregationType.Single, $"Test{index}", 1, "Passed");
        }

        private sealed record ResultAttachment(
            long TestResultId,
            long? TestSubResultId,
            string FileName,
            string Stream);
    }
}
