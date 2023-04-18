// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

[LanguageServerEndpoint(Methods.TextDocumentDidSaveName)]
internal class DocumentDidSaveEndpoint : IRazorNotificationHandler<DidSaveTextDocumentParams>, ITextDocumentIdentifierHandler<DidSaveTextDocumentParams, TextDocumentIdentifier>
{
    public bool MutatesSolutionState => false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(DidSaveTextDocumentParams request)
    {
        return request.TextDocument;
    }

    public Task HandleNotificationAsync(DidSaveTextDocumentParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        requestContext.Logger.LogInformation($"Saved Document {request.TextDocument.Uri.GetAbsoluteOrUNCPath()}");

        return Task.CompletedTask;
    }
}
