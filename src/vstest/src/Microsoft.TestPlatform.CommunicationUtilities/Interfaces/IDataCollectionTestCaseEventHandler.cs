// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

/// <summary>
/// Interface for interacting with execution process for getting test case events in data collection process.
/// </summary>
internal interface IDataCollectionTestCaseEventHandler
{
    // TODO : Similar interfaces exist, need redesign.

    /// <summary>
    /// Initializes the communication for sending requests
    /// </summary>
    /// <returns>Port Number of the communication channel</returns>
    int InitializeCommunication();

    /// <summary>
    /// Waits for Request Handler to be connected.
    /// </summary>
    /// <param name="connectionTimeout">Time to wait for connection</param>
    /// <returns>True, if Handler is connected</returns>
    bool WaitForRequestHandlerConnection(int connectionTimeout);

    /// <summary>
    /// Close the handler
    /// </summary>
    void Close();

    /// <summary>
    /// Listens to the commands from execution process
    /// </summary>
    void ProcessRequests();
}
