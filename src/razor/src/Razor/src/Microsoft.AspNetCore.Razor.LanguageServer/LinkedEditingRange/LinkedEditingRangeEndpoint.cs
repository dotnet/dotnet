// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.LinkedEditingRange;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange;

[RazorLanguageServerEndpoint(Methods.TextDocumentLinkedEditingRangeName)]
internal class LinkedEditingRangeEndpoint(ILoggerFactory loggerFactory)
    : IRazorRequestHandler<LinkedEditingRangeParams, LinkedEditingRanges?>, ICapabilitiesProvider
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<LinkedEditingRangeEndpoint>();

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
            _logger.LogWarning($"Unable to resolve document for {request.TextDocument.Uri} or cancellation was requested.");
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            _logger.LogWarning($"FileKind {codeDocument.FileKind} is unsupported");
            return null;
        }

        if (LinkedEditingRangeHelper.GetLinkedSpans(request.Position.ToLinePosition(), codeDocument) is { } linkedSpans && linkedSpans.Length == 2)
        {
            var ranges = new[] { linkedSpans[0].ToRange(), linkedSpans[1].ToRange() };

            return new LinkedEditingRanges
            {
                Ranges = ranges,
                WordPattern = LinkedEditingRangeHelper.WordPattern
            };
        }

        _logger.LogInformation($"LinkedEditingRange request was null at line {request.Position.Line}, column {request.Position.Character} for {request.TextDocument.Uri}");

        return null;
    }
}
