// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

[Shared]
[Export(typeof(RemoteSnapshotManager))]
[method: ImportingConstructor]
internal sealed class RemoteSnapshotManager(LanguageServerFeatureOptions languageServerFeatureOptions, IFilePathService filePathService, ITelemetryReporter telemetryReporter)
{
    private static readonly ConditionalWeakTable<Solution, RemoteSolutionSnapshot> s_solutionToSnapshotMap = new();

    public RazorCompilerOptions CompilerOptions { get; } = languageServerFeatureOptions.ToCompilerOptions();
    public IFilePathService FilePathService { get; } = filePathService;
    public ITelemetryReporter TelemetryReporter { get; } = telemetryReporter;

    public RemoteSolutionSnapshot GetSnapshot(Solution solution)
    {
        return s_solutionToSnapshotMap.GetValue(solution, s => new RemoteSolutionSnapshot(s, this));
    }

    public RemoteProjectSnapshot GetSnapshot(Project project)
    {
        return GetSnapshot(project.Solution).GetProject(project);
    }

    public RemoteDocumentSnapshot GetSnapshot(TextDocument document)
    {
        return GetSnapshot(document.Project).GetDocument(document);
    }
}
