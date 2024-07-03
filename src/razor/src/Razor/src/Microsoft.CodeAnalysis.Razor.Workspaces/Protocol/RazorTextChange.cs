// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

/// <summary>
/// A representation of a Roslyn TextChange that can be serialized with System.Text.Json. Also needs to match
/// https://github.com/dotnet/vscode-csharp/blob/main/src/razor/src/rpc/serverTextChange.ts for VS Code.
/// </summary>
internal sealed record RazorTextChange
{
    [JsonPropertyName("span")]
    public required RazorTextSpan Span { get; set; }

    [JsonPropertyName("newText")]
    public string? NewText { get; set; }
}
