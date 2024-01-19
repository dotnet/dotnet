// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

[LanguageServerEndpoint(Methods.TextDocumentDidOpenName)]
internal class DocumentDidOpenEndpoint : IRazorNotificationHandler<DidOpenTextDocumentParams>
{
    public bool MutatesSolutionState => true;

    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly IRazorProjectService _projectService;

    public DocumentDidOpenEndpoint(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher, IRazorProjectService razorProjectService)
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
