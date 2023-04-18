// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

# if NETFRAMEWORK
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.TestPlatform.AcceptanceTests.Performance.TranslationLayer;

/// <inheritdoc />
public class RunEventHandler : ITestRunEventsHandler
{
    /// <summary>
    /// Gets the test results.
    /// </summary>
    public List<TestResult> TestResults { get; private set; }

    /// <summary>
    /// Gets the metrics.
    /// </summary>
    public IDictionary<string, object>? Metrics { get; private set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets the log message.
    /// </summary>
    public List<string> LogMessages { get; } = new List<string>();

    public RunEventHandler()
    {
        TestResults = new List<TestResult>();
    }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        LogMessages.Add($"[{level.ToString().ToUpperInvariant()}]: {message}");
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

        Metrics = testRunCompleteArgs.Metrics;
        if (testRunCompleteArgs.Error != null)
        {
            LogMessages.Add($"[ERROR] {testRunCompleteArgs.Error}");
        }
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
}
#endif
