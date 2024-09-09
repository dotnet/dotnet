// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Response = Microsoft.CodeAnalysis.Razor.Remote.IRemoteFormattingService.TriggerKind;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteFormattingService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteFormattingService
{
    internal sealed class Factory : FactoryBase<IRemoteFormattingService>
    {
        protected override IRemoteFormattingService CreateService(in ServiceArgs args)
            => new RemoteFormattingService(in args);
    }

    private readonly IRazorFormattingService _formattingService = args.ExportProvider.GetExportedValue<IRazorFormattingService>();

    public ValueTask<ImmutableArray<TextChange>> GetDocumentFormattingEditsAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        ImmutableArray<TextChange> htmlChanges,
        RazorFormattingOptions options,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetDocumentFormattingEditsAsync(context, htmlChanges, options, cancellationToken),
            cancellationToken);

    private async ValueTask<ImmutableArray<TextChange>> GetDocumentFormattingEditsAsync(
        RemoteDocumentContext context,
        ImmutableArray<TextChange> htmlChanges,
        RazorFormattingOptions options,
        CancellationToken cancellationToken)
    {
        var sourceText = await context.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var htmlEdits = htmlChanges.Select(sourceText.GetTextEdit).ToArray();

        var edits = await _formattingService.GetDocumentFormattingEditsAsync(context, htmlEdits, range: null, options, cancellationToken).ConfigureAwait(false);

        if (edits is null)
        {
            return [];
        }

        return edits.SelectAsArray(sourceText.GetTextChange);
    }

    public ValueTask<ImmutableArray<TextChange>> GetRangeFormattingEditsAsync(
         RazorPinnedSolutionInfoWrapper solutionInfo,
         DocumentId documentId,
         ImmutableArray<TextChange> htmlChanges,
         LinePositionSpan linePositionSpan,
         RazorFormattingOptions options,
         CancellationToken cancellationToken)
            => RunServiceAsync(
                solutionInfo,
                documentId,
                context => GetRangeFormattingEditsAsync(context, htmlChanges, linePositionSpan, options, cancellationToken),
                cancellationToken);

    private async ValueTask<ImmutableArray<TextChange>> GetRangeFormattingEditsAsync(
        RemoteDocumentContext context,
        ImmutableArray<TextChange> htmlChanges,
        LinePositionSpan linePositionSpan,
        RazorFormattingOptions options,
        CancellationToken cancellationToken)
    {
        var sourceText = await context.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var htmlEdits = htmlChanges.Select(sourceText.GetTextEdit).ToArray();

        var edits = await _formattingService.GetDocumentFormattingEditsAsync(context, htmlEdits, range: linePositionSpan.ToRange(), options, cancellationToken).ConfigureAwait(false);

        if (edits is null)
        {
            return [];
        }

        return edits.SelectAsArray(sourceText.GetTextChange);
    }

    public ValueTask<ImmutableArray<TextChange>> GetOnTypeFormattingEditsAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        ImmutableArray<TextChange> htmlChanges,
        LinePosition linePosition,
        string character,
        RazorFormattingOptions options,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetOnTypeFormattingEditsAsync(context, htmlChanges, linePosition, character, options, cancellationToken),
            cancellationToken);

    private async ValueTask<ImmutableArray<TextChange>> GetOnTypeFormattingEditsAsync(RemoteDocumentContext context, ImmutableArray<TextChange> htmlChanges, LinePosition linePosition, string triggerCharacter, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = await context.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(linePosition, out var hostDocumentIndex))
        {
            return [];
        }

        if (!_formattingService.TryGetOnTypeFormattingTriggerKind(codeDocument, hostDocumentIndex, triggerCharacter, out var triggerCharacterKind))
        {
            return [];
        }

        TextEdit[] result;
        if (triggerCharacterKind is RazorLanguageKind.Html)
        {
            var htmlEdits = htmlChanges.Select(sourceText.GetTextEdit).ToArray();
            result = await _formattingService.GetHtmlOnTypeFormattingEditsAsync(context, htmlEdits, options, hostDocumentIndex, triggerCharacter[0], cancellationToken).ConfigureAwait(false);
        }
        else if (triggerCharacterKind is RazorLanguageKind.CSharp)
        {
            result = await _formattingService.GetCSharpOnTypeFormattingEditsAsync(context, options, hostDocumentIndex, triggerCharacter[0], cancellationToken).ConfigureAwait(false);
        }
        else
        {
            return Assumed.Unreachable<ImmutableArray<TextChange>>();
        }

        return result.SelectAsArray(sourceText.GetTextChange);
    }

    public ValueTask<Response> GetOnTypeFormattingTriggerKindAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePosition linePosition,
        string triggerCharacter,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => IsValidOnTypeFormattingTriggerAsync(context, linePosition, triggerCharacter, cancellationToken),
            cancellationToken);

    private async ValueTask<Response> IsValidOnTypeFormattingTriggerAsync(RemoteDocumentContext context, LinePosition linePosition, string triggerCharacter, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = codeDocument.Source.Text;
        if (!sourceText.TryGetAbsoluteIndex(linePosition, out var hostDocumentIndex))
        {
            return Response.Invalid;
        }

        if (!_formattingService.TryGetOnTypeFormattingTriggerKind(codeDocument, hostDocumentIndex, triggerCharacter, out var triggerCharacterKind))
        {
            return Response.Invalid;
        }

        if (triggerCharacterKind is RazorLanguageKind.Html)
        {
            return Response.ValidHtml;
        }

        // TryGetOnTypeFormattingTriggerKind only returns true for C# or Html
        Debug.Assert(triggerCharacterKind is RazorLanguageKind.CSharp);

        return Response.ValidCSharp;
    }
}
