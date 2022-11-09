// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// The test discovery event handler.
/// </summary>
public class TestDiscoveryEventHandler : ITestDiscoveryEventsHandler2
{
    private readonly ITestRequestHandler _requestHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestDiscoveryEventHandler"/> class.
    /// </summary>
    /// <param name="requestHandler"> The Request Handler. </param>
    public TestDiscoveryEventHandler(ITestRequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }

    /// <summary>
    /// Handles discovered tests
    /// </summary>
    /// <param name="discoveredTestCases">List of test cases</param>
    public void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases)
    {
        EqtTrace.Info("Test Cases found ");
        _requestHandler.SendTestCases(discoveredTestCases);
    }

    /// <summary>
    /// Handle discovery complete.
    /// </summary>
    /// <param name="discoveryCompleteEventArgs"> Discovery Complete Events Args. </param>
    /// <param name="lastChunk"> The last chunk. </param>
    public void HandleDiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk)
    {
        EqtTrace.Info(discoveryCompleteEventArgs.IsAborted ? "Discover Aborted." : "Discover Finished.");

        _requestHandler.DiscoveryComplete(discoveryCompleteEventArgs, lastChunk);
    }

    /// <summary>
    /// The handle discovery message.
    /// </summary>
    /// <param name="level"> Logging level. </param>
    /// <param name="message"> Logging message. </param>
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        switch (level)
        {
            case TestMessageLevel.Informational:
                EqtTrace.Info(message);
                break;

            case TestMessageLevel.Warning:
                EqtTrace.Warning(message);
                break;

            case TestMessageLevel.Error:
                EqtTrace.Error(message);
                break;

            default:
                EqtTrace.Info(message);
                break;
        }

        _requestHandler.SendLog(level, message);
    }

    public void HandleRawMessage(string rawMessage)
    {
        // No-Op
        // TestHost at this point has no functionality where it requires rawmessage
    }
}
