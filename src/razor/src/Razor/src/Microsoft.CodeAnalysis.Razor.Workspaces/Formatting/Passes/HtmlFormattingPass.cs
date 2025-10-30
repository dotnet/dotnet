// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.TextDifferencing;
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
            var filteredChanges = await FilterIncomingChangesAsync(context, changes, cancellationToken).ConfigureAwait(false);
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

        using var changesToKeep = new PooledArrayBuilder<TextChange>(capacity: changes.Length);

        foreach (var change in changes)
        {
            // We don't keep changes that start inside of a razor comment block.
            var node = syntaxRoot.FindInnermostNode(change.Span.Start);
            var comment = node?.FirstAncestorOrSelf<RazorCommentBlockSyntax>();
            if (comment is not null && change.Span.Start > comment.SpanStart)
            {
                continue;
            }

            // When render fragments are inside a C# code block, eg:
            //
            // @code {
            //      void Foo()
            //      {
            //          Render(@<SurveyPrompt />);
            //      {
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
                if (csharpSyntaxRoot is null)
                {
                    var csharpSyntaxTree = await context.OriginalSnapshot.GetCSharpSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
                    csharpSyntaxRoot = await csharpSyntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
                }

                if (_documentMappingService.TryMapToCSharpDocumentPosition(csharpDocument, change.Span.Start, out _, out var csharpIndex) &&
                    csharpSyntaxRoot.FindNode(new TextSpan(csharpIndex, 0), getInnermostNodeForTie: true) is { } csharpNode &&
                    csharpNode is CSharp.Syntax.LiteralExpressionSyntax or CSharp.Syntax.InterpolatedStringTextSyntax)
                {
                    continue;
                }
            }

            changesToKeep.Add(change);
        }

        return changesToKeep.ToImmutableAndClear();
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(HtmlFormattingPass pass)
    {
        public Task<ImmutableArray<TextChange>> FilterIncomingChangesAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
            => pass.FilterIncomingChangesAsync(context, changes, cancellationToken);
    }
}
