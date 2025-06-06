// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Completion;

namespace Microsoft.CodeAnalysis.Razor.Completion.Delegation;

/// <summary>
/// Helper methods for C# and HTML completion ("delegated" completion) that are used both in LSP and cohosting
/// completion handler code.
/// </summary>
internal static class DelegatedCompletionHelper
{
    // Ordering should be:
    // 1. Changes items
    // 2. Adds items
    // 3. Filters items
    private static readonly ImmutableArray<IDelegatedCSharpCompletionResponseRewriter> s_delegatedCSharpCompletionResponseRewriters =
        [new SnippetResponseRewriter(), new TextEditResponseRewriter(), new DesignTimeHelperResponseRewriter()];

    // Currently we only have one HTML response re-writer. Should we ever need more, we can create a common base and a collection
    private static readonly HtmlCommitCharacterResponseRewriter s_delegatedHtmlCompletionResponseRewriter = new();

    /// <summary>
    /// Modifies completion context if needed so that it's acceptable to the delegated language.
    /// </summary>
    /// <param name="context">Original completion context passed to the completion handler</param>
    /// <param name="languageKind">Language of the completion position</param>
    /// <param name="triggerAndCommitCharacters">Per-client set of trigger and commit characters</param>
    /// <returns>Possibly modified completion context</returns>
    /// <remarks>For example, if we invoke C# completion in Razor via @ character, we will not
    /// want C# to see @ as the trigger character and instead will transform completion context
    /// into "invoked" and "explicit" rather than "typing", without a trigger character</remarks>
    public static VSInternalCompletionContext? RewriteContext(
        VSInternalCompletionContext context,
        RazorLanguageKind languageKind,
        CompletionTriggerAndCommitCharacters triggerAndCommitCharacters)
    {
        Debug.Assert(languageKind != RazorLanguageKind.Razor,
            $"{nameof(RewriteContext)} should be called for delegated completion only");

        if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter
            || context.TriggerCharacter is not { } triggerCharacter)
        {
            // Non-triggered based completion, the existing context is valid.
            return context;
        }

        if (languageKind == RazorLanguageKind.CSharp
            && triggerAndCommitCharacters.IsCSharpTriggerCharacter(triggerCharacter))
        {
            // C# trigger character for C# content
            return context;
        }

        if (languageKind == RazorLanguageKind.Html)
        {
            // For HTML we don't want to delegate to HTML language server is completion is due to a trigger characters that is not
            // HTML trigger character. Doing so causes bad side effects in VSCode HTML client as we will end up with non-matching
            // completion entries
            return triggerAndCommitCharacters.IsHtmlTriggerCharacter(triggerCharacter) ? context : null;
        }

        // Trigger character not associated with the current language. Transform the context into an invoked context.
        var rewrittenContext = new VSInternalCompletionContext()
        {
            InvokeKind = context.InvokeKind,
            TriggerKind = CompletionTriggerKind.Invoked,
        };

        if (languageKind == RazorLanguageKind.CSharp
            && triggerAndCommitCharacters.IsTransitionCharacter(triggerCharacter))
        {
            // The C# language server will not return any completions for the '@' character unless we
            // send the completion request explicitly.
            rewrittenContext.InvokeKind = VSInternalCompletionInvokeKind.Explicit;
        }

