// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
            updater => updater.ProjectAdded(HostProject),
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
                    updater.DocumentAdded(HostProject.Key, Documents[i], TextLoaders[i % 4]);
                }
            },
            CancellationToken.None);

        await Task.WhenAll(Tasks);
    }

    private void SnapshotManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        // The real work happens here.
        var project = ProjectManager.GetLoadedProject(e.ProjectKey);
        var document = project.GetDocument(e.DocumentFilePath);

        Tasks.Add(document.GetGeneratedOutputAsync(CancellationToken.None).AsTask());
    }
}
