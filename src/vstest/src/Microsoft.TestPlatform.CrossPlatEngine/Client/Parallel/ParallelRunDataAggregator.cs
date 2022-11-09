// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;

/// <summary>
/// ParallelRunDataAggregator aggregates test run data from execution managers running in parallel
/// </summary>
internal class ParallelRunDataAggregator
{

    private readonly List<string> _executorUris;

    private readonly List<ITestRunStatistics> _testRunStatsList;

    private readonly ConcurrentDictionary<string, object> _metricsAggregator;

    private readonly object _dataUpdateSyncObject = new();

    public ParallelRunDataAggregator(string runSettingsXml)
    {
        RunSettings = runSettingsXml ?? throw new ArgumentNullException(nameof(runSettingsXml));
        ElapsedTime = TimeSpan.Zero;
        RunContextAttachments = new Collection<AttachmentSet>();
        RunCompleteArgsAttachments = new List<AttachmentSet>();
        InvokedDataCollectors = new Collection<InvokedDataCollector>();
        Exceptions = new List<Exception>();
        DiscoveredExtensions = new Dictionary<string, HashSet<string>>();
        _executorUris = new List<string>();
        _testRunStatsList = new List<ITestRunStatistics>();

        _metricsAggregator = new ConcurrentDictionary<string, object>();

        IsAborted = false;
        IsCanceled = false;
    }

    public TimeSpan ElapsedTime { get; set; }

    public Collection<AttachmentSet> RunContextAttachments { get; set; }

    public List<AttachmentSet> RunCompleteArgsAttachments { get; }

    public Collection<InvokedDataCollector> InvokedDataCollectors { get; set; }

    public List<Exception> Exceptions { get; }

    public HashSet<string> ExecutorUris => new(_executorUris);

    /// <summary>
    /// A collection of aggregated discovered extensions.
    /// </summary>
    public Dictionary<string, HashSet<string>> DiscoveredExtensions { get; private set; }

    public bool IsAborted { get; private set; }

    public bool IsCanceled { get; private set; }

    public string RunSettings { get; }

    public ITestRunStatistics GetAggregatedRunStats()
    {
        var testOutcomeMap = new Dictionary<TestOutcome, long>();
        long totalTests = 0;
        if (_testRunStatsList.Count > 0)
        {
            foreach (var runStats in _testRunStatsList)
            {
                // TODO: we get nullref here if the stats are empty.
                foreach (var outcome in runStats.Stats!.Keys)
                {
                    if (!testOutcomeMap.ContainsKey(outcome))
                    {
                        testOutcomeMap.Add(outcome, 0);
                    }
                    testOutcomeMap[outcome] += runStats.Stats[outcome];
                }
                totalTests += runStats.ExecutedTests;
            }
        }

        var overallRunStats = new TestRunStatistics(testOutcomeMap);
        overallRunStats.ExecutedTests = totalTests;
        return overallRunStats;
    }

    /// <summary>
    /// Returns the Aggregated Run Data Metrics
    /// </summary>
    /// <returns></returns>
    public IDictionary<string, object> GetAggregatedRunDataMetrics()
    {
        if (_metricsAggregator == null || _metricsAggregator.IsEmpty)
        {
            return new ConcurrentDictionary<string, object>();
        }

        var adapterUsedCount = _metricsAggregator.Count(metrics =>
            metrics.Key.Contains(TelemetryDataConstants.TotalTestsRanByAdapter));

        var adaptersDiscoveredCount = _metricsAggregator.Count(metrics =>
            metrics.Key.Contains(TelemetryDataConstants.TimeTakenToRunTestsByAnAdapter));

        // Aggregating Total Adapter Used Count
        _metricsAggregator.TryAdd(TelemetryDataConstants.NumberOfAdapterUsedToRunTests, adapterUsedCount);

        // Aggregating Total Adapters Discovered Count
        _metricsAggregator.TryAdd(
            TelemetryDataConstants.NumberOfAdapterDiscoveredDuringExecution,
            adaptersDiscoveredCount);

        return _metricsAggregator;
    }

    public Exception? GetAggregatedException()
    {
        return Exceptions == null || Exceptions.Count < 1 ? null : new AggregateException(Exceptions);
    }

    /// <summary>
    /// Aggregate Run Data
    /// Must be thread-safe as this is expected to be called by parallel managers
    /// </summary>
    public void Aggregate(
        ITestRunStatistics? testRunStats,
        ICollection<string>? executorUris,
        Exception? exception,
        TimeSpan elapsedTime,
        bool isAborted,
        bool isCanceled,
        ICollection<AttachmentSet>? runContextAttachments,
        Collection<AttachmentSet>? runCompleteArgsAttachments,
        Collection<InvokedDataCollector>? invokedDataCollectors,
        Dictionary<string, HashSet<string>>? discoveredExtensions)
    {
        lock (_dataUpdateSyncObject)
        {
            IsAborted = IsAborted || isAborted;
            IsCanceled = IsCanceled || isCanceled;

            ElapsedTime = TimeSpan.FromMilliseconds(Math.Max(ElapsedTime.TotalMilliseconds, elapsedTime.TotalMilliseconds));
            if (runContextAttachments != null)
            {
                foreach (var attachmentSet in runContextAttachments)
                {
                    RunContextAttachments.Add(attachmentSet);
                }
            }

            if (runCompleteArgsAttachments != null) RunCompleteArgsAttachments.AddRange(runCompleteArgsAttachments);
            if (exception != null) Exceptions.Add(exception);
            if (executorUris != null) _executorUris.AddRange(executorUris);
            if (testRunStats != null) _testRunStatsList.Add(testRunStats);

            if (invokedDataCollectors?.Count > 0)
            {
                foreach (var invokedDataCollector in invokedDataCollectors)
                {
                    if (!InvokedDataCollectors.Contains(invokedDataCollector))
                    {
                        InvokedDataCollectors.Add(invokedDataCollector);
                    }
                }
            }

            // Aggregate the discovered extensions.
            DiscoveredExtensions = TestExtensions.CreateMergedDictionary(DiscoveredExtensions, discoveredExtensions);
        }
    }

    /// <summary>
    /// Aggregates Run Data Metrics from each Test Host Process
    /// </summary>
    /// <param name="metrics"></param>
    public void AggregateRunDataMetrics(IDictionary<string, object>? metrics)
    {
        if (metrics == null || metrics.Count == 0 || _metricsAggregator == null)
        {
            return;
        }

        foreach (var metric in metrics)
        {
            if (metric.Key.Contains(TelemetryDataConstants.TimeTakenToRunTestsByAnAdapter) || metric.Key.Contains(TelemetryDataConstants.TimeTakenByAllAdaptersInSec) || (metric.Key.Contains(TelemetryDataConstants.TotalTestsRun) || metric.Key.Contains(TelemetryDataConstants.TotalTestsRanByAdapter)))
            {
                var newValue = Convert.ToDouble(metric.Value, CultureInfo.InvariantCulture);

                if (_metricsAggregator.TryGetValue(metric.Key, out var oldValue))
                {
                    var oldDoubleValue = Convert.ToDouble(oldValue, CultureInfo.InvariantCulture);
                    _metricsAggregator[metric.Key] = newValue + oldDoubleValue;
                }
                else
                {
                    _metricsAggregator.TryAdd(metric.Key, newValue);
                }
            }
        }
    }


    public void MarkAsAborted()
    {
        lock (_dataUpdateSyncObject)
        {
            IsAborted = true;
        }
    }
}
