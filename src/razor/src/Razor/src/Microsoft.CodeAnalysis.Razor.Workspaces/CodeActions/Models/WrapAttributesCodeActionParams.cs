// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Models;

internal sealed class WrapAttributesCodeActionParams
{
    [JsonPropertyName("indentSize")]
    public int IndentSize { get; init; }

    [JsonPropertyName("newLinePositions")]
    public required int[] NewLinePositions { get; init; }
}
