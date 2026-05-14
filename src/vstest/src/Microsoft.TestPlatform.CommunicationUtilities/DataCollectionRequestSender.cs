// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;
using System.Net;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using CommonResources = Microsoft.VisualStudio.TestPlatform.Common.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;

/// <summary>
/// Utility class that facilitates the IPC communication. Acts as server.
/// </summary>
public sealed class DataCollectionRequestSender : IDataCollectionRequestSender
{
    private readonly ICommunicationManager _communicationManager;
    private readonly IDataSerializer _dataSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionRequestSender"/> class.
    /// </summary>
    public DataCollectionRequestSender()
        : this(new SocketCommunicationManager(), JsonDataSerializer.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionRequestSender"/> class.
    /// </summary>
    /// <param name="communicationManager">
    /// The communication manager.
    /// </param>
    /// <param name="dataSerializer">
    /// The data serializer.
    /// </param>
    internal DataCollectionRequestSender(ICommunicationManager communicationManager, IDataSerializer dataSerializer)
    {
        _communicationManager = communicationManager;
        _dataSerializer = dataSerializer;
    }

    /// <summary>
    /// Creates an endpoint and listens for client connection asynchronously
    /// </summary>
    /// <returns>Port number</returns>
    public int InitializeCommunication()
    {
        EqtTrace.Verbose("DataCollectionRequestSender.InitializeCommunication : Initialize communication. ");

        var endpoint = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0));
        _communicationManager.AcceptClientAsync();
        return endpoint.Port;
    }

    /// <summary>
    /// Waits for Request Handler to be connected
    /// </summary>
    /// <param name="clientConnectionTimeout">Time to wait for connection</param>
    /// <returns>True, if Handler is connected</returns>
    public bool WaitForRequestHandlerConnection(int clientConnectionTimeout)
    {
        EqtTrace.Verbose("DataCollectionRequestSender.WaitForRequestHandlerConnection : Waiting for connection with timeout: {0}", clientConnectionTimeout);

        return _communicationManager.WaitForClientConnection(clientConnectionTimeout);
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    public void Dispose()
    {
        _communicationManager?.StopServer();
    }

    /// <summary>
    /// Closes the connection
    /// </summary>
    public void Close()
    {
        EqtTrace.Info("Closing the connection");

        _communicationManager?.StopServer();
    }

    /// <inheritdoc/>
    public void SendTestHostLaunched(TestHostLaunchedPayload testHostLaunchedPayload)
    {
        _communicationManager.SendMessage(MessageType.TestHostLaunched, testHostLaunchedPayload);
    }

    /// <inheritdoc/>
    public BeforeTestRunStartResult? SendBeforeTestRunStartAndGetResult(string? settingsXml, IEnumerable<string> sources, bool isTelemetryOptedIn, ITestMessageEventHandler? runEventsHandler)
    {
        var isDataCollectionStarted = false;
        BeforeTestRunStartResult? result = null;

        EqtTrace.Verbose("DataCollectionRequestSender.SendBeforeTestRunStartAndGetResult: Send BeforeTestRunStart message with settingsXml {0} and sources {1}: ", settingsXml, string.Join(" ", sources));

        var payload = new BeforeTestRunStartPayload
        {
            SettingsXml = settingsXml,
            Sources = sources,
            IsTelemetryOptedIn = isTelemetryOptedIn
        };

        _communicationManager.SendMessage(MessageType.BeforeTestRunStart, payload);

        while (!isDataCollectionStarted)
        {
            var rawMessage = _communicationManager.ReceiveRawMessage();
            TPDebug.Assert(rawMessage is not null, "rawMessage is null");

            var message = !rawMessage.IsNullOrEmpty() ? _dataSerializer.DeserializeMessage(rawMessage) : null;
            TPDebug.Assert(message is not null, "message is null");

            EqtTrace.Verbose("DataCollectionRequestSender.SendBeforeTestRunStartAndGetResult: Received message: {0}", message);

            if (message.MessageType == MessageType.DataCollectionMessage)
            {
                var dataCollectionMessageEventArgs = _dataSerializer.DeserializePayload<DataCollectionMessageEventArgs>(message);
                TPDebug.Assert(dataCollectionMessageEventArgs is not null, $"{nameof(dataCollectionMessageEventArgs)} is null");
                LogDataCollectorMessage(dataCollectionMessageEventArgs, runEventsHandler);
            }
            else if (message.MessageType == MessageType.BeforeTestRunStartResult)
            {
                isDataCollectionStarted = true;
                result = _dataSerializer.DeserializePayload<BeforeTestRunStartResult>(message);
            }
            else if (message.MessageType == MessageType.TelemetryEventMessage)
            {
                runEventsHandler?.HandleRawMessage(rawMessage);
            }
        }

        return result;
    }

    /// <inheritdoc/>
    public AfterTestRunEndResult? SendAfterTestRunEndAndGetResult(ITestMessageEventHandler? runEventsHandler, bool isCancelled)
    {
        var isDataCollectionComplete = false;
        AfterTestRunEndResult? result = null;

        EqtTrace.Verbose("DataCollectionRequestSender.SendAfterTestRunStartAndGetResult: Send AfterTestRunEnd message with isCancelled: {0}", isCancelled);

        _communicationManager.SendMessage(MessageType.AfterTestRunEnd, isCancelled);

        // Cycle through the messages that the datacollector sends.
        // Currently each of the operations are not separate tasks since they should not each take much time. This is just a notification.
        while (!isDataCollectionComplete && !isCancelled)
        {
            var rawMessage = _communicationManager.ReceiveRawMessage();
            TPDebug.Assert(rawMessage is not null, "rawMessage is null");

            var message = !rawMessage.IsNullOrEmpty() ? _dataSerializer.DeserializeMessage(rawMessage) : null;
            TPDebug.Assert(message is not null, "message is null");

            EqtTrace.Verbose("DataCollectionRequestSender.SendAfterTestRunStartAndGetResult: Received message: {0}", message);

            if (message.MessageType == MessageType.DataCollectionMessage)
            {
                var dataCollectionMessageEventArgs = _dataSerializer.DeserializePayload<DataCollectionMessageEventArgs>(message);
                TPDebug.Assert(dataCollectionMessageEventArgs is not null, $"{nameof(dataCollectionMessageEventArgs)} is null");
                LogDataCollectorMessage(dataCollectionMessageEventArgs, runEventsHandler);
            }
            else if (message.MessageType == MessageType.AfterTestRunEndResult)
            {
                result = _dataSerializer.DeserializePayload<AfterTestRunEndResult>(message);
                isDataCollectionComplete = true;
            }
            else if (message.MessageType == MessageType.TelemetryEventMessage)
            {
                runEventsHandler?.HandleRawMessage(rawMessage);
            }
        }

        return result;
    }

    private static void LogDataCollectorMessage(DataCollectionMessageEventArgs dataCollectionMessageEventArgs, ITestMessageEventHandler? requestHandler)
    {
        string logMessage;
        if (dataCollectionMessageEventArgs.FriendlyName.IsNullOrWhiteSpace())
        {
            // Message from data collection framework.
            logMessage = string.Format(CultureInfo.CurrentCulture, CommonResources.DataCollectionMessageFormat, dataCollectionMessageEventArgs.Message);
        }
        else
        {
            // Message from individual data collector.
            logMessage = string.Format(CultureInfo.CurrentCulture, CommonResources.DataCollectorMessageFormat, dataCollectionMessageEventArgs.FriendlyName, dataCollectionMessageEventArgs.Message);
        }

        requestHandler?.HandleLogMessage(dataCollectionMessageEventArgs.Level, logMessage);
    }
}
