// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

internal class RazorDidCloseTextDocumentEndpoint : IVSDidCloseTextDocumentEndpoint
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly RazorProjectService _projectService;

    public bool MutatesSolutionState => true;

    public RazorDidCloseTextDocumentEndpoint(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        RazorProjectService projectService)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectService = projectService;
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DidCloseTextDocumentParams request)
    {
        return request.TextDocument;
    }

    public async Task HandleNotificationAsync(DidCloseTextDocumentParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            () => _projectService.CloseDocument(request.TextDocument.Uri.GetAbsoluteOrUNCPath()),
            cancellationToken).ConfigureAwait(false);
    }
}
