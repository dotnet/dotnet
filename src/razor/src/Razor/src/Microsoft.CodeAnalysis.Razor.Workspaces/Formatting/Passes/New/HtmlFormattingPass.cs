// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting.New;

internal sealed class HtmlFormattingPass(ILoggerFactory loggerFactory) : IFormattingPass
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<HtmlFormattingPass>();

    public Task<ImmutableArray<TextChange>> ExecuteAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        var changedText = context.SourceText;

        _logger.LogTestOnly($"Before HTML formatter:\r\n{changedText}");

        if (changes.Length > 0)
        {
            var filteredChanges = FilterIncomingChanges(context.CodeDocument.GetSyntaxTree(), changes);
            changedText = changedText.WithChanges(filteredChanges);

            _logger.LogTestOnly($"After FilterIncomingChanges:\r\n{changedText}");
        }

        return Task.FromResult(changedText.GetTextChangesArray(context.SourceText));
    }

    private static ImmutableArray<TextChange> FilterIncomingChanges(RazorSyntaxTree syntaxTree, ImmutableArray<TextChange> changes)
    {
        var sourceText = syntaxTree.Source.Text;

        using var changesToKeep = new PooledArrayBuilder<TextChange>(capacity: changes.Length);

        foreach (var change in changes)
        {
            // Don't keep changes that start inside of a razor comment block.
            var comment = syntaxTree.Root.FindInnermostNode(change.Span.Start)?.FirstAncestorOrSelf<RazorCommentBlockSyntax>();
            if (comment is not null && change.Span.Start > comment.SpanStart)
            {
                continue;
            }

            // Normally we don't touch Html changes much but there is one
            // edge case when including render fragments in a C# code block, eg:
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
            // the tag, but obviously that breaks things.
            //
            // It's straight forward enough to just check for this situation and ignore the change.

            // There needs to be a newline being inserted between an '@' and a '<'.
            if (change.NewText is ['\r' or '\n', ..] &&
                sourceText.Length > 1 &&
                sourceText[change.Span.Start - 1] == '@' &&
                sourceText[change.Span.Start] == '<')
            {
                continue;
            }

            changesToKeep.Add(change);
        }

        return changesToKeep.DrainToImmutable();
    }
}
