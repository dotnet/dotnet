// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor;

internal class TestProjectSnapshotManagerDispatcher : ProjectSnapshotManagerDispatcher
{
    public TestProjectSnapshotManagerDispatcher()
    {
        DispatcherScheduler = SynchronizationContext.Current is null
            ? new ThrowingTaskScheduler()
            : TaskScheduler.FromCurrentSynchronizationContext();
    }

    public override TaskScheduler DispatcherScheduler { get; }

    private Thread Thread { get; } = Thread.CurrentThread;

    public override bool IsDispatcherThread => Thread.CurrentThread == Thread;

    private class ThrowingTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Enumerable.Empty<Task>();
        }

        protected override void QueueTask(Task task)
        {
            throw new InvalidOperationException($"Use [{nameof(UIFactAttribute)}]");
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new InvalidOperationException($"Use [{nameof(UIFactAttribute)}]");
        }
    }
}
