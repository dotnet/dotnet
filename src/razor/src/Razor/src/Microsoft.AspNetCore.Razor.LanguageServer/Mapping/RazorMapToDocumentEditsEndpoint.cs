// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

[LanguageServerEndpoint(LanguageServerConstants.RazorMapToDocumentEditsEndpoint)]
internal sealed class RazorMapToDocumentEditsEndpoint : IRazorRequestHandler<RazorMapToDocumentEditsParams, RazorMapToDocumentEditsResponse>
{
    public bool MutatesSolutionState { get; } = false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(RazorMapToDocumentEditsParams request)
    {
        return new TextDocumentIdentifier
        {
            Uri = request.RazorDocumentUri
        };
    }

    public async Task<RazorMapToDocumentEditsResponse> HandleRequestAsync(RazorMapToDocumentEditsParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var documentContext = requestContext.GetRequiredDocumentContext();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return new RazorMapToDocumentEditsResponse()
            {
                TextEdits = Array.Empty<TextEdit>(),
                HostDocumentVersion = documentContext.Version
            };
        }

        var razorFormattingService = requestContext.GetRequiredService<IRazorFormattingService>();
        if (request.TextEditKind == TextEditKind.FormatOnType)
        {
            var mappedEdits = await razorFormattingService.FormatOnTypeAsync(documentContext, request.Kind, request.ProjectedTextEdits, request.FormattingOptions, hostDocumentIndex: 0, triggerCharacter: '\0', cancellationToken).ConfigureAwait(false);

            return new RazorMapToDocumentEditsResponse()
            {
                TextEdits = mappedEdits,
                HostDocumentVersion = documentContext.Version,
            };
        }
        else if (request.TextEditKind == TextEditKind.Snippet)
        {
            if (request.Kind == RazorLanguageKind.CSharp)
            {
                WrapCSharpSnippets(request.ProjectedTextEdits);
            }

            var mappedEdits = await razorFormattingService.FormatSnippetAsync(documentContext, request.Kind, request.ProjectedTextEdits, request.FormattingOptions, cancellationToken).ConfigureAwait(false);

            if (request.Kind == RazorLanguageKind.CSharp)
            {
                UnwrapCSharpSnippets(mappedEdits);
            }

            return new RazorMapToDocumentEditsResponse()
            {
                TextEdits = mappedEdits,
                HostDocumentVersion = documentContext.Version,
            };
        }

        if (request.Kind != RazorLanguageKind.CSharp)
        {
            // All other non-C# requests map directly to where they are in the document.
            return new RazorMapToDocumentEditsResponse()
            {
                TextEdits = request.ProjectedTextEdits,
                HostDocumentVersion = documentContext.Version,
            };
        }

        var documentMappingService = requestContext.GetRequiredService<IRazorDocumentMappingService>();
        var edits = new List<TextEdit>();
        for (var i = 0; i < request.ProjectedTextEdits.Length; i++)
        {
            var projectedRange = request.ProjectedTextEdits[i].Range;
            if (!documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), projectedRange, out var originalRange))
            {
                // Can't map range. Discard this edit.
                continue;
            }

            var edit = new TextEdit()
            {
                Range = originalRange,
                NewText = request.ProjectedTextEdits[i].NewText
            };

            edits.Add(edit);
        }

        return new RazorMapToDocumentEditsResponse()
        {
            TextEdits = edits.ToArray(),
            HostDocumentVersion = documentContext.Version,
        };

        static void WrapCSharpSnippets(TextEdit[] snippetEdits)
        {
            for (var i = 0; i < snippetEdits.Length; i++)
            {
                var snippetEdit = snippetEdits[i];

                // Formatting doesn't work with syntax errors caused by the cursor marker ($0).
                // So, let's avoid the error by wrapping the cursor marker in a comment.
                var wrappedText = snippetEdit.NewText.Replace("$0", "/*$0*/");
                snippetEdit.NewText = wrappedText;
            }
        }

        static void UnwrapCSharpSnippets(TextEdit[] snippetEdits)
        {
            for (var i = 0; i < snippetEdits.Length; i++)
            {
                var snippetEdit = snippetEdits[i];

                // Unwrap the cursor marker.
                var unwrappedText = snippetEdit.NewText.Replace("/*$0*/", "$0");
                snippetEdit.NewText = unwrappedText;
            }
        }
    }
}
