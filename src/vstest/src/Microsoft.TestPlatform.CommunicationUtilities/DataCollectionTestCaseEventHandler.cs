// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Net;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;

/// <summary>
/// The test case data collection request handler.
/// </summary>
internal class DataCollectionTestCaseEventHandler : IDataCollectionTestCaseEventHandler
{
    private readonly ICommunicationManager _communicationManager;
    private readonly IDataCollectionManager? _dataCollectionManager;
    private readonly IDataSerializer _dataSerializer;
    private readonly IMessageSink _messageSink;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionTestCaseEventHandler"/> class.
    /// </summary>
    internal DataCollectionTestCaseEventHandler(IMessageSink messageSink)
        : this(messageSink, new SocketCommunicationManager(), DataCollectionManager.Instance, JsonDataSerializer.Instance)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionTestCaseEventHandler"/> class.
    /// </summary>
    /// <param name="communicationManager">Communication manager implementation.</param>
    /// <param name="dataCollectionManager">Data collection manager implementation.</param>
    /// <param name="dataSerializer">Serializer for serialization and deserialization of the messages.</param>
    internal DataCollectionTestCaseEventHandler(IMessageSink messageSink, ICommunicationManager communicationManager, IDataCollectionManager? dataCollectionManager, IDataSerializer dataSerializer)
    {
        _communicationManager = communicationManager;
        _dataCollectionManager = dataCollectionManager;
        _dataSerializer = dataSerializer;
        _messageSink = messageSink;
    }

    /// <inheritdoc />
    public int InitializeCommunication()
    {
        var endpoint = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0));
        _communicationManager.AcceptClientAsync();
        return endpoint.Port;
    }

    /// <inheritdoc />
    public bool WaitForRequestHandlerConnection(int connectionTimeout)
    {
        return _communicationManager.WaitForClientConnection(connectionTimeout);
    }

    /// <inheritdoc />
    public void Close()
    {
        _communicationManager?.StopServer();
    }

    /// <inheritdoc />
    public void ProcessRequests()
    {
        var isSessionEnd = false;

        do
        {
            var message = _communicationManager.ReceiveMessage();
            switch (message?.MessageType)
            {
                case MessageType.DataCollectionTestStart:
                    EqtTrace.Info("DataCollectionTestCaseEventHandler: Test case starting.");

                    var testCaseStartEventArgs = _dataSerializer.DeserializePayload<TestCaseStartEventArgs>(message);

                    try
                    {
                        TPDebug.Assert(_dataCollectionManager is not null, "_dataCollectionManager is null");
                        TPDebug.Assert(testCaseStartEventArgs is not null, "testCaseStartEventArgs is null");
                        _dataCollectionManager.TestCaseStarted(testCaseStartEventArgs);
                    }
                    catch (Exception ex)
                    {
                        _messageSink.SendMessage(new DataCollectionMessageEventArgs(TestMessageLevel.Error, $"Error occurred during TestCaseStarted event handling: {ex}"));
                        EqtTrace.Error($"DataCollectionTestCaseEventHandler.ProcessRequests: Error occurred during TestCaseStarted event handling: {ex}");
                    }

                    _communicationManager.SendMessage(MessageType.DataCollectionTestStartAck);

                    EqtTrace.Info("DataCollectionTestCaseEventHandler: Test case '{0} - {1}' started.", testCaseStartEventArgs?.TestCaseName, testCaseStartEventArgs?.TestCaseId);

                    break;

                case MessageType.DataCollectionTestEnd:
                    EqtTrace.Info("DataCollectionTestCaseEventHandler: Test case completing.");

                    var testCaseEndEventArgs = _dataSerializer.DeserializePayload<TestCaseEndEventArgs>(message);

                    Collection<AttachmentSet> attachmentSets;
                    try
                    {
                        TPDebug.Assert(_dataCollectionManager is not null, "_dataCollectionManager is null");
                        TPDebug.Assert(testCaseEndEventArgs is not null, "testCaseEndEventArgs is null");
                        attachmentSets = _dataCollectionManager.TestCaseEnded(testCaseEndEventArgs);
                    }
                    catch (Exception ex)
                    {
                        _messageSink.SendMessage(new DataCollectionMessageEventArgs(TestMessageLevel.Error, $"Error occurred during DataCollectionTestEnd event handling: {ex}"));
                        EqtTrace.Error($"DataCollectionTestCaseEventHandler.ProcessRequests: Error occurred during DataCollectionTestEnd event handling: {ex}");
                        attachmentSets = new Collection<AttachmentSet>();
                    }

                    _communicationManager.SendMessage(MessageType.DataCollectionTestEndResult, attachmentSets);

                    EqtTrace.Info("DataCollectionTestCaseEventHandler: Test case '{0} - {1}' completed", testCaseEndEventArgs?.TestCaseName, testCaseEndEventArgs?.TestCaseId);
                    break;

                case MessageType.SessionEnd:
                    isSessionEnd = true;

                    EqtTrace.Info("DataCollectionTestCaseEventHandler: Test session ended");

                    try
                    {
                        Close();
                    }
                    catch (Exception ex)
                    {
                        _messageSink.SendMessage(new DataCollectionMessageEventArgs(TestMessageLevel.Error, $"Error occurred during SessionEnd event handling: {ex}"));
                        EqtTrace.Error($"DataCollectionTestCaseEventHandler.ProcessRequests: Error occurred during SessionEnd event handling: {ex}");
                    }

                    break;

                default:
                    EqtTrace.Info("DataCollectionTestCaseEventHandler: Invalid Message type '{0}'", message?.MessageType);

                    break;
            }
        }
        while (!isSessionEnd);
    }
}
