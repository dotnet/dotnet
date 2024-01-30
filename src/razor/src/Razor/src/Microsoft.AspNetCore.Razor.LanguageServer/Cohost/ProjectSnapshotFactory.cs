// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Cohost;

[Export(typeof(ProjectSnapshotFactory)), Shared]
[method: ImportingConstructor]
internal class ProjectSnapshotFactory(DocumentSnapshotFactory documentSnapshotFactory, ITelemetryReporter telemetryReporter)
{
    private static readonly ConditionalWeakTable<Project, CohostProjectSnapshot> _projectSnapshots = new();

    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    public CohostProjectSnapshot GetOrCreate(Project project)
    {
        if (!_projectSnapshots.TryGetValue(project, out var projectSnapshot))
        {
            projectSnapshot = new CohostProjectSnapshot(project, _documentSnapshotFactory, _telemetryReporter);
            _projectSnapshots.Add(project, projectSnapshot);
        }

        return projectSnapshot;
    }
}
