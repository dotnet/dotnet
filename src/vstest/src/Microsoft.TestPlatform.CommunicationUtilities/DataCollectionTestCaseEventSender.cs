// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Net;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

public class DataCollectionTestCaseEventSender : IDataCollectionTestCaseEventSender
{
    private static readonly object SyncObject = new();

    private readonly ICommunicationManager _communicationManager;
    private readonly IDataSerializer _dataSerializer;

    // Protocol version negotiated with the datacollector test case event handler.
    // Updated from the DataCollectionTestStartAck echo after the first SendTestCaseStart.
    private int _protocolVersion = ProtocolVersioning.HighestSupportedVersion;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionTestCaseEventSender"/> class.
    /// </summary>
    protected DataCollectionTestCaseEventSender()
        : this(new SocketCommunicationManager(), JsonDataSerializer.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionTestCaseEventSender"/> class.
    /// </summary>
    /// <param name="communicationManager">Communication manager.</param>
    /// <param name="dataSerializer">Serializer for serialization and deserialization of the messages.</param>
    protected DataCollectionTestCaseEventSender(ICommunicationManager communicationManager, IDataSerializer dataSerializer)
    {
        _communicationManager = communicationManager;
        _dataSerializer = dataSerializer;
    }

    /// <summary>
    /// Gets the singleton instance of DataCollectionTestCaseEventSender.
    /// </summary>
    // TODO : Re-factor to pass the instance as singleton.
    public static DataCollectionTestCaseEventSender? Instance { get; private set; }

    /// <summary>
    /// Gets singleton instance of DataCollectionRequestHandler.
    /// </summary>
    /// <returns>A singleton instance of <see cref="DataCollectionTestCaseEventSender"/></returns>
    public static DataCollectionTestCaseEventSender Create()
    {
        if (Instance == null)
        {
            lock (SyncObject)
            {
                Instance ??= new DataCollectionTestCaseEventSender();
            }
        }

        return Instance;
    }

    /// <inheritdoc />
    public void InitializeCommunication(int port)
    {
        _communicationManager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));
    }

    /// <inheritdoc />
    public bool WaitForRequestSenderConnection(int connectionTimeout)
    {
        return _communicationManager.WaitForServerConnection(connectionTimeout);
    }

    /// <inheritdoc />
    public void Close()
    {
        _communicationManager?.StopClient();
        EqtTrace.Info("Closing the connection!");
    }

    /// <inheritdoc />
    public void SendTestCaseStart(TestCaseStartEventArgs e)
    {
        _communicationManager.SendMessage(MessageType.DataCollectionTestStart, e, _protocolVersion);

        var message = _communicationManager.ReceiveMessage();
        if (message != null && message.MessageType != MessageType.DataCollectionTestStartAck)
        {
            EqtTrace.Error("DataCollectionTestCaseEventSender.SendTestCaseStart : MessageType.DataCollectionTestStartAck not received.");
        }

        // Adopt the version echoed by the handler as the negotiated protocol version for all
        // subsequent sends on this sub-channel.
        if (message?.Version > 0)
        {
            _protocolVersion = Math.Min(message.Version, ProtocolVersioning.HighestSupportedVersion);
        }
    }

    /// <inheritdoc />
    public Collection<AttachmentSet>? SendTestCaseEnd(TestCaseEndEventArgs e)
    {
        var attachmentSets = new Collection<AttachmentSet>();
        _communicationManager.SendMessage(MessageType.DataCollectionTestEnd, e, _protocolVersion);

        var message = _communicationManager.ReceiveMessage();
        if (message != null && message.MessageType == MessageType.DataCollectionTestEndResult)
        {
            attachmentSets = _dataSerializer.DeserializePayload<Collection<AttachmentSet>>(message);
        }

        return attachmentSets;
    }

    /// <inheritdoc />
    public void SendTestSessionEnd(SessionEndEventArgs e)
    {
        _communicationManager.SendMessage(MessageType.SessionEnd, e, _protocolVersion);
    }
}
