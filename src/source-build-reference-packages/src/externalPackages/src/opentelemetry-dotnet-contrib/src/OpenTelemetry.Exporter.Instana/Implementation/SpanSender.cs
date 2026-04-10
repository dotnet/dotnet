// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;

namespace OpenTelemetry.Exporter.Instana.Implementation;

#pragma warning disable CA1001 // Types that own disposable fields should be disposable
internal sealed class SpanSender : ISpanSender
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
{
    private readonly Task queueSenderTask;
    private readonly ConcurrentQueue<InstanaSpan> spansQueue = new();

    public SpanSender()
    {
        // create a task that will send a batch of spans every second at least
        this.queueSenderTask = new Task(this.TaskSpanSender, TaskCreationOptions.LongRunning);
        this.queueSenderTask.Start();
    }

    public void Enqueue(InstanaSpan instanaSpan)
    {
        if (Transport.IsAvailable)
        {
            this.spansQueue.Enqueue(instanaSpan);
        }
    }

    private async void TaskSpanSender()
    {
        // this will be an infinite loop
        while (true)
        {
            // check if we can send spans
            if (this.spansQueue.TryPeek(out var _))
            {
                // actually send spans
                await Transport.SendSpansAsync(this.spansQueue).ConfigureAwait(false);
            }

            // rest for a while
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}