        return rewrittenContext;
    }

    /// <summary>
    /// Modifies C# completion response to be usable by Razor.
    /// </summary>
    /// <returns>
    /// Possibly modified completion response.
    /// </returns>
    public static RazorVSInternalCompletionList RewriteCSharpResponse(
        RazorVSInternalCompletionList? delegatedResponse,
        int absoluteIndex,
        RazorCodeDocument codeDocument,
        Position projectedPosition,
        RazorCompletionOptions completionOptions)
    {
        if (delegatedResponse?.Items is null)
        {
            // If we don't get a response from the delegated server, we have to make sure to return an incomplete completion
            // list. When a user is typing quickly, the delegated request from the first keystroke will fail to synchronize,
            // so if we return a "complete" list then the query won't re-query us for completion once the typing stops/slows
            // so we'd only ever return Razor completion items.
            return new RazorVSInternalCompletionList() { IsIncomplete = true, Items = [] };
        }

        var rewrittenResponse = delegatedResponse;

        foreach (var rewriter in s_delegatedCSharpCompletionResponseRewriters)
        {
            rewrittenResponse = rewriter.Rewrite(
                rewrittenResponse,
                codeDocument,
                absoluteIndex,
                projectedPosition,
                completionOptions);
        }

        return rewrittenResponse;
    }

    public static RazorVSInternalCompletionList RewriteHtmlResponse(
        RazorVSInternalCompletionList? delegatedResponse,
        RazorCompletionOptions completionOptions)
    {
        if (delegatedResponse?.Items is null)
        {
            // If we don't get a response from the delegated server, we have to make sure to return an incomplete completion
            // list. When a user is typing quickly, the delegated request from the first keystroke will fail to synchronize,
            // so if we return a "complete" list then the query won't re-query us for completion once the typing stops/slows
            // so we'd only ever return Razor completion items.
            return new RazorVSInternalCompletionList() { IsIncomplete = true, Items = [] };
        }

        var rewrittenResponse = s_delegatedHtmlCompletionResponseRewriter.Rewrite(
            delegatedResponse,
            completionOptions);

        return rewrittenResponse;
    }

    /// <summary>
    /// Returns possibly update document position info and provisional edit (if any)
    /// </summary>
    /// <remarks>
    /// Provisional completion happens when typing something like @DateTime. in a document.
    /// In this case the '.' initially is parsed as belonging to HTML. However, we want to
    /// show C# member completion in this case, so we want to make a temporary change to the
    /// generated C# code so that '.' ends up in C#. This method will check for such case,
    /// and provisional completion case is detected, will update position language from HTML
    /// to C# and will return a temporary edit that should be made to the generated document
    /// in order to add the '.' to the generated C# contents.
    /// </remarks>
    public static bool TryGetProvisionalCompletionInfo(
        RazorCodeDocument codeDocument,
        VSInternalCompletionContext completionContext,
        DocumentPositionInfo originalPositionInfo,
        IDocumentMappingService documentMappingService,
        out CompletionPositionInfo result)
    {
        result = default;

        if (originalPositionInfo.LanguageKind != RazorLanguageKind.Html ||
            completionContext.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
            completionContext.TriggerCharacter != ".")
        {
            // Invalid provisional completion context
            return false;
        }

        if (originalPositionInfo.Position.Character == 0)
        {
            // We're at the start of line. Can't have provisional completions here.
            return false;
        }

        var previousCharacterPositionInfo = documentMappingService.GetPositionInfo(codeDocument, originalPositionInfo.HostDocumentIndex - 1);

        if (previousCharacterPositionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return false;
        }

        var previousPosition = previousCharacterPositionInfo.Position;

        // Edit the CSharp projected document to contain a '.'. This allows C# completion to provide valid
        // completion items for moments when a user has typed a '.' that's typically interpreted as Html.
        var addProvisionalDot = LspFactory.CreateTextEdit(previousPosition, ".");

        var provisionalPositionInfo = new DocumentPositionInfo(
            RazorLanguageKind.CSharp,
            LspFactory.CreatePosition(
                previousPosition.Line,
                previousPosition.Character + 1),
            previousCharacterPositionInfo.HostDocumentIndex + 1);

        result = new CompletionPositionInfo(addProvisionalDot, provisionalPositionInfo, ShouldIncludeDelegationSnippets: false);
        return true;
    }

    public static bool ShouldIncludeSnippets(RazorCodeDocument razorCodeDocument, int absoluteIndex)
    {
        var tree = razorCodeDocument.GetSyntaxTree();

        var token = tree.Root.FindToken(absoluteIndex, includeWhitespace: false);
        if (token.Kind == SyntaxKind.EndOfFile &&
            token.GetPreviousToken().Parent is { } parent &&
            parent.FirstAncestorOrSelf<RazorSyntaxNode>(RazorSyntaxFacts.IsAnyStartTag) is not null)
        {
            // If we're at the end of the file, we check if the previous token is part of a start tag, because the parser
            // treats whitespace at the end different. eg with "<$$[EOF]" or "<div $$", the EndOfFile won't be seen as being
            // in the tag, so without this special casing snippets would be shown.
            return false;
        }

        var node = token.Parent;
        var startOrEndTag = node?.FirstAncestorOrSelf<RazorSyntaxNode>(n => RazorSyntaxFacts.IsAnyStartTag(n) || RazorSyntaxFacts.IsAnyEndTag(n));

        if (startOrEndTag is null)
        {
            return token.Kind is not (SyntaxKind.OpenAngle or SyntaxKind.CloseAngle);
        }

        if (startOrEndTag.Span.Start == absoluteIndex)
        {
            // We're at the start of the tag, we should include snippets. This is the case for things like $$<div></div> or <div>$$</div>, since the
            // index is right associative to the token when using FindToken.
            return true;
        }

        return !startOrEndTag.Span.Contains(absoluteIndex);
    }

    public static object? GetOriginalCompletionItemData(
        VSInternalCompletionItem requestCompletionItem,
        VSInternalCompletionList containingCompletionList,
        object? originalCompletionListData)
    {
        var requestLabel = requestCompletionItem.Label;
        var requestKind = requestCompletionItem.Kind;
        var originalDelegatedCompletionItem = containingCompletionList.Items.FirstOrDefault(
            completionItem => string.Equals(requestLabel, completionItem.Label, StringComparison.Ordinal)
                && requestKind == completionItem.Kind);

        if (originalDelegatedCompletionItem is null)
        {
            return null;
        }

        object? originalData;

        // If the data was merged to combine resultId with original data, undo that merge and set the data back
        // to what it originally was for the delegated request
        if (CompletionListMerger.TrySplit(originalDelegatedCompletionItem.Data, out var splitData) && splitData.Length == 2)
        {
            originalData = splitData[1];
        }
        else
        {
            originalData = originalDelegatedCompletionItem.Data ?? originalCompletionListData;
        }

        return originalData;
    }

    public static async Task<VSInternalCompletionItem> FormatCSharpCompletionItemAsync(VSInternalCompletionItem resolvedCompletionItem, DocumentContext documentContext, RazorFormattingOptions options, IRazorFormattingService formattingService, CancellationToken cancellationToken)
    {
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var csharpSourceText = await documentContext.GetCSharpSourceTextAsync(cancellationToken).ConfigureAwait(false);

        if (resolvedCompletionItem.TextEdit is not null)
        {
            if (resolvedCompletionItem.TextEdit.Value.TryGetFirst(out var textEdit))
            {
                var textChange = csharpSourceText.GetTextChange(textEdit);
                var formattedTextChange = await formattingService.TryGetCSharpSnippetFormattingEditAsync(
                    documentContext,
                    [textChange],
                    options,
                    cancellationToken).ConfigureAwait(false);

                if (formattedTextChange is { } change)
                {
                    resolvedCompletionItem.TextEdit = sourceText.GetTextEdit(change);
                }
            }
            else
            {
                // TODO: Handle InsertReplaceEdit type
                // https://github.com/dotnet/razor/issues/8829
                Debug.Fail("Unsupported edit type.");
            }
        }

        if (resolvedCompletionItem.AdditionalTextEdits is not null)
        {
            var additionalChanges = resolvedCompletionItem.AdditionalTextEdits.SelectAsArray(csharpSourceText.GetTextChange);
            var formattedTextChange = await formattingService.TryGetCSharpSnippetFormattingEditAsync(
                documentContext,
                additionalChanges,
                options,
                cancellationToken).ConfigureAwait(false);

            resolvedCompletionItem.AdditionalTextEdits = formattedTextChange is { } change ? [sourceText.GetTextEdit(change)] : null;
        }

        return resolvedCompletionItem;
    }
}
