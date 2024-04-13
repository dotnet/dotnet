// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

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
            request.TextDocument.Uri.GetAbsoluteOrUNCPath(), sourceText, request.TextDocument.Version, cancellationToken);
    }
}
