// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Exposes events that Test Loggers can register for.
/// </summary>
public abstract class TestLoggerEvents
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    protected TestLoggerEvents()
    {
    }

    /// <summary>
    /// Raised when a test message is received.
    /// </summary>
    public abstract event EventHandler<TestRunMessageEventArgs>? TestRunMessage;

    /// <summary>
    /// Raised when a test run starts.
    /// </summary>
    public abstract event EventHandler<TestRunStartEventArgs>? TestRunStart;

    /// <summary>
    /// Raised when a test result is received.
    /// </summary>
    public abstract event EventHandler<TestResultEventArgs>? TestResult;

    /// <summary>
    /// Raised when a test run is complete.
    /// </summary>
    public abstract event EventHandler<TestRunCompleteEventArgs>? TestRunComplete;

    /// <summary>
    /// Raised when test discovery starts
    /// </summary>
    public abstract event EventHandler<DiscoveryStartEventArgs>? DiscoveryStart;

    /// <summary>
    /// Raised when a discovery message is received.
    /// </summary>
    public abstract event EventHandler<TestRunMessageEventArgs>? DiscoveryMessage;

    /// <summary>
    /// Raised when discovered tests are received
    /// </summary>
    public abstract event EventHandler<DiscoveredTestsEventArgs>? DiscoveredTests;

    /// <summary>
    /// Raised when test discovery is complete
    /// </summary>
    public abstract event EventHandler<DiscoveryCompleteEventArgs>? DiscoveryComplete;

}
