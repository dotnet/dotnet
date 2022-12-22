// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class DefaultRazorFormattingService : RazorFormattingService
{
    private readonly List<IFormattingPass> _formattingPasses;
    private readonly ILogger _logger;
    private readonly AdhocWorkspaceFactory _workspaceFactory;

    public DefaultRazorFormattingService(
        IEnumerable<IFormattingPass> formattingPasses,
        ILoggerFactory loggerFactory,
        AdhocWorkspaceFactory workspaceFactory)
    {
        if (formattingPasses is null)
        {
            throw new ArgumentNullException(nameof(formattingPasses));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        if (workspaceFactory is null)
        {
            throw new ArgumentNullException(nameof(workspaceFactory));
        }

        _formattingPasses = formattingPasses.OrderBy(f => f.Order).ToList();
        _logger = loggerFactory.CreateLogger<DefaultRazorFormattingService>();
        _workspaceFactory = workspaceFactory;
    }

    public override async Task<TextEdit[]> FormatAsync(
        DocumentContext documentContext,
        Range? range,
        FormattingOptions options,
        CancellationToken cancellationToken)
    {
        if (documentContext is null)
        {
            throw new ArgumentNullException(nameof(documentContext));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var codeDocument = await documentContext.Snapshot.GetGeneratedOutputAsync();

        // Range formatting happens on every paste, and if there are Razor diagnostics in the file
        // that can make some very bad results. eg, given:
        //
        // |
        // @code {
        // }
        //
        // When pasting "<button" at the | the HTML formatter will bring the "@code" onto the same
        // line as "<button" because as far as it's concerned, its an attribute.
        //
        // To defeat that, we simply don't do range formatting if there are diagnostics.

        // Despite what it looks like, codeDocument.GetCSharpDocument().Diagnostics is actually the
        // Razor diagnostics, not the C# diagnostics 🤦‍
        if (range is not null &&
            codeDocument.GetCSharpDocument().Diagnostics.Any(d => range.OverlapsWith(d.Span.AsRange(codeDocument.GetSourceText()))))
        {
            return Array.Empty<TextEdit>();
        }

        var uri = documentContext.Uri;
        var documentSnapshot = documentContext.Snapshot;
        var hostDocumentVersion = documentContext.Version;
        using var context = FormattingContext.Create(uri, documentSnapshot, codeDocument, options, _workspaceFactory);
        var originalText = context.SourceText;

        var result = new FormattingResult(Array.Empty<TextEdit>());
        foreach (var pass in _formattingPasses)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result = await pass.ExecuteAsync(context, result, cancellationToken);
        }

        var filteredEdits = range is null
            ? result.Edits
            : result.Edits.Where(e => range.LineOverlapsWith(e.Range));

        return GetMinimalEdits(originalText, filteredEdits);
    }

    private static TextEdit[] GetMinimalEdits(SourceText originalText, IEnumerable<TextEdit> filteredEdits)
    {
        // Make sure the edits actually change something, or its not worth responding
        var textChanges = filteredEdits.Select(e => e.AsTextChange(originalText));
        var changedText = originalText.WithChanges(textChanges);
        if (changedText.ContentEquals(originalText))
        {
            return Array.Empty<TextEdit>();
        }

        // Only send back the minimum edits
        var minimalChanges = SourceTextDiffer.GetMinimalTextChanges(originalText, changedText, lineDiffOnly: false);
        var finalEdits = minimalChanges.Select(f => f.AsTextEdit(originalText)).ToArray();

        return finalEdits;
    }

    public override Task<TextEdit[]> FormatOnTypeAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        => ApplyFormattedEditsAsync(documentContext, kind, formattedEdits, options, hostDocumentIndex, triggerCharacter, bypassValidationPasses: false, collapseEdits: false, automaticallyAddUsings: false, cancellationToken: cancellationToken);

    public override Task<TextEdit[]> FormatCodeActionAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, CancellationToken cancellationToken)
        => ApplyFormattedEditsAsync(documentContext, kind, formattedEdits, options, hostDocumentIndex: 0, triggerCharacter: '\0', bypassValidationPasses: true, collapseEdits: false, automaticallyAddUsings: true, cancellationToken: cancellationToken);

    public override async Task<TextEdit[]> FormatSnippetAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] edits, FormattingOptions options, CancellationToken cancellationToken)
    {
        if (kind == RazorLanguageKind.CSharp)
        {
            WrapCSharpSnippets(edits);
        }

        var formattedEdits = await ApplyFormattedEditsAsync(
            documentContext,
            kind,
            edits,
            options,
            hostDocumentIndex: 0,
            triggerCharacter: '\0',
            bypassValidationPasses: true,
            collapseEdits: true,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (kind == RazorLanguageKind.CSharp)
        {
            UnwrapCSharpSnippets(formattedEdits);
        }

        return formattedEdits;
    }

    private async Task<TextEdit[]> ApplyFormattedEditsAsync(
        DocumentContext documentContext,
        RazorLanguageKind kind,
        TextEdit[] formattedEdits,
        FormattingOptions options,
        int hostDocumentIndex,
        char triggerCharacter,
        bool bypassValidationPasses,
        bool collapseEdits,
        bool automaticallyAddUsings,
        CancellationToken cancellationToken)
    {
        // If we only received a single edit, let's always return a single edit back.
        // Otherwise, merge only if explicitly asked.
        collapseEdits |= formattedEdits.Length == 1;

        var documentSnapshot = documentContext.Snapshot;
        var uri = documentContext.Identifier.Uri;
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync();
        using var context = FormattingContext.CreateForOnTypeFormatting(uri, documentSnapshot, codeDocument, options, _workspaceFactory, automaticallyAddUsings: automaticallyAddUsings, hostDocumentIndex, triggerCharacter);
        var result = new FormattingResult(formattedEdits, kind);

        foreach (var pass in _formattingPasses)
        {
            if (pass.IsValidationPass && bypassValidationPasses)
            {
                continue;
            }

            cancellationToken.ThrowIfCancellationRequested();
            result = await pass.ExecuteAsync(context, result, cancellationToken);
        }

        var originalText = context.SourceText;
        var edits = GetMinimalEdits(originalText, result.Edits);

        if (collapseEdits)
        {
            var collapsedEdit = MergeEdits(edits, originalText);
            if (collapsedEdit.NewText.Length == 0)
            {
                return Array.Empty<TextEdit>();
            }

            return new[] { collapsedEdit };
        }

        return edits;
    }

    // Internal for testing
    internal static TextEdit MergeEdits(TextEdit[] edits, SourceText sourceText)
    {
        if (edits.Length == 1)
        {
            return edits[0];
        }

        var textChanges = new List<TextChange>();
        foreach (var edit in edits)
        {
            var change = new TextChange(edit.Range.AsTextSpan(sourceText), edit.NewText);
            textChanges.Add(change);
        }

        var changedText = sourceText.WithChanges(textChanges);
        var affectedRange = changedText.GetEncompassingTextChangeRange(sourceText);
        var spanBeforeChange = affectedRange.Span;
        var spanAfterChange = new TextSpan(spanBeforeChange.Start, affectedRange.NewLength);
        var newText = changedText.GetSubTextString(spanAfterChange);

        var encompassingChange = new TextChange(spanBeforeChange, newText);

        return encompassingChange.AsTextEdit(sourceText);
    }

    private static void WrapCSharpSnippets(TextEdit[] snippetEdits)
    {
        // Currently this method only supports wrapping `$0`, any additional markers aren't formatted properly.

        for (var i = 0; i < snippetEdits.Length; i++)
        {
            var snippetEdit = snippetEdits[i];

            // Formatting doesn't work with syntax errors caused by the cursor marker ($0).
            // So, let's avoid the error by wrapping the cursor marker in a comment.
            var wrappedText = snippetEdit.NewText.Replace("$0", "/*$0*/");
            snippetEdit.NewText = wrappedText;
        }
    }

    private static void UnwrapCSharpSnippets(TextEdit[] snippetEdits)
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
