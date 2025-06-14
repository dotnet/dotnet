// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Models;

internal sealed class ExtractToComponentCodeActionParams
{
    [JsonPropertyName("start")]
    public required int Start { get; set; }

    [JsonPropertyName("end")]
    public required int End { get; set; }

    [JsonPropertyName("namespace")]
    public string? Namespace { get; set; }
}
