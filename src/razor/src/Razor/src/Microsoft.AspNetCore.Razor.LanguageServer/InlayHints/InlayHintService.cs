// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.InlayHints;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;

internal sealed class InlayHintService(IRazorDocumentMappingService documentMappingService) : IInlayHintService
{
    private readonly IRazorDocumentMappingService _documentMappingService = documentMappingService;

    public async Task<InlayHint[]?> GetInlayHintsAsync(IClientConnection clientConnection, VersionedDocumentContext documentContext, Range range, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetCSharpDocument();

        var span = range.ToLinePositionSpan();

        // We are given a range by the client, but our mapping only succeeds if the start and end of the range can both be mapped
        // to C#. Since that doesn't logically match what we want from inlay hints, we instead get the minimum range of mappable
        // C# to get hints for. We'll filter that later, to remove the sections that can't be mapped back.
        if (!_documentMappingService.TryMapToGeneratedDocumentRange(csharpDocument, span, out var projectedLinePositionSpan) &&
            !codeDocument.TryGetMinimalCSharpRange(span, out projectedLinePositionSpan))
        {
            // There's no C# in the range.
            return null;
        }

        // For now we only support C# inlay hints. Once Web Tools adds support we'll need to request from both servers and combine
        // the results, much like folding ranges.
        var delegatedRequest = new DelegatedInlayHintParams(
            Identifier: documentContext.Identifier,
            ProjectedRange: projectedLinePositionSpan.ToRange(),
            ProjectedKind: RazorLanguageKind.CSharp
        );

        var inlayHints = await clientConnection.SendRequestAsync<DelegatedInlayHintParams, InlayHint[]?>(
            CustomMessageNames.RazorInlayHintEndpoint,
            delegatedRequest,
            cancellationToken).ConfigureAwait(false);

        if (inlayHints is null)
        {
            return null;
        }

        var csharpSourceText = codeDocument.GetCSharpSourceText();
        using var _1 = ArrayBuilderPool<InlayHint>.GetPooledObject(out var inlayHintsBuilder);
        foreach (var hint in inlayHints)
        {
            if (hint.Position.TryGetAbsoluteIndex(csharpSourceText, null, out var absoluteIndex) &&
                _documentMappingService.TryMapToHostDocumentPosition(csharpDocument, absoluteIndex, out Position? hostDocumentPosition, out _))
            {
                if (hint.TextEdits is not null)
                {
                    hint.TextEdits = _documentMappingService.GetHostDocumentEdits(csharpDocument, hint.TextEdits);
                }

                hint.Data = new RazorInlayHintWrapper
                {
                    TextDocument = documentContext.Identifier,
                    OriginalData = hint.Data,
                    OriginalPosition = hint.Position
                };
                hint.Position = hostDocumentPosition;
                inlayHintsBuilder.Add(hint);
            }
        }

        return inlayHintsBuilder.ToArray();
    }

    public async Task<InlayHint?> ResolveInlayHintAsync(IClientConnection clientConnection, InlayHint inlayHint, CancellationToken cancellationToken)
    {
        var inlayHintWrapper = inlayHint.Data as RazorInlayHintWrapper;
        if (inlayHintWrapper is null &&
            inlayHint.Data is JObject dataObj)
        {
            inlayHintWrapper = dataObj.ToObject<RazorInlayHintWrapper>();
        }

        if (inlayHintWrapper is null)
        {
            return null;
        }

        var razorPosition = inlayHint.Position;
        inlayHint.Position = inlayHintWrapper.OriginalPosition;
        inlayHint.Data = inlayHintWrapper.OriginalData;

        // For now we only support C# inlay hints. Once Web Tools adds support we'll need to inlayHint from both servers and combine
        // the results, much like folding ranges.
        var delegatedRequest = new DelegatedInlayHintResolveParams(
            Identifier: inlayHintWrapper.TextDocument,
            InlayHint: inlayHint,
            ProjectedKind: RazorLanguageKind.CSharp
        );

        var resolvedHint = await clientConnection.SendRequestAsync<DelegatedInlayHintResolveParams, InlayHint?>(
            CustomMessageNames.RazorInlayHintResolveEndpoint,
            delegatedRequest,
            cancellationToken).ConfigureAwait(false);

        if (resolvedHint is null)
        {
            return null;
        }

        Debug.Assert(inlayHint.Position == resolvedHint.Position, "Resolving inlay hints should not change the position of them.");
        resolvedHint.Position = razorPosition;

        return resolvedHint;
    }
}
