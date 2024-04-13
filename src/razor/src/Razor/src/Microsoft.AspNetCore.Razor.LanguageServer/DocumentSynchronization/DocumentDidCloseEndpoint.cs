// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

[RazorLanguageServerEndpoint(Methods.TextDocumentDidCloseName)]
internal class DocumentDidCloseEndpoint(IRazorProjectService projectService) : IRazorNotificationHandler<DidCloseTextDocumentParams>, ITextDocumentIdentifierHandler<DidCloseTextDocumentParams, TextDocumentIdentifier>
{
    private readonly IRazorProjectService _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));

    public bool MutatesSolutionState => true;

    public TextDocumentIdentifier GetTextDocumentIdentifier(DidCloseTextDocumentParams request)
    {
        return request.TextDocument;
    }

    public Task HandleNotificationAsync(DidCloseTextDocumentParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        return _projectService.CloseDocumentAsync(request.TextDocument.Uri.GetAbsoluteOrUNCPath(), cancellationToken);
    }
}
