// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.ExternalAccess.RoslynWorkspace;

// Copied from https://github.com/dotnet/project-system/blob/e4db47666e0a49f6c38e701f8630dbc31380fb64/src/Microsoft.VisualStudio.ProjectSystem.Managed/Threading/Tasks/TaskDelayScheduler.cs
// (with changes to remove JTF concepts)

/// <summary>
/// Helper class which allows a task to be scheduled to run after some delay, but if a new task
/// is scheduled before the delay runs out, the previous task is cancelled.
/// </summary>
internal sealed class TaskDelayScheduler
{
    private readonly TimeSpan _taskDelayTime;
    private readonly CancellationSeries _cancellationSeries;

    /// <summary>
    /// Creates an instance of the TaskDelayScheduler. If an originalSourceToken is passed, it will be linked to the PendingUpdateTokenSource so
    /// that cancelling that token will also flow through and cancel a pending update.
    /// </summary>
    public TaskDelayScheduler(TimeSpan taskDelayTime, CancellationToken originalSourceToken)
    {
        _taskDelayTime = taskDelayTime;
        _cancellationSeries = new CancellationSeries(originalSourceToken);
    }

    public void ScheduleAsyncTask(Func<CancellationToken, Task> operation, CancellationToken token)
    {
        var nextToken = _cancellationSeries.CreateNext(token);

        _ = Task.Run(async () =>
        {
            if (nextToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                await Task.Delay(_taskDelayTime, nextToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (nextToken.IsCancellationRequested)
            {
                return;
            }

            await operation(nextToken).ConfigureAwait(false);
        });
    }

    /// <summary>
    /// Cancels any pending tasks and disposes this object.
    /// </summary>
    public void Dispose()
    {
        _cancellationSeries.Dispose();
    }
}
