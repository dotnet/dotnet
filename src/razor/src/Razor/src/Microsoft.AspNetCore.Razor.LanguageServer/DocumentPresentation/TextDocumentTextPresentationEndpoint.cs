// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal class TextDocumentTextPresentationEndpoint(
    IRazorDocumentMappingService razorDocumentMappingService,
    IClientConnection clientConnection,
    FilePathService filePathService,
    IRazorLoggerFactory loggerFactory)
    : AbstractTextDocumentPresentationEndpointBase<TextPresentationParams>(razorDocumentMappingService, clientConnection, filePathService, loggerFactory.CreateLogger<TextDocumentTextPresentationEndpoint>()), ITextDocumentTextPresentationHandler
{
    public override string EndpointName => CustomMessageNames.RazorTextPresentationEndpoint;

    public override void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.TextPresentationProvider = true;
    }

    public override TextDocumentIdentifier GetTextDocumentIdentifier(TextPresentationParams request)
    {
        return request.TextDocument;
    }

    protected override IRazorPresentationParams CreateRazorRequestParameters(TextPresentationParams request)
        => new RazorTextPresentationParams()
        {
            TextDocument = request.TextDocument,
            Range = request.Range,
            Text = request.Text
        };

    protected override Task<WorkspaceEdit?> TryGetRazorWorkspaceEditAsync(
        RazorLanguageKind languageKind,
        TextPresentationParams request,
        CancellationToken cancellationToken)
    {
        // We don't do anything special with text
        return Task.FromResult<WorkspaceEdit?>(null);
    }
}
