// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer;

/// <summary>
/// The Discovery Events Handler Converter.
/// Converts the ITestDiscoveryEventsHandler to ITestDiscoveryEventsHandler2
/// </summary>
public class DiscoveryEventsHandleConverter : ITestDiscoveryEventsHandler2
{
    private readonly ITestDiscoveryEventsHandler _testDiscoveryEventsHandler;

    /// <summary>
    /// The Discovery Complete Handler.
    /// Converts the ITestDiscoveryEventsHandler to ITestDiscoveryEventsHandler2
    /// </summary>
    /// <param name="testDiscoveryEventsHandler"></param>
    public DiscoveryEventsHandleConverter(ITestDiscoveryEventsHandler testDiscoveryEventsHandler)
    {
        _testDiscoveryEventsHandler = testDiscoveryEventsHandler ?? throw new ArgumentNullException(nameof(testDiscoveryEventsHandler));
    }

    /// <summary>
    /// Handles Raw Message
    /// </summary>
    /// <param name="rawMessage"></param>
    public void HandleRawMessage(string rawMessage)
    {
        _testDiscoveryEventsHandler.HandleRawMessage(rawMessage);
    }

    /// <summary>
    /// Handles Log Message
    /// </summary>
    /// <param name="level"></param>
    /// <param name="message"></param>
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _testDiscoveryEventsHandler.HandleLogMessage(level, message);
    }

    /// <summary>
    /// Handle Discovery Complete
    /// </summary>
    /// <param name="discoveryCompleteEventArgs"></param>
    /// <param name="lastChunk"></param>
    public void HandleDiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk)
    {
        _testDiscoveryEventsHandler.HandleDiscoveryComplete(discoveryCompleteEventArgs.TotalCount, lastChunk, discoveryCompleteEventArgs.IsAborted);
    }

    /// <summary>
    /// Handles Discovery Tests
    /// </summary>
    /// <param name="discoveredTestCases"></param>
    public void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases)
    {
        _testDiscoveryEventsHandler.HandleDiscoveredTests(discoveredTestCases);
    }
}
