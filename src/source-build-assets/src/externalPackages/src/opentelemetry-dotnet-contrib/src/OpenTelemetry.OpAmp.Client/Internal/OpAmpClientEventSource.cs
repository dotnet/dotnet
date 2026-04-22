// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.OpAmp.Client.Internal;

[EventSource(Name = "OpenTelemetry-OpAmp-Client")]
internal class OpAmpClientEventSource : EventSource
{
    public static OpAmpClientEventSource Log = new();

    // General events 1-499
    private const int EventIdInvalidWsFrame = 1;

    // Service events 500-999
    private const int EventIdHeartbeatServiceStart = 500;
    private const int EventIdHeartbeatServiceStop = 501;
    private const int EventIdHeartbeatServiceTickFailure = 502;
    private const int EventIdHeartbeatServiceTimerUpdateFailure = 503;
    private const int EventIdHeartbeatServiceTimerUpdateReceived = 504;

    // FrameDispatcher verbose messages 1000-1099
    private const int EventIdSendingIdentificationMessage = 1_000;
    private const int EventIdSendingHeartbeatMessage = 1_001;
    private const int EventIdSendingAgentDisconnectMessage = 1_002;

    // FrameDispatcher error messages 1100-1199
    private const int EventIdFailedToSendIdentificationMessage = 1_100;
    private const int EventIdFailedToSendHeartbeatMessage = 1_101;
    private const int EventIdFailedToSendAgentDisconnectMessage = 1_102;

    [Event(EventIdInvalidWsFrame, Message = "Received invalid WebSocket frame header: {0}. Dropping the frame.", Level = EventLevel.Warning)]
    public void InvalidWsFrame(string errorMessage)
    {
        this.WriteEvent(EventIdInvalidWsFrame, errorMessage);
    }

    [Event(EventIdHeartbeatServiceStart, Message = "Heartbeat service started.", Level = EventLevel.Informational)]
    public void HeartbeatServiceStart()
    {
        this.WriteEvent(EventIdHeartbeatServiceStart);
    }

    [Event(EventIdHeartbeatServiceStop, Message = "Heartbeat service stopped.", Level = EventLevel.Informational)]
    public void HeartbeatServiceStop()
    {
        this.WriteEvent(EventIdHeartbeatServiceStop);
    }

    [NonEvent]
    public void HeartbeatServiceTickException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.HeartbeatServiceTickFailure(ex.ToInvariantString());
        }
    }

    [Event(EventIdHeartbeatServiceTickFailure, Message = "Heartbeat error: {0}", Level = EventLevel.Error)]
    public void HeartbeatServiceTickFailure(string exception)
    {
        this.WriteEvent(EventIdHeartbeatServiceTickFailure, exception);
    }

    [NonEvent]
    public void HeartbeatServiceTimerUpdateException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.HeartbeatServiceTimerUpdateFailure(ex.ToInvariantString());
        }
    }

    [Event(EventIdHeartbeatServiceTimerUpdateFailure, Message = "Failed to update timer interval: {0}", Level = EventLevel.Error)]
    public void HeartbeatServiceTimerUpdateFailure(string exception)
    {
        this.WriteEvent(EventIdHeartbeatServiceTimerUpdateFailure, exception);
    }

    [Event(EventIdHeartbeatServiceTimerUpdateReceived, Message = "New heartbeat interval received: {0}s", Level = EventLevel.Informational)]
    public void HeartbeatServiceTimerUpdateReceived(ulong seconds)
    {
        this.WriteEvent(EventIdHeartbeatServiceTimerUpdateReceived, seconds);
    }

    /* Sending messages */

    [Event(EventIdSendingIdentificationMessage, Message = "Sending identification message.", Level = EventLevel.Informational)]
    public void SendingIdentificationMessage()
    {
        this.WriteEvent(EventIdSendingIdentificationMessage);
    }

    [Event(EventIdSendingHeartbeatMessage, Message = "Sending heartbeat message.", Level = EventLevel.Informational)]
    public void SendingHeartbeatMessage()
    {
        this.WriteEvent(EventIdSendingHeartbeatMessage);
    }

    [Event(EventIdSendingAgentDisconnectMessage, Message = "Sending agent disconnect.", Level = EventLevel.Informational)]
    public void SendingAgentDisconnectMessage()
    {
        this.WriteEvent(EventIdSendingAgentDisconnectMessage);
    }

    [NonEvent]
    public void SendIdentificationMessageException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.FailedToSendIdentificationMessage(ex.ToInvariantString());
        }
    }

    [Event(EventIdFailedToSendIdentificationMessage, Message = "Failed to send identification message: {0}", Level = EventLevel.Error)]
    public void FailedToSendIdentificationMessage(string exception)
    {
        this.WriteEvent(EventIdFailedToSendIdentificationMessage, exception);
    }

    [NonEvent]
    public void SendHeartbeatMessageException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.FailedToSendHeartbeatMessage(ex.ToInvariantString());
        }
    }

    [Event(EventIdFailedToSendHeartbeatMessage, Message = "Failed to send heartbeat message: {0}", Level = EventLevel.Error)]
    public void FailedToSendHeartbeatMessage(string exception)
    {
        this.WriteEvent(EventIdFailedToSendHeartbeatMessage, exception);
    }

    [NonEvent]
    public void SendAgentDisconnectMessageException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.FailedToSendAgentDisconnectMessage(ex.ToInvariantString());
        }
    }

    [Event(EventIdFailedToSendAgentDisconnectMessage, Message = "Failed to send agent disconnect message: {0}", Level = EventLevel.Error)]
    public void FailedToSendAgentDisconnectMessage(string exception)
    {
        this.WriteEvent(EventIdFailedToSendAgentDisconnectMessage, exception);
    }
}
