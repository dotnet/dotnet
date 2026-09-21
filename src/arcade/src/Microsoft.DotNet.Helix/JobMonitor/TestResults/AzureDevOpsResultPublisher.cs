// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

internal sealed class AzureDevOpsResultPublisher : IAzureDevOpsResultPublisher
{
    // Azure DevOps rejects requests containing more than 1,000 top-level TestCaseResult objects.
    // Nested sub-results do not count toward this limit.
    private const int MaximumResultsPerRequest = 1000;

    // Preserve the legacy bound on the recursive size of one result hierarchy independently
    // from the top-level per-request limit.
    private const int MaximumNodesPerResultHierarchy = 950;

    private readonly ILogger _logger;
    private readonly bool _useFullyQualifiedTestName;
    private readonly JobMonitorMetrics _metrics;
    private readonly IAzureDevOpsResultTransport _transport;

    internal AzureDevOpsResultPublisher(
        ILogger logger,
        bool useFullyQualifiedTestName,
        IAzureDevOpsResultTransport transport,
        JobMonitorMetrics? metrics = null)
    {
        _logger = logger;
        _useFullyQualifiedTestName = useFullyQualifiedTestName;
        _transport = transport;
        _metrics = metrics ?? new JobMonitorMetrics();
    }

    public Task<long> PublishAsync(
        int testRunId,
        WorkItemTestResults results,
        PreparedTestResults prepared,
        CancellationToken cancellationToken)
        => PublishTestResultsAsync(
            prepared,
            results.WorkItemName,
            results.JobName,
            testRunId,
            cancellationToken);

    private async Task<long> PublishTestResultsAsync(
        PreparedTestResults prepared,
        string workItemName,
        string jobId,
        int testRunId,
        CancellationToken cancellationToken)
    {
        if (prepared.Results.Count == 0)
        {
            return 0;
        }

        long publishStartedAt = JobMonitorMetrics.StartOperation();
        try
        {
            return await UploadTestResultsWithCountAsync(
                prepared.Results,
                workItemName,
                jobId,
                testRunId,
                cancellationToken);
        }
        finally
        {
            _metrics.RecordPipelineOperation(PipelineOperation.WorkItemPublish, publishStartedAt);
        }
    }

    private async Task<long> UploadTestResultsWithCountAsync(
        IEnumerable<AggregatedResult> results,
        string workItemName,
        string jobId,
        int testRunId,
        CancellationToken cancellationToken)
    {
        try
        {
            long publishedTestCount = 0;
            foreach (List<ConvertedResult> requestBatch in CreateResultRequestBatches(ConvertResults(results, workItemName, jobId)))
            {
                IReadOnlyList<PublishedTestCase> publishedTests =
                    await PublishResultsAsync(requestBatch, testRunId, cancellationToken);
                publishedTestCount += publishedTests.Count;
            }

            _logger.LogDebug("Uploaded {Count} results", publishedTestCount);

            return publishedTestCount;
        }
        catch (TerminalError ex)
        {
            _logger.LogError(ex, "Failed to upload test results to Azure DevOps.");
            throw;
        }
    }

    private async Task<IReadOnlyList<PublishedTestCase>> PublishResultsAsync(
        IReadOnlyList<ConvertedResult> converted,
        int testRunId,
        CancellationToken cancellationToken)
    {
        var testCaseResults = converted.Select(static c => c.Converted).ToList();
        var originalList = converted.Select(static c => c.Aggregated).ToList();

        string response = await _transport.PublishResultsAsync(testRunId, testCaseResults, cancellationToken);
        IReadOnlyList<PublishedTestCaseResultReference> publishedResults = ReadPublishedResults(response);
        if (publishedResults.Count == 0)
        {
            _logger.LogWarning("The test run appears to have been closed, aborting test result uploads.");
            return [];
        }

        List<PublishedTestCase> publishedTestCases = [];

        foreach ((PublishedTestCaseResultReference published, AggregatedResult original, PublishedTestCase testCase) in publishedResults.Zip(originalList, testCaseResults))
        {
            if (published.Id == -1)
            {
                _logger.LogWarning("Azure DevOps test ID returned -1, unable to attach files.");
                continue;
            }

            async Task IterateSubResultsAsync(
                IReadOnlyList<PublishedSubResultReference>? publishedSubResults,
                IReadOnlyList<AggregatedResult> originalSubResults,
                long testId)
            {
                if (publishedSubResults is null || publishedSubResults.Count == 0)
                {
                    if (originalSubResults.Count > 0)
                    {
                        _logger.LogError("Published results do not include sub-results, attachments lost.");
                    }

                    return;
                }

                if (publishedSubResults.Count != originalSubResults.Count)
                {
                    _logger.LogError("Published sub-result counts do not match uploaded attachments. Attachments lost.");
                    return;
                }

                foreach ((PublishedSubResultReference publishedSubResult, AggregatedResult originalSubResult) subTriplet in publishedSubResults.Zip(originalSubResults, (publishedSubResult, originalSubResult) => (publishedSubResult, originalSubResult)))
                {
                    foreach (TestResultAttachment attachment in subTriplet.originalSubResult.Attachments)
                    {
                        await SendAttachmentAsync(
                            attachment,
                            testRunId,
                            testId,
                            subTriplet.publishedSubResult.Id,
                            cancellationToken);
                    }

                    await IterateSubResultsAsync(subTriplet.publishedSubResult.SubResults, subTriplet.originalSubResult.SubResults, testId);
                }
            }

            foreach (TestResultAttachment attachment in original.Attachments)
            {
                await SendAttachmentAsync(
                    attachment,
                    testRunId,
                    published.Id,
                    null,
                    cancellationToken);
            }

            await IterateSubResultsAsync(published.SubResults, original.SubResults, published.Id);

            publishedTestCases.Add(testCase);
        }

        return publishedTestCases;
    }

