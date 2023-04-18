// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

/// <summary>
/// Class representing the response of an WrapWithTag response.
/// </summary>
internal class WrapWithTagResponse
{
    /// <summary>
    /// Gets or sets the range of the wrapping tag.
    /// </summary>
    [JsonProperty("_vs_tagRange")]
    public Range? TagRange { get; set; }

    /// <summary>
    /// Gets or sets the text edits.
    /// </summary>
    [JsonProperty("_vs_textEdits")]
    public TextEdit[]? TextEdits { get; set; }
}
