// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

[Shared]
[Export(typeof(RemoteSnapshotManager))]
[method: ImportingConstructor]
internal sealed class RemoteSnapshotManager(IFilePathService filePathService, ITelemetryReporter telemetryReporter)
{
    private static readonly ConditionalWeakTable<Solution, RemoteSolutionSnapshot> s_solutionToSnapshotMap = new();
    private static readonly object s_gate = new();

    public IFilePathService FilePathService { get; } = filePathService;
    public ITelemetryReporter TelemetryReporter { get; } = telemetryReporter;

    public RemoteSolutionSnapshot GetSnapshot(Solution solution)
    {
        lock (s_gate)
        {
            return s_solutionToSnapshotMap.GetValue(solution, s => new RemoteSolutionSnapshot(s, this));
        }
    }

    public RemoteProjectSnapshot GetSnapshot(Project project)
    {
        return GetSnapshot(project.Solution).GetProject(project);
    }

    public RemoteDocumentSnapshot GetSnapshot(TextDocument document)
    {
        return GetSnapshot(document.Project).GetDocument(document);
    }

    internal Project? TryGetRetryProject(Project project)
    {
        if (project.IsRetryProject())
        {
            // If the passed in project is already a retry project, then it means whatever failure the caller had is real
            return null;
        }

        lock (s_gate)
        {
            // Check if we already have performed a retry for this project. We only expect retry projects to be needed early in the life of a session,
            // so its highly likely the first few requests will all come in parallel (ie, inlay hints, folding ranges, semantic tokens) and well all
            // need to retry. This extra check means we don't create multiple retry projects for the same underlying project, and hence only run generators
            // once. Once almost anything in the project has changed, the source generator cache will be un-stuck, and this method won't be called, and
            // our retry solution snapshot will be removed from the CWT as normal.
            if (s_solutionToSnapshotMap.TryGetValue(project.Solution, out var snapshot) &&
                snapshot.GetProject(project) is { } existingProject &&
                existingProject.Project.IsRetryProject())
            {
                return existingProject.Project;
            }

            // The passed in project isn't a retry, and we don't have one already, so create one now and replace
            // our current snapshot. This means future requests will just get the right thing from their first OOP service call.
            var retryProject = project.ForkToRetryProject();
            s_solutionToSnapshotMap.Remove(project.Solution);
            s_solutionToSnapshotMap.Add(project.Solution, new RemoteSolutionSnapshot(retryProject.Solution, this));
            return retryProject;
        }
    }

    public Task<RazorCodeDocument?> TryGetRazorCodeDocumentAsync(Solution solution, Uri generatedDocumentUri, CancellationToken cancellationToken)
    {
        if (!solution.TryGetSourceGeneratedDocumentIdentity(generatedDocumentUri, out var identity) ||
            !solution.TryGetProject(identity.DocumentId.ProjectId, out var project))
        {
            return SpecializedTasks.Null<RazorCodeDocument>();
        }

        var snapshot = GetSnapshot(project);
        return snapshot.TryGetCodeDocumentForGeneratedDocumentAsync(identity, cancellationToken);
    }
}
