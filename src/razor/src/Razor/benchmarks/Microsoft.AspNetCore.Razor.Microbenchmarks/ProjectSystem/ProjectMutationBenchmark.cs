// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public class ProjectMutationBenchmark : ProjectSnapshotManagerBenchmarkBase
{
    public ProjectMutationBenchmark()
        : base(100000)
    {
    }

    private Thread _addRemoveThread;
    private Thread _readThread;

    [IterationSetup]
    public void Setup()
    {
        ProjectManager = CreateProjectSnapshotManager();
    }

    private ProjectSnapshotManager ProjectManager { get; set; }

    [Benchmark(Description = "Does thread contention add/remove of documents", OperationsPerInvoke = 100)]
    public async Task ProjectMutation_Mutates100kFilesAsync()
    {
        await ProjectManager.UpdateAsync(
            updater => updater.AddProject(HostProject),
            CancellationToken.None);

        var cancellationSource = new CancellationTokenSource();
        var done = false;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
        _addRemoveThread = new Thread(async () =>
        {
            for (var i = 0; i < Documents.Length; i++)
            {
                var document = Documents[i];
                await ProjectManager.UpdateAsync(updater => updater.AddDocument(HostProject.Key, document, TextLoaders[i % 4]), CancellationToken.None).ConfigureAwait(false);
                Thread.Sleep(0);
                await ProjectManager.UpdateAsync(updater => updater.RemoveDocument(HostProject.Key, document.FilePath), CancellationToken.None).ConfigureAwait(false);
                Thread.Sleep(0);
            }

            cancellationSource.Cancel();
        });

        _readThread = new Thread(() =>
        {
            while (true)
            {
                if (cancellationSource.IsCancellationRequested)
                {
                    done = true;
                    return;
                }

                _ = ProjectManager.GetProjects();
                Thread.Sleep(0);
                _ = ProjectManager.GetOpenDocuments();
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
}