    private async Task SendAttachmentAsync(
        TestResultAttachment attachment,
        int testRunId,
        long testId,
        long? subResultId,
        CancellationToken cancellationToken)
    {
        await _transport.UploadAttachmentAsync(
            testRunId,
            testId,
            subResultId,
            attachment.Name,
            Convert.ToBase64String(Encoding.UTF8.GetBytes(attachment.Text)),
            cancellationToken);
    }

    private IEnumerable<ConvertedResult> ConvertResults(
        IEnumerable<AggregatedResult> results,
        string workItemName,
        string jobId)
    {
        static string GetResultGroupType(AggregationType aggregationType)
        {
            return aggregationType switch
            {
                AggregationType.Single => "None",
                AggregationType.DataDriven => "dataDriven",
                AggregationType.Rerun => "rerun",
                _ => "None",
            };
        }

        string comment = JsonSerializer.Serialize(new
        {
            HelixJobId = jobId,
            HelixWorkItemName = workItemName,
        });
        bool useFullyQualifiedName = _useFullyQualifiedTestName;

        string DisplayNameFor(AggregatedResult result, bool isDataDrivenSubResult)
            => useFullyQualifiedName
                ? TestNameFormatter.FormatDisplayName(result.FullyQualifiedName, result.Name, isDataDrivenSubResult)
                : result.Name;

        PublishedSubResult ConvertToSubTest(AggregatedResult result, bool isDataDrivenSubResult)
        {
            var customFields = new List<CustomField>();
            if (result.IsFlaky)
            {
                customFields.Add(new CustomField("IsTestResultFlaky", true));
            }

            if ((result.AttemptId ?? 0) > 1)
            {
                customFields.Add(new CustomField("AttemptId", result.AttemptId!.Value - 1));
            }

            return new PublishedSubResult
            {
                Comment = comment,
                CustomFields = customFields,
                DisplayName = DisplayNameFor(result, isDataDrivenSubResult),
                Outcome = result.Result,
                DurationInMs = result.DurationSeconds * 1000.0,
                StackTrace = result.StackTrace,
                ErrorMessage = result.FailureMessage,
                SubResults = result.SubResults.Count == 0
                    ? null
                    : [.. result.SubResults.Select(subResult => ConvertToSubTest(
                        subResult,
                        result.AggregationType == AggregationType.DataDriven))],
                ResultGroupType = GetResultGroupType(result.AggregationType),
            };
        }

        ConvertedResult ConvertResult(AggregatedResult result)
        {
            var customFields = new List<CustomField>();
            if (result.IsFlaky)
            {
                customFields.Add(new CustomField("IsTestResultFlaky", true));
            }

            if (result.AggregationType == AggregationType.Rerun && result.SubResults.Count > 1)
            {
                customFields.Add(new CustomField("AttemptId", result.SubResults.Count - 1));
            }

            string displayName = DisplayNameFor(result, isDataDrivenSubResult: false);

            return new ConvertedResult(
                new PublishedTestCase
                {
                    TestCaseTitle = displayName,
                    AutomatedTestName = useFullyQualifiedName ? result.FullyQualifiedName : result.Name,
                    AutomatedTestType = "helix",
                    // Azure DevOps uses this as part of the test definition identity. The work item
                    // name must remain stable across builds so existing test references are reused.
                    AutomatedTestStorage = workItemName,
                    Priority = 1,
                    DurationInMs = result.DurationSeconds * 1000.0,
                    Outcome = result.Result,
                    State = "Completed",
                    Comment = comment,
                    StackTrace = result.StackTrace,
                    ErrorMessage = result.FailureMessage,
                    SubResults = result.SubResults.Count == 0
                        ? null
                        : [.. result.SubResults.Select(subResult => ConvertToSubTest(
                            subResult,
                            result.AggregationType == AggregationType.DataDriven))],
                    ResultGroupType = GetResultGroupType(result.AggregationType),
                    CustomFields = customFields,
                },
                result);
        }

        foreach (AggregatedResult result in results)
        {
            foreach (ConvertedResult hierarchyPart in SplitOversizedResultHierarchy(
                ConvertResult(result),
                MaximumNodesPerResultHierarchy))
            {
                yield return hierarchyPart;
            }
        }
    }

