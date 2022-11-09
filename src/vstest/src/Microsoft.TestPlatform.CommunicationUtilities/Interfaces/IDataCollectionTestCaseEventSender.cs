// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

/// <summary>
/// Interface for sending test case events from test execution process to data collection process
/// </summary>
public interface IDataCollectionTestCaseEventSender
{
    /// <summary>
    /// Setups client based on port
    /// </summary>
    /// <param name="port">port number to connect</param>
    void InitializeCommunication(int port);

    /// <summary>
    /// Waits for Request Handler to connect to Request Sender
    /// </summary>
    /// <param name="connectionTimeout">Timeout for establishing connection</param>
    /// <returns>True if connected, false if timed-out</returns>
    bool WaitForRequestSenderConnection(int connectionTimeout);

    /// <summary>
    /// Closes the connection
    /// </summary>
    void Close();

    /// <summary>
    /// Sends the TestCaseStart event.
    /// </summary>
    /// <param name="e">
    /// The args containing info about TestCaseStart event.
    /// </param>
    void SendTestCaseStart(TestCaseStartEventArgs e);

    /// <summary>
    /// Sends the TestCaseCompleted event along with outcome.
    /// </summary>
    /// <param name="e">
    /// The args containing info about TestResult event.
    /// </param>
    /// <returns>
    /// The <see cref="Collection"/>Collection of TestCase attachments.
    /// </returns>
    Collection<AttachmentSet>? SendTestCaseEnd(TestCaseEndEventArgs e);

    /// <summary>
    /// Sends the SessionEnd event. This is used to as a trigger to close communication channel between datacollector process and testhost process.
    /// </summary>
    /// <param name="e">
    /// The args containing info about SessionEnd event.
    /// </param>
    void SendTestSessionEnd(SessionEndEventArgs e);
}
