// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;

/// <summary>
/// DiscoveryDataAggregator aggregates discovery data from multiple sources running in parallel or in series.
/// </summary>
internal sealed class DiscoveryDataAggregator
{
    private readonly object _dataUpdateSyncObject = new();
    private readonly ConcurrentDictionary<string, object> _metricsAggregator = new();
    private readonly ConcurrentDictionary<string, DiscoveryStatus> _sourcesWithDiscoveryStatus = new();

    /// <summary>
    /// Atomic boolean used to detect if message was already sent.
    /// </summary>
    private int _isMessageSent;

    /// <summary>
    /// Set to initialized if any of the request is aborted
    /// </summary>
    public bool IsAborted { get; private set; }

    /// <summary>
    /// Aggregate total test count
    /// </summary>
    public long TotalTests { get; private set; }

    /// <summary>
    /// A collection of aggregated discovered extensions.
    /// </summary>
    public Dictionary<string, HashSet<string>> DiscoveredExtensions { get; private set; } = new();

    /// <summary>
    /// Returns the Aggregated Metrics.
    /// </summary>
    /// <returns></returns>
    public IDictionary<string, object> GetMetrics()
    {
        if (_metricsAggregator == null || _metricsAggregator.IsEmpty)
        {
            return new ConcurrentDictionary<string, object>();
        }

        var adapterUsedCount = _metricsAggregator.Count(metrics =>
            metrics.Key.Contains(TelemetryDataConstants.TotalTestsByAdapter));

        var adaptersDiscoveredCount = _metricsAggregator.Count(metrics =>
            metrics.Key.Contains(TelemetryDataConstants.TimeTakenToDiscoverTestsByAnAdapter));

        // Aggregating Total Adapter Used Count
        _metricsAggregator.TryAdd(
            TelemetryDataConstants.NumberOfAdapterUsedToDiscoverTests,
            adapterUsedCount);

        // Aggregating Total Adapters Discovered Count
        _metricsAggregator.TryAdd(
            TelemetryDataConstants.NumberOfAdapterDiscoveredDuringDiscovery,
            adaptersDiscoveredCount);

        return _metricsAggregator;
    }

    public void MarkAsAborted()
    {
        lock (_dataUpdateSyncObject)
        {
            IsAborted = true;
            TotalTests = -1;
        }
    }

    /// <summary>
    /// Aggregate discovery data
    /// Must be thread-safe as this is expected to be called by parallel managers
    /// </summary>
    public void Aggregate(DiscoveryCompleteEventArgs discoveryCompleteEventArgs)
    {
        if (_isMessageSent == 1)
        {
            EqtTrace.Verbose("DiscoveryDataAggregator.Aggregate: Message was already sent so skipping event aggregation.");
            return;
        }

        lock (_dataUpdateSyncObject)
        {
            IsAborted = IsAborted || discoveryCompleteEventArgs.IsAborted;

            // Do not aggregate tests count if test discovery is aborted. It is mandated by
            // platform that tests count is negative for discovery abort event.
            // See `DiscoveryCompleteEventArgs`.
            TotalTests = IsAborted ? -1 : TotalTests + discoveryCompleteEventArgs.TotalCount;

            // Aggregate the discovered extensions.
            DiscoveredExtensions = TestExtensions.CreateMergedDictionary(DiscoveredExtensions, discoveryCompleteEventArgs.DiscoveredExtensions);
        }

        AggregateMetrics(discoveryCompleteEventArgs.Metrics);

        // Do not aggregate NotDiscovered, PartiallyDiscovered, FullyDiscovered sources here
        // because this aggregator is shared with proxies so the state is already up-to-date.
        //
        // The reason to share the aggregator is that we only get 1 notification for completed
        // discovery when discovery is done. Instead of 1 notification per discovery manager.
        // And we also don't get any notification for discovery of sources that did not start
        // discovering yet. So we need the state in the aggregator to start with all the sources
        // that we were requested to discover in NotDiscovered state. And then as we discover them,
        // we need to update that directly. So when any discovery is cancelled we can just take the
        // current state in the aggregator and report it.
    }

    /// <summary>
    /// Aggregates the metrics from Test Host Process.
    /// </summary>
    /// <param name="metrics"></param>
    public void AggregateMetrics(IDictionary<string, object>? metrics)
    {
        if (metrics == null || metrics.Count == 0 || _metricsAggregator == null)
        {
            return;
        }

        foreach (var metric in metrics)
        {
            if (metric.Key.Contains(TelemetryDataConstants.TimeTakenToDiscoverTestsByAnAdapter)
                || metric.Key.Contains(TelemetryDataConstants.TimeTakenInSecByAllAdapters)
                || metric.Key.Contains(TelemetryDataConstants.TotalTestsDiscovered)
                || metric.Key.Contains(TelemetryDataConstants.TotalTestsByAdapter)
                || metric.Key.Contains(TelemetryDataConstants.TimeTakenToLoadAdaptersInSec))
            {
                var newValue = Convert.ToDouble(metric.Value, CultureInfo.InvariantCulture);

                if (_metricsAggregator.TryGetValue(metric.Key, out object? oldValue))
                {
                    double oldDoubleValue = Convert.ToDouble(oldValue, CultureInfo.InvariantCulture);
                    _metricsAggregator[metric.Key] = newValue + oldDoubleValue;
                }
                else
                {
                    _metricsAggregator.TryAdd(metric.Key, newValue);
                }
            }
        }
    }