    /// <summary>
    /// Groups converted top-level results into Azure DevOps request bodies. Each item counts
    /// once regardless of how many nested sub-results it contains.
    /// </summary>
    private static IEnumerable<List<ConvertedResult>> CreateResultRequestBatches(
        IEnumerable<ConvertedResult> results)
        => PartitionBySize(results, MaximumResultsPerRequest, static _ => 1);

    /// <summary>
    /// Splits one logical data-driven or rerun result into multiple top-level payload entries
    /// when its recursive hierarchy exceeds <paramref name="maximumNodesPerHierarchy"/>.
    /// This is separate from grouping top-level entries into Azure DevOps request batches.
    /// </summary>
    private static IEnumerable<ConvertedResult> SplitOversizedResultHierarchy(
        ConvertedResult test,
        int maximumNodesPerHierarchy)
    {
        if (CountResultTreeNodes(test.Converted) <= maximumNodesPerHierarchy)
        {
            yield return test;
            yield break;
        }

        if (maximumNodesPerHierarchy <= 1 || test.Converted.SubResults is not { Count: > 0 })
        {
            throw new InvalidOperationException(
                "A test-result hierarchy is deeper than the Azure DevOps hierarchy limit.");
        }

        IEnumerable<ChunkPair> splitSubTests = (test.Converted.SubResults ?? [])
            .Zip(test.Aggregated.SubResults, (converted, aggregated) => new ChunkPair(converted, aggregated))
            .SelectMany(pair => SplitOversizedSubResultHierarchy(pair, maximumNodesPerHierarchy - 1));

        // Each emitted hierarchy includes the copied top-level result, leaving the remaining
        // node budget for its sub-results.
        foreach (List<ChunkPair> hierarchyPart in PartitionBySize(
            splitSubTests,
            maximumNodesPerHierarchy - 1,
            static pair => CountResultTreeNodes(pair.Converted)))
        {
            yield return new ConvertedResult(
                test.Converted with { SubResults = [.. hierarchyPart.Select(static x => x.Converted)], Id = null },
                CopyAggregatedResult(
                    test.Aggregated,
                    [.. hierarchyPart.Select(static x => x.Aggregated)]));
        }
    }

    private static IEnumerable<ChunkPair> SplitOversizedSubResultHierarchy(
        ChunkPair test,
        int maximumNodesPerHierarchy)
    {
        if (CountResultTreeNodes(test.Converted) <= maximumNodesPerHierarchy)
        {
            yield return test;
            yield break;
        }

        IEnumerable<ChunkPair> splitSubTests = (test.Converted.SubResults ?? [])
            .Zip(test.Aggregated.SubResults, (converted, aggregated) => new ChunkPair(converted, aggregated))
            .SelectMany(pair => SplitOversizedSubResultHierarchy(pair, maximumNodesPerHierarchy - 1));

        foreach (List<ChunkPair> hierarchyPart in PartitionBySize(
            splitSubTests,
            maximumNodesPerHierarchy - 1,
            static pair => CountResultTreeNodes(pair.Converted)))
        {
            yield return new ChunkPair(
                test.Converted with
                {
                    SubResults = [.. hierarchyPart.Select(static x => x.Converted)],
                    Id = null,
                },
                CopyAggregatedResult(
                    test.Aggregated,
                    [.. hierarchyPart.Select(static x => x.Aggregated)]));
        }
    }

    private static AggregatedResult CopyAggregatedResult(
        AggregatedResult result,
        IReadOnlyList<AggregatedResult> subResults)
        => new(
            result.AggregationType,
            result.Name,
            result.DurationSeconds,
            result.Result,
            subResults,
            result.Attachments,
            result.FailureMessage,
            result.StackTrace,
            isFlaky: result.IsFlaky,
            attemptId: result.AttemptId,
            fullyQualifiedName: result.FullyQualifiedName);

    private static int CountResultTreeNodes(PublishedTestCase test)
    {
        return 1 + (test.SubResults?.Sum(CountResultTreeNodes) ?? 0);
    }

