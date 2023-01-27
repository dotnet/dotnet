// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

internal class RazorDidOpenTextDocumentEndpoint : IVSDidOpenTextDocumentEndpoint
{
    public bool MutatesSolutionState => true;

    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly RazorProjectService _projectService;

    public RazorDidOpenTextDocumentEndpoint(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher, RazorProjectService razorProjectService)
    {
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectService = razorProjectService;
    }

    public async Task HandleNotificationAsync(DidOpenTextDocumentParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var sourceText = SourceText.From(request.TextDocument.Text);

        await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            () => _projectService.OpenDocument(request.TextDocument.Uri.GetAbsoluteOrUNCPath(), sourceText, request.TextDocument.Version),
            cancellationToken).ConfigureAwait(false);
    }
}
