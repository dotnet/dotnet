// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

[RazorLanguageServerEndpoint(LanguageServerConstants.RazorWrapWithTagEndpoint)]
internal class WrapWithTagEndpoint(IClientConnection clientConnection, ILoggerFactory loggerFactory) : IRazorRequestHandler<WrapWithTagParams, WrapWithTagResponse?>
{
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<WrapWithTagEndpoint>();

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
            _logger.LogWarning($"Failed to find document {request.TextDocument.DocumentUri}.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = codeDocument.Source.Text;

        if (request.Range?.Start is not { } start ||
            !sourceText.TryGetAbsoluteIndex(start, out var hostDocumentIndex))
        {
            return null;
        }

        // First thing we do is make sure we start at a non-whitespace character. This is important because in some
        // situations the whitespace can be technically C#, but move one character to the right and it's HTML. eg
        //
        // @if (true) {
        //   |   <p></p>
        // }
        //
        // Limiting this to only whitespace on the same line, as it's not clear what user expectation would be otherwise.
        var requestSpan = sourceText.GetTextSpan(request.Range);
        if (sourceText.TryGetFirstNonWhitespaceOffset(requestSpan, out var offset, out var newLineCount) &&
            newLineCount == 0)
        {
            request.Range.Start.Character += offset;
            requestSpan = sourceText.GetTextSpan(request.Range);
            hostDocumentIndex += offset;
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
        var languageKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: true);

        // However, reverse scenario is possible as well, when we have
        // <div>
        // |@if (true) {}
        // <p></p>
        // </div>
        // in which case right-associative GetLanguageKind will return Razor and left-associative will return HTML
        // We should hand that case as well, see https://github.com/dotnet/razor/issues/10819
        if (languageKind is RazorLanguageKind.Razor)
        {
            languageKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: false);
        }

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
            var node = tree.Root.FindNode(requestSpan, includeWhitespace: false, getInnermostNodeForTie: true);
            if (node?.FirstAncestorOrSelf<CSharpImplicitExpressionSyntax>() is { Parent: CSharpCodeBlockSyntax codeBlock } &&
                (requestSpan == codeBlock.Span || requestSpan.Length == 0))
            {
                // Pretend we're in Html so the rest of the logic can continue
                request.Range = sourceText.GetRange(codeBlock.Span);
                languageKind = RazorLanguageKind.Html;
            }
        }

        if (languageKind is not RazorLanguageKind.Html)
        {
            _logger.LogInformation($"Unsupported language {languageKind:G}.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var versioned = new VersionedTextDocumentIdentifier
        {
            DocumentUri = request.TextDocument.DocumentUri,
            Version = documentContext.Snapshot.Version,
        };
        var parameter = new DelegatedWrapWithTagParams(versioned, request);

        var htmlResponse = await _clientConnection.SendRequestAsync<DelegatedWrapWithTagParams, WrapWithTagResponse>(
            LanguageServerConstants.RazorWrapWithTagEndpoint,
            parameter,
            cancellationToken).ConfigureAwait(false);

        if (htmlResponse.TextEdits is not null)
        {
            var htmlSourceText = await documentContext.GetHtmlSourceTextAsync(cancellationToken).ConfigureAwait(false);
            htmlResponse.TextEdits = FormattingUtilities.FixHtmlTextEdits(htmlSourceText, htmlResponse.TextEdits);
        }

        return htmlResponse;
    }
}
