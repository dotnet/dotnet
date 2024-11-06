// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Completion;

/// <summary>
/// Completion-related information about a position.
/// </summary>
/// <param name="ProvisionalTextEdit">Text edit that should be applied to generated C# document prior to invoking completion</param>
/// <remarks>
/// Provisional completion happens when the user just type "." in something like @DateTime.
/// and the dot is initially in HTML rather than C#. Since we don't want HTML completions
/// in that case, we cheat and modify C# buffer immediately but temporarily, not waiting for
/// reparse/regen, before showing completion.
/// </remarks>
/// <param name="DocumentPositionInfo">Document position mapping data for language mappings</param>
/// <param name="ShouldIncludeDelegationSnippets">Indicates that snippets should be added to delegated completion list (currently for HTML only)</param>
internal record struct CompletionPositionInfo(
    [property:JsonPropertyName("provisionalTextEdit")] TextEdit? ProvisionalTextEdit,
    [property:JsonPropertyName("documentPositionInfo")] DocumentPositionInfo DocumentPositionInfo,
    [property:JsonPropertyName("shouldIncludeDelegationSnippets")] bool ShouldIncludeDelegationSnippets);
