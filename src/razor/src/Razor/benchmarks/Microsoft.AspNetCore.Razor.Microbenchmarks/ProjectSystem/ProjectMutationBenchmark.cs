// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public class ProjectMutationBenchmark : ProjectSnapshotManagerBenchmarkBase
{
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;

    public ProjectMutationBenchmark()
        : base(100000)
    {
        _dispatcher = new SnapshotDispatcher(nameof(ProjectMutationBenchmark));
    }

    private Thread _addRemoveThread;
    private Thread _readThread;

    [IterationSetup]
    public void Setup()
    {
        SnapshotManager = CreateProjectSnapshotManager();
    }

    private DefaultProjectSnapshotManager SnapshotManager { get; set; }

    [Benchmark(Description = "Does thread contention add/remove of documents", OperationsPerInvoke = 100)]
    public async Task ProjectMutation_Mutates100kFiles()
    {
        await _dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            SnapshotManager.ProjectAdded(HostProject);
        }, CancellationToken.None);

        var cancellationSource = new CancellationTokenSource();
        var done = false;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
        _addRemoveThread = new Thread(async () =>
        {
            for (var i = 0; i < Documents.Length; i++)
            {
                var document = Documents[i];
                await _dispatcher.RunOnDispatcherThreadAsync(() => SnapshotManager.DocumentAdded(HostProject.Key, document, TextLoaders[i % 4]), CancellationToken.None).ConfigureAwait(false);
                Thread.Sleep(0);
                await _dispatcher.RunOnDispatcherThreadAsync(() => SnapshotManager.DocumentRemoved(HostProject.Key, document), CancellationToken.None).ConfigureAwait(false);
                Thread.Sleep(0);
            }

            cancellationSource.Cancel();
        });

        _readThread = new Thread(async () =>
        {
            while (true)
            {
                if (cancellationSource.IsCancellationRequested)
                {
                    done = true;
                    return;
                }

                await _dispatcher.RunOnDispatcherThreadAsync(() => SnapshotManager.GetProjects(), CancellationToken.None).ConfigureAwait(false);
                Thread.Sleep(0);
                await _dispatcher.RunOnDispatcherThreadAsync(() => SnapshotManager.GetOpenDocuments(), CancellationToken.None).ConfigureAwait(false);
                Thread.Sleep(0);
            }
        });

        _addRemoveThread.Start();
        _readThread.Start();
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates

        while (!done)
        {
            Thread.Sleep(0);
        }
    }

    private class SnapshotDispatcher : ProjectSnapshotManagerDispatcherBase
    {
        public SnapshotDispatcher(string threadName) : base(threadName)
        {
        }

        public override void LogException(Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
