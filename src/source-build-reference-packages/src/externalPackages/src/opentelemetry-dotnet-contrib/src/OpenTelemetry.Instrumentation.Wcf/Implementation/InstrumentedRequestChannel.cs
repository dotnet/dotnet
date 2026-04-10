// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using System.ServiceModel.Channels;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class InstrumentedRequestChannel : InstrumentedChannel<IRequestChannel>, IRequestSessionChannel
{
    public InstrumentedRequestChannel(IRequestChannel inner)
        : base(inner)
    {
    }

    EndpointAddress IRequestChannel.RemoteAddress => this.Inner.RemoteAddress;

    Uri IRequestChannel.Via => this.Inner.Via;

    IOutputSession ISessionChannel<IOutputSession>.Session => ((ISessionChannel<IOutputSession>)this.Inner).Session;

    Message IRequestChannel.Request(Message message)
    {
        Guard.ThrowIfNull(message);

        var telemetryState = ClientChannelInstrumentation.BeforeSendRequest(message, ((IRequestChannel)this).RemoteAddress?.Uri);
        Message reply;
        try
        {
            reply = this.Inner.Request(message);
        }
        catch (Exception ex)
        {
            ClientChannelInstrumentation.AfterRequestCompleted(null, telemetryState, ex);
            throw;
        }

        ClientChannelInstrumentation.AfterRequestCompleted(reply, telemetryState);
        return reply;
    }

    Message IRequestChannel.Request(Message message, TimeSpan timeout)
    {
        Guard.ThrowIfNull(message);

        var telemetryState = ClientChannelInstrumentation.BeforeSendRequest(message, ((IRequestChannel)this).RemoteAddress?.Uri);
        Message reply;
        try
        {
            reply = this.Inner.Request(message, timeout);
        }
        catch (Exception ex)
        {
            ClientChannelInstrumentation.AfterRequestCompleted(null, telemetryState, ex);
            throw;
        }

        ClientChannelInstrumentation.AfterRequestCompleted(reply, telemetryState);
        return reply;
    }

    IAsyncResult IRequestChannel.BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(message);
        Guard.ThrowIfNull(callback);

        return this.InternalBeginRequest(message, (cb, s) => this.Inner.BeginRequest(message, timeout, cb, s), callback, state);
    }

    IAsyncResult IRequestChannel.BeginRequest(Message message, AsyncCallback callback, object? state)
    {
        Guard.ThrowIfNull(message);
        Guard.ThrowIfNull(callback);

        return this.InternalBeginRequest(message, (cb, s) => this.Inner.BeginRequest(message, cb, s), callback, state);
    }

    Message IRequestChannel.EndRequest(IAsyncResult result)
    {
        Guard.ThrowIfNull(result);

        var asyncResult = (AsyncResultWithTelemetryState)result;
        Message reply;
        try
        {
            reply = this.Inner.EndRequest(asyncResult.Inner);
        }
        catch (Exception ex)
        {
            ClientChannelInstrumentation.AfterRequestCompleted(null, asyncResult.TelemetryState, ex);
            throw;
        }

        ClientChannelInstrumentation.AfterRequestCompleted(reply, asyncResult.TelemetryState);
        return reply;
    }

    private IAsyncResult InternalBeginRequest(Message message, Func<AsyncCallback, object?, IAsyncResult> beginRequestDelegate, AsyncCallback callback, object? state)
    {
        IAsyncResult? result = null;

        void ExecuteInChildContext(object? unused)
        {
            var telemetryState = ClientChannelInstrumentation.BeforeSendRequest(message, ((IRequestChannel)this).RemoteAddress?.Uri);
            var asyncCallback = AsyncResultWithTelemetryState.GetAsyncCallback(callback, telemetryState);
            result = new AsyncResultWithTelemetryState(beginRequestDelegate(asyncCallback, state), telemetryState);
        }

        var executionContext = ExecutionContext.Capture();
        if (executionContext == null)
        {
            throw new InvalidOperationException("Cannot fetch execution context");
        }

        ExecutionContext.Run(executionContext, ExecuteInChildContext, null);
        return result!;
    }
}
