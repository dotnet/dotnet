// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

[RazorLanguageServerEndpoint(Methods.TextDocumentDidOpenName)]
internal class DocumentDidOpenEndpoint(IRazorProjectService razorProjectService) : IRazorNotificationHandler<DidOpenTextDocumentParams>
{
    public bool MutatesSolutionState => true;

    private readonly IRazorProjectService _projectService = razorProjectService;

    public Task HandleNotificationAsync(DidOpenTextDocumentParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var sourceText = SourceText.From(request.TextDocument.Text);

        return _projectService.OpenDocumentAsync(
            request.TextDocument.DocumentUri.GetAbsoluteOrUNCPath(), sourceText, cancellationToken);
    }
}
