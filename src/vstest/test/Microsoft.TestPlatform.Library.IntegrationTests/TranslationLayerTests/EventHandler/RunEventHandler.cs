// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests.EventHandler;

/// <inheritdoc />
public class RunEventHandler : ITestRunEventsHandler
{
    /// <summary>
    /// Gets the test results.
    /// </summary>
    public ConcurrentList<TestResult> TestResults { get; private set; }

    /// <summary>
    /// Gets the attachments.
    /// </summary>
    public ConcurrentList<AttachmentSet> Attachments { get; private set; }

    /// <summary>
    /// Gets the list of the invoked data collectors.
    /// </summary>
    public ConcurrentList<InvokedDataCollector> InvokedDataCollectors { get; private set; }

    /// <summary>
    /// Gets the metrics.
    /// </summary>
    public IDictionary<string, object>? Metrics { get; private set; }

    /// <summary>
    /// Gets the log message.
    /// </summary>
    public string? LogMessage { get; private set; }

    public ConcurrentList<string?> Errors { get; set; }

    /// <summary>
    /// Gets the test message level.
    /// </summary>
    public TestMessageLevel TestMessageLevel { get; private set; }

    public RunEventHandler()
    {
        TestResults = new ConcurrentList<TestResult>();
        Errors = new ConcurrentList<string?>();
        Attachments = new ConcurrentList<AttachmentSet>();
        InvokedDataCollectors = new ConcurrentList<InvokedDataCollector>();
    }

    public void EnsureSuccess()
    {
        if (Errors.Any())
        {
            throw new InvalidOperationException($"Test run reported errors:{Environment.NewLine}{string.Join(Environment.NewLine + Environment.NewLine, Errors)}");
        }
    }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        LogMessage = message;
        TestMessageLevel = level;
        if (level == TestMessageLevel.Error)
        {
            Errors.Add(message);
        }
    }

    public void HandleTestRunComplete(
        TestRunCompleteEventArgs testRunCompleteArgs,
        TestRunChangedEventArgs? lastChunkArgs,
        ICollection<AttachmentSet>? runContextAttachments,
        ICollection<string>? executorUris)
    {
        if (lastChunkArgs != null && lastChunkArgs.NewTestResults != null)
        {
            TestResults.AddRange(lastChunkArgs.NewTestResults);
        }

        if (testRunCompleteArgs.AttachmentSets != null)
        {
            Attachments.AddRange(testRunCompleteArgs.AttachmentSets);
        }

        if (testRunCompleteArgs.InvokedDataCollectors != null)
        {
            InvokedDataCollectors.AddRange(testRunCompleteArgs.InvokedDataCollectors);
        }

        Metrics = testRunCompleteArgs.Metrics;
    }

    public void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs)
    {
        if (testRunChangedArgs != null && testRunChangedArgs.NewTestResults != null)
        {
            TestResults.AddRange(testRunChangedArgs.NewTestResults);
        }
    }

    public void HandleRawMessage(string rawMessage)
    {
        // No op
    }

    public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo)
    {
        // No op
        return -1;
    }

    public override string ToString()
    {
        return $"""
        TestResults: {TestResults.Count}
          {string.Join("\n  ", TestResults?.Select(t => t.DisplayName + " " + t.Outcome) ?? [])}
        Errors: {Errors.Count}
        {string.Join("\n  ", Errors ?? [])}
        
        """;
    }
}
