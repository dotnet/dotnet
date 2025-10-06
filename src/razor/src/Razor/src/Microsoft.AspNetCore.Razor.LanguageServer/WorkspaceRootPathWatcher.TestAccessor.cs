// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class WorkspaceRootPathWatcher
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(WorkspaceRootPathWatcher instance)
    {
        public void AddWork(string filePath, RazorFileChangeKind kind)
        {
            if (kind is not (RazorFileChangeKind.Added or RazorFileChangeKind.Removed))
            {
                throw new ArgumentException("Only adds and removes are allowed", nameof(kind));
            }

            instance._workQueue.AddWork((filePath, kind));
        }

        public void AddWork(params (string filePath, RazorFileChangeKind kind)[] items)
        {
            foreach (var (filePath, kind) in items)
            {
                if (kind is not (RazorFileChangeKind.Added or RazorFileChangeKind.Removed))
                {
                    throw new ArgumentException("Only adds and removes are allowed", nameof(items));
                }
            }

            instance._workQueue.AddWork(items);
        }

        public Task WaitUntilCurrentBatchCompletesAsync()
        {
            return instance._workQueue.WaitUntilCurrentBatchCompletesAsync();
        }
    }
}
