// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Microsoft.DotNet.Helix.JobMonitor;

internal enum AzureDevOpsRequestKind
{
    Control,
    ResultBatch,
    Attachment,
}

internal enum PipelineOperation
{
    WorkItemDownload,
    WorkItemPublish,
    TestRunCreate,
    TestRunComplete,
    ResultParseAndAggregate,
}

internal sealed class JobMonitorMetrics
{
    private readonly long _startedAt = Stopwatch.GetTimestamp();
    private long _pipelineStartedAt;
    private long _pipelineFinishedAt;
    private long _azdoControlRequests;
    private long _azdoResultRequests;
    private long _azdoAttachmentRequests;
    private long _azdoRetries;
    private long _azdoFailedAttempts;
    private long _azdoPayloadBytes;
    private long _azdoRequestTicks;
    private long _azdoMaximumRequestTicks;
    private long _helixRequests;
    private long _helixRetries;
    private long _helixFailedAttempts;
    private long _resultBlobDownloads;
    private long _resultBlobDownloadFailures;
    private long _rateLimitWaits;
    private long _rateLimitWaitTicks;
    private long _maximumRateLimitWaitTicks;
    private long _rateLimitDeferrals;
    private long _rateLimitDeferredTicks;
    private long _maximumRateLimitDeferralTicks;
    private long _workItemDownloads;
    private long _workItemDownloadTicks;
    private long _maximumWorkItemDownloadTicks;
    private long _workItemPublishes;
    private long _workItemPublishTicks;
    private long _maximumWorkItemPublishTicks;
    private long _testRunCreates;
    private long _testRunCreateTicks;
    private long _maximumTestRunCreateTicks;
    private long _testRunCompletes;
    private long _testRunCompleteTicks;
    private long _maximumTestRunCompleteTicks;
    private long _parseOperations;
    private long _parseTicks;
    private long _maximumParseTicks;

    public static long StartOperation() => Stopwatch.GetTimestamp();

    public void RecordAzureDevOpsRequest(
        AzureDevOpsRequestKind kind,
        int payloadBytes,
        bool isRetry,
        bool failed,
        long startedAt)
    {
        switch (kind)
        {
            case AzureDevOpsRequestKind.Control:
                Interlocked.Increment(ref _azdoControlRequests);
                break;
            case AzureDevOpsRequestKind.ResultBatch:
                Interlocked.Increment(ref _azdoResultRequests);
                break;
            case AzureDevOpsRequestKind.Attachment:
                Interlocked.Increment(ref _azdoAttachmentRequests);
                break;
        }

        if (isRetry)
        {
            Interlocked.Increment(ref _azdoRetries);
        }
        if (failed)
        {
            Interlocked.Increment(ref _azdoFailedAttempts);
        }

        Interlocked.Add(ref _azdoPayloadBytes, payloadBytes);
        RecordElapsed(ref _azdoRequestTicks, ref _azdoMaximumRequestTicks, startedAt);
    }

    public void RecordHelixRequest(bool isRetry, bool failed)
    {
        Interlocked.Increment(ref _helixRequests);
        if (isRetry)
        {
            Interlocked.Increment(ref _helixRetries);
        }
        if (failed)
        {
            Interlocked.Increment(ref _helixFailedAttempts);
        }
    }

    public void RecordResultBlobDownload(bool failed)
    {
        Interlocked.Increment(ref _resultBlobDownloads);
        if (failed)
        {
            Interlocked.Increment(ref _resultBlobDownloadFailures);
        }
    }

    public void RecordRateLimitWait(long startedAt)
    {
        Interlocked.Increment(ref _rateLimitWaits);
        RecordElapsed(ref _rateLimitWaitTicks, ref _maximumRateLimitWaitTicks, startedAt);
    }

    public void RecordRateLimitDeferral(TimeSpan delay)
    {
        Interlocked.Increment(ref _rateLimitDeferrals);
        long delayTicks = (long)(delay.TotalSeconds * Stopwatch.Frequency);
        Interlocked.Add(ref _rateLimitDeferredTicks, delayTicks);

        long observed;
        while (delayTicks > (observed = Interlocked.Read(ref _maximumRateLimitDeferralTicks)))
        {
            if (Interlocked.CompareExchange(
                ref _maximumRateLimitDeferralTicks,
                delayTicks,
                observed) == observed)
            {
                break;
            }
        }
    }

