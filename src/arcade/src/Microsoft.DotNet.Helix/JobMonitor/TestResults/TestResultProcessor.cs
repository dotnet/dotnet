// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

internal sealed class TestResultProcessor : ITestResultProcessor
{
    private readonly TestResultAttachmentMode _attachmentMode;
    private readonly bool _useFullyQualifiedTestName;
    private readonly ILogger _logger;
    private readonly JobMonitorMetrics _metrics;

    public TestResultProcessor(
        TestResultAttachmentMode attachmentMode,
        bool useFullyQualifiedTestName,
        ILogger logger,
        JobMonitorMetrics metrics)
    {
        _attachmentMode = attachmentMode;
        _useFullyQualifiedTestName = useFullyQualifiedTestName;
        _logger = logger;
        _metrics = metrics;
    }

    public async Task<PreparedTestResults> PrepareAsync(
        WorkItemTestResults results,
        CancellationToken cancellationToken)
    {
        if (results.TestResultFiles.Count == 0)
        {
            return new PreparedTestResults([], AllPassed: true);
        }

        long parseStartedAt = JobMonitorMetrics.StartOperation();
        try
        {
            var reader = new LocalTestResultsReader(_logger, _attachmentMode);
            var parsedResults = new List<IReadOnlyList<TestResult>>(results.TestResultFiles.Count);
            foreach (string file in results.TestResultFiles)
            {
                parsedResults.Add(await reader.ReadResultFileAsync(file, cancellationToken));
            }

            IReadOnlyList<AggregatedResult> aggregatedResults =
                new ResultAggregator().Aggregate(parsedResults, _useFullyQualifiedTestName);
            if (aggregatedResults.Count == 0)
            {
                _logger.LogDebug("Test results were discovered but none could be aggregated");
            }

            return new PreparedTestResults(
                aggregatedResults,
                AllPassed: ComputeAllPassed(aggregatedResults));
        }
        finally
        {
            _metrics.RecordPipelineOperation(
                PipelineOperation.ResultParseAndAggregate,
                parseStartedAt);
        }
    }

    /// <summary>
    /// A work item's results are only considered a failure when a test actually failed
    /// or could not be parsed into a known outcome ("None"). "Inconclusive" is a legitimate,
    /// non-failing outcome produced by the aggregator for data-driven tests that mix passing and
    /// skipped data rows.
    /// </summary>
    internal static bool ComputeAllPassed(IReadOnlyList<AggregatedResult> results)
        => results.All(result => result.Result != "Failed" && result.Result != "None");
}