    private static int CountResultTreeNodes(PublishedSubResult test)
    {
        return 1 + (test.SubResults?.Sum(CountResultTreeNodes) ?? 0);
    }

    /// <summary>
    /// Partitions items in order so the sum of <paramref name="getSize"/> values in each
    /// partition does not exceed <paramref name="maximumPartitionSize"/>.
    /// </summary>
    private static IEnumerable<List<T>> PartitionBySize<T>(
        IEnumerable<T> items,
        int maximumPartitionSize,
        Func<T, int> getSize)
    {
        var currentPartition = new List<T>();
        int currentSize = 0;

        foreach (T? item in items)
        {
            int size = getSize(item);
            if (size > maximumPartitionSize)
            {
                throw new InvalidOperationException("Cannot partition an item larger than the size limit.");
            }

            if (currentSize + size > maximumPartitionSize && currentPartition.Count > 0)
            {
                yield return currentPartition;
                currentPartition = [];
                currentSize = 0;
            }

            currentPartition.Add(item);
            currentSize += size;
        }

        if (currentPartition.Count > 0)
        {
            yield return currentPartition;
        }
    }

    private static IReadOnlyList<PublishedTestCaseResultReference> ReadPublishedResults(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return [];
        }

        using var document = JsonDocument.Parse(content);
        JsonElement root = document.RootElement;
        if (root.ValueKind == JsonValueKind.Array)
        {
            return [.. root.EnumerateArray().Select(ParsePublishedResult)];
        }

        if (root.TryGetProperty("value", out JsonElement value) && value.ValueKind == JsonValueKind.Array)
        {
            return [.. value.EnumerateArray().Select(ParsePublishedResult)];
        }

        return [];
    }

    private static PublishedTestCaseResultReference ParsePublishedResult(JsonElement element)
    {
        var subResults = new List<PublishedSubResultReference>();
        if (element.TryGetProperty("subResults", out JsonElement subResultElement) && subResultElement.ValueKind == JsonValueKind.Array)
        {
            subResults.AddRange(subResultElement.EnumerateArray().Select(ParsePublishedSubResult));
        }

        return new PublishedTestCaseResultReference(
            element.TryGetProperty("id", out JsonElement idElement) ? idElement.GetInt64() : -1,
            subResults);
    }

    private static PublishedSubResultReference ParsePublishedSubResult(JsonElement element)
    {
        var subResults = new List<PublishedSubResultReference>();
        if (element.TryGetProperty("subResults", out JsonElement subResultElement) && subResultElement.ValueKind == JsonValueKind.Array)
        {
            subResults.AddRange(subResultElement.EnumerateArray().Select(ParsePublishedSubResult));
        }

        return new PublishedSubResultReference(
            element.TryGetProperty("id", out JsonElement idElement) ? idElement.GetInt64() : -1,
            subResults);
    }

    private sealed record ConvertedResult(PublishedTestCase Converted, AggregatedResult Aggregated);

    private sealed record ChunkPair(PublishedSubResult Converted, AggregatedResult Aggregated);

    private sealed record CustomField(string FieldName, object Value);

    private sealed record PublishedTestCase
    {
        public long? Id { get; init; }

        public string TestCaseTitle { get; init; } = string.Empty;

        public string AutomatedTestName { get; init; } = string.Empty;

        public string AutomatedTestType { get; init; } = string.Empty;

        public string AutomatedTestStorage { get; init; } = string.Empty;

        public int Priority { get; init; }

        public double DurationInMs { get; init; }

        public string Outcome { get; init; } = string.Empty;

        public string State { get; init; } = string.Empty;

        public string Comment { get; init; } = string.Empty;

        public string? StackTrace { get; init; }

        public string? ErrorMessage { get; init; }

        public List<PublishedSubResult>? SubResults { get; init; }

        public string ResultGroupType { get; init; } = string.Empty;

        public List<CustomField>? CustomFields { get; init; }
    }

    private sealed record PublishedSubResult
    {
        public long? Id { get; init; }

        public string Comment { get; init; } = string.Empty;

        public List<CustomField>? CustomFields { get; init; }

        public string DisplayName { get; init; } = string.Empty;

        public string Outcome { get; init; } = string.Empty;

        public double DurationInMs { get; init; }

        public string? StackTrace { get; init; }

        public string? ErrorMessage { get; init; }

        public List<PublishedSubResult>? SubResults { get; init; }

        public string ResultGroupType { get; init; } = string.Empty;
    }

    private sealed record PublishedTestCaseResultReference(long Id, IReadOnlyList<PublishedSubResultReference> SubResults);

    private sealed record PublishedSubResultReference(long Id, IReadOnlyList<PublishedSubResultReference> SubResults);
}
