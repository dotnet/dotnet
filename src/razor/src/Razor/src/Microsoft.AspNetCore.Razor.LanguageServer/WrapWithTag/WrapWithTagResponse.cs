// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

/// <summary>
/// Class representing the response of an WrapWithTag response.
/// </summary>
internal class WrapWithTagResponse
{
    /// <summary>
    /// Gets or sets the range of the wrapping tag.
    /// </summary>
    [JsonPropertyName("_vs_tagRange")]
    public LspRange? TagRange { get; set; }

    /// <summary>
    /// Gets or sets the text edits.
    /// </summary>
    [JsonPropertyName("_vs_textEdits")]
    public TextEdit[]? TextEdits { get; set; }
}