    public void RecordPipelineOperation(PipelineOperation operation, long startedAt)
    {
        RecordPipelineActivity(startedAt);
        switch (operation)
        {
            case PipelineOperation.WorkItemDownload:
                Interlocked.Increment(ref _workItemDownloads);
                RecordElapsed(ref _workItemDownloadTicks, ref _maximumWorkItemDownloadTicks, startedAt);
                break;
            case PipelineOperation.WorkItemPublish:
                Interlocked.Increment(ref _workItemPublishes);
                RecordElapsed(ref _workItemPublishTicks, ref _maximumWorkItemPublishTicks, startedAt);
                break;
            case PipelineOperation.TestRunCreate:
                Interlocked.Increment(ref _testRunCreates);
                RecordElapsed(ref _testRunCreateTicks, ref _maximumTestRunCreateTicks, startedAt);
                break;
            case PipelineOperation.TestRunComplete:
                Interlocked.Increment(ref _testRunCompletes);
                RecordElapsed(ref _testRunCompleteTicks, ref _maximumTestRunCompleteTicks, startedAt);
                break;
            case PipelineOperation.ResultParseAndAggregate:
                Interlocked.Increment(ref _parseOperations);
                RecordElapsed(ref _parseTicks, ref _maximumParseTicks, startedAt);
                break;
        }
    }

    public JobMonitorMetricsSnapshot Snapshot()
    {
        long controlRequests = Interlocked.Read(ref _azdoControlRequests);
        long resultRequests = Interlocked.Read(ref _azdoResultRequests);
        long attachmentRequests = Interlocked.Read(ref _azdoAttachmentRequests);
        return new JobMonitorMetricsSnapshot(
            Elapsed: Stopwatch.GetElapsedTime(_startedAt),
            PipelineElapsed: GetPipelineElapsed(),
            AzureDevOpsRequests: controlRequests + resultRequests + attachmentRequests,
            AzureDevOpsControlRequests: controlRequests,
            AzureDevOpsResultRequests: resultRequests,
            AzureDevOpsAttachmentRequests: attachmentRequests,
            AzureDevOpsRetries: Interlocked.Read(ref _azdoRetries),
            AzureDevOpsFailedAttempts: Interlocked.Read(ref _azdoFailedAttempts),
            AzureDevOpsPayloadBytes: Interlocked.Read(ref _azdoPayloadBytes),
            AzureDevOpsRequestTime: GetElapsed(_azdoRequestTicks),
            MaximumAzureDevOpsRequestTime: GetElapsed(_azdoMaximumRequestTicks),
            HelixRequests: Interlocked.Read(ref _helixRequests),
            HelixRetries: Interlocked.Read(ref _helixRetries),
            HelixFailedAttempts: Interlocked.Read(ref _helixFailedAttempts),
            ResultBlobDownloads: Interlocked.Read(ref _resultBlobDownloads),
            ResultBlobDownloadFailures: Interlocked.Read(ref _resultBlobDownloadFailures),
            RateLimitWaits: Interlocked.Read(ref _rateLimitWaits),
            RateLimitWaitTime: GetElapsed(_rateLimitWaitTicks),
            MaximumRateLimitWaitTime: GetElapsed(_maximumRateLimitWaitTicks),
            RateLimitDeferrals: Interlocked.Read(ref _rateLimitDeferrals),
            RateLimitDeferredTime: GetElapsed(_rateLimitDeferredTicks),
            MaximumRateLimitDeferral: GetElapsed(_maximumRateLimitDeferralTicks),
            WorkItemDownloads: Interlocked.Read(ref _workItemDownloads),
            WorkItemDownloadTime: GetElapsed(_workItemDownloadTicks),
            MaximumWorkItemDownloadTime: GetElapsed(_maximumWorkItemDownloadTicks),
            WorkItemPublishes: Interlocked.Read(ref _workItemPublishes),
            WorkItemPublishTime: GetElapsed(_workItemPublishTicks),
            MaximumWorkItemPublishTime: GetElapsed(_maximumWorkItemPublishTicks),
            TestRunCreates: Interlocked.Read(ref _testRunCreates),
            TestRunCreateTime: GetElapsed(_testRunCreateTicks),
            MaximumTestRunCreateTime: GetElapsed(_maximumTestRunCreateTicks),
            TestRunCompletes: Interlocked.Read(ref _testRunCompletes),
            TestRunCompleteTime: GetElapsed(_testRunCompleteTicks),
            MaximumTestRunCompleteTime: GetElapsed(_maximumTestRunCompleteTicks),
            ParseOperations: Interlocked.Read(ref _parseOperations),
            ParseTime: GetElapsed(_parseTicks),
            MaximumParseTime: GetElapsed(_maximumParseTicks));
    }

