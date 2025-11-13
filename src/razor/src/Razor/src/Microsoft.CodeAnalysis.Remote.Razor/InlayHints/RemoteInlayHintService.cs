// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.Protocol.InlayHints;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteInlayHintService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteInlayHintService
{
    internal sealed class Factory : FactoryBase<IRemoteInlayHintService>
    {
        protected override IRemoteInlayHintService CreateService(in ServiceArgs args)
            => new RemoteInlayHintService(in args);
    }

    private readonly InlayHintCacheWrapperProvider _cacheWrapperProvider = args.ExportProvider.GetExportedValue<InlayHintCacheWrapperProvider>();

    public ValueTask<InlayHint[]?> GetInlayHintsAsync(JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo, JsonSerializableDocumentId razorDocumentId, InlayHintParams inlayHintParams, bool displayAllOverride, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetInlayHintsAsync(context, inlayHintParams, displayAllOverride, cancellationToken),
            cancellationToken);

    private async ValueTask<InlayHint[]?> GetInlayHintsAsync(RemoteDocumentContext context, InlayHintParams inlayHintParams, bool displayAllOverride, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetRequiredCSharpDocument();

        var span = inlayHintParams.Range.ToLinePositionSpan();

        cancellationToken.ThrowIfCancellationRequested();

        // Sometimes the client sends us a request that doesn't match the file contents. Could be a bug with old requests
        // not being cancelled, but no harm in being defensive
        if (!codeDocument.Source.Text.TryGetAbsoluteIndex(span.Start, out var startIndex) ||
            !codeDocument.Source.Text.TryGetAbsoluteIndex(span.End, out var endIndex))
        {
            return null;
        }

        // We are given a range by the client, but our mapping only succeeds if the start and end of the range can both be mapped
        // to C#. Since that doesn't logically match what we want from inlay hints, we instead get the minimum range of mappable
        // C# to get hints for. We'll filter that later, to remove the sections that can't be mapped back.
        if (!DocumentMappingService.TryMapToCSharpDocumentRange(csharpDocument, span, out var projectedLinePositionSpan) &&
            !codeDocument.TryGetMinimalCSharpRange(span, out projectedLinePositionSpan))
        {
            // There's no C# in the range.
            return null;
        }

        var generatedDocument = await context.Snapshot
            .GetGeneratedDocumentAsync(cancellationToken)
            .ConfigureAwait(false);

        var textDocument = inlayHintParams.TextDocument.WithUri(generatedDocument.CreateUri());
        var range = projectedLinePositionSpan.ToRange();

        var hints = await InlayHints.GetInlayHintsAsync(generatedDocument, textDocument, range, displayAllOverride, _cacheWrapperProvider.GetCache(), cancellationToken).ConfigureAwait(false);
        if (hints is null)
        {
            return null;
        }

        using var inlayHintsBuilder = new PooledArrayBuilder<InlayHint>();
        var razorSourceText = codeDocument.Source.Text;
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var root = codeDocument.GetRequiredSyntaxRoot();
        foreach (var hint in hints)
        {
            if (csharpSourceText.TryGetAbsoluteIndex(hint.Position.ToLinePosition(), out var absoluteIndex) &&
                DocumentMappingService.TryMapToRazorDocumentPosition(csharpDocument, absoluteIndex, out var hostDocumentPosition, out var hostDocumentIndex))
            {
                // We know this C# maps to Razor, but does it map to Razor that we like?

                // We don't want inlay hints in tag helper attributes
                var node = root.FindInnermostNode(hostDocumentIndex);
                if (node?.FirstAncestorOrSelf<MarkupTagHelperAttributeValueSyntax>() is not null)
                {
                    continue;
                }

                // Inlay hints in directives are okay, eg '@attribute [Description(description: "Desc")]', but if the hint is going to be
                // at the very start of the directive, we want to strip any TextEdit as it would make for an invalid document. eg: '// @page template: "/"'
                if (node?.SpanStart == hostDocumentIndex &&
                    node.FirstAncestorOrSelf<RazorDirectiveSyntax>(static n => n.IsDirectiveKind(DirectiveKind.SingleLine)) is not null)
                {
                    hint.TextEdits = null;
                }

                if (hint.TextEdits is not null)
                {
                    var changes = hint.TextEdits.SelectAsArray(csharpSourceText.GetTextChange);
                    var mappedChanges = DocumentMappingService.GetRazorDocumentEdits(csharpDocument, changes);
                    hint.TextEdits = mappedChanges.Select(razorSourceText.GetTextEdit).ToArray();
                }

                hint.Data = new InlayHintDataWrapper(inlayHintParams.TextDocument, hint.Data, hint.Position);
                hint.Position = hostDocumentPosition.ToPosition();

                inlayHintsBuilder.Add(hint);
            }
        }

        return inlayHintsBuilder.ToArray();
    }

    public ValueTask<InlayHint> ResolveHintAsync(JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo, JsonSerializableDocumentId razorDocumentId, InlayHint inlayHint, CancellationToken cancellationToken)
       => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => ResolveInlayHintAsync(context, inlayHint, cancellationToken),
            cancellationToken);

    private async ValueTask<InlayHint> ResolveInlayHintAsync(RemoteDocumentContext context, InlayHint inlayHint, CancellationToken cancellationToken)
    {
        var generatedDocument = await context.Snapshot
            .GetGeneratedDocumentAsync(cancellationToken)
            .ConfigureAwait(false);

        return await InlayHints.ResolveInlayHintAsync(generatedDocument, inlayHint, _cacheWrapperProvider.GetCache(), cancellationToken).ConfigureAwait(false);
    }
}
