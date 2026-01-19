// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.TextDifferencing;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal sealed class HtmlFormattingPass(IDocumentMappingService documentMappingService) : IFormattingPass
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;

    public async Task<ImmutableArray<TextChange>> ExecuteAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        var changedText = context.SourceText;

        if (changes.Length > 0)
        {
            context.Logger?.LogSourceText("HtmlSourceText", context.CodeDocument.GetHtmlSourceText(cancellationToken));

            // There is a lot of uncertainty when we're dealing with edits that come from the Html formatter
            // because we are not responsible for it. It could make all sorts of strange edits, and it could
            // structure those edits is all sorts of ways. eg, it could have individual character edits, or
            // it could have a single edit that replaces a whole section of text, or the whole document.
            // Since the Html formatter doesn't understand Razor, and in fact doesn't even format the actual
            // Razor document directly (all C# is replaced), we have to be selective about what edits we will
            // actually use, but being selective is tricky because we might be missing some intentional edits
            // that the formatter made.
            //
            // To solve this, and work around various issues due to the Html formatter seeing a much simpler
            // document that we are actually dealing with, the first thing we do is take the changes it suggests
            // and apply them to the document it saw, then use our own algorithm to produce a set of changes
            // that more closely match what we want to get out of it. Specifically, we only want to see changes
            // to whitespace, or Html, not changes that include C#. Fortunately since we encode all C# as tildes
            // it means we can do a word-based diff, and all C# will essentially be equal to all other C#, so
            // won't appear in the diff.
            //
            // So we end up with a set of changes that are only ever to whitespace, or legitimate Html (though
            // in reality the formatter doesn't change that anyway).

            // Avoid computing a minimal diff if we don't need to. Slightly wasteful if we've come from one
            // of the other overloads, but worth it if we haven't (and worth it for them to validate before
            // doing the work to convert edits to changes).
            if (changes.Any(static e => e.NewText?.Contains('~') ?? false))
            {
                var htmlSourceText = context.CodeDocument.GetHtmlSourceText(cancellationToken);
                var htmlWithChanges = htmlSourceText.WithChanges(changes);

                changes = SourceTextDiffer.GetMinimalTextChanges(htmlSourceText, htmlWithChanges, DiffKind.Word);
                if (changes.Length == 0)
                {
                    return [];
                }
            }

            // Now that the changes are on our terms, we can apply our own filtering without having to worry
            // that we're missing something important. We could still, in theory, be missing something the Html
            // formatter intentionally did, but we also know the Html formatter made its decisions without an
            // awareness of Razor anyway, so it's not a reliable source.
            var filteredChanges = await FilterIncomingChangesAsync(context, changes, cancellationToken).ConfigureAwait(false);
            if (filteredChanges.Length == 0)
            {
                return [];
            }

            changedText = changedText.WithChanges(filteredChanges);

            context.Logger?.LogSourceText("AfterHtmlFormatter", changedText);
        }

        return SourceTextDiffer.GetMinimalTextChanges(context.SourceText, changedText, DiffKind.Char);
    }

    private async Task<ImmutableArray<TextChange>> FilterIncomingChangesAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        var codeDocument = context.CodeDocument;
        var csharpDocument = codeDocument.GetRequiredCSharpDocument();
        var syntaxRoot = codeDocument.GetRequiredSyntaxRoot();
        var sourceText = codeDocument.Source.Text;
        SyntaxNode? csharpSyntaxRoot = null;
        ImmutableArray<TextSpan> scriptAndStyleElementContentSpans = default;

        using var changesToKeep = new PooledArrayBuilder<TextChange>(capacity: changes.Length);

        foreach (var change in changes)
        {
            // We don't keep indentation changes that aren't in a script or style block
            // As a quick check, we only care about dropping edits that affect indentation - ie, are before the first
            // whitespace char on a line
            var line = sourceText.Lines.GetLineFromPosition(change.Span.Start);
            if (change.Span.Start <= line.GetFirstNonWhitespacePosition() &&
                !ChangeIsInsideScriptOrStyleElement(change))
            {
                context.Logger?.LogMessage($"Dropping change {change} because it's a change to line indentation, but not in a script or style block");
                continue;
            }

            var node = syntaxRoot.FindInnermostNode(change.Span.Start);
            // We don't keep changes that start inside of a razor comment block.
            var comment = node?.FirstAncestorOrSelf<RazorCommentBlockSyntax>();
            if (comment is not null && change.Span.Start > comment.SpanStart)
            {
                context.Logger?.LogMessage($"Dropping change {change} because it's in a Razor comment");
                continue;
            }

            // When render fragments are inside a C# code block, eg:
            //
            // @code {
            //      void Foo()
            //      {
            //          Render(@<SurveyPrompt />);
            //      }
            // }
            //
            // This is popular in some libraries, like bUnit. The issue here is that
            // the Html formatter sees ~~~~~<SurveyPrompt /> and puts a newline before
            // the tag, but obviously that breaks things by separating the transition and the tag.
            //
            // It's straight forward enough to just check for this situation and ignore the change.

            // There needs to be a newline being inserted between an '@' and a '<'.
            if (change.NewText is ['\r' or '\n', ..])
            {
                if (change.Span.Start > 0 &&
                    sourceText.Length > 1 &&
                    sourceText[change.Span.Start - 1] == '@' &&
                    sourceText[change.Span.Start] == '<')
                {
                    context.Logger?.LogMessage($"Dropping change {change} because it breaks a C# template");
                    continue;
                }

                // The Html formatter in VS Code wraps long lines, based on a user setting, but when there
                // are long C# string literals that ends up breaking the code. For example:
                //
                // @("this is a long string that spans past some user set maximum limit")
                //
                // could become
                //
                // @("this is a long string that spans past
                // some user set maximum limit")
                //
                // That doesn't compile, and depending on the scenario, can even cause a crash inside the
                // Roslyn formatter.
                //
                // Strictly speaking if literal is a verbatim string, or multiline raw string literal, then
                // it would compile, but it would also change the value of the string, and since these edits
                // come from the Html formatter which clearly has no idea it's doing that, it is safer to
                // disregard them all equally, and let the user make the final decision.
                //
                // In order to avoid hard coding all of the various string syntax kinds here, we can just check
                // for any literal, as the only literals that can contain spaces, which is what the Html formatter
                // will wrap on, are strings. And if it did decide to insert a newline into a number, or the 'null'
                // keyword, that would be pretty bad too.
                if (await ChangeIsInStringLiteralAsync(context, csharpDocument, change, cancellationToken).ConfigureAwait(false))
                {
                    continue;
                }
            }

            // As well as breaking long string literals, above, in VS Code the formatter will also potentially remove spaces
            // within a string literal, and in both VS and VS Code, they will happily remove indentation inside a multi-line
            // verbatim string, or raw string literal. Simply dropping any edit that is removing content from a string literal
            // fixes this. Strictly speaking we only need to care about removing whitespace, not removing anything else, but
            // we never want the formatter to remove anything else anyway.
            if (change.NewText?.Length == 0 &&
                await ChangeIsInStringLiteralAsync(context, csharpDocument, change, cancellationToken).ConfigureAwait(false))
            {
                continue;
            }

            changesToKeep.Add(change);
        }

        return changesToKeep.ToImmutableAndClear();

        async Task<bool> ChangeIsInStringLiteralAsync(FormattingContext context, RazorCSharpDocument csharpDocument, TextChange change, CancellationToken cancellationToken)
        {
            if (csharpSyntaxRoot is null)
            {
                var csharpSyntaxTree = await context.OriginalSnapshot.GetCSharpSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
                csharpSyntaxRoot = await csharpSyntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
            }

            if (_documentMappingService.TryMapToCSharpDocumentPosition(csharpDocument, change.Span.Start, out _, out var csharpIndex) &&
                csharpSyntaxRoot.FindNode(new TextSpan(csharpIndex, 0), getInnermostNodeForTie: true) is { } csharpNode &&
                csharpNode.IsStringLiteral())
            {
                context.Logger?.LogMessage($"Dropping change {change} because it breaks a C# string literal");
                return true;
            }

            return false;
        }

        bool ChangeIsInsideScriptOrStyleElement(TextChange change)
        {
            // Rather than ascend up the tree for every change, and look for script/style elements, we build up an index
            // first and just reuse it for every subsequent edit.
            InitializeElementContentSpanArray();

            // If there aren't any elements we're interested in, we don't need to do anything
            if (scriptAndStyleElementContentSpans.Length == 0)
            {
                return false;
            }

            var index = scriptAndStyleElementContentSpans.BinarySearchBy(change.Span.Start, static (span, pos) =>
            {
                if (span.Contains(pos))
                {
                    return 0;
                }

                return span.Start.CompareTo(pos);
            });

            // If we got a hit, then we're inside a tag we care about
            return index >= 0;
        }

        void InitializeElementContentSpanArray()
        {
            if (!scriptAndStyleElementContentSpans.IsDefault)
            {
                return;
            }

            using var boundsBuilder = new PooledArrayBuilder<TextSpan>();

            // We only care about "top level" block type structures (ie, a script tag isn't going to appear in the middle of a C# expression) so
            // we only need to descend into the document itself, or nodes that might contain directives, or other elements in case of nesting.
            foreach (var element in syntaxRoot.DescendantNodes(static node => node is MarkupElementSyntax || node.MayContainDirectives()).OfType<MarkupElementSyntax>())
            {
                if (RazorSyntaxFacts.IsScriptOrStyleBlock(element) &&
                    element.GetLinePositionSpan(codeDocument.Source).SpansMultipleLines())
                {

                    // We only want the contents of the script tag to be included, but not whitespace before the end tag if
                    // there is only whitespace before the tag, so the calculation of the end is a little annoying.
                    var endTagline = sourceText.Lines.GetLineFromPosition(element.EndTag.SpanStart);
                    var firstNonWhitespace = endTagline.GetFirstNonWhitespacePosition();
                    var end = firstNonWhitespace == element.EndTag.SpanStart
                        ? endTagline.Start
                        : firstNonWhitespace.GetValueOrDefault() + 1; // Add one to the end, because we want end to be inclusive, and the parameter is exclusive
                    boundsBuilder.Add(TextSpan.FromBounds(element.StartTag.EndPosition, end));
                }
            }

            scriptAndStyleElementContentSpans = boundsBuilder.ToImmutableAndClear();
        }
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(HtmlFormattingPass pass)
    {
        public Task<ImmutableArray<TextChange>> FilterIncomingChangesAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
            => pass.FilterIncomingChangesAsync(context, changes, cancellationToken);
    }
}
