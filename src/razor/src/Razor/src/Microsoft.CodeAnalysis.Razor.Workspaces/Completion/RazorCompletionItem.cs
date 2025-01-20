// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Tooltip;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal sealed class RazorCompletionItem
{
    public RazorCompletionItemKind Kind { get; }
    public string DisplayText { get; }
    public string InsertText { get; }

    /// <summary>
    /// A string that is used to alphabetically sort the completion item.
    /// </summary>
    public string SortText { get; }

    public object DescriptionInfo { get; }
    public ImmutableArray<RazorCommitCharacter> CommitCharacters { get; }
    public bool IsSnippet { get; }

    /// <summary>
    /// Creates a new Razor completion item
    /// </summary>
    /// <param name="kind">The type of completion item this is. Used for icons and resolving extra information like tooltip text.</param>
    /// <param name="displayText">The text to display in the completion list.</param>
    /// <param name="insertText">Content to insert when completion item is committed.</param>
    /// <param name="sortText">A string that is used to alphabetically sort the completion item. If omitted defaults to <paramref name="displayText"/>.</param>
    /// <param name="descriptionInfo">An object that provides description information for this completion item.</param>
    /// <param name="commitCharacters">Characters that can be used to commit the completion item.</param>
    /// <param name="isSnippet">Indicates whether the completion item's <see cref="InsertText"/> is an LSP snippet or not.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="displayText"/> or <paramref name="insertText"/> are <see langword="null"/>.</exception>
    private RazorCompletionItem(
        RazorCompletionItemKind kind,
        string displayText,
        string insertText,
        string? sortText,
        object descriptionInfo,
        ImmutableArray<RazorCommitCharacter> commitCharacters,
        bool isSnippet)
    {
        ArgHelper.ThrowIfNull(displayText);
        ArgHelper.ThrowIfNull(insertText);

        Kind = kind;
        DisplayText = displayText;
        InsertText = insertText;
        SortText = sortText ?? displayText;
        DescriptionInfo = descriptionInfo;
        CommitCharacters = commitCharacters.NullToEmpty();
        IsSnippet = isSnippet;
    }

    public static RazorCompletionItem CreateDirective(
        string displayText, string insertText, string? sortText,
        DirectiveCompletionDescription descriptionInfo,
        ImmutableArray<RazorCommitCharacter> commitCharacters, bool isSnippet)
        => new(RazorCompletionItemKind.Directive, displayText, insertText, sortText, descriptionInfo, commitCharacters, isSnippet);

    public static RazorCompletionItem CreateDirectiveAttribute(
        string displayText, string insertText,
        AggregateBoundAttributeDescription descriptionInfo,
        ImmutableArray<RazorCommitCharacter> commitCharacters)
        => new(RazorCompletionItemKind.DirectiveAttribute, displayText, insertText, sortText: null, descriptionInfo, commitCharacters, isSnippet: false);

    public static RazorCompletionItem CreateDirectiveAttributeParameter(
        string displayText, string insertText,
        AggregateBoundAttributeDescription descriptionInfo)
        => new(RazorCompletionItemKind.DirectiveAttributeParameter, displayText, insertText, sortText: null, descriptionInfo, commitCharacters: [], isSnippet: false);

    public static RazorCompletionItem CreateMarkupTransition(
        string displayText, string insertText,
        MarkupTransitionCompletionDescription descriptionInfo,
        ImmutableArray<RazorCommitCharacter> commitCharacters)
        => new(RazorCompletionItemKind.MarkupTransition, displayText, insertText, sortText: null, descriptionInfo, commitCharacters, isSnippet: false);

    public static RazorCompletionItem CreateTagHelperElement(
        string displayText, string insertText,
        AggregateBoundElementDescription descriptionInfo,
        ImmutableArray<RazorCommitCharacter> commitCharacters)
        => new(RazorCompletionItemKind.TagHelperElement, displayText, insertText, sortText: null, descriptionInfo, commitCharacters, isSnippet: false);

    public static RazorCompletionItem CreateTagHelperAttribute(
        string displayText, string insertText, string? sortText,
        AggregateBoundAttributeDescription descriptionInfo,
        ImmutableArray<RazorCommitCharacter> commitCharacters, bool isSnippet)
        => new(RazorCompletionItemKind.TagHelperAttribute, displayText, insertText, sortText, descriptionInfo, commitCharacters, isSnippet);
}
