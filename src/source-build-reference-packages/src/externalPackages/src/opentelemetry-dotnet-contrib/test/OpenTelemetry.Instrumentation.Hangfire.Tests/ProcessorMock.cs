// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Hangfire.Tests;

internal class ProcessorMock<T> : BaseProcessor<T>
{
    private readonly Action<T>? onStart;
    private readonly Action<T>? onEnd;

    public ProcessorMock(Action<T>? onStart = null, Action<T>? onEnd = null)
    {
        this.onStart = onStart;
        this.onEnd = onEnd;
    }

    public override void OnStart(T data)
    {
        this.onStart?.Invoke(data);
    }

    public override void OnEnd(T data)
    {
        this.onEnd?.Invoke(data);
    }
}
