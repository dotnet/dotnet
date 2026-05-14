// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Orchestrates logger operations for this engine.
/// </summary>
public interface ITestLoggerManager : IDisposable
{
    /// <summary>
    /// Loggers initialized flag.
    /// </summary>
    bool LoggersInitialized
    {
        get;
    }

    /// <summary>
    /// Initialize loggers.
    /// </summary>
    void Initialize(string? runSettings);

    /// <summary>
    /// Handles test run message.
    /// </summary>
    /// <param name="e"></param>
    void HandleTestRunMessage(TestRunMessageEventArgs e);

    /// <summary>
    /// Handles test run start.
    /// </summary>
    /// <param name="e"></param>
    void HandleTestRunStart(TestRunStartEventArgs e);

    /// <summary>
    /// Handles test run stats change.
    /// </summary>
    /// <param name="e"></param>
    void HandleTestRunStatsChange(TestRunChangedEventArgs e);

    /// <summary>
    /// Handles test run complete.
    /// </summary>
    /// <param name="e"></param>
    void HandleTestRunComplete(TestRunCompleteEventArgs e);

    /// <summary>
    /// Handles discovery message.
    /// </summary>
    /// <param name="e"></param>
    void HandleDiscoveryMessage(TestRunMessageEventArgs e);

    /// <summary>
    /// Handles discovery start.
    /// </summary>
    /// <param name="e"></param>
    void HandleDiscoveryStart(DiscoveryStartEventArgs e);

    /// <summary>
    /// Handles discovered tests.
    /// </summary>
    /// <param name="e"></param>
    void HandleDiscoveredTests(DiscoveredTestsEventArgs e);

    /// <summary>
    /// Handles discovery complete.
    /// </summary>
    /// <param name="e"></param>
    void HandleDiscoveryComplete(DiscoveryCompleteEventArgs e);
}
