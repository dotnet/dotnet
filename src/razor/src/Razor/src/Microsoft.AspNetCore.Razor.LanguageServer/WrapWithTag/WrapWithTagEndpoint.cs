// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

[LanguageServerEndpoint(LanguageServerConstants.RazorWrapWithTagEndpoint)]
internal class WrapWithTagEndpoint : IRazorRequestHandler<WrapWithTagParams, WrapWithTagResponse?>
{
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly IRazorDocumentMappingService _razorDocumentMappingService;

    public WrapWithTagEndpoint(
        ClientNotifierServiceBase languageServer,
        IRazorDocumentMappingService razorDocumentMappingService)
    {
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _razorDocumentMappingService = razorDocumentMappingService ?? throw new ArgumentNullException(nameof(razorDocumentMappingService));
    }

    public bool MutatesSolutionState => false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(WrapWithTagParams request)
    {
        return request.TextDocument;
    }

    public async Task<WrapWithTagResponse?> HandleRequestAsync(WrapWithTagParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            requestContext.Logger.LogWarning("Failed to find document {textDocumentUri}.", request.TextDocument.Uri);
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            requestContext.Logger.LogWarning("Failed to retrieve generated output for document {textDocumentUri}.", request.TextDocument.Uri);
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (request.Range?.Start.TryGetAbsoluteIndex(sourceText, requestContext.Logger, out var hostDocumentIndex) != true)
        {
            return null;
        }

        // Since we're at the start of the selection, lets prefer the language to the right of the cursor if possible.
        // That way with the following situation:
        //
        // @if (true) {
        //   |<p></p>
        // }
        //
        // Instead of C#, which certainly would be expected to go in an if statement, we'll see HTML, which obviously
        // is the better choice for this operation.
        var languageKind = _razorDocumentMappingService.GetLanguageKind(codeDocument, hostDocumentIndex, rightAssociative: true);
        if (languageKind is not RazorLanguageKind.Html)
        {
            // In general, we don't support C# for obvious reasons, but we can support implicit expressions. ie
            //
            // <p>@curr$$entCount</p>
            //
            // We can expand the range to encompass the whole implicit expression, and then it will wrap as expected.
            // Similarly if they have selected the implicit expression, then we can continue. ie
            //
            // <p>[|@currentCount|]</p>

            var tree = await documentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var requestSpan = request.Range.ToRazorTextSpan(sourceText);
            var node = tree.Root.FindNode(requestSpan, includeWhitespace: false, getInnermostNodeForTie: true);
            if (node?.FirstAncestorOrSelf<CSharpImplicitExpressionSyntax>() is { Parent: CSharpCodeBlockSyntax codeBlock } &&
                (requestSpan == codeBlock.FullSpan || requestSpan.Length == 0))
            {
                // Pretend we're in Html so the rest of the logic can continue
                request.Range = codeBlock.FullSpan.ToRange(sourceText);
                languageKind = RazorLanguageKind.Html;
            }
        }

        if (languageKind is not RazorLanguageKind.Html)
        {
            requestContext.Logger.LogInformation("Unsupported language {languageKind:G}.", languageKind);
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var versioned = new VersionedTextDocumentIdentifier
        {
            Uri = request.TextDocument.Uri,
            Version = documentContext.Version,
        };
        var parameter = new DelegatedWrapWithTagParams(versioned, request);

        var htmlResponse = await _languageServer.SendRequestAsync<DelegatedWrapWithTagParams, WrapWithTagResponse>(
            LanguageServerConstants.RazorWrapWithTagEndpoint,
            parameter,
            cancellationToken).ConfigureAwait(false);

        if (htmlResponse.TextEdits is not null)
        {
            var htmlSourceText = await documentContext.GetHtmlSourceTextAsync(cancellationToken).ConfigureAwait(false);
            htmlResponse.TextEdits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, htmlResponse.TextEdits);
        }

        return htmlResponse;
    }
}