    private static void RecordElapsed(ref long totalTicks, ref long maximumTicks, long startedAt)
    {
        long elapsedTicks = Stopwatch.GetTimestamp() - startedAt;
        Interlocked.Add(ref totalTicks, elapsedTicks);

        long observed;
        while (elapsedTicks > (observed = Interlocked.Read(ref maximumTicks)))
        {
            if (Interlocked.CompareExchange(ref maximumTicks, elapsedTicks, observed) == observed)
            {
                break;
            }
        }
    }

    private static TimeSpan GetElapsed(long stopwatchTicks)
        => TimeSpan.FromSeconds((double)stopwatchTicks / Stopwatch.Frequency);

    private void RecordPipelineActivity(long startedAt)
    {
        long observedStart;
        while ((observedStart = Interlocked.Read(ref _pipelineStartedAt)) == 0
            || startedAt < observedStart)
        {
            if (Interlocked.CompareExchange(ref _pipelineStartedAt, startedAt, observedStart) == observedStart)
            {
                break;
            }
        }

        long finishedAt = Stopwatch.GetTimestamp();
        long observedFinish;
        while (finishedAt > (observedFinish = Interlocked.Read(ref _pipelineFinishedAt)))
        {
            if (Interlocked.CompareExchange(ref _pipelineFinishedAt, finishedAt, observedFinish) == observedFinish)
            {
                break;
            }
        }
    }

    private TimeSpan GetPipelineElapsed()
    {
        long startedAt = Interlocked.Read(ref _pipelineStartedAt);
        if (startedAt == 0)
        {
            return TimeSpan.Zero;
        }

        long finishedAt = Math.Max(startedAt, Interlocked.Read(ref _pipelineFinishedAt));
        return TimeSpan.FromSeconds((double)(finishedAt - startedAt) / Stopwatch.Frequency);
    }
}

internal readonly record struct JobMonitorMetricsSnapshot(
    TimeSpan Elapsed,
    TimeSpan PipelineElapsed,
    long AzureDevOpsRequests,
    long AzureDevOpsControlRequests,
    long AzureDevOpsResultRequests,
    long AzureDevOpsAttachmentRequests,
    long AzureDevOpsRetries,
    long AzureDevOpsFailedAttempts,
    long AzureDevOpsPayloadBytes,
    TimeSpan AzureDevOpsRequestTime,
    TimeSpan MaximumAzureDevOpsRequestTime,
    long HelixRequests,
    long HelixRetries,
    long HelixFailedAttempts,
    long ResultBlobDownloads,
    long ResultBlobDownloadFailures,
    long RateLimitWaits,
    TimeSpan RateLimitWaitTime,
    TimeSpan MaximumRateLimitWaitTime,
    long RateLimitDeferrals,
    TimeSpan RateLimitDeferredTime,
    TimeSpan MaximumRateLimitDeferral,
    long WorkItemDownloads,
    TimeSpan WorkItemDownloadTime,
    TimeSpan MaximumWorkItemDownloadTime,
    long WorkItemPublishes,
    TimeSpan WorkItemPublishTime,
    TimeSpan MaximumWorkItemPublishTime,
    long TestRunCreates,
    TimeSpan TestRunCreateTime,
    TimeSpan MaximumTestRunCreateTime,
    long TestRunCompletes,
    TimeSpan TestRunCompleteTime,
    TimeSpan MaximumTestRunCompleteTime,
    long ParseOperations,
    TimeSpan ParseTime,
    TimeSpan MaximumParseTime);
