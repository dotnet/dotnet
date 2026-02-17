// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public class ProjectLoadBenchmark : ProjectSnapshotManagerBenchmarkBase
{
    [IterationSetup]
    public void Setup()
    {
        ProjectManager = CreateProjectSnapshotManager();
    }

    private ProjectSnapshotManager ProjectManager { get; set; }

    [Benchmark(Description = "Initializes a project and 100 files", OperationsPerInvoke = 100)]
    public async Task ProjectLoad_AddProjectAnd100Files()
    {
        await ProjectManager.UpdateAsync(
            updater =>
            {
                updater.AddProject(HostProject);

                for (var i = 0; i < Documents.Length; i++)
                {
                    updater.AddDocument(HostProject.Key, Documents[i], TextLoaders[i % 4]);
                }
            },
            CancellationToken.None);
    }
}
