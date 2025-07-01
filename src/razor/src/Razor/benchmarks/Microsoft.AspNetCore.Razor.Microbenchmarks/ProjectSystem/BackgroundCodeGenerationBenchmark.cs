// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public class BackgroundCodeGenerationBenchmark : ProjectSnapshotManagerBenchmarkBase
{
    [IterationSetup]
    public async Task SetupAsync()
    {
        ProjectManager = CreateProjectSnapshotManager();

        await ProjectManager.UpdateAsync(
            updater => updater.AddProject(HostProject),
            CancellationToken.None);

        ProjectManager.Changed += SnapshotManager_Changed;
    }

    [IterationCleanup]
    public void Cleanup()
    {
        ProjectManager.Changed -= SnapshotManager_Changed;

        Tasks.Clear();
    }

    private List<Task> Tasks { get; } = new List<Task>();

    private ProjectSnapshotManager ProjectManager { get; set; }

    [Benchmark(Description = "Generates the code for 100 files", OperationsPerInvoke = 100)]
    public async Task BackgroundCodeGeneration_Generate100FilesAsync()
    {
        await ProjectManager.UpdateAsync(
            updater =>
            {
                for (var i = 0; i < Documents.Length; i++)
                {
                    updater.AddDocument(HostProject.Key, Documents[i], TextLoaders[i % 4]);
                }
            },
            CancellationToken.None);

        await Task.WhenAll(Tasks);
    }

    private void SnapshotManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        // The real work happens here.
        var document = ProjectManager.GetRequiredDocument(e.ProjectKey, e.DocumentFilePath);

        Tasks.Add(document.GetGeneratedOutputAsync(CancellationToken.None).AsTask());
    }
}
