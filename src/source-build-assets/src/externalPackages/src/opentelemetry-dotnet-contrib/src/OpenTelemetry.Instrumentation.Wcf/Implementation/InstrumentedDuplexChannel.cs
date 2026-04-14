// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using System.ServiceModel.Channels;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class InstrumentedDuplexChannel : InstrumentedChannel<IDuplexChannel>, IDuplexSessionChannel
{
    private readonly TimeSpan telemetryTimeout;

    public InstrumentedDuplexChannel(IDuplexChannel inner, TimeSpan telemetryTimeout)
        : base(inner)
    {
        this.telemetryTimeout = telemetryTimeout;
    }

    public EndpointAddress LocalAddress => this.Inner.LocalAddress;

    public EndpointAddress RemoteAddress => this.Inner.RemoteAddress;

    public Uri Via => this.Inner.Via;

    public IDuplexSession Session => ((IDuplexSessionChannel)this.Inner).Session;

    public void Send(Message message)
    {
        Guard.ThrowIfNull(message);

        this.SendInternal(message, this.telemetryTimeout, _ => this.Inner.Send(message));
    }

    public void Send(Message message, TimeSpan timeout)
    {
        Guard.ThrowIfNull(message);

        this.SendInternal(message, timeout, _ => this.Inner.Send(message, timeout));
    }

    public IAsyncResult BeginSend(Message message, AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(message);
        Guard.ThrowIfNull(callback);

        return this.SendInternal(message, this.telemetryTimeout, (cb, s) => this.Inner.BeginSend(message, cb, s), callback, state);
    }

    public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(message);
        Guard.ThrowIfNull(callback);

        return this.SendInternal(message, timeout, (cb, s) => this.Inner.BeginSend(message, timeout, cb, s), callback, state);
    }

    public void EndSend(IAsyncResult result)
    {
        Guard.ThrowIfNull(result);

        var asyncResult = (AsyncResultWithTelemetryState)result;
        try
        {
            this.Inner.EndSend(asyncResult.Inner);
        }
        catch (Exception ex)
        {
            ClientChannelInstrumentation.AfterRequestCompleted(null, asyncResult.TelemetryState, ex);
            throw;
        }
    }

    public Message Receive()
    {
        var message = this.Inner.Receive();
        HandleReceivedMessage(message);
        return message;
    }

    public Message Receive(TimeSpan timeout)
    {
        var message = this.Inner.Receive(timeout);
        HandleReceivedMessage(message);
        return message;
    }

    public IAsyncResult BeginReceive(AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(callback);

        return this.Inner.BeginReceive(callback, state);
    }

    public IAsyncResult BeginReceive(TimeSpan timeout, AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(callback);

        return this.Inner.BeginReceive(timeout, callback, state);
    }

    public Message EndReceive(IAsyncResult result)
    {
        Guard.ThrowIfNull(result);

        var message = this.Inner.EndReceive(result);
        HandleReceivedMessage(message);
        return message;
    }

    public bool TryReceive(TimeSpan timeout, out Message message)
    {
        var returnValue = this.Inner.TryReceive(timeout, out message);
        HandleReceivedMessage(message);
        return returnValue;
    }

    public IAsyncResult BeginTryReceive(TimeSpan timeout, AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(callback);

        return this.Inner.BeginTryReceive(timeout, callback, state);
    }

    public bool EndTryReceive(IAsyncResult result, out Message message)
    {
        Guard.ThrowIfNull(result);

        var returnValue = this.Inner.EndTryReceive(result, out message);
        HandleReceivedMessage(message);
        return returnValue;
    }

    public bool WaitForMessage(TimeSpan timeout)
    {
        return this.Inner.WaitForMessage(timeout);
    }

    public IAsyncResult BeginWaitForMessage(TimeSpan timeout, AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(callback);

        return this.Inner.BeginWaitForMessage(timeout, callback, state);
    }

    public bool EndWaitForMessage(IAsyncResult result)
    {
        Guard.ThrowIfNull(result);

        return this.Inner.EndWaitForMessage(result);
    }

    private static void HandleReceivedMessage(Message message)
    {
        var telemetryState = RequestTelemetryStateTracker.PopTelemetryState(message);
        if (telemetryState != null)
        {
            ClientChannelInstrumentation.AfterRequestCompleted(message, telemetryState);
        }
    }

    private static void OnTelemetryStateTimedOut(RequestTelemetryState telemetryState)
    {
        ClientChannelInstrumentation.AfterRequestCompleted(null, telemetryState);
    }

    private IAsyncResult SendInternal(Message message, TimeSpan timeout, Func<AsyncCallback, object?, IAsyncResult> executeSend, AsyncCallback callback, object? state)
    {
        IAsyncResult? result = null;
        this.SendInternal(message, timeout, telemetryState =>
        {
            var asyncCallback = AsyncResultWithTelemetryState.GetAsyncCallback(callback, telemetryState);
            result = new AsyncResultWithTelemetryState(executeSend(asyncCallback, state), telemetryState);
        });
        return result!;
    }

    private void SendInternal(Message message, TimeSpan timeout, Action<RequestTelemetryState> executeSend)
    {
        RequestTelemetryState? telemetryState = null;

        void ExecuteInChildContext(object? unused)
        {
            telemetryState = ClientChannelInstrumentation.BeforeSendRequest(message, this.RemoteAddress?.Uri);
            RequestTelemetryStateTracker.PushTelemetryState(message, telemetryState, timeout, OnTelemetryStateTimedOut);
            executeSend(telemetryState);
        }

        var executionContext = ExecutionContext.Capture();
        if (executionContext == null)
        {
            throw new InvalidOperationException("Cannot fetch execution context");
        }

        try
        {
            ExecutionContext.Run(executionContext, ExecuteInChildContext, null);
        }
        catch (Exception ex)
        {
            ClientChannelInstrumentation.AfterRequestCompleted(null, telemetryState, ex);
            throw;
        }
    }
}
