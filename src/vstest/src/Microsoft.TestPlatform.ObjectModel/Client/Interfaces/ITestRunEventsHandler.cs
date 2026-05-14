// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Interface contract for handling test run events during run operation
/// </summary>
public interface ITestRunEventsHandler : ITestMessageEventHandler
{
    /// <summary>
    /// Handle the TestRunCompletion event from a test engine
    /// </summary>
    /// <param name="testRunCompleteArgs">TestRunCompletion Data</param>
    /// <param name="lastChunkArgs">Last set of test results</param>
    /// <param name="runContextAttachments">Attachments of the test run</param>
    /// <param name="executorUris">ExecutorURIs of the adapters involved in test run</param>
    void HandleTestRunComplete(TestRunCompleteEventArgs testRunCompleteArgs, TestRunChangedEventArgs? lastChunkArgs, ICollection<AttachmentSet>? runContextAttachments, ICollection<string>? executorUris);

    /// <summary>
    /// Handle a change in TestRun i.e. new testresults and stats
    /// </summary>
    /// <param name="testRunChangedArgs">TestRunChanged Data</param>
    void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs);

    /// <summary>
    /// Launches a process with a given process info under debugger
    /// Adapter get to call into this to launch any additional processes under debugger
    /// </summary>
    /// <param name="testProcessStartInfo">Process start info</param>
    /// <returns>ProcessId of the launched process</returns>
    int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo);
}

/// <summary>
/// Interface for handling generic message events during various operations
/// </summary>
public interface ITestMessageEventHandler
{
    /// <summary>
    /// Raw Message from the host directly
    /// </summary>
    /// <param name="rawMessage">raw message args from host</param>
    void HandleRawMessage(string rawMessage);

    /// <summary>
    /// Handle a IMessageLogger message event from Adapter
    /// Whenever adapters call IMessageLogger.SendMessage, TestEngine notifies client with this event
    /// </summary>
    /// <param name="level">Message Level</param>
    /// <param name="message">string message</param>
    void HandleLogMessage(TestMessageLevel level, string? message);
}
