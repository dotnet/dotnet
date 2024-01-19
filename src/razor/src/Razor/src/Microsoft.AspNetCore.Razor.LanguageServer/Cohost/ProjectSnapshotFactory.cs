// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Cohost;

[Export(typeof(ProjectSnapshotFactory)), Shared]
[method: ImportingConstructor]
internal class ProjectSnapshotFactory(DocumentSnapshotFactory documentSnapshotFactory, ITelemetryReporter telemetryReporter, JoinableTaskContext joinableTaskContext)
{
    private static readonly ConditionalWeakTable<Project, IProjectSnapshot> _projectSnapshots = new();

    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;

    public IProjectSnapshot GetOrCreate(Project project)
    {
        if (!_projectSnapshots.TryGetValue(project, out var projectSnapshot))
        {
            projectSnapshot = new CohostProjectSnapshot(project, _documentSnapshotFactory, _telemetryReporter, _joinableTaskContext);
            _projectSnapshots.Add(project, projectSnapshot);
        }

        return projectSnapshot;
    }
}
