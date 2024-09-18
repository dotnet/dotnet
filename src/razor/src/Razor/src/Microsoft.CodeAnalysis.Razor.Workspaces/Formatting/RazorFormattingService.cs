// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal class RazorFormattingService : IRazorFormattingService
{
    public static readonly string FirstTriggerCharacter = "}";
    public static readonly ImmutableArray<string> MoreTriggerCharacters = [";", "\n", "{"];
    public static readonly FrozenSet<string> AllTriggerCharacterSet = FrozenSet.ToFrozenSet([FirstTriggerCharacter, .. MoreTriggerCharacters], StringComparer.Ordinal);

    private static readonly FrozenSet<string> s_csharpTriggerCharacterSet = FrozenSet.ToFrozenSet(["}", ";"], StringComparer.Ordinal);
    private static readonly FrozenSet<string> s_htmlTriggerCharacterSet = FrozenSet.ToFrozenSet(["\n", "{", "}", ";"], StringComparer.Ordinal);

    private readonly IFormattingCodeDocumentProvider _codeDocumentProvider;

    private readonly ImmutableArray<IFormattingPass> _documentFormattingPasses;
    private readonly ImmutableArray<IFormattingPass> _validationPasses;
    private readonly CSharpOnTypeFormattingPass _csharpOnTypeFormattingPass;
    private readonly HtmlOnTypeFormattingPass _htmlOnTypeFormattingPass;

    public RazorFormattingService(
        IFormattingCodeDocumentProvider codeDocumentProvider,
        IDocumentMappingService documentMappingService,
        IHostServicesProvider hostServicesProvider,
        ILoggerFactory loggerFactory)
    {
        _codeDocumentProvider = codeDocumentProvider;

        _htmlOnTypeFormattingPass = new HtmlOnTypeFormattingPass(loggerFactory);
        _csharpOnTypeFormattingPass = new CSharpOnTypeFormattingPass(documentMappingService, hostServicesProvider, loggerFactory);
        _validationPasses =
        [
            new FormattingDiagnosticValidationPass(loggerFactory),
            new FormattingContentValidationPass(loggerFactory)
        ];
        _documentFormattingPasses =
        [
            new HtmlFormattingPass(loggerFactory),
            new RazorFormattingPass(),
            new CSharpFormattingPass(documentMappingService, hostServicesProvider, loggerFactory),
            .. _validationPasses
        ];
    }

    public async Task<ImmutableArray<TextChange>> GetDocumentFormattingChangesAsync(
        DocumentContext documentContext,
        ImmutableArray<TextChange> htmlChanges,
        LinePositionSpan? range,
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
        var sourceText = codeDocument.Source.Text;
        if (range is { } span)
        {
            if (codeDocument.GetCSharpDocument().Diagnostics.Any(d => d.Span != SourceSpan.Undefined && span.OverlapsWith(sourceText.GetLinePositionSpan(d.Span))))
            {
                return [];
            }
        }

        var uri = documentContext.Uri;
        var documentSnapshot = documentContext.Snapshot;
        var hostDocumentVersion = documentContext.Snapshot.Version;
        var context = FormattingContext.Create(
            documentSnapshot,
            codeDocument,
            options,
            _codeDocumentProvider);
        var originalText = context.SourceText;

        var result = htmlChanges;
        foreach (var pass in _documentFormattingPasses)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result = await pass.ExecuteAsync(context, result, cancellationToken).ConfigureAwait(false);
        }

        var filteredChanges = range is not { } linePositionSpan
            ? result
            : result.Where(e => linePositionSpan.LineOverlapsWith(sourceText.GetLinePositionSpan(e.Span))).ToImmutableArray();

        var normalizedChanges = NormalizeLineEndings(originalText, filteredChanges);
        return originalText.MinimizeTextChanges(normalizedChanges);
    }

    public Task<ImmutableArray<TextChange>> GetCSharpOnTypeFormattingChangesAsync(DocumentContext documentContext, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        => ApplyFormattedChangesAsync(
            documentContext,
            generatedDocumentChanges: [],
            options,
            hostDocumentIndex,
            triggerCharacter,
            [_csharpOnTypeFormattingPass, .. _validationPasses],
            collapseChanges: false,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken);

    public Task<ImmutableArray<TextChange>> GetHtmlOnTypeFormattingChangesAsync(DocumentContext documentContext, ImmutableArray<TextChange> htmlChanges, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        => ApplyFormattedChangesAsync(
            documentContext,
            htmlChanges,
            options,
            hostDocumentIndex,
            triggerCharacter,
            [_htmlOnTypeFormattingPass, .. _validationPasses],
            collapseChanges: false,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken);

    public async Task<TextChange?> TryGetSingleCSharpEditAsync(DocumentContext documentContext, TextChange csharpEdit, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var razorChanges = await ApplyFormattedChangesAsync(
            documentContext,
            [csharpEdit],
            options,
            hostDocumentIndex: 0,
            triggerCharacter: '\0',
            [_csharpOnTypeFormattingPass, .. _validationPasses],
            collapseChanges: false,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return razorChanges.SingleOrDefault();
    }

    public async Task<TextChange?> TryGetCSharpCodeActionEditAsync(DocumentContext documentContext, ImmutableArray<TextChange> csharpChanges, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var razorChanges = await ApplyFormattedChangesAsync(
            documentContext,
            csharpChanges,
            options,
            hostDocumentIndex: 0,
            triggerCharacter: '\0',
            [_csharpOnTypeFormattingPass],
            collapseChanges: true,
            automaticallyAddUsings: true,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return razorChanges.SingleOrDefault();
    }

    public async Task<TextChange?> TryGetCSharpSnippetFormattingEditAsync(DocumentContext documentContext, ImmutableArray<TextChange> csharpChanges, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        csharpChanges = WrapCSharpSnippets(csharpChanges);

        var razorChanges = await ApplyFormattedChangesAsync(
            documentContext,
            csharpChanges,
            options,
            hostDocumentIndex: 0,
            triggerCharacter: '\0',
            [_csharpOnTypeFormattingPass],
            collapseChanges: true,
            automaticallyAddUsings: false,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        razorChanges = UnwrapCSharpSnippets(razorChanges);

        return razorChanges.SingleOrDefault();
    }

    public bool TryGetOnTypeFormattingTriggerKind(RazorCodeDocument codeDocument, int hostDocumentIndex, string triggerCharacter, out RazorLanguageKind triggerCharacterKind)
    {
        triggerCharacterKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: false);

        return triggerCharacterKind switch
        {
            RazorLanguageKind.CSharp => s_csharpTriggerCharacterSet.Contains(triggerCharacter),
            RazorLanguageKind.Html => s_htmlTriggerCharacterSet.Contains(triggerCharacter),
            _ => false,
        };
    }

    private async Task<ImmutableArray<TextChange>> ApplyFormattedChangesAsync(
        DocumentContext documentContext,
        ImmutableArray<TextChange> generatedDocumentChanges,
        RazorFormattingOptions options,
        int hostDocumentIndex,
        char triggerCharacter,
        ImmutableArray<IFormattingPass> formattingPasses,
        bool collapseChanges,
        bool automaticallyAddUsings,
        CancellationToken cancellationToken)
    {
        // If we only received a single edit, let's always return a single edit back.
        // Otherwise, merge only if explicitly asked.
        collapseChanges |= generatedDocumentChanges.Length == 1;

        var documentSnapshot = documentContext.Snapshot;
        var codeDocument = await _codeDocumentProvider.GetCodeDocumentAsync(documentSnapshot).ConfigureAwait(false);
        var context = FormattingContext.CreateForOnTypeFormatting(
            documentSnapshot,
            codeDocument,
            options,
            _codeDocumentProvider,
            automaticallyAddUsings,
            hostDocumentIndex,
            triggerCharacter);
        var result = generatedDocumentChanges;

        foreach (var pass in formattingPasses)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result = await pass.ExecuteAsync(context, result, cancellationToken).ConfigureAwait(false);
        }

        var originalText = context.SourceText;
        var razorChanges = originalText.MinimizeTextChanges(result);

        if (collapseChanges)
        {
            var collapsedEdit = MergeChanges(razorChanges, originalText);
            if (collapsedEdit.NewText is null or { Length: 0 } &&
                collapsedEdit.Span.IsEmpty)
            {
                return [];
            }

            return [collapsedEdit];
        }

        return razorChanges;
    }

    // Internal for testing
    internal static TextChange MergeChanges(ImmutableArray<TextChange> changes, SourceText sourceText)
    {
        if (changes.Length == 1)
        {
            return changes[0];
        }

        var changedText = sourceText.WithChanges(changes);
        var affectedRange = changedText.GetEncompassingTextChangeRange(sourceText);
        var spanBeforeChange = affectedRange.Span;
        var spanAfterChange = new TextSpan(spanBeforeChange.Start, affectedRange.NewLength);
        var newText = changedText.GetSubTextString(spanAfterChange);

        return new TextChange(spanBeforeChange, newText);
    }

    private static ImmutableArray<TextChange> WrapCSharpSnippets(ImmutableArray<TextChange> csharpChanges)
    {
        // Currently this method only supports wrapping `$0`, any additional markers aren't formatted properly.

        return ReplaceInChanges(csharpChanges, "$0", "/*$0*/");
    }

    private static ImmutableArray<TextChange> UnwrapCSharpSnippets(ImmutableArray<TextChange> razorChanges)
    {
        return ReplaceInChanges(razorChanges, "/*$0*/", "$0");
    }

    /// <summary>
    /// This method counts the occurrences of CRLF and LF line endings in the original text. 
    /// If LF line endings are more prevalent, it removes any CR characters from the text changes 
    /// to ensure consistency with the LF style.
    /// </summary>
    private static ImmutableArray<TextChange> NormalizeLineEndings(SourceText originalText, ImmutableArray<TextChange> changes)
    {
        if (originalText.HasLFLineEndings())
        {
            return ReplaceInChanges(changes, "\r", "");
        }

        return changes;
    }

    private static ImmutableArray<TextChange> ReplaceInChanges(ImmutableArray<TextChange> csharpChanges, string toFind, string replacement)
    {
        using var changes = new PooledArrayBuilder<TextChange>(csharpChanges.Length);
        foreach (var change in csharpChanges)
        {
            if (change.NewText is not { } newText ||
                newText.IndexOf(toFind) == -1)
            {
                changes.Add(change);
                continue;
            }

            // Formatting doesn't work with syntax errors caused by the cursor marker ($0).
            // So, let's avoid the error by wrapping the cursor marker in a comment.
            changes.Add(new(change.Span, newText.Replace(toFind, replacement)));
        }

        return changes.DrainToImmutable();
    }

    internal static class TestAccessor
    {
        public static FrozenSet<string> GetCSharpTriggerCharacterSet() => s_csharpTriggerCharacterSet;
        public static FrozenSet<string> GetHtmlTriggerCharacterSet() => s_htmlTriggerCharacterSet;
    }
}
