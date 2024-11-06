// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion.Delegation;

using SyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

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
    private static readonly HtmlCommitCharacterResponseRewriter s_delegatedHtmlCompletionResponseRewriter = new HtmlCommitCharacterResponseRewriter();

    /// <summary>
    /// Modifies completion context if needed so that it's acceptable to the delegated language.
    /// </summary>
    /// <param name="context">Original completion context passed to the completion handler</param>
    /// <param name="languageKind">Language of the completion position</param>
    /// <returns>Possibly modified completion context</returns>
    /// <remarks>For example, if we invoke C# completion in Razor via @ character, we will not
    /// want C# to see @ as the trigger character and instead will transform completion context
    /// into "invoked" and "explicit" rather than "typing", without a trigger character</remarks>
    public static VSInternalCompletionContext RewriteContext(VSInternalCompletionContext context, RazorLanguageKind languageKind)
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
            && CompletionTriggerAndCommitCharacters.CSharpTriggerCharacters.Contains(triggerCharacter))
        {
            // C# trigger character for C# content
            return context;
        }

        if (languageKind == RazorLanguageKind.Html
            && CompletionTriggerAndCommitCharacters.HtmlTriggerCharacters.Contains(triggerCharacter))
        {
            // HTML trigger character for HTML content
            return context;
        }

        // Trigger character not associated with the current language. Transform the context into an invoked context.
        var rewrittenContext = new VSInternalCompletionContext()
        {
            InvokeKind = context.InvokeKind,
            TriggerKind = CompletionTriggerKind.Invoked,
        };

        if (languageKind == RazorLanguageKind.CSharp
            && CompletionTriggerAndCommitCharacters.RazorDelegationTriggerCharacters.Contains(triggerCharacter))
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
    public static async ValueTask<VSInternalCompletionList> RewriteCSharpResponseAsync(
        VSInternalCompletionList? delegatedResponse,
        int absoluteIndex,
        DocumentContext documentContext,
        Position projectedPosition,
        RazorCompletionOptions completionOptions,
        CancellationToken cancellationToken)
    {
        if (delegatedResponse?.Items is null)
        {
            // If we don't get a response from the delegated server, we have to make sure to return an incomplete completion
            // list. When a user is typing quickly, the delegated request from the first keystroke will fail to synchronize,
            // so if we return a "complete" list then the query won't re-query us for completion once the typing stops/slows
            // so we'd only ever return Razor completion items.
            return new VSInternalCompletionList() { IsIncomplete = true, Items = [] };
        }

        var rewrittenResponse = delegatedResponse;

        foreach (var rewriter in s_delegatedCSharpCompletionResponseRewriters)
        {
            rewrittenResponse = await rewriter.RewriteAsync(
                rewrittenResponse,
                absoluteIndex,
                documentContext,
                projectedPosition,
                completionOptions,
                cancellationToken).ConfigureAwait(false);
        }

        return rewrittenResponse;
    }

    public static VSInternalCompletionList RewriteHtmlResponse(
        VSInternalCompletionList? delegatedResponse,
        RazorCompletionOptions completionOptions)
    {
        if (delegatedResponse?.Items is null)
        {
            // If we don't get a response from the delegated server, we have to make sure to return an incomplete completion
            // list. When a user is typing quickly, the delegated request from the first keystroke will fail to synchronize,
            // so if we return a "complete" list then the query won't re-query us for completion once the typing stops/slows
            // so we'd only ever return Razor completion items.
            return new VSInternalCompletionList() { IsIncomplete = true, Items = [] };
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
    public static async Task<CompletionPositionInfo?> TryGetProvisionalCompletionInfoAsync(
        DocumentContext documentContext,
        VSInternalCompletionContext completionContext,
        DocumentPositionInfo originalPositionInfo,
        IDocumentMappingService documentMappingService,
        CancellationToken cancellationToken)
    {
        if (originalPositionInfo.LanguageKind != RazorLanguageKind.Html ||
            completionContext.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
            completionContext.TriggerCharacter != ".")
        {
            // Invalid provisional completion context
            return null;
        }

        if (originalPositionInfo.Position.Character == 0)
        {
            // We're at the start of line. Can't have provisional completions here.
            return null;
        }

        var previousCharacterPositionInfo = await documentMappingService
            .GetPositionInfoAsync(documentContext, originalPositionInfo.HostDocumentIndex - 1, cancellationToken)
            .ConfigureAwait(false);

        if (previousCharacterPositionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return null;
        }

        var previousPosition = previousCharacterPositionInfo.Position;

        // Edit the CSharp projected document to contain a '.'. This allows C# completion to provide valid
        // completion items for moments when a user has typed a '.' that's typically interpreted as Html.
        var addProvisionalDot = VsLspFactory.CreateTextEdit(previousPosition, ".");

        var provisionalPositionInfo = new DocumentPositionInfo(
            RazorLanguageKind.CSharp,
            VsLspFactory.CreatePosition(
                previousPosition.Line,
                previousPosition.Character + 1),
            previousCharacterPositionInfo.HostDocumentIndex + 1);

        return new CompletionPositionInfo(addProvisionalDot, provisionalPositionInfo, ShouldIncludeDelegationSnippets: false);
    }

    public static bool ShouldIncludeSnippets(RazorCodeDocument razorCodeDocument, int absoluteIndex)
    {
        var tree = razorCodeDocument.GetSyntaxTree();

        var token = tree.Root.FindToken(absoluteIndex, includeWhitespace: false);
        var node = token.Parent;
        var startOrEndTag = node?.FirstAncestorOrSelf<SyntaxNode>(n => RazorSyntaxFacts.IsAnyStartTag(n) || RazorSyntaxFacts.IsAnyEndTag(n));

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
}
