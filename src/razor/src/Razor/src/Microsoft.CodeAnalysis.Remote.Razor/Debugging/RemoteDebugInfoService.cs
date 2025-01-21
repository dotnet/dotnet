// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteDebugInfoService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteDebugInfoService
{
    internal sealed class Factory : FactoryBase<IRemoteDebugInfoService>
    {
        protected override IRemoteDebugInfoService CreateService(in ServiceArgs args)
            => new RemoteDebugInfoService(in args);
    }

    private readonly IDocumentMappingService _documentMappingService = args.ExportProvider.GetExportedValue<IDocumentMappingService>();

    public ValueTask<LinePositionSpan?> ValidateBreakableRangeAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId documentId, LinePositionSpan span, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => ValidateBreakableRangeAsync(context, span, cancellationToken),
            cancellationToken);

    public async ValueTask<LinePositionSpan?> ValidateBreakableRangeAsync(RemoteDocumentContext context, LinePositionSpan span, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetCSharpDocument();

        if (!_documentMappingService.TryMapToGeneratedDocumentRange(csharpDocument, span, out var mappedSpan))
        {
            return null;
        }

        var generatedDocument = await context.Snapshot.GetGeneratedDocumentAsync(cancellationToken).ConfigureAwait(false);

        var result = await ExternalAccess.Razor.Cohost.Handlers.ValidateBreakableRange.GetBreakableRangeAsync(generatedDocument, mappedSpan, cancellationToken).ConfigureAwait(false);
        if (result is { } csharpSpan &&
            _documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), csharpSpan, MappingBehavior.Inclusive, out var hostSpan))
        {
            return hostSpan;
        }

        return null;
    }

    public ValueTask<LinePositionSpan?> ResolveBreakpointRangeAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId documentId, LinePosition position, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => ResolveBreakpointRangeAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<LinePositionSpan?> ResolveBreakpointRangeAsync(RemoteDocumentContext context, LinePosition position, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (!TryGetUsableProjectedIndex(codeDocument, position, out var projectedIndex))
        {
            return null;
        }

        // Now ask Roslyn to adjust the breakpoint to a valid location in the code
        var syntaxTree = await context.Snapshot.GetCSharpSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        if (!RazorBreakpointSpans.TryGetBreakpointSpan(syntaxTree, projectedIndex, cancellationToken, out var csharpBreakpointSpan))
        {
            return null;
        }

        var csharpText = codeDocument.GetCSharpSourceText();
        var projectedRange = csharpText.GetLinePositionSpan(csharpBreakpointSpan);

        // Inclusive mapping means we are lenient to portions of the breakpoint that might be outside of use code in the Razor file
        if (!_documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), projectedRange, MappingBehavior.Inclusive, out var hostDocumentRange))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        return hostDocumentRange;
    }

    public ValueTask<string[]?> ResolveProximityExpressionsAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId documentId, LinePosition position, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => ResolveProximityExpressionsAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<string[]?> ResolveProximityExpressionsAsync(RemoteDocumentContext context, LinePosition position, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (!TryGetUsableProjectedIndex(codeDocument, position, out var projectedIndex))
        {
            return null;
        }

        // Now ask Roslyn to adjust the breakpoint to a valid location in the code
        var syntaxTree = await context.Snapshot.GetCSharpSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var result = RazorCSharpProximityExpressionResolverService.GetProximityExpressions(syntaxTree, projectedIndex, cancellationToken);

        return result?.ToArray();
    }

    private bool TryGetUsableProjectedIndex(RazorCodeDocument codeDocument, LinePosition hostDocumentPosition, out int projectedIndex)
    {
        var hostDocumentIndex = codeDocument.Source.Text.GetPosition(hostDocumentPosition);

        projectedIndex = 0;
        var languageKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: false);
        // For C#, we just map
        if (languageKind == RazorLanguageKind.CSharp &&
            !_documentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), hostDocumentIndex, out _, out projectedIndex))
        {
            return false;
        }
        // Otherwise see if there is more C# on the line to map to. This is for situations like "$$<p>@DateTime.Now</p>"
        else if (languageKind == RazorLanguageKind.Html &&
            !_documentMappingService.TryMapToGeneratedDocumentOrNextCSharpPosition(codeDocument.GetCSharpDocument(), hostDocumentIndex, out _, out projectedIndex))
        {
            return false;
        }
        else if (languageKind == RazorLanguageKind.Razor)
        {
            return false;
        }

        return true;
    }
}
