// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal abstract class TagHelperResolver : IWorkspaceService
{
    private readonly ITelemetryReporter _telemetryReporter;
    private CompilationTagHelperResolver? _compilationTagHelperResolver;

    public TagHelperResolver(ITelemetryReporter telemetryReporter)
    {
        _telemetryReporter = telemetryReporter;
    }

    public abstract Task<TagHelperResolutionResult> GetTagHelpersAsync(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken = default);


    protected Task<TagHelperResolutionResult> GetTagHelpersAsync(Project workspaceProject, RazorProjectEngine engine, CancellationToken cancellationToken)
    {
        _compilationTagHelperResolver ??= new CompilationTagHelperResolver(_telemetryReporter);

        return _compilationTagHelperResolver.GetTagHelpersAsync(workspaceProject, engine, cancellationToken);
    }
    protected virtual Task<TagHelperResolutionResult> GetTagHelpersAsync(Project workspaceProject, RazorProjectEngine engine) => GetTagHelpersAsync(workspaceProject, engine, CancellationToken.None);
}
