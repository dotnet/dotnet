// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Completion;

/// <summary>
/// Provides pre-filled sort text items to make setting <see cref="RazorCompletionItem.SortText"/> consistent.
/// </summary>
internal static class CompletionSortTextHelper
{
    /// <summary>
    /// The default sort priority. Typically this means an LSP client will fall-back to sorting the completion item
    /// based off of the displayed label in the completion list.
    /// </summary>
    public static string? DefaultSortPriority => null;

    /// <summary>
    /// A high sort priority. Displayed above <see cref="DefaultSortPriority"/> items.
    /// </summary>
    /// <remarks>
    /// Note how this property doesn't take into account the actual completion items content. Ultimately this property
    /// simply returns whitespace. The reason it returns whitespace is that whitespace is alphabetically ordered at the
    /// top of all other characters. Meaning, for a reasonable client to interpret this sort priority it'll sort by the
    /// whitespace sort text then will need to fallback to something else to handle collisions (items that have the same
    /// sort text). The only reasonable fallback will be the display text of a completion item; meaning, we'll have all
    /// of our "high priority" completion items appear above any other completion item because it'll first sort by whitespace
    /// and then by display text.
    /// </remarks>
    public static string HighSortPriority => " ";
}
