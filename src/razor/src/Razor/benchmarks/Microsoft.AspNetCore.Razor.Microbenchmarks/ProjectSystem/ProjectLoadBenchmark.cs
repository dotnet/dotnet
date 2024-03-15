// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
        await Dispatcher.RunAsync(
            () =>
            {
                ProjectManager.ProjectAdded(HostProject);

                for (var i = 0; i < Documents.Length; i++)
                {
                    ProjectManager.DocumentAdded(HostProject.Key, Documents[i], TextLoaders[i % 4]);
                }
            },
            CancellationToken.None);
    }
}
