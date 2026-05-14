// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.EventHandlers;

/// <summary>
/// The test run events handler.
/// </summary>
public class TestRunEventsHandler : IInternalTestRunEventsHandler
{
    private readonly ITestRequestHandler _requestHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunEventsHandler"/> class.
    /// </summary>
    /// <param name="requestHandler">test request handler</param>
    public TestRunEventsHandler(ITestRequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }

    /// <summary>
    /// Handle test run stats change.
    /// </summary>
    /// <param name="testRunChangedArgs"> The test run changed args. </param>
    public void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs)
    {
        EqtTrace.Info("Sending test run statistics");
        _requestHandler.SendTestRunStatistics(testRunChangedArgs);
    }

    /// <summary>
    /// Handle test run complete.
    /// </summary>
    /// <param name="testRunCompleteArgs"> The test run complete args. </param>
    /// <param name="lastChunkArgs"> The last chunk args. </param>
    /// <param name="runContextAttachments"> The run context attachments. </param>
    /// <param name="executorUris"> The executor uris. </param>
    public void HandleTestRunComplete(TestRunCompleteEventArgs testRunCompleteArgs, TestRunChangedEventArgs? lastChunkArgs, ICollection<AttachmentSet>? runContextAttachments, ICollection<string>? executorUris)
    {
        EqtTrace.Info("Sending test run complete");
        _requestHandler.SendExecutionComplete(testRunCompleteArgs, lastChunkArgs, runContextAttachments, executorUris);
    }

    /// <summary>
    /// Handles a test run message.
    /// </summary>
    /// <param name="level"> The level. </param>
    /// <param name="message"> The message. </param>
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

    /// <summary>
    /// Launches a process with a given process info under debugger
    /// Adapter get to call into this to launch any additional processes under debugger
    /// </summary>
    /// <param name="testProcessStartInfo">Process start info</param>
    /// <returns>ProcessId of the launched process</returns>
    public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo? testProcessStartInfo)
    {
        EqtTrace.Info("Sending LaunchProcessWithDebuggerAttached on additional test process: {0}", testProcessStartInfo?.FileName);
        return _requestHandler.LaunchProcessWithDebuggerAttached(testProcessStartInfo);
    }

    /// <inheritdoc/>
    public bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo)
    {
        EqtTrace.Info("Sending AttachDebuggerToProcess on additional test process with pid: {0}", attachDebuggerInfo.ProcessId);
        return _requestHandler.AttachDebuggerToProcess(attachDebuggerInfo);
    }
}
