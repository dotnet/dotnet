// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol.InlayHints;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Roslyn.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed partial class RemoteInlayHintService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteInlayHintService
{
    internal sealed class Factory : FactoryBase<IRemoteInlayHintService>
    {
        protected override IRemoteInlayHintService CreateService(in ServiceArgs args)
            => new RemoteInlayHintService(in args);
    }

    private readonly IDocumentMappingService _documentMappingService = args.ExportProvider.GetExportedValue<IDocumentMappingService>();
    private readonly IFilePathService _filePathService = args.ExportProvider.GetExportedValue<IFilePathService>();

    public ValueTask<InlayHint[]?> GetInlayHintsAsync(JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo, JsonSerializableDocumentId razorDocumentId, InlayHintParams inlayHintParams, bool displayAllOverride, CancellationToken cancellationToken)
       => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetInlayHintsAsync(context, inlayHintParams, displayAllOverride, cancellationToken),
            cancellationToken);

    private async ValueTask<InlayHint[]?> GetInlayHintsAsync(RemoteDocumentContext context, InlayHintParams inlayHintParams, bool displayAllOverride, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetCSharpDocument();

        var span = inlayHintParams.Range.ToLinePositionSpan();

        // We are given a range by the client, but our mapping only succeeds if the start and end of the range can both be mapped
        // to C#. Since that doesn't logically match what we want from inlay hints, we instead get the minimum range of mappable
        // C# to get hints for. We'll filter that later, to remove the sections that can't be mapped back.
        if (!_documentMappingService.TryMapToGeneratedDocumentRange(csharpDocument, span, out var projectedLinePositionSpan) &&
            !codeDocument.TryGetMinimalCSharpRange(span, out projectedLinePositionSpan))
        {
            // There's no C# in the range.
            return null;
        }

        var generatedDocument = await context.GetGeneratedDocumentAsync(_filePathService, cancellationToken).ConfigureAwait(false);

        var textDocument = inlayHintParams.TextDocument.WithUri(generatedDocument.CreateUri());
        var range = projectedLinePositionSpan.ToRange();

        var hints = await InlayHints.GetInlayHintsAsync(generatedDocument, textDocument, range, displayAllOverride, cancellationToken).ConfigureAwait(false);

        if (hints is null)
        {
            return null;
        }

        using var inlayHintsBuilder = new PooledArrayBuilder<InlayHint>();
        var razorSourceText = codeDocument.Source.Text;
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var syntaxTree = codeDocument.GetSyntaxTree();
        foreach (var hint in hints)
        {
            if (csharpSourceText.TryGetAbsoluteIndex(hint.Position.ToLinePosition(), out var absoluteIndex) &&
                _documentMappingService.TryMapToHostDocumentPosition(csharpDocument, absoluteIndex, out var hostDocumentPosition, out var hostDocumentIndex))
            {
                // We know this C# maps to Razor, but does it map to Razor that we like?
                var node = syntaxTree.Root.FindInnermostNode(hostDocumentIndex);
                if (node?.FirstAncestorOrSelf<MarkupTagHelperAttributeValueSyntax>() is not null)
                {
                    continue;
                }

                if (hint.TextEdits is not null)
                {
                    var changes = hint.TextEdits.Select(csharpSourceText.GetTextChange);
                    var mappedChanges = _documentMappingService.GetHostDocumentEdits(csharpDocument, changes);
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
        var generatedDocument = await context.GetGeneratedDocumentAsync(_filePathService, cancellationToken).ConfigureAwait(false);

        return await InlayHints.ResolveInlayHintAsync(generatedDocument, inlayHint, cancellationToken).ConfigureAwait(false);
    }
}
