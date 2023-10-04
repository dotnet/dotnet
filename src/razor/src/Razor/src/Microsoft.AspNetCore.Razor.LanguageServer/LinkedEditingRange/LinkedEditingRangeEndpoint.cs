// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange;

[LanguageServerEndpoint(Methods.TextDocumentLinkedEditingRangeName)]
internal class LinkedEditingRangeEndpoint : IRazorRequestHandler<LinkedEditingRangeParams, LinkedEditingRanges?>, ICapabilitiesProvider
{
    // The regex below excludes characters that can never be valid in a TagHelper name.
    // This is loosely based off logic from the Razor compiler:
    // https://github.com/dotnet/aspnetcore/blob/9da42b9fab4c61fe46627ac0c6877905ec845d5a/src/Razor/Microsoft.AspNetCore.Razor.Language/src/Legacy/HtmlTokenizer.cs
    // Internal for testing only.
    internal static readonly string WordPattern = @"!?[^ <>!\/\?\[\]=""\\@" + Environment.NewLine + "]+";

    private readonly ILogger _logger;

    public LinkedEditingRangeEndpoint(ILoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<LinkedEditingRangeEndpoint>();
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.LinkedEditingRangeProvider = new LinkedEditingRangeOptions();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(LinkedEditingRangeParams request)
    {
        return request.TextDocument;
    }

    public async Task<LinkedEditingRanges?> HandleRequestAsync(
        LinkedEditingRangeParams request,
        RazorRequestContext requestContext,
        CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null || cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Unable to resolve document for {Uri} or cancellation was requested.", request.TextDocument.Uri);
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            _logger.LogWarning("FileKind {FileKind} is unsupported", codeDocument.GetFileKind());
            return null;
        }

        var syntaxTree = await documentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);

        var location = await GetSourceLocation(request, documentContext, cancellationToken).ConfigureAwait(false);

        // We only care if the user is within a TagHelper or HTML tag with a valid start and end tag.
        if (TryGetNearestMarkupNameTokens(syntaxTree, location, out var startTagNameToken, out var endTagNameToken) &&
            (startTagNameToken.Span.Contains(location.AbsoluteIndex) || endTagNameToken.Span.Contains(location.AbsoluteIndex) ||
            startTagNameToken.Span.End == location.AbsoluteIndex || endTagNameToken.Span.End == location.AbsoluteIndex))
        {
            var startSpan = startTagNameToken.GetLinePositionSpan(codeDocument.Source);
            var endSpan = endTagNameToken.GetLinePositionSpan(codeDocument.Source);
            var ranges = new Range[2] { startSpan.ToRange(), endSpan.ToRange() };

            return new LinkedEditingRanges
            {
                Ranges = ranges,
                WordPattern = WordPattern
            };
        }

        _logger.LogInformation("LinkedEditingRange request was null at {location} for {uri}", location, request.TextDocument.Uri);
        return null;

        static async Task<SourceLocation> GetSourceLocation(
            LinkedEditingRangeParams request,
            DocumentContext documentContext,
            CancellationToken cancellationToken)
        {
            var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
            var linePosition = new LinePosition(request.Position.Line, request.Position.Character);
            var hostDocumentIndex = sourceText.Lines.GetPosition(linePosition);
            var location = new SourceLocation(hostDocumentIndex, request.Position.Line, request.Position.Character);

            return location;
        }

        static bool TryGetNearestMarkupNameTokens(
            RazorSyntaxTree syntaxTree,
            SourceLocation location,
            [NotNullWhen(true)] out SyntaxToken? startTagNameToken,
            [NotNullWhen(true)] out SyntaxToken? endTagNameToken)
        {
            var owner = syntaxTree.Root.FindInnermostNode(location.AbsoluteIndex);
            var element = owner?.FirstAncestorOrSelf<MarkupSyntaxNode>(
                a => a.Kind is SyntaxKind.MarkupTagHelperElement || a.Kind is SyntaxKind.MarkupElement);

            if (element is null)
            {
                startTagNameToken = null;
                endTagNameToken = null;
                return false;
            }

            switch (element)
            {
                // Tag helper
                case MarkupTagHelperElementSyntax markupTagHelperElement:
                    startTagNameToken = markupTagHelperElement.StartTag?.Name;
                    endTagNameToken = markupTagHelperElement.EndTag?.Name;
                    return startTagNameToken is not null && endTagNameToken is not null;
                // HTML
                case MarkupElementSyntax markupElement:
                    startTagNameToken = markupElement.StartTag?.Name;
                    endTagNameToken = markupElement.EndTag?.Name;
                    return startTagNameToken is not null && endTagNameToken is not null;
                default:
                    throw new InvalidOperationException("Element is expected to be a MarkupTagHelperElement or MarkupElement.");
            }
        }
    }
}
