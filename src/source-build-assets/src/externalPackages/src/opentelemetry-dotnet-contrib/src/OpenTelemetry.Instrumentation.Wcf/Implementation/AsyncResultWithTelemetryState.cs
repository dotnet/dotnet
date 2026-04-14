// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class AsyncResultWithTelemetryState : IAsyncResult
{
    public AsyncResultWithTelemetryState(IAsyncResult inner, RequestTelemetryState telemetryState)
    {
        this.Inner = inner;
        this.TelemetryState = telemetryState;
    }

    public IAsyncResult Inner { get; }

    public RequestTelemetryState TelemetryState { get; }

    object? IAsyncResult.AsyncState => this.Inner.AsyncState;

    WaitHandle IAsyncResult.AsyncWaitHandle => this.Inner.AsyncWaitHandle;

    bool IAsyncResult.CompletedSynchronously => this.Inner.CompletedSynchronously;

    bool IAsyncResult.IsCompleted => this.Inner.IsCompleted;

    public static AsyncCallback GetAsyncCallback(AsyncCallback innerCallback, RequestTelemetryState telemetryState)
    {
        return (IAsyncResult ar) =>
        {
            innerCallback(new AsyncResultWithTelemetryState(ar, telemetryState));
        };
    }
}
