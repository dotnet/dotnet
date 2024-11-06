// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Models;

internal sealed class ExtractToCodeBehindCodeActionParams
{
    [JsonPropertyName("extractStart")]
    public int ExtractStart { get; set; }

    [JsonPropertyName("extractEnd")]
    public int ExtractEnd { get; set; }

    [JsonPropertyName("removeStart")]
    public int RemoveStart { get; set; }

    [JsonPropertyName("removeEnd")]
    public int RemoveEnd { get; set; }

    [JsonPropertyName("namespace")]
    public required string Namespace { get; set; }
}
