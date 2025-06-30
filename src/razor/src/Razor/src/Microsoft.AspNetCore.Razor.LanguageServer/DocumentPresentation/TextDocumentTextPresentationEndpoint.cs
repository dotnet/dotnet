// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal class TextDocumentTextPresentationEndpoint(
    IDocumentMappingService documentMappingService,
    IClientConnection clientConnection,
    IFilePathService filePathService,
    ILoggerFactory loggerFactory)
    : AbstractTextDocumentPresentationEndpointBase<TextPresentationParams>(documentMappingService, clientConnection, filePathService, loggerFactory.GetOrCreateLogger<TextDocumentTextPresentationEndpoint>()), ITextDocumentTextPresentationHandler
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
        return SpecializedTasks.Null<WorkspaceEdit>();
    }
}
