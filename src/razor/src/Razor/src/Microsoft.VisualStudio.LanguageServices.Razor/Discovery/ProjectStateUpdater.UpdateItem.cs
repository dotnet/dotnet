// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal sealed partial class ProjectStateUpdater
{
    private sealed class UpdateItem
    {
        private readonly Task _task;
        private readonly CancellationTokenSource _tokenSource;

        public bool IsCancellationRequested => _tokenSource.IsCancellationRequested;
        public bool IsCompleted => _task.IsCompleted;
        public bool IsRunning => !IsCompleted && !IsCancellationRequested;

        private UpdateItem(Task task, CancellationTokenSource tokenSource)
        {
            _task = task;
            _tokenSource = tokenSource;
        }

        public static UpdateItem CreateAndStartWork(Func<CancellationToken, Task> updater)
        {
            var tokenSource = new CancellationTokenSource();

            var task = Task.Run(
                () => updater(tokenSource.Token),
                tokenSource.Token);

            return new(task, tokenSource);
        }

        public void CancelWorkAndCleanUp()
        {
            if (_tokenSource.IsCancellationRequested)
            {
                return;
            }

            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }
}
