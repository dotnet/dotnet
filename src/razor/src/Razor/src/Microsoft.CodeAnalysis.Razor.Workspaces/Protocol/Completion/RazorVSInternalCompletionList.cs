// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Roslyn.LanguageServer.Protocol;

/// <summary>
/// A subclass of the LSP protocol <see cref="VSInternalCompletionList"/> that ensures correct serialization between LSP servers.
/// </summary>
/// <remarks>
/// This is the same as the LSP protocol <see cref="VSInternalCompletionList"/> except that it strongly types the <see cref="Items"/> property,
/// because our custom message target gets handled by a JsonRpc connection set up by the editor, that has no Roslyn converters.
/// </remarks>
internal sealed class RazorVSInternalCompletionList : VSInternalCompletionList
{
    public RazorVSInternalCompletionList()
    {
    }

    [SetsRequiredMembers]
    public RazorVSInternalCompletionList(VSInternalCompletionList completionList)
    {
        this.Data = completionList.Data;
        this.CommitCharacters = completionList.CommitCharacters;
        this.ContinueCharacters = completionList.ContinueCharacters;
        this.IsIncomplete = completionList.IsIncomplete;
        this.Items = JsonHelpers.ConvertAll<CompletionItem, VSInternalCompletionItem>(completionList.Items);
        this.ItemDefaults = completionList.ItemDefaults;
        this.SuggestionMode = completionList.SuggestionMode;
    }

    /// <summary>
    /// The completion items.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonRequired]
    public new required VSInternalCompletionItem[] Items
    {
        get => (VSInternalCompletionItem[])base.Items;
        set => base.Items = value;
    }
}
