// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CommonLanguageServerProtocol.Framework;

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
        return _projectService.CloseDocumentAsync(request.TextDocument.DocumentUri.GetAbsoluteOrUNCPath(), cancellationToken);
    }
}
