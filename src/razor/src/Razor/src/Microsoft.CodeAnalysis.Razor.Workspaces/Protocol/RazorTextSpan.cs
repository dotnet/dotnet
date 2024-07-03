// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
