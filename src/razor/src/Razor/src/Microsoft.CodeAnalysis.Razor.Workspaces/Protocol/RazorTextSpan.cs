// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

/// <summary>
/// A representation of a Roslyn TextSpan that can be serialized with System.Text.Json. Also needs to match
/// https://github.com/dotnet/vscode-csharp/blob/main/src/razor/src/rpc/serverTextSpan.ts for VS Code.
/// </summary>
internal sealed record RazorTextSpan
{
    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }
}
