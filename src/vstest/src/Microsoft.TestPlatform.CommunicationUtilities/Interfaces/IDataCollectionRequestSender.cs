// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;

/// <summary>
/// Defines contract to send test platform requests to test host
/// </summary>
internal interface IDataCollectionRequestSender
{
    /// <summary>
    /// Initializes the communication for sending requests
    /// </summary>
    /// <returns>Port Number of the communication channel</returns>
    int InitializeCommunication();

    /// <summary>
    /// Waits for Request Handler to be connected
    /// </summary>
    /// <param name="connectionTimeout">Time to wait for connection</param>
    /// <returns>True, if Handler is connected</returns>
    bool WaitForRequestHandlerConnection(int connectionTimeout);

    /// <summary>
    /// Close the Sender
    /// </summary>
    void Close();

    /// <summary>
    /// Sends the TestHostLaunched event
    /// </summary>
    /// <param name="testHostLaunchedPayload">
    /// Test host launched payload
    /// </param>
    void SendTestHostLaunched(TestHostLaunchedPayload testHostLaunchedPayload);

    /// <summary>
    /// Sends the BeforeTestRunStart event and waits for result
    /// </summary>
    /// <param name="settingXml">
    /// Run settings for test run.
    /// </param>
    /// <param name="sources">
    /// Test run sources
    /// </param>
    /// <param name="isTelemetryOptedIn">
    /// Telemetry opted in flag.
    /// </param>
    /// <param name="runEventsHandler">
    /// Test message event handler for handling messages.
    /// </param>
    /// <returns>
    /// BeforeTestRunStartResult containing environment variables
    /// </returns>
    BeforeTestRunStartResult? SendBeforeTestRunStartAndGetResult(string? settingXml, IEnumerable<string> sources, bool isTelemetryOptedIn, ITestMessageEventHandler? runEventsHandler);

    /// <summary>
    /// Sends the AfterTestRunEnd event and waits for result
    /// </summary>
    /// <param name="runEventsHandler">
    /// Test message event handler for handling messages.
    /// </param>
    /// <param name="isCancelled">
    /// The value to specify whether the test run is canceled or not.
    /// </param>
    /// <returns>
    /// AfterTestRunEndResult containing dataCollector attachments and metrics
    /// </returns>
    AfterTestRunEndResult? SendAfterTestRunEndAndGetResult(ITestMessageEventHandler? runEventsHandler, bool isCancelled);
}
