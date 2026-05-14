// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal class InstrumentedCommunicationObject<T> : ICommunicationObject
    where T : ICommunicationObject
{
    public InstrumentedCommunicationObject(T inner)
    {
        this.Inner = inner;
    }

    event EventHandler ICommunicationObject.Closed { add => this.Inner.Closed += value; remove => this.Inner.Closed -= value; }

    event EventHandler ICommunicationObject.Closing { add => this.Inner.Closing += value; remove => this.Inner.Closing -= value; }

    event EventHandler ICommunicationObject.Faulted { add => this.Inner.Faulted += value; remove => this.Inner.Faulted -= value; }

    event EventHandler ICommunicationObject.Opened { add => this.Inner.Opened += value; remove => this.Inner.Opened -= value; }

    event EventHandler ICommunicationObject.Opening { add => this.Inner.Opening += value; remove => this.Inner.Opening -= value; }

    CommunicationState ICommunicationObject.State => this.Inner.State;

    protected T Inner { get; }

    void ICommunicationObject.Abort()
    {
        this.Inner.Abort();
    }

    IAsyncResult ICommunicationObject.BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
        return this.Inner.BeginClose(timeout, callback, state);
    }

    IAsyncResult ICommunicationObject.BeginClose(AsyncCallback callback, object state)
    {
        return this.Inner.BeginClose(callback, state);
    }

    IAsyncResult ICommunicationObject.BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    {
        return this.Inner.BeginOpen(timeout, callback, state);
    }

    IAsyncResult ICommunicationObject.BeginOpen(AsyncCallback callback, object state)
    {
        return this.Inner.BeginOpen(callback, state);
    }

    void ICommunicationObject.Close(TimeSpan timeout)
    {
        this.Inner.Close(timeout);
    }

    void ICommunicationObject.Close()
    {
        this.Inner.Close();
    }

    void ICommunicationObject.EndClose(IAsyncResult result)
    {
        this.Inner.EndClose(result);
    }

    void ICommunicationObject.EndOpen(IAsyncResult result)
    {
        this.Inner.EndOpen(result);
    }

    void ICommunicationObject.Open(TimeSpan timeout)
    {
        this.Inner.Open(timeout);
    }

    void ICommunicationObject.Open()
    {
        this.Inner.Open();
    }
}
