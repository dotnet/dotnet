// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.OpAmp.Client.Internal;

[EventSource(Name = "OpenTelemetry-OpAmp-Client")]
internal sealed class OpAmpClientEventSource : EventSource
{
    public static OpAmpClientEventSource Log = new();

    // General events 1-499
    private const int EventIdInvalidWsFrame = 1;
    private const int EventIdTransportCloseFailure = 2;
    private const int EventIdHttpResponseReceived = 3;
    private const int EventIdOversizedWebSocketMessage = 4;
    private const int EventIdFrameProcessingFailure = 5;

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
    private const int EventIdSendingEffectiveConfigMessage = 1_003;
    private const int EventIdSendingCustomCapabilitiesMessage = 1_004;
    private const int EventIdSendingCustomMessageMessage = 1_005;

    // FrameDispatcher error messages 1100-1199
    private const int EventIdFailedToSendIdentificationMessage = 1_100;
    private const int EventIdFailedToSendHeartbeatMessage = 1_101;
    private const int EventIdFailedToSendAgentDisconnectMessage = 1_102;
    private const int EventIdFailedToSendEffectiveConfigMessage = 1_103;
    private const int EventIdFailedToSendCustomCapabilitiesMessage = 1_104;
    private const int EventIdFailedToSendCustomMessageMessage = 1_105;

    [Event(EventIdInvalidWsFrame, Message = "Received invalid WebSocket frame header: {0}. Dropping the frame.", Level = EventLevel.Warning)]
    public void InvalidWsFrame(string errorMessage)
    {
        this.WriteEvent(EventIdInvalidWsFrame, errorMessage);
    }

    [NonEvent]
    public void TransportCloseException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, EventKeywords.All))
        {
            this.TransportCloseFailure(ex.ToInvariantString());
        }
    }

    [Event(EventIdTransportCloseFailure, Message = "WebSocket close failed: {0}", Level = EventLevel.Warning)]
    public void TransportCloseFailure(string exception)
    {
        this.WriteEvent(EventIdTransportCloseFailure, exception);
    }

    [NonEvent]
    public void OversizedWebSocketMessageReceived(int minimumBytes, int limitBytes)
    {
        if (this.IsEnabled(EventLevel.Warning, EventKeywords.All))
        {
            this.OversizedWebSocketMessage(minimumBytes, limitBytes);
        }
    }

    [Event(EventIdOversizedWebSocketMessage, Message = "OpAMP server WebSocket message discarded: message is at least {0} bytes, exceeding the {1}-byte limit. The connection will be closed and the frame will not be processed.", Level = EventLevel.Warning)]
    public void OversizedWebSocketMessage(int minimumBytes, int limitBytes)
    {
        this.WriteEvent(EventIdOversizedWebSocketMessage, minimumBytes, limitBytes);
    }

    [NonEvent]
    public void HttpResponseBytesReceived(int bytes)
    {
        if (this.IsEnabled(EventLevel.Verbose, EventKeywords.All))
        {
            this.HttpResponseReceived(bytes);
        }
    }

    [Event(EventIdHttpResponseReceived, Message = "OpAMP HTTP response received: {0} bytes.", Level = EventLevel.Verbose)]
    public void HttpResponseReceived(int bytes)
    {
        this.WriteEvent(EventIdHttpResponseReceived, bytes);
    }

    [NonEvent]
    public void FrameProcessingException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, EventKeywords.All))
        {
            this.FrameProcessingFailure(ex.ToInvariantString());
        }
    }

    [Event(EventIdFrameProcessingFailure, Message = "Failed to process incoming server frame. The frame was dropped: {0}", Level = EventLevel.Warning)]
    public void FrameProcessingFailure(string exception)
    {
        this.WriteEvent(EventIdFrameProcessingFailure, exception);
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

    [Event(EventIdSendingEffectiveConfigMessage, Message = "Sending effective config message.", Level = EventLevel.Informational)]
    public void SendingEffectiveConfigMessage()
    {
        this.WriteEvent(EventIdSendingEffectiveConfigMessage);
    }

    [Event(EventIdSendingCustomCapabilitiesMessage, Message = "Sending custom capabilities message.", Level = EventLevel.Informational)]
    public void SendingCustomCapabilitiesMessage()
    {
        this.WriteEvent(EventIdSendingCustomCapabilitiesMessage);
    }

    [Event(EventIdSendingCustomMessageMessage, Message = "Sending custom message.", Level = EventLevel.Informational)]
    public void SendingCustomMessageMessage()
    {
        this.WriteEvent(EventIdSendingCustomMessageMessage);
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

    [NonEvent]
    public void SendEffectiveConfigMessageException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.FailedToSendEffectiveConfigMessage(ex.ToInvariantString());
        }
    }

    [Event(EventIdFailedToSendEffectiveConfigMessage, Message = "Failed to send effective config message: {0}", Level = EventLevel.Error)]
    public void FailedToSendEffectiveConfigMessage(string exception)
    {
        this.WriteEvent(EventIdFailedToSendEffectiveConfigMessage, exception);
    }

    [NonEvent]
    public void SendCustomCapabilitiesMessageException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.FailedToSendCustomCapabilitiesMessage(ex.ToInvariantString());
        }
    }

    [Event(EventIdFailedToSendCustomCapabilitiesMessage, Message = "Failed to send custom capabilities message: {0}", Level = EventLevel.Error)]
    public void FailedToSendCustomCapabilitiesMessage(string exception)
    {
        this.WriteEvent(EventIdFailedToSendCustomCapabilitiesMessage, exception);
    }

    [NonEvent]
    public void SendCustomMessageMessageException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.FailedToSendCustomMessageMessage(ex.ToInvariantString());
        }
    }

    [Event(EventIdFailedToSendCustomMessageMessage, Message = "Failed to send a custom message: {0}", Level = EventLevel.Error)]
    public void FailedToSendCustomMessageMessage(string exception)
    {
        this.WriteEvent(EventIdFailedToSendCustomMessageMessage, exception);
    }
}
