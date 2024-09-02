// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal class RazorFormattingService : IRazorFormattingService
{
    private readonly IFormattingCodeDocumentProvider _codeDocumentProvider;
    private readonly IAdhocWorkspaceFactory _workspaceFactory;

    private readonly ImmutableArray<IFormattingPass> _documentFormattingPasses;
    private readonly ImmutableArray<IFormattingPass> _validationPasses;
    private readonly CSharpOnTypeFormattingPass _csharpOnTypeFormattingPass;
    private readonly HtmlOnTypeFormattingPass _htmlOnTypeFormattingPass;

    public RazorFormattingService(
        IFormattingCodeDocumentProvider codeDocumentProvider,
        IDocumentMappingService documentMappingService,
        IAdhocWorkspaceFactory workspaceFactory,
        ILoggerFactory loggerFactory)
    {
        _codeDocumentProvider = codeDocumentProvider;
        _workspaceFactory = workspaceFactory;

        _htmlOnTypeFormattingPass = new HtmlOnTypeFormattingPass(loggerFactory);
        _csharpOnTypeFormattingPass = new CSharpOnTypeFormattingPass(documentMappingService, loggerFactory);
        _validationPasses =
        [
            new FormattingDiagnosticValidationPass(loggerFactory),
            new FormattingContentValidationPass(loggerFactory)
        ];
        _documentFormattingPasses =
        [
            new HtmlFormattingPass(loggerFactory),
            new RazorFormattingPass(),
            new CSharpFormattingPass(documentMappingService, loggerFactory),
            .. _validationPasses
        ];
    }

    public async Task<TextEdit[]> GetDocumentFormattingEditsAsync(
        DocumentContext documentContext,
        TextEdit[] htmlEdits,
        Range? range,
        RazorFormattingOptions options,
        CancellationToken cancellationToken)
    {
        var codeDocument = await _codeDocumentProvider.GetCodeDocumentAsync(documentContext.Snapshot).ConfigureAwait(false);

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
        if (range is not null)
        {
            var sourceText = codeDocument.Source.Text;
            if (codeDocument.GetCSharpDocument().Diagnostics.Any(d => d.Span != SourceSpan.Undefined && range.OverlapsWith(sourceText.GetRange(d.Span))))
            {
                return [];
            }
        }

        var uri = documentContext.Uri;
        var documentSnapshot = documentContext.Snapshot;
        var hostDocumentVersion = documentContext.Snapshot.Version;
        using var context = FormattingContext.Create(
            uri,
            documentSnapshot,
            codeDocument,
            options,
            _codeDocumentProvider,
            _workspaceFactory);
        var originalText = context.SourceText;

        var result = htmlEdits;
        foreach (var pass in _documentFormattingPasses)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result = await pass.ExecuteAsync(context, result, cancellationToken).ConfigureAwait(false);
        }

        var filteredEdits = range is null
            ? result
            : result.Where(e => range.LineOverlapsWith(e.Range)).ToArray();

        return originalText.MinimizeTextEdits(filteredEdits);
    }

    public Task<TextEdit[]> GetCSharpOnTypeFormattingEditsAsync(DocumentContext documentContext, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        => ApplyFormattedEditsAsync(
            documentContext,
            generatedDocumentEdits: [],
            options,
            hostDocumentIndex,
            triggerCharacter,
            [_csharpOnTypeFormattingPass, .. _validationPasses],
            collapseEdits: false,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken);

    public Task<TextEdit[]> GetHtmlOnTypeFormattingEditsAsync(DocumentContext documentContext, TextEdit[] htmlEdits, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        => ApplyFormattedEditsAsync(
            documentContext,
            htmlEdits,
            options,
            hostDocumentIndex,
            triggerCharacter,
            [_htmlOnTypeFormattingPass, .. _validationPasses],
            collapseEdits: false,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken);

    public async Task<TextEdit?> GetSingleCSharpEditAsync(DocumentContext documentContext, TextEdit csharpEdit, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var razorEdits = await ApplyFormattedEditsAsync(
            documentContext,
            [csharpEdit],
            options,
            hostDocumentIndex: 0,
            triggerCharacter: '\0',
            [_csharpOnTypeFormattingPass, .. _validationPasses],
            collapseEdits: false,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return razorEdits.SingleOrDefault();
    }

    public async Task<TextEdit?> GetCSharpCodeActionEditAsync(DocumentContext documentContext, TextEdit[] csharpEdits, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var razorEdits = await ApplyFormattedEditsAsync(
            documentContext,
            csharpEdits,
            options,
            hostDocumentIndex: 0,
            triggerCharacter: '\0',
            [_csharpOnTypeFormattingPass],
            collapseEdits: true,
            automaticallyAddUsings: true,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return razorEdits.SingleOrDefault();
    }

    public async Task<TextEdit?> GetCSharpSnippetFormattingEditAsync(DocumentContext documentContext, TextEdit[] csharpEdits, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        WrapCSharpSnippets(csharpEdits);

        var razorEdits = await ApplyFormattedEditsAsync(
            documentContext,
            csharpEdits,
            options,
            hostDocumentIndex: 0,
            triggerCharacter: '\0',
            [_csharpOnTypeFormattingPass],
            collapseEdits: true,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        UnwrapCSharpSnippets(razorEdits);

        return razorEdits.SingleOrDefault();
    }

    private async Task<TextEdit[]> ApplyFormattedEditsAsync(
        DocumentContext documentContext,
        TextEdit[] generatedDocumentEdits,
        RazorFormattingOptions options,
        int hostDocumentIndex,
        char triggerCharacter,
        ImmutableArray<IFormattingPass> formattingPasses,
        bool collapseEdits,
        bool automaticallyAddUsings,
        CancellationToken cancellationToken)
    {
        // If we only received a single edit, let's always return a single edit back.
        // Otherwise, merge only if explicitly asked.
        collapseEdits |= generatedDocumentEdits.Length == 1;

        var documentSnapshot = documentContext.Snapshot;
        var uri = documentContext.Uri;
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        using var context = FormattingContext.CreateForOnTypeFormatting(
            uri,
            documentSnapshot,
            codeDocument,
            options,
            _codeDocumentProvider,
            _workspaceFactory,
            automaticallyAddUsings: automaticallyAddUsings,
            hostDocumentIndex,
            triggerCharacter);
        var result = generatedDocumentEdits;

        foreach (var pass in formattingPasses)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result = await pass.ExecuteAsync(context, result, cancellationToken).ConfigureAwait(false);
        }

        var originalText = context.SourceText;
        var razorEdits = originalText.MinimizeTextEdits(result);

        if (collapseEdits)
        {
            var collapsedEdit = MergeEdits(razorEdits, originalText);
            if (collapsedEdit.NewText.Length == 0 &&
                collapsedEdit.Range.IsZeroWidth())
            {
                return [];
            }

            return [collapsedEdit];
        }

        return razorEdits;
    }

    // Internal for testing
    internal static TextEdit MergeEdits(TextEdit[] edits, SourceText sourceText)
    {
        if (edits.Length == 1)
        {
            return edits[0];
        }

        var changedText = sourceText.WithChanges(edits.Select(sourceText.GetTextChange));
        var affectedRange = changedText.GetEncompassingTextChangeRange(sourceText);
        var spanBeforeChange = affectedRange.Span;
        var spanAfterChange = new TextSpan(spanBeforeChange.Start, affectedRange.NewLength);
        var newText = changedText.GetSubTextString(spanAfterChange);

        var encompassingChange = new TextChange(spanBeforeChange, newText);

        return sourceText.GetTextEdit(encompassingChange);
    }

    private static void WrapCSharpSnippets(TextEdit[] csharpEdits)
    {
        // Currently this method only supports wrapping `$0`, any additional markers aren't formatted properly.

        foreach (var edit in csharpEdits)
        {
            // Formatting doesn't work with syntax errors caused by the cursor marker ($0).
            // So, let's avoid the error by wrapping the cursor marker in a comment.
            edit.NewText = edit.NewText.Replace("$0", "/*$0*/");
        }
    }

    private static void UnwrapCSharpSnippets(TextEdit[] razorEdits)
    {
        foreach (var edit in razorEdits)
        {
            // Formatting doesn't work with syntax errors caused by the cursor marker ($0).
            // So, let's avoid the error by wrapping the cursor marker in a comment.
            edit.NewText = edit.NewText.Replace("/*$0*/", "$0");
        }
    }
}
