// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