    /// <summary>
    /// Determines if we should send the message to the client.
    /// </summary>
    /// <remarks>
    /// Handles race conditions as this aggregator is shared across various event handler for the
    /// same discovery request but we want to notify only once.
    /// </remarks>
    /// <returns><see langword="true"/> if first to send the message; <see langword="false"/> otherwise.</returns>
    public bool TryAggregateIsMessageSent()
        => Interlocked.CompareExchange(ref _isMessageSent, 1, 0) == 0;

    public List<string> GetSourcesWithStatus(DiscoveryStatus discoveryStatus)
        => _sourcesWithDiscoveryStatus.IsEmpty
            ? new List<string>()
            : _sourcesWithDiscoveryStatus
                .Where(source => source.Value == discoveryStatus)
                .Select(source => source.Key)
                .ToList();

    public void MarkSourcesWithStatus(IEnumerable<string?>? sources, DiscoveryStatus status)
    {
        if (_isMessageSent == 1)
        {
            EqtTrace.Verbose("DiscoveryDataAggregator.MarkSourcesWithStatus: Message was already sent so skipping source update.");
            return;
        }

        if (sources is null)
        {
            return;
        }

        foreach (var source in sources)
        {
            if (source is null)
            {
                continue;
            }

            _sourcesWithDiscoveryStatus.AddOrUpdate(source,
                _ =>
                {
                    if (status != DiscoveryStatus.NotDiscovered)
                    {
                        EqtTrace.Warning($"DiscoveryDataAggregator.MarkSourcesWithStatus: Undiscovered {source} added with status: '{status}'.");
                    }
                    else
                    {
                        EqtTrace.Verbose($"DiscoveryDataAggregator.MarkSourcesWithStatus: Adding {source} with status: '{status}'.");
                    }

                    return status;
                },
                (_, previousStatus) =>
                {
                    if (previousStatus == DiscoveryStatus.FullyDiscovered && status != DiscoveryStatus.FullyDiscovered
                        || previousStatus == DiscoveryStatus.PartiallyDiscovered && (status == DiscoveryStatus.NotDiscovered || status == DiscoveryStatus.SkippedDiscovery))
                    {
                        EqtTrace.Warning($"DiscoveryDataAggregator.MarkSourcesWithStatus: Downgrading source {source} status from '{previousStatus}' to '{status}'.");
                    }
                    else if (previousStatus != status)
                    {
                        EqtTrace.Verbose($"DiscoveryDataAggregator.MarkSourcesWithStatus: Upgrading {source} status from '{previousStatus}' to '{status}'.");
                    }
                    return status;
                });
        }
    }

    /// <summary>
    /// Updates the discovery status of the source based on the discovered test cases.
    /// </summary>
    /// <param name="previousSource">The last discovered sources or null.</param>
    /// <param name="testCases">The discovered sources.</param>
    /// <returns>The last discovered source or null.</returns>
    public string? MarkSourcesBasedOnDiscoveredTestCases(string? previousSource, IEnumerable<TestCase>? testCases)
    {
        if (_isMessageSent == 1)
        {
            EqtTrace.Verbose("DiscoveryDataAggregator.MarkSourcesBasedOnDiscoveredTestCases: Message was already sent so skipping source update.");
            return previousSource;
        }

        // When all testcases count in source is dividable by chunk size (e.g. 100 tests and
        // chunk size of 10) then lastChunk is coming as empty. Otherwise, we receive the
        // remaining test cases to process.
        if (testCases is null)
        {
            return previousSource;
        }

        foreach (var testCase in testCases)
        {
            var currentSource = testCase.Source;

            // We rely on the fact that sources are processed in a sequential way, which
            // means that when we receive a different source than the previous, we can
            // assume that the previous source was fully discovered.
            if (previousSource is null || previousSource == currentSource)
            {
                MarkSourcesWithStatus(new[] { currentSource }, DiscoveryStatus.PartiallyDiscovered);
            }
            else if (currentSource != previousSource)
            {
                EqtTrace.Verbose($"DiscoveryDataAggregator.MarkSourcesBasedOnDiscoveredTestCases: Discovered test source changed from {previousSource} to {currentSource}.");
                MarkSourcesWithStatus(new[] { previousSource }, DiscoveryStatus.FullyDiscovered);
                MarkSourcesWithStatus(new[] { currentSource }, DiscoveryStatus.PartiallyDiscovered);
            }

            previousSource = currentSource;
        }

        return previousSource;
    }
}
