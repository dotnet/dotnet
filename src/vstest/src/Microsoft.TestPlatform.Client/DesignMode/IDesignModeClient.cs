// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Client.DesignMode;

/// <summary>
/// The interface for design mode client.
/// </summary>
public interface IDesignModeClient : IDisposable
{
    /// <summary>
    /// Setups client based on port
    /// </summary>
    /// <param name="port">port number to connect</param>
    void ConnectToClientAndProcessRequests(int port, ITestRequestManager testRequestManager);

    /// <summary>
    /// Send a custom host launch message to IDE
    /// </summary>
    /// <param name="defaultTestHostStartInfo">Default TestHost Start Info</param>
    /// <param name="cancellationToken">The cancellation Token.</param>
    /// <returns>Process id of the launched test host.</returns>
    int LaunchCustomHost(TestProcessStartInfo defaultTestHostStartInfo, CancellationToken cancellationToken);

    /// <summary>
    /// Attach debugger to an already running process.
    /// </summary>
    /// <param name="attachDebuggerInfo">Process Id and other information about the process that IDE should attach to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> if the debugger was successfully attached to the requested process, <see langword="false"/> otherwise.</returns>
    bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo, CancellationToken cancellationToken);

    /// <summary>
    /// Handles parent process exit
    /// </summary>
    void HandleParentProcessExit();

    /// <summary>
    /// Send the raw messages to IDE
    /// </summary>
    /// <param name="rawMessage"></param>
    void SendRawMessage(string rawMessage);

    /// <summary>
    /// Send the test session messages to IDE
    /// </summary>
    /// <param name="level">Level for the message</param>
    /// <param name="message">Actual message string</param>
    void SendTestMessage(TestMessageLevel level, string? message);
}
